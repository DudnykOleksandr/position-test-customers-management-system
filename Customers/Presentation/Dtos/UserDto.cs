using System;

namespace Presentation.Dtos
{
    public class UserDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsDepartmentManager { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public short Role { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordHashSalt { get; set; }

        public CustomerDto Customer { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
