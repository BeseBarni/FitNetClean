using FitNetClean.Domain.Constants;
using FitNetClean.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FitNetClean.WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {

            options.AddPolicy(Policies.AdminOnly, policy =>
                policy.RequireRole(Roles.Admin));

            options.AddPolicy(Policies.ProgramWriterOrAdmin, policy =>
                policy.RequireRole(Roles.ProgramWriter, Roles.Admin));

            options.AddPolicy(Policies.ProgramReaderOrHigher, policy =>
                policy.RequireRole(Roles.ProgramReader, Roles.ProgramWriter, Roles.Admin));

            options.AddPolicy(Policies.ActiveUsersOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new ActiveUserRequirement());
            });
        });

        services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();

        return services;
    }
}

public class ActiveUserRequirement : IAuthorizationRequirement
{
}

public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ActiveUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ActiveUserRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            context.Fail();
            return;
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
