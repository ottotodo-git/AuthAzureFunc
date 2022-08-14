using Otto.Todo.AuthAzureFunc.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Repository.Interfaces
{
    public interface IAuthRepository
    {
        public Task<AuthRequest> addUserAsync(AuthRequest auth);

        public Task<AuthRequest> updateUserAsync(AuthRequest auth);

        public Task<AuthRequest> getAuthUserAsync(long userId);

        public Task<AuthUser> getUserAsync(long userId);

        public Task<IEnumerable<AuthUser>> getUsersAsync();

        public Task<AuthRequest> getUserByExternalIdAsync(string externalid);
    }
}
