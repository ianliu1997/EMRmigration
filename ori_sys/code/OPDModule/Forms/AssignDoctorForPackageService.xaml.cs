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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;

namespace OPDModule.Forms
{
    public partial class AssignDoctorForPackageService : ChildWindow
    {
        public long ServiceID { get; set; }
        public long VisitID { get; set; }
        List<MasterListItem> DoctorList = new List<MasterListItem>();

        ObservableCollection<clsPackageServiceDetailsVO> ServiceList { get; set; }
     
        public AssignDoctorForPackageService()
        {
            InitializeComponent();
            
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkValidation())
            {
                UpdateQueue();
            }
            
            
        }

        private void UpdateQueue()
        {
            clsUpdateDoctorInQueueBizActionVO BizAction = new clsUpdateDoctorInQueueBizActionVO();
            BizAction.VisitId = VisitID;
            BizAction.QueueDetails = ServiceList.ToList();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Assign successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    this.DialogResult = false;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    

        private void GetServiceDetails(long iServiceID)
        {
            clsGetPackageServiceFromServiceIDBizActionVO BizAction = new clsGetPackageServiceFromServiceIDBizActionVO();
            try
            {
                BizAction.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                BizAction.ServiceID = iServiceID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<clsPackageServiceDetailsVO> ObjList=((clsGetPackageServiceFromServiceIDBizActionVO)arg.Result).PackageDetailList;
                        foreach (var item in ObjList)
                        {
                            ServiceList.Add(item);    
                        }
                        dgServiceList.ItemsSource = ServiceList;
                       
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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceList = new ObservableCollection<clsPackageServiceDetailsVO>();
            GetServiceDetails(ServiceID);
        }

        private void AssignDoctor_Click(object sender, RoutedEventArgs e)
        {
            FillDoctor Win = new FillDoctor();
            Win.DepartmentId = ((clsPackageServiceDetailsVO)dgServiceList.SelectedItem).DepartmentID;
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillDoctor ObjDetails = (FillDoctor)sender;
            if (ObjDetails.DialogResult == true)
            {
                if (ObjDetails.cmbDoctor.SelectedItem != null)
                {
                    if (((MasterListItem)((ObjDetails).cmbDoctor.SelectedItem)).ID != 0)
                    {

                        ((clsPackageServiceDetailsVO)dgServiceList.SelectedItem).DoctorID = ((MasterListItem)((ObjDetails).cmbDoctor.SelectedItem)).ID;
                        ((clsPackageServiceDetailsVO)dgServiceList.SelectedItem).DoctorName = ((MasterListItem)((ObjDetails).cmbDoctor.SelectedItem)).Description;
                       
                    }
                }
            }
        }


        private bool chkValidation()
        {
            bool Result = true;

            if (ServiceList.Where(Items => Items.DoctorID == 0).Any() == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Doctor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW13.Show();
                Result = false;
                return Result;
            }
            
            return Result;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

