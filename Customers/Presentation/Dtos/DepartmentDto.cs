using Data.Models;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Dtos
{
    public class DepartmentDto : BaseDto
    {
        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string DepartmentId { get; set; }

        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string AddressId { get; set; }

        [StringLength(maximumLength: 36)]
        public string ManagerUserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public AddressDto Address { get; set; }

        public Department ToDataModel()
        {
            var dataModel = new Department
            {
                DepartmentId = Guid.Parse(DepartmentId),
                CustomerId = Guid.Parse(CustomerId),
                Name = this.Name,
                ActionType = this.ActionType,

                AddressId = Guid.Parse(this.AddressId),
                Address = this.Address.ToDataModel()
            };

            dataModel.Address.ActionType = dataModel.ActionType;

            return dataModel;
        }

        public static DepartmentDto FromDataModel(Department dataModel)
        {
            var dto = new DepartmentDto
            {
                DepartmentId = dataModel.DepartmentId.ToString(),
                CustomerId = dataModel.CustomerId.ToString(),
                Name = dataModel.Name,

                AddressId = dataModel.AddressId.ToString(),
                Address = AddressDto.FromDataModel(dataModel.Address)
            };

            var managerUser = dataModel.Users.SingleOrDefault(u => u.IsDepartmentManager);
            if (managerUser != null)
            {
                dto.ManagerUserId = managerUser.UserId.ToString();
            }


            return dto;
        }
    }
}
