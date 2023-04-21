using HRWeb.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRWeb.Models
{
    public class Department
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid department format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }

        [NotMapped]
        public List<RegisterUserViewModel> RegisterUserViewModels { get; set; }


        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
