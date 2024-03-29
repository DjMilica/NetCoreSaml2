using ExampleWebApp.Models;
using ExampleWebApp.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saml2.Core.Configuration;
using Saml2.Core.Extensions;
using Saml2.Core.Models;

namespace ExampleWebApp
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
            services.AddTransient<IUserStore<ApplicationUser>, SimpleUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, SimpleRoleStore>();
            services.AddIdentity<ApplicationUser, ApplicationRole>();

            services.AddOptions();

            services.Configure<SamlConfiguration>(Configuration.GetSection("Saml"));

            services.AddDistributedMemoryCache();

            services.AddSamlServices();

            services
                .AddAuthentication(options =>
                {
                    //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                })
                .AddSaml("lala", options =>
                {
                    options.IdentityProviderConfiguration = new IdentityProviderConfiguration()
                    {
                        EntityId = "http://lala/exk19fnc2urbp2lBa5d7",
                    };
                })
                .AddSaml("okta", options =>
                {
                    options.IdentityProviderConfiguration = new IdentityProviderConfiguration()
                    {
                        EntityId = "http://www.okta.com/exk19fnc2urbp2lBa5d7",
                        HttpRedirectSingleSignOnService = "https://dev-7375101.okta.com/app/dev-7375101_milicatest_1/exk19fnc2urbp2lBa5d7/sso/saml",
                        AuthnRequestBinding = Saml2.Core.Enums.BindingType.HttpRedirect,
                        PublicKeyPath = "./Certs/idpPublicKey.cert",
                        UseNameIdAsSpUserId = true,
                        UserAttributeMapping = new UserAttributeMapping()
                        {
                            Email = "Email",
                            FirstName = "FirstName",
                            LastName = "LastName"
                        }
                    
                    };
                });


            services.AddControllersWithViews();
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
