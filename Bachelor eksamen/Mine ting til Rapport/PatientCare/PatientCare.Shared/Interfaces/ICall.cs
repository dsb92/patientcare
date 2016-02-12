using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Model;

namespace PatientCare.Shared.Interfaces
{
    public interface ICall
    {
        String MakeCall(CallEntity call);

        void UpdateCall(CallEntity call);

        string CreateJSONCall(CallEntity call);
    }
}
