using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PatientCare.Android.Resources.src;
using PatientCare.Shared;
using PatientCare.Shared.Model;

namespace PatientCare.Android
{
    public class CategoryFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater,
        ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(
                Resource.Layout.activity_category, container, false);

            GridView gridView = (GridView)view.FindViewById(Resource.Id.grid_view);
            // Instance of ImageAdapter Class

            var categories = DataHandler.LoadCategoriesFromLocalDatabase(new LocalDB());

            gridView.SetAdapter(new CategoryAdapter(view.Context, categories));

            gridView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                Intent intent = new Intent(Application.Context, typeof(ChoiceActivity));

                List<string> choiceList = new List<string>();
                List<string> detailList = new List<string>();

                var category = categories[args.Position];
                var choices = categories[args.Position].Choices;

                // Add each choice from selected category to list
                // Add each detail from selected choice to list
                foreach (var choice in choices)
                {
                    choiceList.Add(choice.Name);
 
                    detailList.AddRange(choice.Details.Select(detail => detail.Name));
                }

                if (choices == null || choices.Count == 0 || String.IsNullOrEmpty(choices[0].Name))
                    // Dummy, hvis der er en tom Choice liste uden et navn
                {
                    CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, category);
                    //AppDelegate.MakeCall(callEntity, category.Name, this);
                }
                else
                {
                    Bundle bundle = new Bundle();

                    bundle.PutStringArray("choices", choiceList.ToArray());
                    bundle.PutStringArray("details", detailList.ToArray());
                    bundle.PutString("category", categories[args.Position].Name);
                    intent.PutExtras(bundle);

                    StartActivity(intent);
                }
            };
            return view;
        }
    }
}