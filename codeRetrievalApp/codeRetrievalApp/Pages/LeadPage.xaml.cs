using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace codeRetrievalApp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LeadPage : Page
    {
        public LeadPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(GRIDtitle);
        }

        private void BTNcancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        
        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FLPVW.SelectedIndex)
            {
                case 0:
                    {
                       // T2.lead();
                        KW1.quitLead1();
                    }
                    break;
                case 1:
                    {
                        //T2.quitLead();
                        KW1.lead1();
                    }
                    break;
                case 2:
                    {
                       // T2.quitLead();
                        KW1.quitLead1();
                        KW2.lead2();
                    }
                    break;
                default:
                    break;
            }
        }


        private void T2_ShowAssociates(FrameworkElement kwItem, string keyword)
        {

        }

        private void KW1_KeywordComplete()
        {

        }
    }
}
