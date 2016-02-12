using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using PatientCare.iOS.CustomRendering;
using PatientCare.iOS.TableViewSources;
using PatientCare.Shared;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
    public partial class MyCallsViewController : UIViewController
    {
        private UIRefreshControl refreshControl;
        public CallEntity callEntity;
        public UITableView myCallsTableView;
        private MyCallsSource CallSource;
        public MyCallsViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (myCallsTableView != null)
            {
                if (callEntity != null)
                {
                    CallSource.SetCallEntities(callEntity);
                    myCallsTableView.ReloadData();
                } 
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            SetupLayout();
        }

        // Refresh calls from web
        private void StartRefresh(object sender, EventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                UpdateStatus();

                this.InvokeOnMainThread(() =>
                {
                    CallSource.SetCallEntities(null);
                    myCallsTableView.ReloadData();
                    refreshControl.EndRefreshing();
                });

            }).Start();
        }

        public void UpdateStatus()
        {
            try
            {
                // Hent kald fra lokale database
                var calls = DataHandler.LoadCallsFromLocalDatabase(new LocalDB());

                // For hver kald der blevet gemt, tjek om dens status er blevet ændret siden sidst og opdater listen
                foreach (var call in calls)
                {
                    calls = DataHandler.GetUpdatedStatusForAllCalls(calls.ToList(), call);
                }

                // Gem listen i den lokale database
                DataHandler.SaveCallsToLocalDatabase(new LocalDB(), calls);

            }
            catch (Exception ex)
            {
                this.InvokeOnMainThread(() =>
                {
                    Console.WriteLine("Error loading calls from web" + ex.Message + "...loading from local database");

                    new UIAlertView(Strings.Error, Strings.ErrorReading, null, Strings.OK, null).Show();
                });
            }
        }

        private void SetupLayout()
        {
            NavigationItem.RightBarButtonItem = new MenuBarButtonItem(this).GetCustomButtom();

            if (myCallsTableView == null)
            {
                myCallsTableView = new UITableView();
                // Fill the whole view with the table (minus top bar and bottom bar)
                myCallsTableView.ContentInset = new UIEdgeInsets(70, 0, 70, 0);
                myCallsTableView.Frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);

                // Remove empty cells
                myCallsTableView.TableFooterView = new UIView(CGRect.Empty);

                CallSource = new MyCallsSource(callEntity, this);

                myCallsTableView.Source = CallSource;

                Add(myCallsTableView);

                myCallsTableView.ReloadData();
            }

            refreshControl = new UIRefreshControl();
            refreshControl.ValueChanged += StartRefresh;

            myCallsTableView.AddSubview(refreshControl);
        }
    }
}