using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;

namespace PatientCare.Android
{
    [Activity(Label = "CallOverviewActivity")]
    public class CallOverviewFragment : Fragment
    {
        private ListView listView;
        private List<string> callList;
        private ArrayAdapter<String> arrayAdapter;
        public override View OnCreateView(LayoutInflater inflater,
        ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(
                Resource.Layout.activity_call_overview, container, false);

            // generate list
            GenerateList();

            arrayAdapter = new ArrayAdapter<String>(
                 view.Context,
                 global::Android.Resource.Layout.SimpleListItem1,
                 callList);

            listView = (ListView)view.FindViewById<ListView>(Resource.Id.lvMyCalls);
            listView.Adapter = arrayAdapter;
            listView.ItemClick += OnListItemClick;

            return view;
        }

        private void GenerateList()
        {
            callList = new List<string>();

            foreach (var call in Call.CallEntities)
            {
                // Top label
                var category = call.Category;
                var choice = call.Choice ?? "";
                var detail = call.Detail ?? "";
                var topLabel = category + " " + choice + " " + detail;

                // Bottom label
                var status = call.Status;
                var timeStamp = call.CreatedOn;
                var bottomLabel = "";
                // Detail Text, TimeStamp, and Badge number value
                switch (status)
                {
                    case (int)CallUtil.StatusCode.Active:
                        bottomLabel = Strings.StatusActive + "\t" + Strings.CallCreated + " " + timeStamp;
                        break;

                    case (int)CallUtil.StatusCode.Completed:
                        bottomLabel = Strings.StatusCompleted + "\t" + Strings.CallCreated + " " + timeStamp;
                        break;

                    case (int)CallUtil.StatusCode.Canceled:
                        bottomLabel = Strings.StatusCanceled + "\t" + Strings.CallCreated + " " + timeStamp;
                        break;
                }

                callList.Add(topLabel + "\n" + bottomLabel);
            }
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var callEntity = Call.CallEntities[e.Position];

            // Do not allow user to tap on the cell if call is canceled or completed
            // return if so
            if (callEntity.Status == (int)CallUtil.StatusCode.Canceled || callEntity.Status == (int)CallUtil.StatusCode.Completed) return;

            new AlertDialog.Builder(this.Activity)
                .SetTitle(Strings.CallRegretTitle)
                .SetMessage(Strings.CallRegretMessage)
                .SetPositiveButton(Strings.CallSend, delegate
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        // Update status
                        callEntity.Status = (int)CallUtil.StatusCode.Canceled;

                        try
                        {
                            // Put the async patient call here
                            ICall patientCall = new PatientCall();
                            patientCall.UpdateCall(callEntity);

                            this.Activity.RunOnUiThread(() =>
                            {
                                new AlertDialog.Builder(this.Activity).SetTitle(Strings.CallRegretted)
                                    .SetPositiveButton("OK", delegate { })
                                    .Show();

                                DataHandler.UpdateMyCallToLocalDatabase(new LocalDB(), callEntity);
                                Call.SetNewCalls(callEntity);
                                GenerateList();
                                arrayAdapter.NotifyDataSetChanged();
                            });
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        

                    });

                })
            .SetNegativeButton(Strings.Cancel, delegate {/* Do nothing */ })
            .Show();
        }
    }
}