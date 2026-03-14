using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Entities;
using Microsoft.Extensions.Configuration;
using FieldReservation.Application.Interfaces;
using FieldReservation.Infrastructure.Services;
using FieldReservation.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Infrastructure.Persistence.Data;

namespace FieldReservation.Infrastructure.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAppDbContext>(sp =>
                sp.GetRequiredService<AppDbContext>());

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(10));

            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<GoogleAuthSettings>(configuration.GetSection(nameof(GoogleAuthSettings)));

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}