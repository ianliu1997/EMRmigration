using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamic.Localization;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmPostInstructionDetails : UserControl
    {
        #region DataMember
        ObservableCollection<clsProcedureInstructionDetailsVO> PostInstructionList;
        string msgText = string.Empty;
        LocalizationManager objLocalizationManager = null;
        public bool lIsEmergency { get; set; }
        public long lOTDetailsIDView { get; set; }
        public long lScheduleID = 0;
        public long lPatientID = 0;
        public List<clsOTDetailsPostInstructionDetailsVO> PostInsList;
        WaitIndicator ObjWaitIndicator = null;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor
        public frmPostInstructionDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmPostInstructionDetails_Loaded);
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            ObjWaitIndicator = new WaitIndicator();
        }

        void frmPostInstructionDetails_Loaded(object sender, RoutedEventArgs e)
        {
            FillInstructioncmb();

            //if (lOTDetailsIDView > 0)
            //{
            //    FillDetailTablesOfOTDetails(lOTDetailsIDView);

            //}
            //else
            //    FillDetailsOfProcedureSchedule(lScheduleID);

            if (this.lScheduleID > 0 && this.lPatientID > 0)
                FillDetailTablesOfOTDetails(lOTDetailsIDView);
        }
        #endregion

        #region Fill ComboBox

        /// <summary>
        /// Fetch post Instruction According To Procedure
        /// </summary>
        private void FillInstructioncmb()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.MasterTable = MasterTableNameList.M_PostOperativeInstructionsMaster;
                BizAction.MasterTable = MasterTableNameList.M_DoctorNotes;
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
                        cmbPostInstruction.ItemsSource = null;
                        cmbPostInstruction.ItemsSource = objList;
                        cmbPostInstruction.SelectedItem = objList[0];
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }


            //List<MasterListItem> PostInstructionList = new List<MasterListItem>();
            //PostInstructionList.Add(new MasterListItem(0, "-- Select --"));
            //foreach (var item in procedureList)
            //{
            //    try
            //    {
            //        clsGetServicesForProcedureIDBizActionVO BizAction = new clsGetServicesForProcedureIDBizActionVO();
            //        BizAction.ProcedureID = item.ProcedureID;
            //        BizAction.ProcedureUnitID = item.ProcedureUnitID;
            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        Client.ProcessCompleted += (s, e) =>
            //        {
            //            if (e.Error == null && e.Result != null && ((clsGetServicesForProcedureIDBizActionVO)e.Result) != null)
            //            {
            //                foreach (var item1 in ((clsGetServicesForProcedureIDBizActionVO)e.Result).InstructionList)
            //                {
            //                    if (PostInstructionList.SingleOrDefault(S => S.ID.Equals(item1.PostInstructionID)) == null)
            //                    {
            //                        PostInstructionList.Add(new MasterListItem(item1.PostInstructionID, item1.PostInstruction));
            //                    }
            //                }
            //                cmbPostInstruction.ItemsSource = null;
            //                cmbPostInstruction.ItemsSource = PostInstructionList;
            //                cmbPostInstruction.SelectedItem = PostInstructionList[0];
            //            }
            //        };
            //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        Client.CloseAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}
        }

        #endregion

        #region Click Event
        /// <summary>
        /// CheckBox Click Event
        /// </summary>
        private void chkMultiplePostInstructions_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            TextBox txtTarget = txtPostInstructions;
            string strDescription = (chk.DataContext as MasterListItem).Description;
            if (chk.IsChecked == true && (chk.DataContext as MasterListItem).ID != 0 && !txtTarget.Text.Contains(strDescription))
            {
                txtTarget.Text = String.Format(txtTarget.Text + strDescription + "\r\n ").Trim(',');
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SavePostInstructions();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        #endregion

        #region Private Methods
        private void SavePostInstructions()
        {
            try
            {
                clsAddUpdateOTDoctorNotesDetailsBizActionVO BizAction = new clsAddUpdateOTDoctorNotesDetailsBizActionVO();
                BizAction.objOTDetails.DoctorNotesList.DoctorNotes = txtPostInstructions.Text;



                //clsAddUpdatOtPostInstructionDetailsBizActionVO BizAction = new clsAddUpdatOtPostInstructionDetailsBizActionVO();
                //BizAction.objOTDetails.DoctorNotesList.DoctorNotes = txtPostInstructions.Text;


                ////BizAction.objOTDetails.PostInstructionList.PostInstruction = txtPostInstructions.Text;

                frmOTDetails winOTDetails;
                winOTDetails = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmOTDetails;
                BizAction.objOTDetails.ID = winOTDetails.lOTDetailsID;
                BizAction.objOTDetails.PatientID = lPatientID;
                BizAction.objOTDetails.ScheduleID = lScheduleID;
                BizAction.objOTDetails.IsEmergency = lIsEmergency;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdateOTDoctorNotesDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                        {

                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record saved successfully.";
                            //}
                          //  ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            string unitid = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString();
                           // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/OperationTheatre/OTSchedulesDetailsReport.aspx?PatientID=" + lPatientID + "&ScheduleID=" + lScheduleID + "&OTDetailsID=" + winOTDetails.lOTDetailsID + "&UnitID=" + unitid), "_blank");
            

                            TreeView tvOTDetails = winOTDetails.FindName("tvOTDetails") as TreeView;
                            (tvOTDetails.Items[7] as TreeViewItem).IsSelected = true;
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

                clsGetDoctorNotesByOTDetailsIDBizActionVO BizAction = new clsGetDoctorNotesByOTDetailsIDBizActionVO();
                BizAction.DoctorNotes = new clsOTDetailsDoctorNotesDetailsVO();
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;
                BizAction.OTDetailsID = OtDetailsID;

                //clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO BizAction = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
                //BizAction.OTSheetDetailsObj = new clsOTDetailsOTSheetDetailsVO();
                //BizAction.ProcedureList = new List<clsPatientProcedureVO>();
                //BizAction.DoctorList = new List<clsOTDetailsDocDetailsVO>();
                //BizAction.StaffList = new List<clsOTDetailsStaffDetailsVO>();
                //BizAction.ItemList = new List<clsOTDetailsItemDetailsVO>();
                //BizAction.SurgeryNotesObj = new clsOTDetailsSurgeryDetailsVO();
                //BizAction.AnesthesiaNotesObj = new clsOtDetailsAnesthesiaNotesDetailsVO();
                //BizAction.ObjPostInstruction = new clsOTDetailsPostInstructionDetailsVO();
                //BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetDoctorNotesByOTDetailsIDBizActionVO DetailsVO = new clsGetDoctorNotesByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetDoctorNotesByOTDetailsIDBizActionVO)arg.Result;

                        //clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO DetailsVO = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
                        //DetailsVO = (clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO)arg.Result;

                        if (DetailsVO.DoctorNotes.DoctorNotes != null)
                        {
                            txtPostInstructions.Text = DetailsVO.DoctorNotes.DoctorNotes;
                            cmdModify.IsEnabled = true;
                            cmdSave.IsEnabled = false;
                            //PostInsList.Add(DetailsVO.ObjPostInstruction.PostInstruction);
                            //if (DetailsVO.ObjPostInstruction.PostInstruction != null)
                            //{
                            //    txtPostInstructions.Text = DetailsVO.ObjPostInstruction.PostInstruction;
                            //}
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
                BizAction.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
                BizAction.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
                BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                BizAction.ScheduleID = lScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        txtPostInstructions.Text = "";
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                        //procedureList = procedureList = new List<clsPatientProcedureVO>();
                        if (DetailsVO.PostInstructionList != null && DetailsVO.PostInstructionList.Count > 0)
                        {
                            txtPostInstructions.Text = string.Join(Environment.NewLine, DetailsVO.PostInstruction);
                            txtPostInstructions.Text = txtPostInstructions.Text + "\n";
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


        #endregion

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SavePostInstructions();
        }

    }
}
