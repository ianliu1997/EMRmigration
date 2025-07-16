using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamic.Localization;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTSheetDetails : UserControl
    {

        #region Data Members

        WaitIndicator ObjWiaitIndicator = null;
        LocalizationManager ObjLocalizationManager = null;
        public string msgText = string.Empty;
        public bool lIsEmergency { get; set; }
        public long lScheduleID { get; set; }
        public long PatientID { get; set; }
        public long lOTDetailsID { get; set; }       
        #endregion

        #region Constructor and Loaded
        public frmOTSheetDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmOTSheetDetails_Loaded);
            ObjWiaitIndicator = new WaitIndicator();
            ObjLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
        }

        void frmOTSheetDetails_Loaded(object sender, RoutedEventArgs e)
        {
            FetchOT();
            //FetchAnesthesiaType();
            //FetchProcedureType();
            //FetchOTResult();
            //FetchOTStatus();


            //if (this.lOTDetailsID > 0)
            //{
            //FillDetailTablesOfOTDetails(lOTDetailsID);
            //}
            //else
            //    FillDetailsOfProcedureSchedule(lScheduleID);

            //if (this.lScheduleID > 0 && this.PatientID > 0)
            //    FillDetailTablesOfOTDetails(lOTDetailsID);

        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region FillCombo





        /// <summary>
        /// Fetches anesthesia type
        /// </summary>
        private void FetchAnesthesiaType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_AnesthesiaTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbAnesthesia.ItemsSource = null;
                        CmbAnesthesia.ItemsSource = objList;
                        CmbAnesthesia.SelectedItem = objList[0];
                        //CmbAnesthesia.SelectedValue = ((clsOTDetailsOTSheetDetailsVO)grdOTSheet.DataContext).AnesthesiaTypeID;
                        FetchProcedureType();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches procedure type
        /// </summary>
        private void FetchProcedureType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbProcedureType.ItemsSource = null;
                        CmbProcedureType.ItemsSource = objList;
                        CmbProcedureType.SelectedItem = objList[0];                        
                        //CmbProcedureType.SelectedValue = ((clsOTDetailsOTSheetDetailsVO)grdOTSheet.DataContext).ProcedureTypeID;
                        FetchOTResult();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches OT Result
        /// </summary>
        private void FetchOTResult()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OperationResultMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOTResult.ItemsSource = null;
                        CmbOTResult.ItemsSource = objList;
                        CmbOTResult.SelectedItem = objList[0];
                        //CmbOTResult.SelectedValue = ((clsOTDetailsOTSheetDetailsVO)grdOTSheet.DataContext).OTResultID;
                        FetchOTStatus();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches OT Result
        /// </summary>
        private void FetchOTStatus()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OperationStatusMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOTStatus.ItemsSource = null;
                        CmbOTStatus.ItemsSource = objList;
                        CmbOTStatus.SelectedItem = objList[0];
                        //CmbOTStatus.SelectedValue = ((clsOTDetailsOTSheetDetailsVO)grdOTSheet.DataContext).OTStatusID;
                        FillDetailsOfProcedureSchedule(lScheduleID);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch Operation Theatre
        /// </summary>
        private void FetchOT()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOT.ItemsSource = null;
                        CmbOT.ItemsSource = objList;
                        CmbOT.SelectedItem = objList[0];
                        FetchAnesthesiaType();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Button Click Events
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveOTSheetDetails();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
        #endregion

        #region Private Methods
        private void ClearUI()
        {
            tpFromtime.Value = null;
            tpToTime1.Value = null;
            procDate1.SelectedDate = null;
            CmbOT.SelectedValue = (long)0;
            txttotalHours.Text = string.Empty;
            CmbAnesthesia.SelectedValue = (long)0;
            CmbProcedureType.SelectedValue = (long)0;
            CmbOTResult.SelectedValue = (long)0;
            CmbOTStatus.SelectedValue = (long)0;
            tpAnesthesiaStartTime.Value = null;
            tpAnesthesiaEndTime.Value = null;
            tpWheelITime.Value = null;
            tpWheelOutTime.Value = null;
            tpSurgeryStartTime.Value = null;
            tpSurgeryEndTime.Value = null;
            txtManPower.Text = string.Empty;
            txtRemark.Text = string.Empty;

        }

        private void SaveOTSheetDetails()
        {
            try
            {
                clsAddupdatOtDetailsBizActionVO BizAction = new clsAddupdatOtDetailsBizActionVO();
                BizAction.objOTDetails.objOtSheetDetails = new clsOTDetailsOTSheetDetailsVO();
                BizAction.objOTDetails.ID = this.lOTDetailsID;
                BizAction.objOTDetails.PatientID = PatientID;
                BizAction.objOTDetails.ScheduleID = lScheduleID;
                BizAction.objOTDetails.IsEmergency = lIsEmergency;
                BizAction.objOTDetails.objOtSheetDetails.FromTime = tpFromtime.Value;
                BizAction.objOTDetails.objOtSheetDetails.ToTime = tpToTime1.Value;
                BizAction.objOTDetails.objOtSheetDetails.Date = procDate1.SelectedDate;
                BizAction.objOTDetails.objOtSheetDetails.AnesthesiaStartTime = tpAnesthesiaStartTime.Value;
                BizAction.objOTDetails.objOtSheetDetails.AnesthesiaEndTime = tpAnesthesiaEndTime.Value;
                BizAction.objOTDetails.objOtSheetDetails.WheelInTime = tpWheelITime.Value;
                BizAction.objOTDetails.objOtSheetDetails.WheelOutTime = tpWheelOutTime.Value;
                BizAction.objOTDetails.objOtSheetDetails.SurgeryStartTime = tpSurgeryStartTime.Value;
                BizAction.objOTDetails.objOtSheetDetails.SurgeryEndTime = tpSurgeryEndTime.Value;

                if (!string.IsNullOrEmpty(txttotalHours.Text))
                    BizAction.objOTDetails.objOtSheetDetails.TotalHours = txttotalHours.Text;
                if (!string.IsNullOrEmpty(txtManPower.Text))
                    BizAction.objOTDetails.objOtSheetDetails.ManPower = Convert.ToInt64(txtManPower.Text);

                if (!string.IsNullOrEmpty(txtRemark.Text))
                    BizAction.objOTDetails.objOtSheetDetails.Remark = txtRemark.Text;

                if (!string.IsNullOrEmpty(txtSpecialRequirement.Text))
                    BizAction.objOTDetails.objOtSheetDetails.SpecialRequirement = txtSpecialRequirement.Text;

                if (CmbOT.SelectedItem != null)
                    BizAction.objOTDetails.objOtSheetDetails.OTID = ((MasterListItem)CmbOT.SelectedItem).ID;

                if (CmbAnesthesia.SelectedItem != null)
                    BizAction.objOTDetails.objOtSheetDetails.AnesthesiaTypeID = ((MasterListItem)CmbAnesthesia.SelectedItem).ID;

                if (CmbProcedureType.SelectedItem != null)
                    BizAction.objOTDetails.objOtSheetDetails.ProcedureTypeID = ((MasterListItem)CmbProcedureType.SelectedItem).ID;

                if (CmbOTResult.SelectedItem != null)
                    BizAction.objOTDetails.objOtSheetDetails.OTResultID = ((MasterListItem)CmbOTResult.SelectedItem).ID;

                if (CmbOTStatus.SelectedItem != null)
                    BizAction.objOTDetails.objOtSheetDetails.OTStatusID = ((MasterListItem)CmbOTStatus.SelectedItem).ID;

                //if (chkEmergency.IsChecked == true)
                //    BizAction.objOTDetails.objOtSheetDetails.IsEmergency = true;
                //else
                //    BizAction.objOTDetails.objOtSheetDetails.IsEmergency = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddupdatOtDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                        {

                            //if (ObjLocalizationManager != null)
                            //{
                            //    msgText = ObjLocalizationManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                //msgText = "Record saved successfully.";
                                //string unitid = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString();
                                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/OperationTheatre/OTSchedulesDetailsReport.aspx?PatientID=" + PatientID + "&ScheduleID=" + lScheduleID + "&OTDetailsID=" + lOTDetailsID + "&UnitID=" + unitid), "_blank");
      
                            //}
                            //ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            frmOTDetails winOTDetails;
                            winOTDetails = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmOTDetails;
                            winOTDetails.lOTDetailsID = ((clsAddupdatOtDetailsBizActionVO)arg.Result).objOTDetails.ID;                           
                            TreeView tvOTDetails = winOTDetails.FindName("tvOTDetails") as TreeView;
                            (tvOTDetails.Items[1] as TreeViewItem).IsSelected = true;
                            
                        }
                    }
                    else
                    {
                        //if (ObjLocalizationManager != null)
                        //{
                        //    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private bool ValidateControls()
        {
            bool result = true;
            if (lScheduleID == 0 || lScheduleID == null)
            {
                msgText = "Please select schedule.";
                //msgText = ObjLocalizationManager.GetValue("ScheduleVlidation_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                result = false;
                return result;
            }

            //if (String.IsNullOrEmpty(txtRemark.Text))
            //{
            //    msgText = ObjLocalizationManager.GetValue("RemarkValidation_Msg");
            //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    result = false;
            //    return result;
            //}
            return result;
        }

        public void FillDetailsOfProcedureSchedule(long ScheduleID)
        {
            try
            {
                if (ObjWiaitIndicator == null)
                    ObjWiaitIndicator = new WaitIndicator();
                if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWiaitIndicator.Show();
                }
                else
                {
                    ObjWiaitIndicator.Close();
                    ObjWiaitIndicator.Show();
                }
                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                BizAction.ScheduleID = ScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        ClearUI();
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;

                        if (DetailsVO.OTScheduleList != null && DetailsVO.OTScheduleList.Count > 0)
                        {
                            List<MasterListItem> OTList = new List<MasterListItem>();
                            foreach (var item in DetailsVO.OTScheduleList)
                            {
                                MasterListItem OTObject = new MasterListItem();
                                OTObject.ID = (long)item.OTID;
                                OTList.Add(OTObject);
                                tpFromtime.Value = item.StartTime;
                                tpToTime1.Value = item.EndTime;
                                CmbOT.SelectedValue = item.OTID;
                                procDate1.SelectedDate = item.Date;
                                txttotalHours.Text = Convert.ToString(item.EndTime - item.StartTime);
                                txtRemark.Text = Convert.ToString(item.Remarks);
                                txtSpecialRequirement.Text = Convert.ToString(item.SpecialRequirement);
                            }
                        }

                        if (DetailsVO.PatientProcList != null && DetailsVO.PatientProcList.Count > 0)
                        {
                            foreach (var item in DetailsVO.PatientProcList)
                            {
                                MasterListItem OTObject = new MasterListItem();
                                CmbAnesthesia.SelectedValue = item.AnesthesiaTypeID;
                                CmbProcedureType.SelectedValue = item.ProcedureTypeID;
                            }
                        }

                        if (this.lScheduleID > 0 && this.PatientID > 0)
                            FillDetailTablesOfOTDetails(lOTDetailsID);
                    }
                    else
                    {
                        //if (ObjLocalizationManager != null)
                        //{
                        //    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWiaitIndicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillDetailTablesOfOTDetails(long OtDetailsID)
        {
            try
            {
                if (ObjWiaitIndicator == null)
                    ObjWiaitIndicator = new WaitIndicator();
                if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWiaitIndicator.Show();
                }
                else
                {
                    ObjWiaitIndicator.Close();
                    ObjWiaitIndicator.Show();
                }
                clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO BizAction = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
                BizAction.PatientID = this.PatientID;
                BizAction.ScheduleId = this.lScheduleID;
                //BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        //ClearUI();
                        clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO DetailsVO = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
                        DetailsVO = (clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO)arg.Result;
                        if (DetailsVO.OTSheetDetailsObj != null)
                        {
                            grdOTSheet.DataContext = DetailsVO.OTSheetDetailsObj;
                            if (DetailsVO.OTSheetDetailsObj.FromTime != null)
                                tpFromtime.Value = DetailsVO.OTSheetDetailsObj.FromTime;
                            if (DetailsVO.OTSheetDetailsObj.ToTime != null)
                                tpToTime1.Value = DetailsVO.OTSheetDetailsObj.ToTime;
                            procDate1.SelectedDate = DetailsVO.OTSheetDetailsObj.Date;
                            CmbOT.SelectedValue = DetailsVO.OTSheetDetailsObj.OTID;
                            txttotalHours.Text = (tpToTime1.Value - tpFromtime.Value).ToString();
                            CmbAnesthesia.SelectedValue = DetailsVO.OTSheetDetailsObj.AnesthesiaTypeID;
                            CmbProcedureType.SelectedValue = DetailsVO.OTSheetDetailsObj.ProcedureTypeID;
                            CmbOTResult.SelectedValue = DetailsVO.OTSheetDetailsObj.OTResultID;
                            CmbOTStatus.SelectedValue = DetailsVO.OTSheetDetailsObj.OTStatusID;
                            txtManPower.Text = Convert.ToString(DetailsVO.OTSheetDetailsObj.ManPower);
                            //if (DetailsVO.OTSheetDetailsObj.Remark != null)
                            if (txtRemark.Text != null || txtRemark.Text != "")
                            {
                                txtRemark.Text = DetailsVO.OTSheetDetailsObj.Remark;
                            }
                            if (txtSpecialRequirement.Text != null || txtSpecialRequirement.Text != "")
                            {
                                txtSpecialRequirement.Text = DetailsVO.OTSheetDetailsObj.SpecialRequirement;
                            }
                            
                           // txtSpecialRequirement.Text = DetailsVO.OTSheetDetailsObj.SpecialRequirement;
                            tpAnesthesiaStartTime.Value = DetailsVO.OTSheetDetailsObj.AnesthesiaStartTime;
                            tpAnesthesiaEndTime.Value = DetailsVO.OTSheetDetailsObj.AnesthesiaEndTime;
                            tpWheelITime.Value = DetailsVO.OTSheetDetailsObj.WheelInTime;
                            tpWheelOutTime.Value = DetailsVO.OTSheetDetailsObj.WheelOutTime;
                            tpSurgeryStartTime.Value = DetailsVO.OTSheetDetailsObj.SurgeryStartTime;
                            tpSurgeryEndTime.Value = DetailsVO.OTSheetDetailsObj.SurgeryEndTime;
                            cmdModify.IsEnabled = true;
                            cmdSave.IsEnabled = false;
                        }

                    }
                    else
                    {
                        //if (ObjLocalizationManager != null)
                        //{
                        //    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWiaitIndicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveOTSheetDetails();
            }
        }

        //public void FillOTSheetDetailsByOTID()
        //{
        //    try
        //    {
        //        if (ObjWiaitIndicator == null)
        //            ObjWiaitIndicator = new WaitIndicator();
        //        if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
        //        {
        //            ObjWiaitIndicator.Show();
        //        }
        //        else
        //        {
        //            ObjWiaitIndicator.Close();
        //            ObjWiaitIndicator.Show();
        //        }

        //        clsGetOTSheetDetailsBizActionVO BizAction = new clsGetOTSheetDetailsBizActionVO();
        //        BizAction.objOTDetails.PatientID = this.PatientID;
        //        BizAction.objOTDetails.ScheduleID = this.lScheduleID;

        //        //clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO BizAction = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
        //        //BizAction.OTDetailsID = OtDetailsID;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                ClearUI();
        //                clsGetOTSheetDetailsBizActionVO DetailsVO = new clsGetOTSheetDetailsBizActionVO();
        //                DetailsVO = (clsGetOTSheetDetailsBizActionVO)arg.Result;
        //                grdOTSheet.DataContext = DetailsVO.objOTDetails.objOtSheetDetails;


        //                //grdOTSheet.DataContext = DetailsVO.OTSheetDetailsObj;
        //                //if (DetailsVO.OTSheetDetailsObj.FromTime != null)
        //                //    tpFromtime.Value = DetailsVO.OTSheetDetailsObj.FromTime;
        //                //if (DetailsVO.OTSheetDetailsObj.ToTime != null)
        //                //    tpToTime1.Value = DetailsVO.OTSheetDetailsObj.ToTime;
        //                //procDate1.SelectedDate = DetailsVO.OTSheetDetailsObj.Date;
        //                //CmbOT.SelectedValue = DetailsVO.OTSheetDetailsObj.OTID;
        //                //txttotalHours.Text = (tpToTime1.Value - tpFromtime.Value).ToString();
        //                //CmbAnesthesia.SelectedValue = DetailsVO.OTSheetDetailsObj.AnesthesiaTypeID;
        //                //CmbProcedureType.SelectedValue = DetailsVO.OTSheetDetailsObj.ProcedureTypeID;
        //                //CmbOTResult.SelectedValue = DetailsVO.OTSheetDetailsObj.OTResultID;
        //                //CmbOTStatus.SelectedValue = DetailsVO.OTSheetDetailsObj.OTStatusID;
        //                //tpAnesthesiaStartTime.Value = DetailsVO.OTSheetDetailsObj.AnesthesiaStartTime;
        //                //tpAnesthesiaEndTime.Value = DetailsVO.OTSheetDetailsObj.AnesthesiaStartTime;
        //                //tpWheelITime.Value = DetailsVO.OTSheetDetailsObj.WheelInTime;
        //                //tpWheelOutTime.Value = DetailsVO.OTSheetDetailsObj.WheelOutTime;
        //                //tpSurgeryStartTime.Value = DetailsVO.OTSheetDetailsObj.SurgeryStartTime;
        //                //tpSurgeryEndTime.Value = DetailsVO.OTSheetDetailsObj.SurgeryEndTime;
        //                //txtManPower.Text = Convert.ToString(DetailsVO.OTSheetDetailsObj.ManPower);
        //                //txtRemark.Text = DetailsVO.OTSheetDetailsObj.Remark;

        //            }
        //            else
        //            {
        //                if (ObjLocalizationManager != null)
        //                {
        //                    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
        //                }
        //                else
        //                {
        //                    msgText = "Error occured while processing.";
        //                }
        //                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            }
        //            ObjWiaitIndicator.Close();
        //        };

        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        #endregion
    }
}
