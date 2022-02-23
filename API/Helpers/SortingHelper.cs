using System.Collections.Generic;

namespace API.Helpers
{
    public static class SortingHelper
    {
        public static KeyValuePair<string, string>[] GetSortFields()
        {
            return new[]
            {
                SortFields.Title, SortFields.CreationDate, SortFields.Cathegory, SortFields.IsPromoted
            };
        }
    }

    public static class SortFields
    {
        public static KeyValuePair<string, string> Title { get; set; } = new KeyValuePair<string, string>("title", "Title");
        public static KeyValuePair<string, string> CreationDate { get; set; } = new KeyValuePair<string, string>("creationdate", "Created");
        public static KeyValuePair<string, string> Cathegory { get; set; } = new KeyValuePair<string, string>("cathegory", "Cathegory");
        public static KeyValuePair<string, string> IsPromoted { get; set; } = new KeyValuePair<string, string>("ispromoted", "IsPromoted");
    }
}