using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class MemberBoard
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BoardId { get; set; }
        public string Role { get; set; }
        public virtual User User { get; set; }
        public virtual Board Board{ get; set; }
    }
}
