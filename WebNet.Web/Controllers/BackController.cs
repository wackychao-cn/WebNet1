using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebNet.DAL;
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
        public IActionResult Preview(string title,string body)
        {
            BackModels backModels= new BackModels();
            backModels.title=title;
            backModels.body= body;
            return View(backModels);
        }
    }
}
