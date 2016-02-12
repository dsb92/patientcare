using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.App;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Model.Utils;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

namespace PatientCare.Android
{
    public class LocalDB : SharedLocalDB, ISQLConnection
    {
        public LocalDB()
        {
            // Make the SQL server connection
            db = CreateConnection();
        }

        public string DatabaseFilePath()
        {
            //data/data/<Your-Application-Package-Name>/databases/<your-database-name>
            //var dbName = "localservicesDb";
            //var packageName = Application.Context.PackageName;
            //var filePath = "//data/data/<" + packageName + ">/databases/<" + dbName + ">";

            //var filePath = Application.Context.GetDatabasePath(dbName);
            
            //return (string) filePath;

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
