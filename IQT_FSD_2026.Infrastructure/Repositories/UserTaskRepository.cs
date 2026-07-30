 
using Btech.Package.EntityFramework.Contracts;
using IQT_FSD_2026.Infrastructure.DbContexts;
using IQT_FSD_2026.Infrastructure.DbModels;

namespace IQT_FSD_2026.Infrastructure.Repositories;
  
#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
public class UserTaskRepository : GenericRepository<UserTask, ApplicationDbContext>, IUserTaskRepository
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
{
    public UserTaskRepository(ApplicationDbContext _dbContext) : base(_dbContext)
    {
    }
}  