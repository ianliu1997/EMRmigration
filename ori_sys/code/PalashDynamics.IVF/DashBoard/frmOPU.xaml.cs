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

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmOPU : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlannedTreatmentID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        public DateTime? OPUDate;
        public DateTime? TherapyStartDate { get; set; }
        public List<clsTherapyExecutionVO> TherapyExecutionList { get; set; }
        public bool IsPACChecked = false;
        public bool IsConsentCheck = false;
        WaitIndicator wait = new WaitIndicator();
        public DateTime? Triggerdate;
        public DateTime? TriggerTime;
        public long AnesthesistId;

        public frmOPU()
        {
            InitializeComponent();
            this.DataContext = new clsIVFDashboard_OPUVO();
            //dtOPUDate.SelectedDate = Convert.ToDateTime(OPUDate);
        }
        private void FillOPUDate()
        {
            if (TherapyExecutionList != null)
            {
                foreach (var item in TherapyExecutionList)
                {
                    if (item.Head == "OPU")
                    {
                        if (item.Day1 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(0);
                        }
                        if (item.Day2 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(1);
                        }
                        if (item.Day3 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(2);
                        }
                        if (item.Day4 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(3);
                        }
                        if (item.Day5 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(4);
                        }
                        if (item.Day6 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(5);
                        }
                        if (item.Day7 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(6);
                        }
                        if (item.Day8 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(7);
                        }
                        if (item.Day9 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(8);
                        }
                        if (item.Day10 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(9);
                        }
                        if (item.Day11 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(10);
                        }
                        if (item.Day12 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(11);
                        }
                        if (item.Day13 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(12);
                        }
                        if (item.Day14 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(13);
                        }
                        if (item.Day15 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(14);
                        }
                        if (item.Day16 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(15);
                        }
                        if (item.Day17 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(16);
                        }
                        if (item.Day18 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(17);
                        }
                        if (item.Day19 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(18);
                        }
                        if (item.Day20 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(19);
                        }
                        if (item.Day21 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(20);
                        }
                        if (item.Day22 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(21);
                        }
                        if (item.Day23 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(22);
                        }
                        if (item.Day24 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(23);
                        }
                        if (item.Day25 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(24);
                        }
                        if (item.Day26 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(25);
                        }
                        if (item.Day27 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(26);
                        }
                        if (item.Day28 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(27);
                        }
                        if (item.Day29 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(28);
                        }
                        if (item.Day30 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(29);
                        }
                        if (item.Day31 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(30);
                        }
                        if (item.Day32 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(31);
                        }
                        if (item.Day33 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(32);
                        }
                        if (item.Day34 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(33);
                        }
                        if (item.Day35 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(34);
                        }
                        if (item.Day36 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(35);
                        }
                        if (item.Day37 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(36);
                        }
                        if (item.Day38 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(37);
                        }
                        if (item.Day39 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(38);
                        }
                        if (item.Day40 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(39);
                        }
                        if (item.Day41 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(40);
                        }
                        if (item.Day42 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(41);
                        }
                        if (item.Day43 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(42);
                        }
                        if (item.Day44 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(43);
                        }
                        if (item.Day45 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(44);
                        }
                        if (item.Day46 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(45);
                        }
                        if (item.Day47 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(46);
                        }
                        if (item.Day48 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(47);
                        }
                        if (item.Day49 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(48);
                        }
                        if (item.Day50 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(49);
                        }
                        if (item.Day51 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(50);
                        }
                        if (item.Day52 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(51);
                        }
                        if (item.Day53 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(52);
                        }
                        if (item.Day54 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(53);
                        }
                        if (item.Day55 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(54);
                        }
                        if (item.Day56 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(55);
                        }
                        if (item.Day57 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(56);
                        }
                        if (item.Day58 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(57);
                        }
                        if (item.Day59 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(58);
                        }
                        if (item.Day60 == "True")
                        {
                            dtOPUDate.SelectedDate = TherapyStartDate.Value.AddDays(59);
                        }
                    }
                }
            }
        }

        private void FillLeftRightFolliculeSize()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetFolliculeLRSumBizActionVO BizAction = new clsIVFDashboard_GetFolliculeLRSumBizActionVO();
                BizAction.FollicularMonitoringDetial = new clsFollicularMonitoring();
                BizAction.FollicularMonitoringDetial.TherapyId = PlanTherapyID;
                BizAction.FollicularMonitoringDetial.TherapyUnitId = PlanTherapyUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetFolliculeLRSumBizActionVO)arg.Result).FollicularMonitoringSizeDetials != null)
                        {
                            txtLeftFollicule.Text = ((clsIVFDashboard_GetFolliculeLRSumBizActionVO)arg.Result).FollicularMonitoringSizeDetials.LeftSum.ToString();
                            txtRightFollicule.Text = ((clsIVFDashboard_GetFolliculeLRSumBizActionVO)arg.Result).FollicularMonitoringSizeDetials.RightSum.ToString();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
            finally
            {
                wait.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillLeftRightFolliculeSize();
            cmbAnesthesia.IsEnabled = false;
            txtReason.IsEnabled = false;
            //dtTriggDate.SelectedDate = DateTime.Now.Date;v
            //txtTrigTime.Value = DateTime.Now;

            rdFlushing.IsChecked = false;
            rdFlushing.IsEnabled = false;


            if (TherapyStartDate != null)
            {
                dtOPUDate.DisplayDateStart = TherapyStartDate.Value;
                dtOPUDate.DisplayDateEnd = TherapyStartDate.Value.AddDays(59);
            }
            //dtOPUDate.SelectedDate = Convert.ToDateTime(OPUDate);

            dtTriggDate.SelectedDate = Triggerdate;
            txtTrigTime.Value = TriggerTime;
            if (IsClosed == true)
            {
                //  cmdNew.IsEnabled = false;
            }
            FillEmbryologist();
            //  fillOPUDetails();
            //if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
            //{
            //    chkIsSetForED.Visibility = Visibility.Visible;
            //    txtOocyteForED.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    chkIsSetForED.Visibility = Visibility.Collapsed;
            //    txtOocyteForED.Visibility = Visibility.Collapsed;
            //}
            txtOocyteForED.IsEnabled = false;
            txtNeedlesUsed.Text = "1";
            FillOPUDate();
        }
        #region Fill Combox
        private void FillEmbryologist()
        {
            wait.Show();
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

                    cmbEmbroyologist.ItemsSource = null;
                    cmbEmbroyologist.ItemsSource = objList;
                    cmbEmbroyologist.SelectedItem = objList[0];

                    cmbAssistantEmbryologist.ItemsSource = null;
                    cmbAssistantEmbryologist.ItemsSource = objList;
                    cmbAssistantEmbryologist.SelectedItem = objList[0];

                    cmbAnaesthetist.ItemsSource = null;
                    cmbAnaesthetist.ItemsSource = objList;
                    cmbAnaesthetist.SelectedItem = objList[0];
                    FillAnesthetist();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
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
                    cmbAnesthetic.ItemsSource = null;
                    cmbAnesthetic.ItemsSource = objList;
                    cmbAnesthetic.SelectedItem = objList[0];

                    if (AnesthesistId != null && AnesthesistId > 0)
                    {
                        cmbAnesthetic.SelectedValue = (long)AnesthesistId;
                    }

                    fillAnesthesia();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void fillAnesthesia()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_Anesthesia;
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

                    cmbAnesthesia.ItemsSource = null;
                    cmbAnesthesia.ItemsSource = objList;
                    cmbAnesthesia.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    fillNeedleUsed();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillNeedleUsed()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.view_ListOfOPUNeedles;
            BizAction.MasterTable = MasterTableNameList.M_IVF_NeedleMaster;
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

                    cmbNeedle.ItemsSource = null;
                    cmbNeedle.ItemsSource = objList;
                    cmbNeedle.SelectedItem = objList[1];

                    if (this.DataContext != null)
                    {

                    }
                    //  fillLevelOfDifficulty();
                    fillTypeOfNeedle();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillTypeOfNeedle()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_TypeOfNeedle;
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

                    cmbTONeedle.ItemsSource = null;
                    cmbTONeedle.ItemsSource = objList;
                    cmbTONeedle.SelectedItem = objList[2];

                    if (this.DataContext != null)
                    {

                    }
                    fillLevelOfDifficulty();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillLevelOfDifficulty()
        {
            List<MasterListItem> Items = new List<MasterListItem>();

            MasterListItem Item = new MasterListItem();
            Item.ID = (int)0;
            Item.Description = "--Select--";
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)LevelOfDifficulty.Difficult;
            Item.Description = LevelOfDifficulty.Difficult.ToString();
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)LevelOfDifficulty.Easy;
            Item.Description = LevelOfDifficulty.Easy.ToString();
            Items.Add(Item);

            cmbDifficulty.ItemsSource = Items;
            cmbDifficulty.SelectedValue = (long)2;
            fillOPUDetails();
            wait.Close();

            //fillOocyteQuality();

        }
        //private void fillOocyteQuality()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVF_OocyteQualityMaster;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

        //            cmbOocyteQualitity.ItemsSource = null;
        //            cmbOocyteQualitity.ItemsSource = objList;
        //            cmbOocyteQualitity.SelectedItem = objList[0];

        //            if (this.DataContext != null)
        //            {

        //            }
        //            fillELevelOnDayMaster();
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        private void fillELevelOnDayMaster()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ELevelOnDayMaster;
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

                    cmbE2LevelOnDay.ItemsSource = null;
                    cmbE2LevelOnDay.ItemsSource = objList;
                    cmbE2LevelOnDay.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    fillOPUDetails();
                    wait.Close();


                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnSaveFreezeButton_Click;
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


                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ID > 0)
                            {

                                this.DataContext = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details;

                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                                {
                                    cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.EmbryologistID;
                                }

                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AnesthetistID != null)
                                {
                                    cmbAnesthetic.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AnesthetistID;
                                }


                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.DifficultyID != null)
                                {
                                    cmbDifficulty.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.DifficultyID;
                                }

                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                {
                                    chkFreeze.IsChecked = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Isfreezed;
                                    if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    {
                                        //chkIsSetForED.IsEnabled = false;
                                        chkFreeze.IsEnabled = false;
                                        dtOPUDate.IsEnabled = false;
                                    }
                                    else
                                    {
                                        dtOPUDate.IsEnabled = true;
                                        chkFreeze.IsEnabled = true;
                                    }
                                    cmdNew.IsEnabled = false;
                                    dtTriggDate.SelectedDate = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerDate);
                                    txtTrigTime.Value = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerTime);
                               
                                }

                                else
                                {
                                    if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Date == null)
                                    {
                                        dtOPUDate.SelectedDate = Convert.ToDateTime(OPUDate);
                                        txtTime.Value = DateTime.Now;
                                        dtTriggDate.SelectedDate = Triggerdate;
                                        txtTrigTime.Value = TriggerTime;
                                        //dtTriggDate.SelectedDate = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerDate);
                                        //txtTrigTime.Value = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerTime);

                                    }

                                    else
                                    {
                                        dtOPUDate.SelectedDate = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Date);
                                        txtTime.Value = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Date);
                                        dtTriggDate.SelectedDate = Triggerdate;
                                        txtTrigTime.Value = TriggerTime;
                                    //    dtTriggDate.SelectedDate = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerDate);
                                    //    txtTrigTime.Value = Convert.ToDateTime(((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TriggerTime);
                                    }
                                }

                                if (chkFreeze.IsChecked == true && chkIsSetForED.IsChecked == true)
                                {
                                    if (OnSaveButton_Click != null)
                                        OnSaveButton_Click(this, new RoutedEventArgs());
                                }

                                if (chkFreeze.IsChecked == true)
                                {
                                    if (OnSaveFreezeButton_Click != null)
                                        OnSaveFreezeButton_Click(this, new RoutedEventArgs());
                                }

                                // Added to retrieve parameters in the Opu Details form.
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.NeedleUsedID != null)
                                {
                                    cmbNeedle.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.NeedleUsedID;
                                }

                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TypeOfNeedleID != null)
                                {
                                    cmbTONeedle.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.TypeOfNeedleID;
                                }
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AnesthesiaID != null)
                                {
                                    cmbAnesthesia.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AnesthesiaID;
                                }
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsPreAnesthesia == true)
                                {
                                    rdAnesthesiaCheck.IsChecked = true;
                                    cmbAnesthesia.IsEnabled = true;
                                }
                                else
                                {
                                    rdAnesthesiaCheck.IsChecked = false;
                                    cmbAnesthesia.IsEnabled = false;
                                }


                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsFlushing == true)
                                    rdFlushing.IsChecked = true;
                                else
                                    rdFlushing.IsChecked = false;
                                if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsCycleCancellation == true)
                                {
                                    chkCycleCancellation.IsChecked = true;
                                    txtReason.IsEnabled = true;
                                    txtReason.Text = (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Reasons).ToString();
                                }
                                else
                                {
                                    chkCycleCancellation.IsChecked = false;
                                    txtReason.IsEnabled = false;
                                }

                                txtRemarkDetails.Text = (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.Remark).ToString();



                                #region commented

                                //if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.IsSetForED == true)
                                //{
                                //    chkIsSetForED.IsChecked = true;
                                //    txtOocyteForED.IsEnabled = true;
                                //}
                                //else
                                //{
                                //    chkIsSetForED.IsChecked = false;
                                //    txtOocyteForED.IsEnabled = false;
                                //}
                                //if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                                //{
                                //    cmbAssistantEmbryologist.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                                //}
                                //if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteQualityID != null)
                                //{
                                //    cmbOocyteQualitity.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteQualityID;
                                //}
                                //if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.NeedleUsedID != null)
                                //{
                                //    cmbNeedle.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.NeedleUsedID;
                                //}
                                //if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ELevelDayID != null)
                                //{
                                //    cmbE2LevelOnDay.SelectedValue = ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ELevelDayID;
                                //}
                                #endregion
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
            finally
            {
                wait.Close();
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                //if (Total != Convert.ToDecimal(txtOocyteRetrived.Text.Trim()) || SecTotal != Convert.ToDecimal(txtOocyteRetrived.Text.Trim()))
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Valid Values", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //    msgW1.Show();
                //    return;
                //}

                if (dtOPUDate.SelectedDate != null && txtTime.Value != null && dtTriggDate.SelectedDate != null && txtTrigTime.Value != null)
                {
                    DateTime OPUDateTime = ((DateTime)dtOPUDate.SelectedDate).Date.Add(((DateTime)txtTime.Value).TimeOfDay);
                    DateTime TriggerDateTime = ((DateTime)dtTriggDate.SelectedDate).Date.Add(((DateTime)txtTrigTime.Value).TimeOfDay);
                    TimeSpan diff = OPUDateTime - TriggerDateTime;
                    hours = diff.TotalHours;
                }

                if (hours <= 34)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow("", "34 hours is not completed yet.. Still you want to continue", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else if (hours > 36)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow("", "36 hours are crossed.. Still you want to continue", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Save OPU Details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
            //else
            //    clearUI();
        }

        private void Save()
        {
            clsIVFDashboard_AddUpdateOPUDetailsBizActionVO BizAction = new clsIVFDashboard_AddUpdateOPUDetailsBizActionVO();
            BizAction.OPUDetails = new clsIVFDashboard_OPUVO();
            BizAction.OPUDetails.ID = ((clsIVFDashboard_OPUVO)this.DataContext).ID;
            BizAction.OPUDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.OPUDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.OPUDetails.PlanTherapyID = PlanTherapyID;
            BizAction.OPUDetails.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.OPUDetails.Date = dtOPUDate.SelectedDate.Value.Date;
            BizAction.OPUDetails.Time = txtTime.Value;

            if (cmbAssistantEmbryologist.SelectedItem != null)
                BizAction.OPUDetails.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
            if (cmbAnaesthetist.SelectedItem != null)
                BizAction.OPUDetails.AnesthetistID = ((MasterListItem)cmbAnesthetic.SelectedItem).ID;
            BizAction.OPUDetails.AnesthetiaDetails = txtAnaesthesiaDetails.Text;
            if (cmbNeedle.SelectedItem != null)
                BizAction.OPUDetails.NeedleUsedID = ((MasterListItem)cmbNeedle.SelectedItem).ID;


            if (chkFreeze.IsChecked == true)
                BizAction.OPUDetails.Isfreezed = true;
            else
                BizAction.OPUDetails.Isfreezed = false;

            #region commented
            //if (cmbOocyteQualitity.SelectedItem != null)
            //    BizAction.OPUDetails.OocyteQualityID = ((MasterListItem)cmbOocyteQualitity.SelectedItem).ID;
            //if (txtOocyteRetrived.Text != null)
            //    BizAction.OPUDetails.OocyteRetrived = Convert.ToInt64(txtOocyteRetrived.Text);
            //if (chkIsSetForED.IsChecked == true)
            //    BizAction.OPUDetails.IsSetForED = true;
            //else
            //    BizAction.OPUDetails.IsSetForED = false;
            //if (txtOocyteForED.Text != null)
            //{
            //    BizAction.OPUDetails.OocyteForED = Convert.ToInt64(txtOocyteForED.Text);
            //    BizAction.OPUDetails.BalanceOocyte = Convert.ToInt64(txtOocyteRetrived.Text) - Convert.ToInt64(txtOocyteForED.Text);
            //}
            //else
            //    BizAction.OPUDetails.BalanceOocyte = Convert.ToInt64(txtOocyteRetrived.Text);
            //BizAction.OPUDetails.OocyteRemark = txtRemark.Text;
            //if (cmbE2LevelOnDay.SelectedItem != null)
            //    BizAction.OPUDetails.ELevelDayID = ((MasterListItem)cmbE2LevelOnDay.SelectedItem).ID;
            //BizAction.OPUDetails.ELevelDayremark = txtE2Remark.Text;
            //BizAction.OPUDetails.Evalue = txtE2Value.Text;
            //if (txtMI.Text != null)
            //{
            //    BizAction.OPUDetails.MI = Convert.ToDecimal(txtMI.Text.Trim());
            //}
            //if (txtMII.Text != null)
            //{
            //    BizAction.OPUDetails.MII = Convert.ToDecimal(txtMII.Text.Trim());
            //}
            //if (txtGV.Text != null)
            //{
            //    BizAction.OPUDetails.GV = Convert.ToDecimal(txtGV.Text.Trim());
            //}

            //if (txtOocyteCytoplasmDysmorphisimPresent.Text != null)
            //    BizAction.OPUDetails.OocyteCytoplasmDysmorphisimPresent = Convert.ToDecimal(txtOocyteCytoplasmDysmorphisimPresent.Text.Trim());

            //if (txtOocyteCytoplasmDysmorphisimAbsent.Text != null)
            //    BizAction.OPUDetails.OocyteCytoplasmDysmorphisimAbsent = Convert.ToDecimal(txtOocyteCytoplasmDysmorphisimAbsent.Text.Trim());

            //if (txtExtracytoplasmicDysmorphisimPresent.Text != null)
            //    BizAction.OPUDetails.ExtracytoplasmicDysmorphisimPresent = Convert.ToDecimal(txtExtracytoplasmicDysmorphisimPresent.Text.Trim());

            //if (txtExtracytoplasmicDysmorphisimAbsent.Text != null)
            //    BizAction.OPUDetails.ExtracytoplasmicDysmorphisimAbsent = Convert.ToDecimal(txtExtracytoplasmicDysmorphisimAbsent.Text.Trim());

            //if (txtOocyteCoronaCumulusComplexPresent.Text != null)
            //    BizAction.OPUDetails.OocyteCoronaCumulusComplexPresent = Convert.ToDecimal(txtOocyteCoronaCumulusComplexPresent.Text.Trim());

            //if (txtOocyteCoronaCumulusComplexAbsent.Text != null)
            //    BizAction.OPUDetails.OocyteCoronaCumulusComplexAbsent = Convert.ToDecimal(txtOocyteCoronaCumulusComplexAbsent.Text.Trim());

            #endregion

            // Addded as Per the new OPU Details requirments 

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.OPUDetails.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbDifficulty.SelectedItem != null)
                BizAction.OPUDetails.DifficultyID = ((MasterListItem)cmbDifficulty.SelectedItem).ID;


            if (cmbTONeedle.SelectedItem != null)
                BizAction.OPUDetails.TypeOfNeedleID = ((MasterListItem)cmbTONeedle.SelectedItem).ID;

            BizAction.OPUDetails.TriggerDate = dtTriggDate.SelectedDate.Value.Date;
            BizAction.OPUDetails.TriggerTime = txtTrigTime.Value;

            if (chkCycleCancellation.IsChecked == true)
                BizAction.OPUDetails.IsCycleCancellation = true;

            else
                BizAction.OPUDetails.IsCycleCancellation = false;




            if (rdAnesthesiaCheck.IsChecked == true)
            {
                BizAction.OPUDetails.IsPreAnesthesia = true;
                if (cmbAnesthesia.SelectedItem != null)
                    BizAction.OPUDetails.AnesthesiaID = ((MasterListItem)cmbAnesthesia.SelectedItem).ID;
            }
            else
                BizAction.OPUDetails.IsPreAnesthesia = false;

            if (rdFlushing.IsChecked == true)
                BizAction.OPUDetails.IsFlushing = true;
            else
                BizAction.OPUDetails.IsFlushing = false;


            BizAction.OPUDetails.NeedleUsed = Convert.ToInt64(txtNeedlesUsed.Text.Trim());
            BizAction.OPUDetails.LeftFollicule = txtLeftFollicule.Text.Trim();
            BizAction.OPUDetails.RightFollicule = txtRightFollicule.Text.Trim();
            BizAction.OPUDetails.Remark = txtRemarkDetails.Text.Trim();
            BizAction.OPUDetails.Reasons = txtReason.Text.Trim();

            //Update OPU Date For Dashboard OverView
            double dayNo = 0;
            if (dtOPUDate.SelectedDate.Value.Date != null)
                dayNo = (dtOPUDate.SelectedDate.Value.Date - TherapyStartDate.Value.Date).TotalDays + 1;
            BizAction.TherapyExecutionDetial = new clsTherapyExecutionVO();
            BizAction.TherapyExecutionDetial = SetValueOfVO();
            RetriveValue(BizAction.TherapyExecutionDetial, (int)dayNo);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "OPU Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    TherapyExecutionList = null;
                    fillOPUDetails();
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


        public clsTherapyExecutionVO SetValueOfVO()
        {
            clsTherapyExecutionVO ExeVO = new clsTherapyExecutionVO();
            ExeVO.Day1 = "False";
            ExeVO.Day2 = "False";
            ExeVO.Day3 = "False";
            ExeVO.Day4 = "False";
            ExeVO.Day5 = "False";
            ExeVO.Day6 = "False";
            ExeVO.Day7 = "False";
            ExeVO.Day8 = "False";
            ExeVO.Day9 = "False";
            ExeVO.Day10 = "False";
            ExeVO.Day11 = "False";
            ExeVO.Day12 = "False";
            ExeVO.Day13 = "False";
            ExeVO.Day14 = "False";
            ExeVO.Day15 = "False";
            ExeVO.Day16 = "False";
            ExeVO.Day17 = "False";
            ExeVO.Day18 = "False";
            ExeVO.Day19 = "False";
            ExeVO.Day20 = "False";
            ExeVO.Day21 = "False";
            ExeVO.Day22 = "False";
            ExeVO.Day23 = "False";
            ExeVO.Day24 = "False";
            ExeVO.Day25 = "False";
            ExeVO.Day26 = "False";
            ExeVO.Day27 = "False";
            ExeVO.Day28 = "False";
            ExeVO.Day29 = "False";
            ExeVO.Day30 = "False";
            ExeVO.Day31 = "False";
            ExeVO.Day32 = "False";
            ExeVO.Day33 = "False";
            ExeVO.Day34 = "False";
            ExeVO.Day35 = "False";
            ExeVO.Day36 = "False";
            ExeVO.Day37 = "False";
            ExeVO.Day38 = "False";
            ExeVO.Day39 = "False";
            ExeVO.Day40 = "False";
            ExeVO.Day41 = "False";
            ExeVO.Day42 = "False";
            ExeVO.Day43 = "False";
            ExeVO.Day44 = "False";
            ExeVO.Day45 = "False";
            ExeVO.Day46 = "False";
            ExeVO.Day47 = "False";
            ExeVO.Day48 = "False";
            ExeVO.Day49 = "False";
            ExeVO.Day50 = "False";
            ExeVO.Day51 = "False";
            ExeVO.Day52 = "False";
            ExeVO.Day53 = "False";
            ExeVO.Day54 = "False";
            ExeVO.Day55 = "False";
            ExeVO.Day56 = "False";
            ExeVO.Day57 = "False";
            ExeVO.Day58 = "False";
            ExeVO.Day59 = "False";
            ExeVO.Day60 = "False";
            return ExeVO;
        }
        public clsTherapyExecutionVO RetriveValue(clsTherapyExecutionVO ExeVO, int DayNo)
        {
            switch (DayNo)
            {
                case 1:
                    ExeVO.Day1 = "True";

                    break;
                case 2:
                    ExeVO.Day2 = "True";
                    break;
                case 3:
                    ExeVO.Day3 = "True";
                    break;
                case 4:
                    ExeVO.Day4 = "True";
                    break;
                case 5:
                    ExeVO.Day5 = "True";
                    break;
                case 6:
                    ExeVO.Day6 = "True";
                    break;
                case 7:
                    ExeVO.Day7 = "True";
                    break;
                case 8:
                    ExeVO.Day8 = "True";
                    break;
                case 9:
                    ExeVO.Day9 = "True";
                    break;
                case 10:
                    ExeVO.Day10 = "True";
                    break;
                case 11:
                    ExeVO.Day11 = "True";
                    break;
                case 12:
                    ExeVO.Day12 = "True";
                    break;
                case 13:
                    ExeVO.Day13 = "True";
                    break;
                case 14:
                    ExeVO.Day14 = "True";
                    break;
                case 15:
                    ExeVO.Day15 = "True";
                    break;
                case 16:
                    ExeVO.Day16 = "True";
                    break;
                case 17:
                    ExeVO.Day17 = "True";
                    break;
                case 18:
                    ExeVO.Day18 = "True";
                    break;
                case 19:
                    ExeVO.Day19 = "True";
                    break;
                case 20:
                    ExeVO.Day20 = "True";
                    break;
                case 21:
                    ExeVO.Day21 = "True";
                    break;
                case 22:
                    ExeVO.Day22 = "True";
                    break;
                case 23:
                    ExeVO.Day23 = "True";
                    break;
                case 24:
                    ExeVO.Day24 = "True";
                    break;
                case 25:
                    ExeVO.Day25 = "True";
                    break;
                case 26:
                    ExeVO.Day26 = "True";
                    break;
                case 27:
                    ExeVO.Day27 = "True";
                    break;
                case 28:
                    ExeVO.Day28 = "True";
                    break;
                case 29:
                    ExeVO.Day29 = "True";
                    break;
                case 30:
                    ExeVO.Day30 = "True";
                    break;
                case 31:
                    ExeVO.Day31 = "True";
                    break;
                case 32:
                    ExeVO.Day32 = "True";
                    break;
                case 33:
                    ExeVO.Day33 = "True";
                    break;
                case 34:
                    ExeVO.Day34 = "True";
                    break;
                case 35:
                    ExeVO.Day35 = "True";
                    break;
                case 36:
                    ExeVO.Day36 = "True";
                    break;
                case 37:
                    ExeVO.Day37 = "True";
                    break;
                case 38:
                    ExeVO.Day38 = "True";
                    break;
                case 39:
                    ExeVO.Day39 = "True";
                    break;
                case 40:
                    ExeVO.Day40 = "True";
                    break;
                case 41:
                    ExeVO.Day41 = "True";
                    break;
                case 42:
                    ExeVO.Day42 = "True";
                    break;
                case 43:
                    ExeVO.Day43 = "True";
                    break;
                case 44:
                    ExeVO.Day44 = "True";
                    break;
                case 45:
                    ExeVO.Day45 = "True";
                    break;
                case 46:
                    ExeVO.Day46 = "True";
                    break;
                case 47:
                    ExeVO.Day47 = "True";
                    break;
                case 48:
                    ExeVO.Day48 = "True";
                    break;
                case 49:
                    ExeVO.Day49 = "True";
                    break;
                case 50:
                    ExeVO.Day50 = "True";
                    break;
                case 51:
                    ExeVO.Day51 = "True";
                    break;
                case 52:
                    ExeVO.Day52 = "True";
                    break;
                case 53:
                    ExeVO.Day53 = "True";
                    break;
                case 54:
                    ExeVO.Day54 = "True";
                    break;
                case 55:
                    ExeVO.Day55 = "True";
                    break;
                case 56:
                    ExeVO.Day56 = "True";
                    break;
                case 57:
                    ExeVO.Day57 = "True";
                    break;
                case 58:
                    ExeVO.Day58 = "True";
                    break;
                case 59:
                    ExeVO.Day59 = "True";
                    break;
                case 60:
                    ExeVO.Day60 = "True";
                    break;
                default:
                    break;
            }
            return ExeVO;
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

        }

        double hours = 0;
        private bool Validate()
        {
            bool result = true;
            if (dtOPUDate.SelectedDate == null)
            {
                dtOPUDate.SetValidation("Please Select Date");
                dtOPUDate.RaiseValidationError();
                dtOPUDate.Focus();
                return false;
            }
            else
                dtOPUDate.ClearValidationError();

            if (txtTime.Value == null)
            {
                txtTime.SetValidation("Please Select Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else
                txtTime.ClearValidationError();

            if (dtTriggDate.SelectedDate == null)
            {
                dtTriggDate.SetValidation("Trigger Date is not available");
                dtTriggDate.RaiseValidationError();
                dtTriggDate.Focus();
                return false;
            }
            else
                dtTriggDate.ClearValidationError();

            if (txtTrigTime.Value == null)
            {
                txtTrigTime.SetValidation("Trigger Time is not available");
                txtTrigTime.RaiseValidationError();
                txtTrigTime.Focus();
                return false;
            }
            else
                txtTrigTime.ClearValidationError();

            if (cmbEmbroyologist.SelectedItem == null)
            {
                cmbEmbroyologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbroyologist.TextBox.RaiseValidationError();
                cmbEmbroyologist.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbEmbroyologist.SelectedItem).ID == 0)
            {
                cmbEmbroyologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbroyologist.TextBox.RaiseValidationError();
                cmbEmbroyologist.Focus();
                result = false;
            }
            else
                cmbEmbroyologist.TextBox.ClearValidationError();



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


            if (cmbAnesthetic.SelectedItem == null)
            {
                cmbAnesthetic.TextBox.SetValidation("Please select Anaesthetist");
                cmbAnesthetic.TextBox.RaiseValidationError();
                cmbAnesthetic.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbAnesthetic.SelectedItem).ID == 0)
            {
                cmbAnesthetic.TextBox.SetValidation("Please select Anaesthetist");
                cmbAnesthetic.TextBox.RaiseValidationError();
                cmbAnesthetic.Focus();
                result = false;
            }
            else
                cmbAnesthetic.TextBox.ClearValidationError();


            if (cmbNeedle.SelectedItem == null)
            {
                cmbNeedle.TextBox.SetValidation("Please select Needle Used");
                cmbNeedle.TextBox.RaiseValidationError();
                cmbNeedle.Focus();
                result = false;
            }
            else
                if (((MasterListItem)cmbNeedle.SelectedItem).ID == 0)
                {
                    cmbNeedle.TextBox.SetValidation("Please select Needle Used");
                    cmbNeedle.TextBox.RaiseValidationError();
                    cmbNeedle.Focus();
                    result = false;
                }
                else
                    cmbNeedle.TextBox.ClearValidationError();




            if (cmbTONeedle.SelectedItem == null)
            {
                cmbTONeedle.TextBox.SetValidation("Please select Type Of Needle Used");
                cmbTONeedle.TextBox.RaiseValidationError();
                cmbTONeedle.Focus();
                result = false;
            }
            else
                if (((MasterListItem)cmbTONeedle.SelectedItem).ID == 0)
                {
                    cmbTONeedle.TextBox.SetValidation("Please select Type Of Needle Used");
                    cmbTONeedle.TextBox.RaiseValidationError();
                    cmbTONeedle.Focus();
                    result = false;
                }
                else
                    cmbTONeedle.TextBox.ClearValidationError();

            if (string.IsNullOrEmpty(txtNeedlesUsed.Text) || string.IsNullOrWhiteSpace(txtNeedlesUsed.Text))
            {
                txtNeedlesUsed.SetValidation("Please Enter Number Of Needles Used");
                txtNeedlesUsed.RaiseValidationError();
                txtNeedlesUsed.Focus();
                result = false;
            }
            else
                txtNeedlesUsed.ClearValidationError();

            if (string.IsNullOrEmpty(txtLeftFollicule.Text) || string.IsNullOrWhiteSpace(txtLeftFollicule.Text))
            {
                txtLeftFollicule.SetValidation("Please Enter Number Of Left Follicule");
                txtLeftFollicule.RaiseValidationError();
                txtLeftFollicule.Focus();
                result = false;
            }
            else
                txtLeftFollicule.ClearValidationError();

            if (string.IsNullOrEmpty(txtRightFollicule.Text) || string.IsNullOrWhiteSpace(txtRightFollicule.Text))
            {
                txtRightFollicule.SetValidation("Please Enter Number Of Right Follicule");
                txtRightFollicule.RaiseValidationError();
                txtRightFollicule.Focus();
                result = false;
            }
            else
                txtRightFollicule.ClearValidationError();

            if (cmbDifficulty.SelectedItem == null)
            {
                cmbDifficulty.TextBox.SetValidation("Please select Level of Difficulty");
                cmbDifficulty.TextBox.RaiseValidationError();
                cmbDifficulty.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDifficulty.SelectedItem).ID == 0)
            {
                cmbDifficulty.TextBox.SetValidation("Please select Level of Difficulty");
                cmbDifficulty.TextBox.RaiseValidationError();
                cmbDifficulty.Focus();
                result = false;
            }
            else
                cmbDifficulty.TextBox.ClearValidationError();

            if (rdAnesthesiaCheck.IsChecked == true)
            {
                if (cmbAnesthesia.SelectedItem == null)
                {
                    cmbAnesthesia.TextBox.SetValidation("Please select Anesthesia");
                    cmbAnesthesia.TextBox.RaiseValidationError();
                    cmbAnesthesia.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbAnesthesia.SelectedItem).ID == 0)
                {
                    cmbAnesthesia.TextBox.SetValidation("Please select Anesthesia");
                    cmbAnesthesia.TextBox.RaiseValidationError();
                    cmbAnesthesia.Focus();
                    result = false;
                }
                else
                    cmbAnesthesia.TextBox.ClearValidationError();
            }

            //if (cmbE2LevelOnDay.SelectedItem == null)
            //{
            //    cmbE2LevelOnDay.TextBox.SetValidation("Please select E2 Level on Day");
            //    cmbE2LevelOnDay.TextBox.RaiseValidationError();
            //    cmbE2LevelOnDay.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbE2LevelOnDay.SelectedItem).ID == 0)
            //{
            //    cmbE2LevelOnDay.TextBox.SetValidation("Please select E2 Level on Day");
            //    cmbE2LevelOnDay.TextBox.RaiseValidationError();
            //    cmbE2LevelOnDay.Focus();
            //    result = false;
            //}
            //else
            //    cmbE2LevelOnDay.TextBox.ClearValidationError();



            //if (cmbOocyteQualitity.SelectedItem == null)
            //{
            //    cmbOocyteQualitity.TextBox.SetValidation("Please select oocyte Quality");
            //    cmbOocyteQualitity.TextBox.RaiseValidationError();
            //    cmbOocyteQualitity.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbOocyteQualitity.SelectedItem).ID == 0)
            //{
            //    cmbOocyteQualitity.TextBox.SetValidation("Please select oocyte Quality");
            //    cmbOocyteQualitity.TextBox.RaiseValidationError();
            //    cmbOocyteQualitity.Focus();
            //    result = false;
            //}
            //else
            //    cmbOocyteQualitity.TextBox.ClearValidationError();



            //if (txtOocyteRetrived.Text.IsItNumber() == false || txtOocyteRetrived.Text == string.Empty  ) 
            //{
            //    txtOocyteRetrived.SetValidation("Please Enter Number for Oocyte retrieved");
            //    txtOocyteRetrived.RaiseValidationError();
            //    txtOocyteRetrived.Focus();
            //    result = false;
            //}
            //else if (txtOocyteRetrived.Text == "0")
            //{
            //    txtOocyteRetrived.SetValidation("Oocyte retrieved should be greater than zero");
            //    txtOocyteRetrived.RaiseValidationError();
            //    txtOocyteRetrived.Focus();
            //    result = false;
            //}
            //else
            //    txtOocyteRetrived.ClearValidationError();

            //else if (txtOocyteForED.Text == "0")
            //{
            //    txtOocyteForED.SetValidation("Oocyte Set for Embryo Donation should be greater than zero");
            //    txtOocyteForED.RaiseValidationError();
            //    txtOocyteForED.Focus();
            //    result = false;
            //}
            //else if (Convert.ToInt64(txtOocyteForED.Text) > Convert.ToInt64(txtOocyteRetrived.Text))
            //{
            //    txtOocyteForED.SetValidation("Oocyte set for ED cannot be greater than Oocyte retrived");
            //    txtOocyteForED.RaiseValidationError();
            //    txtOocyteForED.Focus();
            //    result = false;
            //}
            //else
            //    txtOocyteForED.ClearValidationError();


            if (dtTriggDate.SelectedDate > dtOPUDate.SelectedDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Trigger Date can not be greater than OPU Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
            }

            return result;
        }

        private void txtOocyteRetrived_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtOocyteRetrived_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtE2Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtE2Value_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void clearUI()
        {
            cmbAnaesthetist.SelectedValue = (long)0;
            cmbAssistantEmbryologist.SelectedValue = (long)0;
            cmbEmbryologist.SelectedValue = (long)0;
            cmbNeedle.SelectedValue = (long)0;
            //cmbOocyteQualitity.SelectedValue = (long)0;
            cmbLevelOfDifficulty.SelectedValue = (long)0;
            cmbE2LevelOnDay.SelectedValue = (long)0;
            txtRemark.Text = "";
            txtE2Remark.Text = "";
            txtE2Value.Text = "";
            txtAnaesthesiaDetails.Text = "";
            //txtTime.Value = DateTime.Now;
            dtOPUDate.SelectedDate = Convert.ToDateTime(OPUDate);
            chkFreeze.IsChecked = false;
        }

        private void chkIsSetForED_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsSetForED.IsChecked == true)
            {
                txtOocyteForED.IsEnabled = true;
            }
            else
            {
                txtOocyteForED.IsEnabled = false;
            }
        }

        private void txtOocyteForED_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtOocyteForED.Text != string.Empty && txtOocyteRetrived.Text != string.Empty)
            {
                if (Convert.ToInt64(txtOocyteForED.Text) > Convert.ToInt64(txtOocyteRetrived.Text))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Oocyte set for ED cannot be greater than Oocyte retrived", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                    txtOocyteForED.Text = txtOocyteRetrived.Text;
                }
            }
        }
        decimal Total = 0;
        private void GetTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal MI = 0;
                decimal MII = 0;
                decimal GV = 0;


                if (txtMI.Text == "" || txtMI.Text == null)
                    MI = 0;
                else
                    MI = Convert.ToDecimal(txtMI.Text.Trim());

                if (txtMII.Text == "" || txtMII.Text == null)
                    MII = 0;
                else
                    MII = Convert.ToDecimal(txtMII.Text.Trim());

                if (txtGV.Text == "" || txtGV.Text == null)
                    GV = 0;
                else
                    GV = Convert.ToDecimal(txtGV.Text.Trim());

                Total = (Convert.ToDecimal(MI)) + (Convert.ToDecimal(MII)) + (Convert.ToDecimal(GV));
            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);
            }
        }
        private void txtMI_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        decimal SecTotal = 0;
        private void txtOocyteCytoplasmDysmorphisimPresent_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal CytoplasmP = 0;
                decimal CytoplasmA = 0;
                decimal ExtracytoplasmicP = 0;
                decimal ExtracytoplasmicA = 0;
                decimal OocyteP = 0;
                decimal OocyteA = 0;

                if (txtOocyteCytoplasmDysmorphisimPresent.Text == "" || txtOocyteCytoplasmDysmorphisimPresent.Text == null)
                    CytoplasmP = 0;
                else
                    CytoplasmP = Convert.ToDecimal(txtOocyteCytoplasmDysmorphisimPresent.Text.Trim());

                if (txtOocyteCytoplasmDysmorphisimAbsent.Text == "" || txtOocyteCytoplasmDysmorphisimAbsent.Text == null)
                    CytoplasmA = 0;
                else
                    CytoplasmA = Convert.ToDecimal(txtOocyteCytoplasmDysmorphisimAbsent.Text.Trim());

                if (txtExtracytoplasmicDysmorphisimPresent.Text == "" || txtExtracytoplasmicDysmorphisimPresent.Text == null)
                    ExtracytoplasmicP = 0;
                else
                    ExtracytoplasmicP = Convert.ToDecimal(txtExtracytoplasmicDysmorphisimPresent.Text.Trim());

                if (txtExtracytoplasmicDysmorphisimAbsent.Text == "" || txtExtracytoplasmicDysmorphisimAbsent.Text == null)
                    ExtracytoplasmicA = 0;
                else
                    ExtracytoplasmicA = Convert.ToDecimal(txtExtracytoplasmicDysmorphisimAbsent.Text.Trim());

                if (txtOocyteCoronaCumulusComplexPresent.Text == "" || txtOocyteCoronaCumulusComplexPresent.Text == null)
                    OocyteP = 0;
                else
                    OocyteP = Convert.ToDecimal(txtOocyteCoronaCumulusComplexPresent.Text.Trim());

                if (txtOocyteCoronaCumulusComplexAbsent.Text == "" || txtOocyteCoronaCumulusComplexAbsent.Text == null)
                    OocyteA = 0;
                else
                    OocyteA = Convert.ToDecimal(txtOocyteCoronaCumulusComplexAbsent.Text.Trim());

                SecTotal = (Convert.ToDecimal(CytoplasmP)) + (Convert.ToDecimal(CytoplasmA)) + (Convert.ToDecimal(ExtracytoplasmicP)) + (Convert.ToDecimal(ExtracytoplasmicA)) + (Convert.ToDecimal(OocyteP)) + (Convert.ToDecimal(OocyteA));
            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);
            }
        }

        private void chkIsSetCancellation_Click(object sender, RoutedEventArgs e)
        {
            if (chkCycleCancellation.IsChecked == true)
                txtReason.IsEnabled = true;
            else
                txtReason.IsEnabled = false;
        }

        private void rdAnesthesia_Click(object sender, RoutedEventArgs e)
        {
            cmbAnesthesia.IsEnabled = true;
        }

        private void rdFlushing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtLeft_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtRight_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtReason_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbTONeedle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbTONeedle.SelectedItem).ID == 4)
            {
                rdFlushing.IsChecked = false;
                rdFlushing.IsEnabled = false;
            }
            else if (((MasterListItem)cmbTONeedle.SelectedItem).ID == 5)
            {
                rdFlushing.IsChecked = true;
                rdFlushing.IsEnabled = true;
            }
        }
    }
}
