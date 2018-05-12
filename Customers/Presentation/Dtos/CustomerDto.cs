using Data.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class CustomerDto : BaseDto
    {
        public CustomerDto()
        {
            Contacts = new HashSet<ContactDto>();
            Departments = new HashSet<DepartmentDto>();
            Users = new HashSet<UserDto>();
        }

        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
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
            var dataModel = new Customer
            {
                CustomerId = Guid.Parse(CustomerId),
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone,
                Comments = this.Comments,
                Type = this.Type,
                NumberOfSchools = this.NumberOfSchools,
                ActionType = this.ActionType,

                AddressId = Guid.Parse(this.AddressId),
                Address = this.Address.ToDataModel()
            };
            dataModel.Address.ActionType = dataModel.ActionType;

            foreach (var contact in Contacts)
                dataModel.Contacts.Add(contact.ToDataModel());

            foreach (var department in Departments)
                dataModel.Departments.Add(department.ToDataModel());

            foreach (var user in Users)
                dataModel.Users.Add(user.ToDataModel());

            return dataModel;
        }

        public static CustomerDto FromDataModel(Customer dataModel)
        {
            var dto = new CustomerDto
            {
                CustomerId = dataModel.CustomerId.ToString(),
                Name = dataModel.Name,
                Email = dataModel.Email,
                Phone = dataModel.Phone,
                Comments = dataModel.Comments,
                Type = dataModel.Type,
                NumberOfSchools = dataModel.NumberOfSchools,
                AddressId = dataModel.AddressId.ToString(),
                Address = AddressDto.FromDataModel(dataModel.Address)
            };

            foreach (var contact in dataModel.Contacts)
                dto.Contacts.Add(ContactDto.FromDataModel(contact));

            foreach (var department in dataModel.Departments)
                dto.Departments.Add(DepartmentDto.FromDataModel(department));

            foreach (var user in dataModel.Users)
                dto.Users.Add(UserDto.FromDataModel(user));

            return dto;
        }
    }
}
