using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace codeRetrievalApp.Lib
{
    class CodeInfo : INotifyPropertyChanged
    {
        private int id;
        private string _code;
        private string _post;
        private string _title;
        
        public CodeInfo(String c, String p, String t, int i)
        {
            id = i;
            code = c;
            post = p;
            title = t;
        }
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public String code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }
        public String post
        {
            get
            {
                return _post;
            }
            set
            {
                _post = value;
                OnPropertyChanged();
            }
        }
        public String title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
