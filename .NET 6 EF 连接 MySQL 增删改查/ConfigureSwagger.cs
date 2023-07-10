using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
namespace WebApplication1
{
    public static class ConfigureSwagger
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "APIs" });

                // 在接口文档上显示 XML 注释
                var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
                options.IncludeXmlComments(filePath, true);
            });
        }

        public static void UseSwaggerPkg(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.RoutePrefix = "api-docs/swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs");
            });
            //app.UseReDoc(options => {
            //    options.RoutePrefix = "api-docs/redoc";
            //    options.SpecUrl = "/swagger/v1/swagger.json";
            //});
        }
    }
}
