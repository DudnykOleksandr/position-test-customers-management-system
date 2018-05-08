using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Address
    {
        public Address()
        {
            Customer = new HashSet<Customer>();
            Department = new HashSet<Department>();
        }

        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }

        public ICollection<Customer> Customer { get; set; }
        public ICollection<Department> Department { get; set; }
    }
}
