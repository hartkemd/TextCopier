using DataAccessLibrary.Models;

namespace TextCopierLibrary
{
    public static class HelperMethods
    {
        public static void SwapTextItemSortPositions(TextItemModel textItemModel1, TextItemModel textItemModel2)
        {
            int tempSortPosition;

            tempSortPosition = textItemModel1.SortPosition;
            textItemModel1.SortPosition = textItemModel2.SortPosition;
            textItemModel2.SortPosition = tempSortPosition;
        }
    }
}
