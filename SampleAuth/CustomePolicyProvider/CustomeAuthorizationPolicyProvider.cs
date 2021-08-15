using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SampleAuth.CustomePolicyProvider
{
    public class CustomeAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach (var customePolicy in DynamicPolicies.Get())
            {
                if (policyName.StartsWith(customePolicy))
                {
                    var policy = DynamicAuthorizationpolicyFactory.Create(policyName);
                    return Task.FromResult(policy);
                }
            }
            return base.GetPolicyAsync(policyName);
        }

    }
    // Dynamic policy that we want to add
    public static class DynamicPolicies
    {
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Rank;
        }
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }
    public static class DynamicAuthorizationpolicyFactory
    {
        //{type}.{value}
        //{securitylevel}.{5}
        public static AuthorizationPolicy Create(string policyName)
        {
            var parts = policyName.Split('.');
            var type = parts.First();
            var value = parts.Last();
            switch (type)
            {
                case DynamicPolicies.Rank:
                    return new AuthorizationPolicyBuilder()
                            .RequireClaim("Rank", value).Build();
                case DynamicPolicies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                           .AddRequirements(new SecurityLevelRequirment(Convert.ToInt32(value)))
                           .Build();
                default:
                    return null;
            }
        }
    }
    public class SecurityLevelRequirment : IAuthorizationRequirement
    {
        public int Level { get; }
        public SecurityLevelRequirment(int level)
        => Level = level;
    }
    public class SecurityHandler : AuthorizationHandler<SecurityLevelRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecurityLevelRequirment requirement)
        {
            var claimType = Convert.ToInt32(context.User.Claims.FirstOrDefault(x => x.Type == DynamicPolicies.SecurityLevel)?.Value ?? "0");
            if (requirement.Level <= claimType)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}