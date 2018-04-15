using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using codeRetrievalApp.Lib;
using codeRetrievalApp.Controls;
using codeRetrievalApp.Pages;
using Microsoft.Graphics.Canvas.Effects;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace codeRetrievalApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 1000));
            ApplicationView.PreferredLaunchViewSize = new Size(600, 1000);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(GRIDtitle);
            Constants.mainPage = this;
            Constants.KWbox = Box;
            //LEAD.Visibility = Visibility.Visible;
            //LEAD.Show(T2input, "");
        }

        private void T3input_ShowAssociates(FrameworkElement kwItem, string keyword)
        {
            Asso.Visibility = Visibility.Visible;
            Asso.Show(kwItem, keyword);
        }

        private void BTNsearch_Click(object sender, RoutedEventArgs e)
        {
           // Constants.rootFrame.Navigate(typeof(SearchResultPage), T2input.Query);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //T2input.lead();
        }

        private void T3input_Search(List<string> keywords)
        {
            Constants.rootFrame.Navigate(typeof(SearchResultPage), keywords);
        }
    }
}
