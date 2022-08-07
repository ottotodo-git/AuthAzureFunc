using Otto.Todo.AuthAzureFunc.Repository.Context;
using Otto.Todo.AuthAzureFunc.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Repository.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DapperContext _dpContext;
        private IAuthRepository _AuthRepository;

        public RepositoryWrapper(DapperContext dpContext)
        {
            _dpContext = dpContext;
        }
        public IAuthRepository Auth
        {
            get
            {
                if (_AuthRepository == null)
                {
                    _AuthRepository = new AuthRepository(_dpContext);
                }
                return _AuthRepository;
            }
        }
    }
}
