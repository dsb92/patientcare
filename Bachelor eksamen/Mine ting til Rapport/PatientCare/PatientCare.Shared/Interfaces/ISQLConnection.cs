using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;

namespace PatientCare.Shared.Interfaces
{
    public interface ISQLConnection
    {
        string DatabaseFilePath();

        SQLiteConnection CreateConnection();
    }
}
