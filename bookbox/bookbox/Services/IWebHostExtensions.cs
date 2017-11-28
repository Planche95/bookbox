using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookBox.Models
{
    public static class IWebHostExtensions
    {
        public static IWebHost Seed(this IWebHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var services = scope.ServiceProvider;

                using (AppDbContext context = services.GetRequiredService<AppDbContext>())
                using (RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>())
                using (UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>())
                {
                    DbInitializer.SeedAsync(context, userManager, roleManager).GetAwaiter().GetResult();
                }
            }

            return webhost;
        }
    }
}
