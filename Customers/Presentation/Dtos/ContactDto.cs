using Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class ContactDto : BaseDto
    {
        [Required]
        [StringLength(maximumLength: 36,MinimumLength = 36)]
        public string ContactId { get; set; }

        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(30)]
        public string Phone { get; set; }

        public Contact ToDataModel()
        {
            var dataModel = new Contact();
            dataModel.ContactId = Guid.Parse(ContactId);
            dataModel.CustomerId = Guid.Parse(CustomerId);
            dataModel.Name = this.Name;
            dataModel.Role = this.Role;
            dataModel.Email = this.Email;
            dataModel.Phone = this.Phone;
            dataModel.ActionType = this.ActionType;

            return dataModel;
        }

        public static ContactDto FromDataModel(Contact dataModel)
        {
            var dto = new ContactDto
            {
                ContactId = dataModel.ContactId.ToString(),
                CustomerId = dataModel.CustomerId.ToString(),
                Name = dataModel.Name,
                Role = dataModel.Role,
                Email = dataModel.Email,
                Phone = dataModel.Phone
            };

            return dto;
        }
    }
}
