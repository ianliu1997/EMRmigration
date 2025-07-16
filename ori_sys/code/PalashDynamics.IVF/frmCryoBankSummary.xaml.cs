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
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Collections;
using System.Windows.Browser;
using MessageBoxControl;

namespace PalashDynamics.IVF
{
    public partial class frmCryoBankSummary : UserControl
    {

        #region Variables
        WaitIndicator wait = new WaitIndicator();
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        ListBox lstFUBox;

        #endregion

        #region Properties
        public Boolean IsEdit { get; set; }
        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }
        private ObservableCollection<clsGetVitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
        public ObservableCollection<clsGetVitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }

        private ObservableCollection<clsGetVitrificationDetailsVO> _RemoveVitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
        public ObservableCollection<clsGetVitrificationDetailsVO> RemoveVitriDetails
        {
            get { return _RemoveVitriDetails; }
            set { _RemoveVitriDetails = value; }
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

        public PagedSortableCollectionView<clsGetVitrificationDetailsVO> EmbryoList { get; private set; }
        public PagedSortableCollectionView<clsSpermFreezingVO> SpermCollectionList { get; private set; }
        public int PageSize
        {
            get
            {
                return EmbryoList.PageSize;
            }
            set
            {
                if (value == EmbryoList.PageSize) return;
                EmbryoList.PageSize = value;
                //  OnPropertyChanged("PageSize");
            }
        }
        public int SpermListPageSize
        {
            get
            {
                return SpermCollectionList.PageSize;
            }
            set
            {
                if (value == SpermCollectionList.PageSize) return;
                SpermCollectionList.PageSize = value;
            }
        }
        public string Fname, Mname, Lname, Familyname, MRNo, CtcNo, CoupleUnitID;

        #endregion

        #region Unloaded Event
        void CryoBank_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.GlobalDeleteFileCompleted += (s1, args1) =>
                //{
                //    if (args1.Error == null)
                //    {

                //    }
                //};
                //client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", AttachedFileNameList);
            }
            catch (Exception Exception) { }
        }
        #endregion

        #region Constructor
        public frmCryoBankSummary()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(CryoBank_Loaded);
            this.Unloaded += new RoutedEventHandler(CryoBank_Unloaded);

            EmbryoList = new PagedSortableCollectionView<clsGetVitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            //  this.EmbryoDataGrid.DataContext = EmbryoList;
            PageSize = 7;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;



            SpermCollectionList = new PagedSortableCollectionView<clsSpermFreezingVO>();
            SpermCollectionList.OnRefresh += new EventHandler<RefreshEventArgs>(SpermCollection_OnRefresh);
            // this.SpermsDataGrid.DataContext = SpermCollectionList;
            SpermListPageSize = 7;
            SpermCollectionList.PageSize = SpermListPageSize;
            SpermsDataPager.DataContext = SpermCollectionList;
        }
        #endregion

        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>
        void SpermCollection_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSpermCollectionCryoBank();
        }

        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        #region Loaded Event
        void CryoBank_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FillEmbryoCryoBank();
                FillSpermCollectionCryoBank();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        private void FillEmbryoCryoBank()
        {
            try
            {
                clsGetVitrificationForCryoBankBizActionVO BizAction = new clsGetVitrificationForCryoBankBizActionVO();
                // BizAction.SearchExpression = txtSearch.Text;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = EmbryoList.PageSize;
                BizAction.StartRowIndex = EmbryoList.PageIndex * EmbryoList.PageSize;
                BizAction.Vitrification = new clsGetVitrificationVO();
                // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
                BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                //Search
                if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    BizAction.FName = txtFirstName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                    BizAction.MName = txtMiddleName.Text.Trim();
                if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                    BizAction.LName = txtLastName.Text.Trim();
                if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                    BizAction.FamilyName = txtFamilyName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
                    BizAction.MRNo = txtMrno.Text.Trim();

                //if (dtpFromDate.SelectedDate.Value != null)
                //    BizAction.FromDate = dtpFromDate.SelectedDate.Value;
                //if (dtpToDate.SelectedDate.Value != null)
                //    BizAction.ToDate = dtpToDate.SelectedDate.Value;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        EmbryoDataGrid.ItemsSource = null;
                        EmbryoDataPager.DataContext = null;
                        BizAction.Vitrification = (((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification);
                        BizAction.Vitrification.VitrificationDetails = (((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails);
                        // BizAction.Vitrification.PatientDetails=(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.PatientDetails);
                        if (BizAction.Vitrification.VitrificationDetails.Count > 0)
                        {
                            EmbryoList.Clear();
                            EmbryoList.TotalItemCount = Convert.ToInt16(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).TotalRows);
                            foreach (clsGetVitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetails)
                            {
                                EmbryoList.Add(item);
                            }
                            EmbryoDataGrid.ItemsSource = EmbryoList;
                            EmbryoDataPager.DataContext = EmbryoList;
                        }
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

        private void FillSpermCollectionCryoBank()
        {
            try
            {
                clsGetSpremFreezingDetailsBizActionVO BizAction = new clsGetSpremFreezingDetailsBizActionVO();
                // BizAction.SearchExpression = txtSearch.Text;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = SpermCollectionList.PageSize;
                BizAction.StartRowIndex = SpermCollectionList.PageIndex * SpermCollectionList.PageSize;
                BizAction.Vitrification = new List<clsSpermFreezingVO>();

                if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    BizAction.FName = txtFirstName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                    BizAction.MName = txtMiddleName.Text.Trim();
                if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                    BizAction.LName = txtLastName.Text.Trim();
                if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                    BizAction.FamilyName = txtFamilyName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
                    BizAction.MRNo = txtMrno.Text.Trim();
                // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
                BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        SpermsDataGrid.ItemsSource = null;
                        SpermsDataPager.DataContext = null;
                        BizAction.Vitrification = (((clsGetSpremFreezingDetailsBizActionVO)arg.Result).Vitrification);
                        if (BizAction.Vitrification.Count > 0)
                        {
                            SpermCollectionList.Clear();
                            SpermCollectionList.TotalItemCount = Convert.ToInt16(((clsGetSpremFreezingDetailsBizActionVO)arg.Result).TotalRows);
                            foreach (clsSpermFreezingVO item in BizAction.Vitrification)
                            {
                                SpermCollectionList.Add(item);
                            }
                            SpermsDataGrid.ItemsSource = SpermCollectionList;
                            SpermsDataPager.DataContext = SpermCollectionList;
                        }
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

        #region Fill Master Item

        //private void fillGrade()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVF_GradeMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                fillCanID();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void fillCanID()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillStrawList();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillStrawList()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                Straw = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillGobletshape();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillGobletshape()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFGobletShapeMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                GobletShape = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillGobletSize();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillGobletSize()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFGobletSizeMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                GobletSize = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillCanister();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillCanister()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                Canister = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillTank();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillTank()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                Tank = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                FillCellStage();
        //            }
        //        };

        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        //private void FillCellStage()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IVF_FertilizationStageMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //                CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //                fillOnlyVitrificationDetails();
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //    }
        //}

        #endregion

        #region  Fill Vitrification Details

        //public void fillVitrificationDetails()
        //{
        //    try
        //    {
        //        clsGetVitrificationDetailsBizActionVO BizAction = new clsGetVitrificationDetailsBizActionVO();
        //        BizAction.Vitrification = new clsGetVitrificationVO();
        //        BizAction.IsEdit = true;
        //        BizAction.ID = 0;
        //        BizAction.UnitID = 0;
        //        //if (CoupleDetails != null)
        //        //{
        //        //    BizAction.CoupleID = CoupleDetails.CoupleId;
        //        //    BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
        //        //}
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                MasterListItem SS = new MasterListItem();
        //                MasterListItem OS = new MasterListItem();
        //                MasterListItem Gr = new MasterListItem();
        //                MasterListItem PT = new MasterListItem(); 
        //                MasterListItem CS = new MasterListItem();                         

        //                for (int i = 0; i < ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails.Count; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen))
        //                    {
        //                        SS = SSemen.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen));
        //                    }
        //                    if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes))
        //                    {
        //                        OS = SOctyes.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes));
        //                    }
        //                    if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade))
        //                    {
        //                        Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade));
        //                    }
        //                    if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType))
        //                    {
        //                        PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType));
        //                    }
        //                    if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange))
        //                    {
        //                        CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange));
        //                    }

        //                    if (SS != null)
        //                    {
        //                        if (SS.ID > 0)
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen = SS.Description;
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemenID = SS.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen = "";
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemenID = SS.ID;
        //                        }


        //                    }
        //                    if (OS != null)
        //                    {
        //                        if (OS.ID > 0)
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes = OS.Description;
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytesID = OS.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes = "";
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytesID = OS.ID;
        //                        }
        //                    }
        //                    if (Gr != null)
        //                    {
        //                        if (Gr.ID > 0)
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade = Gr.Description;
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Gr.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade = "";
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Gr.ID;
        //                        }
        //                    }
        //                    if (PT != null)
        //                    {
        //                        if (PT.ID > 0)
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType = PT.Description;
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolTypeID = PT.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType = "";
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolTypeID = PT.ID;
        //                        }
        //                    }
        //                    if (CS != null)
        //                    {
        //                        if (CS.ID > 0)
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = CS.Description;
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
        //                        }
        //                        else
        //                        {
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = "";
        //                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
        //                        }
        //                    }
        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanIdList = CanList;
        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID));

        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].CellStage = CellStage;
        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);

        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].Grade = Grade;
        //                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);

        //                    VitriDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i]);
        //                   // ThawDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i]);
        //                    wait.Close();
        //                }



        //                dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
        //               // dgThawingDetilsGrid.ItemsSource = ThawDetails;
        //               // LoadFURepeaterControl();
        //                wait.Close();
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

        public void fillOnlyVitrificationDetails()
        {
            try
            {
                clsGetVitrificationForCryoBankBizActionVO BizAction = new clsGetVitrificationForCryoBankBizActionVO();
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.IsEdit = IsEdit;
                BizAction.ID = ID;
                BizAction.UnitID = UnitID;
                BizAction.FromID = (int)IVFLabWorkForm.OnlyVitrification;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        EmbryoDataGrid.DataContext = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification;
                        //if (IsEdit)
                        //{
                        //    EmbryoDataGrid.DataContext = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification;                           
                        //    //chkFreeze.IsChecked = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.IsFreezed;
                        //}
                        //else
                        //{
                        //    rdoYes.IsChecked = true;
                        //}                     

                        for (int i = 0; i < ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails.Count; i++)
                        {
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStageList = CellStage;
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange));
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange);
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeList = Grade;
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade));
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade);
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanIdList = CanList;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID;

                            //Added by Saily P 
                            //Straw
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawIdList = Straw;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId;

                            //Goblet Shape
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeList = GobletShape;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId;

                            //Goblet Size
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeList = GobletSize;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId;

                            //Canister
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterIdList = Canister;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId;

                            //Tank
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankList = Tank;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId) > 0)
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId));
                                }
                                else
                                {
                                    ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId = ((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId;
                            VitriDetails.Add(((clsGetVitrificationForCryoBankBizActionVO)arg.Result).Vitrification.VitrificationDetails[i]);
                            wait.Close();
                        }

                        EmbryoDataGrid.ItemsSource = VitriDetails;
                        // LoadFURepeaterControl();
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
            }
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                txtFamilyName.Text.ToTitleCase();
        }

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            //if (chkEmbryo.IsChecked == true)
            //{
            //    EmbryoDockPanel.Visibility = Visibility.Visible;
            //    SpermsDockPanel.Visibility = Visibility.Collapsed;
            //}
            //if (chkSperm.IsChecked == true)
            //{
            //    EmbryoDockPanel.Visibility = Visibility.Collapsed;
            //    SpermsDockPanel.Visibility = Visibility.Visible;

            //}
            //if (chkBoth.IsChecked == true)
            //{
            //    EmbryoDockPanel.Visibility = Visibility.Visible;
            //    SpermsDockPanel.Visibility = Visibility.Visible;
            //}
        }

        #region commented
        //private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        //{
        //}
        //private void txtState_LostFocus(object sender, RoutedEventArgs e)
        //{
        //}
        //private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        //{
        //}
        //private void txtPinCode_LostFocus(object sender, RoutedEventArgs e)
        //{
        //}
        //private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        //{
        //}
        //private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        //{
        //}
        #endregion

        private void ValidationsForSearch()
        {
            if (dtpFromDate.SelectedDate.Value != null && dtpToDate.SelectedDate.Value != null)
            {
                if (dtpFromDate.SelectedDate.Value > dtpToDate.SelectedDate.Value)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    //res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }
            if (dtpFromDate.SelectedDate.Value != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                // res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))            
            //    Fname = txtFirstName.Text.Trim();
            //if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
            //    Mname = txtMiddleName.Text.Trim();
            //if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
            //    Lname = txtLastName.Text.Trim();
            //if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
            //    Familyname = txtFamilyName.Text.Trim();
            //if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
            //    MRNo = txtMrno.Text.Trim();
            //if (!string.IsNullOrEmpty(txtContactNo.Text.Trim()))
            //    CtcNo = txtContactNo.Text.Trim();
            // ValidationsForSearch();          

            if (chkEmbryo.IsChecked == true)
                FillEmbryoCryoBank();
            else if (chkSperm.IsChecked == true)
                FillSpermCollectionCryoBank();
            else if (chkBoth.IsChecked == true)
            {
                FillEmbryoCryoBank();
                FillSpermCollectionCryoBank();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

            //string URL = "../Reports/IVF/CryoBank.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            if (txtMrno.Text != null)
                MRNo = txtMrno.Text;

            if (txtFirstName.Text != null)
                Fname = txtFirstName.Text;

            if (txtLastName.Text != null)
                Lname = txtLastName.Text;

            if (txtFamilyName.Text != null)
                Familyname = txtFamilyName.Text;

            UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            string URL = "../Reports/IVF/CryoBank.aspx?UnitID=" + UnitID + "&MRNo=" + MRNo + "&FName=" + Fname + "&LName=" + Lname + "&FamilyName=" + Familyname;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            //string URL = "../Reports/IVF/CryoBank.aspx?UnitID=" + UnitID + "&MRNo=" + MRNo + "&CoupleUnitID=" +CoupleUnitID  + "&FName=" + Fname + "&LName=" + Lname + "&FamilyName=" + Familyname;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (EmbryoDataGrid.SelectedItem != null)
            {
                //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
                //{
                BarcodeForm win = new BarcodeForm();

                long VitrificationNo = ((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).VitrificationNo;

                win.PrintData = "EM" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select embryo.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            //}
            

        }

        private void CmdPrintSpermBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (SpermsDataGrid.SelectedItem != null)
            {
                //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
                //{
                BarcodeForm win = new BarcodeForm();

                long VitrificationNo = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).VtirificationNo;

                win.PrintData = "SP" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select sperm.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            //}

        }

    }
}
