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

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmGraphicalRepresentationofLabDays : UserControl
    {
        long EmbryologistID = 0;
        long AssiEmbryologistID=0;
        public  DateTime StartDate;
        DateTime DisplayDate;
        public long PlannedNoOfEmb;
        long Anethetist = 0;
        public bool ISSetForED;
        public long OocyteforED;
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long PlannedTreatmentID;
        WaitIndicator wait = new WaitIndicator();

        
        public frmGraphicalRepresentationofLabDays()
        {
            InitializeComponent();
            this.DataContext = null;
            //this.Loaded += new RoutedEventHandler(UserControl_Loaded);
           
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
            if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID)
            {
                GetOocyteFromDonor();
            }
            else if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.EmbryoReceipentID)
            {
                fillGrid();
            }
            else
            {
                fillOPUDetails();
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
                            for (int i = 1; i < dgDayList.Columns.Count; i++)
                            {
                                dgDayList.Columns[i].Header = "Day" + (i - 1) + "(" + DisplayDate.ToString("dd/MM/yyyy") + ")";
                                DisplayDate = DisplayDate.AddDays(1);
                               // DataGridColumn column = dgDayList.Columns[i];
                               // FrameworkElement fe = column.GetCellContent(i);
                               //// FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                //if (result != null)
                                //{
                                //    DataGridCell cell = (DataGridCell)result;
                                //    cell.IsEnabled = false;
                                    
                                //}
                            }
                            dgDayList.ItemsSource = null;
                            dgDayList.ItemsSource = ((cls_IVFDashboar_GetGraphicalRepBizActionVO)arg.Result).GraphicalOocList;
                            GetTherapyDetails();
                            //
                        
                            //
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
           
        }

        private void dgDayList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            
        }
            

        private void btnLavDay1_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay1 Day1frm = new frmLabDay1();
            Day1frm.Title = "Embryology - Lab Day 1 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber ;
            //Day1frm.Title = "IVF Lab Day 1(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day1frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day1frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day1frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day1frm.CoupleDetails = CoupleDetails;
            Day1frm.date = StartDate.AddDays(1);
            Day1frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day1frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day1frm.embryologistID = EmbryologistID;
            Day1frm.AssiEmbryologistID = AssiEmbryologistID;
            Day1frm.Anethetist = Anethetist;
            Day1frm.Show();
        }

        private void btnLabDay0_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay0 Day0Frm = new frmLabDay0();
            Day0Frm.Title = "Embryology - Lab Day 0 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber ;           
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
            Day0Frm.Show();
        }
        void Win_OnCloseButton_Click(object sender,  EventArgs e)
        {
            fillGrid();
        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            fillGrid();
        }

        private void btnLabDay2_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay2 Day2Frm = new frmLabDay2();
            Day2Frm.Title = "Embryology - Lab Day 2 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.Title = "IVF Lab Day 2(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day2Frm.CoupleDetails = CoupleDetails;
            Day2Frm.date = StartDate.AddDays(2);
            Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day2Frm.embryologistID = EmbryologistID;
            Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day2Frm.Anethetist = Anethetist;
            Day2Frm.Show();
        }

        private void btnLabDay6_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay6 Day2Frm = new frmLabDay6();
            Day2Frm.Title = "Embryology - Lab Day 6 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber ;
            //Day2Frm.Title = "IVF Lab Day 6(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day2Frm.CoupleDetails = CoupleDetails;
            Day2Frm.date = StartDate.AddDays(6);
            Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day2Frm.embryologistID = EmbryologistID;
            Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day2Frm.Anethetist = Anethetist;
            Day2Frm.Show();
        }

        private void btnLabDay5_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay5 Day2Frm = new frmLabDay5();
            Day2Frm.Title = "Embryology - Lab Day 5 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber ;
            //Day2Frm.Title = "IVF Lab Day 5(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day2Frm.CoupleDetails = CoupleDetails;
            Day2Frm.date = StartDate.AddDays(5);
            Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day2Frm.embryologistID = EmbryologistID;
            Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day2Frm.Anethetist = Anethetist;
            Day2Frm.Show();
        }

        private void btnLabDay4_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay4 Day2Frm = new frmLabDay4();
            Day2Frm.Title = "Embryology - Lab Day 4 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber ;
            //Day2Frm.Title = "IVF Lab Day 4(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day2Frm.CoupleDetails = CoupleDetails;
            Day2Frm.date = StartDate.AddDays(4);
            Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day2Frm.embryologistID = EmbryologistID;
            Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day2Frm.Anethetist = Anethetist;
            Day2Frm.Show();
        }

        private void btnLabDay3_Click(object sender, RoutedEventArgs e)
        {
            frmLabDay3 Day2Frm = new frmLabDay3();
            Day2Frm.Title = "Embryology - Lab Day 3 - Oocyte/Embryo No. -" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            //Day2Frm.Title = "IVF Lab Day 3(Ooc No. :-" + ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber + ")";
            Day2Frm.OocyteNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).OocyteNumber;
            Day2Frm.SerialOocNo = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SerialOocyteNumber;
            Day2Frm.TherapyDetails.DataContext = (clsPlanTherapyVO)this.DataContext;
            Day2Frm.CoupleDetails = CoupleDetails;
            Day2Frm.date = StartDate.AddDays(3);
            Day2Frm.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Day2Frm.Closed += new EventHandler(Win_OnCloseButton_Click);
            Day2Frm.embryologistID = EmbryologistID;
            Day2Frm.AssiEmbryologistID = AssiEmbryologistID;
            Day2Frm.Anethetist = Anethetist;
            Day2Frm.Show();
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


    }
   
}
