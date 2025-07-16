using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamic.Localization;
namespace EMR
{
    public partial class frmAssignDoctor : ChildWindow
    {
        public frmAssignDoctor()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = LocalizationManager.resourceManager.GetString("ttlAssignDoctor");
            FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
        }
        public event RoutedEventHandler OnAddButton_Click;
        public long ServiceID = 0;
        public List<MasterListItem> ServiceDoctorList { get; set; }
        private bool UsePrevDoctorID = false;

        private void FillDepartmentList(long iUnitId)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;
                if (iUnitId > 0)
                    BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbDeparment.ItemsSource = null;
                        cmbDeparment.ItemsSource = objList;
                        cmbDeparment.SelectedItem = objList[0];
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception) { }

        }

        private void FillDoctor(long iUnitID, long iDeptID)
        {
            try
            {
                if (iDeptID != null && iDeptID > 0)
                {

                    clsGetDoctorlistforReferralAsperServiceBizActionVO BizAction = new clsGetDoctorlistforReferralAsperServiceBizActionVO();
                    BizAction.UnitId = iUnitID;
                    BizAction.DepartmentId = iDeptID;
                    BizAction.ServiceId = ServiceID;

                    if ((MasterListItem)cmbDeparment.SelectedItem != null)
                    {
                        BizAction.DepartmentId = iDeptID;
                    }
                    else
                    {
                        BizAction.DepartmentId = 0;
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsGetDoctorlistforReferralAsperServiceBizActionVO)arg.Result).MasterList != null)
                            {
                                List<MasterListItem> objList = new List<MasterListItem>();
                                objList.Add(new MasterListItem(0, "-- Select --"));
                                objList.AddRange(((clsGetDoctorlistforReferralAsperServiceBizActionVO)arg.Result).MasterList);
                                cmbDoctor.ItemsSource = null;
                                cmbDoctor.ItemsSource = objList;
                                cmbDoctor.SelectedItem = objList[0];
                            }
                        }

                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception) { }
        }

        private void FillDoctoratLoad()
        {
            try
            {
                clsGetDoctorlistReferralServiceBizActionVO BizAction = new clsGetDoctorlistReferralServiceBizActionVO();
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.ServiceId = ServiceID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetDoctorlistReferralServiceBizActionVO)arg.Result).MasterList != null)
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            objList.Add(new MasterListItem(0, "-- Select --"));
                            objList.AddRange(((clsGetDoctorlistReferralServiceBizActionVO)arg.Result).MasterList);
                            cmbDoctor.ItemsSource = null;
                            cmbDoctor.ItemsSource = objList;
                            cmbDoctor.SelectedItem = objList[0];
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            { }
        }
        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDoctor.TextBox != null)
                cmbDoctor.TextBox.Text = "";
            cmbDoctor.Text = "";
            cmbDoctor.ItemsSource = null;
            if ((MasterListItem)cmbDeparment.SelectedItem != null)
            {
                FillDoctor(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDeparment.SelectedItem).ID);

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID != 0)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

