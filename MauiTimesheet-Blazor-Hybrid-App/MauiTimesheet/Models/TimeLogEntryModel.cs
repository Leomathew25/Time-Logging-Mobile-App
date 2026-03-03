using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MauiTimesheet.Models;

public class TimeLogEntryModel
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Project selection is required")]
    public int ProjectId { get; set; }
    public DateTime Date { get; set; }

    [Range(0.25, 12, ErrorMessage = "Hours must be between 0.25 (i.e. 15 minutes) and 12")]
    public double Hours { get; set; }

    public string? Task { get; set; }
    public string? Description { get; set; }
}
