﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
   public class Board
   {
      [Key]
      public string Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.Now;
      public int CreatedBy { get; set; }
      public virtual User User { get; set; }
      public virtual ICollection<VerifyInvite> VerifyInvites { get; set; }
      public virtual ICollection<List> Lists { get; set; }
      public virtual ICollection<MemberBoard> MemberBoards { get; set; }
   }
}
