using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositories _userRepositories;
        private readonly GetJwtToken _getToken;

        public UserController(IUserRepositories userRepositories,GetJwtToken getToken )
        {
            _userRepositories = userRepositories;
            _getToken = getToken;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(RegisterDto userDto)
        {

            //if (ModelState.IsValid)
            //{
            //    return BadRequest(new { message = "Invalid model" });
            //}

            try
            {
                var userId = await _userRepositories.AddUserAsync(userDto);
                return Ok(new { message = "User registered Succesfully", UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error registering user", error = ex.Message });
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
        {
            try
            {
                var userId = await _userRepositories.UpdateUserAsync(userDto);
                return Ok(new { message = "User updated Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error registering user", error = ex.Message });
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userId = await _userRepositories.DeleteUserAsync(id);
                return Ok(new { message = "User deleted Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "User delete error", error = ex.Message });
            }
        }

        [HttpGet("getUserById")]
        public async Task<IActionResult> getUserById(int id)
        {
            try
            {
                var user = await _userRepositories.GetUserByIdAsync(id);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> getAllUser()
        {
            try
            {
                var user = await _userRepositories.GetAllUserAsync();

                if (user == null)
                    return NotFound(new { message = "Users not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpGet("getAllEmployeeManagerHr")]
        public async Task<IActionResult> GetAllEmployeeManagerHr()
        {
            try
            {
                var user = await _userRepositories.GetAllEmployeeManagerHr();

                if (user == null)
                    return NotFound(new { message = "users not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpGet("getAllEmployeeManager")]
        public async Task<IActionResult> getAllEmployeeManager()
        {
            try
            {
                var user = await _userRepositories.GetAllEmployeeManagers();

                if (user == null)
                    return NotFound(new { message = "users not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpGet("getAllEmployee")]
        public async Task<IActionResult> getAllemployee()
        {
            try
            {
                var user = await _userRepositories.GetAllEmployeeAsync();

                if (user == null)
                    return NotFound(new { message = "Employees not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving Employees", error = ex.Message });
            }
        }

        [HttpGet("getAllManagers")]
        public async Task<IActionResult> getAllManagers()
        {
            try
            {
                var user = await _userRepositories.GetAllManagerAsync();

                if (user == null)
                    return NotFound(new { message = "Managers not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving Managers", error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            try
            {
                var user = await _userRepositories.LoginUser(logindto);

                if (user == null)
                    return Unauthorized(new { message = "Invalid email or password" });

                var response = new LoginResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleName = user.RoleName
                };
                var Token = _getToken.GenerateJwtToken(response);

                return Ok(new { message = "Login successful", token = Token, role = user.RoleName, userId = user.Id });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(int userId)
        {
            try 
            {
                await _userRepositories.LogoutUser(userId);

                return Ok(new
                {
                    message = "user logout"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "logout failed", error = ex.Message });
            }

        }
    
        [HttpGet("getManagerTeam")]
        public async Task<IActionResult> getManagerTeam(int managerId)
        {
            try
            {
                var user = await _userRepositories.GetManagerTeam(managerId);

                if (user == null)
                    return NotFound(new { message = "Team not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving Team", error = ex.Message });
            }
        }

        [HttpGet("GetEmployeeByDepartment")]
        public async Task<IActionResult> GetEmployeeByDepartment(string department)
        {
            try
            {
                var user = await _userRepositories.GetEmployeeByDepartment(department);

                if (user == null)
                    return NotFound(new { message = "Employee not found" });

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving Employee", error = ex.Message });
            }
        }

        [HttpPost("assign-manager")]
        public async Task<IActionResult> AssignManager(int userId, int managerId)
        {
            try
            {
                var result = await _userRepositories.AssignManagerAsync(userId, managerId);

                //if (!result) {
                //    return BadRequest("Manager assignment failed");
                //}
                    

                return Ok(new
                {
                    message = "Manager assigned successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "failed", error = ex.Message });
            }
        }

        //----------------notifications-----------------
 
        [HttpGet("getUsersNotifications")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            try
            {
                var data = await _userRepositories.GetUserNotifications(userId);

                if (data == null)
                    return NotFound(new { message = "notifications not found" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving notifications", error = ex.Message });
            }
        }                                                                                                                                                                                                                                                                                    

        [HttpPost("MarkAsReadNotifications")]
        public async Task<IActionResult> MarkAsRead(int notificationId, int userId)
        {
            try
            {
                await _userRepositories.MarkAsRead(notificationId, userId);
                return Ok(new {message="notification mark as read"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving notifications", error = ex.Message });
            }
        }

        [HttpPut("MarkAllasRead")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            try
            {
                var result = await _userRepositories.MarkAllAsRead(userId);

                return Ok(new
                {
                    success = true,
                    message = "All notifications marked as read."
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

        //----------------forgot password section-----------------

        [HttpPost("SendForgotPasswordOtp")]
        public async Task<IActionResult> SendForgotPasswordOtp(string email)
        {
            try
            {
                var result = await _userRepositories.VerifyEmailAndSendOtp(email);

                if (!result)
                {
                    return BadRequest(new{success = false,message = "Email not found or OTP send failed."});
                }

                return Ok(new{success = true,message = "OTP sent successfully."});
            }
            catch (Exception ex)
            {
                return BadRequest(new{success = false,message = ex.Message});
            }
        }

        [HttpPost("VerifyForgotPasswordOtp")]
        public async Task<IActionResult> VerifyForgotPasswordOtp(string email,string otp)
        {
            try
            {
                var result = await _userRepositories.VerifyOtp(email, otp);

                if (!result)
                {
                    return BadRequest(new{success = false,message = "Invalid or expired OTP."});
                }

                return Ok(new{success = true,message = "OTP verified successfully."});
            }
            catch (Exception ex)
            {
                return BadRequest(new{success = false,message = ex.Message});
            }
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ResetForgotPassword(ChangePasswordDto model)
        {
            try
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    return BadRequest(new{success = false,message = "New Password and Confirm Password does not match."});
                }

                var result = await _userRepositories.changePassword(model.Email,model.NewPassword);

                if (!result)
                {
                    return BadRequest(new{success = false,message = "Password reset failed."});
                }

                return Ok(new{success = true,message = "Password changed successfully."});
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
