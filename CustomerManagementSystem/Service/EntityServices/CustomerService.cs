using CustomerManagementSystem.Domain;
using CustomerManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerManagementSystem.Service
{
    public class CustomerService : GenericService<Customer>, ICustomer
    {
        public CustomerService(IUnitOfWork unitOfWork, IRepository<Customer> repository)
           : base(unitOfWork, repository)
        {
        }
    }
}