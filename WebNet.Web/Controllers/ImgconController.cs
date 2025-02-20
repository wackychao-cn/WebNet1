using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebNet.Web.Controllers
{
    [Authorize]
    public class ImgconController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> FileSave()
        {
            try
            {
                //文件上传
                var date = Request;
                var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = Directory.GetCurrentDirectory();
            string contentRootPath = "\\wwwroot\\pic\\lunbo\\";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                        var filePath = webRootPath + contentRootPath+ formFile.FileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await formFile.CopyToAsync(stream);
                    }
                }
            }
         
                return Json(new { code = 0, msg = "新增成功！下次重启服务器生效" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }
    }
}
