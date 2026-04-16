using Microsoft.AspNetCore.Http.HttpResults;

namespace Corporate_Management.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; } 
        public string Address { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }

    public class UserListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }    
        public string Department { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int ManagerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
