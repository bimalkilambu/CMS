using System;
using System.Collections.Generic;

namespace CustomerManagementSystem.DTO
{
    public class CustomerMapper
    {
        public Customers Map(List<Domain.Customer> model)
        {
            if (model == null || model.Count == 0)
            {
                return null;
            }

            Customers customers = new Customers();
            foreach (var item in model)
            {
                Customer customer = new Customer();
                customer.Fullname = $"{item.FirstName} {item.LastName}";
                customer.Address = item.Address;
                customer.Email = item.Email;
                customer.Contact = item.ContactNumber;
                customer.Gender = item.Gender;
                customer.CustomerId = item.Guid.ToString();
                
                customers.Add(customer);
            }

            return customers;
        }

        public Customer Map(Domain.Customer model)
        {
            if (model == null)
            {
                return null;
            }

            Customer customer = new Customer();
            customer.Firstname = model.FirstName;
            customer.Lastname = model.LastName;
            customer.Fullname = $"{model.FirstName} {model.LastName}";
            customer.Address = model.Address;
            customer.Email = model.Email;
            customer.Contact = model.ContactNumber;
            customer.Gender = model.Gender;
            customer.CustomerId = model.Guid.ToString();

            return customer;
        }

        public Domain.Customer Map(Customer model, Domain.Customer customer)
        {
            customer.FirstName = model.Firstname;
            customer.LastName = model.Lastname;
            customer.Address = model.Address;
            customer.Email = model.Email;
            customer.ContactNumber = model.Contact;
            customer.Gender = model.Gender;

            if (customer.Guid == Guid.Empty)
            {
                customer.DateCreated = DateTime.Now;
                customer.Guid = Guid.NewGuid();
            }
            else
            {
                customer.DateModified = DateTime.Now;
            }

            return customer;
        }
    }
}