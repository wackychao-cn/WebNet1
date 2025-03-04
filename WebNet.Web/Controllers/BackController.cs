using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using WebNet.DAL;
using WebNet.Model;
using WebNet.Web.Models;

namespace WebNet.Web.Controllers
{
    [Authorize]
    public class BackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Left()
        {
            return View();
        }
        public IActionResult Middle()
        {
            return View();
        }
        public IActionResult Top()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Preview(string title, string body)
        {
            if (body != null)
            {
                // 使用正则表达式提取 href 属性的值
                var match = Regex.Match(body, @"href=\""([^\""]+)\""");
                if (match.Success)
                {
                    string relativeUrl = match.Groups[1].Value;

                    // 自动获取当前请求的根路径
                    string rootUrl = $"{Request.Scheme}://{Request.Host}";

                    // 拼接根路径和相对路径
                    string absoluteUrl = new Uri(new Uri(rootUrl), relativeUrl).AbsoluteUri;

                    // 将绝对URL传递给视图
                    ViewBag.AbsoluteUrl = absoluteUrl;
                }
            }
            ViewBag.Title = title;
            ViewBag.Body = body;
            return View();
        }
    }
}
