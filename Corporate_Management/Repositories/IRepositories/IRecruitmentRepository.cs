using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IRecruitmentRepository
    {
        Task<int> createJob(CreateJobsDto model);
        Task<int> deleteJob(int jobId);
        Task<IEnumerable<JobModel>> GetAllJobs();
    }
}
