using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class MemberCard
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public virtual User User { get; set; }
        public virtual Card Card{ get; set; }
    }
}
