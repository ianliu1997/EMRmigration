using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.Converters;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Controls;
using PalashDynamics.SearchResultLists;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.Collections;
using PalashDynamics.SearchResultLists;

namespace EMR
{
    public partial class frmIPDEMRInvestigation : UserControl
    {

        #region Data Member
        WaitIndicator indicator;
        DateConverter dateConverter;
        List<MasterListItem> Priority;
        public Boolean IsEnableControl { get; set; }
        ObservableCollection<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsEnabledControl { get; set; }
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        ObservableCollection<clsServiceMasterVO> getpastServiceList { get; set; }
        #endregion

        #region Property
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }

        #endregion

        public frmIPDEMRInvestigation()
        {
            InitializeComponent();
            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
            this.Loaded += new RoutedEventHandler(frmEMRInvestigation_Loaded);
            indicator = new WaitIndicator();

            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPatientPastVisitServices();
        }

        #region Loaded
        void frmEMRInvestigation_Loaded(object sender, RoutedEventArgs e)
        {
            dgServiceList.ItemsSource = ServiceList;
            FillReferenceType();
            FillPatientPastVisitServices();
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                IsEnabledControl = false;
            }
            else
            {
                spSpecDoctor.Visibility = Visibility.Visible;
                if (CurrentVisit.ISIPDDischarge)
                {
                    this.IsEnabledControl = false;
                }
                else
                {
                    IsEnabledControl = true;
                }
            }
            FillSpecialization();
            FillDoctor();
            cmdSave.IsEnabled = IsEnabledControl;
            cmdLab.IsEnabled = IsEnabledControl;
            cmdRadiology.IsEnabled = IsEnabledControl;
            //cmdDiagnostik.IsEnabled = IsEnabledControl;
        }
        #endregion

        #region Button Click

        //private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSpecialization.SelectedItem != null && ((MasterListItem)cmbSpecialization.SelectedItem).Code != "0")
        //            FillDoctor(((MasterListItem)cmbSpecialization.SelectedItem).Code);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //msgW.Show();
            if (ServiceList != null && ServiceList.Count > 0)
            {
                SaveInvestigations();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Add the Investigation details..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveInvestigations();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            NavigateToDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigateToDashBoard();
            }
        }

        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        private void cmdLab_Click(object sender, RoutedEventArgs e)
        {
            frmCPOELabServicesSelection Win = new frmCPOELabServicesSelection();
            dgServiceList.SelectedIndex = -1;
            Win.CurrentVisit = CurrentVisit;
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;

            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.Pathology = true;
            Win.OnAddButton_Click += new RoutedEventHandler(WinLab_OnAddButton_Click);
            Win.Show();
        }

        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEServiceSelectionList Win = new frmCPOEServiceSelectionList();
            dgServiceList.SelectedIndex = -1;
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;

            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.CurrentVisit = CurrentVisit;
            Win.Radiology = true;
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
        }

        void WinLab_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmCPOELabServicesSelection winService = (frmCPOELabServicesSelection)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList.Count > 0)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceName == item.ServiceName && r.ServiceCode == item.ServiceCode
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        GroupName = r.GroupName,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceID = item.ServiceID;
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.ServiceRate = Convert.ToDouble(item.Rate);
                            OBj.GroupName = "Pathology";//item.Group;
                            OBj.SpecializationId = item.Specialization;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                                OBj.ServiceType = "Other";
                            ServiceList.Add(OBj);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Service already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceID = item.ServiceID;
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.ServiceRate = Convert.ToDouble(item.Rate);
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.SpecializationId = item.Specialization;
                        OBj.Priority = Priority;
                        OBj.GroupName = "Pathology";//item.Group;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                            OBj.ServiceType = "Other";

                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;
                //dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEServiceSelectionList winService = (frmCPOEServiceSelectionList)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList.Count > 0)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceCode == item.ServiceCode
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceID = item.ServiceID;
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.ServiceRate = Convert.ToDouble(item.Rate);
                            OBj.SpecializationId = item.Specialization;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                                OBj.GroupName = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                            {
                                OBj.ServiceType = "Other";
                                OBj.GroupName = "Diagnostik";
                            }
                            ServiceList.Add(OBj);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Service already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceID = item.ServiceID;
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.ServiceRate = Convert.ToDouble(item.Rate);
                        OBj.SpecializationId = item.Specialization;
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.Priority = Priority;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                            OBj.GroupName = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                        {
                            OBj.ServiceType = "Other";
                            OBj.GroupName = "Diagnostik";
                        }

                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;

                //dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        private void cmdDeleteInvest_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                clsDoctorSuggestedServiceDetailVO objService = dgServiceList.SelectedItem as clsDoctorSuggestedServiceDetailVO;
                string msgText = "Are you sure you want to delete record ?";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        //List<clsDoctorSuggestedServiceDetailVO> lstServices = ((dgServiceList.ItemsSource as PagedCollectionView).SourceCollection as ObservableCollection<clsDoctorSuggestedServiceDetailVO>).ToList();
                        //var lstDelete = new List<clsDoctorSuggestedServiceDetailVO>();
                        //if (objService.ServiceType == "Pathology")
                        //{
                        //    lstDelete = lstServices.Where(z => z.GroupName == objService.GroupName).ToList();
                        //}
                        //else
                        //    lstDelete = lstServices.Where(z => z.ServiceName == objService.ServiceName).ToList();
                        //if (lstDelete != null && lstDelete.Count > 0)
                        //{
                        //    foreach (clsDoctorSuggestedServiceDetailVO item in lstDelete)
                        //    {
                        //        ServiceList.Remove(item);
                        //    }
                        //}

                        ServiceList.RemoveAt(dgServiceList.SelectedIndex);
                        PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = pcv;
                        //dgServiceList.ItemsSource = ServiceList;
                        dgServiceList.UpdateLayout();
                    }
                };
                msgWD.Show();
            }
        }

        private void cmdDiagnostik_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList Win = new frmCPOEOtherSelectionList();
            dgServiceList.SelectedIndex = -1;
            Win.IsOther = true;

            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;

            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.CurrentVisit = CurrentVisit;
            Win.OnAddButton_Click += new RoutedEventHandler(WinDiagnostik_OnAddButton_Click);
            Win.Show();
        }

        void WinDiagnostik_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList winService = (frmCPOEOtherSelectionList)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList.Count > 0)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceCode == item.ServiceCode
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                                OBj.GroupName = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                            {
                                OBj.ServiceType = "Other";
                                OBj.GroupName = "Diagnostik";
                            }
                            ServiceList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.Priority = Priority;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                            OBj.GroupName = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                        {
                            OBj.ServiceType = "Other";
                            OBj.GroupName = "Diagnostik";
                        }

                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;
                //dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        private void cmbPriorityGrp_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox Combo = (AutoCompleteComboBox)sender;
            Combo.ItemsSource = Priority;
            var grp = (((CollectionViewGroup)Combo.DataContext).Items).ToList();
            foreach (clsDoctorSuggestedServiceDetailVO item in grp.ToList())
            {
                if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                {
                    Combo.SelectedItem = item.SelectedPriority;
                }
                else
                {
                    (Combo.Parent as StackPanel).Children[1].Visibility = Visibility.Collapsed;
                    Combo.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cmbPriorityGrp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteComboBox Ch = (AutoCompleteComboBox)sender;

            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            foreach (clsDoctorSuggestedServiceDetailVO item in grp)
            {
                if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                    item.SelectedPriority = (MasterListItem)Ch.SelectedItem;
            }
        }
        #endregion

        #region Private Method
        private void FillDoctor()
        {
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();
            //if (cmbSpecialization.SelectedItem != null)
            //{
            //    BizAction.IsForReferral = true;
            //    BizAction.SpecialCode = sDeptCode;
            //}
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //        cmbDoctor.ItemsSource = null;
            //        cmbDoctor.ItemsSource = objList;
            //        cmbDoctor.SelectedItem = objList[0];
            //        if (this.DataContext != null)
            //        {
            //            cmbDoctor.SelectedValue = objList[0].ID;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
            //                cmbDoctor.IsEnabled = false;
            //            }
            //            else
            //                cmbDoctor.SelectedValue = "0";
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            //try
            //{
            //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            //    BizAction.MasterTable = MasterTableNameList.SPESIAL;
            //    BizAction.CodeColumn = "KDSPESIAL";
            //    BizAction.DescriptionColumn = "NMSPESIAL";
            //    BizAction.MasterList = new List<MasterListItem>();
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            List<MasterListItem> objList = new List<MasterListItem>();
            //            objList.Add(new MasterListItem("0", "-- Select --"));
            //            objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
            //            cmbSpecialization.ItemsSource = null;
            //            cmbSpecialization.ItemsSource = objList;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                string sSpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
            //                cmbSpecialization.SelectedItem = objList.Where(z => z.Code == sSpecCode).FirstOrDefault();
            //                cmbSpecialization.IsEnabled = false;
            //            }
            //            else
            //            {
            //                cmbSpecialization.SelectedItem = objList[0];
            //            }
            //        }
            //    };
            //    client.ProcessAsync(BizAction, new clsUserVO());
            //}
            //catch (Exception)
            //{
            //}
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        int ClickFlag = 0;
        private void SaveInvestigations()
        {
            ClickFlag += 1;
            if (ClickFlag == 1)
            {
                try
                {
                    clsAddUpdatePatientInvestigationsBizActionVO BizAction = new clsAddUpdatePatientInvestigationsBizActionVO();
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.VisitID = CurrentVisit.ID;
                    //BizAction.DoctorCode = CurrentVisit.DoctorCode;
                    BizAction.DoctorID = CurrentVisit.DoctorID;
                    BizAction.InvestigationList = ServiceList.ToList();
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    if (indicator == null || indicator.Visibility == Visibility.Collapsed)
                    {
                        indicator = new WaitIndicator();
                        indicator.Show();
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        indicator.Close();
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdatePatientInvestigationsBizActionVO)args.Result).SuccessStatus > 0)
                            {
                                //string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //            new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                //{
                                //    this.Content = null;
                                //    NavigateToNextMenu();
                                //};
                                //msgW1.Show();
                                #region UpdateEncounterList
                                if (this.CurrentVisit.VisitTypeID == 2)
                                {
                                    int Radiology = 0, Pathology = 0, Other = 0;
                                    foreach (var item in BizAction.InvestigationList)
                                    {
                                        if (item.ServiceType == "Radiology")
                                        {
                                            Radiology += 1;
                                        }
                                        if (item.ServiceType == "Pathology")
                                        {
                                            Pathology += 1;
                                        }
                                        if (item.ServiceType == "Other")
                                        {
                                            Other += 1;
                                        }
                                    }
                                    if (Pathology != 0)
                                    {
                                        frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                                        ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                        DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                                        DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                                        clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                                        objPatientHeader.IsLaboratoryRight = "Visible";
                                        objPatientHeader.IsLaboratoryWrong = "Collapsed";
                                        dgEncounterList.ItemsSource = null;
                                        dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                                    }
                                    if (Radiology != 0)
                                    {
                                        frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                                        ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                        DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                                        DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                                        clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                                        objPatientHeader.IsRadiologyRight = "Visible";
                                        objPatientHeader.IsRadiologyWrong = "Collapsed";
                                        dgEncounterList.ItemsSource = null;
                                        dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                                    }
                                    if (Other != 0)
                                    {
                                        frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                                        ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                        DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                                        DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                                        clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                                        objPatientHeader.IsDiagnostickRight = "Visible";
                                        objPatientHeader.IsDiagnostickWrong = "Collapsed";
                                        dgEncounterList.ItemsSource = null;
                                        dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                                    }
                                }
                                #endregion

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                ClickFlag = 0;
                                // NavigateToNextMenu();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    ClickFlag = 0;
                }
            }
        }
        private void FillReferenceType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReferenceType;
                BizAction.MasterList = new List<MasterListItem>();
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            Priority = new List<MasterListItem>();
                            Priority.Add(new MasterListItem(0, "-- Select --"));
                            Priority.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                            FillCurrentServices();
                        }
                    };
                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void FillCurrentServices()
        {
            clsGetPatientCurrentServicesBizActionVO BizAction = new clsGetPatientCurrentServicesBizActionVO();
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.UnitID = CurrentVisit.UnitId;
            //BizAction.DoctorCode = CurrentVisit.DoctorCode;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetPatientCurrentServicesBizActionVO ObjBizAction = ((clsGetPatientCurrentServicesBizActionVO)args.Result);
                        if (ObjBizAction.ServiceDetails != null && ObjBizAction.ServiceDetails.Count > 0)
                        {
                            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
                            foreach (var item in ObjBizAction.ServiceDetails)
                            {
                                item.Priority = this.Priority;
                                item.SelectedPriority = Priority.Where(z => z.ID == item.PriorityIndex).FirstOrDefault();
                                ServiceList.Add(item);
                            }
                            dgServiceList.ItemsSource = null;

                            PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgServiceList.ItemsSource = pcv;
                            //dgServiceList.ItemsSource = ServiceList;
                        }
                        dgServiceList.UpdateLayout();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                indicator.Close();
            }
        }

        private void FillPatientPastVisitServices()
        {
            if (CurrentVisit != null)
            {
                try
                {
                    clsGetServicesBizActionVO BizActionObj = new clsGetServicesBizActionVO();
                    BizActionObj.PatientID = CurrentVisit.PatientId;
                    BizActionObj.VisitID = CurrentVisit.ID;
                    BizActionObj.IsOPDIPD = CurrentVisit.OPDIPD;
                    // BizActionObj.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionObj.DoctorID = CurrentVisit.DoctorID;
                    BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                    BizActionObj.MaximumRows = DataList.PageSize;
                    if (indicator == null || indicator.Visibility == Visibility.Collapsed)
                    {
                        indicator = new WaitIndicator();
                        indicator.Show();
                    }
                    try
                    {
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsGetServicesBizActionVO)args.Result).VisitServicesList != null)
                                {
                                    dateConverter = new DateConverter();
                                    dgPastVisitServices.ItemsSource = null;
                                    DataList.TotalItemCount = ((clsGetServicesBizActionVO)args.Result).TotalRows;
                                    DataList.Clear();
                                    //PagedCollectionView pcvPastInvest = new PagedCollectionView(((clsGetServicesBizActionVO)args.Result).VisitServicesList);
                                    foreach (var item in ((clsGetServicesBizActionVO)args.Result).VisitServicesList)
                                    {
                                        DataList.Add(item);
                                        if (this.CurrentVisit.OPDIPD)
                                            item.Datetime = String.Format(item.VisitDate.ToString("dd/MM/yyyy") + " - " + item.DoctorName.Trim() + " - " + item.SpecializationString);
                                    }
                                    getpastServiceList = new ObservableCollection<clsServiceMasterVO>(DataList);
                                    PagedCollectionView pcvPastInvest = new PagedCollectionView(getpastServiceList);
                                    if (CurrentVisit.VisitTypeID == 2)
                                        if (this.CurrentVisit.OPDIPD)
                                            pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                        else
                                        {
                                            pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                                        }
                                    else
                                        pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                                    pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
                                    dgPastVisitServices.ItemsSource = pcvPastInvest;
                                    dgPastVisitServices.UpdateLayout();
                                    pgrPrevInvestigations.Source = DataList;
                                }
                            }
                        };
                        client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        indicator.Close();
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return;
            }
        }



        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

    }
}
