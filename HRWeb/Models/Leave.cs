namespace HRWeb.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string Reason { get; set; }


        public Employee? Employee { get; set; }

        public Leave() { }

        public Leave(int id, string employeeId, DateTime leaveStartDate, DateTime leaveEndDate, string reason, Employee? employee)
        {
            Id = id;
            EmployeeId = employeeId;
            LeaveStartDate = leaveStartDate;
            LeaveEndDate = leaveEndDate;
            Reason = reason;
            Employee = employee;
        }
    }

    
}
