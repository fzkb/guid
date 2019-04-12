﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GUIDCRUD.Models;
using GUIDCRUD.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GUIDCRUD
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add configuration for DbContext
            // Use connection string from appsettings.json file
            services.AddDbContext<GUIDEntityDbContext>(options =>
            {
                options.UseSqlServer(Configuration["AppSettings:ConnectionStrings:SQLConnection"]);
            });

            // Set up dependency injection for controller's logger
            services.AddScoped<ILogger, Logger<GUIDEntitiesController>>();

            ////Setup Redis Cache
            //services.AddDistributedRedisCache(option =>
            //{
            //    option.Configuration = Configuration["AppSettings:ConnectionStrings:RedisConnection"]; 
            //    option.InstanceName = "master";                
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
