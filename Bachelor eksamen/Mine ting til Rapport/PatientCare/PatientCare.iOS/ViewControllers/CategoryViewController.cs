using System;
using System.Drawing;
using System.Reflection;
using CoreGraphics;
using Foundation;
using PatientCare.iOS.CustomRendering;
using PatientCare.iOS.TableViewSources;
using PatientCare.Shared;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
    public partial class CategoryViewController : UIViewController
    {
        private UIRefreshControl refreshControl;
        private CategorySource CategorySource;
        public CategoryEntity Category { get; set; }
        public CategoryEntity[] Categories { get; set; }

        public CategoryViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupLayout();

            SetupCategorySource();

            PopulateTable();
        }



        private void SetupLayout()
        {
            refreshControl = new UIRefreshControl();
            refreshControl.ValueChanged += StartRefresh;
            collectionViewUser.AddSubview(refreshControl);

            // Observe when this viewcontroller enters background
            NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.DidEnterBackgroundNotification, DidEnterBackground);

            Console.WriteLine(NavigationItem);
            Console.WriteLine(this.NavigationController);
            // Remove the back button
            this.NavigationItem.SetHidesBackButton(true, true);

            var customBarButtonItem = new MenuBarButtonItem(this);
            var customButton = customBarButtonItem.GetCustomButtom();
            NavigationItem.RightBarButtonItem = customButton;
        }

        private void SetupCategorySource()
        {
            CategorySource = new CategorySource();
            CategorySource.FontSize = 11f;
            CategorySource.ImageViewSize = new SizeF(100f, 100f);

            collectionViewUser.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellID);
            collectionViewUser.ShowsHorizontalScrollIndicator = false;
            collectionViewUser.Source = CategorySource;
        }

        // Only refresh Categories
        private void StartRefresh(object sender, EventArgs e)
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    Categories = DataHandler.LoadCategoriesFromWeb();

                    // Delete Categories
                    DataHandler.DeleteCategoriesFromLocalDatabase(new LocalDB());

                    // If data loading from web not succeded, nothing will be saved to the local database (Categories instance will be null)
                    DataHandler.SaveCategoriesToLocalDatabase(new LocalDB(), Categories);

                    this.InvokeOnMainThread(() =>
                    {
                        SetupCategorySource();
                        LoadCategories();
                        collectionViewUser.ReloadData();
                        refreshControl.EndRefreshing();
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR refreshing data: " + ex.Message);

                    this.InvokeOnMainThread(() =>
                    {
                        new UIAlertView(Strings.Error, Strings.ErrorReading, null, Strings.OK, null).Show();
                        refreshControl.EndRefreshing();
                    });
                }

            }).Start();
        }

        private void PopulateTable()
        {
            LoadCategories();

            LoadCalls();

            collectionViewUser.ReloadData();
        }

        private void LoadCategories()
        {
            Categories = DataHandler.LoadCategoriesFromLocalDatabase(new LocalDB());

            if (Categories != null && Categories.Length != 0)
            {
                foreach (var category in Categories)
                {
                    category.Tapped = () => CategoryTapped(category);
                    CategorySource.Rows.Add(category);
                }
            }
            else
            {
                new UIAlertView(Strings.ErrorNoCategories, null, null, Strings.OK, null).Show();
            }
        }

        private void LoadCalls()
        {
            // Load My Calls and increment bagde value for each waiting call
            var myCalls = DataHandler.LoadCallsFromLocalDatabase(new LocalDB());

            if (myCalls != null && myCalls.Length != 0)
            {
                foreach (var myCall in myCalls)
                {
                    switch (myCall.Status)
                    {
                        // Ventende
                        case (int)CallUtil.StatusCode.Active:
                            TabBar.IncrementBadgeValue(this);
                            break;
                    }
                }
            }
        }

        private void CategoryTapped(CategoryEntity category)
        {
            // The category tapped
            Category = category;

            // Hvis der ikke er nogle typer, så start kaldet her
            if (Category.Choices == null || Category.Choices.Count == 0 || String.IsNullOrEmpty(Category.Choices[0].Name)) // Dummy, hvis der er en tom Choice liste uden et navn
            {
                CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, Category);
                AppDelegate.MakeCall(callEntity, this);
            }

            // Ellers gå videre til næste view.
            else
            {
                PerformSegue("CategorySegue", this);
            }

        }

        public void DidEnterBackground(NSNotification notification)
        {

        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // Transfere the service tapped to the next controller (CategoryViewController)
            var choiceViewController = segue.DestinationViewController as ChoiceViewController;
            if (choiceViewController != null)
            {
                choiceViewController.CategoryName = Category.Name;
                choiceViewController.Choices = Category.Choices.ToArray();
            }
        }
    }
}