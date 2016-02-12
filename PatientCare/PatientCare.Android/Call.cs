using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;

namespace PatientCare.Android
{
	public static class Call
	{
        public static List<CallEntity> CallEntities = DataHandler.LoadCallsFromLocalDatabase(new LocalDB()).ToList();
        private static ProgressDialog dialog;
	    public static void MakeCall(CallEntity callEntity, Activity activity)
	    {
            var category = callEntity.Category;
            var choice = callEntity.Choice ?? "";
            var detail = callEntity.Detail ?? "";

	        new AlertDialog.Builder(activity)
	            .SetTitle(category + " " + choice + " " + detail)
	            .SetMessage(Strings.CallSendMessage)
	            .SetPositiveButton(Strings.CallSend, delegate
	            {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        activity.RunOnUiThread(() =>
                        {
                            dialog = new ProgressDialog(activity);
                            dialog.SetMessage(Strings.SpinnerDataSending);
                            dialog.SetCancelable(false);
                            dialog.Show();
                        });

                        try
                        {
                            ICall patientCall = new PatientCall();
                            // Assign the callid with the returned MongoDB id
                            callEntity._id = patientCall.MakeCall(callEntity);

                            activity.RunOnUiThread(() =>
                            {
                                // Call successfull, take the user to myCalls
                                activity.ActionBar.SelectTab(activity.ActionBar.GetTabAt(1));
                                SetNewCalls(callEntity);
                                dialog.Hide();
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR making call: " + ex.Message);

                            activity.RunOnUiThread(() =>
                            {
                                dialog.Hide();

                                new AlertDialog.Builder(activity).SetTitle(Strings.Error)
                                    .SetMessage(Strings.ErrorSendingCall)
                                    .SetPositiveButton(Strings.OK,
                                        delegate { }).Show();
                            });
                        }
              
                    });
             
	            })
            .SetNegativeButton(Strings.Cancel, delegate {/* Do nothing */ })
            .Show();
	    }

	    public static void SetNewCalls(CallEntity callEntity)
	    {
            // Load calls from database
            CallEntities = DataHandler.LoadCallsFromLocalDatabase(new LocalDB()).ToList();

            if (CallEntities == null || CallEntities.Count == 0)
            {
                CallEntities = new List<CallEntity>();
            }

            // If we are making a new call
            if (callEntity != null)
            {
                // And if the call is the same as before
                if (CallEntities.Any(call => call._id == callEntity._id))
                {
                    //CallEntities.Remove(callEntity);
                    callEntity = null;
                }
            }

            // If the call is not the same as before, e.g. if we are just updating the call
            if (callEntity != null)
            {
                CallEntities.Add(callEntity);

                DataHandler.SaveCallsToLocalDatabase(new LocalDB(), CallEntities.ToArray());
            }
	    }
	}
}