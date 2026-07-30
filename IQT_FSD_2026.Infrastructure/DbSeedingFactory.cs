
using Microsoft.Extensions.Logging; 
using Btech.Package.EntityFramework.Context;
using Btech.Package.EntityFramework.Interfaces;

using IQT_FSD_2026.Infrastructure.DbContexts;
using IQT_FSD_2026.Infrastructure.DbModels;

namespace IQT_FSD_2026.Infrastructure
{
    public class DbSeedingFactory : FrameworkDbSeedingFactory<ApplicationDbContext>
    {
        private readonly ILogger<DbSeedingFactory> _logger;
        public DbSeedingFactory(ILogger<DbSeedingFactory> logger) { _logger = logger; }

        public override void EnsureDatabaseSeeding(ApplicationDbContext _context, IUserContext _userContext)
        {
            try
            {
                
                if (_context.ChangeTracker.HasChanges())
                {
                    _context.SaveChanges();
                    _logger.LogInformation($"Data seeding successfully completed for client {_userContext!.Jwt!.ClientId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception on SaveChanges() while data seeding for the client {_userContext!.Jwt!.ClientId}");
                throw;
            }
        }

        private List<UserTask> UserTasks(IUserContext _userContext)
        {
            return new List<UserTask>()
            {
                new UserTask() {
                    TaskId = 0,
                    Title = "Base",
                    ClientId = 1,
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow
                }
            };
        }

    }
}