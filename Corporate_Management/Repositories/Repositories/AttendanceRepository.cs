using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace Corporate_Management.Repositories.Repositories
{
    public class AttendanceRepository: IAttendanceRepository
    {
        private readonly string _connectionString;
        public AttendanceRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");


        }
        public async Task<int> userCheckIn(int Id)
        {
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId",Id);
                var checkindata =await connection.ExecuteScalarAsync<int>("sp_checkIn", parameters, commandType: CommandType.StoredProcedure);
                return checkindata;   
            }
            catch (SqlException ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> userCheckOut(int AId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();          
                parameters.Add("@AttendenceId",AId);
                var checkindata = await connection.ExecuteAsync("sp_checkOut", parameters, commandType: CommandType.StoredProcedure);
                return checkindata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> AutoCheckout()
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.ExecuteAsync(
                "sp_AutoCheckout",
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<IEnumerable<AttendanceDto>> getByUserId(int Id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                var userdata = await connection.QueryAsync<AttendanceDto>("sp_getAttendenceByUserId", parameters, commandType: CommandType.StoredProcedure);
                return userdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> getByAttendanceId(int Id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                var attendancedata = await connection.QueryAsync<AttendanceDto>("sp_getByAttendenceId", parameters, commandType: CommandType.StoredProcedure);
                return attendancedata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAttendanceAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var data = await connection.QueryAsync<AttendanceDto>("sp_getAllAttendance", commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> GetTeamAllAttendance(int managerId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@ManagerId", managerId);

            var result = await connection.QueryAsync<AttendanceDto>(
                "sp_getTeamAllAttendance",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

    }
}
