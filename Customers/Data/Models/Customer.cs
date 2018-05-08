using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contact = new HashSet<Contact>();
            Department = new HashSet<Department>();
            User = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comments { get; set; }
        public short Type { get; set; }
        public int NumberOfSchools { get; set; }

        public Address Address { get; set; }
        public ICollection<Contact> Contact { get; set; }
        public ICollection<Department> Department { get; set; }
        public ICollection<User> User { get; set; }
    }
}
