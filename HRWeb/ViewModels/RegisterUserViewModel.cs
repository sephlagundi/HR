using HRWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HRWeb.ViewModels
{
    [Keyless]
    public class RegisterUserViewModel
    {
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid name format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid name format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Birth")]
        /*[Range(typeof(DateTime), "1900,01,01", "{0:dd/MM/yyyy}", ErrorMessage = "Date of Birth cannot be in the future")]*/
        public DateTime DOB { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }


        public int DepartmentId { get; set; }
        public SelectList? DepartmentList { get; set; }
    }
}