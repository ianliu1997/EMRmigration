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
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.IPD
{
    public partial class frmDischargeApprovalDepartment : ChildWindow
    {
        List<clsUnitMasterVO> List = new List<clsUnitMasterVO>();
        public event RoutedEventHandler OnCancelButton_Click;
        public frmDischargeApprovalDepartment()
        {
            InitializeComponent();
            FillDepartmentList();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkValidation())
            {
                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                msgWD_1.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Atleast One Department", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD_1.Show();

            }
        }

        private void msgWD_1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddDiischargeApprovedByDepartmentBizActionVO BizActionobj = new clsAddDiischargeApprovedByDepartmentBizActionVO();
                BizActionobj.AddAdviseList = new List<clsDiischargeApprovedByDepartmentVO>();
                List = new List<clsUnitMasterVO>();
                List = dgDepartmentList.ItemsSource as List<clsUnitMasterVO>;
                foreach (var item in List)
                {
                    if (item.ChkSelect)
                    {
                        clsDiischargeApprovedByDepartmentVO obj = new clsDiischargeApprovedByDepartmentVO();
                        obj.ID = 0;
                        obj.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        obj.AdmissionId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
                        obj.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
                        obj.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        obj.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        obj.DepartmentID = item.DepartmentID;
                        obj.DepartmentName = item.Department;
                        obj.Remark = item.Remark == null ? Convert.ToString(txtRemark.Text) : item.Remark;
                        BizActionobj.AddAdviseList.Add(obj);
                    }
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Approval Send Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD_1.Show();
                        this.DialogResult = false;
                        if (OnCancelButton_Click != null)
                            OnCancelButton_Click(this, new RoutedEventArgs());
                    }
                };
                Client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        public void FillDepartmentList()
        {
            try
            {
                clsGetDepartmentListBizActionVO BizAction = new clsGetDepartmentListBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsGetDepartmentListBizActionVO)arg.Result).UnitDetails != null)
                            {
                                List = (from item in ((clsGetDepartmentListBizActionVO)arg.Result).UnitDetails
                                        where item.IsClinic == true
                                        select item).ToList();
                                dgDepartmentList.ItemsSource = List;
                            }
                            else
                            {
                                dgDepartmentList.ItemsSource = null;
                            }
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool chkValidation()
        {
            bool flag = true;
            List = new List<clsUnitMasterVO>();
            List = dgDepartmentList.ItemsSource as List<clsUnitMasterVO>;

            var xyz = List.Where(s => s.ChkSelect == true).ToList();
            if (xyz == null || xyz.Count <= 0)
            {
                flag = false;
            }
            return flag;
        }

    }
}

