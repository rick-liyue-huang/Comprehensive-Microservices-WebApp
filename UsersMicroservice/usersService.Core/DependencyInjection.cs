using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using usersService.Core.ServiceContracts;
using usersService.Core.Services;
using usersService.Core.Validators;

namespace usersService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreService(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>(); 
        // don't need to add 'RegisterRequestValidator', it because they are in the ame assembly
        
        return services;
    }
}