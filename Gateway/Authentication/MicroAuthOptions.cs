using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Authentication
{
    public class MicroAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "MicroAuth";
        public string Scheme => DefaultScheme;
    }
}
