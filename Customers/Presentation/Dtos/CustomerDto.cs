using Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            Contacts = new HashSet<ContactDto>();
            Departments = new HashSet<DepartmentDto>();
            Users = new HashSet<UserDto>();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string AddressId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(30)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Comments { get; set; }

        [Required]
        public CustomerType Type { get; set; }

        public int NumberOfSchools { get; set; }

        public AddressDto Address { get; set; }
        public ICollection<ContactDto> Contacts { get; set; }
        public ICollection<DepartmentDto> Departments { get; set; }
        public ICollection<UserDto> Users { get; set; }

        public Customer ToDataModel()
        {
            return new Customer();
        }
    }
}
