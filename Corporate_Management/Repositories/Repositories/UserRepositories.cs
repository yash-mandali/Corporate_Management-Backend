using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.IRepositories.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly string _connectionString;
        public UserRepositories(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");
        }
       
        public async Task<int> AddUserAsync(RegisterDto userDto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userDto.UserName);
                parameters.Add("@Email", userDto.Email);
                parameters.Add("@PhoneNumber", userDto.PhoneNumber);
                parameters.Add("@Password", BCrypt.Net.BCrypt.HashPassword(userDto.Password));
                parameters.Add("@Department", userDto.Department);
                parameters.Add("@Gender", userDto.Gender);
                parameters.Add("@Address", userDto.Address);
                parameters.Add("@RoleId", userDto.RoleId);
                parameters.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddUser", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@newId");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   
        public async Task<int> UpdateUserAsync(UpdateUserDto userDto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", userDto.Id);
                parameters.Add("@UserName", userDto.UserName);
                parameters.Add("@Email", userDto.Email);
                parameters.Add("@PhoneNumber", userDto.PhoneNumber);
                parameters.Add("@Department", userDto.Department);
                parameters.Add("@Gender", userDto.Gender);
                parameters.Add("@Address", userDto.Address);
                            

                var updatedrows = await connection.ExecuteAsync("sp_UpdateUser", parameters, commandType: CommandType.StoredProcedure);
                return updatedrows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> DeleteUserAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var updatedrows = await connection.ExecuteAsync("sp_DeleteUser", parameters, commandType: CommandType.StoredProcedure);
                return updatedrows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UserListDto> GetUserByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var updatedrows = await connection.QueryFirstOrDefaultAsync<UserListDto>("sp_GetUserById", parameters, commandType: CommandType.StoredProcedure);
                return updatedrows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<UserListDto>> GetAllUserAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var Users = await connection.QueryAsync<UserListDto>("sp_GetAllUsers", commandType: CommandType.StoredProcedure);
                return Users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<UserListDto>> GetAllEmployeeManagers()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var Users = await connection.QueryAsync<UserListDto>("sp_GetAllEmployeeManager", commandType: CommandType.StoredProcedure);
                return Users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<UserListDto>> GetAllEmployeeAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var Users = await connection.QueryAsync<UserListDto>("sp_GetAllEmployee", commandType: CommandType.StoredProcedure);
                return Users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<UserListDto>> GetAllManagerAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var Users = await connection.QueryAsync<UserListDto>("sp_getAllManagers", commandType: CommandType.StoredProcedure);
                return Users;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<User> LoginUser(LoginDto logindto)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Email", logindto.Email);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "sp_LoginUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (user == null)
                return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(logindto.Password, user.Password);

            if (!isPasswordValid)
                return null;

            return user;
        }
        public async Task LogoutUser(int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            await connection.ExecuteAsync(
                "sp_LogoutUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<UserListDto>> GetManagerTeam(int managerId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@ManagerId", managerId);

                var updatedrows = await connection.QueryAsync<UserListDto>("sp_getManagerTeam", parameters, commandType: CommandType.StoredProcedure);
                return updatedrows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<UserListDto>> GetEmployeeByDepartment(string department)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Department", department);

                var updatedrows = await connection.QueryAsync<UserListDto>("sp_GetEmployeeByDepartment", parameters, commandType: CommandType.StoredProcedure);
                return updatedrows;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> AssignManagerAsync(int userId, int managerId)
        {
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ManagerId", managerId);

                var result = await connection.ExecuteAsync(
                    "sp_AssignManager",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //-------------------------------notification section-----------------------------------------

        public async Task CreateNotification(string title, string message, string type, List<int> userIds)
        {
            using var connection = new SqlConnection(_connectionString);
            // Step 1: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new { Title = title, Message = message, Type = type },
                commandType: CommandType.StoredProcedure
            );

            // Step 2: Map Users
            foreach (var userId in userIds)
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new { NotificationId = notificationId, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<List<int>> GetUsersByRoles(List<int> roleIds)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var users = await connection.QueryAsync<int>(
                    "SELECT Id FROM Users WHERE RoleId IN @RoleIds AND IsDeleted = 0",
                    new { RoleIds = roleIds }
                );

                return users.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<dynamic>> GetUserNotifications(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var data = await connection.QueryAsync(
                    "sp_GetUserNotifications",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task MarkAsRead(int notificationId, int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(
                    "sp_MarkNotificationAsRead",
                    new { NotificationId = notificationId, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
