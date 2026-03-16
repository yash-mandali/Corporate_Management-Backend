using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.IRepositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepositories;


        public LeaveController(ILeaveRepository leaveRepositories)
        {
            _leaveRepositories = leaveRepositories;
        }


        [HttpPost("ApplyLeave")]
        public async Task<IActionResult> ApplyLeave(Leave leave)
        {
            try
            {

                var userId = await _leaveRepositories.CreateLeave(leave);

                return Ok(new { message = "Leave Applied Succesfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Leave Error", error = ex.Message });
            }
        }

        [HttpPut("updateLeave/{id}")]
        public async Task<IActionResult> UpdateLeave(int id, updateLeaveDto leavemodel)
        {
            try
            {
                var result = await _leaveRepositories.UpdateLeave(id, leavemodel);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be updated. It may already be approved, rejected, or withdrawn."
                    });
                }

                return Ok(new { message = "Leave updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to update leaves", error = ex.Message });
            }
        }

        [HttpGet("getMyLeaves")]
        public async Task<IActionResult> getMyLeaves(int id)
        {
            try
            {
                var leave = await _leaveRepositories.GetLeaveByIUserId(id);

                if (leave == null || !leave.Any())
                    return NotFound(new { message = "Leave not found" });

                return Ok(leave);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving leave", error = ex.Message });
            }
        }

        [HttpGet("getLeaveById")]
        public async Task<IActionResult> getLeaveById(int id)
        {
            try
            {
                var leave = await _leaveRepositories.GetLeaveByLeaveId(id);

                if (leave == null || !leave.Any())
                    return NotFound(new { message = "Leave not found" });

                return Ok(leave);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving leave", error = ex.Message });
            }
        }

        [HttpGet("GetAllLeaves")]
        public async Task<IActionResult> getAllLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllLeaveAsync();

                if (leaves == null)
                    return NotFound(new { message = "Leaves not found" });

                return Ok(leaves);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch leaves", error = ex.Message });
            }
        }

        [HttpPut("withdrawLeave/{leaveRequestId}")]
        public async Task<IActionResult> WithdrawLeave(int leaveRequestId)
        {
            try 
            {
                var result = await _leaveRepositories.WithdrawLeave(leaveRequestId);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be withdrawn. It may already be approved or rejected."
                    });
                }

                return Ok(new { message = "Leave withdrawn successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to withdrawn laeves", error = ex.Message });
            }
        }

        [HttpGet("GetAllPendingLeaves")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllPendingLeaves();

                return Ok(new
                {
                    message = "Pending leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get Pending leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllApprovedLeaves")]
        public async Task<IActionResult> GetApprovedLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllApprovedLeaves();

                return Ok(new
                {
                    message = "approved leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get approved leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllRejectedLeaves")]
        public async Task<IActionResult> GetAllRejectedLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllRejectedLeaves();

                return Ok(new
                {
                    message = "rejected leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get rejected leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllWithdrawnLeaves")]
        public async Task<IActionResult> GetAllWithdrawnLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllWithdrawnLeaves();

                return Ok(new
                {
                    message = "withdrawn leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get withdrawn leaves", error = ex.Message });
            }
        }

        [HttpPut("Approve-Leave")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            try 
            {
                var result = await _leaveRepositories.ApproveLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be approved."
                    });
                }

                return Ok(new { message = "Leave approved" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Approve leave", error = ex.Message });
            }
        }

        [HttpPut("Reject-Leave")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            try
            {
                var result = await _leaveRepositories.RejectLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be Rejected."
                    });
                }

                return Ok(new { message = "Leave Rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Reject leave", error = ex.Message });
            }
        }
    }
}
