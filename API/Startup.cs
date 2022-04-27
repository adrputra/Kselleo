using API.Context;
using API.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
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
         services.AddScoped<AccountRepository>();
         services.AddScoped<BoardRepository>();
         services.AddScoped<CardRepository>();
         services.AddScoped<CheckListItemAssignRepository>();
         services.AddScoped<CheckListItemRepository>();
         services.AddScoped<CommentRepository>();
         services.AddScoped<ListRepository>();
         services.AddScoped<MemberBoardRepository>();
         services.AddScoped<MemberCardRepository>();
         services.AddScoped<UserRepository>();
         services.AddScoped<VerifyInviteRepository>();

         services.AddDbContext<MyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("API")));

         //services.AddDbContext<MyContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("API")));

         services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
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
               ValidIssuer = Configuration["Jwt:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
            };
         });// cors
         services.AddCors(c =>
      {
         c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
      });

      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseHttpsRedirection();

         app.UseRouting();

         // cors
         app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());


         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}