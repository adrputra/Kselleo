using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CheckListItemAssign
    {
        [Key]
        public int Id { get; set; }
        public int CheckListItemId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual CheckListItem CheckListItem{ get; set; }
    }
}
