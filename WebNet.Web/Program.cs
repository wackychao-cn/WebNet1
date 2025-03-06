using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebNet.Web.Models;

namespace WebNet.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //启用生产环境
        builder.Environment.IsProduction();
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                //返回的JSON里的首字母不要变成小写，与模型里的一样 nuget:Microsoft.AspNetCore.Mvc.NewtonsoftJson
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //返回的JSON里的日期格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });


        string db = builder.Configuration.GetSection("Db").Value;

 builder.Services.AddSingleton(DAL.DataAccess.CreateUserInfoDAL(db)); 
builder.Services.AddSingleton(DAL.DataAccess.CreateCategoryDAL(db)); 
builder.Services.AddSingleton(DAL.DataAccess.CreateNewsDAL(db));
        builder.Services.AddSingleton(DAL.DataAccess.CreateTravelDAL(db));
        builder.Services.AddSingleton(DAL.DataAccess.CreateDutyDAL(db));

        //验证登录
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>

{

    //登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径

    o.LoginPath = new PathString("/Account/Login");

    //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径。

    o.AccessDeniedPath = new PathString("/Home/Index");

});
      
        builder.Services.Configure<CookiePolicyOptions>(options =>
              {
                  // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                  options.CheckConsentNeeded = context => false;       //改为false或者直接注释掉，上面的Session才能正常使用
                  options.MinimumSameSitePolicy = SameSiteMode.None;
             });
        //session
        //注册Session服务

     
        //基于内存的Session
        builder.Services.AddDistributedMemoryCache();
        //默认过期时间为20分钟，每次访问都会重置
        builder.Services.AddSession();

        builder.Services.AddMvc();
        //添加启用Gzip压缩
        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        //启用静态文件缓存
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers["Cache-Control"] = "no-store, no-cache";
                ctx.Context.Response.Headers["Pragma"] = "no-cache"; // 兼容旧版 HTTP 协议
            }
        });
        //启用响应压缩
        app.UseResponseCompression();
        app.UseCookiePolicy();
        //启用Session管道
        app.UseSession();
        app.UseRouting();
        app.UseAuthorization();// 授权

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

