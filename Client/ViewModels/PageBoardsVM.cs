using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
   public class PageBoardVM
   {
      public string BoardId { get; set; }
      public NewBoard NewBoard { get; set; }
      public DecodeJwtVM DecodeJwtVM { get; set; }
   }
}