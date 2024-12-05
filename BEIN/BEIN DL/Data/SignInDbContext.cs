using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BEIN_DL.Data
{
    public class SignInDbContext(DbContextOptions<SignInDbContext> options) : IdentityDbContext(options)  
    {
    }
}
