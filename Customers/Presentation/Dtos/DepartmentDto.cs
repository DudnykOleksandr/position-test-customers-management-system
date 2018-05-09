using Data.Models;
using System;
using System.Collections.Generic;

namespace Presentation.Dtos
{
    public partial class DepartmentDto
    {
        public DepartmentDto()
        {
        }

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }

        public AddressDto Address { get; set; }
    }
}
