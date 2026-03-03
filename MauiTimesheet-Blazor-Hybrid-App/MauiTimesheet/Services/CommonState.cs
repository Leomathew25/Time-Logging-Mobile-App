using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTimesheet.Services;
public static class CommonState
{
    public static event Action<bool>? ToggleLoader;

    public static void ShowLoader() => ToggleLoader?.Invoke(true);
    public static void HideLoader() => ToggleLoader?.Invoke(false);
}
