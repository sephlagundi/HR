using System.ComponentModel.DataAnnotations;

namespace HRWeb.Models
{
    public class Department
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid department format. Only alphabets, spaces, hyphens, and apostrophes are allowed.")]
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }


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
