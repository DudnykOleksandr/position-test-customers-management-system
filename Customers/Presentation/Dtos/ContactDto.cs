using Data.Models;
using System;

namespace Presentation.Dtos
{
    public class ContactDto : BaseDto
    {
        public string ContactId { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
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
