using BiliStart.ViewModels.PageViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;


namespace BiliStart.Pages;
public sealed partial class RankPage : Page
{
    public RankViewModel ViewModel
    {
        get;
    }

    public RankPage()
    {
        ViewModel = App.GetService<RankViewModel>();
        this.InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        try
        {
            //��������
            // Tip:������һ���������а�ҳ�浽������ͱ���������û�д����Ŷ����TagЯ����Tid�����������������ȫ��������F5��ȥ��
            var value = int.Parse(e.Parameter.ToString()!);
            if(value != null)
            {
                await ViewModel!.refersh(value.ToString()!);
            }
        }
        catch (Exception)
        {
            await ViewModel.Loaded();
        }
    }

}
