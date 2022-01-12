using System;

namespace DataAccessLibrary.Models
{
    public class TextItemModel : IComparable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is TextItemModel otherTextItem)
                return this.Description.CompareTo(otherTextItem.Description);
            else
                throw new ArgumentException("Object is not a text item");
        }
    }
}
