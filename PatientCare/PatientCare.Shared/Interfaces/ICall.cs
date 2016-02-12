using System;
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
