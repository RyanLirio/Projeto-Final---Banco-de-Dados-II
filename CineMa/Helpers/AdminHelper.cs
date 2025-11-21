using Microsoft.AspNetCore.Http;

namespace CineMa.Helpers
{
    public static class AdminHelper
    {
        public static bool IsAdmin(HttpContext? httpContext)
        {
            if (httpContext == null)
                return false;

            var role = httpContext.Session.GetString("UsuarioRole");
            return role == "Admin";
        }
    }
}

