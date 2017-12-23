using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace MagicMirror.DataAccess
{
    public class MirrorContext : DbContext
    {
        public MirrorContext() : base()
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ComplimentEntity> Compliments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
    }
}

//private string _dbFile = "magicmirror.db";

//public MirrorContext()
//{
//    CheckDb();
//}

//public MirrorContext(string dbFile)
//{
//    _dbFile = dbFile;
//    CheckDb();
//}

//private void CheckDb()
//{
//    Database.EnsureCreated();
//    var home = AddressTypes.Find(AddressType.Home.Id);
//    if (home == null)
//    {
//        AddressTypes.Add(AddressType.Home);
//    }
//    var work = AddressTypes.Find(AddressType.Work.Id);
//    if (work == null)
//    {
//        AddressTypes.Add(AddressType.Work);
//    }
//    if (home == null || work == null)
//    {
//        SaveChanges();
//    }
//}

//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//{
//    optionsBuilder.UseSqlite($"Data Source={_dbFile}");
//}

//public void DeleteDatabase()
//{
//    System.IO.File.Delete(_dbFile);
//}