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
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.IPD
{
    public partial class frmAdviseDischarge : ChildWindow
    {
        bool IsPatientExist = true;
        long newVisitAdmID, newVisitAdmUnitID;
        public PagedSortableCollectionView<clsIPDAdmissionVO> DataList { get; private set; }

        public frmAdviseDischarge()
        {
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                newVisitAdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                newVisitAdmUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                IsPatientExist = true;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + ((clsIPDAdmissionVO)((IApplicationConfiguration)App.Current).SelectedIPDPatient).PatientName;

                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.IsDischarge == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Already Discharged.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW5.Show();
                    IsPatientExist = false;
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW5 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW5.Show();
                IsPatientExist = false;
            }
            
            InitializeComponent();
        }

        void frmAdviseDischarge_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
                this.DialogResult = false;
            else
            {
                FillDepartments();
                SetPatientDetails();
                OKButton.IsEnabled = false;
            }
            newAddAdviseList = new List<MasterListItem>();
        }
        #region IInitiateCIMS Members

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #endregion
        List<MasterListItem> objList = new List<MasterListItem>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Are you sure you want to save ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    SaveAdviseDischarge();
                }
            };
            msgW.Show();
        }

        #region Private Methods
        private void FillDepartments()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_DepartmentMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)ea.Result).MasterList);
                    }
                    dgAdviseDischargeList.ItemsSource = null;
                    dgAdviseDischargeList.ItemsSource = objList;
                    GetAdvisedDischargeListByAdmIDAndAdmUnitID();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {

            }
        }
        List<MasterListItem> newAddAdviseList = new List<MasterListItem>();
        public void SaveAdviseDischarge()
        {
            clsAddIPDAdviseDischargeListBizActionVO bizActionVO = new clsAddIPDAdviseDischargeListBizActionVO();
            bizActionVO.AddAdviseDetails = new clsIPDAdmissionVO();

            bizActionVO.AddAdviseDList = new List<MasterListItem>();

            bizActionVO.AddAdviseDList = newAddAdviseList;

            //foreach (var item in objList)
            //{
            //    if (item.Status == true)
            //    {
            //        bizActionVO.AddAdviseDList.Add(item);
            //    }
            //}
            if (bizActionVO.AddAdviseDList.Count > 0)
            {
                bizActionVO.AddAdviseDetails.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                bizActionVO.AddAdviseDetails.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string msgTitle = "";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Record saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        this.DialogResult = true;

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                string msgTitle = "PALASH";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Department", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW.Show();
            }
        }

        private void GetAdvisedDischargeListByAdmIDAndAdmUnitID()
        {
            clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO BizAction = new clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO();
            BizAction.GetAdviseDetails = new clsIPDAdmissionVO();
            BizAction.GetAdviseDList = new List<clsIPDAdmissionVO>();

            BizAction.GetAdviseDetails.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
            BizAction.GetAdviseDetails.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO result = arg.Result as clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO;
                    if (result.GetAdviseDList != null)
                    {
                        foreach (var item in result.GetAdviseDList)
                        {
                            foreach (var objData in objList)
                            {
                                if (objData.ID == item.DepartmentID)
                                {
                                    objData.Status = true;
                                    if (objData.Status == true)
                                    {
                                        objData.IsEnable = false;
                                    }
                                    else
                                    {
                                        objData.IsEnable = true;
                                    }
                                }
                            }
                        }
                        dgAdviseDischargeList.ItemsSource = null;
                        dgAdviseDischargeList.ItemsSource = objList;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        private void SetPatientDetails()
        {
            if (IsPatientExist == true)
            {
                lblPatientName1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                lblAdmissionDate1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionDate.ToString().Substring(0,10);
                lblAdmissionNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                lblMrNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                lblPatientGender1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName;
            }
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkStatusSelect_Click(object sender, RoutedEventArgs e)
        {
            List<MasterListItem> SelectDepartment = new List<MasterListItem>();
            SelectDepartment = ((List<MasterListItem>)dgAdviseDischargeList.ItemsSource).ToList();

            var item = from r in SelectDepartment
                       where r.Status == true && r.IsEnable ==true
                       select r;
            if (((CheckBox)sender).IsChecked == true)
            {
                newAddAdviseList.Add(((MasterListItem)dgAdviseDischargeList.SelectedItem));
            }
            else
            {
                newAddAdviseList.Remove(((MasterListItem)dgAdviseDischargeList.SelectedItem));
            }

            if (item != null && item.ToList().Count > 0)
            {
                OKButton.IsEnabled = true;
            }
            else
            {
                OKButton.IsEnabled = false;
            }
        }
    }
}
