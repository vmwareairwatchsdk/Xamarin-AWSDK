﻿//  Copyright © 2019 VMware, Inc. All rights reserved. This product is protected
//  by copyright and intellectual property laws in the United States and other
//  countries as well as by international treaties. VMware products are covered
//  by one or more patents listed at http://www.vmware.com/go/patents.

using System;
using UIKit;
using AirWatchSDK;
using Foundation;
using CoreGraphics;

namespace XamarinSampleApp
{
	public partial class GeneralInfoViewController : UIViewController, IUITableViewDelegate, IUITableViewDataSource
	{
		UITableView tableView;
		AWController clientInstance;
		string[] generalInfoElements = new string[] { "SDK Version : ", "Server URL: ", "Group ID: ", "Username: " };

		public GeneralInfoViewController() : base("GeneralInfoViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			clientInstance = AWController.ClientInstance();
			setUpViews();
		}

		public void setUpViews()
		{
			this.Title = "Information";

            tableView = new UITableView(View.Bounds);
			tableView.DataSource = this;
			View.AddSubview(tableView);

            //remove additional empty cells at the end of table view
            tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 0));
   		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            tableView.Frame = View.Bounds;
        }

		public nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public nint RowsInSection(UITableView tableview, nint section)
		{
            return generalInfoElements.Length;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			string cellIdentifier = "cell";
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			nuint rowNo = (System.nuint)indexPath.Row;

			AWEnrollmentAccount account = clientInstance.Account;
			switch (rowNo)
			{
				case 0:
					cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + clientInstance.AWSDKVersion);
					break;

				case 1:
                    NSUrl dsUrl = AWServer.SharedInstance().DeviceServicesURL;
                    cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + "Not Recieved");
                    if (dsUrl != null && dsUrl.Host != null)
                    {
                        string url = dsUrl.Host;
                        cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + url);
                    }
					
				break;

				case 2:
                    cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + "Not Recieved");
                    if(account != null && account.ActivationCode != null)
                    {
						string gid = clientInstance.Account.ActivationCode;
						cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + gid);
                    }
					
				break;

				case 3:
                    cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + "Not Recieved");
                    if (account != null && account.Username != null)
                    {
						string username = clientInstance.Account.Username;
						cell.TextLabel.Text = string.Concat(generalInfoElements[rowNo] + username);
                    }
					
				break;

				default:
					cell.TextLabel.Text = "Title Not Set";
					break;
			}
			return cell;
		}
	}
}

