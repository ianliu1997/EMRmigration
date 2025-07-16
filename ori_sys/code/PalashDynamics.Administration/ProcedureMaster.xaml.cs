using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.Pharmacy;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using System.Collections.ObjectModel;
using System.Windows.Data;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.ValueObjects.Inventory;
using EMR;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Controls;

namespace PalashDynamics.Administration
{
    public partial class ProcedureMaster : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsCancel = true;
        string msgTitle = "PALASH";
        string msgText = "";
        LocalizationManager localizationManager;
        List<MasterListItem> ServiceList1 = new List<MasterListItem>();
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }

        long? ServiceID = 0;
        public clsVisitVO CurrentVisit { get; set; }
        List<MasterListItem> Priority;
        public string Action { get; set; }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Properties
        public string ModuleName { get; set; }
        public ObservableCollection<clsProcedureEquipmentDetailsVO> EquipList { get; set; }
        public PagedSortableCollectionView<clsProcedureMasterVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        #endregion
        public ProcedureMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            localizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            DataList = new PagedSortableCollectionView<clsProcedureMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();

        }

        private void FillContent()
        {
            string strLabel = string.Empty;
            try
            {
                //For PreOperative Insructions
                PreOPResultListContent.Content = new SearchResultList();
                if (PreOPResultListContent.Content != null)
                {
                    ((SearchResultList)PreOPResultListContent.Content).TableName = MasterTableNameList.M_PreOperativeInstructionsMaster;
                    strLabel = "Pre-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblPreOperativeInstructions");
                    ((SearchResultList)PreOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;
                }

                //For PostOperative Insructions
                PostOPResultListContent.Content = new SearchResultList();
                if (PostOPResultListContent.Content != null)
                {
                    ((SearchResultList)PostOPResultListContent.Content).TableName = MasterTableNameList.M_PostOperativeInstructionsMaster;
                    strLabel = "Post-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblPostOperativeInstructions");
                    ((SearchResultList)PostOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;
                }

                //For IntraOperative Insructions
                IntraOPResultListContent.Content = new SearchResultList();
                if (IntraOPResultListContent.Content != null)
                {
                    ((SearchResultList)IntraOPResultListContent.Content).TableName = MasterTableNameList.M_IntraOperativeInstructionsMaster;
                    strLabel = "Intra-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblIntraOperativeInstructions");
                    ((SearchResultList)IntraOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;
                }

                //For Consent
                ConsentResultListContent.Content = new SearchResultList();
                if (ConsentResultListContent.Content != null)
                {
                    //((SearchResultList)ConsentResultListContent.Content).TableName = MasterTableNameList.M_ConsentMaster; //***//
                    ((SearchResultList)ConsentResultListContent.Content).TableName = MasterTableNameList.M_PatientConsentMaster;
                    strLabel = "Consent";
                    //strLabel = localizationManager.GetValue("tbhConsent");
                    ((SearchResultList)ConsentResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;
                }

            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Used when refresh event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = new clsProcedureMasterVO();
                ItemList1 = new ObservableCollection<clsProcedureItemDetailsVO>();
                KitList = new ObservableCollection<clsProcedureItemDetailsVO>();
                SetCommandButtonState("Load");
                EquipList = new ObservableCollection<clsProcedureEquipmentDetailsVO>();
                StaffList = new ObservableCollection<clsProcedureStaffDetailsVO>();
                StaffListOther = new ObservableCollection<clsProcedureStaffDetailsVO>();
                PocedureTempateList = new ObservableCollection<clsProcedureTemplateDetailsVO>();
                FetchDoctorClassification();
                FetchDesignations();
                FetchProcedureType();
                FetchRecommondedAnesthesiaType();
                FetchService();
                FetchOT();
                FetchData();
                FillCategory();
                SetCommandButtonState("ViewStaff");
                //ValidateBackPanel();
                FillTemplateDetails();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Fill Combo
        /// <summary>
        /// fills Service combo box
        /// </summary>
        private void FetchService()
        {
            try
            {

                //clsGetServicesForProcedureBizActionVO Bizaction = new clsGetServicesForProcedureBizActionVO();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                //BizAction.Parent = new KeyValue { Key = true, Value = "OTProcedure" };
                BizAction.Parent = new KeyValue { Key = 16, Value = "SpecializationId" };

                BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbServiceName.ItemsSource = null;
                        CmbServiceName.ItemsSource = objList;
                        CmbServiceName.SelectedItem = objList[0];


                        if (this.DataContext != null)
                        {
                            CmbServiceName.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ServiceID;

                        }
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void FillCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureCategoryMaster;
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

                        CmbCategory.ItemsSource = null;
                        CmbCategory.ItemsSource = objList;
                        CmbCategory.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            CmbCategory.SelectedValue = ((clsProcedureMasterVO)this.DataContext).CategoryID;
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillSubCategory(long SubCatgyID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureSubCategoryMaster;
                BizAction.Parent = new KeyValue { Value = "ProcedureCategoryID", Key = SubCatgyID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        CmbSubCategory.ItemsSource = null;
                        CmbSubCategory.ItemsSource = objList;
                        CmbSubCategory.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            CmbSubCategory.SelectedValue = ((clsProcedureMasterVO)this.DataContext).SubCategoryID;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillCheckList(long CheckLstID, long CategoryID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureCheklistDetails;

                BizAction.Parent = new KeyValue { Value = "SubCategoryID", Key = CheckLstID.ToString() };
                BizAction.Category = new KeyValue { Value = "CategoryID", Key = CategoryID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        CmbCheckList.ItemsSource = null;
                        CmbCheckList.ItemsSource = objList;
                        CmbCheckList.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            CmbCheckList.SelectedValue = ((clsProcedureMasterVO)this.DataContext).CheckListID;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FetchDoctorClassification()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objList.ForEach(z => z.Code = z.ID.ToString());
                        cmbDoctorClassification.ItemsSource = null;
                        cmbDoctorClassification.ItemsSource = objList;
                        cmbDoctorClassification.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private void FetchDoctorClassification()
        //{
        //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
        //    BizAction.DescriptionColumn = "NMSPESIAL";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID > 0)
        //    {
        //        BizAction.MasterTable = MasterTableNameList.BAGIAN_DOC;
        //        BizAction.CodeColumn = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
        //    }
        //    else
        //    {
        //        BizAction.MasterTable = MasterTableNameList.SPESIAL;
        //        BizAction.CodeColumn = "KDSPESIAL";
        //    }


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem("0", "-- Select --"));
        //            objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
        //            cmbDoctorClassification.ItemsSource = null;
        //            cmbDoctorClassification.ItemsSource = objList;

        //            if (this.DataContext != null)
        //            {
        //                cmbDoctorClassification.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ID;
        //            }
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //}
        #endregion

        /// <summary>
        /// fills Service combo box
        /// </summary>
        private void FetchProcedureType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbProcedureType.ItemsSource = null;
                        CmbProcedureType.ItemsSource = objList;
                        CmbProcedureType.SelectedItem = objList[0];
                        CmbProcedureTypeSearch.ItemsSource = null;
                        CmbProcedureTypeSearch.ItemsSource = objList;
                        CmbProcedureTypeSearch.SelectedItem = objList[0];
                        if (this.DataContext != null)
                        {
                            CmbProcedureType.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ProcedureTypeID;
                        }
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// fills recommonded anesthesia combo
        /// </summary>
        private void FetchRecommondedAnesthesiaType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_AnesthesiaTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbAnesthesiaType.ItemsSource = null;
                        CmbAnesthesiaType.ItemsSource = objList;
                        CmbAnesthesiaType.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            CmbAnesthesiaType.SelectedValue = ((clsProcedureMasterVO)this.DataContext).RecommandedAnesthesiaTypeID;
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FetchOT()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOT.ItemsSource = null;
                        CmbOT.ItemsSource = objList;
                        CmbOT.SelectedItem = objList[0];
                        if (this.DataContext != null)
                        {
                            CmbOT.SelectedValue = ((clsProcedureMasterVO)this.DataContext).OperationTheatreID;
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Fetch OT Table according to OT
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTTable(long OtTableID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTableMaster;
                BizAction.Parent = new KeyValue { Value = "OTTheatreID", Key = OtTableID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOTTable.ItemsSource = null;
                        CmbOTTable.ItemsSource = objList;
                        CmbOTTable.SelectedItem = objList[0];
                        if (this.DataContext != null)
                        {
                            CmbOTTable.SelectedValue = ((clsProcedureMasterVO)this.DataContext).OTTableID;
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FetchDesignations()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objList.ForEach(z => z.Code = z.ID.ToString());
                        cmbStaff.ItemsSource = null;
                        cmbStaff.ItemsSource = objList;
                        cmbStaff.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    this.IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "EditDoc":
                    cmdAddStaff.IsEnabled = false;
                    cmdModifyStaff.IsEnabled = true;
                    break;

                case "EditStaff":
                    cmdOtherStaffAdd.IsEnabled = false;
                    cmdModifyStaffOther.IsEnabled = true;
                    break;

                case "ViewStaff":
                    cmdAddStaff.IsEnabled = true;
                    cmdModifyStaff.IsEnabled = false;
                    cmdOtherStaffAdd.IsEnabled = true;
                    cmdModifyStaffOther.IsEnabled = false;
                    break;

                case "EditProcTemplate":
                    cmdAddProcedureTemp.IsEnabled = false;
                    cmdModifyProcedureTemp.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Clears UI
        /// </summary>
        private void ClearUI()
        {
            try
            {
                dgCheckList.ItemsSource = null;
                myList = new List<clsProcedureChecklistDetailsVO>();

                if (CmbCategory.ItemsSource != null && CmbCategory.SelectedValue != null)
                    CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;

                if (CmbSubCategory.ItemsSource != null && CmbSubCategory.SelectedValue != null)
                    CmbSubCategory.SelectedValue = ((List<MasterListItem>)CmbSubCategory.ItemsSource)[0].ID;

                if (CmbCheckList.ItemsSource != null && CmbCheckList.SelectedValue != null)
                    CmbCheckList.SelectedValue = ((List<MasterListItem>)CmbCheckList.ItemsSource)[0].ID;

                dgDocClassificationList.ItemsSource = null;
                dgStaffList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = null;
                isView = false;
                StaffList.Clear();
                StaffListOther.Clear();
                PocedureTempateList.Clear();
                ItemList1.Clear();
                ServiceList.Clear();
                this.DataContext = new clsProcedureMasterVO();
                CmbAnesthesiaType.SelectedValue = (long)0;
                CmbProcedureType.SelectedValue = (long)0;
                cmbDoctorClassification.SelectedValue = (long)0;
                cmbStaff.SelectedValue = (long)0;
                CmbOT.SelectedValue = (long)0;
                CmbOTTable.SelectedValue = (long)0;
                ServiceID = 0;
                FillContent();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        int ClickedFlag = 0;
        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    bool SaveTest = true;
                    SaveTest = ValidateBackPanel();
                    if (SaveTest == true)
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("SaveVerification_Msg");
                        //}
                        //else
                        //{
                        msgText = "Are you sure you want to Save?";
                        //}
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                        msgW.Show();
                    }
                    else
                        ClickedFlag = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Validates Back panel
        /// </summary>
        /// <returns></returns>
        private bool ValidateBackPanel()
        {
            string strVal = string.Empty;
            bool result = true;
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                {
                    strVal = "Please enter code !";
                    //strVal = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeReqd_Msg");
                    txtCode.SetValidation(strVal);
                    txtCode.RaiseValidationError();
                    txtCode.Focus();
                    result = false;
                    //return false;
                }
                else
                    txtCode.ClearValidationError();

                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    strVal = "Please Enter Description !";                   
                    txtDescription.SetValidation(strVal);
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();
                    result = false;
                    //return false;
                }
                else
                    txtDescription.ClearValidationError();

                if (string.IsNullOrEmpty(txtDuration.Text.Trim()))
                {
                    strVal = "Please Enter Duration !";
                    txtDuration.SetValidation(strVal);
                    txtDuration.RaiseValidationError();
                    txtDuration.Focus();
                    result = false;
                    //return false;
                }
                else
                    txtDuration.ClearValidationError();

                if ((MasterListItem)CmbProcedureType.SelectedItem == null)
                {
                    CmbProcedureType.TextBox.SetValidation("Please select procedure type");
                    CmbProcedureType.TextBox.RaiseValidationError();
                    CmbProcedureType.Focus();
                    result = false;
                }

                else
                {
                    if (CmbProcedureType.SelectedItem != null)
                    {
                        if (((MasterListItem)CmbProcedureType.SelectedItem).ID == 0)
                        {
                            CmbProcedureType.TextBox.SetValidation("Please select procedure type");
                            CmbProcedureType.TextBox.RaiseValidationError();
                            CmbProcedureType.Focus();
                            result = false;
                        }
                    }
                    else
                        CmbProcedureType.TextBox.ClearValidationError();
                }



                //if (string.IsNullOrEmpty(txtCode.Text))
                //{
                //    msgText = "Please Enter Code";
                //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    txtCode.BorderBrush = new SolidColorBrush(Colors.Red);
                //    txtCode.Focus();
                //    return false;
                //}
                //if (txtDescription.Text == "")
                //{
                //    msgText = "Please Enter Description. . .";
                //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    txtDescription.BorderBrush = new SolidColorBrush(Colors.Red);
                //    txtDescription.Focus();
                //    return false;
                //}

                //if (txtDuration.Text == "")
                //{
                //    msgText = "Please Enter Duration";
                //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    txtDuration.BorderBrush = new SolidColorBrush(Colors.Red);
                //    txtDuration.Focus();
                //    return false;
                //}

                //if ((CmbProcedureType.ItemsSource) == null)
                //{
                //    msgText = "Please select procedure type";
                //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    CmbProcedureType.BorderBrush = new SolidColorBrush(Colors.Red);
                //    CmbProcedureType.Focus();
                //    return false;
                //}
                //if (((MasterListItem)CmbProcedureType.SelectedItem) == null || ((MasterListItem)CmbProcedureType.SelectedItem).ID == 0)
                //{
                //    msgText = "Please select procedure type";
                //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    CmbProcedureType.BorderBrush = new SolidColorBrush(Colors.Red);
                //    CmbProcedureType.Focus();
                //    return false;
                //}

                if ((CmbAnesthesiaType.ItemsSource) == null)
                {
                    msgText = "Please select recommended anesthesia type";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbProcedureType.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbAnesthesiaType.Focus();
                    return false;
                }
                if (((MasterListItem)CmbAnesthesiaType.SelectedItem) == null || ((MasterListItem)CmbAnesthesiaType.SelectedItem).ID == 0)
                {
                    msgText = "Please select recommended anesthesia type";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbAnesthesiaType.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbAnesthesiaType.Focus();
                    return false;
                }
                if ((CmbOT.ItemsSource) == null)
                {
                    msgText = "Please select operation theatre";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOT.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbOT.Focus();
                    return false;
                }
                if (((MasterListItem)CmbOT.SelectedItem) == null || ((MasterListItem)CmbOT.SelectedItem).ID == 0)
                {
                    msgText = "Please select operation theatre";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOT.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbOT.Focus();
                    return false;
                }
                if ((CmbOTTable.ItemsSource) == null)
                {
                    msgText = "Please select operation table";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOTTable.BorderBrush = new SolidColorBrush(Colors.Red);
                    return false;
                }
                if (((MasterListItem)CmbOTTable.SelectedItem) == null || ((MasterListItem)CmbOTTable.SelectedItem).ID == 0)
                {
                    msgText = "Please select operation table";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOTTable.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbOTTable.Focus();
                    return false;
                }
                if ((CmbServiceName.ItemsSource) == null)
                {
                    msgText = "Please select service name";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOTTable.BorderBrush = new SolidColorBrush(Colors.Red);
                    return false;
                }
                if (((MasterListItem)CmbServiceName.SelectedItem) == null || ((MasterListItem)CmbServiceName.SelectedItem).ID == 0)
                {
                    msgText = "Please select service name";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbOTTable.BorderBrush = new SolidColorBrush(Colors.Red);
                    CmbOTTable.Focus();
                    return false;
                }
                if (dgItemDetailsList.ItemsSource != null)
                {
                    foreach (clsProcedureItemDetailsVO item in dgItemDetailsList.ItemsSource)
                    {
                        // BizAction.ProcDetails.ServiceList.Add(item);
                        if (item.Quantity  == 0 || item.Quantity < 0)
                        {
                            msgText = "Please Enter Quantity In Item Details.";
                            //msgText = localizationManager.GetValue("RecordSaved_Msg");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// This function get called if user clicks ok on save message
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
            else
            {
                ClickedFlag = 0;
            }
        }

        /// <summary>
        ///  Save pathoTestMaster
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <param name="?"></param>
        /// <returns></returns>

        private void Save()
        {
            try
            {
                clsAddProcedureMasterBizActionVO BizAction = new clsAddProcedureMasterBizActionVO();
                BizAction.ProcDetails = (clsProcedureMasterVO)grdBackPanel.DataContext;
                BizAction.ProcDetails.Status = true;

                if (CmbAnesthesiaType.SelectedItem != null)
                    BizAction.ProcDetails.RecommandedAnesthesiaTypeID = ((MasterListItem)CmbAnesthesiaType.SelectedItem).ID;

                if (CmbProcedureType.SelectedItem != null)
                    BizAction.ProcDetails.ProcedureTypeID = ((MasterListItem)CmbProcedureType.SelectedItem).ID;

                if (CmbOT.SelectedItem != null)
                    BizAction.ProcDetails.OperationTheatreID = ((MasterListItem)CmbOT.SelectedItem).ID;

                if (CmbOTTable.SelectedItem != null)
                    BizAction.ProcDetails.OTTableID = ((MasterListItem)CmbOTTable.SelectedItem).ID;

                if (CmbServiceName.SelectedItem != null)
                    BizAction.ProcDetails.ServiceID = ((MasterListItem)CmbServiceName.SelectedItem).ID;


                if (dgCheckList.ItemsSource != null)
                {
                    foreach (clsProcedureChecklistDetailsVO item in dgCheckList.ItemsSource)
                    {
                        if ((item).Status == true)
                        {
                            item.ChecklistUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            BizAction.ProcDetails.CheckList.Add(item);
                        }
                    }
                }

                if (dgDocClassificationList.ItemsSource != null)
                {
                    foreach (clsProcedureStaffDetailsVO item in dgDocClassificationList.ItemsSource)
                    {
                        clsProcedureStaffDetailsVO docObject = new clsProcedureStaffDetailsVO();
                        docObject.IsDoctor = true;
                        docObject.DocOrStaffCode = (item).DocOrStaffCode;
                        docObject.NoofDocOrStaff = (item).NoofDocOrStaff;
                        docObject.DocClassification = item.DocClassification;
                        BizAction.ProcDetails.StaffList.Add(docObject);
                    }
                }

                if (dgStaffList.ItemsSource != null)
                {
                    foreach (clsProcedureStaffDetailsVO item in dgStaffList.ItemsSource)
                    {
                        clsProcedureStaffDetailsVO staffObject = new clsProcedureStaffDetailsVO();
                        staffObject.IsDoctor = false;
                        staffObject.DocOrStaffCode = item.DocOrStaffCode;
                        staffObject.NoofDocOrStaff = (item).NoofDocOrStaff;
                        staffObject.DocClassification = item.DocClassification;
                        BizAction.ProcDetails.StaffList.Add(staffObject);
                    }
                }

                BizAction.ProcDetails.InstructionList = new List<clsProcedureInstructionDetailsVO>();
                if (((SearchResultList)PreOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)PreOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO preOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        preOperativeInstructioObj.InstructionTypeID = 2;
                        preOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(preOperativeInstructioObj);
                    }
                }
                if (((SearchResultList)PostOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)PostOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO postOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        postOperativeInstructioObj.InstructionTypeID = 3;
                        postOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(postOperativeInstructioObj);
                    }
                }
                if (((SearchResultList)IntraOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)IntraOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO IntraOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        IntraOperativeInstructioObj.InstructionTypeID = 1;
                        IntraOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(IntraOperativeInstructioObj);
                    }
                }

                if (((SearchResultList)ConsentResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)ConsentResultListContent.Content).lstObjectList)
                    {
                        clsProcedureConsentDetailsVO ConsentObject = new clsProcedureConsentDetailsVO();
                        ConsentObject.ConsentID = item.ID;
                        BizAction.ProcDetails.ConsentList.Add(ConsentObject);
                    }
                }

                if (dgItemDetailsList.ItemsSource != null)
                {
                    foreach (clsProcedureItemDetailsVO item in dgItemDetailsList.ItemsSource)
                    {
                        BizAction.ProcDetails.ItemList.Add((clsProcedureItemDetailsVO)item);
                     double a = item.Quantity;
                    }
                }

                if (dgServiceList.ItemsSource != null)
                {
                    foreach (clsDoctorSuggestedServiceDetailVO item in dgServiceList.ItemsSource)
                    {
                        BizAction.ProcDetails.ServiceList.Add(item);
                          

                    }

                }
                if (PocedureTempateList != null && PocedureTempateList.Count() > 0)
                    BizAction.ProcDetails.ProcedureTempalateList = PocedureTempateList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            msgText = "Record Saved Successfully.";
                            //msgText = localizationManager.GetValue("RecordSaved_Msg");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {
                            //if (localizationManager != null)
                            //{
                            //    msgText = localizationManager.GetValue("CodeDuplicate_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record cannot be save because CODE already exist!";

                            //}

                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                        else if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {

                            //if (localizationManager != null)
                            //{
                            //    msgText = localizationManager.GetValue("DescriptionDuplicate_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record cannot be save because Description already exist!";

                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        private void FetchData()
        {
            try
            {
                clsGetProcedureMasterBizActionVO BizAction = new clsGetProcedureMasterBizActionVO();
                BizAction.ProcDetails = new List<clsProcedureMasterVO>();

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    BizAction.Description = txtSearch.Text;
                }
                if (CmbProcedureTypeSearch.SelectedItem != null)
                {
                    BizAction.ProcedureTypeID = ((MasterListItem)CmbProcedureTypeSearch.SelectedItem).ID;
                }
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && ((clsGetProcedureMasterBizActionVO)arg.Result).ProcDetails != null)
                    {
                        clsGetProcedureMasterBizActionVO result = arg.Result as clsGetProcedureMasterBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.ProcDetails != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.ProcDetails)
                            {
                                DataList.Add(item);
                            }
                            grdPathoTest.ItemsSource = null;
                            grdPathoTest.ItemsSource = DataList;
                            dataGrid2Pager.IsEnabled = true;
                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = result.MaximumRows;
                            dataGrid2Pager.Source = DataList;
                        }
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    bool ModifyTest = true;
                    ModifyTest = ValidateBackPanel();
                    if (ModifyTest == true)
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("UpdateVerification_Msg");
                        //}
                        //else
                        //{
                        msgText = "Are you sure you want to Update ?";
                        //}
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                        msgW1.Show();
                    }
                    else
                        ClickedFlag = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        ///  This function get called if user clicks ok on modify message
        /// </summary>
        /// <param name="result">MessageBoxResult </param>
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Modify();
                ClickedFlag = 0;
            }
            else
            {
                ClickedFlag = 0;
            }
        }
        /// <summary>
        /// Modified ProcedureMaster
        /// </summary>
        private void Modify()
        {
            try
            {
                clsAddProcedureMasterBizActionVO BizAction = new clsAddProcedureMasterBizActionVO();
                BizAction.ProcDetails = (clsProcedureMasterVO)grdBackPanel.DataContext;
                BizAction.ProcDetails.Status = true;

                if (CmbAnesthesiaType.SelectedItem != null)
                    BizAction.ProcDetails.RecommandedAnesthesiaTypeID = ((MasterListItem)CmbAnesthesiaType.SelectedItem).ID;

                if (CmbProcedureType.SelectedItem != null)
                    BizAction.ProcDetails.ProcedureTypeID = ((MasterListItem)CmbProcedureType.SelectedItem).ID;

                if (CmbOT.SelectedItem != null)
                    BizAction.ProcDetails.OperationTheatreID = ((MasterListItem)CmbOT.SelectedItem).ID;

                if (CmbOTTable.SelectedItem != null)
                    BizAction.ProcDetails.OTTableID = ((MasterListItem)CmbOTTable.SelectedItem).ID;

                if (CmbServiceName.SelectedItem != null)
                    BizAction.ProcDetails.ServiceID= ((MasterListItem)CmbServiceName.SelectedItem).ID;

                if (dgCheckList.ItemsSource != null)
                {
                    foreach (clsProcedureChecklistDetailsVO item in dgCheckList.ItemsSource)
                    {
                        BizAction.ProcDetails.CheckList.Add(item);
                    }
                }

                if (dgDocClassificationList.ItemsSource != null)
                {
                    foreach (clsProcedureStaffDetailsVO item in dgDocClassificationList.ItemsSource)
                    {
                        clsProcedureStaffDetailsVO docObject = new clsProcedureStaffDetailsVO();
                        docObject.IsDoctor = true;
                        docObject.DocOrStaffCode = item.DocOrStaffCode;
                        docObject.NoofDocOrStaff = item.NoofDocOrStaff;
                        docObject.DocClassification = item.DocClassification;
                        BizAction.ProcDetails.StaffList.Add(docObject);
                    }
                }
                if (dgStaffList.ItemsSource != null)
                {
                    foreach (clsProcedureStaffDetailsVO item in dgStaffList.ItemsSource)
                    {
                        clsProcedureStaffDetailsVO staffObject = new clsProcedureStaffDetailsVO();
                        staffObject.IsDoctor = false;
                        staffObject.DocOrStaffCode = Convert.ToString(item.DocOrStaffCode);
                        staffObject.NoofDocOrStaff = item.NoofDocOrStaff;
                        staffObject.DocClassification = item.DocClassification;
                        BizAction.ProcDetails.StaffList.Add(staffObject);
                    }
                }

                if (((SearchResultList)PreOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)PreOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO preOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        preOperativeInstructioObj.InstructionTypeID = 2;
                        preOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(preOperativeInstructioObj);
                    }
                }

                if (((SearchResultList)PostOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)PostOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO postOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        postOperativeInstructioObj.InstructionTypeID = 3;
                        postOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(postOperativeInstructioObj);
                    }
                }

                if (((SearchResultList)IntraOPResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)IntraOPResultListContent.Content).lstObjectList)
                    {
                        clsProcedureInstructionDetailsVO IntraOperativeInstructioObj = new clsProcedureInstructionDetailsVO();
                        IntraOperativeInstructioObj.InstructionTypeID = 1;
                        IntraOperativeInstructioObj.InstructionID = item.ID;
                        BizAction.ProcDetails.InstructionList.Add(IntraOperativeInstructioObj);
                    }
                }

                if (((SearchResultList)ConsentResultListContent.Content).lstObjectList != null)
                {
                    foreach (var item in ((SearchResultList)ConsentResultListContent.Content).lstObjectList)
                    {
                        clsProcedureConsentDetailsVO ConsentObject = new clsProcedureConsentDetailsVO();
                        ConsentObject.ConsentID = item.ID;
                        BizAction.ProcDetails.ConsentList.Add(ConsentObject);
                    }
                }
                if (dgItemDetailsList.ItemsSource != null)
                {
                    foreach (var item in dgItemDetailsList.ItemsSource)
                    {
                        BizAction.ProcDetails.ItemList.Add((clsProcedureItemDetailsVO)item);
                    }
                }              

                if (dgServiceList.ItemsSource != null)
                {
                    foreach (clsDoctorSuggestedServiceDetailVO item in dgServiceList.ItemsSource)
                    {
                        BizAction.ProcDetails.ServiceList.Add(item);
                    }
                }
                if (PocedureTempateList != null && PocedureTempateList.Count() > 0)
                    BizAction.ProcDetails.ProcedureTempalateList = PocedureTempateList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Modify");
                            objAnimation.Invoke(RotationType.Backward);
                            //if (localizationManager != null)
                            //{
                            //    msgText = localizationManager.GetValue("RecordModify_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record Updated Successfully.";
                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        }
                        else if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            //if (localizationManager != null)
                            //{
                            //    msgText = localizationManager.GetValue("CodeDuplicate_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record cannot be save because CODE already exist!";

                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                        else if (((clsAddProcedureMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {
                            //if (localizationManager != null)
                            //{
                            //    msgText = localizationManager.GetValue("DescDuplicate_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record cannot be save because Description already exist!";

                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                FetchData();
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmOTConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "OT Configuration";

                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = "  ";
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// When cancel button click on front panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //myData = asm.CreateInstance("PalashDynamics.Administration.frmOTConfiguration") as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void c_OpenReadCompletedForChild(object sender, OpenReadCompletedEventArgs e)
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
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        bool isView = false;
        /// <summary>
        /// View hyperlink on front panel clicked
        /// </summary>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            string strLabel = string.Empty;
            try
            {
                SetCommandButtonState("View");
                cmdModify.IsEnabled = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).Status;
                ClearUI();
                isView = true;
                if (grdPathoTest.SelectedItem != null)
                { //For PreOperative Insructions
                    PreOPResultListContent.Content = new SearchResultList();
                    ((SearchResultList)PreOPResultListContent.Content).TableName = MasterTableNameList.M_PreOperativeInstructionsMaster;
                    strLabel = "Pre-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblPreOperativeInstructions");
                    ((SearchResultList)PreOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;

                    //For PostOperative Insructions
                    PostOPResultListContent.Content = new SearchResultList();
                    ((SearchResultList)PostOPResultListContent.Content).TableName = MasterTableNameList.M_PostOperativeInstructionsMaster;
                    strLabel = "Post-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblPostOperativeInstructions");
                    ((SearchResultList)PostOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;

                    //For IntraOperative Insructions
                    IntraOPResultListContent.Content = new SearchResultList();
                    ((SearchResultList)IntraOPResultListContent.Content).TableName = MasterTableNameList.M_IntraOperativeInstructionsMaster;
                    strLabel = "Intra-Operative Instructions";
                    //strLabel = localizationManager.GetValue("lblIntraOperativeInstructions");
                    ((SearchResultList)IntraOPResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;

                    //For Consent
                    ConsentResultListContent.Content = new SearchResultList();
                    //((SearchResultList)ConsentResultListContent.Content).TableName = MasterTableNameList.M_ConsentMaster; //***//
                    ((SearchResultList)ConsentResultListContent.Content).TableName = MasterTableNameList.M_PatientConsentMaster;
                    strLabel = "Consent";
                    //strLabel = localizationManager.GetValue("tbhConsent");
                    ((SearchResultList)ConsentResultListContent.Content).labelOfDescriptionOnGrid = strLabel;
                    strLabel = string.Empty;


                    this.DataContext = ((clsProcedureMasterVO)grdPathoTest.SelectedItem);

                    ServiceID = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).ServiceID;
                    CmbServiceName.SelectedValue = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).ServiceID;
                    CmbProcedureType.SelectedValue = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).ProcedureTypeID;
                    CmbAnesthesiaType.SelectedValue = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).RecommandedAnesthesiaTypeID;
                    CmbOT.SelectedValue = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).OperationTheatreID;
                    CmbOTTable.SelectedValue = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).OTTableID;
                    if (((clsProcedureMasterVO)grdPathoTest.SelectedItem).ID > 0)
                    {
                        FillDetailTablesOfProcedure(((clsProcedureMasterVO)grdPathoTest.SelectedItem).ID);
                    }
                    objAnimation.Invoke(RotationType.Forward);
                    TabControlMain.SelectedIndex = 0;
                }

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + ((clsProcedureMasterVO)grdPathoTest.SelectedItem).Description;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fill Detail tables of procedure
        /// </summary>
        private void FillDetailTablesOfProcedure(long procID)
        {
            try
            {
                clsGetProcDetailsByProcIDBizActionVO BizAction = new clsGetProcDetailsByProcIDBizActionVO();
                BizAction.CheckList = new List<clsProcedureChecklistDetailsVO>();
                BizAction.ConsentList = new List<clsProcedureConsentDetailsVO>();
                BizAction.InstructionList = new List<clsProcedureInstructionDetailsVO>();
                BizAction.ItemList = new List<clsProcedureItemDetailsVO>();
                //BizAction.EquipList = new List<clsProcedureEquipmentDetailsVO>();
                BizAction.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
                BizAction.StaffList = new List<clsProcedureStaffDetailsVO>();
                BizAction.ProcID = procID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetProcDetailsByProcIDBizActionVO DetailsVO = new clsGetProcDetailsByProcIDBizActionVO();
                            DetailsVO.CheckList = new List<clsProcedureChecklistDetailsVO>();
                            DetailsVO = (clsGetProcDetailsByProcIDBizActionVO)arg.Result;
                            if (DetailsVO.CheckList != null)
                            {
                                myList.Clear();
                                myList = DetailsVO.CheckList;
                                pcv = new PagedCollectionView(myList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                                dgCheckList.ItemsSource = pcv;
                                if (CmbCategory.ItemsSource != null)
                                    CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
                                if (CmbSubCategory.ItemsSource != null)
                                    CmbSubCategory.SelectedValue = ((List<MasterListItem>)CmbSubCategory.ItemsSource)[0].ID;
                                if (CmbCheckList.ItemsSource != null)
                                    CmbCheckList.SelectedValue = ((List<MasterListItem>)CmbCheckList.ItemsSource)[0].ID;
                            }
                            if (DetailsVO.ConsentList.Count > 0)
                            {
                                ((SearchResultList)ConsentResultListContent.Content).grid2ViewList = null;
                                ((SearchResultList)ConsentResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                                foreach (var item in DetailsVO.ConsentList)
                                {
                                    MasterListItem ConsentObj = new MasterListItem();
                                    ConsentObj.ID = item.ConsentID;
                                    ConsentObj.Description = item.ConsentDesc;
                                    //ConsentObj.Description = item.TemplateName;
                                    ((SearchResultList)ConsentResultListContent.Content).grid2ViewList.Add(ConsentObj);
                                }
                                ((SearchResultList)ConsentResultListContent.Content).FillGrid2();
                            }
                            if (DetailsVO.InstructionList != null && DetailsVO.InstructionList.Count > 0)
                            {
                                ((SearchResultList)IntraOPResultListContent.Content).grid2ViewList = null;
                                ((SearchResultList)IntraOPResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                                foreach (var item in DetailsVO.InstructionList)
                                {
                                    MasterListItem InstructionObj = new MasterListItem();
                                    InstructionObj.ID = item.InstructionID;
                                    InstructionObj.Description = item.InstructionDesc;

                                    ((SearchResultList)IntraOPResultListContent.Content).grid2ViewList.Add(InstructionObj);
                                }
                                ((SearchResultList)IntraOPResultListContent.Content).FillGrid2();
                            }
                            if (DetailsVO.PreInstructionList != null && DetailsVO.PreInstructionList.Count > 0)
                            {
                                ((SearchResultList)PreOPResultListContent.Content).grid2ViewList = null;
                                ((SearchResultList)PreOPResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                                foreach (var item in DetailsVO.PreInstructionList)
                                {
                                    MasterListItem InstructionObj = new MasterListItem();
                                    InstructionObj.ID = item.InstructionID;
                                    InstructionObj.Description = item.InstructionDesc;


                                    ((SearchResultList)PreOPResultListContent.Content).grid2ViewList.Add(InstructionObj);
                                }
                                ((SearchResultList)PreOPResultListContent.Content).FillGrid2();
                            }
                            if (DetailsVO.PostInstructionList != null && DetailsVO.PostInstructionList.Count > 0)
                            {
                                ((SearchResultList)PostOPResultListContent.Content).grid2ViewList = null;
                                ((SearchResultList)PostOPResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                                foreach (var item in DetailsVO.PostInstructionList)
                                {
                                    MasterListItem InstructionObj = new MasterListItem();
                                    InstructionObj.ID = item.InstructionID;
                                    InstructionObj.Description = item.InstructionDesc;

                                    ((SearchResultList)PostOPResultListContent.Content).grid2ViewList.Add(InstructionObj);
                                }
                                ((SearchResultList)PostOPResultListContent.Content).FillGrid2();
                            }
                            if (DetailsVO.ItemList != null)
                            {
                                foreach (var item in DetailsVO.ItemList)
                                {
                                    ItemList1.Add(item);
                                }
                                dgItemDetailsList.ItemsSource = null;
                                dgItemDetailsList.ItemsSource = ItemList1;
                                dgItemDetailsList.Focus();
                                dgItemDetailsList.UpdateLayout();
                            }

                            if (DetailsVO.ServiceList.Count > 0)
                            {
                                foreach (var item in DetailsVO.ServiceList)
                                {
                                    clsDoctorSuggestedServiceDetailVO ServiceObj = new clsDoctorSuggestedServiceDetailVO();
                                    ServiceObj.ServiceCode = item.ServiceCode;
                                    ServiceObj.ServiceName = item.ServiceName;
                                    ServiceObj.GroupName = item.GroupName;
                                    ServiceObj.ServiceType = item.ServiceType;
                                    ServiceList.Add(ServiceObj);
                                }

                                dgServiceList.ItemsSource = null;
                                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                                dgServiceList.ItemsSource = pcv;
                                //dgServiceList.ItemsSource = ServiceList;
                                dgServiceList.UpdateLayout();
                                dgServiceList.Focus();
                            }

                            if (DetailsVO.StaffList.Count > 0)
                            {
                                foreach (var item in DetailsVO.StaffList)
                                {
                                    if (item.IsDoctor == true)
                                    {
                                        StaffList.Add(item);
                                    }
                                    else
                                    {
                                        StaffListOther.Add(item);
                                    }
                                }
                                dgDocClassificationList.ItemsSource = null;
                                dgDocClassificationList.ItemsSource = StaffList;
                                dgDocClassificationList.Focus();
                                dgDocClassificationList.UpdateLayout();
                                dgStaffList.ItemsSource = null;
                                dgStaffList.ItemsSource = StaffListOther;
                                dgStaffList.Focus();
                                dgStaffList.UpdateLayout();
                            }
                            if (DetailsVO.ProcedureTempalateList != null && DetailsVO.ProcedureTempalateList.Count > 0)
                            {
                                foreach (var item in DetailsVO.ProcedureTempalateList)
                                {
                                    PocedureTempateList.Add(item);
                                }
                                dgProcedureTempate.ItemsSource = null;
                                dgProcedureTempate.ItemsSource = PocedureTempateList;
                                dgProcedureTempate.Focus();
                                dgProcedureTempate.UpdateLayout();
                            }
                        }
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        /// <summary>
        /// Deletes the Items
        /// </summary>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemDetailsList.SelectedItem != null)
                {
                    msgText = "Are you sure you want to delete record ?";
                    //msgText = localizationManager.GetValue("DeleteValidation_Msg");
                    int index = dgItemDetailsList.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ItemList1.RemoveAt(dgItemDetailsList.SelectedIndex);
                            if (ItemList1.Count == 0)
                            {
                                dgItemDetailsList.ItemsSource = null;
                            }
                            else
                            {
                                dgItemDetailsList.ItemsSource = null;
                                dgItemDetailsList.ItemsSource = ItemList1;
                                dgItemDetailsList.UpdateLayout();
                            }
                        }
                    };
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds Item
        /// </summary>
        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                //ModuleName = "EMR";
                //Action = "EMR.frmEMRMedicationDrugSelectionList";
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompletedForChild);
                //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


                EMR.frmEMRMedDrugSelectionList WinDrugList = new frmEMRMedDrugSelectionList();
                //EMR.frmEMRMedDrugSelectionList WinDrugList = new EMR.frmEMRMedDrugSelectionList();
                WinDrugList.CurrentVisit = new ValueObjects.OutPatientDepartment.clsVisitVO();
                WinDrugList.Height = this.ActualHeight * 0.8;
                WinDrugList.Width = this.ActualWidth * 0.8;
                WinDrugList.Isprocedure = true;
                WinDrugList.OnAddButton_Click += new RoutedEventHandler(WinDrug_OnAddButton_Click);
                WinDrugList.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        List<clsProcedureItemDetailsVO> ItemList = new List<clsProcedureItemDetailsVO>();
        void WinDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((EMR.frmEMRMedDrugSelectionList)sender).DialogResult == true)
            {
                foreach (var item in (((EMR.frmEMRMedDrugSelectionList)sender).DrugList))
                {
                    if (ItemList1.Count > 0)
                    {
                        var item1 = from r in ItemList1
                                    where (r.ItemName.Trim() == item.Description.Trim() && r.ItemID == item.ID)
                                    select new clsItemMasterVO
                                    {
                                        ItemCode = r.ItemCode,
                                        ItemName = r.ItemName,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
                            //OBj.ItemCode = item.DrugCode;
                            OBj.ItemID = item.ID;
                            OBj.ItemName = item.Description;
                            OBj.ItemCode = item.ID.ToString();
                            ItemList1.Add(OBj);
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList1;
                        }
                    }
                    else
                    {
                        clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
                        OBj.ItemID = item.ID;
                        OBj.ItemName = item.Description;
                        OBj.ItemCode = item.ID.ToString();
                        ItemList1.Add(OBj);
                        dgItemDetailsList.ItemsSource = null;
                        dgItemDetailsList.ItemsSource = ItemList1;
                    }
                }

            }

            //if (((EMR.frmEMRMedDrugSelectionList)sender).DialogResult == true)
            //{
            //    foreach (var item in (((EMR.frmEMRMedDrugSelectionList)sender).DrugList))
            //    {
            //        if (ItemList.Count > 0)
            //        {
            //            var item1 = from r in ItemList
            //                        where (r.ItemName.Trim() == item.DrugName.Trim() && r.ItemCode == item.DrugCode)
            //                        select new clsItemMasterVO
            //                        {
            //                            ItemCode = r.ItemCode,
            //                            ItemName = r.ItemName,
            //                            Status = r.Status
            //                        };
            //            if (item1.ToList().Count == 0)
            //            {
            //                clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
            //                OBj.ItemCode = item.DrugCode;
            //                OBj.ItemName = item.DrugName;
            //                ItemList.Add(OBj);
            //                dgItemDetailsList.ItemsSource = null;
            //                dgItemDetailsList.ItemsSource = ItemList;
            //            }
            //        }
            //        else
            //        {
            //            clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
            //            OBj.ItemName = item.DrugName;
            //            OBj.ItemCode = item.DrugCode;
            //            ItemList.Add(OBj);
            //            dgItemDetailsList.ItemsSource = null;
            //            dgItemDetailsList.ItemsSource = ItemList;
            //        }
            //    }

            //}
        }

        /// <summary>
        /// Adds items to the item grid
        /// </summary>
        /// <param name="sender">Item search ok button</param>
        /// <param name="e">Item search ok button click</param>

        public ObservableCollection<clsProcedureItemDetailsVO> ItemList1 { get; set; }
        public ObservableCollection<clsProcedureItemDetailsVO> KitList { get; set; }
        /// <summary>
        /// Item search window ok button click
        /// </summary>
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                foreach (var item in Itemswin.SelectedItems)
                {

                    if (ItemList1.Where(Items => Items.ItemID == item.ID).Any() == false)
                        ItemList1.Add(
                        new clsProcedureItemDetailsVO()
                        {
                            ItemID = item.ID,
                            ItemName = item.ItemName,
                            Status = item.Status,
                            ItemCode=item.ItemCode

                        });
                }

                dgItemDetailsList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = ItemList1;
            }
        }

        /// <summary>
        /// New button click
        /// </summary>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // FillService();
                this.SetCommandButtonState("New");
                ClearUI();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New Procedure Details";
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fill OT Table according to OT 
        /// </summary>
        private void CmbOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbOT.SelectedItem != null && ((MasterListItem)CmbOT.SelectedItem).ID != 0)
                {

                    FetchOTTable(((MasterListItem)CmbOT.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        PagedCollectionView pcv = null;
        List<clsProcedureChecklistDetailsVO> myList = new List<clsProcedureChecklistDetailsVO>();

        /// <summary>
        /// Adds checklist item
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateCheckList())
                {
                    if (((MasterListItem)CmbCategory.SelectedItem).ID > 0 && ((MasterListItem)CmbSubCategory.SelectedItem).ID > 0 && ((MasterListItem)CmbCheckList.SelectedItem).ID > 0)
                    {
                        myList.Add(new clsProcedureChecklistDetailsVO() { UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID, Category = ((MasterListItem)CmbCategory.SelectedItem).Description, SubCategory = ((MasterListItem)CmbSubCategory.SelectedItem).Description, SubCategoryId = ((MasterListItem)CmbSubCategory.SelectedItem).ID, SubCategory1 = ((MasterListItem)CmbSubCategory.SelectedItem).Description, Name = ((MasterListItem)CmbCheckList.SelectedItem).Description, CheckListId = ((MasterListItem)CmbCheckList.SelectedItem).ID, Status = true, Remarks = "" });
                        pcv = new PagedCollectionView(myList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));

                        dgCheckList.ItemsSource = pcv;
                        CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
                        CmbSubCategory.SelectedValue = ((List<MasterListItem>)CmbSubCategory.ItemsSource)[0].ID;
                        CmbCheckList.SelectedValue = ((List<MasterListItem>)CmbCheckList.ItemsSource)[0].ID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private bool ValidateCheckList()
        {
            bool result = true;
            try
            {
                if (CmbCategory.ItemsSource == null)
                {
                    msgText = "Please Select Category";
                    CmbCategory.TextBox.SetValidation(msgText);
                    CmbCategory.TextBox.RaiseValidationError();
                    CmbCategory.Focus();
                    result = false;
                }
                else if ((MasterListItem)CmbCategory.SelectedItem == null || ((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                {
                    msgText = "Please Select Category";
                    CmbCategory.TextBox.SetValidation(msgText);
                    CmbCategory.TextBox.RaiseValidationError();
                    CmbCategory.Focus();
                    result = false;
                }
                else if (CmbSubCategory.ItemsSource == null)
                {
                    msgText = "Please Select Sub Category";
                    CmbSubCategory.TextBox.SetValidation(msgText);
                    CmbSubCategory.TextBox.RaiseValidationError();
                    CmbSubCategory.Focus();
                    result = false;
                }
                else if ((MasterListItem)CmbSubCategory.SelectedItem == null || ((MasterListItem)CmbSubCategory.SelectedItem).ID == 0)
                {
                    msgText = "Please Select Sub Category";
                    CmbSubCategory.TextBox.SetValidation(msgText);
                    CmbSubCategory.TextBox.RaiseValidationError();
                    CmbSubCategory.Focus();
                    result = false;
                }
                else if (CmbCheckList.ItemsSource == null)
                {
                    msgText = "Please Select Check List";
                    CmbCheckList.TextBox.SetValidation(msgText);
                    CmbCheckList.TextBox.RaiseValidationError();
                    CmbCheckList.Focus();
                    result = false;
                }
                else if ((MasterListItem)CmbCheckList.SelectedItem == null || ((MasterListItem)CmbCheckList.SelectedItem).ID == 0)
                {
                    msgText = "Please Select Check List";
                    CmbCheckList.TextBox.SetValidation(msgText);
                    CmbCheckList.TextBox.RaiseValidationError();
                    CmbCheckList.Focus();
                    result = false;
                }

                //}
                //else
                //cmbDoctorClassification.TextBox.ClearValidationError();



                //}
                //else
                //CmbSubCategory.TextBox.ClearValidationError();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes checklist item
        /// </summary>
        private void cmdDeleteCheckListItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCheckList.SelectedItem != null)
                {
                    msgText = "Are you sure you want to delete record ?";
                    //msgText = localizationManager.GetValue("DeleteValidation_Msg");
                    int index = dgCheckList.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            myList.RemoveAt(dgCheckList.SelectedIndex);
                            dgCheckList.ItemsSource = null;
                            pcv = new PagedCollectionView(myList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                            dgCheckList.ItemsSource = pcv;
                            dgCheckList.UpdateLayout();
                        }
                    };
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ObservableCollection<clsProcedureStaffDetailsVO> StaffList { get; set; }
        public ObservableCollection<clsProcedureStaffDetailsVO> StaffListOther { get; set; }

        /// <summary>
        /// Adds staff
        /// </summary>
        private void cmdAddStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateStaff())
                {
                    clsProcedureStaffDetailsVO tempDoc = new clsProcedureStaffDetailsVO();


                    var staff = from r in StaffList
                                where (r.DocOrStaffCode == ((MasterListItem)cmbDoctorClassification.SelectedItem).Code)
                                select new clsProcedureStaffDetailsVO
                                {
                                    Status = r.Status,
                                    DocOrStaffID = r.DocOrStaffID,
                                    DocOrStaffCode = r.DocOrStaffCode,
                                    NoofDocOrStaff = r.NoofDocOrStaff,
                                    DocClassification = ((MasterListItem)cmbDoctorClassification.SelectedItem).Description,
                                };


                    if (staff.ToList().Count == 0)
                    {
                        tempDoc.DocOrStaffCode = ((MasterListItem)cmbDoctorClassification.SelectedItem).Code;
                        tempDoc.DocClassification = ((MasterListItem)cmbDoctorClassification.SelectedItem).Description;
                        //tempDoc.NoofDocOrStaff = Convert.ToInt64(txtDoctorQuantity.Text);

                        tempDoc.Status = true;
                        StaffList.Add(tempDoc);

                        dgDocClassificationList.ItemsSource = null;
                        dgDocClassificationList.ItemsSource = StaffList;

                        cmbDoctorClassification.SelectedValue = (long)0;
                        txtDoctorQuantity.Text = string.Empty;

                    }
                    else
                    {
                        msgText = "Entry already present";
                        //msgText = localizationManager.GetValue("EntryAlreadyPresentValidation_Msg");
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// validates Staff
        /// </summary>
        /// <returns></returns>
        private bool ValidateStaff()
        {
            bool result = true;
            try
            {
                //if (txtDoctorQuantity.Text == "")
                //{
                //    if (localizationManager != null)
                //    {
                //        msgText = localizationManager.GetValue("DoctorQuantityValidation_Msg");
                //    }
                //    else
                //    {
                //        msgText = "Please Enter Doctor Quantity";
                //    }
                //    txtDoctorQuantity.SetValidation(msgText);
                //    txtDoctorQuantity.RaiseValidationError();
                //    txtDoctorQuantity.Focus();
                //    result = false;
                //}
                //else
                //    txtDoctorQuantity.ClearValidationError();

                if (cmbDoctorClassification.SelectedItem == null || ((MasterListItem)cmbDoctorClassification.SelectedItem).ID == 0)
                {
                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("DoctorClassificationValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please Select Doctor Specialization";
                    //}
                    cmbDoctorClassification.TextBox.SetValidation(msgText);
                    cmbDoctorClassification.TextBox.RaiseValidationError();
                    cmbDoctorClassification.Focus();
                    result = false;
                }

                else
                    cmbDoctorClassification.TextBox.ClearValidationError();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// deletes doctor
        /// </summary>
        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDocClassificationList.SelectedItem != null)
                {
                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("DeleteValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are you sure you want to Delete ?";
                    //}
                    int index = dgDocClassificationList.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            StaffList.RemoveAt(index);
                            cmdAddStaff.IsEnabled = true;
                            cmdModifyStaff.IsEnabled = false;
                        }
                    };

                    msgWD.Show();
                    //dgItemDetailsList.ItemsSource = null;
                    //dgItemDetailsList.ItemsSource = StaffList;

                    dgDocClassificationList.ItemsSource = null;
                    dgDocClassificationList.ItemsSource = StaffList;
                }
                  
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Edits doctor
        /// </summary>
        private void hlbEditDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbDoctorClassification.SelectedValue = Convert.ToInt64(((clsProcedureStaffDetailsVO)dgDocClassificationList.SelectedItem).DocOrStaffCode);
                txtDoctorQuantity.Text = ((clsProcedureStaffDetailsVO)dgDocClassificationList.SelectedItem).NoofDocOrStaff.ToString();
                SetCommandButtonState("EditDoc");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds staff
        /// </summary>
        private void cmdOtherStaffAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateStaffOther())
                {
                    clsProcedureStaffDetailsVO tempStaff = new clsProcedureStaffDetailsVO();


                    var staff = from r in StaffListOther
                                where (r.DocOrStaffCode == ((MasterListItem)cmbStaff.SelectedItem).Code)
                                select new clsProcedureStaffDetailsVO
                                {
                                    Status = r.Status,
                                    DocOrStaffID = r.DocOrStaffID,
                                    DocOrStaffCode = r.DocOrStaffCode,
                                    NoofDocOrStaff = r.NoofDocOrStaff,
                                    DocClassification = ((MasterListItem)cmbStaff.SelectedItem).Description,
                                };


                    if (staff.ToList().Count == 0)
                    {
                        tempStaff.DocOrStaffCode = ((MasterListItem)cmbStaff.SelectedItem).Code.ToString();
                        tempStaff.DocClassification = ((MasterListItem)cmbStaff.SelectedItem).Description;
                        //tempStaff.NoofDocOrStaff = (long)Convert.ToDouble(txtStaffQuantity.Text);

                        tempStaff.Status = true;
                        StaffListOther.Add(tempStaff);

                        dgStaffList.ItemsSource = null;
                        dgStaffList.ItemsSource = StaffListOther;

                        cmbStaff.SelectedValue = (long)0;
                        txtStaffQuantity.Text = "";

                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("EntryAlreadyPresentValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Entry Already Present";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// validates sample
        /// </summary>
        /// <returns></returns>
        private bool ValidateStaffOther()
        {
            bool result = true;
            try
            {
                //if (txtStaffQuantity.Text == "")
                //{
                //    if (localizationManager != null)
                //    {
                //        msgText = localizationManager.GetValue("StaffQuantityValidation_Msg");
                //    }
                //    else
                //    {
                //        msgText = "Please Enter Staff Quantity";
                //    }
                //    txtStaffQuantity.SetValidation(msgText);
                //    txtStaffQuantity.RaiseValidationError();
                //    txtStaffQuantity.Focus();
                //    result = false;
                //}
                //else
                //    txtStaffQuantity.ClearValidationError();

                if ((MasterListItem)cmbStaff.SelectedItem == null || ((MasterListItem)cmbStaff.SelectedItem).ID == 0)
                {

                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("StaffValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please Select Staff Designation";
                    //}
                    cmbStaff.TextBox.SetValidation(msgText);
                    cmbStaff.TextBox.RaiseValidationError();
                    cmbStaff.Focus();
                    result = false;
                }

                else
                    cmbStaff.TextBox.ClearValidationError();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Modify staff
        /// </summary>
        private void cmdModifyStaffOther_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaffListOther.Count > 0)
                {
                    if (ValidateStaffOther())
                    {
                        int var = dgStaffList.SelectedIndex;
                        StaffListOther.RemoveAt(dgStaffList.SelectedIndex);

                        StaffListOther.Insert(var, new clsProcedureStaffDetailsVO
                        {
                            DocOrStaffID = ((MasterListItem)cmbStaff.SelectedItem).ID,
                            DocOrStaffCode = ((MasterListItem)cmbStaff.SelectedItem).Code,
                            DocClassification = ((MasterListItem)cmbStaff.SelectedItem).Description,
                            NoofDocOrStaff = Convert.ToInt64(txtStaffQuantity.Text),
                            Status = true
                        }
                        );

                        dgStaffList.ItemsSource = StaffListOther;
                        dgStaffList.Focus();
                        dgStaffList.UpdateLayout();
                        dgStaffList.SelectedIndex = StaffListOther.Count - 1;
                        cmbStaff.SelectedValue = (long)0;
                        txtStaffQuantity.Text = "";
                        cmdOtherStaffAdd.IsEnabled = true;
                        cmdModifyStaffOther.IsEnabled = false;
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("MandetoryFieldsValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please Select Mandatory Fields";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Modify staff click
        /// </summary>

        private void cmdModifyStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaffList.Count > 0)
                {
                    if (ValidateStaff())
                    {
                        int var = dgDocClassificationList.SelectedIndex;
                        StaffList.RemoveAt(dgDocClassificationList.SelectedIndex);

                        StaffList.Insert(var, new clsProcedureStaffDetailsVO
                        {
                            DocOrStaffCode = ((MasterListItem)cmbDoctorClassification.SelectedItem).Code,
                            DocClassification = ((MasterListItem)cmbDoctorClassification.SelectedItem).Description,
                            NoofDocOrStaff = Convert.ToInt64(txtDoctorQuantity.Text),
                            Status = true
                        }
                        );

                        dgDocClassificationList.ItemsSource = StaffList;
                        dgDocClassificationList.Focus();
                        dgDocClassificationList.UpdateLayout();
                        dgDocClassificationList.SelectedIndex = StaffList.Count - 1;

                        cmbDoctorClassification.SelectedValue = (long)0;
                        txtDoctorQuantity.Text = string.Empty;

                        cmdModifyStaff.IsEnabled = false;
                        cmdAddStaff.IsEnabled = true;

                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("MandetoryFieldsValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please Select Mandatory Fields";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes staff
        /// </summary>
        private void cmdDeleteStaffOther_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgStaffList.SelectedItem != null)
                {
                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("DeleteValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are You Sure You Want to Delete?";
                    //}
                    int index = dgStaffList.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            StaffListOther.RemoveAt(index);
                            cmdOtherStaffAdd.IsEnabled = true;
                            cmdModifyStaffOther.IsEnabled = false;
                        }
                    };
                    msgWD.Show();
                    dgStaffList.ItemsSource = null;
                    dgStaffList.ItemsSource = StaffListOther;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Edit hyperlink on staff grid click
        /// </summary>
        private void hlbEditStaffOther_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbStaff.SelectedValue = Convert.ToInt64(((clsProcedureStaffDetailsVO)dgStaffList.SelectedItem).DocOrStaffCode);
                txtStaffQuantity.Text = ((clsProcedureStaffDetailsVO)dgStaffList.SelectedItem).NoofDocOrStaff.ToString();
                SetCommandButtonState("EditStaff");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbCategory.SelectedItem != null && ((MasterListItem)CmbCategory.SelectedItem).ID != 0)
                {
                    FillSubCategory(((MasterListItem)CmbCategory.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmbSubCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbSubCategory.SelectedItem != null && ((MasterListItem)CmbSubCategory.SelectedItem).ID != 0)
                {
                    FillCheckList(((MasterListItem)CmbSubCategory.SelectedItem).ID, ((MasterListItem)CmbCategory.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (grdPathoTest.SelectedItem != null)
            {
                clsUpdStatusProcedureMasterBizActionVO BizAction = new clsUpdStatusProcedureMasterBizActionVO();
                BizAction.ProcedureObj = new clsProcedureMasterVO();

                if (grdPathoTest.SelectedItem != null)
                {
                    BizAction.ProcedureObj.ID = ((clsProcedureMasterVO)grdPathoTest.SelectedItem).ID;
                }
                BizAction.ProcedureObj.Status = (bool)((CheckBox)sender).IsChecked;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("lblStatusUpdatedSucessfully");
                        //}
                        //else
                        //{
                        msgText = "Status Updated Successfully";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    else
                    {
                        //if (localizationManager != null)
                        //{
                        //    msgText = localizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        private void txtDoctorQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialCharAndMinus())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);

                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Special characters are not allowed.";

                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void txtDoctorQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 109)
            {
                e.Handled = true;
            }
            else
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);

                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Special characters are not allowed.";

                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        private void txtDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);

                    //if (localizationManager != null)
                    //{
                    //    msgText = localizationManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Special characters are not allowed.";

                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void txtDuration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 109 || e.PlatformKeyCode == 189)
            {
                e.Handled = true;
            }
            else
                e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        /// <summary>
        /// Add Laboratory Service
        /// </summary>
        private void cmdLab_Click(object sender, RoutedEventArgs e)
        {
            frmCPOELabServicesSelection Win = new frmCPOELabServicesSelection();
            dgServiceList.SelectedIndex = -1;
            Win.CurrentVisit = CurrentVisit;
            Win.Width = this.ActualWidth * 0.8;
            Win.Height = this.ActualHeight * 0.8;
            Win.Pathology = true;
            Win.OnAddButton_Click += new RoutedEventHandler(WinLaboratory_OnAddButton_Click);
            Win.Show();
        }

        void WinLaboratory_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            EMR.frmCPOELabServicesSelection winService = (EMR.frmCPOELabServicesSelection)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList != null)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceName == item.ServiceName //&& r.GroupName == item.Group
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        GroupName = r.GroupName,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.GroupName = "Pathology";//item.Group;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                                OBj.ServiceType = "Other";
                            ServiceList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.Priority = Priority;
                        OBj.GroupName = "Pathology";//item.Group;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                            OBj.ServiceType = "Other";

                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        /// <summary>
        /// Add Radiology Service
        /// </summary>
        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEServiceSelectionList Win = new frmCPOEServiceSelectionList();
            dgServiceList.SelectedIndex = -1;
            Win.Width = this.ActualWidth * 0.8;
            Win.Height = this.ActualHeight * 0.8;
            Win.CurrentVisit = CurrentVisit;
            Win.Radiology = true;
            Win.OnAddButton_Click += new RoutedEventHandler(WinRadiology_OnAddButton_Click);
            Win.Show();
        }

        void WinRadiology_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEServiceSelectionList winService = (frmCPOEServiceSelectionList)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList.Count > 0)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceCode == item.ServiceCode
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                                OBj.GroupName = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                            {
                                OBj.ServiceType = "Other";
                                OBj.GroupName = "Diagnostik";
                            }
                            ServiceList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.Priority = Priority;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                            OBj.GroupName = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                        {
                            OBj.ServiceType = "Other";
                            OBj.GroupName = "Diagnostik";
                        }

                        ServiceList.Add(OBj);
                    }
                }


                dgServiceList.ItemsSource = null;

                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;

                //dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        /// <summary>
        /// Add Diagnostik Service
        /// </summary>
        private void cmdDiagnostik_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList Win = new frmCPOEOtherSelectionList();
            dgServiceList.SelectedIndex = -1;
            Win.IsOther = true;
            Win.Width = this.ActualWidth * 0.8;
            Win.Height = this.ActualHeight * 0.8;
            Win.CurrentVisit = CurrentVisit;
            Win.OnAddButton_Click += new RoutedEventHandler(WinDiagnostik_OnAddButton_Click);
            Win.Show();
        }

        void WinDiagnostik_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            frmCPOEOtherSelectionList winService = (frmCPOEOtherSelectionList)sender;
            if (winService.DialogResult == true)
            {
                foreach (var item in winService.ServiceList)
                {
                    if (ServiceList.Count > 0)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceCode == item.ServiceCode
                                   )
                                    select new clsDoctorSuggestedServiceDetailVO
                                    {
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                            OBj.ServiceCode = item.ServiceCode;
                            OBj.ServiceName = item.ServiceName;
                            OBj.SpecializationCode = item.SpecializationString;
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                            if (winService.Radiology == true)
                            {
                                OBj.ServiceType = "Radiology";
                                OBj.GroupName = "Radiology";
                            }
                            else if (winService.Pathology)
                                OBj.ServiceType = "Pathology";
                            else
                            {
                                OBj.ServiceType = "Other";
                                OBj.GroupName = "Diagnostik";
                            }
                            ServiceList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                        OBj.ServiceName = item.ServiceName;
                        OBj.ServiceCode = item.ServiceCode;
                        OBj.SpecializationCode = item.SpecializationString;
                        OBj.Priority = Priority;
                        OBj.SelectedPriority = new MasterListItem(0, "-- Select --");
                        if (winService.Radiology == true)
                        {
                            OBj.ServiceType = "Radiology";
                            OBj.GroupName = "Radiology";
                        }
                        else if (winService.Pathology)
                            OBj.ServiceType = "Pathology";
                        else
                        {
                            OBj.ServiceType = "Other";
                            OBj.GroupName = "Diagnostik";
                        }

                        ServiceList.Add(OBj);
                    }
                }
                dgServiceList.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                dgServiceList.ItemsSource = pcv;
                //dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                //if (localizationManager != null)
                //{
                //    msgText = localizationManager.GetValue("DeleteValidation_Msg");
                //}
                //else
                //{
                msgText = "Are you sure you want to Delete ?";
                //}

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsDoctorSuggestedServiceDetailVO objVo = (clsDoctorSuggestedServiceDetailVO)dgServiceList.SelectedItem;
                        List<clsDoctorSuggestedServiceDetailVO> lstServices = ((dgServiceList.ItemsSource as PagedCollectionView).SourceCollection as List<clsDoctorSuggestedServiceDetailVO>).ToList();
                        var lstDelete = new List<clsDoctorSuggestedServiceDetailVO>();
                        if (objVo.ServiceType == "Pathology")
                        {
                            lstDelete = lstServices.Where(z => z.GroupName == objVo.GroupName).ToList();
                        }
                        else
                            lstDelete = lstServices.Where(z => z.ServiceName == objVo.ServiceName).ToList();
                        if (lstDelete != null && lstDelete.Count > 0)
                        {
                            foreach (clsDoctorSuggestedServiceDetailVO item in lstDelete)
                            {
                                if (objVo.SelectedService == item.SelectedService && objVo.ServiceCode == item.ServiceCode)
                                {
                                    ServiceList.Remove(item);
                                }

                            }
                        }

                        if (ServiceList.Count == 0)
                        {
                            dgServiceList.ItemsSource = null;
                        }
                        else
                        {
                            PagedCollectionView pcv = new PagedCollectionView(ServiceList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = pcv;
                            dgServiceList.UpdateLayout();
                        }
                    }
                };
                msgWD.Show();
            }
        }


        #region Procedure Template added By vikrant. on dated 10-01-2017
        public ObservableCollection<clsProcedureTemplateDetailsVO> PocedureTempateList { get; set; }
        private void FillTemplateDetails()
        {
            try
            {
                clsGetEMRTemplateListBizActionVO BizAction = new clsGetEMRTemplateListBizActionVO();
                BizAction.IsProcedureTemplate = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "- Select -"));
                        objList.AddRange(((clsGetEMRTemplateListBizActionVO)args.Result).objMasterList);
                        cmbTempate.ItemsSource = null;
                        cmbTempate.ItemsSource = objList;
                        cmbTempate.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void cmdDeleteTempateProc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProcedureTempate.SelectedItem != null)
                {
                    msgText = "Are you sure you want to Delete ?";
                    int index = dgProcedureTempate.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            PocedureTempateList.RemoveAt(index);
                            cmdAddProcedureTemp.IsEnabled = true;
                            cmdModifyProcedureTemp.IsEnabled = false;
                        }
                    };
                    msgWD.Show();
                    dgProcedureTempate.ItemsSource = null;
                    dgProcedureTempate.ItemsSource = PocedureTempateList;
                }
                else
                {
                    msgText = "Please select Template";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void hlbEditDeleteTempateProc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbTempate.SelectedValue = Convert.ToInt64((dgProcedureTempate.SelectedItem as clsProcedureTemplateDetailsVO).TemplateID);
                SetCommandButtonState("EditProcTemplate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAddProcedureTemp_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTempate.SelectedItem != null && (cmbTempate.SelectedItem as MasterListItem).ID > 0)
            {
                clsProcedureTemplateDetailsVO tempDoc = new clsProcedureTemplateDetailsVO();


                var Template = from r in PocedureTempateList
                               where (r.TemplateID == (cmbTempate.SelectedItem as MasterListItem).ID)
                               select r;

                if (Template.ToList().Count == 0)
                {
                    tempDoc.TemplateID = (cmbTempate.SelectedItem as MasterListItem).ID;
                    tempDoc.Description = (cmbTempate.SelectedItem as MasterListItem).Description;
                    PocedureTempateList.Add(tempDoc);

                    dgProcedureTempate.ItemsSource = null;
                    dgProcedureTempate.ItemsSource = PocedureTempateList;
                    cmbTempate.SelectedValue = (long)0;
                }
                else
                {
                    msgText = "Template already present";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            else
            {
                msgText = "Please select Template";
                cmbTempate.TextBox.SetValidation(msgText);
                cmbTempate.TextBox.RaiseValidationError();
                cmbTempate.Focus();
            }
        }

        private void cmdModifyProcedureTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProcedureTempate.SelectedItem != null)
                {
                    int var = dgProcedureTempate.SelectedIndex;
                    PocedureTempateList.RemoveAt(dgProcedureTempate.SelectedIndex);

                    PocedureTempateList.Insert(var, new clsProcedureTemplateDetailsVO
                    {
                        TemplateID = ((MasterListItem)cmbTempate.SelectedItem).ID,
                        Description = ((MasterListItem)cmbTempate.SelectedItem).Description,
                    }
                    );

                    dgProcedureTempate.ItemsSource = PocedureTempateList;
                    dgProcedureTempate.Focus();
                    dgProcedureTempate.UpdateLayout();
                    dgProcedureTempate.SelectedIndex = PocedureTempateList.Count - 1;

                    cmbTempate.SelectedValue = (long)0;
                    cmdAddProcedureTemp.IsEnabled = true;
                    cmdModifyProcedureTemp.IsEnabled = false;
                }
                else
                {
                    msgText = "Please Select Template";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
