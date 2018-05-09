using Data.Models;
using System;

namespace Presentation.Dtos
{
    public partial class ContactDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
