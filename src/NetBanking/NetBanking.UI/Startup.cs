using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetBanking.DATA.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBanking.UI
{
    public class Startup
    {
        const string NET_BANKING_CONNECTION = "NetBankingConnection";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSession();
            services.AddDbContext<netbankingContext>(options => options.UseSqlServer(Configuration.GetConnectionString(NET_BANKING_CONNECTION)));
            //services.AddScoped<netbankingContext>();
            services.AddAuthentication("AUT").AddCookie("AUT",op=> 
            { 
                op.Cookie.Name = "AUT"; 
                op.LoginPath = "/Index";
                op.LogoutPath = "/Index";
                op.AccessDeniedPath = "/Authenticate/AccessDenied";
                op.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });
            //services.AddAuthorization(
            //    op => 
            //    {
            //        op.AddPolicy("Administradores", p=>p.RequireClaim("Admin"));
            //        op.AddPolicy("SoloRRHH", opt => opt.RequireClaim("Departamento", "RRHH"));
            //    });
            services.AddRazorPages().AddRazorPagesOptions(op =>
            {
                op.Conventions.AuthorizeFolder("/");
                op.Conventions.AllowAnonymousToPage("/Index");
                op.Conventions.AllowAnonymousToPage("/Authenticate/Register");
                op.Conventions.AllowAnonymousToPage("/Authenticate/ForgotPassword");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
