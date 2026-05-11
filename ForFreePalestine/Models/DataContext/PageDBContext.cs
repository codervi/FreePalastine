using Microsoft.EntityFrameworkCore;

namespace ForFreePalestine.Models.DataContext
{
    public class PageDBContext : DbContext
    {
        public PageDBContext(DbContextOptions<PageDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Bu satırın olduğundan emin ol

            // ForumReply ile User arasındaki silme döngüsünü kırıyoruz
            modelBuilder.Entity<ForumReply>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Döngüyü kıran sihirli dokunuş

            // ForumReply ile Thread arasındaki silme döngüsünü de sağlama alalım
            modelBuilder.Entity<ForumReply>()
                .HasOne(r => r.Thread)
                .WithMany(t => t.Replies)
                .HasForeignKey(r => r.ThreadId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<HistoryInfo> HistoryInfos { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumReply> ForumReplies { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
    }
}
