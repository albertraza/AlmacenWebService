using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlmacenWebService.Entities;
using AlmacenWebService.Entities.Abstactions;
using AlmacenWebService.Models;
using AlmacenWebService.Services;
using AlmacenWebService.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlmacenWebService
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
           
            services.AddScoped<ICrud<Product>, ProductDbHandler>();
            services.AddScoped<ICrud<Category>, CategoryDbHandler>();
            services.AddControllers();
            services.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<Product, ProductDTO>().ReverseMap();
                configuration.CreateMap<Product, ProductCreateDTO>().ReverseMap();
            }, typeof(Startup));

            //var mapperConfig = new MapperConfiguration(configuration =>
            //{
            //    configuration.AddProfile(new MappingProfiles());
            //});
            //IMapper mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
