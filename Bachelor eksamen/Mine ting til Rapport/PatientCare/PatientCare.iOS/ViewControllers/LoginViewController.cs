using System;
using CoreGraphics;
using Foundation;
using PatientCare.iOS.CustomRendering;
using PatientCare.Shared;
using PatientCare.Shared.DAL;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
    public partial class LoginViewController : UIViewController
    {
        private CategoryEntity[] Categories { get; set; }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupLayout();

        }

        private void SetupLayout()
        {
            // Perform any additional setup after loading the view, typically from a nib.

            loginButton.TouchUpInside += (sender, e) =>
            {
                if (UserData.IsUserLoggedIn)
                {
                    LogOffUser();
                }
                else
                {
                    var userCprInput = userNameTextField.Text;
                    // If user cpr nr is valid
                    if (ValidateCpr(userCprInput))
                    {
                        new System.Threading.Thread(() =>
                        {
                            // Send cpr til web api for yderligere validering (er patient indlagt/udskrevet?)
                            // Hvis Response kode er OK betyder det er patient er indlagt og ikke udskrevet endnu
                       
                            try
                            {
                                var manager = new LoginManager();
                                var cpr = manager.GetPatient(userCprInput);

                                if (cpr == null) throw new Exception(Strings.ErrorPatientNotValid);
                            }
                            // Response kode er forskellig fra OK, hvilket kan betyde en netværksfejl eller at patient ikke er indlagt (antager vi).
                            catch (Exception ex)
                            {
                                Console.WriteLine("Login failed with error: " + ex.Message);

                                this.InvokeOnMainThread(() =>
                                {
                                    var errorMessage = ex.Message.Equals(Strings.ErrorPatientNotValid) ? Strings.ErrorPatientNotValid : Strings.ErrorNoNetwork;
            
                                    new UIAlertView(Strings.ErrorLogin, errorMessage, null, null, "OK").Show();
                                });

                                // Fortsæt ikke programmet, hvis login fejlede
                                return;
                            }
                            

                            // Skriv evt. en besked til bruger om at login gik OK
                            this.InvokeOnMainThread(() =>
                            {
                                // Show the overlay (loading screen)
                                AppDelegate.ShowLoadingScreen(this, Strings.SpinnerDataReading);
                            });

                            // Hent kategorier for den pågældende patient/afdeling/sygehus
                            try
                            {
                                Categories = DataHandler.LoadCategoriesFromWeb();
                                //Categories = DataHandler.LoadCategoriesTESTDATA();

                                // Delete categories from web
                                DataHandler.DeleteCategoriesFromLocalDatabase(new LocalDB());

                                // If data loading from web not succeded, nothing will be saved to the local database (Categories instance will be null)
                                DataHandler.SaveCategoriesToLocalDatabase(new LocalDB(), Categories);
                            }
                            // Mislykkedes at hente kategorier fra web, indlæser fra lokal database (i næste view)
                            catch (Exception ex)
                            {
                                Console.WriteLine("ERROR loading data: " + ex.Message + "...loading from local database");

                                this.InvokeOnMainThread(() =>
                                {
                                    new UIAlertView(Strings.Error, Strings.ErrorReading, null, Strings.OK, null).Show();
                                });
                            }

                            this.InvokeOnMainThread(() =>
                            {
                                // Login user
                                LoginInUser();
                                // Hide the overlay (loading screen)
                                AppDelegate.loadingOverlay.Hide();

                                // Go to valgmuligheder
                                GoToCategories();
                            });

                        }).Start();

                    }
                }

            };

            this.userNameTextField.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }

        private void GoToCategories()
        {
            PerformSegue("ServiceSegue", this);
        }

   
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // If user is logged in
            if (UserData.IsUserLoggedIn)
            {
                userNameTextField.Text = UserData.CPRNR;
                loginButton.SetTitle("Log ud", UIControlState.Normal);
            }
            else
            {
                LogOffUser();
            }
        }

        #endregion

        // To hide keyboard when user taps outside the inputs.
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            userNameTextField.ResignFirstResponder();
        }

        #region Login Validation

        private bool ValidateCpr(string cpr)
        {
            //return true;
            var errTitle = "Login fejl";
            // If textfield are not empty
            if (cpr != "")
            {
                CprValidator.CprError cprError;

                CprValidator.CheckCPR(cpr, out cprError);

                switch (cprError)
                {
                    case CprValidator.CprError.NoError:
                        return true;

                    case CprValidator.CprError.FormatError:
                        new UIAlertView(errTitle, "Forkert CPR format", null, null, "OK").Show();
                        return false;

                    case CprValidator.CprError.DateError:
                        new UIAlertView(errTitle, "Dato i CPR er ugyldig", null, null, "OK").Show();
                        return false;

                    case CprValidator.CprError.Check11Error:
                        new UIAlertView(errTitle, "CPR er ugyldigt", null, null, "OK").Show();
                        return false;

                    default:
                        new UIAlertView(errTitle, "Ukendt fejl", null, null, "OK");
                        return false;

                }
            }

            new UIAlertView(errTitle, "Indtast venligst et CPR nr", null, null, "OK").Show();

            return false;
        }

        public void LoginInUser()
        {
            UserData.CPRNR = userNameTextField.Text;

            loginButton.SetTitle("Log ud", UIControlState.Normal);

            UserData.IsUserLoggedIn = true;
        }

        public void LogOffUser()
        {
            UserData.CPRNR = "";
            UserData.IsUserLoggedIn = false;

            if (userNameTextField != null && loginButton != null)
            {
                userNameTextField.Text = UserData.CPRNR;

                loginButton.SetTitle("Login", UIControlState.Normal);
            }

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
        }

        #endregion

        #region PerformSegue

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // Transfere any data here
            // Take the user to Valgmuligheder
            var tabbar = segue.DestinationViewController as UITabBarController;
            var navController = tabbar.ViewControllers[0];
            var myCallsController = (CategoryViewController)navController.ChildViewControllers[0];
            //myCallsController.Categories = Categories;  // No need to pass Categories if we are loading it from the database after it has been saved to it. 

            tabbar.SelectedViewController = navController;
        }

        #endregion
    }
}