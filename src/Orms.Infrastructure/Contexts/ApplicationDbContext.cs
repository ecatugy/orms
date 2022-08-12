using Microsoft.EntityFrameworkCore;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces.DataBase;
using System.Data;


namespace Orms.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public IDbConnection Connection => Database.GetDbConnection();   

        /// <summary>
        /// For use in sqlserver
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Seed database with users 
            modelBuilder.Entity<User>().HasData(new User { UserID = 1, Name = "Edson", Surname="Catugy", DateInsert = DateTime.Now });
            modelBuilder.Entity<User>().HasData(new User { UserID = 2, Name = "Carlos", Surname = "Mendes", DateInsert = DateTime.Now });
            modelBuilder.Entity<User>().HasData(new User { UserID = 3, Name = "Patrick", Surname = "Siqueira", DateInsert = DateTime.Now });
            modelBuilder.Entity<User>().HasData(new User { UserID = 4, Name = "Luciano", Surname = "Silva", DateInsert = DateTime.Now });

            //Unique names
            modelBuilder.Entity<User>()
           .HasIndex(b => b.Name)
           .HasDatabaseName("Index_Name")
           .IsUnique();

        }
    }
}
