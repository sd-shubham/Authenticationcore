using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuth.AuthRequirement
{
    public class CustomRequireCliam : IAuthorizationRequirement
    {
        public string CliamType { get; }
        public CustomRequireCliam(string cliamType)
        {
            CliamType = cliamType;
        }

    }
    public class CustomRequireCliamHandler : AuthorizationHandler<CustomRequireCliam>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireCliam requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.CliamType);
            if (hasClaim) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder RequireCustomeClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomRequireCliam(claimType));
            return builder;
        }
    }
}