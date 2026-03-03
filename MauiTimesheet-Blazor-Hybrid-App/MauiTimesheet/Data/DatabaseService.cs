using MauiTimesheet.Data.Entities;
using MauiTimesheet.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTimesheet.Data;
public class DatabaseService
{
    private readonly SQLiteAsyncConnection _connection;
    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "maui-timesheet1.db3");
        _connection = new SQLiteAsyncConnection(dbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create
            );

        InitializeDatabase();
    }

    //private void InitializeDatabase()
    //{
    //    var connection = _connection.GetConnection();
    //    using (connection.Lock())
    //    {
    //        connection.CreateTable<User>();
    //        connection.CreateTable<Project>();
    //        connection.CreateTable<TimeLogEntry>();
    //    }
    //}
    private void InitializeDatabase()
    {
        var connection = _connection.GetConnection();
        using (connection.Lock())
        {
            connection.CreateTable<User>();
            connection.CreateTable<Project>();
            connection.CreateTable<TimeLogEntry>();

            // Seed user
            if (connection.Table<User>().Count() == 0)
            {
                connection.Insert(new User
                {
                    Name = "Abhay Prince",
                    Username = "abhay",
                    Password = "123456"
                });
            }

            // Color options
            string[] _colorOptions =
            [
                "#007AFF", "#34C759", "#FF9500", "#FF3B30",
            "#AF52DE", "#FF2D92"
            ];

            // Seed projects
            if (connection.Table<Project>().Count() == 0)
            {
                connection.InsertAll(new[]
                {
                new Project { Name = "Personal Portfolio", Color = _colorOptions[0], Description = "Website to showcase work" },
                new Project { Name = "Client Billing App", Color = _colorOptions[1], Description = "Simple invoice tracking app" },
                new Project { Name = "School Management System", Color = _colorOptions[1], Description = "App for schools to manage students" },
                new Project { Name = "Grocery POS", Color = _colorOptions[3], Description = "Point of Sale for small grocery store" },
                new Project { Name = "Fitness Tracker", Color = _colorOptions[4], Description = "Health and fitness mobile app" },
                new Project { Name = "Event Booking Portal", Color = _colorOptions[5], Description = "Web app to manage event bookings" },
            });
            }

            // Seed time logs
            if (connection.Table<TimeLogEntry>().Count() == 0)
            {
                var projects = connection.Table<Project>().ToList();
                var today = DateTime.Today;

                var timeLogs = new List<TimeLogEntry>();

                timeLogs.Add(new TimeLogEntry
                {
                    ProjectId = projects[0].Id,
                    Date = today,
                    Hours = 2.5,
                    Task = "Landing Page",
                    Description = "Created responsive layout for landing page"
                });

                for (int i = 0; i < 5; i++)
                {
                    timeLogs.Add(new TimeLogEntry
                    {
                        ProjectId = projects[1].Id,
                        Date = today.AddDays(-i),
                        Hours = 1.5 + i,
                        Task = $"Billing Module - Part {i + 1}",
                        Description = $"Worked on billing module section {i + 1}"
                    });
                }

                for (int i = 0; i < 15; i++)
                {
                    timeLogs.Add(new TimeLogEntry
                    {
                        ProjectId = projects[3].Id,
                        Date = today.AddDays(-i),
                        Hours = 1 + (i % 3),
                        Task = $"POS Feature {i + 1}",
                        Description = $"Implemented POS logic for feature {i + 1}"
                    });
                }

                for (int i = 0; i < 3; i++)
                {
                    timeLogs.Add(new TimeLogEntry
                    {
                        ProjectId = projects[4].Id,
                        Date = today.AddDays(-i),
                        Hours = 2,
                        Task = $"Fitness Sync {i + 1}",
                        Description = $"Integrated wearable sync feature {i + 1}"
                    });
                }


                connection.InsertAll(timeLogs);
            }
        }
    }


    public Task InsertUserAsync(User user) => _connection.InsertAsync(user);

    public Task<User> GetUserByUsername(string username) => 
        _connection.Table<User>()
        .Where(u => u.Username == username)
        .FirstOrDefaultAsync();

    public Task AddProject(Project project) => _connection.InsertAsync(project);
    public Task UpdateProject(Project project) => _connection.UpdateAsync(project);
    public Task DeleteProject(Project project) => _connection.DeleteAsync(project);
    public Task<List<Project>> GetProjects() => _connection.Table<Project>().ToListAsync();
    public Task<Project> GetProject(int projectId) => _connection.FindAsync<Project>(projectId);


    public Task AddTimeLog(TimeLogEntry timeLog) => _connection.InsertAsync(timeLog);
    public Task DeleteTimeLog(TimeLogEntry timeLog) => _connection.DeleteAsync(timeLog);
    public Task<List<TimeLogListModel>> GetTimeLogs(DateTime date)
    {
        var query = @"
            SELECT t.Id, t.Date, t.Hours, t.Task, t.Description, p.Name AS ProjectName, p.Color AS ProjectColor
            FROM TimeLogEntry AS t 
                INNER JOIN Project AS p 
                ON t.ProjectId = p.Id
            WHERE t.Date = ?
        ";
        return _connection.QueryAsync<TimeLogListModel>(query, date);
    }
    public Task<List<TimeLogListModel>> GetRecentTimeLogsAsync(int count)
    {
        var query = @"
            SELECT t.Id, t.Date, t.Hours, t.Task, t.Description, p.Name AS ProjectName, p.Color AS ProjectColor
            FROM TimeLogEntry AS t 
                INNER JOIN Project AS p 
                ON t.ProjectId = p.Id
            ORDER BY t.Id DESC 
            LIMIT ?
        ";
        return _connection.QueryAsync<TimeLogListModel>(query, count);
    }

    public Task<TimeLogEntry> GetTimeLog(int timeLogId) => _connection.FindAsync<TimeLogEntry>(timeLogId);

    public async Task<double> GetTotalHoursAsync(DateTime from, DateTime to)
    {
        from = from.Date;
        to = to.Date;

        var list = await _connection.Table<TimeLogEntry>()
                .Where(t => t.Date >= from && t.Date <= to)
                .ToListAsync();
        return list.Sum(t=> t.Hours);
    }
}
