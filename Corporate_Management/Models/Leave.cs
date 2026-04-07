namespace Corporate_Management.Models
{
    public class Leave
    {
        public int LeaveRequestId { get; set; }
        public int UserId { get; set; }
        public string? RequestType { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        //public double TotalDays { get; set; }
        public string? Session { get; set; }
        public string? Reason { get; set; }
        public string? HandoverTo { get; set; }
        //public string Status { get; set; } = "Pending";
        public DateOnly AppliedOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }

    public class LeaveListDto
    {
        public int LeaveRequestId { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public string UserName { get; set; }
        public string? RequestType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalDays { get; set; }
        public string? Session { get; set; }
        public string? Reason { get; set; }
        public string? HandoverTo { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }    

    }
    public class updateLeaveDto
    {
        public int LeaveRequestId { get; set; }
        public string? RequestType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalDays { get; set; }
        public string? Session { get; set; }
        public string? Reason { get; set; }
        public string? HandoverTo { get; set; }
    }

    public class UserLeaveBalanceDto
    {
        public int LeavebalanceId { get; set; }
        public int UserId { get; set; }
        public int Leavetype_Id { get; set; }
        public decimal? TotalLeaveBalance { get; set; }
        public decimal? UsedLeaveBalance { get; set; }
        public decimal? RemainingLeaveBalance { get; set; }
        public int Balance_year { get; set; }
    }
}
