using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Shapes;
using EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using System.Windows.Data;
using CIMS;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Controls;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTDetailsService : UserControl
    {

        #region Variable Declaration
        List<MasterListItem> Priority;
        ObservableCollection<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }
        LocalizationManager ObjLocalizationManager = null;
        WaitIndicator ObjWaitIndicator = null;
        public bool lIsEmergency { get; set; }
        public long lOTDetailsIDView = 0;
        public long lScheduleID = 0;
        public long lPatientID = 0;
        string msgText = string.Empty;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor and Loaded
        public frmOTDetailsService()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmOTDetailsService_Loaded);
            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
            ObjLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            ObjWaitIndicator = new WaitIndicator();

        }

        void frmOTDetailsService_Loaded(object sender, RoutedEventArgs e)
        {
            dgService.ItemsSource = ServiceList;
            FillReferenceType();
            //if (this.lOTDetailsIDView > 0)
            //{
            //    FillDetailTablesOfOTDetails(lOTDetailsIDView);
            //}
            //else
            //FillDetailsOfProcedureSchedule(lScheduleID);
        }
        #endregion

        #region Click Events
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveServices();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        private void cmdLab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmCPOELabServicesSelection Win = new frmCPOELabServicesSelection();
                //Win.Width = this.ActualWidth * 0.5;
                //Win.Height = this.ActualHeight * 1.5;
                Win.Width = this.ActualWidth * 0.75;
                Win.Height = this.ActualHeight * 0.95;
                Win.Pathology = true;
                Win.OnAddButton_Click += new RoutedEventHandler(WinLab_OnAddButton_Click);
                Win.Show();
            }
            catch
            {

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
                            //FillCurrentServices();
                        }
                        FillDetailsOfProcedureSchedule(lScheduleID);
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
                                    where (r.ServiceName == item.ServiceName && r.GroupName == item.Group
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
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.GroupName = item.Group;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                                OBj.GroupName = "Radiology";
                            }
                            else if (winService.Pathology)
                            {
                                OBj.ServiceType = "Pathology";
                                OBj.GroupName = "Pathology";
                            }
                            else
                                OBj.ServiceType = "Other";
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
                        OBj.GroupName = item.Group;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                            OBj.GroupName = "Radiology";
                        }
                        else if (winService.Pathology)
                        {
                            OBj.ServiceType = "Pathology";
                            OBj.GroupName = "Pathology";
                        }
                        else
                            OBj.ServiceType = "Other";

                        ServiceList.Add(OBj);
                    }
                }
                dgService.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgService.ItemsSource = pcv;
                //dgServiceList.ItemsSource = ServiceList;
                dgService.UpdateLayout();
                dgService.Focus();
            }
        }

        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {

            frmCPOEServiceSelectionList Win = new frmCPOEServiceSelectionList();
            dgService.SelectedIndex = -1;
            //Win.Width = this.ActualWidth * 0.5;
            //Win.Height = this.ActualHeight * 1.5;
            Win.Width = this.ActualWidth * 0.75;
            Win.Height = this.ActualHeight * 0.95;
            Win.Radiology = true;
            Win.OnAddButton_Click += new RoutedEventHandler(WinRad_OnAddButton_Click);
            Win.Show();

        }

        void WinRad_OnAddButton_Click(object sender, RoutedEventArgs e)
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
                            {
                                OBj.ServiceType = "Pathology";
                                OBj.GroupName = "Pathology";
                            }

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


                dgService.ItemsSource = null;

                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgService.ItemsSource = pcv;

                //dgServiceList.ItemsSource = ServiceList;
                dgService.UpdateLayout();
                dgService.Focus();
            }
        }

        private void cmdDiagnostik_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList Win = new frmCPOEOtherSelectionList();
            dgService.SelectedIndex = -1;
            Win.IsOther = true;
            Win.Width = this.ActualWidth * 0.75;
            Win.Height = this.ActualHeight * 0.95;
            //Win.Width = this.ActualWidth * 0.5;
            //Win.Height = this.ActualHeight * 1.5;
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
                dgService.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgService.ItemsSource = pcv;
                dgService.UpdateLayout();
                dgService.Focus();
            }
        }
        #endregion

        #region Private Methods
        private void SaveServices()
        {
            try
            {
                clsAddUpdatOtServicesDetailsBizActionVO BizAction = new clsAddUpdatOtServicesDetailsBizActionVO();
                BizAction.objOTDetails.OTServicesList = ServiceList.ToList();

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
                        if (((clsAddUpdatOtServicesDetailsBizActionVO)arg.Result).SuccessStatus == 1)
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
                            (tvOTDetails.Items[5] as TreeViewItem).IsSelected = true;
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

        private void FillDetailTablesOfOTDetails(long OtDetailsID)
        {
            try
            {
                if (ObjWaitIndicator == null || ObjWaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWaitIndicator = new WaitIndicator();
                    ObjWaitIndicator.Show();
                }
                clsGetServicesByOTDetailsIDBizActionVO BizAction = new clsGetServicesByOTDetailsIDBizActionVO();
                BizAction.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;
                BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetServicesByOTDetailsIDBizActionVO DetailsVO = new clsGetServicesByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetServicesByOTDetailsIDBizActionVO)arg.Result;

                        List<clsDoctorSuggestedServiceDetailVO> NewList = new List<clsDoctorSuggestedServiceDetailVO>();
                        //NewList = ServiceList.ToList();

                        if (DetailsVO.ServiceDetails != null && DetailsVO.ServiceDetails.Count > 0)
                        {
                            //dgService.ItemsSource = null;
                            //ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
                            foreach (var item in DetailsVO.ServiceDetails)
                            {
                                item.Priority = Priority;
                                long id = item.SelectedPriority.ID;
                                //item.SelectedPriority = Priority[id];
                                item.SelectedPriority = Priority[Convert.ToInt32(id)]; 
                                //for (int i = ServiceList.Count - 1; i >=0; i--)
                                //{
                                //    if (ServiceList[i].ServiceName.Equals(item.ServiceName))
                                //        ServiceList.RemoveAt(i);
                                //}


                                var itemToRemove = ServiceList.SingleOrDefault(r => r.ServiceName.Equals(item.ServiceName));
                                if (itemToRemove != null)
                                    ServiceList.Remove(itemToRemove);

                                //var objService = from r in ServiceList
                                //                 where r.ServiceName == item.ServiceName
                                //                 select new clsDoctorSuggestedServiceDetailVO
                                //                 {
                                //                     //SelectedPriority = new MasterListItem();
                                //                     ID = r.ID,
                                //                     ServiceType = r.ServiceType,
                                //                     ServiceName = r.ServiceName,
                                //                     ServiceCode = r.ServiceCode,
                                //                     GroupName = r.GroupName,
                                //                     Quantity = r.Quantity,
                                //                     SelectedPriority = r.SelectedPriority
                                //                 };
                                //if (objService.ToList().Count == 0)
                                //{
                                //    ServiceList.Add(item);
                                //}
                            }
                            foreach (var item in DetailsVO.ServiceDetails)
                            {
                                if (!ServiceList.Where(z => z.ServiceName.Contains(item.ServiceName)).Any())
                                    ServiceList.Add(item);
                            }

                            dgService.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgService.ItemsSource = pcv;
                            dgService.Focus();
                            dgService.UpdateLayout();
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
            finally
            {
            }
        }

        public void FillDetailsOfProcedureSchedule(long lScheduleID)
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
                        dgService.ItemsSource = null;
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                        //procedureList = procedureList = new List<clsPatientProcedureVO>();
                        if (DetailsVO.ServiceList != null && DetailsVO.ServiceList.Count > 0)
                        {
                            foreach (var item in DetailsVO.ServiceList)
                            {
                                item.Priority = Priority;
                                item.SelectedPriority = Priority[0];
                                //item.SelectedPriority = new MasterListItem(0, "-- Select --");
                                ServiceList.Add(item);
                            }

                            dgService.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgService.ItemsSource = pcv;
                            dgService.Focus();
                            dgService.UpdateLayout();
                        }

                        if (this.lScheduleID > 0 && this.lPatientID > 0)
                            FillDetailTablesOfOTDetails(lOTDetailsIDView);
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
                //msgText = ObjLocalizationManager.GetValue("ScheduleVlidation_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                result = false;
                return result;
            }
            return result;
        }
        #endregion

        private void cmbPriorityGrp_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox Combo = (AutoCompleteComboBox)sender;

            Combo.ItemsSource = Priority;
            var grp = (((CollectionViewGroup)Combo.DataContext).Items).ToList();
            foreach (clsDoctorSuggestedServiceDetailVO item in grp.ToList())
            {
                if (item.ServiceType == "Pathology")
                {
                    Combo.SelectedItem = item.SelectedPriority;
                }
                else
                    if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                    {
                        Combo.SelectedItem = item.SelectedPriority;
                    }
                    else
                    {
                        (Combo.Parent as StackPanel).Children[1].Visibility = Visibility.Collapsed;
                        Combo.Visibility = Visibility.Collapsed;
                        (Combo.Parent as StackPanel).Children[3].Visibility = Visibility.Collapsed;
                        (Combo.Parent as StackPanel).Children[4].Visibility = Visibility.Collapsed;
                    }
            }
        }

        private void cmbPriorityGrp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteComboBox Ch = (AutoCompleteComboBox)sender;

            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            foreach (clsDoctorSuggestedServiceDetailVO item in grp)
            {
                if (item.ServiceType == "Pathology")
                {
                    item.SelectedPriority = (MasterListItem)Ch.SelectedItem;
                }
                else
                    if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                    {
                        item.SelectedPriority = (MasterListItem)Ch.SelectedItem;
                    }

            }
        }

        private void txtQuntity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBx = (TextBox)sender;

            string TextValue = txtBx.Text;
            int InputValue;
            if (TextValue != "")
            {
                if (!int.TryParse(TextValue, out InputValue))
                {
                    msgText = "Enter Valid Quantity";
                    //msgText = ObjLocalizationManager.GetValue("QuantityValidation");
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    txtBx.Text = "";
                    var grp = (((CollectionViewGroup)txtBx.DataContext).Items).ToList();
                    foreach (clsDoctorSuggestedServiceDetailVO item in grp)
                    {
                        if (item.ServiceType == "Pathology")
                        {
                            item.Quantity = 0;
                        }
                        else
                            if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                                item.Quantity = 0;
                    }
                }
                else
                {
                    var grp = (((CollectionViewGroup)txtBx.DataContext).Items).ToList();
                    foreach (clsDoctorSuggestedServiceDetailVO item in grp)
                    {
                        if (item.ServiceType == "Pathology")
                        {
                            item.Quantity = Convert.ToInt64(txtBx.Text);
                        }
                        else
                            if (item.ServiceType == "Pathology" && item.SpecializationCode == "9404")
                                item.Quantity = Convert.ToInt64(txtBx.Text);
                    }

                }
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
                SaveServices();
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgService.SelectedItem != null)
            {
                clsDoctorSuggestedServiceDetailVO objService = dgService.SelectedItem as clsDoctorSuggestedServiceDetailVO;
                string msgText = "Are you sure you want to delete record ?";
                //string msgText = ObjLocalizationManager.GetValue("DeleteValidation_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        List<clsDoctorSuggestedServiceDetailVO> lstServices = ((dgService.ItemsSource as PagedCollectionView).SourceCollection as ObservableCollection<clsDoctorSuggestedServiceDetailVO>).ToList();
                        var lstDelete = new List<clsDoctorSuggestedServiceDetailVO>();
                        if (objService.ServiceType == "Pathology")
                        {
                            lstDelete = lstServices.Where(z => z.ServiceName == objService.ServiceName).ToList();
                        }
                        else
                            lstDelete = lstServices.Where(z => z.ServiceName == objService.ServiceName).ToList();
                        if (lstDelete != null && lstDelete.Count > 0)
                        {
                            foreach (clsDoctorSuggestedServiceDetailVO item in lstDelete)
                            {
                                ServiceList.Remove(item);
                            }
                        }
                        dgService.ItemsSource = null;
                        PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                        dgService.ItemsSource = pcv;
                        dgService.UpdateLayout();
                    }
                };
                msgWD.Show();
            }
        }
    }
}
