using System.IO;
using Windows.Storage;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace PatientCare.Windows
{
    public class LocalDB : DAL, ISQLConnection
    {
        public LocalDB()
        {
            // Make the SQL server connection
            db = CreateConnection();
        }

        public string DatabaseFilePath()
        {
            var sqliteFilename = "localservicesDb";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            return path;
        }

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), DatabaseFilePath());
        }

    }
}
