using Microsoft.AspNetCore.Identity;

namespace Shared_Library.GlobalUtilities
{
    public static class StaticUtilites
    {
        public static bool IdentityOutcome(this IdentityResult result, out string? error)
        {
            error = null;
            //result.Errors.ToList().ForEach(e => error = $"{e.Code}: {e.Description}");
            throw new(error);
        }
    }
}
