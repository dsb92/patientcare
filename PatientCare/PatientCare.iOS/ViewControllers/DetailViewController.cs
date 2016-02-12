using System;
using CoreGraphics;
using PatientCare.iOS.TableViewSources;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
    public partial class DetailViewController : UIViewController
    {
        public DetailEntity[] Details = null;
        public string ChoiceName = null;
        public int ChoiceIndexPathRow;
        public DetailViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            var categoryTableView = new UITableView();

            // Center the table
            categoryTableView.ContentInset = new UIEdgeInsets(70, 0, 0, 0);
            const int width = 300;
            categoryTableView.Frame = new CGRect(View.Frame.X, View.Center.Y - (width / 2), View.Frame.Width, 300);

            // Remove empty cells
            categoryTableView.TableFooterView = new UIView(CGRect.Empty);

            if (ChoiceName != null)
            {
                this.NavigationItem.Title = ChoiceName;
            }


            if (ChoiceName != null)
                categoryTableView.Source = new DetailSource(Details, ChoiceIndexPathRow, this);

            Add(categoryTableView);
        }
    }
}