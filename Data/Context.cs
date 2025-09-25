using Microsoft.EntityFrameworkCore;
using StarCitizenCompanion.Models;

namespace StarCitizenCompanion.Data
{
    public class Context : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<MaterialDeposit> MaterialDeposits { get; set; }
        public DbSet<NotificationEvent> Notifications { get; set; }

        private string _dbPath;

        public Context()
        {
            _dbPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "StarCitizenCompanion",
                "scc_data.db");

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_dbPath));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }
}