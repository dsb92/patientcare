using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;

namespace PatientCare.Android
{
    [Activity(Label = "DetailChoiceActivity")]
    public class DetailChoiceActivity : Activity
    {
        private List<String> detailList;
        private ListView listView;
        private String category;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_detailchoice);
            // get intent data
            Bundle b = Intent.Extras;
            // Set Activity Title with the selected choice
            this.Title = b.GetString("choice");
            category = b.GetString("category");
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
            var detail = detailList[e.Position];

            CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, category, Title, detail);
            Call.MakeCall(callEntity, this);
        }
    }
}