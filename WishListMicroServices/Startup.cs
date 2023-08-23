using CommonLayer;
using Inventory_Management_System_BE.Common_Utility;
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
using RepositoryLayers.Interface;
using RepositoryLayers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishListMicroServices
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

            services.AddControllers();
            //if (DateTime.Now < Convert.ToDateTime(CommonUtility.Decrypt(Configuration["Master"])))
            //{
                services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WishListMicroServices", Version = "v1" });
            });
            //}
            //services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration["ConnectionStrings:DBSettingConnection"]));
            #region Database Connectivity

            string mySqlConnectionStr = Configuration.GetConnectionString("MySqlDBConnection");
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            #endregion


            #region
            //if (DateTime.Now < Convert.ToDateTime(CommonUtility.Decrypt(Configuration["Master"])))
            //{
            //services.AddScoped<IAuthRL, AuthRL>();
            //services.AddScoped<ICardRL, CardRL>();
            //services.AddScoped<IProductRL, ProductRL>();
            services.AddScoped<IWishListRL, WishListRL>();
                //services.AddScoped<IFeedbackRL, FeedbackRL>();
                //services.AddScoped<SqlQueries>();
            //}
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //if (DateTime.Now < Convert.ToDateTime(CommonUtility.Decrypt(Configuration["Master"])))
                //{
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WishListMicroServices v1"));
                //}
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            #region Cors
            //if (DateTime.Now < Convert.ToDateTime(CommonUtility.Decrypt(Configuration["Master"])))
            //{
                app.UseCors();
                app.UseCors(builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            //}

            #endregion


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
