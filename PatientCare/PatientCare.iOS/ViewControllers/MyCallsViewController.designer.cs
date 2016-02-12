// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
	[Register ("MyCallsViewController")]
	partial class MyCallsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView tasksLoadIndicator { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (tasksLoadIndicator != null) {
				tasksLoadIndicator.Dispose ();
				tasksLoadIndicator = null;
			}
		}
	}
}
