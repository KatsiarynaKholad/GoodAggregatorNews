using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace GoodAggregatorNews.WebAPI.Filters
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.Identity?.Name == "Admin")
            {
                return true;
            }
            return false;

        }
    }
}
