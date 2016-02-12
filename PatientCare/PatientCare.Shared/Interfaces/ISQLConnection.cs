using SQLite.Net;

namespace PatientCare.Shared.Interfaces
{
    public interface ISQLConnection
    {
        string DatabaseFilePath();

        SQLiteConnection CreateConnection();
    }
}
