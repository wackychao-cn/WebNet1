using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Collections;
using System.Diagnostics;
using WebNet.DAL;
using WebNet.Web.Models;

namespace WebNet.Web.Controllers
{
    public class HomeController : Controller
    {
        HomeModel model;
        //用于读取网站静态文件目录
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }
        public ActionResult Index()
        {
            //获取登录账号名
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {

                ViewBag.name= HttpContext.Session.GetString("Username");
            }
            else
            {
                ViewBag.name = null;
            }
            //获取值班领导名
            DutyDAL dutyDAL = new DutyDAL();
            if (dutyDAL.GetModelByCond("[DutyTime] = '" + DateTime.Now.ToShortDateString() + "'") != null)
            {
                ViewBag.dutyname = dutyDAL.GetModelByCond("[DutyTime] = '" + DateTime.Now.ToShortDateString() + "'").Name;
                ViewBag.dutyname1 = dutyDAL.GetModelByCond("[DutyTime] = '" + DateTime.Now.ToShortDateString() + "'").Name1;
            }
            else
            {
                ViewBag.dutyname= null;
                ViewBag.dutyname1 = null;
            }
            return View();
        }
        public ActionResult Login( )
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // 获取当前请求的错误信息
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            // 返回错误视图，并传递错误信息
            return View(errorViewModel);
        }
        /// <summary>
        /// layui编辑器里的上传图片功能 
        /// {
        ///   "code": 0 //0表示成功，其它失败
        ///   ,"msg": "" //提示信息 //一般上传失败后返回
        ///   ,"data": {
        ///     "src": "图片路径"
        ///     ,"title": "图片名称" //可选
        ///   }
        /// }
        /// </summary>
        /// <returns></returns>
        public IActionResult ImgUpload()
        {
            var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                long size = 0;
                string tempname = "";
                var filename = System.Net.Http.Headers.ContentDispositionHeaderValue
                                .Parse(imgFile.ContentDisposition)
                                .FileName
                                .Trim();
                var extname = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.')); //扩展名，如.jpg

                extname = extname.Replace("\"", "");

                #region 判断后缀
                if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
                {
                    return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片." });
                }
                #endregion

                #region 判断大小
                long mb = imgFile.Length / 1024 / 1024; // MB
                if (mb > 5)
                {
                    return Json(new { code = 1, msg = "只允许上传小于 5MB 的图片." });
                }
                #endregion

                var filename1 = System.Guid.NewGuid().ToString().Substring(0, 6) + extname;
                tempname = filename1;
                var path = hostingEnv.WebRootPath; //网站静态文件目录  wwwroot
                string dir = DateTime.Now.ToString("yyyyMMdd");
                //完整物理路径
                string wuli_path = path + $"{Path.DirectorySeparatorChar}upload{Path.DirectorySeparatorChar}{dir}{Path.DirectorySeparatorChar}";
                if (!System.IO.Directory.Exists(wuli_path))
                {
                    System.IO.Directory.CreateDirectory(wuli_path);
                }
                filename = wuli_path + filename1;
                size += imgFile.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    imgFile.CopyTo(fs);
                    fs.Flush();
                }
                // 返回符合 TinyMCE 期望的 JSON 格式
                return Json(new { location = $"/upload/{dir}/{filename1}" });
            }
            return Json(new { code = 1, msg = "上传失败" });
        }


        /// <summary>
        /// kindeditor在线编辑器的上传
        /// </summary>
        /// <returns></returns>
        public IActionResult KE_Upload()
        {


            var imgFile = Request.Form.Files[0];
            if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
            {
                long size = 0;
                string tempname = "";
                var filename = System.Net.Http.Headers.ContentDispositionHeaderValue
                                .Parse(imgFile.ContentDisposition)
                                .FileName
                                .Trim();
                var extname = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.')); //扩展名，如.jpg

                extname = extname.Replace("\"", "");

                #region 判断后缀
                var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".pdf", ".docx" };
                var videoExtensions = new[] { ".mp4", ".avi", ".mov" }; // 单独定义视频类型
                if (!allowedExtensions.Contains(extname.ToLower()))
                {
                    return Json(new { code = 1, msg = "不支持的文件类型" });
                }
                #endregion

                #region 动态判断大小
                long maxAllowedMB = videoExtensions.Contains(extname.ToLower()) ? 300 : 20; // 视频类型 300MB，其他 20MB
                long fileSizeMB = imgFile.Length / 1024 / 1024;
                if (fileSizeMB > maxAllowedMB)
                {
                    return Json(new { code = 1, msg = $"只允许上传小于 {maxAllowedMB}MB 的文件" });
                }
                #endregion

                var filename1 = System.Guid.NewGuid().ToString().Substring(0, 6) + extname;
                tempname = filename1;
                var path = hostingEnv.WebRootPath; //网站静态文件目录  wwwroot
                string dir = DateTime.Now.ToString("yyyyMMdd");
                //完整物理路径
                string wuli_path = path + $"{Path.DirectorySeparatorChar}upload{Path.DirectorySeparatorChar}{dir}{Path.DirectorySeparatorChar}";
                if (!System.IO.Directory.Exists(wuli_path))
                {
                    System.IO.Directory.CreateDirectory(wuli_path);
                }
                filename = wuli_path + filename1;
                size += imgFile.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    imgFile.CopyTo(fs);
                    fs.Flush();
                }
                // 返回符合 TinyMCE 期望的 JSON 格式
                return Json(new { location = $"/upload/{dir}/{filename1}" });
            }
            return Json(new { code = 1, msg = "上传失败" });

        }

        private string showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(hash);
            return str;
        }
    }
}