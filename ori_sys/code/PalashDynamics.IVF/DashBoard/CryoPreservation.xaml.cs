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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class CryoPreservation : ChildWindow
    {
        public clsCoupleVO CoupleDetails;
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

        // Added by Saily P

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
        public CryoPreservation()
        {
            InitializeComponent();
             this.DataContext=null;
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
                        fillDetails();
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
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Vitrification != null)
                Vitrification.IsSelected = true;
            SSemenMaster();
          
        }
        private void fillDetails() 
        {
            try
            {

                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
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
                        
                        //if(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm==true)
                        //{
                        //    rdoYes.IsChecked=true;
                        //}
                        //if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm == false)
                        //{
                        //    rdoNo.IsChecked = true;
                        //}
               
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                     
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID !=null)
                            {
                                PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ProtocolTypeID));
                            }
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID !=null)
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
                            
                            VitriDetails.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);
                          
                        }
                        if (Vitrification != null)
                        {
                            if (Vitrification.IsSelected == true)
                            {
                                dgVitrificationDetilsGrid.ItemsSource = null;
                                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                            }
                        }
                        if (Thawing != null)
                        {
                            if (Thawing.IsSelected == true)
                            {
                                dgThawingViDetilsGrid.ItemsSource = null;
                                dgThawingViDetilsGrid.ItemsSource = VitriDetails;
                            }
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

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Thawing != null)
            {
                if (Thawing.IsSelected)
                {
                    fillLabIncharge();
                }
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
                        fillThawingDetails();
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
        long ThawID = 0;
        private void fillThawingDetails() 
        {
            try
            {
                clsIVFDashboard_GetThawingBizActionVO BizAction = new clsIVFDashboard_GetThawingBizActionVO();
                BizAction.Thawing = new clsIVFDashBoard_ThawingVO();
                BizAction.Thawing.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                BizAction.Thawing.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
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
                        ThawDetails.Clear();
                        this.DataContext = (clsIVFDashboard_GetThawingBizActionVO)arg.Result;
                        ThawID = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.ID;
                        dtthawingDate.SelectedDate = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        txtthawingTime.Value = ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.DateTime;
                        cmbLabPerson.SelectedValue = (long)((clsIVFDashboard_GetThawingBizActionVO)arg.Result).Thawing.LabPersonId;
                   
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList.Count; i++)
                        {
                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeList = Grade;
                            ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageList = CellStage;
                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID !=0)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID !=0)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID));
                            }
                            if (Gr != null)
                            {
                                if (Gr.ID > 0)
                                {
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
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = CS.Description;
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStage = "";
                                    ((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i].CellStageID = CS.ID;

                                }
                            }
                           

                            ThawDetails.Add(((clsIVFDashboard_GetThawingBizActionVO)arg.Result).ThawingDetailsList[i]);
                          
                        }
                        dgThawingDetilsGrid.ItemsSource = null;
                        dgThawingDetilsGrid.ItemsSource = ThawDetails;
                        fillDetails();
                       
                  
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
        private void Colorsel_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void cmbCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool Validate() 
        {
            if (dtVitrificationDate.SelectedDate == null)
            {
                dtVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtVitrificationDate.RaiseValidationError();
                dtVitrificationDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                dtVitrificationDate.ClearValidationError();
                txtTime.SetValidation("Please Select Vitrification Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else if (VitriDetails.Count <= 0)
            {
               
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                return true;
            }
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
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
            BizAction.VitrificationMain.DateTime = dtVitrificationDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.IsOnlyVitrification = false;
            for (int i = 0; i < VitriDetails.Count; i++)
            {
                VitriDetails[i].CanId = VitriDetails[i].SelectedCanId.ID;
                VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID;
                VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID;
                VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID;
                VitriDetails[i].ConistorNo= VitriDetails[i].SelectedCanisterId.ID;
                VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID;
                VitriDetails[i].ColorCodeID= VitriDetails[i].SelectedColorSelector.ID;
            }
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetails.ToList());
            if (chkFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;
    

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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ColorSelector_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private bool ValidateThaw()
        {

            if (dtthawingDate.SelectedDate == null)
            {
                dtthawingDate.SetValidation("Please Select Thawing Date");
                dtthawingDate.RaiseValidationError();
                dtthawingDate.Focus();
                return false;
            }
            else if (txtthawingTime.Value == null)
            {
                dtthawingDate.ClearValidationError();
                txtthawingTime.SetValidation("Please Select Thawing Time");
                txtthawingTime.RaiseValidationError();
                txtthawingTime.Focus();
                return false;
            }
            else if (cmbLabPerson.SelectedItem== null)
            {

                dtthawingDate.ClearValidationError();
                txtthawingTime.ClearValidationError();
                cmbLabPerson.TextBox.SetValidation("Please Select Lab Person");
                cmbLabPerson.TextBox.RaiseValidationError();
                cmbLabPerson.Focus();
                return false;
            }
            else if (ThawDetails.Count <= 0)
            {

                dtthawingDate.ClearValidationError();
                txtthawingTime.ClearValidationError();
                cmbLabPerson.TextBox.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {

                dtthawingDate.ClearValidationError();
                txtthawingTime.ClearValidationError();
                cmbLabPerson.TextBox.ClearValidationError();
                return true;
            }
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
            BizAction.Thawing = new  clsIVFDashBoard_ThawingVO();
            BizAction.Thawing.ID = ThawID;
            BizAction.Thawing.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Thawing.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Thawing.PlanTherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
            BizAction.Thawing.PlanTherapyUnitID = ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
            BizAction.Thawing.DateTime = dtthawingDate.SelectedDate.Value.Date;
            BizAction.Thawing.DateTime = BizAction.Thawing.DateTime.Value.Add(txtthawingTime.Value.Value.TimeOfDay);
            BizAction.Thawing.LabPersonId= ((MasterListItem)cmbLabPerson.SelectedItem).ID;
            BizAction.ThawingDetailsList = ((List<clsIVFDashBoard_ThawingDetailsVO>)ThawDetails.ToList());


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

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsIVFDashBoard_ThawingDetailsVO)(row.DataContext)).Plan == true)
            {
               
                e.Row.IsEnabled = false;
               
            }
        }

        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(((clsPlanTherapyVO)TherapyDetails.DataContext).ID, ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID, CoupleDetails, "Vitrification");
            Win.Show();


        }
    }
}

