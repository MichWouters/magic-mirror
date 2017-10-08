using MagicMirror.DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace MagicMirror.DataAccess
{
    public class SqliteContext : DbContext
    {
        private string _dbFile = "magicmirror.db";

        public SqliteContext()
        {
            CheckDb();
        }

        public SqliteContext(string dbFile)
        {
            _dbFile = dbFile;
            CheckDb();
        }

        private void CheckDb()
        {
            if (!System.IO.File.Exists(_dbFile))
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbFile}");
        }

        public void DeleteDatabase()
        {
            System.IO.File.Delete(_dbFile);
        }
    }
}