using Microsoft.AspNetCore.Identity;

namespace EmployeesManagment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";
        public string? CreatedById { get; set; }
        public int?  NationalId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LoginDate { get; set; }
        public string? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set;}
 public DateTime? PasswordChangedOn { get; set; }
        public string? RoleId { get; set; }

    public IdentityRole Role { get; set; }
    }
}
