using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCare.Shared
{
    public static class Strings
    {
        public const string StatusActive = "Afventende";
        public const string StatusCompleted = "Udført";
        public const string StatusCanceled = "Fortrudt";

        public const string CallSend = "Send";
        public const string CallCancel = "Annullér";
        public const string CallCreated = "Oprettet";
        public const string CallSent = "Forespørgsel sendt";
        public const string CallAlreadySent = "Forespørgsel er allerede sendt og ventende";
        public const string CallSendMessage = "Send forespørgsel";
        public const string CallRegretTitle = "Fortryd";
        public const string CallRegretMessage = "Fortryd denne forespørgsel";
        public const string CallRegretted = "Forespørgsel fortrudt";

        public const string Cancel = "Annullér";
        public const string Confirm = "Bekræft";
        public const string OK = "OK";

        public const string LogOff = "Log ud";
        public const string LogIn = "Log ind";
        public const string LoginAs = "Du er logget ind som";

        public const string Error = "Fejl";
        public const string ErrorSendingCall = "Der skete en fejl ved forespørgsel";
        public const string ErrorReading = "Der skete en fejl ved indlæsning af data";
        public const string ErrorNoCategories = "Ingen kategorier at indlæse";
        public const string ErrorPatientNotValid = "Patient ikke indlagt";
        public const string ErrorLogin = "Login fejl";
        public const string ErrorNoNetwork = "Der skete en fejl, venligst tjek din netværksforbindelse";

        public const string SpinnerDataReading = "Indlæser Data...";
        public const string SpinnerDataSending = "Sender forespørgsel...";
    }
}
