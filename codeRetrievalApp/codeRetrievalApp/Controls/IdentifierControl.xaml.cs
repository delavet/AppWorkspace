using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI;
using System.Numerics;
using System.Text.RegularExpressions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class IdentifierControl : UserControl
    {
        private VoidDelegate _selectDetect;
        private VoidDelegate _unselectDetect;
        private String[] DodgerBlue = { "const", "goto", "public", "protected", "private", "class", "interface", "abstract", "implements", "extends", "new",
        "import", "package","byte","char","boolean","short","int","float","long","double","void","if","else","while","for","switch","case","default","do","break",
        "contine","return","instanceof","static","final","super","this","native","strictfp","synchronized","transient","volatile","catch","try","finally","throw","throws","enum","assert"};
        private String[] LimeGreen = { "true", "false", "null" };
        private Boolean _isSpace = false;
        private Boolean _selected = false;
        private CodeT2Control _attached;
        private IDManageControl IDManage;

        public Boolean IsSpace
        {
            get
            {
                return _isSpace;
            }
        }

        public Boolean Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected)
                {
                    Select();
                }
                else
                {
                    UnSelect();
                }
            }
        }
        public String Identifier
        {
            get
            {
                return TXTBLKid.Text;
            }
            set
            {
                TXTBLKid.Text = value;
                SetUp();
            }
        }
        public IdentifierControl()
        {
            this.InitializeComponent();
        }

        public IdentifierControl(String id, VoidDelegate s = null, VoidDelegate u = null, CodeT2Control a = null)
        {
            this.InitializeComponent();
            Identifier = id;
            _selectDetect = s;
            _unselectDetect = u;
            _attached = a;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Style FlyoutStyle = new Style(typeof(FlyoutPresenter));
                Setter bgSetter = new Setter(FlyoutPresenter.BackgroundProperty, Application.Current.Resources["CustomAcrylicInAppBrush"]);
                Setter bdSetter = new Setter(FlyoutPresenter.BorderThicknessProperty, new Thickness(0));
                FlyoutStyle.Setters.Add(bgSetter);
                FlyoutStyle.Setters.Add(bdSetter);
                Flyout fly = new Flyout();
                fly.FlyoutPresenterStyle = FlyoutStyle;
                Grid rootGrid = new Grid();
                IDManage = new IDManageControl(Identifier, Rename, _attached.GetAppearances);
                rootGrid.Children.Add(IDManage);
                fly.Content = rootGrid;
                fly.Opened += Fly_Opened;
                FlyoutBase.SetAttachedFlyout(this, fly);
            }
            catch
            {
                return;
            }
        }

        private void Fly_Opened(object sender, object e)
        {
            if (IDManage!=null){
                IDManage.LoadDescription();
            }
        }

        private bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]+$";
            const string SixteenPattern = "^[0][xX][0-9a-fA-F]+$";
            Regex rx = new Regex(pattern);
            Regex rx16 = new Regex(SixteenPattern);
            var amatch = rx.IsMatch(s);
            var bmatch = rx16.IsMatch(s);
            return amatch || bmatch;
        }

        private void SetUp()
        {
            int i = 0;
            for (i = 0; i < Identifier.Length; i++)
            {
                if (Identifier[i] != ' ')
                {
                    break;
                }
            }
            if (i == Identifier.Length)
            {
                _isSpace = true;
                return;
            }
            if (Identifier.StartsWith("\"") && Identifier.EndsWith("\""))
            {
                //Salmon
                TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xfa, 0x80, 0x72));
            }
            else if (Identifier.StartsWith("//"))
            {
                //FirestGreen
                TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x22, 0x8b, 0x22));
            }
            else if (LimeGreen.Contains(Identifier))
            {
                //LimeGreen
                TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x32, 0xcd, 0x32));
            }
            else if (DodgerBlue.Contains(Identifier))
            {
                //DodgerBlue
                TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x1e, 0x90, 0xff));
            }
            else if (IsNumber(Identifier))
            {
                //Feldspar
                TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xd1, 0x92, 0x75));
            }
        }

        public void MakeComment()
        {
            TXTBLKid.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x22, 0x8b, 0x22));
        }

        private void BDid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //# FF72ACE3  #FFF7EED6
            BDid.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xdd, 0xdd, 0xdd));
        }

        private void BDid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            BDid.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xdd, 0xdd, 0xdd));
        }

        private void Select()
        {
            _selected = true;
            BDid.Background = new SolidColorBrush(Color.FromArgb(0xcf, 0xf7, 0xee, 0xd6));
            if (_selectDetect != null && !_isSpace)
                _selectDetect();
        }

        private void UnSelect()
        {
            _selected = false;
            BDid.Background = new SolidColorBrush(Color.FromArgb(0xcf, 0xff, 0xff, 0xff));
            if (_unselectDetect != null && !_isSpace)
                _unselectDetect();
        }

        private void BDid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isSpace) return;
            Selected = !Selected;
        }

        private void BDid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout(this);
        }

        private void Rename(String oldId, String newId, bool all)
        {
            if (all)
            {
                _attached.RequestAllReplacing(oldId, newId);
            }
            else
            {
                Identifier = newId;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            if (!((sender as Button).DataContext is RadioButton)) return;
            RadioButton radioButton = (sender as Button).DataContext as RadioButton;
            if (!(radioButton.DataContext is TextBox)) return;
            TextBox textBox = radioButton.DataContext as TextBox;
            if (radioButton.IsChecked == true)
            {
                if (textBox.Text == "") return;
                _attached.RequestAllReplacing(Identifier, textBox.Text);
            }
            else
            {
                if (textBox.Text == "") return;
                Identifier = textBox.Text;
            }
        }

        public void RequestAllReplacing(String oldId, String newId)
        {
            if (Identifier == oldId)
            {
                Identifier = newId;
            }
        }
    }
}
