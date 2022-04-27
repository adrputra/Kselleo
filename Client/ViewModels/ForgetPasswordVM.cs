using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   public class ForgetPasswordVM
   {
      [Display(Name = "Email")]
      [Required(ErrorMessage = "Email is required")]
      public string Email { get; set; }

   }
}