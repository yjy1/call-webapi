using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApplication1.Controllers;
using static System.Net.WebRequestMethods;

namespace WebApplication1
{
    public class Startup
    {
        //参考资料：
        //作者：追逐时光者
        //博客地址：https://www.cnblogs.com/Can-daydayup/p/12593599.html#_label2

        // ASP.NET Core MVC+Layui使用EF Core连接MySQL执行简单的CRUD操作
        //思维导航
        //前言
        //示例实现功能预览
        // 博客实例源码下载地址
        //一、创建ASP.NET Core Web应用程序
        //二、添加EF Core NuGet包
        //三、创建对应数据库表的实体模型
        //四、将数据库连接字符串添加到 appsettings.json
        //五、创建数据库上下文
        //六、将上下文添加到 Startup.cs 中的依赖项注入
        //七、引入Layui样式和js
        //八、 ASP.NET Core MVC 和 EF Core实现MySQL  CRUD功能:
        //前言：

        //本章主要通过一个完整的示例讲解ASP.NET Core MVC+EF Core对MySQL数据库进行简单的CRUD操作，希望能够为刚入门.NET Core的小伙伴们提供一个完整的参考实例。关于ASP.NET Core MVC+EF操作MsSQL Server详情请参考官方文档(https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-mvc/?view=aspnetcore-3.1)。
        //博客实例源码下载地址：
        //https://github.com/YSGStudyHards/ASP.NET-Core-MVC-Layui-EF-Core-CRUD_Sample

        //一、创建ASP.NET Core Web应用程序：
        //注意，本章节主要以APS.NET Core 3.1版本作为博客的样式实例！

        //二、添加EF Core NuGet包：
        //若要在项目中使用EF Core操作MySQL数据库，需要安装相应的数据库驱动包。 本章教程主要使用 MySQL数据库，所以我们需要安装相关驱动包MySql.Data.EntityFrameworkCore。

        //安装方式：
        //点击工具=>NuGet包管理器=>程序包管理器控制台输入以下命令：
        //Install-Package MySql.Data.EntityFrameworkCore -Version 8.0.20
        //点击工具=>NuGet包管理器=>管理解决方案的NuGet程序包：
        //搜索：MySql.Data.EntityFrameworkCore 点击安装。

        //三、创建对应数据库表的实体模型：
        //注意该篇博客使用的是手动模型优先的方式进行数据库表字段与模型属性映射，当然如果大家觉得这样子比较麻烦的话可以真正意义上的模型优先，直接创建模型在program.cs中配置创建对应模型的数据库逻辑代码即可无需手动创建数据库，可参考官网文档教程（https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.1&tabs=visual-studio#create-the-database）。

        //创建用户模型（UserInfo）：
        //注意：属性大小写和数据库中的表字段保持一致，Id 属性成为此类对应的数据库表的主键列。 默认情况下，EF Core 将名为 Id 或 xxxID 的属性视为主键。 有关详细信息，请参阅密钥-EF Core | Microsoft Docs。
         

        //四、将数据库连接字符串添加到 appsettings.json：
        
        //五、创建数据库上下文：
        //概述：
        // 数据库上下文类是为给定数据模型协调 EF Core 功能的主类。 上下文派生自 Microsoft.EntityFrameworkCore.DbContext。 上下文指定数据模型中包含哪些实体。 在此项目中将数据库上下文类命名为 SchoolUserInfoContext。

        //创建：

        //六、将上下文添加到 Startup.cs 中的依赖项注入：
        public void ConfigureServices(IServiceCollection services)
        {
            //注入EF Core数据库上下文服务
            services.AddDbContext<SchoolUserInfoContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));
            // 注册Swagger服务
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ".Net Core中间件API文档", Version = "v1" });
                // 为 Swagger 设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //注册 UserInfoService服务类
            services.AddScoped<IUserInfoService, UserInfoService>();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //启用中间件服务生成Swagger
            app.UseSwagger();
            //启用中间件服务生成SwaggerUI，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ".Net Core中间件API文档 V1");
                c.RoutePrefix = string.Empty;//设置根节点访问
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //使用swagger中间件,并提供UI界面
            //app.UseSwagger();
            //app.UseSwaggerUI((s) =>
            //{
            //    //注意：/swagger/唯一标识文档的URI友好名称/swagger.josn   
            //    s.SwaggerEndpoint("/swagger/swaggerName/swagger.json", "项目名称");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
