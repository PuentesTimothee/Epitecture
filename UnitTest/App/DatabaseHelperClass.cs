using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace epitecture
{
    class DatabaseHelperClass
    {
        //Create Table   
        public void CreateDatabase(string DB_PATH)
        {
            if (!CheckFileExists(DB_PATH).Result)
            {
                using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), DB_PATH))
                {
                    conn.CreateTable<Picture>();

                }
            }
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        // Insert the new Picture in the Picture table.   
        public void Insert(Picture objPicture)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(objPicture);
                });
            }
        }
        // Retrieve the specific Picture from the database.     
        public Picture ReadPicture(int PictureId)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {
                var existingPicture = conn.Query<Picture>("select * from Picture where Id =" + PictureId).FirstOrDefault();
                return existingPicture;
            }
        }
        public ObservableCollection<Picture> ReadAllPicture()
        {
            try
            {
                using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
                {
                    List<Picture> myCollection = conn.Table<Picture>().ToList<Picture>();
                    ObservableCollection<Picture> PictureList = new ObservableCollection<Picture>(myCollection);
                    return PictureList;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }

        }
        //Update existing Picture  
        public void UpdateDetails(Picture ObjPicture)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {

                var existingPicture = conn.Query<Picture>("select * from Picture where Id =" + ObjPicture.Id).FirstOrDefault();
                if (existingPicture != null)
                {

                    conn.RunInTransaction(() =>
                    {
                        conn.Update(ObjPicture);
                    });
                }

            }
        }
        //Delete all contactlist or delete Picture table     
        public void DeleteAllPicture()
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {

                conn.DropTable<Picture>();
                conn.CreateTable<Picture>();
                conn.Dispose();
                conn.Close();

            }
        }
        //Delete specific Picture     
        public void DeletePicture(int Id)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {

                var existingconact = conn.Query<Picture>("select * from Picture where Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    conn.RunInTransaction(() =>
                    {
                        conn.Delete(existingconact);
                    });
                }
            }
        }
    }
} 

