using Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class AddressDto : BaseDto
    {
        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string AddressId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }

        [Required]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        public Address ToDataModel()
        {
            var dataModel = new Address();
            dataModel.AddressId = Guid.Parse(AddressId);
            dataModel.Country = this.Country;
            dataModel.City = this.City;
            dataModel.Address1 = this.Address;
            dataModel.ActionType = this.ActionType;

            return dataModel;
        }

        public static AddressDto FromDataModel(Address dataModel)
        {
            var dto = new AddressDto
            {
                AddressId = dataModel.AddressId.ToString(),
                Country = dataModel.Country,
                City = dataModel.City,
                Address = dataModel.Address1
            };

            return dto;
        }
    }
}
