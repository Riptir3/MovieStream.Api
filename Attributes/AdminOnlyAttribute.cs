using Microsoft.AspNetCore.Authorization;

namespace MovieStream.Api.Attributes
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Policy = "AdminOnly";
        }
    }
}
