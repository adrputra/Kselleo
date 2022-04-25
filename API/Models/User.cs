using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public virtual Account Account { get; set; }
        public virtual Board Board { get; set; }
        public virtual List List { get; set; }
        public virtual Card Card { get; set; }
        public virtual ICollection<MemberBoard> MemberBoards{ get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<MemberCard> MemberCards { get; set; }
        public virtual ICollection<CheckListItemAssign> CheckListItemAssigns{ get; set; }

    }
}
