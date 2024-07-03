using System.ComponentModel.DataAnnotations;

namespace EmployeesManagment.Models
{
    public class LeaveApplication : ApprovalActivity
    {
        public int Id { get; set; }
        [Display(Name = "Employee Name")]
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public Employee Employee { get; set; }
        [Display(Name = "N° of Days")]
        public int NoOfDays { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Duration")]
        public int DurationId { get; set; }
        public SystemCodeDetail Duration { get; set; }
        [Display(Name = "Leave Type")]
        public int leaveTypeId { get; set; }
        public LeaveType leaveType { get; set; }
        public string? Attachment { get; set; }
        [Display(Name = "Note")]
        public string Description { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public SystemCodeDetail Status { get; set; }
        public string? ApprovalNotes { get; set; }


    }

}
