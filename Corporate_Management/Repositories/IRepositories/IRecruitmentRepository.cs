using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IRecruitmentRepository
    {
        Task<int> createJob(CreateJobsDto model);
        Task<int> deleteJob(int jobId);
        Task<IEnumerable<JobModel>> GetAllJobs();
        Task<int> PublishJobAsync(int jobId);
        Task<JobModel> getJobById(int jobId);
        Task<bool> UpdateJobAsync(int jobId, UpdateJobDto dto);
        Task<int> setStatusOnHold(int jobId);
        Task<int> setStatusClosed(int jobId);
        Task<bool> ApplyJob(int jobId, int userId);
        Task<IEnumerable<CandidateDto>> GetCandidatesByJobId(int jobId);

    }
}
