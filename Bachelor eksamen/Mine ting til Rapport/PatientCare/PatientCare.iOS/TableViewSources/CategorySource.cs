using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Foundation;
using PatientCare.Shared.Model;
using UIKit;

namespace PatientCare.iOS.TableViewSources
{
    public class CategoryCell : UICollectionViewCell
    {
        public static NSString CellID = new NSString("CategorySource");

        [Export("initWithFrame:")]
        public CategoryCell(RectangleF frame)
            : base(frame)
        {
            InitializeImageView();

            InitializeLabelView();
        }

        public UIImageView ImageView { get; private set; }

        public UILabel LabelView { get; private set; }

        public void InitializeImageView()
        {
            ImageView = new UIImageView();
            ImageView.BackgroundColor = UIColor.White;
            ImageView.Layer.BorderColor = UIColor.DarkGray.CGColor;
            ImageView.Layer.BorderWidth = 1f;
            ImageView.Layer.CornerRadius = 3f;
            ImageView.Layer.MasksToBounds = true;
            ImageView.ContentMode = UIViewContentMode.ScaleToFill;

            ContentView.AddSubview(ImageView);
        }

        public void InitializeLabelView()
        {
            LabelView = new UILabel();
            LabelView.BackgroundColor = UIColor.Clear;
            LabelView.TextColor = UIColor.Black;
            LabelView.TextAlignment = UITextAlignment.Center;

            ContentView.AddSubview(LabelView);
        }

        public void UpdateRow(CategoryEntity category, Single fontSize, SizeF imageViewSize)
        {
            LabelView.Text = category.Name;
            
            if (category.Picture != null && category.Picture != "BsonNull")
            {
                try
                {
                    var webClient = new WebClient();
                    webClient.DownloadDataCompleted += (s, e) =>
                    {
                        var bytes = e.Result; // get the downloaded data
                            ImageView.Image = ImageHandler.BytesToImage(bytes); // convert the data to an actual image
                    };
                    webClient.DownloadDataAsync(new Uri(category.Picture));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong loading image for cell..." + ex.Message);
                }
                
            }
     
            LabelView.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

            ImageView.Frame = new RectangleF(0, 0, imageViewSize.Width, imageViewSize.Height);
            // Position category label below image
            LabelView.Frame = new RectangleF(0, (float) ImageView.Frame.Bottom+60, imageViewSize.Width, (float) (ContentView.Frame.Height - ImageView.Frame.Bottom));
        }
    }

    public class CategorySource : UICollectionViewSource
    {
        public CategorySource()
        {
            Rows = new List<CategoryEntity>();
        }

        public List<CategoryEntity> Rows { get; private set; }

        public Single FontSize { get; set; }

        public SizeF ImageViewSize { get; set; }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint nint)
        {
            return Rows.Count;
        }

        public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CategoryCell)collectionView.CellForItem(indexPath);
            cell.ImageView.Alpha = 0.5f;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CategoryCell)collectionView.CellForItem(indexPath);
            cell.ImageView.Alpha = 1;

            CategoryEntity row = Rows[indexPath.Row];
            row.Tapped.Invoke();
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CategoryCell)collectionView.DequeueReusableCell(CategoryCell.CellID, indexPath);
            cell.InitializeImageView();
            cell.InitializeLabelView();
            CategoryEntity row = Rows[indexPath.Row];

            cell.UpdateRow(row, FontSize, ImageViewSize);

            return cell;
        }

    }
}
