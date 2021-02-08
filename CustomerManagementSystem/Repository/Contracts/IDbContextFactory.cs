using System.Data.Entity;

namespace CustomerManagementSystem.Repository
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}