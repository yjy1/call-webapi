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
        //�ο����ϣ�
        //���ߣ�׷��ʱ����
        //���͵�ַ��https://www.cnblogs.com/Can-daydayup/p/12593599.html#_label2

        // ASP.NET Core MVC+Layuiʹ��EF Core����MySQLִ�м򵥵�CRUD����
        //˼ά����
        //ǰ��
        //ʾ��ʵ�ֹ���Ԥ��
        // ����ʵ��Դ�����ص�ַ
        //һ������ASP.NET Core WebӦ�ó���
        //�������EF Core NuGet��
        //����������Ӧ���ݿ���ʵ��ģ��
        //�ġ������ݿ������ַ�����ӵ� appsettings.json
        //�塢�������ݿ�������
        //��������������ӵ� Startup.cs �е�������ע��
        //�ߡ�����Layui��ʽ��js
        //�ˡ� ASP.NET Core MVC �� EF Coreʵ��MySQL  CRUD����:
        //ǰ�ԣ�

        //������Ҫͨ��һ��������ʾ������ASP.NET Core MVC+EF Core��MySQL���ݿ���м򵥵�CRUD������ϣ���ܹ�Ϊ������.NET Core��С������ṩһ�������Ĳο�ʵ��������ASP.NET Core MVC+EF����MsSQL Server������ο��ٷ��ĵ�(https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-mvc/?view=aspnetcore-3.1)��
        //����ʵ��Դ�����ص�ַ��
        //https://github.com/YSGStudyHards/ASP.NET-Core-MVC-Layui-EF-Core-CRUD_Sample

        //һ������ASP.NET Core WebӦ�ó���
        //ע�⣬���½���Ҫ��APS.NET Core 3.1�汾��Ϊ���͵���ʽʵ����

        //�������EF Core NuGet����
        //��Ҫ����Ŀ��ʹ��EF Core����MySQL���ݿ⣬��Ҫ��װ��Ӧ�����ݿ��������� ���½̳���Ҫʹ�� MySQL���ݿ⣬����������Ҫ��װ���������MySql.Data.EntityFrameworkCore��

        //��װ��ʽ��
        //�������=>NuGet��������=>���������������̨�����������
        //Install-Package MySql.Data.EntityFrameworkCore -Version 8.0.20
        //�������=>NuGet��������=>������������NuGet�������
        //������MySql.Data.EntityFrameworkCore �����װ��

        //����������Ӧ���ݿ���ʵ��ģ�ͣ�
        //ע���ƪ����ʹ�õ����ֶ�ģ�����ȵķ�ʽ�������ݿ���ֶ���ģ������ӳ�䣬��Ȼ�����Ҿ��������ӱȽ��鷳�Ļ��������������ϵ�ģ�����ȣ�ֱ�Ӵ���ģ����program.cs�����ô�����Ӧģ�͵����ݿ��߼����뼴�������ֶ��������ݿ⣬�ɲο������ĵ��̳̣�https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.1&tabs=visual-studio#create-the-database����

        //�����û�ģ�ͣ�UserInfo����
        //ע�⣺���Դ�Сд�����ݿ��еı��ֶα���һ�£�Id ���Գ�Ϊ�����Ӧ�����ݿ��������С� Ĭ������£�EF Core ����Ϊ Id �� xxxID ��������Ϊ������ �й���ϸ��Ϣ���������Կ-EF Core | Microsoft Docs��
         

        //�ġ������ݿ������ַ�����ӵ� appsettings.json��
        
        //�塢�������ݿ������ģ�
        //������
        // ���ݿ�����������Ϊ��������ģ��Э�� EF Core ���ܵ����ࡣ ������������ Microsoft.EntityFrameworkCore.DbContext�� ������ָ������ģ���а�����Щʵ�塣 �ڴ���Ŀ�н����ݿ�������������Ϊ SchoolUserInfoContext��

        //������

        //��������������ӵ� Startup.cs �е�������ע�룺
        public void ConfigureServices(IServiceCollection services)
        {
            //ע��EF Core���ݿ������ķ���
            services.AddDbContext<SchoolUserInfoContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));
            // ע��Swagger����
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ".Net Core�м��API�ĵ�", Version = "v1" });
                // Ϊ Swagger ����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //ע�� UserInfoService������
            services.AddScoped<IUserInfoService, UserInfoService>();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�����м����������Swagger
            app.UseSwagger();
            //�����м����������SwaggerUI��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ".Net Core�м��API�ĵ� V1");
                c.RoutePrefix = string.Empty;//���ø��ڵ����
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

            //ʹ��swagger�м��,���ṩUI����
            //app.UseSwagger();
            //app.UseSwaggerUI((s) =>
            //{
            //    //ע�⣺/swagger/Ψһ��ʶ�ĵ���URI�Ѻ�����/swagger.josn   
            //    s.SwaggerEndpoint("/swagger/swaggerName/swagger.json", "��Ŀ����");
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
