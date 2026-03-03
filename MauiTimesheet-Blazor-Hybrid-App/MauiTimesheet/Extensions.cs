using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTimesheet;
public static class Extensions
{
    public static string ToFormattedTime(this double totalHours)
    {
        var h = Math.Floor(totalHours);
        var m = Math.Round((totalHours - h) * 60);
        return $"{h}h {m}m";
    }
}
