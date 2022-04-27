using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   public class DecodeJwtVM
   {
      public string Id { get; set; }
      public string Email { get; set; }

      public string FullName { get; set; }
      public string Image { get; set; }
      public string Roles { get; set; }
      public string Exp { get; set; }
      public string Iss { get; set; }
   }
}