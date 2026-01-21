using System.Reflection;
using FitNetClean.Application.Common.Behaviors;
using FitNetClean.Application.Common.Validation;
using FitNetClean.Application.Features.Shared.Commands;
using FitNetClean.Application.Features.Shared.Queries;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FitNetClean.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            
            // Add validation behavior to the pipeline
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            // Add cache invalidation behavior
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ContraIndicationCacheInvalidationBehavior<,>));
        });

        RegisterGenericHandlers(services);

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(assembly);

        services.AddScoped<IDeleteValidator, DeleteValidator>();

        return services;
    }

    private static void RegisterGenericHandlers(IServiceCollection services)
    {
        var domainAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "FitNetClean.Domain");

        if (domainAssembly == null) return;

        var entityTypes = domainAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "FitNetClean.Domain.Entities");

        foreach (var entityType in entityTypes)
        {
            // Register GetListHandler
            RegisterHandler(services, entityType, typeof(GetListQuery<>), typeof(List<>), typeof(GetListHandler<>));

            // Register GetByIdHandler
            RegisterHandler(services, entityType, typeof(GetByIdQuery<>), entityType, typeof(GetByIdHandler<>));

            // Register CreateCommandHandler
            RegisterHandler(services, entityType, typeof(CreateCommand<>), entityType, typeof(CreateCommandHandler<>));

            // Register UpdateCommandHandler
            RegisterHandler(services, entityType, typeof(UpdateCommand<>), entityType, typeof(UpdateCommandHandler<>));

            // Register DeleteCommandHandler
            RegisterHandler(services, entityType, typeof(DeleteCommand<>), typeof(bool), typeof(DeleteCommandHandler<>));
        }
    }

    private static void RegisterHandler(
        IServiceCollection services,
        Type entityType,
        Type requestType,
        Type responseTypeOrGeneric,
        Type handlerType)
    {
        var closedRequestType = requestType.MakeGenericType(entityType);

        Type closedResponseType;
        if (responseTypeOrGeneric.IsGenericTypeDefinition)
        {
            closedResponseType = responseTypeOrGeneric.MakeGenericType(entityType);
        }
        else
        {
            closedResponseType = responseTypeOrGeneric;
        }

        var handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(closedRequestType, closedResponseType);
        var handlerImplementationType = handlerType.MakeGenericType(entityType);

        services.AddTransient(handlerInterfaceType, handlerImplementationType);
    }
}
