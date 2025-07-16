using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTNotes : UserControl
    {
        #region Variable Declaration
        LocalizationManager ObjLocalizationManager = null;
        public List<clsOtDetailsAnesthesiaNotesDetailsVO> OTDetailsAnesthesiaNotesList;
        public List<clsOTDetailsSurgeryDetailsVO> OTDetailsSurgeyNotesList;
        List<clsOTDetailsInstructionListDetailsVO> AllInstructionList { get; set; }
        List<clsOTDetailsInstructionListDetailsVO> AllSurgeryList { get; set; }
        List<clsOTDetailsInstructionListDetailsVO> AllAnesthesiaList { get; set; }
        public bool lIsEmergency { get; set; }
        string msgText = string.Empty;
        public long lOTDetailsIDView = 0;
        public long lScheduleID = 0;
        public long lPatientID = 0;
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
        public frmOTNotes()
        {
            InitializeComponent();
            //FetchOTSurgeryNotes();
            //FetchOTAnesthesiaNotes();
            LoadNotesDetails();
            //FillOTNotesDetails();
            ObjLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            this.Loaded += new RoutedEventHandler(frmOTSurgeryDetails_Loaded);
            OTDetailsSurgeyNotesList = new List<clsOTDetailsSurgeryDetailsVO>();
            OTDetailsAnesthesiaNotesList = new List<clsOtDetailsAnesthesiaNotesDetailsVO>();
            ObjWaitIndicator = new WaitIndicator();
        }

        private void frmOTSurgeryDetails_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.lOTDetailsIDView > 0)
            //{
            //    FillDetailTablesOfOTDetails(lOTDetailsIDView);
            //}
            AllInstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
            AllAnesthesiaList = new List<clsOTDetailsInstructionListDetailsVO>();
            AllSurgeryList = new List<clsOTDetailsInstructionListDetailsVO>();
            FillDetailsOfProcedureScheduleNotes(lScheduleID);
            //FillDetailsOfProcedureSchedulePreNotes(lScheduleID);
            //FillDetailsOfProcedureScheduleIntraNotes(lScheduleID);
            //FillDetailsOfProcedureSchedulePostNotes(lScheduleID);
        }
        #endregion

        #region Fill ComboBox

        //public void FillOTNotesDetails()
        //{
        //    List<MasterListItem> detailsList = new List<MasterListItem>();

        //    MasterListItem defaultObj = new MasterListItem();
        //    defaultObj.ID = 0;
        //    defaultObj.Description = "-- Select --";
        //    detailsList.Add(defaultObj);

        //    MasterListItem preObj = new MasterListItem();
        //    preObj.ID = 1;
        //    preObj.Description = "Pre-OT Details";
        //    detailsList.Add(preObj);

        //    MasterListItem IntraObj = new MasterListItem();
        //    IntraObj.ID = 2;
        //    IntraObj.Description = "Intra-OT Details";
        //    detailsList.Add(IntraObj);

        //    MasterListItem PostObj = new MasterListItem();
        //    PostObj.ID = 3;
        //    PostObj.Description = "Post-OT Details";
        //    detailsList.Add(PostObj);

        //    CmbDetails.ItemsSource = null;
        //    CmbDetails.ItemsSource = detailsList;
        //    CmbDetails.SelectedItem = detailsList[0];

        //}



        private void FetchOTSurgeryNotes()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SurgeryNotes;
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
                        CmbSurgeryNotes.ItemsSource = null;
                        CmbSurgeryNotes.ItemsSource = objList;
                        CmbSurgeryNotes.SelectedItem = objList[0];
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

        private void FetchOTAnesthesiaNotes()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_AnesthesiaNotes;
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
                        CmbAnethesiaNotes.ItemsSource = null;
                        CmbAnethesiaNotes.ItemsSource = objList;
                        CmbAnethesiaNotes.SelectedItem = objList[0];
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

        #region Click Event

        private void chkMultipleSurNotes_Click(object sender, RoutedEventArgs e)
        {
            //AllSurgeryList = AllInstructionList.ToList();
            //AllAnesthesiaList = AllInstructionList.ToList();

            CheckBox chk = sender as CheckBox;
            string strDescription = (chk.DataContext as MasterListItem).Description;
            long ID=Convert.ToInt32((chk.DataContext as MasterListItem).ID );
            if( ID >  0)
            {
            string SelectedItem = CmbDetails.SelectedItem.ToString();
            if (SelectedItem.Equals("Pre-OT Details"))
            {
                if (SurgeryNotes.IsSelected)
                {
                    if (AllSurgeryList.Count > 0)
                    {
                        var item1 = from r in AllSurgeryList
                                    where (r.GroupName.Equals("Pre Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Pre Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllSurgeryList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Pre Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllSurgeryList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgSurgeryNotes.ItemsSource = pcv;
                    dgSurgeryNotes.UpdateLayout();
                    // dgSurgeryNotes.Focus();
                }
                if (AnesthesiaNotes.IsSelected)
                {
                    if (AllAnesthesiaList.Count > 0)
                    {
                        var item1 = from r in AllAnesthesiaList
                                    where (r.GroupName.Equals("Pre Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Pre Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllAnesthesiaList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Pre Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllAnesthesiaList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgAnesthesiaNotes.ItemsSource = pcv;
                    dgAnesthesiaNotes.UpdateLayout();
                    // dgAnesthesiaNotes.Focus();
                }

                //if (AllInstructionList.Count > 0)
                //{
                //    var item1 = from r in AllInstructionList
                //                where (r.GroupName.Equals("Pre Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                //                select new clsOTDetailsInstructionListDetailsVO
                //                {
                //                    GroupName = r.GroupName,
                //                    Instruction = r.Instruction
                //                };
                //    if (item1.ToList().Count == 0)
                //    {
                //        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //        obj.GroupName = "Pre Operative Instruction Notes";
                //        obj.Instruction = strDescription;
                //        if (SurgeryNotes.IsSelected)
                //            AllSurgeryList.Add(obj);
                //        else if (AnesthesiaNotes.IsSelected)
                //            AllAnesthesiaList.Add(obj);

                //        //AllInstructionList.Add(obj);
                //    }
                //}
                //else
                //{
                //    clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //    obj.GroupName = "Pre Operative Instruction Notes";
                //    obj.Instruction = strDescription;
                //    if (SurgeryNotes.IsSelected)
                //        AllSurgeryList.Add(obj);
                //    else if (AnesthesiaNotes.IsSelected)
                //        AllAnesthesiaList.Add(obj);
                //    //AllInstructionList.Add(obj);
                //}

                //if (SurgeryNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgSurgeryNotes.ItemsSource = pcv;
                //    dgSurgeryNotes.UpdateLayout();
                //    dgSurgeryNotes.Focus();
                //}
                //else if (AnesthesiaNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgAnesthesiaNotes.ItemsSource = pcv;
                //    dgAnesthesiaNotes.UpdateLayout();
                //    dgAnesthesiaNotes.Focus();
                //}
            }
            else if (SelectedItem.Equals("Intra-OT Details"))
            {
                if (SurgeryNotes.IsSelected)
                {
                    if (AllSurgeryList.Count > 0)
                    {
                        var item1 = from r in AllSurgeryList
                                    where (r.GroupName.Equals("Intra Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Intra Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllSurgeryList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Intra Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllSurgeryList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgSurgeryNotes.ItemsSource = pcv;
                    dgSurgeryNotes.UpdateLayout();
                    //dgSurgeryNotes.Focus();
                }
                if (AnesthesiaNotes.IsSelected)
                {
                    if (AllAnesthesiaList.Count > 0)
                    {
                        var item1 = from r in AllAnesthesiaList
                                    where (r.GroupName.Equals("Intra Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Intra Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllAnesthesiaList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Intra Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllAnesthesiaList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgAnesthesiaNotes.ItemsSource = pcv;
                    dgAnesthesiaNotes.UpdateLayout();
                    //  dgAnesthesiaNotes.Focus();
                }


                //if (AllInstructionList.Count > 0)
                //{
                //    var item1 = from r in AllInstructionList
                //                where (r.GroupName.Equals("Intra Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                //                select new clsOTDetailsInstructionListDetailsVO
                //                {
                //                    GroupName = r.GroupName,
                //                    Instruction = r.Instruction
                //                };
                //    if (item1.ToList().Count == 0)
                //    {
                //        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //        obj.GroupName = "Intra Operative Instruction Notes";
                //        obj.Instruction = strDescription;
                //        if (SurgeryNotes.IsSelected)
                //            AllSurgeryList.Add(obj);
                //        else if (AnesthesiaNotes.IsSelected)
                //            AllAnesthesiaList.Add(obj);
                //        //AllInstructionList.Add(obj);
                //    }
                //}
                //else
                //{
                //    clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //    obj.GroupName = "Intra Operative Instruction Notes";
                //    obj.Instruction = strDescription;
                //    if (SurgeryNotes.IsSelected)
                //        AllSurgeryList.Add(obj);
                //    else if (AnesthesiaNotes.IsSelected)
                //        AllAnesthesiaList.Add(obj);
                //    //AllInstructionList.Add(obj);
                //}

                //if (SurgeryNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgSurgeryNotes.ItemsSource = pcv;
                //    dgSurgeryNotes.UpdateLayout();
                //    dgSurgeryNotes.Focus();
                //}
                //else if (AnesthesiaNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgAnesthesiaNotes.ItemsSource = pcv;
                //    dgAnesthesiaNotes.UpdateLayout();
                //    dgAnesthesiaNotes.Focus();
                //}
            }
            else if (SelectedItem.Equals("Post-OT Details"))
            {
                if (SurgeryNotes.IsSelected)
                {
                    if (AllSurgeryList.Count > 0)
                    {
                        var item1 = from r in AllSurgeryList
                                    where (r.GroupName.Equals("Post Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Post Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllSurgeryList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Post Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllSurgeryList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgSurgeryNotes.ItemsSource = pcv;
                    dgSurgeryNotes.UpdateLayout();
                    //dgSurgeryNotes.Focus();
                }
                if (AnesthesiaNotes.IsSelected)
                {
                    if (AllAnesthesiaList.Count > 0)
                    {
                        var item1 = from r in AllAnesthesiaList
                                    where (r.GroupName.Equals("Post Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                                    select new clsOTDetailsInstructionListDetailsVO
                                    {
                                        GroupName = r.GroupName,
                                        Instruction = r.Instruction
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.GroupName = "Post Operative Instruction Notes";
                            obj.Instruction = strDescription;
                            AllAnesthesiaList.Add(obj);
                        }
                    }
                    else
                    {
                        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                        obj.GroupName = "Post Operative Instruction Notes";
                        obj.Instruction = strDescription;
                        AllAnesthesiaList.Add(obj);
                    }
                    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                    dgAnesthesiaNotes.ItemsSource = pcv;
                    dgAnesthesiaNotes.UpdateLayout();
                    //dgAnesthesiaNotes.Focus();
                }

                //if (AllInstructionList.Count > 0)
                //{
                //    var item1 = from r in AllInstructionList
                //                where (r.GroupName.Equals("Post Operative Instruction Notes") && r.Instruction.Equals(strDescription))
                //                select new clsOTDetailsInstructionListDetailsVO
                //                {
                //                    GroupName = r.GroupName,
                //                    Instruction = r.Instruction
                //                };
                //    if (item1.ToList().Count == 0)
                //    {
                //        clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //        obj.GroupName = "Post Operative Instruction Notes";
                //        obj.Instruction = strDescription;
                //        if (SurgeryNotes.IsSelected)
                //            AllSurgeryList.Add(obj);
                //        else if (AnesthesiaNotes.IsSelected)
                //            AllAnesthesiaList.Add(obj);
                //        //AllInstructionList.Add(obj);
                //    }
                //}
                //else
                //{
                //    clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                //    obj.GroupName = "Post Operative Instruction Notes";
                //    obj.Instruction = strDescription;
                //    if (SurgeryNotes.IsSelected)
                //        AllSurgeryList.Add(obj);
                //    else if (AnesthesiaNotes.IsSelected)
                //        AllAnesthesiaList.Add(obj);
                //    //AllInstructionList.Add(obj);
                //}

                //if (SurgeryNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgSurgeryNotes.ItemsSource = pcv;
                //    dgSurgeryNotes.UpdateLayout();
                //    dgSurgeryNotes.Focus();
                //}
                //else if (AnesthesiaNotes.IsSelected)
                //{
                //    PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                //    pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                //    dgAnesthesiaNotes.ItemsSource = pcv;
                //    dgAnesthesiaNotes.UpdateLayout();
                //    dgAnesthesiaNotes.Focus();
                //}
            }




            //dgService.ItemsSource = null;
            //PagedCollectionView pcv = new PagedCollectionView(ServiceList);
            //pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            //dgService.ItemsSource = pcv;
            ////dgServiceList.ItemsSource = ServiceList;
            //dgService.UpdateLayout();
            //dgService.Focus();

            //CheckBox chk = sender as CheckBox;
            //TextBox txtTarget = chk.Name == "chkMultipleSurNotes" ? txtSurgeryNotes : txtAnesthesiaNotes;
            //string strDescription = (chk.DataContext as MasterListItem).Description;

            //string SelectedItem = CmbDetails.SelectedItem.ToString();

            ////if (SelectedItem.Equals("Pre-OT Details"))
            ////{
            ////    if (txtTarget.Text.Contains("Pre Operative Instruction Notes : "))
            ////    {
            //        if (chk.IsChecked == true && (chk.DataContext as MasterListItem).ID != 0 && !txtTarget.Text.Contains(strDescription))
            //        {
            //            txtTarget.Text = String.Format(txtTarget.Text + strDescription + "\r\n ").Trim(',');
            //        }
            ////    }
            ////}

            ////if (chk.IsChecked == true && (chk.DataContext as MasterListItem).ID != 0 && !txtTarget.Text.Contains(strDescription))
            ////{
            ////    txtTarget.Text = String.Format(txtTarget.Text + strDescription + ",\r\n ").Trim(',');
            ////}
            ////else if (!String.IsNullOrEmpty(txtSurgeryNotes.Text))
            ////{
            ////    txtTarget.Text = String.Format(txtTarget.Text.Replace(strDescription, String.Empty).Trim(',')).Trim();
            ////}
        }
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveOTNotes();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
        #endregion

        #region Private Methods
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

        private void SaveOTNotes()
        {
            try
            {
                clsAddUpdatOtNotesDetailsBizActionVO BizAction = new clsAddUpdatOtNotesDetailsBizActionVO();
                BizAction.SurgeryInstructionList = AllSurgeryList.ToList();
                BizAction.AnesthesiaInstructionList = AllAnesthesiaList.ToList();

                //BizAction.objOTDetails.objAnesthesiaNotes.AnesthesiaNotes = txtAnesthesiaNotes.Text;
                //BizAction.objOTDetails.objSurgeryNotes.SurgeyNotes = txtSurgeryNotes.Text;

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
                        if (((clsAddUpdatOtNotesDetailsBizActionVO)arg.Result).SuccessStatus == 1)
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
                            (tvOTDetails.Items[6] as TreeViewItem).IsSelected = true;

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
                    ObjWaitIndicator.Close();
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

                clsGetOTNotesByOTDetailsIDBizActionVO BizAction = new clsGetOTNotesByOTDetailsIDBizActionVO();
                BizAction.OTDetailsID = this.lOTDetailsIDView;
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetOTNotesByOTDetailsIDBizActionVO DetailsVO = new clsGetOTNotesByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetOTNotesByOTDetailsIDBizActionVO)arg.Result;

                        if (DetailsVO != null)
                        {
                            if (DetailsVO.SurgeryInstructionList != null && DetailsVO.SurgeryInstructionList.Count > 0)
                            {
                                //AllSurgeryList = DetailsVO.SurgeryInstructionList.ToList();
                                if (DetailsVO.SurgeryInstructionList.Count > 0)
                                {
                                    AllSurgeryList.Clear();
                                
                                }
                                foreach (var item in DetailsVO.SurgeryInstructionList)
                                {
                                    if (!AllSurgeryList.Where(z => z.Instruction.Contains(item.Instruction)).Any())
                                        AllSurgeryList.Add(item);
                                }

                                //PagedCollectionView pcv = new PagedCollectionView(DetailsVO.SurgeryInstructionList);
                                PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                                dgSurgeryNotes.ItemsSource = pcv;
                                dgSurgeryNotes.UpdateLayout();
                                cmdModify.IsEnabled = true;
                                cmdSave.IsEnabled = false;
                            }
                            if (DetailsVO.AnesthesiaInstructionList != null && DetailsVO.AnesthesiaInstructionList.Count > 0)
                            {
                                //AllAnesthesiaList = DetailsVO.AnesthesiaInstructionList.ToList();
                                if (DetailsVO.AnesthesiaInstructionList.Count > 0)
                                {
                                    AllAnesthesiaList.Clear();
                                
                                }
                                foreach (var item in DetailsVO.AnesthesiaInstructionList)
                                {
                                    if (!AllAnesthesiaList.Where(z => z.Instruction.Contains(item.Instruction)).Any())
                                        AllAnesthesiaList.Add(item);
                                }
                                //PagedCollectionView pcv = new PagedCollectionView(DetailsVO.AnesthesiaInstructionList);
                                PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                                dgAnesthesiaNotes.ItemsSource = pcv;
                                dgAnesthesiaNotes.UpdateLayout();
                                cmdModify.IsEnabled = true;
                                cmdSave.IsEnabled = false;
                            }
                            //if (DetailsVO.AnesthesiaNotesObj != null)
                            //{
                            //    if (DetailsVO.AnesthesiaNotesObj.AnesthesiaNotes != null)
                            //    {
                            //        //+txtAnesthesiaNotes.Text = DetailsVO.AnesthesiaNotesObj.AnesthesiaNotes;
                            //        cmdModify.IsEnabled = true;
                            //        cmdSave.IsEnabled = false;
                            //    }
                            //}

                            //if (DetailsVO.SurgeryNotesObj != null)
                            //{
                            //    if (DetailsVO.SurgeryNotesObj.SurgeyNotes != null)
                            //    {
                            //        //txtSurgeryNotes.Text = DetailsVO.SurgeryNotesObj.SurgeyNotes;
                            //        cmdModify.IsEnabled = true;
                            //        cmdSave.IsEnabled = false;
                            //    }
                            //}
                        }


                        //clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO DetailsVO = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO();
                        //DetailsVO = (clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO)arg.Result;

                        //if (DetailsVO.AnesthesiaNotesObj != null)
                        //{
                        //    OTDetailsAnesthesiaNotesList.Add(DetailsVO.AnesthesiaNotesObj);
                        //    if (DetailsVO.AnesthesiaNotesObj.AnesthesiaNotes != null)
                        //    {
                        //        txtAnesthesiaNotes.Text = DetailsVO.AnesthesiaNotesObj.AnesthesiaNotes;
                        //    }
                        //}
                        //if (DetailsVO.SurgeryNotesObj != null)
                        //{
                        //    OTDetailsSurgeyNotesList.Add(DetailsVO.SurgeryNotesObj);
                        //    if (DetailsVO.SurgeryNotesObj.SurgeyNotes != null)
                        //    {
                        //        txtSurgeryNotes.Text = DetailsVO.SurgeryNotesObj.SurgeyNotes;
                        //    }
                        //}

                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }
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

        #endregion

        private void LoadNotesDetails()
        {
            List<MasterListItem> detailsList = new List<MasterListItem>();

            MasterListItem defaultObj = new MasterListItem();
            defaultObj.ID = 0;
            defaultObj.Description = "--Select--";
            detailsList.Add(defaultObj);

            MasterListItem preObj = new MasterListItem();
            preObj.ID = 1;
            preObj.Description = "Pre-OT Details";
            detailsList.Add(preObj);

            MasterListItem IntraObj = new MasterListItem();
            IntraObj.ID = 2;
            IntraObj.Description = "Intra-OT Details";
            detailsList.Add(IntraObj);

            MasterListItem PostObj = new MasterListItem();
            PostObj.ID = 3;
            PostObj.Description = "Post-OT Details";
            detailsList.Add(PostObj);

            CmbDetails.ItemsSource = null;
            CmbDetails.ItemsSource = detailsList;
            //CmbDetails.SelectedItem = new MasterListItem(0, "-- Select --");           
            CmbDetails.SelectedItem = detailsList[0];
        }

        public void FillDetailsOfProcedureScheduleNotes(long ScheduleID)
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
                BizAction.PreOperativeInstructionList = new List<string>();
                BizAction.ScheduleID = lScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //txtSurgeryNotes.Text = "";
                        //txtAnesthesiaNotes.Text = "";
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                        if (DetailsVO.InstructionList != null && DetailsVO.InstructionList.Count > 0)
                        {
                            AllInstructionList = DetailsVO.InstructionList;
                            PagedCollectionView pcv = new PagedCollectionView(AllInstructionList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            //txtSurgeryNotes.Text = pcv.ToString();
                            dgSurgeryNotes.ItemsSource = pcv;
                            dgAnesthesiaNotes.ItemsSource = pcv;

                            AllSurgeryList = AllInstructionList.ToList();
                            AllAnesthesiaList = AllInstructionList.ToList();
                        }


                        //if (DetailsVO.PreOperativeInstructionList != null && DetailsVO.PreOperativeInstructionList.Count > 0)
                        //{


                        //    txtSurgeryNotes.Text = "Pre Operative Instruction Notes : " + "\n";
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + string.Join(Environment.NewLine, DetailsVO.PreOperativeInstructionList);
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";

                        //    txtAnesthesiaNotes.Text = "Pre Operative Instruction Notes : " + "\n";
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + string.Join(Environment.NewLine, DetailsVO.PreOperativeInstructionList);
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
                        //}
                        //if (DetailsVO.IntraOperativeInstructionList != null && DetailsVO.IntraOperativeInstructionList.Count > 0)
                        //{
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n" + "Intra Operative Instruction Notes : " + "\n";
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + string.Join(Environment.NewLine, DetailsVO.IntraOperativeInstructionList);
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";

                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n" + "Intra Operative Instruction Notes : " + "\n";
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + string.Join(Environment.NewLine, DetailsVO.IntraOperativeInstructionList);
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
                        //}
                        //if (DetailsVO.PostOperativeInstructionList != null && DetailsVO.PostOperativeInstructionList.Count > 0)
                        //{
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n" + "Post Operative Instruction Notes : " + "\n";
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + string.Join(Environment.NewLine, DetailsVO.PostOperativeInstructionList);
                        //    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";

                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n" + "Post Operative Instruction Notes : " + "\n";
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + string.Join(Environment.NewLine, DetailsVO.PostOperativeInstructionList);
                        //    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
                        //}

                        if (this.lScheduleID > 0 && this.lPatientID > 0)
                            FillDetailTablesOfOTDetails(lOTDetailsIDView);
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }
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

        //private void FillDetailsOfProcedureSchedulePreNotes(long ScheduleID)
        //{
        //    try
        //    {
        //        if (ObjWaitIndicator == null)
        //            ObjWaitIndicator = new WaitIndicator();
        //        if (ObjWaitIndicator.Visibility == Visibility.Collapsed)
        //        {
        //            ObjWaitIndicator.Show();
        //        }
        //        else
        //        {
        //            ObjWaitIndicator.Close();
        //            ObjWaitIndicator.Show();
        //        }
        //        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //        BizAction.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        //        BizAction.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
        //        BizAction.PatientProcList = new List<clsPatientProcedureVO>();
        //        BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
        //        BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
        //        BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
        //        BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        //        BizAction.PreOperativeInstructionList = new List<string>();
        //        BizAction.ScheduleID = lScheduleID;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //                DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
        //                if (DetailsVO.PreOperativeInstructionList != null && DetailsVO.PreOperativeInstructionList.Count > 0)
        //                {
        //                    txtSurgeryNotes.Text = string.Join(Environment.NewLine, DetailsVO.PreOperativeInstructionList);
        //                    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";
        //                    txtAnesthesiaNotes.Text = string.Join(Environment.NewLine, DetailsVO.PreOperativeInstructionList);
        //                    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
        //                }


        //                //else
        //                //{
        //                //    txtSurgeryNotes.Text = "";
        //                //    txtAnesthesiaNotes.Text = "";
        //                //}
        //            }
        //            else
        //            {
        //                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //                {
        //                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
        //                }
        //                else
        //                {
        //                    msgText = "Error occured while processing.";
        //                }
        //                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            }
        //            ObjWaitIndicator.Close();

        //        };

        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //private void FillDetailsOfProcedureScheduleIntraNotes(long ScheduleID)
        //{
        //    try
        //    {
        //        if (ObjWaitIndicator == null)
        //            ObjWaitIndicator = new WaitIndicator();
        //        if (ObjWaitIndicator.Visibility == Visibility.Collapsed)
        //        {
        //            ObjWaitIndicator.Show();
        //        }
        //        else
        //        {
        //            ObjWaitIndicator.Close();
        //            ObjWaitIndicator.Show();
        //        }
        //        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //        BizAction.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        //        BizAction.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
        //        BizAction.PatientProcList = new List<clsPatientProcedureVO>();
        //        BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
        //        BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
        //        BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
        //        BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        //        BizAction.PreOperativeInstructionList = new List<string>();
        //        BizAction.IntraOperativeInstructionList = new List<string>();
        //        BizAction.ScheduleID = lScheduleID;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //                DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
        //                if (DetailsVO.IntraOperativeInstructionList != null && DetailsVO.IntraOperativeInstructionList.Count > 0)
        //                {
        //                    txtSurgeryNotes.Text = string.Join(Environment.NewLine, DetailsVO.IntraOperativeInstructionList);
        //                    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";
        //                    txtAnesthesiaNotes.Text = string.Join(Environment.NewLine, DetailsVO.IntraOperativeInstructionList);
        //                    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
        //                }
        //                //else
        //                //{
        //                //    txtSurgeryNotes.Text = "";
        //                //    txtAnesthesiaNotes.Text = "";
        //                //}
        //            }
        //            else
        //            {
        //                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //                {
        //                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
        //                }
        //                else
        //                {
        //                    msgText = "Error occured while processing.";
        //                }
        //                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            }
        //            ObjWaitIndicator.Close();

        //        };

        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //private void FillDetailsOfProcedureSchedulePostNotes(long ScheduleID)
        //{
        //    try
        //    {
        //        if (ObjWaitIndicator == null)
        //            ObjWaitIndicator = new WaitIndicator();
        //        if (ObjWaitIndicator.Visibility == Visibility.Collapsed)
        //        {
        //            ObjWaitIndicator.Show();
        //        }
        //        else
        //        {
        //            ObjWaitIndicator.Close();
        //            ObjWaitIndicator.Show();
        //        }
        //        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //        BizAction.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        //        BizAction.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
        //        BizAction.PatientProcList = new List<clsPatientProcedureVO>();
        //        BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
        //        BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
        //        BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
        //        BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        //        BizAction.PreOperativeInstructionList = new List<string>();
        //        BizAction.IntraOperativeInstructionList = new List<string>();
        //        BizAction.PostOperativeInstructionList = new List<string>();
        //        BizAction.ScheduleID = lScheduleID;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
        //                DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
        //                if (DetailsVO.PostOperativeInstructionList != null && DetailsVO.PostOperativeInstructionList.Count > 0)
        //                {
        //                    txtSurgeryNotes.Text = string.Join(Environment.NewLine, DetailsVO.PostOperativeInstructionList);
        //                    txtSurgeryNotes.Text = txtSurgeryNotes.Text + "\n";
        //                    txtAnesthesiaNotes.Text = string.Join(Environment.NewLine, DetailsVO.PostOperativeInstructionList);
        //                    txtAnesthesiaNotes.Text = txtAnesthesiaNotes.Text + "\n";
        //                }
        //                //else
        //                //{
        //                //    txtSurgeryNotes.Text = "";
        //                //    txtAnesthesiaNotes.Text = "";
        //                //}
        //            }
        //            else
        //            {
        //                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //                {
        //                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
        //                }
        //                else
        //                {
        //                    msgText = "Error occured while processing.";
        //                }
        //                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            }
        //            ObjWaitIndicator.Close();

        //        };

        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        private void cmbDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string InstructionText = string.Empty;
            InstructionText = Convert.ToString(CmbDetails.SelectedItem);
            if (InstructionText.Equals("Pre-OT Details"))
            {
                try
                {
                    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                    BizAction.MasterTable = MasterTableNameList.M_PreOperativeInstructionsMaster;
                    BizAction.MasterList = new List<MasterListItem>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, ee) =>
                    {
                        if (ee.Error == null && ee.Result != null)
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            objList.Add(new MasterListItem(0, "-- Select --"));
                            objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                            CmbAnethesiaNotes.ItemsSource = null;
                            CmbAnethesiaNotes.ItemsSource = objList;
                            CmbAnethesiaNotes.SelectedItem = objList[0];
                            CmbSurgeryNotes.ItemsSource = null;
                            CmbSurgeryNotes.ItemsSource = objList;
                            CmbSurgeryNotes.SelectedItem = objList[0];
                            //FillDetailsOfProcedureSchedulePreNotes(lScheduleID);
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
            else
                if (InstructionText.Equals("Intra-OT Details"))
                {
                    try
                    {
                        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                        BizAction.MasterTable = MasterTableNameList.M_IntraOperativeInstructionsMaster;
                        BizAction.MasterList = new List<MasterListItem>();

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, ee) =>
                        {
                            if (ee.Error == null && ee.Result != null)
                            {
                                List<MasterListItem> objList = new List<MasterListItem>();
                                objList.Add(new MasterListItem(0, "-- Select --"));
                                objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                                CmbAnethesiaNotes.ItemsSource = null;
                                CmbAnethesiaNotes.ItemsSource = objList;
                                CmbAnethesiaNotes.SelectedItem = objList[0];
                                CmbSurgeryNotes.ItemsSource = null;
                                CmbSurgeryNotes.ItemsSource = objList;
                                CmbSurgeryNotes.SelectedItem = objList[0];
                                //FillDetailsOfProcedureScheduleIntraNotes(lScheduleID);
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
                else
                    if (InstructionText.Equals("Post-OT Details"))
                    {
                        try
                        {
                            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                            BizAction.MasterTable = MasterTableNameList.M_PostOperativeInstructionsMaster;
                            BizAction.MasterList = new List<MasterListItem>();

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, ee) =>
                            {
                                if (ee.Error == null && ee.Result != null)
                                {
                                    List<MasterListItem> objList = new List<MasterListItem>();
                                    objList.Add(new MasterListItem(0, "-- Select --"));
                                    objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                                    CmbAnethesiaNotes.ItemsSource = null;
                                    CmbAnethesiaNotes.ItemsSource = objList;
                                    CmbAnethesiaNotes.SelectedItem = objList[0];
                                    CmbSurgeryNotes.ItemsSource = null;
                                    CmbSurgeryNotes.ItemsSource = objList;
                                    CmbSurgeryNotes.SelectedItem = objList[0];
                                    //FillDetailsOfProcedureSchedulePostNotes(lScheduleID);
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


            //    long DetailID = 0;
            //    if (CmbDetails.SelectedItem != null)
            //    {
            //        //if (OTDetailsAnesthesiaNotesList.SingleOrDefault(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)) != null || OTDetailsSurgeyNotesList.SingleOrDefault(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)) != null)
            //        //{
            //        //    if (OTDetailsAnesthesiaNotesList.SingleOrDefault(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)) != null)
            //        //    {
            //        //        DetailID = OTDetailsAnesthesiaNotesList.SingleOrDefault(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).detailsID;
            //        //    }
            //        //    else
            //        //    {
            //        //        DetailID = OTDetailsSurgeyNotesList.SingleOrDefault(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).detailsID;
            //        //    }
            //        //}

            //        if (OTDetailsAnesthesiaNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList() != null || OTDetailsSurgeyNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList() != null)
            //        {
            //            if (OTDetailsAnesthesiaNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList() != null && OTDetailsAnesthesiaNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList().Count > 0)
            //            {
            //                DetailID = OTDetailsAnesthesiaNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList()[0].detailsID;
            //            }
            //            else if (OTDetailsSurgeyNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList() != null && OTDetailsSurgeyNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList().Count > 0)
            //            {
            //                DetailID = OTDetailsSurgeyNotesList.Where(S => S.detailsID.Equals(((MasterListItem)CmbDetails.SelectedItem).ID)).ToList()[0].detailsID;
            //            }
            //        }
            //    }
            //    if (CmbDetails.SelectedItem != null)
            //    {
            //        if (DetailID != ((MasterListItem)CmbDetails.SelectedItem).ID)
            //        {
            //            txtAnesthesiaNotes.Text = string.Empty;
            //            txtSurgeryNotes.Text = string.Empty;
            //        }
            //    }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveOTNotes();
        }

        private void cmdDeleteSurgeryNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSurgeryNotes.SelectedItem != null)
                {
                    string msgTitle = "";
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");

                    int index = dgSurgeryNotes.SelectedIndex;
                    msgText = "Are You sure you want to delete this record?";
                    MessageBoxControl.MessageBoxChildWindow msgWD = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            AllSurgeryList.RemoveAt(index);
                            dgSurgeryNotes.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(AllSurgeryList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgSurgeryNotes.ItemsSource = pcv;
                            dgSurgeryNotes.UpdateLayout();
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

        private void cmdDeleteAnesthesiaNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAnesthesiaNotes.SelectedItem != null)
                {
                    string msgTitle = "Are You sure you want to delete this record?";
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");

                    int index = dgAnesthesiaNotes.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            AllAnesthesiaList.RemoveAt(index);
                            dgAnesthesiaNotes.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(AllAnesthesiaList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgAnesthesiaNotes.ItemsSource = pcv;
                            dgAnesthesiaNotes.UpdateLayout();
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



        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDetails != null)
            {
                Tabselection_Change();
            }
        }


        private void Tabselection_Change()
        {
            string InstructionText = string.Empty;

            InstructionText = Convert.ToString(CmbDetails.SelectedItem);
            if (InstructionText.Equals("Pre-OT Details"))
            {
                try
                {
                    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                    BizAction.MasterTable = MasterTableNameList.M_PreOperativeInstructionsMaster;
                    BizAction.MasterList = new List<MasterListItem>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, ee) =>
                    {
                        if (ee.Error == null && ee.Result != null)
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            objList.Add(new MasterListItem(0, "-- Select --"));
                            objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                            CmbAnethesiaNotes.ItemsSource = null;
                            CmbAnethesiaNotes.ItemsSource = objList;
                            CmbAnethesiaNotes.SelectedItem = objList[0];
                            CmbSurgeryNotes.ItemsSource = null;
                            CmbSurgeryNotes.ItemsSource = objList;
                            CmbSurgeryNotes.SelectedItem = objList[0];
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
            else
                if (InstructionText.Equals("Intra-OT Details"))
                {
                    try
                    {
                        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                        BizAction.MasterTable = MasterTableNameList.M_IntraOperativeInstructionsMaster;
                        BizAction.MasterList = new List<MasterListItem>();

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, ee) =>
                        {
                            if (ee.Error == null && ee.Result != null)
                            {
                                List<MasterListItem> objList = new List<MasterListItem>();
                                objList.Add(new MasterListItem(0, "-- Select --"));
                                objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                                CmbAnethesiaNotes.ItemsSource = null;
                                CmbAnethesiaNotes.ItemsSource = objList;
                                CmbAnethesiaNotes.SelectedItem = objList[0];
                                CmbSurgeryNotes.ItemsSource = null;
                                CmbSurgeryNotes.ItemsSource = objList;
                                CmbSurgeryNotes.SelectedItem = objList[0];
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
                else
                    if (InstructionText.Equals("Post-OT Details"))
                    {
                        try
                        {
                            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                            BizAction.MasterTable = MasterTableNameList.M_PostOperativeInstructionsMaster;
                            BizAction.MasterList = new List<MasterListItem>();

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, ee) =>
                            {
                                if (ee.Error == null && ee.Result != null)
                                {
                                    List<MasterListItem> objList = new List<MasterListItem>();
                                    objList.Add(new MasterListItem(0, "-- Select --"));
                                    objList.AddRange(((clsGetMasterListBizActionVO)ee.Result).MasterList);
                                    CmbAnethesiaNotes.ItemsSource = null;
                                    CmbAnethesiaNotes.ItemsSource = objList;
                                    CmbAnethesiaNotes.SelectedItem = objList[0];
                                    CmbSurgeryNotes.ItemsSource = null;
                                    CmbSurgeryNotes.ItemsSource = objList;
                                    CmbSurgeryNotes.SelectedItem = objList[0];
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
        }
        
    }
}
