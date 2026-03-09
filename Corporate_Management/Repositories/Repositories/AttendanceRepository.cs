using Corporate_Management.DTOs;
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
                var checkindata =await connection.ExecuteAsync("sp_checkIn", parameters, commandType: CommandType.StoredProcedure);
                return checkindata;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<int> userCheckOut(int Id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();          
                parameters.Add("@AttendenceId",Id);
                var checkindata = await connection.ExecuteAsync("sp_checkOut", parameters, commandType: CommandType.StoredProcedure);
                return checkindata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
