using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;


namespace DiamondShopSystem.API.Policies
{
    /// <summary>
    /// Custom attribute to apply a named CORS policy to controllers or actions.
    /// Usage: [UseCorsPolicy(AuthorizationPolicies.AuthEndpoints)]
    /// </summary>
    public class UseCorsPolicyAttribute : EnableCorsAttribute
    {
        public UseCorsPolicyAttribute(string policyName) : base(policyName) { }
    }
} 