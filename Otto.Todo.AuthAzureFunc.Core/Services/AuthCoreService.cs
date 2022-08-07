using AutoMapper;
using Otto.Todo.AuthAzureFunc.Core.Interfaces;
using Otto.Todo.AuthAzureFunc.Core.Utilities;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using Otto.Todo.AuthAzureFunc.Models.Models;
using Otto.Todo.AuthAzureFunc.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Core.Services
{
    public class AuthCoreService : IAuthCoreService
    {
        IRepositoryWrapper _repoWrapper;
        IMapper _mapper;
        public AuthCoreService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<AuthRequestDTO> registerUserAsync(AuthRequestDTO auth)
        {
            var data = await _repoWrapper.Auth.addUserAsync(_mapper.Map<AuthRequest>(auth));
            //generate 6 digit SMS code and send SMS
            var message = SMSProviderUtility.sendSMSToCustomer(auth.PhoneNumber);
            return _mapper.Map<AuthRequestDTO>(data);
            //throw new NotImplementedException();
        }
    }
}
