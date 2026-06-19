namespace CommonVoice.API.Infrastructure;

using CommonVoice.API.Application.Common;
using CommonVoice.API.Application.Events;
using CommonVoice.API.Domain.Repositories;
using CommonVoice.API.Infrastructure.Events;
using CommonVoice.API.Infrastructure.Repositories;
using CommonVoice.API.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository,           InMemoryUserRepository>();
        services.AddSingleton<IProtestRepository,        InMemoryProtestRepository>();
        services.AddSingleton<IParticipantRepository,    InMemoryParticipantRepository>();
        services.AddSingleton<IGrievanceScoreRepository, InMemoryGrievanceScoreRepository>();

        services.AddScoped<IDomainEventDispatcher,       DomainEventDispatcher>();

        services.AddSingleton<IPasswordHasher,           Pbkdf2PasswordHasher>();
        services.AddSingleton<IJwtTokenService,          JwtTokenService>();

        return services;
    }
}
