using MauiTimesheet.Data;
using MauiTimesheet.Data.Entities;
using MauiTimesheet.Models;

namespace MauiTimesheet.Services;

public class TimeLogService
{
    private readonly DatabaseService _database;
    private readonly ProjectsService _projectsService;

    public TimeLogService(DatabaseService database, ProjectsService projectsService)
    {
        _database = database;
        _projectsService = projectsService;
    }

    public DateTime SelectedDate { get; private set; } = DateTime.Today.Date;
    public List<TimeLogListModel> Entries { get; set; } = [];

    public async Task LoadTimeLogsAsync(DateTime? date = null)
    {
        date = date.HasValue ? date.Value.Date : SelectedDate;

        Entries = await _database.GetTimeLogs(date.Value);                    
    }

    public async Task<bool> SaveEntryAsync(TimeLogEntryModel model)
    {
        try
        {
            if (model.Id > 0)
                return false;

            if (model.Id == 0)
            {
                // New time log entry case
                var entry = new TimeLogEntry
                {
                    Date = model.Date,
                    Description = model.Description,
                    Hours = model.Hours,
                    ProjectId = model.ProjectId,
                    Task = model.Task
                };
                await _database.AddTimeLog(entry);

                var timeListEntry = new TimeLogListModel
                {
                    Id = entry.Id,
                    Task = entry.Task,
                    Hours = entry.Hours,
                    Description = entry.Description,
                    Date = entry.Date,
                };
                (timeListEntry.ProjectName, timeListEntry.ProjectColor) = _projectsService.GetProjectNameAndColor(entry.ProjectId);

                Entries.Add(timeListEntry);
            }
            return true;
        }
        catch (Exception ex)
        {
            await MauiInterop.AlertAsync(ex.Message, "Error");
        }
        return false;
    }

    public async Task<bool> DeleteEntryAsync(int timeLogEntryId)
    {
        try
        {
            var tineLogEntry = await _database.GetTimeLog(timeLogEntryId);
            if (tineLogEntry is null)
            {
                await MauiInterop.AlertAsync("Entry does not exist", "Error");
                return false;
            }
            await _database.GetTimeLog(timeLogEntryId);

            var entryIndex = Entries.FindIndex(p => p.Id == timeLogEntryId);
            if (entryIndex > -1)
            {
                Entries.RemoveAt(entryIndex);
            }
            return true;
        }
        catch (Exception ex)
        {
            await MauiInterop.AlertAsync(ex.Message, "Error");
        }
        return false;
    }

    
    public async Task<SummaryHours> GetSummaryAsync(DateTime today)
    {
        var todaysHours = await _database.GetTotalHoursAsync(today, today);

        var dayDiff = DayOfWeek.Monday - today.DayOfWeek;
        if (dayDiff > 0)
        {
            // Sunday, we will skip sunday
            dayDiff = 0;
        }
        var startOfWeek = today.AddDays(dayDiff);
        var thisWeekHours = await _database.GetTotalHoursAsync(startOfWeek, today);

        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        var thisMonthHours = await _database.GetTotalHoursAsync(startOfMonth, today);

        return new SummaryHours(todaysHours, thisWeekHours, thisMonthHours);
    }

    public Task<List<TimeLogListModel>> GetRecentEntriesAsync(int count = 5) =>
        _database.GetRecentTimeLogsAsync(count);

}
