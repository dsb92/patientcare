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
using PatientCare.Shared.Managers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace PatientCare.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
            if (UserData.IsUserLoggedIn)
            {
                LogOffUser();
            }
            else
            {
                if (ValidateLogin())
                {
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

                        // Go to valgmuligheder
                        GoToCategories();
                    }, context);
                }
            }
            
        }

        private void LogOffUser()
        {
            UserData.CPRNR = "";
            UserData.IsUserLoggedIn = false;

            if (userNameTextBox != null && loginButton != null)
            {
                userNameTextBox.Text = "";

                loginButton.Content = "Login";
            }

        }

        private void GoToCategories()
        {
            Frame.Navigate(typeof (CategoriesPage));
        }

        private void LoginInUser()
        {
            UserData.CPRNR = userNameTextBox.Text;

            loginButton.Content = "Log ud";

            UserData.IsUserLoggedIn = true;
        }

        internal bool ValidateLogin()
        {
            var userinput = userNameTextBox.Text;
            // If textfield are not empty
            if (userinput != "")
            {
                // CPR VALIDERING HER

                // PATIENT VALIDERING HER
                var manager = new LoginManager();
                var cpr = manager.GetPatient(userinput);

                if (cpr == null)
                {
                    throw new Exception(Strings.ErrorPatientNotValid);
                }

                return true;
            }

            return false;
        }
    }
}
