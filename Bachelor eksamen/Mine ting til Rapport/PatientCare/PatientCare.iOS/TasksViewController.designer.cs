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

namespace PatientCare.iOS
{
	[Register ("TasksViewController")]
	partial class TasksViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView tasksLoadIndicator { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView tasksTextView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (tasksLoadIndicator != null) {
				tasksLoadIndicator.Dispose ();
				tasksLoadIndicator = null;
			}
			if (tasksTextView != null) {
				tasksTextView.Dispose ();
				tasksTextView = null;
			}
		}
	}
}
