using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
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
            services.AddControllersWithViews();
            //Dil desteði :
            services.AddSession(); //Oturum ekleme //Session kýsmýný controllerdan sildim. 
            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("tr"),
                    new CultureInfo("en")
                };
                opt.DefaultRequestCulture = new RequestCulture("tr");
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            //Proje seviyesinde authorization yapmak için kullanýlýr.
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddMvc();
            services.AddAuthentication(
                        CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(X =>
                        {
                            X.LoginPath = "/Login/Index/"; //return url yapacaðýmýz yeri yazarýz.
                        }
                );

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePages(); 404 sayfasýna yönlendirir.
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession(); //oTURUMU KULLAN Dil desteði

            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Widget}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
