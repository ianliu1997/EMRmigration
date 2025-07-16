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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmIUI : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        #region FillCombox
        private void FillInseminatedBy()
        {
            clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                    cmbInseminatedBy.ItemsSource = null;
                    cmbInseminatedBy.ItemsSource = objList;
                    cmbInseminatedBy.SelectedItem = objList[0];

                    cmbWitnessedBy.ItemsSource = null;
                    cmbWitnessedBy.ItemsSource = objList;
                    cmbWitnessedBy.SelectedItem = objList[0];
                    fillInseminationLocationMaster();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
       private void fillInseminationLocationMaster()
        {
           
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_InseminationLocationMaster;
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

                    cmbnseminationLocation.ItemsSource = null;
                    cmbnseminationLocation.ItemsSource = objList;
                    cmbnseminationLocation.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                       
                    }
                    FillCollectionMethod();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
       private void FillCollectionMethod() 
       {
           
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedSpermCollection;
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

                    cmbHomoCollectionMethod.ItemsSource = null;
                    cmbHomoCollectionMethod.ItemsSource = objList;
                    cmbHomoCollectionMethod.SelectedItem = objList[0];

                    cmbHetroCollectionMethod.ItemsSource = null;
                    cmbHetroCollectionMethod.ItemsSource = objList;
                    cmbHetroCollectionMethod.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                       
                    }
                    fillIUIDetails();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
       }
        #endregion
       private void fillIUIDetails() 
       {
           clsIVFDashboard_GetIUIDetailsBizActionVO BizAction = new clsIVFDashboard_GetIUIDetailsBizActionVO();
           BizAction.Details = new clsIVFDashboard_IUIVO();
           BizAction.Details.PlanTherapyID = PlanTherapyID;
           BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
           if (CoupleDetails != null)
           {
               BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
               BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
           }
           Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
           PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

           client.ProcessCompleted += (s, arg) =>
           {
               if (arg.Error == null && arg.Result != null)
               {
                   if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details != null)
                   {
                        this.DataContext = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details;
                        if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.ID != 0)
                        {
                            if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminatedByID != null)
                            {
                                cmbInseminatedBy.SelectedValue = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminatedByID;
                            }
                            if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.WitnessedByID != null)
                            {
                                cmbWitnessedBy.SelectedValue = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminatedByID;
                            }
                            if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminationLocationID != null)
                            {
                                cmbnseminationLocation.SelectedValue = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminationLocationID;
                            }
                            if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.IsHomologous == true)
                            {
                                rdoHomologous.IsChecked = true;
                                Homologous.IsEnabled = true;
                                Hetrologous.IsEnabled = false;
                                if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionMethodID != null)
                                {
                                    cmbHomoCollectionMethod.SelectedValue = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionMethodID;
                                }

                                HomoCollectionDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionDate;
                                HomoPreperationDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.PreperationDate;
                                HomoThawingDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.ThawingDate;
                                if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.SampleID != null)
                                    txtHomoSampleID.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.SampleID;
                                txtHomoPurpose.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Purpose;
                                txtHomoDiagnosis.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Diagnosis;

                                txtHomoInseminatedAmounts.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminatedAmounts);
                                txtHomoNumberofMotileSperm.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NumberofMotileSperm);
                                txtHomoNativeAmount.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeAmount);
                                txtHomoAfterPrepAmount.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepAmount);
                                txtHomoNativeConcentration.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeConcentration);
                                txtHomoAfterPrepConcentration.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepConcentration);
                                txtHomoNativeProgressiveMotatity.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeProgressiveMotatity);
                                txtHomoAfterPrepProgressiveMotatity.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepProgressiveMotatity);
                                txtHomoNativeOverallMotality.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeOverallMotality);
                                txtHomoAfterPrepOverallMotality.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepOverallMotality);
                                txtHomoNativeNormalForms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeNormalForms);
                                txtHomoAfterPrepNormalForms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepNormalForms);
                                txtHomoNativeTotalNoOfSperms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeTotalNoOfSperms);
                                txtHomoAfterPrepTotalNoOfSperms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepTotalNoOfSperms);
                                txtHomoNotes.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Notes;

                                clearHetroTab();
                            }
                            else
                            {
                                rdoHetrologous.IsChecked = true;
                                Hetrologous.IsEnabled = false;
                                Homologous.IsEnabled = false;
                                if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionMethodID != null)
                                {
                                    cmbHetroCollectionMethod.SelectedValue = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionMethodID;

                                }

                                HetroCollectionDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.CollectionDate;
                                HetroPreperationDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.PreperationDate;
                                HetroThawingDate.SelectedDate = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.ThawingDate;
                                if (((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.SampleID != null)
                                    txtHetroSampleID.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.SampleID;
                                txtHetroPurpose.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Purpose;
                                txtHetroDiagnosis.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Diagnosis;

                                txtHetroInseminatedAmounts.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.InseminatedAmounts);
                                txtHetroNumberofMotileSperm.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NumberofMotileSperm);
                                txtHetroNativeAmount.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeAmount);
                                txtHetroAfterPrepAmount.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepAmount);
                                txtHetroNativeConcentration.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeConcentration);
                                txtHetroAfterPrepConcentration.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepConcentration);
                                txtHetroNativeProgressiveMotatity.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeProgressiveMotatity);
                                txtHetroAfterPrepProgressiveMotatity.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepProgressiveMotatity);
                                txtHetroNativeOverallMotality.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeOverallMotality);
                                txtHetroAfterPrepOverallMotality.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepOverallMotality);
                                txtHetroNativeNormalForms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeNormalForms);
                                txtHetroAfterPrepNormalForms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepNormalForms);
                                txtHetroNativeTotalNoOfSperms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.NativeTotalNoOfSperms);
                                txtHetroAfterPrepTotalNoOfSperms.Text = Convert.ToString(((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.AfterPrepTotalNoOfSperms);
                                txtHetroNotes.Text = ((clsIVFDashboard_GetIUIDetailsBizActionVO)arg.Result).Details.Notes;


                                clearHomoTab();
                            }
                        }
                      
                   }
               }
           };
           client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
           client.CloseAsync();

       }
        public FrmIUI()
        {
            InitializeComponent();
            //this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            this.DataContext = new clsIVFDashboard_IUIVO();
            
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //rdoHomologous.IsChecked = true;
            //Homologous.IsSelected = true;
            //Hetrologous.IsEnabled = false;
            if (IsClosed == true)
                cmdNew.IsEnabled = false;
            rdoHetrologous.IsChecked = true;
            rdoHomologous.IsChecked = false;
            Hetrologous.IsSelected = true;
            Homologous.IsEnabled = false;
            Hetrologous.IsEnabled = true;
            FillInseminatedBy();
            
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save IUI details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }
         void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

         private void Save()
         {
             clsIVFDashboard_AddUpdateIUIDetailsBizActionVO BizAction = new clsIVFDashboard_AddUpdateIUIDetailsBizActionVO();
             BizAction.IUIDetails = new clsIVFDashboard_IUIVO();
             BizAction.IUIDetails.ID = ((clsIVFDashboard_IUIVO)this.DataContext).ID;
             BizAction.IUIDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
             BizAction.IUIDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
             BizAction.IUIDetails.PlanTherapyID = PlanTherapyID;
             BizAction.IUIDetails.PlanTherapyUnitID = PlanTherapyUnitID;
             if (rdoHetrologous.IsChecked == true)
             {
                 BizAction.IUIDetails.IsHomologous = false;
                 if (HetroCollectionDate.SelectedDate !=null)
                 BizAction.IUIDetails.CollectionDate = HetroCollectionDate.SelectedDate.Value.Date;
                 if (HetroPreperationDate.SelectedDate != null)
                 BizAction.IUIDetails.PreperationDate = HetroPreperationDate.SelectedDate.Value.Date;
                 if (HetroThawingDate.SelectedDate != null)
                 BizAction.IUIDetails.ThawingDate = HetroThawingDate.SelectedDate.Value.Date;
                 BizAction.IUIDetails.SampleID = txtHetroSampleID.Text;
                 BizAction.IUIDetails.Purpose = txtHetroPurpose.Text;
                 BizAction.IUIDetails.Diagnosis = txtHetroDiagnosis.Text;
                 BizAction.IUIDetails.CollectionMethodID = ((MasterListItem)cmbHetroCollectionMethod.SelectedItem).ID;
                 BizAction.IUIDetails.InseminatedAmounts = Convert.ToDouble(txtHetroInseminatedAmounts.Text);
                 BizAction.IUIDetails.NumberofMotileSperm = Convert.ToDouble(txtHetroNumberofMotileSperm.Text);
                 BizAction.IUIDetails.NativeAmount = Convert.ToDouble(txtHetroNativeAmount.Text);
                 BizAction.IUIDetails.AfterPrepAmount = Convert.ToDouble(txtHetroAfterPrepAmount.Text);
                 BizAction.IUIDetails.NativeConcentration = Convert.ToDouble(txtHetroNativeConcentration.Text);
                 BizAction.IUIDetails.AfterPrepConcentration = Convert.ToDouble(txtHetroAfterPrepConcentration.Text);
                 BizAction.IUIDetails.NativeProgressiveMotatity = Convert.ToDouble(txtHetroNativeProgressiveMotatity.Text);
                 BizAction.IUIDetails.AfterPrepProgressiveMotatity = Convert.ToDouble(txtHetroAfterPrepProgressiveMotatity.Text);
                 BizAction.IUIDetails.NativeOverallMotality = Convert.ToDouble(txtHetroNativeOverallMotality.Text);
                 BizAction.IUIDetails.AfterPrepOverallMotality = Convert.ToDouble(txtHetroAfterPrepOverallMotality.Text);
                 BizAction.IUIDetails.NativeNormalForms = Convert.ToDouble(txtHetroNativeNormalForms.Text);
                 BizAction.IUIDetails.AfterPrepNormalForms = Convert.ToDouble(txtHetroAfterPrepNormalForms.Text);
                 BizAction.IUIDetails.NativeTotalNoOfSperms = Convert.ToDouble(txtHetroNativeTotalNoOfSperms.Text);
                 BizAction.IUIDetails.AfterPrepTotalNoOfSperms = Convert.ToDouble(txtHetroAfterPrepTotalNoOfSperms.Text);
                 BizAction.IUIDetails.Notes = txtHetroNotes.Text;
             }
             else
             {
                 BizAction.IUIDetails.IsHomologous = true;
                 if (HomoCollectionDate.SelectedDate != null)
                 BizAction.IUIDetails.CollectionDate = HomoCollectionDate.SelectedDate.Value.Date;
                 if (HomoPreperationDate.SelectedDate != null)
                 BizAction.IUIDetails.PreperationDate = HomoPreperationDate.SelectedDate.Value.Date;
                 if (HomoThawingDate.SelectedDate != null)
                 BizAction.IUIDetails.ThawingDate = HomoThawingDate.SelectedDate.Value.Date;
                 BizAction.IUIDetails.SampleID = txtHomoSampleID.Text;
                 BizAction.IUIDetails.Purpose =txtHomoPurpose.Text;
                 BizAction.IUIDetails.Diagnosis= txtHomoDiagnosis.Text;
                 BizAction.IUIDetails.CollectionMethodID = ((MasterListItem)cmbHomoCollectionMethod.SelectedItem).ID;
                 BizAction.IUIDetails.InseminatedAmounts =Convert.ToDouble(txtHomoInseminatedAmounts.Text);
                 BizAction.IUIDetails.NumberofMotileSperm =Convert.ToDouble( txtHomoNumberofMotileSperm.Text);
                 BizAction.IUIDetails.NativeAmount = Convert.ToDouble(txtHomoNativeAmount.Text);
                 BizAction.IUIDetails.AfterPrepAmount = Convert.ToDouble(txtHomoAfterPrepAmount.Text);
                 BizAction.IUIDetails.NativeConcentration = Convert.ToDouble(txtHomoNativeConcentration.Text);
                 BizAction.IUIDetails.AfterPrepConcentration = Convert.ToDouble(txtHomoAfterPrepConcentration.Text);
                 BizAction.IUIDetails.NativeProgressiveMotatity = Convert.ToDouble(txtHomoNativeProgressiveMotatity.Text);
                 BizAction.IUIDetails.AfterPrepProgressiveMotatity = Convert.ToDouble(txtHomoAfterPrepProgressiveMotatity.Text);
                 BizAction.IUIDetails.NativeOverallMotality = Convert.ToDouble(txtHomoNativeOverallMotality.Text);
                 BizAction.IUIDetails.AfterPrepOverallMotality = Convert.ToDouble(txtHomoAfterPrepOverallMotality.Text);
                 BizAction.IUIDetails.NativeNormalForms = Convert.ToDouble(txtHomoNativeNormalForms.Text);
                 BizAction.IUIDetails.AfterPrepNormalForms= Convert.ToDouble(txtHomoAfterPrepNormalForms.Text);
                 BizAction.IUIDetails.NativeTotalNoOfSperms= Convert.ToDouble(txtHomoNativeTotalNoOfSperms.Text);
                 BizAction.IUIDetails.AfterPrepTotalNoOfSperms = Convert.ToDouble(txtHomoAfterPrepTotalNoOfSperms.Text);
                 BizAction.IUIDetails.Notes= txtHomoNotes.Text;
               
             }

             BizAction.IUIDetails.IUIDate = dtIUIDate.SelectedDate.Value.Date;
             BizAction.IUIDetails.IUITime =Convert.ToDateTime(IUITime.Value);
             BizAction.IUIDetails.InseminatedByID = ((MasterListItem)cmbInseminatedBy.SelectedItem).ID;
             BizAction.IUIDetails.WitnessedByID = ((MasterListItem)cmbWitnessedBy.SelectedItem).ID;
             BizAction.IUIDetails.InseminationLocationID = ((MasterListItem)cmbnseminationLocation.SelectedItem).ID;
             
          
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
             PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
             client.ProcessCompleted += (s, arg) =>
             {
                 if (arg.Error == null && arg.Result != null)
                 {
                     MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "IUI Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                     msgW1.Show();
                 }
                 else
                 {
                     MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                     msgW1.Show();
                 }
             };
             client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
             client.CloseAsync();
         }
        private bool Validate() 
        {
            bool result = true;
            if (dtIUIDate.SelectedDate == null)
            {
                dtIUIDate.SetValidation("Please Select Date");
                dtIUIDate.RaiseValidationError();
                dtIUIDate.Focus();
                return false;
            }
            else
                dtIUIDate.ClearValidationError();

            if (IUITime.Value == null)
            {
                IUITime.SetValidation("Please Select Time");
                IUITime.RaiseValidationError();
                IUITime.Focus();
                return false;
            }
            else
                IUITime.ClearValidationError();

            if (cmbInseminatedBy.SelectedItem == null)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Inseminated by");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbInseminatedBy.SelectedItem).ID == 0)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Embryologist");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else
                cmbInseminatedBy.TextBox.ClearValidationError();


            if (cmbWitnessedBy.SelectedItem == null)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbWitnessedBy.SelectedItem).ID == 0)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else
                cmbWitnessedBy.TextBox.ClearValidationError();


            if (cmbnseminationLocation.SelectedItem == null)
            {
                cmbnseminationLocation.TextBox.SetValidation("Please select Insemination Location");
                cmbnseminationLocation.TextBox.RaiseValidationError();
                cmbnseminationLocation.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbnseminationLocation.SelectedItem).ID == 0)
            {
                cmbnseminationLocation.TextBox.SetValidation("Please select Insemination Location");
                cmbnseminationLocation.TextBox.RaiseValidationError();
                cmbnseminationLocation.Focus();
                result = false;
            }
            else
                cmbnseminationLocation.TextBox.ClearValidationError();

            if (rdoHomologous.IsChecked == true)
            {
                if (HomoCollectionDate.SelectedDate == null)
                {
                    HomoCollectionDate.SetValidation("Please select Collection Date");
                    HomoCollectionDate.RaiseValidationError();
                    HomoCollectionDate.Focus();
                    result = false;
                }
                else
                    HomoCollectionDate.ClearValidationError();

                if (HomoPreperationDate.SelectedDate == null)
                {
                    HomoPreperationDate.SetValidation("Please select Preperation Date");
                    HomoPreperationDate.RaiseValidationError();
                    HomoPreperationDate.Focus();
                    result = false;
                }
                else
                    HomoPreperationDate.ClearValidationError();
                if (HomoThawingDate.SelectedDate == null)
                {
                    HomoThawingDate.SetValidation("Please select Thawing Date");
                    HomoThawingDate.RaiseValidationError();
                    HomoThawingDate.Focus();
                    result = false;
                }
                else
                    HomoThawingDate.ClearValidationError();

                if (cmbHomoCollectionMethod.SelectedItem == null)
                {
                    cmbHomoCollectionMethod.TextBox.SetValidation("Please select Collection Method");
                    cmbHomoCollectionMethod.TextBox.RaiseValidationError();
                    cmbHomoCollectionMethod.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbHomoCollectionMethod.SelectedItem).ID == 0)
                {
                    cmbHomoCollectionMethod.TextBox.SetValidation("Please select Collection Method");
                    cmbHomoCollectionMethod.TextBox.RaiseValidationError();
                    cmbHomoCollectionMethod.Focus();
                    result = false;
                }
                else
                    cmbHomoCollectionMethod.TextBox.ClearValidationError();

            }

            if (rdoHetrologous.IsChecked == true)
            {
                if (HetroCollectionDate.SelectedDate == null)
                {
                    HetroCollectionDate.SetValidation("Please select Collection Date");
                    HetroCollectionDate.RaiseValidationError();
                    HetroCollectionDate.Focus();
                    result = false;
                }
                else
                    HetroCollectionDate.ClearValidationError();

                if (HetroPreperationDate.SelectedDate == null)
                {
                    HetroPreperationDate.SetValidation("Please select Preperation Date");
                    HetroPreperationDate.RaiseValidationError();
                    HetroPreperationDate.Focus();
                    result = false;
                }
                else
                    HetroPreperationDate.ClearValidationError();
                if (HetroThawingDate.SelectedDate == null)
                {
                    HetroThawingDate.SetValidation("Please select Thawing Date");
                    HetroThawingDate.RaiseValidationError();
                    HetroThawingDate.Focus();
                    result = false;
                }
                else
                    HetroThawingDate.ClearValidationError();

                if (cmbHetroCollectionMethod.SelectedItem == null)
                {
                    cmbHetroCollectionMethod.TextBox.SetValidation("Please select Collection Method");
                    cmbHetroCollectionMethod.TextBox.RaiseValidationError();
                    cmbHetroCollectionMethod.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbHetroCollectionMethod.SelectedItem).ID == 0)
                {
                    cmbHetroCollectionMethod.TextBox.SetValidation("Please select Collection Method");
                    cmbHetroCollectionMethod.TextBox.RaiseValidationError();
                    cmbHetroCollectionMethod.Focus();
                    result = false;
                }
                else
                    cmbHetroCollectionMethod.TextBox.ClearValidationError();

            }

            return result;

        }
        private void rdoHomologous_Click(object sender, RoutedEventArgs e)
        {
            if (rdoHomologous.IsChecked == true)
            {
                if (((clsIVFDashboard_IUIVO)this.DataContext).IsHomologous == true)
                {
                    clearHetroTab();
                }
                else
                {
                    clearHomoTab();
                }
                rdoHomologous.IsChecked = true;
                rdoHetrologous.IsChecked = false;
                Homologous.IsSelected = true;
                Hetrologous.IsEnabled = false;
                Homologous.IsEnabled = true;
               
              


            }
            if (rdoHetrologous.IsChecked == true)
            {
                if (((clsIVFDashboard_IUIVO)this.DataContext).IsHomologous == true)
                {
                    clearHetroTab();
                }
                else
                {
                    clearHomoTab();
                }
                rdoHetrologous.IsChecked = true;
                rdoHomologous.IsChecked = false;
                Hetrologous.IsSelected = true;
                Homologous.IsEnabled = false;
                Hetrologous.IsEnabled = true;
               
               
            }
            
        }
        string textBefore = "0";
        int selectionStart = 0;
        int selectionLength = 0;
   

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "0";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void clearHomoTab() 
        {
            HomoCollectionDate.SelectedDate = DateTime.Now;
            HomoPreperationDate.SelectedDate = DateTime.Now;
            HomoThawingDate.SelectedDate = DateTime.Now;
            txtHomoSampleID.Text = "";
            txtHomoPurpose.Text = "";
            txtHomoDiagnosis.Text = "";
            cmbHomoCollectionMethod.SelectedValue = (long)0;
            txtHomoInseminatedAmounts.Text = "0";
            txtHomoNumberofMotileSperm.Text = "0";
            txtHomoNativeAmount.Text = "0";
            txtHomoAfterPrepAmount.Text = "0";
            txtHomoNativeConcentration.Text = "0";
            txtHomoAfterPrepConcentration.Text = "0";
            txtHomoNativeProgressiveMotatity.Text = "0";
            txtHomoAfterPrepProgressiveMotatity.Text = "0";
            txtHomoNativeOverallMotality.Text = "0";
            txtHomoAfterPrepOverallMotality.Text = "0";
            txtHomoNativeNormalForms.Text = "0";
            txtHomoAfterPrepNormalForms.Text = "0";
            txtHomoNativeTotalNoOfSperms.Text = "0";
            txtHomoAfterPrepTotalNoOfSperms.Text = "0";
            txtHomoNotes.Text = "";
        }
        private void clearHetroTab()
        {
            HetroCollectionDate.SelectedDate = DateTime.Now;
            HetroPreperationDate.SelectedDate = DateTime.Now;
            HetroThawingDate.SelectedDate = DateTime.Now;
            txtHetroSampleID.Text = "";
            txtHetroPurpose.Text = "";
            txtHetroDiagnosis.Text = "";
            cmbHetroCollectionMethod.SelectedValue = (long)0;
            txtHetroInseminatedAmounts.Text = "0";
            txtHetroNumberofMotileSperm.Text = "0";
            txtHetroNativeAmount.Text = "0";
            txtHetroAfterPrepAmount.Text = "0";
            txtHetroNativeConcentration.Text = "0";
            txtHetroAfterPrepConcentration.Text = "0";
            txtHetroNativeProgressiveMotatity.Text = "0";
            txtHetroAfterPrepProgressiveMotatity.Text = "0";
            txtHetroNativeOverallMotality.Text = "0";
            txtHetroAfterPrepOverallMotality.Text = "0";
            txtHetroNativeNormalForms.Text = "0";
            txtHetroAfterPrepNormalForms.Text = "0";
            txtHetroNativeTotalNoOfSperms.Text = "0";
            txtHetroAfterPrepTotalNoOfSperms.Text = "0";
            txtHetroNotes.Text = "";
        }
    }
}
