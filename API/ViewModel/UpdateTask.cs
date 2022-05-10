using System;
using System.Collections.Generic;

namespace API.ViewModel
{
   public class UpdateTaskVM
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public int CardId { get; set; }
      public DateTime Due { get; set; }
      public DateTime StartDate { get; set; }
      public List<int> Members { get; set; }
   }
}
