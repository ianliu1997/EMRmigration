using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamic.Localization;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTSurgeryDetails : UserControl
    {
        #region Variable Declarations
        List<clsPatientProcedureVO> procList = new List<clsPatientProcedureVO>();
        LocalizationManager objLocalizationManager = null;
        WaitIndicator ObjWaitIndicator = null;
        public List<clsPatientProcedureVO> procedureList;
        List<MasterListItem> ProcedureList = new List<MasterListItem>();
        public long lScheduleID;
        public bool lIsEmergency { get; set; }
        public long lPatientID { get; set; }
        public long lOTDetailsID { get; set; }
        public long lOTDetailsIDView { get; set; }

        string msgText;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor and Loaded
        public frmOTSurgeryDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmOTSurgeryDetails_Loaded);
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;

        }

        private void frmOTSurgeryDetails_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.lOTDetailsIDView > 0)
            //{
            //    FillDetailTablesOfOTDetails(lOTDetailsIDView);
            //}

            //else
            //{
            FillDetailsOfProcedureSchedule(lScheduleID);
            //}

        }
        #endregion

        #region Private Methods
        private bool ValidateControls()
        {
            bool result = true;
            if (lScheduleID == 0 || lScheduleID == null)
            {
                msgText = "Please select schedule.";
                //msgText = objLocalizationManager.GetValue("ScheduleVlidation_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                result = false;
                return result;
            }
            return result;
        }

        public void FillDetailsOfProcedureSchedule(long ScheduleID)
        {
            try
            {
                if (ObjWaitIndicator == null)
                    ObjWaitIndicator = new WaitIndicator();
                if (ObjWaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWaitIndicator.Show();
                }
                else
                {
                    ObjWaitIndicator.Close();
                    ObjWaitIndicator.Show();
                }
                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                BizAction.ScheduleID = ScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        grdSurgery.ItemsSource = null;
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                        procedureList = procedureList = new List<clsPatientProcedureVO>();
                        if (DetailsVO.PatientProcList != null && DetailsVO.PatientProcList.Count > 0)
                        {
                            foreach (var item in DetailsVO.PatientProcList)
                            {
                                procedureList.Add(item);
                            }
                            grdSurgery.ItemsSource = null;
                            grdSurgery.ItemsSource = procedureList;
                            grdSurgery.Focus();
                            grdSurgery.UpdateLayout();
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
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWaitIndicator.Close();

                    if (this.lScheduleID > 0 && this.lPatientID > 0)
                        FillDetailTablesOfOTDetails(lOTDetailsIDView);

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
                if (ObjWaitIndicator == null)
                    ObjWaitIndicator = new WaitIndicator();
                if (ObjWaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWaitIndicator.Show();
                }
                else
                {
                    ObjWaitIndicator.Close();
                    ObjWaitIndicator.Show();
                }
                clsGetSurgeryDetailsByOTDetailsIDBizActionVO BizAction = new clsGetSurgeryDetailsByOTDetailsIDBizActionVO();
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;
                BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetSurgeryDetailsByOTDetailsIDBizActionVO DetailsVO = new clsGetSurgeryDetailsByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetSurgeryDetailsByOTDetailsIDBizActionVO)arg.Result;

                        if (DetailsVO.ProcedureList != null && DetailsVO.ProcedureList.Count > 0)
                        {
                            //procedureList = new List<clsPatientProcedureVO>();
                            foreach (var item in DetailsVO.ProcedureList)
                            {
                                var ObjService = procedureList.SingleOrDefault(z => z.ProcedureID == item.ProcedureID);
                                if (ObjService != null)
                                    procedureList.Remove(ObjService);                               

                            }

                            foreach (var item in DetailsVO.ProcedureList)
                            {
                                var proc = from r in procedureList
                                           where (r.ProcedureID == item.ProcedureID)
                                           select new clsPatientProcedureVO
                                           {
                                               ID = r.ID,
                                               Quantity = r.Quantity,
                                               ProcedureID = r.ProcedureID,
                                               ProcDesc = r.ProcDesc,
                                               IsEmergency = r.IsEmergency,
                                               IsHighRisk = r.IsHighRisk
                                           };

                                if (proc.ToList().Count == 0)
                                {
                                    procedureList.Add(item);
                                }
                            }

                            grdSurgery.ItemsSource = null;
                            grdSurgery.ItemsSource = procedureList;
                            grdSurgery.Focus();
                            grdSurgery.UpdateLayout();
                            cmdModify.IsEnabled = true;
                            cmdSave.IsEnabled = false;
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
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWaitIndicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void SaveOTSurgeryDetails()
        {
            try
            {
                clsAddUpdatOtSurgeryDetailsBizActionVO BizAction = new clsAddUpdatOtSurgeryDetailsBizActionVO();
                //if (grdSurgery.ItemsSource != null)
                //    foreach (var item in grdSurgery.ItemsSource)
                //    {   
                //        BizAction.objOTDetails.ProcedureList.Add((clsPatientProcedureVO)item);
                //    }


                if (grdSurgery.ItemsSource != null)
                    foreach (clsPatientProcedureVO item in grdSurgery.ItemsSource)
                    {
                        BizAction.objOTDetails.ProcedureList.Add((clsPatientProcedureVO)item);
                    }

                //BizAction.objOTDetails.ID = this.lOTDetailsID;
                frmOTDetails winOTDetails;
                winOTDetails = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmOTDetails;
                BizAction.objOTDetails.ID = this.lOTDetailsIDView;
                BizAction.objOTDetails.PatientID = lPatientID;
                BizAction.objOTDetails.ScheduleID = lScheduleID;
                BizAction.objOTDetails.IsEmergency = lIsEmergency;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdatOtSurgeryDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                        {

                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record saved successfully.";
                            //}
                            //ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            TreeView tvOTDetails = winOTDetails.FindName("tvOTDetails") as TreeView;
                            (tvOTDetails.Items[2] as TreeViewItem).IsSelected = true;
                        }

                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
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

        #endregion

        #region Click Events

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveOTSurgeryDetails();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        /// <summary>
        /// Gets Procedures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void hlProcedure_Click(object sender, RoutedEventArgs e)
        {
            frmProcedureSearch winProcedure = new frmProcedureSearch();
            winProcedure.OnAddButton_Click += new RoutedEventHandler(winProcedure_OnSaveButton_Click);
            winProcedure.Show();
        }

        /// <summary>
        /// Search Procedure Window Ok Button Click
        /// </summary>
        void winProcedure_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool blChkProc = false;
            frmProcedureSearch procwin = (frmProcedureSearch)sender;
            if(procwin.AddedprocedureList!=null)
            //if (procwin.procedureList != null)
            {
                if (procedureList == null)
                    procedureList = new List<clsPatientProcedureVO>();
                if (ProcedureList == null)
                    ProcedureList = new List<MasterListItem>();
                ProcedureList.Add(new MasterListItem(0, "--Select--"));

                //foreach (var item in procwin.procedureList)
                foreach (var item in procwin.AddedprocedureList)
                {
                    if (item.Status == true)
                    {
                        if (procedureList.Where(S => S.ProcedureID == item.ID).Any() == false)
                        {
                            clsPatientProcedureVO obj = new clsPatientProcedureVO();
                            obj.ProcedureID = item.ID;
                            obj.ProcDesc = item.Description;
                            obj.ProcedureUnitID = item.UnitID;
                            if (ProcedureList.SingleOrDefault(S => S.ID.Equals(item.ID)) == null)
                                ProcedureList.Add(new MasterListItem(item.ID, item.Description));
                            procedureList.Add(obj);
                        }
                        else
                        {
                            blChkProc = true;                            
                        }
                    }
                }
                if (blChkProc)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ProcedureAddInList_Msg");
                    //}
                    //else
                    //{
                        msgText = "Procedure is already added in the list";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    blChkProc = false;
                }
                grdSurgery.ItemsSource = null;
                grdSurgery.ItemsSource = procedureList;

            }
        }
        #endregion

        private void cmdNewProcedure_Click(object sender, RoutedEventArgs e)
        {
            frmProcedureSearch winProcedure = new frmProcedureSearch();
            //winProcedure.Height = ActualHeight - 150;
            //winProcedure.Width = ActualWidth - 250;
            winProcedure.Width = this.ActualWidth * 0.75;
            winProcedure.Height = this.ActualHeight * 0.95;
            winProcedure.OnAddButton_Click += new RoutedEventHandler(winProcedure_OnSaveButton_Click);
            winProcedure.Show();
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveOTSurgeryDetails();
            }
        }

        private void cmdDeleteSurgeryItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdSurgery.SelectedItem != null)
                {
                    string msgTitle = "";
                    msgText = "Are you sure you want to delete record ?";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");

                    int index = grdSurgery.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            procedureList.RemoveAt(index);
                            grdSurgery.ItemsSource = null;
                            grdSurgery.ItemsSource = procedureList;
                            grdSurgery.UpdateLayout();
                        }
                    };
                    msgWD.Show();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
