using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRWeb.ViewModels
{
    public class EditUserViewModel
    {
        // view validations
/*        public string? Id { get; set; }*/
        [Required]
        public string FirstName { get; set; }
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

        public string Role { get; set; }
        public IList<string> Roles { get; set; }
        public IList<string>? Departments { get; set; }

        /*public string? SelectedRole { get; set; }*/
        [Display(Name ="Department")]
        public int DepartmentId { get; set; }

        public SelectList? DepartmentList { get; set; }
        /*public SelectList? RoleList { get; set; }*/
        /*public IEnumerable<SelectListItem> AllRoles { get; set; }*/

    }
}
