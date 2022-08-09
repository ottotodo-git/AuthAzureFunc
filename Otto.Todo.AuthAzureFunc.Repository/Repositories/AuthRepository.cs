using Dapper;
using Otto.Todo.AuthAzureFunc.Models.Models;
using Otto.Todo.AuthAzureFunc.Repository.Context;
using Otto.Todo.AuthAzureFunc.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _dpContext;
        public AuthRepository(DapperContext dpContext)
        {
            _dpContext = dpContext;
        }
        public async Task<AuthRequest> addUserAsync(AuthRequest auth)
        {
            var query = "INSERT INTO authuser (appid,name,phonenumber,email,verifycode,verificationstatus) " +
               "VALUES (@AppId,@Name,@PhoneNumber,@Email,@VerifyCode,@VerificationStatus) returning userid;";
            auth.AppId = Guid.NewGuid().ToString();
            var parameters = new DynamicParameters();
            parameters.Add("AppId", auth.AppId, DbType.String);
            parameters.Add("Name", auth.Name, DbType.String);
            parameters.Add("PhoneNumber", auth.PhoneNumber, DbType.String);
            parameters.Add("Email", auth.Email, DbType.String);
            parameters.Add("VerifyCode", auth.VerifyCode, DbType.String);
            parameters.Add("VerificationStatus", auth.VerificationStatus, DbType.String);

            using (var connection = _dpContext.CreateConnection())
            {
                //var id = connection.QuerySingleAsync<int>(query, parameters);
                var row = await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);
                auth.UserId = row.userid;
                return auth;
            }
        }

        public async Task<AuthRequest> getUserAsync(long userId)
        {
            var query = "select appid,phonenumber,email,verifycode,verificationstatus from authuser where userid = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int64);
            using (var connection = _dpContext.CreateConnection())
            {
                dynamic authuser = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters);

                if (authuser == null)
                    return null;

                AuthRequest authUser = new AuthRequest()
                {
                    UserId = userId,
                    AppId = authuser.AppId,
                    PhoneNumber = authuser.PhoneNumber,
                    Email = authuser.Email,
                    VerifyCode = authuser.VerifyCode,
                    VerificationStatus = authuser.VerificationStatus
                };
                return authUser;

            }
        }

        public async Task<AuthRequest> updateUserAsync(AuthRequest auth)
        {
            var query = "update authuser set VerificationStatus=@VerificationStatus where userid = @UserId and appid = @AppId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", auth.UserId, DbType.Int64);
            parameters.Add("AppId", auth.AppId, DbType.String);
            parameters.Add("VerificationStatus", auth.VerificationStatus, DbType.String);

            using (var connection = _dpContext.CreateConnection())
            {
                //var id = connection.QuerySingleAsync<int>(query, parameters);
                await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);
                return auth;
            }
        }
    }
}
