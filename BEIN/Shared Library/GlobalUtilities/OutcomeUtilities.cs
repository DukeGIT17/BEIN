using Microsoft.AspNetCore.Identity;
namespace Shared_Library.GlobalUtilities
{
    public static class OutcomeUtilities
    {
        /// <summary>
        /// Processes an <see cref="IdentityResult"/> to determine whether an identity operation was successful 
        /// and populates an <see cref="out"/> error variable with the error messages if the operation fails.
        /// </summary>
        /// <param name="result">The <see cref="IdentityResult"/> object to be processed.</param>
        /// <param name="error">The string variable in which the error messages should be assigned, if there are any.</param>
        /// <returns>A <see cref="bool"/> value determine if the operation succeeded.</returns>
        public static bool IdentityOutcome(this IdentityResult result, out string error)
        {
            error = "";
            string err = "";
            result.Errors.ToList().ForEach(e => err += $"{e.Code}: {e.Description}\n");
            error = err;
            return !string.IsNullOrEmpty(error);
        }
    }
}