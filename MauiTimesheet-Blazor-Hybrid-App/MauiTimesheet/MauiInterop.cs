namespace MauiTimesheet;
internal static class MauiInterop
{
    public static Task AlertAsync(string message, string title) =>
        App.Current!.Windows[0].Page!.DisplayAlert(title, message, "OK");

    public static Task<bool> ConfirmAsync(string message, string title) =>
        App.Current!.Windows[0].Page!.DisplayAlert(title, message, "Yes", "No");
}
