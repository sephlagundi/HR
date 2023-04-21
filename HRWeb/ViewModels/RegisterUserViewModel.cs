using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HRWeb.ViewModels
{
    public class RegisterUserViewModel
    {
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid name format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid name format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }

       /* public string roleName { get; set; }*/
    }
}