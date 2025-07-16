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
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.IVF.PatientList;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.IVF.DashBoard;

namespace PalashDynamics.IVF
{
    public partial class SemenDetails_DashBoard : ChildWindow
    {
        public long PatientID;
        public long PatientUnitID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long PlannedtreatmentID;
        public clsPlanTherapyVO SelectedTherapyDetails;
        public event RoutedEventHandler OnSaveButton_Click;
        public clsFemaleSemenDetailsVO Details;
        public bool IsUpdate = false;
        long ID;
        long UnitID;
        bool IsEdit = false;
        public clsCoupleVO coupleDetails;
        public bool IsClosed;
        public long SourceOfSperm = 0;
        public SemenDetails_DashBoard()
        {
            InitializeComponent();
            this.DataContext = new clsFemaleSemenDetailsVO();

            if (IsUpdate == true)
            {
                if (Details != null)
                {
                    this.DataContext = Details;
                }
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillMOSP();
            // fillNeedleSource();
            IsEdit = false;
            cmdSave.Content = "Save";
            //cmdSpermiogram.Visibility = Visibility.Collapsed;
            cmdAspiration.Visibility = Visibility.Collapsed;
            if (Details == null)
            {
                Details = new clsFemaleSemenDetailsVO();
            }
            else
            {
                this.DataContext = Details;
            }
            this.Title = "Semen Details :-(Cycle Code - " + SelectedTherapyDetails.Cyclecode + " )";
            // fillOPUDetails();
            //Getdetails();
        }

        // Fill Combo      
        private void FillMOSP()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_MethodOfSpermPreparationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMethodOfSpermpreparation.ItemsSource = null;
                    cmbMethodOfSpermpreparation.ItemsSource = objList;
                    cmbMethodOfSpermpreparation.SelectedItem = objList[0];

                    if (Details.MethodOfSpermPreparation != null && Details.MethodOfSpermPreparation != 0)
                    {
                        cmbMethodOfSpermpreparation.SelectedValue = Details.MethodOfSpermPreparation;
                    }

                }
                fillNeedleSource();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void fillSemenSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceSemenMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    //if (PlannedtreatmentID == Convert.ToInt64(((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID))
                    //{
                    //    objList.RemoveAt(2);
                    //    cmbSrcSemen.ItemsSource = objList;
                    //    cmbSrcSemen.SelectedItem = objList[0];
                    //}
                    //else
                    //{
                    //    cmbSrcSemen.ItemsSource = null;
                    //    cmbSrcSemen.ItemsSource = objList;
                    //    cmbSrcSemen.SelectedItem = objList[0];
                    //}

                    cmbSrcSemen.ItemsSource = null;
                    cmbSrcSemen.ItemsSource = objList;
                    cmbSrcSemen.SelectedItem = objList[0];

                    if (SourceOfSperm > 0)
                    {
                        cmbSrcSemen.SelectedValue = SourceOfSperm;
                        cmbSrcSemen.IsEnabled = false;
                    }
                    else
                    {
                        cmbSrcSemen.SelectedItem = objList[0];
                        cmbSrcSemen.IsEnabled = true;
                    }

                }
                fillOPUDetails();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillOocyteSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceOocyteMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    if (PlannedtreatmentID == Convert.ToInt64(((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID) || PlannedtreatmentID == Convert.ToInt64(((IApplicationConfiguration)App.Current).ApplicationConfigurations.EmbryoReceipentID))
                    {
                        objList.RemoveAt(2);
                        cmbSrcOocyte.ItemsSource = objList;
                        cmbSrcOocyte.SelectedItem = objList[0];
                    }
                    else
                    {
                        cmbSrcOocyte.ItemsSource = null;
                        cmbSrcOocyte.ItemsSource = objList;
                        cmbSrcOocyte.SelectedItem = objList[0];
                    }
                }
                fillSemenSource();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillNeedleSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceNeedleMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbSrcNeedle.ItemsSource = null;
                    cmbSrcNeedle.ItemsSource = objList;
                    cmbSrcNeedle.SelectedItem = objList[0];
                }
                fillOocyteSource();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        string PreviousSampleID = string.Empty;
        public bool IsSampleIDChanged = false;
        public string ThawedSampleID = string.Empty;


        private void Getdetails()
        {
            cls_NewGetDonorDetailsBizActionVO BizActionObj = new cls_NewGetDonorDetailsBizActionVO();
            BizActionObj.DonorDetails.PatientID = PatientID;
            BizActionObj.DonorDetails.PatientUnitID = PatientUnitID;
            BizActionObj.DonorDetails.PlanTherapyID = PlanTherapyID;
            BizActionObj.DonorDetails.PlanTherapyUnitID = PlanTherapyUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails != null)
                    {
                        PatientID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PatientID);
                        PatientUnitID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PatientUnitID);
                        PlanTherapyID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PlanTherapyID);
                        PlanTherapyUnitID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PlanTherapyUnitID);
                        ID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.ID);
                        UnitID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.UnitID);

                        cmdSave.Content = "Modify";

                        MasterListItem obj = new MasterListItem();
                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.MethodOfSpermPreparation != null)
                        {
                            //obj.ID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.MethodOfSpermPreparation);
                            //cmbMethodOfSpermpreparation.SelectedValue = obj.ID;
                            cmbMethodOfSpermpreparation.SelectedValue = Convert.ToInt64(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.MethodOfSpermPreparation);
                        }
                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfNeedle_1 != null)
                        {
                            //obj.ID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfNeedle_1);
                            //cmbSrcNeedle.SelectedValue = obj.ID;
                            cmbSrcNeedle.SelectedValue = Convert.ToInt64(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfNeedle_1);
                        }
                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorID != null)
                        {
                            //obj.ID = Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfOoctye_1);
                            // cmbSrcOocyte.SelectedValue = obj.ID;
                            cmbSrcOocyte.SelectedValue = Convert.ToInt64(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfOoctye_1);
                        }
                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfSemen_new != null)
                        {
                            //obj.ID= Convert.ToInt32(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfSemen_new);
                            //cmbSrcSemen.SelectedValue = obj.ID;
                            cmbSrcSemen.SelectedValue = Convert.ToInt64(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SourceOfSemen_new);
                        }

                        txtOocyteDonorCode.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorMrNo);
                        txtSemenDonorCode.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenDonorMrNo);
                        // Added by CDS
                        if (!string.IsNullOrEmpty(txtSemenDonorCode.Text) || ((MasterListItem)cmbSrcSemen.SelectedItem).ID > 0)
                        {
                            cmdSpermiogram.Visibility = Visibility.Visible;
                            cmdSpermiogram.IsEnabled = true;
                            txtSemenDonorCode.IsReadOnly = true;
                        }

                        if (!string.IsNullOrEmpty(txtOocyteDonorCode.Text) || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID > 0)
                        {
                            cmdAspiration.Visibility = Visibility.Visible;
                            cmdAspiration.IsEnabled = true;
                            txtOocyteDonorCode.IsReadOnly = true;
                        }

                        ooctyDonorID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorID;
                        ooctyDonorUnitID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorUnitID;
                        SemenDonorID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenDonorID;
                        SemenDonorUnitID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenDonorUnitID;

                        txtComany.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.Company);
                        txtpostConcentrationdonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostDonorConcentration_1);
                        txtpostConcentrationself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostSelfConcentration_1);
                        txtpostMotalitydonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostDonorMotality_1);
                        txtpostMotalityself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostSelfMotality_1);
                        txtpostVolumedonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostDonorVolume_1);
                        txtpostVolumeself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostSelfVolume_1);
                        txtpostWBCdonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostDonorWBC_1);
                        txtpostWBCself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PostSelfWBC_1);
                        txtpreMotilitydonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreDonorMotality_1);
                        txtpreMotilityself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreSelfMotality_1);
                        txtpreVolumedonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreDonorVolume_1);
                        txtpreVolumeself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreSelfVolume_1);
                        txtpreVoncentrdonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreDonorConcentration_1);
                        txtpreVoncentrself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreSelfConcentration_1);
                        txtpreWBCdonor.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreDonorWBC_1);
                        txtpreWBCself.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.PreSelfWBC_1);
                        IsEdit = true;

                        txtSemenSampleCode.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenSampleCode);
                        txtSemenSampleCodeSelf.Text = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenSampleCodeSelf);

                        PreviousSampleID = Convert.ToString(((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.SemenSampleCode);
                        if (PreviousSampleID.Equals(ThawedSampleID))
                            IsSampleIDChanged = false;
                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.IsDonorFromModuleDonor)
                            btnSearchSemenDonor.IsEnabled = false;
                        else
                            btnSearchSemenDonor.IsEnabled = true;

                        if (((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.IsSampleUsedInDay0)
                        {
                            cmdSpermiogram.IsEnabled = false;
                            cmdSave.IsEnabled = false;
                        }

                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsSampleIDChanged)
            {
                MessageBoxControl.MessageBoxChildWindow msgWIUI =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save Semen Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                msgWIUI.Show();
            }
            else
            {
                this.DialogResult = false;
                if (Save_ChildClick != null)
                    Save_ChildClick(this, new RoutedEventArgs());
            }
        }

        void msgWIUI_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
                txtSemenSampleCode.Text = ThawedSampleID;
        }

        public bool validition()
        {
            bool result = true;
            if (((MasterListItem)cmbMethodOfSpermpreparation.SelectedItem).ID == 0)
            {
                cmbMethodOfSpermpreparation.TextBox.SetValidation("Please Select Method Of Sperm Preparation");
                cmbMethodOfSpermpreparation.TextBox.RaiseValidationError();
                cmbMethodOfSpermpreparation.Focus();
                result = false;
            }
            //else if (((MasterListItem)cmbSrcNeedle.SelectedItem).ID == 0)
            //{
            //     cmbSrcNeedle.TextBox.SetValidation("Please Select Source Of Needle.");
            //    cmbSrcNeedle.TextBox.RaiseValidationError();
            //    cmbSrcNeedle.Focus();
            //    result = false;
            //}          
            else
            {
                cmbMethodOfSpermpreparation.ClearValidationError();
                // cmbSrcSemen.ClearValidationError();
                //cmbSrcNeedle.ClearValidationError();
                result = true;
            }
            return result;

        }

        WaitIndicator wait = new WaitIndicator();
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (validition())
            {
                try
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n You Want To Save Semen Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgWin.Show();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public event RoutedEventHandler Save_ChildClick;
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {

            if (result == MessageBoxResult.Yes)
            {
                wait.Show();
                try
                {
                    cls_NewAddUpdateDonorDetailsBizActionVO BizActionObj = new cls_NewAddUpdateDonorDetailsBizActionVO();
                    BizActionObj.DonorDetails = new clsFemaleSemenDetailsVO();
                    BizActionObj.IsEdit = IsEdit;
                    if (BizActionObj.IsEdit == true)
                    {
                        BizActionObj.DonorDetails.ID = ID;
                    }
                    else { BizActionObj.DonorDetails.ID = 0; }

                    BizActionObj.DonorDetails.SourceOfSemen = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
                    BizActionObj.DonorDetails.PatientID = PatientID;
                    BizActionObj.DonorDetails.PatientUnitID = PatientUnitID;
                    BizActionObj.DonorDetails.PlanTherapyID = PlanTherapyID;
                    BizActionObj.DonorDetails.PlanTherapyUnitID = PlanTherapyUnitID;

                    BizActionObj.DonorDetails.MethodOfSpermPreparation = ((MasterListItem)cmbMethodOfSpermpreparation.SelectedItem).ID;
                    BizActionObj.DonorDetails.SourceOfNeedle_1 = ((MasterListItem)cmbSrcNeedle.SelectedItem).ID;
                    BizActionObj.DonorDetails.SourceOfOoctye_1 = ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;
                    BizActionObj.DonorDetails.SourceOfSemen_new = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;

                    BizActionObj.DonorDetails.SemenDonorID = SemenDonorID;
                    BizActionObj.DonorDetails.SemenDonorUnitID = SemenDonorUnitID;
                    BizActionObj.DonorDetails.SemenDonorMrNo = Convert.ToString(txtSemenDonorCode.Text);
                    BizActionObj.DonorDetails.OoctyDonorID = ooctyDonorID;
                    BizActionObj.DonorDetails.OoctyDonorUnitID = ooctyDonorUnitID;
                    BizActionObj.DonorDetails.OoctyDonorMrNo = Convert.ToString(txtOocyteDonorCode.Text);
                    if (txtpostVolumedonor.Text != null && txtpostVolumedonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PostDonorVolume_1 = Convert.ToInt64(txtpostVolumedonor.Text);
                    if (txtpostConcentrationdonor.Text != null && txtpostConcentrationdonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PostDonorConcentration_1 = Convert.ToInt64(txtpostConcentrationdonor.Text);

                    if (txtpostMotalitydonor.Text != null && txtpostMotalitydonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PostDonorMotality_1 = Convert.ToInt64(txtpostMotalitydonor.Text);
                    if (txtpostWBCdonor.Text != null && txtpostWBCdonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PostDonorWBC_1 = Convert.ToInt64(txtpostWBCdonor.Text);
                    if (txtpostConcentrationself.Text != null && txtpostConcentrationself.Text != string.Empty)
                        BizActionObj.DonorDetails.PostSelfConcentration_1 = Convert.ToInt64(txtpostConcentrationself.Text);
                    if (txtpostMotalityself.Text != null && txtpostMotalityself.Text != string.Empty)
                        BizActionObj.DonorDetails.PostSelfMotality_1 = Convert.ToInt64(txtpostMotalityself.Text);
                    if (txtpostMotalityself.Text != null && txtpostMotalityself.Text != string.Empty)
                        BizActionObj.DonorDetails.PostSelfVolume_1 = Convert.ToInt64(txtpostVolumeself.Text);
                    if (txtpostWBCself.Text != null && txtpostWBCself.Text != string.Empty)
                        BizActionObj.DonorDetails.PostSelfWBC_1 = Convert.ToInt64(txtpostWBCself.Text);

                    if (txtpreVolumedonor.Text != null && txtpreVolumedonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PreDonorVolume_1 = Convert.ToInt64(txtpreVolumedonor.Text);
                    if (txtpreVoncentrdonor.Text != null && txtpreVoncentrdonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PreDonorConcentration_1 = Convert.ToInt64(txtpreVoncentrdonor.Text);
                    if (txtpreMotilitydonor.Text != null && txtpreMotilitydonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PreDonorMotality_1 = Convert.ToInt64(txtpreMotilitydonor.Text);
                    if (txtpreWBCdonor.Text != null && txtpreWBCdonor.Text != string.Empty)
                        BizActionObj.DonorDetails.PreDonorWBC_1 = Convert.ToInt64(txtpreWBCdonor.Text);
                    if (txtpreVoncentrself.Text != null && txtpreVoncentrself.Text != string.Empty)
                        BizActionObj.DonorDetails.PreSelfConcentration_1 = Convert.ToInt64(txtpreVoncentrself.Text);
                    if (txtpreMotilityself.Text != null && txtpreMotilityself.Text != string.Empty)
                        BizActionObj.DonorDetails.PreSelfMotality_1 = Convert.ToInt64(txtpreMotilityself.Text);
                    if (txtpreVolumeself.Text != null && txtpreVolumeself.Text != string.Empty)
                        BizActionObj.DonorDetails.PreSelfVolume_1 = Convert.ToInt64(txtpreVolumeself.Text);
                    if (txtpreWBCself.Text != null && txtpreWBCself.Text != string.Empty)
                        BizActionObj.DonorDetails.PreSelfWBC_1 = Convert.ToInt64(txtpreWBCself.Text);

                    if (txtComany.Text != null && txtComany.Text != string.Empty)
                        BizActionObj.DonorDetails.Company = Convert.ToString(txtComany.Text);

                    BizActionObj.DonorDetails.SemenSampleCode = Convert.ToString(txtSemenSampleCode.Text);
                    BizActionObj.DonorDetails.SemenSampleCodeSelf = Convert.ToString(txtSemenSampleCodeSelf.Text);
                    BizActionObj.DonorDetails.IsDonorFromModuleDonor = IsDonor;

                    //CIMS_IVFDAshBoard_AddDonorDetails............ Sp Name

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Semen Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    if (((MasterListItem)cmbSrcSemen.SelectedItem).ID > 0 || !string.IsNullOrEmpty(txtSemenDonorCode.Text))
                                    {
                                        Getdetails();
                                        cmdSpermiogram.Visibility = Visibility.Visible;
                                        cmdSpermiogram.IsEnabled = true;
                                        fillGrid();
                                    }
                                    if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID > 0 || !string.IsNullOrEmpty(txtOocyteDonorCode.Text))
                                    {
                                        cmdAspiration.Visibility = Visibility.Visible;
                                        cmdAspiration.IsEnabled = true;
                                        fillGridAspiration();
                                    }
                                    //this.DialogResult = true;
                                    //if (Save_ChildClick != null)
                                    //    Save_ChildClick(this, new RoutedEventArgs());
                                }
                            };
                            msgW1.Show();

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                    wait.Close();
                }
                catch (Exception ex)
                {
                    wait.Show();
                    throw ex;
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
                {
                    if (textBefore != null)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void TexBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 3)
            {
                PatientSearch Win = new PatientSearch("SemenDetail");
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.PatientCategoryID = 8;
                Win.IsSelfAndDonor = 1;
                Win.Show();
            }
            else
            {
                PatientSearch Win = new PatientSearch("SemenDetail");
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.PatientCategoryID = 8;

                Win.Show();
            }
        }

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
            {
                PatientSearch Win = new PatientSearch("SemenDetail");
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.ShowOutSourceDonor = true;
                Win.PatientCategoryID = 9;
                Win.IsSelfAndDonor = 2;
                Win.Show();
            }
            else
            {
                PatientSearch Win = new PatientSearch("SemenDetail");
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.ShowOutSourceDonor = true;
                Win.PatientCategoryID = 9;

                Win.Show();
            }
        }

        private void fillGrid()
        {
            try
            {
                // bool fill=false;
                cls_GetSemenBatchAndSpermiogramBizActionVO BizAction = new cls_GetSemenBatchAndSpermiogramBizActionVO();
                BizAction.DetailsList = new List<ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO>();
                BizAction.Details = new ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO();
                BizAction.Details.PlanTherapyID = PlanTherapyID;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //if (((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList != null && ((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList.Count > 0)
                        //{
                        //    cmdSpermiogram.Visibility = Visibility.Visible;
                        //    cmdSpermiogram.IsEnabled = true;
                        //    cmbSrcSemen.IsEnabled = false;
                        //    btnSearchSemenDonor.IsEnabled = false;
                        //}



                        //else
                        //{
                        //    cmdSpermiogram.Visibility = Visibility.Visible;
                        //    cmdSpermiogram.IsEnabled = true;
                        //}                    
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        long ooctyDonorID;
        long ooctyDonorUnitID;
        long SemenDonorID;
        long SemenDonorUnitID;
        bool IsDonor;
        string DonorCode;

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 8)
                {
                    txtOocyteDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                    ooctyDonorID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).PatientID;
                    ooctyDonorUnitID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).UnitId;
                }
                else
                {
                    txtSemenDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                    SemenDonorID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).PatientID;
                    SemenDonorUnitID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).UnitId;
                    IsDonor = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).IsDonor;
                    DonorCode = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).DonorCode;
             
                }
            }
        }

        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long selectedID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
            controlvisobility(selectedID);

            if (cmbSrcSemen.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    cmdSpermiogram.IsEnabled = false;
                    txtSemenSampleCode.Text = "";
                    txtSemenSampleCode.IsEnabled = false;
                    cmdSpermiogramSelf.IsEnabled = false;
                    txtSemenSampleCodeSelf.Text = "";
                    txtSemenSampleCodeSelf.IsEnabled = false;

                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    cmdSpermiogram.IsEnabled = false;
                    txtSemenSampleCode.Text = "";
                    txtSemenSampleCode.IsEnabled = false;
                    cmdSpermiogramSelf.IsEnabled = true;
                    txtSemenSampleCodeSelf.Text = "";
                    txtSemenSampleCodeSelf.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 2)
                {
                    btnSearchSemenDonor.IsEnabled = true;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    cmdSpermiogram.IsEnabled = true;
                    txtSemenSampleCode.Text = "";
                    txtSemenSampleCode.IsEnabled = false;
                    cmdSpermiogramSelf.IsEnabled = false;
                    txtSemenSampleCodeSelf.Text = "";
                    txtSemenSampleCodeSelf.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
                {
                    btnSearchSemenDonor.IsEnabled = true;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    cmdSpermiogram.IsEnabled = true;
                    txtSemenSampleCode.Text = "";
                    txtSemenSampleCode.IsEnabled = false;
                    cmdSpermiogramSelf.IsEnabled = true;
                    txtSemenSampleCodeSelf.Text = "";
                    txtSemenSampleCodeSelf.IsEnabled = false;
                }
            }
            fillGrid();
        }

        private void cmbSrcOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcOocyte.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 0)
                {
                    btnSearchOocyteDonor.IsEnabled = false;

                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 1)
                {
                    btnSearchOocyteDonor.IsEnabled = false;
                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;

                }
                else
                {
                    btnSearchOocyteDonor.IsEnabled = true;
                    txtOocyteDonorCode.IsEnabled = true;
                }
            }
            fillGridAspiration();
        }

        private void fillGridAspiration()
        {
            try
            {
                clsGetDetailsOfReceivedOocyteBizActionVO BizActionObj = new clsGetDetailsOfReceivedOocyteBizActionVO();
                BizActionObj.Details.PatientID = PatientID;
                BizActionObj.Details.PatientUnitID = PatientUnitID;
                BizActionObj.Details.TherapyID = PlanTherapyID;
                BizActionObj.Details.TherapyUnitID = PlanTherapyUnitID;
                BizActionObj.Details.IsReceiveOocyte = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result) != null)
                        {
                            if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details != null && ((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.ID > 0)
                            {
                                Boolean bValue = Convert.ToBoolean(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.IsFreezed);
                                if (bValue == true)
                                {
                                    cmdAspiration.Visibility = Visibility.Visible;
                                    cmdAspiration.IsEnabled = true;
                                    //cmdSave.IsEnabled = false;

                                    cmbSrcOocyte.IsEnabled = false;
                                    btnSearchOocyteDonor.IsEnabled = false;
                                }
                                else
                                {
                                    cmdAspiration.Visibility = Visibility.Visible;
                                    cmdAspiration.IsEnabled = true;
                                    //cmdSave.IsEnabled = false;

                                    cmbSrcOocyte.IsEnabled = true;
                                    btnSearchOocyteDonor.IsEnabled = true;
                                }
                            }
                        }
                        //else
                        //{
                        //    cmdAspiration.Visibility = Visibility.Visible;
                        //    cmdAspiration.IsEnabled = true;
                        //    //cmdSave.IsEnabled = false;

                        //    cmbSrcOocyte.IsEnabled = true;
                        //    btnSearchOocyteDonor.IsEnabled = true;
                        //}
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void controlvisobility(long selectid)
        {
            switch (selectid)
            {
                case 0:
                    txtpostVolumeself.IsEnabled = false;
                    txtpostConcentrationself.IsEnabled = false;
                    txtpostMotalityself.IsEnabled = false;
                    txtpostWBCself.IsEnabled = false;
                    txtpostVolumedonor.IsEnabled = false;
                    txtpostConcentrationdonor.IsEnabled = false;
                    txtpostMotalitydonor.IsEnabled = false;
                    txtpostWBCdonor.IsEnabled = false;

                    txtpreVolumeself.IsEnabled = false;
                    txtpreVoncentrself.IsEnabled = false;
                    txtpreMotilityself.IsEnabled = false;
                    txtpreWBCself.IsEnabled = false;
                    txtpreVolumedonor.IsEnabled = false;
                    txtpreVoncentrdonor.IsEnabled = false;
                    txtpreMotilitydonor.IsEnabled = false;
                    txtpreWBCdonor.IsEnabled = false;
                    break;


                case 1:
                    txtpostVolumeself.IsEnabled = true;
                    txtpostConcentrationself.IsEnabled = true;
                    txtpostMotalityself.IsEnabled = true;
                    txtpostWBCself.IsEnabled = true;
                    txtpostVolumedonor.IsEnabled = false;
                    txtpostConcentrationdonor.IsEnabled = false;
                    txtpostMotalitydonor.IsEnabled = false;
                    txtpostWBCdonor.IsEnabled = false;

                    txtpreVolumeself.IsEnabled = true;
                    txtpreVoncentrself.IsEnabled = true;
                    txtpreMotilityself.IsEnabled = true;
                    txtpreWBCself.IsEnabled = true;
                    txtpreVolumedonor.IsEnabled = false;
                    txtpreVoncentrdonor.IsEnabled = false;
                    txtpreMotilitydonor.IsEnabled = false;
                    txtpreWBCdonor.IsEnabled = false;
                    break;

                case 2:
                    txtpostVolumeself.IsEnabled = false;
                    txtpostConcentrationself.IsEnabled = false;
                    txtpostMotalityself.IsEnabled = false;
                    txtpostWBCself.IsEnabled = false;
                    txtpostVolumedonor.IsEnabled = true;
                    txtpostConcentrationdonor.IsEnabled = true;
                    txtpostMotalitydonor.IsEnabled = true;
                    txtpostWBCdonor.IsEnabled = true;

                    txtpreVolumeself.IsEnabled = false;
                    txtpreVoncentrself.IsEnabled = false;
                    txtpreMotilityself.IsEnabled = false;
                    txtpreWBCself.IsEnabled = false;
                    txtpreVolumedonor.IsEnabled = true;
                    txtpreVoncentrdonor.IsEnabled = true;
                    txtpreMotilitydonor.IsEnabled = true;
                    txtpreWBCdonor.IsEnabled = true;
                    break;

                case 3:
                    txtpostVolumeself.IsEnabled = true;
                    txtpostConcentrationself.IsEnabled = true;
                    txtpostMotalityself.IsEnabled = true;
                    txtpostWBCself.IsEnabled = true;
                    txtpostVolumedonor.IsEnabled = true;
                    txtpostConcentrationdonor.IsEnabled = true;
                    txtpostMotalitydonor.IsEnabled = true;
                    txtpostWBCdonor.IsEnabled = true;

                    txtpreVolumeself.IsEnabled = true;
                    txtpreVoncentrself.IsEnabled = true;
                    txtpreMotilityself.IsEnabled = true;
                    txtpreWBCself.IsEnabled = true;
                    txtpreVolumedonor.IsEnabled = true;
                    txtpreVoncentrdonor.IsEnabled = true;
                    txtpreMotilitydonor.IsEnabled = true;
                    txtpreWBCdonor.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }

        private void fillOPUDetails()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = new clsIVFDashboard_GetOPUDetailsBizActionVO();
                BizAction.Details = new clsIVFDashboard_OPUVO();
                BizAction.Details.PlanTherapyID = PlanTherapyID;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
                BizAction.Details.PatientID = PatientID;
                BizAction.Details.PatientUnitID = PatientUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            {
                                cmdSave.IsEnabled = false;
                            }

                        }

                    }
                    if (IsClosed == true)
                        cmdSave.IsEnabled = false;
                    wait.Close();
                    Getdetails();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        void Spermiogram_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            fillGrid();
            txtSemenDonorCode.IsReadOnly = true;
        }


        private void cmdSpermiogram_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSrcSemen.SelectedItem != null && ((MasterListItem)cmbSrcSemen.SelectedItem).ID > 0)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 2 || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
                {
                    if (txtSemenDonorCode.Text != string.Empty)
                    {
                        if (IsDonor)
                        {
                            SpermThawingForPartnerIUI1 winPartner = new SpermThawingForPartnerIUI1();
                            winPartner.IsClosed = IsClosed;
                            winPartner.DonorCode = DonorCode;
                            winPartner.IsFromSemenDetails = true;
                            //winPartner.CoupleDetails = coupleDetails;
                            winPartner.DonorID = SemenDonorID;
                            winPartner.DonorUnitID = SemenDonorUnitID;
                            winPartner.PlanTherapyID = PlanTherapyID;
                            winPartner.PlanTherapyUnitID = PlanTherapyUnitID;
                            winPartner.OKButtonCode_Click += new RoutedEventHandler(winPartner_OKButtonCode_Click);
                            winPartner.Show();
                        }
                        else
                        {
                            // SemenWash_Dashboard_Details obj = new SemenWash_Dashboard_Details();
                            FrmSemenPreparation_Details obj = new FrmSemenPreparation_Details();
                            obj.DonorID = SemenDonorID;
                            obj.DonorUnitID = SemenDonorUnitID;
                            obj.OKButtonCode_Click += new RoutedEventHandler(objOk_Closed);
                            obj.Closed += new EventHandler(obj_Closed);
                            obj.PlanTherapyID = PlanTherapyID;
                            obj.PlanTherapyUnitID = PlanTherapyUnitID;
                            //obj.CoupleDetails = CoupleDetails;
                            obj.Show();


                            //frmSpermiogram win = new frmSpermiogram();
                            //win.DonorID = SemenDonorID;
                            //win.DonorUnitID = SemenDonorUnitID;
                            //win.SourceOfSemenCode = txtSemenDonorCode.Text;
                            //win.PlanTherapyID = PlanTherapyID;
                            //win.PlanTherapyUnitID = PlanTherapyUnitID;
                            //win.OnCloseButton_Click += new RoutedEventHandler(Spermiogram_OnCloseButton_Click);
                            //win.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please Select Source of Semen.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }


            //if (cmbSrcSemen.SelectedItem != null && ((MasterListItem)cmbSrcSemen.SelectedItem).ID > 0)
            //{

            //    if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1 || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
            //    {
            //        frmSpermiogram win = new frmSpermiogram();
            //        win.DonorID = coupleDetails.MalePatient.PatientID;
            //        win.DonorUnitID = coupleDetails.MalePatient.UnitId;
            //        win.SourceOfSemenCode = coupleDetails.MalePatient.MRNo;
            //        win.PlanTherapyID = PlanTherapyID;
            //        win.PlanTherapyUnitID = PlanTherapyUnitID;
            //        win.OnCloseButton_Click += new RoutedEventHandler(Spermiogram_OnCloseButton_Click);
            //        win.Show();
            //    }
            //    else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 2)
            //    {
            //        if (txtSemenDonorCode.Text != string.Empty)
            //        {
            //            frmSpermiogram win = new frmSpermiogram();
            //            win.DonorID = SemenDonorID;
            //            win.DonorUnitID = SemenDonorUnitID;
            //            win.SourceOfSemenCode = txtSemenDonorCode.Text;
            //            win.PlanTherapyID = PlanTherapyID;
            //            win.PlanTherapyUnitID = PlanTherapyUnitID;
            //            win.OnCloseButton_Click += new RoutedEventHandler(Spermiogram_OnCloseButton_Click);
            //            win.Show();
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    }

            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Source of Semen.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        void winPartner_OKButtonCode_Click(object sender, RoutedEventArgs e)
        {
            fillGrid();
            if (((SpermThawingForPartnerIUI1)sender).SampleID != null)
            {
                txtSemenSampleCode.Text = ((SpermThawingForPartnerIUI1)sender).SampleID.ToString();
                ThawedSampleID = ((SpermThawingForPartnerIUI1)sender).SampleID.ToString();
                if (!PreviousSampleID.Equals(ThawedSampleID))
                    IsSampleIDChanged = true;
            }
        }

        void objOk_Closed(object sender, EventArgs e)
        {
            fillGrid();
            if (((FrmSemenPreparation_Details)sender).SampleID != null)
            {
                txtSemenSampleCode.Text = ((FrmSemenPreparation_Details)sender).SampleID.ToString();
            }

        }

        void obj_Closed(object sender, EventArgs e)
        {
            fillGrid();
        }

        //void objSelf_Closed(object sender, EventArgs e)
        //{
        //    fillGrid();
        //    if (((FrmSemenPreparation_Details)sender).SampleID != null)
        //    {
        //        txtSemenSampleCodeSelf.Text = ((FrmSemenPreparation_Details)sender).SampleID.ToString();

        //    }

        //}


        private void cmdAspiration_Click(object sender, RoutedEventArgs e)
        {
            if (PlannedtreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID)
            {
                if (cmbSrcOocyte.SelectedItem != null && ((MasterListItem)cmbSrcOocyte.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 1 || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 3)
                    {
                        frmReceiveOocyte win = new frmReceiveOocyte();
                        win.DonorID = coupleDetails.FemalePatient.PatientID;
                        win.DonorUnitID = coupleDetails.FemalePatient.UnitId;
                        win.SourceOfOocytesCode = coupleDetails.FemalePatient.MRNo;
                        win.PlanTherapyID = PlanTherapyID;
                        win.PlanTherapyUnitID = PlanTherapyUnitID;
                        win.PatientID = coupleDetails.FemalePatient.PatientID;
                        win.PatientUnitID = coupleDetails.FemalePatient.UnitId;
                        win.OnCloseButton_Click += new RoutedEventHandler(Aspiration_OnCloseButton_Click);
                        win.Show();
                    }
                    else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 2)
                    {
                        if (txtOocyteDonorCode.Text != string.Empty)
                        {
                            frmReceiveOocyte win = new frmReceiveOocyte();
                            win.DonorID = ooctyDonorID;
                            win.DonorUnitID = ooctyDonorUnitID;
                            win.SourceOfOocytesCode = txtOocyteDonorCode.Text;
                            win.PlanTherapyID = PlanTherapyID;
                            win.PlanTherapyUnitID = PlanTherapyUnitID;
                            win.PatientID = coupleDetails.FemalePatient.PatientID;
                            win.PatientUnitID = coupleDetails.FemalePatient.UnitId;
                            win.OnCloseButton_Click += new RoutedEventHandler(Aspiration_OnCloseButton_Click);
                            win.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Source of Oocyte.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else if (PlannedtreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.EmbryoReceipentID)
            {
                if (cmbSrcOocyte.SelectedItem != null && ((MasterListItem)cmbSrcOocyte.SelectedItem).ID > 0)
                {

                    if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 1 || ((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 3)
                    {
                        frmReceiveEmbryo win = new frmReceiveEmbryo();
                        win.DonorID = coupleDetails.FemalePatient.PatientID;
                        win.DonorUnitID = coupleDetails.FemalePatient.UnitId;
                        win.PatientID = coupleDetails.FemalePatient.PatientID;
                        win.PatientUnitID = coupleDetails.FemalePatient.UnitId;
                        win.SourceOfOocyteCode = coupleDetails.FemalePatient.MRNo;
                        win.PlanTherapyID = PlanTherapyID;
                        win.PlanTherapyUnitID = PlanTherapyUnitID;
                        win.OnCloseButton_Click += new RoutedEventHandler(Aspiration_OnCloseButton_Click);
                        win.Show();
                    }
                    else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 2)
                    {
                        if (txtOocyteDonorCode.Text != string.Empty)
                        {
                            frmReceiveEmbryo win = new frmReceiveEmbryo();
                            win.DonorID = ooctyDonorID;
                            win.DonorUnitID = ooctyDonorUnitID;
                            win.PatientID = coupleDetails.FemalePatient.PatientID;
                            win.PatientUnitID = coupleDetails.FemalePatient.UnitId;
                            win.SourceOfOocyteCode = txtOocyteDonorCode.Text;
                            win.PlanTherapyID = PlanTherapyID;
                            win.PlanTherapyUnitID = PlanTherapyUnitID;
                            win.OnCloseButton_Click += new RoutedEventHandler(Aspiration_OnCloseButton_Click);
                            win.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Source of Oocyte.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }

        }

        void Aspiration_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            fillGridAspiration();
            txtOocyteDonorCode.IsReadOnly = true;
        }


        private void cmdSpermiogramSelf_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSrcSemen.SelectedItem != null && ((MasterListItem)cmbSrcSemen.SelectedItem).ID > 0)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1 || ((MasterListItem)cmbSrcSemen.SelectedItem).ID == 3)
                {
                    //if (txtSemenDonorCode.Text != string.Empty)
                    //{
                    FrmSemenPreparation_Details obj = new FrmSemenPreparation_Details();
                    obj.IsSelf = true;
                    //SemenWash_Dashboard_Details obj = new SemenWash_Dashboard_Details();
                    obj.DonorID = coupleDetails.MalePatient.PatientID;
                    obj.DonorUnitID = coupleDetails.MalePatient.UnitId;
                    obj.OKButtonCode_Click += new RoutedEventHandler(objSelf_Closed);
                    obj.Closed += new EventHandler(objSelf_Closed);
                    obj.PlanTherapyID = PlanTherapyID;
                    obj.PlanTherapyUnitID = PlanTherapyUnitID;
                    //obj.CoupleDetails = CoupleDetails;
                    obj.Show();

                    //frmSpermiogram win = new frmSpermiogram();
                    //win.DonorID = SemenDonorID;
                    //win.DonorUnitID = SemenDonorUnitID;
                    //win.SourceOfSemenCode = txtSemenDonorCode.Text;
                    //win.PlanTherapyID = PlanTherapyID;
                    //win.PlanTherapyUnitID = PlanTherapyUnitID;
                    //win.OnCloseButton_Click += new RoutedEventHandler(Spermiogram_OnCloseButton_Click);
                    //win.Show();
                    //}
                    //else
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //            new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //}
                }

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please Select Source of Semen.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }        

        void objSelf_Closed(object sender, EventArgs e)
        {
            fillGrid();
            if (((FrmSemenPreparation_Details)sender).SampleSelfID != null)
            {
                txtSemenSampleCodeSelf.Text = ((FrmSemenPreparation_Details)sender).SampleSelfID.ToString();

            }

        }
    }
}

