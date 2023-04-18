﻿namespace HRWeb.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public Leave() { }

        public Leave(int id, DateTime leaveStartDate, DateTime leaveEndDate, string reason, string status, int employeeid, int leavetypeid)
        {
            Id = id;
            LeaveStartDate = leaveStartDate;
            LeaveEndDate = leaveEndDate;
            Reason = reason;
            Status = status;
            EmployeeId = employeeid;
            LeaveTypeId = leavetypeid;
        }
    }

    
}
