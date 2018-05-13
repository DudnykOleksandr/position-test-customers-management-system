using Data.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bussiness;

namespace Presentation.Dtos
{
    public class UserDto : BaseDto
    {
        [Required]
        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string UserId { get; set; }

        [StringLength(maximumLength: 36, MinimumLength = 36)]
        public string CustomerId { get; set; }

        [StringLength(maximumLength: 36)]
        public string DepartmentId { get; set; }

        public bool IsDepartmentManager { get; set; }

        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [MaxLength(128)]
        public string MiddleName { get; set; }

        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(30)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9-_\.]{1,30}$")]
        public string UserName { get; set; }

        [RegularExpression(@"[0-9a-zA-Z]{4,8}")]
        public string Password { get; set; }

        public User ToDataModel(IEnumerable<DepartmentDto> departments)
        {
            var dataModel = new User();
            dataModel.UserId = Guid.Parse(UserId);
            dataModel.CustomerId = Guid.Parse(CustomerId);
            dataModel.DepartmentId = string.IsNullOrEmpty(DepartmentId) ? (Guid?)null : Guid.Parse(DepartmentId);
            dataModel.FirstName = this.FirstName;
            dataModel.MiddleName = this.MiddleName;
            dataModel.LastName = this.LastName;
            dataModel.Phone = this.Phone;
            dataModel.Email = this.Email;
            dataModel.Role = UserRole.RegularUser;
            dataModel.UserName = this.UserName;
            dataModel.IsDepartmentManager = this.IsDepartmentManager;
            dataModel.ActionType = this.ActionType;

            if (dataModel.ActionType == EntityActionType.Add && string.IsNullOrEmpty(this.Password))
                throw new Exception("User model is not valid");

            if (!string.IsNullOrEmpty(this.Password))
            {
                dataModel.PasswordHashSalt = AccountManager.GenerateRandomSalt();
                dataModel.PasswordHash = AccountManager.Hash(dataModel.PasswordHashSalt, this.Password);
            }


            return dataModel;
        }

        public static UserDto FromDataModel(User dataModel)
        {
            var dto = new UserDto
            {
                UserId = dataModel.UserId.ToString(),
                DepartmentId = dataModel.DepartmentId.ToString(),
                CustomerId = dataModel.CustomerId.ToString(),
                FirstName = dataModel.FirstName,
                MiddleName = dataModel.MiddleName,
                LastName = dataModel.LastName,
                Phone = dataModel.Phone,
                Email = dataModel.Email,
                UserName = dataModel.UserName,
                IsDepartmentManager = dataModel.IsDepartmentManager
            };

            return dto;
        }
    }
}
