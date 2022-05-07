using System;
using System.Collections.Generic;

namespace API.ViewModel
{
   public class CreateTaskVM
   {
      public string Name { get; set; }
      public int CardId { get; set; }
      public DateTime Due { get; set; }
      public List<int> Members { get; set; }
   }
}
