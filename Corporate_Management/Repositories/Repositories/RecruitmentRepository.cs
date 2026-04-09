using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.Repositories
{
    public class RecruitmentRepository:IRecruitmentRepository
    {
        public readonly string _connectionString;
        public RecruitmentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");
        }

        public async Task<int> createJob (CreateJobsDto model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Title", model.Title);
                parameters.Add("@Description", model.Description);
                parameters.Add("@Department", model.Department);
                parameters.Add("@Location", model.Location);
                parameters.Add("@Employment_type", model.Employment_type);
                parameters.Add("@Experience_required", model.Experience_required);
                parameters.Add("@Vacancies", model.Vacancies);
                parameters.Add("@Required_skills", model.Required_skills);
                parameters.Add("@Qualifications", model.Qualifications);
                parameters.Add("@Responsibilities", model.Responsibilities);
                parameters.Add("@Salary_min", model.Salary_min);
                parameters.Add("@Salary_max", model.Salary_max);
                parameters.Add("@Currency", model.Currency);
                parameters.Add("@Publish_date", model.Publish_date);
                parameters.Add("@Application_deadline", model.Application_deadline);
                parameters.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("sp_createJobs", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return parameters.Get<int>("@newId");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> deleteJob(int jobId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobId);
                var result = await connection.ExecuteAsync("sp_DeleteJob", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<JobModel>> GetAllJobs()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var result = await connection.QueryAsync<JobModel>(
                    "sp_getAllJobs",
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
