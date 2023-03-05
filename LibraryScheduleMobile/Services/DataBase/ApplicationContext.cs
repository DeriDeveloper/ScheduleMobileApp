using Microsoft.EntityFrameworkCore;

namespace ScheduleMobileApp.Services.DataBase
{
    internal class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<Models.Settings> Settings { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
