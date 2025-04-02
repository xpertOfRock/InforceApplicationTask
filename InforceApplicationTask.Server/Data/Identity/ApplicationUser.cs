using Microsoft.AspNetCore.Identity;

namespace InforceApplicationTask.Server.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = string.Empty;
    }
}
