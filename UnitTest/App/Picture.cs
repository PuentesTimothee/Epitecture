using System;
using Windows.UI;

namespace epitecture
{
    sealed public class Picture
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int Id { get; set; }

        public string UploadedId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Favorite { get; set; }
        public Color GetFavoriteColor { get => ((Favorite) ? (Color.FromArgb(150, 200, 175, 75)) : (Color.FromArgb(150, 150, 150, 150))); }

        public Picture() {}
    }
}
