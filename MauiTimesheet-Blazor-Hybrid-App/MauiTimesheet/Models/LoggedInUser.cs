using SQLite;
using System.Text.Json;

namespace MauiTimesheet.Models;

public record struct LoggedInUser(int Id, string Name)
{
    public readonly string ToJson() => JsonSerializer.Serialize(this);

    public static LoggedInUser FromJson(string json) =>
        JsonSerializer.Deserialize<LoggedInUser>(json);
}
