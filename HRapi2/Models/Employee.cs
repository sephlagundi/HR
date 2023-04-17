using System.ComponentModel.DataAnnotations;

namespace HRapi2.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public DateTime DOB { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        public string Phone { get; set; }

        //public int DepartmentId { get; set; }
        //public Department? Department { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Review { get; set; }

        //public int BenefitId { get; set; }
        //public Benefit? Benefit { get; set; }

        public Employee()
        {

        }


        public Employee(int id, string name, string email, string phone, string review)
        {
            Id = id;
            Name = name;
            //DOB = dob;
            Email = email;
            Phone = phone;
            //DepartmentId = departmentid;
            Review = review;
            //BenefitId = benefitid;


        }


    }
}
