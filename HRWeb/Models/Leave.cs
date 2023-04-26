using HRWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRWeb.Models
{
    public class Leave
    {
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        [FutureDate(ErrorMessage = "Start date must be a future date.")]
        public DateTime LeaveStartDate { get; set; }

        [Display(Name = "End Date")]
        [FutureDate(ErrorMessage = "End date must be a future date.")]
        public DateTime LeaveEndDate { get; set; }

        public string Reason { get; set; }
        public string Status { get; set; } = "pending";


        public string? OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }


        public int LeaveTypeId { get; set; }
        [Display(Name = "Type of Leave")]
        public LeaveType? LeaveType { get; set; }






        [Display(Name = "Date Created")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(8);

        public Leave()
        {
            Status = "pending";
        }

        public Leave(int id, DateTime leaveStartDate, DateTime leaveEndDate, string reason, string status, int employeeid, int leavetypeid, string ownerId)
        {
            Id = id;
            LeaveStartDate = leaveStartDate;
            LeaveEndDate = leaveEndDate;
            Reason = reason;
            Status = status;
            LeaveTypeId = leavetypeid;
            OwnerId = ownerId;
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date >= DateTime.Today;
        }
    }
}



