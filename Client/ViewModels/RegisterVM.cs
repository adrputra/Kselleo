using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   public class RegisterVM
   {
      [Display(Name ="Full Name")]
      [Required(ErrorMessage = "Name is required")]
      public string FullName { get; set; }

      [Display(Name = "Email")]
      [Required(ErrorMessage = "Email is required")]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string ConfirmPassword { get; set; }
    }
}