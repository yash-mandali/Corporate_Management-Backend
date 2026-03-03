using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<int> UpdateUserAsync(RegisterDto userDto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", userDto.Id);
                parameters.Add("@UserName", userDto.UserName);
                parameters.Add("@Email", userDto.Email);
                parameters.Add("@PhoneNumber", userDto.PhoneNumber);
                parameters.Add("@Password", BCrypt.Net.BCrypt.HashPassword(userDto.Password));
                parameters.Add("@Gender", userDto.Gender);
                parameters.Add("@Address", userDto.Address);
                parameters.Add("@RoleId", userDto.RoleId);
                

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

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var updatedrows = await connection.QueryFirstOrDefaultAsync<User>("sp_GetUserById", parameters, commandType: CommandType.StoredProcedure);
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
    }
}
