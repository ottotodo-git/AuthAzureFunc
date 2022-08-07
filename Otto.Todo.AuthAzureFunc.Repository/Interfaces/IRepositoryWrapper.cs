using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAuthRepository Auth { get; }
    }
}
