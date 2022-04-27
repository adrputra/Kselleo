using Client.Base;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
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
         services.AddControllersWithViews().AddNewtonsoftJson();
         services.AddSession(options =>
         {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
         });

         services.AddScoped<AuthRepository>();
         services.AddScoped<Address>();


         services.AddAuthentication(auth =>
         {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(options =>
         {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
               ValidateIssuer = true,
               ValidateAudience = false,
               ValidAudience = Configuration["Jwt:Audience"],
               ValidIssuer = Configuration["Jwt:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"])),
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
            };
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
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }
         app.UseHttpsRedirection();
         app.UseStaticFiles();

         app.UseRouting();
         app.UseSession();
         app.Use(async (context, next) =>
         {
            var JWToken = context.Session.GetString("JWToken");
            Console.WriteLine($"token from startup => {JWToken}");

            if (!string.IsNullOrEmpty(JWToken))
            {
               context.Request.Headers.Add("Authorization", "Bearer" + JWToken);
            }
            await next();
         });

         // app.UseStatusCodePages(async context =>
         //       {
         //          var request = context.HttpContext.Request;
         //          var response = context.HttpContext.Response;

         //          // Console.WriteLine("STATUS CODE {0}", response.StatusCode);

         //          if (response.StatusCode.Equals((int)HttpStatusCode.Forbidden))
         //          {
         //             response.Redirect("/forbidden");
         //          }
         //          else if (response.StatusCode.Equals((int)HttpStatusCode.Unauthorized))
         //          {
         //             response.Redirect("/unauthorized");
         //          }
         //          else if (response.StatusCode.Equals((int)HttpStatusCode.NotFound))
         //          {
         //             response.Redirect("/notfound");
         //          }
         //       });

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Auth}/{action=Login}/{id?}");
         });
      }
   }
}
