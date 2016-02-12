using System;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using Newtonsoft.Json;
using PatientCare.Shared.Managers;

namespace PatientCare.Shared
{
    /// <summary>
    /// Klasse til håndtering af patient kald
    /// Som patient kan man oprette et kald ved at vælge en valgmulighed eller en service, som bliver udstillet til Patienten, når der logges ind.
    /// Herved gives der en årsag til kaldet f.eks. et glas vand.
    /// </summary>
    public class PatientCall : ICall
    {
        /// <summary>
        /// Opret et kald
        /// </summary>
        /// <param name="call">Kald objekt der indeholder properties for hvad kaldet skal indeholde</param>
        public String MakeCall(CallEntity call)
        {
            // Json repræsentation af et kald der sendes afsted
            //var jsonWorking = "{\"PatientCPR\" : \"123456-1234\", \"Category\" : \"TestTestTest\",\"Choice\" : null, \"Detail\" : null ,\"CreatedOn\" : \"onsdag, 28 oktober 15.27.31\",\"ModifiedOn\" : null,\"Status\" : 0}";
            call.CreatedOn = DateTime.Now.ToString("HH:mm:ss");

            var json = CreateJSONCall(call);

            var manager = new CallManager();
            manager.PostCall(json);

            return HttpHandler.MongoCallId;
        }

        /// <summary>
        /// Opdater et kald. Dette kan f.eks. være når en Patient fortryder et kald, hvorved status på kaldet ændres fra "Afventende" til "Fortrudt"
        /// </summary>
        /// <param name="call">Kald objekt der indeholder properties for hvad kaldet skal indeholde</param>
        public void UpdateCall(CallEntity call)
        {
            var json = "{\"_id\" : \""+call._id+"\", \"Status\" : "+call.Status+"}";

            var manager = new CallManager();
            manager.PutCall(json);
        }

        /// <summary>
        /// Laver kaldet om til JSON format, da det skal sendes som HTTP content.
        /// </summary>
        /// <param name="call">Kald objekt der indeholder properties for hvad kaldet skal indeholde</param>
        /// <returns>Returner et JSON repræsentation af et kald</returns>
        public string CreateJSONCall(CallEntity call)
        {
            return JsonConvert.SerializeObject(call);
        }
    }
}
