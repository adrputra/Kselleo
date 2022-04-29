using System;
using System.Collections;
using System.Linq;
using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
   public class MemberBoardRepository : GeneralRepository<MyContext, MemberBoard, int>

   {
      public MemberBoardRepository(MyContext myContext) : base(myContext)
      {
         this.myContext = myContext;
      }

      public IEnumerable GetMemberByBoard(int Id)
      {
         var members = myContext.MemberBoards.Include(mb => mb.User).Include(mb => mb.Board).Where(mb => mb.BoardId == Id).ToList();
         return members;
      }

      public int DeleteMember(int id)
      {
         // get data memberBoards
         var member = myContext.MemberBoards.SingleOrDefault(mb => mb.Id == id);
         Console.WriteLine($"MEMBER INVITE => {0}", member);
         if (member != null)
         {
            // delete data verifyInvite by userId and boardId  from memberBoards
            var verifyInvite = myContext.VerifyInvites.SingleOrDefault(vi => vi.UserId == member.UserId && vi.BoardID == member.BoardId);
            Console.WriteLine($"VERIFY INVITE => {0}", verifyInvite);
            myContext.VerifyInvites.Remove(verifyInvite);
            myContext.MemberBoards.Remove(member);
            myContext.SaveChanges();
            return 1;
         }
         throw new Exception("Member not found");
      }


   }
}
