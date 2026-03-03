using MauiTimesheet.Data;
using MauiTimesheet.Data.Entities;
using MauiTimesheet.Models;
using System.Text.Json;

namespace MauiTimesheet.Services;
public class AuthService
{
    private const string UserStateKey = "u-data";

    private readonly DatabaseService _database;

    public LoggedInUser User { get; private set; }

    public bool IsLoggedIn => User.Id > 0;

    public AuthService(DatabaseService database)
    {
        _database = database;
    }

    public void Initialize()
    {
        var loggedInUserJson = Preferences.Default.Get<string?>(UserStateKey, null);
        if (string.IsNullOrWhiteSpace(loggedInUserJson))
            return;

        User = LoggedInUser.FromJson(loggedInUserJson);
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var user = await _database.GetUserByUsername(model.Username);
        if (user == null || user.Password != model.Password)
        {
            await MauiInterop.AlertAsync("Invalid credentials", "Error");
            return false;
        }
        User = new LoggedInUser(user.Id, user.Name);

        Preferences.Default.Set<string>(UserStateKey, User.ToJson());

        return true;
    }
    public async Task RegisterAsync(RegisterModel model)
    {
        var existing = await _database.GetUserByUsername(model.Username);
        if (existing != null)
        {
            existing.Name = model.FullName;
            existing.Password = model.Password;
        }
        else
        {
            var user = new User
            {
                Name = model.Username,
                Password = model.Password,
                Username = model.Username,
            };
            await _database.InsertUserAsync(user);
        }
    }

    public void Logout()
    {
        User = new();
        Preferences.Default.Remove(UserStateKey);
    }
}
