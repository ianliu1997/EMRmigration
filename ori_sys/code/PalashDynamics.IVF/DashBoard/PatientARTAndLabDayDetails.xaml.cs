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
using PalashDynamics.IVF.PatientList;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Animations;
//using DataDrivenApplication;
using OPDModule;
using PalashDynamics.ValueObjects;
using DataDrivenApplication.Forms;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Markup;
using System.Windows.Data;
using System.Text;
using PalashDynamics.IVF.TherpyExecution;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Collections;
//using EMR;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Converters;
using System.Windows.Controls.Primitives;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class PatientARTAndLabDayDetails : UserControl, IInitiateCIMS, IInitiateIVFDashBoard
    {
        #region Variables and properties
        bool IsCancel = true;
        bool IsClosed = false;
        public bool IsExpendedWindow { get; set; }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        public Boolean a = true;
        private bool IsPatientExist = false;
        private SwivelAnimation objAnimation;
        WaitIndicator wait = new WaitIndicator();
        PagedCollectionView Execollection;
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        string username = string.Empty;
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        private List<clsCoupleVO> _AllCoupleDetails = new List<clsCoupleVO>();
        public List<clsCoupleVO> AllCoupleDetails
        {
            get
            {
                return _AllCoupleDetails;
            }
            set
            {
                _AllCoupleDetails = value;
            }
        }

        private clsIVFDashboard_GetTherapyListBizActionVO _TherapyDetails = new clsIVFDashboard_GetTherapyListBizActionVO();
        public clsIVFDashboard_GetTherapyListBizActionVO SelectedTherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private List<clsPlanTherapyVO> _TherapyDetailsList = new List<clsPlanTherapyVO>();
        public List<clsPlanTherapyVO> TherapyDetailsList
        {
            get
            {
                return _TherapyDetailsList;
            }
            set
            {
                _TherapyDetailsList = value;
            }
        }

        frmGraphicalRepresentationofLabDays grpRep = null;
        //FrmIUI frmIUI = null;
        frmIUIComeSemenWash frmIUI = null;
        frmBirthDetails Birth = null;
        frmTransfer ET = null;
        frmOPU FrOPU = null;
        frmCryoPreservation frmCryo = null;
        frmOnlyET onlyET = null;
        frmDocumentforDashTherapy Doc = null;
        FrmSurrogateOutcome SurrogacyOutcome = null;
        string billno;
        long billID = 0;
        long billUnitID = 0;
        string msgTitle = "";
        string msgText = "";
        long ChildID;
        DateTime? PregnancyAchivedDate;
        bool? IsPregnancyAchived;
        long FollicularID = 0;
        long FollicularUnitID = 0;

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

        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //added by neena
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
        //

        #endregion

        #region fillCombox
        private void fillProtocolType()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFProtocolType;
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
                    cmbProtocol.ItemsSource = null;
                    cmbProtocol.ItemsSource = objList;
                    cmbProtocol.SelectedValue = (long)0;
                    cmbAdsercchProtocol.ItemsSource = null;
                    cmbAdsercchProtocol.ItemsSource = objList;
                    cmbAdsercchProtocol.SelectedValue = (long)0;

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                //FillBabyType();
                fillMainIndication();

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMainIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_MainIndication;
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
                    cmbMainIndication.ItemsSource = null;
                    cmbMainIndication.ItemsSource = objList;
                    cmbMainIndication.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillMainSubIndication();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMainSubIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_MainSubIndication;
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
                    cmbSubMainIndication.ItemsSource = null;
                    cmbSubMainIndication.ItemsSource = objList;
                    cmbSubMainIndication.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillSpermCollection();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        List<MasterListItem> objSpermCollList = null;
        List<MasterListItem> objList = null;
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
                    objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbSpermCollection.ItemsSource = null;
                    cmbSpermCollection.ItemsSource = objList;
                    cmbSpermCollection.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillPlannedTreatment();
                //fillFinalPlannedTreatment();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillFinalPlannedTreatment()
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
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbFinalPlanofTreatment.ItemsSource = null;
                    cmbFinalPlanofTreatment.ItemsSource = objList;
                    cmbFinalPlanofTreatment.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                //fillExternalStimulation();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillPlannedTreatment()
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
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbPlannedTreatment.ItemsSource = null;
                    cmbPlannedTreatment.ItemsSource = objList;
                    cmbPlannedTreatment.SelectedValue = (long)0;

                    cmbAdsercchPlannedTreatment.ItemsSource = null;
                    cmbAdsercchPlannedTreatment.ItemsSource = objList;
                    cmbAdsercchPlannedTreatment.SelectedValue = (long)0;


                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                //    fillExternalStimulation();
                fillExternalStimulation();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillExternalStimulation()
        {
            List<MasterListItem> Items = new List<MasterListItem>();

            MasterListItem Item = new MasterListItem();
            Item.ID = (int)0;
            Item.Description = "--Select--";
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.Yes;
            Item.Description = ExternalStimulation.Yes.ToString();
            Items.Add(Item);

            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.No;
            Item.Description = ExternalStimulation.No.ToString();
            Items.Add(Item);

            cmbSimulation.ItemsSource = Items;
            cmbSimulation.SelectedValue = (long)0;

            fillSpermSource();
        }

        private void fillDoctor()
        {
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

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList;
                    cmbPhysician.SelectedValue = (long)0;

                    cmbAdsercchDoctor.ItemsSource = null;
                    cmbAdsercchDoctor.ItemsSource = objList;
                    cmbAdsercchDoctor.SelectedValue = (long)0;


                }
                FillAnesthetist();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillPatientName()
        {
            //clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            ////BizAction.IsActive = true;
            //BizAction.TableName = "T_Registration";
            //BizAction.ColumnName = "Country";
            //BizAction.IsDecode = true;

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        txtPatientName.ItemsSource = null;
            //        txtPatientName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

            //        fillCoupleDetails();
            //    }
            //};
            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();
            GetSearchkeywordForPatientBizActionVO BizAction = new GetSearchkeywordForPatientBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPatientName.ItemsSource = null;
                    txtPatientName.ItemsSource = ((GetSearchkeywordForPatientBizActionVO)e.Result).MasterList.DeepCopyd();
                }

                if (EMRLink)
                {
                    fillCoupleDetails();
                }
                else
                    wait.Close();

                //  FillMrno();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        private void FillBabyType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BabyType;
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
                    cmbBabyType.ItemsSource = null;
                    cmbBabyType.ItemsSource = objList;
                    cmbBabyType.SelectedValue = (long)0;
                }
                fillMainIndication();
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //added by neena

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


        private void fillSpermSource()
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
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbSourceOfSperm.ItemsSource = null;
                    cmbSourceOfSperm.ItemsSource = objList;
                    cmbSourceOfSperm.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillDoctor();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        //
        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            IsPatientExist = true;
        }

        //added by neena
        bool EMRLink = false;
        long EMRPlanTherapyId;
        long EMRPlanTherapyUnitId;
        long EMRPatientID;
        long EMRPatientUnitID;
        long GridIndex;
        public void InitiateDashBoard(long TherapyId, long TherapyUnitId, long PatientId, long PatientUnitId, long GdIndex)
        {
            EMRLink = true;
            EMRPlanTherapyId = TherapyId;
            EMRPlanTherapyUnitId = TherapyUnitId;
            EMRPatientID = PatientId;
            EMRPatientUnitID = PatientUnitId;
            GridIndex = GdIndex;
            IsPatientExist = true;
        }

        //

        #endregion

        #region Constructor
        public PatientARTAndLabDayDetails()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(ARTFrontPanel, ARTBackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            //SelectedTherapyDetails = new clsGetTherapyListBizActionVO();
            //SelectedTherapyDetails.TherapyDetailsList = new List<clsPlanTherapyVO>();
            App.TherapyExecutionNew = this;

            DataList1 = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
            DataListOvarian = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataListStimulation = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataListTrigger = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataListOtherDrug = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataListLutealPhase = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataListInvestigation = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListConsent = new PagedSortableCollectionView<clsPatientConsentVO>();
            DataListPageSize = 15;
        }
        #endregion
        #region Paging
        public PagedSortableCollectionView<clsEMRAddDiagnosisVO> DataList1 { get; private set; }
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListOvarian { get; private set; }
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListStimulation { get; private set; }
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListTrigger { get; private set; }
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListOtherDrug { get; private set; }
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListLutealPhase { get; private set; }

        public PagedSortableCollectionView<clsServiceMasterVO> DataListInvestigation { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList1.PageSize;
            }
            set
            {
                if (value == DataList1.PageSize) return;
                DataList1.PageSize = value;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetPatientPreviousDiagnosis();
        }
        #endregion

        #region Fill Master Item
        //void fillPatientName()
        //{
        //    clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

        //    BizAction.TableName = "T_Registration";
        //    BizAction.ColumnName = "FirstName";
        //    BizAction.IsDecode = true;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            txtPatientName.ItemsSource = null;
        //            txtPatientName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopyd();
        //        }
        //        FillMrno();
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        void fillSearchPatientField()
        {
            GetPatientListForDashboardBizActionVO BizAction = new GetPatientListForDashboardBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPatientName.ItemsSource = null;
                    txtPatientName.ItemsSource = ((GetPatientListForDashboardBizActionVO)e.Result).MasterList.DeepCopyd();
                }
                //  FillMrno();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        void FillMrno()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "LastName";
            BizAction.IsDecode = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtMrno.ItemsSource = null;
                    txtMrno.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopyd();
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

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
                        fillOocyteSource();
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
                        fillCanID();
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
                        fillPlan();
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
                        FillCellStage();
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
                        fillOnlyVitrificationDetails();
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

        DateTime? startDate = null;

        #region Setup Page
        public void setupPage(long TherapyID, long TabId)
        {
            try
            {
                if (wait == null)
                {
                    wait = new WaitIndicator();
                    wait.Show();
                }
                clsIVFDashboard_GetTherapyListBizActionVO BizAction = new clsIVFDashboard_GetTherapyListBizActionVO();
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyID = TherapyID;
                BizAction.TabID = TabId;
                BizAction.Flag = true;

                if (SelectedTherapyDetails.TherapyDetails.UnitID == 0)
                {
                    BizAction.TherapyUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                else
                {
                    BizAction.TherapyUnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
                }

                if (CoupleDetails != null)
                {
                    BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                if (((MasterListItem)cmbAdsercchDoctor.SelectedItem).ID != 0)
                {
                    BizAction.PhysicianId = ((MasterListItem)cmbAdsercchDoctor.SelectedItem).ID;
                }
                if (((MasterListItem)cmbAdsercchProtocol.SelectedItem).ID != 0)
                {
                    BizAction.ProtocolTypeID = ((MasterListItem)cmbAdsercchProtocol.SelectedItem).ID;
                }
                if (((MasterListItem)cmbAdsercchPlannedTreatment.SelectedItem).ID != 0)
                {
                    BizAction.PlannedTreatmentID = ((MasterListItem)cmbAdsercchPlannedTreatment.SelectedItem).ID;
                }
                BizAction.rdoSuccessful = rdoSuccessful.IsChecked;
                BizAction.rdoAll = rdoAll.IsChecked;
                BizAction.rdoUnsuccessful = rdoUnsuccessful.IsChecked;
                BizAction.rdoClosed = rdoClosed.IsChecked;
                BizAction.rdoActive = rdoActive.IsChecked;
                BizAction.AttachedSurrogate = false;
                BizAction.TherapyDetails.IsIVFBillingCriteria = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria;

                //if (CoupleDetails.CoupleId == 0)
                //    BizAction.TherapyDetails.IsDonorCycle = true;
                //else
                //    BizAction.TherapyDetails.IsDonorCycle = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (TherapyID == 0)
                        {
                            this.DataContext = null;
                            dgtheropyList.ItemsSource = null;
                            dgtheropyList.ItemsSource = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyDetailsList;

                            dgtheropyList.SelectedIndex = 0;
                            TherapyDetailsList = new List<clsPlanTherapyVO>();
                            TherapyDetailsList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyDetailsList;
                            if (((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).NewButtonVisibility == true)
                            {
                                //CmdNewTheorpy.Visibility = Visibility.Visible;
                                CmdNewTheorpy.IsEnabled = true;
                            }
                            else
                            {
                                //CmdNewTheorpy.Visibility = Visibility.Collapsed;
                                CmdNewTheorpy.IsEnabled = false;
                            }
                            //if (TherapyDetailsList.Count > 0)
                            //{
                            //    if (((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).NewButtonVisibility == true)
                            //    {
                            //        //CmdNewTheorpy.Visibility = Visibility.Visible;
                            //        CmdNewTheorpy.IsEnabled = true;
                            //    }
                            //    else
                            //    {
                            //        //CmdNewTheorpy.Visibility = Visibility.Collapsed;
                            //        CmdNewTheorpy.IsEnabled = false;
                            //    }
                            //}
                            if (EMRLink)
                            {
                                EMRLink = false;
                                dgtheropyList.SelectedIndex = Convert.ToInt32(GridIndex);
                                cmdCycleDetails_Click(null, null);
                                AccItem.IsSelected = false;
                            }
                            else
                                wait.Close();
                        }
                        else
                        {
                            if (TabId == 0)
                            {
                                //this.DataContext = null;
                                SelectedTherapyDetails.TherapyExecutionList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                SelectedTherapyDetails.FollicularMonitoringList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                this.DataContext = SelectedTherapyDetails;

                                // For Birthdetails Tab
                                ChildID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).BabyTypeID;
                                PregnancyAchivedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PregnanacyConfirmDate;
                                IsPregnancyAchived = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsPregnancyAchieved;

                                //commented by neena
                                //if ((ChildID != null && ChildID != 0) && PregnancyAchivedDate != null && IsPregnancyAchived == true)
                                //{
                                //    Tabbirthdetails.Visibility = Visibility.Visible;
                                //}

                                //...............................................
                                if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == true && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == true)
                                    AccItem.Header = "Management Overview " + "Oocyte Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].OoctyDonorMrNo + ") Semen Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].SemenDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == true && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == false)
                                    AccItem.Header = "Management Overview " + "Oocyte Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].OoctyDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == false && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == true)
                                    AccItem.Header = "Management Overview " + "Semen Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].SemenDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == false && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == false)
                                    AccItem.Header = "Management Overview ";

                                //...................................................
                                BindExecutionGrid();
                                InitExecutionGrid();
                                wait.Close();
                                TriggerDateForOPU = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;
                                TriggerTimeForOPU = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;
                            }
                            else
                            {
                                if (TabId == 2)
                                {
                                    SelectedTherapyDetails.TherapyExecutionList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    BindExecutionGrid();
                                    wait.Close();                                
                                }
                                if (TabId == 3)
                                {
                                    //dgFollicularMonitoring.ItemsSource = null;
                                    //dgFollicularMonitoring.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                    SelectedTherapyDetails.FollicularMonitoringList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                    this.DataContext = SelectedTherapyDetails;
                                    username = SelectedTherapyDetails.FollicularMonitoringList[0].UserName;
                                    InitExecutionGrid();
                                    BindExecutionGrid();
                                    wait.Close();
                                }

                            }
                            if (((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList.Count > 0)
                                username = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList[0].UserName;

                            dgtheropyList.SelectedItem = TherapyDetailsList.FirstOrDefault(p => p.ID == TherapyID);

                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                            {
                                dtStartDateTrigger.IsEnabled = false;
                                txtTrigTime.IsEnabled = false;
                            }


                        }

                        wait.Close();

                    }
                    else
                    {
                        wait.Close();
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }
        #endregion

        #region load event
        //added by neena   
        long SelectedPlanTreatmentID = 0;
        long EMRProcedureID = 0;
        long EMRProcedureUnitID = 0;
        bool IsDonorCycle = false;
        bool IsSurrogacy = false;
        private void IVFServiceBilled()
        {
            try
            {
                //wait.Show();
                clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                BizAction.TherapyDetails.IsIVFBillingCriteria = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria;

                //if (CoupleDetails.CoupleId == 0)
                //    BizAction.TherapyDetails.IsDonorCycle = true;
                //else
                //    BizAction.TherapyDetails.IsDonorCycle = false;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            CmdNewTheorpy.IsEnabled = true;
                            SelectedPlanTreatmentID = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.PlannedTreatmentID;
                            // ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsDonorCycle = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.IsDonorCycle;
                            IsDonorCycle = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.IsDonorCycle;
                            EMRProcedureID = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.ID;
                            EMRProcedureUnitID = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.UnitID;
                            IsSurrogacy = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.IsSurrogate;

                            //CmdNewTheorpy.Visibility = Visibility.Visible;

                        }
                        else
                        {
                            CmdNewTheorpy.IsEnabled = false;
                            //CmdNewTheorpy.Visibility = Visibility.Collapsed;
                        }
                        setupPage(0, 0);//Fill Therpy List
                        //IVFPACServiceBilled();
                        //wait.Close();

                    }
                    else
                    {
                        wait.Close();
                    }
                    //wait.Close();


                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }

        }

        private void IVFPACServiceBilled()
        {
            try
            {
                //wait.Show();
                clsIVFDashboard_GetPACVisibleBizActionVO BizAction = new clsIVFDashboard_GetPACVisibleBizActionVO();
                BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIVFDashboard_GetPACVisibleBizActionVO)args.Result).SuccessStatus == 1)
                        {

                        }
                        // setupPage(0, 0);//Fill Therpy List
                        // wait.Close();

                    }
                    else
                    {
                        wait.Close();
                    }
                    //wait.Close();

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }

        }

        //
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbPhysician.IsEnabled = false;
            if (!IsExpendedWindow)
            {
                try
                {
                    if (IsPatientExist == false)
                    {
                        //ModuleName = "PalashDynamics";
                        //Action = "CIMS.Forms.PatientList";
                        //UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //WebClient c2 = new WebClient();
                        //c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        IsPatientExist = false;
                        //    fillPatientName();
                    }
                    else
                    {
                        // FillBabyType();
                        fillProtocolType();



                        // fillPatientName();// By BHUSHAN 
                        //fillSearchPatientField();
                        DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
                        PageSize = 15;
                        dtStartDate.IsEnabled = false;
                        dtEndDate.IsEnabled = false;
                        if (chkPill.IsChecked == true)
                        {
                            dtStartDate.IsEnabled = true;
                            dtEndDate.IsEnabled = true;
                        }
                        else
                        {
                            dtStartDate.IsEnabled = false;
                            dtEndDate.IsEnabled = false;
                        }


                    }
                    //if (DiagnosisList == null)
                    //    DiagnosisList = new ObservableCollection<clsEMRAddDiagnosisVO>();
                    //dgDiagnosisList.ItemsSource = DiagnosisList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }
        #endregion

        #region fill Diagnosis
        DateConverter dateConverter;
        public ObservableCollection<clsEMRAddDiagnosisVO> DiagnosisList { get; set; }
        public List<clsEMRAddDiagnosisVO> DiagnosisListHistory { get; set; }
        List<clsEMRAddDiagnosisVO> PatientDiagnosiDeletedList = new List<clsEMRAddDiagnosisVO>();
        List<MasterListItem> DiagnosisTypeList = new List<MasterListItem>();
        private void FillDiagnosisType()
        {
            try
            {
                wait.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DiagnosisMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        DiagnosisTypeList = new List<MasterListItem>();
                        DiagnosisTypeList.Add(new MasterListItem(0, "-- Select --"));
                        DiagnosisTypeList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    }
                    FillRouteList();
                    // GetPatientPreviousDiagnosis();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        //added by neena
        List<FrequencyMaster> FreqList = new List<FrequencyMaster>();
        List<MasterListItem> RouteList = new List<MasterListItem>();
        List<MasterListItem> InstructionList = new List<MasterListItem>();
        List<clsPatientPrescriptionDetailVO> OvarianDurg = new List<clsPatientPrescriptionDetailVO>();
        List<clsPatientPrescriptionDetailVO> StimulationDurg = new List<clsPatientPrescriptionDetailVO>();
        List<clsPatientPrescriptionDetailVO> TriggerDurg = new List<clsPatientPrescriptionDetailVO>();
        List<clsPatientPrescriptionDetailVO> OtherDurg = new List<clsPatientPrescriptionDetailVO>();
        List<clsPatientPrescriptionDetailVO> LutealDurg = new List<clsPatientPrescriptionDetailVO>();

        List<clsServiceMasterVO> InvestigationList = new List<clsServiceMasterVO>();
        List<clsServiceMasterVO> RadiologyList = new List<clsServiceMasterVO>();

        private void FillRouteList()
        {
            try
            {
                //wait.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Route;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //FillInstructionList();
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        RouteList = objList;
                        FillInstructionList();

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void FillInstructionList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_EMRInstructionMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        InstructionList = objList;
                        FillFrequencyList();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        public void FillFrequencyList()
        {
            try
            {
                clsGetEMRFrequencyBizActionVO BizAction = new clsGetEMRFrequencyBizActionVO();
                BizAction.FrequencyList = new List<PalashDynamics.ValueObjects.EMR.FrequencyMaster>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<FrequencyMaster> objList = new List<FrequencyMaster>();
                        objList.Add(new FrequencyMaster(0, "-- Select --"));
                        objList.AddRange(((clsGetEMRFrequencyBizActionVO)args.Result).FrequencyList);
                        FreqList = objList;
                        FillDrugSourse();
                        //ShowPrescriptionAndServices();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                //ShowCurrentMedication();
            }
        }

        List<MasterListItem> DrugSourseList = null;
        private void FillDrugSourse()
        {
            DrugSourseList = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            DrugSourseList.Insert(0, Default);
            EnumToList(typeof(DrugSource), DrugSourseList);
            GetPatientPreviousDiagnosis();
            //GetPatientPreviousPrescription();
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
                    cmbAnesthesist.ItemsSource = null;
                    cmbAnesthesist.ItemsSource = objList;
                    cmbAnesthesist.SelectedItem = objList[0];



                }
                wait.Close();
                // FillPatientName();

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        List<clsPatientConsentVO> SavedList = null;
        private void GetSavedConsent()
        {
            clsGetIVFSavedPackegeConsentBizActionVO BizAction = new clsGetIVFSavedPackegeConsentBizActionVO();
            try
            {
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                BizAction.PlanTherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.PlanTherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    //if (arg.Error == null && arg.Result != null)
                    //{
                    //    if (((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null && ((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails.Count > 0)
                    //    {
                    //        clsGetIVFSavedPackegeConsentBizActionVO result = arg.Result as clsGetIVFSavedPackegeConsentBizActionVO;
                    //        //DataList.TotalItemCount = result.TotalRows;

                    //        if (result.ConsentMatserDetails != null && result.ConsentMatserDetails.Count > 0)
                    //        {
                    //            var item = ((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails.Where(x => x.IsConsentCheck == false);
                    //            if (item.ToList().Count > 0)
                    //            {
                    //                IsConsentCheck = true;
                    //                chkConsentCheck.IsChecked = false;
                    //            }

                    //        }


                    //    }
                    //    else
                    //    {
                    //        IsConsentCheck = true;
                    //        chkConsentCheck.IsChecked = false;
                    //    }

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetIVFSavedPackegeConsentBizActionVO result = arg.Result as clsGetIVFSavedPackegeConsentBizActionVO;
                            //DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null && result.ConsentMatserDetails.Count > 0)
                            {
                                SavedList = new List<clsPatientConsentVO>();
                                SavedList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    SavedList.Add(item);
                                }

                            }

                        }
                        GetConsent();

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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetConsent()
        {
            clsGetIVFPackegeConsentBizActionVO BizAction = new clsGetIVFPackegeConsentBizActionVO();
            try
            {
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                BizAction.PlanTherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.PlanTherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }

                //BizAction.PatientID = PatientID;
                //BizAction.PatientUnitID = PatientUnitID;
                //BizAction.PlanTherapyId = PlanTherapyID;
                //BizAction.PlanTherapyUnitId = PlanTherapyUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetIVFPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetIVFPackegeConsentBizActionVO result = arg.Result as clsGetIVFPackegeConsentBizActionVO;

                            if (result.ConsentMatserDetails != null)
                            {
                                DataListConsent.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    DataListConsent.Add(item);
                                }

                                if (SavedList != null && SavedList.Count > 0)
                                {
                                    (from a1 in DataListConsent
                                     join a2 in SavedList on a1.ConsentID equals a2.ConsentID
                                     select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => x.A1.IsConsentCheck = x.A2.IsConsentCheck);
                                }

                                var item1 = DataListConsent.Where(x => x.IsConsentCheck == false);
                                if (item1.ToList().Count > 0)
                                {
                                    IsConsentCheck = true;
                                    chkConsentCheck.IsChecked = false;
                                }

                            }
                        }

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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetPatientPreviousPrescription()
        {

            try
            {
                dgOvarianDrugList.ItemsSource = null;
                dgStimulationDrugList.ItemsSource = null;
                dgTriggerDrugList.ItemsSource = null;
                dgOtherDrugList.ItemsSource = null;
                dgLutealPhaseDrugList.ItemsSource = null;
                //wait.Show();
                clsGetIVFDashboardPatientPrescriptionDataBizActionVO BizAction = new clsGetIVFDashboardPatientPrescriptionDataBizActionVO();
                //if (EMRLink)
                //{
                //    BizAction.PatientID = EMRPatientID;
                //    BizAction.PatientUnitID = EMRPatientUnitID;
                //}
                //else
                //{
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                //}
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList1.PageIndex * DataList1.PageSize;
                BizAction.MaximumRows = DataList1.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardPatientPrescriptionDataBizActionVO)arg.Result != null)
                    {
                        DataListOvarian.Clear();
                        DataListStimulation.Clear();
                        DataListTrigger.Clear();
                        DataListOtherDrug.Clear();
                        DataListLutealPhase.Clear();

                        if (((clsGetIVFDashboardPatientPrescriptionDataBizActionVO)arg.Result).PatientPrescriptionList != null)
                        {
                            //DataList1.TotalItemCount = ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;

                            foreach (var item in ((clsGetIVFDashboardPatientPrescriptionDataBizActionVO)arg.Result).PatientPrescriptionList)
                            {
                                item.FrequencyName = FreqList;
                                item.RouteName = RouteList;
                                item.InstructionName = InstructionList;
                                //if ((item.SelectedFrequency == null || item.SelectedFrequency.Description == null || item.SelectedFrequency.Description == "-- Select --") && FreqList != null)
                                //{
                                //    item.SelectedFrequency = item.FrequencyName[0];
                                //}
                                //if ((item.SelectedInstruction == null || item.SelectedInstruction.Description == null || item.SelectedInstruction.Description == "-- Select --") && InstructionList != null)
                                //{
                                //    item.SelectedInstruction = item.InstructionName[0];
                                //}
                                //if ((item.SelectedRoute == null || item.SelectedRoute.Description == null || item.SelectedRoute.Description == "-- Select --") && RouteList != null)
                                //{
                                //    item.SelectedRoute = item.RouteName[0];
                                //}
                                //item.Frequency = item.Frequency;
                                //item.Instruction = item.Instruction;

                                //added by neena
                                item.DrugSourceList = DrugSourseList;
                                if (Convert.ToInt64(item.DrugSource) > 0)
                                {
                                    item.SelectedDrugSource = DrugSourseList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.DrugSource));
                                }
                                else
                                {
                                    item.SelectedDrugSource = DrugSourseList.FirstOrDefault(p => p.ID == 0);
                                }
                                //

                                if (item.DrugSource == 1)
                                    DataListOvarian.Add(item);
                                else if (item.DrugSource == 2)
                                    DataListStimulation.Add(item);
                                else if (item.DrugSource == 3)
                                    DataListTrigger.Add(item);
                                else if (item.DrugSource == 4)
                                    DataListOtherDrug.Add(item);
                                else if (item.DrugSource == 5)
                                    DataListLutealPhase.Add(item);
                            }
                        }
                        OvarianDurg = DataListOvarian.ToList();
                        PagedCollectionView pcvOvarianDurgListHistory = new PagedCollectionView(OvarianDurg);
                        pcvOvarianDurgListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgOvarianDrugList.ItemsSource = null;
                        dgOvarianDrugList.ItemsSource = pcvOvarianDurgListHistory;
                        dgOvarianDrugList.UpdateLayout();

                        StimulationDurg = DataListStimulation.ToList();
                        PagedCollectionView pcvStimulationDurgListHistory = new PagedCollectionView(StimulationDurg);
                        pcvStimulationDurgListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgStimulationDrugList.ItemsSource = null;
                        dgStimulationDrugList.ItemsSource = pcvStimulationDurgListHistory;
                        dgStimulationDrugList.UpdateLayout();

                        TriggerDurg = DataListTrigger.ToList();
                        PagedCollectionView pcvTriggerDurgListHistory = new PagedCollectionView(TriggerDurg);
                        pcvTriggerDurgListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgTriggerDrugList.ItemsSource = null;
                        dgTriggerDrugList.ItemsSource = pcvTriggerDurgListHistory;
                        dgTriggerDrugList.UpdateLayout();

                        OtherDurg = DataListOtherDrug.ToList();
                        PagedCollectionView pcvOtherDurgListHistory = new PagedCollectionView(OtherDurg);
                        pcvOtherDurgListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgOtherDrugList.ItemsSource = null;
                        dgOtherDrugList.ItemsSource = pcvOtherDurgListHistory;
                        dgOtherDrugList.UpdateLayout();
                        //pgrPatientpreviousDiagnosis.Source = DataList1;

                        LutealDurg = DataListLutealPhase.ToList();
                        PagedCollectionView pcvLutealDurgListHistory = new PagedCollectionView(LutealDurg);
                        pcvLutealDurgListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgLutealPhaseDrugList.ItemsSource = null;
                        dgLutealPhaseDrugList.ItemsSource = pcvLutealDurgListHistory;
                        dgLutealPhaseDrugList.UpdateLayout();

                        if (OvarianDurg.Count > 0)
                        {
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartOvarian == null)
                                dtStartDateOVarian.SelectedDate = OvarianDurg[0].VisitDate;
                            else
                                dtStartDateOVarian.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartOvarian;
                        }
                        if (StimulationDurg.Count > 0)
                        {
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartStimulation == null)
                                dtStartDateStimulation.SelectedDate = StimulationDurg[0].VisitDate;
                            else
                                dtStartDateStimulation.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartStimulation;
                        }
                        if (TriggerDurg.Count > 0)
                        {
                            //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger == null)
                            if (TriggerDateForOPU == null)
                            {
                                dtStartDateTrigger.SelectedDate = TriggerDurg[0].VisitDate;
                                txtTrigTime.Value = TriggerDurg[0].VisitDate;
                                //((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger = null;
                                //((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime = null;
                            }
                            else
                            {
                                dtStartDateTrigger.SelectedDate = TriggerDateForOPU; // ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;
                                txtTrigTime.Value = TriggerTimeForOPU; // ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;
                            }
                        }

                        if (LutealDurg.Count > 0)
                        {
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).LutealStartDate == null)
                                dtLutealStartDate.SelectedDate = LutealDurg[0].VisitDate;
                            else
                                dtLutealStartDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).LutealStartDate;
                        }
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    GetPatientPreviousInvestigation();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void GetPatientPreviousInvestigation()
        {
            try
            {
                dgInvestigation.ItemsSource = null;
                dgUltrasonographyGrid.ItemsSource = null;
                clsGetIVFDashboardPatientInvestigationDataBizActionVO BizAction = new clsGetIVFDashboardPatientInvestigationDataBizActionVO();
                //if (EMRLink)
                //{
                //    BizAction.PatientID = EMRPatientID;
                //    BizAction.PatientUnitID = EMRPatientUnitID;
                //}
                //else
                //{
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                BizAction.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                //}
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList1.PageIndex * DataList1.PageSize;
                BizAction.MaximumRows = DataList1.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardPatientInvestigationDataBizActionVO)arg.Result != null)
                    {
                        DataListInvestigation.Clear();
                        if (((clsGetIVFDashboardPatientInvestigationDataBizActionVO)arg.Result).PatientInvestigationList != null)
                        {
                            //DataList1.TotalItemCount = ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;
                            //DataListInvestigation.Clear();
                            foreach (var item in ((clsGetIVFDashboardPatientInvestigationDataBizActionVO)arg.Result).PatientInvestigationList)
                            {
                                DataListInvestigation.Add(item);
                            }

                        }
                        InvestigationList = DataListInvestigation.ToList();
                        PagedCollectionView pcvInvestigationListHistory = new PagedCollectionView(InvestigationList);
                        pcvInvestigationListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgInvestigation.ItemsSource = null;
                        dgInvestigation.ItemsSource = pcvInvestigationListHistory;
                        dgInvestigation.UpdateLayout();

                        RadiologyList = DataListInvestigation.Where(x => x.Specialization == 13).ToList();
                        PagedCollectionView pcvRadiologyListHistory = new PagedCollectionView(RadiologyList);
                        pcvRadiologyListHistory.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        dgUltrasonographyGrid.ItemsSource = null;
                        dgUltrasonographyGrid.ItemsSource = pcvRadiologyListHistory;
                        dgUltrasonographyGrid.UpdateLayout();


                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        //

        private void GetPatientPreviousDiagnosis()
        {
            try
            {
                if (wait == null)
                {
                    wait = new WaitIndicator();
                    wait.Show();
                }

                dgPreviousDiagnosisList.ItemsSource = null;
                DiagnosisListHistory = new List<clsEMRAddDiagnosisVO>();
                clsGetIVFDashboardPatientDiagnosisDataBizActionVO BizAction = new clsGetIVFDashboardPatientDiagnosisDataBizActionVO();
                //if (EMRLink)
                //{
                //    BizAction.PatientID = EMRPatientID;
                //    BizAction.PatientUnitID = EMRPatientUnitID;
                //}
                //else
                //{
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                //}
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList1.PageIndex * DataList1.PageSize;
                BizAction.MaximumRows = DataList1.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        DataList1.Clear();
                        if (((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DataList1.TotalItemCount = ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;                           
                            foreach (var item in ((clsGetIVFDashboardPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DataList1.Add(item);
                            }
                        }
                        DiagnosisListHistory = DataList1.ToList();
                        PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(DiagnosisListHistory);
                        pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                        dgPreviousDiagnosisList.ItemsSource = null;
                        dgPreviousDiagnosisList.ItemsSource = pcvDiagnosisListHistory;
                        dgPreviousDiagnosisList.UpdateLayout();
                        pgrPatientpreviousDiagnosis.Source = DataList1;
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    GetPatientPreviousPrescription();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        private void GetPatientDiagnosis()
        {
            try
            {
                clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO BizAction = new clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO();
                //if (EMRLink)
                //{
                //    BizAction.PatientID = EMRPatientID;
                //    BizAction.PatientUnitID = EMRPatientUnitID;
                //}
                //else
                //{
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                //}
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if ((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DiagnosisList = new ObservableCollection<clsEMRAddDiagnosisVO>();
                            foreach (var item in ((clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                //if (IsEnabledControl == false)
                                //    item.IsEnabled = true;
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DiagnosisList.Add(item);
                            }
                        }
                        dgDiagnosisList.ItemsSource = DiagnosisList;
                        dgDiagnosisList.UpdateLayout();
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion


        #region fillCoupleDetails
        private void fillCoupleDetails()
        {
            if (EMRLink == false)
                wait.Show();
            txtPatientName.Text = string.Empty;
            try
            {
                clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                if (EMRLink)
                {
                    BizAction.PatientID = EMRPatientID;
                    BizAction.PatientUnitID = EMRPatientUnitID;
                }
                else
                {
                    BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                    BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                }
                BizAction.IsAllCouple = false;
                BizAction.CoupleDetails = new clsCoupleVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //wait.Show();
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                        {
                            BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                            BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                            CoupleDetails.MalePatient = new clsPatientGeneralVO();
                            CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                            CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                            //.........................................
                            ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = CoupleDetails;
                            IsPatientExist = true;
                            //...................................................
                            if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                ModuleName = "PalashDynamics";
                                Action = "CIMS.Forms.PatientList";
                                UserControl rootPage = Application.Current.RootVisual as UserControl;

                                WebClient c2 = new WebClient();
                                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                                wait.Close();
                            }
                            else
                            {
                                GetHeightAndWeight(BizAction.CoupleDetails);
                                //if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                                //if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName.Length > 0)
                                //{
                                //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                //    //bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                                //    //imgPhoto13.Source = bmp;

                                //    //byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo;
                                //    //BitmapImage img = new BitmapImage();
                                //    //img.SetSource(new MemoryStream(imageBytes, false));
                                //    //imgPhoto13.Source = img;

                                //    //added by neena
                                //    imgPhoto13.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName, UriKind.Absolute));
                                //    //


                                //    imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                //    imgP1.Visibility = System.Windows.Visibility.Collapsed;
                                //}

                                if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName.Length > 0)
                                {
                                    imgPhoto13.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.ImageName, UriKind.Absolute));

                                    imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                    imgP1.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                                {
                                    byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo;
                                    BitmapImage img = new BitmapImage();
                                    img.SetSource(new MemoryStream(imageBytes, false));
                                    imgPhoto13.Source = img;

                                    imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                    imgP1.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                {
                                    imgP1.Visibility = System.Windows.Visibility.Visible;
                                    imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
                                }

                                //if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                                //if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName.Length > 0)
                                //{
                                //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                //    //bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                                //    //imgPhoto12.Source = bmp;

                                //    //byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo;
                                //    //BitmapImage img = new BitmapImage();
                                //    //img.SetSource(new MemoryStream(imageBytes, false));
                                //    //imgPhoto12.Source = img;

                                //    //added by neena
                                //    imgPhoto12.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName, UriKind.Absolute));
                                //    //

                                //    imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                //    imgP2.Visibility = System.Windows.Visibility.Collapsed;
                                //}

                                if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName.Length > 0)
                                {
                                    imgPhoto12.Source = new BitmapImage(new Uri(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.ImageName, UriKind.Absolute));

                                    imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                    imgP2.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                                {
                                    byte[] imageBytes = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo;
                                    BitmapImage img = new BitmapImage();
                                    img.SetSource(new MemoryStream(imageBytes, false));
                                    imgPhoto12.Source = img;

                                    imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                    imgP2.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                {
                                    imgP2.Visibility = System.Windows.Visibility.Visible;
                                    imgPhoto12.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                //wait.Close();
                                //CmdNewTheorpy.Visibility = Visibility.Visible;
                                objAnimation.Invoke(RotationType.Backward);



                                Maleborder.Visibility = Visibility.Visible;
                                Femaleborder.Visibility = Visibility.Visible;

                                //MaleInfo.Visibility = Visibility.Visible;
                                //FemaleInfo.Visibility = Visibility.Visible;
                                // Doc2.Visibility = Visibility.Visible;
                                //Doc1.Visibility = Visibility.Visible;



                                IVFServiceBilled();
                            }
                        }
                        else
                        {
                            LoadPatientHeader();
                        }
                        wait.Close();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }

        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                    // PatientInfo.Visibility = Visibility.Visible;
                    //CoupleInfo.Visibility = Visibility.Collapsed;
                    if (BizAction.PatientDetails.GeneralDetails.Gender.ToString().ToLower() == "Female" || BizAction.PatientDetails.GeneralDetails.Gender == "FEMALE")
                    {
                        Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.FemalePatient = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.FemalePatient.ImageName = BizAction.PatientDetails.ImageName;
                        Maleborder.Visibility = Visibility.Collapsed;
                        Femaleborder.Visibility = Visibility.Visible;
                        Male.DataContext = null;
                        CoupleDetails.MalePatient = null;

                        if (CoupleDetails.FemalePatient.ImageName != null && CoupleDetails.FemalePatient.ImageName.Length > 0)
                        {
                            imgPhoto13.Source = new BitmapImage(new Uri(CoupleDetails.FemalePatient.ImageName, UriKind.Absolute));

                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            imgP1.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (CoupleDetails.FemalePatient.Photo != null)
                        {
                            byte[] imageBytes = CoupleDetails.FemalePatient.Photo;
                            BitmapImage img = new BitmapImage();
                            img.SetSource(new MemoryStream(imageBytes, false));
                            imgPhoto13.Source = img;

                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            imgP1.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            imgP1.Visibility = System.Windows.Visibility.Visible;
                            imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        GetHeightAndWeight(CoupleDetails);
                        //CmdNewTheorpy.Visibility = Visibility.Visible;
                        //MaleInfo.Visibility = Visibility.Collapsed;
                        // Doc1.Visibility = Visibility.Visible;
                        // Doc2.Visibility = Visibility.Collapsed;
                        //CoupleInfo.Width = Convert.ToDouble(GridUnitType.Auto);
                        CoupleDetails.CoupleId = 0;
                        objAnimation.Invoke(RotationType.Backward);
                        IVFServiceBilled();
                        //setupPage(0, 0); 
                        //CmdNewTheorpy.IsEnabled = false; //added by neena

                    }
                    else if (BizAction.PatientDetails.GeneralDetails.Gender.ToString().ToLower() == "Male" || BizAction.PatientDetails.GeneralDetails.Gender == "MALE")
                    {
                        CoupleDetails.CoupleId = 0;
                        Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.MalePatient = BizAction.PatientDetails.GeneralDetails;
                        CoupleDetails.MalePatient.ImageName = BizAction.PatientDetails.ImageName;
                        Femaleborder.Visibility = Visibility.Collapsed;
                        Maleborder.Visibility = Visibility.Visible;
                        Female.DataContext = null;
                        CoupleDetails.FemalePatient = null;

                        if (CoupleDetails.MalePatient.ImageName != null && CoupleDetails.MalePatient.ImageName.Length > 0)
                        {
                            imgPhoto12.Source = new BitmapImage(new Uri(CoupleDetails.MalePatient.ImageName, UriKind.Absolute));

                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (CoupleDetails.MalePatient.Photo != null)
                        {
                            byte[] imageBytes = CoupleDetails.MalePatient.Photo;
                            BitmapImage img = new BitmapImage();
                            img.SetSource(new MemoryStream(imageBytes, false));
                            imgPhoto12.Source = img;

                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            imgP2.Visibility = System.Windows.Visibility.Visible;
                            imgPhoto12.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        GetHeightAndWeight(CoupleDetails);

                        CmdNewTheorpy.IsEnabled = false;
                        objAnimation.Invoke(RotationType.Backward);
                        //CmdNewTheorpy.Visibility = Visibility.Collapsed;
                        //FemaleInfo.Visibility = Visibility.Collapsed;
                        // Doc2.Visibility = Visibility.Visible;
                        //Doc1.Visibility = Visibility.Collapsed;
                        dgtheropyList.ItemsSource = null;

                    }
                    else
                    {
                        Male.DataContext = null;
                        Female.DataContext = null;
                        CoupleDetails = null;
                        Femaleborder.Visibility = Visibility.Visible;
                        Maleborder.Visibility = Visibility.Visible;
                        // CmdNewTheorpy.Visibility = Visibility.Visible;
                        //MaleInfo.Visibility = Visibility.Visible;
                        //FemaleInfo.Visibility = Visibility.Visible;
                        // Doc2.Visibility = Visibility.Visible;
                        //Doc1.Visibility = Visibility.Visible;

                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("VISIT");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();

            if (CoupleDetails.FemalePatient != null)
            {
                BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.FemalePatientUnitID = CoupleDetails.FemalePatient.UnitId;
            }
            if (CoupleDetails.MalePatient != null)
            {
                BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
                BizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();

                        if (CoupleDetails.FemalePatient != null)
                        {
                            FemalePatientDetails = CoupleDetails.FemalePatient;
                            FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                            FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                            FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                            FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                            Female.DataContext = FemalePatientDetails;

                            FemaleHeight.Text = FemalePatientDetails.Height.ToString();
                            FemaleWeight.Text = FemalePatientDetails.Weight.ToString();
                            txtBMI.Text = FemalePatientDetails.BMI.ToString();
                        }

                        if (CoupleDetails.MalePatient != null)
                        {
                            clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                            MalePatientDetails = CoupleDetails.MalePatient;
                            MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                            MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                            MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                            MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                            Male.DataContext = MalePatientDetails;
                            MaleHeight.Text = MalePatientDetails.Height.ToString();
                            MaleWeight.Text = MalePatientDetails.Weight.ToString();
                            txtMBMI.Text = MalePatientDetails.BMI.ToString();
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region Calling EMR
        // Templte For Female History = 19
        private void cmdhistoryFemale_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Female");
                ((ChildWindow)myData).Title = "Female History ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Templte For Male History = 22
        private void cmdhistorymale_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleHistory);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
            }

        }
        void c_OpenReadCompleted_MaleHistory(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Male");
                ((ChildWindow)myData).Title = "Male History ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Templte For Female Finding = 27
        private void cmdFemaleFindings_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FeamleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FeamleFinding(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Female-Findings");
                ((ChildWindow)myData).Title = "Female Findings ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )"; ;
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Templte For male Finding=43
        private void cmdmaleFindings_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                // Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_MaleFinding(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Male-Findings");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Male Findings ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FemaleUSG=28
        private void cmdFemaleUSG_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleUSG);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleUSG(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("USG");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "USG ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FemaleHystroscopy=23
        private void cmdFemaleHystroscopy_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleHystroscopy);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleHystroscopy(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Hysteroscopy");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "Hysteroscopy ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemaleLaproscopy=24
        private void cmdFemaleLaproscopy_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                // Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleLaproscopy);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleLaproscopy(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Laproscopy");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "Laproscopy ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //FemaleTBPCR=30
        private void cmdFemaleTBPCR_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleTBPCR);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleTBPCR(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("TBPCR");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "TBPCR ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemalePCT=24
        private void cmdFemalePCT_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemalePCT);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemalePCT(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("PCT");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "PCT ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemaleHSG=62
        private void cmdFemaleHSG_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                // Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleHSG);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleHSG(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("HSG");
                ((ChildWindow)myData).Title = "HSG ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //SemenWash=37
        private void cmdSemenWash_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                //((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                //ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenWash);
                //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


                SemenWash_Dashboard frm = new SemenWash_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                // SemenExamination_Dashboard frm = new SemenExamination_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                frm.Title = "Semen Preparation (Name:- " + CoupleDetails.MalePatient.FirstName +
                    " " + CoupleDetails.MalePatient.LastName + ")";

                frm.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                frm.PatientID = CoupleDetails.MalePatient.PatientID;
                frm.PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frm.VisitID = CoupleDetails.MalePatient.VisitID;
                frm.CoupleDetails = CoupleDetails; //added by neena
                frm.TransactionTypeID = 1;
                frm.CallType = 1;
                frm.IsFromDashBoard = true;
                frm.Initiate("1");
                frm.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_SemenWash(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("37");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Wash";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //maleSemenCultureNSentity=39
        private void cmdmaleSemenCultureNSentity_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenCultureNSentity);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_SemenCultureNSentity(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Semen-Culture-And-Sensitivity");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Culture And Sensitivity ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //maleSemenExam=40
        private void cmdmaleSemenExam_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                //((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                //ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenExam);
                //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                SemenExamination_Dashboard frm = new SemenExamination_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                frm.Title = "Semen Examination (Name:- " + CoupleDetails.MalePatient.FirstName +
                    " " + CoupleDetails.MalePatient.LastName + ")";

                frm.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                frm.PatientID = CoupleDetails.MalePatient.PatientID;
                frm.PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frm.VisitID = CoupleDetails.MalePatient.VisitID;
                frm.IsFromDashBoard = true;
                frm.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_maleSemenExam(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("40");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Examination";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //SemenSurvival=38
        private void cmdmaleSemenSurvival_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("Semen-Survival");
                ((ChildWindow)myData).Title = "Semen Survival ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // maleTESE ID- 57
        private void cmdmaleTESE_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                Action = "DataDrivenApplication.Forms.frmNewChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_TESE);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_TESE(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("TESE");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "TESE ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Calling Form
        // Lab Test For male
        private void cmdmaleLabtest_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frmLabTestMale invFemale = new frmLabTestMale(PatientID, PatientUnitID);
                invFemale.Title = "Male Investigation (Name:- " + CoupleDetails.MalePatient.FirstName +
                     " " + CoupleDetails.MalePatient.LastName + ")";
                invFemale.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Lab Test For Female
        private void cmdFemaleLabtest_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                frmLabTestFemale invFemale = new frmLabTestFemale(PatientID, PatientUnitID);
                invFemale.Title = "Female Investigation (Name:- " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + ")";
                invFemale.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
            }
        }

        // Form maleSemenExamination
        private void cmdmaleSemenExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.PatientEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Form maleSemenThawing
        private void cmdmaleSemenThawing_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID > 0)
            {
                //long PatientID = CoupleDetails.MalePatient.PatientID;
                //long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                //frmSpremThawing objwin = new frmSpremThawing(PatientID, PatientUnitID);
                //objwin.Show();
                SpermThawingForDashboard win = new SpermThawingForDashboard();
                win.CoupleDetails = CoupleDetails;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Form SemenFreezing
        private void cmdmaleSemenFreezing_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID > 0)
            {
                ////long PatientID = CoupleDetails.MalePatient.PatientID;
                ////long PatientUnitID = CoupleDetails.MalePatient.UnitId;

                ////frmSpremFrezing objWin = new frmSpremFrezing(PatientID, PatientUnitID, CoupleDetails);
                ////objWin.Show();

                SpermFreezingForDashboard win = new SpermFreezingForDashboard();
                win.CoupleDetails = CoupleDetails;

                //win.Title = "Semen Preparation (Name:- " + CoupleDetails.MalePatient.FirstName +
                //    " " + CoupleDetails.MalePatient.LastName + ")";

                win.Title = "Self Sperm Freezing (Name:- " + CoupleDetails.MalePatient.FirstName +
                  " " + CoupleDetails.MalePatient.LastName + ")";
                win.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                win.PatientID = CoupleDetails.MalePatient.PatientID;
                win.PatientUnitID = CoupleDetails.MalePatient.UnitId;
                win.IsPatientExist = true;
                win.IsFromDashBoard = true;

                win.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // For maleConsent . . 
        private void cmdmaleConsent_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                frmMaleConsent obj = new frmMaleConsent(PatientID, PatientUnitID, CoupleDetails);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        //for FemaleConsent
        private void cmdFemaleConsent_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                frmFemaleConsent obj = new frmFemaleConsent(PatientID, PatientUnitID, CoupleDetails);
                obj.PatientID = PatientID;
                obj.PatientUnitID = PatientUnitID;
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        //FemaleExamination
        private void cmdFemaleExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;

                frmFemaleExamination obj = new frmFemaleExamination(PatientID, PatientUnitID);
                obj.Title = "Female Examination :- (Name - " + CoupleDetails.FemalePatient.FirstName +
                  " " + CoupleDetails.FemalePatient.LastName + ")";
                obj.Closed += new EventHandler(OnExaminationClosed);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        //maleExamination
        private void cmdmaleExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;

                frmMaleExamination_Dashboard obj = new frmMaleExamination_Dashboard(PatientID, PatientUnitID);
                obj.Title = "Male Examination :- (Name - " + CoupleDetails.MalePatient.FirstName +
                " " + CoupleDetails.MalePatient.LastName + ")";
                obj.Closed += new EventHandler(OnExaminationClosed);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        #endregion

        private void OnExaminationClosed(object sender, EventArgs e)
        {
            fillCoupleDetails();
            //try
            //{
            //    clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            //    BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            //    BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            //    BizAction.IsAllCouple = false;
            //    BizAction.CoupleDetails = new clsCoupleVO();

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    wait.Show();
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
            //            {
            //                BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
            //                BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
            //                CoupleDetails.MalePatient = new clsPatientGeneralVO();
            //                CoupleDetails.FemalePatient = new clsPatientGeneralVO();
            //                CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
            //                //.........................................
            //                ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = CoupleDetails;
            //                IsPatientExist = true;
            //                //...................................................
            //                if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
            //                {
            //                    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                    msgW1.Show();

            //                    ModuleName = "PalashDynamics";
            //                    Action = "CIMS.Forms.PatientList";
            //                    UserControl rootPage = Application.Current.RootVisual as UserControl;

            //                    WebClient c2 = new WebClient();
            //                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
            //                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            //                    wait.Close();
            //                }
            //                else
            //                {
            //                    GetHeightAndWeight(BizAction.CoupleDetails);
            //                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
            //                    {
            //                        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
            //                        bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
            //                        imgPhoto13.Source = bmp;
            //                        imgPhoto13.Visibility = System.Windows.Visibility.Visible;
            //                        imgP1.Visibility = System.Windows.Visibility.Collapsed;
            //                    }
            //                    else
            //                    {
            //                        imgP1.Visibility = System.Windows.Visibility.Visible;
            //                        imgPhoto13.Visibility = System.Windows.Visibility.Collapsed;
            //                    }

            //                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
            //                    {
            //                        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
            //                        bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
            //                        imgPhoto12.Source = bmp;
            //                        imgPhoto12.Visibility = System.Windows.Visibility.Visible;
            //                        imgP2.Visibility = System.Windows.Visibility.Collapsed;
            //                    }
            //                    else
            //                    {
            //                        imgP2.Visibility = System.Windows.Visibility.Visible;
            //                        imgPhoto12.Visibility = System.Windows.Visibility.Collapsed;
            //                    }
            //                    wait.Close();
            //                    CmdNewTheorpy.Visibility = Visibility.Visible;
            //                    objAnimation.Invoke(RotationType.Backward);
            //                }
            //            }

            //            wait.Close();
            //        }
            //        else
            //        {
            //            wait.Close();
            //        }
            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //}
            //catch (Exception ex)
            //{
            //    wait.Close();
            //    throw ex;
            //}
        }

        private bool CheckPACConsentCheck()
        {
            bool result = true;
            //if (IsPACChecked && IsConsentCheck)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //     new MessageBoxControl.MessageBoxChildWindow("", "PAC and Consent check is not done", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    TabOverview.IsSelected = true;
            //    result = false;
            //}
            //else if (IsPACChecked)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //      new MessageBoxControl.MessageBoxChildWindow("", "PAC is not done", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    TabOverview.IsSelected = true;
            //    result = false;
            //}
            // else 
            if (IsConsentCheck)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Consent check is not done", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                TabOverview.IsSelected = true;
                result = false;
            }
            else if (TabOPU.IsSelected)
            {
                //if (dtStartDateTrigger.SelectedDate == null || txtTrigTime.Value == null || dgTriggerDrugList.ItemsSource == null) // || (dtStartDateTrigger.SelectedDate == null || txtTrigTime.Value == null || dgTriggerDrugList.ItemsSource == null)
               // if ((((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger == null || ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime == null || dgTriggerDrugList.ItemsSource == null))
                if ((TriggerDateForOPU == null || TriggerTimeForOPU == null || dgTriggerDrugList.ItemsSource == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("", "Please select trigger and modify it.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    TabOverview.IsSelected = true;
                    result = false;
                }
            }

            return result;
        }

        #region Tab Selection Changed
        bool LoadIUI = false;
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabOverview != null)
            {
                if (TabOverview.IsSelected)
                {
                    //CmdNewTheorpy.IsEnabled = false; //added by neena
                    //if (frmIUI != null && frmIUI.ISSavedIUIID == 0 && frmIUI.ThawedSampleID != "")
                    if (frmIUI != null && frmIUI.IsSampleIDChanged)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWIUI =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                        msgWIUI.Show();
                        LoadIUI = true;
                        TabIUI.IsSelected = true;
                        LoadIUI = true;

                    }
                    else
                        setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
                }
            }
            if (TabOPU != null)
            {
                if (TabOPU.IsSelected)
                {
                    if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == true)  //added by neena
                    {
                        try
                        {
                            wait.Show();
                            clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                            BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                            BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                            BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                            //if (CoupleDetails.CoupleId == 0)
                            //    BizAction.TherapyDetails.IsDonorCycle = true;
                            //else
                            //    BizAction.TherapyDetails.IsDonorCycle = false;
                            BizAction.TherapyDetails.IsHalfBilled = true;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    double BillAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillAmount;
                                    double BillBalanceAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillBalanceAmount;

                                    double CheckPercentage = Convert.ToDouble(((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).ForOPUProcedureBilling);
                                    double CalPer = 0;
                                    double PaidAmount = BillAmount - BillBalanceAmount;
                                    CalPer = (PaidAmount / BillAmount) * 100;

                                    //if (Math.Round(BillBalanceAmount, 0) != 0)
                                    if (Math.Round(CalPer, 2) < CheckPercentage) //50
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWBill1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Bill Amount is Pending", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWBill1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWBill1_OnMessageBoxClosed);
                                        msgWBill1.Show();
                                    }
                                    else //if (BillBalanceAmount == 0)
                                    {
                                        if (CheckPACConsentCheck())
                                        {
                                            UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmOPU") as UIElement;
                                            FrOPU = new frmOPU();
                                            FrOPU = (frmOPU)mydata5;
                                            FrOPU.IsPACChecked = IsPACChecked;
                                            FrOPU.IsConsentCheck = IsConsentCheck;
                                            FrOPU.CoupleDetails = CoupleDetails;
                                            FrOPU.AnesthesistId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).AnesthesistId;
                                            FrOPU.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                            FrOPU.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                            FrOPU.OPUDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                                            FrOPU.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                            FrOPU.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                                            FrOPU.TherapyExecutionList = SelectedTherapyDetails.TherapyExecutionList;
                                            //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                                            //    FrOPU.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                                            //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                                            //    FrOPU.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value;    
                                            if (TriggerDateForOPU != null)
                                                FrOPU.Triggerdate = TriggerDateForOPU;
                                            if (TriggerTimeForOPU != null)
                                                FrOPU.TriggerTime = TriggerTimeForOPU;
                                            FrOPU.IsClosed = IsClosed;
                                            FrOPU.OnSaveButton_Click += new RoutedEventHandler(OnSaveButtonOPU_Click);
                                            FrOPU.OnSaveFreezeButton_Click += new RoutedEventHandler(FrOPU_OnSaveFreezeButton_Click);
                                            OPUContent.Content = FrOPU;
                                        }
                                    }

                                    wait.Close();
                                }
                                else
                                {
                                    wait.Close();
                                }
                                wait.Close();


                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                            wait.Close();
                            throw ex;
                        }
                    }
                    else if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == false)  //added by neena
                    {
                        if (CheckPACConsentCheck())
                        {
                            UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmOPU") as UIElement;
                            FrOPU = new frmOPU();
                            FrOPU = (frmOPU)mydata5;
                            FrOPU.IsPACChecked = IsPACChecked;
                            FrOPU.IsConsentCheck = IsConsentCheck;
                            FrOPU.CoupleDetails = CoupleDetails;
                            FrOPU.AnesthesistId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).AnesthesistId;
                            FrOPU.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            FrOPU.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                            FrOPU.OPUDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                            FrOPU.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                            FrOPU.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                            FrOPU.TherapyExecutionList = SelectedTherapyDetails.TherapyExecutionList;
                            //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                            //    FrOPU.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                            //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                            //    FrOPU.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value; 
                            if (TriggerDateForOPU != null)
                                FrOPU.Triggerdate = TriggerDateForOPU;
                            if (TriggerTimeForOPU != null)
                                FrOPU.TriggerTime = TriggerTimeForOPU;
                            FrOPU.IsClosed = IsClosed;
                            FrOPU.OnSaveButton_Click += new RoutedEventHandler(OnSaveButtonOPU_Click);
                            FrOPU.OnSaveFreezeButton_Click += new RoutedEventHandler(FrOPU_OnSaveFreezeButton_Click);
                            OPUContent.Content = FrOPU;
                        }
                    }

                }
            }
            if (TabCryoPreservation != null)
            {
                if (TabCryoPreservation.IsSelected)
                {
                    if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == true)  //added by neena
                    {
                        try
                        {
                            wait.Show();
                            clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                            BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                            BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                            BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                            //if (CoupleDetails.CoupleId == 0)
                            //    BizAction.TherapyDetails.IsDonorCycle = true;
                            //else
                            //    BizAction.TherapyDetails.IsDonorCycle = false;
                            BizAction.TherapyDetails.IsHalfBilled = true;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    double BillAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillAmount;
                                    double BillBalanceAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillBalanceAmount;

                                    double CheckPercentage = Convert.ToDouble(((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).ForOPUProcedureBilling);
                                    double CalPer = 0;
                                    double PaidAmount = BillAmount - BillBalanceAmount;
                                    CalPer = (PaidAmount / BillAmount) * 100;

                                    //if (Math.Round(BillBalanceAmount, 0) != 0)
                                    if (Math.Round(CalPer, 2) < CheckPercentage) //50
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWBill1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Bill Amount is Pending", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWBill1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWBill1_OnMessageBoxClosed);
                                        msgWBill1.Show();
                                    }
                                    else //if (BillBalanceAmount == 0)
                                    {
                                        if (CheckPACConsentCheck())
                                        {
                                            UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmCryoPreservation") as UIElement;
                                            frmCryo = new frmCryoPreservation();
                                            frmCryo = (frmCryoPreservation)mydata5;
                                            frmCryo.CoupleDetails = CoupleDetails;
                                            frmCryo.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                            frmCryo.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                            frmCryo.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                            frmCryo.PlanTherapyVO = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                                            frmCryo.IsClosed = IsClosed;
                                            frmCryo.IsEmbryoDonation = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation;
                                            frmCryo.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                                            CryopreservationContent.Content = frmCryo;
                                        }
                                    }
                                    wait.Close();
                                }
                                else
                                {
                                    wait.Close();
                                }
                                wait.Close();


                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                            wait.Close();
                            throw ex;
                        }
                    }
                    else if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == false)  //added by neena
                    {
                        if (CheckPACConsentCheck())
                        {
                            UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmCryoPreservation") as UIElement;
                            frmCryo = new frmCryoPreservation();
                            frmCryo = (frmCryoPreservation)mydata5;
                            frmCryo.CoupleDetails = CoupleDetails;
                            frmCryo.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                            frmCryo.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            frmCryo.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                            frmCryo.PlanTherapyVO = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                            frmCryo.IsClosed = IsClosed;
                            frmCryo.IsEmbryoDonation = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation;
                            frmCryo.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                            CryopreservationContent.Content = frmCryo;
                        }
                    }
                }
            }

            if (TabIUI != null)
            {
                if (TabIUI.IsSelected)
                {
                    //UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.FrmIUI") as UIElement;
                    //frmIUI = new FrmIUI();
                    //frmIUI = (FrmIUI)mydata2;
                    //frmIUI.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
                    //frmIUI.PlanTherapyID = SelectedTherapyDetails.TherapyDetails.ID;
                    //frmIUI.PlanTherapyUnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
                    //frmIUI.IsClosed = IsClosed;
                    //IUIContent.Content = frmIUI;
                    if (LoadIUI == true)
                        LoadIUI = false;
                    else
                    {
                        if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == true)  //added by neena
                        {
                            try
                            {
                                wait.Show();
                                clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                                BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                                BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                                //if (CoupleDetails.CoupleId == 0)
                                //    BizAction.TherapyDetails.IsDonorCycle = true;
                                //else
                                //    BizAction.TherapyDetails.IsDonorCycle = false;
                                BizAction.TherapyDetails.IsHalfBilled = true;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        double BillAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillAmount;
                                        double BillBalanceAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillBalanceAmount;

                                        double CheckPercentage = Convert.ToDouble(((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).ForOPUProcedureBilling);
                                        double CalPer = 0;
                                        double PaidAmount = BillAmount - BillBalanceAmount;
                                        CalPer = (PaidAmount / BillAmount) * 100;

                                        //if (Math.Round(BillBalanceAmount, 0) != 0)
                                        if (Math.Round(CalPer, 2) < CheckPercentage) //50                                       
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgWBill1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("", "Bill Amount is Pending", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgWBill1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWBill1_OnMessageBoxClosed);
                                            msgWBill1.Show();
                                        }
                                        else //if (BillBalanceAmount == 0)
                                        {
                                            if (CheckPACConsentCheck())
                                            {
                                                UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIUIComeSemenWash") as UIElement;
                                                frmIUI = new frmIUIComeSemenWash();
                                                frmIUI = (frmIUIComeSemenWash)mydata2;
                                                frmIUI.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
                                                frmIUI.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                                frmIUI.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                                frmIUI.PlannedSpermCollection = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedSpermCollectionID;
                                                frmIUI.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                                frmIUI.IsClosed = IsClosed;
                                                IUIContent.Content = frmIUI;
                                            }
                                        }
                                        wait.Close();
                                    }
                                    else
                                    {
                                        wait.Close();
                                    }
                                    wait.Close();


                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();
                            }
                            catch (Exception ex)
                            {
                                wait.Close();
                                throw ex;
                            }
                        }
                        else if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == false)  //added by neena
                        {
                            if (CheckPACConsentCheck())
                            {
                                UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIUIComeSemenWash") as UIElement;
                                frmIUI = new frmIUIComeSemenWash();
                                frmIUI = (frmIUIComeSemenWash)mydata2;
                                frmIUI.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
                                frmIUI.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                frmIUI.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                frmIUI.PlannedSpermCollection = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedSpermCollectionID;
                                frmIUI.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                frmIUI.IsClosed = IsClosed;
                                IUIContent.Content = frmIUI;
                            }
                        }
                    }
                }
            }
            if (TabLutualPhase != null)
            {
                if (TabLutualPhase.IsSelected)
                {
                    fillLutealPhaseDetails();
                }
            }
            if (Taboutcome != null)
            {
                if (Taboutcome.IsSelected)
                {
                    //if (frmIUI != null && frmIUI.ISSavedIUIID == 0 && frmIUI.ThawedSampleID != "")
                    if (frmIUI != null && frmIUI.IsSampleIDChanged)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWIUI =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                        msgWIUI.Show();
                        LoadIUI = true;
                        TabIUI.IsSelected = true;
                    }
                    else
                    {
                        if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsSurrogate == false)
                        {
                            GridOutcome.Visibility = Visibility.Visible;
                            GridSurrogateOutcome.Visibility = Visibility.Collapsed;
                            txtNoOfSacs.Text = "";
                            dgSacsDetailsGrid.ItemsSource = null;
                            PregnacncyAchievedGrid.Visibility = Visibility.Collapsed;
                            //by neena
                            FillOutcomeResult();
                            //FillOutcomePregnacyAchieved();
                            //
                        }
                        else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsSurrogate == true)
                        {
                            GridSurrogateOutcome.Visibility = Visibility.Visible;
                            GridOutcome.Visibility = Visibility.Collapsed;
                            UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.FrmSurrogateOutcome") as UIElement;
                            SurrogacyOutcome = new FrmSurrogateOutcome();
                            SurrogacyOutcome = (FrmSurrogateOutcome)mydata5;
                            SurrogacyOutcome.CoupleDetails = CoupleDetails;
                            SurrogacyOutcome.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            SurrogacyOutcome.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                            SurrogacyOutcome.OPUDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                            SurrogacyOutcome.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                            SurrogacyOutcome.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                            SurrogacyOutcome.IsClosed = IsClosed;
                            SurrogacyOutcome.IsSurrogate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsSurrogate;
                            SurrogacyOutcome.EMRProcedureID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EMRProcedureID;
                            SurrogacyOutcome.EMRProcedureUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EMRProcedureUnitID;
                            SurrogacyOutcome.ETDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ETDate;
                            Tabbirthdetails.IsEnabled = true;
                            SurrogateOutcomeContent.Content = SurrogacyOutcome;
                        }
                    }

                }
            }

            frmGraphicalRepresentationofLabDaysNew grpRepNew = null;
            if (TabGraphicalRepresentation != null)
            {
                if (TabGraphicalRepresentation.IsSelected)
                {
                    if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == true)  //added by neena
                    {
                        try
                        {
                            wait.Show();
                            clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                            BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                            BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                            BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                            BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                            //if (CoupleDetails.CoupleId == 0)
                            //    BizAction.TherapyDetails.IsDonorCycle = true;
                            //else
                            //    BizAction.TherapyDetails.IsDonorCycle = false;
                            BizAction.TherapyDetails.IsHalfBilled = true;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    double BillAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillAmount;
                                    double BillBalanceAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillBalanceAmount;

                                    double CheckPercentage = Convert.ToDouble(((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).ForOPUProcedureBilling);
                                    double CalPer = 0;
                                    double PaidAmount = BillAmount - BillBalanceAmount;
                                    CalPer = (PaidAmount / BillAmount) * 100;

                                    //if (Math.Round(BillBalanceAmount, 0) != 0)
                                    if (Math.Round(CalPer, 2) < CheckPercentage) //50                                    
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWBill1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Bill Amount is Pending", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWBill1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWBill1_OnMessageBoxClosed);
                                        msgWBill1.Show();
                                    }
                                    else //if (BillBalanceAmount == 0)
                                    {
                                        if (CheckPACConsentCheck())
                                        {
                                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul == true)
                                            {
                                                UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysNew") as UIElement;
                                                grpRepNew = new frmGraphicalRepresentationofLabDaysNew();
                                                grpRepNew = (frmGraphicalRepresentationofLabDaysNew)mydata1;
                                                grpRepNew.CoupleDetails = CoupleDetails;
                                                grpRepNew.SourceOfSperm = SrcOfSperm;
                                                grpRepNew.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                                                grpRepNew.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                                grpRepNew.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                                grpRepNew.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                                grpRepNew.IsDonorCycle = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsDonorCycle;
                                                // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;   
                                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                                                //    grpRepNew.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                                                //    grpRepNew.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value; 
                                                if (TriggerDateForOPU != null)
                                                    grpRepNew.Triggerdate = TriggerDateForOPU;
                                                if (TriggerTimeForOPU != null)
                                                    grpRepNew.TriggerTime = TriggerTimeForOPU;
                                                grpRepNew.IsIsthambul = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul;
                                                ConEmbryology.Content = grpRepNew;
                                            }
                                            else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul == false)
                                            {
                                                frmGraphicalRepresentationofLabDaysWithoutIstambul grpRepNewWithoutIstambul = new frmGraphicalRepresentationofLabDaysWithoutIstambul();
                                                UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysWithoutIstambul") as UIElement;
                                                grpRepNewWithoutIstambul = (frmGraphicalRepresentationofLabDaysWithoutIstambul)mydata1;
                                                grpRepNewWithoutIstambul.CoupleDetails = CoupleDetails;
                                                grpRepNewWithoutIstambul.SourceOfSperm = SrcOfSperm;
                                                grpRepNewWithoutIstambul.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                                                grpRepNewWithoutIstambul.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                                grpRepNewWithoutIstambul.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                                grpRepNewWithoutIstambul.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                                grpRepNewWithoutIstambul.IsDonorCycle = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsDonorCycle;
                                                grpRepNewWithoutIstambul.IsClosed = IsClosed;
                                                // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;  
                                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                                                //    grpRepNewWithoutIstambul.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                                                //    grpRepNewWithoutIstambul.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value; 
                                                if (TriggerDateForOPU != null)
                                                    grpRepNewWithoutIstambul.Triggerdate = TriggerDateForOPU;
                                                if (TriggerTimeForOPU != null)
                                                    grpRepNewWithoutIstambul.TriggerTime = TriggerTimeForOPU;
                                                grpRepNewWithoutIstambul.IsIsthambul = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul;
                                                ConEmbryology.Content = grpRepNewWithoutIstambul;
                                            }
                                        }
                                    }
                                    wait.Close();
                                }
                                else
                                {
                                    wait.Close();
                                }
                                wait.Close();


                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                            wait.Close();
                            throw ex;
                        }
                    }
                    else if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == false)  //added by neena
                    {
                        if (CheckPACConsentCheck())
                        {
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul == true)
                            {
                                UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysNew") as UIElement;
                                grpRepNew = new frmGraphicalRepresentationofLabDaysNew();
                                grpRepNew = (frmGraphicalRepresentationofLabDaysNew)mydata1;
                                grpRepNew.CoupleDetails = CoupleDetails;
                                grpRepNew.SourceOfSperm = SrcOfSperm;
                                grpRepNew.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                                grpRepNew.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                grpRepNew.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                grpRepNew.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                grpRepNew.IsDonorCycle = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsDonorCycle;
                                // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;   
                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                                //    grpRepNew.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                                //    grpRepNew.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value;
                                if (TriggerDateForOPU != null)
                                    grpRepNew.Triggerdate = TriggerDateForOPU;
                                if (TriggerTimeForOPU != null)
                                    grpRepNew.TriggerTime = TriggerTimeForOPU;
                                grpRepNew.IsIsthambul = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul;
                                ConEmbryology.Content = grpRepNew;
                            }
                            else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul == false)
                            {
                                frmGraphicalRepresentationofLabDaysWithoutIstambul grpRepNewWithoutIstambul = new frmGraphicalRepresentationofLabDaysWithoutIstambul();
                                UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysWithoutIstambul") as UIElement;
                                grpRepNewWithoutIstambul = (frmGraphicalRepresentationofLabDaysWithoutIstambul)mydata1;
                                grpRepNewWithoutIstambul.CoupleDetails = CoupleDetails;
                                grpRepNewWithoutIstambul.SourceOfSperm = SrcOfSperm;
                                grpRepNewWithoutIstambul.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                                grpRepNewWithoutIstambul.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                                grpRepNewWithoutIstambul.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                                grpRepNewWithoutIstambul.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                                grpRepNewWithoutIstambul.IsDonorCycle = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsDonorCycle;
                                grpRepNewWithoutIstambul.IsClosed = IsClosed;
                                // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;
                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger != null)
                                //    grpRepNewWithoutIstambul.Triggerdate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;  //dtStartDateTrigger.SelectedDate;
                                //if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime != null)
                                //    grpRepNewWithoutIstambul.TriggerTime = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;   //txtTrigTime.Value;
                                if (TriggerDateForOPU != null)
                                    grpRepNewWithoutIstambul.Triggerdate = TriggerDateForOPU;
                                if (TriggerTimeForOPU != null)
                                    grpRepNewWithoutIstambul.TriggerTime = TriggerTimeForOPU;
                                grpRepNewWithoutIstambul.IsIsthambul = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsIsthambul;
                                ConEmbryology.Content = grpRepNewWithoutIstambul;
                            }
                        }
                    }

                    //UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDays") as UIElement;
                    //grpRep = new frmGraphicalRepresentationofLabDays();
                    //grpRep = (frmGraphicalRepresentationofLabDays)mydata1;
                    //grpRep.CoupleDetails = CoupleDetails;
                    //grpRep.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                    //grpRep.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    //grpRep.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    //grpRep.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;

                    //// grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
                    //AdmissionContent.Content = grpRep;
                }
            }
            if (Tabtransfer != null)
            {
                if (Tabtransfer.IsSelected)
                {
                    UIElement mydata4 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmTransfer") as UIElement;
                    ET = new frmTransfer();
                    ET = (frmTransfer)mydata4;
                    ET.CoupleDetails = CoupleDetails;
                    ET.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    ET.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    ET.IsSurrogate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsSurrogate;
                    ET.EMRProcedureID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EMRProcedureID;
                    ET.EMRProcedureUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EMRProcedureUnitID;

                    ET.IsClosed = IsClosed;
                    TransferContent.Content = ET;
                }
            }
            if (TabOnlyTransfer != null)
            {
                if (TabOnlyTransfer.IsSelected)
                {
                    UIElement mydata4 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmOnlyET") as UIElement;
                    onlyET = new frmOnlyET();
                    onlyET = (frmOnlyET)mydata4;
                    onlyET.CoupleDetails = CoupleDetails;
                    onlyET.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    onlyET.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    onlyET.IsClosed = IsClosed;
                    OnlyTransferContent.Content = onlyET;
                }
            }
            if (TabOnlyVitrification != null)
            {
                if (TabOnlyVitrification.IsSelected)
                {
                    SSemenMaster();
                }
            }
            // For Birth Details
            if (Tabbirthdetails != null)
            {
                if (Tabbirthdetails.IsSelected)
                {
                    //if (frmIUI != null && frmIUI.ISSavedIUIID == 0 && frmIUI.ThawedSampleID != "")
                    if (frmIUI != null && frmIUI.IsSampleIDChanged)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWIUI =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                        msgWIUI.Show();
                        LoadIUI = true;
                        TabIUI.IsSelected = true;
                    }
                    else
                    {
                        UIElement mydata3 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmBirthDetails") as UIElement;
                        Birth = new frmBirthDetails();
                        Birth = (frmBirthDetails)mydata3;

                        Birth.CoupleDetails = CoupleDetails;
                        Birth.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        Birth.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                        Birth.IsSurrogate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsSurrogate;
                        Birth.ChildID = ChildID;
                        Birth.IsPregnancyAchived = IsPregnancyAchived;
                        Birth.PregnancyAchivedDate = PregnancyAchivedDate;
                        Birth.FetalHeartCount = FetalHeartCnt;
                        BirthContent.Content = Birth;

                        //UIElement mydata3 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmBirth") as UIElement;
                        //Birth = new frmBirth();
                        //Birth = (frmBirth)mydata3;

                        //Birth.CoupleDetails = CoupleDetails;
                        //Birth.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        //Birth.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                        //Birth.ChildID = ChildID;
                        //Birth.IsPregnancyAchived = IsPregnancyAchived;
                        //Birth.PregnancyAchivedDate = PregnancyAchivedDate;

                        //BirthContent.Content = Birth;
                    }
                }
            }
            if (TabDocument != null)
            {
                if (TabDocument.IsSelected)
                {
                    UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmDocumentforDashTherapy") as UIElement;
                    Doc = new frmDocumentforDashTherapy();
                    Doc = (frmDocumentforDashTherapy)mydata2;
                    Doc.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
                    Doc.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    Doc.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    Doc.IsClosed = IsClosed;
                    Document.Content = Doc;

                }
            }

            //added by neena
            //frmGraphicalRepresentationofLabDaysNew grpRepNew = null;
            if (TabEmbryology != null)
            {
                if (TabEmbryology.IsSelected)
                {
                    //FillEmbryologySummary();

                    //UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysNew") as UIElement;
                    //grpRepNew = new frmGraphicalRepresentationofLabDaysNew();
                    //grpRepNew = (frmGraphicalRepresentationofLabDaysNew)mydata1;
                    //grpRepNew.CoupleDetails = CoupleDetails;
                    //grpRepNew.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                    //grpRepNew.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    //grpRepNew.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    //grpRepNew.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;

                    //// grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
                    //ConEmbryology.Content = grpRepNew;

                    UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDays") as UIElement;
                    grpRep = new frmGraphicalRepresentationofLabDays();
                    grpRep = (frmGraphicalRepresentationofLabDays)mydata1;
                    grpRep.CoupleDetails = CoupleDetails;
                    grpRep.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                    grpRep.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    grpRep.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    grpRep.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;

                    // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
                    AdmissionContent.Content = grpRep;

                }
            }
            //

        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            tabItems.Add(TabGraphicalRepresentation);
            TabGraphicalRepresentation.Visibility = Visibility.Visible;
        }

        void msgWIUI_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                frmIUI.txtSampleID.Text = frmIUI.ThawedSampleID;
                frmIUI.txtDonorCode.Text = frmIUI.ThawedDonorCode;
                frmIUI.dtThawDate.SelectedDate = frmIUI.ThawedDateTime.Value;
                frmIUI.ThawTime.Value = frmIUI.ThawedDateTime.Value;
            }
            //throw new NotImplementedException();
        }

        void msgWBill1_OnMessageBoxClosed(MessageBoxResult result)
        {
            TabOverview.IsSelected = true;
        }




        private void OnSaveButtonOPU_Click(object sender, RoutedEventArgs e)
        {
            tabItems.Add(TabGraphicalRepresentation);   //commented by neena
            //tabItems.Add(TabEmbryology);  //added by neena
            tabItems.Add(TabCryoPreservation);
            RequestXML("IVFDashboardSubmenu");
            ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation = true;
        }
        private void FrOPU_OnSaveFreezeButton_Click(object sender, RoutedEventArgs e)
        {
            dtStartDateTrigger.IsEnabled = false;
            txtTrigTime.IsEnabled = false;
        }
        #endregion

        #region ExecutionCalender
        private DataTemplate CreateExe(string BindingExp)
        {
            DataTemplate dt = null;
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:PalashDynamics.IVF;assembly=PalashDynamics.IVF"">"
            + "<Converter:ExeExtendedGridNew>"
            + "<Converter:ExeExtendedGridNew.Resources>"
            + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
            + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
            + "<Converter:ToggleBooleanValueConverter  x:Key='ToggleBooleanValueConverter'></Converter:ToggleBooleanValueConverter>"
            + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
            + "<Converter:NumberToBrushConverter  x:Key='NumberToBrushConverter' Id='{Binding Id}'></Converter:NumberToBrushConverter>"
             + "</Converter:ExeExtendedGridNew.Resources>"
            + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
             + "<TextBox Background='{Binding CurrentTherapy, Converter={StaticResource NumberToBrushConverter},ConverterParameter=" + BindingExp + "}' Text='{Binding " + BindingExp + ",Mode=TwoWay}' IsReadOnly='{Binding IsReadOnly}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' MaxLength='6'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExp + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
                + "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}' IsEnabled='{Binding IsBool,Converter={StaticResource ToggleBooleanValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                //+ "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' IsEnabled='{Binding "+ BindingExp +",Converter={StaticResource NegateValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                + "</Converter:ExeExtendedGridNew>"
        + "</DataTemplate>";
            //NumberToBrushConverter
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }

        public void BindExecutionGrid()
        {
            try
            {
                List<clsTherapyExecutionVO> NewExcutionGrid = new List<clsTherapyExecutionVO>();
                if (SelectedTherapyDetails.TherapyExecutionList == null)
                {
                    SelectedTherapyDetails.TherapyExecutionList = new List<clsTherapyExecutionVO>();
                }
                ExeGrid.ItemsSource = null;
                ////var list = from n in App.ExecutionTherapyList where (n.IsDeactive != true) select n;

                foreach (var item in SelectedTherapyDetails.TherapyExecutionList)
                {
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 15 || ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 16)
                    {
                        if (item.TherapyTypeId == 4 || item.TherapyTypeId == 5)
                            continue;
                        else
                            NewExcutionGrid.Add(item);
                    }
                    else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 5 || ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 12)
                    {
                        if (item.TherapyTypeId == 4)
                            continue;
                        else
                            NewExcutionGrid.Add(item);
                    }
                    else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 11)
                    {
                        if (item.TherapyTypeId == 5)
                            continue;
                        else
                            NewExcutionGrid.Add(item);
                    }
                    else
                        NewExcutionGrid.Add(item);
                }

                //Execollection = new PagedCollectionView(SelectedTherapyDetails.TherapyExecutionList); //commented by neena
                Execollection = new PagedCollectionView(NewExcutionGrid); //added by neena
                //Execollection.GroupDescriptions.Add(new PropertyGroupDescription("Therapy Group"));
                Execollection.GroupDescriptions.Add(new PropertyGroupDescription("Particulars"));

                ExeGrid.ItemsSource = Execollection;
                ExeGrid.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitExecutionGrid()
        {
            if (ExeGrid.Columns.Count > 1)
            {
                while (ExeGrid.Columns.Count != 1)
                {
                    ExeGrid.Columns.RemoveAt(1);
                }
            }
            for (int i = 0; i < 30; i++)
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn();
                //column.Width = new DataGridLength(70);

                column.CellTemplate = CreateExe("Day" + (i + 1).ToString());
                DateTime? drsd;
                if (((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate != null)
                    drsd = ((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate;
                else
                    drsd = startDate;
                column.Header = drsd.HasValue ? drsd.Value.AddDays(i).ToString("dd-MMM-yy") : (i + 1).ToString();
                ExeGrid.Columns.Add(column);
            }
        }

        private void ExeGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (((clsTherapyExecutionVO)ExeGrid.SelectedItem) != null)
            {
                if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 1)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "Date of LMP ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 3)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "Follicular Scan ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.MultipleRange;
                    win.ExeCal.DisplayDateStart = startDate;
                    //win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
                    win.RemarkLabel.Visibility = Visibility.Visible;
                    //foreach(var r in SetValueToCal())
                    //{
                    //    win.ExeCal.SelectedDate=r.Date;
                    //}
                    //win.ExeCal.SelectedDates=(SetValueToCal())


                    //SelectedDatesCollection selecteddates = win.ExeCal.SelectedDates;
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));

                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 4)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "OPU ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 5)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "ET ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 6)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "E2 ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 7)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "Progesterone ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 8)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "FSH ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 9)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "LH ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 10)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "Prolactin ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
                //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 11)
                //{
                //    FrmExecutionCalender win = new FrmExecutionCalender();
                //    win.Title = "BHCG ( " + CoupleDetails.FemalePatient.FirstName +
                //         " " + CoupleDetails.FemalePatient.LastName + " )";
                //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                //    win.ExeCal.DisplayDateStart = startDate;
                //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                //    if (win.selecteddates != null)
                //        win.selecteddates.Clear();
                //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                //    win.Show();
                //}
            }
        }

        private void ExeGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void ExeGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

        }

        private void ExeGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
        #endregion

        #region set tab therapywise
        private void SetTabPlanTherapyWise(long pPlanTherapyID)
        {

            TabOverview.Visibility = Visibility.Collapsed;
            TabIUI.Visibility = Visibility.Collapsed;
            TabOPU.Visibility = Visibility.Collapsed;
            TabTableRepresentation.Visibility = Visibility.Collapsed;
            TabGraphicalRepresentation.Visibility = Visibility.Collapsed; //commented by neena
            //TabEmbryology.Visibility = Visibility.Collapsed;  //added by neena
            TabSummedRepresentation.Visibility = Visibility.Collapsed;
            TabCryoPreservation.Visibility = Visibility.Collapsed;
            TabOnlyVitrification.Visibility = Visibility.Collapsed;
            TabOnlyTransfer.Visibility = Visibility.Collapsed;
            Tabtransfer.Visibility = Visibility.Collapsed;
            TabLutualPhase.Visibility = Visibility.Collapsed;
            Taboutcome.Visibility = Visibility.Collapsed;
            Tabbirthdetails.Visibility = Visibility.Collapsed;

            switch (pPlanTherapyID)
            {
                case 1://IVF
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();

                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation);  //commented by neena
                    //tabItems.Add(TabEmbryology);//added by neena
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Visible;
                    txtAnesthetist.Visibility = Visibility.Visible;
                    cmbAnesthesist.Visibility = Visibility.Visible;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                        cmddonorDetails.IsEnabled = false;
                    else
                        cmddonorDetails.IsEnabled = true;
                    cmbSourceOfSperm.IsEnabled = true;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 2://ICSI
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    //TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation);  //commented by neena
                    //tabItems.Add(TabEmbryology); //added by neena
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Visible;
                    txtAnesthetist.Visibility = Visibility.Visible;
                    cmbAnesthesist.Visibility = Visibility.Visible;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                        cmddonorDetails.IsEnabled = false;
                    else
                        cmddonorDetails.IsEnabled = true;
                    cmbSourceOfSperm.IsEnabled = true;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 3://IVF - ICSI
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    // TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation); //commented by neena
                    //tabItems.Add(TabEmbryology); //added by neena
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Visible;
                    txtAnesthetist.Visibility = Visibility.Visible;
                    cmbAnesthesist.Visibility = Visibility.Visible;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                        cmddonorDetails.IsEnabled = false;
                    else
                        cmddonorDetails.IsEnabled = true;
                    cmbSourceOfSperm.IsEnabled = true;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 5://Thaw/Transfer
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    //TabLutualPhase.Visibility = Visibility.Visible;  commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    //added by neena
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).NoOfEmbryos > 0)
                    {
                        tabItems.Add(TabGraphicalRepresentation);
                        TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    }
                    chkPACEnabled.Visibility = Visibility.Visible;
                    txtAnesthetist.Visibility = Visibility.Visible;
                    cmbAnesthesist.Visibility = Visibility.Visible;
                    cmddonorDetails.IsEnabled = true;
                    cmbSourceOfSperm.IsEnabled = true;
                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 11://OocyteDonation
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Visible;
                    //TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    //tabItems.Add(TabOnlyVitrification);
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation == true)
                    {
                        tabItems.Add(TabGraphicalRepresentation); //commented by neena
                        //tabItems.Add(TabEmbryology); //added by neena
                        tabItems.Add(TabCryoPreservation);
                    }

                    //added by neena
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    chkPACEnabled.Visibility = Visibility.Visible;
                    txtAnesthetist.Visibility = Visibility.Visible;
                    cmbAnesthesist.Visibility = Visibility.Visible;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                        cmddonorDetails.IsEnabled = false;
                    else
                        cmddonorDetails.IsEnabled = true;
                    cmbSourceOfSperm.IsEnabled = true;

                    BHCG1.IsEnabled = false;
                    BHCG2.IsEnabled = false;
                    txtNoOfSacs.IsEnabled = false;
                    dtObservationDate.IsEnabled = false;
                    txtSacRemark.IsEnabled = false;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 12://Oocyte Receipant
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Visible;
                    //TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();
                    //tabItems.Add(TabOnlyTransfer);
                    tabItems.Add(TabGraphicalRepresentation); //commented by neena
                    //tabItems.Add(TabEmbryology); //added by neena
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Collapsed;
                    txtAnesthetist.Visibility = Visibility.Collapsed;
                    cmbAnesthesist.Visibility = Visibility.Collapsed;
                    cmddonorDetails.IsEnabled = false;
                    cmbSourceOfSperm.IsEnabled = false;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Collapsed;
                    GbStimulation.Visibility = Visibility.Collapsed;
                    GbTrigger.Visibility = Visibility.Collapsed;
                    break;

                case 15: // IUI(H)
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Visible;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    // TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    Tabbirthdetails.IsEnabled = false;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabIUI);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Collapsed;
                    txtAnesthetist.Visibility = Visibility.Collapsed;
                    cmbAnesthesist.Visibility = Visibility.Collapsed;
                    cmddonorDetails.IsEnabled = false;
                    cmbSourceOfSperm.IsEnabled = false;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;

                case 16: // IUI(D)
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Visible;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    //TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    Tabbirthdetails.IsEnabled = false;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabIUI);

                    //added by neena
                    chkPACEnabled.Visibility = Visibility.Collapsed;
                    cmbAnesthesist.Visibility = Visibility.Collapsed;
                    txtAnesthetist.Visibility = Visibility.Collapsed;
                    cmddonorDetails.IsEnabled = false;
                    cmbSourceOfSperm.IsEnabled = false;

                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Visible;
                    GbStimulation.Visibility = Visibility.Visible;
                    GbTrigger.Visibility = Visibility.Visible;
                    break;
                // BY bHUSHSN

                case 14: // Embryo Recepient
                    TabOverview.Visibility = Visibility.Visible;
                    // TabIUI.Visibility = Visibility.Visible;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    // TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    Tabbirthdetails.Visibility = Visibility.Visible; //added by neena
                    tabItems = new List<TabItem>();
                    // tabItems.Add(TabOnlyTransfer);
                    tabItems.Add(TabGraphicalRepresentation); //commented by neena
                    //tabItems.Add(TabEmbryology); //added by neena
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);
                    // tabItems.Add(Tabbirthdetails);
                    BHCG1.IsEnabled = true;
                    BHCG2.IsEnabled = true;
                    txtNoOfSacs.IsEnabled = true;
                    dtObservationDate.IsEnabled = true;
                    txtSacRemark.IsEnabled = true;
                    GbOvarianSuppression.Visibility = Visibility.Collapsed;
                    GbStimulation.Visibility = Visibility.Collapsed;
                    GbTrigger.Visibility = Visibility.Collapsed;
                    break;

                default:
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    // TabLutualPhase.Visibility = Visibility.Visible; commented by neena
                    Taboutcome.Visibility = Visibility.Visible;
                    // Tabbirthdetails.Visibility = Visibility.Collapsed;
                    break;


            }
            RequestXML("IVFDashboard");
        }
        #endregion

        #region follicular monitoring click
        private void cmdFollicularMonitoring_Click(object sender, RoutedEventArgs e)
        {
            // setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
            frmFollicularMonitoring FMWin = new frmFollicularMonitoring();
            FMWin.Show();
            FMWin.Title = "Follicular Monitoring Details : " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " (Cyclecode :" + ((clsPlanTherapyVO)dgtheropyList.SelectedItem).Cyclecode + ")";
            FMWin.dgFollicularMonitoring.ItemsSource = null;
            FMWin.FollicularMonitoringList = SelectedTherapyDetails.FollicularMonitoringList;
            if (SelectedTherapyDetails.FollicularMonitoringList.Count > 0)
                FMWin.dgFollicularMonitoring.ItemsSource = SelectedTherapyDetails.FollicularMonitoringList;
            FMWin.TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            FMWin.TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            FMWin.FollicularMonitoringAttachemntView_ChildClick += new RoutedEventHandler(WinFollicularMonitoringAttachemntView_ChildClick);
        }

        void WinFollicularMonitoringAttachemntView_ChildClick(object sender, RoutedEventArgs e)
        {
            clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO BizAction = new clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO();
            BizAction.FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();
            BizAction.FollicularID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).ID;
            BizAction.UnitID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).SizeList = new List<clsFollicularMonitoringSizeDetails>();
                    ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).SizeList = ((clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO)arg.Result).FollicularMonitoringSizeList;

                    ((frmFollicularMonitoring)sender).DialogResult = false;
                    ARTFollicularMonitoring addFollic = new ARTFollicularMonitoring();
                    //addFollic.cmbTakenBy1.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                    addFollic.cmbTakenBy1.Text = username;
                    //.....................
                    addFollic.DataContext = null;
                    //..........................
                    addFollic.fillDoctor(((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).PhysicianID);
                    addFollic.DataContext = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem);
                    addFollic.AttachedFileContents = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).AttachmentFileContent;
                    addFollic.dtpFollicularDate.IsEnabled = false;
                    FollicularID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).ID;
                    FollicularUnitID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).UnitID;

                    addFollic.OnSaveButton_Click += new RoutedEventHandler(addFollic_OnSaveButton_Click);
                    addFollic.Show();

                    addFollic.Closed += new EventHandler(addFollic_Closing);

                    SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
                    if ((((clsPlanTherapyVO)dgtheropyList.SelectedItem)).IsClosed)
                        addFollic.OKButton.IsEnabled = (a == true) ? false : true;
                    // End
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public long TherpyID, TherapyUnitID;
        private void addFollic_Closing(object sender, EventArgs e)
        {

            frmFollicularMonitoring FMWin = new frmFollicularMonitoring();
            FMWin.Show();
            FMWin.Title = "Follicular Monitoring Details : " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " (Cyclecode :" + ((clsPlanTherapyVO)dgtheropyList.SelectedItem).Cyclecode + ")";
            FMWin.dgFollicularMonitoring.ItemsSource = null;
            FMWin.dgFollicularMonitoring.ItemsSource = SelectedTherapyDetails.FollicularMonitoringList;
            FMWin.TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            FMWin.TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            FMWin.FollicularMonitoringAttachemntView_ChildClick += new RoutedEventHandler(WinFollicularMonitoringAttachemntView_ChildClick);
        }

        void addFollic_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wait.Show();
                clsIVFDashboard_UpdateFollicularMonitoringBizActionVO BizAction = new clsIVFDashboard_UpdateFollicularMonitoringBizActionVO();
                BizAction.FollicularID = FollicularID;
                BizAction.FollicularMonitoringDetial = ((clsFollicularMonitoring)((ARTFollicularMonitoring)sender).DataContext);
                BizAction.FollicularMonitoringDetial.UnitID = FollicularUnitID;
                BizAction.FollicularMonitoringDetial.Date = ((ARTFollicularMonitoring)sender).dtpFollicularDate.SelectedDate.Value.Date;
                BizAction.FollicularMonitoringDetial.Date = BizAction.FollicularMonitoringDetial.Date.Value.Add(((ARTFollicularMonitoring)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.FollicularMonitoringDetial.AttachmentFileContent = ((ARTFollicularMonitoring)sender).AttachedFileContents;
                BizAction.FollicularMonitoringDetial.PhysicianID = ((MasterListItem)((ARTFollicularMonitoring)sender).cmbPhysician.SelectedItem).ID;
                BizAction.FollicularMonitoringDetial.FollicularNoList = string.Empty;
                BizAction.FollicularMonitoringDetial.LeftSizeList = string.Empty;
                BizAction.FollicularMonitoringDetial.RightSizeList = string.Empty;
                foreach (clsFollicularMonitoringSizeDetails item in BizAction.FollicularMonitoringDetial.SizeList)
                {
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.FollicularNoList))
                    {
                        BizAction.FollicularMonitoringDetial.FollicularNoList = item.FollicularNumber;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.FollicularNoList = BizAction.FollicularMonitoringDetial.FollicularNoList + "," + item.FollicularNumber;
                    }
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.LeftSizeList))
                    {
                        BizAction.FollicularMonitoringDetial.LeftSizeList = item.LeftSize;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.LeftSizeList = BizAction.FollicularMonitoringDetial.LeftSizeList + "," + item.LeftSize;
                    }
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.RightSizeList))
                    {
                        BizAction.FollicularMonitoringDetial.RightSizeList = item.RightSIze;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.RightSizeList = BizAction.FollicularMonitoringDetial.RightSizeList + "," + item.RightSIze;
                    }
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        wait.Close();
                        //setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 3);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular Monitoring Details Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        // setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }
        #endregion

        #region View CycleDetails
        bool IsPACChecked = false;
        bool IsConsentCheck = false;
        DateTime? TriggerDateForOPU = null;
        DateTime? TriggerTimeForOPU = null;

        private void cmdCycleDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgtheropyList.SelectedItem != null)
            {
                //by vikrant
                //GetPrevious diagnosis Patient aganist                
                FillDiagnosisType();
                //GetPatientDiagnosis();

                try
                {
                    SelectedTherapyDetails = new clsIVFDashboard_GetTherapyListBizActionVO();
                    objAnimation.Invoke(RotationType.Forward);
                    if (TabOverview != null)
                        TabOverview.IsSelected = true;
                    grdBackPanel.DataContext = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                    IVFPACServiceBilled();
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PACAnabled == true)
                    {
                        IsPACChecked = false;
                        cmbAnesthesist.IsEnabled = true;
                        // chkPACEnabled.IsChecked = true;
                    }
                    else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PACAnabled == false)
                    {
                        IsPACChecked = true;
                        cmbAnesthesist.IsEnabled = false;
                        //  chkPACEnabled.IsChecked = false;
                    }
                    cmbAnesthesist.SelectedValue = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).AnesthesistId;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).ConsentCheck == true)
                    {
                        IsConsentCheck = false;
                        chkConsentCheck.IsChecked = true;
                    }
                    else if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).ConsentCheck == false)
                    {
                        IsConsentCheck = true;
                        chkConsentCheck.IsChecked = false;
                    }
                    objSpermCollList = new List<MasterListItem>();
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 15)
                        objList = (from r in objList where r.ID != 11 select r).ToList();
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == 16)
                        objList = (from r in objList where r.ID != 4 select r).ToList();
                    cmbSpermCollection.ItemsSource = null;
                    cmbSpermCollection.ItemsSource = objList;
                    cmbSpermCollection.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedSpermCollectionID;

                    SelectedTherapyDetails.TherapyDetails = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                    startDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
                    cmbPhysician.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                    cmbAnesthesist.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).AnesthesistId;
                    cmbMainIndication.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).MainInductionID;
                    cmbPlannedTreatment.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                    cmbFinalPlanofTreatment.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).FinalPlannedTreatmentID;
                    txtPlannedNoOfEmbryos.Text = Convert.ToString(((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedEmbryos);
                    cmbProtocol.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).ProtocolTypeID;
                    cmbSimulation.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).ExternalSimulationID;
                    cmbSourceOfSperm.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).SpermSource;
                    dtOPUDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                    dtSpermCollDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).SpermCollectionDate;
                    dtStartDateOVarian.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartOvarian;
                    dtEndDateOvarian.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EndOvarian;
                    dtStartDateStimulation.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartStimulation;
                    dtEndDateOvarian.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).EndStimulation;
                    dtStartDateTrigger.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;
                    txtTrigTime.Value = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;
                    cmbSubMainIndication.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).MainSubInductionID; //added by neena
                    SrcOfSperm = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).SpermSource;
                    dtLutealStartDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).LutealStartDate;
                    dtLutealEndDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).LutealEndDate;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsCancellation == true)
                    {
                        chkCycleCancellation.IsChecked = true;
                        txtReason.IsEnabled = true;
                        txtReason.Text = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).CancellationReason;
                        chkCycleCancellation.IsEnabled = false;

                    }
                    else
                    {
                        chkCycleCancellation.IsChecked = false;
                        txtReason.IsEnabled = false;

                    }
                    txtTherapyNotes.Text = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).Note;

                    TriggerDateForOPU = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).StartTrigger;
                    TriggerTimeForOPU = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TriggerTime;

                    // CmdNewTheorpy.IsEnabled = false; //added by neena
                    if (SelectedTherapyDetails.TherapyDetails.IsClosed == false)
                        GetSavedConsent();
                    setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);

                    IsCancel = false;
                    if (chkPill.IsChecked == true)
                    {
                        dtStartDate.IsEnabled = true;
                        dtEndDate.IsEnabled = true;
                    }
                    else
                    {
                        dtStartDate.IsEnabled = false;
                        dtEndDate.IsEnabled = false;
                    }
                    if (SelectedTherapyDetails.TherapyDetails.IsClosed == true)
                    {
                        IsClosed = true;
                        cmdSaveTherapy.IsEnabled = false;
                        cmdNewExeDrug.IsEnabled = false;
                        Tabbirthdetails.IsEnabled = true;
                    }
                    else
                    {
                        IsClosed = false;
                        cmdSaveTherapy.IsEnabled = true;
                        cmdNewExeDrug.IsEnabled = true;
                        Tabbirthdetails.IsEnabled = false;
                    }
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsClosed)
                    {
                        cmdNewExeDrug.IsEnabled = false;
                    }
                    else { cmdNewExeDrug.IsEnabled = true; }
                    SetTabPlanTherapyWise((long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID);

                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsCancellation || ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsOPUCycleCancellation)
                    {
                        TabCryoPreservation.IsEnabled = false;
                        TabIUI.IsEnabled = false;
                        TabLutualPhase.IsEnabled = false;
                        TabGraphicalRepresentation.IsEnabled = false;
                        Tabtransfer.IsEnabled = false;
                        TabOnlyTransfer.IsEnabled = false;
                        TabOnlyVitrification.IsEnabled = false;
                        Tabbirthdetails.IsEnabled = false;
                        TabDocument.IsEnabled = false;
                        TabEmbryology.IsEnabled = false;
                        BHCG1.IsEnabled = false;
                        BHCG2.IsEnabled = false;
                        txtNoOfSacs.IsEnabled = false;
                        dtObservationDate.IsEnabled = false;
                        txtSacRemark.IsEnabled = false;
                    }
                    else
                    {
                        TabCryoPreservation.IsEnabled = true;
                        TabIUI.IsEnabled = true;
                        TabLutualPhase.IsEnabled = true;
                        TabGraphicalRepresentation.IsEnabled = true;
                        Tabtransfer.IsEnabled = true;
                        TabOnlyTransfer.IsEnabled = true;
                        TabOnlyVitrification.IsEnabled = true;
                        Tabbirthdetails.IsEnabled = true;
                        TabDocument.IsEnabled = true;
                        TabEmbryology.IsEnabled = true;
                        BHCG1.IsEnabled = true;
                        BHCG2.IsEnabled = true;
                        txtNoOfSacs.IsEnabled = true;
                        dtObservationDate.IsEnabled = true;
                        txtSacRemark.IsEnabled = true;
                    }

                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsCancellation)
                        TabOPU.IsEnabled = false;
                    else
                        TabOPU.IsEnabled = true;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUFreeze)
                    {
                        dtStartDateTrigger.IsEnabled = false;
                        txtTrigTime.IsEnabled = false;
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Media Click
        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID, CoupleDetails, "OnlyVitrification");
            Win.Show();
        }
        #endregion

        #region Patient Search
        // By BHUSHAN . . . .  For Patient Search.  .  . 
        private void btnPatientName_Click(object sender, RoutedEventArgs e)
        {
            //if (frmIUI != null && frmIUI.ISSavedIUIID == 0 && frmIUI.ThawedSampleID != "")
            if (frmIUI != null && frmIUI.IsSampleIDChanged)
            {
                MessageBoxControl.MessageBoxChildWindow msgWIUI =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                msgWIUI.Show();
                LoadIUI = true;
                TabIUI.IsSelected = true;
            }
            else
            {
                FrmPatientListForDashboard Win = new FrmPatientListForDashboard();
                //Win.PatientName = txtPatientName.Text;
                //Win.Mrno = txtMrno.Text;
                Win.SearchKeyword = txtPatientName.Text;
                // Win.PatientCategoryID = 7;
                Win.OnSaveButton_Click += new RoutedEventHandler(CoupleSearch_OnSaveButton_Click);
                Win.Show();
                txtMrno.Text = "";
                //txtPatientName.Text = "";
                frmIUI = null;
            }
        }

        private void txtMrno_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtMrno_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtPatientName_KeyDown(object sender, KeyEventArgs e)
        {
            // textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtPatientName_TextChanged(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            //{
            //    if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
            //    {
            //        ((AutoCompleteBox)sender).Text = textBefore;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}
        }

        private void btnSearchpatient_Click(object sender, RoutedEventArgs e)
        {
            frmCoupleSearch Win = new frmCoupleSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(CoupleSearch_OnSaveButton_Click);
            Win.Show();
        }

        #endregion

        #region Modify Therapy Details
        public bool GenValidation()
        {
            if (cmbPlannedTreatment.SelectedItem == null)
            {

                //CIMS.Extensions.SetValidation(cmbPlannedTreatment.TextBox, "Please Select Planned Treatment");
                //CIMS.Extensions.RaiseValidationError(cmbPlannedTreatment.TextBox);
                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 0)
            {

                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (cmbMainIndication.SelectedItem == null)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }

            else if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }
            else if (cmbPhysician.SelectedItem == null)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (((MasterListItem)cmbSourceOfSperm.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbSourceOfSperm.TextBox.SetValidation("Please Select Source of Sperm");
                cmbSourceOfSperm.TextBox.RaiseValidationError();
                cmbSourceOfSperm.Focus();
                return false;
            }
            else if (cmbSourceOfSperm.SelectedItem == null)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.ClearValidationError();
                cmbSourceOfSperm.TextBox.SetValidation("Please Select Source of Sperm");
                cmbSourceOfSperm.TextBox.RaiseValidationError();
                cmbSourceOfSperm.Focus();
                return false;
            }
            else if (((MasterListItem)cmbSourceOfSperm.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.TextBox.ClearValidationError();
                cmbMainIndication.TextBox.ClearValidationError();
                cmbSourceOfSperm.TextBox.SetValidation("Please Select Source of Sperm");
                cmbSourceOfSperm.TextBox.RaiseValidationError();
                cmbSourceOfSperm.Focus();
                return false;
            }
            //else if (dtOPUDate.SelectedDate == null)
            //{
            //    if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 1 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 2 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 3)
            //    {
            //        cmbPlannedTreatment.ClearValidationError();
            //        cmbMainIndication.ClearValidationError();
            //        cmbSourceOfSperm.ClearValidationError();
            //        dtOPUDate.SetValidation("Please Select OPU Date");
            //        dtOPUDate.RaiseValidationError();
            //        dtOPUDate.Focus();
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }


            //}
            else
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.ClearValidationError();
                cmbSourceOfSperm.ClearValidationError();
                dtOPUDate.ClearValidationError();
                return true;
            }
        }

        private void cmdSaveTherapy_Click(object sender, RoutedEventArgs e)
        {
            if (GenValidation())
            {
                if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == true)  //added by neena
                {
                    try
                    {
                        wait.Show();
                        clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = new clsIVFDashboard_GetManagementVisibleBizActionVO();
                        BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                        BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                        BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

                        //if (CoupleDetails.CoupleId == 0)
                        //    BizAction.TherapyDetails.IsDonorCycle = true;
                        //else
                        //    BizAction.TherapyDetails.IsDonorCycle = false;
                        BizAction.TherapyDetails.IsHalfBilled = true;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                double CheckPercentage = Convert.ToDouble(((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).ForTriggerProcedureBilling);
                                double CalPer = 0;
                                double BillAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillAmount;
                                double BillBalanceAmount = ((clsIVFDashboard_GetManagementVisibleBizActionVO)args.Result).TherapyDetails.BillBalanceAmount;

                                double PaidAmount = BillAmount - BillBalanceAmount;
                                CalPer = (PaidAmount / BillAmount) * 100;
                                if (dgTriggerDrugList.ItemsSource != null && dtStartDateTrigger.SelectedDate != null && Math.Round(CalPer, 2) < CheckPercentage) //50
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWBill =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Bill Amount is Pending", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWBill.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWBill_OnMessageBoxClosed);
                                    msgWBill.Show();

                                }
                                else
                                {
                                    wait.Close();
                                    string msgTitle = "Palash";
                                    string msgText = "Are You Sure \n  You Want To Modify Therapy General Details?";
                                    MessageBoxControl.MessageBoxChildWindow msgWin =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                    msgWin.OnMessageBoxClosed += (result1) =>
                                    {
                                        if (result1 == MessageBoxResult.Yes)
                                        {
                                            saveUpdateGeneralDetails(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, (long)IVFDashBoardTab.TabOverview, 0, "Therapy General Details Modified Successfully");
                                        }

                                    };
                                    msgWin.Show();
                                }
                                wait.Close();
                            }
                            else
                            {
                                wait.Close();
                            }
                            wait.Close();


                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        wait.Close();
                        throw ex;
                    }
                }
                else if (((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsIVFBillingCriteria == false)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Modify Therapy General Details?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result1) =>
                    {
                        if (result1 == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, (long)IVFDashBoardTab.TabOverview, 0, "Therapy General Details Modified Successfully");
                        }

                    };
                    msgWin.Show();
                }
            }
        }

        void msgWBill_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Modify Therapy General Details?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result1) =>
                {
                    if (result1 == MessageBoxResult.Yes)
                    {
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, (long)IVFDashBoardTab.TabOverview, 0, "Therapy General Details Modified Successfully");
                    }

                };
                msgWin.Show();
            }
        }

        void frmARTGeneralDetails_OnSaveButton_Click(object sender, EventArgs e)
        {
            //CmdNewTheorpy.IsEnabled = false; //added by neena
            setupPage(0, 0);
        }

        long SrcOfSperm = 0;
        private void saveUpdateGeneralDetails(long ID, long TabID, long DocumentId, String Msg)
        {
            try
            {
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails = ((clsPlanTherapyVO)grdBackPanel.DataContext);
                BizAction.TherapyDetails.ID = ID;
                BizAction.TherapyDetails.TabID = TabID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                BizAction.TherapyDetails.CoupleId = CoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = CoupleDetails.CoupleUnitId;

                BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;

                if (DiagnosisList != null)
                {
                    List<clsEMRAddDiagnosisVO> SavePatientDiagnosiList = new List<clsEMRAddDiagnosisVO>();
                    foreach (clsEMRAddDiagnosisVO item in DiagnosisList)
                    {
                        SavePatientDiagnosiList.Add(item);
                    }
                    BizAction.DiagnosisDetails = SavePatientDiagnosiList;
                }

                #region Data Used For Therapy General Details
                if (TabID == 1)
                {
                    //Added By Anumani

                    BizAction.TherapyDetails.OPUtDate = dtOPUDate.SelectedDate;
                    BizAction.TherapyDetails.StartOvarian = dtStartDateOVarian.SelectedDate;
                    BizAction.TherapyDetails.EndOvarian = dtEndDateOvarian.SelectedDate;
                    BizAction.TherapyDetails.StartStimulation = dtStartDateStimulation.SelectedDate;
                    BizAction.TherapyDetails.EndStimulation = dtEndDateStimulation.SelectedDate;
                    BizAction.TherapyDetails.StartTrigger = dtStartDateTrigger.SelectedDate;
                    BizAction.TherapyDetails.TriggerTime = txtTrigTime.Value;
                    BizAction.TherapyDetails.CancellationReason = txtReason.Text;
                    BizAction.TherapyDetails.Note = txtTherapyNotes.Text;
                    BizAction.TherapyDetails.SpermCollectionDate = dtSpermCollDate.SelectedDate;

                    BizAction.TherapyDetails.OPUtDate = dtOPUDate.SelectedDate;

                    BizAction.TherapyDetails.LutealStartDate = dtLutealStartDate.SelectedDate;
                    BizAction.TherapyDetails.LutealEndDate = dtLutealEndDate.SelectedDate;

                    if (cmbPhysician.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PhysicianId = ((MasterListItem)cmbPhysician.SelectedItem).ID;

                    }
                    if (cmbMainIndication.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.MainInductionID = ((MasterListItem)cmbMainIndication.SelectedItem).ID;
                    }
                    //added by neena
                    if (cmbAnesthesist.SelectedItem != null)
                        BizAction.TherapyDetails.AnesthesistId = ((MasterListItem)cmbAnesthesist.SelectedItem).ID;

                    if (cmbSubMainIndication.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.MainSubInductionID = ((MasterListItem)cmbSubMainIndication.SelectedItem).ID;
                    }
                    //
                    if (cmbPlannedTreatment.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedTreatmentID = ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID;
                    }
                    //if (cmbPlannedTreatment.SelectedItem != null)
                    //{
                    //    BizAction.TherapyDetails.FinalPlannedTreatmentID = ((MasterListItem)cmbFinalPlanofTreatment.SelectedItem).ID;
                    //}
                    if (cmbProtocol.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ProtocolTypeID = ((MasterListItem)cmbProtocol.SelectedItem).ID;
                    }
                    if (txtPlannedNoOfEmbryos.Text != null)
                    {
                        BizAction.TherapyDetails.PlannedEmbryos = txtPlannedNoOfEmbryos.Text.Trim();
                    }
                    if (txtLongtermMedication.Text != null)
                    {
                        BizAction.TherapyDetails.LongtermMedication = txtLongtermMedication.Text.Trim();
                    }
                    if (cmbSimulation.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ExternalSimulationID = ((MasterListItem)cmbSimulation.SelectedItem).ID;
                    }
                    if (cmbSpermCollection.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedSpermCollectionID = ((MasterListItem)cmbSpermCollection.SelectedItem).ID;
                    }
                    // Added by Anumani
                    if (chkCycleCancellation.IsChecked == true)
                    {
                        BizAction.TherapyDetails.IsCancellation = true;
                    }
                    else
                    {
                        BizAction.TherapyDetails.IsCancellation = false;
                    }
                    if (cmbSpermCollection.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedSpermCollectionID = ((MasterListItem)cmbSpermCollection.SelectedItem).ID;
                    }

                    if (cmbSourceOfSperm.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.SpermSource = ((MasterListItem)cmbSourceOfSperm.SelectedItem).ID;
                    }
                }
                #endregion

                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", Msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        msgW1.Show();

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
                setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
            //throw new NotImplementedException();
        }
        #endregion

        #region Search therapy
        private void btnSearchTherapy_Click(object sender, RoutedEventArgs e)
        {
            //CmdNewTheorpy.IsEnabled = false; //added by neena
            setupPage(0, 0);
        }
        #endregion

        #region outcome
        private List<clsIVFDashboard_OutcomeVO> _OutcomePregnancySacList = new List<clsIVFDashboard_OutcomeVO>();
        public List<clsIVFDashboard_OutcomeVO> OutcomePregnancySacList
        {
            get
            {
                return _OutcomePregnancySacList;
            }
            set
            {
                _OutcomePregnancySacList = value;
            }
        }

        bool blSacsNo = false;
        long FetalHeartCount = 0;
        private void fillOutcomeDetails()
        {
            clsIVFDashboard_GetOutcomeBizActionVO BizAction = new clsIVFDashboard_GetOutcomeBizActionVO();
            BizAction.Details = new clsIVFDashboard_OutcomeVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

            BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;

            //if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            //}
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details;
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList.Count > 0)
                            OutcomePregnancySacList = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).OutcomePregnancySacList;

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            BHCGAss1Date.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            BHCGAss2Date.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtpPregnancy.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtFetalDate.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BabyTypeID != null)
                        {
                            cmbBabyType.SelectedValue = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BabyTypeID;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1IsBSCG == true)
                        {
                            rdoBSCGAss1Positve.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1IsBSCG == false)
                        {
                            rdoBSCGAss1Negative.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2IsBSCG == true)
                        {
                            rdoBSCGAss2Positve.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2IsBSCG == false)
                        {
                            rdoBSCGAss2Negative.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsPregnancyAchieved == true)
                        {
                            rdoPregnancyAchievedYes.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsPregnancyAchieved == false)
                        {
                            rdoPregnancyAchievedNo.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsClosed == true)
                        {
                            chkIsclosed.IsChecked = true;
                            cmdSaveOut.IsEnabled = false;
                            txtNoOfSacs.IsEnabled = false;
                            Tabbirthdetails.IsEnabled = true;
                        }
                        else
                        {
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OocyteDonationID)
                                txtNoOfSacs.IsEnabled = false;
                            if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsCancellation || ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsOPUCycleCancellation)
                                txtNoOfSacs.IsEnabled = false;
                            else
                            {
                                chkIsclosed.IsChecked = false;
                                cmdSaveOut.IsEnabled = true;
                                txtNoOfSacs.IsEnabled = true;
                                Tabbirthdetails.IsEnabled = false;
                            }
                        }

                        //added by neena
                        PregnancySacsDetails = new ObservableCollection<clsPregnancySacsDetailsVO>();
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.NoOfSacs > 0)
                        {
                            blSacsNo = true;
                            txtNoOfSacs.Text = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.NoOfSacs.ToString();
                            txtSacRemark.Text = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.SacRemarks;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.SacsObservationDate != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.SacsObservationDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.SacsObservationDate;
                        }

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnancyAchievedID != null)
                        {
                            cmbPregnancyAchieved.SelectedValue = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnancyAchievedID;
                        }

                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList.Count > 0)
                        {
                            for (int i = 0; i < ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList.Count; i++)
                            {
                                ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].OutcomeResultListVO = OutcomeResultList;
                                ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].OutcomePregnancyListVO = OutcomePregnancyList;

                                if ((((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].ResultListID) >= 0)
                                {
                                    if (Convert.ToInt64(((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].ResultListID) > 0)
                                    {
                                        ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].ResultListID));
                                    }
                                    else
                                    {
                                        ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].selectedResultList = OutcomeResultList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].PregnancyListID) >= 0)
                                {
                                    if (Convert.ToInt64(((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].PregnancyListID) > 0)
                                    {
                                        ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].PregnancyListID));
                                    }
                                    else
                                    {
                                        ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].selectedPregnancyList = OutcomePregnancyList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].CongenitalAbnormalityYes == true)
                                {
                                    ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].Status = true;
                                }
                                else
                                    ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i].Status = false;

                                PregnancySacsDetails.Add(((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).PregnancySacsList[i]);
                            }
                            dgSacsDetailsGrid.ItemsSource = PregnancySacsDetails;
                            dgSacsDetailsGrid.UpdateLayout();
                            blSacsNo = false;
                        }


                        //

                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool saveDtls = true;
            saveDtls = validateOutcome();
            if (saveDtls == true)
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
        }
        private void SaveOutcome()
        {
            clsIVFDashboard_AddUpdateOutcomeBizActionVO BizAction = new clsIVFDashboard_AddUpdateOutcomeBizActionVO();
            BizAction.OutcomeDetails = new clsIVFDashboard_OutcomeVO();
            BizAction.OutcomeDetails.ID = ((clsIVFDashboard_OutcomeVO)this.DataContext).ID;
            BizAction.OutcomeDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.OutcomeDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;

            BizAction.OutcomeDetails.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.OutcomeDetails.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;

            //if (CoupleDetails.CoupleId == 0)
            //    BizAction.OutcomeDetails.IsDonorCycle = true;
            //else
            //    BizAction.OutcomeDetails.IsDonorCycle = false;

            if (rdoBSCGAss1Negative.IsChecked != null)
            {
                if (rdoBSCGAss1Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = false;
                }
            }
            if (rdoBSCGAss1Positve.IsChecked != null)
            {
                if (rdoBSCGAss1Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = true;
                }
            }
            if (rdoBSCGAss2Negative.IsChecked != null)
            {
                if (rdoBSCGAss2Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = false;
                }
            }
            if (rdoBSCGAss2Positve.IsChecked != null)
            {
                if (rdoBSCGAss2Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = true;
                }
            }
            if (rdoPregnancyAchievedNo.IsChecked != null)
            {
                if (rdoPregnancyAchievedNo.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = false;
                }
            }
            if (rdoPregnancyAchievedYes.IsChecked != null)
            {
                if (rdoPregnancyAchievedYes.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = true;
                }
            }

            if (cmbBabyType.SelectedItem != null)
            {
                BizAction.OutcomeDetails.BabyTypeID = ((MasterListItem)cmbBabyType.SelectedItem).ID;
            }

            if (txtOHSSremark.Text != null && txtOHSSremark.Text != string.Empty)
            {
                BizAction.OutcomeDetails.OHSSRemark = txtOHSSremark.Text.Trim();
            }
            if (txtOutComeRemarks.Text != null && txtOutComeRemarks.Text != string.Empty)
            {
                BizAction.OutcomeDetails.OutComeRemarks = txtOutComeRemarks.Text.Trim();
            }

            if (txtUSG.Text != null && txtUSG.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2USG = txtUSG.Text.Trim();
            }

            if (txtCount.Text != null && txtCount.Text != string.Empty)
            {
                BizAction.OutcomeDetails.Count = txtCount.Text.Trim();
            }

            if (txtOutβhCGValue.Text != null && txtOutβhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss1BSCGValue = txtOutβhCGValue.Text.Trim();
            }
            if (txtSrProgest.Text != null && txtSrProgest.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss1SrProgest = txtSrProgest.Text.Trim();
            }
            if (txtOutAssment2βhCGValue.Text != null && txtOutAssment2βhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2BSCGValue = txtOutAssment2βhCGValue.Text.Trim();
            }
            if (txtOutAssment2βhCGValue.Text != null && txtOutAssment2βhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2BSCGValue = txtOutAssment2βhCGValue.Text.Trim();
            }
            if (dtFetalDate.SelectedDate != null)
            {
                BizAction.OutcomeDetails.FetalDate = dtFetalDate.SelectedDate;
            }
            if (BHCGAss1Date.SelectedDate != null)
            {
                BizAction.OutcomeDetails.BHCGAss1Date = BHCGAss1Date.SelectedDate;
            }
            if (BHCGAss2Date.SelectedDate != null)
            {
                BizAction.OutcomeDetails.BHCGAss2Date = BHCGAss2Date.SelectedDate;
            }
            if (dtpPregnancy.SelectedDate != null)
            {
                BizAction.OutcomeDetails.PregnanacyConfirmDate = dtpPregnancy.SelectedDate;
            }
            if (chkOHSSEarly.IsChecked != null)
            {
                if (chkOHSSEarly.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSEarly = true;
                }
            }
            if (chkOHSSLate.IsChecked != null)
            {
                if (chkOHSSLate.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSLate = true;
                }
            }
            if (chkChemicalPregancy.IsChecked != null)
            {
                if (chkChemicalPregancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsChemicalPregnancy = true;
                }
            }
            if (chkPreTermPregnancy.IsChecked != null)
            {
                if (chkPreTermPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.PretermDelivery = true;
                }
            }
            if (chkFullTermPregnancy.IsChecked != null)
            {
                if (chkFullTermPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsFullTermDelivery = true;
                }
            }
            if (chkOHSSLate.IsChecked != null)
            {
                if (chkOHSSLate.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSLate = true;
                }
            }
            if (chkOHSSmild.IsChecked != null)
            {
                if (chkOHSSmild.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSMild = true;
                }
            }
            if (chkOHSSmod.IsChecked != null)
            {
                if (chkOHSSmod.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSMode = true;
                }
            }
            if (chkOHSSsevere.IsChecked != null)
            {
                if (chkOHSSsevere.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSSereve = true;
                }
            }
            if (rdoBSCGAss1Negative.IsChecked != null)
            {
                if (rdoBSCGAss1Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = false;
                }
            }
            if (rdoBSCGAss1Positve.IsChecked != null)
            {
                if (rdoBSCGAss1Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = true;
                }
            }
            if (rdoBSCGAss2Negative.IsChecked != null)
            {
                if (rdoBSCGAss2Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = false;
                }
            }
            if (rdoBSCGAss2Positve.IsChecked != null)
            {
                if (rdoBSCGAss2Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = true;
                }
            }
            if (rdoPregnancyAchievedNo.IsChecked != null)
            {
                if (rdoPregnancyAchievedNo.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = false;
                }
            }
            if (rdoPregnancyAchievedYes.IsChecked != null)
            {
                if (rdoPregnancyAchievedYes.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = true;
                }
            }
            if (chkOHSSsevere.IsChecked != null)
            {
                if (chkOHSSsevere.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSSereve = true;
                }
            }
            if (BiochemPregnancy.IsChecked != null)
            {
                if (BiochemPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BiochemPregnancy = true;
                }
            }
            if (Abortion.IsChecked != null)
            {
                if (Abortion.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Abortion = true;
                }
            }
            if (FetalHeartSound.IsChecked != null)
            {
                if (FetalHeartSound.IsChecked == true)
                {
                    BizAction.OutcomeDetails.FetalHeartSound = true;
                }
            }
            if (IUD.IsChecked != null)
            {
                if (IUD.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IUD = true;
                }
            }
            if (LiveBirth.IsChecked != null)
            {
                if (LiveBirth.IsChecked == true)
                {
                    BizAction.OutcomeDetails.LiveBirth = true;
                }
            }
            if (Congenitalabnormality.IsChecked != null)
            {
                if (Congenitalabnormality.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Congenitalabnormality = true;
                }
            }
            if (Ectopic.IsChecked != null)
            {
                if (Ectopic.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Ectopic = true;
                }
            }
            if (chkMissed.IsChecked != null)
            {
                if (chkMissed.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Missed = true;
                }
            }
            if (chkIncomplete.IsChecked != null)
            {
                if (chkIncomplete.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Incomplete = true;
                }
            }
            if (chkIsclosed.IsChecked != null)
            {
                if (chkIsclosed.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsClosed = true;
                }
            }

            //added by neena
            if (txtNoOfSacs.Text != null)
            {
                BizAction.OutcomeDetails.NoOfSacs = Convert.ToInt64(txtNoOfSacs.Text.Trim());
            }

            BizAction.OutcomeDetails.SacRemarks = txtSacRemark.Text;

            if (dtObservationDate.SelectedDate != null)
            {
                BizAction.OutcomeDetails.SacsObservationDate = dtObservationDate.SelectedDate;
            }

            if (PregnancySacsDetails.Count > 0)
            {
                BizAction.PregnancySacsList = PregnancySacsDetails.ToList();
            }

            if (cmbPregnancyAchieved.SelectedItem != null)
            {
                BizAction.OutcomeDetails.PregnancyAchievedID = ((MasterListItem)cmbPregnancyAchieved.SelectedItem).ID;
            }

            for (int i = 0; i < PregnancySacsDetails.Count; i++)
            {
                if (PregnancySacsDetails[i].ID != 0)
                {
                    BizAction.PregnancySacsList[i].ID = PregnancySacsDetails[i].ID;
                }
                else
                {
                    BizAction.PregnancySacsList[i].ID = 0;
                }
                BizAction.PregnancySacsList[i].ResultListID = PregnancySacsDetails[i].selectedResultList.ID;
                BizAction.PregnancySacsList[i].PregnancyListID = PregnancySacsDetails[i].selectedPregnancyList.ID;

            }

            BizAction.OutcomePregnancySacList = OutcomePregnancySacList;

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
            if (rdoPregnancyAchievedYes.IsChecked == true)
            {
                if (string.IsNullOrEmpty(dtpPregnancy.Text))
                {
                    dtpPregnancy.SetValidation("Pregnancy Confirm Date Is Required");
                    dtpPregnancy.RaiseValidationError();
                    dtpPregnancy.Focus();
                    result = false;
                }
            }
            else
            {
                dtpPregnancy.ClearValidationError();
            }
            return result;
        }
        #endregion

        #region Luteal Phase
        private void fillLutealPhaseDetails()
        {
            clsIVFDashboard_GetLutealPhaseBizActionVO BizAction = new clsIVFDashboard_GetLutealPhaseBizActionVO();
            BizAction.Details = new cls_IVFDashboard_LutualPhaseVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
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
                    if (((clsIVFDashboard_GetLutealPhaseBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetLutealPhaseBizActionVO)arg.Result).Details;
                        if (IsClosed == true)
                        {
                            cmdNewLP.IsEnabled = false;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdNewLP_Click(object sender, RoutedEventArgs e)
        {
            if (txtLutealSupport.Text != string.Empty || txtLutealRemarks.Text != string.Empty)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter the details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
            clsIVFDashboard_AddUpdateLutealPhaseBizActionVO BizAction = new clsIVFDashboard_AddUpdateLutealPhaseBizActionVO();
            BizAction.LutualPhaseDetails = new cls_IVFDashboard_LutualPhaseVO();
            BizAction.LutualPhaseDetails.ID = ((cls_IVFDashboard_LutualPhaseVO)this.DataContext).ID;
            BizAction.LutualPhaseDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
            BizAction.LutualPhaseDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            BizAction.LutualPhaseDetails.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.LutualPhaseDetails.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            BizAction.LutualPhaseDetails.LutealSupport = txtLutealSupport.Text;
            BizAction.LutualPhaseDetails.LutealRemark = txtLutealRemarks.Text;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Luteal Phase Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
        #endregion

        #region events for Only Vitrification
        private void chkUseownoocyte_Click(object sender, RoutedEventArgs e)
        {
            if (chkUseownoocyte.IsChecked == true)
            {
                GetEmbfromVitrification();
            }
            else
            {
                GetEmbfromVitrificationWithoutUsingOwnOocyte();
            }
        }

        private void GetEmbfromVitrificationWithoutUsingOwnOocyte()
        {
            try
            {
                clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;
                BizAction.VitrificationMain.UsedOwnOocyte = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
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

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;

                        wait.Close();
                        if (VitriDetails.Count == 0)
                        {
                            fillInitailOnlyVitrificationDetails();
                        }
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

        private void GetEmbfromVitrification()
        {
            try
            {
                clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;
                BizAction.VitrificationMain.UsedOwnOocyte = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
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

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
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

        private bool ValidateOnlyVtri()
        {
            if (dtonlyVitrificationDate.SelectedDate == null)
            {
                dtonlyVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtonlyVitrificationDate.RaiseValidationError();
                dtonlyVitrificationDate.Focus();
                return false;
            }
            else if (txtonlyVitTime.Value == null)
            {
                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.SetValidation("Please Select Vitrification Time");
                txtonlyVitTime.RaiseValidationError();
                txtonlyVitTime.Focus();
                return false;
            }
            else if (VitriDetails.Count <= 0)
            {

                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.ClearValidationError();
                dtonlyvitPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.ClearValidationError();
                dtonlyvitPickUpDate.ClearValidationError();
                return true;
            }
        }

        private void cmdSaveOnlyVit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateOnlyVtri())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedonlyVitri);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosedonlyVitri(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveOnlyVtri();
        }

        private void SaveOnlyVtri()
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            BizAction.VitrificationMain.DateTime = dtonlyVitrificationDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtonlyVitTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.PickUpDate = dtonlyvitPickUpDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.IsOnlyVitrification = true;

            for (int i = 0; i < VitriDetails.Count; i++)
            {
                VitriDetails[i].CanId = VitriDetails[i].SelectedCanId.ID;
                VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID;
                VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID;
                VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID;
                VitriDetails[i].ConistorNo = VitriDetails[i].SelectedCanisterId.ID;
                VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID;
                VitriDetails[i].ColorCodeID = VitriDetails[i].SelectedColorSelector.ID;
                VitriDetails[i].GradeID = VitriDetails[i].SelectedGrade.ID;
                VitriDetails[i].CellStageID = VitriDetails[i].SelectedCellStage.ID;
                VitriDetails[i].EmbNumber = i + 1;
            }
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetails.ToList());
            if (chkonlyvitFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;

            if (rdoYes.IsChecked == true)
                BizAction.VitrificationMain.ConsentForm = true;
            else
                BizAction.VitrificationMain.ConsentForm = false;
            if (chkUseownoocyte.IsChecked == true)
                BizAction.VitrificationMain.UsedOwnOocyte = true;
            else
                BizAction.VitrificationMain.UsedOwnOocyte = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillOnlyVitrificationDetails();
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

        public void fillOnlyVitrificationDetails()
        {
            try
            {
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails.Clear();
                        this.DataContext = null;
                        this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdSaveOnlyVit.IsEnabled = false;
                            chkUseownoocyte.IsEnabled = false;
                        }
                        if (IsClosed == true)
                        {
                            cmdSaveOnlyVit.IsEnabled = false;
                        }
                        chkonlyvitFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtonlyVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtonlyVitTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtonlyVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.PickUpDate != null)
                        {
                            dtonlyvitPickUpDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.PickUpDate;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm == true)
                        {
                            rdoYes.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm == false)
                        {
                            rdoNo.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.UsedOwnOocyte == true)
                        {
                            chkUseownoocyte.IsChecked = true;
                        }
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
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

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                            }

                            VitriDetails.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                        }

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;

                        if (VitriDetails.Count == 0)
                        {
                            fillInitailOnlyVitrificationDetails();
                        }

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

        public void fillInitailOnlyVitrificationDetails()
        {
            VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
            VitriDetails.Add(new clsIVFDashBoard_VitrificationDetailsVO() { CanId = 0, StrawId = 0, GobletShapeId = 0, ColorCodeID = 0, GobletSizeId = 0, ConistorNo = 0, TankId = 0, CanIdList = CanList, StrawIdList = Straw, GobletSizeList = GobletSize, GobletShapeList = GobletShape, CanisterIdList = Canister, TankList = Tank, ColorSelectorList = GobletColor, EmbNumber = 0, GradeList = Grade, CellStageList = CellStage });
            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
            wait.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (VitriDetails == null)
            {
                VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
            }
            VitriDetails.Add(new clsIVFDashBoard_VitrificationDetailsVO() { CanId = 0, StrawId = 0, GobletShapeId = 0, ColorCodeID = 0, GobletSizeId = 0, ConistorNo = 0, TankId = 0, CanIdList = CanList, StrawIdList = Straw, GobletSizeList = GobletSize, GobletShapeList = GobletShape, CanisterIdList = Canister, TankList = Tank, ColorSelectorList = GobletColor, EmbNumber = 0, GradeList = Grade, CellStageList = CellStage });

            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgonlyVitrificationDetilsGrid.SelectedItem != null)
            {
                VitriDetails.RemoveAt(dgonlyVitrificationDetilsGrid.SelectedIndex);
            }
            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }
        #endregion

        #region Bill History
        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            billID = ((OPDModule.frmBillListPatientWise)(sender)).BillID;
            billUnitID = ((OPDModule.frmBillListPatientWise)(sender)).BillUnitID;
            DateTime billDate = ((OPDModule.frmBillListPatientWise)(sender)).BillDate;
            billno = ((OPDModule.frmBillListPatientWise)(sender)).BillNo;
        }
        private void CmdBillHistory_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                long patientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                frmBillListPatientWise serviceSearch = null;
                serviceSearch = new frmBillListPatientWise(patientID, PatientUnitID);
                serviceSearch.Show();
                serviceSearch.OnSaveButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            }
            else
            {
                msgText = "Please Select The Patient";
                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
            }
        }
        #endregion

        #region New Therapy Click
        private void CmdNewTheorpy_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails != null)
            {
                if (CoupleDetails.FemalePatient.PatientID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
                else
                {
                    frmARTGeneralDetails objARTdetails = new frmARTGeneralDetails(CoupleDetails);
                    objARTdetails.SelectedPlanTreatmentID = SelectedPlanTreatmentID;
                    objARTdetails.EMRProcedureID = EMRProcedureID;
                    objARTdetails.EMRProcedureUnitID = EMRProcedureUnitID;
                    objARTdetails.IsDonorCycle = IsDonorCycle;
                    objARTdetails.IsSurrogacy = IsSurrogacy;

                    objARTdetails.Show();
                    objARTdetails.Closed += new EventHandler(frmARTGeneralDetails_OnSaveButton_Click);
                    // objARTdetails.OnCancelButton_Click += new RoutedEventHandler(objARTdetails_OnCancelButton_Click);
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW2.Show();
            }
        }

        //void objARTdetails_OnCancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    CmdNewTheorpy.IsEnabled = true;
        //    //throw new NotImplementedException();
        //}
        #endregion

        #region checkbox click

        private void ChkAbortion_Click(object sender, RoutedEventArgs e)
        {
            chkMissed.Visibility = Visibility.Visible;
            chkIncomplete.Visibility = Visibility.Visible;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            chkMissed.Visibility = Visibility.Collapsed;
            chkIncomplete.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Radio Button Click Event
        private void rdoActive_Click(object sender, RoutedEventArgs e)
        {
            if (rdoActive.IsChecked == true)
            {
                rdoAll.IsChecked = false;
                rdoUnsuccessful.IsChecked = false;
                rdoSuccessful.IsChecked = false;
                rdoClosed.IsChecked = false;
            }
        }

        private void rdoClosed_Click(object sender, RoutedEventArgs e)
        {
            if (rdoClosed.IsChecked == true)
            {
                rdoAll.IsChecked = false;
                rdoUnsuccessful.IsChecked = false;
                rdoSuccessful.IsChecked = false;
                rdoActive.IsChecked = false;
            }
            if (rdoUnsuccessful.IsChecked == false && rdoSuccessful.IsChecked == false && rdoClosed.IsChecked == false && rdoActive.IsChecked == false)
            {
                rdoAll.IsChecked = true;
            }
        }

        private void rdoSuccessful_Click(object sender, RoutedEventArgs e)
        {
            if (rdoSuccessful.IsChecked == true)
            {
                rdoAll.IsChecked = false;
                rdoUnsuccessful.IsChecked = false;
                rdoClosed.IsChecked = false;
                rdoActive.IsChecked = false;
            }
            if (rdoUnsuccessful.IsChecked == false && rdoSuccessful.IsChecked == false && rdoClosed.IsChecked == false && rdoActive.IsChecked == false)
            {
                rdoAll.IsChecked = true;
            }
        }

        private void rdoUnsuccessful_Click(object sender, RoutedEventArgs e)
        {
            if (rdoUnsuccessful.IsChecked == true)
            {
                rdoAll.IsChecked = false;
                rdoSuccessful.IsChecked = false;
                rdoClosed.IsChecked = false;
                rdoActive.IsChecked = false;
            }
            if (rdoUnsuccessful.IsChecked == false && rdoSuccessful.IsChecked == false && rdoClosed.IsChecked == false && rdoActive.IsChecked == false)
            {
                rdoAll.IsChecked = true;
            }
        }

        private void rdoAll_Click(object sender, RoutedEventArgs e)
        {
            if (rdoAll.IsChecked == true)
            {
                rdoUnsuccessful.IsChecked = false;
                rdoSuccessful.IsChecked = false;
                rdoClosed.IsChecked = false;
                rdoActive.IsChecked = false;
            }
            if (rdoUnsuccessful.IsChecked == false && rdoSuccessful.IsChecked == false && rdoClosed.IsChecked == false && rdoActive.IsChecked == false)
            {
                rdoAll.IsChecked = true;
            }
        }


        #endregion

        #region pill check uncheck event
        private void chkPill_Checked(object sender, RoutedEventArgs e)
        {
            if (chkPill.IsChecked == true)
            {
                dtStartDate.IsEnabled = true;
                dtEndDate.IsEnabled = true;
            }
        }

        private void chkPill_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkPill.IsChecked == false)
            {
                dtStartDate.IsEnabled = false;
                dtEndDate.IsEnabled = false;
            }
        }
        #endregion

        #region Cancel event
        private void CmdCancleMain_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    objAnimation.Invoke(RotationType.Backward);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            if (IsCancel == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
            else
            {
                //if (frmIUI != null && frmIUI.ISSavedIUIID == 0 && frmIUI.ThawedSampleID != "")
                if (frmIUI != null && frmIUI.IsSampleIDChanged)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWIUI =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWIUI.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWIUI_OnMessageBoxClosed);
                    msgWIUI.Show();
                    LoadIUI = true;
                    TabIUI.IsSelected = true;
                }
                else
                {
                    objAnimation.Invoke(RotationType.Backward);
                    IsCancel = true;
                    //CmdNewTheorpy.IsEnabled = false;  //added by neena
                    setupPage(0, 0);
                    //IVFServiceBilled();
                    frmIUI = null;
                }
            }
        }
        #endregion

        #region New Drug
        private void cmdNewExeDrug_Click(object sender, RoutedEventArgs e)
        {
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click += new RoutedEventHandler(addDrug_OnSaveButton_Click);
            addDrug.Show();
        }

        public void addDrug_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((AddDrug)sender).IsEdit == false)
            {
                var result = from c in SelectedTherapyDetails.TherapyExecutionList
                             where c.ThearpyTypeDetailId == ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID
                             select c;
                if (result.ToList().Count == 0)
                {
                    string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                    string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                    clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    BizAction.TherapyDetails = new clsPlanTherapyVO();
                    BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    BizAction.TherapyDetails.ThreapyExecutionId = 0;
                    BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                    BizAction.TherapyDetails.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                    BizAction.TherapyDetails.PhysicianId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                    BizAction.TherapyDetails.TabID = 2;
                    BizAction.TherapyDetails.Day = kEY;
                    BizAction.TherapyDetails.Value = VALUE;
                    BizAction.TherapyDetails.DrugTime = ((AddDrug)sender).dtthreapystartdate.SelectedDate.Value;
                    BizAction.TherapyDetails.DrugTime = BizAction.TherapyDetails.DrugTime.Value.Add(((AddDrug)sender).txtTime.Value.Value.TimeOfDay);
                    BizAction.TherapyDetails.DrugNotes = ((AddDrug)sender).txtDrugNotes.Text.Trim();
                    BizAction.TherapyDetails.ThearpyTypeDetailId = ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID;
                    #region Service Call (Check Validation)

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            //CmdNewTheorpy.IsEnabled = false; //added by neena
                            setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();

                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                    #endregion
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                }
            }
            else if (((AddDrug)sender).IsEdit == true)
            {
                string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                BizAction.TherapyDetails.ThreapyExecutionId = ((AddDrug)sender).TherpayExeId;
                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                BizAction.TherapyDetails.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                BizAction.TherapyDetails.TabID = 2;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;

                BizAction.TherapyDetails.DrugTime = ((AddDrug)sender).dtthreapystartdate.SelectedDate.Value.Date;
                BizAction.TherapyDetails.DrugTime = BizAction.TherapyDetails.DrugTime.Value.Add(((AddDrug)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.TherapyDetails.DrugNotes = ((AddDrug)sender).txtDrugNotes.Text.Trim();
                BizAction.TherapyDetails.ThearpyTypeDetailId = ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //CmdNewTheorpy.IsEnabled = false; //added by neena
                        setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();


                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
        }
        #endregion

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void CoupleSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            fillCoupleDetails();

            if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientTypeID == 8 || ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientTypeID == 9)
            {
                CmdDoonarReport.Visibility = Visibility.Visible;
            }
            else
                CmdDoonarReport.Visibility = Visibility.Collapsed;
        }

        private void TherpayExeRowDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdmaleFinding_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmddonorDetails_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails_DashBoard win = new SemenDetails_DashBoard();
            win.SourceOfSperm = SrcOfSperm;
            win.coupleDetails = CoupleDetails;
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            win.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            win.PlannedtreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
            win.SelectedTherapyDetails = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
            win.IsClosed = IsClosed;
            win.Save_ChildClick += new RoutedEventHandler(SaveDonorChild_Click);
            win.Show();

        }
        private void SaveDonorChild_Click(object sender, RoutedEventArgs e)
        {
            //CmdNewTheorpy.IsEnabled = false; //added by neena
            setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
        }
        private void CmdImages_Click(object sender, RoutedEventArgs e)
        {
            frmSurgeryImages win = new frmSurgeryImages();
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            win.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            win.PlannedtreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
            win.SelectedTherapyDetails = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
            win.Show();

        }

        private void txtOutβhCGValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
                {
                    if (textBefore != null)
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

        private void txtOutβhCGValue_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtCount_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbSpermCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpermCollection.SelectedItem == null)
            {
                cmbSpermCollection.SelectedValue = (long)0;
            }
        }

        private void cmbSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSimulation.SelectedItem == null)
            {
                cmbSimulation.SelectedValue = (long)0;
            }
        }

        private void cmbProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProtocol.SelectedItem == null)
            {
                cmbProtocol.SelectedValue = (long)0;
            }
        }
        long TherapyId, TherapyUnitId;
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgtheropyList.SelectedItem != null)
            {
                TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            }
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID > 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_PatientARTReport.aspx?TherapyId=" + TherapyId + "&TherapyUnitId=" + TherapyUnitId + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&PlannedTreatmentID=" + ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        //..................................................................................
        List<clsMenuVO> MenuList = new List<clsMenuVO>();
        private List<TabItem> tabItems = new List<TabItem>();
        //TabControl tb =new TabControl();

        //private TabItem _tabAdd;   
        public void RequestXML(string Parent)
        {
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList.Count > 0)
            {
                var i1 = tabControl1.Items;

                MenuList = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList;
                var Menus = from r in MenuList
                            where r.Parent == Parent && r.Status == true
                            orderby r.MenuOrder
                            select r;

                foreach (var s in tabItems)
                {
                    if (s.Name != "TabOverview" || s.Name != "TabDocument" || s.Name != "TabLutualPhase" || s.Name != "Taboutcome")
                    {
                        var h = from u in Menus
                                where u.Title == s.Name
                                select u;
                        if (h.ToList().Count > 0)
                        {
                            s.Visibility = Visibility.Visible;
                        }

                    }
                }
            }
        }

        private void cmdFemaleEmbryoBank_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient.PatientID != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                frmEmbryoBankNew win = new frmEmbryoBankNew();
                //frmEmbryoBank win = new frmEmbryoBank();
                win.MRNO = CoupleDetails.FemalePatient.MRNo;
                win.Title = "Embryo Bank ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                win.Show();
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void cmdMaleEmbryoBank_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient.PatientID != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                frmSpermBank win = new frmSpermBank();
                win.MRNO = CoupleDetails.MalePatient.MRNo;
                win.Title = "Sprem Bank ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                win.Show();
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void txtPatientName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
            }
        }

        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsPatientConsentVO> DataListConsent { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        public void GetData()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

            BizActionObject.VisitWise = false;

            //BizActionObject.PatientCategoryID = 7;

            BizActionObject.SearchKeyword = txtPatientName.Text;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
            }
            else
            {
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizActionObject.IsPagingEnabled = true;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            DataList.Add(person);
                        }
                        if (DataList.Count != null || DataList.Count > 0)
                        {
                            if (DataList.Count == 1)
                            {
                                ((IApplicationConfiguration)App.Current).SelectedPatient = DataList[0];
                                fillCoupleDetails();
                                if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientTypeID == 8 || ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientTypeID == 9)
                                {
                                    CmdDoonarReport.Visibility = Visibility.Visible;
                                }
                                else
                                    CmdDoonarReport.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                FrmPatientListForDashboard Win = new FrmPatientListForDashboard();
                                Win.SearchKeyword = txtPatientName.Text;
                                // Win.PatientCategoryID = 7;
                                Win.OnSaveButton_Click += new RoutedEventHandler(CoupleSearch_OnSaveButton_Click);
                                Win.Show();

                            }
                        }

                    }
                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }


        private void PGD_Click(object sender, RoutedEventArgs e)
        {
            if (dgtheropyList.SelectedItem != null)
            {
                TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            }
            if (CoupleDetails.FemalePatient.PatientID > 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_PatientPGDReport.aspx?TherapyId=" + TherapyId + "&TherapyUnitId=" + TherapyUnitId + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        //New Report by CDS
        private void CmdReport_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID > 0)
            {
                long FPatientID = CoupleDetails.FemalePatient.PatientID;
                long FPatientUnitID = CoupleDetails.FemalePatient.UnitId;
                long MPatientID = CoupleDetails.MalePatient.PatientID;
                long MPatientUnitID = CoupleDetails.MalePatient.UnitId;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/Rpt_IVFDashboard_Investigation.aspx?FPatientID=" + FPatientID + "&FPatientUnitID=" + FPatientUnitID + "&MPatientID=" + MPatientID + "&MPatientUnitID=" + MPatientUnitID), "_blank");

            }
        }

        private void CmdDoonarReport_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID > 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/Rpt_IVFDashboard_InvestigationDonar.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&Gender=" + CoupleDetails.MalePatient.Gender), "_blank");

            }
            else if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID > 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/Rpt_IVFDashboard_InvestigationDonar.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&Gender=" + CoupleDetails.FemalePatient.Gender), "_blank");
            }
        }


        private void ExeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((clsTherapyExecutionVO)ExeGrid.SelectedItem) != null)
            //{
            //    if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 1)
            //    {
            //        FrmExecutionCalender win = new FrmExecutionCalender();
            //        win.Title = "Date of LMP ( " + CoupleDetails.FemalePatient.FirstName +
            //             " " + CoupleDetails.FemalePatient.LastName + " )";
            //        win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //        win.ExeCal.DisplayDateStart = startDate;
            //        // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //        win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
            //        if (win.selecteddates != null)
            //            win.selecteddates.Clear();
            //        win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //        win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //        win.Show();
            //    }
            //    else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 3)
            //    {
            //        FrmExecutionCalender win = new FrmExecutionCalender();
            //        win.Title = "Follicular Scan ( " + CoupleDetails.FemalePatient.FirstName +
            //             " " + CoupleDetails.FemalePatient.LastName + " )";
            //        win.ExeCal.SelectionMode = CalendarSelectionMode.MultipleRange;
            //        win.ExeCal.DisplayDateStart = startDate;
            //        //win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //        win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
            //        win.RemarkLabel.Visibility = Visibility.Visible;
            //        //foreach(var r in SetValueToCal())
            //        //{
            //        //    win.ExeCal.SelectedDate=r.Date;
            //        //}
            //        //win.ExeCal.SelectedDates=(SetValueToCal())


            //        //SelectedDatesCollection selecteddates = win.ExeCal.SelectedDates;
            //        if (win.selecteddates != null)
            //            win.selecteddates.Clear();
            //        win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));

            //        win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //        win.Show();
            //    }
            //    else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 4)
            //    {
            //        FrmExecutionCalender win = new FrmExecutionCalender();
            //        win.Title = "OPU ( " + CoupleDetails.FemalePatient.FirstName +
            //             " " + CoupleDetails.FemalePatient.LastName + " )";
            //        win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //        win.ExeCal.DisplayDateStart = startDate;
            //        // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //        win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
            //        if (win.selecteddates != null)
            //            win.selecteddates.Clear();
            //        win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //        win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //        win.Show();
            //    }
            //    else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 5)
            //    {
            //        FrmExecutionCalender win = new FrmExecutionCalender();
            //        win.Title = "ET ( " + CoupleDetails.FemalePatient.FirstName +
            //             " " + CoupleDetails.FemalePatient.LastName + " )";
            //        win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //        win.ExeCal.DisplayDateStart = startDate;
            //        // win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //        win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(29);
            //        if (win.selecteddates != null)
            //            win.selecteddates.Clear();
            //        win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //        win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //        win.Show();
            //    }
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 6)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "E2 ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 7)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "Progesterone ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 8)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "FSH ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 9)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "LH ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 10)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "Prolactin ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //    //else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 11)
            //    //{
            //    //    FrmExecutionCalender win = new FrmExecutionCalender();
            //    //    win.Title = "BHCG ( " + CoupleDetails.FemalePatient.FirstName +
            //    //         " " + CoupleDetails.FemalePatient.LastName + " )";
            //    //    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
            //    //    win.ExeCal.DisplayDateStart = startDate;
            //    //    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
            //    //    if (win.selecteddates != null)
            //    //        win.selecteddates.Clear();
            //    //    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
            //    //    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
            //    //    win.Show();
            //    //}
            //}
        }

        public clsTherapyExecutionVO SetValueOfVO(clsTherapyExecutionVO ExeVO)
        {
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
        void ExecutionCalender_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SelectedDatesCollection DateValues = ((FrmExecutionCalender)sender).ExeCal.SelectedDates;
                clsUpdateTherapyExecutionBizActionVO BizActionVO = new clsUpdateTherapyExecutionBizActionVO();
                BizActionVO.TherapyExecutionDetial = new clsTherapyExecutionVO();
                BizActionVO.TherapyExecutionDetial = SetValueOfVO(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                double dayNo = 0;
                Day = new StringBuilder();
                foreach (var r in DateValues)
                {
                    dayNo = (r.Date - startDate.Value.Date).TotalDays + 1;
                    if (Day.Length == 0)
                    {
                        Day.Append(dayNo);
                    }
                    else
                    {
                        Day.Append(",");
                        Day.Append(dayNo);
                    }
                    RetriveValue(BizActionVO.TherapyExecutionDetial, (int)dayNo);
                }

                if (Day != null)
                    BizActionVO.TherapyExecutionDetial.Day = Day.ToString();
                BizActionVO.TherapyExecutionDetial.IsSurrogate = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsUpdateTherapyExecutionBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "follicular monitoring details already added for selection so cannot save the selection.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid.SelectedItem).PlanTherapyId, 0);
                            //CmdNewTheorpy.IsEnabled = false; //added by neena
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid.SelectedItem).PlanTherapyId, 0);
                            //CmdNewTheorpy.IsEnabled = false; //added by neena
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        StringBuilder Day = new StringBuilder();

        private SelectedDatesCollection SetValueToCal(clsTherapyExecutionVO ExeVO)
        {
            Calendar WinCal = new Calendar();
            WinCal.SelectionMode = CalendarSelectionMode.MultipleRange;
            SelectedDatesCollection SelectedDates = WinCal.SelectedDates;

            if (ExeVO.Day1 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(0));
            }
            if (ExeVO.Day2 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(1));
            }
            if (ExeVO.Day3 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(2));
            }
            if (ExeVO.Day4 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(3));
            }
            if (ExeVO.Day5 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(4));
            }
            if (ExeVO.Day6 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(5));
            }
            if (ExeVO.Day7 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(6));
            }
            if (ExeVO.Day8 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(7));
            }
            if (ExeVO.Day9 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(8));
            }
            if (ExeVO.Day10 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(8));
            }
            if (ExeVO.Day11 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(10));
            }
            if (ExeVO.Day12 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(11));
            }
            if (ExeVO.Day13 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(12));
            }
            if (ExeVO.Day14 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(13));
            }
            if (ExeVO.Day15 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(14));
            }
            if (ExeVO.Day16 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(15));
            }
            if (ExeVO.Day17 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(16));
            }
            if (ExeVO.Day18 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(17));
            }
            if (ExeVO.Day19 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(18));
            }
            if (ExeVO.Day20 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(19));
            }
            if (ExeVO.Day21 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(20));
            }
            if (ExeVO.Day22 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(21));
            }
            if (ExeVO.Day23 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(22));
            }
            if (ExeVO.Day24 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(23));
            }
            if (ExeVO.Day25 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(24));
            }
            if (ExeVO.Day26 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(25));
            }
            if (ExeVO.Day27 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(26));
            }
            if (ExeVO.Day28 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(27));
            }
            if (ExeVO.Day29 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(28));
            }
            if (ExeVO.Day30 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(29));
            }
            if (ExeVO.Day31 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(30));
            }
            if (ExeVO.Day32 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(31));
            }
            if (ExeVO.Day33 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(32));
            }
            if (ExeVO.Day34 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(33));
            }
            if (ExeVO.Day35 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(34));
            }
            if (ExeVO.Day36 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(35));
            }
            if (ExeVO.Day37 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(36));
            }
            if (ExeVO.Day38 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(37));
            }
            if (ExeVO.Day39 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(38));
            }
            if (ExeVO.Day40 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(39));
            }
            if (ExeVO.Day41 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(40));
            }
            if (ExeVO.Day42 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(41));
            }
            if (ExeVO.Day43 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(42));
            }
            if (ExeVO.Day44 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(43));
            }
            if (ExeVO.Day45 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(44));
            }
            if (ExeVO.Day46 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(45));
            }
            if (ExeVO.Day47 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(46));
            }
            if (ExeVO.Day48 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(47));
            }
            if (ExeVO.Day49 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(48));
            }
            if (ExeVO.Day50 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(49));
            }
            if (ExeVO.Day51 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(50));
            }
            if (ExeVO.Day52 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(51));
            }
            if (ExeVO.Day53 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(52));
            }
            if (ExeVO.Day54 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(53));
            }
            if (ExeVO.Day55 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(54));
            }
            if (ExeVO.Day56 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(55));
            }
            if (ExeVO.Day57 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(56));
            }
            if (ExeVO.Day58 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(57));
            }
            if (ExeVO.Day59 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(58));
            }
            if (ExeVO.Day60 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(59));
            }
            return SelectedDates;
        }

        private void cmdNewServices_Click(object sender, RoutedEventArgs e)
        {
            FrmIVFDashBoardDiagnosisService win = new FrmIVFDashBoardDiagnosisService();
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.TherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            win.Show();
        }
        private void cmdAddtionalMeasure_Click(object sender, RoutedEventArgs e)
        {
            AdditionalMeasure Win = new AdditionalMeasure();
            Win.TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            Win.Show();
        }
        private void cmbDiagnosis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void cmdDeleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            if (dgDiagnosisList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Diagnosis ?"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ConfirmDeleteDiagnosis_Msg");

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsEMRAddDiagnosisVO objVo = (clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem;
                            objVo.Status = false;
                            objVo.IsDeleted = true;
                            DiagnosisList.RemoveAt(dgDiagnosisList.SelectedIndex);
                            if (objVo.ID != null && objVo.ID > 0)
                                PatientDiagnosiDeletedList.Add(objVo);
                            dgDiagnosisList.UpdateLayout();
                        }
                    }
                };
                msgWD.Show();
            }
        }
        private void cmdAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            EMR.frmDiagnosisSelection Win = new EMR.frmDiagnosisSelection();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
        }
        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((EMR.frmDiagnosisSelection)sender).DialogResult == true)
                {
                    foreach (var item in (((EMR.frmDiagnosisSelection)sender).DiagnosisList))
                    {
                        if (DiagnosisList == null)
                            DiagnosisList = new ObservableCollection<clsEMRAddDiagnosisVO>();
                        if (DiagnosisList.Count > 0)
                        {
                            var item1 = from r in DiagnosisList
                                        where (r.Code == item.Code)
                                        select r;
                            if (item1.ToList().Count == 0)
                            {
                                clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                                OBj.Diagnosis = item.Diagnosis;
                                OBj.DiagnosisId = item.ID;
                                OBj.Code = item.Code;
                                OBj.ICDId = item.ICDId;
                                OBj.DiagnosisTypeId = item.DiagnosisTypeId;
                                OBj.IsEnabled = true;
                                OBj.TemplateID = item.TemplateID;
                                OBj.TemplateName = item.TemplateName;
                                OBj.DiagnosisTypeList = DiagnosisTypeList;
                                OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                                DiagnosisList.Add(OBj);
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Diagnosis already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                            OBj.Diagnosis = item.Diagnosis;
                            OBj.DiagnosisId = item.ID;
                            OBj.Code = item.Code;
                            OBj.Diagnosis = item.Diagnosis;
                            OBj.ICDId = item.ICDId;
                            OBj.DiagnosisTypeId = item.DiagnosisTypeId;
                            OBj.IsEnabled = true;
                            OBj.TemplateID = item.TemplateID;
                            OBj.TemplateName = item.TemplateName;
                            OBj.DiagnosisTypeList = DiagnosisTypeList;
                            OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                            DiagnosisList.Add(OBj);
                        }
                    }
                    dgDiagnosisList.ItemsSource = DiagnosisList;
                    dgDiagnosisList.UpdateLayout();
                    dgDiagnosisList.Focus();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void cmbFinalPlanofTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdSaveGeneral_Click(object sender, RoutedEventArgs e)
        {

        }


        //added by neena
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

        private void txtNoOfSacs_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbResult"))
            {
                for (int i = 0; i < PregnancySacsDetails.Count; i++)
                {
                    if (PregnancySacsDetails[i] == ((clsPregnancySacsDetailsVO)dgSacsDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            PregnancySacsDetails[i].ResultListID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbPregancy"))
            {
                for (int i = 0; i < PregnancySacsDetails.Count; i++)
                {
                    if (PregnancySacsDetails[i] == ((clsPregnancySacsDetailsVO)dgSacsDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            PregnancySacsDetails[i].PregnancyListID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
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


        private void dgSacsDetailsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

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

        private void ChkFetalHeart_Checked(object sender, RoutedEventArgs e)
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

        private void GetTotal_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtMI_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbSpermSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PresOvarian_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        private void CheckVisitForPrescription()
        {
            clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO BizAction = new clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO();
            if (CoupleDetails != null)
            {
                BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO)arg.Result).ID == 0 || ((clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO)arg.Result).ID == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please mark the visit before prescription..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else if (((clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO)arg.Result).ID > 0)
                    {
                        newObj = new clsMenuVO();
                        newObj.IsArt = true;
                        newObj.PlanTherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        newObj.PlanTherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                        newObj.PatientID = CoupleDetails.FemalePatient.PatientID;
                        newObj.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                        newObj.GridIndex = dgtheropyList.SelectedIndex;
                        FrmEMRIVFLinking EMRIVFLink = new FrmEMRIVFLinking(newObj);
                        EMRIVFLink.OnSaveButton_Click += new RoutedEventHandler(EMRIVFLink_OnSaveButton_Click);
                        EMRIVFLink.NewObj = newObj;
                        EMRIVFLink.Show();
                        ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID = ((clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO)arg.Result).ID;
                        //ModuleName = "EMR";
                        //Action = "EMR.frmEMR";
                        //UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //WebClient c = new WebClient();
                        //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_EMROpenReadCompleted);
                        //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    }



                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        void EMRIVFLink_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            GetPatientPreviousDiagnosis();
        }


        UserControl rootPage = Application.Current.RootVisual as UserControl;
        clsMenuVO newObj = null;
        void c_EMROpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                newObj = new clsMenuVO();
                newObj.IsArt = true;
                newObj.PlanTherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                newObj.PlanTherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                newObj.PatientID = CoupleDetails.FemalePatient.PatientID;
                newObj.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                newObj.GridIndex = dgtheropyList.SelectedIndex;
                ((IInitiateCIMSIVF)myData).Initiate(newObj);
                //TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                //Header.Text = "Patient Queue List";
                //ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                //PART_MaximizeToggle.IsChecked = false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PresStimulation_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        private void PrescTriggeer_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        private void chkIsSetCancellation_Click(object sender, RoutedEventArgs e)
        {
            if (chkCycleCancellation.IsChecked == true)
            {
                txtReason.IsEnabled = true;
            }
            else
            {
                txtReason.IsEnabled = false;
            }

        }

        private void txtReason_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbMainIndication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 8)
                cmbSubMainIndication.IsEnabled = true;
            else
                cmbSubMainIndication.IsEnabled = false;
        }

        private void InvestigationAndTrigger_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        private void LutealPhaseDrug_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        FrmIVFConsent Win = null;
        private void cmdConsent_Click(object sender, RoutedEventArgs e)
        {
            Win = new FrmIVFConsent();
            Win.IsClosed = IsClosed;
            Win.Onsaved += new EventHandler(Win_Onsaved);
            //Win.Closed += new EventHandler(Win_Closed);
            Win.PatientID = CoupleDetails.FemalePatient.PatientID;
            Win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            Win.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            Win.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            Win.Show();
        }

        void Win_Onsaved(object sender, EventArgs e)
        {
            if (Win.ConsentCheck == true)
            {
                IsConsentCheck = false;
                chkConsentCheck.IsChecked = true;
            }
        }

        //void Win_Closed(object sender, EventArgs e)
        //{
        //    UpdateConsentCheck();
        //}

        private void UpdateConsentCheck()
        {

        }



        private void hlbOrderServices_Click(object sender, RoutedEventArgs e)
        {
            CheckVisitForPrescription();
        }

        private void cmdSemenWashMag_SEP_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                //((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                //ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenWash);
                //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


                SemenWash_Dashboard frm = new SemenWash_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                // SemenExamination_Dashboard frm = new SemenExamination_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                frm.Title = "Semen Preparation MAG-SEP (Name:- " + CoupleDetails.MalePatient.FirstName +
                    " " + CoupleDetails.MalePatient.LastName + ")";

                frm.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                frm.PatientID = CoupleDetails.MalePatient.PatientID;
                frm.PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frm.VisitID = CoupleDetails.MalePatient.VisitID;
                frm.CoupleDetails = CoupleDetails; //added by neena
                frm.TransactionTypeID = 2;
                frm.CallType = 1;
                frm.IsFromDashBoard = true;
                frm.Initiate("2");

                frm.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void cmdSemenWashCryofrozen_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                //((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                //ModuleName = "DataDrivenApplication";
                //Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenWash);
                //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


                SemenWash_Dashboard frm = new SemenWash_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                // SemenExamination_Dashboard frm = new SemenExamination_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                frm.Title = "Semen Preparation Cryo Frozen (Name:- " + CoupleDetails.MalePatient.FirstName +
                    " " + CoupleDetails.MalePatient.LastName + ")";

                frm.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                frm.PatientID = CoupleDetails.MalePatient.PatientID;
                frm.PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frm.VisitID = CoupleDetails.MalePatient.VisitID;
                frm.CoupleDetails = CoupleDetails; //added by neena
                frm.TransactionTypeID = 3;
                frm.CallType = 1;
                frm.IsFromDashBoard = true;
                frm.Initiate("3");

                frm.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void TabIUI_LostFocus(object sender, RoutedEventArgs e)
        {
            //if(frmIUI.ISSavedIUIID==0 &&  frmIUI.ThawedSampleID != "")
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Save IUI Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }


        //private void SaveNoOfOocyte_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtNoOocyteRetrieved.Text))
        //    {
        //        dtpPregnancy.SetValidation("No of Oocyte Retrieved Is Required");
        //        dtpPregnancy.RaiseValidationError();
        //        dtpPregnancy.Focus();
        //    }
        //    else if (Convert.ToInt32(txtNoOocyteRetrieved.Text) > 50)
        //    {
        //        dtpPregnancy.SetValidation("Enter Valid Number");
        //        dtpPregnancy.RaiseValidationError();
        //        dtpPregnancy.Focus();
        //    }
        //    else
        //    {

        //        clsIVFDashboard_AddUpdateOocyteNumberBizActionVO BizAction = new clsIVFDashboard_AddUpdateOocyteNumberBizActionVO();
        //        BizAction.OPUDetails = new clsIVFDashboard_OPUVO();

        //        BizAction.OPUDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
        //        BizAction.OPUDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
        //        BizAction.OPUDetails.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
        //        BizAction.OPUDetails.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;



        //        if (txtNoOocyteRetrieved.Text != null)
        //            BizAction.OPUDetails.OocyteRetrived = Convert.ToInt64(txtNoOocyteRetrieved.Text);



        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", " Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
        //                msgW1.Show();
        //                //txtNoOocyteRetrieved.IsEnabled = false;
        //                //hlbOocyteNoSave.IsEnabled = false;
        //            }

        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //}

        //
        //void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        //{
        //    if (result == MessageBoxResult.OK)
        //    {
        //        UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDaysNew") as UIElement;
        //        grpRepNew = new frmGraphicalRepresentationofLabDaysNew();
        //        grpRepNew = (frmGraphicalRepresentationofLabDaysNew)mydata1;
        //        grpRepNew.CoupleDetails = CoupleDetails;
        //        grpRepNew.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
        //        grpRepNew.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
        //        grpRepNew.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
        //        grpRepNew.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;

        //        // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
        //        ConEmbryology.Content = grpRepNew;

        //    }
        //    //throw new NotImplementedException();
        //}
        //


        //.........................................................................................
        //.........................................................................................


    }
}
