using Btech.Package.Domain.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQT_FSD_2026.Infrastructure.DbModels;

public class UserTask : BaseEFEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TaskId { get; set; }

    [Required]
    public string Title { get; set; }

    [DefaultValue(null)]
    public string? Description { get; set; }
    
    [DefaultValue(false)]
    public bool Completed { get; set; }
}
  