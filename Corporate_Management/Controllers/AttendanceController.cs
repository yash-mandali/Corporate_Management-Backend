using Corporate_Management.DTOs;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(int Id)
        {
            try 
            {
                var checkin = await _attendanceRepository.userCheckIn(Id);

                if (checkin == null)
                {
                    return BadRequest(new { message = "failed to checkin" });
                }
                return Ok(new { message = "CheckedIn" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Failed to checkIn", error = ex.Message });
            }
        }

        [HttpPut("CheckOut")]
        public async Task<IActionResult> CheckOut(int Id)
        {
            try
            {
                var checkout = await _attendanceRepository.userCheckOut(Id);

                if (checkout == null)
                {
                    return BadRequest(new { message = "failed to checkOut" });
                }
                return Ok(new { message = "CheckOut" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to checkOut", error = ex.Message });
            }
        }
    }

}
