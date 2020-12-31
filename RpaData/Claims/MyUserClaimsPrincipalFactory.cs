using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RpaData.DataContext;
using RpaData.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RpaData.Claims
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly ApplicationDbContext _context;
        public MyUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext appDbContext,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, roleManager, optionsAccessor)
        {
            _context = appDbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            if (await UserManager.IsInRoleAsync(user,"Client"))
            {
                var clientId = _context.tblPharmacists.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
                identity.AddClaim(new Claim("ClientId",clientId.Result.Id.ToString()));
                identity.AddClaim(new Claim("IsProfileComplete", clientId.Result.profileComplete.ToString()));
            }
            identity.AddClaim(new Claim("FullName", user.FirstName +" "+ user.LastName?? "[Click to edit profile]"));
            identity.AddClaim(new Claim("UserId", user.Id));
            return identity;
        }
    }
}
