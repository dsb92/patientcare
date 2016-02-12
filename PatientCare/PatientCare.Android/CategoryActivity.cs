using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Fragment = Android.App.Fragment;


namespace PatientCare.Android
{
    [Activity(Label = "CategoryActivity")]
    public class CategoryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
             
            SetContentView(Resource.Layout.activity_fragment_view);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            AddTab(GetString(Resource.String.ChoiceList), 0, new CategoryFragment());
            AddTab(GetString(Resource.String.MyCall), 0, new CallOverviewFragment());
            AddTab(GetString(Resource.String.Login), 0, null);
            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            //tab.SetIcon(Resource.Drawable.ic_tab_white);

            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                if (view != null)
                {
                    var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                    if (fragment != null)
                        e.FragmentTransaction.Remove(fragment);
                    e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
                }
                else
                {
                    Intent intent = new Intent(Application.Context, typeof(LoginActivity));
                    StartActivity(intent);
                }
                
            };
            tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.logoff:
                    Intent intent = new Intent(Application.Context, typeof(LoginActivity));
                    intent.PutExtra("logoff", true);
                    StartActivity(intent);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}