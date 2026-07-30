
using Btech.Package.Domain.Base;

namespace IQT_FSD_2026.Domain.Dtos;
public class UserTaskDto : EntityDto
{
    public long TaskId { get; set; } 
    public string Title { get; set; } 
    public string? Description { get; set; } 
    public bool Completed { get; set; }
}

public class UserTaskSearchCriteriaDto : SearchCriteriaDto
{
    public string? StatusFilter { get; set; } = string.Empty;
}