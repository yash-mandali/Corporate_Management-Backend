using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.Repositories;
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
        public async Task<IActionResult> CreateSalaryStructure(createSalaryStructure model)
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

        [HttpDelete("deleteSalaryStructure")]
        public async Task<IActionResult> deleteSalaryStructure(int SalaryStructureId)
        {
            try
            {
                var userId = await _payrollRepository.DeleteSalaryStructure(SalaryStructureId);
                return Ok(new { message = "SalaryStructure deleted Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "SalaryStructure delete error", error = ex.Message });
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

        [HttpGet("getPayrollbyUserId")]
        public async Task<IActionResult> getPayrollbyUserId(int userId)
        {
            try
            {
                var data = await _payrollRepository.getPayrollbyUserId(userId);

                if (data == null)
                {
                    return NotFound(new { success = false, message = "Payroll not found" });
                }

                return Ok(new
                {
                    message = "Payroll found",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "Payroll not found",
                    error = ex.Message
                });
            }
        }

        [HttpGet("getPayrollbyPayrollId")]
        public async Task<IActionResult> getPayrollbyPayrollId(int PayrollId)
        {
            try
            {
                var data = await _payrollRepository.getPayrollbyPayrollId(PayrollId);

                if (data == null)
                {
                    return NotFound(new { success = false, message = "Payroll not found" });
                }

                return Ok(new
                {
                    message = "Payroll found",
                    data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "Payroll not found",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("deletePayroll")]
        public async Task<IActionResult> deletePayroll(int PayrollId)
        {
            try
            {
                var data = await _payrollRepository.deletePayroll(PayrollId);
                return Ok(new { message = "Payroll deleted Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Payroll delete error", error = ex.Message });
            }
        }

        [HttpPost("generate-All-Payroll")]
        public async Task<IActionResult> generateAllPayrollForUser(int month,int year)
        {
            try 
            {
                var data = await _payrollRepository.GeneratePayrollForAll(month, year);
                if (data == null)
                {
                    return NotFound(new { success = false, message = "Payroll not generated" });
                }
                return Ok(new { message = "generated" });
            } 
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error" , error=ex.Message});
            }
        }

        [HttpGet("getAllPayrollByMonth")]
        public async Task<IActionResult> getAllPayrollByMonth(int month)
        {
            try
            {
                var data = await _payrollRepository.GetAllPayrollByMonth(month);
                if (data == null)
                {
                    return NotFound(new { success = false, message = "Payroll get error" });
                }
                return Ok(new { message = "payroll data", data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error", error = ex.Message });
            }
        }
    }
}
