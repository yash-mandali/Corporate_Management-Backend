using Corporate_Management.DTOs;
using Corporate_Management.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        public readonly IRecruitmentRepository _recruitmentRepository;

        public RecruitmentController(IRecruitmentRepository recruitmentRepository)
        {
            _recruitmentRepository = recruitmentRepository;
        }

        [HttpPost("createJob")]
        public async Task<IActionResult> createJob(CreateJobsDto model)
        {
            try
            {
                var newId = await _recruitmentRepository.createJob(model);
                return Ok(new
                {
                    success = true,
                    message = "Job created successfully",
                    jobId = newId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("deleteJob")]
        public async Task<IActionResult> deleteJob(int jobId)
        {
            try
            {
                var result = await _recruitmentRepository.deleteJob(jobId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Job deleted successfully"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Job not found"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("getAllJobs")]
        public async Task<IActionResult> getAllJobs()
        {
            try
            {
                var jobs = await _recruitmentRepository.GetAllJobs();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
