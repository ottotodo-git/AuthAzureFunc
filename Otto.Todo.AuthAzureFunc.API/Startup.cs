using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Otto.Todo.AuthAzureFunc.API.Mappers;
using Otto.Todo.AuthAzureFunc.Core.Interfaces;
using Otto.Todo.AuthAzureFunc.Core.Services;
using Otto.Todo.AuthAzureFunc.Repository.Context;
using Otto.Todo.AuthAzureFunc.Repository.Interfaces;
using Otto.Todo.AuthAzureFunc.Repository.Repositories;
using System;

[assembly: FunctionsStartup(typeof(Otto.Todo.AuthAzureFunc.API.Startup))]

namespace Otto.Todo.AuthAzureFunc.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AuthProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IAuthCoreService, AuthCoreService>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
    }
}
