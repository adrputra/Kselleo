using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CardId { get; set; }
        public string Comment_ { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
        public virtual Card Card { get; set; }
    }
}
