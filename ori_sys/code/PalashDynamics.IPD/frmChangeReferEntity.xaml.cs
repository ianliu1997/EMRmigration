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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;

namespace PalashDynamics.IPD
{
    public partial class frmChangeReferEntity : ChildWindow
    {
        bool IsPatientExist = true;
        long VisitID, VisitUnitID;
        ObservableCollection<clsChangeReferEntityVO> RefEntityList = null;
        bool IsOther = false;
        List<clsChangeReferEntityVO> todelete;
        List<clsChangeReferEntityVO> tolist;
        long RefEntityType = 0, RefEntity = 0;
        public frmChangeReferEntity()
        {
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.IsDischarge == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Already Discharged.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW5.Show();
                    IsPatientExist = false;
                }
                else
                {
                    VisitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    RefEntity = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.RefEntityID;
                    RefEntityType = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.RefEntityTypeID;
                    GetRefEntityInfo();
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
            RefEntityList = new ObservableCollection<clsChangeReferEntityVO>();
            this.Loaded += new RoutedEventHandler(frmChangeReferEntity_Loaded);
        }
        void frmChangeReferEntity_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
                this.DialogResult = false;
            else
            {
                FillRefEntityType();
                SetPatientDetails();
                OKButton.IsEnabled = false;
                todelete = new List<clsChangeReferEntityVO>();
                tolist = new List<clsChangeReferEntityVO>();
                RefEntityList = new ObservableCollection<clsChangeReferEntityVO>();
            }
        }
        #region IInitiateCIMS Members


        #endregion
        private void FillRefEntityType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RefEntityMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbRefEntityType.ItemsSource = null;
                    cmbRefEntityType.ItemsSource = objList;
                    cmbRefEntityType.SelectedItem = objList[0];

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }
        List<clsRefEntityDetailsVO> DataList = new List<clsRefEntityDetailsVO>();

        #region Private Methods
        int count = 0;
        public void GetRefEntityInfo()
        {
            clsGetRefEntityDetailsBizActionVO Bizaction = new clsGetRefEntityDetailsBizActionVO();
            Bizaction.Details = new clsRefEntityDetailsVO();
            Bizaction.Details.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
            Bizaction.Details.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        Bizaction = (clsGetRefEntityDetailsBizActionVO)args.Result;
                        if (Bizaction.List.Count > 0)
                        {
                            foreach (clsRefEntityDetailsVO item in Bizaction.List)
                            {
                                DataList.Add(item);
                            }
                            dgRefEntity.ItemsSource = null;
                            dgRefEntity.ItemsSource = DataList;
                            count = DataList.Count;
                        }
                    }
                };
                client.ProcessAsync(Bizaction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }
        
        private void SetPatientDetails()
        {
            if (IsPatientExist == true)
            {
                lblPatientName1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                lblAdmissionDate1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionDate.ToString().Substring(0, 10);
                lblAdmissionNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                lblMrNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                lblPatientGender1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName;
            }
        }

        #endregion
        public bool CheckValidations()
        {
            bool result = true;

            if ((MasterListItem)cmbRefEntityType.SelectedItem == null)
            {
                cmbRefEntityType.TextBox.SetValidation("Please select Ref Type Entity");

                cmbRefEntityType.TextBox.RaiseValidationError();
                cmbRefEntityType.Focus();

                result = false;
            }
            else if (((MasterListItem)cmbRefEntityType.SelectedItem).ID == 0)
            {
                cmbRefEntityType.TextBox.SetValidation("Please select the Doctor");

                cmbRefEntityType.TextBox.RaiseValidationError();
                cmbRefEntityType.Focus();

                result = false;
            }
            else
                cmbRefEntityType.ClearValidationError();
            if (cmbRefEntity.SelectedItem == null)
            {
                cmbRefEntity.SetValidation("Please select Ref Entity");

                cmbRefEntity.RaiseValidationError();
                cmbRefEntity.Focus();

                result = false;
            }
            else
                cmbRefEntity.ClearValidationError();
            return result;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }
        long RefID=0, RefTypeID=0;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidations())
            {
                btnAdd.IsEnabled = false;
                clsRefEntityDetailsVO Details = new clsRefEntityDetailsVO();
                Details.RefEntityIDDesc = ((MasterListItem)cmbRefEntity.SelectedItem).Description;
                Details.RefEntityTypeIDDesc = ((MasterListItem)cmbRefEntityType.SelectedItem).Description;
                RefID = ((MasterListItem)cmbRefEntity.SelectedItem).ID;
                RefTypeID = ((MasterListItem)cmbRefEntityType.SelectedItem).ID;
                DataList.Add(Details);
                dgRefEntity.ItemsSource = null;
                dgRefEntity.ItemsSource = DataList;
                // cmbRefEntityType.SelectedItem = 0;
                cmbRefEntity.Text ="";
                if (DataList.Count > count)
                {
                    OKButton.IsEnabled = true;
                    FillRefEntityType();
                }
            }
        }

        private void SaveRefEntity()
        {
            clsAddRefEntityDetailsBizActionVO bizAction = new clsAddRefEntityDetailsBizActionVO();
            bizAction.Details = new clsRefEntityDetailsVO();
            bizAction.Details.UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
            bizAction.Details.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
            bizAction.Details.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
            bizAction.Details.RefEntityID = RefID;
            bizAction.Details.RefEntityTypeID = RefTypeID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    bizAction = (clsAddRefEntityDetailsBizActionVO)args.Result;
                    if (bizAction.SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Details Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                this.DialogResult = true;
                            }
                        };
                        msgW1.Show();
                    }
                }
            };
            client.ProcessAsync(bizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Save Referral Entity", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    SaveRefEntity();
                }
            };
            msgW1.Show();   
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgRefEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbRefEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (((MasterListItem)cmbRefEntityType.SelectedItem).ID == 0)
            {
                cmbRefEntity.ItemsSource = null;
            }
            else if (((MasterListItem)cmbRefEntityType.SelectedItem).ID == 1)
            {
                cmbRefEntity.ItemsSource = null;
                fillDoctor();
            }
            else if (((MasterListItem)cmbRefEntityType.SelectedItem).ID == 3)
            {
                cmbRefEntity.ItemsSource = null;
            }
            else if (((MasterListItem)cmbRefEntityType.SelectedItem).ID == 2)
            {
                cmbRefEntity.ItemsSource = null;
                fillCompany();
            }
        }
        List<MasterListItem> objList = new List<MasterListItem>();
        List<MasterListItem> DoctorList = new List<MasterListItem>();
        List<MasterListItem> CompanyList = new List<MasterListItem>();
        private void fillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    DoctorList = new List<MasterListItem>();
                    DoctorList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    cmbRefEntity.ItemsSource = null;
                    cmbRefEntity.ItemsSource = DoctorList;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void fillCompany()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    CompanyList = new List<MasterListItem>();
                    CompanyList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbRefEntity.ItemsSource = null;
                    cmbRefEntity.ItemsSource = CompanyList;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void cmdDeleteRefEntity_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    class ReferalEntityList
    {
        public long RefEntityTypeID { get; set; }
        public long RefEntityID { get; set; }
    }

}
