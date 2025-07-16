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
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;

namespace PalashDynamics.Forms.Home
{
    public partial class frmDischargeApproval : UserControl
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        public long _AdmissionId = 0;
        public long _AdmissionUnitId = 0;
        public frmDischargeApproval()
        {
            InitializeComponent();
            GetPatientDischargeApprovalList();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GetPatientDischargeApprovalList()
        {
            clsGetIPDAdmissionListBizActionVO BizAction = new clsGetIPDAdmissionListBizActionVO();
            BizAction.AdmDetails = new clsIPDAdmissionVO();
            BizAction.IsDischargeApproval = true;
            BizAction.IsPagingEnabled = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetIPDAdmissionListBizActionVO result = arg.Result as clsGetIPDAdmissionListBizActionVO;
                    //DataList.TotalItemCount = result.TotalRows;
                    if (result.AdmList != null)
                    {
                        dgPatientAdmissionList.ItemsSource = null;
                        dgPatientAdmissionList.ItemsSource = result.AdmList;
                    }

                }
                
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dgPatientAdmissionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                GetApprovalDepartmentList();
        }

        public void GetApprovalDepartmentList()
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsGetListOfAdviseDischargeForApprovalBizActionVO BizAction = new clsGetListOfAdviseDischargeForApprovalBizActionVO();
                BizAction.AddAdviseDetails = new clsDiischargeApprovedByDepartmentVO();
                BizAction.AddAdviseDetails.AdmissionId = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).ID;
                BizAction.AddAdviseDetails.AdmissionUnitID = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetListOfAdviseDischargeForApprovalBizActionVO result = arg.Result as clsGetListOfAdviseDischargeForApprovalBizActionVO;
                        _AdmissionId = BizAction.AddAdviseDetails.AdmissionId;
                        _AdmissionUnitId = BizAction.AddAdviseDetails.AdmissionUnitID;
                        dgDepartmentList.ItemsSource = result.AddAdviseList;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }
        private void chkApproval_Click(object sender, RoutedEventArgs e)
        {
            if (dgDepartmentList.SelectedItem != null)
            {
                clsDiischargeApprovedByDepartmentVO obj = new clsDiischargeApprovedByDepartmentVO();
                obj = (clsDiischargeApprovedByDepartmentVO)dgDepartmentList.SelectedItem;
                if (obj.ApprovalStatus)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Approve?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                    msgWD_1.Show();
                }
            }
        }

        private void msgWD_1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddDiischargeApprovedByDepartmentBizActionVO BizActionobj = new clsAddDiischargeApprovedByDepartmentBizActionVO();
                BizActionobj.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
                clsDiischargeApprovedByDepartmentVO obj = new clsDiischargeApprovedByDepartmentVO();
                obj.ID = ((clsDiischargeApprovedByDepartmentVO)dgDepartmentList.SelectedItem).ID;
                obj.DepartmentID = ((clsDiischargeApprovedByDepartmentVO)dgDepartmentList.SelectedItem).DepartmentID;
                BizActionobj.IsUpdateApproval = true;
                //obj.DepartmentName = item.Department;
                obj.AdmissionId = _AdmissionId;
                obj.AdmissionUnitID = _AdmissionUnitId;
                obj.ApprovalRemark = ((clsDiischargeApprovedByDepartmentVO)dgDepartmentList.SelectedItem).ApprovalRemark;
                obj.ApprovalStatus = ((clsDiischargeApprovedByDepartmentVO)dgDepartmentList.SelectedItem).ApprovalStatus;
                BizActionobj.AddAdviseList.Add(obj);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Approve Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD_1.Show();
                        GetApprovalDepartmentList();
                    }
                };
                Client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else if (result == MessageBoxResult.No)
            {
                GetApprovalDepartmentList(); 
            }
        }
    }
}
