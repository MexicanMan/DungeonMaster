using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Authentication
{
    public class MonsterAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "MonsterAuth";
        public string Scheme => DefaultScheme;
    }
}
