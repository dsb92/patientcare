using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Foundation;
using PatientCare.iOS.CustomRendering;
using PatientCare.iOS.ViewControllers;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;
using UIKit;

namespace PatientCare.iOS.TableViewSources
{
    public class MyCallsSource : UITableViewSource
    {
        private List<CallEntity> CallEntities;

        private string CellIdentifier = "MyCallCell";
        private MyCallsViewController vc;
        public MyCallsSource(CallEntity callEntity, MyCallsViewController vc)
        {

            if (vc != null)
            {
                this.vc = vc;
            }

            SetCallEntities(callEntity);
        }

        public void SetCallEntities(CallEntity callEntity)
        {
            // Load calls from database
            CallEntities = DataHandler.LoadCallsFromLocalDatabase(new LocalDB()).ToList();

            if (CallEntities == null || CallEntities.Count == 0)
            {
                CallEntities = new List<CallEntity>();
            }

            // If we are making a new call
            if (callEntity != null)
            {
                // And if the call is the same as before
                if (CallEntities.Any(call => call._id == callEntity._id))
                {
                    //CallEntities.Remove(callEntity);
                    callEntity = null;
                }
            }

            // If the call is not the same as before, e.g. if we are just updating the call
            if (callEntity != null)
            {
                CallEntities.Add(callEntity);
                
                DataHandler.SaveCallsToLocalDatabase(new LocalDB(), CallEntities.ToArray());
            }

            TabBar.ResetBadgeValue(vc);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);
            }

            var categoryName = CallEntities[indexPath.Row].Category;
            var choiceName = CallEntities[indexPath.Row].Choice;
            var detailName = CallEntities[indexPath.Row].Detail;
            var status = CallEntities[indexPath.Row].Status;

            var imageToDownload = "";
            var timeStamp = CallEntities[indexPath.Row].CreatedOn;

            // Text
            if (!String.IsNullOrEmpty(categoryName))
            {
                cell.TextLabel.Text = categoryName;

                if (!String.IsNullOrEmpty(choiceName))
                {
                    cell.TextLabel.Text += " " + choiceName;

                    if (!String.IsNullOrEmpty(detailName))
                    {
                        cell.TextLabel.Text += " " + detailName;
                    }
                }
            }

            // Detail Text, TimeStamp, and Badge number value
            switch (status)
            {
                case (int)CallUtil.StatusCode.Active:
                    // Increment My Calls Badge Value
                    cell.DetailTextLabel.Text = Strings.StatusActive + "\t" + Strings.CallCreated + " " + timeStamp;
                    // Change the cell color to white
                    cell.BackgroundColor = UIColor.White;
                    // Decrement Badge value
                    TabBar.IncrementBadgeValue(vc);
                    break;
                case (int)CallUtil.StatusCode.Waiting:
                    cell.DetailTextLabel.Text = Strings.StatusWaiting + "\t" + Strings.CallCreated + " " + timeStamp;
                    // Change the cell color to red
                    cell.BackgroundColor = UIColor.Red;
                    break;
                case (int)CallUtil.StatusCode.Completed:
                    cell.DetailTextLabel.Text = Strings.StatusCompleted + "\t" + Strings.CallCreated + " " + timeStamp;
                    // Change the cell color to green
                    cell.BackgroundColor = UIColor.Green;
                    break;
                case (int)CallUtil.StatusCode.Canceled:
                    cell.DetailTextLabel.Text = Strings.StatusCanceled + "\t" + Strings.CallCreated + " " + timeStamp;
                    // Change the cell color to blue
                    cell.BackgroundColor = UIColor.Blue;
                    break;
            }

            //// Image
            
            //if (!String.IsNullOrEmpty(imageToDownload))
            //{
            //    var webClient = new WebClient();
            //    webClient.DownloadDataCompleted += (s, e) =>
            //    {
            //        try
            //        {
            //            var bytes = e.Result; // get the downloaded data
            //            cell.ImageView.Image = ImageHandler.BytesToImage(bytes);
            //            cell.SetNeedsLayout();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("Something went wrong loading image for cell..." + ex.Message);
            //        }
                    
            //    };
            //    webClient.DownloadDataAsync(new Uri(imageToDownload));
            //}
            

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return CallEntities.Count;
        }

        // Fortryd kald
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            // Get the selected call
            var callEntity = CallEntities[indexPath.Row];

            // Do not allow user to tap on the cell if call is canceled or completed
            // return if so
            if (callEntity.Status == (int)CallUtil.StatusCode.Canceled || callEntity.Status == (int)CallUtil.StatusCode.Completed) return;

            // Selected Call is active
            tableView.DeselectRow(indexPath, true);

            var regretAlertController = UIAlertController.Create(Strings.CallRegretTitle, Strings.CallRegretMessage, UIAlertControllerStyle.Alert);

            var regretAction = UIAlertAction.Create(Strings.OK, UIAlertActionStyle.Destructive, action =>
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    // Update status
                    callEntity.Status = (int)CallUtil.StatusCode.Canceled;

                    // Try update the call
                    try
                    {
                        // Put the async patient call here
                        ICall patientCall = new PatientCall();
                        patientCall.UpdateCall(callEntity);

                        vc.InvokeOnMainThread(() =>
                        {
                            // (Get a confirm message that the patient call was successfull)
                            new UIAlertView(Strings.CallRegretted, null, null, "OK", null).Show();

                            DataHandler.UpdateMyCallToLocalDatabase(new LocalDB(), callEntity);

                            // Reload data
                            SetCallEntities(callEntity);
                            tableView.ReloadData();
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR updaing call: " + ex.Message);

                        vc.InvokeOnMainThread(() =>
                        {
                            new UIAlertView(Strings.Error, Strings.ErrorSendingCall, null, "OK", null).Show();

                            return;
                        });
                    } 
                })).Start();
            });

            // When user cancels the service
            var cancelAction = UIAlertAction.Create("Annullér", UIAlertActionStyle.Cancel, action =>
            {
                // Do nothing.

            });

            regretAlertController.AddAction(regretAction);
            regretAlertController.AddAction(cancelAction);

            // Display the alert
            vc.PresentViewController(regretAlertController, true, null);
        }

        //public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        //{
        //    switch (editingStyle)
        //    {
        //        case UITableViewCellEditingStyle.Delete:
        //            // remove the item from the underlying data source
        //            CallEntities.RemoveAt(indexPath.Row);
        //            // save to local database
        //            //DataHandler.SaveCallsToLocalDatabase(db, CallEntities.ToArray());
        //            DataHandler.UpdateMyCallsToLocalDatabase(new LocalDB(), CallEntities.ToArray());
        //            var calls = DataHandler.LoadCallsFromLocalDatabase(new LocalDB());
        //            // delete the row from the table
        //            tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
        //            break;
        //        case UITableViewCellEditingStyle.None:
        //            Console.WriteLine("CommitEditingStyle:None called");
        //            break;
        //    }
        //}
        //public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    var status = CallEntities[indexPath.Row].Status;
        //    if (status == (int) CallUtil.StatusCode.Canceled || status == (int) CallUtil.StatusCode.Completed)
        //    {
        //        return true;
        //    }

        //    return false;
        //}
        //public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        //{   // Optional - default text is 'Delete'
        //    return "Slet";
        //}
    }
}
