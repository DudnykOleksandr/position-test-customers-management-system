using System;

namespace Presentation.Dtos
{
    public class ContactDto : BaseDto
    {
        public Guid ContactId { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
