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
            var query = "INSERT INTO authuser (externaluserid,appid,name,verificationcode,verificationstatus) " +
               "VALUES (@ExternalUserId,@AppId,@Name,@VerificationCode,@VerificationStatus) returning userid;";
            auth.AppId = Guid.NewGuid().ToString();
            var parameters = new DynamicParameters();
            parameters.Add("ExternalUserId", auth.ExternalUserId, DbType.String);
            parameters.Add("AppId", auth.AppId, DbType.String);
            parameters.Add("Name", auth.Name, DbType.String);
            parameters.Add("VerificationCode", auth.VerificationCode, DbType.Int32);
            parameters.Add("VerificationStatus", auth.VerificationStatus, DbType.String);

            using (var connection = _dpContext.CreateConnection())
            {
                //var id = connection.QuerySingleAsync<int>(query, parameters);
                var row = await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);
                auth.UserId = row.userid;
                return auth;
            }
        }

        public async Task<AuthRequest> getAuthUserAsync(long userId)
        {
            var query = "select externaluserid,appid,verificationcode,verificationstatus from authuser where userid = @UserId";
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
                    ExternalUserId = authuser.ExternalUserId,
                    AppId = authuser.appid,
                    VerificationCode = authuser.verificationcode,
                    VerificationStatus = authuser.verificationstatus
                };
                return authUser;

            }
        }

        public async Task<AuthUser> getUserAsync(long userId)
        {
            var query = "select appid,name from authuser where userid = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int64);
            using (var connection = _dpContext.CreateConnection())
            {
                dynamic user = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters);

                if (user == null)
                    return null;

                AuthUser dbuser = new AuthUser()
                {
                    UserId = userId,
                    AppId = user.appid,
                    Name = user.name
                };
                return dbuser;

            }
        }

        public async Task<IEnumerable<AuthUser>> getUsersAsync()
        {
            var query = "select userid,appid,name from authuser";
            var parameters = new DynamicParameters();
            using (var connection = _dpContext.CreateConnection())
            {
                IEnumerable<dynamic> userlist = await connection.QueryAsync<dynamic>(query);

                if (userlist == null)
                    return null;

                List<AuthUser> getUsers = new List<AuthUser>();
                foreach (var user in userlist)
                {
                    AuthUser getuser = new AuthUser
                    {
                        UserId = user.userid,
                        AppId = user.appid,
                        Name = user.name
                    };
                    getUsers.Add(getuser);
                }


                return getUsers;
            }
        }

        public async Task<AuthRequest> getUserByExternalIdAsync(string externaluserId)
        {
            var query = "select userid,appid,phonenumber,email,verificationcode,verificationstatus from authuser where externaluserId = @ExternalUserId";
            var parameters = new DynamicParameters();
            parameters.Add("ExternalUserId", externaluserId, DbType.String);
            using (var connection = _dpContext.CreateConnection())
            {
                dynamic authuser = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters);

                if (authuser == null)
                    return null;

                AuthRequest authUser = new AuthRequest()
                {
                    UserId = authuser.userid,
                    ExternalUserId = externaluserId,
                    AppId = authuser.appid,
                    VerificationCode = authuser.verificationcode,
                    VerificationStatus = authuser.verificationstatus
                };
                return authUser;

            }
        }

        public async Task<AuthRequest> updateUserAsync(AuthRequest auth)
        {
            var query = "update authuser set VerificationStatus=@VerificationStatus,VerificationCode=@VerificationCode where userid = @UserId and appid = @AppId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", auth.UserId, DbType.Int64);
            parameters.Add("AppId", auth.AppId, DbType.String);
            parameters.Add("VerificationCode", auth.VerificationCode, DbType.Int32);
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
