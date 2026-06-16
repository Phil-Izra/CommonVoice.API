namespace CommonVoice.API.Application;

using CommonVoice.API.Application.Events;
using CommonVoice.API.Application.Events.Handlers;
using CommonVoice.API.Application.UseCases.GrievanceScores;
using CommonVoice.API.Application.UseCases.Participants;
using CommonVoice.API.Application.UseCases.Protests;
using CommonVoice.API.Application.UseCases.Users;
using CommonVoice.API.Domain.Aggregates.Participants;
using CommonVoice.API.Domain.Aggregates.Protests;
using CommonVoice.API.Domain.Aggregates.Users;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<LoginUserUseCase>();

        services.AddScoped<CreateProtestUseCase>();
        services.AddScoped<AddDemandUseCase>();
        services.AddScoped<ActivateProtestUseCase>();
        services.AddScoped<CompleteProtestUseCase>();

        services.AddScoped<CheckInParticipantUseCase>();
        services.AddScoped<CalculateProvincialGwsUseCase>();

        services.AddScoped<IDomainEventHandler<UserRegisteredEvent>,      UserRegisteredEventHandler>();
        services.AddScoped<IDomainEventHandler<ProtestCreatedEvent>,      ProtestCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<ProtestActivatedEvent>,    ProtestActivatedEventHandler>();
        services.AddScoped<IDomainEventHandler<ProtestCompletedEvent>,    ProtestCompletedEventHandler>();
        services.AddScoped<IDomainEventHandler<ParticipantCheckedInEvent>, ParticipantCheckedInEventHandler>();

        return services;
    }
}
