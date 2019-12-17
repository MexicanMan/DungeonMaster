using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        // Custom authentication extension method
        public static AuthenticationBuilder AddMicroAuth(this AuthenticationBuilder builder, Action<MicroAuthOptions> configureOptions)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<MicroAuthOptions, MicroAuthHandler>(MicroAuthOptions.DefaultScheme, configureOptions);
        }
    }
}
