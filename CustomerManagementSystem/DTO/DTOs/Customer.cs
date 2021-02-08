using System.Collections.Generic;

namespace CustomerManagementSystem.DTO
{
    public class Customers : List<Customer> { }
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
    }
}