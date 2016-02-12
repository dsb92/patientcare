using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using PatientCare.iOS.ViewControllers;
using PatientCare.Shared;
using UIKit;

namespace PatientCare.iOS.CustomRendering
{
    public class MenuBarButtonItem
    {
        private UIBarButtonItem customButton;
        public MenuBarButtonItem(UIViewController vc)
        {
            this.customButton = new UIBarButtonItem(
            UIImage.FromBundle("Images/menu.png"),
            UIBarButtonItemStyle.Plain,
            (s, e) =>
            {
                // menu tapped
                MenuTapped(vc);
            });
        }

        public UIBarButtonItem GetCustomButtom()
        {
            return customButton;
        }

        private void MenuTapped(UIViewController vc)
        {
            var menuAlertController = UIAlertController.Create("", Strings.LoginAs + " " + UserData.CPRNR, UIAlertControllerStyle.ActionSheet);

            // When user confirms the service
            var logAfAction = UIAlertAction.Create(Strings.LogOff, UIAlertActionStyle.Destructive, action =>
            {
                // Take the user back to Login
                var tabbar = vc.TabBarController;
                var loginController = (LoginViewController)tabbar.ViewControllers[2];
                UserData.IsUserLoggedIn = false;
                tabbar.SelectedViewController = loginController;

            });

            // When user cancels the service
            var cancelAction = UIAlertAction.Create(Strings.Cancel, UIAlertActionStyle.Cancel, action =>
            {
                // Do nothing.

            });

            menuAlertController.AddAction(logAfAction);
            menuAlertController.AddAction(cancelAction);

            var popover = menuAlertController.PopoverPresentationController;

            if (popover != null)
            {
                popover.SourceView = vc.View;
                popover.SourceRect = new CGRect(vc.View.Bounds.Size.Width / 2.0, vc.View.Bounds.Size.Height / 2.0, 1.0, 1.0);
            }

            // Display the alert
            vc.PresentViewController(menuAlertController, true, null);
        }
    }
}
