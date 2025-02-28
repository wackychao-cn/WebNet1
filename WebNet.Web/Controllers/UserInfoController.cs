using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebNet.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserInfoController : Controller
    {
        private readonly  DAL.Interface.IUserInfo dal ;
 
        public UserInfoController(DAL.Interface.IUserInfo dal) {
            this.dal = dal;
        }
        public ActionResult Index()
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
                cond += $" and " + cabh + " like '%" + key + "%'";
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
            //    cabh = Tool.GetSafeSQL(cabh);
            //    cond += $" and cabh='{cabh}'";
            //}
            return cond;
        }

		 /// <summary>
        /// 取总记录数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTotalCount(string key, string start, string end, string cabh)
        {
            int totalcount = dal.CalcCount(GetCond(key,start,end,cabh));
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
            List<Model.UserInfo> list = dal.GetList("*", order, ordertype, pagesize, pageindex, GetCond(key, start, end, cabh));
            return Json(list);
        }

        public ActionResult Add(int? id) {
            Model.UserInfo n = new Model.UserInfo();
            if (id != null)
            {
                n = dal.GetModel(id.Value);
            }
            return View(n);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost] 
        public ActionResult Add(Model.UserInfo m) {
try
            {
            if (m.Id==0)
            {
                dal.Add(m);
                return Json(new { code=0,msg="新增成功！"});
            }
            else
            {
                dal.Update(m);
                return Json(new { code = 0, msg = "编辑成功！" });
            }
               }
            catch (Exception ex)
            {
                return Json(new { code = 1, msg = $"出错：{ex.Message}" });
            }
        }

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