using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class Department
    {
        public Department()
        {
            User = new HashSet<User>();
        }

        public Guid DepartmentId { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }

        public Address Address { get; set; }
        public Customer Customer { get; set; }
        public ICollection<User> User { get; set; }
    }
}
