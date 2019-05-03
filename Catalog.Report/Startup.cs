using Catalog.BusinessLogicLayer.Service.Report;
using Catalog.DataAccessLayer;
using Catalog.Report.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Report
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(o =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
                    .UseNpgsql(Configuration.GetConnectionString(nameof(CatalogDbContext)));
                return dbContextOptionsBuilder.Options;
            });
            
            services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(nameof(CatalogDbContext)));
            services.AddScoped<IExchangeDataCalculator, ExchangeDataCalculator>();
            services.AddScoped<IExchangeDataReportService, ExchangeDataReportService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}