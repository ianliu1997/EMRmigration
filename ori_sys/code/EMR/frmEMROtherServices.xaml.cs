using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;

namespace EMR
{
    public partial class frmEMROtherServices : UserControl
    {
        #region Data Member
        List<MasterListItem> Priority;
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsEnabledControl { get; set; }
        ObservableCollection<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }

        #endregion

        #region Constructor
        public frmEMROtherServices()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmEMROtherServices_Loaded);
        }
        #endregion 

        #region Loaded
        void frmEMROtherServices_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
            dgOtherServices.ItemsSource = ServiceList;
            if (IsEnabled == false)
            {
                this.cmdSave.IsEnabled = CurrentVisit.VisitStatus;
            }
            FillReferenceType();
        }
        #endregion
        private void cmdOtherServices_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList Win = new frmCPOEOtherSelectionList();
            dgOtherServices.SelectedIndex = -1;
            Win.IsOther = true;
            Win.CurrentVisit = CurrentVisit;
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
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
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
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
                dgOtherServices.ItemsSource = null;
                dgOtherServices.ItemsSource = ServiceList;
                dgOtherServices.UpdateLayout();
                dgOtherServices.Focus();
            }
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            MessageBoxControl.MessageBoxChildWindow msgW =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            msgW.Show();
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes && CurrentVisit != null)
            {
                if (ServiceList != null && ServiceList.Count > 0)
                {
                    clsAddUpdatePatientInvestigationsBizActionVO BizAction = new clsAddUpdatePatientInvestigationsBizActionVO();
                    BizAction.DoctorCode = this.CurrentVisit.DoctorCode;
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.IsOtherServices = true;
                    BizAction.InvestigationList = new List<clsDoctorSuggestedServiceDetailVO>();
                    ServiceList.ToList().ForEach(item => item.IsOther = true);
                    BizAction.InvestigationList = ServiceList.ToList();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                this.Content = null;
                                NavigateToNextMenu();
                            };
                            msgW1.Show();
                        }
                        else
                        {
                            string strErrorMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                            ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    string sErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ServiceValidation_Msg");
                    ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
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

        private void FillCurrentServices()
        {
            clsGetPatientCurrentServicesBizActionVO BizAction = new clsGetPatientCurrentServicesBizActionVO();
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.IsOtherServices = true;
            BizAction.VisitID = CurrentVisit.ID;

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
                        dgOtherServices.ItemsSource = null;
                        dgOtherServices.ItemsSource = ServiceList;
                        dgOtherServices.UpdateLayout();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgOtherServices.SelectedItem != null)
            {
                clsDoctorSuggestedServiceDetailVO obj = dgOtherServices.SelectedItem as clsDoctorSuggestedServiceDetailVO;
                clsDoctorSuggestedServiceDetailVO obj1 = ServiceList.Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault();
                ServiceList.Remove(obj1);
            }
            else
            {
                string sErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectService_Msg");
                ShowMessageBox("Please select the service", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            }
        }

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            MessageBoxControl.MessageBoxChildWindow msgWinCancel =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            msgWinCancel.Show();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                this.Content = null;
                ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
            }
        }

        private void NavigateToNextMenu()
        {
            EMR.frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }

    }
}
