using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   public class ChangePasswordVM
   {
      [Display(Name = "Email")]
      [Required(ErrorMessage = "Email is required")]
      public string Email { get; set; }
      public int OTP { get; set; }
      public string NewPassword { get; set; }
      public string ConfirmPassword { get; set; }

   }
}