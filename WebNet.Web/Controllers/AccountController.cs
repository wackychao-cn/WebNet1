using System;

using System.Collections.Generic;

using System.Linq;

using System.Security.Claims;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using WebNet.DAL;
using WebNet.DAL.Interface;
using WebNet.Model;



namespace Server.Controllers

{

    public class AccountController : Controller

    {

        /// <summary>

        /// 登录页面

        /// </summary>

        /// <returns></returns>

        public IActionResult Login()

        {

            return View();

        }



        public ActionResult Regist(string Name, string Identity, string Password, string Phone, string DepartMent)
        {

                return Json(new { code = 0, msg = "暂未开放" });

        }
        /// <summary>

        /// post 登录请求

        /// </summary>

        /// <returns></returns>

        [HttpPost]

        public async Task<IActionResult> Login(string userName, string password)

        {
            // 先登出当前用户
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (userName==null || password==null)
            {
                return Json(new { code = 0, msg = "用户名或密码不能为空" });
            }

            UserInfoDAL userInfoDAL = new UserInfoDAL();
            ///以下为验证内容
            ///
            if (userInfoDAL.GetModelByUsernameAndPassword(userName, password) != null)

            {
                // 根据用户权限设置角色
                string role = "guest";
                if (userName == "admin")
                {
                    role = "admin";
                }
                var claims = new List<Claim>(){

                new Claim(ClaimTypes.Name,userName),new Claim("password",password), new Claim(ClaimTypes.Role, role)

                };

                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties

                {

                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),

                    IsPersistent = false,

                    AllowRefresh = false

                });
                HttpContext.Session.SetString("Username", userName);
                return Json(new { code = 1, msg = $"{userName}，欢迎登录" });

            }
            return Json(new { code = 0, msg = "用户名或密码不能为空" });


        }



        /// <summary>

        /// 退出登录

        /// </summary>

        /// <returns></returns>

        public async Task<IActionResult> Logout()

        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login");

        }

    }

}
