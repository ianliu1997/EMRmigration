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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.IVF.PatientList;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmOnlyET : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        private List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
            }
        }
        private ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO> _ETDetails = new ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO>();
        public ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO> ETDetails
        {
            get { return _ETDetails; }
            set { _ETDetails = value; }
        }
        private ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO> _RemoveETDetails = new ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO>();
        public ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO> RemoveETDetails
        {
            get { return _RemoveETDetails; }
            set { _RemoveETDetails = value; }
        }
        private List<MasterListItem> _CellStage = new List<MasterListItem>();
        public List<MasterListItem> CellStage
        {
            get
            {
                return _CellStage;
            }
            set
            {
                _CellStage = value;
            }
        }
        public frmOnlyET()
        {
            InitializeComponent();
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

                    cmbSrcSemen.ItemsSource = null;
                    cmbSrcSemen.ItemsSource = objList;
                    cmbSrcSemen.SelectedValue = (long)0;
                    fillPattern();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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

                    cmbSrcOocyte.ItemsSource = null;
                    cmbSrcOocyte.ItemsSource = objList;
                    cmbSrcOocyte.SelectedValue = (long)0;
                    fillSemenSource();
                  
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #region Fill Master Items

        private void fillDoctor()
        {
            try
            {
                
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = 0;
                BizAction.DepartmentId = 0;
                BizAction.SpecializationID = 0;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                        cmbEmbryologist.ItemsSource = null;
                        cmbEmbryologist.ItemsSource = objList;
                        cmbEmbryologist.SelectedValue = (long)0;


                        cmbAnesthetist.ItemsSource = null;
                        cmbAnesthetist.ItemsSource = objList;
                        cmbAnesthetist.SelectedValue = (long)0;

                        cmbAssistantAnesthetist.ItemsSource = null;
                        cmbAssistantAnesthetist.ItemsSource = objList;
                        cmbAssistantAnesthetist.SelectedValue = (long)0;

                        cmbAssistantEmbryologist.ItemsSource = null;
                        cmbAssistantEmbryologist.ItemsSource = objList;
                        cmbAssistantEmbryologist.SelectedValue = (long)0;

                        if (this.DataContext != null)
                        {
                           
                        }
                        fillCatheterType();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void fillCatheterType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_CatheterTypeMaster;
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

                    cmbCatheterType.ItemsSource = null;
                    cmbCatheterType.ItemsSource = objList;
                    cmbCatheterType.SelectedValue = (long)0;

                    if (this.DataContext != null)
                    {
                     
                    }
                    fillOocyteSource();
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillDifficultyType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_DifficultyTypeMaster;
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

                    cmbDifficultyType.ItemsSource = null;
                    cmbDifficultyType.ItemsSource = objList;
                    cmbDifficultyType.SelectedValue = (long)0;

                    if (this.DataContext != null)
                    {
                       
                    }
                    fillGrade();
                   
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillGrade()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_GradeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem(0, "--Select--"));
                        Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                    }
                    FillCellStage();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
               
            }
        }

        private void FillCellStage()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_FertilizationStageMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem(0, "--Select--"));
                        CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillDetails();
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
               
            }
        }
        private void fillPattern()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ETPattern;
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
                    cmbPattern.ItemsSource = null;
                    cmbPattern.ItemsSource = objList;
                    cmbPattern.SelectedValue = (long)0;
                    fillDifficultyType();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion
        public void fillInitailOnlyETDetails()
        {
            ETDetails.Add(new clsIVFDashboard_EmbryoTransferDetailsVO() { OocNo = "Auto Generated", FertilizationStage = "0", FertilizationStageID = 0, Grade = "0", GradeID = 0, FertilizationList = CellStage, GradeList = Grade});
            dgETDetilsGrid.ItemsSource = ETDetails;
           
        }
        private void ChkIsDiificulty_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                cmbDifficultyType.Visibility = Visibility.Visible;
            }
            else
            {
                cmbDifficultyType.Visibility = Visibility.Collapsed;
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtThickNess_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtThickNess_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private bool Validate()
        {
            bool result = true;
            if (dtETDate.SelectedDate == null)
            {
                dtETDate.SetValidation("Please Select Date");
                dtETDate.RaiseValidationError();
                dtETDate.Focus();
                return false;
            }
            else
                dtETDate.ClearValidationError();

            if (txtTime.Value == null)
            {
                txtTime.SetValidation("Please Select Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else
                txtTime.ClearValidationError();
            if (cmbEmbryologist.SelectedItem == null)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;
            }
            else
                cmbEmbryologist.TextBox.ClearValidationError();
            if (cmbAssistantEmbryologist.SelectedItem == null)
            {
                cmbAssistantEmbryologist.TextBox.SetValidation("Please select Assistant Embryologist");
                cmbAssistantEmbryologist.TextBox.RaiseValidationError();
                cmbAssistantEmbryologist.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID == 0)
            {
                cmbAssistantEmbryologist.TextBox.SetValidation("Please select Assistant Embryologist");
                cmbAssistantEmbryologist.TextBox.RaiseValidationError();
                cmbAssistantEmbryologist.Focus();
                result = false;
            }
            else
                cmbAssistantEmbryologist.TextBox.ClearValidationError();

            if (cmbAnesthetist.SelectedItem == null)
            {
                cmbAnesthetist.TextBox.SetValidation("Please select  Anesthetist");
                cmbAnesthetist.TextBox.RaiseValidationError();
                cmbAnesthetist.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbAnesthetist.SelectedItem).ID == 0)
            {
                cmbAnesthetist.TextBox.SetValidation("Please select  Anesthetist");
                cmbAnesthetist.TextBox.RaiseValidationError();
                cmbAnesthetist.Focus();
                result = false;
            }
            else
                cmbAnesthetist.TextBox.ClearValidationError();

            //if (cmbAssistantAnesthetist.SelectedItem == null)
            //{
            //    cmbAssistantAnesthetist.TextBox.SetValidation("Please select Assistant Anesthetist");
            //    cmbAssistantAnesthetist.TextBox.RaiseValidationError();
            //    cmbAssistantAnesthetist.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID == 0)
            //{
            //    cmbAssistantAnesthetist.TextBox.SetValidation("Please select Assistant Anesthetist");
            //    cmbAssistantAnesthetist.TextBox.RaiseValidationError();
            //    cmbAssistantAnesthetist.Focus();
            //    result = false;
            //}
            //else
            //    cmbAssistantAnesthetist.TextBox.ClearValidationError();

            if (cmbCatheterType.SelectedItem == null)
            {
                cmbCatheterType.TextBox.SetValidation("Please select Catheter Type");
                cmbCatheterType.TextBox.RaiseValidationError();
                cmbCatheterType.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbCatheterType.SelectedItem).ID == 0)
            {
                cmbCatheterType.TextBox.SetValidation("Please select  Catheter Type");
                cmbCatheterType.TextBox.RaiseValidationError();
                cmbCatheterType.Focus();
                result = false;
            }
            else
                cmbCatheterType.TextBox.ClearValidationError();

            if (cmbPattern.SelectedItem == null)
            {
                cmbPattern.TextBox.SetValidation("Please select Pattern");
                cmbPattern.TextBox.RaiseValidationError();
                cmbPattern.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbPattern.SelectedItem).ID == 0)
            {
                cmbPattern.TextBox.SetValidation("Please select Pattern");
                cmbPattern.TextBox.RaiseValidationError();
                cmbPattern.Focus();
                result = false;
            }
            else
                cmbPattern.TextBox.ClearValidationError();
            //if (cmbSrcOocyte.SelectedItem == null)
            //{
            //    cmbSrcOocyte.TextBox.SetValidation("Please select source of Oocyte");
            //    cmbSrcOocyte.TextBox.RaiseValidationError();
            //    cmbSrcOocyte.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 0)
            //{
            //    cmbSrcOocyte.TextBox.SetValidation("Please select source of Oocyte");
            //    cmbSrcOocyte.TextBox.RaiseValidationError();
            //    cmbSrcOocyte.Focus();
            //    result = false;
            //}
            //else
            //    cmbSrcOocyte.TextBox.ClearValidationError();
            //if (cmbSrcSemen.SelectedItem == null)
            //{
            //    cmbSrcSemen.TextBox.SetValidation("Please select source of semen");
            //    cmbSrcSemen.TextBox.RaiseValidationError();
            //    cmbSrcSemen.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0)
            //{
            //    cmbSrcSemen.TextBox.SetValidation("Please select source of semen");
            //    cmbSrcSemen.TextBox.RaiseValidationError();
            //    cmbSrcSemen.Focus();
            //    result = false;
            //}
            //else
            //    cmbSrcSemen.TextBox.ClearValidationError();

            return result;
        }       
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
            clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO BizAction = new clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO();
            BizAction.ETDetails = new clsIVFDashboard_EmbryoTransferVO();
           // BizAction.ETDetailsList = new List<clsIVFDashboard_EmbryoTransferDetailsVO>();
            BizAction.ETDetails.ID = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)this.DataContext).ETDetails.ID;
            BizAction.ETDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.ETDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.ETDetails.PlanTherapyID = PlanTherapyID;
            BizAction.ETDetails.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.ETDetails.IsOnlyET = true;
            BizAction.ETDetails.Date = dtETDate.SelectedDate.Value.Date;
            BizAction.ETDetails.Time = Convert.ToDateTime(txtTime.Value);
            if ((MasterListItem)cmbEmbryologist.SelectedItem != null)
                BizAction.ETDetails.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantEmbryologist.SelectedItem != null)
                BizAction.ETDetails.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbCatheterType.SelectedItem != null)
                BizAction.ETDetails.CatheterTypeID = ((MasterListItem)cmbCatheterType.SelectedItem).ID;
            if ((MasterListItem)cmbPattern.SelectedItem != null)
                BizAction.ETDetails.PatternID = ((MasterListItem)cmbPattern.SelectedItem).ID;

            if ((MasterListItem)cmbAnesthetist.SelectedItem != null)
                BizAction.ETDetails.AnethetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantAnesthetist.SelectedItem != null && ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID > 0)
                BizAction.ETDetails.AssistantAnethetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;

            if ((MasterListItem)cmbSrcOocyte.SelectedItem != null)
                BizAction.ETDetails.SrcOoctyID= ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;
            if ((MasterListItem)cmbSrcSemen.SelectedItem != null)
                BizAction.ETDetails.SrcSemenID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
            BizAction.ETDetails.SrcOoctyCode = txtOocyteDonorCode.Text;
            BizAction.ETDetails.SrcSemenCode = txtSemenDonorCode.Text;

            BizAction.ETDetails.EndometriumThickness = Convert.ToDecimal(txtThickNess.Text);
            BizAction.ETDetails.DistanceFundus = Convert.ToDecimal(txtDistanceFromFundus.Text);

            if (chkIsFreezed.IsChecked == true)
                BizAction.ETDetails.IsFreezed = true;
            else
                BizAction.ETDetails.IsFreezed = false;

            if (chkUterinePI.IsChecked == true)
                BizAction.ETDetails.UterineArtery_PI = true;
            else
                BizAction.ETDetails.UterineArtery_PI = false;

            if (chkUterineRI.IsChecked == true)
                BizAction.ETDetails.UterineArtery_RI = true;
            else
                BizAction.ETDetails.UterineArtery_RI = false;

            if (chkUterineSD.IsChecked == true)
                BizAction.ETDetails.UterineArtery_SD = true;
            else
                BizAction.ETDetails.UterineArtery_SD = false;

            if (chkEndometerialPI.IsChecked == true)
                BizAction.ETDetails.Endometerial_PI = true;
            else
                BizAction.ETDetails.Endometerial_PI = false;

            if (chkEndometerialRI.IsChecked == true)
                BizAction.ETDetails.Endometerial_RI = true;
            else
                BizAction.ETDetails.Endometerial_RI = false;

            if (chkEndometerialSD.IsChecked == true)
                BizAction.ETDetails.Endometerial_SD = true;
            else
                BizAction.ETDetails.Endometerial_SD = false;
            if (chkTreatment.IsChecked == true)
                BizAction.ETDetails.TeatmentUnderGA = true;
            else
                BizAction.ETDetails.TeatmentUnderGA = false;
            
            if (ChkIsDiificulty.IsChecked == true)
                BizAction.ETDetails.Difficulty = true;
            else
                BizAction.ETDetails.Difficulty = false;
            if ((MasterListItem)cmbDifficultyType.SelectedItem != null)
                BizAction.ETDetails.DifficultyID = ((MasterListItem)cmbDifficultyType.SelectedItem).ID;

            //foreach (clsIVFDashboard_EmbryoTransferDetailsVO item in dgETDetilsGrid.ItemsSource)
            //{
            //    BizAction.ETDetailsList.Add(item);
            //}
            for (int i = 0; i < ETDetails.Count; i++)
            {

                ETDetails[i].FertStageID= ETDetails[i].SelectedFertilizationStage.ID;
                ETDetails[i].GradeID = ETDetails[i].SelectedGrade.ID;
                ETDetails[i].OocyteNumber = i + 1;
                ETDetails[i].Status = true;
            }
            //for (int i = 0; i < RemoveETDetails.Count; i++)
            //{
            //    ETDetails.Add(RemoveETDetails[i]);
            //}
            foreach (var item in ETDetails)
            {
                BizAction.ETDetailsList.Add(item);
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

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

        private void FillDetails() 
        {
            clsIVFDashboard_GetEmbryoTansferBizActionVO BizAction = new clsIVFDashboard_GetEmbryoTansferBizActionVO();
            BizAction.ETDetails = new clsIVFDashboard_EmbryoTransferVO();
            BizAction.ETDetails.PlanTherapyID = PlanTherapyID;
            BizAction.ETDetails.PlanTherapyUnitID = PlanTherapyUnitID;
            if (CoupleDetails != null)
            {
                BizAction.ETDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.ETDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            }
            BizAction.ETDetails.IsOnlyET = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result != null)
                    {
                        this.DataContext = (clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result;
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EmbryologistID != null)
                        {
                            cmbEmbryologist.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EmbryologistID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Date != null)
                        {
                            dtETDate.SelectedDate = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Date;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Time != null)
                        {
                            txtTime.Value = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Time;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EndometriumThickness != null)
                        {
                            txtThickNess.Text = Convert.ToString(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EndometriumThickness);
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DistanceFundus != null)
                        {
                            txtDistanceFromFundus.Text = Convert.ToString(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DistanceFundus);
                        }

                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssitantEmbryologistID != null)
                        {
                            cmbAssistantEmbryologist.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssitantEmbryologistID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.PatternID != null)
                        {
                            cmbPattern.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.PatternID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.CatheterTypeID != null)
                        {
                            cmbCatheterType.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.CatheterTypeID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Difficulty == true)
                        {
                            ChkIsDiificulty.IsChecked = true;
                            cmbDifficultyType.Visibility = Visibility.Visible;
                            if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID != null)
                            {
                                cmbDifficultyType.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID;
                            }

                        }
                        else
                        {
                            ChkIsDiificulty.IsChecked = false;
                            cmbDifficultyType.Visibility = Visibility.Collapsed;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Endometerial_PI == true)
                        {
                            chkEndometerialPI.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Endometerial_RI == true)
                        {
                            chkEndometerialRI.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Endometerial_SD == true)
                        {
                            chkEndometerialSD.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.UterineArtery_PI == true)
                        {
                            chkUterinePI.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.UterineArtery_RI == true)
                        {
                            chkUterineRI.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.UterineArtery_SD == true)
                        {
                            chkUterineSD.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.TeatmentUnderGA == true)
                        {
                            chkTreatment.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AnethetistID != null)
                        {
                            cmbAnesthetist.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AnethetistID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssistantAnethetistID != null)
                        {
                            cmbAssistantAnesthetist.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssistantAnethetistID;
                        }

                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcOoctyID != null)
                        {
                            cmbSrcOocyte.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcOoctyID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcSemenID != null)
                        {
                            cmbSrcSemen.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcSemenID;
                        }
                        if ( ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcSemenCode != null)
                        {
                             txtSemenDonorCode.Text =  ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcSemenCode;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcSemenCode != null)
                        {
                            txtOocyteDonorCode.Text  = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.SrcOoctyCode;
                        }
                  
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList != null)
                        {
                            if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList.Count > 0)
                            {
                                for (int i = 0; i < ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList.Count; i++)
                                {
                                    ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].FertilizationList = CellStage;
                                    ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].GradeList = Grade;

                                        if (Convert.ToInt64(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].GradeID) > 0)
                                        {
                                            ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].GradeID));
                                        }
                                        else
                                        {
                                            ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                                        }
                                    
                                        if (Convert.ToInt64(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].FertStageID) > 0)
                                        {
                                            ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].SelectedFertilizationStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].FertStageID));
                                        }
                                        else
                                        {
                                            ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i].SelectedFertilizationStage = CellStage.FirstOrDefault(p => p.ID == 0);
                                        }
                                    


                                    ETDetails.Add(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList[i]);
                                }
                               

                                dgETDetilsGrid.ItemsSource = null;
                                dgETDetilsGrid.ItemsSource = ETDetails;
                            }
                            else
                            {
                                fillInitailOnlyETDetails();
                            }
                        }
                      
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
   
        #region File Attachment Events
        byte[] AttachedFileContents;
        string AttachedFileName;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                ETDetails[dgETDetilsGrid.SelectedIndex].FileName = openDialog.File.Name;

                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        ETDetails[dgETDetilsGrid.SelectedIndex].FileContents = AttachedFileContents;
                    }

                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        private void hprlnkViewFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileName))
            {
                if (((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileContents != null)
                {


                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileName });
                            AttachedFileNameList.Add(((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileName, ((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem).FileContents);
                }
            }
        }
        #endregion

        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbCellStage"))
            {
                for (int i = 0; i < ETDetails.Count; i++)
                {
                    if (ETDetails[i] == ((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            ETDetails[i].FertStageID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGrade"))
            {
                for (int i = 0; i < ETDetails.Count; i++)
                {
                    if (ETDetails[i] == ((clsIVFDashboard_EmbryoTransferDetailsVO)dgETDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            ETDetails[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ETDetails == null)
            {
                ETDetails = new ObservableCollection<clsIVFDashboard_EmbryoTransferDetailsVO>();
            }
            ETDetails.Add(new clsIVFDashboard_EmbryoTransferDetailsVO() { OocNo = "Auto Generated", FertilizationStage = "0", FertilizationStageID = 0, Grade = "0", GradeID = 0, FertilizationList = CellStage, GradeList = Grade });

            dgETDetilsGrid.ItemsSource = ETDetails;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgETDetilsGrid.SelectedItem != null)
            {
                //if (ETDetails.Count > 1)
                //{
                //    ETDetails[dgETDetilsGrid.SelectedIndex].OocNo = "-1";
                //    if (ETDetails[dgETDetilsGrid.SelectedIndex].ID != 0 && !ETDetails[dgETDetilsGrid.SelectedIndex].OocNo.Equals("Auto Generated"))
                //    {
                //        RemoveETDetails.Add(ETDetails[dgETDetilsGrid.SelectedIndex]);
                //    }
                    ETDetails.RemoveAt(dgETDetilsGrid.SelectedIndex);
                //}
            }

            dgETDetilsGrid.ItemsSource = ETDetails;
        }
        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcSemen.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                }
                else
                {
                    btnSearchSemenDonor.IsEnabled = true;
                    txtSemenDonorCode.IsEnabled = true;
                }
            }

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
        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 8;
            Win.Show();

        }

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 9;
            Win.Show();
        }
        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 8)
                    txtOocyteDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                else
                    txtSemenDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
             if (IsClosed == true)
            {
                cmdSave.IsEnabled = false;
            }
            fillDoctor();
        }

        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(PlanTherapyID, PlanTherapyUnitID, CoupleDetails, "OnlyET");
            Win.Show();


        }
    }
}
