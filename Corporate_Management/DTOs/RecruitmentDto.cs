namespace Corporate_Management.DTOs
{
    public class CreateJobsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Employment_type { get; set; }
        public string Experience_required { get; set; }
        public int? Vacancies { get; set; }
        public string Required_skills { get; set; }
        public string Qualifications { get; set; }
        public string Responsibilities { get; set; }
        public decimal Salary_min { get; set; }
        public decimal Salary_max { get; set; }
        public string Currency { get; set; } = "INR";
        public DateTime? Publish_date { get; set; }
        public DateTime? Application_deadline { get; set; }
    }

    public class UpdateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Employment_type { get; set; }
        public string Experience_required { get; set; }
        public int Vacancies { get; set; }
        public string Required_skills { get; set; }
        public string Qualifications { get; set; }
        public string Responsibilities { get; set; }
        public decimal Salary_min { get; set; }
        public decimal Salary_max { get; set; }
        public string Currency { get; set; }
        public DateTime Application_deadline { get; set; }
    }

    public class CandidateDto
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ResumeUrl { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string department { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime AppliedDate { get; set; }
    }

    public class ApplyJobRequest
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public IFormFile Resume { get; set; }
    }
}
