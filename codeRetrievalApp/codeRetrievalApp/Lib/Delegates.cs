using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace codeRetrievalApp.Lib
{
    delegate void DataLoadingEventHandler();
    delegate void DataLoadedEventHandler();
    public delegate void AssociateKeyWordsHandler(FrameworkElement kwItem,String keyword);
    public delegate void KeywordInputCompleteHandler();
    public delegate void ChangeVarHandler(List<Parameters> list);
    public delegate void SearchHandler(List<String> keywords);
}