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
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmTransfer : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        public bool IsSurrogate = false;
        public long EMRProcedureID;
        public long EMRProcedureUnitID;

        public frmTransfer()
        {
            InitializeComponent();
            this.DataContext = null;
        }
        #region Fill Master Items

        private void fillDoctor()
        {
            try
            {

                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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
                        //cmbEmbryologist.SelectedValue = (long)0;
                        cmbEmbryologist.SelectedValue = EmbryologistID;
                        cmbAssistantEmbryologist.ItemsSource = null;
                        cmbAssistantEmbryologist.ItemsSource = objList;
                        // cmbAssistantEmbryologist.SelectedValue = (long)0;
                        cmbAssistantEmbryologist.SelectedValue = AssiEmbryologistID;
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
                    fillDifficultyType();

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //added by neena
        private void FillAnesthetist()
        {
            clsGetAnesthetistBizActionVO BizAction = new clsGetAnesthetistBizActionVO();
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
                    objList.AddRange(((clsGetAnesthetistBizActionVO)arg.Result).MasterList);
                    cmbAnesthetist.ItemsSource = null;
                    cmbAnesthetist.ItemsSource = objList;
                    cmbAnesthetist.SelectedItem = objList[0];
                    fillDetails();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public List<MasterListItem> SurrogatePatientList;
        public void GetSurrogatePatientList()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();
            BizActionObject.IsSurrogacyForTransfer = true;
            BizActionObject.EMRProcedureID = EMRProcedureID;
            BizActionObject.EMRProcedureUnitID = EMRProcedureUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        SurrogatePatientList = new List<MasterListItem>();
                        SurrogatePatientList.Add(new MasterListItem { PatientID = 0, PatientUnitID = 0, MrNo = "--Select--", Description = "--Select--" });
                        SurrogatePatientList.Add(new MasterListItem { PatientID = CoupleDetails.FemalePatient.PatientID, PatientUnitID = CoupleDetails.FemalePatient.UnitId, MrNo = "Self", Description = "Self" });

                        //SurrogatePatientList = new List<clsPatientGeneralVO>();
                        //SurrogatePatientList.Add(new clsPatientGeneralVO { PatientID = 0, MRNo = "--Select--" });
                        //SurrogatePatientList.Add(new clsPatientGeneralVO { PatientID = CoupleDetails.FemalePatient.PatientID,UnitId=CoupleDetails.FemalePatient.UnitId, MRNo = "Self" });
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.PatientID = person.PatientID;
                            Obj.PatientUnitID = person.UnitId;
                            Obj.MrNo = person.MRNo;
                            Obj.Description = person.MRNo;
                            SurrogatePatientList.Add(Obj);
                        }
                    }

                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }

        //

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
                    fillPattern();

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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
                    FillAnesthetist();

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillDetails()
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
            BizAction.ETDetails.IsOnlyET = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result != null)
                    {
                        this.DataContext = (clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result;
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EmbryologistID != null && ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.EmbryologistID != 0)
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

                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssitantEmbryologistID != null && ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AssitantEmbryologistID != 0)
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
                        //if(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.Difficulty ==true)
                        //{
                        //    ChkIsDiificulty.IsChecked = true;
                        //    cmbDifficultyType.Visibility=Visibility.Visible;
                        //    if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID!= null)
                        //    {
                        //        cmbDifficultyType.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID;
                        //    }

                        //}
                        //else
                        //{
                        //    ChkIsDiificulty.IsChecked = false;
                        //    cmbDifficultyType.Visibility = Visibility.Collapsed;
                        //}
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
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.TenaculumUsed == true)
                        {
                            chkTenaculumUsed.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.IsFreezed == true)
                        {
                            chkIsFreezed.IsChecked = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.IsFreezed;
                            chkIsFreezed.IsEnabled = false;
                            cmdNew.IsEnabled = false;
                        }
                        else
                        {
                            chkIsFreezed.IsEnabled = true;
                        }

                        //added by neena
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.IsAnesthesia == true)
                        {
                            rdoYesAns.IsChecked = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.IsAnesthesia;
                        }

                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AnethetistID != null)
                        {
                            cmbAnesthetist.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.AnethetistID;
                        }

                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID != null)
                        {
                            cmbDifficultyType.SelectedValue = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.DifficultyID;
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.FreshEmb != null)
                        {
                            txtEmbryosFresh.Text = Convert.ToString(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.FreshEmb);
                        }
                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.FrozenEmb != null)
                        {
                            txtEmbryosFrozen.Text = Convert.ToString(((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetails.FrozenEmb);
                        }

                        //


                        if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList != null)
                        {
                            if (((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList.Count > 0)
                            {
                                if (IsSurrogate)
                                {
                                    dgETDetilsGrid.Columns[14].Visibility = Visibility.Visible;

                                }

                                foreach (var item in ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList)
                                {
                                    item.SurrogatePatientList = SurrogatePatientList;

                                    if ((item.SurrogateID) > 0)
                                    {
                                        item.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == item.SurrogateID);
                                    }
                                    else
                                    {
                                        item.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == 0);
                                    }

                                    if (item.IsFreshEmbryoPGDPGS || item.IsFrozenEmbryoPGDPGS)
                                        item.PGDPGS = "PGD/PGS";
                                }

                                dgETDetilsGrid.ItemsSource = null;
                                dgETDetilsGrid.ItemsSource = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)arg.Result).ETDetailsList;
                            }
                        }
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion
        private bool Validate()
        {
            bool result = true;
            if (dtETDate.SelectedDate == null)
            {
                dtETDate.SetValidation("Please Select Date");
                dtETDate.RaiseValidationError();
                dtETDate.Focus();
                result = false;
            }
            else
                dtETDate.ClearValidationError();

            if (txtTime.Value == null)
            {
                txtTime.SetValidation("Please Select Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                result = false;
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

            if (rdoYesAns.IsChecked == true)
            {
                if (cmbAnesthetist.SelectedItem == null)
                {
                    cmbAnesthetist.TextBox.SetValidation("Please select Anesthetist");
                    cmbAnesthetist.TextBox.RaiseValidationError();
                    cmbAnesthetist.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbAnesthetist.SelectedItem).ID == 0)
                {
                    cmbAnesthetist.TextBox.SetValidation("Please select Anesthetist");
                    cmbAnesthetist.TextBox.RaiseValidationError();
                    cmbAnesthetist.Focus();
                    result = false;
                }
                else
                    cmbAnesthetist.TextBox.ClearValidationError();
            }

            //if (cmbAssistantEmbryologist.SelectedItem == null)
            //{
            //    cmbAssistantEmbryologist.TextBox.SetValidation("Please select Assistant Embryologist");
            //    cmbAssistantEmbryologist.TextBox.RaiseValidationError();
            //    cmbAssistantEmbryologist.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID == 0)
            //{
            //    cmbAssistantEmbryologist.TextBox.SetValidation("Please select Assistant Embryologist");
            //    cmbAssistantEmbryologist.TextBox.RaiseValidationError();
            //    cmbAssistantEmbryologist.Focus();
            //    result = false;
            //}
            //else
            //    cmbAssistantEmbryologist.TextBox.ClearValidationError();

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
            BizAction.ETDetailsList = new List<clsIVFDashboard_EmbryoTransferDetailsVO>();
            BizAction.ETDetails.ID = ((clsIVFDashboard_GetEmbryoTansferBizActionVO)this.DataContext).ETDetails.ID;
            BizAction.ETDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.ETDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.ETDetails.PlanTherapyID = PlanTherapyID;
            BizAction.ETDetails.PlanTherapyUnitID = PlanTherapyUnitID;

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
            if (chkTenaculumUsed.IsChecked == true)
                BizAction.ETDetails.TenaculumUsed = true;
            else
                BizAction.ETDetails.TenaculumUsed = false;
            if (ChkIsDiificulty.IsChecked == true)
                BizAction.ETDetails.Difficulty = true;
            else
                BizAction.ETDetails.Difficulty = false;
            if ((MasterListItem)cmbDifficultyType.SelectedItem != null)
                BizAction.ETDetails.DifficultyID = ((MasterListItem)cmbDifficultyType.SelectedItem).ID;

            //added by neena
            if (rdoYesAns.IsChecked == true)
                BizAction.ETDetails.IsAnesthesia = true;
            else
                BizAction.ETDetails.IsAnesthesia = false;

            if ((MasterListItem)cmbAnesthetist.SelectedItem != null)
                BizAction.ETDetails.AnethetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;

            BizAction.ETDetails.FreshEmb = Convert.ToInt64(txtEmbryosFresh.Text);
            BizAction.ETDetails.FrozenEmb = Convert.ToInt64(txtEmbryosFrozen.Text);

            //

            foreach (clsIVFDashboard_EmbryoTransferDetailsVO item in dgETDetilsGrid.ItemsSource)
            {
                if (IsSurrogate)
                {
                    item.SurrogateID = item.SelectedSurrogatePatientList.PatientID;
                    item.SurrogateUnitID = item.SelectedSurrogatePatientList.PatientUnitID;
                    item.SurrogatePatientMrNo = item.SelectedSurrogatePatientList.MrNo;
                }
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
                    fillDetails();
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SurrogatePatientList = new List<MasterListItem>();
            if (IsClosed == true)
            {
                cmdNew.IsEnabled = false;
            }
            fillOPUDetails();
            fillDoctor();

            if (IsSurrogate)
            {
                GetSurrogatePatientList();
                dgETDetilsGrid.Columns[14].Visibility = Visibility.Visible;
            }
            else
                dgETDetilsGrid.Columns[14].Visibility = Visibility.Collapsed;

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    cmbDifficultyType.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    cmbDifficultyType.Visibility = Visibility.Collapsed;
            //}
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

        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(PlanTherapyID, PlanTherapyUnitID, CoupleDetails, "ET");
            Win.Show();


        }
        long EmbryologistID = 0;
        long AssiEmbryologistID = 0;
        private void fillOPUDetails()
        {
            try
            {

                clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = new clsIVFDashboard_GetOPUDetailsBizActionVO();
                BizAction.Details = new clsIVFDashboard_OPUVO();
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
                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                        {
                            EmbryologistID = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.EmbryologistID;
                            AssiEmbryologistID = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }
                    }


                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private void rdoYesAns_Click(object sender, RoutedEventArgs e)
        {
            if (rdoYesAns.IsChecked == true)
                cmbAnesthetist.IsEnabled = true;
        }

        private void AutoCompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //for (int i = 0; i < SurrogatePatientList.Count; i++)
            //{
            //    if (i == dgETDetilsGrid.SelectedIndex)
            //    {
            //        if (((clsPatientGeneralVO)((AutoCompleteBox)sender).SelectedItem) != null)
            //        {
            //            SurrogatePatientList[i].MRNo= ((clsPatientGeneralVO)((AutoCompleteBox)sender).SelectedItem).MRNo;
            //        }
            //    }
            //}
        }

        private void txtThickNess_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidTwoDigitWithOneDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtDistanceFromFundus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidOneDigitWithOneDecimal() && textBefore != null)
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
