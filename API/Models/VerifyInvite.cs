using System.ComponentModel.DataAnnotations;

namespace API.Models
{
   public class VerifyInvite
   {
      [Key]
      public int Id { get; set; }
      public int UserId { get; set; }
      public int BoardID { get; set; }
      public bool IsAccept { get; set; }
      public bool IsUsed { get; set; }
      public string Role { get; set; }
      public User User { get; set; }
      public Board Board { get; set; }
   }
}
