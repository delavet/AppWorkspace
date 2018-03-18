using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace codeRetrievalApp.Lib
{
    delegate void DataLoadingEventHandler();
    delegate void DataLoadedEventHandler();
    public delegate void AssociateKeyWordsHandler(String keyword);
    public delegate void KeywordInputCompleteHandler();
}