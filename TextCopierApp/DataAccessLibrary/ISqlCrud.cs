using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public interface ISqlCrud
    {
        void CreateTextItem(TextItemModel textItem);
        List<TextItemModel> ReadAllTextItemsOrderedBySortPosition();
        List<TextItemModel> ReadAllTextItemsOrderedByDescription();
        void UpdateTextItem(TextItemModel textItem);
        void DeleteTextItem(int textItemId);
    }
}
