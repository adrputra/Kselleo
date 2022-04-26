using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CheckListItem
    {
        [Key]
        public int Id { get; set; }
        public int CardId { get; set; }
        public string Name { get; set; }
        public DateTime Due { get; set; }
        public virtual Card Card{ get; set; }
        public virtual ICollection<CheckListItemAssign> CheckListItemAssigns{ get; set; }
    }
}
