using Azure;
using Microsoft.AspNetCore.Mvc;
using WebNet.DAL;
using WebNet.Model;
using WebNet.Web.Models;


namespace WebNet.Web.Controllers
{
    public class NewsPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {

            //如果没有传值 默认返回最新新闻
            NewsPageModel np= new NewsPageModel();
            NewsDAL ndal=new NewsDAL();
            List<News> list = ndal.GetList("");
            string con = "Id like ";
            if (id > 0)
            {
                con += id.ToString();
            }
            else
            { 
               
                con = "Id like "+list.Last().Id.ToString();
            }
            WebNet.Model.News model = new NewsDAL().GetModelByCond(con);
            if (model != null)
            {
                np.Title = model.Title;
                np.Body = model.Body;
                np.CreateTime = model.CreateTime;
                np.Caname = model.Caname;
            }
            else
            {
                np.Title = list.Last().Title;
                np.Body = list.Last().Body;
                np.CreateTime = list.Last().CreateTime;
                np.Caname = list.Last().Caname;
            }
            return View(np);
        }
        public IActionResult Left()
        {
            return View();
        }
    }
}
