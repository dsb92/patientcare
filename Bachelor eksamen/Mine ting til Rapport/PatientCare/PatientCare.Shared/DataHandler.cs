using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.DAL;
using PatientCare.Shared.Model;

namespace PatientCare.Shared
{
    /// <summary>
    /// Klasse til håndtering af alt data til og fra WEB og PatientApp's egen lokale SQL database
    /// </summary>
    public static class DataHandler
    {
        /// <summary>
        /// Indlæser alle Categorier fra Web og tilknyttet Typer og Tilbehør, tilknyttet en afdeling på et sygehus.
        /// </summary>
        /// <returns>Returner et array med Categorier, tilknyttet Choices og Details</returns>
        public static CategoryEntity[] LoadCategoriesFromWeb()
        {
            var categoryManager = new CategoryManager();

            return categoryManager.GetCategoriesWithAll().ToArray();

        }

        public static CategoryEntity[] LoadCategoriesTESTDATA()
        {
            var categoryManager = new CategoryManager();

            return categoryManager.GetCategoriesTESTDATA().ToArray();

        }

        /// <summary>
        /// Indlæser et kald fra Web en Patient har foretaget og returner dens status
        /// </summary>
        /// <returns>Returnere en string med en status: "0" = Active, "1" = Completed, "2" = Canceled</returns>
        private static String GetStatusCall(CallEntity callEntity)
        {
            CallManager callManager = new CallManager();

            return callManager.GetStatusCall(callEntity);
        }

        /// <summary>
        /// Opdater alle kaldenes statuser ud fra en given liste 
        /// </summary>
        /// <param name="calls"></param>
        /// <param name="callEntity"></param>
        /// <returns>En opdateret liste af kald hvor hver deres status er opdateret</returns>
        public static CallEntity[] GetUpdatedStatusForAllCalls(List<CallEntity> calls, CallEntity callEntity)
        {
            var newCallStatus = GetStatusCall(callEntity);
            callEntity.Status = Int32.Parse(newCallStatus);
            var oldCall = calls.First(s => s._id == callEntity._id);

            // Hvis status på kaldet har ændret sig
            if (oldCall.Status != callEntity.Status)
            {
                // Fjern det gamle kald fra listen
                calls.Remove(oldCall);
                // Tilføj det nye kald til listen
                calls.Add(callEntity);
            }

            return calls.ToArray();
        }

        /// <summary>
        /// Gemmer alle Categories, tilknyttet Choices og Details til PatientApp'ens egen lokale SQL database
        /// </summary>
        /// <param name="db">Den delte database PCL</param>
        /// <param name="categories">Et array med Categories, tilknyttet Choices og Details som skal gemmes</param>
        public static void SaveCategoriesToLocalDatabase(SharedLocalDB db, CategoryEntity[] categories)
        {

            if (categories != null)
            {
                db.SaveCategories(categories.ToList());
            }

        }

        /// <summary>
        /// Gemmer alle foretaget kald til PatientApp'ens egen lokale SQL database
        /// </summary>
        /// <param name="db">Den delte database PCL</param>
        /// <param name="calls">Et array med alle kald</param>
        public static void SaveCallsToLocalDatabase(SharedLocalDB db, CallEntity[] calls)
        {
            if (calls != null)
            {
                db.SaveMyCalls(calls.ToList());
            } 
        }

        /// <summary>
        /// Opdater et kald
        /// </summary>
        /// <param name="db">Den delte database PCL</param>
        /// <param name="callEntity">Kaldet</param>
        public static void UpdateMyCallToLocalDatabase(SharedLocalDB db, CallEntity callEntity)
        {
            if (callEntity != null)
            {
                db.UpdateMyCall(callEntity);
            }
        }

        /// <summary>
        /// Indlæser alle Categories, tilknyttet Choices og Details fra PatientApp'ens egen lokale SQL database
        /// </summary>
        /// <param name="db">Den delte database PCL</param>
        /// <returns>Returner et array med Categories, tilknyttet Choices og Details</returns>
        public static CategoryEntity[] LoadCategoriesFromLocalDatabase(SharedLocalDB db)
        {
            return db.LoadCategories().ToArray();
        }

        /// <summary>
        /// Indlæser alle kald fra PatientApp'ens egen lokale SQL database
        /// </summary>
        /// <param name="db">Den delte database PCL</param>
        /// <returns></returns>
        public static CallEntity[] LoadCallsFromLocalDatabase(SharedLocalDB db)
        {
            return db.LoadMyCalls().ToArray();
        }

        public static void DeleteCategoriesFromLocalDatabase(SharedLocalDB db)
        {
            db.DeleteCategories();
        }
    }
}
