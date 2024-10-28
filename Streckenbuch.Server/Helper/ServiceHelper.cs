using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;

namespace Streckenbuch.Server.Helper;

public static class ServiceHelper
{
    public static async Task<ApplicationUser> GetAuthenticatedUser(this ServerCallContext context, UserManager<ApplicationUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(context);

        var httpContext = context.GetHttpContext();

        if (httpContext is null)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Invalid login credentials"));
        }

        ApplicationUser? user = await userManager.GetUserAsync(httpContext.User);

        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Invalid login credentials"));
        }

        return user;
    }
}
