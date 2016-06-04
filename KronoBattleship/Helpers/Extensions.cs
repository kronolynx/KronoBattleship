using KronoBattleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace KronoBattleship.Helpers
{
    public static class Extensions
    {
        public static string GetPicture(this IIdentity identity)
        {
            var picture = ((ClaimsIdentity)identity).FindFirst("Picture");
            return picture?.Value;
        }
    }
}