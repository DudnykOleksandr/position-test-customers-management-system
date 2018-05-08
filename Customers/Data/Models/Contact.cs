using System;

namespace Data.Models
{
    public partial class Contact
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public Customer Customer { get; set; }
    }
}
