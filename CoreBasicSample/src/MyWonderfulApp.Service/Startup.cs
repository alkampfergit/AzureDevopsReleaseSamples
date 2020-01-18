using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using Serilog;

namespace MyWonderfulApp.Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(config =>
                {
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                // .AddMvc(); // Swagger needs full MVC to render the UI
                .AddMvcCore()
                .AddJsonFormatters()
                .AddApiExplorer();

            services
                .AddApiVersioning(o =>
                {
                    o.DefaultApiVersion = new ApiVersion(1, 0); // specify the default api version
                    o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don'
                    o.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvcCore();

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services
               .AddSwaggerDocument(document =>
               {
                   document.DocumentName = "v1";
                   document.ApiGroupNames = new[] { "1" };
               })
               .AddSwaggerDocument(document =>
               {
                   document.DocumentName = "v2";
                   document.ApiGroupNames = new[] { "2" };
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider,
            IApplicationLifetime _
            )
        {
            loggerFactory.AddSerilog();

            // see: https://docs.microsoft.com/it-it/aspnet/core/fundamentals/environments?view=aspnetcore-2.2
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseMvcWithDefaultRoute();
        }
    }
}
