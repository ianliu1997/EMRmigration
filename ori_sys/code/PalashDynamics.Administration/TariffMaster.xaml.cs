using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Billing;
using System.Reflection;
using System.Text;
using PalashDynamics.UserControls;
using System.Windows.Input;
namespace PalashDynamics.Administration
{
    public partial class TariffMaster : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        public int SericeListPageSize
        {
            get
            {
                return SericeList.PageSize;
            }
            set
            {
                if (value == SericeList.PageSize) return;
                SericeList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        #endregion

        #region Variables Declarations
        private SwivelAnimation objAnimation;
        WaitIndicator objWIndicator;
        //public List<clsServiceMasterVO> SericeList { get; set; }
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public List<bool> check = new List<bool>();
        public PagedSortableCollectionView<clsTariffMasterBizActionVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsTariffMasterBizActionVO> GroupSubGroupDetailList { get; private set; }
        private long TariffId;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool IsCancel = true;
        //public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

        //public int DataListPageSize
        //{
        //    get
        //    {
        //        return DataList.PageSize;
        //    }
        //    set
        //    {
        //        if (value == DataList.PageSize) return;
        //        DataList.PageSize = value;
        //        // RaisePropertyChanged("DataListPageSize");
        //    }
        //}
        //======================================================
        public PagedSortableCollectionView<clsServiceMasterVO> SericeList { get; private set; }
        #endregion

        #region Constructor and Loaded 
        public TariffMaster()
        {
            InitializeComponent();

            ////======================================================
            ////Paging for Tariff Master
            //DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //DataListPageSize = 15;
            //dataGrid2Pager.PageSize = DataListPageSize;
            //dataGrid2Pager.Source = DataList;
            ////======================================================

            //======================================================
            //Paging for Services
            SericeList = new PagedSortableCollectionView<clsServiceMasterVO>();
            SericeList.OnRefresh += new EventHandler<RefreshEventArgs>(SericeList_OnRefresh);
            SericeListPageSize = 15;
            //  dgDataPager.PageSize = SericeListPageSize;
            // dgDataPager.Source = SericeList;

            //this.dgDataPager.DataContext = SericeList;
            //this.dgServiceList.DataContext = SericeList;
            //======================================================

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            objWIndicator = new WaitIndicator();
            this.DataContext = new clsTariffMasterBizActionVO();
            // SericeList = new List<clsServiceMasterVO>();

            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdTariff.DataContext = MasterList;

            SetupPage();



        }

        private void TariffMaster_Loaded(object sender, RoutedEventArgs e)
        {
            GroupSubGroupDetailList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            SetCommandButtonState("Load");
            SetCommandButtonGroupSubGroup("Load");
            FillSpecialization();
            txtTariffCode.Focus();
        }

        void SericeList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetServiceByTariffID();
        }

        //void DataList_OnRefresh(object sender, RefreshEventArgs e)
        //{
        //    //FillService();
        //    GetServiceByTariffID();
        //}

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        //private void FillService()
        //{
        //    try
        //    {

        //        clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
        //        BizAction.ServiceList = new List<clsServiceMasterVO>();

        //        BizAction.IsPagingEnabled = true;
        //        BizAction.MaximumRows = SericeList.PageSize;
        //        BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        dgServiceList.ItemsSource = null;
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
        //                {
        //                    DataList.TotalItemCount = (int)(((clsGetServiceMasterListBizActionVO)arg.Result).TotalRows);   //result.TotalRows;
        //                    DataList.Clear();

        //                    //dgServiceList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
        //                    BizAction.ServiceList = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
        //                    List<clsServiceMasterVO> ObjServiceList = new List<clsServiceMasterVO>();

        //                    foreach (var item in BizAction.ServiceList)
        //                    {
        //                        ObjServiceList.Add(new clsServiceMasterVO()
        //                        {
        //                            ServiceID = item.ID,
        //                            ServiceCode = item.ServiceCode, 
        //                            ServiceName=item.ServiceName,
        //                            Specialization=item.Specialization,
        //                            SubSpecialization=item.SubSpecialization,
        //                            Description=item.Description,
        //                            ShortDescription=item.ShortDescription,
        //                            LongDescription=item.LongDescription,
        //                            Rate = item.Rate,
        //                            SelectService = false,
        //                        });
        //                    }

        //                    //dgServiceList.ItemsSource = ObjServiceList;

        //                    foreach (var item in ObjServiceList)   //((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList)  
        //                    {
        //                        DataList.Add(item);
        //                    }

        //                    dgServiceList.ItemsSource = null;
        //                    dgServiceList.ItemsSource = DataList;

        //                    dgDataPager.Source = null;
        //                    dgDataPager.PageSize = BizAction.MaximumRows;
        //                    dgDataPager.Source = DataList;

        //                }
        //            }

        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                msgW1.Show();
        //            }

        //        };
        //        client.ProcessAsync(BizAction, User);// new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

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
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = false;  // true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;  // true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = false;  // true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;  // true;
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
                default:
                    break;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Method is for adding service details in the bill
        /// </summary>
        /// <param name="mServices"></param>
        private void AddCharges(List<clsServiceMasterVO> mServices)
        {
            StringBuilder strError = new StringBuilder();
            //strError.Append("");

            for (int i = 0; i < mServices.Count; i++)
            {
                bool Addcharge = true;

                if (SericeList != null && SericeList.Count > 0)
                {
                    var item = from r in SericeList
                               where r.ServiceID == mServices[i].ServiceID
                               select new clsServiceMasterVO
                               {
                                   Status = r.Status,
                                   ID = r.ID,
                                   ServiceName = r.ServiceName
                               };

                    if (item.ToList().Count > 0)
                    {
                        if (strError.ToString().Length > 0)
                            strError.Append(",");
                        strError.Append(item.ToList()[0].ServiceName);
                        Addcharge = false;

                    }
                }

                if (Addcharge)
                {
                    clsServiceMasterVO itemC = new clsServiceMasterVO();

                    itemC.ServiceID = mServices[i].ServiceID;
                    itemC.ServiceCode = mServices[i].ServiceCode;
                    itemC.ServiceName = mServices[i].ServiceName;
                    itemC.Specialization = mServices[i].Specialization;
                    itemC.SubSpecialization = mServices[i].SubSpecialization;
                    itemC.Description = mServices[i].Description;
                    itemC.ShortDescription = mServices[i].ShortDescription;
                    itemC.LongDescription = mServices[i].LongDescription;
                    itemC.Rate = mServices[i].Rate;
                    itemC.SelectService = mServices[i].SelectService;

                    SericeList.Add(itemC);

                    dgServiceList.ItemsSource = null;
                    dgServiceList.ItemsSource = SericeList;
                    dgServiceList.UpdateLayout();
                    dgServiceList.Focus();


                }

            }

        }



        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Tariff Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetServiceByTariffID()
        {
            try
            {
                if (objWIndicator != null)
                    objWIndicator.Show();
                clsGetServiceByTariffIDBizActionVO BizAction = new clsGetServiceByTariffIDBizActionVO();
                BizAction.TariffDetails = new clsTariffMasterBizActionVO();
                BizAction.TariffID = TariffId;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = SericeList.PageSize;
                BizAction.StartRowIndex = SericeList.PageIndex * SericeList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    objWIndicator.Close();
                    if (ea.Error == null && ea.Result != null)
                    {
                        if (((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails != null)
                        {

                            if (grdTariff.SelectedItem != null)
                                objAnimation.Invoke(RotationType.Forward);

                            clsGetServiceByTariffIDBizActionVO TariffVO = new clsGetServiceByTariffIDBizActionVO();
                            TariffVO = ((clsGetServiceByTariffIDBizActionVO)ea.Result);

                            txtTariffCode.Text = TariffVO.TariffDetails.Code;
                            txtTariffName.Text = TariffVO.TariffDetails.Description;

                            if (TariffVO.TariffDetails.AllVisit == true)
                            {
                                cmdAll.IsChecked = TariffVO.TariffDetails.AllVisit;
                                cmdAll.IsEnabled = true;
                            }
                            if (TariffVO.TariffDetails.Specify == true)
                            {
                                cmdSpecify.IsChecked = TariffVO.TariffDetails.Specify;
                                cmdSpecify.IsEnabled = true;
                            }
                            if (TariffVO.TariffDetails.NoOfVisit > 0)
                            {
                                txtSpecify.Text = TariffVO.TariffDetails.NoOfVisit.ToString();
                                txtSpecify.IsReadOnly = false;
                            }
                            if (TariffVO.TariffDetails.CheckDate == true)
                            {
                                chkDate.IsChecked = TariffVO.TariffDetails.CheckDate;
                                chkDate.IsEnabled = true;
                            }
                            if (TariffVO.TariffDetails.EffectiveDate != null)
                            {
                                dtpEffectiveDate.SelectedDate = TariffVO.TariffDetails.EffectiveDate;
                                dtpEffectiveDate.IsEnabled = true;
                            }
                            if (TariffVO.TariffDetails.ExpiryDate != null)
                            {
                                dtpExpiryDate.SelectedDate = TariffVO.TariffDetails.ExpiryDate;
                                dtpExpiryDate.IsEnabled = true;
                            }

                            SericeList.TotalItemCount = (int)(((clsGetServiceByTariffIDBizActionVO)ea.Result).TotalRows);   //result.TotalRows;
                            SericeList.Clear();

                            if (TariffVO.TariffDetails.ServiceMasterList != null)
                            {
                                //PagedSortableCollectionView<clsServiceMasterVO> lstService = (PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource;
                                //foreach (var item1 in TariffVO.TariffDetails.ServiceMasterList)
                                //{
                                //    foreach (var item in lstService)
                                //    {
                                //        if (item.ServiceID == item1.ServiceID)
                                //        {
                                //            item.SelectService = item1.SelectService;

                                //        }
                                //    }
                                //}

                                List<clsServiceMasterVO> lst = new List<clsServiceMasterVO>();

                                lst = TariffVO.TariffDetails.ServiceMasterList;

                                //dgServiceList.ItemsSource = null;
                                //dgServiceList.ItemsSource = SericeList;  // lstService;

                                foreach (var item in lst)  //ObjServiceList)   //((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList)  
                                {
                                    SericeList.Add(item);
                                }

                                dgServiceList.ItemsSource = null;
                                dgServiceList.ItemsSource = SericeList;

                                dgDataPager.Source = null;
                                dgDataPager.PageSize = BizAction.MaximumRows;
                                dgDataPager.Source = SericeList;

                                dgServiceList.UpdateLayout();
                                dgServiceList.Focus();
                            }


                            //dgServiceList.ItemsSource = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.ServiceMasterList;


                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }

        }

        private bool IsTariffchk()
        {
            clsServiceMasterVO ObjSer = ((List<clsServiceMasterVO>)dgServiceList.ItemsSource).FirstOrDefault(p => p.SelectService == true);

            if (ObjSer != null)
            {
                return true;
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select Tariff";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }

        }


        private void SetCommandButtonGroupSubGroup(String strMode)
        {
            switch (strMode)
            {
                case "Load":
                    cmdAddSpecialization.IsEnabled = true;
                    cmdModifySpecialization.IsEnabled = false;
                    break;
                case "AddGroup":
                    cmdAddSpecialization.IsEnabled = true;
                    cmdModifySpecialization.IsEnabled = false;
                    break;
                case "ModifyGroup":
                    cmdAddSpecialization.IsEnabled = true;
                    cmdModifySpecialization.IsEnabled = false;
                    break;
                case "EditGroup":
                    cmdAddSpecialization.IsEnabled = false;
                    cmdModifySpecialization.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        public void ClearFunctionForAddGroup()
        {
            if (cmbSpecialization.ItemsSource != null)
            {
                cmbSpecialization.SelectedItem = ((List<MasterListItem>)cmbSpecialization.ItemsSource)[0];
            }
            if (cmbSubSpecialization.ItemsSource != null)
            {
                cmbSubSpecialization.SelectedItem = ((List<MasterListItem>)cmbSubSpecialization.ItemsSource)[0];
            }
        }

        private bool checkGroupValidation()
        {
            bool result = true;
            if ((MasterListItem)cmbSpecialization.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgwin1 = new MessageBoxControl.MessageBoxChildWindow("", "Please select Specialization.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgwin1.Show();
                cmbSpecialization.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSpecialization.SelectedItem).ID.Equals(0))
            {
                MessageBoxControl.MessageBoxChildWindow msgwin1 = new MessageBoxControl.MessageBoxChildWindow("", "Please select Specialization.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgwin1.Show();
                cmbSpecialization.Focus();
                result = false;
            }
            return result;
        }

        private bool CheckDuplicateGroup()
        {
            if (GroupSubGroupDetailList != null)
            {
                var user = (from u in GroupSubGroupDetailList
                            where u.GroupID == ((MasterListItem)cmbSpecialization.SelectedItem).ID && u.SubGroupID == ((MasterListItem)cmbSubSpecialization.SelectedItem).ID
                            select u).FirstOrDefault();

                if (user != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgwin1 = new MessageBoxControl.MessageBoxChildWindow("", "Duplicate Specialization and SubSpecialization should not get add.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgwin1.Show();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

        }

        private void FillSpecialization()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
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

                        cmbSpecialization.ItemsSource = null;
                        cmbSpecialization.ItemsSource = objList;
                        cmbSpecialization.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            if (fkSpecializationID != null)
            {
                BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
            }
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
                    cmbSubSpecialization.ItemsSource = null;
                    cmbSubSpecialization.ItemsSource = objList;
                    cmbSubSpecialization.SelectedValue = objList[0].ID;
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Tariff Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Tariff on Which we Click  
        /// </summary>
        public void SetupPage()
        {
            if (objWIndicator != null)
                objWIndicator.Show();
            clsGetTariffListBizActionVO BizAction = new clsGetTariffListBizActionVO();
            BizAction.TariffList = new List<clsTariffMasterBizActionVO>();
            BizAction.SearchExpression = txtSearch.Text;

            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objWIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.TariffList = (((clsGetTariffListBizActionVO)args.Result).TariffList);
                        ///Setup Page Fill DataGrid

                        //if (TariffId ==0 && BizAction.TariffList.Count > 0)
                        //{
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetTariffListBizActionVO)args.Result).TotalRows);

                        foreach (clsTariffMasterBizActionVO item in BizAction.TariffList)
                        {
                            MasterList.Add(item);
                        }
                        //}
                    }

                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objWIndicator.Close();
                throw;
            }

        }

        /// <summary>
        /// When We Click On Add Button All UI Must Empty
        /// </summary>
        public void ClearUI()
        {
            txtTariffCode.Text = string.Empty;
            txtTariffName.Text = string.Empty;

            cmdAll.IsChecked = false;
            cmdSpecify.IsChecked = false;
            chkDate.IsChecked = false;
            dtpEffectiveDate.SelectedDate = null;
            dtpExpiryDate.SelectedDate = null;
            TariffId = 0;
            SericeList = new PagedSortableCollectionView<clsServiceMasterVO>();
            SericeList.OnRefresh += new EventHandler<RefreshEventArgs>(SericeList_OnRefresh);
            SericeList.PageSize = 15;
            dgServiceList.ItemsSource = null;
            dgServiceList.ItemsSource = SericeList;
            if (GroupSubGroupDetailList != null)
            {
                GroupSubGroupDetailList = null;
                GroupSubGroupDetailList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
                dgSPecializationList.ItemsSource = GroupSubGroupDetailList.ToList();
                dgSPecializationList.UpdateLayout();
            }
        }

        private bool Validation()
        {
            bool Result = true;

            if (cmdSpecify.IsChecked == true)
            {
                if (txtSpecify.Text == "0")
                {
                    txtSpecify.SetValidation("Please Enter No. Of Visit");
                    txtSpecify.RaiseValidationError();
                    txtSpecify.RaiseValidationError();
                    txtSpecify.Focus();
                    txtSpecify.Focus();
                    Result = false;
                }
                else
                    txtSpecify.ClearValidationError();
            }

            if (txtTariffName.Text == "")
            {
                txtTariffName.SetValidation("Please Enter Valid Tariff Name");
                txtTariffName.RaiseValidationError();
                txtTariffName.Focus();

                Result = false;
            }
            else
                txtTariffName.ClearValidationError();

            if (txtTariffCode.Text == "")
            {
                txtTariffCode.SetValidation("Please Enter Code");
                txtTariffCode.RaiseValidationError();
                txtTariffCode.Focus();

                Result = false;
            }
            else
                txtTariffCode.ClearValidationError();
            if (dtpExpiryDate.SelectedDate <= dtpEffectiveDate.SelectedDate)
            {
                dtpExpiryDate.SetValidation("Please Enter a Valid Expiry Date");
                dtpExpiryDate.RaiseValidationError();
                dtpExpiryDate.Focus();
                Result = false;
            }
            else dtpExpiryDate.ClearValidationError();

            if (dtpEffectiveDate.SelectedDate < DateTime.Now)
            {
                dtpEffectiveDate.SetValidation("Please Enter a Valid Effective Date");
                dtpEffectiveDate.RaiseValidationError();
                dtpEffectiveDate.Focus();
                Result = false;
            }
            else dtpEffectiveDate.ClearValidationError();

            return Result;
        }

        private bool CheckDuplicasy()
        {
            clsTariffMasterBizActionVO Item;
            clsTariffMasterBizActionVO Item1;

            Item = ((PagedSortableCollectionView<clsTariffMasterBizActionVO>)grdTariff.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtTariffCode.Text.ToUpper()));
            Item1 = ((PagedSortableCollectionView<clsTariffMasterBizActionVO>)grdTariff.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtTariffName.Text.ToUpper()));


            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Button Click Events
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save Tariff Master?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();

            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                try
                {
                    if (CheckDuplicasy())
                    {
                        SaveTariff();
                        SetCommandButtonState("Save");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
        }

        private void SaveTariff()
        {
            try
            {
                if (objWIndicator != null)
                    objWIndicator.Show();
                clsAddTariffMasterBizActionVO BizAction = new clsAddTariffMasterBizActionVO();
                BizAction.TariffDetails = (clsTariffMasterBizActionVO)this.DataContext;

                //List<clsServiceMasterVO> ObjServiceList = new List<clsServiceMasterVO>();
                //ObjServiceList =  ((PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource).ToList();


                if (txtTariffCode.Text != "")
                {
                    BizAction.TariffDetails.Code = txtTariffCode.Text;
                }
                if (txtTariffName.Text != "")
                {
                    BizAction.TariffDetails.Description = txtTariffName.Text;
                }
                if (txtSpecify.Text != null && txtSpecify.Text != "")
                {
                    BizAction.TariffDetails.NoOfVisit = Convert.ToInt32(txtSpecify.Text);
                }
                if (cmdAll.IsChecked == true)
                {
                    BizAction.TariffDetails.AllVisit = Convert.ToBoolean(cmdAll.IsChecked);
                    txtSpecify.IsEnabled = false;
                }
                if (cmdSpecify.IsChecked == true)
                {
                    BizAction.TariffDetails.Specify = Convert.ToBoolean(cmdSpecify.IsChecked);
                }
                if (chkDate.IsChecked == true)
                {
                    BizAction.TariffDetails.CheckDate = Convert.ToBoolean(chkDate.IsChecked);
                }
                if (dtpEffectiveDate.SelectedDate != null)
                {
                    BizAction.TariffDetails.EffectiveDate = dtpEffectiveDate.SelectedDate;
                }
                if (dtpExpiryDate.SelectedDate != null)
                {
                    BizAction.TariffDetails.ExpiryDate = dtpExpiryDate.SelectedDate;
                }


                BizAction.TariffDetails.ServiceMasterList = SericeList.ToList();
                //BizAction.TariffDetails.ServiceMasterList = ((PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource).ToList();
                BizAction.TariffDetails.ServiceSpecializationMasterList = GroupSubGroupDetailList.ToList();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    objWIndicator.Close();
                    if (arg.Error == null)
                    {
                        if ((clsAddTariffMasterBizActionVO)arg.Result != null)
                        {
                            SetupPage();
                            ClearUI();
                            objAnimation.Invoke(RotationType.Backward);

                            if (((clsAddTariffMasterBizActionVO)arg.Result).TariffDetails != null)
                                TariffId = ((clsAddTariffMasterBizActionVO)arg.Result).TariffDetails.ID;


                            //GetServiceByTariffID();

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Tariff Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            SetCommandButtonState("Save");
                        }
                    }


                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();

                    }


                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }

        }

        /// <summary>
        /// This Event is Call When We click on Modify Button and Update Tariff Details
        /// (For Add and Modify Tariff Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();



            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Modify();
                    SetCommandButtonState("Modify");
                }

                catch (Exception ex)
                {
                    throw;
                }
            }


        }

        private void Modify()
        {
            try
            {
                if (objWIndicator != null)
                    objWIndicator.Show();
                clsAddTariffMasterBizActionVO BizAction = new clsAddTariffMasterBizActionVO();
                BizAction.TariffDetails = new clsTariffMasterBizActionVO();
                BizAction.TariffDetails.ID = TariffId;  // ((clsTariffMasterBizActionVO)grdTariff.SelectedItem).ID;
                if (txtTariffCode.Text != "")
                {
                    BizAction.TariffDetails.Code = txtTariffCode.Text;
                }
                if (txtTariffName.Text != "")
                {
                    BizAction.TariffDetails.Description = txtTariffName.Text;
                }
                if (txtSpecify.Text != null && txtSpecify.Text != "" && cmdAll.IsChecked == false)
                {
                    BizAction.TariffDetails.NoOfVisit = Convert.ToInt32(txtSpecify.Text);
                }
                if (cmdAll.IsChecked == true)
                {
                    BizAction.TariffDetails.AllVisit = Convert.ToBoolean(cmdAll.IsChecked);
                    txtSpecify.IsEnabled = false;
                }
                if (cmdSpecify.IsChecked == true)
                {
                    BizAction.TariffDetails.Specify = Convert.ToBoolean(cmdSpecify.IsChecked);
                }
                if (chkDate.IsChecked == true)
                {
                    BizAction.TariffDetails.CheckDate = Convert.ToBoolean(chkDate.IsChecked);
                }
                if (dtpEffectiveDate.SelectedDate != null)
                {
                    BizAction.TariffDetails.EffectiveDate = dtpEffectiveDate.SelectedDate;
                }
                if (dtpExpiryDate.SelectedDate != null)
                {
                    BizAction.TariffDetails.ExpiryDate = dtpExpiryDate.SelectedDate;
                }
                BizAction.TariffDetails.ServiceMasterList = SericeList.ToList();
                //BizAction.TariffDetails.ServiceMasterList = ((PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource).ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    objWIndicator.Close();
                    if (arg.Error == null)
                    {
                        SetupPage();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);

                        TariffId = ((clsAddTariffMasterBizActionVO)arg.Result).TariffDetails.ID;

                        //GetServiceByTariffID();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Tariff Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetCommandButtonState("Modify");

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (GroupSubGroupDetailList != null)
            {
                GroupSubGroupDetailList = null;
                GroupSubGroupDetailList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
                dgSPecializationList.ItemsSource = GroupSubGroupDetailList.ToList();
                dgSPecializationList.UpdateLayout();
            }
            SetCommandButtonState("New");
            this.DataContext = new clsTariffMasterBizActionVO();
            Validation();
            ClearUI();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Tariff Details";
            objAnimation.Invoke(RotationType.Forward);
            cmdAll.IsChecked = true;

        }

        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All Tariff List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            SetCommandButtonGroupSubGroup("Load");
            TariffId = 0;
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                SetupPage();
                IsCancel = true;
            }
            SericeList = null;
            dgDataPager.Source = null;
            dgDataPager.PageSize = 15;
            dgDataPager.Source = SericeList;
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearUI();
            TariffId = ((clsTariffMasterBizActionVO)grdTariff.SelectedItem).ID;
            if (TariffId > 0)
            {
                SericeList = new PagedSortableCollectionView<clsServiceMasterVO>();
                SericeList.OnRefresh += new EventHandler<RefreshEventArgs>(SericeList_OnRefresh);
                SericeList.PageSize = 15;
                GetServiceByTariffID();
            }
            if (((clsTariffMasterBizActionVO)grdTariff.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else
                cmdModify.IsEnabled = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsTariffMasterBizActionVO)grdTariff.SelectedItem).Description;

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //MasterList = new PagedSortableCollectionView<clsTariffVO>();
            //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            //PageSize = 15;
            //SetupPage();
            //this.grdTariff.DataContext = MasterList;
            //this.dataGrid2Pager.DataContext = MasterList;

            MasterList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdTariff.DataContext = MasterList;

            SetupPage();

        }

        private void lnkAddService_Click(object sender, RoutedEventArgs e)
        {
            ServiceSearchAddToTariff serviceSearchOnAdd = null;
            serviceSearchOnAdd = new ServiceSearchAddToTariff();
            //serviceSearch.PatientSourceID = myPatient.
            serviceSearchOnAdd.OnAddButton_Click += new RoutedEventHandler(serviceSearchOnAdd_OnAddButton_Click);
            serviceSearchOnAdd.Show();
        }

        private void cmdSpecify_Click(object sender, RoutedEventArgs e)
        {
            if (cmdSpecify.IsChecked == true)
            {
                txtSpecify.IsReadOnly = false;
                chkDate.IsEnabled = true;
                dtpEffectiveDate.IsEnabled = true;
                dtpExpiryDate.IsEnabled = true;
            }
        }

        private void cmdAll_Click(object sender, RoutedEventArgs e)
        {
            txtSpecify.Text = "";
            txtSpecify.IsReadOnly = true;
            chkDate.IsChecked = false;
            dtpEffectiveDate.SelectedDate = null;
            dtpExpiryDate.SelectedDate = null;

        }

        private void hlkDeleteSpecializationList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSPecializationList.SelectedItem != null)
                {
                    int index = dgSPecializationList.SelectedIndex;
                    GroupSubGroupDetailList.RemoveAt(index);
                    dgSPecializationList.ItemsSource = null;
                    dgSPecializationList.ItemsSource = GroupSubGroupDetailList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void hlkEdit_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonGroupSubGroup("EditGroup");
            clsTariffMasterBizActionVO objtariff = new clsTariffMasterBizActionVO();
            if (dgSPecializationList.SelectedItem != null)
            {
                objtariff = (clsTariffMasterBizActionVO)dgSPecializationList.SelectedItem;
                cmbSpecialization.SelectedValue = objtariff.GroupID;
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID.ToString());
                cmbSubSpecialization.SelectedValue = objtariff.SubGroupID;

            }
        }

        private void cmdAddSpecialization_Click(object sender, RoutedEventArgs e)
        {
            clsTariffMasterBizActionVO objGroup = new clsTariffMasterBizActionVO();

            if (checkGroupValidation() && CheckDuplicateGroup())
            {
                objGroup.RowID = GroupSubGroupDetailList.Count + 1;
                if (cmbSpecialization.SelectedItem != null)
                {
                    objGroup.GroupID = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                    objGroup.StrGroup = ((MasterListItem)cmbSpecialization.SelectedItem).Description;
                }
                if (cmbSubSpecialization.SelectedItem != null)
                {
                    objGroup.SubGroupID = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                    if (((MasterListItem)cmbSubSpecialization.SelectedItem).ID.Equals(0))
                        objGroup.StrSubGroup = string.Empty;
                    else
                        objGroup.StrSubGroup = ((MasterListItem)cmbSubSpecialization.SelectedItem).Description;
                }
                GroupSubGroupDetailList.Add(objGroup);
                dgSPecializationList.ItemsSource = null;
                dgSPecializationList.ItemsSource = GroupSubGroupDetailList;
                dgSPecializationList.UpdateLayout();
                ClearFunctionForAddGroup();
                SetCommandButtonGroupSubGroup("AddGroup");
            }
        }

        private void cmdModifySpecialization_Click(object sender, RoutedEventArgs e)
        {
            clsTariffMasterBizActionVO objmodf = new clsTariffMasterBizActionVO();

            if (checkGroupValidation())
            {
                long RowID = ((clsTariffMasterBizActionVO)dgSPecializationList.SelectedItem).RowID;

                if (cmbSpecialization.SelectedItem != null)
                {
                    GroupSubGroupDetailList.SingleOrDefault(S => S.RowID.Equals(RowID)).GroupID = ((List<MasterListItem>)cmbSpecialization.ItemsSource).SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbSpecialization.SelectedItem).ID)).ID;
                    GroupSubGroupDetailList.SingleOrDefault(S => S.RowID.Equals(RowID)).StrGroup = ((List<MasterListItem>)cmbSpecialization.ItemsSource).SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbSpecialization.SelectedItem).ID)).Description;
                }
                if (cmbSubSpecialization.SelectedItem != null)
                {
                    GroupSubGroupDetailList.SingleOrDefault(S => S.RowID.Equals(RowID)).SubGroupID = ((List<MasterListItem>)cmbSubSpecialization.ItemsSource).SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbSubSpecialization.SelectedItem).ID)).ID;
                    GroupSubGroupDetailList.SingleOrDefault(S => S.RowID.Equals(RowID)).StrSubGroup = ((List<MasterListItem>)cmbSubSpecialization.ItemsSource).SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbSubSpecialization.SelectedItem).ID)).Description;
                }
                dgSPecializationList.ItemsSource = null;
                dgSPecializationList.ItemsSource = GroupSubGroupDetailList;
                ClearFunctionForAddGroup();
                SetCommandButtonGroupSubGroup("ModifyGroup");
            }
        }

        void serviceSearchOnAdd_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ServiceSearchAddToTariff)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((ServiceSearchAddToTariff)sender).SelectedServices;

                AddCharges(lServices);

                //if (TariffId == 0)
                //{
                //    SaveTariff();
                //}
                //else
                //{

                //    Modify();
                //}


            }

        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {


        }
        #endregion

        #region Other Events (Checked,UnCheched,LostFocus,SelectionChanged)
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            clsAddUpdateTariffBizActionVO bizactionVO = new clsAddUpdateTariffBizActionVO();
            clsTariffVO addNewTariffVO = new clsTariffVO();
            clsTariffMasterBizActionVO obj = ((clsTariffMasterBizActionVO)grdTariff.SelectedItem);

            if (grdTariff.SelectedItem != null)
            {
                try
                {
                    addNewTariffVO.Id = obj.ID;
                    addNewTariffVO.Code = obj.Code;
                    addNewTariffVO.Description = obj.Description;
                    addNewTariffVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewTariffVO.UnitId = obj.UnitID;
                    addNewTariffVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTariffVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewTariffVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewTariffVO.UpdatedDateTime = System.DateTime.Now;
                    addNewTariffVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewTariffVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateTariffBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                // TariffId = 0;
                                SetupPage();

                                string msgText = "Status Updated Successfully ";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                // objAnimation.Invoke(RotationType.Backward);
                                //cmdAdd.IsEnabled = true;
                                //cmdModify.IsEnabled = false;
                                //SetCommandButtonState("Modify");

                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID.ToString());
            }
        }

        private void cmdSpecify_Unchecked(object sender, RoutedEventArgs e)
        {
            if (cmdSpecify.IsChecked == false)
            {
                txtSpecify.IsReadOnly = true;
                chkDate.IsEnabled = false;
                dtpEffectiveDate.IsEnabled = false;
                dtpExpiryDate.IsEnabled = false;
            }
        }

        private void txtTariffCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtTariffCode.Text = txtTariffCode.Text.ToTitleCase();
        }

        private void txtTariffName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtTariffName.Text = txtTariffName.Text.ToTitleCase();
        }

        List<clsServiceMasterVO> lst = new List<clsServiceMasterVO>();
        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == true)
                {
                    lst = (List<clsServiceMasterVO>)dgServiceList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectService = true;
                        }
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = lst;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == false)
                {

                    lst = (List<clsServiceMasterVO>)dgServiceList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectService = false;
                        }
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = lst;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        //** Added By Ashish Z.
        private void cmdTariffCopyUtility_Click(object sender, RoutedEventArgs e)
        {
            frmTariffCopyUtility _win = new frmTariffCopyUtility();
            _win.Show();
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdTariff.DataContext = MasterList;
                SetupPage();
            }

        }


    }
}
