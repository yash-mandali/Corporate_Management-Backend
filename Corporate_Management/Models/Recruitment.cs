namespace Corporate_Management.Models
{
      public class JobModel
        {
            public int JobId { get; set; }
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
            public string Status { get; set; } = "Draft";   
            public DateTime? Publish_date { get; set; }
            public DateTime? Application_deadline { get; set; }
            public bool IdDeleted { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public DateTime? Deleted_At { get; set; }
    }
    }

