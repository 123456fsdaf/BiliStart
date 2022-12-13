// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace BiliStart.UI.Controls;
public sealed partial class PopupDialog : UserControl
{
    
    //��ŵ������е���Ϣ
    private string _popupContent;
    private readonly Panel uIElement;

    //����һ��popup����
    private Popup _popup = null;
    public PopupDialog()
    {
        this.InitializeComponent();

        //����ǰ�ĳ��Ϳ� ��ֵ���ؼ�

        //����ǰ�Ŀؼ۸�ֵ��������Child����  Child�����ǵ�����Ҫ��ʾ������ ��ǰ��this��һ��Grid�ؼ���
        _popup = new Popup();
        _popup.Child = this;

        //����ǰ��grid���һ��loaded�¼�����ʹ����ShowAPopup()��ʱ��Ҳ���ǵ�����ʾ�ˣ�������������ݾ������ǵ�grid������������Ҫ���������ˡ�
        this.Loaded += PopupNoticeLoaded;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="popupContentString">��Ҫ����������</param>
    /// <param name="uIElement">Panel����</param>
    public PopupDialog(string popupContentString,Panel uIElement,Symbol symbol) : this()
    {
        PopupIcon.Symbol = symbol;
        _popupContent = popupContentString;
        this.uIElement = uIElement;
        _popup.XamlRoot = uIElement.XamlRoot;
    }

    /// <summary>
    /// ��ʾһ��popup���� ����Ҫ��ʾһ������ʱ��ִ�д˷���
    /// </summary>
    public void ShowAPopup()
    {
        _popup.IsOpen = true;
    }

    private void PopupNoticeLoaded(object sender, RoutedEventArgs e)
    {
        PopupContent.Text = _popupContent;
        //�򿪶���
        this.PopupIn.Begin();
        //�����붯��ִ��֮�󣬴����ŵ����Ѿ���ָ��λ���ˣ���ָ��λ�õ�һ�� �Ϳ�����ʧ��ȥ��
        this.PopupIn.Completed += PopupInCompleted;
        this.Width = uIElement.ActualWidth;
        this.Height = uIElement.ActualHeight;
    }


    /// <summary>
    /// �����붯����ɺ� ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void PopupInCompleted(object sender, object e)
    {
        //��ԭ����һ��
        await Task.Delay(1000);

        //����ʧ������
        this.PopupOut.Begin();
        //popout ������ɺ� ����
        this.PopupOut.Completed += PopupOutCompleted;
    }

    //�����˳��������� �����������̽��� �������ر�
    public void PopupOutCompleted(object sender, object e)
    {
        _popup.IsOpen = false;
    }
}
