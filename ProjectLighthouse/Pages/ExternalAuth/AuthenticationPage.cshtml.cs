using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Pages.ExternalAuth
{
    public class AuthenticationPage : BaseLayout
    {
        public AuthenticationPage(Database database) : base(database)
        {}

        public List<AuthenticationAttempt> AuthenticationAttempts;

        public async Task<IActionResult> OnGet()
        {
            this.AuthenticationAttempts = this.Database.AuthenticationAttempts.Include
                    (a => a.GameToken)
                .Where(a => a.GameToken.UserId == this.User.UserId)
                .ToList();

            return this.Page();
        }
    }
}