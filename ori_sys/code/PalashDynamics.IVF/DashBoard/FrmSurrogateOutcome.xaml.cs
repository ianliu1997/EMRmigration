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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Patient;
using MessageBoxControl;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmSurrogateOutcome : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlannedTreatmentID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        public DateTime? OPUDate;
        public DateTime? TherapyStartDate { get; set; }
        public event RoutedEventHandler OnSaveButton_Click;
        WaitIndicator wait = new WaitIndicator();
        public bool IsSurrogate = false;
        public long EMRProcedureID;
        public long EMRProcedureUnitID;

        public FrmSurrogateOutcome()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillBHCGResult();

        }

        private List<MasterListItem> _OutcomeResultList = new List<MasterListItem>();
        public List<MasterListItem> OutcomeResultList
        {
            get
            {
                return _OutcomeResultList;
            }
            set
            {
                _OutcomeResultList = value;
            }
        }

        private List<MasterListItem> _OutcomePregnancyList = new List<MasterListItem>();
        public List<MasterListItem> OutcomePregnancyList
        {
            get
            {
                return _OutcomePregnancyList;
            }
            set
            {
                _OutcomePregnancyList = value;
            }
        }

        private List<MasterListItem> _BHCGResultList = new List<MasterListItem>();
        public List<MasterListItem> BHCGResultList
        {
            get
            {
                return _BHCGResultList;
            }
            set
            {
                _BHCGResultList = value;
            }
        }

        private List<MasterListItem> _SurrogatePatientList = new List<MasterListItem>();
        public List<MasterListItem> SurrogatePatientList
        {
            get
            {
                return _SurrogatePatientList;
            }
            set
            {
                _SurrogatePatientList = value;
            }
        }

        ObservableCollection<clsIVFDashboard_OutcomeVO> lstFile { get; set; }

        private void FillBHCGResult()
        {
            List<MasterListItem> BHCGResult = new List<MasterListItem>();
            BHCGResult.Add(new MasterListItem(0, "--Select--"));
            BHCGResult.Add(new MasterListItem(1, "Positive"));
            BHCGResult.Add(new MasterListItem(2, "Negative"));
            BHCGResultList = BHCGResult;
            GetSurrogatePatientList();
        }

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

                        if (result.PatientDetailsList.Count == 1)
                            AccItemSurrogate1.Header = "Surrogate 1 : " + result.PatientDetailsList[0].MRNo;
                        if (result.PatientDetailsList.Count == 2)
                        {
                            AccItemSurrogate1.Header = "Surrogate 1 : " + result.PatientDetailsList[0].MRNo;
                            AccItemSurrogate2.Header = "Surrogate 2 : " + result.PatientDetailsList[1].MRNo;
                        }
                    }
                    FillOutcomeResult();

                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }

        private void FillOutcomeResult()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_OutcomeResultMaster;
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
                        OutcomeResultList = null;
                        OutcomeResultList = objList;
                    }
                    FillOutcomePregnacyAchieved();

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void FillOutcomePregnacyAchieved()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_OutcomePregnancyAchievedMaster;
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
                        cmbPregnancyAchieved.ItemsSource = null;
                        cmbPregnancyAchieved.ItemsSource = objList;
                        cmbPregnancyAchieved.SelectedItem = objList[0];
                        cmbPregnancyAchievedSurrogate1.ItemsSource = null;
                        cmbPregnancyAchievedSurrogate1.ItemsSource = objList;
                        cmbPregnancyAchievedSurrogate1.SelectedItem = objList[0];
                        cmbPregnancyAchievedSurrogate2.ItemsSource = null;
                        cmbPregnancyAchievedSurrogate2.ItemsSource = objList;
                        cmbPregnancyAchievedSurrogate2.SelectedItem = objList[0];
                    }
                    FillOutcomePregnancy();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                //wait.Close();
            }
        }

        private void FillOutcomePregnancy()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_OutcomePregnancyMaster;
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
                        OutcomePregnancyList = null;
                        OutcomePregnancyList = objList;
                    }
                    fillOutcomeDetails();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        #region outcome
        bool blSacsNo = false;
        //long FetalHeartCount = 0;
        private void fillOutcomeDetails()
        {
            clsIVFDashboard_GetOutcomeBizActionVO BizAction = new clsIVFDashboard_GetOutcomeBizActionVO();
            BizAction.Details = new clsIVFDashboard_OutcomeVO();
            BizAction.Details.PlanTherapyID = PlanTherapyID;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details;

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsClosed == true)
                        {
                            chkIsclosed.IsChecked = true;
                            cmdSaveOut.IsEnabled = false;
                            txtNoOfSacs.IsEnabled = false;
                        }
                        else
                        {
                            if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                                txtNoOfSacs.IsEnabled = false;
                            else
                            {
                                chkIsclosed.IsChecked = false;
                                cmdSaveOut.IsEnabled = true;
                                txtNoOfSacs.IsEnabled = true;
                            }
                        }

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsFreeze == true)
                        {
                            txtNoOfSacsSurrogate1.IsEnabled = false;
                            dtObservationDateSurrogate1.IsEnabled = false;
                            txtSacRemarkSurrogate1.IsEnabled = false;
                            chkIsUnlinkSurrogate1.IsEnabled = false;
                            dgSacsDetailsGridSurrogate1.IsEnabled = false;
                            txtNoOfSacsSurrogate2.IsEnabled = false;
                            dtObservationDateSurrogate2.IsEnabled = false;
                            txtSacRemarkSurrogate2.IsEnabled = false;
                            chkIsUnlinkSurrogate2.IsEnabled = false;
                            dgSacsDetailsGridSurrogate2.IsEnabled = false;
                            chkIsFreeze.IsEnabled = false;
                            chkIsFreeze.IsChecked = true;
                        }
                        else
                        {
                            txtNoOfSacsSurrogate1.IsEnabled = true;
                            dtObservationDateSurrogate1.IsEnabled = true;
                            txtSacRemarkSurrogate1.IsEnabled = true;
                            chkIsUnlinkSurrogate1.IsEnabled = true;
                            dgSacsDetailsGridSurrogate1.IsEnabled = true;
                            txtNoOfSacsSurrogate2.IsEnabled = true;
                            dtObservationDateSurrogate2.IsEnabled = true;
                            txtSacRemarkSurrogate2.IsEnabled = true;
                            chkIsUnlinkSurrogate2.IsEnabled = true;
                            dgSacsDetailsGridSurrogate2.IsEnabled = true;
                            chkIsFreeze.IsEnabled = true;
                            chkIsFreeze.IsChecked = false;
                        }

                        //added by neena
                        if ((((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomeDetailsList) != null && (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomeDetailsList).Count > 0)
                        {
                            lstFile = new ObservableCollection<clsIVFDashboard_OutcomeVO>();
                            foreach (var item in (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomeDetailsList))
                            {
                                item.BHCGResultListVO = BHCGResultList;
                                item.SurrogatePatientList = SurrogatePatientList;

                                if ((item.ResultListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.ResultListID) > 0)
                                    {
                                        item.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ResultListID));
                                    }
                                    else
                                    {
                                        item.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((item.SurrogateID) > 0)
                                {
                                    item.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == item.SurrogateID);
                                }
                                else
                                {
                                    item.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == 0);
                                }
                                lstFile.Add(item);
                            }
                            dgBHCGDetailsGrid.ItemsSource = lstFile;
                        }
                        else
                        {
                            lstFile = new ObservableCollection<clsIVFDashboard_OutcomeVO>();
                            clsIVFDashboard_OutcomeVO Obj = new clsIVFDashboard_OutcomeVO();
                            Obj.BHCGResultListVO = BHCGResultList;
                            Obj.SurrogatePatientList = SurrogatePatientList;

                            if ((Obj.ResultListID) >= 0)
                            {
                                if (Convert.ToInt64(Obj.ResultListID) > 0)
                                {
                                    Obj.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(Obj.ResultListID));
                                }
                                else
                                {
                                    Obj.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == 0);
                                }
                            }

                            if ((Obj.SurrogateID) > 0)
                            {
                                Obj.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == Obj.SurrogateID);
                            }
                            else
                            {
                                Obj.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == 0);
                            }
                            lstFile.Add(Obj);
                            dgBHCGDetailsGrid.ItemsSource = lstFile;
                        }

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList.Count > 0)
                        {
                            List<clsIVFDashboard_OutcomeVO> TempOutcomeDetails = new List<clsIVFDashboard_OutcomeVO>();
                            List<clsIVFDashboard_OutcomeVO> TempOutcomeSurrogate1 = new List<clsIVFDashboard_OutcomeVO>();
                            List<clsIVFDashboard_OutcomeVO> TempOutcomeSurrogate2 = new List<clsIVFDashboard_OutcomeVO>();

                            TempOutcomeDetails = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList.Where(x => x.SurrogateTypeID == 0).ToList();
                            TempOutcomeSurrogate1 = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList.Where(x => x.SurrogateTypeID == 1).ToList();
                            TempOutcomeSurrogate2 = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList.Where(x => x.SurrogateTypeID == 2).ToList();


                            if (TempOutcomeDetails.Count > 0)
                            {
                                foreach (var item in TempOutcomeDetails)
                                {
                                    if (item.NoOfSacs > 0)
                                    {
                                        blSacsNo = true;
                                        txtNoOfSacs.Text = item.NoOfSacs.ToString();
                                        txtSacRemark.Text = item.SacRemarks;
                                    }
                                    if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        dtObservationDate.SelectedDate = item.SacsObservationDate;
                                    }

                                    if (item.PregnancyAchievedID != null)
                                    {
                                        cmbPregnancyAchieved.SelectedValue = item.PregnancyAchievedID;
                                    }

                                    txtID.Text = item.ID.ToString();
                                    txtUnitID.Text = item.UnitID.ToString();

                                    if (item.FetalHeartCount > 0)
                                        PregnacncyAchievedGrid.Visibility = Visibility.Visible;
                                }

                            }

                            if (TempOutcomeSurrogate1.Count > 0)
                            {
                                foreach (var item in TempOutcomeSurrogate1)
                                {
                                    if (item.NoOfSacs > 0)
                                    {
                                        blSacsNoSurrogate1 = true;
                                        txtNoOfSacsSurrogate1.Text = item.NoOfSacs.ToString();
                                        txtSacRemarkSurrogate1.Text = item.SacRemarks;
                                    }
                                    if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        dtObservationDateSurrogate1.SelectedDate = item.SacsObservationDate;
                                    }

                                    if (item.PregnancyAchievedID != null)
                                    {
                                        cmbPregnancyAchievedSurrogate1.SelectedValue = item.PregnancyAchievedID;
                                    }

                                    if (item.IsUnlinkSurrogate == true)
                                        chkIsUnlinkSurrogate1.IsChecked = true;
                                    else
                                        chkIsUnlinkSurrogate1.IsChecked = false;
                                    txtSurrogate1ID.Text = item.ID.ToString();
                                    txtSurrogate1UnitID.Text = item.UnitID.ToString();

                                    if (item.FetalHeartCount > 0)
                                    {
                                        txbPregnancyAchievedSurrogate1.Visibility = Visibility.Visible;
                                        cmbPregnancyAchievedSurrogate1.Visibility = Visibility.Visible;
                                    }

                                }

                            }

                            if (TempOutcomeSurrogate2.Count > 0)
                            {
                                foreach (var item in TempOutcomeSurrogate2)
                                {
                                    if (item.NoOfSacs > 0)
                                    {
                                        blSacsNoSurrogate2 = true;
                                        txtNoOfSacsSurrogate2.Text = item.NoOfSacs.ToString();
                                        txtSacRemarkSurrogate2.Text = item.SacRemarks;
                                    }
                                    if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        dtObservationDateSurrogate2.SelectedDate = item.SacsObservationDate;
                                    }

                                    if (item.PregnancyAchievedID != null)
                                    {
                                        cmbPregnancyAchievedSurrogate2.SelectedValue = item.PregnancyAchievedID;
                                    }

                                    if (item.IsUnlinkSurrogate == true)
                                        chkIsUnlinkSurrogate2.IsChecked = true;
                                    else
                                        chkIsUnlinkSurrogate2.IsChecked = false;
                                    txtSurrogate2ID.Text = item.ID.ToString();
                                    txtSurrogate2UnitID.Text = item.UnitID.ToString();

                                    if (item.FetalHeartCount > 0)
                                    {
                                        txbPregnancyAchievedSurrogate2.Visibility = Visibility.Visible;
                                        cmbPregnancyAchievedSurrogate2.Visibility = Visibility.Visible;
                                    }
                                }
                            }
                        }



                        PregnancySacsDetails = new ObservableCollection<clsPregnancySacsDetailsVO>();
                        PregnancySacsDetailsSurrogate1 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                        PregnancySacsDetailsSurrogate2 = new ObservableCollection<clsPregnancySacsDetailsVO>();

                        List<clsPregnancySacsDetailsVO> TempPregnancySacsDetails = new List<clsPregnancySacsDetailsVO>();
                        List<clsPregnancySacsDetailsVO> TempPregnancySacsDetailsSurrogate1 = new List<clsPregnancySacsDetailsVO>();
                        List<clsPregnancySacsDetailsVO> TempPregnancySacsDetailsSurrogate2 = new List<clsPregnancySacsDetailsVO>();

                        TempPregnancySacsDetails = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList.Where(x => x.SurrogateTypeID == 0).ToList();
                        TempPregnancySacsDetailsSurrogate1 = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList.Where(x => x.SurrogateTypeID == 1).ToList();
                        TempPregnancySacsDetailsSurrogate2 = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList.Where(x => x.SurrogateTypeID == 2).ToList();

                        if (TempPregnancySacsDetails.Count > 0)
                        {
                            foreach (var item in TempPregnancySacsDetails)
                            {
                                //if (item.NoOfSacs > 0)
                                //{
                                //    blSacsNo = true;
                                //    txtNoOfSacs.Text = item.NoOfSacs.ToString();
                                //    txtSacRemark.Text = item.SacRemarks;
                                //}
                                //if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                //{
                                //    dtObservationDate.SelectedDate = item.SacsObservationDate;
                                //}

                                //if (item.PregnancyAchievedID != null)
                                //{
                                //    cmbPregnancyAchieved.SelectedValue = item.PregnancyAchievedID;
                                //}

                                item.OutcomeResultListVO = OutcomeResultList;
                                item.OutcomePregnancyListVO = OutcomePregnancyList;

                                if ((item.ResultListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.ResultListID) > 0)
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ResultListID));
                                    }
                                    else
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((item.PregnancyListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.PregnancyListID) > 0)
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PregnancyListID));
                                    }
                                    else
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                if (item.CongenitalAbnormalityYes == true)
                                {
                                    item.Status = true;
                                }
                                else
                                    item.Status = false;

                                PregnancySacsDetails.Add(item);
                            }
                            dgSacsDetailsGrid.ItemsSource = PregnancySacsDetails;
                            dgSacsDetailsGrid.UpdateLayout();
                            blSacsNo = false;
                        }

                        if (TempPregnancySacsDetailsSurrogate1.Count > 0)
                        {
                            foreach (var item in TempPregnancySacsDetailsSurrogate1)
                            {
                                //if (item.NoOfSacs > 0)
                                //{
                                //    blSacsNoSurrogate1 = true;
                                //    txtNoOfSacsSurrogate1.Text = item.NoOfSacs.ToString();
                                //    txtSacRemarkSurrogate1.Text = item.SacRemarks;
                                //}
                                //if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                //{
                                //    dtObservationDateSurrogate1.SelectedDate = item.SacsObservationDate;
                                //}

                                //if (item.PregnancyAchievedID != null)
                                //{
                                //    cmbPregnancyAchievedSurrogate1.SelectedValue = item.PregnancyAchievedID;
                                //}

                                //if (item.IsUnlinkSurrogate == true)
                                //    chkIsUnlinkSurrogate1.IsChecked = true;
                                //else
                                //    chkIsUnlinkSurrogate1.IsChecked = false;

                                item.OutcomeResultListVO = OutcomeResultList;
                                item.OutcomePregnancyListVO = OutcomePregnancyList;

                                if ((item.ResultListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.ResultListID) > 0)
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ResultListID));
                                    }
                                    else
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((item.PregnancyListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.PregnancyListID) > 0)
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PregnancyListID));
                                    }
                                    else
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                if (item.CongenitalAbnormalityYes == true)
                                {
                                    item.Status = true;
                                }
                                else
                                    item.Status = false;

                                PregnancySacsDetailsSurrogate1.Add(item);
                            }
                            dgSacsDetailsGridSurrogate1.ItemsSource = PregnancySacsDetailsSurrogate1;
                            dgSacsDetailsGridSurrogate1.UpdateLayout();
                            blSacsNoSurrogate1 = false;
                        }

                        if (TempPregnancySacsDetailsSurrogate2.Count > 0)
                        {
                            foreach (var item in TempPregnancySacsDetailsSurrogate2)
                            {
                                //if (item.NoOfSacs > 0)
                                //{
                                //    blSacsNoSurrogate2 = true;
                                //    txtNoOfSacsSurrogate2.Text = item.NoOfSacs.ToString();
                                //    txtSacRemarkSurrogate2.Text = item.SacRemarks;
                                //}
                                //if (item.SacsObservationDate != null && item.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                //{
                                //    dtObservationDateSurrogate2.SelectedDate = item.SacsObservationDate;
                                //}

                                //if (item.PregnancyAchievedID != null)
                                //{
                                //    cmbPregnancyAchievedSurrogate2.SelectedValue = item.PregnancyAchievedID;
                                //}

                                //if (item.IsUnlinkSurrogate == true)
                                //    chkIsUnlinkSurrogate2.IsChecked = true;
                                //else
                                //    chkIsUnlinkSurrogate2.IsChecked = false;


                                item.OutcomeResultListVO = OutcomeResultList;
                                item.OutcomePregnancyListVO = OutcomePregnancyList;

                                if ((item.ResultListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.ResultListID) > 0)
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ResultListID));
                                    }
                                    else
                                    {
                                        item.selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((item.PregnancyListID) >= 0)
                                {
                                    if (Convert.ToInt64(item.PregnancyListID) > 0)
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PregnancyListID));
                                    }
                                    else
                                    {
                                        item.selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                if (item.CongenitalAbnormalityYes == true)
                                {
                                    item.Status = true;
                                }
                                else
                                    item.Status = false;

                                PregnancySacsDetailsSurrogate2.Add(item);
                            }
                            dgSacsDetailsGridSurrogate2.ItemsSource = PregnancySacsDetailsSurrogate2;
                            dgSacsDetailsGridSurrogate2.UpdateLayout();
                            blSacsNoSurrogate2 = false;
                        }

                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //bool saveDtls = true;
            //saveDtls = validateOutcome();
            //if (saveDtls == true)
            //{

            if (chkIsFreeze.IsChecked == false)
            {
                string msgTitle1 = "Palash";
                string msgText1 = "USG Details is pending.. Are You Sure \n You Want To Save OutCome Details";
                MessageBoxControl.MessageBoxChildWindow msgWin1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle1, msgText1, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin1.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveOutcome();
                    }
                };
                msgWin1.Show();
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save OutCome Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveOutcome();
                    }
                };
                msgWin.Show();
            }
            //}
        }

        clsIVFDashboard_OutcomeVO PregnancySac = null;
        private void SaveOutcome()
        {
            clsIVFDashboard_AddUpdateOutcomeBizActionVO BizAction = new clsIVFDashboard_AddUpdateOutcomeBizActionVO();
            BizAction.OutcomeDetails = new clsIVFDashboard_OutcomeVO();
            BizAction.OutcomeDetailsList = new List<clsIVFDashboard_OutcomeVO>();
            BizAction.OutcomePregnancySacList = new List<clsIVFDashboard_OutcomeVO>();
            BizAction.OutcomeDetails.ID = ((clsIVFDashboard_OutcomeVO)this.DataContext).ID;
            BizAction.OutcomeDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.OutcomeDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;

            BizAction.OutcomeDetails.PlanTherapyID = PlanTherapyID;
            BizAction.OutcomeDetails.PlanTherapyUnitID = PlanTherapyUnitID;

            BizAction.OutcomeDetails.IsSurrogate = IsSurrogate;
            BizAction.OutcomeDetails.FreeSurrogate = FreeSurrogate;

            if (chkIsclosed.IsChecked != null)
            {
                if (chkIsclosed.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsClosed = true;
                }
            }

            if (chkIsFreeze.IsChecked != null)
            {
                if (chkIsFreeze.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsFreeze = true;
                }
            }


            ////added by neena
            for (int i = 0; i < lstFile.Count; i++)
            {
                lstFile[i].ResultListID = lstFile[i].selectedBHCGResultList.ID;
                lstFile[i].SurrogateID = lstFile[i].SelectedSurrogatePatientList.PatientID;
                lstFile[i].SurrogateUnitID = lstFile[i].SelectedSurrogatePatientList.PatientUnitID;
                lstFile[i].SurrogatePatientMrNo = lstFile[i].SelectedSurrogatePatientList.MrNo;
                BizAction.OutcomeDetailsList.Add(lstFile[i]);
            }

            PregnancySac = new clsIVFDashboard_OutcomeVO();
            if (txtNoOfSacs.Text != null)
            {
                PregnancySac.NoOfSacs = Convert.ToInt64(txtNoOfSacs.Text.Trim());
            }

            PregnancySac.SacRemarks = txtSacRemark.Text;

            if (dtObservationDate.SelectedDate != null)
            {
                PregnancySac.SacsObservationDate = dtObservationDate.SelectedDate;
            }

            if (cmbPregnancyAchieved.SelectedItem != null)
            {
                PregnancySac.PregnancyAchievedID = ((MasterListItem)cmbPregnancyAchieved.SelectedItem).ID;
            }
            if (txtID.Text != "" && txtID.Text != null)
                PregnancySac.ID = Convert.ToInt64(txtID.Text);
            if (txtUnitID.Text != "" && txtUnitID.Text != null)
                PregnancySac.UnitID = Convert.ToInt64(txtUnitID.Text);
            PregnancySac.SurrogateTypeID = 0;
            PregnancySac.SurrogateID = SurrogatePatientList[1].PatientID;
            PregnancySac.SurrogateUnitID = SurrogatePatientList[1].PatientUnitID;
            PregnancySac.SurrogatePatientMrNo = SurrogatePatientList[1].MrNo;

            if (PregnancySacsDetails != null)
            {
                if (PregnancySacsDetails.Count > 0)
                {
                    PregnancySac.PregnancySacsList = PregnancySacsDetails.ToList();
                }

                for (int i = 0; i < PregnancySacsDetails.Count; i++)
                {
                    if (PregnancySacsDetails[i].ID != 0)
                    {
                        PregnancySac.PregnancySacsList[i].ID = PregnancySacsDetails[i].ID;
                    }
                    else
                    {
                        PregnancySac.PregnancySacsList[i].ID = 0;
                    }
                    PregnancySac.PregnancySacsList[i].ResultListID = PregnancySacsDetails[i].selectedResultList.ID;
                    PregnancySac.PregnancySacsList[i].PregnancyListID = PregnancySacsDetails[i].selectedPregnancyList.ID;

                }
            }
            BizAction.OutcomePregnancySacList.Add(PregnancySac);



            PregnancySac = new clsIVFDashboard_OutcomeVO();
            if (txtNoOfSacsSurrogate1.Text != null)
            {
                PregnancySac.NoOfSacs = Convert.ToInt64(txtNoOfSacsSurrogate1.Text.Trim());
            }

            PregnancySac.SacRemarks = txtSacRemarkSurrogate1.Text;

            if (dtObservationDateSurrogate1.SelectedDate != null)
            {
                PregnancySac.SacsObservationDate = dtObservationDateSurrogate1.SelectedDate;
            }

            if (cmbPregnancyAchievedSurrogate1.SelectedItem != null)
            {
                PregnancySac.PregnancyAchievedID = ((MasterListItem)cmbPregnancyAchievedSurrogate1.SelectedItem).ID;
            }
            if (txtSurrogate1ID.Text != "" && txtSurrogate1ID.Text != null)
                PregnancySac.ID = Convert.ToInt64(txtSurrogate1ID.Text);
            if (txtSurrogate1UnitID.Text != "" && txtSurrogate1UnitID.Text != null)
                PregnancySac.UnitID = Convert.ToInt64(txtSurrogate1UnitID.Text);

            if (SurrogatePatientList.Count >= 3)
            {
                PregnancySac.SurrogateTypeID = 1;
                PregnancySac.SurrogateID = SurrogatePatientList[2].PatientID;
                PregnancySac.SurrogateUnitID = SurrogatePatientList[2].PatientUnitID;
                PregnancySac.SurrogatePatientMrNo = SurrogatePatientList[2].MrNo;
            }

            if (chkIsUnlinkSurrogate1.IsChecked != null)
            {
                if (chkIsUnlinkSurrogate1.IsChecked == true)
                {
                    PregnancySac.IsUnlinkSurrogate = true;
                }
            }

            if (PregnancySacsDetailsSurrogate1 != null)
            {
                if (PregnancySacsDetailsSurrogate1.Count > 0)
                {
                    PregnancySac.PregnancySacsList = PregnancySacsDetailsSurrogate1.ToList();
                }

                for (int i = 0; i < PregnancySacsDetailsSurrogate1.Count; i++)
                {
                    if (PregnancySacsDetailsSurrogate1[i].ID != 0)
                    {
                        PregnancySac.PregnancySacsList[i].ID = PregnancySacsDetailsSurrogate1[i].ID;
                    }
                    else
                    {
                        PregnancySac.PregnancySacsList[i].ID = 0;
                    }
                    PregnancySac.PregnancySacsList[i].ResultListID = PregnancySacsDetailsSurrogate1[i].selectedResultList.ID;
                    PregnancySac.PregnancySacsList[i].PregnancyListID = PregnancySacsDetailsSurrogate1[i].selectedPregnancyList.ID;

                }
            }
            BizAction.OutcomePregnancySacList.Add(PregnancySac);


            if (SurrogatePatientList.Count > 3)
            {

                PregnancySac = new clsIVFDashboard_OutcomeVO();
                if (txtNoOfSacsSurrogate2.Text != null)
                {
                    PregnancySac.NoOfSacs = Convert.ToInt64(txtNoOfSacsSurrogate2.Text.Trim());
                }

                PregnancySac.SacRemarks = txtSacRemarkSurrogate2.Text;

                if (dtObservationDateSurrogate2.SelectedDate != null)
                {
                    PregnancySac.SacsObservationDate = dtObservationDateSurrogate2.SelectedDate;
                }

                if (cmbPregnancyAchievedSurrogate2.SelectedItem != null)
                {
                    PregnancySac.PregnancyAchievedID = ((MasterListItem)cmbPregnancyAchievedSurrogate2.SelectedItem).ID;
                }
                if (txtSurrogate2ID.Text != "" && txtSurrogate2ID.Text != null)
                    PregnancySac.ID = Convert.ToInt64(txtSurrogate2ID.Text);
                if (txtSurrogate2UnitID.Text != "" && txtSurrogate2UnitID.Text != null)
                    PregnancySac.UnitID = Convert.ToInt64(txtSurrogate2UnitID.Text);
                if (SurrogatePatientList.Count >= 4)
                {
                    PregnancySac.SurrogateTypeID = 2;
                    PregnancySac.SurrogateID = SurrogatePatientList[3].PatientID;
                    PregnancySac.SurrogateUnitID = SurrogatePatientList[3].PatientUnitID;
                    PregnancySac.SurrogatePatientMrNo = SurrogatePatientList[3].MrNo;
                }
                if (chkIsUnlinkSurrogate2.IsChecked != null)
                {
                    if (chkIsUnlinkSurrogate2.IsChecked == true)
                    {
                        PregnancySac.IsUnlinkSurrogate = true;
                    }
                }
                if (PregnancySacsDetailsSurrogate2 != null)
                {
                    if (PregnancySacsDetailsSurrogate2.Count > 0)
                    {
                        PregnancySac.PregnancySacsList = PregnancySacsDetailsSurrogate2.ToList();
                    }

                    for (int i = 0; i < PregnancySacsDetailsSurrogate2.Count; i++)
                    {
                        if (PregnancySacsDetailsSurrogate2[i].ID != 0)
                        {
                            PregnancySac.PregnancySacsList[i].ID = PregnancySacsDetailsSurrogate2[i].ID;
                        }
                        else
                        {
                            PregnancySac.PregnancySacsList[i].ID = 0;
                        }
                        PregnancySac.PregnancySacsList[i].ResultListID = PregnancySacsDetailsSurrogate2[i].selectedResultList.ID;
                        PregnancySac.PregnancySacsList[i].PregnancyListID = PregnancySacsDetailsSurrogate2[i].selectedPregnancyList.ID;

                    }
                }
                BizAction.OutcomePregnancySacList.Add(PregnancySac);

            }





            //




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Outcome Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    fillOutcomeDetails();
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

        private bool validateOutcome()
        {
            bool result = true;


            //if (rdoPregnancyAchievedYes.IsChecked == true)
            //{
            //    if (string.IsNullOrEmpty(dtpPregnancy.Text))
            //    {
            //        dtpPregnancy.SetValidation("Pregnancy Confirm Date Is Required");
            //        dtpPregnancy.RaiseValidationError();
            //        dtpPregnancy.Focus();
            //        result = false;
            //    }
            //}
            //else
            //{
            //    dtpPregnancy.ClearValidationError();
            //}
            return result;
        }
        #endregion

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            clsIVFDashboard_OutcomeVO Obj = new clsIVFDashboard_OutcomeVO();
            Obj.BHCGResultListVO = BHCGResultList;
            Obj.SurrogatePatientList = SurrogatePatientList;

            if ((Obj.ResultListID) >= 0)
            {
                if (Convert.ToInt64(Obj.ResultListID) > 0)
                {
                    Obj.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(Obj.ResultListID));
                }
                else
                {
                    Obj.selectedBHCGResultList = BHCGResultList.FirstOrDefault(p => p.ID == 0);
                }
            }

            if ((Obj.SurrogateID) > 0)
            {
                Obj.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == Obj.SurrogateID);
            }
            else
            {
                Obj.SelectedSurrogatePatientList = SurrogatePatientList.FirstOrDefault(p => p.PatientID == 0);
            }
            lstFile.Add(Obj);
            dgBHCGDetailsGrid.ItemsSource = lstFile;
        }

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgBHCGDetailsGrid.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected details ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        lstFile.RemoveAt(dgBHCGDetailsGrid.SelectedIndex);
                        dgBHCGDetailsGrid.ItemsSource = null;
                        dgBHCGDetailsGrid.ItemsSource = lstFile;
                        dgBHCGDetailsGrid.UpdateLayout();
                        dgBHCGDetailsGrid.Focus();
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Record Deleted Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                    }
                };
                msgWD.Show();
            }
        }

        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacsDetails = null;
        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacList = null;
        int cnt = 0;

        private void txtNoOfSacs_TextChanged(object sender, TextChangedEventArgs e)
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
            else
            {
                if (blSacsNo != true)
                {
                    if (txtNoOfSacs.Text != string.Empty && txtNoOfSacs.Text != "0")
                    {
                        //dgSacsDetailsGrid.ItemsSource = null;

                        if (dgSacsDetailsGrid.ItemsSource != null)
                        {
                            PregnancySacList = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            PregnancySacList = PregnancySacsDetails;
                            if (PregnancySacList.Count < Convert.ToInt32(txtNoOfSacs.Text))
                            {
                                for (int i = PregnancySacList.Count; i < Convert.ToInt32(txtNoOfSacs.Text); i++)
                                {
                                    cnt = i + 1;
                                    PregnancySacsDetails.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cnt,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });
                                }
                                dgSacsDetailsGrid.ItemsSource = PregnancySacList;
                                PregnancySacsDetails = PregnancySacList;
                            }
                            else if (PregnancySacList.Count > Convert.ToInt32(txtNoOfSacs.Text))
                            {
                                PregnancySacsDetails = new ObservableCollection<clsPregnancySacsDetailsVO>();
                                for (int i = 0; i < Convert.ToInt32(txtNoOfSacs.Text); i++)
                                {
                                    cnt = i + 1;
                                    PregnancySacsDetails.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cnt,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });

                                }
                                dgSacsDetailsGrid.ItemsSource = null;
                                dgSacsDetailsGrid.ItemsSource = PregnancySacsDetails;
                            }

                        }
                        else
                        {
                            PregnancySacsDetails = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            for (int i = 0; i < Convert.ToInt32(txtNoOfSacs.Text); i++)
                            {
                                cnt = i + 1;
                                PregnancySacsDetails.Add(new clsPregnancySacsDetailsVO()
                                {
                                    SaceNoStr = "Sac " + cnt,
                                    ResultListID = 0,
                                    OutcomeResultListVO = OutcomeResultList,
                                    PregnancyListID = 0,
                                    OutcomePregnancyListVO = OutcomePregnancyList,
                                });

                            }
                            dgSacsDetailsGrid.ItemsSource = PregnancySacsDetails;
                            //SacGrid.Visibility = Visibility.Visible;
                        }
                    }

                }

            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNoOfSacs_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        int FetalHeartCnt = 0;

        private void ChkFetalHeart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PregnancySacsDetails)
            {
                if (item.IsFetalHeart == true)
                    FetalHeartCnt = FetalHeartCnt + 1;
            }

            if (FetalHeartCnt > 0)
            {
                PregnacncyAchievedGrid.Visibility = Visibility.Visible;
                cmbPregnancyAchieved.SelectedValue = (long)FetalHeartCnt;
                FetalHeartCnt = 0;
            }
            else
                PregnacncyAchievedGrid.Visibility = Visibility.Collapsed;
        }


        FrameworkElement element;
        DataGridRow row;
        TextBox TxtCongenitalAbnormality;

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
       where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        private void CongenitalAbnormalityYes_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGrid.SelectedItem != null)
            {
                element = dgSacsDetailsGrid.Columns.Last().GetCellContent(dgSacsDetailsGrid.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = true;
            }
        }

        private void CongenitalAbnormalityNo_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGrid.SelectedItem != null)
            {
                element = dgSacsDetailsGrid.Columns.Last().GetCellContent(dgSacsDetailsGrid.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = false;
            }
        }

        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacsDetailsSurrogate1 = null;
        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacListSurrogate1 = null;
        int cntSurrogate1 = 0;
        bool blSacsNoSurrogate1 = false;

        private void txtNoOfSacsSurrogate1_TextChanged(object sender, TextChangedEventArgs e)
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
            else
            {
                if (blSacsNoSurrogate1 != true)
                {
                    if (txtNoOfSacsSurrogate1.Text != string.Empty && txtNoOfSacsSurrogate1.Text != "0")
                    {
                        //dgSacsDetailsGrid.ItemsSource = null;

                        if (dgSacsDetailsGridSurrogate1.ItemsSource != null)
                        {
                            PregnancySacListSurrogate1 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            PregnancySacListSurrogate1 = PregnancySacsDetailsSurrogate1;
                            if (PregnancySacListSurrogate1.Count < Convert.ToInt32(txtNoOfSacsSurrogate1.Text))
                            {
                                for (int i = PregnancySacListSurrogate1.Count; i < Convert.ToInt32(txtNoOfSacsSurrogate1.Text); i++)
                                {
                                    cntSurrogate1 = i + 1;
                                    PregnancySacsDetailsSurrogate1.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cntSurrogate1,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });
                                }
                                dgSacsDetailsGridSurrogate1.ItemsSource = PregnancySacListSurrogate1;
                                PregnancySacsDetailsSurrogate1 = PregnancySacListSurrogate1;
                            }
                            else if (PregnancySacListSurrogate1.Count > Convert.ToInt32(txtNoOfSacsSurrogate1.Text))
                            {
                                PregnancySacsDetailsSurrogate1 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                                for (int i = 0; i < Convert.ToInt32(txtNoOfSacsSurrogate1.Text); i++)
                                {
                                    cntSurrogate1 = i + 1;
                                    PregnancySacsDetailsSurrogate1.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cntSurrogate1,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });

                                }
                                dgSacsDetailsGridSurrogate1.ItemsSource = null;
                                dgSacsDetailsGridSurrogate1.ItemsSource = PregnancySacsDetailsSurrogate1;
                            }

                        }
                        else
                        {
                            PregnancySacsDetailsSurrogate1 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            for (int i = 0; i < Convert.ToInt32(txtNoOfSacsSurrogate1.Text); i++)
                            {
                                cntSurrogate1 = i + 1;
                                PregnancySacsDetailsSurrogate1.Add(new clsPregnancySacsDetailsVO()
                                {
                                    SaceNoStr = "Sac " + cntSurrogate1,
                                    ResultListID = 0,
                                    OutcomeResultListVO = OutcomeResultList,
                                    PregnancyListID = 0,
                                    OutcomePregnancyListVO = OutcomePregnancyList,
                                });

                            }
                            dgSacsDetailsGridSurrogate1.ItemsSource = PregnancySacsDetailsSurrogate1;
                            //SacGrid.Visibility = Visibility.Visible;
                        }
                    }

                }

            }
        }

        int FetalHeartCntSurrogate1 = 0;
        private void ChkFetalHeartSurrogate1_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PregnancySacsDetailsSurrogate1)
            {
                if (item.IsFetalHeart == true)
                    FetalHeartCntSurrogate1 = FetalHeartCntSurrogate1 + 1;
            }

            if (FetalHeartCntSurrogate1 > 0)
            {
                //PregnacncyAchievedGridSurrogate1.Visibility = Visibility.Visible;
                txbPregnancyAchievedSurrogate1.Visibility = Visibility.Visible;
                cmbPregnancyAchievedSurrogate1.Visibility = Visibility.Visible;
                cmbPregnancyAchievedSurrogate1.SelectedValue = (long)FetalHeartCntSurrogate1;
                FetalHeartCntSurrogate1 = 0;
            }
            else
            {
                // PregnacncyAchievedGridSurrogate1.Visibility = Visibility.Collapsed;
                txbPregnancyAchievedSurrogate1.Visibility = Visibility.Collapsed;
                cmbPregnancyAchievedSurrogate1.Visibility = Visibility.Collapsed;
            }
        }

        private void CongenitalAbnormalitySurrogate1Yes_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGridSurrogate1.SelectedItem != null)
            {
                element = dgSacsDetailsGridSurrogate1.Columns.Last().GetCellContent(dgSacsDetailsGridSurrogate1.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = true;
            }
        }

        private void CongenitalAbnormalitySurrogate1No_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGridSurrogate1.SelectedItem != null)
            {
                element = dgSacsDetailsGridSurrogate1.Columns.Last().GetCellContent(dgSacsDetailsGridSurrogate1.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = false;
            }
        }

        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacsDetailsSurrogate2 = null;
        ObservableCollection<clsPregnancySacsDetailsVO> PregnancySacListSurrogate2 = null;
        int cntSurrogate2 = 0;
        bool blSacsNoSurrogate2 = false;
        int FetalHeartCntSurrogate2 = 0;

        private void txtNoOfSacsSurrogate2_TextChanged(object sender, TextChangedEventArgs e)
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
            else
            {
                if (blSacsNoSurrogate2 != true)
                {
                    if (txtNoOfSacsSurrogate2.Text != string.Empty && txtNoOfSacsSurrogate2.Text != "0")
                    {
                        //dgSacsDetailsGrid.ItemsSource = null;

                        if (dgSacsDetailsGridSurrogate2.ItemsSource != null)
                        {
                            PregnancySacListSurrogate2 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            PregnancySacListSurrogate2 = PregnancySacsDetailsSurrogate2;
                            if (PregnancySacListSurrogate2.Count < Convert.ToInt32(txtNoOfSacsSurrogate2.Text))
                            {
                                for (int i = PregnancySacListSurrogate2.Count; i < Convert.ToInt32(txtNoOfSacsSurrogate2.Text); i++)
                                {
                                    cntSurrogate2 = i + 1;
                                    PregnancySacsDetailsSurrogate2.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cntSurrogate2,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });
                                }
                                dgSacsDetailsGridSurrogate2.ItemsSource = PregnancySacListSurrogate2;
                                PregnancySacsDetailsSurrogate2 = PregnancySacListSurrogate2;
                            }
                            else if (PregnancySacListSurrogate2.Count > Convert.ToInt32(txtNoOfSacsSurrogate2.Text))
                            {
                                PregnancySacsDetailsSurrogate2 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                                for (int i = 0; i < Convert.ToInt32(txtNoOfSacsSurrogate2.Text); i++)
                                {
                                    cntSurrogate2 = i + 1;
                                    PregnancySacsDetailsSurrogate2.Add(new clsPregnancySacsDetailsVO()
                                    {
                                        SaceNoStr = "Sac " + cntSurrogate2,
                                        ResultListID = 0,
                                        OutcomeResultListVO = OutcomeResultList,
                                        PregnancyListID = 0,
                                        OutcomePregnancyListVO = OutcomePregnancyList,
                                    });

                                }
                                dgSacsDetailsGridSurrogate2.ItemsSource = null;
                                dgSacsDetailsGridSurrogate2.ItemsSource = PregnancySacsDetailsSurrogate2;
                            }

                        }
                        else
                        {
                            PregnancySacsDetailsSurrogate2 = new ObservableCollection<clsPregnancySacsDetailsVO>();
                            for (int i = 0; i < Convert.ToInt32(txtNoOfSacsSurrogate2.Text); i++)
                            {
                                cntSurrogate2 = i + 1;
                                PregnancySacsDetailsSurrogate2.Add(new clsPregnancySacsDetailsVO()
                                {
                                    SaceNoStr = "Sac " + cntSurrogate2,
                                    ResultListID = 0,
                                    OutcomeResultListVO = OutcomeResultList,
                                    PregnancyListID = 0,
                                    OutcomePregnancyListVO = OutcomePregnancyList,
                                });

                            }
                            dgSacsDetailsGridSurrogate2.ItemsSource = PregnancySacsDetailsSurrogate2;
                            //SacGrid.Visibility = Visibility.Visible;
                        }
                    }

                }

            }
        }

        private void ChkFetalHeartSurrogate2_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PregnancySacsDetailsSurrogate2)
            {
                if (item.IsFetalHeart == true)
                    FetalHeartCntSurrogate2 = FetalHeartCntSurrogate2 + 1;
            }

            if (FetalHeartCntSurrogate2 > 0)
            {
                //PregnacncyAchievedGridSurrogate2.Visibility = Visibility.Visible;
                txbPregnancyAchievedSurrogate2.Visibility = Visibility.Visible;
                cmbPregnancyAchievedSurrogate2.Visibility = Visibility.Visible;
                cmbPregnancyAchievedSurrogate2.SelectedValue = (long)FetalHeartCntSurrogate2;
                FetalHeartCntSurrogate2 = 0;
            }
            else
            {
                // PregnacncyAchievedGridSurrogate2.Visibility = Visibility.Collapsed;
                txbPregnancyAchievedSurrogate2.Visibility = Visibility.Collapsed;
                cmbPregnancyAchievedSurrogate2.Visibility = Visibility.Collapsed;
            }
        }

        private void CongenitalAbnormalitySurrogate2Yes_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGridSurrogate2.SelectedItem != null)
            {
                element = dgSacsDetailsGridSurrogate2.Columns.Last().GetCellContent(dgSacsDetailsGridSurrogate2.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = true;
            }
        }

        private void CongenitalAbnormalitySurrogate2No_Click(object sender, RoutedEventArgs e)
        {
            if (dgSacsDetailsGridSurrogate2.SelectedItem != null)
            {
                element = dgSacsDetailsGridSurrogate2.Columns.Last().GetCellContent(dgSacsDetailsGridSurrogate2.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtCongenitalAbnormality = FindVisualChild<TextBox>(row, "txtCongenitalAbnormality");
                TxtCongenitalAbnormality.IsEnabled = false;
            }
        }

        private void chkIsFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsFreeze.IsChecked == true)
            {
                if (SurrogatePatientList.Count == 3)
                {
                    if (chkIsUnlinkSurrogate1.IsChecked == false)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Do you want to freeze details without unlink the surrogate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        msgbx.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgbx_OnMessageBoxClosed);
                        msgbx.Show();
                    }
                }
                else if (SurrogatePatientList.Count > 3)
                {
                    if (chkIsUnlinkSurrogate1.IsChecked == false || chkIsUnlinkSurrogate2.IsChecked == false)
                    {
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Do you want to freeze details without unlink the surrogate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        msgbx.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgbx_OnMessageBoxClosed);
                        msgbx.Show();
                    }
                }
            }
        }

        void msgbx_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                chkIsFreeze.IsChecked = true;
            else if (result == MessageBoxResult.No)
                chkIsFreeze.IsChecked = false;
        }

        public DateTime? ETDate = null;
        bool FreeSurrogate = false;
        private void chkIsclosed_Click(object sender, RoutedEventArgs e)
        {
            if (ETDate != null && chkIsclosed.IsChecked == true)
            {
                DateTime SurrogateStartDate = ETDate.Value.AddDays(-15);
                DateTime SurrogateUnlickDate = SurrogateStartDate.AddMonths(9);

                if (SurrogateUnlickDate > DateTime.Today && chkIsFreeze.IsChecked == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "To unlink the surrogate 9 months are not completed yet.. please unlink the surrogate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                    FreeSurrogate = true;
            }

        }
    }
}
