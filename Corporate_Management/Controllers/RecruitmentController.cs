using Corporate_Management.DTOs;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.Repositories;
using DocumentFormat.OpenXml.EMMA;
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

        [HttpPost("publishJob")]
        public async Task<IActionResult> PublishJob(int jobId)
        {
            try
            {
                var result = await _recruitmentRepository.PublishJobAsync(jobId);
                if (result == 1)
                {
                    return Ok(new { success = true,message = "Job published successfully"});
                }

                if (result == 0)
                {
                    return BadRequest(new {success = false, message = "Job is already published or deleted"});
                }

                if (result == -1)
                {
                    return NotFound(new{success = false,message = "Job not found"});
                }

                return StatusCode(500, new{success = false,message = "Unexpected error"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{success = false,message = ex.Message});
            }
        }

        [HttpGet("getJobById")]
        public async Task<IActionResult> getJobByJobId(int jobId)
        {
            try
            {
                var data = await _recruitmentRepository.getJobById(jobId);

                if (data == null)
                {
                    return NotFound(new { success = false, message = "job not found" });
                }

                return Ok(new
                {
                    message = "job found",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "job not found",
                    error = ex.Message
                });
            }
        }

        [HttpPut("updateJob")]
        public async Task<IActionResult> updateJob(int jobId, UpdateJobDto dto)
        {
            try
            {
                var result = await _recruitmentRepository.UpdateJobAsync(jobId,dto);
                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "job cannot be updated."
                    });
                }

                return Ok(new { message = "job updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "job not updated",
                    error = ex.Message
                });
            }
        }

        [HttpPost("OnHold")]
        public async Task<IActionResult> setStatusOnHold(int jobId)
        {
            try
            {
                var result = await _recruitmentRepository.setStatusOnHold(jobId);

                if (result == null)
                {
                    return BadRequest(new { message = "failed to change status" });
                }
                return Ok(new { message = "status changed", attendanceId = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "status change failed", error = ex.Message });
            }
        }

        [HttpPost("closed")]
        public async Task<IActionResult> setStatusClosed(int jobId)
        {
            try
            {
                var result = await _recruitmentRepository.setStatusClosed(jobId);

                if (result == null)
                {
                    return BadRequest(new { message = "failed to change status" });
                }
                return Ok(new { message = "status changed", attendanceId = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "status change failed", error = ex.Message });
            }
        }

        [HttpPost("applyJob")]
        public async Task<IActionResult> ApplyJob(int jobId, int userId)
        {
            try
            {
                var result = await _recruitmentRepository.ApplyJob(jobId, userId);

                if (result)
                {
                    return Ok(new{success = true,message = "Applied for job successfully"});
                }

                return BadRequest(new{success = false,message = "Unable to apply for job"});
            }
            catch (Exception ex)
            {
                return BadRequest(new{success = false,message = ex.Message});
            }
        }

        [HttpGet("getCandidatesByJobId")]
        public async Task<IActionResult> GetCandidatesByJobId(int jobId)
        {
            try
            {
                var data = await _recruitmentRepository.GetCandidatesByJobId(jobId);

                return Ok(new
                {
                    success = true,
                    message = "Candidates fetched successfully",
                    data
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

    }
}
