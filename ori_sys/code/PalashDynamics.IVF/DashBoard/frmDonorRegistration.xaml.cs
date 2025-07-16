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
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmDonorRegistration : UserControl
    {
        #region Public variables
        bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        public bool IsCancel = true;
        #endregion
        public frmDonorRegistration()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            DataList = new PagedSortableCollectionView<clsPatientVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDonorList();
        }
        private void CmdApplyBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                frmApplyBatchForDonor win = new frmApplyBatchForDonor();
                win.DonorID = ((clsPatientVO)dataGrid2.SelectedItem).DonorID;
                win.DonorUnitID = ((clsPatientVO)dataGrid2.SelectedItem).DonorUnitID;
                win.Title = ((clsPatientVO)dataGrid2.SelectedItem).DonorCode;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        #region Fill Combobox

        private void FillBloodGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
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

                    cmbBloodGroup.ItemsSource = null;
                    cmbBloodGroup.ItemsSource = objList.DeepCopy();
                    cmbBloodGroup.SelectedItem = objList[0];

                    cmbSearchBloodGroup.ItemsSource = null;
                    cmbSearchBloodGroup.ItemsSource = objList.DeepCopy();
                    cmbSearchBloodGroup.SelectedItem = objList[0];

                    FillHairColor();

                }

                if (this.DataContext != null)
                {
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillHairColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_HairColor;
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
                    cmbHairColor.ItemsSource = null;
                    cmbHairColor.ItemsSource = objList.DeepCopy();
                    cmbHairColor.SelectedItem = objList[0];

                    cmbSearchHairColor.ItemsSource = null;
                    cmbSearchHairColor.ItemsSource = objList.DeepCopy();
                    cmbSearchHairColor.SelectedItem = objList[0];

                    FillEyeColor();

                }
                if (this.DataContext != null)
                {
                    cmbHairColor.SelectedValue = ((clsPatientVO)this.DataContext).HairColorID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillEyeColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_EyeColor;
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
                    cmbEyeColor.ItemsSource = null;
                    cmbEyeColor.ItemsSource = objList.DeepCopy();
                    cmbEyeColor.SelectedItem = objList[0];

                    cmbSearchEyecolor.ItemsSource = null;
                    cmbSearchEyecolor.ItemsSource = objList.DeepCopy();
                    cmbSearchEyecolor.SelectedItem = objList[0];

                    FillSkinColor();

                }
                if (this.DataContext != null)
                {
                    cmbEyeColor.SelectedValue = ((clsPatientVO)this.DataContext).EyeColorID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillSkinColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SkinColor;
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
                    cmbSkinColor.ItemsSource = null;
                    cmbSkinColor.ItemsSource = objList.DeepCopy();
                    cmbSkinColor.SelectedItem = objList[0];


                    cmbSearchSkinColor.ItemsSource = null;
                    cmbSearchSkinColor.ItemsSource = objList.DeepCopy();
                    cmbSearchSkinColor.SelectedItem = objList[0];

                    FillReferralName();
                }
                if (this.DataContext != null)
                {
                    cmbSkinColor.SelectedValue = ((clsPatientVO)this.DataContext).SkinColorID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillReferralName()
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
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


                    cmbReferralName.ItemsSource = null;
                    cmbReferralName.ItemsSource = objList;
                    cmbReferralName.SelectedItem = objList[0];


                }
                FillSurrogateAgency((long)0);
                if (this.DataContext != null)
                {
                    cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void FillAgency()
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SurrogateAgencyMaster;
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

                    cmbSearchAgencyName.ItemsSource = null;
                    cmbSearchAgencyName.ItemsSource = objList;
                    cmbSearchAgencyName.SelectedItem = objList[0];


                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillSurrogateAgency(long Refvalue)
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SurrogateAgencyMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = Convert.ToString(Refvalue);
            BizAction.Parent.Value = "ReferralTypeID";
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

                    cmbAgencyName.ItemsSource = null;
                    cmbAgencyName.ItemsSource = objList;
                    cmbAgencyName.SelectedItem = objList[0];


                }

                if (this.DataContext != null)
                {
                    cmbAgencyName.SelectedValue = ((clsPatientVO)this.DataContext).AgencyID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Load");
            FillBloodGroup();
            FillAgency();
            FillDonorList();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            ClearFormData();
            this.DataContext = new clsPatientVO();
            objAnimation.Invoke(RotationType.Forward);
        }
        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {

            switch (strFormMode)
            {
                case "Load":
                    cmdNew.Visibility = Visibility.Visible;
                    cmdSave.Visibility = Visibility.Collapsed;
                    CmdApplyBatch.Visibility = Visibility.Visible;
                    CmdClose.IsEnabled = true;

                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.Visibility = Visibility.Collapsed;
                    cmdSave.Visibility = Visibility.Visible;
                    CmdApplyBatch.Visibility = Visibility.Collapsed;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.Visibility = Visibility.Visible;
                    cmdSave.Visibility = Visibility.Collapsed;
                    CmdApplyBatch.Visibility = Visibility.Visible;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.Visibility = Visibility.Visible;
                    cmdSave.Visibility = Visibility.Collapsed;
                    CmdApplyBatch.Visibility = Visibility.Visible;
                    CmdClose.IsEnabled = true;

                    IsCancel = true;
                    break;
                default:
                    break;
            }
        }

        #endregion
        private void ClearFormData()
        {
            txtDonorCode.Text = string.Empty;
            cmbHairColor.SelectedValue = (long)0;
            cmbBloodGroup.SelectedValue = (long)0;
            cmbEyeColor.SelectedValue = (long)0;
            cmbSkinColor.SelectedValue = (long)0;
            cmbReferralName.SelectedValue = (long)0;
            cmbAgencyName.SelectedValue = (long)0;
            txtBoneStructure.Text = string.Empty;
            txtHeight.Text = string.Empty;

        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == true)
            {
                new frmDonorRegistration();
            }
            else
            {
                IsCancel = true;
                IsModify = false;
                objAnimation.Invoke(RotationType.Backward);
            }
            SetCommandButtonState("Cancel");
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void dgSpremFreezingDetailsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private bool Validate()
        {
            bool result = true;

            if (txtDonorCode.Text == string.Empty)
            {
                txtDonorCode.SetValidation("Please Enter Donor Code");
                txtDonorCode.RaiseValidationError();
                txtDonorCode.Focus();
                result = false;
            }
            else
                txtDonorCode.ClearValidationError();
            //if (rbtNew.IsChecked == true)
            //{
            //    if (cmbLab.SelectedItem == null)
            //    {
            //        cmbLab.TextBox.SetValidation("Please select Lab");
            //        cmbLab.TextBox.RaiseValidationError();
            //        cmbLab.Focus();
            //        result = false;
            //    }
            //    else if (((MasterListItem)cmbLab.SelectedItem).ID == 0)
            //    {
            //        cmbLab.TextBox.SetValidation("Please select Lab");
            //        cmbLab.TextBox.RaiseValidationError();
            //        cmbLab.Focus();
            //        result = false;
            //    }
            //    else
            //        cmbLab.TextBox.ClearValidationError();

            //    if (dtpReceivedDate.SelectedDate == null)
            //    {
            //        dtpReceivedDate.SetValidation("Please Select Received Date");
            //        dtpReceivedDate.RaiseValidationError();
            //        dtpReceivedDate.Focus();
            //        return false;
            //    }
            //    else
            //        dtpReceivedDate.ClearValidationError();
            //    if (txtBatch.Text == string.Empty)
            //    {
            //        txtBatch.SetValidation("Please Enter Batch Code");
            //        txtBatch.RaiseValidationError();
            //        txtBatch.Focus();
            //        result = false;
            //    }
            //    else
            //        txtBatch.ClearValidationError();

            //}
            return result;
        }
        int ClickedFlag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {

                if (Validate())
                {
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Donor Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }

            }
            else
                ClickedFlag = 0;

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDonor();
            else
                ClickedFlag = 0;

        }
        private void SaveDonor()
        {



            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddUpdateDonorBizActionVO BizAction = new clsAddUpdateDonorBizActionVO();
            BizAction.DonorDetails = new clsPatientVO();
            BizAction.DonorDetails = (clsPatientVO)this.DataContext;
            if (IsModify == true)
            {
                BizAction.DonorDetails.GeneralDetails.PatientID = ((clsPatientVO)this.DataContext).DonorID;
                BizAction.DonorDetails.GeneralDetails.PatientUnitID = ((clsPatientVO)this.DataContext).DonorUnitID;
            }
            if (cmbBloodGroup.SelectedItem != null)
                BizAction.DonorDetails.BloodGroupID = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;
            BizAction.DonorDetails.GenderID = (long)1;
            BizAction.DonorDetails.GeneralDetails.PatientTypeID = (long)9;
            BizAction.DonorDetails.IsDonor = true;
            if (cmbSkinColor.SelectedItem != null)
                BizAction.DonorDetails.SkinColorID = ((MasterListItem)cmbSkinColor.SelectedItem).ID;
            if (cmbHairColor.SelectedItem != null)
                BizAction.DonorDetails.HairColorID = ((MasterListItem)cmbHairColor.SelectedItem).ID;
            if (cmbEyeColor.SelectedItem != null)
                BizAction.DonorDetails.EyeColorID = ((MasterListItem)cmbEyeColor.SelectedItem).ID;
            BizAction.DonorDetails.BoneStructure = txtBoneStructure.Text;
            BizAction.DonorDetails.DonorSourceID = (long)1;
            if (txtHeight.Text != null)
                BizAction.DonorDetails.Height = Convert.ToDouble(txtHeight.Text);
            BizAction.DonorDetails.DonorCode = txtDonorCode.Text;
            BizAction.DonorDetails.Photo = null;
            if (cmbReferralName.SelectedItem != null)
                BizAction.DonorDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;
            if (cmbAgencyName.SelectedItem != null)
                BizAction.DonorDetails.GeneralDetails.AgencyID = ((MasterListItem)cmbAgencyName.SelectedItem).ID;

            //if (dgPastBatchList.SelectedItem != null && rbtExisting.IsChecked == true)
            //{
            //    BizAction.BatchDetails.BatchCode = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).BatchCode;
            //    BizAction.BatchDetails.ReceivedDate = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).ReceivedDate;
            //    BizAction.BatchDetails.ReceivedByID = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).ReceivedByID;
            //    BizAction.BatchDetails.LabID = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).LabID;
            //    BizAction.BatchDetails.InvoiceNo = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).InvoiceNo;
            //    BizAction.BatchDetails.NoOfVails = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).NoOfVails;
            //    BizAction.BatchDetails.Remark = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).Remark;
            //    BizAction.BatchDetails.ID = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).ID;
            //    BizAction.BatchDetails.UnitID = ((clsDonorBatchVO)dgPastBatchList.SelectedItem).UnitID;
            //}
            //else
            //{

            //    BizAction.BatchDetails.BatchCode = txtBatch.Text;
            //    if (dtpReceivedDate.SelectedDate != null)
            //        BizAction.BatchDetails.ReceivedDate = dtpReceivedDate.SelectedDate.Value.Date;
            //    if (cmbReceivedBy.SelectedItem != null)
            //        BizAction.BatchDetails.ReceivedByID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
            //    if (cmbLab.SelectedItem != null)
            //        BizAction.BatchDetails.LabID = ((MasterListItem)cmbLab.SelectedItem).ID;
            //    BizAction.BatchDetails.InvoiceNo = txtInvoiceNumber.Text;
            //    if (txtNoofVials.Text != null)
            //        BizAction.BatchDetails.NoOfVails = Convert.ToInt32(txtNoofVials.Text);
            //    BizAction.BatchDetails.Remark = txtRemark.Text;
            //    BizAction.BatchDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    SetCommandButtonState("Save");

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    IsModify = false;
                    FillDonorList();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                }
                else
                {
                    ClickedFlag = 0;
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    Indicatior.Close();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        public PagedSortableCollectionView<clsPatientVO> DataList { get; private set; }
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        private void FillDonorList()
        {
            clsGetDonorListBizActionVO BizActionVo = new clsGetDonorListBizActionVO();
            BizActionVo.DonorGeneralDetailsList = new List<clsPatientVO>();
            BizActionVo.DonorDetails = new clsPatientVO();
            if (txtSearchDonorCode.Text != string.Empty)
                BizActionVo.DonorDetails.DonorCode = txtSearchDonorCode.Text.Trim();
            if (cmbSearchAgencyName.SelectedItem != null)
                BizActionVo.DonorDetails.AgencyID = ((MasterListItem)cmbSearchAgencyName.SelectedItem).ID;
            if (cmbSearchHairColor.SelectedItem != null)
                BizActionVo.DonorDetails.HairColorID = ((MasterListItem)cmbSearchHairColor.SelectedItem).ID;
            if (cmbSearchSkinColor.SelectedItem != null)
                BizActionVo.DonorDetails.SkinColorID = ((MasterListItem)cmbSearchSkinColor.SelectedItem).ID;
            if (cmbSearchEyecolor.SelectedItem != null)
                BizActionVo.DonorDetails.EyeColorID = ((MasterListItem)cmbSearchEyecolor.SelectedItem).ID;
            if (cmbSearchBloodGroup.SelectedItem != null)
                BizActionVo.DonorDetails.BloodGroupID = ((MasterListItem)cmbSearchBloodGroup.SelectedItem).ID;



            BizActionVo.IsPagingEnabled = true;
            BizActionVo.MaximumRows = DataList.PageSize; ;
            BizActionVo.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        clsGetDonorListBizActionVO result = ea.Result as clsGetDonorListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientVO person in result.DonorGeneralDetailsList)
                        {
                            DataList.Add(person);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizActionVo.MaximumRows;
                        dgDataPager.Source = DataList;
                    }
                }
            };
            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            FillDonorList();
        }
        long ReferralNamevalue = 0;
        private void cmbReferralName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbReferralName.SelectedItem != null)
            {
                ReferralNamevalue = ((MasterListItem)cmbReferralName.SelectedItem).ID;
                if (ReferralNamevalue > 0)
                    FillSurrogateAgency(ReferralNamevalue);
            }
        }
        bool IsModify;
        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                SetCommandButtonState("New");
                objAnimation.Invoke(RotationType.Forward);
                IsModify = true;
                this.DataContext = (clsPatientVO)dataGrid2.SelectedItem;
                if (((clsPatientVO)dataGrid2.SelectedItem).DonorCode != null)
                    txtDonorCode.Text = ((clsPatientVO)dataGrid2.SelectedItem).DonorCode;
                if (((clsPatientVO)dataGrid2.SelectedItem).SkinColorID != null)
                    cmbSkinColor.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).SkinColorID;
                if (((clsPatientVO)dataGrid2.SelectedItem).EyeColorID != null)
                    cmbEyeColor.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).EyeColorID;
                if (((clsPatientVO)dataGrid2.SelectedItem).HairColorID != null)
                    cmbHairColor.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).HairColorID;
                if (((clsPatientVO)dataGrid2.SelectedItem).BloodGroupID != null)
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).BloodGroupID;
                if (((clsPatientVO)dataGrid2.SelectedItem).BoneStructure != null)
                    txtBoneStructure.Text = ((clsPatientVO)dataGrid2.SelectedItem).BoneStructure;
                if (((clsPatientVO)dataGrid2.SelectedItem).Height != null)
                    txtHeight.Text = Convert.ToString(((clsPatientVO)dataGrid2.SelectedItem).Height);
                if (((clsPatientVO)dataGrid2.SelectedItem).ReferralTypeID != null)
                    cmbReferralName.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).ReferralTypeID;
                if (((clsPatientVO)dataGrid2.SelectedItem).AgencyID != null)
                    cmbAgencyName.SelectedValue = ((clsPatientVO)dataGrid2.SelectedItem).AgencyID;

            }
        }
    }
}
