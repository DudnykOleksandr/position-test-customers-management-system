using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class AddressDto
    {
        public AddressDto()
        {
        }

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
    }
}
