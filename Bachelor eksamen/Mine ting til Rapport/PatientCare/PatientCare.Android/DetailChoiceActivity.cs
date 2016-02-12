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

namespace PatientCare.Android
{
    [Activity(Label = "DetailChoiceActivity")]
    public class DetailChoiceActivity : Activity
    {
        private List<String> detailList;
        private ListView listView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_detailchoice);
            // get intent data
            Bundle b = Intent.Extras;
            // Set Activity Title with the selected category
            this.Title = b.GetString("choice");
            detailList = b.GetStringArray("details").ToList();

            // Change title of Activity with selected choice
            this.Title = Intent.GetStringExtra("choice") ?? "Not available";

            ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                 this,
                 global::Android.Resource.Layout.SimpleListItem1,
                 detailList);

            listView = (ListView)FindViewById<ListView>(Resource.Id.lvDetailChoices);
            listView.Adapter = arrayAdapter;
            listView.ItemClick += OnListItemClick;
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            AlertDialog.Builder alertConfirm = new AlertDialog.Builder(this);

            alertConfirm.SetTitle("Bekræft");
            alertConfirm.SetMessage(detailList[e.Position]);

            alertConfirm.SetPositiveButton("OK", (confirmAlert, confirmArgs) =>
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    // Make the async patient call here
                    //ICall patientCall = new PatientCall();
                    //patientCall.Call("", "");

                    RunOnUiThread(() =>
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Anmodning sendt");

                        alert.SetPositiveButton("OK", (senderAlert, senderArgs) =>
                        {
                            Finish();
                        });

                        alert.Show();
                    });

                });
                
            });

            alertConfirm.SetNegativeButton("Annullér", (senderAlert, args) =>
            {
                
            });

            RunOnUiThread(() =>
            {
                alertConfirm.Show();
            });
        }
    }
}