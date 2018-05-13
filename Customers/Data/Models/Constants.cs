
namespace Data.Models
{
    public static class Constants
    {
        public const string UserNamePattern = @"^[a-zA-Z][a-zA-Z0-9-_\.]{1,30}$";
        public const string UserPasswordPattern = @"[0-9a-zA-Z]{4,8}";

        public const string AdminPolicyName = "Admin";
        public const string AdminClaimTypeName= "IsAdmin";
        public const string CustomerIdClaimTypeName = "CustomerId";
        
        

    }
}
