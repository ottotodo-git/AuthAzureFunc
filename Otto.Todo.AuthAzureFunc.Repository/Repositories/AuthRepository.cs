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
            var query = "INSERT INTO authuser (appid,name,phonenumber,email) " +
               "VALUES (@AppId,@Name,@PhoneNumber,@Email) returning userid;";
            auth.AppId = Guid.NewGuid().ToString();
            var parameters = new DynamicParameters();
            parameters.Add("AppId", auth.AppId, DbType.String);
            parameters.Add("Name", auth.Name, DbType.String);
            parameters.Add("PhoneNumber", auth.PhoneNumber, DbType.String);
            parameters.Add("Email", auth.Email, DbType.String);

            using (var connection = _dpContext.CreateConnection())
            {
                //var id = connection.QuerySingleAsync<int>(query, parameters);
                var row = await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);
                auth.UserId = row.userid;
                return auth;
            }
        }

        public Task<AuthRequest> getUserAsync(string appId)
        {
            throw new NotImplementedException();
        }

        public Task<AuthRequest> updateUserAsync(AuthRequest auth)
        {
            throw new NotImplementedException();
        }
    }
}
