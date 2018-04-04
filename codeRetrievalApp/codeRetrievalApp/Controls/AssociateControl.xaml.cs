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
using codeRetrievalApp.Lib;
using Windows.UI.Composition;
using System.Threading.Tasks;
using System.Numerics;
using Windows.Data.Json;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class AssociateControl : UserControl
    {
        public bool IsShown { get; set; }
        private Compositor _compositor;
        private Visual _detailContentGridVisual;
        private FrameworkElement _kwItem;
        private Visual _kwItemVisual;

        public AssociateControl()
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
            TXTBLKkw.Text = Keyword;
            _kwItem = KeywordItem;
            _kwItemVisual = _kwItem.GetVisual();
            Visibility = Visibility.Visible;
            await ToggleKwItemAnimationAsync(true);
            PRGRS.ProgressStart();
            await InnerShow(Keyword);
            PRGRS.ProgressEnd();
        }


        private async Task InnerShow(String k)
        {
            String json = "{\"keyword\":\"" + k + "\"}";
            var param = await WebConnection.Connect_by_json("http://127.0.0.1:8000/asso", json);
            if (!param.name.Equals("200")) return;
            JsonObject jsonObject = JsonObject.Parse(param.value);
            String message = jsonObject.GetNamedString("message");
            if (!message.Equals("success")) return;
            JsonArray array = jsonObject.GetNamedArray("result");
            foreach(var a in array)
            {
                String assoWord = a.GetString();
                Button btn = new Button();
                btn.Content = assoWord;
                btn.Click += Button_Click;
                btn.Margin = new Thickness(5);
                WPPNasso.Children.Add(btn);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            _kwItemVisual.Opacity = 0f;

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


        private async void BTNcancel_Click(object sender, RoutedEventArgs e)
        {
            WPPNasso.Children.Clear();
            await DoHideListAsync();
            
        }
    }
}
