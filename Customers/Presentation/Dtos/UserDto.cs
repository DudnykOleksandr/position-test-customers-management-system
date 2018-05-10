
using Data.Models;
using System;
using System.Security;

namespace Presentation.Dtos
{
    public class UserDto : BaseDto
    {
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string DepartmentId { get; set; }
        public bool IsDepartmentManager { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public User ToDataModel()
        {
            var dataModel = new User();
            dataModel.UserId = Guid.Parse(UserId);
            dataModel.CustomerId = Guid.Parse(CustomerId);
            dataModel.DepartmentId = string.IsNullOrEmpty(DepartmentId) ? (Guid?)null : Guid.Parse(DepartmentId);
            dataModel.IsDepartmentManager = this.IsDepartmentManager;
            dataModel.FirstName = this.FirstName;
            dataModel.MiddleName = this.MiddleName;
            dataModel.LastName = this.LastName;
            dataModel.Phone = this.Phone;
            dataModel.Email = this.Email;
            dataModel.Role = this.Role;
            dataModel.UserName = this.UserName;
            dataModel.ActionType = this.ActionType;

            if (this.ActionType == EntityActionType.Add || !string.IsNullOrEmpty(this.Password))
            {
                //todo add password management
            }


            return dataModel;
        }

        public static UserDto FromDataModel(User dataModel)
        {
            var dto = new UserDto
            {
                UserId= dataModel.UserId.ToString(),
                DepartmentId = dataModel.DepartmentId.ToString(),
                CustomerId = dataModel.CustomerId.ToString(),
                IsDepartmentManager = dataModel.IsDepartmentManager,
                FirstName = dataModel.FirstName,
                MiddleName = dataModel.MiddleName,
                LastName = dataModel.LastName,
                Phone = dataModel.Phone,
                Email = dataModel.Email,
                Role = dataModel.Role,
                UserName = dataModel.UserName
            };

            return dto;
        }
    }
}
