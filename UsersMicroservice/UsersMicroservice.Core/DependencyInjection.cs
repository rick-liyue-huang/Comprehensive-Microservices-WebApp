using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Core.Services;
using UsersMicroservice.Core.Validators;

namespace UsersMicroservice.Core;

public static class DependencyInjection
{
  public static IServiceCollection AddCore(this IServiceCollection services)
  {
    services.AddScoped<IUsersService, UsersService>();
    services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

    return services;
  }
}