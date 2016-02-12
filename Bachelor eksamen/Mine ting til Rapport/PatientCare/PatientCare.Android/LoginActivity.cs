using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Test.Mock;
using PatientCare.Shared;
using PatientCare.Shared.Model;

namespace PatientCare.Android
{
    [Activity(Label = "PatientCare.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private Button btnLogin;
        private EditText etCprNr;

        const string CprNrKey = "Username";
        const string LoginKey = "Login";

        private CategoryEntity[] Categories { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            InitializeLayout();

            CreateSQLiteTables();

            btnLogin.Click += delegate
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

                var isLoggedIn = prefs.GetBoolean(LoginKey, false);

                if (isLoggedIn)
                {
                    LogOffUser();
                }
                else
                {
                    // If user login info is correct, show services
                    var userCprInput = etCprNr.Text;
                    if (ValidateCpr(userCprInput))
                    {
                        // Send cpr til web api for yderligere validering (er patient indlagt/udskrevet?)
                        // Hvis Response kode er OK betyder det er patient er indlagt og ikke udskrevet endnu
                        /*
                        try
                        {
                            var manager = new LoginManager();
                            manager.PostLogin(userCprInput);
                        }
                        // Response kode er forskellig fra OK, hvilket betyder at patient enten ikke er indlagt eller er blevet udskrevet
                        catch (Exception ex)
                        {
                            Console.WriteLine("Login failed with error: " + ex.Message);

                            this.InvokeOnMainThread(() =>
                            {
                                new UIAlertView("Login fejl", "Du er ikke indlagt", null, null, "OK").Show();
                            });

                            // Fortsæt ikke programmet, hvis login fejlede
                            return;
                        }
                         * */

                        // Show the overlay (loading screen)
                        //ShowLoadingScreen();

                        Categories = DataHandler.LoadCategoriesFromWeb();
                        DataHandler.SaveCategoriesToLocalDatabase(new LocalDB(), Categories);

                        LoginInUser();
                        // Hide the overlay (loading screen)
                        //loadingOverlay.Hide();

                        // Go to valgmuligheder
                        GoToCategories();
                    }
                }
            };
        }

        private void LoginInUser()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            editor.PutString(CprNrKey, etCprNr.Text);
            editor.PutBoolean(LoginKey, true);
            editor.Apply();

            btnLogin.Text = "Log ud";
        }

        private void ShowLoadingScreen()
        {
            throw new NotImplementedException();
        }

        private void LogOffUser()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            editor.PutString(CprNrKey, "");
            editor.PutBoolean(LoginKey, false);
            editor.Apply();

            if (etCprNr != null && btnLogin != null)
            {
                btnLogin.Text = "Login";
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
            Intent intent = new Intent(Application.Context, typeof(CategoryActivity));
            StartActivity(intent);
        }


        private void InitializeLayout()
        {
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            etCprNr = FindViewById<EditText>(Resource.Id.etUserName);
        }

        private void CreateSQLiteTables()
        {
            const string launchkey = "LaunchedBeforeKey";

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            var hasLaunched = prefs.GetBoolean(launchkey, false);

            if (!hasLaunched)
            {
                editor.PutBoolean(launchkey, true);
                editor.Apply();

                // Create localDb tables
                var localDb = new LocalDB();
                localDb.CreateTables();
            }
        }

        private bool ValidateCpr(string cpr)
        {
            //return true;
            var errTitle = "Login fejl";

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(errTitle);

            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", delegate { });

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
                        builder.SetMessage("Forkert CPR format");
                        builder.Show();

                        return false;

                    case CprValidator.CprError.DateError:
                        builder.SetMessage("Dato i CPR er ugyldig");
                        builder.Show();

                        return false;

                    case CprValidator.CprError.Check11Error:
                        builder.SetMessage("CPR er ugyldigt");
                        builder.Show();

                        return false;

                    default:
                        builder.SetMessage("Ukendt fejl");
                        builder.Show();

                        return false;

                }

            }

            builder.SetMessage("Indtast venligst et CPR nr");
            builder.Show();

            return false;
        }

    }
}

