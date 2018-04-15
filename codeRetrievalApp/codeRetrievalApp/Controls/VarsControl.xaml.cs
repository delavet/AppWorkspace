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
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class VarsControl : UserControl
    {
        public event ChangeVarHandler ChangeVar;
        private List<String> vs;
        private List<TextBox> boxList = new List<TextBox>();
        public VarsControl()
        {
            this.InitializeComponent();
        }

        public void Init(List<String> vars)
        {
            vs = vars;
            boxList.Clear();
            STKPNvars.Children.Clear();
            foreach(var v in vars)
            {
                Border bd = new Border();
                bd.BorderBrush = new SolidColorBrush(Color.FromArgb(0XFF, 0X11, 0X11, 0X11));
                bd.BorderThickness = new Thickness(0.3);
                bd.CornerRadius = new CornerRadius(5);
                Grid pn = new Grid();
                pn.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                pn.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                pn.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                TextBlock txt1 = new TextBlock();
                txt1.Text = v;
                txt1.Margin = new Thickness(3);
                txt1.FontSize = 22;
                txt1.VerticalAlignment = VerticalAlignment.Center;
                txt1.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(txt1, 0);
                TextBlock txt2 = new TextBlock();
                txt2.Text = "change to";
                txt2.Margin = new Thickness(3);
                txt2.FontSize = 18;
                txt2.Foreground = new SolidColorBrush(Color.FromArgb(0XFF, 0X55, 0X55, 0X55));
                txt2.VerticalAlignment = VerticalAlignment.Center;
                txt2.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumn(txt2, 1);
                TextBox txt3 = new TextBox();
                txt3.PlaceholderText = "input label name";
                txt3.FontSize = 22;
                txt3.VerticalAlignment = VerticalAlignment.Center;
                txt3.HorizontalAlignment = HorizontalAlignment.Right;
                txt3.Margin = new Thickness(3);
                txt3.Style = (Style)Application.Current.Resources["MyTextBoxStyle"];
                Grid.SetColumn(txt3, 2);
                pn.Children.Add(txt1);
                pn.Children.Add(txt2);
                pn.Children.Add(txt3);
                pn.HorizontalAlignment = HorizontalAlignment.Center;
                pn.VerticalAlignment = VerticalAlignment.Center;
                bd.Child = pn;
                STKPNvars.Children.Add(bd);
                boxList.Add(txt3);
            }
        }

        public void Show(List<String> vars)
        {
            GRIDroot.Visibility = Visibility.Visible;
            STRBDpopin.Begin();
            Init(vars);
        }

        private void BTNconfirm_Click(object sender, RoutedEventArgs e)
        {
            STRBDpopout.Begin();
            List<Parameters> pass = new List<Parameters>();
            int i = 0;
            foreach(var box in boxList)
            {
                if (box.Text != "" && box.Text != null)
                {
                    pass.Add(new Parameters(vs[i], box.Text));
                }
                i++;
            }
            ChangeVar(pass);
        }

        private void STRBDpopout_Completed(object sender, object e)
        {
            GRIDroot.Visibility = Visibility.Collapsed;
        }
    }
}
