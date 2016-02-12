using System;
using Foundation;
using PatientCare.iOS.ViewControllers;
using PatientCare.Shared;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.TableViewSources
{
    // The implementation of UITableViewSource methods isolates the table from the underlying data structure.
    public class ChoiceSource : UITableViewSource
    {
        private readonly ChoiceEntity[] Choices;
        private string CellIdentifier = "ChoiceCell";
        private ChoiceViewController vc;

        public ChoiceSource(ChoiceEntity[] choices, ChoiceViewController vc)
        {
            Choices = choices;

            if (vc != null)
            {
                this.vc = vc;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            var choiceName = Choices[indexPath.Row].Name;

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = choiceName;
            cell.TextLabel.TextAlignment = UITextAlignment.Center;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Choices.Length;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {

            tableView.DeselectRow(indexPath, true);

            var category = Choices[indexPath.Row].CategoryEntity;
            var choice = Choices[indexPath.Row];
            var details = Choices[indexPath.Row].Details;
            // Hvis der er ikke er nogen detailer, start kaldet her
            if (details == null || details.Count == 0)
            {
                CallEntity callEntity = CallWrapper.WrapCall(UserData.CPRNR, CallUtil.StatusCode.Active, category, choice);
                AppDelegate.MakeCall(callEntity, vc);
            }
            // Ellers overføre detaljer videre til næste view
            else
            {
                // Transfer details back and switch to DetailChoiceViewController
                vc.ChoiceName = choice.Name;
                vc.Details = details.ToArray();
                vc.ChoiceIndexPathRow = indexPath.Row;
                vc.PerformSegue("DetailChoiceSegue", this);
            }
        }
    }
}
