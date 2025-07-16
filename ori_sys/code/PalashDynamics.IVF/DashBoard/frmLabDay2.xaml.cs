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
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmLabDay2 : ChildWindow
    {
        public DateTime date;
        public long embryologistID;
        public long AssiEmbryologistID;
        public long OocyteNo;
        public long SerialOocNo;
        public long Anethetist;
        public clsCoupleVO CoupleDetails;
        public event RoutedEventHandler OnSaveButton_Click;

        //added by neena
        List<MasterListItem> OocyteList = new List<MasterListItem>();
        List<MasterListItem> SelectedOocyteList = new List<MasterListItem>();
        //

        public frmLabDay2()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //FillFertilization();
            fillTreatmentForOocyte();
            FillCleavageGrade();
            fillOocyteetails();
            Labday1Date.SelectedDate =date;
            Labday1Time.Value = DateTime.Now;
            cellstageObservationDate.SelectedDate = DateTime.Now;
            cellstageObservationTime.Value = DateTime.Now;
        }

        #region "Fill Combo."
        private void fillTreatmentForOocyte()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TreatmentForOocyteMaster;
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
                    cmbTreatment.ItemsSource = null;
                    cmbTreatment.ItemsSource = objList;
                    cmbTreatment.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {

                }
                FillEmbryologist();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillEmbryologist()
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
                    cmbEmbryologist.ItemsSource = null;
                    cmbEmbryologist.ItemsSource = objList;
                    cmbAssistantEmbryologist.ItemsSource = null;
                    cmbAssistantEmbryologist.ItemsSource = objList;
                    cmbEmbryologist.SelectedValue = embryologistID;
                    cmbAssistantEmbryologist.SelectedValue = AssiEmbryologistID;

                    if (this.DataContext != null)
                    {
                       
                    }
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
                    cmbAnesthetist.ItemsSource = null;
                    cmbAnesthetist.ItemsSource = objList;
                    cmbAssistantAnesthetist.ItemsSource = null;
                    cmbAssistantAnesthetist.ItemsSource = objList;
                    cmbAnesthetist.SelectedValue = Anethetist;
                    cmbAssistantAnesthetist.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        //cmbAnesthetist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AnesthetistID;
                        //cmbAssistantAnesthetist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AssAnesthetistID;
                    }
                    fillCumulus();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillCumulus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_CumulusMaster;
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
                    cmbCumulus.ItemsSource = null;
                    cmbCumulus.ItemsSource = objList;
                    cmbCumulus.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                  
                }
                fillGrade();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillGrade()
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
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbGrade.ItemsSource = null;
                    cmbGrade.ItemsSource = objList;
                    cmbGrade.SelectedItem = objList[0];
                }

                if (this.DataContext != null)
                {
                    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                }
                fillMOI();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMOI()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_MOIMaster;
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

                    cmbMOI.ItemsSource = null;
                    cmbMOI.ItemsSource = objList;
                    cmbMOI.SelectedItem = objList[0];

                }

                FillFertilization();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillFertilization()
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
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbcellstage.ItemsSource = null;
                    cmbcellstage.ItemsSource = objList;
                    cmbcellstage.SelectedItem = objList[0];                 
                }
                fillPlan();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillCleavageGrade()
        {
            clsGetCleavageGradeMasterListBizActionVO BizAction = new clsGetCleavageGradeMasterListBizActionVO();
            BizAction.CleavageGrade = new clsCleavageGradeMasterVO();
            BizAction.CleavageGrade.ApplyTo = 2;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> list = new List<MasterListItem>();
                    list.Add(new MasterListItem(0, "-- Select --"));
                    list.AddRange(((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList);
                    cmbCleavageGrade.ItemsSource = null;
                    cmbCleavageGrade.ItemsSource = list;
                    cmbCleavageGrade.SelectedItem = list[0];
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();                     
        }
       
        private void fillPlan()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PlanMaster;
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

                    if (((clsPlanTherapyVO)TherapyDetails.DataContext).PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                    {
                        var results = from r in objList
                                      where r.ID != 4
                                      select r;
                        cmbnextplan.ItemsSource = null;
                        cmbnextplan.ItemsSource = results.ToList();
                        cmbnextplan.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbnextplan.ItemsSource = null;
                        cmbnextplan.ItemsSource = objList;
                        cmbnextplan.SelectedItem = objList[0];
                    }
                }

                if (this.DataContext != null)
                {
                  
                }
                FillFragmentation();
               
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillFragmentation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_FragmentationMaster;
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
                    cmbFragmentation.ItemsSource = null;
                    cmbFragmentation.ItemsSource = objList;
                    cmbFragmentation.SelectedItem = objList[0];   
                    
                   
                    FillBlastomereSymmetry();


                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillBlastomereSymmetry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BlastomereSymmetryMaster;
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

                    cmbBlastrometrysymtry.ItemsSource = null;
                    cmbBlastrometrysymtry.ItemsSource = objList;
                    cmbBlastrometrysymtry.SelectedItem = objList[0];
                    fillDetails();
               

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
       
        private void fillDOS()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_DOSMaster;
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

                  
                }

                if (this.DataContext != null)
                {
                  
                }
                fillPIC();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillPIC()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PICMaster;
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

                  
                }

                if (this.DataContext != null)
                {
                    
                }
            
      
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillIncubator() 
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_Incubator;
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
                
                }

                if (this.DataContext != null)
                {
                   
                }
          
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //added by neena
        List<MasterListItem> objList = new List<MasterListItem>();
        private void fillOocyteetails()
        {
            try
            {

                clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = new clsIVFDashboard_GetOPUDetailsBizActionVO();
                BizAction.Details = new clsIVFDashboard_OPUVO();
                BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
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

                                //objList.Add(new MasterListItem(0, "-- Select --"));
                                //objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                                for (int i = 1; i <= ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteRetrived; i++)
                                {
                                    if (i != OocyteNo && OocyteNo != 0)
                                    {

                                        objList.Add(new MasterListItem(i, "Oocyte No " + i));
                                    }
                                }
                                filldata();
                                //added by neena

                            }



                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        private void filldata()
        {

            clsIVFDashboard_GetDay2OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay2OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay2OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay2OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                where (c.Isfreezed == false)
                                                select c.OocyteNumber
                                                ;

                        //var missingVehicles = productFirstChars.Except(customerFirstChars);
                        //by neena
                        var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        // var missingVehicles = customerFirstChars.Except(productFirstChars);
                        //List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        //var missingVehicles = from uV in objList
                        //                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        //                      where uV.ID != de.OocyteNumber
                        //                      select uV;

                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in missingVehicles)
                        {
                            string str = "";
                            str = "Oocyte Number " + item;
                            long id = item;
                            newItem = new MasterListItem(item, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion
        long OocyteDonorID = 0;
        long OocyteDonorUnitID = 0;
         private void fillDetails() 
         {
             clsIVFDashboard_GetDay2DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay2DetailsBizActionVO();
             BizAction.Details = new clsIVFDashboard_LabDaysVO();
             BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
             BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
             
             BizAction.Details.SerialOocyteNumber = SerialOocNo;
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
             PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

             client.ProcessCompleted += (s, arg) =>
             {
                 if (arg.Error == null && arg.Result != null)
                 {
                     if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details != null)
                     {
                         this.DataContext = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details;
                         OocyteDonorID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                         OocyteDonorUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Date != null)
                         {
                             Labday1Date.SelectedDate = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Date;
                         }
                         else
                         {
                             Labday1Date.SelectedDate = date;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Time != null)
                         {
                             Labday1Time.Value = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Time;
                         }
                         else
                         {
                             Labday1Time.Value = DateTime.Now;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.EmbryologistID != null && ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.EmbryologistID != 0)
                         {
                             cmbEmbryologist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                         }

                         txtRemark.Text = Convert.ToString(((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null && ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != 0)
                         {
                             cmbAssistantEmbryologist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AnesthetistID != null && ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AnesthetistID != 0)
                         {
                             cmbAnesthetist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AnesthetistID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantAnesthetistID != null)
                         {
                             cmbAssistantAnesthetist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantAnesthetistID;
                         }

                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CumulusID!= null)
                         {
                             cmbCumulus.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CumulusID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.GradeID != null)
                         {
                             cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.GradeID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.MOIID != null)
                         {
                             cmbMOI.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.MOIID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.TreatmentID != null)
                         {
                             cmbTreatment.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.TreatmentID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellStageID != null)
                         {
                             cmbcellstage.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellStageID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FrgmentationID!= null)
                         {
                             cmbFragmentation.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FrgmentationID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BlastmereSymmetryID != null)
                         {
                             cmbBlastrometrysymtry.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BlastmereSymmetryID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.NextPlanID != null)
                         {
                             cmbnextplan.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.NextPlanID;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Photo != null)
                         {
                             MyPhoto = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Photo;
           

                             WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                             bmp.FromByteArray(MyPhoto);
                             imgPhoto.Source = bmp;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FileName != null)
                         {
                             txtFN.Text = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FileName;
                         }
                         if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                         {
                             //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                             //{
                             //    cmdNew.IsEnabled = true;
                             //    cmbnextplan.IsEnabled = false;
                             //}
                             //else
                             //{
                                  cmdNew.IsEnabled=false;
                             //}
                         }

                         //by neena

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.FertilizationID != null)
                         {
                             cmbcellstage.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.FertilizationID;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CleavageGrade != null)
                         {
                             cmbCleavageGrade.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CleavageGrade;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.TreatmentStartDate != null)
                         {
                             TreatmentStartDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.TreatmentStartDate;
                         }
                         else
                         {
                             TreatmentStartDate.SelectedDate = date;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.TreatmentEndDate != null)
                         {
                             TreatmentEndDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.TreatmentEndDate;
                         }
                         else
                         {
                             TreatmentEndDate.SelectedDate = date;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ObservationDate != null)
                         {
                             ObservationDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ObservationDate;
                         }
                         else
                         {
                             ObservationDate.SelectedDate = date;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ObservationTime != null)
                         {
                             ObservationTime.Value = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ObservationTime;
                         }
                         else
                         {
                             ObservationTime.Value = DateTime.Now;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                         {
                             cellstageObservationDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                         }
                         else
                         {
                             cellstageObservationDate.SelectedDate = date;
                         }

                         if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                         {
                             cellstageObservationTime.Value = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                         }
                         else
                         {
                             cellstageObservationTime.Value = DateTime.Now;
                         }

                       
                     }
                 }
             };
             client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
             client.CloseAsync();
         }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        byte[] MyPhoto { get; set; }
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {
                WriteableBitmap imageSource = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                try
                {
                    txtFN.Text = OpenFile.File.Name;
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    imgPhoto.Source = imageSource;



                    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                    bmp.Render(imgPhoto, new MatrixTransform());
                    bmp.Invalidate();

                    int[] p = bmp.Pixels;
                    int len = p.Length * 4;
                    byte[] result = new byte[len]; // ARGB
                    Buffer.BlockCopy(p, 0, result, 0, len);

                    MyPhoto = result;


                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }

        private bool Validate()
        {
            bool result = true;
            if (Labday1Date.SelectedDate == null)
            {
                Labday1Date.SetValidation("Please Select Date");
                Labday1Date.RaiseValidationError();
                Labday1Date.Focus();
                return false;
            }
            else
                Labday1Date.ClearValidationError();

            if (Labday1Time.Value == null)
            {
                Labday1Time.SetValidation("Please Select Time");
                Labday1Time.RaiseValidationError();
                Labday1Time.Focus();
                return false;
            }
            else
                Labday1Time.ClearValidationError();
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

            //commented by neena
            //if (cmbAnesthetist.SelectedItem == null)
            //{
            //    cmbAnesthetist.TextBox.SetValidation("Please select Anesthetist");
            //    cmbAnesthetist.TextBox.RaiseValidationError();
            //    cmbAnesthetist.Focus();
            //    result = false;
            //}
            //else if (((MasterListItem)cmbAnesthetist.SelectedItem).ID == 0)
            //{
            //    cmbAnesthetist.TextBox.SetValidation("Please select Anesthetist");
            //    cmbAnesthetist.TextBox.RaiseValidationError();
            //    cmbAnesthetist.Focus();
            //    result = false;
            //}
            //else
            //    cmbAnesthetist.TextBox.ClearValidationError();

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
            if (cmbnextplan.SelectedItem == null)
            {
                cmbnextplan.TextBox.SetValidation("Please select Next Plan");
                cmbnextplan.TextBox.RaiseValidationError();
                cmbnextplan.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbnextplan.SelectedItem).ID == 0)
            {
                cmbnextplan.TextBox.SetValidation("Please select Next Plan");
                cmbnextplan.TextBox.RaiseValidationError();
                cmbnextplan.Focus();
                result = false;
            }
            else
                cmbnextplan.TextBox.ClearValidationError();
            if (cmbcellstage.SelectedItem == null)
            {
                cmbcellstage.TextBox.SetValidation("Please select cell stage");
                cmbcellstage.TextBox.RaiseValidationError();
                cmbcellstage.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbcellstage.SelectedItem).ID == 0)
            {
                cmbcellstage.TextBox.SetValidation("Please select cell stage");
                cmbcellstage.TextBox.RaiseValidationError();
                cmbcellstage.Focus();
                result = false;
            }
            else
                cmbcellstage.TextBox.ClearValidationError();

            return result;
        }       
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public long addSerialOocNo;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();

                //-----added by neena

                OocyteList = (List<MasterListItem>)cmbApplay.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();
                //List<MasterListItem> SelectedOocyteList = new List<MasterListItem>();
                foreach (var item in OocyteList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;
                        addSerialOocNo = item.ID - OocyteNo;
                        if (addSerialOocNo == 0 || addSerialOocNo < 0)
                        {
                            addSerialOocNo = 1;
                        }


                        newItem1 = new MasterListItem(id1, str, addSerialOocNo);
                        SelectedOocyteList.Add(newItem1);
                        // SelectedOocyteList.Add(item);
                    }
                }
                //-------------

            }
        }


        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
        private void Save()
        {
            clsIVFDashboard_AddUpdateDay2BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay2BizActionVO();
            BizAction.Day2Details = new clsIVFDashboard_LabDaysVO();

            BizAction.Day2Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day2Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day2Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day2Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Day2Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
            BizAction.Day2Details.OocyteNumber = OocyteNo;
            BizAction.Day2Details.SerialOocyteNumber = SerialOocNo;
            BizAction.Day2Details.Date = Labday1Date.SelectedDate.Value.Date;

            //added by neena
            BizAction.Day2Details.OcyteListList = SelectedOocyteList;

            BizAction.Day2Details.CellObservationDate = cellstageObservationDate.SelectedDate.Value.Date;
            BizAction.Day2Details.CellObservationTime = Convert.ToDateTime(cellstageObservationTime.Value);
            if ((MasterListItem)cmbCleavageGrade.SelectedItem != null)
                BizAction.Day2Details.CleavageGrade = ((MasterListItem)cmbCleavageGrade.SelectedItem).ID;
            //

            BizAction.Day2Details.Impression = Convert.ToString(txtRemark.Text); // By BHUSHAN
            BizAction.Day2Details.Time = Convert.ToDateTime(Labday1Time.Value);
            if ((MasterListItem)cmbEmbryologist.SelectedItem != null)
                BizAction.Day2Details.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantEmbryologist.SelectedItem != null)
                BizAction.Day2Details.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbAnesthetist.SelectedItem != null)
                BizAction.Day2Details.AnesthetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantAnesthetist.SelectedItem != null && ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID > 0)
                BizAction.Day2Details.AssitantAnesthetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;
            if ((MasterListItem)cmbCumulus.SelectedItem != null)
                BizAction.Day2Details.CumulusID = ((MasterListItem)cmbCumulus.SelectedItem).ID;
            if ((MasterListItem)cmbGrade.SelectedItem != null)
                BizAction.Day2Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;
            if ((MasterListItem)cmbMOI.SelectedItem != null)
                BizAction.Day2Details.MOIID = ((MasterListItem)cmbMOI.SelectedItem).ID;
            if ((MasterListItem)cmbcellstage.SelectedItem != null)
                BizAction.Day2Details.CellStageID = ((MasterListItem)cmbcellstage.SelectedItem).ID;
            if ((MasterListItem)cmbBlastrometrysymtry.SelectedItem != null)
                BizAction.Day2Details.BlastmereSymmetryID = ((MasterListItem)cmbBlastrometrysymtry.SelectedItem).ID;
            if ((MasterListItem)cmbFragmentation.SelectedItem != null)
                BizAction.Day2Details.FrgmentationID = ((MasterListItem)cmbFragmentation.SelectedItem).ID;
            if ((MasterListItem)cmbnextplan.SelectedItem != null)
                BizAction.Day2Details.NextPlanID = ((MasterListItem)cmbnextplan.SelectedItem).ID;
            BizAction.Day2Details.OccDiamension = txtOodimension.Text;
            BizAction.Day2Details.OocytePreparationMedia = txtOoPrepMedia.Text;
            BizAction.Day2Details.SpermPreperationMedia = txtSpermPrepMedia.Text;
            BizAction.Day2Details.FinalLayering = txtFinalLayering.Text;
            if (chkFreeze.IsChecked == true)
                BizAction.Day2Details.Isfreezed = true;
            else
                BizAction.Day2Details.Isfreezed = false;
            BizAction.Day2Details.Photo = MyPhoto;
            BizAction.Day2Details.OtherDetails = txtothrdetails.Text;
            BizAction.Day2Details.FileName = txtFN.Text;
            BizAction.Day2Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day2Details.OocyteDonorUnitID = OocyteDonorUnitID;
          

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
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
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
    }
}

