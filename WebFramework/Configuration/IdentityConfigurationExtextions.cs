using Common;
using Data;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WebFramework.Configuration
{
    public static class IdentityConfigurationExtextions
    {
        public static void AddCustomIdentity(this IServiceCollection services,IdentitySettings identitySettings)
        {
            services.AddIdentity<User, Role>(identityoptions =>
            {
                // Password Settings
                identityoptions.Password.RequireDigit = identitySettings.PasswordRequireDigit;
                identityoptions.Password.RequiredLength = identitySettings.PasswordRequiredLength;
                identityoptions.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
                identityoptions.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
                identityoptions.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
                
                // User Settings
                identityoptions.User.RequireUniqueEmail = identitySettings.UserRequireUniqueEmail;

                // SignIn Settings
                //identityoptions.SignIn.RequireConfirmedPhoneNumber = false;
                //identityoptions.SignIn.RequireConfirmedAccount=false;
                //identityoptions.SignIn.RequireConfirmedEmail=false; 

                //Lockout Settings for use SignIn manager
                //identityoptions.Lockout.MaxFailedAccessAttempts = 5;
                //identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // Lock account after MaxFailedAccessAttempts mMinutes
                //identityoptions.Lockout.AllowedForNewUsers = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
