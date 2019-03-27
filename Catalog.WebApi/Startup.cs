using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Service;
using Catalog.Domain.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserIdProvider<string>, HttpContextUserIdProvider>();
            services.AddDbContext<CatalogDbContext>(builder => {
                builder.UseNpgsql(Configuration.GetConnectionString("CatalogDb"));
            });

            services.AddIdentity<CatalogUser, IdentityRole>()
                .AddEntityFrameworkStores<CatalogDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configuration => {
                configuration.RequireHttpsMetadata = true;
                configuration.SaveToken = true;
                configuration.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}