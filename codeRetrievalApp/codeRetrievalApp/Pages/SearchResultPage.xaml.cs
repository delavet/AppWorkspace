using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        private Compositor _compositor;
        private Visual _listVisual;
        public SearchResultPage()
        {
            this.InitializeComponent();
            this._compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            this._listVisual = GRIDVWcode.GetVisual();
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
            var item = e.ClickedItem as CodeInfo;
            
            if(gridView.ContainerFromItem(item) is GridViewItem)
            {
                gridView.PrepareConnectedAnimation("controlAnimation", item, "TXTBLKtitle");
                gridView.PrepareConnectedAnimation("postAnimation", item, "TXTBLKpost");
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
                else if (w < 850) wrap.ItemWidth = w;
                else if (w < 1150) wrap.ItemWidth = w / 2;
                else if (w < 1600) wrap.ItemWidth = w / 2;
                else wrap.ItemWidth = w / 5;
            }
            catch
            {

            }
        }

        private void GRIDVWcode_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GRIDVWcode_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            int index = args.ItemIndex;

            if (!args.InRecycleQueue)
            {
                args.ItemContainer.Loaded -= ItemContainer_Loaded;
                args.ItemContainer.Loaded += ItemContainer_Loaded;
            }
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)GRIDVWcode.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;
            var itemIndex = GRIDVWcode.IndexFromContainer(itemContainer);
            if (itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = itemContainer.GetVisual();
                var delayIndex = itemIndex - itemsPanel.FirstVisibleIndex;

                itemVisual.Opacity = 0f;
                itemVisual.SetTranslation(new Vector3(50, 0, 0));

                // Create KeyFrameAnimations
                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertKeyFrame(1f, 0f);
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds((delayIndex * 30));

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertKeyFrame(1f, 1f);
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(delayIndex * 30);

                // Start animations
                itemVisual.StartAnimation(itemVisual.GetTranslationXPropertyName(), offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            itemContainer.Loaded -= ItemContainer_Loaded;
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var rootGrid = sender as Grid;

            rootGrid.PointerEntered += RootGrid_PointerEntered;
            rootGrid.PointerExited += RootGrid_PointerExited;

            var maskBorder = rootGrid.Children[2] as FrameworkElement;
            var maskVisual = maskBorder.GetVisual();
            maskVisual.Opacity = 0f;
        }

        private void RootGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                return;
            }
            var rootGrid = sender as Grid;
            var codeBorder = rootGrid.Children[2] as FrameworkElement;
            var postBorder = rootGrid.Children[1] as FrameworkElement;
            var maskBorder = rootGrid.Children[0] as FrameworkElement;

            ToggleItemPointOverAnimation(maskBorder, codeBorder, postBorder, false);
        }


        private void RootGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                return;
            }
            var rootGrid = sender as Grid;
            var codeBorder = rootGrid.Children[2] as FrameworkElement;
            var postBorder = rootGrid.Children[1] as FrameworkElement;
            var maskBorder = rootGrid.Children[0] as FrameworkElement;

            ToggleItemPointOverAnimation(maskBorder, codeBorder, postBorder, true);
        }

        private const float SCALE_ANIMATION_FACTOR = 1.05f;

        private ScalarKeyFrameAnimation CreateScaleAnimation(bool show)
        {
            var scaleAnimation = _compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(0f, show ? SCALE_ANIMATION_FACTOR: 1f );
            scaleAnimation.InsertKeyFrame(1f, show ?  1f: SCALE_ANIMATION_FACTOR);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        private ScalarKeyFrameAnimation CreateFadeAnimation(bool show)
        {
            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, show ? 1f : 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(500);

            return fadeAnimation;
        }

        private void ToggleItemPointOverAnimation(FrameworkElement mask, FrameworkElement codeBorder, FrameworkElement postBorder, bool show)
        {
            var codeVisual = codeBorder.GetVisual();
            var postVisual = postBorder.GetVisual();

            var fadeAnimationCode = CreateFadeAnimation(show);
            var scaleAnimationCode = CreateScaleAnimation(show);
            var fadeAnimationPost = CreateFadeAnimation(!show);
            var scaleAnimationPost = CreateScaleAnimation(!show);

            if (postVisual.CenterPoint.X == 0 && postVisual.CenterPoint.Y == 0)
            {
                postVisual.CenterPoint= new Vector3((float)mask.ActualWidth / 2, (float)mask.ActualHeight / 2, 0f);
            }

            codeVisual.StartAnimation("Opacity", fadeAnimationCode);
            codeVisual.StartAnimation("Scale.X", scaleAnimationCode);
            codeVisual.StartAnimation("Scale.Y", scaleAnimationCode);
            postVisual.StartAnimation("Opacity", fadeAnimationPost);
            postVisual.StartAnimation("Scale.X", scaleAnimationPost);
            postVisual.StartAnimation("Scale.Y", scaleAnimationPost);
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }

        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void RootGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            var rootGrid = sender as Grid;
            rootGrid.PointerEntered -= RootGrid_PointerEntered;
            rootGrid.PointerExited -= RootGrid_PointerExited;
        }
    }
}
