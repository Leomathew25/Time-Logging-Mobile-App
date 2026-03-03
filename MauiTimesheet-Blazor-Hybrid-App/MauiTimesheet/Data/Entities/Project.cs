using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MauiTimesheet.Data.Entities;

public class Project
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Required]
    public string Color { get; set; }
}
