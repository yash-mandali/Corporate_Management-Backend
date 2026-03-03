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


        [HttpPost("createLeave")]
        public async Task<IActionResult> createLeave(Leave leave)
        {
            try
            {
                var userId = await _leaveRepositories.CreateLeave(leave);

                return Ok(new { message = "Leave created Succesfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Leave Error", error = ex.Message });
            }
        }
    }
}
