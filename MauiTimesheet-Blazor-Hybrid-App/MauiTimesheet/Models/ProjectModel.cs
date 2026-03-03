using System.ComponentModel.DataAnnotations;

namespace MauiTimesheet.Models;

public class ProjectModel
{
    public int Id { get; set; }
    
    [Required, MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(300)]
    public string? Description { get; set; }

    [Required (ErrorMessage = "Color selection is required"), MaxLength(10), MinLength(3)]
    public string Color { get; set; }

    public ProjectModel Clone() => (MemberwiseClone() as ProjectModel)!;
}