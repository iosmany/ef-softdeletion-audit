// See https://aka.ms/new-console-template for more information
using ef_softdeletion_audit;
using ef_softdeletion_audit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var hostBuilder = Host.CreateDefaultBuilder();

hostBuilder.ConfigureServices((context, services) =>
{
    // Add services to the container.
    services.AddSingleton<SoftDeleteAuditInterceptor>();

    // Register your services here
    services.AddDbContext<ApplicationDbContext>((sp, options) => 
    {
        options.AddInterceptors(sp.GetRequiredService<SoftDeleteAuditInterceptor>());
        options.UseInMemoryDatabase("SofDelAuditDB");
    });
});

var host = hostBuilder.Build();

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
// Add some test data
var user = new User()
{
    Email = "test@test.com",
    FirstName = "Test",
    LastName = "User",
    LoginAttempts = 0,
    IsDeleted = false
};

dbContext.Users.Add(user);
await dbContext.SaveChangesAsync();

// Soft delete the user
dbContext.Remove(user);
await dbContext.SaveChangesAsync();

await host.RunAsync();
