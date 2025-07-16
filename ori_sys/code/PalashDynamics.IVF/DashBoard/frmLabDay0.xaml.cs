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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.IO;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmLabDay0 : ChildWindow
    {
        public DateTime date;
        public long embryologistID;
        public long AssiEmbryologistID;
        public long OocyteNo;
        public long SerialOocNo;
        public long Anethetist;
        public clsCoupleVO CoupleDetails;
        public event RoutedEventHandler OnSaveButton_Click;
        public DateTime? OPUDate;
        public DateTime? OPUTime;
        public long SourceOfSperm = 0;
        //added by neena dated 18/5/16
        public long addSerialOocNo;
        List<MasterListItem> OocyteList = new List<MasterListItem>();
        List<MasterListItem> SelectedOocyteList = new List<MasterListItem>();
        public long PlannedTreatmentID;
        public bool IsClosed;
        //

        public frmLabDay0()
        {
            InitializeComponent();
        }

        #region "Fill Combo."

        private void fillSpermCollection()
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
                    cmbSpermMethod.ItemsSource = null;
                    cmbSpermMethod.ItemsSource = objList;
                    cmbSpermMethod.SelectedValue = (long)0;
                    cmbSpermMethodICSI.ItemsSource = null;
                    cmbSpermMethodICSI.ItemsSource = objList;
                    cmbSpermMethodICSI.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillPlan();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }



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
                    //cmbEmbryologist.SelectedItem = objList[0];
                    //cmbAssistantEmbryologist.SelectedItem = objList[0];
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
                    //cmbAnesthetist.SelectedItem = objList[0];

                    cmbAssistantAnesthetist.ItemsSource = null;
                    cmbAssistantAnesthetist.ItemsSource = objList;
                    cmbAssistantAnesthetist.SelectedItem = objList[0];
                    cmbAnesthetist.SelectedValue = Anethetist;

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
                                      where r.ID != 2
                                      select r;
                        cmbnextplan.ItemsSource = null;
                        cmbnextplan.ItemsSource = results.ToList();
                        cmbnextplan.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        var results = from r in objList
                                      where r.ID != 8 && r.ID != 9
                                      select r;
                        cmbnextplan.ItemsSource = null;
                        cmbnextplan.ItemsSource = results.ToList();
                        cmbnextplan.SelectedItem = results.ToList()[0];
                    }
                }

                if (this.DataContext != null)
                {

                }
                fillSemenSampleList();

                //fillIncubator(); // commented by neena
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

                    cmbDOS.ItemsSource = null;
                    cmbDOS.ItemsSource = objList;
                    cmbDOS.SelectedItem = objList[0];
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

                    cmbPIC.ItemsSource = null;
                    cmbPIC.ItemsSource = objList;
                    cmbPIC.SelectedItem = objList[0];
                }

                if (this.DataContext != null)
                {
                }
                fillDetails();
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
                    cmbIncubator.ItemsSource = null;
                    cmbIncubator.ItemsSource = objList;
                    cmbIncubator.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {

                }
                fillDOS();
                //fillDetails();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //fillTreatmentForOocyte();

            //fillGroupbox(((clsPlanTherapyVO)TherapyDetails.DataContext).PlannedTreatmentID); // For groupbox Visbility       //commented by neena     
            Labday0Date.SelectedDate = date;
            Labday0Time.Value = DateTime.Now;
            TreatmentStartDate.SelectedDate = DateTime.Now;
            TreatmentEndDate.SelectedDate = DateTime.Now;
            ObservationDate.SelectedDate = DateTime.Now;
            ObservationTime.Value = DateTime.Now;


            //imgPhoto.Height = 150;
            //imgPhoto.Width = 150;
            //imgPhoto2.Height = 150;
            //imgPhoto2.Width = 150;

            //added by neena
            GetProcedureDate();
            fillOocyteMaturity();
            if (((clsPlanTherapyVO)TherapyDetails.DataContext).PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteReceipentID)
                fillOocyteetailsForOocyteReceipent();
            else
                fillOocyteetails();

            if (IsClosed)
                cmdNew.IsEnabled = false;
            //

        }

        private void GetProcedureDate()
        {
            try
            {
                clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay0DetailsBizActionVO();
                BizAction.IsGetDate = true;
                BizAction.Details = new clsIVFDashboard_LabDaysVO();
                BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate != null)
                            {
                                dtProcedureDate.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate != null)
                            {
                                dtProcedureTime.Value = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate;
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

        List<MasterListItem> objListSource = null;
        private void fillSourceOfSperm()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SpermSource;
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
                    objListSource = new List<MasterListItem>();
                    objListSource.Add(new MasterListItem(0, "-- Select --"));
                    objListSource.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbSource.ItemsSource = null;
                    cmbSource.ItemsSource = objListSource;
                    cmbSource.SelectedItem = objListSource[0];
                    cmbSourceICSI.ItemsSource = null;
                    cmbSourceICSI.ItemsSource = objListSource;
                    cmbSourceICSI.SelectedItem = objListSource[0];
                }

                if (SourceOfSperm > 0)
                {
                    if (SourceOfSperm == 3)
                    {
                        List<MasterListItem> objList1 = new List<MasterListItem>();
                        objList1.AddRange(objListSource.Where(x => x.ID != 3).ToList());
                        cmbSource.ItemsSource = null;
                        cmbSource.ItemsSource = objList1;
                        cmbSource.SelectedItem = objList1[0];
                        cmbSourceICSI.ItemsSource = null;
                        cmbSourceICSI.ItemsSource = objList1;
                        cmbSourceICSI.SelectedItem = objList1[0];
                    }
                    else
                    {
                        cmbSource.SelectedValue = SourceOfSperm;
                        cmbSourceICSI.SelectedValue = SourceOfSperm;
                    }
                }
                fillSpermCollection();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

            //List<MasterListItem> mlSourceOfSperm = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //mlSourceOfSperm.Insert(0, Default);
            //EnumToList(typeof(SourceOfSperm), mlSourceOfSperm);
            //cmbSource.ItemsSource = mlSourceOfSperm;
            //cmbSource.SelectedItem = Default;
            // cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;

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

        private void fillOocyteMaturity()
        {
            List<MasterListItem> mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteMaturity), mlSourceOfSperm);
            cmbOocyteMaturity.ItemsSource = mlSourceOfSperm;
            cmbOocyteMaturity.SelectedItem = Default;

            FillICSICOmboDetails();

            //List<MasterListItem> ObjOocyteList = new List<MasterListItem>();
            //ObjOocyteList.Add(new MasterListItem(0, "--Select--"));
            //ObjOocyteList.Add(new MasterListItem(1, "GV"));
            //ObjOocyteList.Add(new MasterListItem(2, "M I"));
            //ObjOocyteList.Add(new MasterListItem(3, "M II"));
            //cmbOocyteMaturity.ItemsSource = null;
            //cmbOocyteMaturity.ItemsSource = ObjOocyteList;
            //cmbOocyteMaturity.SelectedItem = ObjOocyteList[0];
        }

        private void FillICSICOmboDetails()
        {
            List<MasterListItem> ICSIDetailsList = new List<MasterListItem>();
            ICSIDetailsList.Add(new MasterListItem(0, "- Select -"));
            ICSIDetailsList.Add(new MasterListItem(1, "Normal"));
            ICSIDetailsList.Add(new MasterListItem(2, "Abnormal"));
            cmbOocyteZona.ItemsSource = null;
            cmbOocyteZona.ItemsSource = ICSIDetailsList;
            cmbOocyteZona.SelectedItem = ICSIDetailsList[1];

            cmbPVS.ItemsSource = null;
            cmbPVS.ItemsSource = ICSIDetailsList;
            cmbPVS.SelectedItem = ICSIDetailsList[1];

            cmbIstPB.ItemsSource = null;
            cmbIstPB.ItemsSource = ICSIDetailsList;
            cmbIstPB.SelectedItem = ICSIDetailsList[1];

            cmbCytoplasm.ItemsSource = null;
            cmbCytoplasm.ItemsSource = ICSIDetailsList;
            cmbCytoplasm.SelectedItem = ICSIDetailsList[1];

            fillComboDetails();
        }

        private void fillComboDetails()
        {
            List<MasterListItem> mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteCytoplasmDysmorphisim), mlSourceOfSperm);
            cmbOocyteCytoplasmDysmorphisim.ItemsSource = mlSourceOfSperm;
            cmbOocyteCytoplasmDysmorphisim.SelectedItem = Default;
            cmbExtracytoplasmicDysmorphisim.ItemsSource = mlSourceOfSperm;
            cmbExtracytoplasmicDysmorphisim.SelectedItem = Default;

            fillComboDetails1();
            //List<MasterListItem> ObjComboDetailsList = new List<MasterListItem>();
            //ObjComboDetailsList.Add(new MasterListItem(0, "--Select--"));
            //ObjComboDetailsList.Add(new MasterListItem(1, "Present"));
            //ObjComboDetailsList.Add(new MasterListItem(2, "Absent"));
            //cmbOocyteCytoplasmDysmorphisim.ItemsSource = null;
            //cmbOocyteCytoplasmDysmorphisim.ItemsSource = ObjComboDetailsList;
            //cmbOocyteCytoplasmDysmorphisim.SelectedItem = ObjComboDetailsList[0];
            //cmbExtracytoplasmicDysmorphisim.ItemsSource = null;
            //cmbExtracytoplasmicDysmorphisim.ItemsSource = ObjComboDetailsList;
            //cmbExtracytoplasmicDysmorphisim.SelectedItem = ObjComboDetailsList[0];
        }

        private void fillComboDetails1()
        {
            List<MasterListItem> mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteCoronaCumulusComplex), mlSourceOfSperm);
            cmbOocyteCoronaCumulusComplex.ItemsSource = mlSourceOfSperm;
            cmbOocyteCoronaCumulusComplex.SelectedItem = Default;

            fillSourceOfSperm();
            //List<MasterListItem> ObjComboDetailsList = new List<MasterListItem>();
            //ObjComboDetailsList.Add(new MasterListItem(0, "--Select--"));
            //ObjComboDetailsList.Add(new MasterListItem(1, "Normal"));
            //ObjComboDetailsList.Add(new MasterListItem(2, "Abnormal"));
            //cmbOocyteCoronaCumulusComplex.ItemsSource = null;
            //cmbOocyteCoronaCumulusComplex.ItemsSource = ObjComboDetailsList;
            //cmbOocyteCoronaCumulusComplex.SelectedItem = ObjComboDetailsList[0];
        }

        private void FillIndex()
        {
            List<MasterListItem> ObjIndex = new List<MasterListItem>();
            ObjIndex.Add(new MasterListItem(0, "--Select--"));
            ObjIndex.Add(new MasterListItem(1, "1"));
            ObjIndex.Add(new MasterListItem(2, "2"));
            ObjIndex.Add(new MasterListItem(3, "3"));
            ObjIndex.Add(new MasterListItem(4, "4"));
            ObjIndex.Add(new MasterListItem(5, "5"));
            ObjIndex.Add(new MasterListItem(6, "6"));
            ObjIndex.Add(new MasterListItem(7, "7"));
            ObjIndex.Add(new MasterListItem(8, "8"));
            ObjIndex.Add(new MasterListItem(9, "9"));
            ObjIndex.Add(new MasterListItem(10, "10"));
            //cmbIndex1.ItemsSource = null;
            //cmbIndex1.ItemsSource = ObjIndex;
            //cmbIndex1.SelectedItem = ObjIndex[0];
        }

        void fillGroupbox(long PlanTherapyID)
        {
            switch (PlanTherapyID)
            {
                case 1:
                    grpboxICSI.Visibility = Visibility.Collapsed;
                    grpboxIVF.Visibility = Visibility.Visible;
                    break;
                case 2:
                    grpboxICSI.Visibility = Visibility.Visible;
                    grpboxIVF.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    grpboxICSI.Visibility = Visibility.Visible;
                    grpboxIVF.Visibility = Visibility.Visible;
                    break;
            }
        }

        private bool Validate()
        {
            bool result = true;


            //if (Labday0Date.SelectedDate == null)
            //{
            //    Labday0Date.SetValidation("Please Select Date");
            //    Labday0Date.RaiseValidationError();
            //    Labday0Date.Focus();
            //    return false;
            //}
            //else
            //    Labday0Date.ClearValidationError();

            //if (Labday0Time.Value == null)
            //{
            //    Labday0Time.SetValidation("Please Select Time");
            //    Labday0Time.RaiseValidationError();
            //    Labday0Time.Focus();
            //    return false;
            //}
            //else
            //    Labday0Time.ClearValidationError();

            //if (cmbTreatment.ItemsSource != null)
            //{
            //    if (cmbTreatment.SelectedItem == null)
            //    {
            //        cmbTreatment.TextBox.SetValidation("Please select Treatment");
            //        cmbTreatment.TextBox.RaiseValidationError();
            //        cmbTreatment.Focus();
            //        result = false;
            //    }
            //    else if (((MasterListItem)cmbTreatment.SelectedItem).ID == 0)
            //    {
            //        cmbTreatment.TextBox.SetValidation("Please select Treatment");
            //        cmbTreatment.TextBox.RaiseValidationError();
            //        cmbTreatment.Focus();
            //        result = false;
            //    }
            //    else
            //        cmbTreatment.TextBox.ClearValidationError();
            //}

            //if (cmbEmbryologist.ItemsSource != null)
            //{
            //    if (cmbEmbryologist.SelectedItem == null)
            //    {
            //        cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
            //        cmbEmbryologist.TextBox.RaiseValidationError();
            //        cmbEmbryologist.Focus();
            //        result = false;
            //    }
            //    else if (((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            //    {
            //        cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
            //        cmbEmbryologist.TextBox.RaiseValidationError();
            //        cmbEmbryologist.Focus();
            //        result = false;
            //    }
            //    else
            //        cmbEmbryologist.TextBox.ClearValidationError();
            //}
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


            if (cmbnextplan.ItemsSource != null)
            {
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
            }

            //if (cmbcellstage.ItemsSource != null)
            //{
            //    if (((clsPlanTherapyVO)TherapyDetails.DataContext).PlannedTreatmentID == 1 || ((clsPlanTherapyVO)TherapyDetails.DataContext).PlannedTreatmentID == 3)
            //    {
            //        if (cmbcellstage.SelectedItem == null)
            //        {
            //            cmbcellstage.TextBox.SetValidation("Please select cell stage");
            //            cmbcellstage.TextBox.RaiseValidationError();
            //            cmbcellstage.Focus();
            //            result = false;
            //        }
            //        else if (((MasterListItem)cmbcellstage.SelectedItem).ID == 0)
            //        {
            //            cmbcellstage.TextBox.SetValidation("Please select cell stage");
            //            cmbcellstage.TextBox.RaiseValidationError();
            //            cmbcellstage.Focus();
            //            result = false;
            //        }
            //        else
            //            cmbcellstage.TextBox.ClearValidationError();
            //    }
            //}

            if (cmbOocyteMaturity.ItemsSource != null)
            {
                if (cmbOocyteMaturity.SelectedItem == null)
                {
                    cmbOocyteMaturity.TextBox.SetValidation("Please select Oocyte Maturity");
                    cmbOocyteMaturity.TextBox.RaiseValidationError();
                    cmbOocyteMaturity.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbOocyteMaturity.SelectedItem).ID == 0)
                {
                    cmbOocyteMaturity.TextBox.SetValidation("Please select Oocyte Maturity");
                    cmbOocyteMaturity.TextBox.RaiseValidationError();
                    cmbOocyteMaturity.Focus();
                    result = false;
                }
                else
                    cmbOocyteMaturity.TextBox.ClearValidationError();
            }

            if (cmbOocyteCytoplasmDysmorphisim.ItemsSource != null)
            {
                if (cmbOocyteCytoplasmDysmorphisim.SelectedItem == null)
                {
                    cmbOocyteCytoplasmDysmorphisim.TextBox.SetValidation("Please select Oocyte Cytoplasm Dysmorphisim");
                    cmbOocyteCytoplasmDysmorphisim.TextBox.RaiseValidationError();
                    cmbOocyteCytoplasmDysmorphisim.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbOocyteCytoplasmDysmorphisim.SelectedItem).ID == 0)
                {
                    cmbOocyteCytoplasmDysmorphisim.TextBox.SetValidation("Please select Oocyte Cytoplasm Dysmorphisim");
                    cmbOocyteCytoplasmDysmorphisim.TextBox.RaiseValidationError();
                    cmbOocyteCytoplasmDysmorphisim.Focus();
                    result = false;
                }
                else
                    cmbOocyteCytoplasmDysmorphisim.TextBox.ClearValidationError();
            }

            if (cmbExtracytoplasmicDysmorphisim.ItemsSource != null)
            {
                if (cmbExtracytoplasmicDysmorphisim.SelectedItem == null)
                {
                    cmbExtracytoplasmicDysmorphisim.TextBox.SetValidation("Please select Extracytoplasmic Dysmorphisim");
                    cmbExtracytoplasmicDysmorphisim.TextBox.RaiseValidationError();
                    cmbExtracytoplasmicDysmorphisim.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbExtracytoplasmicDysmorphisim.SelectedItem).ID == 0)
                {
                    cmbExtracytoplasmicDysmorphisim.TextBox.SetValidation("Please select Extracytoplasmic Dysmorphisim");
                    cmbExtracytoplasmicDysmorphisim.TextBox.RaiseValidationError();
                    cmbExtracytoplasmicDysmorphisim.Focus();
                    result = false;
                }
                else
                    cmbExtracytoplasmicDysmorphisim.TextBox.ClearValidationError();
            }

            if (cmbOocyteCoronaCumulusComplex.ItemsSource != null)
            {
                if (cmbOocyteCoronaCumulusComplex.SelectedItem == null)
                {
                    cmbOocyteCoronaCumulusComplex.TextBox.SetValidation("Please select Oocyte Corona Cumulus Complex");
                    cmbOocyteCoronaCumulusComplex.TextBox.RaiseValidationError();
                    cmbOocyteCoronaCumulusComplex.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbOocyteCoronaCumulusComplex.SelectedItem).ID == 0)
                {
                    cmbOocyteCoronaCumulusComplex.TextBox.SetValidation("Please select Oocyte Corona Cumulus Complex");
                    cmbOocyteCoronaCumulusComplex.TextBox.RaiseValidationError();
                    cmbOocyteCoronaCumulusComplex.Focus();
                    result = false;
                }
                else
                    cmbOocyteCoronaCumulusComplex.TextBox.ClearValidationError();
            }

            if (PlannedTreatmentID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
            {
                if (((MasterListItem)cmbnextplan.SelectedItem).ID == 8 || ((MasterListItem)cmbnextplan.SelectedItem).ID == 9)
                {
                    if (txtSelectedPatientName.Text.Trim() == string.Empty)
                    {
                        txtSelectedPatientName.SetValidation("Please Select Recepient ");
                        txtSelectedPatientName.RaiseValidationError();
                        txtSelectedPatientName.Focus();
                        result = false;
                    }
                    else
                        txtSelectedPatientName.ClearValidationError();

                    if (txtSelectedMrNO.Text.Trim() == string.Empty)
                    {
                        txtSelectedMrNO.SetValidation("Please Select Recepient ");
                        txtSelectedMrNO.RaiseValidationError();
                        txtSelectedMrNO.Focus();
                        result = false;
                    }
                    else
                        txtSelectedMrNO.ClearValidationError();
                }
            }

            if (grpboxIVF.Visibility == Visibility.Visible)
            {
                if (dtProcedureDate.SelectedDate == null)
                {
                    dtProcedureDate.SetValidation("Please Select Date");
                    dtProcedureDate.RaiseValidationError();
                    dtProcedureDate.Focus();
                    return false;
                }
                else
                    dtProcedureDate.ClearValidationError();

                if (dtProcedureTime.Value == null)
                {
                    dtProcedureTime.SetValidation("Please Select Time");
                    dtProcedureTime.RaiseValidationError();
                    dtProcedureTime.Focus();
                    return false;
                }
                else
                    dtProcedureTime.ClearValidationError();

                if (cmbSpermMethod.ItemsSource != null)
                {
                    if (cmbSpermMethod.SelectedItem == null)
                    {
                        cmbSpermMethod.TextBox.SetValidation("Please select Sperm Collection Method");
                        cmbSpermMethod.TextBox.RaiseValidationError();
                        cmbSpermMethod.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbSpermMethod.SelectedItem).ID == 0)
                    {
                        cmbSpermMethod.TextBox.SetValidation("Please select Sperm Collection Method");
                        cmbSpermMethod.TextBox.RaiseValidationError();
                        cmbSpermMethod.Focus();
                        result = false;
                    }
                    else
                        cmbSpermMethod.TextBox.ClearValidationError();
                }

                if (cmbSource.ItemsSource != null)
                {
                    if (cmbSource.SelectedItem == null)
                    {
                        cmbSource.TextBox.SetValidation("Please select Source of Sperm");
                        cmbSource.TextBox.RaiseValidationError();
                        cmbSource.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbSource.SelectedItem).ID == 0)
                    {
                        cmbSource.TextBox.SetValidation("Please select Source of Sperm");
                        cmbSource.TextBox.RaiseValidationError();
                        cmbSource.Focus();
                        result = false;
                    }
                    else
                        cmbSource.TextBox.ClearValidationError();
                }

                if (dtProcedureDate.SelectedDate < OPUDate)
                {
                    dtProcedureDate.SetValidation("Procedure Date Cannot Be Less Than OPU Date");
                    dtProcedureDate.RaiseValidationError();
                    dtProcedureDate.Focus();
                    dtProcedureDate.Text = " ";
                    dtProcedureDate.Text = OPUDate.ToString();
                    result = false;
                }
                else
                    dtProcedureDate.ClearValidationError();

            }

            if (grpboxICSI.Visibility == Visibility.Visible)
            {
                if (dtProcedureDateICSI.SelectedDate == null)
                {
                    dtProcedureDateICSI.SetValidation("Please Select Date");
                    dtProcedureDateICSI.RaiseValidationError();
                    dtProcedureDateICSI.Focus();
                    return false;
                }
                else
                    dtProcedureDateICSI.ClearValidationError();

                if (dtProcedureTimeICSI.Value == null)
                {
                    dtProcedureTimeICSI.SetValidation("Please Select Time");
                    dtProcedureTimeICSI.RaiseValidationError();
                    dtProcedureTimeICSI.Focus();
                    return false;
                }
                else
                    dtProcedureTimeICSI.ClearValidationError();

                if (cmbSpermMethodICSI.ItemsSource != null)
                {
                    if (cmbSpermMethodICSI.SelectedItem == null)
                    {
                        cmbSpermMethodICSI.TextBox.SetValidation("Please select Sperm Collection Method");
                        cmbSpermMethodICSI.TextBox.RaiseValidationError();
                        cmbSpermMethodICSI.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbSpermMethodICSI.SelectedItem).ID == 0)
                    {
                        cmbSpermMethodICSI.TextBox.SetValidation("Please select Sperm Collection Method");
                        cmbSpermMethodICSI.TextBox.RaiseValidationError();
                        cmbSpermMethodICSI.Focus();
                        result = false;
                    }
                    else
                        cmbSpermMethodICSI.TextBox.ClearValidationError();
                }

                if (cmbSourceICSI.ItemsSource != null)
                {
                    if (cmbSourceICSI.SelectedItem == null)
                    {
                        cmbSourceICSI.TextBox.SetValidation("Please select Source of Sperm");
                        cmbSourceICSI.TextBox.RaiseValidationError();
                        cmbSourceICSI.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbSourceICSI.SelectedItem).ID == 0)
                    {
                        cmbSourceICSI.TextBox.SetValidation("Please select Source of Sperm");
                        cmbSourceICSI.TextBox.RaiseValidationError();
                        cmbSourceICSI.Focus();
                        result = false;
                    }
                    else
                        cmbSourceICSI.TextBox.ClearValidationError();
                }

                if (dtProcedureDateICSI.SelectedDate < OPUDate)
                {
                    dtProcedureDateICSI.SetValidation("Procedure Date Cannot Be Less Than OPU Date");
                    dtProcedureDateICSI.RaiseValidationError();
                    dtProcedureDateICSI.Focus();
                    dtProcedureDateICSI.Text = " ";
                    dtProcedureDateICSI.Text = OPUDate.ToString();
                    result = false;
                }
                else
                    dtProcedureDateICSI.ClearValidationError();

            }

            //if (result == true)
            //{
            //    if (mylistitem.Count == 0)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgWindow =
            //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add image.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //        result = false;
            //    }
            //}

            //if (CurrentObervationDateTime < OPUDateTime)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //              new MessageBoxControl.MessageBoxChildWindow("Palash", "You can not save Lab Day 0 details before OPU date.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //    msgW.Show();
            //    result = false;
            //}

            return result;
        }

        double hours;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                DateTime CurrentObervationDateTime = new DateTime();
                DateTime OPUDateTime = ((DateTime)OPUDate).Date.Add(((DateTime)OPUTime).TimeOfDay);
                if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("IVF"))
                    CurrentObervationDateTime = ((DateTime)dtProcedureDate.SelectedDate).Date.Add(((DateTime)dtProcedureTime.Value).TimeOfDay);
                else if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("ICSI"))
                    CurrentObervationDateTime = ((DateTime)dtProcedureDateICSI.SelectedDate).Date.Add(((DateTime)dtProcedureTimeICSI.Value).TimeOfDay);

                TimeSpan diff = CurrentObervationDateTime - OPUDateTime;
                hours = diff.TotalHours;

                if (hours > 4)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Procedure hours limit is Crossed.. Do you want to continue.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 0 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }

                //-----added by neena dated 18/5/16

                OocyteList = (List<MasterListItem>)cmbApplay.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();

                foreach (var item in OocyteList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;
                        addSerialOocNo = item.ID - OocyteNo;
                        if (addSerialOocNo == 0 ) //|| addSerialOocNo < 0)
                        {
                            addSerialOocNo = 1;
                        }

                        newItem1 = new MasterListItem(id1, str, addSerialOocNo);
                        SelectedOocyteList.Add(newItem1);
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

        List<clsAddImageVO> ImageList = new List<clsAddImageVO>();
        private void Save()
        {
            clsIVFDashboard_AddUpdateDay0BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay0BizActionVO();
            BizAction.Day0Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Day0Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day0Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day0Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day0Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Day0Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;

            //added by neena dated 18/5/16
            addSerialOocNo = 0;
            BizAction.Day0Details.OcyteListList = SelectedOocyteList;
            BizAction.Day0Details.TreatmentStartDate = TreatmentStartDate.SelectedDate.Value.Date;
            BizAction.Day0Details.TreatmentEndDate = TreatmentEndDate.SelectedDate.Value.Date;
            BizAction.Day0Details.ObservationDate = ObservationDate.SelectedDate.Value.Date;
            BizAction.Day0Details.ObservationTime = Convert.ToDateTime(ObservationTime.Value);

            BizAction.Day0Details.OocyteNumber = OocyteNo;
            BizAction.Day0Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day0Details.Date = OPUDate;
            BizAction.Day0Details.Date = BizAction.Day0Details.Date.Value.Add(OPUTime.Value.TimeOfDay);


            //BizAction.Day0Details.Date = Labday0Date.SelectedDate.Value.Date;
            BizAction.Day0Details.Time = Convert.ToDateTime(Labday0Time.Value);
            BizAction.Day0Details.Impression = Convert.ToString(txtImpression.Text);
            if ((MasterListItem)cmbEmbryologist.SelectedItem != null)
                BizAction.Day0Details.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantEmbryologist.SelectedItem != null)
                BizAction.Day0Details.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
            if ((MasterListItem)cmbAnesthetist.SelectedItem != null)
                BizAction.Day0Details.AnesthetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;
            if ((MasterListItem)cmbAssistantAnesthetist.SelectedItem != null && ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID > 0)
                BizAction.Day0Details.AssitantAnesthetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;
            if ((MasterListItem)cmbCumulus.SelectedItem != null)
                BizAction.Day0Details.CumulusID = ((MasterListItem)cmbCumulus.SelectedItem).ID;
            if ((MasterListItem)cmbGrade.SelectedItem != null)
                BizAction.Day0Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;
            if ((MasterListItem)cmbMOI.SelectedItem != null)
                BizAction.Day0Details.MOIID = ((MasterListItem)cmbMOI.SelectedItem).ID;
            if ((MasterListItem)cmbcellstage.SelectedItem != null)
                BizAction.Day0Details.CellStageID = ((MasterListItem)cmbcellstage.SelectedItem).ID;
            if ((MasterListItem)cmbIncubator.SelectedItem != null)
                BizAction.Day0Details.IncubatorID = ((MasterListItem)cmbIncubator.SelectedItem).ID;
            if ((MasterListItem)cmbnextplan.SelectedItem != null)
                BizAction.Day0Details.NextPlanID = ((MasterListItem)cmbnextplan.SelectedItem).ID;
            BizAction.Day0Details.OccDiamension = txtOodimension.Text;
            BizAction.Day0Details.OocytePreparationMedia = txtOoPrepMedia.Text;
            BizAction.Day0Details.SpermPreperationMedia = txtSpermPrepMedia.Text;
            BizAction.Day0Details.FinalLayering = txtFinalLayering.Text;
            if (chkFreeze.IsChecked == true)
                BizAction.Day0Details.Isfreezed = true;
            else
                BizAction.Day0Details.Isfreezed = false;
            //BizAction.Day0Details.Photo = MyPhoto;
            //BizAction.Day0Details.FileName = txtFN.Text;
            if ((MasterListItem)cmbDOS.SelectedItem != null)
                BizAction.Day0Details.DOSID = ((MasterListItem)cmbDOS.SelectedItem).ID;
            if ((MasterListItem)cmbPIC.SelectedItem != null)
                BizAction.Day0Details.PICID = ((MasterListItem)cmbPIC.SelectedItem).ID;
            BizAction.Day0Details.MBD = txtMBD.Text;
            BizAction.Day0Details.IC = txtIC.Text;
            BizAction.Day0Details.Comment = txtComments.Text;

            BizAction.Day0Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day0Details.OocyteDonorUnitID = OocyteDonorUnitID;


            if ((MasterListItem)cmbTreatment.SelectedItem != null)
                BizAction.Day0Details.TreatmentID = ((MasterListItem)cmbTreatment.SelectedItem).ID;

            //added by neena
            BizAction.Day0Details.DiscardReason = txtDiscardReason.Text;
            BizAction.Day0Details.IsFreezeOocytes = true;

            if ((MasterListItem)cmbOocyteMaturity.SelectedItem != null)
                BizAction.Day0Details.OocyteMaturityID = ((MasterListItem)cmbOocyteMaturity.SelectedItem).ID;

            if ((MasterListItem)cmbOocyteCytoplasmDysmorphisim.SelectedItem != null)
                BizAction.Day0Details.OocyteCytoplasmDysmorphisim = ((MasterListItem)cmbOocyteCytoplasmDysmorphisim.SelectedItem).ID;

            if ((MasterListItem)cmbExtracytoplasmicDysmorphisim.SelectedItem != null)
                BizAction.Day0Details.ExtracytoplasmicDysmorphisim = ((MasterListItem)cmbExtracytoplasmicDysmorphisim.SelectedItem).ID;

            if ((MasterListItem)cmbOocyteCoronaCumulusComplex.SelectedItem != null)
                BizAction.Day0Details.OocyteCoronaCumulusComplex = ((MasterListItem)cmbOocyteCoronaCumulusComplex.SelectedItem).ID;

            if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("IVF"))
            {
                DateTime? ProcedureDate = null;
                if (dtProcedureDate.SelectedDate != null)
                    ProcedureDate = dtProcedureDate.SelectedDate.Value.Date;
                if (dtProcedureTime.Value != null)
                    ProcedureDate = ProcedureDate.Value.Add(dtProcedureTime.Value.Value.TimeOfDay);

                if (dtProcedureDate.SelectedDate != null)
                    BizAction.Day0Details.ProcedureDate = ProcedureDate; //dtProcedureDate.SelectedDate.Value.Date;

                if (dtProcedureTime.Value != null)
                    BizAction.Day0Details.ProcedureTime = ProcedureDate; //Convert.ToDateTime(dtProcedureTime.Value);

                if ((MasterListItem)cmbSource.SelectedItem != null)
                    BizAction.Day0Details.SourceOfSperm = ((MasterListItem)cmbSource.SelectedItem).ID;

                if ((MasterListItem)cmbSpermMethod.SelectedItem != null)
                    BizAction.Day0Details.SpermCollectionMethod = ((MasterListItem)cmbSpermMethod.SelectedItem).ID;

                if (txtDonorCode.Text != "")
                    BizAction.Day0Details.DonorCode = txtDonorCode.Text;

                if (txtSampleNo.Text != "")
                    BizAction.Day0Details.SampleNo = txtSampleNo.Text;

                if (rdoYes.IsChecked != null)
                {
                    if (rdoYes.IsChecked == true)
                    {
                        BizAction.Day0Details.Embryoscope = true;
                    }
                }

                if (rdoNo.IsChecked != null)
                {
                    if (rdoNo.IsChecked == true)
                    {
                        BizAction.Day0Details.Embryoscope = false;
                    }
                }

                if ((MasterListItem)cmbSemenSample.SelectedItem != null)
                    BizAction.Day0Details.SemenSample = ((MasterListItem)cmbSemenSample.SelectedItem).ID.ToString();

            }
            if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("ICSI"))
            {
                DateTime? ProcedureDate = null;
                if (dtProcedureDateICSI.SelectedDate != null)
                    ProcedureDate = dtProcedureDateICSI.SelectedDate.Value.Date;
                if (dtProcedureTimeICSI.Value != null)
                    ProcedureDate = ProcedureDate.Value.Add(dtProcedureTimeICSI.Value.Value.TimeOfDay);

                if (dtProcedureDateICSI.SelectedDate != null)
                    BizAction.Day0Details.ProcedureDate = ProcedureDate;//dtProcedureDateICSI.SelectedDate.Value.Date;

                if (dtProcedureTimeICSI.Value != null)
                    BizAction.Day0Details.ProcedureTime = ProcedureDate;//Convert.ToDateTime(dtProcedureTimeICSI.Value);

                if ((MasterListItem)cmbSourceICSI.SelectedItem != null)
                    BizAction.Day0Details.SourceOfSperm = ((MasterListItem)cmbSourceICSI.SelectedItem).ID;

                if ((MasterListItem)cmbSpermMethodICSI.SelectedItem != null)
                    BizAction.Day0Details.SpermCollectionMethod = ((MasterListItem)cmbSpermMethodICSI.SelectedItem).ID;

                if (txtDonorCodeICSI.Text != "")
                    BizAction.Day0Details.DonorCode = txtDonorCodeICSI.Text;

                if (txtSampleNoICSI.Text != "")
                    BizAction.Day0Details.SampleNo = txtSampleNoICSI.Text;

                if (rdoYesICSI.IsChecked != null)
                {
                    if (rdoYesICSI.IsChecked == true)
                    {
                        BizAction.Day0Details.Embryoscope = true;
                    }
                }

                if (rdoNoICSI.IsChecked != null)
                {
                    if (rdoNoICSI.IsChecked == true)
                    {
                        BizAction.Day0Details.Embryoscope = false;
                    }
                }

                if (rdoYesIMSI.IsChecked != null)
                {
                    if (rdoYesIMSI.IsChecked == true)
                    {
                        BizAction.Day0Details.IMSI = true;
                    }
                }
                if (rdoNoIMSI.IsChecked != null)
                {
                    if (rdoNoIMSI.IsChecked == true)
                    {
                        BizAction.Day0Details.IMSI = false;
                    }
                }
                if ((MasterListItem)cmbSemenSampleICSI.SelectedItem != null)
                    BizAction.Day0Details.SemenSample = ((MasterListItem)cmbSemenSampleICSI.SelectedItem).ID.ToString();


                if ((MasterListItem)cmbOocyteZona.SelectedItem != null)
                    BizAction.Day0Details.OocyteZonaID = ((MasterListItem)cmbOocyteZona.SelectedItem).ID;
                if (txtOocyteZona.Text != "")
                    BizAction.Day0Details.OocyteZona = txtOocyteZona.Text;

                if ((MasterListItem)cmbPVS.SelectedItem != null)
                    BizAction.Day0Details.PVSID = ((MasterListItem)cmbPVS.SelectedItem).ID;
                if (txtPVS.Text != "")
                    BizAction.Day0Details.PVS = txtPVS.Text;

                if ((MasterListItem)cmbIstPB.SelectedItem != null)
                    BizAction.Day0Details.IstPBID = ((MasterListItem)cmbIstPB.SelectedItem).ID;
                if (txtIstPB.Text != "")
                    BizAction.Day0Details.IstPB = txtIstPB.Text;

                if ((MasterListItem)cmbCytoplasm.SelectedItem != null)
                    BizAction.Day0Details.CytoplasmID = ((MasterListItem)cmbCytoplasm.SelectedItem).ID;
                if (txtCytoplasm.Text != "")
                    BizAction.Day0Details.Cytoplasm = txtCytoplasm.Text;

            }

            if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
            {
                if (((MasterListItem)cmbnextplan.SelectedItem).ID == 8)
                    BizAction.Day0Details.IsDonorCycleDonate = true;
                else if (((MasterListItem)cmbnextplan.SelectedItem).ID == 9)
                    BizAction.Day0Details.IsDonorCycleDonateCryo = true;
            }
            else
            {
                if (((MasterListItem)cmbnextplan.SelectedItem).ID == 8)
                    BizAction.Day0Details.IsDonate = true;
                else if (((MasterListItem)cmbnextplan.SelectedItem).ID == 9)
                    BizAction.Day0Details.IsDonateCryo = true;
                if (BizAction.Day0Details.IsDonate == true || BizAction.Day0Details.IsDonateCryo == true)
                {
                    BizAction.Day0Details.RecepientPatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.Day0Details.RecepientPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
            }



            // BizAction.Day0Details.PlannedTreatmentID = PlannedTreatmentID;

            // 
            long cnt = 0;
            bool blCheckCnt = true;
            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day0Details.ImgList = ImageList;

            //




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

        long OocyteDonorID = 0;
        long OocyteDonorUnitID = 0;
        private void fillDetails()
        {
            try
            {
                clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay0DetailsBizActionVO();
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
                        if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details != null)
                        {
                            this.DataContext = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details;
                            OocyteDonorID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                            OocyteDonorUnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Date != null)
                            {
                                Labday0Date.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Date;
                            }
                            else
                            {
                                Labday0Date.SelectedDate = date;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Time != null)
                            {
                                Labday0Time.Value = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Time;
                            }
                            else
                            {
                                Labday0Time.Value = DateTime.Now;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.EmbryologistID != null && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.EmbryologistID != 0)
                            {
                                cmbEmbryologist.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                            }
                            txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != 0)
                            {
                                cmbAssistantEmbryologist.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AnesthetistID != null && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AnesthetistID != 0)
                            {
                                cmbAnesthetist.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AnesthetistID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AssitantAnesthetistID != null)
                            {
                                cmbAssistantAnesthetist.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.AssitantAnesthetistID;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CumulusID != null)
                            {
                                cmbCumulus.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CumulusID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.GradeID != null)
                            {
                                cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.GradeID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.MOIID != null)
                            {
                                cmbMOI.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.MOIID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CellStageID != null)
                            {
                                cmbcellstage.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CellStageID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IncubatorID != null)
                            {
                                cmbIncubator.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IncubatorID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.NextPlanID != null)
                            {
                                cmbnextplan.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.NextPlanID;
                            }

                            //commented by neena
                            //if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Photo != null)
                            //{
                            //    MyPhoto = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Photo;

                            //    BitmapImage img = new BitmapImage();
                            //    img.SetSource(new MemoryStream(MyPhoto, false));

                            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            //    //bmp.FromByteArray(MyPhoto);
                            //    //imgPhoto.Source = bmp;
                            //}
                            //

                            //if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.FileName != null)
                            //{
                            //    txtFN.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.FileName;
                            //}
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentID != null)
                            {
                                cmbTreatment.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            {
                                //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                                //{
                                //    cmdNew.IsEnabled = true;
                                //    cmbnextplan.IsEnabled = false;
                                //    cmbTreatment.IsEnabled = false;
                                //}
                                //else
                                //{
                                cmdNew.IsEnabled = false;
                                //}
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PICID != null)
                            {
                                cmbPIC.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PICID;
                            }
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DOSID != null)
                            {
                                cmbDOS.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DOSID;
                            }


                            //by neena
                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentStartDate != null)
                            {
                                TreatmentStartDate.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentStartDate;
                            }
                            else
                            {
                                TreatmentStartDate.SelectedDate = date;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentEndDate != null)
                            {
                                TreatmentEndDate.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.TreatmentEndDate;
                            }
                            else
                            {
                                TreatmentEndDate.SelectedDate = date;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ObservationDate != null)
                            {
                                ObservationDate.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ObservationDate;
                            }
                            else
                            {
                                ObservationDate.SelectedDate = date;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ObservationTime != null)
                            {
                                ObservationTime.Value = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ObservationTime;
                            }
                            else
                            {
                                ObservationTime.Value = DateTime.Now;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteMaturityID != null)
                            {
                                cmbOocyteMaturity.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteMaturityID;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteCytoplasmDysmorphisim != null)
                            {
                                cmbOocyteCytoplasmDysmorphisim.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteCytoplasmDysmorphisim;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ExtracytoplasmicDysmorphisim != null)
                            {
                                cmbExtracytoplasmicDysmorphisim.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ExtracytoplasmicDysmorphisim;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteCoronaCumulusComplex != null)
                            {
                                cmbOocyteCoronaCumulusComplex.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteCoronaCumulusComplex;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.NextPlanID == 6)
                            {
                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate != null)
                                {
                                    dtProcedureDate.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate;
                                }
                                //else
                                //{
                                //    dtProcedureDate.SelectedDate = DateTime.Now;
                                //}

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureTime != null)
                                {
                                    dtProcedureTime.Value = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureTime;
                                }
                                //else
                                //{
                                //    dtProcedureTime.Value = DateTime.Now;
                                //}

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SourceOfSperm != null)
                                {
                                    cmbSource.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SourceOfSperm;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SpermCollectionMethod != null)
                                {
                                    cmbSpermMethod.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SpermCollectionMethod;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Embryoscope == true)
                                {
                                    rdoYes.IsChecked = true;
                                }
                                else if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Embryoscope == false)
                                {
                                    rdoNo.IsChecked = true;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DonorCode != null)
                                {
                                    txtDonorCode.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DonorCode;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SampleNo != null)
                                {
                                    txtSampleNo.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SampleNo;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample != "" && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample != null)
                                {
                                    cmbSemenSample.SelectedItem = objSemenSampleList.Where(x => x.ID == Convert.ToInt64(((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample)).FirstOrDefault();
                                }
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.NextPlanID == 7)
                            {
                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate != null)
                                {
                                    dtProcedureDateICSI.SelectedDate = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureDate;
                                }
                                //else
                                //{
                                //    dtProcedureDate.SelectedDate = DateTime.Now;
                                //}

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureTime != null)
                                {
                                    dtProcedureTimeICSI.Value = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ProcedureTime;
                                }
                                //else
                                //{
                                //    dtProcedureTime.Value = DateTime.Now;
                                //}

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SourceOfSperm != null)
                                {
                                    cmbSourceICSI.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SourceOfSperm;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SpermCollectionMethod != null)
                                {
                                    cmbSpermMethodICSI.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SpermCollectionMethod;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Embryoscope == true)
                                {
                                    rdoYesICSI.IsChecked = true;
                                }
                                else if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Embryoscope == false)
                                {
                                    rdoNoICSI.IsChecked = true;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DonorCode != null)
                                {
                                    txtDonorCodeICSI.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.DonorCode;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SampleNo != null)
                                {
                                    txtSampleNoICSI.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SampleNo;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IMSI == true)
                                {
                                    rdoYesIMSI.IsChecked = true;
                                }
                                else if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IMSI == false)
                                {
                                    rdoNoIMSI.IsChecked = true;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample != "" && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample != null)
                                {
                                    cmbSemenSampleICSI.SelectedItem = objSemenSampleList.Where(x => x.ID == Convert.ToInt64(((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SemenSample)).FirstOrDefault();
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteZonaID != null)
                                {
                                    cmbOocyteZona.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteZonaID;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteZona != null)
                                {
                                    txtOocyteZona.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteZona;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PVSID != null)
                                {
                                    cmbPVS.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PVSID;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PVS != null)
                                {
                                    txtPVS.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PVS;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IstPBID != null)
                                {
                                    cmbIstPB.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IstPBID;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IstPB != null)
                                {
                                    txtIstPB.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.IstPB;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CytoplasmID != null)
                                {
                                    cmbCytoplasm.SelectedValue = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.CytoplasmID;
                                }

                                if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Cytoplasm != null)
                                {
                                    txtCytoplasm.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Cytoplasm;
                                }
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.RecepientPatientName != null)
                            {
                                txtSelectedPatientName.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.RecepientPatientName;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.RecepientMrNO != null)
                            {
                                txtSelectedMrNO.Text = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.RecepientMrNO;
                            }

                            if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                            {
                                ImageList = new List<clsAddImageVO>();
                                mylistitem = new List<ListItems>();
                                foreach (var item in ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.ImgList)
                                {
                                    //BitmapImage img = new BitmapImage();
                                    //img.SetSource(new MemoryStream(item.Photo, false));

                                    //clsAddImageVO ObjNew = new clsAddImageVO();
                                    //ObjNew.Photo = item.Photo;
                                    //ObjNew.ImagePath = item.ImagePath;
                                    ////ObjNew.BitImagePath = img;

                                    //ImageList.Add(ObjNew);

                                    ListItems obj = new ListItems();
                                    obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                    obj.ImagePath = item.ImagePath;
                                    obj.ID = item.ID;
                                    obj.SeqNo = item.SeqNo;
                                    obj.Photo = item.Photo;
                                    obj.Day = item.Day;
                                    obj.UnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.UnitID;
                                    obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                    obj.OocyteNumber = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                    obj.PatientID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PatientID;
                                    obj.PatientUnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                    obj.PlanTherapyID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                    obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                    obj.OriginalImagePath = item.OriginalImagePath;
                                    if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                        obj.IsDelete = false;
                                    else
                                        obj.IsDelete = true;
                                    mylistitem.Add(obj);
                                    GetMylistitem.Add(obj);

                                    //mylistitem.Add(
                                    //    new ListItems
                                    //    {
                                    //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                    //        ImagePath = item.ImagePath,
                                    //        ID = item.ID,
                                    //        SeqNo = item.SeqNo,
                                    //        Photo = item.Photo,
                                    //        Day = item.Day,
                                    //        UnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.UnitID,
                                    //        SerialOocyteNumber = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                    //        OocyteNumber = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                    //        PatientID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PatientID,
                                    //        PatientUnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                    //        PlanTherapyID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                    //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                    //        OriginalImagePath = item.OriginalImagePath
                                    //    });

                                }

                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                // GetMylistitem = mylistitem.DeepCopy();
                                txtFN.Text = mylistitem.Count.ToString();
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

        List<MasterListItem> objSemenSampleList = null;
        private void fillSemenSampleList()
        {
            clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay0DetailsBizActionVO();
            BizAction.IsSemenSample = true;
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;

            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).SemenSampleList != null && ((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).SemenSampleList.Count > 0)
                    {
                        //List<MasterListItem> objList = new List<MasterListItem>();
                        objSemenSampleList = new List<MasterListItem>();
                        objSemenSampleList.Add(new MasterListItem(0, "-- Select --"));
                        objSemenSampleList.AddRange(((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).SemenSampleList);
                        cmbSemenSample.ItemsSource = null;
                        cmbSemenSample.ItemsSource = objSemenSampleList;
                        cmbSemenSample.SelectedItem = objSemenSampleList[0];
                        cmbSemenSampleICSI.ItemsSource = null;
                        cmbSemenSampleICSI.ItemsSource = objSemenSampleList;
                        cmbSemenSampleICSI.SelectedItem = objSemenSampleList[0];
                    }
                }
                fillDetails();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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

        private void cmbSrcTreatmentPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSrcOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdMediaDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        byte[] MyPhoto { get; set; }
        //private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog OpenFile = new OpenFileDialog();


        //    OpenFile.Multiselect = false;
        //    OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
        //    OpenFile.FilterIndex = 1;
        //    if (OpenFile.ShowDialog() == true)
        //    {
        //        txtFN.Text = OpenFile.File.Name;

        //        WriteableBitmap imageSource = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
        //        try
        //        {
        //            imageSource.SetSource(OpenFile.File.OpenRead());
        //            imgPhoto.Source = imageSource;



        //            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
        //            bmp.Render(imgPhoto, new MatrixTransform());
        //            bmp.Invalidate();

        //            int[] p = bmp.Pixels;
        //            int len = p.Length * 4;
        //            byte[] result = new byte[len]; // ARGB
        //            Buffer.BlockCopy(p, 0, result, 0, len);

        //            MyPhoto = result;

        //            FillIndex();

        //        }
        //        catch (Exception)
        //        {
        //            HtmlPage.Window.Alert("Error Loading File");
        //        }

        //    }
        //}


        //List<BitmapImage> ImageList = new List<BitmapImage>();
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                txtFN.Text = openDialog.File.Name;
                WriteableBitmap imageSource = new WriteableBitmap(200, 100);
                try
                {
                    //imageSource.SetSource(openDialog.File.OpenRead());
                    //ImageList.Add(imageSource);
                    //dgImgList.ItemsSource = ImageList;

                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        MyPhoto = new byte[stream.Length];
                        stream.Read(MyPhoto, 0, (int)stream.Length);
                        ShowPhoto(MyPhoto, txtFN.Text);


                    }

                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        List<ListItems> mylistitem = new List<ListItems>();
        List<ListItems> GetMylistitem = new List<ListItems>();
        private void ShowPhoto(byte[] MyPhoto, string ImgName)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource(new MemoryStream(MyPhoto, false));

            //mylistitem.Add(new ListItems { Image1 = img, Photo = MyPhoto, ImagePath = ImgName });

            ListItems obj = new ListItems();
            obj.Image1 = img;
            obj.Photo = MyPhoto;
            obj.ImagePath = ImgName;
            obj.IsDelete = true;

            var item1 = from r in mylistitem.ToList()
                        where r.ImagePath == obj.ImagePath
                        select r;

            if (item1.ToList().Count == 0)
            {
                mylistitem.Add(obj);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + obj.ImagePath + "'" + " File is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

            dgImgList.ItemsSource = null;
            dgImgList.ItemsSource = mylistitem;

            txtFN.Text = mylistitem.Count.ToString();


            //clsAddImageVO ObjNew = new clsAddImageVO();
            //ObjNew.Photo = MyPhoto;
            //ObjNew.ImagePath = ImgName;
            //ObjNew.BitImagePath = img;

            //ImageList.Add(ObjNew);
            //dgImgList.ItemsSource = null;
            //dgImgList.ItemsSource = ImageList;
        }

        // BY bHUSHAN..

        //by neena
        List<MasterListItem> objList = new List<MasterListItem>();

        private void fillOocyteetailsForOocyteReceipent()
        {
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO();
            BizAction.IsOocyteRecipient = true;
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        foreach (var item in ((clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)arg.Result).Oocytelist)
                        {
                            if (item.OocyteNumber != OocyteNo && OocyteNo != 0)
                                objList.Add(new MasterListItem(item.OocyteNumber, "Oocyte No " + item.OocyteNumber));

                        }
                        filldata();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


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
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO();
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
                    if (((clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        // //  by neena
                        // List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        // List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        // var productFirstChars =
                        //                        from p in objList
                        //                        select p.ID;
                        // var customerFirstChars =
                        //                         from c in mlist
                        //                         where (c.Isfreezed == true)
                        //                         select c.OocyteNumber
                        //                         ;

                        //var missingVehicles = productFirstChars.Except(customerFirstChars);

                        // MasterListItem newItem = new MasterListItem();
                        // foreach (var item in missingVehicles)
                        // {
                        //     string str = "";
                        //     str = "Oocyte Number " + item;
                        //     long id = item;
                        //     newItem = new MasterListItem(item, str);

                        //     missingVehicle1.Add(newItem);
                        // }

                        // cmbApplay.ItemsSource = null;
                        // cmbApplay.ItemsSource = missingVehicle1;



                        //  by neena 
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                where (c.NextPlanID == 0)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var missingVehicles = productFirstChars.Intersect(customerFirstChars);
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


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoNo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hlbSelectSampleICSI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoYesIMSI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoNoIMSI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbnextplan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("Discarded"))
            {
                lblReason.Visibility = Visibility.Visible;
                txtDiscardReason.Visibility = Visibility.Visible;
                grpboxIVF.Visibility = Visibility.Collapsed;
                grpboxICSI.Visibility = Visibility.Collapsed;
                //lblIMSI.Visibility = Visibility.Collapsed;
                //spIMSI.Visibility = Visibility.Collapsed;
                DonateGrid.Visibility = Visibility.Collapsed;
            }
            else if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("IVF"))
            {
                grpboxIVF.Visibility = Visibility.Visible;
                grpboxICSI.Visibility = Visibility.Collapsed;
                //lblIMSI.Visibility = Visibility.Collapsed;
                //spIMSI.Visibility = Visibility.Collapsed;
                lblReason.Visibility = Visibility.Collapsed;
                txtDiscardReason.Visibility = Visibility.Collapsed;
                DonateGrid.Visibility = Visibility.Collapsed;
                dtProcedureDate.SelectedDate = OPUDate;
                dtProcedureTime.Value = OPUTime;
                cmbSource.SelectedValue = SourceOfSperm;
            }
            else if (((MasterListItem)cmbnextplan.SelectedItem).Description.Equals("ICSI"))
            {
                grpboxICSI.Visibility = Visibility.Visible;
                grpboxIVF.Visibility = Visibility.Collapsed;
                //lblIMSI.Visibility = Visibility.Visible;
                //spIMSI.Visibility = Visibility.Visible;
                lblReason.Visibility = Visibility.Collapsed;
                txtDiscardReason.Visibility = Visibility.Collapsed;
                DonateGrid.Visibility = Visibility.Collapsed;
                dtProcedureDateICSI.SelectedDate = OPUDate;
                dtProcedureTimeICSI.Value = OPUTime;
                cmbSourceICSI.SelectedValue = SourceOfSperm;
            }
            else if (((MasterListItem)cmbnextplan.SelectedItem).ID == 8 || ((MasterListItem)cmbnextplan.SelectedItem).ID == 9)
            {
                if (PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                {
                    DonateGrid.Visibility = Visibility.Collapsed;
                    grpboxIVF.Visibility = Visibility.Collapsed;
                    grpboxICSI.Visibility = Visibility.Collapsed;
                    //lblIMSI.Visibility = Visibility.Collapsed;
                    //spIMSI.Visibility = Visibility.Collapsed;
                    lblReason.Visibility = Visibility.Collapsed;
                    txtDiscardReason.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DonateGrid.Visibility = Visibility.Visible;
                    grpboxIVF.Visibility = Visibility.Collapsed;
                    grpboxICSI.Visibility = Visibility.Collapsed;
                    //lblIMSI.Visibility = Visibility.Collapsed;
                    //spIMSI.Visibility = Visibility.Collapsed;
                    lblReason.Visibility = Visibility.Collapsed;
                    txtDiscardReason.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                lblReason.Visibility = Visibility.Collapsed;
                txtDiscardReason.Visibility = Visibility.Collapsed;
                grpboxIVF.Visibility = Visibility.Collapsed;
                DonateGrid.Visibility = Visibility.Collapsed;
                grpboxICSI.Visibility = Visibility.Collapsed;
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)  //delete images
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to delete image", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
            msgW1.Show();
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (mylistitem.Count > GetMylistitem.Count)
                {
                    mylistitem.Remove((ListItems)dgImgList.SelectedItem);
                    dgImgList.ItemsSource = null;
                    dgImgList.ItemsSource = mylistitem;
                    txtFN.Text = mylistitem.Count.ToString();
                }
                else
                {

                    clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO BizAction = new clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO();
                    BizAction.ImageObj = new clsAddImageVO();
                    ListItems obj = (ListItems)dgImgList.SelectedItem;
                    BizAction.ImageObj.PatientID = obj.PatientID;
                    BizAction.ImageObj.PatientUnitID = obj.PatientUnitID;
                    BizAction.ImageObj.PlanTherapyID = obj.PlanTherapyID;
                    BizAction.ImageObj.PlanTherapyUnitID = obj.PlanTherapyUnitID;
                    BizAction.ImageObj.OocyteNumber = obj.OocyteNumber;
                    BizAction.ImageObj.SerialOocyteNumber = obj.SerialOocyteNumber;
                    BizAction.ImageObj.Day = obj.Day;
                    //BizAction.ImageObj.ServerImageName = obj.ServerImageName;
                    BizAction.ImageObj.OriginalImagePath = obj.OriginalImagePath;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList != null && ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList.Count > 0)
                            {
                                ImageList = new List<clsAddImageVO>();
                                foreach (var item in ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList)
                                {
                                    mylistitem = new List<ListItems>();
                                    mylistitem.Add(
                                        new ListItems
                                        {
                                            Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative)),
                                            ImagePath = item.ImagePath,
                                            ID = item.ID,
                                            SeqNo = item.SeqNo,
                                            Photo = item.Photo,
                                            Day = item.Day,
                                            UnitID = item.UnitID,
                                            SerialOocyteNumber = item.SerialOocyteNumber,
                                            OocyteNumber = item.OocyteNumber,
                                            PatientID = item.PatientID,
                                            PatientUnitID = item.PatientUnitID,
                                            PlanTherapyID = item.PlanTherapyID,
                                            PlanTherapyUnitID = item.PlanTherapyUnitID,
                                            OriginalImagePath = item.OriginalImagePath,
                                            IsDelete = true
                                        });
                                }

                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }
                            else
                            {
                                mylistitem = new List<ListItems>();
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }

                            ////MessageBoxControl.MessageBoxChildWindow msgW1 =
                            ////       new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            ////msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            ////{
                            ////    if (res == MessageBoxResult.OK)
                            ////    {

                            ////    }
                            ////};
                            ////msgW1.Show();
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

        private void dtProcedureDate_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (dtProcedureDate.SelectedDate < OPUDate)
            //{
            //    dtProcedureDate.SetValidation("Procedure Date Cannot Be Less Than OPU Date");
            //    dtProcedureDate.RaiseValidationError();
            //    dtProcedureDate.Focus();
            //    dtProcedureDate.Text = " ";
            //    dtProcedureDate.Text = OPUDate.ToString();
            //}
            //else
            //    dtProcedureDate.ClearValidationError();
        }

        private void hlbSelectPatient_Click(object sender, RoutedEventArgs e)
        {
            PtListDonateOnDay0EmbDashboard WinDonate = new PtListDonateOnDay0EmbDashboard();
            WinDonate.CurrentCycleCoupleDetails = CoupleDetails;
            WinDonate.OnSaveButton_Click += new RoutedEventHandler(WinDonate_OnSaveButton_Click);
            WinDonate.Show();
        }

        void WinDonate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                txtSelectedPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientName;
                txtSelectedMrNO.Text = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.MRNo;

                //txtSelectedPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                //txtSelectedMrNO.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
            }
        }

        private void cmbOocyteZona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbOocyteZona.SelectedItem).ID == 2)
                txtOocyteZona.Visibility = Visibility.Visible;
            else
                txtOocyteZona.Visibility = Visibility.Collapsed;
        }

        private void cmbPVS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbPVS.SelectedItem).ID == 2)
                txtPVS.Visibility = Visibility.Visible;
            else
                txtPVS.Visibility = Visibility.Collapsed;
        }

        private void cmbIstPB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbIstPB.SelectedItem).ID == 2)
                txtIstPB.Visibility = Visibility.Visible;
            else
                txtIstPB.Visibility = Visibility.Collapsed;
        }

        private void cmbCytoplasm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbCytoplasm.SelectedItem).ID == 2)
                txtCytoplasm.Visibility = Visibility.Visible;
            else
                txtCytoplasm.Visibility = Visibility.Collapsed;
        }

        private void cmbSpermMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSpermMethodICSI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }




    }

    public class ListItems
    {
        public BitmapImage Image1 { get; set; }
        public byte[] Photo { get; set; }
        public string ImagePath { get; set; }
        public string OriginalImagePath { get; set; }
        public long ID { get; set; }
        public string SeqNo { get; set; }
        public long UnitID { get; set; }
        public long SerialOocyteNumber { get; set; }
        public long OocyteNumber { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long PlanTherapyID { get; set; }
        public long PlanTherapyUnitID { get; set; }
        public long Day { get; set; }
        public bool IsDelete { get; set; }
    }

}

