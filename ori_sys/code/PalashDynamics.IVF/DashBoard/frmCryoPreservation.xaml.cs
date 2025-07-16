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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmCryoPreservation : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public clsPlanTherapyVO PlanTherapyVO;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        //added by neena
        public long PlannedTreatmentID;
        //

        //added by neena
        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _RefeezeVitriDetailsOocytes = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> RefeezeVitriDetailsOocytes
        {
            get { return _RefeezeVitriDetailsOocytes; }
            set { _RefeezeVitriDetailsOocytes = value; }
        }
        //

        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetailsOocytes = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetailsOocytes
        {
            get { return _VitriDetailsOocytes; }
            set { _VitriDetailsOocytes = value; }
        }


        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }
        private ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> _ThawDetails = new ObservableCollection<clsIVFDashBoard_ThawingDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> ThawDetails
        {
            get { return _ThawDetails; }
            set { _ThawDetails = value; }
        }

        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetailsForOocyte = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetailsForOocyte
        {
            get { return _VitriDetailsForOocyte; }
            set { _VitriDetailsForOocyte = value; }
        }
        private ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> _ThawDetailsForOocyte = new ObservableCollection<clsIVFDashBoard_ThawingDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> ThawDetailsForOocyte
        {
            get { return _ThawDetailsForOocyte; }
            set { _ThawDetailsForOocyte = value; }
        }

        private ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> _ThawDetailsOocytes = new ObservableCollection<clsIVFDashBoard_ThawingDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_ThawingDetailsVO> ThawDetailsOocytes
        {
            get { return _ThawDetailsOocytes; }
            set { _ThawDetailsOocytes = value; }
        }

        WaitIndicator wait = new WaitIndicator();
        private List<MasterListItem> _SSemen = new List<MasterListItem>();
        public List<MasterListItem> SSemen
        {
            get
            {
                return _SSemen;
            }
            set
            {
                _SSemen = value;
            }
        }
        private List<MasterListItem> _SOctyes = new List<MasterListItem>();
        public List<MasterListItem> SOctyes
        {
            get
            {
                return _SOctyes;
            }
            set
            {
                _SOctyes = value;
            }
        }
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

        //private List<MasterListItem> _CleavageGradeDay2 = new List<MasterListItem>();
        //public List<MasterListItem> CleavageGradeDay2
        //{
        //    get
        //    {
        //        return _CleavageGradeDay2;
        //    }
        //    set
        //    {
        //        _CleavageGradeDay2 = value;
        //    }
        //}

        private List<MasterListItem> _CleavageGradeDay3 = new List<MasterListItem>();
        public List<MasterListItem> CleavageGradeDay3
        {
            get
            {
                return _CleavageGradeDay3;
            }
            set
            {
                _CleavageGradeDay3 = value;
            }
        }

        private List<MasterListItem> _CleavageGradeDay4 = new List<MasterListItem>();
        public List<MasterListItem> CleavageGradeDay4
        {
            get
            {
                return _CleavageGradeDay4;
            }
            set
            {
                _CleavageGradeDay4 = value;
            }
        }

        private List<MasterListItem> _StageOfDevelopmentGrade = new List<MasterListItem>();
        public List<MasterListItem> StageOfDevelopmentGrade
        {
            get
            {
                return _StageOfDevelopmentGrade;
            }
            set
            {
                _StageOfDevelopmentGrade = value;
            }
        }

        private List<MasterListItem> _InnerCellMassGrade = new List<MasterListItem>();
        public List<MasterListItem> InnerCellMassGrade
        {
            get
            {
                return _InnerCellMassGrade;
            }
            set
            {
                _InnerCellMassGrade = value;
            }
        }

        private List<MasterListItem> _TrophoectodermGrade = new List<MasterListItem>();
        public List<MasterListItem> TrophoectodermGrade
        {
            get
            {
                return _TrophoectodermGrade;
            }
            set
            {
                _TrophoectodermGrade = value;
            }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }
        private List<MasterListItem> _ProtocolType = new List<MasterListItem>();
        public List<MasterListItem> ProtocolType
        {
            get
            {
                return _ProtocolType;
            }
            set
            {
                _ProtocolType = value;
            }
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

        private List<MasterListItem> _Straw = new List<MasterListItem>();
        public List<MasterListItem> Straw
        {
            get
            {
                return _Straw;
            }
            set
            {
                _Straw = value;
            }
        }

        private List<MasterListItem> _GobletShape = new List<MasterListItem>();
        public List<MasterListItem> GobletShape
        {
            get
            {
                return _GobletShape;
            }
            set
            {
                _GobletShape = value;
            }
        }

        private List<MasterListItem> _GobletSize = new List<MasterListItem>();
        public List<MasterListItem> GobletSize
        {
            get
            {
                return _GobletSize;
            }
            set
            {
                _GobletSize = value;
            }
        }

        private List<MasterListItem> _GobletColor = new List<MasterListItem>();
        public List<MasterListItem> GobletColor
        {
            get
            {
                return _GobletColor;
            }
            set
            {
                _GobletColor = value;
            }
        }

        private List<MasterListItem> _Canister = new List<MasterListItem>();
        public List<MasterListItem> Canister
        {
            get
            {
                return _Canister;
            }
            set
            {
                _Canister = value;
            }
        }

        private List<MasterListItem> _Tank = new List<MasterListItem>();
        public List<MasterListItem> Tank
        {
            get
            {
                return _Tank;
            }
            set
            {
                _Tank = value;
            }
        }

        private List<MasterListItem> _EmbryoPlan = new List<MasterListItem>();
        public List<MasterListItem> EmbryoPlan
        {
            get
            {
                return _EmbryoPlan;
            }
            set
            {
                _EmbryoPlan = value;
            }
        }

        private List<MasterListItem> _EmbryoPlanPGD = new List<MasterListItem>();
        public List<MasterListItem> EmbryoPlanPGD
        {
            get
            {
                return _EmbryoPlanPGD;
            }
            set
            {
                _EmbryoPlanPGD = value;
            }
        }

        private List<MasterListItem> _OocytePlan = new List<MasterListItem>();
        public List<MasterListItem> OocytePlan
        {
            get
            {
                return _OocytePlan;
            }
            set
            {
                _OocytePlan = value;
            }
        }

        public bool IsEmbryoDonation;

        public frmCryoPreservation()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        #region Fill Master Item

        public void SSemenMaster()
        {
            try
            {
                wait.Show();
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
                        SSemen = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillGrade();
                    }
                    // wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillPostThawingPlan()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_PostThawingPlanMaster;
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
                        List<MasterListItem> list = new List<MasterListItem>();
                        list.Add(new MasterListItem(0, "-- Select --"));
                        list.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        EmbryoPlan = (from r in list
                                      where r.ID != 6 && r.ID != 5
                                      select r).ToList();

                        OocytePlan = (from r in list
                                      where r.ID != 2 && r.ID != 4 && r.ID != 5 && r.ID != 7
                                      select r).ToList();

                        //EmbryoPlan = (from r in ((clsGetMasterListBizActionVO)args.Result).MasterList
                        //              where r.ID != 6 && r.ID != 5
                        //              select r).ToList();

                        //OocytePlan = (from r in ((clsGetMasterListBizActionVO)args.Result).MasterList
                        //              where r.ID != 2 && r.ID != 4 && r.ID != 5
                        //              select r).ToList();

                        //fillOocyteSource();
                        fillLabIncharge();

                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void StageofDevelopmentGrade()
        {
            if (wait == null)
            {
                wait = new WaitIndicator();
                wait.Show();
            }
            else
                wait.Show();
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_StageofDevelopmentGrade;
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
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    StageOfDevelopmentGrade = objList;

                    InnerCellMassgrade();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void InnerCellMassgrade()
        {
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_InnerCellMassGrade;
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
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    InnerCellMassGrade = objList;
                    Trophoectodermgrade();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void Trophoectodermgrade()
        {
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TrophoectodermGrade;
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
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    TrophoectodermGrade = objList;
                    FillCleavageGradeDay2();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        StageofDevelopmentGrade();

                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillOocyteSource()
        {
            try
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
                        SOctyes = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillOocyteMaturity();
                    }
                    //wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        List<MasterListItem> mlSourceOfSperm = null;
        private void fillOocyteMaturity()
        {
            mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteMaturity), mlSourceOfSperm);
            fillCanID();
            //cmbOocyteMaturity.ItemsSource = mlSourceOfSperm;
            //cmbOocyteMaturity.SelectedItem = Default;

            //List<MasterListItem> ObjOocyteList = new List<MasterListItem>();
            //ObjOocyteList.Add(new MasterListItem(0, "--Select--"));
            //ObjOocyteList.Add(new MasterListItem(1, "GV"));
            //ObjOocyteList.Add(new MasterListItem(2, "M I"));
            //ObjOocyteList.Add(new MasterListItem(3, "M II"));
            //cmbOocyteMaturity.ItemsSource = null;
            //cmbOocyteMaturity.ItemsSource = ObjOocyteList;
            //cmbOocyteMaturity.SelectedItem = ObjOocyteList[0];
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

        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillStrawList();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillStrawList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Straw = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletshape();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillGobletshape()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletShapeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletShape = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletSize();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillGobletSize()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletSizeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletSize = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillGlobletColor();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillGlobletColor()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletColor;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletColor = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillCanister();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        private void FillCanister()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Canister = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillTank();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        private void FillTank()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Tank = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillDetailsOV();
                        //if (OVitrification != null && OVitrification.IsSelected == true)
                        //{
                        //    fillDetailsOV();
                        //}
                        //else
                        //{
                        //    //wait.Close();
                        //    fillDetails();
                        //}

                        //fillPlan();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillPlan()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
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
                        ProtocolType = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        //FillCellStage();
                        if (OVitrification != null && OVitrification.IsSelected == true)
                        {
                            fillDetailsOV();
                        }
                        else
                        {
                            //wait.Close();
                            fillDetails();
                        }
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
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
                        CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        if (OVitrification != null && OVitrification.IsSelected == true)
                        {
                            fillDetailsOV();
                        }
                        else
                        {
                            //wait.Close();
                            fillDetails();
                        }
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsClosed == true)
            {
                cmdNew.IsEnabled = false;
            }
            //if (Vitrification != null)
            //    Vitrification.IsSelected = true;

            if (OVitrification != null)
                OVitrification.IsSelected = true;

            //SSemenMaster();
            //fillOocyteMaturity();
            StageofDevelopmentGrade();

            if (IsEmbryoDonation == true)
                Thawing.Visibility = Visibility.Collapsed;
            //  fillDoctor();

            ////added by neena
            //if (PlannedTreatmentID == 5)
            //{
            //    //chkFreeze.IsEnabled = true;
            //    //Thawing.IsEnabled = true;
            //    OVitrification.Visibility = Visibility.Collapsed;
            //    ThawingOocyte.Visibility = Visibility.Collapsed;
            //    Vitrification.IsSelected = true;
            //}
            ////
            if (PlannedTreatmentID == 5)
            {
                OVitrification.Visibility = Visibility.Collapsed;
                ThawingOocyte.Visibility = Visibility.Collapsed;
                Vitrification.IsSelected = true;
            }
        }

        bool blEmdSave = false;
        bool blOocyteSave = false;

        private void fillDetails()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails.Clear();
                        this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdNew.IsEnabled = false;
                            //Thawing.IsEnabled = true;
                            //if (PlannedTreatmentID == 5) //added by neena
                            //{
                            //    // chkFreeze.IsEnabled = true;
                            //    // Thawing.IsEnabled = true;
                            //    OVitrification.Visibility = Visibility.Collapsed;
                            //    ThawingOocyte.Visibility = Visibility.Collapsed;
                            //}
                        }
                        else
                            cmdNew.IsEnabled = true;
                        chkFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;
                        }

                        try
                        {
                            for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                            {
                                MasterListItem Gr = new MasterListItem();
                                MasterListItem PT = new MasterListItem();
                                MasterListItem CS = new MasterListItem();

                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                                {
                                    Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                                }
                                //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID != null)
                                //{
                                //    PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID));
                                //}
                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                                {
                                    CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                                }
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                                }

                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                                }

                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                                }
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                                }

                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                                {
                                    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                    {
                                        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                    }
                                    else
                                    {
                                        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                                }

                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                                }

                                //added by neena
                                //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                                //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsEnabled = false;
                                //else
                                //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsEnabled = true;

                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsSaved == true)
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsEnabled = false;
                                else
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsEnabled = true;

                                //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsSaved == false)
                                //    blEmdSave = true;
                                //else
                                //    blEmdSave = false;


                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsRefreezeFromOtherCycle == true)
                                {
                                    dgVitrificationDetilsGrid.Columns[25].Visibility = Visibility.Visible;
                                    dgVitrificationDetilsGrid.Columns[26].Visibility = Visibility.Visible;
                                    //dgVitrificationDetilsGrid.Columns[24].Visibility = Visibility.Collapsed;
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsImgVisible = "Visible";
                                    //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsRefreezeFromBothCycle = "/PalashDynamics;component/Icons/green.png";
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsImgVisible = "Collapsed";
                                }

                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsRefreeze == true)
                                {
                                    dgVitrificationDetilsGrid.Columns[25].Visibility = Visibility.Visible;
                                    //dgVitrificationDetilsGrid.Columns[23].Visibility = Visibility.Collapsed;
                                    dgVitrificationDetilsGrid.Columns[27].Visibility = Visibility.Visible;
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsImg1Visible = "Visible";
                                    //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsRefreezeFromBothCycle = "/PalashDynamics;component/Icons/yellow.png";
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsImg1Visible = "Collapsed";
                                }

                                if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsFreshEmbryoPGDPGS || ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsFrozenEmbryoPGDPGS)
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PGDPGS = "PGDPGS";

                                VitriDetails.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        if (Vitrification != null)
                        {
                            if (Vitrification.IsSelected == true)
                            {
                                dgVitrificationDetilsGrid.ItemsSource = null;
                                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                            }
                        }
                        //commented by neena
                        //if (Thawing != null)
                        //{
                        //    if (Thawing.IsSelected == true)
                        //    {
                        //        dgThawingViDetilsGrid.ItemsSource = null;
                        //        dgThawingViDetilsGrid.ItemsSource = VitriDetails;
                        //    }
                        //}
                        //
                        //...............................................
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 5)
                        {
                            //fillVitificationDetailsForThawTransfer();  //commented by neena
                        }
                        //................................................
                        wait.Close();

                        ////
                        //VitriDetailsForOocyte.Clear();
                        //this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.IsFreezed == true)
                        //{
                        //    cmdNewOcy.IsEnabled = false;
                        //    ThawingOocyte.IsEnabled = true;
                        //}
                        //else
                        //    cmdNewOcy.IsEnabled = true;
                        //chkOcFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.IsFreezed;
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.DateTime != null)
                        //{
                        //    dtOcVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.DateTime;
                        //    txtOcTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.DateTime;
                        //}
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.VitrificationNo != null)
                        //{
                        //    txtOcVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.VitrificationNo;
                        //}

                        ////if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.EmbryologistID != null && ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.EmbryologistID != 0)
                        ////{
                        ////    cmbEmbryologistOocyte.SelectedValue = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.EmbryologistID;
                        ////}

                        ////if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.AssitantEmbryologistID != null && ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.AssitantEmbryologistID != 0)
                        ////{
                        ////    cmbAssistantEmbryologistOocyte.SelectedValue = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.AssitantEmbryologistID;
                        ////}

                        ////MasterListItem Gr = new MasterListItem();
                        ////MasterListItem PT = new MasterListItem(); ;
                        ////MasterListItem CS = new MasterListItem(); ;
                        //for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList.Count; i++)
                        //{
                        //    if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID != null)
                        //    {
                        //        Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID));
                        //    }
                        //    if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ProtocolTypeID != null)
                        //    {
                        //        PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ProtocolTypeID));
                        //    }
                        //    if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID != null)
                        //    {
                        //        CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID));
                        //    }

                        //    //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID != null)
                        //    //{
                        //    //    OPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID));
                        //    //}

                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanIdList = CanList;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanId) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanId));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                        //    }

                        //    //oocyte grade
                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].OocyteGradeList = mlSourceOfSperm;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].OocyteGradeID) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].OocyteGradeID));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
                        //    }
                        //    //

                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorSelectorList = GobletColor;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorCodeID) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorCodeID));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                        //    }

                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawIdList = Straw;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawId) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawId));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                        //    }
                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeList = GobletShape;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeId) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeId));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                        //    }

                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeList = GobletSize;
                        //    {
                        //        if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeId) > 0)
                        //        {
                        //            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeId));
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                        //        }
                        //    }
                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanisterIdList = Canister;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ConistorNo) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ConistorNo));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                        //    }

                        //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankList = Tank;
                        //    if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankId) > 0)
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankId));
                        //    }
                        //    else
                        //    {
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                        //    }



                        //    //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanList = OocytePlan;
                        //    //if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID) > 0)
                        //    //{
                        //    //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID));
                        //    //}
                        //    //else
                        //    //{
                        //    //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == 0);
                        //    //}

                        //    //added by neena
                        //    if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.IsFreezed == true)
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].IsEnabled = false;
                        //    else
                        //        ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].IsEnabled = true;

                        //    if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].IsSaved == false)
                        //        blOocyteSave = true;
                        //    else
                        //        blOocyteSave = false;

                        //    VitriDetailsForOocyte.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i]);

                        //}

                        //if (OVitrification != null)
                        //{
                        //    if (OVitrification.IsSelected == true)
                        //    {
                        //        dgOcyVitrificationDetilsGrid.ItemsSource = null;
                        //        dgOcyVitrificationDetilsGrid.ItemsSource = VitriDetailsForOocyte;
                        //    }
                        //}
                        //if (ThawingOocyte != null)
                        //{
                        //    if (ThawingOocyte.IsSelected == true)
                        //    {
                        //        dgThawingViDetilsGridOocyte.ItemsSource = null;
                        //        dgThawingViDetilsGridOocyte.ItemsSource = VitriDetailsForOocyte;
                        //    }
                        //}

                        ////...............................................
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMainForOocyte.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 5)
                        //{
                        //    fillVitificationDetailsForOocyteForThawTransfer();
                        //}
                        ////................................................
                        ////
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
        public PagedSortableCollectionView<cls_NewThawingDetailsVO> ThawDetailsList { get; private set; }
        public int ThawDetailsListPageSize
        {
            get
            {
                return ThawDetailsList.PageSize;
            }
            set
            {
                if (value == ThawDetailsList.PageSize) return;
                ThawDetailsList.PageSize = value;
            }
        }
        private List<MasterListItem> _LabIncharge = new List<MasterListItem>();
        public List<MasterListItem> SelectedLabIncharge
        {
            get
            {
                return _LabIncharge;
            }
            set
            {
                _LabIncharge = value;
            }
        }
        //private void FillThawingDetails()
        //{
        //    try
        //    {
        //        cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
        //        bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
        //        bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
        //        bizAction.IsThawDetails = true;
        //        bizAction.IsPagingEnabled = true;
        //        bizAction.StartIndex = ThawDetailsList.PageIndex * ThawDetailsList.PageSize;
        //        bizAction.MaximumRows = ThawDetailsList.PageSize;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList != null)
        //            {
        //                ThawDetailsList.Clear();

        //                ThawDetailsList.TotalItemCount = (int)((cls_NewGetSpremThawingBizActionVO)arg.Result).TotalRows;
        //                for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
        //                {
        //                    if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
        //                    {
        //                        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;
        //                    }
        //                    else
        //                    {
        //                        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = true;
        //                    }
        //                    if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
        //                    {
        //                        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
        //                    }
        //                    else
        //                    {
        //                        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
        //                    }
        //                    ThawDetailsList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
        //                }
        //                dgThawingDetilsGrid.ItemsSource = null;
        //                dgThawingDetilsGrid.ItemsSource = ThawDetailsList;
        //                dgThawingDetilsGrid.UpdateLayout();
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }
        //        };
        //        client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (Thawing != null)
                {
                    if (Thawing.IsSelected)
                    {
                        //fillLabIncharge();                       
                        fillThawingDetails();

                        //dgThawingViDetilsGrid.IsEnabled = false;
                    }
                }
                if (Vitrification != null)
                {
                    if (Vitrification.IsSelected)
                    {
                        //SSemenMaster();

                        fillDetails();
                    }
                }

                if (ThawingOocyte != null)
                {
                    if (ThawingOocyte.IsSelected)
                    {
                        // fillLabIncharge();
                        fillThawingDetailsOT();
                        //dgThawingViDetilsGridOocyte.IsEnabled = false;
                    }
                }
                if (OVitrification != null)
                {
                    if (OVitrification.IsSelected)
                    {
                        //SSemenMaster();
                        fillDetailsOV();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void fillLabIncharge()
        {
            try
            {
                wait.Show();
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
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
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                        cmbLabPerson.ItemsSource = null;
                        cmbLabPerson.ItemsSource = objList;
                        cmbLabPerson.SelectedItem = objList[0];
                        cmbLabPersonOcy.ItemsSource = null;
                        cmbLabPersonOcy.ItemsSource = objList;
                        cmbLabPersonOcy.SelectedItem = objList[0];

                        fillOocyteMaturity();

                        //if (Thawing != null && Thawing.IsSelected == true)
                        //    fillThawingDetails();
                        //else
                        //    fillThawingDetailsOT();
                        //fillThawingDetailsForOocyte();

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

        private void FillCleavageGradeDay2()
        {
            clsGetCleavageGradeMasterListBizActionVO BizAction = new clsGetCleavageGradeMasterListBizActionVO();
            BizAction.CleavageGrade = new clsCleavageGradeMasterVO();
            BizAction.CleavageGrade.ApplyTo = 2;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null && ((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList != null)
                {

                    //CleavageGradeDay2 = list;


                    List<MasterListItem> list = new List<MasterListItem>();
                    list.Add(new MasterListItem(0, "-- Select --"));
                    list.AddRange(((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList);

                    //List<MasterListItem> CleavageGrade2and3 = new List<MasterListItem>();
                    //List<MasterListItem> CleavageGrade4 = new List<MasterListItem>();
                    CleavageGradeDay3.Add(new MasterListItem(0, "-- Select --"));
                    CleavageGradeDay3.AddRange(list.Where(x => x.ApplyTo == 3).ToList());

                    CleavageGradeDay4.Add(new MasterListItem(0, "-- Select --"));
                    CleavageGradeDay4.AddRange(list.Where(x => x.ApplyTo == 4).ToList());


                    FillPostThawingPlan();
                    // FillCleavageGradeDay3();


                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //private void FillCleavageGradeDay3()
        //{
        //    clsGetCleavageGradeMasterListBizActionVO BizAction = new clsGetCleavageGradeMasterListBizActionVO();
        //    BizAction.CleavageGrade = new clsCleavageGradeMasterVO();
        //    BizAction.CleavageGrade.ApplyTo = 3;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null && ((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList != null)
        //        {
        //            List<MasterListItem> list = new List<MasterListItem>();
        //            list.Add(new MasterListItem(0, "-- Select --"));
        //            list.AddRange(((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList);
        //            CleavageGradeDay3 = list;
        //            FillCleavageGradeDay4();

        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //private void FillCleavageGradeDay4()
        //{
        //    clsGetCleavageGradeMasterListBizActionVO BizAction = new clsGetCleavageGradeMasterListBizActionVO();
        //    BizAction.CleavageGrade = new clsCleavageGradeMasterVO();
        //    BizAction.CleavageGrade.ApplyTo = 4;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null && ((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList != null)
        //        {
        //            List<MasterListItem> list = new List<MasterListItem>();
        //            list.Add(new MasterListItem(0, "-- Select --"));
        //            list.AddRange(((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList);
        //            CleavageGradeDay4 = list;
        //            FillPostThawingPlan();

        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        long ThawID = 0;

        private void fillThawingDetails()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetThawingBizActionVO BizAction = new clsIVFDashboard_GetThawingBizActionVO();
                BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
                BizAction.Thawing.PlanTherapyID = PlanTherapyID;
                BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                BizAction.IsOnlyForEmbryoThawing = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        ThawDetails.Clear();
                        this.DataContext = (clsIVFDashboard_GetThawingBizActionVO)arg.Result;
                        ThawID = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.ID;
                        //dtthawingDate.SelectedDate = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        //txtthawingTime.Value = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        cmbLabPerson.SelectedValue = (long)((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.LabPersonId;
                        if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.IsETFreezed == true)
                        {
                            cmdSave.IsEnabled = false;
                        }
                        else
                        {
                            cmdSave.IsEnabled = true;
                        }

                        //for (int i = 0; i < ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList.Count; i++)
                        //{
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 1 || ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 2)
                        //    {
                        //        Grade = CleavageGradeDay2;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade5Enable = false;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeEnable = true;
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 3)
                        //    {
                        //        Grade = CleavageGradeDay3;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade5Enable = false;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeEnable = true;
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 4)
                        //    {
                        //        Grade = CleavageGradeDay4;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade5Enable = false;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeEnable = true;
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 5 || ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TransferDayNo == 6)
                        //    {
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeEnable = false;
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade5Enable = true;
                        //    }

                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID > 0)
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].IsEnabled = false;
                        //    else
                        //        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].IsEnabled = true;


                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeList = Grade;
                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageList = CellStage;
                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGradeList = StageOfDevelopmentGrade;
                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGradeList = InnerCellMassGrade;
                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGradeList = TrophoectodermGrade;
                        //    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanList = EmbryoPlan;

                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID != 0)
                        //    {
                        //        Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID));
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID != 0)
                        //    {
                        //        CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID));
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGradeID != 0)
                        //    {
                        //        Dev = StageOfDevelopmentGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGradeID));
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGradeID != 0)
                        //    {
                        //        Inner = InnerCellMassGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGradeID));
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGradeID != 0)
                        //    {
                        //        Tro = TrophoectodermGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGradeID));
                        //    }
                        //    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID != null)
                        //    {
                        //        EPlan = EmbryoPlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID));
                        //    }

                        //    if (Gr != null)
                        //    {
                        //        if (Gr.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedGrade = Gr;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade = Gr.Description;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID = Gr.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID = Gr.ID;
                        //        }
                        //    }


                        //    if (CS != null)
                        //    {
                        //        if (CS.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedCellStage = CS;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = CS.Description;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;
                        //        }
                        //    }

                        //    if (Dev != null)
                        //    {
                        //        if (Dev.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedStageOfDevelopmentGrade = Dev;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGrade = Dev.Name;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGradeID = Dev.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGrade = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].StageOfDevelopmentGradeID = Dev.ID;
                        //        }
                        //    }

                        //    if (Inner != null)
                        //    {
                        //        if (Inner.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedInnerCellMassGrade = Inner;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGrade = Inner.Name;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGradeID = Inner.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGrade = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].InnerCellMassGradeID = Inner.ID;
                        //        }
                        //    }

                        //    if (Tro != null)
                        //    {
                        //        if (Tro.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedTrophoectodermGrade = Tro;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGrade = Tro.Name;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGradeID = Tro.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGrade = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].TrophoectodermGradeID = Tro.ID;
                        //        }
                        //    }

                        //    if (EPlan != null)
                        //    {
                        //        if (EPlan.ID > 0)
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedPostThawingPlan = EPlan;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlan = EPlan.Description;
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID = EPlan.ID;
                        //        }
                        //        else
                        //        {
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlan = "";
                        //            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID = EPlan.ID;
                        //        }
                        //    }

                        //    ThawDetails.Add(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i]);
                        //}

                        foreach (var item in ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem CS = new MasterListItem();
                            MasterListItem Dev = new MasterListItem();
                            MasterListItem Inner = new MasterListItem();
                            MasterListItem Tro = new MasterListItem();
                            MasterListItem EPlan = new MasterListItem();

                            if (item.TransferDayNo == 1 || item.TransferDayNo == 2)
                            {
                                Grade = CleavageGradeDay3;
                                item.Grade5Enable = false;
                                item.GradeEnable = true;
                            }
                            if (item.TransferDayNo == 3)
                            {
                                Grade = CleavageGradeDay3;
                                item.Grade5Enable = false;
                                item.GradeEnable = true;
                            }
                            if (item.TransferDayNo == 4)
                            {
                                Grade = CleavageGradeDay4;
                                item.Grade5Enable = false;
                                item.GradeEnable = true;
                            }
                            if (item.TransferDayNo == 5 || item.TransferDayNo == 6)
                            {
                                item.GradeEnable = false;
                                item.Grade5Enable = true;
                            }

                            if (item.PostThawingPlanID > 0)
                                item.IsEnabled = false;
                            else
                                item.IsEnabled = true;

                            item.GradeList = Grade;
                            //item.CellStageList = CellStage;
                            item.StageOfDevelopmentGradeList = StageOfDevelopmentGrade;
                            item.InnerCellMassGradeList = InnerCellMassGrade;
                            item.TrophoectodermGradeList = TrophoectodermGrade;
                            if (item.IsFreshEmbryoPGDPGS)
                            {

                                EmbryoPlanPGD = (from r in EmbryoPlan where r.ID != 7 select r).ToList();
                                item.PostThawingPlanList = EmbryoPlanPGD;
                            }
                            else
                                item.PostThawingPlanList = EmbryoPlan;

                            if (item.GradeID != 0)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(item.GradeID));
                            }
                            //if (item.CellStageID != 0)
                            //{
                            //    CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(item.CellStageID));
                            //}
                            if (item.StageOfDevelopmentGradeID != 0)
                            {
                                Dev = StageOfDevelopmentGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(item.StageOfDevelopmentGradeID));
                            }
                            if (item.InnerCellMassGradeID != 0)
                            {
                                Inner = InnerCellMassGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(item.InnerCellMassGradeID));
                            }
                            if (item.TrophoectodermGradeID != 0)
                            {
                                Tro = TrophoectodermGrade.FirstOrDefault(p => p.ID == Convert.ToInt64(item.TrophoectodermGradeID));
                            }
                            if (item.PostThawingPlanID != null)
                            {
                                EPlan = EmbryoPlan.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PostThawingPlanID));
                            }

                            if (Gr != null)
                            {
                                if (Gr.ID > 0)
                                {
                                    item.SelectedGrade = Gr;
                                    item.Grade = Gr.Description;
                                    item.GradeID = Gr.ID;
                                }
                                else
                                {
                                    item.Grade = "";
                                    item.GradeID = Gr.ID;
                                }
                            }


                            //if (CS != null)
                            //{
                            //    if (CS.ID > 0)
                            //    {
                            //        item.SelectedCellStage = CS;
                            //        item.CellStage = CS.Description;
                            //        item.CellStageID = CS.ID;
                            //    }
                            //    else
                            //    {
                            //        item.CellStage = "";
                            //        item.CellStageID = CS.ID;
                            //    }
                            //}

                            if (Dev != null)
                            {
                                if (Dev.ID > 0)
                                {
                                    item.SelectedStageOfDevelopmentGrade = Dev;
                                    item.StageOfDevelopmentGrade = Dev.Name;
                                    item.StageOfDevelopmentGradeID = Dev.ID;
                                }
                                else
                                {
                                    item.StageOfDevelopmentGrade = "";
                                    item.StageOfDevelopmentGradeID = Dev.ID;
                                }
                            }

                            if (Inner != null)
                            {
                                if (Inner.ID > 0)
                                {
                                    item.SelectedInnerCellMassGrade = Inner;
                                    item.InnerCellMassGrade = Inner.Name;
                                    item.InnerCellMassGradeID = Inner.ID;
                                }
                                else
                                {
                                    item.InnerCellMassGrade = "";
                                    item.InnerCellMassGradeID = Inner.ID;
                                }
                            }

                            if (Tro != null)
                            {
                                if (Tro.ID > 0)
                                {
                                    item.SelectedTrophoectodermGrade = Tro;
                                    item.TrophoectodermGrade = Tro.Name;
                                    item.TrophoectodermGradeID = Tro.ID;
                                }
                                else
                                {
                                    item.TrophoectodermGrade = "";
                                    item.TrophoectodermGradeID = Tro.ID;
                                }
                            }

                            if (EPlan != null)
                            {
                                if (EPlan.ID > 0)
                                {
                                    item.SelectedPostThawingPlan = EPlan;
                                    item.PostThawingPlan = EPlan.Description;
                                    item.PostThawingPlanID = EPlan.ID;
                                }
                                else
                                {
                                    item.PostThawingPlan = "";
                                    item.PostThawingPlanID = EPlan.ID;
                                }
                            }

                            if (item.IsFreshEmbryoPGDPGS || item.IsFrozenEmbryoPGDPGS)
                                item.PGDPGS = "PGDPGS";

                            ThawDetails.Add(item);
                        }

                        dgThawingDetilsGrid.ItemsSource = null;
                        dgThawingDetilsGrid.ItemsSource = ThawDetails;

                        fillDetailsForThawTab();    //For IVF ADM changes   // method to get cryo embryos those are thawed & show on thaw embryo tab 

                        //fillDetails();  commented by neena
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

        private void fillDetailsForThawTab()    // method to get cryo embryos those are thawed & show on thaw embryo tab    // For IVF ADM Changes
        {
            try
            {
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = false;

                BizAction.IsForThawTab = true;          //Flag set while retrive Cryo Oocytes under FE ICSI cycle from Freeze All Oocytes Cycle 

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails.Clear();
                        this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;

                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdNew.IsEnabled = false;
                            Thawing.IsEnabled = true;
                        }

                        chkFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;

                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;
                        }
                        // For IVF ADM Changes

                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem PT = new MasterListItem();
                            MasterListItem CS = new MasterListItem();

                            //MasterListItem CC = new MasterListItem();       // For IVF ADM Changes
                            MasterListItem TC = new MasterListItem();


                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID != null)
                            //{
                            //    PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID));
                            //}
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }

                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID != null)
                            //{
                            //    CC = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}

                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TabColorID != null)
                            //{
                            //    TC = TabColorList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TabColorID));
                            //}

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeList = CryoCodeList.DeepCopy();
                            //if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID) > 0)
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}
                            //else
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == 0);
                            //}

                            //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TabColorList = TabColorList.DeepCopy();
                            //if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TabColorID) > 0)
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTabColor = TabColorList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TabColorID));
                            //}
                            //else
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTabColor = TabColorList.FirstOrDefault(p => p.ID == 0);
                            //}


                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsFreshEmbryoPGDPGS || ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].IsFrozenEmbryoPGDPGS)
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PGDPGS = "PGDPGS";


                            VitriDetails.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                        }

                        //if (Vitrification != null)
                        //{
                        //    if (Vitrification.IsSelected == true)
                        //    {
                        //        dgVitrificationDetilsGrid.ItemsSource = null;
                        //        dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                        //    }
                        //}

                        if (Thawing != null)
                        {
                            if (Thawing.IsSelected == true)
                            {
                                dgThawingViDetilsGrid.ItemsSource = null;
                                dgThawingViDetilsGrid.ItemsSource = VitriDetails.DeepCopy();
                            }
                        }

                        ////...............................................
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 5)   // Freeze / Thaw transfer
                        //{
                        //    fillVitificationDetailsForThawTransfer();
                        //}
                        ////................................................

                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 
                        //    && PlanTherapyVO.PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreezeAllOocyteCycleID)   // Freeze / Thaw transfer
                        //{
                        //    fillDetailsOV();
                        //}

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

        //private void fillThawingDetailsForOocyte()
        //{
        //    try
        //    {
        //        clsIVFDashboard_GetThawingBizActionVO BizAction = new clsIVFDashboard_GetThawingBizActionVO();
        //        BizAction.ThawingForOocyte = new clsIVFDashBoard_ThawingVO();
        //        BizAction.ThawingForOocyte.PlanTherapyID = PlanTherapyID;
        //        BizAction.ThawingForOocyte.PlanTherapyUnitID = PlanTherapyUnitID;
        //        if (CoupleDetails != null)
        //        {
        //            BizAction.ThawingForOocyte.PatientID = CoupleDetails.FemalePatient.PatientID;
        //            BizAction.ThawingForOocyte.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
        //        }
        //        BizAction.IsOnlyForEmbryoThawing = false;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                ThawDetailsForOocyte.Clear();
        //                this.DataContext = (clsIVFDashboard_GetThawingBizActionVO)arg.Result;
        //                ThawID = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingForOocyte.ID;
        //                //dtthawingDate.SelectedDate = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
        //                //txtthawingTime.Value = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
        //                cmbLabPerson.SelectedValue = (long)((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingForOocyte.LabPersonId;
        //                if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingForOocyte.IsETFreezed == true)
        //                {
        //                    cmdSave.IsEnabled = false;
        //                }
        //                else
        //                {
        //                    cmdSave.IsEnabled = true;
        //                }

        //                MasterListItem Gr = new MasterListItem();
        //                MasterListItem CS = new MasterListItem(); ;

        //                MasterListItem OPlan = new MasterListItem();

        //                for (int i = 0; i < ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList.Count; i++)
        //                {
        //                    Grade = CleavageGradeDay2;
        //                    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanID > 0)
        //                        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].IsEnabled = false;
        //                    else
        //                        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].IsEnabled = true;

        //                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].GradeList = Grade;
        //                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStageList = CellStage;
        //                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanList = OocytePlan;

        //                    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].GradeID != 0)
        //                    {
        //                        Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].GradeID));
        //                    }
        //                    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStageID != 0)
        //                    {
        //                        CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStageID));
        //                    }

        //                    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanID != null)
        //                    {
        //                        OPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanID));
        //                    }


        //                    //oocyte grade
        //                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].OocyteGradeList = mlSourceOfSperm;
        //                    if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].OocyteGradeID > 0)
        //                    {
        //                        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].OocyteGradeID));
        //                    }
        //                    else
        //                    {
        //                        ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
        //                    }
        //                    //

        //                    if (Gr != null)
        //                    {
        //                        if (Gr.ID > 0)
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].SelectedGrade = Gr;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].Grade = Gr.Description;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].GradeID = Gr.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].Grade = "";
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].GradeID = Gr.ID;
        //                        }
        //                    }


        //                    if (CS != null)
        //                    {
        //                        if (CS.ID > 0)
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].SelectedCellStage = CS;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStage = CS.Description;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStageID = CS.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStage = "";
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].CellStageID = CS.ID;
        //                        }
        //                    }


        //                    if (OPlan != null)
        //                    {
        //                        if (OPlan.ID > 0)
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].SelectedPostThawingPlan = OPlan;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlan = OPlan.Description;
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanID = OPlan.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlan = "";
        //                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i].PostThawingPlanID = OPlan.ID;
        //                        }
        //                    }

        //                    ThawDetailsForOocyte.Add(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsForOocyteList[i]);
        //                }
        //                dgThawingDetilsGridOocyte.ItemsSource = null;
        //                dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;
        //                fillDetails();
        //            }
        //            wait.Close();
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        private bool Validate()
        {
            bool result = true;
            if (dtVitrificationDate.SelectedDate == null)
            {
                dtVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtVitrificationDate.RaiseValidationError();
                dtVitrificationDate.Focus();
                result = false;
            }
            else if (txtTime.Value == null)
            {
                dtVitrificationDate.ClearValidationError();
                txtTime.SetValidation("Please Select Vitrification Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                result = false;
            }
            else if (VitriDetails.Count <= 0)
            {

                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
            }
            else
            {
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                result = true;
            }
            return result;
        }

        private bool OocyteValidate()
        {
            bool result = true;
            DateTime? VitriDate = null;
            DateTime? ExpiryDate = null;



            if (dtOcVitrificationDate.SelectedDate != null && txtOcTime.Value != null)
            {
                VitriDate = dtOcVitrificationDate.SelectedDate.Value.Date;
                VitriDate = VitriDate.Value.Add(txtOcTime.Value.Value.TimeOfDay);
            }

            if (dtOcExpiryDate.SelectedDate != null && txtOcExpiryTime.Value != null)
            {
                ExpiryDate = dtOcExpiryDate.SelectedDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(txtOcExpiryTime.Value.Value.TimeOfDay);
            }

            if (dtOcVitrificationDate.SelectedDate == null)
            {
                dtOcVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtOcVitrificationDate.RaiseValidationError();
                dtOcVitrificationDate.Focus();
                result = false;
            }
            else if (txtOcTime.Value == null)
            {
                dtOcVitrificationDate.ClearValidationError();
                txtOcTime.SetValidation("Please Select Vitrification Time");
                txtOcTime.RaiseValidationError();
                txtOcTime.Focus();
                result = false;
            }
            else if (dtOcExpiryDate.SelectedDate == null)
            {
                txtOcTime.ClearValidationError();
                dtOcExpiryDate.SetValidation("Please Select Expiry Date");
                dtOcExpiryDate.RaiseValidationError();
                dtOcExpiryDate.Focus();
                result = false;
            }
            else if (txtOcExpiryTime.Value == null)
            {
                dtOcExpiryDate.ClearValidationError();
                txtOcExpiryTime.ClearValidationError();
                txtOcExpiryTime.SetValidation("Please Select Expiry Time");
                txtOcExpiryTime.RaiseValidationError();
                txtOcExpiryTime.Focus();
                result = false;
            }
            else if (ExpiryDate < VitriDate)
            {
                txtOcExpiryTime.ClearValidationError();
                dtOcExpiryDate.SetValidation("Expiry Date Cannot Be Less Than Start Date  ");
                dtOcExpiryDate.RaiseValidationError();
                dtOcExpiryDate.Focus();
                dtOcExpiryDate.Text = " ";
                dtOcExpiryDate.Focus();
                result = false;
            }
            else if (VitriDetailsOocytes.Count <= 0)
            {
                dtOcExpiryDate.ClearValidationError();
                txtOcExpiryTime.ClearValidationError();
                dtOcVitrificationDate.ClearValidationError();
                txtOcTime.ClearValidationError();
                dtOcPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
            }
            else if (VitriDetailsOocytes.Count > 0)
            {
                var MaxDate = (from d in VitriDetailsOocytes select d.TransferDate).Max();

                if (VitriDate < MaxDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Date can not be less than Plan Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                }
            }
            else
            {
                dtOcExpiryDate.ClearValidationError();
                txtOcExpiryTime.ClearValidationError();
                dtOcVitrificationDate.ClearValidationError();
                txtOcTime.ClearValidationError();
                dtOcPickUpDate.ClearValidationError();
                result = true;
            }


            return result;
        }

        private void cmbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbStraw"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].StrawId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletShape"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletShapeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletSize"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletSizeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanisterId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].ConistorNo = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbTankId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].TankId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanID"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].CanId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbColorSelector"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].ColorCodeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
        }

        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(PlanTherapyID, PlanTherapyUnitID, CoupleDetails, "Vitrification");
            Win.Show();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //if (Validate())
            //{

            if (blEmdSave)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please thaw all embryo", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                fillDetails();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            //}
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
            else
                fillDetails();
        }

        private void Save()
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
            BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
            //BizAction.VitrificationMain.DateTime = dtVitrificationDate.SelectedDate.Value.Date;
            //BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            BizAction.VitrificationMain.IsOnlyForEmbryoVitrification = true;

            for (int i = 0; i < VitriDetails.Count; i++)
            {
                VitriDetails[i].CanId = VitriDetails[i].SelectedCanId.ID;
                VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID;
                VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID;
                VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID;
                VitriDetails[i].ConistorNo = VitriDetails[i].SelectedCanisterId.ID;
                VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID;
                VitriDetails[i].ColorCodeID = VitriDetails[i].SelectedColorSelector.ID;
            }
            //if (PlanTherapyVO.PlannedTreatmentID == 5)
            //{
            //    BizAction.VitrificationMain.UsedByOtherCycle = true;
            //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
            //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
            //}
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetails.ToList());
            if (chkFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;
            if (IsEmbryoDonation == true)
                BizAction.VitrificationMain.IsOnlyVitrification = true;


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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }



        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsIVFDashBoard_ThawingDetailsVO)(row.DataContext)).Plan == true)
            {
                e.Row.IsEnabled = false;
            }
        }

        private void cmbCellStage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].CellStageID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private bool ValidateThaw()
        {
            bool result = true;
            //if (dtthawingDate.SelectedDate == null)
            //{
            //    dtthawingDate.SetValidation("Please Select Thawing Date");
            //    dtthawingDate.RaiseValidationError();
            //    dtthawingDate.Focus();
            //    return false;
            //}
            //else if (txtthawingTime.Value == null)
            //{
            //    dtthawingDate.ClearValidationError();
            //    txtthawingTime.SetValidation("Please Select Thawing Time");
            //    txtthawingTime.RaiseValidationError();
            //    txtthawingTime.Focus();
            //    return false;
            //}
            //else 

            if (Thawing.IsSelected)
            {
                if (cmbLabPerson.ItemsSource != null)
                {
                    if (cmbLabPerson.SelectedItem == null)
                    {
                        cmbLabPerson.TextBox.SetValidation("Please Select Lab Person");
                        cmbLabPerson.TextBox.RaiseValidationError();
                        cmbLabPerson.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbLabPerson.SelectedItem).ID == 0)
                    {
                        cmbLabPerson.TextBox.SetValidation("Please Select Lab Person");
                        cmbLabPerson.TextBox.RaiseValidationError();
                        cmbLabPerson.Focus();
                        result = false;
                    }
                    else
                        cmbLabPerson.TextBox.ClearValidationError();
                }
            }

            if (ThawingOocyte.IsSelected)
            {
                if (cmbLabPersonOcy.ItemsSource != null)
                {
                    if (cmbLabPersonOcy.SelectedItem == null)
                    {
                        cmbLabPersonOcy.TextBox.SetValidation("Please Select Lab Person");
                        cmbLabPersonOcy.TextBox.RaiseValidationError();
                        cmbLabPersonOcy.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbLabPersonOcy.SelectedItem).ID == 0)
                    {
                        cmbLabPersonOcy.TextBox.SetValidation("Please Select Lab Person");
                        cmbLabPersonOcy.TextBox.RaiseValidationError();
                        cmbLabPersonOcy.Focus();
                        result = false;
                    }
                    else
                        cmbLabPersonOcy.TextBox.ClearValidationError();
                }
            }
            //else if (ThawDetails.Count <= 0)
            //{

            //    // dtthawingDate.ClearValidationError();
            //    // txtthawingTime.ClearValidationError();
            //    cmbLabPerson.TextBox.ClearValidationError();

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    return false;
            //}
            return result;
        }
        private void cmdSaveThawing_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateThaw())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedThaw);
                msgW.Show();
            }
        }
        void msgW_OnMessageBoxClosedThaw(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveThawing();
        }
        private void SaveThawing()
        {
            clsIVFDashboard_AddUpdateThawingBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingBizActionVO();
            BizAction.ThawingDetailsList = new List<clsIVFDashBoard_ThawingDetailsVO>();
            BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
            BizAction.Thawing.ID = ThawID;
            BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Thawing.PlanTherapyID = PlanTherapyID;
            BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
            //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
            //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

            BizAction.Thawing.UsedByOtherCycle = true;
            BizAction.Thawing.UsedTherapyID = PlanTherapyID;
            BizAction.Thawing.UsedTherapyUnitID = PlanTherapyUnitID;



            BizAction.Thawing.LabPersonId = ((MasterListItem)cmbLabPerson.SelectedItem).ID;
            BizAction.ThawingDetailsList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetails.ToList());

            BizAction.IsOnlyForEmbryoThawing = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillThawingDetails();
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

        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }

        }

        private void fillVitificationDetailsForThawTransferOT()       // For IVF ADM Changes
        {
            clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            }
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            BizAction.VitrificationMain.UsedOwnOocyte = true;

            BizAction.IsFreezeOocytes = true;   //IsFreezeOocytes Flag set to retrive Freeze Oocytes under FE ICSI Cycle from Freeze All Oocytes Cycle      // For IVF ADM Changes

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    VitriDetailsOocytes = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();       // For IVF ADM Changes

                    MasterListItem Gr = new MasterListItem();
                    MasterListItem PT = new MasterListItem();
                    MasterListItem CS = new MasterListItem();
                    MasterListItem TC = new MasterListItem();

                    for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                    {
                        Gr = new MasterListItem();
                        PT = new MasterListItem();
                        CS = new MasterListItem();
                        TC = new MasterListItem();
                        MasterListItem PoPlan = new MasterListItem();

                        if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                        {
                            Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                        }
                        if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                        {
                            CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                        }

                        if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PostThawingPlanID != null)
                        {
                            PoPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PostThawingPlanID));
                        }


                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList.DeepCopy();   // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor.DeepCopy();   // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw.DeepCopy();   // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape.DeepCopy();     // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize.DeepCopy();       // For IVF ADM Changes
                        {
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                            }
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister.DeepCopy();     // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                        }



                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank.DeepCopy();       // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade.DeepCopy();     // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage.DeepCopy();     // For IVF ADM Changes
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PostThawingPlanList = OocytePlan;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PostThawingPlanID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].PostThawingPlanID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == 0);
                        }

                        VitriDetailsOocytes.Add(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);     // For IVF ADM Changes

                    }

                    dgOcyVitrificationDetilsGrid.ItemsSource = null;                 // For IVF ADM Changes
                    dgOcyVitrificationDetilsGrid.ItemsSource = VitriDetailsOocytes;  // For IVF ADM Changes
                    wait.Close();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            wait.Close();
        }

        //private void fillVitificationDetailsForOocyteForThawTransfer()
        //{
        //    clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
        //    BizAction.VitrificationMainForOocyte = new clsIVFDashBoard_VitrificationVO();
        //    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
        //    {
        //        BizAction.VitrificationMainForOocyte.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
        //        BizAction.VitrificationMainForOocyte.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
        //    }
        //    BizAction.VitrificationMainForOocyte.IsOnlyVitrification = false;
        //    BizAction.VitrificationMainForOocyte.UsedOwnOocyte = true;
        //    BizAction.VitrificationMainForOocyte.IsOnlyForEmbryoVitrification = true;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            VitriDetailsForOocyte = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

        //            MasterListItem Gr = new MasterListItem();
        //            MasterListItem PT = new MasterListItem();
        //            MasterListItem CS = new MasterListItem();

        //            MasterListItem PoPlan = new MasterListItem();

        //            for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList.Count; i++)
        //            {
        //                if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID != null)
        //                {
        //                    Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID));
        //                }
        //                if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID != null)
        //                {
        //                    CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID));
        //                }

        //                if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID != null)
        //                {
        //                    PoPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID));
        //                }

        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanIdList = CanList;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanId) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanId));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
        //                }

        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorSelectorList = GobletColor;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorCodeID) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ColorCodeID));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
        //                }

        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawIdList = Straw;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawId) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].StrawId));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
        //                }
        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeList = GobletShape;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeId) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletShapeId));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
        //                }
        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeList = GobletSize;
        //                {
        //                    if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeId) > 0)
        //                    {
        //                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GobletSizeId));
        //                    }
        //                    else
        //                    {
        //                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
        //                    }
        //                }
        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CanisterIdList = Canister;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ConistorNo) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].ConistorNo));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
        //                }



        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankList = Tank;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankId) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].TankId));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
        //                }

        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeList = Grade;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].GradeID));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
        //                }
        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageList = CellStage;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].CellStageID));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
        //                }


        //                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanList = OocytePlan;
        //                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID) > 0)
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].PostThawingPlanID));
        //                }
        //                else
        //                {
        //                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i].SelectedPostThawingPlan = OocytePlan.FirstOrDefault(p => p.ID == 0);
        //                }

        //                VitriDetailsForOocyte.Add(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsForOocyteList[i]);

        //            }

        //            dgOcyVitrificationDetilsGrid.ItemsSource = null;
        //            dgOcyVitrificationDetilsGrid.ItemsSource = VitriDetailsForOocyte;
        //            wait.Close();
        //        }

        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //    wait.Close();
        //}

        private void fillVitificationDetailsForThawTransfer()
        {
            clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            }
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            BizAction.VitrificationMain.UsedOwnOocyte = true;
            BizAction.VitrificationMain.IsOnlyForEmbryoVitrification = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();


                    for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                    {
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem();
                        MasterListItem CS = new MasterListItem();

                        if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                        {
                            Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                        }
                        if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                        {
                            CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                        {
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                            }
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                        }



                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                        }

                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                        }
                        ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage;
                        if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                        }
                        else
                        {
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                        }

                        VitriDetails.Add(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                    }

                    dgVitrificationDetilsGrid.ItemsSource = null;
                    dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                    wait.Close();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            wait.Close();
        }



        private void cmdSaveOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (OocyteValidate())
            {
                //if (blOocyteSave)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please thaw all oocyte", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgW1.Show();
                //    fillDetails();
                //}
                //else
                //{
                MessageBoxControl.MessageBoxChildWindow msgWOocyte =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWOocyte.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWmsgWOocyte_OnMessageBoxClosed);
                msgWOocyte.Show();
                //}
            }
        }


        void msgWmsgWOocyte_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveOV();
            // SaveOocyte();
            //else
            //    fillDetails();
        }

        private void SaveOV()   // For IVF ADM Changes
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
            BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.VitrificationMain.DateTime = dtOcVitrificationDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtOcTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.ExpiryDate = dtOcExpiryDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.ExpiryDate = BizAction.VitrificationMain.ExpiryDate.Value.Add(txtOcExpiryTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            for (int i = 0; i < VitriDetailsOocytes.Count; i++)
            {
                VitriDetailsOocytes[i].CanId = VitriDetailsOocytes[i].SelectedCanId.ID;
                VitriDetailsOocytes[i].StrawId = VitriDetailsOocytes[i].SelectedStrawId.ID;
                VitriDetailsOocytes[i].GobletShapeId = VitriDetailsOocytes[i].SelectedGobletShape.ID;
                VitriDetailsOocytes[i].GobletSizeId = VitriDetailsOocytes[i].SelectedGobletSize.ID;
                VitriDetailsOocytes[i].ConistorNo = VitriDetailsOocytes[i].SelectedCanisterId.ID;
                VitriDetailsOocytes[i].TankId = VitriDetailsOocytes[i].SelectedTank.ID;
                VitriDetailsOocytes[i].ColorCodeID = VitriDetailsOocytes[i].SelectedColorSelector.ID;

                VitriDetailsOocytes[i].GradeID = VitriDetailsOocytes[i].SelectedOocyteGrade.ID;  //oocytegarde into grade
            }
            //if (PlanTherapyVO.PlannedTreatmentID == 5)
            //{
            //    BizAction.VitrificationMain.UsedByOtherCycle = true;
            //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
            //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
            //}
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetailsOocytes.ToList());
            if (chkOcFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;
            if (IsEmbryoDonation == true)
                BizAction.VitrificationMain.IsOnlyVitrification = true;

            BizAction.IsFreezeOocytes = true;   //Flag set while save Freeze Oocytes under Freeze All Oocytes Cycle for freeze & Thaw   //For IVF ADM changes


            BizAction.VitrificationMain.IsCryoWOThaw = true;  // Flag use to save only Vitrification\Cryo details from IVF/ICSI/IVF-ICSI/FE ICSI cycles which will be thaw under Freeze All Oocyte/Freeze-Thaw Transfer cycles    //For IVF ADM changes

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillDetailsOV();    // For IVF ADM Changes
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

        private void fillDetailsOV()      // For IVF ADM Changes
        {
            try
            {
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }

                BizAction.VitrificationMain.IsOnlyVitrification = false;

                BizAction.IsFreezeOocytes = true;       //Flag set to retrive Freeze Oocytes only under Freeze All Oocytes Cycle 

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetailsOocytes.Clear();        // For IVF ADM Changes
                        RefeezeVitriDetailsOocytes.Clear(); //added by neena
                        this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;


                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdNewOcy.IsEnabled = false;         // For IVF ADM Changes
                            //ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        }
                        else
                            cmdNewOcy.IsEnabled = true;
                        //else
                        //{
                        //    ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        //}

                        //added by neena
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.IsFreezed == true)
                        {
                            cmdRefeezeNewOcy.IsEnabled = false;         // For IVF ADM Changes
                            //ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        }
                        else
                            cmdRefeezeNewOcy.IsEnabled = true;
                        chkOcReFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.IsFreezed;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.DateTime != null)
                        {
                            dtRefeezeOcVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.DateTime;
                            txtRefeezeOcTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.ExpiryDate != null)
                        {
                            dtOcReExpiryDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.ExpiryDate;
                            txtOcReExpiryTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.ExpiryDate;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.VitrificationNo != null)
                        {
                            txtRefeezeOcVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeMain.VitrificationNo;    // For IVF ADM Changes
                        }
                        //

                        chkOcFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;      // For IVF ADM Changes
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtOcVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtOcTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ExpiryDate != null)
                        {
                            dtOcExpiryDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ExpiryDate;
                            txtOcExpiryTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ExpiryDate;
                        }

                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtOcVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;    // For IVF ADM Changes
                        }


                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem PT = new MasterListItem();
                            MasterListItem CS = new MasterListItem();

                            MasterListItem TC = new MasterListItem();       // For IVF ADM Changes

                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID != null)
                            //{
                            //    PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID));
                            //}
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }

                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID != null)
                            //{
                            //    CC = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}

                            //oocyte grade
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].OocyteGradeList = mlSourceOfSperm;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
                            }
                            //

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList.DeepCopy();
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw.DeepCopy();   // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize.DeepCopy();
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank.DeepCopy();       // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeList = CryoCodeList.DeepCopy();
                            //if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID) > 0)
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}
                            //else
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == 0);
                            //}

                            VitriDetailsOocytes.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);     // For IVF ADM Changes

                        }


                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList.Count; i++)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem PT = new MasterListItem();
                            MasterListItem CS = new MasterListItem();

                            MasterListItem TC = new MasterListItem();       // For IVF ADM Changes

                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GradeID));
                            }
                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ProtocolTypeID != null)
                            //{
                            //    PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ProtocolTypeID));
                            //}
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CellStageID));
                            }

                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID != null)
                            //{
                            //    CC = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}

                            //oocyte grade
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].OocyteGradeList = mlSourceOfSperm;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
                            }
                            //

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CanIdList = CanList.DeepCopy();
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].StrawIdList = Straw.DeepCopy();   // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletShapeList = GobletShape.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletSizeList = GobletSize.DeepCopy();
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].CanisterIdList = Canister.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].TankList = Tank.DeepCopy();       // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }


                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].IsRefreezeFromOtherCycle == true)
                            {
                                dgRefeezeOcyVitrificationDetilsGrid.Columns[20].Visibility = Visibility.Visible;
                                dgRefeezeOcyVitrificationDetilsGrid.Columns[21].Visibility = Visibility.Visible;
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].IsRefreezeFromBothCycle = "/PalashDynamics;component/Icons/green.png";
                            }
                            else if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].IsRefreeze == true)
                            {
                                dgRefeezeOcyVitrificationDetilsGrid.Columns[20].Visibility = Visibility.Visible;
                                dgRefeezeOcyVitrificationDetilsGrid.Columns[21].Visibility = Visibility.Visible;
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i].IsRefreezeFromBothCycle = "/PalashDynamics;component/Icons/yellow.png";
                            }


                            RefeezeVitriDetailsOocytes.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationRefeezeDetailsList[i]);

                        }

                        if (OVitrification != null)   // For IVF ADM Changes
                        {
                            if (OVitrification.IsSelected == true)    // For IVF ADM Changes
                            {
                                dgOcyVitrificationDetilsGrid.ItemsSource = null;
                                dgOcyVitrificationDetilsGrid.ItemsSource = VitriDetailsOocytes;    // For IVF ADM Changes

                                //var item1 = VitriDetailsOocytes.Where(x => x.IsRefreeze == true).ToList();
                                //if (item1.Count > 0)
                                //{
                                //    foreach (var item in item1)
                                //    {
                                //        RefeezeVitriDetailsOocytes.Add(item);
                                //    }

                                //    dgRefeezeOcyVitrificationDetilsGrid.ItemsSource = null;
                                //    dgRefeezeOcyVitrificationDetilsGrid.ItemsSource = RefeezeVitriDetailsOocytes;
                                //}

                                dgRefeezeOcyVitrificationDetilsGrid.ItemsSource = null;
                                dgRefeezeOcyVitrificationDetilsGrid.ItemsSource = RefeezeVitriDetailsOocytes;
                            }
                        }
                        if (ThawingOocyte != null)     // For IVF ADM Changes
                        {
                            if (ThawingOocyte.IsSelected == true)
                            {
                                dgThawingViDetilsGridOocyte.ItemsSource = null;                     // For IVF ADM Changes
                                dgThawingViDetilsGridOocyte.ItemsSource = VitriDetailsOocytes;      // For IVF ADM Changes
                            }
                        }
                        //...............................................
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 5)
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 18)   // For IVF ADM Changes
                        {
                            fillVitificationDetailsForThawTransferOT();     // For IVF ADM Changes
                        }
                        //................................................
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


        //private void SaveOocyte()
        //{
        //    clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
        //    BizAction.VitrificationMainForOocyte = new clsIVFDashBoard_VitrificationVO();
        //    BizAction.VitrificationDetailsForOocyteList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        //    BizAction.VitrificationMainForOocyte.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMainForOocyte.ID;
        //    BizAction.VitrificationMainForOocyte.PatientID = CoupleDetails.FemalePatient.PatientID;
        //    BizAction.VitrificationMainForOocyte.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
        //    BizAction.VitrificationMainForOocyte.PlanTherapyID = PlanTherapyID;
        //    BizAction.VitrificationMainForOocyte.PlanTherapyUnitID = PlanTherapyUnitID;
        //    BizAction.VitrificationMainForOocyte.DateTime = dtOcVitrificationDate.SelectedDate.Value.Date;
        //    BizAction.VitrificationMainForOocyte.DateTime = BizAction.VitrificationMainForOocyte.DateTime.Value.Add(txtOcTime.Value.Value.TimeOfDay);

        //    //if ((MasterListItem)cmbEmbryologistOocyte.SelectedItem != null)
        //    //    BizAction.VitrificationMainForOocyte.EmbryologistID = ((MasterListItem)cmbEmbryologistOocyte.SelectedItem).ID;
        //    //if ((MasterListItem)cmbAssistantEmbryologistOocyte.SelectedItem != null)
        //    //    BizAction.VitrificationMainForOocyte.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologistOocyte.SelectedItem).ID;

        //    BizAction.VitrificationMainForOocyte.IsOnlyVitrification = false;
        //    BizAction.VitrificationMain.IsOnlyForEmbryoVitrification = false;

        //    for (int i = 0; i < VitriDetailsForOocyte.Count; i++)
        //    {
        //        VitriDetailsForOocyte[i].CanId = VitriDetailsForOocyte[i].SelectedCanId.ID;
        //        VitriDetailsForOocyte[i].StrawId = VitriDetailsForOocyte[i].SelectedStrawId.ID;
        //        VitriDetailsForOocyte[i].GobletShapeId = VitriDetailsForOocyte[i].SelectedGobletShape.ID;
        //        VitriDetailsForOocyte[i].GobletSizeId = VitriDetailsForOocyte[i].SelectedGobletSize.ID;
        //        VitriDetailsForOocyte[i].ConistorNo = VitriDetailsForOocyte[i].SelectedCanisterId.ID;
        //        VitriDetailsForOocyte[i].TankId = VitriDetailsForOocyte[i].SelectedTank.ID;
        //        VitriDetailsForOocyte[i].ColorCodeID = VitriDetailsForOocyte[i].SelectedColorSelector.ID;
        //        VitriDetailsForOocyte[i].GradeID = VitriDetailsForOocyte[i].SelectedOocyteGrade.ID;  //oocytegarde into grade
        //    }
        //    //if (PlanTherapyVO.PlannedTreatmentID == 5)
        //    //{
        //    //    BizAction.VitrificationMain.UsedByOtherCycle = true;
        //    //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
        //    //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
        //    //}
        //    BizAction.VitrificationDetailsForOocyteList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetailsForOocyte.ToList());
        //    if (chkOcFreeze.IsChecked == true)
        //        BizAction.VitrificationMainForOocyte.IsFreezed = true;
        //    else
        //        BizAction.VitrificationMainForOocyte.IsFreezed = false;
        //    if (IsEmbryoDonation == true)
        //        BizAction.VitrificationMainForOocyte.IsOnlyVitrification = true;


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //            msgW1.Show();
        //            fillDetails();
        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //            msgW1.Show();
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        private bool ValidateThawForOocyte()
        {
            if (cmbLabPersonOcy.SelectedItem == null)
            {
                cmbLabPersonOcy.TextBox.SetValidation("Please Select Lab Person");
                cmbLabPersonOcy.TextBox.RaiseValidationError();
                cmbLabPersonOcy.Focus();
                return false;
            }
            else if (ThawDetailsForOocyte.Count <= 0)
            {
                cmbLabPersonOcy.TextBox.ClearValidationError();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                cmbLabPersonOcy.TextBox.ClearValidationError();
                return true;
            }
        }
        private void cmdSaveThawingOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateThawForOocyte())
            {
                MessageBoxControl.MessageBoxChildWindow msgWOocyte =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWOocyte.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWmsgWOocyte_OnMessageBoxClosedThaw);
                msgWOocyte.Show();
            }
        }

        void msgWmsgWOocyte_OnMessageBoxClosedThaw(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveThawingOocyte();
        }

        private void SaveThawingOocyte()
        {
            clsIVFDashboard_AddUpdateThawingBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingBizActionVO();
            BizAction.ThawingDetailsForOocyteList = new List<clsIVFDashBoard_ThawingDetailsVO>();
            BizAction.ThawingForOocyte = new clsIVFDashBoard_ThawingVO();
            BizAction.ThawingForOocyte.ID = ThawID;
            BizAction.ThawingForOocyte.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.ThawingForOocyte.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.ThawingForOocyte.PlanTherapyID = PlanTherapyID;
            BizAction.ThawingForOocyte.PlanTherapyUnitID = PlanTherapyUnitID;
            //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
            //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

            BizAction.ThawingForOocyte.UsedByOtherCycle = true;
            BizAction.ThawingForOocyte.UsedTherapyID = PlanTherapyID;
            BizAction.ThawingForOocyte.UsedTherapyUnitID = PlanTherapyUnitID;



            BizAction.ThawingForOocyte.LabPersonId = ((MasterListItem)cmbLabPersonOcy.SelectedItem).ID;
            BizAction.ThawingDetailsForOocyteList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetailsForOocyte.ToList());

            for (int i = 0; i < BizAction.ThawingDetailsForOocyteList.Count; i++)
            {
                foreach (var item in VitriDetailsForOocyte.OrderByDescending(x => x.ID))
                {
                    BizAction.ThawingDetailsForOocyteList[i].TransferDay = item.TransferDay;
                }
            }

            BizAction.IsOnlyForEmbryoThawing = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    // fillThawingDetailsForOocyte();
                    fillThawingDetailsOT();
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

        private void cmbComboOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbStraw"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].StrawId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletShape"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].GobletShapeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletSize"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].GobletSizeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanisterId"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].ConistorNo = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbTankId"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].TankId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanID"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].CanId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbColorSelector"))
            {
                for (int i = 0; i < VitriDetailsOocytes.Count; i++)
                {
                    if (VitriDetailsOocytes[i] == ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetailsOocytes[i].ColorCodeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
        }

        private void cmbCellStageOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetailsForOocyte.Count; i++)
            {
                if (i == dgThawingDetilsGridOocyte.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetailsForOocyte[i].CellStageID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void cmbGradeOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetailsForOocyte.Count; i++)
            {
                if (i == dgThawingDetilsGridOocyte.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetailsForOocyte[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void dgThawingDetilsGridOocyte_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsIVFDashBoard_ThawingDetailsVO)(row.DataContext)).Plan == true)
            {
                e.Row.IsEnabled = false;
            }
        }

        long planID = 0;
        private void cmbPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].PostThawingPlanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        planID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void cmbPlanOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetailsForOocyte.Count; i++)
            {
                if (i == dgThawingDetilsGridOocyte.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetailsForOocyte[i].PostThawingPlanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e) //for embryo saving
        {
            //if (Validate())
            //{

            DateTime? VitriDate = null;
            DateTime? ExpiryDate = null;

            if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate != null && ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime != null)
            {
                VitriDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate.Value.Date;
                VitriDate = VitriDate.Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime.Value.TimeOfDay);
            }

            if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate != null && ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime != null)
            {
                ExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime.Value.TimeOfDay);
            }

            if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Vitrification Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Vitrification Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Expiry Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Expiry Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else if (ExpiryDate < VitriDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Expiry Date Cannot Be Less Than Start Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else if (VitriDate < ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).TransferDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Vitrification Date Cannot Be Less Than Plan Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1Error_OnMessageBoxClosed);
                msgW1.Show();
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                //fillDetails();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                msgW1.Show();
            }
            //}
        }

        void msgW1Error_OnMessageBoxClosed(MessageBoxResult result)
        {
            foreach (var item in VitriDetails)
            {
                if (item.IsSaved == false)
                    item.IsEnabled = true;
            }
            dgVitrificationDetilsGrid.ItemsSource = null;
            dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
            //((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).IsEnabled = true;
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                BizAction.VitrificationDetailsObj = new clsIVFDashBoard_VitrificationDetailsVO();
                BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
                BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                BizAction.VitrificationMain.IsOnlyVitrification = false;
                BizAction.VitrificationMain.IsOnlyForEmbryoVitrification = true;
                BizAction.VitrificationMain.SaveForSingleEntry = true;
                //BizAction.VitrificationMain.DateTime = dtVitrificationDate.SelectedDate.Value.Date;
                //BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtTime.Value.Value.TimeOfDay);

                //for (int i = 0; i < VitriDetails.Count; i++)
                //{
                //    VitriDetails[i].CanId = VitriDetails[i].SelectedCanId.ID;
                //    VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID;
                //    VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID;
                //    VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID;
                //    VitriDetails[i].ConistorNo = VitriDetails[i].SelectedCanisterId.ID;
                //    VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID;
                //    VitriDetails[i].ColorCodeID = VitriDetails[i].SelectedColorSelector.ID;

                //}
                //if (PlanTherapyVO.PlannedTreatmentID == 5)
                //{
                //    BizAction.VitrificationMain.UsedByOtherCycle = true;
                //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
                //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
                //}

                BizAction.VitrificationDetailsObj.CanId = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedCanId.ID;
                BizAction.VitrificationDetailsObj.StrawId = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedStrawId.ID;
                BizAction.VitrificationDetailsObj.GobletShapeId = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedGobletShape.ID;
                BizAction.VitrificationDetailsObj.GobletSizeId = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedGobletSize.ID;
                BizAction.VitrificationDetailsObj.ConistorNo = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedCanisterId.ID;
                BizAction.VitrificationDetailsObj.TankId = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedTank.ID;
                BizAction.VitrificationDetailsObj.ColorCodeID = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectedColorSelector.ID;

                BizAction.VitrificationDetailsObj = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem);

                BizAction.VitrificationDetailsObj.VitrificationDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate;
                BizAction.VitrificationDetailsObj.VitrificationTime = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime;

                BizAction.VitrificationDetailsObj.ExpiryDate = (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate).Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime.Value.TimeOfDay);
                BizAction.VitrificationDetailsObj.ExpiryTime = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime;

                //BizAction.VitrificationDetailsObj.DateTime = Date.Value.Add(Time.Value);

                BizAction.VitrificationDetailsObj.TransferDayNo = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).TransferDayNo;
                BizAction.VitrificationDetailsObj.GradeID = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).GradeID;
                BizAction.VitrificationDetailsObj.StageofDevelopmentGrade = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).StageofDevelopmentGrade;
                BizAction.VitrificationDetailsObj.InnerCellMassGrade = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).InnerCellMassGrade;
                BizAction.VitrificationDetailsObj.TrophoectodermGrade = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).TrophoectodermGrade;
                BizAction.VitrificationDetailsObj.CellStage = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).CellStage;
                BizAction.VitrificationDetailsObj.VitrificationNo = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationNo;

                BizAction.VitrificationDetailsObj.IsFreshEmbryoPGDPGS = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).IsFreshEmbryoPGDPGS;


                //BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetails.ToList());

                //if (chkFreeze.IsChecked == true)
                //    BizAction.VitrificationMain.IsFreezed = true;
                //else
                //    BizAction.VitrificationMain.IsFreezed = false;
                BizAction.VitrificationMain.IsFreezed = false;
                if (IsEmbryoDonation == true)
                    BizAction.VitrificationMain.IsOnlyVitrification = true;

                BizAction.VitrificationMain.IsCryoWOThaw = true;    // Flag use to save only Vitrification\Cryo details from IVF/ICSI/IVF-ICSI/FE ICSI cycles which will be thaw under Freeze All Oocyte/Freeze-Thaw Transfer cycles    // For IVF ADM Changes


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
            else
            {
                foreach (var item in VitriDetails)
                {
                    if (item.IsSaved == false)
                        item.IsEnabled = true;
                }
                dgVitrificationDetilsGrid.ItemsSource = null;
                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
            }
            //((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).IsEnabled = true;
            //    fillDetails();
            // Save();
            //throw new NotImplementedException();
        }

        private void ToggleButton_Click_1(object sender, RoutedEventArgs e)  //for oocyte saving
        {
            if (((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).VitrificationDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Vitrification Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                fillDetails();
            }
            else if (((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).VitrificationTime == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Vitrification Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                fillDetails();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
                msgW2.Show();
            }
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
                BizAction.VitrificationMainForOocyte = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationDetailsForOocyteObj = new clsIVFDashBoard_VitrificationDetailsVO();
                BizAction.VitrificationDetailsForOocyteList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                BizAction.VitrificationMainForOocyte.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMainForOocyte.ID;
                BizAction.VitrificationMainForOocyte.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.VitrificationMainForOocyte.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.VitrificationMainForOocyte.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMainForOocyte.PlanTherapyUnitID = PlanTherapyUnitID;
                BizAction.VitrificationMainForOocyte.SaveForSingleEntry = true;
                //BizAction.VitrificationMainForOocyte.DateTime = dtOcVitrificationDate.SelectedDate.Value.Date;
                //BizAction.VitrificationMainForOocyte.DateTime = BizAction.VitrificationMainForOocyte.DateTime.Value.Add(txtOcTime.Value.Value.TimeOfDay);

                //if ((MasterListItem)cmbEmbryologistOocyte.SelectedItem != null)
                //    BizAction.VitrificationMainForOocyte.EmbryologistID = ((MasterListItem)cmbEmbryologistOocyte.SelectedItem).ID;
                //if ((MasterListItem)cmbAssistantEmbryologistOocyte.SelectedItem != null)
                //    BizAction.VitrificationMainForOocyte.AssitantEmbryologistID = ((MasterListItem)cmbAssistantEmbryologistOocyte.SelectedItem).ID;

                BizAction.VitrificationMainForOocyte.IsOnlyVitrification = false;
                BizAction.VitrificationMain.IsOnlyForEmbryoVitrification = false;

                //for (int i = 0; i < VitriDetailsForOocyte.Count; i++)
                //{
                //    VitriDetailsForOocyte[i].CanId = VitriDetailsForOocyte[i].SelectedCanId.ID;
                //    VitriDetailsForOocyte[i].StrawId = VitriDetailsForOocyte[i].SelectedStrawId.ID;
                //    VitriDetailsForOocyte[i].GobletShapeId = VitriDetailsForOocyte[i].SelectedGobletShape.ID;
                //    VitriDetailsForOocyte[i].GobletSizeId = VitriDetailsForOocyte[i].SelectedGobletSize.ID;
                //    VitriDetailsForOocyte[i].ConistorNo = VitriDetailsForOocyte[i].SelectedCanisterId.ID;
                //    VitriDetailsForOocyte[i].TankId = VitriDetailsForOocyte[i].SelectedTank.ID;
                //    VitriDetailsForOocyte[i].ColorCodeID = VitriDetailsForOocyte[i].SelectedColorSelector.ID;
                //}
                //if (PlanTherapyVO.PlannedTreatmentID == 5)
                //{
                //    BizAction.VitrificationMain.UsedByOtherCycle = true;
                //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
                //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
                //}
                //BizAction.VitrificationDetailsForOocyteList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetailsForOocyte.ToList());

                BizAction.VitrificationDetailsForOocyteObj.CanId = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedCanId.ID;
                BizAction.VitrificationDetailsForOocyteObj.StrawId = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedStrawId.ID;
                BizAction.VitrificationDetailsForOocyteObj.GobletShapeId = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedGobletShape.ID;
                BizAction.VitrificationDetailsForOocyteObj.GobletSizeId = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedGobletSize.ID;
                BizAction.VitrificationDetailsForOocyteObj.ConistorNo = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedCanisterId.ID;
                BizAction.VitrificationDetailsForOocyteObj.TankId = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedTank.ID;
                BizAction.VitrificationDetailsForOocyteObj.ColorCodeID = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem).SelectedColorSelector.ID;

                BizAction.VitrificationDetailsForOocyteObj = ((clsIVFDashBoard_VitrificationDetailsVO)dgOcyVitrificationDetilsGrid.SelectedItem);

                if (chkOcFreeze.IsChecked == true)
                    BizAction.VitrificationMainForOocyte.IsFreezed = true;
                else
                    BizAction.VitrificationMainForOocyte.IsFreezed = false;
                if (IsEmbryoDonation == true)
                    BizAction.VitrificationMainForOocyte.IsOnlyVitrification = true;


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
            else
                fillDetails();
        }

        private void cmbStageOfDevelopmentGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].StageOfDevelopmentGradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void cmbInnerCellMassGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].InnerCellMassGradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void cmbTrophoectodermGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].TrophoectodermGradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        //private void SaveThwingSingle_Click(object sender, RoutedEventArgs e)
        //{

        //}

        void msgWSaveThaw_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddUpdateThawingBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingBizActionVO();
                BizAction.ThawingDetailsList = new List<clsIVFDashBoard_ThawingDetailsVO>();
                BizAction.ThawingObj = new clsIVFDashBoard_ThawingDetailsVO();
                BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
                BizAction.Thawing.ID = ThawID;
                BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Thawing.PlanTherapyID = PlanTherapyID;
                BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
                //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
                //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

                BizAction.Thawing.UsedByOtherCycle = true;
                BizAction.Thawing.UsedTherapyID = PlanTherapyID;
                BizAction.Thawing.UsedTherapyUnitID = PlanTherapyUnitID;

                BizAction.IsThawFreezeEmbryos = true;

                BizAction.Thawing.LabPersonId = ((MasterListItem)cmbLabPerson.SelectedItem).ID;
                BizAction.ThawingObj = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem);
                BizAction.ThawingObj.DateTime = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).DateTime;
                //BizAction.ThawingObj.DateTime = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).DateTime.Value.Add((((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).DateTime).Value.TimeOfDay);
                BizAction.PostThawingPlan = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).PostThawingPlan;
                if (BizAction.ThawingObj.PostThawingPlanID == 2)
                    BizAction.ThawingObj.IsFrozenEmbryo = true;

                BizAction.IsOnlyForEmbryoThawing = true;

                //BizAction.ThawingDetailsList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetails.ToList());


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWSave =
                               new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        if (BizAction.ThawingObj.PostThawingPlanID == 4 && PlannedTreatmentID == 5)
                            msgWSave.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWSave_OnMessageBoxClosed);
                        msgWSave.Show();
                        fillThawingDetails();
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
            else
            {
                foreach (var item in ThawDetails)
                {
                    if (item.PostThawingPlanID == 0)
                        item.IsEnabled = true;
                }
                dgThawingDetilsGrid.ItemsSource = null;
                dgThawingDetilsGrid.ItemsSource = ThawDetails;
            }
            //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).IsEnabled = true;
            //fillThawingDetails();

        }

        public event RoutedEventHandler OnSaveButton_Click;

        void msgWSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
            //throw new NotImplementedException();
        }

        private void SaveThawingSingle_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateThaw())
            {
                if (((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).PostThawingPlanID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please select thawing plan.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWErrorThaw_OnMessageBoxClosed);
                    msgW1.Show();
                    foreach (var item in ThawDetails)
                    {
                        if (item.PostThawingPlanID == 0)
                            item.IsEnabled = true;
                    }
                    dgThawingDetilsGrid.ItemsSource = null;
                    dgThawingDetilsGrid.ItemsSource = ThawDetails;
                    //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).IsEnabled = true;
                    //fillThawingDetails();
                }
                else if (((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).DateTime == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please select thawing date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWErrorThaw_OnMessageBoxClosed);
                    msgW1.Show();
                    foreach (var item in ThawDetails)
                    {
                        if (item.PostThawingPlanID == 0)
                            item.IsEnabled = true;
                    }
                    dgThawingDetilsGrid.ItemsSource = null;
                    dgThawingDetilsGrid.ItemsSource = ThawDetails;

                    //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).IsEnabled = true;
                    //fillThawingDetails();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWSaveThaw =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWSaveThaw.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWSaveThaw_OnMessageBoxClosed);
                    msgWSaveThaw.Show();
                }
            }
        }

        void msgWErrorThaw_OnMessageBoxClosed(MessageBoxResult result)
        {
            foreach (var item in ThawDetails)
            {
                if (item.PostThawingPlanID == 0)
                    item.IsEnabled = true;
            }
            dgThawingDetilsGrid.ItemsSource = null;
            dgThawingDetilsGrid.ItemsSource = ThawDetails;
            //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).IsEnabled = true;
        }

        private void SaveOocyteThawingSingle_Click(object sender, RoutedEventArgs e) // save single oocyte thawing
        {
            if (ValidateThaw())
            {
                if (((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).PostThawingPlanID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please select thawing plan.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWErrorOocyteThaw_OnMessageBoxClosed);
                    msgW1.Show();
                    foreach (var item in ThawDetailsForOocyte)
                    {
                        if (item.PostThawingPlanID == 0)
                            item.IsEnabled = true;
                    }
                    dgThawingDetilsGridOocyte.ItemsSource = null;
                    dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;

                    //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).IsEnabled = true;
                    //fillThawingDetailsForOocyte();
                    //fillThawingDetailsOT();                  
                }
                else if (((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).DateTime == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please select thawing date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWErrorOocyteThaw_OnMessageBoxClosed);
                    //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).IsEnabled = true;
                    msgW1.Show();
                    foreach (var item in ThawDetailsForOocyte)
                    {
                        if (item.PostThawingPlanID == 0)
                            item.IsEnabled = true;
                    }
                    dgThawingDetilsGridOocyte.ItemsSource = null;
                    dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;
                    //fillThawingDetailsForOocyte();
                    //fillThawingDetailsOT();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWSaveOocyteThaw =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWSaveOocyteThaw.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWSaveOocyteThaw_OnMessageBoxClosed);
                    msgWSaveOocyteThaw.Show();
                }
            }
        }

        void msgWErrorOocyteThaw_OnMessageBoxClosed(MessageBoxResult result)
        {
            foreach (var item in ThawDetailsForOocyte)
            {
                if (item.PostThawingPlanID == 0)
                    item.IsEnabled = true;
            }
            dgThawingDetilsGridOocyte.ItemsSource = null;
            dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;
            //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).IsEnabled = true;
        }

        void msgWSaveOocyteThaw_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddUpdateThawingBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingBizActionVO();
                BizAction.ThawingDetailsList = new List<clsIVFDashBoard_ThawingDetailsVO>();
                BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
                BizAction.ThawingObj = new clsIVFDashBoard_ThawingDetailsVO();
                BizAction.Thawing.ID = ThawID;
                BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.Thawing.PlanTherapyID = PlanTherapyID;
                BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
                //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
                //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

                BizAction.Thawing.UsedByOtherCycle = true;
                BizAction.Thawing.UsedTherapyID = PlanTherapyID;
                BizAction.Thawing.UsedTherapyUnitID = PlanTherapyUnitID;

                BizAction.IsThawFreezeOocytes = true;

                BizAction.Thawing.LabPersonId = ((MasterListItem)cmbLabPersonOcy.SelectedItem).ID;
                BizAction.ThawingObj = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem);
                //BizAction.ThawingDetailsForOocyteList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetailsForOocyte.ToList());

                //for (int i = 0; i < BizAction.ThawingDetailsForOocyteList.Count; i++)
                //{
                //    foreach (var item in VitriDetailsForOocyte.OrderByDescending(x => x.ID))
                //    {
                //        BizAction.ThawingDetailsForOocyteList[i].TransferDay = item.TransferDay;
                //    }
                //}

                BizAction.ThawingObj.DateTime = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).DateTime;
                //TimeSpan tm = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).DateTime.Value.TimeOfDay;
                //BizAction.ThawingObj.DateTime = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).DateTime.Value.Add(tm);
                BizAction.PostThawingPlan = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).PostThawingPlan;
                BizAction.ThawingObj.GradeID = ((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).SelectedOocyteGrade.ID;

                BizAction.IsOnlyForEmbryoThawing = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        //fillThawingDetailsForOocyte();
                        fillThawingDetailsOT();
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
                //throw new NotImplementedException();
            }
            else
            {
                foreach (var item in ThawDetailsForOocyte)
                {
                    if (item.PostThawingPlanID == 0)
                        item.IsEnabled = true;
                }
                dgThawingDetilsGridOocyte.ItemsSource = null;
                dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;
            }

            //((clsIVFDashBoard_ThawingDetailsVO)dgThawingDetilsGridOocyte.SelectedItem).IsEnabled = true;
            //    fillThawingDetailsOT();
            //fillThawingDetailsForOocyte();
        }

        private void cmbGrade_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbGrade_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbOocyteGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdCryoBankEmbryo_Click(object sender, RoutedEventArgs e)
        {
            frmCryoBankForThawing win = new frmCryoBankForThawing();
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.IsForEmbryo = true;     // flag use to show data for Embryo/Oocyte Bank
            win.OnAddButton_Click += new RoutedEventHandler(win_OnAddEmbryoButton_Click);
            win.Closed += new EventHandler(win_Closed);
            win.Show();
        }

        void win_Closed(object sender, EventArgs e)
        {
            //SaveThawingEmbryoUsingCryoList();           
            //fillThawingDetails();
            foreach (var item in ThawDetails)
            {
                if (item.PostThawingPlanID == 0)
                    item.IsEnabled = true;
            }
            dgThawingDetilsGrid.ItemsSource = null;
            dgThawingDetilsGrid.ItemsSource = ThawDetails;
        }

        void win_OnAddEmbryoButton_Click(object sender, RoutedEventArgs e)
        {
            // Save Cryo Items for Thaw from Thawing Tab Only for FE ICSI Cycle

            if (((frmCryoBankForThawing)sender).DialogResult == true)
            {
                //VitriDetailsOocytes = ((frmCryoBankForThawing)sender).SelectedOocytes.DeepCopy();
                ThawDetails = new ObservableCollection<clsIVFDashBoard_ThawingDetailsVO>();

                foreach (clsIVFDashBoard_VitrificationDetailsVO itemCryo in ((frmCryoBankForThawing)sender).SelectedOocytes)
                {
                    clsIVFDashBoard_ThawingDetailsVO itemThaw = new clsIVFDashBoard_ThawingDetailsVO();

                    itemThaw.EmbNumber = itemCryo.EmbNumber;
                    itemThaw.EmbSerialNumber = itemCryo.EmbSerialNumber;
                    itemThaw.CellStageID = itemCryo.CellStageID;
                    itemThaw.GradeID = itemCryo.GradeID;
                    itemThaw.TransferDayNo = itemCryo.TransferDayNo;
                    itemThaw.CellStage = itemCryo.CellStage;
                    itemThaw.StageOfDevelopmentGradeID = itemCryo.StageofDevelopmentGrade;
                    itemThaw.InnerCellMassGradeID = itemCryo.InnerCellMassGrade;
                    itemThaw.TrophoectodermGradeID = itemCryo.TrophoectodermGrade;
                    itemThaw.CycleCode = itemCryo.CycleCode;
                    itemThaw.IsFreshEmbryoPGDPGS = itemCryo.IsFreshEmbryoPGDPGS;
                    itemThaw.IsFrozenEmbryoPGDPGS = itemCryo.IsFrozenEmbryoPGDPGS;
                    //itemThaw.EmbStatus = itemCryo.EmbStatus;
                    //itemThaw.Comments = itemCryo.Comments;
                    ThawDetails.Add(itemThaw);
                }

                //if (ValidateOV())   // For IVF ADM Changes
                //{
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW3.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW3_OnMessageBoxClosed);
                msgW3.Show();
                //}
            }
        }

        void msgW3_OnMessageBoxClosed(MessageBoxResult result)  // For IVF ADM Changes
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveThawingEmbryoUsingCryoList();
            }
        }

        private void SaveThawingEmbryoUsingCryoList()    // For IVF ADM Changes
        {
            //To Save Thaw Details in FE ICSI & Freeze/Thaw Transfer Cycle w.o. saving Cryo/Vitrification details in the same cycle
            clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO();

            BizAction.ThawingDetailsList = new List<clsIVFDashBoard_ThawingDetailsVO>();
            BizAction.Thawing = new clsIVFDashBoard_ThawingVO();

            BizAction.Thawing.ID = ThawID;               //ThawIDOocytes
            BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Thawing.PlanTherapyID = PlanTherapyID;
            BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
            //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
            //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

            BizAction.Thawing.UsedByOtherCycle = true;
            BizAction.Thawing.UsedTherapyID = PlanTherapyID;
            BizAction.Thawing.UsedTherapyUnitID = PlanTherapyUnitID;

            BizAction.Thawing.LabPersonId = ((MasterListItem)cmbLabPerson.SelectedItem).ID;
            BizAction.ThawingDetailsList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetails.ToList());

            //BizAction.IsThawFreezeOocytes = true;           //Flag use while save Freeze Oocytes under FE ICSI Cycle for Thaw 

            //if (chkFreezeOT.IsChecked == true)              // Flag use to Freeze Thaw Oocyte Details
            //    BizAction.Thawing.IsFreezed = true;         // Flag use to Freeze Thaw Oocyte Details
            //else
            //    BizAction.Thawing.IsFreezed = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillThawingDetails();   // For IVF ADM Changes
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

        private void CmdCryoBank_Click(object sender, RoutedEventArgs e)
        {
            frmCryoBankForThawing win = new frmCryoBankForThawing();
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.PlanTherapyID = PlanTherapyID;
            win.PlanTherapyUnitID = PlanTherapyUnitID;
            win.OnAddButton_Click += new RoutedEventHandler(win_OnAddButton_Click);
            win.Closed += new EventHandler(winOocyte_Closed);
            win.Show();
        }

        void winOocyte_Closed(object sender, EventArgs e)
        {
            // SaveThawingOTUsingCryoList();
           // fillThawingDetailsOT();
            foreach (var item in ThawDetailsForOocyte)
            {
                if (item.PostThawingPlanID == 0)
                    item.IsEnabled = true;
            }
            dgThawingDetilsGridOocyte.ItemsSource = null;
            dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;
        }

        void win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Save Cryo Items for Thaw from Thawing Tab Only for FE ICSI Cycle

            if (((frmCryoBankForThawing)sender).DialogResult == true)
            {
                //VitriDetailsOocytes = ((frmCryoBankForThawing)sender).SelectedOocytes.DeepCopy();
                ThawDetailsOocytes = new ObservableCollection<clsIVFDashBoard_ThawingDetailsVO>();

                foreach (clsIVFDashBoard_VitrificationDetailsVO itemCryo in ((frmCryoBankForThawing)sender).SelectedOocytes)
                {
                    clsIVFDashBoard_ThawingDetailsVO itemThaw = new clsIVFDashBoard_ThawingDetailsVO();

                    itemThaw.EmbNumber = itemCryo.EmbNumber;
                    itemThaw.EmbSerialNumber = itemCryo.EmbSerialNumber;
                    itemThaw.CellStageID = itemCryo.CellStageID;
                    itemThaw.GradeID = itemCryo.GradeID;
                    itemThaw.TransferDayNo = itemCryo.TransferDayNo;
                    itemThaw.CycleCode = itemCryo.CycleCode;
                    itemThaw.IsFreshEmbryoPGDPGS = itemCryo.IsFreshEmbryoPGDPGS;
                    itemThaw.IsFrozenEmbryoPGDPGS = itemCryo.IsFrozenEmbryoPGDPGS;
                    ThawDetailsOocytes.Add(itemThaw);
                }

                //if (ValidateOV())   // For IVF ADM Changes
                //{
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW4.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW4_OnMessageBoxClosed);
                msgW4.Show();
                //}
            }
        }

        void msgW4_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveThawingOTUsingCryoList();
            }

        }

        private void SaveThawingOTUsingCryoList()    // For IVF ADM Changes
        {
            //To Save Thaw Details in FE ICSI & Freeze/Thaw Transfer Cycle w.o. saving Cryo/Vitrification details in the same cycle
            clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO BizAction = new clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO();

            BizAction.ThawingDetailsList = new List<clsIVFDashBoard_ThawingDetailsVO>();
            BizAction.Thawing = new clsIVFDashBoard_ThawingVO();

            BizAction.Thawing.ID = ThawIDOocytes;               //ThawID
            BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Thawing.PlanTherapyID = PlanTherapyID;
            BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
            //BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
            //BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);

            BizAction.Thawing.UsedByOtherCycle = true;
            BizAction.Thawing.UsedTherapyID = PlanTherapyID;
            BizAction.Thawing.UsedTherapyUnitID = PlanTherapyUnitID;

            BizAction.IsFreezeOocytes = true;

            BizAction.Thawing.LabPersonId = ((MasterListItem)cmbLabPersonOcy.SelectedItem).ID;
            BizAction.ThawingDetailsList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetailsOocytes.ToList());

            BizAction.IsThawFreezeOocytes = true;           //Flag use while save Freeze Oocytes under FE ICSI Cycle for Thaw 

            //if (chkFreezeOT.IsChecked == true)              // Flag use to Freeze Thaw Oocyte Details
            //    BizAction.Thawing.IsFreezed = true;         // Flag use to Freeze Thaw Oocyte Details
            //else
            //    BizAction.Thawing.IsFreezed = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillThawingDetailsOT();   // For IVF ADM Changes
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

        long ThawIDOocytes = 0;     // For IVF ADM Changes
        private void fillThawingDetailsOT()     // For IVF ADM Changes
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetThawingBizActionVO BizAction = new clsIVFDashboard_GetThawingBizActionVO();
                BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
                BizAction.Thawing.PlanTherapyID = PlanTherapyID;
                BizAction.Thawing.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        ThawDetailsOocytes.Clear();     // For IVF ADM Changes
                        ThawDetailsForOocyte.Clear();
                        //this.DataContext = (clsIVFDashboard_GetThawingBizActionVO)arg.Result;     // For IVF ADM Changes

                        ThawIDOocytes = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.ID;     // For IVF ADM Changes

                        //dtthawingDate.SelectedDate = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        //txtthawingTime.Value = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        cmbLabPersonOcy.SelectedValue = (long)((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.LabPersonId;   // For IVF ADM Changes

                        //if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.IsETFreezed == true)
                        //{
                        //    cmdSaveOT.IsEnabled = false;    // For IVF ADM Changes
                        //}
                        //else
                        //{
                        //    cmdSaveOT.IsEnabled = true;     // For IVF ADM Changes
                        //}

                        //if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.IsFreezed == true)  // For IVF ADM Changes
                        //{
                        //    chkFreezeOT.IsChecked = true;
                        //}
                        //else
                        //{
                        //    chkFreezeOT.IsChecked = false;
                        //}

                        //if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.IsFreezed == true)
                        //{
                        //    cmdSaveOT.IsEnabled = false;         // For IVF ADM Changes
                        //    //ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        //}
                        //else
                        //{
                        //    cmdSaveOT.IsEnabled = true;     // For IVF ADM Changes
                        //}


                        for (int i = 0; i < ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList.Count; i++)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem CS = new MasterListItem();

                            MasterListItem OPlan = new MasterListItem();

                            Grade = CleavageGradeDay3;
                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID > 0)
                                ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].IsEnabled = false;
                            else
                                ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].IsEnabled = true;

                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeList = Grade;
                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageList = CellStage;
                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanList = OocytePlan;

                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID != 0)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID != 0)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID));
                            }

                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID != null)
                            {
                                OPlan = OocytePlan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID));
                            }


                            //oocyte grade
                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].OocyteGradeList = mlSourceOfSperm;
                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID > 0)
                            {
                                ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
                            }
                            //

                            if (Gr != null)
                            {
                                if (Gr.ID > 0)
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedGrade = Gr;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade = Gr.Description;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID = Gr.ID;
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].Grade = "";
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID = Gr.ID;
                                }
                            }


                            if (CS != null)
                            {
                                if (CS.ID > 0)
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedCellStage = CS;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = CS.Description;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = "";
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;
                                }
                            }


                            if (OPlan != null)
                            {
                                if (OPlan.ID > 0)
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].SelectedPostThawingPlan = OPlan;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlan = OPlan.Description;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID = OPlan.ID;
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlan = "";
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].PostThawingPlanID = OPlan.ID;
                                }
                            }

                            ThawDetailsForOocyte.Add(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i]);


                        }
                        dgThawingDetilsGridOocyte.ItemsSource = null;
                        dgThawingDetilsGridOocyte.ItemsSource = ThawDetailsForOocyte;

                        //fillDetailsOV();          // For IVF ADM Changes
                        fillDetailsOVForThawTab();  // For IVF ADM Changes

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


        private void fillDetailsOVForThawTab()      // For IVF ADM Changes
        {
            try
            {
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }

                BizAction.VitrificationMain.IsOnlyVitrification = false;

                BizAction.IsFreezeOocytes = true;       //Flag set to retrive Freeze Oocytes only under Freeze All Oocytes Cycle 

                BizAction.IsForThawTab = true;          //Flag set while retrive Cryo Oocytes under FE ICSI cycle from Freeze All Oocytes Cycle 

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetailsOocytes.Clear();        // For IVF ADM Changes
                        //this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;       // For IVF ADM Changes



                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdNewOcy.IsEnabled = false;         // For IVF ADM Changes
                            //ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        }
                        //else
                        //{
                        //    ThawingOocytes.IsEnabled = true;    // For IVF ADM Changes
                        //}

                        chkOcFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;      // For IVF ADM Changes
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtOcVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtOcTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtOcVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;    // For IVF ADM Changes
                        }


                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            MasterListItem Gr = new MasterListItem();
                            MasterListItem PT = new MasterListItem();
                            MasterListItem CS = new MasterListItem();

                            MasterListItem TC = new MasterListItem();       // For IVF ADM Changes

                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID != null)
                            //{
                            //    PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID));
                            //}
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }

                            //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID != null)
                            //{
                            //    CC = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}

                            //oocyte grade
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].OocyteGradeList = mlSourceOfSperm;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedOocyteGrade = mlSourceOfSperm.FirstOrDefault(p => p.ID == 0);
                            }
                            //

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList.DeepCopy();
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw.DeepCopy();   // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize.DeepCopy();
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister.DeepCopy();     // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank.DeepCopy();       // For IVF ADM Changes
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            //((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeList = CryoCodeList.DeepCopy();
                            //if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID) > 0)
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CryoCodeID));
                            //}
                            //else
                            //{
                            //    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCryoCode = CryoCodeList.FirstOrDefault(p => p.ID == 0);
                            //}



                            VitriDetailsOocytes.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);     // For IVF ADM Changes

                        }

                        //if (VitrificationOocytes != null)   // For IVF ADM Changes
                        //{
                        //    if (VitrificationOocytes.IsSelected == true)    // For IVF ADM Changes
                        //    {
                        //        dgVitrificationDetilsGridOV.ItemsSource = null;
                        //        dgVitrificationDetilsGridOV.ItemsSource = VitriDetailsOocytes;    // For IVF ADM Changes
                        //    }
                        //}

                        if (ThawingOocyte != null)     // For IVF ADM Changes
                        {
                            if (ThawingOocyte.IsSelected == true)
                            {
                                dgThawingViDetilsGridOocyte.ItemsSource = null;                     // For IVF ADM Changes
                                dgThawingViDetilsGridOocyte.ItemsSource = VitriDetailsOocytes.DeepCopy();      // For IVF ADM Changes dgThawingViDetilsGridOT
                            }
                        }
                        ////...............................................
                        ////if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 5)
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ID == 0 && PlanTherapyVO.PlannedTreatmentID == 18)   // For IVF ADM Changes
                        //{
                        //    fillVitificationDetailsForThawTransferOT();     // For IVF ADM Changes
                        //}
                        ////................................................
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

        private void cmbReStraw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReGobletShape_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReGobletSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReCanisterId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReTankId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbReOocyteGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdRefeezeNewOcy_Click(object sender, RoutedEventArgs e)
        {
            if (RefreezeOocyteValidate())
            {
                //if (blOocyteSave)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please thaw all oocyte", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgW1.Show();
                //    fillDetails();
                //}
                //else
                //{
                MessageBoxControl.MessageBoxChildWindow msgWRefreezeOocyte =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWRefreezeOocyte.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWRefreezeOocyte_OnMessageBoxClosed);
                msgWRefreezeOocyte.Show();
                //}
            }
        }

        void msgWRefreezeOocyte_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveRefreezeOV();
        }


        private void SaveRefreezeOV()   // For IVF ADM Changes
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationRefeezeMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
            BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.VitrificationMain.DateTime = dtRefeezeOcVitrificationDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtRefeezeOcTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.ExpiryDate = dtOcReExpiryDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.ExpiryDate = BizAction.VitrificationMain.ExpiryDate.Value.Add(txtOcReExpiryTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            BizAction.VitrificationMain.IsRefeeze = true;
            for (int i = 0; i < RefeezeVitriDetailsOocytes.Count; i++)
            {
                RefeezeVitriDetailsOocytes[i].CanId = RefeezeVitriDetailsOocytes[i].SelectedCanId.ID;
                RefeezeVitriDetailsOocytes[i].StrawId = RefeezeVitriDetailsOocytes[i].SelectedStrawId.ID;
                RefeezeVitriDetailsOocytes[i].GobletShapeId = RefeezeVitriDetailsOocytes[i].SelectedGobletShape.ID;
                RefeezeVitriDetailsOocytes[i].GobletSizeId = RefeezeVitriDetailsOocytes[i].SelectedGobletSize.ID;
                RefeezeVitriDetailsOocytes[i].ConistorNo = RefeezeVitriDetailsOocytes[i].SelectedCanisterId.ID;
                RefeezeVitriDetailsOocytes[i].TankId = RefeezeVitriDetailsOocytes[i].SelectedTank.ID;
                RefeezeVitriDetailsOocytes[i].ColorCodeID = RefeezeVitriDetailsOocytes[i].SelectedColorSelector.ID;

                RefeezeVitriDetailsOocytes[i].GradeID = RefeezeVitriDetailsOocytes[i].SelectedOocyteGrade.ID;  //oocytegarde into grade
            }
            //if (PlanTherapyVO.PlannedTreatmentID == 5)
            //{
            //    BizAction.VitrificationMain.UsedByOtherCycle = true;
            //    BizAction.VitrificationMain.UsedTherapyID = PlanTherapyID;
            //    BizAction.VitrificationMain.UsedTherapyUnitID = PlanTherapyUnitID;
            //}
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)RefeezeVitriDetailsOocytes.ToList());
            if (chkOcReFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;
            if (IsEmbryoDonation == true)
                BizAction.VitrificationMain.IsOnlyVitrification = true;

            BizAction.IsFreezeOocytes = true;   //Flag set while save Freeze Oocytes under Freeze All Oocytes Cycle for freeze & Thaw   //For IVF ADM changes


            BizAction.VitrificationMain.IsCryoWOThaw = true;  // Flag use to save only Vitrification\Cryo details from IVF/ICSI/IVF-ICSI/FE ICSI cycles which will be thaw under Freeze All Oocyte/Freeze-Thaw Transfer cycles    //For IVF ADM changes

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillDetailsOV();    // For IVF ADM Changes
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


        private bool RefreezeOocyteValidate()
        {
            bool result = true;
            DateTime? VitriDate = null;
            DateTime? ExpiryDate = null;

            if (dtRefeezeOcVitrificationDate.SelectedDate != null && txtRefeezeOcTime.Value != null)
            {
                VitriDate = dtRefeezeOcVitrificationDate.SelectedDate.Value.Date;
                VitriDate = VitriDate.Value.Add(txtRefeezeOcTime.Value.Value.TimeOfDay);
            }

            if (dtOcReExpiryDate.SelectedDate != null && txtOcReExpiryTime.Value != null)
            {
                ExpiryDate = dtOcReExpiryDate.SelectedDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(txtOcReExpiryTime.Value.Value.TimeOfDay);
            }

            if (dtRefeezeOcVitrificationDate.SelectedDate == null)
            {
                dtRefeezeOcVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtRefeezeOcVitrificationDate.RaiseValidationError();
                dtRefeezeOcVitrificationDate.Focus();
                result = false;
            }
            else if (txtRefeezeOcTime.Value == null)
            {
                txtRefeezeOcTime.ClearValidationError();
                txtRefeezeOcTime.SetValidation("Please Select Vitrification Time");
                txtRefeezeOcTime.RaiseValidationError();
                txtRefeezeOcTime.Focus();
                result = false;
            }
            else if (dtOcReExpiryDate.SelectedDate == null)
            {
                txtRefeezeOcTime.ClearValidationError();
                dtOcReExpiryDate.SetValidation("Please Select Expiry Date");
                dtOcReExpiryDate.RaiseValidationError();
                dtOcReExpiryDate.Focus();
                result = false;
            }
            else if (txtOcReExpiryTime.Value == null)
            {
                dtOcReExpiryDate.ClearValidationError();
                txtOcReExpiryTime.SetValidation("Please Select Expiry Time");
                txtOcReExpiryTime.RaiseValidationError();
                txtOcReExpiryTime.Focus();
                result = false;
            }
            else if (ExpiryDate < VitriDate)
            {
                txtOcReExpiryTime.ClearValidationError();
                dtOcReExpiryDate.SetValidation("Expiry Date Cannot Be Less Than Start Date  ");
                dtOcReExpiryDate.RaiseValidationError();
                dtOcReExpiryDate.Focus();
                dtOcReExpiryDate.Text = " ";
                dtOcReExpiryDate.Focus();
                result = false;
            }
            else if (RefeezeVitriDetailsOocytes.Count <= 0)
            {
                dtOcReExpiryDate.ClearValidationError();
                txtOcReExpiryTime.ClearValidationError();
                dtRefeezeOcVitrificationDate.ClearValidationError();
                txtRefeezeOcTime.ClearValidationError();
                dtRefeezeOcPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
            }
            else if (RefeezeVitriDetailsOocytes.Count > 0)
            {
                var MaxDate = (from d in RefeezeVitriDetailsOocytes select d.TransferDate).Max();

                if (VitriDate < MaxDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Date can not be less than Plan Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                }
            }
            else
            {
                dtOcReExpiryDate.ClearValidationError();
                txtOcReExpiryTime.ClearValidationError();
                dtRefeezeOcVitrificationDate.ClearValidationError();
                txtRefeezeOcTime.ClearValidationError();
                dtRefeezeOcPickUpDate.ClearValidationError();
                result = true;
            }
            return result;
        }

        private void dtOcVitrificationDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtRefeezeOcVitrificationDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtOcVitrificationDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime? ExpiryDate = null;
            if (txtOcTime.Value != null)
            {
                ExpiryDate = dtOcVitrificationDate.SelectedDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(txtOcTime.Value.Value.TimeOfDay);
                dtOcExpiryDate.SelectedDate = ExpiryDate.Value.AddYears(1);
                txtOcExpiryTime.Value = ExpiryDate.Value.AddYears(1);
            }
            else
            {
                dtOcExpiryDate.SelectedDate = dtOcVitrificationDate.SelectedDate.Value.AddYears(1);
            }
        }

        private void dtRefeezeOcVitrificationDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime? ExpiryDate = null;
            if (txtRefeezeOcTime.Value != null)
            {
                ExpiryDate = dtRefeezeOcVitrificationDate.SelectedDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(txtRefeezeOcTime.Value.Value.TimeOfDay);
                dtOcReExpiryDate.SelectedDate = ExpiryDate.Value.AddYears(1);
                txtOcReExpiryTime.Value = ExpiryDate.Value.AddYears(1);
            }
            else
            {
                dtOcReExpiryDate.SelectedDate = dtRefeezeOcVitrificationDate.SelectedDate.Value.AddYears(1);
            }
        }

        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime? ExpiryDate = null;
            if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime != null)
            {
                ExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime.Value.TimeOfDay);
                ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate = ExpiryDate.Value.AddYears(1);
                ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime = ExpiryDate.Value.AddYears(1);
            }
            else
            {
                ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate.Value.AddYears(1);
            }

            //if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate != null && ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime.Value != null)
            //{
            //    VitriDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationDate.Value.Date;
            //    VitriDate = VitriDate.Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).VitrificationTime.Value.TimeOfDay);
            //}

            //if (((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate != null && ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime.Value != null)
            //{
            //    ExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryDate.Value.Date;
            //    ExpiryDate = ExpiryDate.Value.Add(((clsIVFDashBoard_VitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).ExpiryTime.Value.TimeOfDay);
            //}
        }
    }
}
