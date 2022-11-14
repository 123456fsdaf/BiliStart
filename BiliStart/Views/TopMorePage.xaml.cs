// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BiliStart.Contracts.Services;
using BiliStart.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BiliStart.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TopMorePage : Page
{
    public TopMoreViewModel ViewModel
    {
        get;
    }
    public TopMorePage()
    {
        ViewModel = App.GetService<TopMoreViewModel>();
        this.InitializeComponent();
        ViewModel.navigationService.Frame = FrameControl;
        ViewModel.navigationViewService.Initialize(NavigationControl);
    }

}
