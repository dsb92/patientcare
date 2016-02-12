using System;
using CoreGraphics;
using Foundation;
using PatientCare.iOS.TableViewSources;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.ViewControllers
{
    public partial class ChoiceViewController : UIViewController
    {
        public string CategoryName = null;
        public ChoiceEntity[] Choices = null;
        
        public string ChoiceName = null;
        public DetailEntity[] Details = null;

        public int ChoiceIndexPathRow;

        public ChoiceViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var choiceTableView = new UITableView();
 
            // Center the table
            choiceTableView.ContentInset = new UIEdgeInsets(70, 0, 0, 0);
            const int width = 300;
            choiceTableView.Frame = new CGRect(View.Frame.X, View.Center.Y - (width/2), View.Frame.Width, 300);

            // Remove empty cells
            choiceTableView.TableFooterView = new UIView(CGRect.Empty);

            if (Choices != null)
            {
                this.NavigationItem.Title = CategoryName;
            }


            if (Choices != null)
                choiceTableView.Source = new ChoiceSource(Choices, this);

            Add(choiceTableView);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var detailChoiceViwController = segue.DestinationViewController as DetailViewController;
            if (detailChoiceViwController != null)
            {
                detailChoiceViwController.ChoiceName = ChoiceName;
                detailChoiceViwController.Details = Details;

                // Transfer the index of the selected choice from the list
                detailChoiceViwController.ChoiceIndexPathRow = ChoiceIndexPathRow;
            }
        }
    }

}