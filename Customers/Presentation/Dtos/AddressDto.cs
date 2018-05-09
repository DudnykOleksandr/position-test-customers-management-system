using Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class AddressDto : BaseDto
    {
         [Required]
        public string Id { get; set; }

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
            dataModel.Id = Guid.Parse(Id);
            dataModel.Country = this.Country;
            dataModel.City = this.City;
            dataModel.Address1 = this.Address;

            return dataModel;
        }
    }
}
