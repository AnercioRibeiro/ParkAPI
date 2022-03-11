using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using ParkyAPI.ParkyMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(ParkyMappings));
            //services.AddAutoMapper(typeof(TrailMappings));
            services.AddScoped<IUnitOfWork, UnitOfWork>();                            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkyOpenAPISpec", new OpenApiInfo 
                { 
                    Title = "Parky API", 
                    Version = "1",
                    Description = "Udemy Parky API National Park",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "anercio.ribeiro@outlook.com",
                        Name = "An�rcio Ribeiro",
                        Url = new Uri("https://www.bhrugen.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
                    }
                });
                //options.SwaggerDoc("ParkyOpenAPISpecTrails", new OpenApiInfo
                //{
                //    Title = "Parky Trails",
                //    Version = "1",
                //    Description = "Udemy Parky API Trails",
                //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                //    {
                //        Email = "anercio.ribeiro@outlook.com",
                //        Name = "An�rcio Ribeiro",
                //        Url = new Uri("https://www.bhrugen.com")
                //    },
                //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                //    {
                //        Name = "MIT License",
                //        Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
                //    }
                //});
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API NP");
                    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
                    //options.RoutePrefix = "";
                    foreach (var desc in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                        //options.RoutePrefix = "";
                    }

                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
