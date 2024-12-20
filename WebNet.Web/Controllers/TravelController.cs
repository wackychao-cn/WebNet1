using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebNet.Web.Controllers
{
    public class TravelController : Controller
    {
        private readonly  DAL.Interface.ITravel dal ;
 
        public TravelController(DAL.Interface.ITravel dal) {
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
          /* 
Expression<Func<Model.Admin, bool>> cond = a => true;
            if (!string.IsNullOrEmpty(key))
            {
                cond = cond.And(a => a.username.Contains(key));
            }
            if (!string.IsNullOrEmpty(start))
            {
                DateTime d;
                if (DateTime.TryParse(start,out d))
                {
                    cond = cond.And(a => a.createtime > d);
                }
            }
            if (!string.IsNullOrEmpty(end))
            {
                DateTime d;
                if (DateTime.TryParse(end,out d))
                {
                    cond = cond.And(a => a.createtime <= d);
                }
            }*/

 if (!string.IsNullOrEmpty(key))
            {
                //key = Tool.GetSafeSQL(key);
                cond += $" and "+ cabh + " like '%"+ key +"%'";
            }
            if (!string.IsNullOrEmpty(start))
            {
                DateTime d;
                if (DateTime.TryParse(start, out d))
                {
                    cond += $" and createdate>='{d.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
            }
            if (!string.IsNullOrEmpty(end))
            {
                DateTime d;
                if (DateTime.TryParse(end, out d))
                {
                    cond += $" and createdate<='{d.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
            }
            //if (!string.IsNullOrEmpty(cabh))
            //{
            //    cabh = Tool.GetSafeSQL(cabh);
            //    cond += $" and cabh='{cabh}'";
            //}
            return cond;
        }
        /// <summary>
        /// 根据出差日期拼接条件，展示到前台
        /// </summary>
        /// <returns></returns>
        public string GetCondbyDate(string key, string start, string end, string cabh)
        {

            string cond = "1=1";

            cond += " and EndTime >= '" + DateTime.Today+"'";
         
            return cond;
        }
        /// <summary>
        /// 取总记录数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTotalCountbyDate(string key, string start, string end, string cabh)
        {
            int totalcount = dal.CalcCount(GetCondbyDate(key,start,end,cabh));
            return Content(totalcount.ToString());
        }
        /// <summary>
        /// 取总记录数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTotalCount(string key, string start, string end, string cabh)
        {
            int totalcount = dal.CalcCount(GetCond(key, start, end, cabh));
            return Content(totalcount.ToString());
        }
        /// <summary>
        /// 取分页数据，返回 JSON
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult List(int pageindex, int pagesize, string key, string start, string end, string cabh)
        {
            List<Model.Travel> list = dal.GetList("*","id","desc", pagesize, pageindex, GetCond(key, start, end, cabh));
            return Json(list);
            /*ArrayList arr = new ArrayList();
            foreach (var item in list)
            {
                arr.Add(new
                { 
					Id = item.Id, 
CreateTime = item.CreateTime, 
Name = item.Name, 
StartTime = item.StartTime, 
EndTime = item.EndTime, 

                });
            }
            return Json(arr);*/
        }
        /// <summary>
        /// 取分页数据，返回 JSON
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult ListbyDate(int pageindex, int pagesize, string key, string start, string end, string cabh)
        {
            List<Model.Travel> list = dal.GetList("*", "id", "desc", pagesize, pageindex, GetCondbyDate(key, start, end, cabh));
            return Json(list);
            /*ArrayList arr = new ArrayList();
            foreach (var item in list)
            {
                arr.Add(new
                { 
					Id = item.Id, 
CreateTime = item.CreateTime, 
Name = item.Name, 
StartTime = item.StartTime, 
EndTime = item.EndTime, 

                });
            }
            return Json(arr);*/
        }
        [Authorize]
        public ActionResult Add(int? id) {
            Model.Travel n = new Model.Travel();
            if (id != null)
            {
                n = dal.GetModel(id.Value);
            }
            return View(n);
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost] 
        public ActionResult Add(Model.Travel m) {
try
            {
            if (m.Id==0)
            {
                    if (m.EndTime>=m.StartTime)
                    {
                        dal.Add(m);
                        return Json(new { code = 0, msg = "新增成功！" });
                    }
                    else
                    {
                        return Json(new { code = 1, msg = "结束日期不能小于开始日期！" });
                    }
             
            }
            else
            {
                    if (m.EndTime >= m.StartTime)
                    {
                        dal.Update(m);
                        return Json(new { code = 0, msg = "编辑成功！" });
                    }
                    else
                    {
                        return Json(new { code = 1, msg = "结束日期不能小于开始日期！" });
                    }
            }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }
        [Authorize]
        public ActionResult Delete(string ids) {
 try
            {
            int success = 0;
            string[] ss = ids.Split(','); 
            foreach (var item in ss)
            {
                int x;
                if (int.TryParse(item, out x))
                {
                    dal.Delete(x);
                    success++;
                }
            }
            return Json(new { code = 0, msg = "成功删除" + success + "条记录！" }) ;
 }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }
    }
}