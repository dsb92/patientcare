using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using CoreGraphics;
using Foundation;
using PatientCare.iOS.CustomRendering;
using PatientCare.iOS.ViewControllers;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public static string UserCprNr = "";
        public static bool IsUserLoggedIn;
        public static LoadingOverlay loadingOverlay;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            var defaults = NSUserDefaults.StandardUserDefaults;
            const string key = "LaunchedBeforeKey";
            if (!defaults.BoolForKey(key))
            {
                // First launch
                defaults.SetBool(true, key);
                defaults.Synchronize();

                // Create localDb tables
                var localDb = new LocalDB();
                localDb.CreateTables();
            }

            return true;
        }

        //
        // This method is invoked when the application is about to move from active to inactive state.
        //
        // OpenGL applications should use this method to pause.
        //
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }

        public static void MakeCall(CallEntity callEntity, UIViewController vc)
        {

            var category = callEntity.Category;
            var choice = callEntity.Choice ?? "";
            var detail = callEntity.Detail ?? "";

            var confirmAlertController = UIAlertController.Create(category + " " + choice + " " + detail, Strings.CallSendMessage, UIAlertControllerStyle.Alert);

            // When user confirms the service
            var okAction = UIAlertAction.Create(Strings.CallSend, UIAlertActionStyle.Destructive, action =>
            {
                ShowLoadingScreen(vc, Strings.SpinnerDataSending);

                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    var callEntities = DataHandler.LoadCallsFromLocalDatabase(new LocalDB());

                    vc.InvokeOnMainThread(() =>
                    {
                        if (callEntities != null && callEntities.Length > 0)
                        {
                            // Check if the call already has been made, then return;
                            if (CallHasBeenMade(callEntities, callEntity)) return;
                        }
          
                        new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                        {
                            // Make the async patient call here
                            try
                            {
                                ICall patientCall = new PatientCall();
                                // Assign the callid with the returned MongoDB id
                                callEntity._id = patientCall.MakeCall(callEntity);

                                vc.InvokeOnMainThread(() =>
                                {
                                    // Call successfull, take the user to myCalls passing the viewcontroller and the requested call
                                    GoToMyCalls(vc,callEntity);

                                });
                            }
                            catch (Exception ex)
                            {
                                // Hide the loading screen
                                Console.WriteLine("ERROR making call: " + ex.Message);

                                vc.InvokeOnMainThread(() =>
                                {
                                    loadingOverlay.Hide();
                                    new UIAlertView(Strings.Error, Strings.ErrorSendingCall, null, Strings.OK, null).Show();
                                });
                            }

                        })).Start();
                        
                    });

                })).Start();

    
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    
                })).Start();

            });

            // When user cancels the service
            var cancelAction = UIAlertAction.Create(Strings.Cancel, UIAlertActionStyle.Cancel, action =>
            {
                // Do nothing.

            });

            confirmAlertController.AddAction(okAction);
            confirmAlertController.AddAction(cancelAction);

            // Display the alert
            vc.PresentViewController(confirmAlertController, true, null);
        }

        private static void GoToMyCalls(UIViewController vc, CallEntity callEntity)
        {
            // Hide the loading screen
            loadingOverlay.Hide();
            // (Get a confirm message that the patient call was successfull)
            //new UIAlertView(Strings.CallSent, null, null, Strings.OK, null).Show();

            // Take the user back to Categories
            //vc.NavigationController.PopViewController(true);

            // Take the user to My Calls
            var tabbar = vc.TabBarController;
            var navController = tabbar.ViewControllers[1];
            var myCallsController = (MyCallsViewController)navController.ChildViewControllers[0];
            myCallsController.callEntity = callEntity;

            tabbar.SelectedViewController = navController;
        }

        private static bool CallHasBeenMade(CallEntity[] callEntities, CallEntity callEntity)
        {
            if (callEntities.Where(
                            myCalls => myCalls.Category == callEntity.Category && myCalls.Choice == callEntity.Choice &&
                                       myCalls.Detail == callEntity.Detail)
                            .Any(myCalls => myCalls.Status == (int)CallUtil.StatusCode.Active))
            {
                loadingOverlay.Hide();
                new UIAlertView(Strings.CallAlreadySent, null, null, "OK", null).Show();

                return true;
            }
            return false;
        }

        public static void ShowLoadingScreen(UIViewController vc, String message)
        {
            // Loading screen setup
            // Determine the correct size to start the overlay (depending on device orientation)
            var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            }
            loadingOverlay = new LoadingOverlay(bounds, message);

            // Start the overlay (loading screen)
            vc.View.Add(loadingOverlay);
        }
    }
}