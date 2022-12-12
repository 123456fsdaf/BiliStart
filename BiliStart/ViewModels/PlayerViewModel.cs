﻿using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using BiliBiliAPI.Models;
using BiliBiliAPI.Models.Videos;
using BiliStart.Contracts.Services;
using BiliStart.Helpers;
using BiliStart.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.Services.Commerce;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace BiliStart.ViewModels;
public partial class PlayerViewModel:ObservableRecipient
{
    public PlayerViewModel(ILocalSettingsService localSettingsService)
    {
        _FullButtonText = "\uE740";
        LocalSettingsService = localSettingsService;
    }


    public MediaPlayer NowMediaPlayer = new();
    public MediaSource? Source;


    public PlayerArgs Args
    {
        get;set;    
    }


    private VideosContent _VideoContent;

    public VideosContent VideoContent
    {
        get => _VideoContent;
        set => SetProperty(ref _VideoContent, value);
    }



    private VideoInfo VideoInfo;

    public VideoInfo _VIdeoInfo
    {
        get => VideoInfo;
        set => SetProperty(ref VideoInfo, value);
    }




    private double _MaxValue;

    public double MaxValue
    {
        get => _MaxValue;
        set=>SetProperty(ref _MaxValue, value);
    }

    

    private double _SliderValue;

    public double SliderValue
    {
        get => _SliderValue;
        set => SetProperty(ref _SliderValue, value);
    }




    private string FullButtonText;

    public string _FullButtonText
    {
        get => FullButtonText;
        set => SetProperty(ref FullButtonText, value);
    }

    public void FullChanged(bool isfull)
    {
        switch (isfull)
        {
            case true:
                _FullButtonText = "\uE73F";
                break;
            case false:
                _FullButtonText = "\uE740";
                break;
        }
    }

    private ObservableCollection<Support_Formats> Supports;

    /// <summary>
    /// 分辨率列表
    /// </summary>
    public ObservableCollection<Support_Formats> _Supports
    {
        get => Supports;
        set => SetProperty(ref Supports, value);
    }
    Support_Formats SelectFormats=null;
    public readonly BiliBiliAPI.Video.Video Video = new();

    public VideoInfo VI = null;


    private ObservableCollection<BiliBiliAPI.Models.Videos.Page> _VideoPages;

    public ObservableCollection<BiliBiliAPI.Models.Videos.Page> VideoPages
    {
        get => _VideoPages;
        set=>SetProperty(ref _VideoPages, value);
    }

    public async void InitVideo(ViewModels.Models.PlayerArgs playerArgs)
    {
        //初始化视频
        this.VideoPages = playerArgs.Content.Pages.ToObservableCollection();
        VideoContent = playerArgs.Content;
        if(VideoContent.Pages.Count == 1)
        {
            this.VI = (await Video.GetVideo(VideoContent, VideoIDType.AV, VideoContent.First_Cid)).Data;
            _Supports = VI.Support_Formats.ToObservableCollection();
            //为一个播放列表直接播放。
            _SupportIndex = -1;
            RefershSupport();
        }
        else
        {
            PageSelectIndex = 0;
        }
    }
    public async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var page = e.AddedItems[0] as BiliBiliAPI.Models.Videos.Page;
        VI = (await Video.GetVideo(VideoContent, VideoIDType.AV, page.Cid)).Data;
        if(_Supports != null)_Supports.Clear();
        _Supports = VI.Support_Formats.ToObservableCollection();
        RefershSupport();
    }

    public void Support_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0) return;
        SelectFormats = (Support_Formats)e.AddedItems[0];
        SetMediaPlayer();
    }

    async void SetMediaPlayer()
    {
        // https://www.microsoft.com/en-us/p/hevc-video-extensions-from-device-manufacturer/9n4wgh0z6vhq
        // HEVC解码器位置
        //https://apps.microsoft.com/store/detail/av1-video-extension/9MVZQVXJBQ9V?hl=zh-cn&gl=cn
        //AV1解码器位置
        foreach (var item in VI.Dash.DashVideos)
        {
            if (item.ID == this.SelectFormats!.Quality)
            {
                // hev 和 avc
                if (item.Codecs.StartsWith("hev"))
                {
                    Source = await PlayerHelper.CreateMediaSourceAsync(item, VI.Dash.DashAudio[0]);
                    NowMediaPlayer.SetMediaSource(Source!.AdaptiveMediaSource);
                    break;
                }
            }
        }
    }

    public async void RefershSupport()
    {
        switch (await LocalSettingsService.ReadSettingAsync<int>(BiliStart.Models.Settings.Player_Supper_Supper))
        {
            default:
            case 0:
                _SupportIndex = GetSupportIndex("4K");
                break;
            case 1:
                _SupportIndex = GetSupportIndex("1080");
                break;
            case 2:
                _SupportIndex = GetSupportIndex("720");
                break;
        }
    }

    private int _PageSelectIndex;

    public int PageSelectIndex
    {
        get => _PageSelectIndex;
        set =>SetProperty(ref _PageSelectIndex, value);
    }
    
    int GetSupportIndex(string value)
    {
        for (int i = 0; i < _Supports.Count; i++)
        {
            //这里有BUG，不应该只筛选文字，不够严谨，应该筛选清晰度代码
            if (_Supports[i].New_description.IndexOf(value) != -1)
            {
                return i;
            }
        }
        return -1;
    }

    private int SupportIndex;

    public int _SupportIndex
    {
        get =>SupportIndex;
        set => SetProperty(ref SupportIndex, value);
    }

    public ILocalSettingsService LocalSettingsService
    {
        get;
    }

    [RelayCommand]
    public void GoBack()
    {
        (App.MainWindow.Content as MainPage)!.RootFrame.GoBack();
    }
}
