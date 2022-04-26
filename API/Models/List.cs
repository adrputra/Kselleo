﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class List
    {
        [Key]
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        //public virtual User User { get; set; }
        public virtual Board Board { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
