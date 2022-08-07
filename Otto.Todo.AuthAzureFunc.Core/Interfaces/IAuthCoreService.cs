using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Core.Interfaces
{
    public interface IAuthCoreService
    {
        public Task<AuthRequestDTO> registerUserAsync(AuthRequestDTO auth);
    }
}
