using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamic.Localization;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmDocEmpDetails : UserControl
    {
        #region Variable Declaration
        List<clsOTDetailsDocDetailsVO> docList = new List<clsOTDetailsDocDetailsVO>();
        List<MasterListItem> ProcedureList = new List<MasterListItem>();
        List<clsOTDetailsStaffDetailsVO> StaffList = new List<clsOTDetailsStaffDetailsVO>();
        public bool lIsEmergency { get; set; }
        public long lScheduleID = 0;
        public long lPatientID = 0;
        public long lODetailsIDView = 0;
        public LocalizationManager ObjLocalizationManager = null;
        WaitIndicator ObjWiaitIndicator = null;
        public string msgText = string.Empty;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor and Loaded
        public frmDocEmpDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmDocEmpDetails_Loaded);
            ObjLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
        }

        private void frmDocEmpDetails_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.lODetailsIDView > 0)
            //{
            //    FillDetailTablesOfOTDetails(lODetailsIDView);
            //}
            //else
            FillDetailsOfProcedureSchedule(lScheduleID);

        }

        #endregion

        #region Private Methods

        long ProcedureId = 0;
        string procedureDesc = string.Empty;
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
                        dgDoctorList.ItemsSource = null;
                        dgStaffList.ItemsSource = null;
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;

                        if (DetailsVO.DocScheduleDetails != null && DetailsVO.DocScheduleDetails.Count > 0)
                        {
                            foreach (var item in DetailsVO.DocScheduleDetails)
                            {
                                clsOTDetailsDocDetailsVO docObject = new clsOTDetailsDocDetailsVO();
                                docObject.DesignationID = item.DocTypeID;
                                docObject.DoctorCode = item.DoctorCode;
                                docObject.SelectedProcedure.ID = item.ProcedureID;
                                ProcedureId = item.ProcedureID;
                                docObject.SelectedProcedure.Description = item.ProcedureName;
                                procedureDesc = item.ProcedureName;
                                docObject.designationDesc = item.docTypeDesc;
                                docObject.DoctorID = item.DocID;
                                docObject.docDesc = item.docDesc;
                                docObject.StrStartTime = item.StrStartTime;
                                docObject.StrEndTime = item.StrEndTime;

                                if (DetailsVO.AddedPatientProcList != null && DetailsVO.AddedPatientProcList.Count > 0)
                                {
                                    foreach (var item1 in DetailsVO.AddedPatientProcList)
                                    {
                                        MasterListItem procObj = new MasterListItem();
                                        procObj.ID = item1.ID;
                                        procObj.Description = item1.ProcDesc;
                                        if (DetailsVO.AddedPatientProcList.SingleOrDefault(S => S.ID.Equals(item1.ProcedureID)) == null)
                                            docObject.ProcedureList.Add(procObj);
                                    }
                                }
                                else if (DetailsVO.PatientProcList != null)
                                {

                                    docObject.ProcedureList.Add(docObject.SelectedProcedure);
                                    foreach (var item1 in DetailsVO.PatientProcList)
                                    {
                                        MasterListItem procObj = new MasterListItem();
                                        procObj.ID = item1.ProcedureID;
                                        procObj.Description = item1.ProcDesc;
                                        if (docObject.ProcedureList.SingleOrDefault(S => S.ID.Equals(item1.ProcedureID)) == null)
                                            docObject.ProcedureList.Add(procObj);
                                        ProcedureList.Add(procObj);
                                    }
                                }
                                docList.Add(docObject);
                            }
                            dgDoctorList.ItemsSource = null;
                            dgDoctorList.ItemsSource = docList;
                        }

                        if (DetailsVO.StaffDetailList != null && DetailsVO.StaffDetailList.Count > 0)
                        {

                            foreach (var item in DetailsVO.StaffDetailList)
                            {
                                clsOTDetailsStaffDetailsVO staffObject = new clsOTDetailsStaffDetailsVO();
                                staffObject.StaffID = item.StaffID;
                                staffObject.StaffDesc = item.stffDesc;
                                staffObject.SelectedProcedure.ID = item.ProcedureID;
                                staffObject.SelectedProcedure.Description = item.ProcedureName;


                                if (DetailsVO.AddedPatientProcList != null && DetailsVO.AddedPatientProcList.Count > 0)
                                {
                                    foreach (var item1 in DetailsVO.AddedPatientProcList)
                                    {
                                        MasterListItem procObj = new MasterListItem();
                                        procObj.ID = item1.ID;
                                        procObj.Description = item1.ProcDesc;
                                        if (DetailsVO.AddedPatientProcList.SingleOrDefault(S => S.ID.Equals(item1.ProcedureID)) == null)
                                            staffObject.ProcedureList.Add(procObj);
                                    }
                                }
                                else if (DetailsVO.PatientProcList != null)
                                {
                                    staffObject.ProcedureList.Add(staffObject.SelectedProcedure);
                                    foreach (var item1 in DetailsVO.PatientProcList)
                                    {
                                        MasterListItem procObj = new MasterListItem();
                                        procObj.ID = item1.ProcedureID;
                                        procObj.Description = item1.ProcDesc;
                                        if (staffObject.ProcedureList.SingleOrDefault(S => S.ID.Equals(item1.ProcedureID)) == null)
                                            staffObject.ProcedureList.Add(procObj);
                                    }
                                }

                                StaffList.Add(staffObject);
                            }
                            dgStaffList.ItemsSource = null;
                            dgStaffList.ItemsSource = StaffList;
                        }

                        if (this.lScheduleID > 0 && this.lPatientID > 0)
                            FillDetailTablesOfOTDetails(lODetailsIDView);
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
            WaitIndicator indicator = new WaitIndicator();
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
                clsGetDocEmpDetailsByOTDetailsIDBizActionVO BizAction = new clsGetDocEmpDetailsByOTDetailsIDBizActionVO();
                BizAction.DoctorList = new List<clsOTDetailsDocDetailsVO>();
                BizAction.StaffList = new List<clsOTDetailsStaffDetailsVO>();
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;
                BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetDocEmpDetailsByOTDetailsIDBizActionVO DetailsVO = new clsGetDocEmpDetailsByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetDocEmpDetailsByOTDetailsIDBizActionVO)arg.Result;
                        if (DetailsVO.ProcedureList != null && DetailsVO.ProcedureList.Count > 0)
                            ProcedureList = DetailsVO.ProcedureList;
                        if (DetailsVO.DoctorList != null && DetailsVO.DoctorList.Count > 0)
                        {
                            //docList.Clear();
                            dgDoctorList.ItemsSource = null;
                            foreach (var item in DetailsVO.DoctorList)
                            {
                                item.ProcedureList = DetailsVO.ProcedureList;

                                var ObjService = docList.SingleOrDefault(z => z.DoctorID.Equals(item.DoctorID));
                                if (ObjService != null)
                                    docList.Remove(ObjService);                               

                                //var ObjDoc = from r in docList
                                //             where (r.DoctorCode != item.DoctorCode)
                                //             select
                                //new clsOTDetailsDocDetailsVO
                                //{
                                //    ID = r.ID,
                                //    DesignationID = r.DesignationID,
                                //    DoctorID = r.DoctorID,
                                //    DoctorCode = r.DoctorCode,
                                //    ProcedureID = r.ProcedureID,
                                //    docDesc = r.docDesc,
                                //    StrStartTime = r.StrStartTime,
                                //    StrEndTime = r.StrEndTime,
                                //    designationDesc = r.designationDesc,
                                //    ProcedureName = r.ProcedureName
                                //};

                                //if (ObjDoc != null)
                                //{
                                //    docList.Add(item);
                                //}
                            }
                            foreach (var item in DetailsVO.DoctorList)
                            {
                                if (!docList.Where(z => z.DoctorID.Equals(item.DoctorID)).Any())
                                    docList.Add(item);
                            }

                            dgDoctorList.ItemsSource = null;
                            dgDoctorList.ItemsSource = docList;
                        }

                        if (DetailsVO.StaffList != null && DetailsVO.StaffList.Count > 0)
                        {
                            //StaffList.Clear();
                            dgStaffList.ItemsSource = null;                           

                            foreach (var item in DetailsVO.StaffList)
                            {
                                item.ProcedureList = DetailsVO.ProcedureList;
                                //item.ProcedureList = ProcedureList;

                                var ObjService = StaffList.SingleOrDefault(z => z.StaffID.Equals(item.StaffID));
                                if (ObjService != null)
                                    StaffList.Remove(ObjService);                                   
                            }

                            foreach (var item in DetailsVO.StaffList)
                            {
                                var objStaff = from r in StaffList
                                               where (r.StaffID == item.StaffID)
                                               select new clsOTDetailsStaffDetailsVO
                                               {
                                                   ID = r.ID,
                                                   StaffID = r.StaffID,
                                                   ProcedureID = r.ProcedureID,
                                                   StaffDesc = r.StaffDesc,
                                                   ProcedureName = r.ProcedureName
                                               };
                                if (objStaff.ToList().Count == 0)
                                {
                                    StaffList.Add(item);
                                }
                            }

                            dgStaffList.ItemsSource = null;
                            dgStaffList.ItemsSource = StaffList;
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
                    ObjWiaitIndicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                indicator.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SaveDocEmpDetails()
        {
            try
            {
                clsAddUpdatOtDocEmpDetailsBizActionVO BizAction = new clsAddUpdatOtDocEmpDetailsBizActionVO();
                if (dgDoctorList.ItemsSource != null)
                    foreach (var item in dgDoctorList.ItemsSource)
                    {
                        ((clsOTDetailsDocDetailsVO)item).ProcedureID = ((clsOTDetailsDocDetailsVO)item).SelectedProcedure.ID;
                        BizAction.objOTDetails.DocList.Add((clsOTDetailsDocDetailsVO)item);
                    }

                if (dgStaffList.ItemsSource != null)
                    foreach (var item in dgStaffList.ItemsSource)
                    {
                        ((clsOTDetailsStaffDetailsVO)item).ProcedureID = ((clsOTDetailsStaffDetailsVO)item).SelectedProcedure.ID;
                        BizAction.objOTDetails.StaffList.Add((clsOTDetailsStaffDetailsVO)item);
                    }

                frmOTDetails winOTDetails;
                winOTDetails = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmOTDetails;
                BizAction.objOTDetails.ID = this.lODetailsIDView;
                BizAction.objOTDetails.PatientID = lPatientID;
                BizAction.objOTDetails.ScheduleID = lScheduleID;
                BizAction.objOTDetails.IsEmergency = lIsEmergency;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdatOtDocEmpDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                        {

                            //if (ObjLocalizationManager != null)
                            //{
                            //    msgText = ObjLocalizationManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record saved successfully.";
                            //}
                            //ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            TreeView tvOTDetails = winOTDetails.FindName("tvOTDetails") as TreeView;
                            (tvOTDetails.Items[3] as TreeViewItem).IsSelected = true;

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
            return result;
        }
        #endregion

        #region Click Event

        /// <summary>
        /// Get doctor button 
        /// </summary>
        private void cmdGetDoctors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoctorSearch docwin = new DoctorSearch();
                docwin.Width = this.ActualWidth * 0.75;
                docwin.Height = this.ActualHeight * 0.95;
                docwin.OnAddButton_Click += new RoutedEventHandler(docwin_OnAddButton_Click);
                docwin.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Doctor window add button click
        /// </summary>
        void docwin_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoctorSearch docwin = (DoctorSearch)sender;
                //if(docwin.AddedDoctorList!=null)
                if (docwin.DoctorList != null)
                {
                    foreach (MasterListItem item in docwin.AddedDoctorList.ToList())
                    //foreach (MasterListItem item in docwin.DoctorList.ToList())
                    {
                        //if (!docList.Where(z => z.DoctorCode == item.Code).Any() && item.Status == true)
                        if (!docList.Where(z => z.DoctorID == item.ID).Any() && item.Status == true)
                        {
                            clsOTDetailsDocDetailsVO docObj = new clsOTDetailsDocDetailsVO();
                            docObj.DoctorID = item.ID;
                            docObj.Code = item.Code;
                            docObj.docDesc = item.Description;
                            docObj.DoctorCode = item.Code;
                            //MasterListItem tempObj = new MasterListItem();
                            //tempObj.ID = 0;
                            //tempObj.Description = "-- Select --";
                            //docObj.ProcedureList.Add(tempObj);
                            foreach (MasterListItem item1 in ProcedureList)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item1.ID;
                                obj.Description = item1.Description;
                                if (docObj.ProcedureList.SingleOrDefault(S => S.ID.Equals(item1.ID)) == null)
                                    docObj.ProcedureList.Add(obj);
                                docObj.SelectedProcedure.ID = obj.ID;
                                docObj.SelectedProcedure.Description = obj.Description;
                            }
                            //docObj.SelectedProcedure.ID = ProcedureId;
                            //docObj.SelectedProcedure.Description = procedureDesc;

                            docList.Add(docObj);
                        }
                    }
                }

                dgDoctorList.ItemsSource = null;
                dgDoctorList.ItemsSource = docList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Modifies OT Details
        /// </summary>
        private void cmdGetStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaffSearch staffWin = new StaffSearch();
                staffWin.Width = this.ActualWidth * 0.75;
                staffWin.Height = this.ActualHeight * 0.95;
                staffWin.OnAddButton_Click += new RoutedEventHandler(staffWin_OnAddButton_Click);
                staffWin.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Staff Search window ok button click
        /// </summary>
        void staffWin_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaffSearch docwin = (StaffSearch)sender;
                if(docwin.AddedStaffList!=null)
                //if (docwin.OTDetailsStaffList != null)
                {
                    //foreach (var item in docwin.OTDetailsStaffList)
                    foreach (var item in docwin.AddedStaffList)
                    {
                        if (!StaffList.Contains(item))
                        {
                            if (item.Status == true)
                            {
                                clsOTDetailsStaffDetailsVO staffObj = new clsOTDetailsStaffDetailsVO();
                                if (StaffList.SingleOrDefault(S => S.StaffID.Equals(item.StaffID)) == null)
                                {
                                    staffObj.StaffID = item.StaffID;
                                    staffObj.StaffDesc = item.StaffDesc;
                                    //MasterListItem tempObj = new MasterListItem();
                                    //tempObj.ID = 0;
                                    //tempObj.Description = "-- Select --";
                                    //staffObj.ProcedureList.Add(tempObj);
                                    foreach (var item1 in ProcedureList)
                                    {
                                        MasterListItem obj = new MasterListItem();
                                        obj.ID = item1.ID;
                                        obj.Description = item1.Description;
                                        if (staffObj.ProcedureList.SingleOrDefault(S => S.ID.Equals(item1.ID)) == null)
                                            staffObj.ProcedureList.Add(obj);
                                        staffObj.SelectedProcedure.ID = obj.ID;
                                        staffObj.SelectedProcedure.Description = obj.Description;
                                    }

                                    StaffList.Add(staffObj);
                                }

                            }
                        }
                    }
                }

                dgStaffList.ItemsSource = null;
                dgStaffList.ItemsSource = StaffList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveDocEmpDetails();

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDoctorList.SelectedItem != null)
                {
                    string msgTitle = "";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)                        
                       // msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");
                    
                    msgText = "Are you sure you want to delete record ?";
                    int index = dgDoctorList.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            docList.RemoveAt(index);
                            dgDoctorList.ItemsSource = null;
                            dgDoctorList.ItemsSource = docList;
                            dgDoctorList.UpdateLayout();
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
        #endregion

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveDocEmpDetails();
        }

        private void cmdDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgStaffList.SelectedItem != null)
                {
                    string msgTitle = "";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)                       
                        //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");
                    msgText = "Are you sure you want to delete record ?";
                    int index = dgStaffList.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            StaffList.RemoveAt(index);
                            dgStaffList.ItemsSource = null;
                            dgStaffList.ItemsSource = StaffList;
                            dgStaffList.UpdateLayout();
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
