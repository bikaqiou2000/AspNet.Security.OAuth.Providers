using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVCClient21.Extensions;
using MVCClient21.Models;
using Newtonsoft.Json.Serialization;

namespace MVCClient21
{
    public class Startup
    {

        public ILogger Logger { get; }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
            Logger = logger;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })
            .AddWeixin(options =>
            {
                if (HostingEnvironment.IsDevelopment())
                {
                    options.UserInformationEndpoint = "http://localhost:5000/cgi-bin/user/getuserinfo";
                    options.AuthorizationEndpoint = "http://localhost:5000/connect/oauth2/authorize";
                    options.TokenEndpoint = "http://localhost:5000/cgi-bin/gettoken";
                }
                options.ClientId = "wwbdf89a5953734930";
                options.ClientSecret = "kpQjF_Z6jwrPeU7HGRGXHUIHAY7EeXYkE3fouzS7wZQ";
                //options.Scope.Add("user:email");
            });


            //.AddGitHub(options =>
            //{
            //    options.ClientId = "49e302895d8b09ea5656";
            //    options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
            //    options.Scope.Add("user:email");

            //})


            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            Logger.LogInformation("confingur service");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts(); //如果代理服务器启用hsts则不需要
            }

            //app.UseHttpsRedirection(); //如果代理服务器启用https则不需要
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            

            app.UseMvc();

            
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});

            //终结
            //app.Run(async (context ) =>
            //{
            //    Console.WriteLine("hello in pipeline !"); 
            //});


            //app.Map("/map1", (appbuild) =>
            //{
            //    appbuild.Run( async (ctx) => await ctx.Response.WriteAsync("ddd"));
            //});

            //app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
            //    HandleBranch);

            //app.UseRequestCulture();

        }

        //private static void HandleBranch(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        var branchVer = context.Request.Query["branch"];
        //        await context.Response.WriteAsync($"Branch used = {branchVer}");
        //    });
        //}
    }
}
