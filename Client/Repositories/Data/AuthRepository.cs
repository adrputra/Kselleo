using Client.Repositories;
using Client.ViewModels;
using Client.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
   public class AuthRepository : GeneralRepository<LoginVM, string>
   {
      private readonly Address address;
      private readonly HttpClient httpClient;
      private readonly string request;
      private readonly IHttpContextAccessor _contextAccessor;

      public AuthRepository(Address address, string request = "accounts/") : base(address, request)
      {
         this.address = address;
         this.request = request;
         _contextAccessor = new HttpContextAccessor();
         httpClient = new HttpClient
         {
            BaseAddress = new Uri(address.link)
         };
      }

      public async Task<JwtResponseVM> Auth(LoginVM loginVM)
      {
         JwtResponseVM jwt = null;

         StringContent content = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");
         Console.WriteLine($"content => {content}");

         var result = await httpClient.PostAsync(address.link + request + "login", content);

         Console.WriteLine($"result => {result}");
         string apiResponse = await result.Content.ReadAsStringAsync();

         jwt = JsonConvert.DeserializeObject<JwtResponseVM>(apiResponse);
         return jwt;
      }

   }
}