using CustomerManagementSystem.Domain;
using System.Data.Entity;

namespace CustomerManagementSystem.Repository
{
    public class DbContextFactory : IDbContextFactory
    {
        public DbContext GetContext()
        {
            return new CMSEntities();
        }
    }
}