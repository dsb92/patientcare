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
	[Register ("ServicesViewController")]
	partial class CategoryViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UICollectionView collectionViewUser { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (collectionViewUser != null) {
				collectionViewUser.Dispose ();
				collectionViewUser = null;
			}
		}
	}
}
