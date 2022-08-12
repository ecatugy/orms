using Microsoft.Extensions.DependencyInjection;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces.DataBase;
using Orms.Persistence.Contexts;

namespace Orms.Persistence.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        }

        public void SeedData()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            context?.Users.Add(new User { UserID = 1, Name = "Edson", Surname = "Catugy", DateInsert = DateTime.Now });
            context?.Users.Add(new User { UserID = 2, Name = "Carlos", Surname = "Mendes", DateInsert = DateTime.Now });
            context?.Users.Add(new User { UserID = 3, Name = "Patrick", Surname = "Siqueira", DateInsert = DateTime.Now });
            context?.Users.Add(new User { UserID = 4, Name = "Luciano", Surname = "Silva", DateInsert = DateTime.Now });
            context?.SaveChanges();
        }
    }
}
