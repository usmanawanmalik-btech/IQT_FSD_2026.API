using Btech.Package.Domain.Base;
using Btech.Package.EntityFramework.Interfaces;
using IQT_FSD_2026.Domain.Dtos; 

namespace IQT_FSD_2026.Application.Services.Interfaces
{
    public interface IUserTaskService : IGenericService<UserTaskDto, UserTaskSearchCriteriaDto>
    {
    }
}
