using Data.Models;
using System;

namespace Presentation.Dtos
{
    public class DepartmentDto : BaseDto
    {
        public string DepartmentId { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string AddressId { get; set; }
        public AddressDto Address { get; set; }

        public Department ToDataModel()
        {
            var dataModel = new Department();
            dataModel.DepartmentId = Guid.Parse(DepartmentId);
            dataModel.CustomerId = Guid.Parse(CustomerId);
            dataModel.Name = this.Name;
            dataModel.ActionType = this.ActionType;

            dataModel.AddressId = Guid.Parse(this.AddressId);
            dataModel.Address = this.Address.ToDataModel();

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

            return dto;
        }
    }
}
