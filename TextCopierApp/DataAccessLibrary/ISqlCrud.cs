using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public interface ISqlCrud
    {
        void CreateTextItem(TextItemModel textItem);
        void DeleteTextItem(int textItemId);
        List<TextItemModel> ReadAllTextItems();
        void UpdateTextItem(TextItemModel textItem);
    }
}
