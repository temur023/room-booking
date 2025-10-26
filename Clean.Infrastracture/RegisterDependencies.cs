using Clean.Application.Abstractions;
using Clean.Application.Services.Authentication;
using Clean.Application.Services.Room;
using Clean.Application.Services.RoomEquipment;
using Clean.Domain.Enitities;
using Clean.Infrastracture.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Infrastracture;

public static class RegisterDependencies
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<DataContext>());
        
        services.AddScoped<IPasswordHasher<Comapany>, PasswordHasher<Comapany>>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomEquipmentService, RoomEquipmentService>();
        return services;
        
    }
}