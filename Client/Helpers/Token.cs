using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Client.ViewModels;
using Microsoft.AspNetCore.Http;


namespace Kselleo.Helpers
{
   public class Token
   {
      public static HttpContext _httpContext;

      public Token(HttpContext httpContext)
      {
         _httpContext = httpContext;
      }

      public static DecodeJwtVM DecodeToken()
      {
         var JWToken = _httpContext.Session.GetString("JWToken");
         Console.WriteLine($"Decode JWT TOKEN => {JWToken}");
         if (JWToken == null) return null;

         var handler = new JwtSecurityTokenHandler();
         var token = handler.ReadJwtToken(JWToken);

         var email = token.Claims.First(claim => claim.Type == "email").Value;
         var fullName = token.Claims.First(claim => claim.Type == "fullName").Value;
         var image = token.Claims.First(claim => claim.Type == "image").Value;
         var role = token.Claims.First(claim => claim.Type == "role").Value;

         var result = new DecodeJwtVM
         {
            Email = email,
            FullName = fullName,
            Image = image,
            Roles = role
         };

         return result;
      }
   }
}