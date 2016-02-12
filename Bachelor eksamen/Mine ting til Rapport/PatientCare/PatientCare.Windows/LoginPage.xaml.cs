using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PatientCare.Shared;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace PatientCare.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Hardcorded brugerdata
        private string username = "test";
        private string password = "1234";

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Global.Instance.IsLoggedIn)
            {
                LogOffUser();
            }
            else
            {
                if (ValidateLogin())
                {
                    // Show the overlay (loading screen)
                    //ShowLoadingScreen();

                    var context = TaskScheduler.FromCurrentSynchronizationContext();

                    Task task = null;

                    task = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var categories = DataHandler.LoadCategoriesFromWeb();
                            // If data loading from web not succeded, nothing will be saved to the local database (Categories instance will be null)
                            DataHandler.SaveCategoriesToLocalDatabase(new LocalDB(), categories);
                        }
                        catch (Exception ex)
                        {
                            if (task != null)
                                task.ContinueWith(c =>
                                {
                                    Debug.WriteLine("ERROR loading data: " + ex.Message + "...loading from local database");
                                    // Create the message dialog and set its content
                                    var msg = new MessageDialog(
                                        "Der skete en fejl ved indlæsning af data").ShowAsync();

                                    
                                }, context);
                        }

                    });

                    if (task != null) task.ContinueWith(c =>
                    {
                        LoginInUser();
                        // Hide the overlay (loading screen)
                        //loadingOverlay.Hide();

                        // Go to valgmuligheder
                        GoToCategories();
                    }, context);
                }
            }
            
        }

        private void LogOffUser()
        {
            Global.Instance.UserCpr = "";
            Global.Instance.Password = "";
            Global.Instance.IsLoggedIn = false;

            if (userNameTextBox != null && passwordTextBox != null && loginButton != null)
            {
                userNameTextBox.Text = "";
                passwordTextBox.Text = "";

                loginButton.Content = "Login";
            }

            /*
            // Disable each tab except login tab
            var tabbar = TabBarController;

            if (tabbar != null)
            {
                foreach (var vc in tabbar.ViewControllers)
                {
                    if (!vc.Equals(tabbar.ViewControllers[2]))
                    {
                        vc.TabBarItem.Enabled = false;
                    }

                }
            }
             */
        }

        private void GoToCategories()
        {
            Frame.Navigate(typeof (CategoriesPage));
        }

        private void LoginInUser()
        {
            Global.Instance.UserCpr = username;
            Global.Instance.Password = password;

            userNameTextBox.Text = username;
            passwordTextBox.Text = password;
            loginButton.Content = "Log ud";

            Global.Instance.IsLoggedIn = true;
        }

        internal bool ValidateLogin()
        {
            var userinput = userNameTextBox.Text;
            var passinput = passwordTextBox.Text;
            // If textfield are not empty
            if (userinput != "" && passinput != "")
            {
                // If username and password matches the patient
                if (userinput == this.username && passinput == this.password)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
    }
}
