using codeRetrievalApp.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace codeRetrievalApp.Lib
{
    class Constants
    {
        private static MainPage _mainPage;
        public static MainPage mainPage
        {
            get
            {
                return _mainPage;
            }
            set
            {
                _mainPage = value;
            }
        }
        public static Frame rootFrame;
        public static BoxControl KWbox;
    }
}
