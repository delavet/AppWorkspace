using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SearchResultPage : Page
    {
        private CodeList codeList;
        public SearchResultPage()
        {
            this.InitializeComponent();
            codeList = new CodeList();
            codeList.DataLoaded += DataLoaded;
            codeList.DataLoading += DataLoading;
            GRIDVWcode.ItemsSource = codeList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SetTitleBar(GRIDtitle);
            STRBDpopin.Begin();
        }

        private void DataLoading()
        {
            
        }

        private void DataLoaded()
        {

        }

        private void GRIDVWcode_ItemClick(object sender, ItemClickEventArgs e)
        {
            var gridView = (GridView)sender;
            var item = (CodeInfo)e.ClickedItem;
            if(gridView.ContainerFromItem(item) is GridViewItem)
            {
                gridView.PrepareConnectedAnimation("controlAnimation", item, "controlRoot");
            }
            this.Frame.Navigate(typeof(CodeDetailPage), e.ClickedItem);
        }

        public ItemsWrapGrid GetItemsWrapGrid(Windows.UI.Xaml.DependencyObject depObj)
        {
            if (depObj is ItemsWrapGrid)
            {
                return depObj as ItemsWrapGrid;
            }
            for (int i = 0; i < Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(depObj, i);
                var result = GetItemsWrapGrid(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = GRIDroot.ActualWidth;
            w -= 20;
            ItemsWrapGrid wrap = GetItemsWrapGrid(GRIDVWcode);
            try
            {
                if (w < 650) wrap.ItemWidth = w;
                else if (w < 850) wrap.ItemWidth = w / 2;
                else if (w < 1150) wrap.ItemWidth = w / 3;
                else if (w < 1600) wrap.ItemWidth = w / 4;
                else wrap.ItemWidth = w / 5;
            }
            catch
            {

            }
        }
    }
}
