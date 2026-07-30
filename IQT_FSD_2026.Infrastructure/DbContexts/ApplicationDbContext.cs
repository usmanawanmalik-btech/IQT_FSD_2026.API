
using Microsoft.EntityFrameworkCore; 
using System.Reflection; 
using IQT_FSD_2026.Infrastructure.DbModels; 

namespace IQT_FSD_2026.Infrastructure.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
     
    public DbSet<UserTask> UserTasks { get; set; } 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
    }  
}
