using Microsoft.Extensions.DependencyInjection;
using Orms.Domain.Interfaces.DataBase;


namespace Orms.Persistence.Helpers
{
    public static class DataBase
    {
        public static void SeedDbMemory(IServiceScope scope)
        {
            var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
            dbInitializer?.Initialize();
            dbInitializer?.SeedData();
        }
    }
}
