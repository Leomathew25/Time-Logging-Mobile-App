using SQLite;

namespace MauiTimesheet.Data.Entities;

public class TimeLogEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ProjectId { get; set; }
    public DateTime Date { get; set; }
    public double Hours { get; set; }
    public string? Task { get; set; }
    public string? Description { get; set; }
}