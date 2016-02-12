using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Util;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Managers;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;

namespace PatientCare.Android
{
    [Activity(Label = "PatientCare.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private Button btnLogin;
        private EditText etCprNr;
        private ProgressDialog dialog;

        const string CprNrKey = "Username";
        const string LoginKey = "Login";

        private CategoryEntity[] Categories { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            InitializeLayout();

            CreateSQLiteTables();

            if (Intent.GetBooleanExtra("logoff", false))
            {
                LogOffUser();
            }

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
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            try
                            {
                                this.RunOnUiThread(() =>
                                {
                                    dialog = new ProgressDialog(this);
                                    dialog.SetMessage(Strings.SpinnerDataReading);
                                    dialog.SetCancelable(false);
                                    dialog.Show();
                                });

                                var manager = new LoginManager();
                                var cpr = manager.GetPatient(userCprInput);

                                if (cpr == null) throw new Exception(Strings.ErrorPatientNotValid);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Login failed with error: " + ex.Message);

                                this.RunOnUiThread(() =>
                                {
                                    dialog.Hide();

                                    var errorMessage = ex.Message.Equals(Strings.ErrorPatientNotValid) ? Strings.ErrorPatientNotValid : Strings.ErrorNoNetwork;

                                    new AlertDialog.Builder(this).SetTitle(Strings.ErrorLogin)
                                        .SetMessage(errorMessage)
                                        .SetPositiveButton("OK", delegate { })
                                        .Show();
                                });

                                return;
                            }

                            try
                            {
                                Categories = DataHandler.LoadCategoriesFromWeb();
                                DataHandler.DeleteCategoriesFromLocalDatabase(new LocalDB());
                                DataHandler.SaveCategoriesToLocalDatabase(new LocalDB(), Categories);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("ERROR loading data: " + ex.Message + "...loading from local database");

                                this.RunOnUiThread(() =>
                                {
                                    dialog.Hide();

                                    new AlertDialog.Builder(this).SetTitle(Strings.Error)
                                        .SetMessage(Strings.ErrorReading)
                                        .SetPositiveButton("OK", delegate { })
                                        .Show();
                                });
                            }

                            this.RunOnUiThread(() =>
                            {
                                LoginInUser();

                                dialog.Hide();

                                // Go to valgmuligheder
                                GoToCategories();
                            });

                        });

                        
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
            UserData.CPRNR = etCprNr.Text;
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

            ActionBar.RemoveAllTabs();
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

