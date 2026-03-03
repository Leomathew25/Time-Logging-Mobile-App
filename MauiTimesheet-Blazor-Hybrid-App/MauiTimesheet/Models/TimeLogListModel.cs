namespace MauiTimesheet.Models;

public class TimeLogListModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Hours { get; set; }
    public string? Task { get; set; }
    public string? Description { get; set; }
    public string ProjectName { get; set; }
    public string ProjectColor { get; set; }
}
