﻿using Microsoft.AspNetCore.Http;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Core.Interfaces
{
    public interface IAuthCoreService
    {
        public Task<AuthRequestDTO> registerUserAsync(AuthRequestDTO auth);
        public Task<AuthRequestDTO> verifyUserAsync(AuthRequestDTO auth);
        public Task<long?> validateTokenAsync(AuthRequestDTO auth);
        public Task<AuthUserDTO> getUserAsync(long userid);
        public Task<IEnumerable<AuthUserDTO>> getUsersAsync();
        public Task<AuthUserDTO> uploadPhotoAsync(IFormFile uploadfile, Hashtable keys);
    }
}
