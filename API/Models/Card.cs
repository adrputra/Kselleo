using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public int ListId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }
        public DateTime Due { get; set; }
        public virtual User User { get; set; }
        public virtual List List { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CheckListItem> CheckListItems{ get; set; }
        public virtual ICollection<MemberCard> MemberCards { get; set; }
    }
}
