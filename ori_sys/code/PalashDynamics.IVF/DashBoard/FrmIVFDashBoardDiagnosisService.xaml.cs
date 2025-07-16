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
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Data;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.DashBoardVO;
namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmIVFDashBoardDiagnosisService : ChildWindow
    {
        public long PatientID { get; set; }
        public long TherapyID { get; set; }
        ObservableCollection<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }
        List<MasterListItem> Priority;
        public FrmIVFDashBoardDiagnosisService()
        {
            InitializeComponent();
            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void cmdAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            frmIVFDashBoardCPOELabServicesSelection Win = new frmIVFDashBoardCPOELabServicesSelection();
            Win.OnAddButton_Click += new RoutedEventHandler(WinLab_OnAddButton_Click);
            Win.Show();
        }
        void WinLab_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmIVFDashBoardCPOELabServicesSelection winService = (frmIVFDashBoardCPOELabServicesSelection)sender;
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
                            OBj.SpecializationId = item.Specialization;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
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
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                dgServiceList.ItemsSource = pcv;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }
        private void lnkAddSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ServiceList != null && ServiceList.Count > 0)
                {
                    SaveInvestigations();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Add the CPOE Service details..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            catch(Exception ex)
            {
            }
        }
        private void SaveInvestigations()
        {
            clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO BizAction = new clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO();
            BizAction.PatientID = PatientID;
            BizAction.TherapyID = TherapyID;
            BizAction.InvestigationList = ServiceList.ToList();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "CPOE Services Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void lnkCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void cmdDeleteInvest_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to delete the record ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ServiceList.RemoveAt(dgServiceList.SelectedIndex);
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = ServiceList;
                            dgServiceList.UpdateLayout();
                        }
                    }
                };
                msgWD.Show();
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
                            GetCurrentServices();
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
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillReferenceType();
        }
        private void GetCurrentServices()
        {
            clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO BizAction = new clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO();
            BizAction.PatientID = PatientID;
            BizAction.TherapyID = TherapyID;
            #region Service Call (Check Validation)
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                       clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO ObjBizAction = ((clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO)args.Result);
                        if (ObjBizAction.ServiceDetails != null && ObjBizAction.ServiceDetails.Count > 0)
                        {
                            ServiceList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
                            foreach (var item in ObjBizAction.ServiceDetails)
                            {
                                item.Priority = this.Priority;
                                if (Priority != null)
                                {
                                    item.SelectedPriority = Priority.Where(z => z.ID == item.PriorityIndex).FirstOrDefault();
                                }
                                ServiceList.Add(item);
                            }
                            dgServiceList.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                            dgServiceList.ItemsSource = pcv;
                        }
                        dgServiceList.UpdateLayout();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            #endregion
        }
    }
}

