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
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Numerics;
using codeRetrievalApp.Lib;
using Windows.UI.Composition;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class BoxControl : UserControl
    {
        private Compositor _compositor;
        private Visual _detailContentGridVisual;
        private List<String> _kws = new List<string>();
        public List<String> KeyWords
        {
            get
            {
                return _kws;
            }
        }
        public BoxControl()
        {
            this.InitializeComponent();
        }

        private void InitComposition()
        {
            _detailContentGridVisual = GRIDroot.GetVisual();
        }

        public Vector2 GetTargetPosition()
        {
            var x = (Window.Current.Bounds.Width - 69);
            var y = (Window.Current.Bounds.Height - 69);
            return new Vector2((float)x, (float)y);
        }

        public void AddKeyWord(FrameworkElement _kwItem, String kw)
        { 
            _kws.Add(kw);
            var _kwItemVisual = _kwItem.GetVisual();
            _compositor = _kwItemVisual.Compositor;
            var targetSize = new Vector2(0, 0);
            var targetPosition = GetTargetPosition();
            var listItemSize = new Vector2((float)_kwItem.ActualWidth, (float)_kwItem.ActualHeight);
            var listItemCenterPosition = _kwItem.TransformToVisual(Window.Current.Content)
                                            .TransformPoint(new Point(_kwItem.ActualWidth / 2, _kwItem.ActualHeight / 2));
            var offsetXAbs = (targetPosition.X + targetSize.X / 2) - (float)listItemCenterPosition.X ;
            var offsetYAbs = (targetPosition.Y + targetSize.Y / 2) - (float)listItemCenterPosition.Y;
            var startX = 0f;
            var startY = 0f;
            var endX = offsetXAbs;
            var endY = offsetYAbs;
            var startScale = 1f;
            var endScale = 0f;
            var scaleAnim = _compositor.CreateVector3KeyFrameAnimation();
            scaleAnim.Duration = TimeSpan.FromMilliseconds(400);
            scaleAnim.InsertKeyFrame(0f, new Vector3(startScale, startScale, 1f));
            scaleAnim.InsertKeyFrame(1f, new Vector3(endScale, endScale, 1f));
            _kwItemVisual.StartAnimation("Scale", scaleAnim);
            var offsetAnim = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim.Duration = TimeSpan.FromMilliseconds(400);
            offsetAnim.InsertKeyFrame(0f, new Vector3(startX, startY, 0f));
            offsetAnim.InsertKeyFrame(1f, new Vector3(endX, endY, 0f));
            _kwItemVisual.StartAnimation("Translation", offsetAnim);
        }

        public void BoxClear()
        {
            _kws.Clear();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = new Style(typeof(FlyoutPresenter));
            WrapPanel tempPanel = new WrapPanel();
            tempPanel.Orientation = Orientation.Horizontal;
            foreach(var kw in _kws)
            {
                AssociateT2Control t = new AssociateT2Control(kw, false);
                t.Margin = new Thickness(5);
                tempPanel.Children.Add(t);
            }
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(this, fly);
            Flyout.ShowAttachedFlyout(this);
        }

        private void GRIDroot_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            STRBDpopin.Begin();
        }

        private void GRIDroot_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            STRBDpopout.Begin();
        }
    }
}
