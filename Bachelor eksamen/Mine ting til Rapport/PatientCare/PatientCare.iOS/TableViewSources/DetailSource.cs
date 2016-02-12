using System;
using Foundation;
using PatientCare.iOS.ViewControllers;
using PatientCare.Shared;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.TableViewSources
{
    public class DetailSource : UITableViewSource
    {
        private readonly DetailEntity[] Details;
        private int ChoiceIndexPathRow;
        private string CellIdentifier = "DetailCell";
        private DetailViewController vc;

        public DetailSource(DetailEntity[] details, int choiceIndexPath, DetailViewController vc)
        {

            Details = details;
            ChoiceIndexPathRow = choiceIndexPath;

            if (vc != null)
            {
                this.vc = vc;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            var detailName = Details[indexPath.Row].Name;

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = detailName;
            cell.TextLabel.TextAlignment = UITextAlignment.Center;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Details.Length;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {

            tableView.DeselectRow(indexPath, true);

            var detail = Details[indexPath.Row];
            var choice = detail.Choices[0];
            if (detail.Choices.Count > 1)
            {
                choice = detail.Choices[ChoiceIndexPathRow];
            }

            var category = choice.CategoryEntity;
            
            
            CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, category, choice, detail);
            AppDelegate.MakeCall(callEntity, vc);

        }
    }
}
