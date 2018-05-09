using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Department : Base
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }

        public Address Address { get; set; }
        public Customer Customer { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
