using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Otto.Todo.AuthAzureFunc.Core.Services;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using Otto.Todo.AuthAzureFunc.Models.Models;
using Otto.Todo.AuthAzureFunc.Repository.Interfaces;
using Otto.Todo.AuthAzureFunc.API;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Web.Http;

namespace Otto.Todo.AuthAzureFunc.Test
{
    public class AuthValidationTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepoWrapper;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<ILogger> _mockLogger;
        private AuthCoreService _AuthService;

        public AuthValidationTests()
        {
            _mockRepoWrapper = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public async void RegisterUserFuncAsync()
        {
            AuthUser user = new AuthUser { UserId = 1, AppId = "100", Name = "Smit" };
            AuthRequest authreq = new AuthRequest { User = user, ExternalUserId = "100", VerificationCode = 101101, VerificationStatus = "Done" };
            AuthUserDTO userdto = new AuthUserDTO { UserId = "1", AppId = "100", Name = "Smit" };
            AuthRequestDTO authreqdto = new AuthRequestDTO { User = userdto, ExternalUserId = "100", VerificationCode = "101101", VerificationStatus = "Done" };

            _mockMapper.Setup(p => p.Map<AuthRequest>(authreqdto)).Returns(authreq);
            _mockRepoWrapper.Setup(p => p.Auth.addUserAsync(authreq)).ReturnsAsync(authreq);
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new RegisterUserFunc(_AuthService);
            JsonContent content = JsonContent.Create(authreqdto);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = content.ReadAsStream();
            var result = await createFunc.Run(request, _mockLogger.Object);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async void RegisterUserFuncWithNullFieldsAsync()
        {
            AuthUser user = new AuthUser { UserId = 1, AppId = "100", Name = "Smit" };
            AuthRequest authreq = new AuthRequest { User = null, ExternalUserId = "100", VerificationCode = 101101, VerificationStatus = null };
            AuthUserDTO userdto = new AuthUserDTO { UserId = "1", AppId = "100", Name = "Smit" };
            AuthRequestDTO authreqdto = new AuthRequestDTO { User = null, ExternalUserId = "100", VerificationCode = "101101", VerificationStatus = null };

            _mockMapper.Setup(p => p.Map<AuthRequest>(authreqdto)).Returns(authreq);
            _mockRepoWrapper.Setup(p => p.Auth.addUserAsync(authreq)).ReturnsAsync(authreq);
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new RegisterUserFunc(_AuthService);
            JsonContent content = JsonContent.Create(authreqdto);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = content.ReadAsStream();
            var result = await createFunc.Run(request, _mockLogger.Object);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async void ValidateTokenAsync()
        {
            AuthUser user = new AuthUser { UserId = 1, AppId = "100", Name = "Smit" };
            AuthRequest authreq = new AuthRequest { User = user, ExternalUserId = "100", VerificationCode = 101101, VerificationStatus = "Done" };
            AuthUserDTO userdto = new AuthUserDTO { UserId = "1", AppId = "100", Name = "Smit" };
            Token token = new Token { IdToken = "101101", TokenType = "Int", Expiry = 10000};
            AuthRequestDTO authreqdto = new AuthRequestDTO { User = userdto, ExternalUserId = "100", VerificationCode = "101101", VerificationStatus = "Done", Token = token };

            //_mockMapper.Setup(p => p.Map<AuthRequest>(authreqdto)).Returns(authreq);
            //_mockRepoWrapper.Setup(p => p.Auth.addUserAsync(authreq)).ReturnsAsync(authreq);
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new ValidateTokenFunc(_AuthService);
            JsonContent content = JsonContent.Create(authreqdto);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = content.ReadAsStream();
            var result = await createFunc.Run(request, _mockLogger.Object);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

        }

        [Fact]
        public async void VerifyUserAsync()
        {
            AuthUser user = new AuthUser { UserId = 1, AppId = "100", Name = "Smit" };
            AuthRequest authreq = new AuthRequest { User = user, ExternalUserId = "100", VerificationCode = 101101, VerificationStatus = "Done" };
            AuthUserDTO userdto = new AuthUserDTO { UserId = "1", AppId = "100", Name = "Smit" };
            Token token = new Token { IdToken = "101101", TokenType = "Int", Expiry = 10000 };
            AuthRequestDTO authreqdto = new AuthRequestDTO { User = userdto, ExternalUserId = "100", VerificationCode = "101101", VerificationStatus = "Done", Token = token };

            _mockMapper.Setup(p => p.Map<AuthRequest>(authreqdto)).Returns(authreq);
            //_mockRepoWrapper.Setup(p => p.Auth. addUserAsync(authreq)).ReturnsAsync(authreq);
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new VerifyUserFunc(_AuthService);
            JsonContent content = JsonContent.Create(authreqdto);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = content.ReadAsStream();
            var result = await createFunc.Run(request, _mockLogger.Object);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async void InviteUserAsync()
        {
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new InviteUserFunc(_AuthService);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var result = await createFunc.Run(request, _mockLogger.Object, +919503594386);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

        }

        /*[Fact]
        public async void GetUserFuncAsync()
        {
            AuthUser user = new AuthUser { UserId = 1, AppId = "100", Name = "Smit" };
            AuthRequest authreq = new AuthRequest { User = user, ExternalUserId = "100", VerificationCode = 101101, VerificationStatus = "Done" };
            AuthUserDTO userdto = new AuthUserDTO { UserId = "1", AppId = "100", Name = "Smit" };
            Token token = new Token { IdToken = "101101", TokenType = "Int", Expiry = 10000 };
            AuthRequestDTO authreqdto = new AuthRequestDTO { User = userdto, ExternalUserId = "100", VerificationCode = "101101", VerificationStatus = "Done", Token = token };

            _mockMapper.Setup(p => p.Map<AuthRequest>(authreqdto)).Returns(authreq);
            _mockRepoWrapper.Setup(p => p.Auth.getAuthUserAsync(1)).ReturnsAsync(authreq);
            _AuthService = new AuthCoreService(_mockRepoWrapper.Object, _mockMapper.Object);
            var createFunc = new GetUserFunc(_AuthService);
            JsonContent content = JsonContent.Create(authreqdto);
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var result = await createFunc.Run(request,1, _mockLogger.Object);
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

        }*/
    }
}