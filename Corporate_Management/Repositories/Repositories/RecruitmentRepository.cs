using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

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

        //public async Task<int> PublishJobAsync(int jobId)
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@JobId", jobId);
        //        parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        //        await connection.ExecuteAsync("sp_PublishJob",parameters,commandType: CommandType.StoredProcedure);
        //        return parameters.Get<int>("@ReturnVal");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<int> PublishJobAsync(int jobId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Publish Job
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", jobId);
            parameters.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "sp_PublishJob",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var result = parameters.Get<int>("@ReturnVal");

            // 👉 Only proceed if publish successful
            if (result != 1)
            {
                return result;
            }

            // 🔹 Step 2: Get Job Title (optional but better UX)
            var jobTitle = await connection.QuerySingleOrDefaultAsync<string>(
                "SELECT Title FROM Jobs WHERE JobId = @JobId",
                new { JobId = jobId }
            );

            // 🔹 Step 3: Get Users (Employee, Manager, Admin)
            var userIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Users WHERE RoleId IN (1,2,3) AND IsDeleted = 0"
            );

            // 🔹 Step 4: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "New Job Posted",
                    Message = $"A new job '{jobTitle}' has been published",
                    Type = "Job"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 5: Map to all users
            foreach (var userId in userIds)
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new { NotificationId = notificationId, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }

            return result;
        }
        public async Task<JobModel> getJobById(int jobId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@JobId", jobId);

                var data = await connection.QueryFirstOrDefaultAsync<JobModel>("sp_GetJobById", parameter, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateJobAsync(int jobId, UpdateJobDto dto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobId);
                parameters.Add("@Title", dto.Title);
                parameters.Add("@Description", dto.Description);
                parameters.Add("@Department", dto.Department);
                parameters.Add("@Location", dto.Location);
                parameters.Add("@Employment_type", dto.Employment_type);
                parameters.Add("@Experience_required", dto.Experience_required);
                parameters.Add("@Vacancies", dto.Vacancies);
                parameters.Add("@Required_skills", dto.Required_skills);
                parameters.Add("@Qualifications", dto.Qualifications);
                parameters.Add("@Responsibilities", dto.Responsibilities);
                parameters.Add("@Salary_min", dto.Salary_min);
                parameters.Add("@Salary_max", dto.Salary_max);
                parameters.Add("@Currency", dto.Currency);
                parameters.Add("@Application_deadline", dto.Application_deadline);

                var result = await connection.ExecuteAsync(
                    "sp_UpdateJob",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> setStatusOnHold(int jobId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobId);

                var result = await connection.ExecuteAsync(
                "sp_OnHoldJob",
                parameters,
                commandType: CommandType.StoredProcedure
            );

                return  result;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //public async Task<int> setStatusClosed(int jobId)
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@JobId", jobId);

        //        var result = await connection.ExecuteAsync(
        //        "sp_CloseJob",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //        return result;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<int> setStatusClosed(int jobId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Close Job
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", jobId);

            var result = await connection.ExecuteAsync(
                "sp_CloseJob",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // 👉 Optional: ensure operation succeeded
            if (result <= 0)
            {
                return result;
            }

            // 🔹 Step 2: Get Job Title
            var jobTitle = await connection.QuerySingleOrDefaultAsync<string>(
                "SELECT Title FROM Jobs WHERE JobId = @JobId",
                new { JobId = jobId }
            );

            // 🔹 Step 3: Get Users (Employee, Manager, Admin)
            var userIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Users WHERE RoleId IN (1,2,3) AND IsDeleted = 0"
            );

            // 🔹 Step 4: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Job Closed",
                    Message = $"The job '{jobTitle}' is now closed",
                    Type = "Job"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 5: Map to Users
            foreach (var userId in userIds)
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new { NotificationId = notificationId, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }

            return result;
        }

        public async Task<bool> ApplyJob(int jobId, int userId, string resumeurl)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobId);
                parameters.Add("@UserId", userId);
                parameters.Add("@ResumeUrl", resumeurl);

                var result = await connection.ExecuteAsync(
                    "sp_ApplyJob",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return true; 
            }
            catch (SqlException)
            {
                throw; 
            }
        }

        public async Task<IEnumerable<CandidateDto>> GetCandidatesByJobId(int jobId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobId);

                var result = await connection.QueryAsync<CandidateDto>(
                    "sp_GetCandidatesByJobId",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
