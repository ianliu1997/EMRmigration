using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmCancelBooking : ChildWindow
    {
        int ClickedFlag = 0;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        string msgText;
        public frmCancelBooking()
        {
            InitializeComponent();
            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            FillCancelledBy(UnitID);
        }

        /// <summary>
        /// OK button click
        /// </summary>   

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CancelleValidation())
            {
                ClickedFlag += 1;

                if (ClickedFlag == 1)
                {
                    if (OnSaveButton_Click != null)
                    {
                        this.DialogResult = true;
                        OnSaveButton_Click(this, new RoutedEventArgs());

                        this.Close();
                    }
                }
            }
        }
        private bool CancelleValidation( )
        {
            bool result = true ;
            if (txtCancelledReason.Text != "")
            {
                if (60>txtCancelledReason.Text.Length  )
                {

                    

                    MessageBoxControl.MessageBoxChildWindow msgWin =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter The  60 Character Cancellation Reason .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                    result = false;
                }

            }
            else
            {


                MessageBoxControl.MessageBoxChildWindow msgWin =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter The Cancellation Reason .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
                result = false;

            }

            return result;
        }

        /// <summary>
        /// Cancel Button Click
        /// </summary>
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            OnCancelButton_Click(this, new RoutedEventArgs());
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillCancelledBy(long UnitID)
        {
            if (!UnitID.Equals(0))
            {
                clsGetStaffMasterByUnitIDBizActionVO BizAction = new clsGetStaffMasterByUnitIDBizActionVO();
                BizAction.StaffMasterList = new List<clsStaffMasterVO>();
                BizAction.UnitID = UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        string UserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                        clsGetStaffMasterByUnitIDBizActionVO result = e.Result as clsGetStaffMasterByUnitIDBizActionVO;
                        List<MasterListItem> TempList = new List<MasterListItem>();
                        TempList.Add(new MasterListItem(0, "- Select -"));
                        if (result.StaffMasterList != null && result.StaffMasterList.Count > 0)
                        {
                            foreach (var item in result.StaffMasterList)
                            {
                                TempList.Add(new MasterListItem(item.ID, item.StaffName));
                            }
                            cmbCancelledBy.ItemsSource = null;
                            cmbCancelledBy.ItemsSource = TempList;
                            cmbCancelledBy.SelectedItem = TempList[0];
                        }
                    }
                    else
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }
    }
}

