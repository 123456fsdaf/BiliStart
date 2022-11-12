﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiliStart.Views;

namespace BiliStart.Services;
public static class TipShow
{
    public static async void SendMessage(string message,string title)
    {
        var result = (App.MainWindow.Content as ShellPage)!.ToggleThemeTeachingTip2;
        result.Title = title;
        result.Subtitle= message;
       
        result.IsOpen = true;
        await Task.Delay(TimeSpan.FromSeconds(2));
        result.IsOpen = false;
    }
}
