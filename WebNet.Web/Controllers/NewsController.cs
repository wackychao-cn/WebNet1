using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet.DAL;

namespace WebNet.Web.Controllers
{

    public class NewsController : Controller
    {
        private readonly  DAL.Interface.INews dal ;
        private static int _uploadedFiles = 0; // 已上传文件数
        public NewsController(DAL.Interface.INews dal) {
            this.dal = dal;
        }
        [Authorize]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Front()
        {

            return View();
        }

        /// <summary>
        /// 拼接条件
        /// </summary>
        /// <returns></returns>
        public string GetCond(string key, string start, string end, string cabh) {

            string cond = "1=1";
          
       

            if (!string.IsNullOrEmpty(key))
            {
                //key = Tool.GetSafeSQL(key);
                cond += $" and "+ cabh + " like '"+ key +"'";
            }
            if (!string.IsNullOrEmpty(start))
            {
                DateTime d;
                if (DateTime.TryParse(start, out d))
                {
                    cond += $" and CreateTime>='{d.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
            }
            if (!string.IsNullOrEmpty(end))
            {
                DateTime d;
                if (DateTime.TryParse(end, out d))
                {
                    cond += $" and CreateTime<='{d.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
            }
            //if (!string.IsNullOrEmpty(cabh))
            //{
            //    //cabh = Tool.GetSafeSQL(cabh);
            //    cond += $" and cabh='{cabh}'";
            //}
            return cond;
        }
        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FileSave()
        {
            try
            {
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);

                // 获取 search_key 的值
                var searchKey = Request.Form["search_key"].ToString();
                if (string.IsNullOrEmpty(searchKey))
                {
                    return Json(new { code = 1, msg = "搜索关键字不能为空" });
                }

                // 验证文件大小和类型
                foreach (var formFile in files)
                {
                    if (formFile.Length > 20 * 1024 * 1024) // 20MB
                    {
                        return Json(new { code = 1, msg = "文件大小不能超过20MB" });
                    }

                    if (Path.GetExtension(formFile.FileName).ToLower() != ".pdf")
                    {
                        return Json(new { code = 1, msg = "只能上传PDF文件" });
                    }
                }

                string webRootPath = Directory.GetCurrentDirectory();
                string uploadFolder = Path.Combine(webRootPath, "wwwroot", "upload", searchKey, DateTime.Now.ToString("yyyyMMdd"));

                // 如果文件夹不存在，则创建
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // 单线程处理每个文件
                _uploadedFiles = 0; // 重置已上传文件数
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(formFile.FileName);
                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        // 将文件路径转换为 href 模板形式
                        string hrefPath = $"/upload/{searchKey}/{DateTime.Now.ToString("yyyyMMdd")}/{fileName}";
                        // 1. 获取当前登录用户
                        var currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
                        if (string.IsNullOrEmpty(currentUser))
                        {
                            return Json(new { code = 1, msg = "用户未登录！" });
                        }
                        // 创建 News 对象
                        var news = new Model.News
                        {
                            Caname = searchKey,
                            Creater = currentUser,
                            Title = fileName,
                            Body = $"href=\"{hrefPath}\""
                        };

                        // 获取 Bh 值
                        var categoryDAL = new CategoryDAL();
                        var category = categoryDAL.GetModelByCond($"[Caname] like '{searchKey}'");
                        if (category != null)
                        {
                            news.Bh = category.Pbh;
                        }

                        // 调用 Add 函数插入数据库
                        dal.Add(news);

                        _uploadedFiles++; // 更新已上传文件数
                    }
                }

                return Json(new { code = 0, msg = "新增成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetUploadProgress()
        {
            return Json(new { code = 0, uploadedFiles = _uploadedFiles });
        }


        /// <summary>
        /// 取总记录数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTotalCount(string key,string start, string end, string cabh)
        {
            int totalcount = dal.CalcCount(GetCond(key,start,end,cabh));
            //如果是找父类集合
            CategoryDAL categoryDAL = new CategoryDAL();
            if (totalcount==0 && key!="null")
            {
                string pbh = categoryDAL.GetModelByCond("[Caname] like '" + key + "'").Bh;
                totalcount = dal.CalcCount(GetCond(pbh, start, end, "Bh"));

            }
            return Content(totalcount.ToString());
        }

		 /// <summary>
        /// 取分页数据，返回 JSON
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult List(int pageindex, int pagesize, string key, string order, string ordertype, string start, string end, string cabh)
        {
            List<Model.News> list = dal.GetList("*", order, ordertype, pagesize, pageindex, GetCond(key, start, end, cabh));
            //如果是找父类集合
            CategoryDAL categoryDAL = new CategoryDAL();
            if (list.Count==0 && key!="null")
            {
                string pbh = categoryDAL.GetModelByCond("[Caname] like '" + key + "'").Bh;
                list=dal.GetList("*", order, ordertype, pagesize, pageindex, GetCond(pbh, start, end, "Bh"));
            }
            return Json(list);
           
        }
        [Authorize]
        public ActionResult Add(int? id) {
            Model.News n = new Model.News();
            if (id != null)
            {
                n = dal.GetModel(id.Value);
            }
            return View(n);
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public ActionResult Add(Model.News n)
        {
            var currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(currentUser))
            {
                return Json(new { code = 1, msg = "用户未登录！" });
            }

            try
            {
                Model.News m = n;
                CategoryDAL categoryDAL = new CategoryDAL();
                m.Bh = categoryDAL.GetModelByCond("[Caname] like '" + m.Caname + "'").Pbh;

                if (m.Id == 0)
                {
                    m.Creater = currentUser;
                    dal.Add(m);
                    return Json(new { code = 0, msg = "新增成功！" });
                }
                else
                {
                    var originalNews = dal.GetModel(m.Id);
                    if (originalNews == null)
                    {
                        return Json(new { code = 1, msg = "记录不存在！" });
                    }

                    // 新增角色校验逻辑
                    bool isAdmin = User.IsInRole("admin"); // 关键角色判断[2,6](@ref)
                    if (isAdmin || originalNews.Creater == currentUser)
                    {
                        // 管理员编辑时保留原始创建者信息
                        m.Creater = isAdmin ? originalNews.Creater : currentUser;
                        dal.Update(m);
                        return Json(new { code = 0, msg = "编辑成功！" });
                    }
                    else
                    {
                        return Json(new { code = 1, msg = "无权限编辑！" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }
        [Authorize]
        public ActionResult Delete(string ids)
        {
            try
            {
                // 1. 获取当前登录用户
                var currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(currentUser))
                {
                    return Json(new { code = 1, msg = "用户未登录！" });
                }

                // 2. 管理员角色校验（需确保角色名大小写一致）
                bool isAdmin = User.IsInRole("admin");

                int success = 0;
                string[] ss = ids.Split(',');
                foreach (var item in ss)
                {
                    // 3. 验证ID有效性
                    if (!int.TryParse(item, out int x)) continue;

                    // 4. 获取原始记录
                    var originalNews = dal.GetModel(x);
                    if (originalNews == null)
                    {
                        return Json(new { code = 1, msg = $"ID {x} 记录不存在！" });
                    }

                    // 5. 权限校验逻辑
                    if (!isAdmin && originalNews.Creater != currentUser)
                    {
                        return Json(new { code = 1, msg = $"无权限删除 ID {x} 的条目！" });
                    }

                    // 6. 执行删除
                    dal.Delete(x);
                    success++;
                }
                return Json(new { code = 0, msg = "成功删除" + success + "条记录！" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }
    }
}