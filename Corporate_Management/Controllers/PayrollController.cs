using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _payrollRepository;

        public PayrollController(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        [HttpPost("createSalaryStructure")]
        public async Task<IActionResult> CreateSalaryStructure(SalaryStructure model)
        {
            try
            {
                var newId = await _payrollRepository.CreateSalaryStructure(model);

                return Ok(new
                {
                    success = true,
                    message = "Salary structure created successfully",
                    salaryStructureId = newId
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

        [HttpPut("updateSalaryStructure")]
        public async Task<IActionResult> updateSalaryStructure(updateSalaryStructure model)
        {
            try
            {
                var result = await _payrollRepository.UpdateSalaryStructure(model);
                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "SalaryStructure cannot be updated."
                    });
                }

                return Ok(new { message = "SalaryStructure updated successfully" });
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

        [HttpGet("GetSalaryStructureByUserId")]
        public async Task<IActionResult> GetSalaryStructureByUserId(int userId)
        {
            try
            {
                var data = await _payrollRepository.GetSalaryStructureByUserId(userId);

                if (data == null)
                {
                    return NotFound(new {success = false, message = "Salary structure not found"});
                }

                return Ok(new
                {
                    message = "SalaryStructure found",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "Salary structure not found",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetAllSalaryStructure")]
        public async Task<IActionResult> GetAllSalaryStructure()
        {
            try
            {
                var data = await _payrollRepository.GetAllSalaryStructure();

                return Ok(new{ success = true, message = "SalaryStructure found", data });
            }
            catch (Exception ex)
            {
                return BadRequest(new{success = false,message = ex.Message});
            }
        }

        [HttpPost("generatePayroll")]
        public async Task<IActionResult> GeneratePayroll(GeneratePayroll model)
        {
            try
            {
                var id = await _payrollRepository.GeneratePayroll(model);

                return Ok(new
                {
                    success = true,
                    message = "Payroll generated successfully",
                    payrollId = id
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
