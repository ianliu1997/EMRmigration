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
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using System.Reflection;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmGraphicalRepresentationofLabDaysWithoutIstambul : UserControl
    {
        long EmbryologistID = 0;
        long AssiEmbryologistID = 0;
        public DateTime StartDate;
        DateTime DisplayDate;
        public long PlannedNoOfEmb;
        long Anethetist = 0;
        public bool ISSetForED;
        public long OocyteforED;
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long PlannedTreatmentID;
        public long SourceOfSperm;
        public DateTime? Triggerdate;
        public DateTime? TriggerTime;
        public bool IsDonorCycle = false;
        public bool IsIsthambul;
        public bool IsClosed;

        WaitIndicator wait = new WaitIndicator();

        public frmGraphicalRepresentationofLabDaysWithoutIstambul()
        {
            InitializeComponent();
            this.DataContext = null;
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
                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ID > 0)
                        {
                            if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                            {
                                // this.DataContext = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details;
                                StartDate = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Date.Value.Date;
                                PlannedNoOfEmb = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteRetrived;
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsSetForED != null)
                                    ISSetForED = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsSetForED;
                                OocyteforED = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteForED;
                                EmbryologistID = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.EmbryologistID;
                                AssiEmbryologistID = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                                Anethetist = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AnesthetistID;
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                {
                                    //AddOocList();
                                    GetDonordetails();
                                    dgDayList.IsEnabled = true;
                                    Cmdmedia.IsEnabled = true;
                                    cmdCryoPreservation.IsEnabled = true;
                                    //txtNoOocyteRetrieved.IsEnabled = true;
                                    //hlbOocyteNoSave.IsEnabled = true;
                                }
                                else
                                {
                                    string msgTitle = "Palash";
                                    string msgText = "You Cannot Go For Embryology Work \n Until You Freeze The OPU. ";
                                    MessageBoxControl.MessageBoxChildWindow msgWin =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                                    msgWin.Show();

                                    // dgDayList.IsEnabled = false;
                                    Cmdmedia.IsEnabled = false;
                                    //  cmdCryoPreservation.IsEnabled = false;
                                    txtNoOocyteRetrieved.IsEnabled = false;
                                    hlbOocyteNoSave.IsEnabled = false;
                                }
                            }
                        }
                        else
                        {
                            string msgTitle = "Palash";
                            string msgText = "You Cannot Go For Embryology Work \n Until You Fill The OPU Details. ";
                            MessageBoxControl.MessageBoxChildWindow msgWin =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgWin.Show();

                            //  dgDayList.IsEnabled = false;
                            Cmdmedia.IsEnabled = false;
                            //  cmdCryoPreservation.IsEnabled = false;
                            txtNoOocyteRetrieved.IsEnabled = false;
                            hlbOocyteNoSave.IsEnabled = false;
                        }
                    }
                    wait.Close();

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        private void AddOocList()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_AddDay0OocyteListBizActionVO BizAction = new clsIVFDashboard_AddDay0OocyteListBizActionVO();
                BizAction.Details = new clsIVFDashboard_LabDaysVO();
                BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Details.PlanTherapyID = PlanTherapyID;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
                if (ISSetForED == true)
                    BizAction.Details.PlannedNoOfEmb = OocyteforED;
                else
                    BizAction.Details.PlannedNoOfEmb = PlannedNoOfEmb;

                BizAction.Details.OocyteDonorID = OocyteDonorID;
                BizAction.Details.OocyteDonorUnitID = OocyteDonorUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        fillGrid();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillEmbryologySummary();
            fillDecision();
            dtTriggerDate.SelectedDate = Triggerdate;
            dtTriggerTime.Value = TriggerTime;


            if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID)
            {
                txtNoOocyteRetrieved.IsEnabled = false;
                hlbOocyteNoSave.IsEnabled = false;
                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsDonorCycle)
                if (IsDonorCycle)
                    GetOocyteEmbryoFromDonoteCycle();
                else
                    GetDonarOocyteEmbryo();
            }
            else if (PlannedTreatmentID == 5)
                fillGrid();
            else
            {
                fillOPUDetails();
            }

            //if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID)
            //{
            //    GetOocyteFromDonor();
            //}
            //else if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.EmbryoReceipentID)
            //{
            //    fillGrid();
            //}
            //else
            //{
            //    fillOPUDetails();
            //}

        }

        private void GetOocyteEmbryoFromDonoteCycle()
        {
            try
            {
                clsGetDetailsOfReceivedOocyteEmbryoBizActionVO BizAction = new clsGetDetailsOfReceivedOocyteEmbryoBizActionVO();
                BizAction.Details = new clsReceiveOocyteVO();
                BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Details.TherapyID = PlanTherapyID;
                BizAction.Details.TherapyUnitID = PlanTherapyUnitID;
                BizAction.Details.IsDonorCycle = IsDonorCycle;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetDetailsOfReceivedOocyteEmbryoBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            fillGrid();
                        }
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void GetDonarOocyteEmbryo()
        {
            try
            {
                clsGetDetailsOfReceivedOocyteEmbryoBizActionVO BizAction = new clsGetDetailsOfReceivedOocyteEmbryoBizActionVO();
                BizAction.Details = new clsReceiveOocyteVO();
                BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Details.TherapyID = PlanTherapyID;
                BizAction.Details.TherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetDetailsOfReceivedOocyteEmbryoBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            fillGrid();
                        }
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }


        private Button AddRemoveClick;
        public event RoutedEventHandler OnAddRemoveClick;

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = child.Parent;
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;
        }


        //string Day = string.Empty;
        //long DayNo;
        //long GradeId;
        //long StageofDevelopmentGrade;
        //long InnerCellMassGrade;
        //long TrophoectodermGrade;
        //string CellStage;


        public List<cls_IVFDashboard_GraphicalRepresentationVO> GraphicalOocDetailsList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
        private void fillGrid()
        {
            try
            {
                cls_IVFDashboar_GetGraphicalRepBizActionVO BizAction = new cls_IVFDashboar_GetGraphicalRepBizActionVO();
                BizAction.Details = new cls_IVFDashboard_GraphicalRepresentationVO();
                BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Details.PlanTherapyID = PlanTherapyID;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList != null)
                        {
                            if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Count > 0)
                            {
                                DisplayDate = StartDate;
                                //for (int i = 1; i < dgDayList.Columns.Count; i++)
                                //{
                                //    dgDayList.Columns[i].Header = "Day" + (i - 1) + "(" + DisplayDate.ToString("dd/MM/yyyy") + ")";
                                //    DisplayDate = DisplayDate.AddDays(1);
                                //    // DataGridColumn column = dgDayList.Columns[i];
                                //    // FrameworkElement fe = column.GetCellContent(i);
                                //    //// FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                //    //if (result != null)
                                //    //{
                                //    //    DataGridCell cell = (DataGridCell)result;
                                //    //    cell.IsEnabled = false;

                                //    //}
                                //}

                                foreach (var item in ((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.ToList())
                                {
                                    if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                                    {
                                        var results = from r in mlDecision
                                                      where r.ID != 2 && r.ID != 3
                                                      select r;
                                        mlDecision = results.ToList();
                                    }
                                    else
                                    {
                                        var results = from r in mlDecision
                                                      where r.ID != 4 && r.ID != 5
                                                      select r;
                                        mlDecision = results.ToList();
                                    }

                                    item.Decision = mlDecision;
                                    if (item.FertCheck == 1 && item.IsFreezed)
                                    {
                                        if (item.DecisionID > 0)
                                        {
                                            item.IsAddObservation = false;
                                            item.IsDecision = false;
                                        }
                                        else
                                        {
                                            dgDayList.Columns[11].Visibility = Visibility.Visible;
                                            item.IsAddObservation = true;
                                        }
                                        //item.IsDecision = true;                                       
                                    }

                                    if (item.Day1 == true)
                                    {
                                        dgDayList.Columns[4].Visibility = Visibility.Visible;//3
                                        if (item.IsLabDay1Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 1";
                                            item.DayNo = 1;
                                            item.GradeID = item.GradeIDDay1;
                                            item.CellStage = item.CellStageDay1;
                                            item.PlanDate = item.ObservationDate1.Value.Add(item.ObservationTime1.Value.TimeOfDay);
                                            //Day = "Day 1";
                                            //DayNo = 1;
                                            //GradeId = item.GradeIDDay1;
                                            //CellStage = item.CellStageDay1;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day2 == true)
                                    {
                                        dgDayList.Columns[5].Visibility = Visibility.Visible;
                                        if (item.IsLabDay2Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 2";
                                            item.DayNo = 2;
                                            item.GradeID = item.GradeIDDay2;
                                            item.CellStage = item.CellStageDay2;
                                            item.PlanDate = item.ObservationDate2.Value.Add(item.ObservationTime2.Value.TimeOfDay);
                                            //Day = "Day 2";
                                            //DayNo = 2;
                                            //GradeId = item.GradeIDDay2;
                                            //CellStage = item.CellStageDay2;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day3 == true)
                                    {
                                        dgDayList.Columns[6].Visibility = Visibility.Visible;
                                        if (item.IsLabDay3Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 3";
                                            item.DayNo = 3;
                                            item.GradeID = item.GradeIDDay3;
                                            item.CellStage = item.CellStageDay3;
                                            item.PlanDate = item.ObservationDate3.Value.Add(item.ObservationTime3.Value.TimeOfDay);
                                            //Day = "Day 3";
                                            //DayNo = 3;
                                            //GradeId = item.GradeIDDay3;
                                            //CellStage = item.CellStageDay3;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day4 == true)
                                    {
                                        dgDayList.Columns[7].Visibility = Visibility.Visible;
                                        if (item.IsLabDay4Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 4";
                                            item.DayNo = 4;
                                            item.GradeID = item.GradeIDDay4;
                                            item.CellStage = item.CellStageDay4;
                                            item.PlanDate = item.ObservationDate4.Value.Add(item.ObservationTime4.Value.TimeOfDay);
                                            //Day = "Day 4";
                                            //DayNo = 4;
                                            //GradeId = item.GradeIDDay4;
                                            //CellStage = item.CellStageDay4;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day5 == true)
                                    {
                                        dgDayList.Columns[8].Visibility = Visibility.Visible;
                                        if (item.IsLabDay5Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 5";
                                            item.DayNo = 5;
                                            item.GradeID = 0;
                                            item.StageofDevelopmentGrade = item.StageofDevelopmentGradeDay5;
                                            item.InnerCellMassGrade = item.InnerCellMassGradeDay5;
                                            item.TrophoectodermGrade = item.TrophoectodermGradeDay5;
                                            item.CellStage = item.CellStageDay5;
                                            item.PlanDate = item.ObservationDate5.Value.Add(item.ObservationTime5.Value.TimeOfDay);
                                            //Day = "Day 5";
                                            //DayNo = 5;
                                            //GradeId = 0;
                                            //StageofDevelopmentGrade = item.StageofDevelopmentGradeDay5;
                                            //InnerCellMassGrade = item.InnerCellMassGradeDay5;
                                            //TrophoectodermGrade = item.TrophoectodermGradeDay5;
                                            //CellStage = item.CellStageDay5;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day6 == true)
                                    {
                                        dgDayList.Columns[9].Visibility = Visibility.Visible;
                                        if (item.IsLabDay6Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 6";
                                            item.DayNo = 6;
                                            item.GradeID = 0;
                                            item.StageofDevelopmentGrade = item.StageofDevelopmentGradeDay6;
                                            item.InnerCellMassGrade = item.InnerCellMassGradeDay6;
                                            item.TrophoectodermGrade = item.TrophoectodermGradeDay6;
                                            item.CellStage = item.CellStageDay6;
                                            item.PlanDate = item.ObservationDate6.Value.Add(item.ObservationTime6.Value.TimeOfDay);
                                            //Day = "Day 6";
                                            //DayNo = 6;
                                            //GradeId = 0;
                                            //StageofDevelopmentGrade = item.StageofDevelopmentGradeDay6;
                                            //InnerCellMassGrade = item.InnerCellMassGradeDay6;
                                            //TrophoectodermGrade = item.TrophoectodermGradeDay6;
                                            //CellStage = item.CellStageDay6;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }

                                    if (item.Day7 == true)
                                    {
                                        dgDayList.Columns[10].Visibility = Visibility.Visible;
                                        dgDayList.Columns[11].Visibility = Visibility.Collapsed;
                                        item.IsAddObservation = false;
                                        if (item.IsLabDay7Freezed)
                                        {
                                            item.IsAddObservation = true;
                                            item.IsDecision = true;
                                            item.Day = "Day 7";
                                            item.DayNo = 7;
                                            item.GradeID = 0;
                                            item.StageofDevelopmentGrade = item.StageofDevelopmentGradeDay7;
                                            item.InnerCellMassGrade = item.InnerCellMassGradeDay7;
                                            item.TrophoectodermGrade = item.TrophoectodermGradeDay7;
                                            item.CellStage = item.CellStageDay7;
                                            item.PlanDate = item.ObservationDate7.Value.Add(item.ObservationTime7.Value.TimeOfDay);
                                            //Day = "Day 6";
                                            //DayNo = 6;
                                            //GradeId = 0;
                                            //StageofDevelopmentGrade = item.StageofDevelopmentGradeDay6;
                                            //InnerCellMassGrade = item.InnerCellMassGradeDay6;
                                            //TrophoectodermGrade = item.TrophoectodermGradeDay6;
                                            //CellStage = item.CellStageDay6;
                                        }
                                        else
                                            item.IsAddObservation = false;
                                    }
                                    else
                                        dgDayList.Columns[11].Visibility = Visibility.Visible;

                                    //if (item.Day1Visible)
                                    //    dgDayList.Columns[4].Visibility = Visibility.Visible;//3
                                    //if (item.Day2Visible)
                                    //    dgDayList.Columns[5].Visibility = Visibility.Visible;
                                    //if (item.Day3Visible)
                                    //    dgDayList.Columns[6].Visibility = Visibility.Visible;
                                    //if (item.Day4Visible)
                                    //    dgDayList.Columns[7].Visibility = Visibility.Visible;
                                    //if (item.Day5Visible)
                                    //    dgDayList.Columns[8].Visibility = Visibility.Visible;
                                    //if (item.Day6Visible)
                                    //    dgDayList.Columns[9].Visibility = Visibility.Visible;
                                    //if (item.Day7Visible)
                                    //{
                                    //    dgDayList.Columns[10].Visibility = Visibility.Visible;
                                    //    dgDayList.Columns[11].Visibility = Visibility.Collapsed;
                                    //    item.IsAddObservation = false;
                                    //}
                                    //else
                                    //    dgDayList.Columns[11].Visibility = Visibility.Visible;

                                    //if (item.Day6Visible)
                                    //{
                                    //    dgDayList.Columns[9].Visibility = Visibility.Visible;
                                    //    dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                    //    item.IsAddObservation = false;
                                    //}
                                    //else
                                    //    dgDayList.Columns[10].Visibility = Visibility.Visible;                                

                                    //if (item.Day5Visible)
                                    //{
                                    //    dgDayList.Columns[8].Visibility = Visibility.Visible;
                                    //    dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                    //    item.IsAddObservation = false;
                                    //}
                                    //else
                                    //    dgDayList.Columns[10].Visibility = Visibility.Visible;


                                    item.Decision = mlDecision;
                                    if (item.DecisionID >= 0)
                                    {
                                        if (Convert.ToInt64(item.DecisionID) > 0)
                                        {
                                            item.IsAddObservation = false;
                                            item.IsDecision = false;
                                            dgDayList.Columns[13].Visibility = Visibility.Visible;
                                            item.IsFreezedecision = false;
                                            item.SelectedDecision = mlDecision.FirstOrDefault(p => p.ID == Convert.ToInt64(item.DecisionID));
                                        }
                                        else
                                        {
                                            item.SelectedDecision = mlDecision.FirstOrDefault(p => p.ID == 0);
                                        }
                                        if (Convert.ToInt64(item.DecisionID) == 4 || Convert.ToInt64(item.DecisionID) == 5)
                                        {
                                            if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                                            {
                                                dgDayList.Columns[16].Visibility = Visibility.Visible;
                                                dgDayList.Columns[17].Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                dgDayList.Columns[15].Visibility = Visibility.Visible;
                                                dgDayList.Columns[16].Visibility = Visibility.Visible;
                                                dgDayList.Columns[17].Visibility = Visibility.Visible;
                                                item.IsSelectPatient = false;
                                            }
                                        }
                                    }

                                    if (item.IsExtendedCulture == true)
                                    {
                                        dgDayList.Columns[0].Visibility = Visibility.Visible;
                                        dgDayList.Columns[14].Visibility = Visibility.Visible;
                                        item.IsExtendedCultureForBoth = "/PalashDynamics;component/Icons/yellow.png";
                                    }
                                    else if (item.IsExtendedCultureFromOtherCycle == true)
                                    {
                                        dgDayList.Columns[0].Visibility = Visibility.Visible;
                                        dgDayList.Columns[14].Visibility = Visibility.Visible;
                                        item.IsExtendedCultureForBoth = "/PalashDynamics;component/Icons/green.png";
                                    }

                                    if (item.EmbryoDayObservation.Count > 1)
                                    {
                                        var maxSample = item.EmbryoDayObservation.Where(x => x.ServerDate == item.EmbryoDayObservation.Max(x1 => x1.ServerDate)).FirstOrDefault();
                                        bool Day1Visibillity = false;
                                        bool Day2Visibillity = false;
                                        bool Day3Visibillity = false;
                                        bool Day4Visibillity = false;
                                        bool Day5Visibillity = false;
                                        bool Day6Visibillity = false;
                                        bool Day7Visibillity = false;

                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay1Freezed == true))
                                            Day1Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay2Freezed == true))
                                            Day2Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay3Freezed == true))
                                            Day3Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay4Freezed == true))
                                            Day4Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay5Freezed == true))
                                            Day5Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay6Freezed == true))
                                            Day6Visibillity = true;
                                        if (((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList.Any(x => x.IsLabDay7Freezed == true))
                                            Day7Visibillity = true;



                                        if (maxSample.StrDay == "Day1")
                                        {
                                            item.Day2 = false;
                                            item.Day3 = false;
                                            item.Day4 = false;
                                            item.Day5 = false;
                                            item.Day6 = false;
                                            item.Day7 = false;

                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;

                                        }
                                        if (maxSample.StrDay == "Day2")
                                        {
                                            item.Day1 = false;
                                            item.Day3 = false;
                                            item.Day4 = false;
                                            item.Day5 = false;
                                            item.Day6 = false;
                                            item.Day7 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                        }
                                        if (maxSample.StrDay == "Day3")
                                        {
                                            item.Day1 = false;
                                            item.Day2 = false;
                                            item.Day4 = false;
                                            item.Day5 = false;
                                            item.Day6 = false;
                                            item.Day7 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;
                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;
                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                        }
                                        if (maxSample.StrDay == "Day4")
                                        {
                                            item.Day1 = false;
                                            item.Day2 = false;
                                            item.Day3 = false;
                                            item.Day5 = false;
                                            item.Day6 = false;
                                            item.Day7 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;

                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                        }
                                        if (maxSample.StrDay == "Day5")
                                        {
                                            item.Day1 = false;
                                            item.Day2 = false;
                                            item.Day3 = false;
                                            item.Day4 = false;
                                            item.Day6 = false;
                                            item.Day7 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;

                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                        }
                                        if (maxSample.StrDay == "Day6")
                                        {
                                            item.Day1 = false;
                                            item.Day2 = false;
                                            item.Day3 = false;
                                            item.Day4 = false;
                                            item.Day5 = false;
                                            item.Day7 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;

                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day7Visibillity == true)
                                                dgDayList.Columns[10].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[10].Visibility = Visibility.Collapsed;
                                        }
                                        if (maxSample.StrDay == "Day7")
                                        {
                                            item.Day1 = false;
                                            item.Day2 = false;
                                            item.Day3 = false;
                                            item.Day4 = false;
                                            item.Day5 = false;
                                            item.Day6 = false;

                                            if (Day1Visibillity == true)
                                                dgDayList.Columns[4].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[4].Visibility = Visibility.Collapsed;

                                            if (Day2Visibillity == true)
                                                dgDayList.Columns[5].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[5].Visibility = Visibility.Collapsed;

                                            if (Day3Visibillity == true)
                                                dgDayList.Columns[6].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[6].Visibility = Visibility.Collapsed;

                                            if (Day4Visibillity == true)
                                                dgDayList.Columns[7].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[7].Visibility = Visibility.Collapsed;

                                            if (Day5Visibillity == true)
                                                dgDayList.Columns[8].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[8].Visibility = Visibility.Collapsed;

                                            if (Day6Visibillity == true)
                                                dgDayList.Columns[9].Visibility = Visibility.Visible;
                                            else
                                                dgDayList.Columns[9].Visibility = Visibility.Collapsed;
                                        }
                                    }
                                }

                                dgDayList.ItemsSource = null;
                                dgDayList.ItemsSource = ((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList;
                                GraphicalOocDetailsList = ((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList;
                                GetTherapyDetails();
                                //

                                //
                            }
                        }
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        //public void get()
        //{
        //    Grid grd = GetTemplateChild("dgDayList") as Grid;
        //    var collection = grd.Children;

        //    foreach (var item in collection)
        //    {
        //        AddRemoveClick = item as Button;
        //        if (AddRemoveClick != null)
        //        {
        //            this.AddRemoveClick.Click += new RoutedEventHandler(this.btnLabDay0_Click);
        //        }
        //    }
        //}

        private void dgDayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgDayList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;

            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day0 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day0Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day0CellStage);


            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day1 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day1Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day1CellStage);
            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day2 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day2Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day2CellStage);
            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day3 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day3Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day3CellStage);
            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day4 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day4Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day4CellStage);
            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day5 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day5Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day5CellStage);
            }
            if (((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day6 == true)
            {
                ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day6Image = setImages(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).Day6CellStage);
            }

            //added by neena
            ((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).FertCheckImage = setImages1(((cls_IVFDashboard_GraphicalRepresentationVO)(row.DataContext)).FertCheck);
            //

        }

        private void dgDayList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }


        private void btnLavDay1_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 1 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 1 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 1;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.FillCleavageGrade();
            //winFert.fillDetailsLabDay1();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.Show();

            //frmLabDay1 Day1frm = new frmLabDay1();
            //Day1frm.Title = "Embryology - Lab Day 1 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day1frm.Title = "IVF Lab Day 1(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day1frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day1frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day1frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day1frm.CoupleDetails = CoupleDetails;
            //Day1frm.date = StartDate.AddDays(1);
            //Day1frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day1frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day1frm.embryologistID = EmbryologistID;
            //Day1frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day1frm.Anethetist = Anethetist;
            //Day1frm.Show();
        }

        private void btnLabDay0_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay0 Day0Frm = new frmLabDay0();
            Day0Frm.Title = "Lab Day 0 - Oocyte No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day0Frm.Title = "IVF Lab Day 0(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day0Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day0Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day0Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day0Frm.CoupleDetails = CoupleDetails;
            Day0Frm.date = StartDate;
            Day0Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day0Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day0Frm.embryologistID = EmbryologistID;
            Day0Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day0Frm.Anethetist = Anethetist;
            Day0Frm.OPUDate = OPUDate;
            Day0Frm.OPUTime = OPUTime;
            Day0Frm.SourceOfSperm = SourceOfSperm;
            Day0Frm.PlannedTreatmentID = PlannedTreatmentID;
            Day0Frm.IsClosed = IsClosed;
            Day0Frm.Show();
        }
        void Win_OnCloseButton_Click(object sender, EventArgs e)
        {
            fillGrid();
            FillEmbryologySummary();
        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            fillGrid();
            FillEmbryologySummary();
        }

        private void btnLabDay2_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 2 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 2 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 2;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.FillCleavageGrade();
            //winFert.fillDetailsLabDay2();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.Show();

            //frmLabDay2 Day2Frm = new frmLabDay2();
            //Day2Frm.Title = "Embryology - Lab Day 2 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day2Frm.Title = "IVF Lab Day 2(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day2Frm.CoupleDetails = CoupleDetails;
            //Day2Frm.date = StartDate.AddDays(2);
            //Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day2Frm.embryologistID = EmbryologistID;
            //Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day2Frm.Anethetist = Anethetist;
            //Day2Frm.Show();
        }

        private void btnLabDay6_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 6 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 6 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 6;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.StageofDevelopmentGrade();
            //winFert.fillDetailsLabDay6();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.FrozenPGDPGS = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS;
            if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay3PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay5PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay7PGDPGS)
                winFert.Day6PGDPGS = true;
            winFert.Show();

            //frmLabDay6 Day2Frm = new frmLabDay6();
            //Day2Frm.Title = "Embryology - Lab Day 6 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day2Frm.Title = "IVF Lab Day 6(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day2Frm.CoupleDetails = CoupleDetails;
            //Day2Frm.date = StartDate.AddDays(6);
            //Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day2Frm.embryologistID = EmbryologistID;
            //Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day2Frm.Anethetist = Anethetist;
            //Day2Frm.Show();
        }

        private void btnLabDay5_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 5 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 5 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 5;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.StageofDevelopmentGrade();
            //winFert.fillDetailsLabDay5();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.FrozenPGDPGS = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS;
            if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay3PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay6PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay7PGDPGS)
                winFert.Day5PGDPGS = true;
            winFert.Show();

            //frmLabDay5 Day2Frm = new frmLabDay5();
            //Day2Frm.Title = "Embryology - Lab Day 5 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day2Frm.Title = "IVF Lab Day 5(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day2Frm.CoupleDetails = CoupleDetails;
            //Day2Frm.date = StartDate.AddDays(5);
            //Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day2Frm.embryologistID = EmbryologistID;
            //Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day2Frm.Anethetist = Anethetist;
            //Day2Frm.Show();
        }

        private void btnLabDay4_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 4 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 4 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 4;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.FillCleavageGrade();
            //winFert.fillDetailsLabDay4();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.Show();

            //frmLabDay4 Day2Frm = new frmLabDay4();
            //Day2Frm.Title = "Embryology - Lab Day 4 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day2Frm.Title = "IVF Lab Day 4(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day2Frm.CoupleDetails = CoupleDetails;
            //Day2Frm.date = StartDate.AddDays(4);
            //Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day2Frm.embryologistID = EmbryologistID;
            //Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day2Frm.Anethetist = Anethetist;
            //Day2Frm.Show();
        }

        private void btnLabDay3_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 3 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 3 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 3;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.FillCleavageGrade();
            //winFert.fillDetailsLabDay3();
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.FrozenPGDPGS = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS;
            if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay5PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay6PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay7PGDPGS)
                winFert.Day3PGDPGS = true;
            winFert.Show();

            //frmLabDay3 Day2Frm = new frmLabDay3();
            //Day2Frm.Title = "Embryology - Lab Day 3 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            ////Day2Frm.Title = "IVF Lab Day 3(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            //Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            //Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            //Day2Frm.CoupleDetails = CoupleDetails;
            //Day2Frm.date = StartDate.AddDays(3);
            //Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            //Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            //Day2Frm.embryologistID = EmbryologistID;
            //Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            //Day2Frm.Anethetist = Anethetist;
            //Day2Frm.Show();
        }

        private void Cmd4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCryoPreservation_Click(object sender, RoutedEventArgs e)
        {
            CryoPreservation cryo = new CryoPreservation();
            cryo.Title = "Cryo-Preservation : ( Patient Name :-" + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
            cryo.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            cryo.CoupleDetails = CoupleDetails;
            cryo.Show();
        }
        long StoreID = 0;

        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(PlanTherapyID, PlanTherapyUnitID, CoupleDetails, "LabDays");
            Win.Closed += new EventHandler(Win_OnCloseButton_Click);
            Win.Show();
        }

        private void GetTherapyDetails()
        {
            try
            {
                clsIVFDashboard_GetTherapyListBizActionVO BizAction = new clsIVFDashboard_GetTherapyListBizActionVO();
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.TherapyUnitID = PlanTherapyUnitID;
                BizAction.TherapyID = PlanTherapyID;
                BizAction.Flag = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyDetails;
                        wait.Close();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        void Win_OnSaveButtonMedia_Click(object sender, RoutedEventArgs e)
        {

        }
        string MyPhoto;// = null;
        private string setImages(long cellStageID)
        {
            switch (cellStageID)
            {
                case 1:// 1PB
                    MyPhoto = "/PalashDynamics;component/Icons/1PB.png";
                    break;
                case 2://2PB
                    MyPhoto = "/PalashDynamics;component/Icons/2PB.png";
                    break;
                case 3://2PN 2PB
                    MyPhoto = "/PalashDynamics;component/Icons/2PN2PB.png";
                    break;
                case 4://Syngamy
                    MyPhoto = "/PalashDynamics;component/Icons/Culture1PN.png";
                    break;
                case 6://4 cells
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell4.png";
                    break;
                case 7://GV
                    MyPhoto = "/PalashDynamics;component/Icons/CultureGV.png";
                    break;
                case 8://M-I
                    MyPhoto = "/PalashDynamics;component/Icons/CultureMI.png";
                    break;
                case 9://2 Cells
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell2.png";
                    break;
                case 10://3 Cells
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell3.png";
                    break;
                case 11://M-II
                    MyPhoto = "/PalashDynamics;component/Icons/CultureMII.png";
                    break;
                case 12://2 PN
                    MyPhoto = "/PalashDynamics;component/Icons/PN2.png";
                    break;
                case 13://3 PN
                    MyPhoto = "/PalashDynamics;component/Icons/PN3.png";
                    break;
                case 14://Degenerated
                    MyPhoto = "/PalashDynamics;component/Icons/CultureDegenerate.png";
                    break;
                case 15://Lost
                    MyPhoto = "/PalashDynamics;component/Icons/CultureLost.png";
                    break;
                case 16://Oocytes
                    MyPhoto = "/PalashDynamics;component/Icons/CultureDegenerate.png";
                    break;
                case 17://1PB 1PN
                    MyPhoto = "/PalashDynamics;component/Icons/1PB1PN.png";
                    break;
                case 18://1PB 2PN
                    MyPhoto = "/PalashDynamics;component/Icons/1PB2PN.png";
                    break;
                case 19://2PB 1PN
                    MyPhoto = "/PalashDynamics;component/Icons/2PB1PN.png";
                    break;
                case 20://2PB 2PN
                    MyPhoto = "/PalashDynamics;component/Icons/2PN2PB.png";
                    break;
                case 21://5 cell
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell5.png";
                    break;
                case 22://6 Cells
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell6.png";
                    break;
                case 23://7 cell
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell7.png";
                    break;
                case 24://8 cell
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell8.png";
                    break;
                case 25://9 cell
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell9.png";
                    break;
                case 26://10 Cell
                    MyPhoto = "/PalashDynamics;component/Icons/10cell.png";
                    break;
                case 27://11 Cell
                    MyPhoto = "/PalashDynamics;component/Icons/11cell.png";
                    break;
                case 28://12 Cell
                    MyPhoto = "/PalashDynamics;component/Icons/12cell.png";
                    break;
                case 29://Compaction
                    MyPhoto = "/PalashDynamics;component/Icons/compaction.png";
                    break;
                case 30://Blastocyst
                    MyPhoto = "/PalashDynamics;component/Icons/Blastocyst.png";
                    break;
                case 31://Early Blastocyst
                    MyPhoto = "/PalashDynamics;component/Icons/CultureBEarly.png";
                    break;
                case 32://Expanded Blastocyst
                    MyPhoto = "/PalashDynamics;component/Icons/CultureBExpand.png";
                    break;
                case 33://Early Compaction
                    MyPhoto = "/PalashDynamics;component/Icons/CultureBFully.png";
                    break;
                case 34://1 Cell
                    MyPhoto = "/PalashDynamics;component/Icons/CultureXCell2.png";
                    break;
                case 35://Hatching Blastocyst
                    MyPhoto = "/PalashDynamics;component/Icons/HatchingBlastocyst.png";
                    break;
                default:
                    MyPhoto = "/PalashDynamics;component/Icons/CultureMI.png";
                    break;
            }
            return MyPhoto;
        }

        private string setImages1(long FertCheckID)
        {
            switch (FertCheckID)
            {
                case 1:// 1PB
                    MyPhoto = "/PalashDynamics;component/Icons/yes.jpg";
                    break;
                case 2://2PB
                    MyPhoto = "/PalashDynamics;component/Icons/no.jpg";
                    break;
                default:
                    MyPhoto = "/PalashDynamics;component/Icons/positive.png";
                    break;
            }
            return MyPhoto;
        }

        private void Cmdsourcedetails_Click(object sender, RoutedEventArgs e)
        {
            //SemenDetails win = new SemenDetails();
            //win.Show();
        }

        private void btnLabDay0_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void dgDayList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //string rrr = e.EditingElement.Name;

        }


        long OocyteDonorID = 0;
        long OocyteDonorUnitID = 0;
        private void GetDonordetails()
        {
            cls_NewGetDonorDetailsBizActionVO BizActionObj = new cls_NewGetDonorDetailsBizActionVO();
            BizActionObj.DonorDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizActionObj.DonorDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
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
                        OocyteDonorID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorID;
                        OocyteDonorUnitID = ((cls_NewGetDonorDetailsBizActionVO)arg.Result).DonorDetails.OoctyDonorUnitID;

                    }
                }
                AddOocList();
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void GetOocyteFromDonor()
        {
            try
            {
                wait.Show();
                clsGetDetailsOfReceivedOocyteBizActionVO BizAction = new clsGetDetailsOfReceivedOocyteBizActionVO();
                BizAction.Details = new clsReceiveOocyteVO();
                BizAction.Details.TherapyID = PlanTherapyID;
                BizAction.Details.TherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.ID > 0)
                        {
                            if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details != null)
                            {
                                // this.DataContext = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details;
                                StartDate = ((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUDate.Date;
                                PlannedNoOfEmb = ((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.OocyteConsumed;
                                if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.IsFreezed == true)
                                {
                                    //AddOocList();
                                    GetDonordetails();
                                    dgDayList.IsEnabled = true;
                                    Cmdmedia.IsEnabled = true;
                                    cmdCryoPreservation.IsEnabled = true;
                                }
                                else
                                {
                                    string msgTitle = "Palash";
                                    string msgText = "You Cannot Go For Embryology Work \n Until You Freeze Received Oocyte Details ";
                                    MessageBoxControl.MessageBoxChildWindow msgWin =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                                    msgWin.Show();

                                    // dgDayList.IsEnabled = false;
                                    Cmdmedia.IsEnabled = false;
                                    //  cmdCryoPreservation.IsEnabled = false;
                                }
                            }
                        }
                        else
                        {
                            string msgTitle = "Palash";
                            string msgText = "You Cannot Go For Embryology Work \n Until You Fill The Received Oocyte Details. ";
                            MessageBoxControl.MessageBoxChildWindow msgWin =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgWin.Show();
                            Cmdmedia.IsEnabled = false;

                        }
                    }
                    wait.Close();

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }


        //added by neena
        private void btnFertCheck_Click(object sender, RoutedEventArgs e)
        {
            frmFertCheck winFert = new frmFertCheck();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Fertilization-Oocyte/Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Fertilization-Oocyte/Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.IsClosed = IsClosed;
            winFert.Show();
        }

        private void hlbSelectSample_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveNoOfOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNoOocyteRetrieved.Text) || Convert.ToInt32(txtNoOocyteRetrieved.Text) == 0)
            {
                txtNoOocyteRetrieved.SetValidation("No of Oocyte Retrieved Is Required");
                txtNoOocyteRetrieved.RaiseValidationError();
                txtNoOocyteRetrieved.Focus();
            }
            else if (Convert.ToInt32(txtNoOocyteRetrieved.Text) > 50)
            {
                txtNoOocyteRetrieved.SetValidation("Enter Valid Number");
                txtNoOocyteRetrieved.RaiseValidationError();
                txtNoOocyteRetrieved.Focus();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save No of Oocyte Retrieved", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW3.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW3_OnMessageBoxClosed);
                msgW3.Show();
            }
        }

        void msgW3_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddUpdateOocyteNumberBizActionVO BizAction = new clsIVFDashboard_AddUpdateOocyteNumberBizActionVO();
                BizAction.OPUDetails = new clsIVFDashboard_OPUVO();

                BizAction.OPUDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.OPUDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.OPUDetails.PlanTherapyID = PlanTherapyID;
                BizAction.OPUDetails.PlanTherapyUnitID = PlanTherapyUnitID;

                if (txtNoOocyteRetrieved.Text != null)
                    BizAction.OPUDetails.OocyteRetrived = Convert.ToInt64(txtNoOocyteRetrieved.Text);



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", " Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        msgW1.Show();

                        dgDayList.Visibility = Visibility.Visible;
                        txtNoOocyteRetrieved.IsEnabled = false;
                        hlbOocyteNoSave.IsEnabled = false;
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else if (result == MessageBoxResult.No)
            {
                fillGrid();
                FillEmbryologySummary();
            }
            //throw new NotImplementedException();
        }

        frmGraphicalRepresentationofLabDaysNew grpRepNew = null;
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                dgDayList.Visibility = Visibility.Visible;

                fillOPUDetails();
                //UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysNew") as UIElement;
                //grpRepNew = new frmGraphicalRepresentationofLabDaysNew();
                //grpRepNew = (frmGraphicalRepresentationofLabDaysNew)mydata1;
                //grpRepNew.CoupleDetails = CoupleDetails;
                //grpRepNew.StartDate = StartDate;
                //grpRepNew.PlanTherapyID = PlanTherapyID;
                //grpRepNew.PlanTherapyUnitID = PlanTherapyUnitID;
                //grpRepNew.PlannedTreatmentID = PlannedTreatmentID;

                // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
                // ConEmbryology.Content = grpRepNew;

            }
            //throw new NotImplementedException();
        }


        //added by neena dated 10/8/16 fro embryology summary

        DateTime? OPUDate = DateTime.Now;
        DateTime? OPUTime = DateTime.Now;
        private void FillEmbryologySummary()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetEmbryologySummaryBizActionVO BizAction = new clsIVFDashboard_GetEmbryologySummaryBizActionVO();
                BizAction.EmbSummary = new clsIVFDashboard_EmbryologySummary();
                BizAction.EmbSummary.PlanTherapyID = PlanTherapyID;
                BizAction.EmbSummary.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.EmbSummary.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.EmbSummary.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {

                        if (((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary != null)
                        {
                            txtNoOocyteRetrieved.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.NoOocyte.ToString();
                            txtGV.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.GV.ToString();
                            txtMI.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.MI.ToString();
                            txtMII.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.MII.ToString();
                            txtOocyteCytoplasmDysmorphisimPresent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OocyteCytoplasmDysmorphisimPresent.ToString();
                            txtOocyteCytoplasmDysmorphisimAbsent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OocyteCytoplasmDysmorphisimAbsent.ToString();
                            txtExtracytoplasmicDysmorphisimPresent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.ExtracytoplasmicDysmorphisimPresent.ToString();
                            txtExtracytoplasmicDysmorphisimAbsent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.ExtracytoplasmicDysmorphisimAbsent.ToString();
                            txtOocyteCoronaCumulusComplexPresent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OocyteCoronaCumulusComplexNormal.ToString();
                            txtOocyteCoronaCumulusComplexAbsent.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OocyteCoronaCumulusComplexAbnormal.ToString();
                            //if (((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.TriggerDate != null)
                            //{
                            //    dtTriggerDate.SelectedDate = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.TriggerDate;
                            //}
                            //if (((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.TriggerTime != null)
                            //{
                            //    dtTriggerTime.Value = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.TriggerTime;
                            //}
                            if (((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUDate != null)
                            {
                                dtOPUDate1.SelectedDate = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUDate;
                                OPUDate = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUDate;
                            }
                            if (((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUTime != null)
                            {
                                dtOPUTime.Value = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUTime;
                                OPUTime = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.OPUTime;
                            }

                            if (Convert.ToInt64(txtNoOocyteRetrieved.Text) > 0)
                            {
                                dgDayList.Visibility = Visibility.Visible;
                                txtNoOocyteRetrieved.IsEnabled = false;
                                hlbOocyteNoSave.IsEnabled = false;
                            }

                            txtFreshPGDPGSEmbryos.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.FreshPGDPGS.ToString();
                            txtFrozenPGDPGSEmbryos.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.FrozenPGDPGS.ToString();
                            txtThawedPGDPGSEmbryos.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.ThawedPGDPGS.ToString();
                            txtSurvivedPGDPGSEmbryos.Text = ((clsIVFDashboard_GetEmbryologySummaryBizActionVO)arg.Result).EmbSummary.PostThawedPGDPGS.ToString();

                        }
                    }
                    wait.Close();

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        List<MasterListItem> mlDecision = new List<MasterListItem>();
        private void fillDecision()
        {

            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlDecision.Insert(0, Default);
            EnumToList(typeof(PlanDesicion), mlDecision);
            //cmbOocyteMaturity.ItemsSource = mlSourceOfSperm;
            //cmbOocyteMaturity.SelectedItem = Default;
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {

                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNoOocyteRetrieved_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtNoOocyteRetrieved_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        long DecisionID = 0;
        private void cmbDecision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (AutoCompleteBox)sender;
            if (dgDayList.SelectedItem != null)
            {
                DecisionID = ((MasterListItem)comboBox.SelectedItem).ID;
                cls_IVFDashboard_GraphicalRepresentationVO obj;
                // long DecisionID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SelectedDecision.ID;
                if (DecisionID > 0)
                {
                    dgDayList.Columns[13].Visibility = Visibility.Visible;
                    obj = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);

                    if ((PlannedTreatmentID != 11 && DecisionID == 4) || (PlannedTreatmentID != 11 && DecisionID == 5))
                    {
                        dgDayList.Columns[15].Visibility = Visibility.Visible;
                        obj.IsSelectPatient = true;
                        obj.IsFreezedecision = false;
                    }
                    else
                    {
                        obj.IsFreezedecision = true;
                        obj.IsSelectPatient = false;
                    }
                }
                else
                    dgDayList.Columns[13].Visibility = Visibility.Collapsed;
            }
        }

        private void hlbAddObservations_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Oocyte/Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Oocyte/Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.IsIsthambul = IsIsthambul;
            winFert.IsClosed = IsClosed;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.Show();

        }

        private void hlbFreezedecision_Click(object sender, RoutedEventArgs e)
        {
            if (IsClosed)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Cycle is closed.. Can't freeze the Decision", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Decision details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                wait.Show();
                cls_IVFDashboard_AddUpdateDecisionBizActionVO BizAction = new cls_IVFDashboard_AddUpdateDecisionBizActionVO();
                BizAction.ETDetails = new clsIVFDashboard_LabDaysVO();

                BizAction.ETDetails.PatientID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PatientID;
                BizAction.ETDetails.PatientUnitID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PatientUnitID;
                BizAction.ETDetails.PlanTherapyID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PlanTherapyID;
                BizAction.ETDetails.PlanTherapyUnitID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PlanTherapyUnitID;
                BizAction.ETDetails.OocyteNumber = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
                BizAction.ETDetails.SerialOocyteNumber = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
                BizAction.ETDetails.DecisionID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SelectedDecision.ID;
                BizAction.ETDetails.OocyteDonorID = OocyteDonorID;
                BizAction.ETDetails.OocyteDonorUnitID = OocyteDonorUnitID;
                BizAction.ETDetails.Day = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).Day;
                BizAction.ETDetails.DayNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).DayNo;
                BizAction.ETDetails.GradeID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).GradeID;
                BizAction.ETDetails.StageofDevelopmentGrade = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).StageofDevelopmentGrade;
                BizAction.ETDetails.InnerCellMassGrade = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).InnerCellMassGrade;
                BizAction.ETDetails.TrophoectodermGrade = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).TrophoectodermGrade;
                BizAction.ETDetails.CellStage = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CellStage;
                BizAction.ETDetails.PlanDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PlanDate;
                if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsExtendedCulture || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsExtendedCultureFromOtherCycle)
                    BizAction.ETDetails.IsFrozenEmbryo = true;
                else
                    BizAction.ETDetails.IsFreshEmbryo = true;

                if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS == true)
                    BizAction.ETDetails.IsFrozenEmbryoPGDPGS = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS;
                else
                {
                    if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay3PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay5PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay6PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay7PGDPGS)
                        BizAction.ETDetails.IsFreshEmbryoPGDPGS = true;
                }



                if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                {
                    if (BizAction.ETDetails.DecisionID == 4)
                        BizAction.ETDetails.IsDonorCycleDonate = true;
                    else if (BizAction.ETDetails.DecisionID == 5)
                        BizAction.ETDetails.IsDonorCycleDonateCryo = true;
                }
                else
                {
                    if (BizAction.ETDetails.DecisionID == 4)
                        BizAction.ETDetails.IsDonate = true;
                    else if (BizAction.ETDetails.DecisionID == 5)
                        BizAction.ETDetails.IsDonateCryo = true;
                    if (BizAction.ETDetails.IsDonate == true || BizAction.ETDetails.IsDonateCryo == true)
                    {
                        BizAction.ETDetails.RecepientPatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                        BizAction.ETDetails.RecepientPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                    }
                }


                //BizAction.ETDetails.Day = Day;
                //BizAction.ETDetails.DayNo = DayNo;
                //BizAction.ETDetails.GradeID = GradeId;
                //BizAction.ETDetails.StageofDevelopmentGrade = StageofDevelopmentGrade;
                //BizAction.ETDetails.InnerCellMassGrade = InnerCellMassGrade;
                //BizAction.ETDetails.TrophoectodermGrade = TrophoectodermGrade;
                //BizAction.ETDetails.CellStage = CellStage;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                          new MessageBoxControl.MessageBoxChildWindow("", " Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
                        msgW2.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else if (result == MessageBoxResult.No)
            {
                fillGrid();
                FillEmbryologySummary();
            }

        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                fillGrid();
                FillEmbryologySummary();
            }


            //throw new NotImplementedException();
        }

        public void SavePlanDecision()
        {
            cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO BizAction = new cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO();
            BizAction.Details = new cls_IVFDashboard_GraphicalRepresentationVO();

            BizAction.Details.PatientID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PatientID;
            BizAction.Details.PatientUnitID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PatientUnitID;
            BizAction.Details.PlanTherapyID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PlanTherapyID;
            BizAction.Details.PlanTherapyUnitID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).PlanTherapyUnitID;
            BizAction.Details.OocyteNumber = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            BizAction.Details.SerialOocyteNumber = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            BizAction.Details.DecisionID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SelectedDecision.ID;
            BizAction.Details.OocyteDonorID = OocyteDonorID;
            BizAction.Details.OocyteDonorUnitID = OocyteDonorUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                           new MessageBoxControl.MessageBoxChildWindow("", " Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    // msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
                    msgW2.Show();
                }
                wait.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void hlbSelectPatient_Click(object sender, RoutedEventArgs e)
        {
            PtListDonateOnDay0EmbDashboard WinDonate = new PtListDonateOnDay0EmbDashboard();
            WinDonate.CurrentCycleCoupleDetails = CoupleDetails;
            WinDonate.OnSaveButton_Click += new RoutedEventHandler(WinDonate_OnSaveButton_Click);
            WinDonate.OnCloseButtonClick += new RoutedEventHandler(WinDonate_OnCloseButtonClick);
            WinDonate.Closed += new EventHandler(WinDonate_Closed);
            WinDonate.Show();
        }

        void WinDonate_Closed(object sender, EventArgs e)
        {
            fillGrid();
            FillEmbryologySummary();
        }

        void WinDonate_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            fillGrid();
            FillEmbryologySummary();
        }

        void WinDonate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                if (dgDayList.SelectedItem != null)
                {
                    cls_IVFDashboard_GraphicalRepresentationVO obj = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
                    obj.PatientName = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientName;
                    obj.MRNO = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.MRNo;
                    dgDayList.Columns[16].Visibility = Visibility.Visible;
                    dgDayList.Columns[17].Visibility = Visibility.Visible;
                    dgDayList.Columns[13].Visibility = Visibility.Visible;
                    obj.IsFreezedecision = true;
                    obj.IsSelectPatient = true;
                    obj.IsDecision = true;
                }

            }
        }

        private void btnLabDay7_Click(object sender, RoutedEventArgs e)
        {
            frmAddObservation winFert = new frmAddObservation();
            if (string.IsNullOrEmpty(((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode))
                winFert.Title = "Lab Day 7 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            else
                winFert.Title = "Lab Day 7 - Embryo No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + "Cycle No.-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).CycleCode;
            winFert.IsSaved = true;
            winFert.Day = 7;
            winFert.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            winFert.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            winFert.ProcedureDate = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureDate;
            winFert.ProcedureTime = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).ProcedureTime;
            winFert.PlanTherapyId = ((clsPlanTherapyVO)this.DataContext).ID;
            winFert.PlanTherapyUnitId = ((clsPlanTherapyVO)this.DataContext).UnitID;
            winFert.SelectedDay = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem);
            winFert.DecisionID = DecisionID;
            //winFert.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            winFert.CoupleDetails = CoupleDetails;
            winFert.date = StartDate;
            winFert.StageofDevelopmentGrade();
            //winFert.fillDetailsLabDay6();
            winFert.IsIsthambul = IsIsthambul;
            winFert.GraphicalOocDetailsList = GraphicalOocDetailsList;
            winFert.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            winFert.Closed += new EventHandler(Win_OnCloseButton_Click);
            winFert.FrozenPGDPGS = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).IsFrozenEmbryoPGDPGS;
            if (((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay3PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay5PGDPGS || ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).BlDay6PGDPGS)
                winFert.Day7PGDPGS = true;
            winFert.Show();
        }



        //

        //

    }

}
