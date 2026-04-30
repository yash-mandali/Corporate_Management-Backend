using ClosedXML.Excel;
using Corporate_Management.DTOs;
using Corporate_Management.Models;
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
                return Ok(new { message = "CheckedIn", attendanceId = checkin });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Failed to checkIn", error = ex.Message });
            }
        }   

        [HttpPut("CheckOut")]
        public async Task<IActionResult> CheckOut(int AId)
        {
            try
            {
                var checkout = await _attendanceRepository.userCheckOut(AId);

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

        [HttpPut("AutoCheckout")]
        public async Task<IActionResult> AutoCheckout()
        {
            try
            {
                var result = await _attendanceRepository.AutoCheckout();

                return Ok(new { message = "Auto checkout completed", rowsUpdated = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Auto checkout failed", error = ex.Message });
            }
        }

        [HttpGet("getByUserId")]
        public async Task<IActionResult> getAttendanceByUserId(int Id)
        {
            try
            {
                var data = await _attendanceRepository.getByUserId(Id);

                if (data == null)
                {
                    return BadRequest(new { message = "failed to load data" });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch data", error = ex.Message });
            }
        }

        [HttpGet("getByAttendanceId")]
        public async Task<IActionResult> getAttendanceByAttendanceId(int Id)
        {
            try
            {
                var data = await _attendanceRepository.getByAttendanceId(Id);

                if (data == null)
                {
                    return BadRequest(new { message = "failed to load data" });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch data", error = ex.Message });
            }
        }

        [HttpGet("GetAllAttendance")]
        public async Task<IActionResult> getAllAttendance()
        {
            try
            {
                var Attendance = await _attendanceRepository.GetAllAttendanceAsync();

                if (Attendance == null)
                    return NotFound(new { message = "Attendance not found" });

                return Ok(Attendance);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch Attendance", error = ex.Message });
            }
        }

        [HttpGet("GetTeamAllAttendance")]
        public async Task<IActionResult> getTeamAllAttendance(int managerId)
        {
            try
            {
                var Attendance = await _attendanceRepository.GetTeamAllAttendance(managerId);

                if (Attendance == null)
                    return NotFound(new { message = "Attendance not found" });

                return Ok(Attendance);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch Attendance", error = ex.Message });
            }
        }

        [HttpGet("ExportAttendanceReport")]
        public async Task<IActionResult> ExportAttendanceReport([FromQuery] AttendanceReportParameters parameters)
        {
            try
            {
                var data = await _attendanceRepository.GetAttendanceReport(parameters);

                if (data == null || !data.Any())
                    return NotFound(new { message = "No records found for report." });

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Attendance Report");

                // Header
                worksheet.Cell(1, 1).Value = "Employee Name";
                worksheet.Cell(1, 2).Value = "Department";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Check In";
                worksheet.Cell(1, 5).Value = "Check Out";
                worksheet.Cell(1, 6).Value = "Working Hours";
                worksheet.Cell(1, 7).Value = "Status";

                var headerRange = worksheet.Range("A1:G1");

                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.FontColor = XLColor.RichBlack;
                headerRange.Style.Fill.BackgroundColor = XLColor.AshGrey;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                int row = 2;

                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.EmployeeName;
                    worksheet.Cell(row, 2).Value = item.Department;
                    worksheet.Cell(row, 3).Value = item.Date.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 4).Value = item.CheckIn;
                    worksheet.Cell(row, 5).Value = item.CheckOut;
                    worksheet.Cell(row, 6).Value = item.WorkingHours;
                    worksheet.Cell(row, 7).Value = item.Status;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Attendance_Report_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to generate report", error = ex.Message });
            }
        }
    }

}
