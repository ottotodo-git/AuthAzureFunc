using AutoMapper;
using Microsoft.Graph;
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
            auth.VerificationStatus = "NOT VERIFIED";
            //generate 6 digit SMS code and send SMS
            SMSProviderUtils.sendSMSToCustomer(auth);
            //fetch user by phone from AD
            var user = await AzureADB2CUtils.getUserByPhoneAsync(auth.PhoneNumber);
            if(user.Count == 1)
            {
                User authUser = user.First();
                auth.PhoneNumber = auth.PhoneNumber.Replace("+", "").Trim();
                //check if phone matches
                if (auth.PhoneNumber == authUser.MobilePhone)
                {
                    //phone already present, so enable 2FA code from user
                    //fetch user using external Id
                    var getUser = await _repoWrapper.Auth.getUserByExternalIdAsync(authUser.Id);
                    var authrequest = _mapper.Map<AuthRequest>(auth);
                    getUser.VerificationCode = authrequest.VerificationCode;
                    //update user with new verification code
                    var update_data = await _repoWrapper.Auth.updateUserAsync(getUser);
                    return _mapper.Map<AuthRequestDTO>(update_data);
                }
            }
            await AzureADB2CUtils.createUserAsync(auth);
            var data = await _repoWrapper.Auth.addUserAsync(_mapper.Map<AuthRequest>(auth));
            return _mapper.Map<AuthRequestDTO>(data);
        }

        public async Task<AuthRequestDTO> verifyUserAsync(AuthRequestDTO auth)
        {
            var authUser = await _repoWrapper.Auth.getUserAsync(long.Parse(auth.UserId));
            if (authUser.VerificationCode == long.Parse(auth.VerificationCode))
            {
                auth.VerificationStatus = "VERIFIED";
                //generate ID token
                auth.Token = new Token();
                auth.Token = JWTUtils.GenerateTokenAsymetric(auth);
            }
            await _repoWrapper.Auth.updateUserAsync(_mapper.Map<AuthRequest>(auth));
            return auth;
        }

        public async Task<long?> validateTokenAsync(AuthRequestDTO auth)
        {
            long? userId = JWTUtils.ValidateTokenAsymetric(auth.Token.IdToken);
            return userId;
        }
    }
}
