using Microsoft.AspNetCore.Identity;

namespace HRWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public int DepartmentId { get; set; }



    }
}