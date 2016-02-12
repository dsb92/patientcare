using System;
using UIKit;

namespace PatientCare.iOS.CustomRendering
{
    public static class TabBar
    {
        public static void IncrementBadgeValue(UIViewController vc)
        {
            // The tabbar
            var tabbar = vc.TabBarController;
            var navController = tabbar.ViewControllers[1];

            // Increment My Calls Badge Value
            string tabItemBadgeValue = navController.TabBarItem.BadgeValue;

            if (!String.IsNullOrEmpty(tabItemBadgeValue))
            {
                var intValue = Int16.Parse(tabItemBadgeValue);
                navController.TabBarItem.BadgeValue = (intValue + 1).ToString();
            }
            else
            {
                navController.TabBarItem.BadgeValue = "1";
            }
        }

        public static void DecrementBadgeValue(UIViewController vc)
        {
            // The tabbar
            var tabbar = vc.TabBarController;
            var navController = tabbar.ViewControllers[1];

            // Decrement My Calls Badge Value
            string tabItemBadgeValue = navController.TabBarItem.BadgeValue;

            if (!String.IsNullOrEmpty(tabItemBadgeValue))
            {
                var intValue = Int16.Parse(tabItemBadgeValue);
                navController.TabBarItem.BadgeValue = (intValue - 1).ToString();

                if (navController.TabBarItem.BadgeValue == "0")
                {
                    navController.TabBarItem.BadgeValue = null;
                }
            }
        }

        public static void ResetBadgeValue(UIViewController vc)
        {
            // The tabbar
            var tabbar = vc.TabBarController;
            var navController = tabbar.ViewControllers[1];
            // Set BadgeValue to Zero/Null
            navController.TabBarItem.BadgeValue = null;
            
        }
    }
}
