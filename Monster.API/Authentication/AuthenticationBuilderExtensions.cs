using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        // Custom authentication extension method
        public static AuthenticationBuilder AddMonsterAuth(this AuthenticationBuilder builder, Action<MonsterAuthOptions> configureOptions)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<MonsterAuthOptions, MonsterAuthHandler>(MonsterAuthOptions.DefaultScheme, configureOptions);
        }
    }
}
