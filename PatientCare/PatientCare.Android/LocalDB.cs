
using System.IO;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

namespace PatientCare.Android
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
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }

        public SQLiteConnection CreateConnection()
        {
            //return new SQLiteConnection(new SQLitePlatformIOS(), DatabaseFilePath());
            return new SQLiteConnection(new SQLitePlatformAndroid(), DatabaseFilePath());

        }

    }
}
