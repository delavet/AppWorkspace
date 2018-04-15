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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    
    public sealed partial class CodeLineControl : UserControl
    {
        public String CodeLine
        {
            get
            {
                return TXTBLKline.Text;
            }
            set
            {
                TXTBLKline.Text = value;
            }
        }

        private Boolean _selected = false;
        public Boolean Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected == false)
                {
                    SCALEline.CenterX = GRIDline.ActualWidth/2;
                    SCALEline.CenterY = GRIDline.ActualHeight / 2;
                    SCALEline.ScaleX = 1;
                    SCALEline.ScaleY = 1;
                }
                else
                {
                    SCALEline.CenterX = GRIDline.ActualWidth / 2;
                    SCALEline.CenterY = GRIDline.ActualHeight / 2;
                    SCALEline.ScaleX = 1.05;
                    SCALEline.ScaleY = 1.05;
                }
            }
        }

        public CodeLineControl()
        {
            this.InitializeComponent();
        }

        public CodeLineControl(String line)
        {
            this.InitializeComponent();
            CodeLine = line;
        }

         
    }
}
