using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class LeadControl : UserControl
    {
        public bool IsShown { get; set; }
        private Compositor _compositor;
        private Visual _detailContentGridVisual;
        private FrameworkElement _kwItem = null;
        private Visual _kwItemVisual = null;

        public LeadControl()
        {
            InitializeComponent();
            InitComposition();
            IsShown = false;
            Visibility = Visibility.Collapsed;
        }

        private void InitComposition()
        {
            _compositor = this.GetVisual().Compositor;
            _detailContentGridVisual = DetailContentGrid.GetVisual();
        }

        public async void Show(FrameworkElement KeywordItem, String Keyword)
        {
            _kwItem = KeywordItem;
            _kwItemVisual = _kwItem.GetVisual();
            Visibility = Visibility.Visible;
            await ToggleKwItemAnimationAsync(true);
        }

        public Vector2 GetTargetSize()
        {
            var windowWidth = Window.Current.Bounds.Width;
            var windowHeight = Window.Current.Bounds.Height;

            var height = Math.Min(windowWidth * (2f / 3f), windowHeight - 200);
            var width = 1.5f * height;


            return new Vector2((float)width, (float)height);
        }

        public Vector2 GetTargetPosition()
        {
            var size = GetTargetSize();
            var x = (Window.Current.Bounds.Width - size.X) / 2;
            var y = (Window.Current.Bounds.Height - size.Y) / 2;
            return new Vector2((float)x, (float)y);
        }

        private bool _detailContentGirdSizeSpecified = false;

        private void UpdateDetailContentGridSize()
        {
            var size = GetTargetSize();
            DetailContentGrid.Width = size.X;
            DetailContentGrid.Height = size.Y;

            _detailContentGirdSizeSpecified = true;
        }

        private async Task ToggleKwItemAnimationAsync(bool show)
        {

            var targetSize = GetTargetSize();
            var targetPosition = GetTargetPosition();

            this.Visibility = Visibility.Visible;

            if (show && !_detailContentGirdSizeSpecified)
            {
                UpdateDetailContentGridSize();
                await DetailContentGrid.WaitForSizeChangedAsync();
            }

            var listItemSize = new Vector2((float)_kwItem.ActualWidth, (float)_kwItem.ActualHeight);
            var listItemCenterPosition = _kwItem.TransformToVisual(Window.Current.Content)
                                            .TransformPoint(new Point(_kwItem.ActualWidth / 2, _kwItem.ActualHeight / 2));

            var offsetXAbs = (float)listItemCenterPosition.X - (targetPosition.X + targetSize.X / 2);
            var offsetYAbs = (float)listItemCenterPosition.Y - (targetPosition.Y + targetSize.Y / 2);

            var startX = show ? offsetXAbs : _detailContentGridVisual.GetTranslation().X;
            var startY = show ? offsetYAbs : _detailContentGridVisual.GetTranslation().Y;

            var endX = show ? 0f : offsetXAbs;
            var endY = show ? 0f : offsetYAbs;

            var startScale = show ? listItemSize.X / targetSize.X : 1f;
            var endScale = show ? 1f : listItemSize.X / targetSize.X;

            _detailContentGridVisual.Opacity = 1f;
            _detailContentGridVisual.CenterPoint = new Vector3(targetSize.X / 2f, targetSize.Y / 2f, 1f);

            var scaleAnim = _compositor.CreateVector3KeyFrameAnimation();
            scaleAnim.Duration = TimeSpan.FromMilliseconds(400);
            scaleAnim.InsertKeyFrame(0f, new Vector3(startScale, startScale, 1f));
            scaleAnim.InsertKeyFrame(1f, new Vector3(endScale, endScale, 1f));
            _detailContentGridVisual.StartAnimation("Scale", scaleAnim);

            var offsetAnim = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim.Duration = TimeSpan.FromMilliseconds(400);
            offsetAnim.InsertKeyFrame(0f, new Vector3(startX, startY, 0f));
            offsetAnim.InsertKeyFrame(1f, new Vector3(endX, endY, 0f));
            _detailContentGridVisual.StartAnimation("Translation", offsetAnim);
        }


        private async Task DoHideListAsync()
        {
            var innerBatch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            await ToggleKwItemAnimationAsync(false);
            innerBatch.Completed += (ss, exx) =>
            {
                if (_kwItem != null)
                {
                    _kwItem.GetVisual().Opacity = 1f;
                    _kwItem = null;
                }
                this.Visibility = Visibility.Collapsed;
            };
            innerBatch.End();
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FLPVW.SelectedIndex)
            {
                case 0:
                    {
                        T2.lead();
                        KW1.quitLead1();
                    }break;
                case 1:
                    {
                        T2.quitLead();
                        KW1.lead1();
                    }break;
                case 2:
                    {
                        T2.quitLead();
                        KW1.quitLead1();
                        KW2.lead2();
                    }break;
                default:
                    break;
            }
        }

        private async void BTNcancel_Click(object sender, RoutedEventArgs e)
        {
            await DoHideListAsync();
        }

        private void T2_ShowAssociates(FrameworkElement kwItem, string keyword)
        {

        }

        private void KW1_KeywordComplete()
        {

        }
    }
}
