using System;

namespace Presentation.Dtos
{
    public class DepartmentDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }

        public AddressDto Address { get; set; }
    }
}
