using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts{ get; set; }
        public DbSet<Board> Boards{ get; set; }
        public DbSet<Card> Cards{ get; set; }
        public DbSet<CheckListItem> CheckListItems{ get; set; }
        public DbSet<CheckListItemAssign> CheckListItemsAssigns{ get; set; }
        public DbSet<Comment> Comments{ get; set; }
        public DbSet<List> Lists{ get; set; }
        public DbSet<MemberBoard> MemberBoards{ get; set; }
        public DbSet<MemberCard> MemberCards{ get; set; }
        public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(user => user.Account)
                .WithOne(acc => acc.User)
                .HasForeignKey<Account>(acc => acc.UserId);

            //modelBuilder.Entity<User>()
            //    .HasOne(user => user.Board)
            //    .WithOne(board => board.User)
            //    .HasForeignKey<Board>(board => board.CreatedBy);

            //modelBuilder.Entity<User>()
            //    .HasOne(user => user.List)
            //    .WithOne(list => list.User)
            //    .HasForeignKey<List>(list => list.CreatedBy);

            //modelBuilder.Entity<User>()
            //    .HasOne(user => user.Card)
            //    .WithOne(card => card.User)
            //    .HasForeignKey<Card>(card => card.CreatedBy);

            modelBuilder.Entity<Board>()
                .HasOne(board => board.User)
                .WithMany(user => user.Boards)
                .HasForeignKey(board => board.CreatedBy);

            modelBuilder.Entity<List>()
                .HasOne(list => list.User)
                .WithMany(user => user.Lists)
                .HasForeignKey(list => list.CreatedBy);

            modelBuilder.Entity<Card>()
                .HasOne(card => card.User)
                .WithMany(user => user.Cards)
                .HasForeignKey(card => card.CreatedBy);

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId);

            modelBuilder.Entity<MemberBoard>()
                .HasOne(mb => mb.User)
                .WithMany(user => user.MemberBoards)
                .HasForeignKey(mb => mb.UserId);

            modelBuilder.Entity<MemberBoard>()
                .HasOne(mb => mb.Board)
                .WithMany(board => board.MemberBoards)
                .HasForeignKey(mb => mb.BoardId);

            modelBuilder.Entity<Board>()
                .HasMany(board => board.Lists)
                .WithOne(list => list.Board)
                .HasForeignKey(list => list.BoardId);

            modelBuilder.Entity<List>()
                .HasMany(list => list.Cards)
                .WithOne(card => card.List)
                .HasForeignKey(card => card.ListId);

            modelBuilder.Entity<Card>()
                .HasMany(card => card.CheckListItems)
                .WithOne(check => check.Card)
                .HasForeignKey(check => check.CardId);

            modelBuilder.Entity<CheckListItemAssign>()
                .HasOne(assign => assign.User)
                .WithMany(user => user.CheckListItemAssigns)
                .HasForeignKey(assign => assign.UserId);

            modelBuilder.Entity<CheckListItemAssign>()
                .HasOne(assign => assign.CheckListItem)
                .WithMany(check => check.CheckListItemAssigns)
                .HasForeignKey(assign => assign.CheckListItemId);
        }
    }
}
