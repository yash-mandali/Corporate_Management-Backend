using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
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
        public async Task<IActionResult> UpdateUser(int id, RegisterDto userDto)
        {
            try
            {
                var userId = await _userRepositories.UpdateUserAsync(id, userDto);
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

                return Ok(new { message = "Login successful", token = Token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving user", error = ex.Message });
            }
        }
    }
}
