using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using PatientCare.Shared;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;

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
            
            // Hvis der er ikke er nogen detailer, start kaldet her
            if (detailList == null || detailList.Count == 0)
            {
                CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, Title, choice);
                Call.MakeCall(callEntity, this);
            }
            else
            {
                Bundle bundle = new Bundle();
                bundle.PutStringArray("details", detailList.ToArray());
                bundle.PutString("choice", choice);
                bundle.PutString("category", Title);

                var intent = new Intent(Application.Context, typeof(DetailChoiceActivity));
                intent.PutExtras(bundle);
                StartActivity(intent);
            }
        }
    }
}