using MagicMirror.DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            Database.EnsureCreated();
            var home = AddressTypes.Find(AddressType.Home.Id);
            if (home == null)
            {
                AddressTypes.Add(AddressType.Home);
            }
            var work = AddressTypes.Find(AddressType.Work.Id);
            if (work == null)
            {
                AddressTypes.Add(AddressType.Work);
            }
            if (home == null || work == null)
            {
                SaveChanges();
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