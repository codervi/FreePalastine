using Microsoft.EntityFrameworkCore;

namespace ForFreePalestine.Models.DataContext
{
    public class PageDBContext : DbContext
    {
        public PageDBContext(DbContextOptions<PageDBContext> options) : base(options)
        {
        }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<HistoryInfo> HistoryInfos { get; set; }
    }
}
