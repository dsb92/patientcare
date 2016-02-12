using System;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;

namespace PatientCare.iOS
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
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = System.IO.Path.Combine(documentsPath, "../Library/");
                var path = System.IO.Path.Combine(libraryPath, "localcallsdb");

                return path;
        }

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(new SQLitePlatformIOS(), DatabaseFilePath());
        }
    }
}
