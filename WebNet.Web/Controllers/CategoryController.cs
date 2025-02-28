using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet.DAL;

namespace WebNet.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly  DAL.Interface.ICategory dal ;
        public CategoryController(DAL.Interface.ICategory dal) {
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
            List<Model.Category> list = dal.GetList("*", order, ordertype, pagesize, pageindex, GetCond(key, start, end, cabh));
            return Json(list);
            /*ArrayList arr = new ArrayList();
            foreach (var item in list)
            {
                arr.Add(new
                { 
					Id = item.Id, 
CreateTime = item.CreateTime, 
Caname = item.Caname, 
Bh = item.Bh, 
Pbh = item.Pbh, 

                });
            }
            return Json(arr);*/
        }

        public ActionResult Add(int? id) {
            Model.Category n = new Model.Category();
            if (id != null)
            {
                n = dal.GetModel(id.Value);
                HttpContext.Session.SetString("HisCaname", n.Caname);
            }
            return View(n);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost] 
        public ActionResult Add(Model.Category m) {
try
            {
                CategoryDAL categoryDAL = new CategoryDAL();
              
            if (m.Id==0)
            {
                    if (categoryDAL.GetModelByCond("[Caname] like '"+ m.Caname+"'")==null)
                    {
                        dal.Add(m);
                        return Json(new { code = 0, msg = "新增成功！" });
                    }
                    else
                    {
                        return Json(new { code = 0, msg = "不能添加重复类目！" });
                    }
               
            }
            else
            {
                    DataTable NextCaname = categoryDAL.GetDistinctTable("[Caname]", "");
                    List<string> list = new List<string>();
                    for (int i = 0; i < NextCaname.Rows.Count;i++)
                    {
                            list.Add(NextCaname.Rows[i][0].ToString());
                    }
                    string his = HttpContext.Session.GetString("HisCaname");
                    list.Remove(HttpContext.Session.GetString("HisCaname"));
                    if (list.Contains(m.Caname))
                    {
                        return Json(new { code = 0, msg = "不能添加重复类目！" });
                    }
                    else
                    {
                        dal.Update(m);
                        return Json(new { code = 0, msg = "编辑成功！" });
                    }
 
               
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