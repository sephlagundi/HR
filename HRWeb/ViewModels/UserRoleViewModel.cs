using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRWeb.ViewModels
{
    public class UserRoleViewModel
    {
        public string userId { get; set; }
        public string roleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Birth")]
        public DateTime DOB { get; set; }


        public string DepartmentName { get; set; }

    }
}
