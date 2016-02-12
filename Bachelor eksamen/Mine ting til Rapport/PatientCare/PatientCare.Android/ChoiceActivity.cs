using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PatientCare.Shared;
using PatientCare.Shared.Model;

namespace PatientCare.Android
{
    [Activity(Label = "ChoiceActivity")]
    public class ChoiceActivity : Activity
    {
        private List<String> choiceList;
        private List<String> detailList; 
        private ListView listView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_choice);
            // get intent data
            //var intent = Intent.GetStringExtra("categoryname") ?? "Not available";

            Bundle b = Intent.Extras;
            // Set Activity Title with the selected category
            Title = b.GetString("category");
            choiceList = b.GetStringArray("choices").ToList();
            detailList = b.GetStringArray("details").ToList();

            ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                 this,
                 global::Android.Resource.Layout.SimpleListItem1,
                 choiceList);

            listView = (ListView) FindViewById<ListView>(Resource.Id.lvChoices);
            listView.Adapter = arrayAdapter;
            listView.ItemClick += OnListItemClick;
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var choice = choiceList[e.Position];
            var intent = new Intent(Application.Context, typeof(DetailChoiceActivity));

            // Hvis der er ikke er nogen detailer, start kaldet her
            if (choice == null || details.Count == 0)
            {
                CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, category, choice);
                AppDelegate.MakeCall(callEntity, choice.Name, vc);
            }


            Bundle bundle = new Bundle();
            bundle.PutStringArray("details", detailList.ToArray());
            bundle.PutString("choice", choice);
            intent.PutExtras(bundle);
            StartActivity(intent);
        }
    }
}