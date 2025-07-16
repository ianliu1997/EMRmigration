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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using PalashDynamics.UserControls;
using System.Windows.Browser;

namespace PalashDynamics.Pharmacy
{
    public partial class frmStoreLocationLock : UserControl
    {
        #region Variables Declarations
        private SwivelAnimation objAnimation;
        public clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool UserStoreAssigned = false;
        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        List<MasterListItem> tempRackCombo = new List<MasterListItem>();
        List<MasterListItem> tempStoreCombo = new List<MasterListItem>();
        List<MasterListItem> tempShelfCombo = new List<MasterListItem>();
        List<MasterListItem> tempBinCombo = new List<MasterListItem>();
        public PagedSortableCollectionView<StoreLocationLockVO> MasterList { get; private set; }
        public PagedSortableCollectionView<StoreLocationLockVO> MasterListDetails { get; private set; }
        List<ItemStoreLocationDetailsVO> tempMasterList;
        List<ItemStoreLocationDetailsVO> tempMasterListView;
        public List<AddUpdateStoreLocationLockBizActionVO> SelectedStore = new List<AddUpdateStoreLocationLockBizActionVO>();
        bool IsCancel = true;
        string msgText = "";
        WaitIndicator waitIndi;
        long StoreID { get; set; }
        long RackID { get; set; }
        long ShelfID { get; set; }
        long lID { get; set; }
        #endregion

        #region Constructor and Loaded
        public frmStoreLocationLock()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            tempMasterList = new List<ItemStoreLocationDetailsVO>();
            tempMasterListView = new List<ItemStoreLocationDetailsVO>();
            waitIndi = new WaitIndicator();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillBlockType();
            FillStore();
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<StoreLocationLockVO>();
            MasterListDetails = new PagedSortableCollectionView<StoreLocationLockVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 5;
            dtpLockDate.SelectedDate = System.DateTime.Now;
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            setup();
        }
        #endregion

        #region Page Refresh
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setup();
        }
        #endregion

        #region Fill ComboBox
        private void FillBlockType()
        {
            List<MasterListItem> mBlockType = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "-- Select --");
            mBlockType.Insert(0, Default);
            EnumToList(typeof(BlockTypes), mBlockType);
            cmbBlockType.ItemsSource = mBlockType;
            cmbBlockType.SelectedItem = mBlockType[0];
        }

        private static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        private static object[] GetValues(Type enumType)
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

        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true && item.IsQuarantineStore == false
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();
                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;
                    UserStores.Clear();
                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {
                            UserStores = item.UserUnitStore;
                        }
                    }

                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }
                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);
                    cmbStoreName.ItemsSource = result1.ToList();
                    cmbStoreNameBack.ItemsSource = result1.ToList();

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (result1.ToList().Count > 0)
                    {
                        cmbStoreName.SelectedItem = result1.ToList()[0];
                        cmbStoreNameBack.SelectedItem = result1.ToList()[0];
                    }
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbStoreName.ItemsSource = result1.ToList();
                        cmbStoreNameBack.ItemsSource = result1.ToList();
                        cmbStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                        cmbStoreNameBack.ItemsSource = BizActionObj.ItemMatserDetails;
                        cmbStoreName.ItemsSource = result1.ToList();
                        cmbStoreNameBack.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            cmbStoreName.SelectedItem = (result1.ToList())[0];
                            cmbStoreNameBack.SelectedItem = (result1.ToList())[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cmbStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbStoreNameBack.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStoreNameBack.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                        //cmbStoreName.ItemsSource = StoreListForClinic;
                        //cmbStoreNameBack.ItemsSource = StoreListForClinic;
                        //if (StoreListForClinic.Count > 0)
                        //{
                        //    cmbStoreName.SelectedItem = StoreListForClinic[0];
                        //    cmbStoreNameBack.SelectedItem = StoreListForClinic[0];
                        //}
                    }
                    //foreach (var item in tempStoreCombo.ToList())
                    //{
                    //    foreach (var item1 in result1.ToList())
                    //    {
                    //        item.ID = item1.StoreId;
                    //        item.Description = item1.StoreName;
                    //    }
                    //}
                }
            };
            client.CloseAsync();
        }

        private void FillRack(long lStoreID)
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                GetRackMasterBizActionVO BizAction = new GetRackMasterBizActionVO();
                BizAction.ID = lStoreID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null) //&& ((GetRackMasterBizActionVO)args.Result).MasterList != null
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        if (((GetRackMasterBizActionVO)args.Result).MasterList != null)
                            objList.AddRange(((GetRackMasterBizActionVO)args.Result).MasterList);
                        tempShelfCombo = objList.ToList();
                        cmbRackNameback.ItemsSource = null;
                        cmbRackNameback.ItemsSource = objList;
                        cmbRackNameback.SelectedItem = objList[0];
                        //FillShelf(lStoreID,RackID);
                    }
                    waitIndi.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
                //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.MasterTable = MasterTableNameList.M_RackMaster;
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, args) =>
                //{
                //    if (args.Error == null && args.Result != null)
                //    {
                //        List<MasterListItem> objList = new List<MasterListItem>();
                //        objList.Add(new MasterListItem(0, "-- Select --"));
                //        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                //        tempRackCombo = objList.ToList();
                //        //cmbRackName.ItemsSource = null;
                //        //cmbRackName.ItemsSource = objList;
                //        //cmbRackName.SelectedItem = objList[0];
                //        cmbRackNameback.ItemsSource = null;
                //        cmbRackNameback.ItemsSource = objList;
                //        cmbRackNameback.SelectedItem = objList[0];
                //        FillShelf();

                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                //client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                waitIndi.Close();
            }
        }

        private void FillContainer(long lStoreID)
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                GetBinMasterBizActionVO BizAction = new GetBinMasterBizActionVO();
                BizAction.StoreID = StoreID;
                BizAction.RackID = RackID;
                BizAction.ShelfID = ShelfID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && ((GetBinMasterBizActionVO)args.Result).MasterList != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((GetBinMasterBizActionVO)args.Result).MasterList);
                        tempShelfCombo = objList.ToList();
                        cmbBinNameback.ItemsSource = null;
                        cmbBinNameback.ItemsSource = objList;
                        cmbBinNameback.SelectedItem = objList[0];
                    }
                    waitIndi.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();


                //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.MasterTable = MasterTableNameList.M_ContainerMaster;
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, args) =>
                //{
                //    if (args.Error == null && ((clsGetMasterListBizActionVO)args.Result).MasterList != null)
                //    {
                //        List<MasterListItem> objList = new List<MasterListItem>();
                //        objList.Add(new MasterListItem(0, "-- Select --"));
                //        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                //        tempBinCombo = objList.ToList();
                //        //cmbBinName.ItemsSource = null;
                //        //cmbBinName.ItemsSource = objList;
                //        //cmbBinName.SelectedItem = objList[0];
                //        cmbBinNameback.ItemsSource = null;
                //        cmbBinNameback.ItemsSource = objList;
                //        cmbBinNameback.SelectedItem = objList[0];
                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                //client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                waitIndi.Close();
            }
        }

        private void FillShelf(long lStoreID, long lRackID)
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                GetShelfMasterBizActionVO BizAction = new GetShelfMasterBizActionVO();
                BizAction.StoreID = lStoreID;
                BizAction.RackID = lRackID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && ((GetShelfMasterBizActionVO)args.Result).MasterList != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((GetShelfMasterBizActionVO)args.Result).MasterList);
                        tempShelfCombo = objList.ToList();
                        cmbShelfNameback.ItemsSource = null;
                        cmbShelfNameback.ItemsSource = objList;
                        cmbShelfNameback.SelectedItem = objList[0];
                        //FillContainer(lStoreID);
                    }
                    waitIndi.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

                //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.MasterTable = MasterTableNameList.M_ShelfMaster;
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, args) =>
                //{
                //    if (args.Error == null && args.Result != null)
                //    {
                //        List<MasterListItem> objList = new List<MasterListItem>();
                //        objList.Add(new MasterListItem(0, "-- Select --"));
                //        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                //        tempShelfCombo = objList.ToList();
                //        //cmbShelfName.ItemsSource = null;
                //        //cmbShelfName.ItemsSource = objList;
                //        //cmbShelfName.SelectedItem = objList[0];
                //        cmbShelfNameback.ItemsSource = null;
                //        cmbShelfNameback.ItemsSource = objList;
                //        cmbShelfNameback.SelectedItem = objList[0];
                //        FillContainer();
                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                //client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                waitIndi.Close();
            }
        }
        #endregion

        #region Private Methods
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdNew.IsEnabled = true;
                    CmdCancel.IsEnabled = false;
                    CmdUnlock.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdModify.IsEnabled = false;
                    CmdPrint.IsEnabled = true;
                    break;
                case "New":
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdUnlock.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdPrint.IsEnabled = false;
                    break;
                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdCancel.IsEnabled = true;
                    CmdUnlock.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdModify.IsEnabled = false;
                    CmdPrint.IsEnabled = true;
                    break;
                case "Modify":
                    CmdNew.IsEnabled = true;
                    CmdCancel.IsEnabled = true;
                    CmdUnlock.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdModify.IsEnabled = false;
                    CmdPrint.IsEnabled = true;
                    break;
                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdCancel.IsEnabled = false;
                    CmdUnlock.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdModify.IsEnabled = false;
                    CmdPrint.IsEnabled = true;
                    break;
                case "View":
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdUnlock.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdModify.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        void msgW_OnMessageBoxClosedDelete(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (grdStoreListback.SelectedItem != null)
                {
                    tempMasterList.Remove((ItemStoreLocationDetailsVO)grdStoreListback.SelectedItem);
                    grdStoreListback.ItemsSource = null;
                    grdStoreListback.ItemsSource = tempMasterList;
                    grdStoreListback.UpdateLayout();
                }
            }
            else
            {

            }
        }

        void msgW_OnMessageBoxClosedSave(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save(false);
            }
            else
            {
                SetCommandButtonState("New");
            }
        }

        void msgW_OnMessageBoxClosedModify(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save(true);
            }
            else
            {
                SetCommandButtonState("New");
            }
        }

        void Save(bool IsModify)
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                AddUpdateStoreLocationLockBizActionVO BizAction = new AddUpdateStoreLocationLockBizActionVO();
                BizAction.ObjStoreLocationLock = new StoreLocationLockVO();
                BizAction.IsModify = IsModify;
                if (IsModify) BizAction.ObjStoreLocationLock.ID = lID;
                else BizAction.ObjStoreLocationLock.ID = 0;
                BizAction.ObjStoreLocationLock.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.ObjStoreLocationLock.LockDate = Convert.ToDateTime(dtpLockDate.SelectedDate);
                BizAction.ObjStoreLocationLock.Remark = txtRemark.Text;
                BizAction.ObjStoreLocationLock.IsFreeze = Convert.ToBoolean(chkFreez.IsChecked);
                BizAction.ObjStoreLocationLock.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;

                foreach (var item in tempMasterList)
                {
                    StoreLocationLockVO StoreLoc = new StoreLocationLockVO();

                    StoreLoc.RackID = item.RackID;
                    StoreLoc.StoreID = item.StoreID;
                    StoreLoc.ShelfID = item.ShelfID;
                    StoreLoc.ContainerID = item.ContainerID;
                    StoreLoc.LockDate = Convert.ToDateTime(dtpLockDate.SelectedDate);
                    StoreLoc.ItemID = item.ItemID;
                    StoreLoc.ID = 0;
                    StoreLoc.BlockType = item.BlockType;
                    StoreLoc.Status = true;
                    StoreLoc.BlockTypeID = item.BlockTypeID;
                    StoreLoc.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    StoreLoc.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    StoreLoc.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    StoreLoc.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    StoreLoc.AddedDateTime = System.DateTime.Now;
                    StoreLoc.AddOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    StoreLoc.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    BizAction.StoreLocationLockDetails.Add(StoreLoc);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        setup();
                        string strMsg = string.Empty;
                        if (IsModify) strMsg = "Record updated Successfully"; else strMsg = "Record added Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Load");
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
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

        void setup()
        {
            if (waitIndi == null) waitIndi = new WaitIndicator();
            waitIndi.Show();
            GetStoreLocationLockBizActionVO BizAction = new GetStoreLocationLockBizActionVO();
            BizAction.IsViewClick = false;
            BizAction.IsForValidation = false;
            BizAction.ObjStoreLocationLock = new StoreLocationLockVO();
            if ((clsStoreVO)cmbStoreName.SelectedItem != null)
            {
                if (((clsStoreVO)cmbStoreName.SelectedItem).StoreId > 0)
                {
                    BizAction.ObjStoreLocationLock.StoreID = ((clsStoreVO)cmbStoreName.SelectedItem).StoreId;
                }
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
            {
                BizAction.ObjStoreLocationLock.UnitID = 0;
            }
            else
            {
                BizAction.ObjStoreLocationLock.UnitID = User.UserLoginInfo.UnitId;
            }
            if (dtpFromDate.SelectedDate != null)
                BizAction.ObjStoreLocationLock.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
            {
                //DateTime date = dtpToDate.SelectedDate.Value.Date;
                BizAction.ObjStoreLocationLock.ToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            }
            if (txtRemarkFront.Text != null && txtRemarkFront.Text != "")
            {
                BizAction.ObjStoreLocationLock.Remark = txtRemarkFront.Text.Trim();
            }
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartIndex = MasterList.PageIndex * MasterList.PageSize;
            BizAction.Flag = 1;

            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.StoreLocationLock = (((GetStoreLocationLockBizActionVO)args.Result).StoreLocationLock);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((GetStoreLocationLockBizActionVO)args.Result).TotalRows);
                        foreach (StoreLocationLockVO item in BizAction.StoreLocationLock)
                        {
                            MasterList.Add(item);
                        }
                        dgStoreList.ItemsSource = null;
                        dgStoreList.ItemsSource = MasterList;
                        //   dgStoreList.SelectedIndex = -1;
                        DataPager.Source = null;
                        DataPager.PageSize = MasterList.PageSize;
                        DataPager.Source = MasterList;
                        dgStoreList.UpdateLayout();
                    }
                    txtTotalCountRecords.Text = MasterList.TotalItemCount.ToString();
                    waitIndi.Close();
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                waitIndi.Close();
                throw;
            }
        }
        #endregion

        #region Click Events
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Forward);
            tempMasterList = new List<ItemStoreLocationDetailsVO>();
            tempMasterList.Clear();
            grdStoreListback.ItemsSource = tempMasterList;
            txtRemark.Text = "";
            chkFreez.IsChecked = false;
            chkFreez.IsEnabled = true;
            cmbStoreNameBack.IsEnabled = true;
            cmbStoreNameBack.SelectedValue = (long)0;
            cmbRackNameback.SelectedValue = (long)0;
            cmbShelfNameback.SelectedValue = (long)0;
            cmbBinNameback.SelectedValue = (long)0;
            cmbBlockType.SelectedValue = (long)0;
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            tempMasterList.Clear();
            grdStoreListback.ItemsSource = tempMasterList;
            grdStoreListback.UpdateLayout();
            DataPager.PageIndex = 0;
            objAnimation.Invoke(RotationType.Backward);
            if (FrontPanel.Visibility == Visibility.Visible)
            {
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Inventory Configuration";
                //WebClient c = new WebClient();
                //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                //c.OpenReadAsync(new Uri("PalashDynamics.Administration" + ".xap", UriKind.Relative));
            }
            else
                setup();

        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "PalashDynamics.Administration" + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("PalashDynamics.Administration.frmInventoryConfiguration") as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (tempMasterList.Count > 0)
            {
                SetCommandButtonState("Save");
                string msgTitle = "Palash";
                msgText = "Are you sure you want to Suspend the Record ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedSave);
                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("", "Record can not Empty, Please add Record in List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void lnkAddStor_Click(object sender, RoutedEventArgs e)
        {
            CheckDuplicatesWithExistingRecords();



            //if (AddStoreValidation())
            //{
            //    if (BlockTypeValidation())
            //    {
            //        if (duplicates())
            //        {
            //            ItemStoreLocationDetailsVO objVO = new ItemStoreLocationDetailsVO();
            //            objVO.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
            //            objVO.StoreName = ((clsStoreVO)cmbStoreNameBack.SelectedItem).ClinicName + " " + ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreName;

            //            objVO.RackID = ((MasterListItem)cmbRackNameback.SelectedItem).ID;
            //            objVO.Rackname = ((MasterListItem)cmbRackNameback.SelectedItem).Description;

            //            if (cmbBinNameback.SelectedItem != null)
            //            {
            //                if (((MasterListItem)cmbShelfNameback.SelectedItem).ID > 0)
            //                {
            //                    objVO.ShelfID = ((MasterListItem)cmbShelfNameback.SelectedItem).ID;
            //                    objVO.Shelfname = ((MasterListItem)cmbShelfNameback.SelectedItem).Description;
            //                }
            //                else
            //                    objVO.Shelfname = "N/A";
            //            }
            //            else
            //                objVO.Shelfname = "N/A";

            //            if (cmbBinNameback.SelectedItem != null)
            //            {
            //                if (((MasterListItem)cmbBinNameback.SelectedItem).ID > 0)
            //                {
            //                    objVO.ContainerID = ((MasterListItem)cmbBinNameback.SelectedItem).ID;
            //                    objVO.Containername = ((MasterListItem)cmbBinNameback.SelectedItem).Description;
            //                }
            //                else
            //                    objVO.Containername = "N/A";
            //            }
            //            else
            //                objVO.Containername = "N/A";

            //            objVO.ItemName = "N/A";

            //            objVO.BlockType = ((MasterListItem)cmbBlockType.SelectedItem).Description;
            //            objVO.BlockTypeID = ((MasterListItem)cmbBlockType.SelectedItem).ID;
            //            tempMasterList.Add(objVO);
            //            grdStoreListback.ItemsSource = null;
            //            grdStoreListback.ItemsSource = tempMasterList;
            //            grdStoreListback.UpdateLayout();
            //        }
            //    }
            //}
        }

        private void cmdCancelSelected_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            msgText = "Are you sure you want to Delete selected Record?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedDelete);
            msgW.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFrontPannelSearch())
            {
                setup();
                DataPager.PageIndex = 0;
            }
            //DataPager.PageIndex = 0;
        }

        private void ViewStore_Click(object sender, RoutedEventArgs e)
        {
            lID = ((StoreLocationLockVO)dgStoreList.SelectedItem).ID;
            if (waitIndi == null) waitIndi = new WaitIndicator();
            waitIndi.Show();
            GetStoreLocationLockBizActionVO BizAction = new GetStoreLocationLockBizActionVO();
            BizAction.IsViewClick = true;
            BizAction.IsForValidation = false;
            BizAction.ObjStoreLocationLock = new StoreLocationLockVO();
            BizAction.ObjStoreLocationLock.ID = Convert.ToInt64(((StoreLocationLockVO)dgStoreList.SelectedItem).ID);
            BizAction.ObjStoreLocationLock.UnitID = Convert.ToInt64(((StoreLocationLockVO)dgStoreList.SelectedItem).UnitID);
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        SetCommandButtonState("View");
                        if (tempMasterList != null)
                        {
                            tempMasterList = new List<ItemStoreLocationDetailsVO>();
                            tempMasterList.Clear();
                        }
                        foreach (ItemStoreLocationDetailsVO item in ((GetStoreLocationLockBizActionVO)args.Result).StoreLocationLockDetailsList.ToList())
                        {
                            dtpLockDate.SelectedDate = item.LockDate;
                            tempMasterList.Add(item);
                        }
                        grdStoreListback.ItemsSource = null;
                        grdStoreListback.ItemsSource = tempMasterList;
                        grdStoreListback.SelectedIndex = -1;
                        grdStoreListback.UpdateLayout();

                        cmbStoreNameBack.SelectedValue = ((GetStoreLocationLockBizActionVO)args.Result).ObjStoreLocationLock.StoreID;
                        txtRemark.Text = ((GetStoreLocationLockBizActionVO)args.Result).ObjStoreLocationLock.Remark;
                        chkFreez.IsChecked = ((GetStoreLocationLockBizActionVO)args.Result).ObjStoreLocationLock.IsFreeze; ;
                        if (((GetStoreLocationLockBizActionVO)args.Result).ObjStoreLocationLock.IsFreeze)
                        {
                            CmdModify.IsEnabled = false;
                            chkFreez.IsEnabled = false;
                            cmbStoreNameBack.IsEnabled = false;
                            cmbBlockType.SelectedValue = (long)0;
                            cmbBlockType.IsEnabled = false;

                            cmbRackNameback.IsEnabled = false;
                            cmbShelfNameback.IsEnabled = false;
                            cmbBinNameback.IsEnabled = false;
                            cmdAddItems.IsEnabled = false;
                            txtRemark.IsReadOnly = true;
                        }
                        else
                        {
                            CmdModify.IsEnabled = true;
                            chkFreez.IsEnabled = true;
                            cmbStoreNameBack.IsEnabled = false;
                            cmbBlockType.SelectedValue = (long)0;
                            cmbBlockType.IsEnabled = true;

                            cmbRackNameback.IsEnabled = true;
                            cmbShelfNameback.IsEnabled = true;
                            cmbBinNameback.IsEnabled = true;
                            cmdAddItems.IsEnabled = true;
                            txtRemark.IsReadOnly = false;
                        }
                        objAnimation.Invoke(RotationType.Forward);
                    }
                    waitIndi.Close();
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                waitIndi.Close();
                throw;
            }
        }

        private void CmdUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to Revoke the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        GetItemStoreLocationLockBizActionVO BizAction = new GetItemStoreLocationLockBizActionVO();
                        BizAction.IsForUnBlockRecord = true;
                        BizAction.IsForMainRecord = true;
                        BizAction.ID = (dgStoreList.SelectedItem as StoreLocationLockVO).ID;
                        BizAction.UnitID = (dgStoreList.SelectedItem as StoreLocationLockVO).UnitID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                setup();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Selected Record Revoked Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        };
                        client.ProcessAsync(BizAction, User);
                        client.CloseAsync();
                    }
                };
                msgWD.Show();
            }
            //else if (dgStoreDetailList.SelectedItem != null)
            //{
            //    GetItemStoreLocationLockBizActionVO BizAction = new GetItemStoreLocationLockBizActionVO();
            //    BizAction.ObjStoreLocationLockDetailsVO = new StoreLocationLockVO();
            //    BizAction.IsForUnBlockRecord = true;
            //    BizAction.IsForMainRecord = false;
            //    BizAction.ID = (dgStoreDetailList.SelectedItem as StoreLocationLockVO).ID;
            //    BizAction.UnitID = (dgStoreDetailList.SelectedItem as StoreLocationLockVO).UnitID;

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                new MessageBoxControl.MessageBoxChildWindow("", "Selected Record UnBlocked Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    };
            //    client.ProcessAsync(BizAction, User);
            //    client.CloseAsync();
            //}
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please select Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        bool IsValid = false;
        private void cmdAddItems_Click(object sender, RoutedEventArgs e)
        {

            string sName = string.Empty;
            if (tempMasterList.Count > 0)
            {
                foreach (var item in tempMasterList.ToList())
                {
                    if (item.StoreID == StoreID)
                    {
                        IsValid = true;
                    }
                }
            }
            else
            {
                IsValid = true;
            }
            if (IsValid)
            {
                if (cmbStoreNameBack.SelectedItem != null && ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId > 0)
                {
                    frmSuspendStockSearchItems Itemswin = new frmSuspendStockSearchItems();
                    Itemswin._SelectedItemList = tempMasterList.ToList();
                    Itemswin.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
                    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    Itemswin.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Please select Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                IsValid = false;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can add only one store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //ItemList Itemswin = (ItemList)sender;
            frmSuspendStockSearchItems Itemswin = (frmSuspendStockSearchItems)sender;
            if (Itemswin.SelectedItems != null)
            {
                foreach (var item in Itemswin.SelectedItems)
                {
                    List<ItemStoreLocationDetailsVO> _ItemList = new List<ItemStoreLocationDetailsVO>();
                    _ItemList = tempMasterList.ToList();
                    bool IsExist = false;
                    foreach (var item1 in _ItemList)
                    {
                        if (item.ItemID == item1.ItemID)
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist == false)
                    {
                        ItemStoreLocationDetailsVO objVO = new ItemStoreLocationDetailsVO();
                        objVO.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
                        objVO.StoreName = item.Store;

                        if (item.RackID > 0)
                        {
                            objVO.RackID = item.RackID;
                            objVO.Rackname = item.Rack;
                        }
                        else
                            objVO.Rackname = "N/A";


                        if (item.ShelfID > 0)
                        {
                            objVO.ShelfID = item.ShelfID;
                            objVO.Shelfname = item.Shelf;
                        }
                        else
                        {
                            objVO.Shelfname = "N/A";
                        }

                        if (item.ContainerID > 0)
                        {
                            objVO.ContainerID = item.ContainerID;
                            objVO.Containername = item.Container;
                        }
                        else
                        {
                            objVO.Containername = "N/A";
                        }

                        if (item.ItemID > 0)
                        {
                            objVO.ItemID = item.ItemID;
                            objVO.ItemName = item.ItemName;
                        }
                        else
                            objVO.ItemName = "N/A";

                        objVO.BlockTypeID = ((MasterListItem)cmbBlockType.SelectedItem).ID;
                        objVO.BlockType = ((MasterListItem)cmbBlockType.SelectedItem).Description;

                        tempMasterList.Add(objVO);
                    }
                }
                grdStoreListback.ItemsSource = null;
                grdStoreListback.ItemsSource = tempMasterList;
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (tempMasterList.Count > 0)
            {
                SetCommandButtonState("Modify");
                string msgTitle = "Palash";
                msgText = "Are you sure you want to update Suspend the Record ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedModify);
                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("", "Record can not Empty, Please add Record in List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

            //if (tempMasterList.Count < 0)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //    new MessageBoxControl.MessageBoxChildWindow("", "Please select Store Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //}
            //else
            //    Save(true);
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Inventory_New/BlockStockCheck.aspx?StoreLocationID=" + ((StoreLocationLockVO)dgStoreList.SelectedItem).ID + "&StoreLocationUnitID=" + ((StoreLocationLockVO)dgStoreList.SelectedItem).UnitID), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("", "Please select Record from Block Request List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void Revoke_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to Revoke the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        GetItemStoreLocationLockBizActionVO BizAction = new GetItemStoreLocationLockBizActionVO();
                        BizAction.ObjStoreLocationLockDetailsVO = new StoreLocationLockVO();
                        BizAction.IsForUnBlockRecord = true;
                        BizAction.IsForMainRecord = true;
                        StoreLocationLockVO objDetailsVO = new StoreLocationLockVO();
                        objDetailsVO = dgStoreList.SelectedItem as StoreLocationLockVO;
                        BizAction.ObjStoreLocationLockDetailsVO.ID = objDetailsVO.ID;
                        BizAction.ObjStoreLocationLockDetailsVO.UnitID = objDetailsVO.UnitID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                if (((GetItemStoreLocationLockBizActionVO)args.Result).SuccessStatus == -1) // Added by Ashish Z. on dated 07102016 for Unrevoke the request while UnApprove Stock Adjustment..
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Unable to Revoke, Please Approve Stock Adjustment for this Request", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                                else if (((GetItemStoreLocationLockBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    setup();
                                    //if (dgStoreList.SelectedItem != null)
                                    //    GetStoreDetailsList((dgStoreList.SelectedItem as StoreLocationLockVO).ID, (dgStoreList.SelectedItem as StoreLocationLockVO).UnitID);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Selected Record Revoked Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW1.Show();
                                }
                            }
                        };
                        client.ProcessAsync(BizAction, User);
                        client.CloseAsync();
                    }
                };
                msgWD.Show();
            }
        }

        private void RevokeDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreDetailList.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to Revoke the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        GetItemStoreLocationLockBizActionVO BizAction = new GetItemStoreLocationLockBizActionVO();
                        BizAction.ObjStoreLocationLockDetailsVO = new StoreLocationLockVO();
                        BizAction.IsForUnBlockRecord = true;
                        BizAction.IsForMainRecord = false;
                        StoreLocationLockVO objDetailsVO = new StoreLocationLockVO();
                        objDetailsVO = dgStoreDetailList.SelectedItem as StoreLocationLockVO;
                        BizAction.ObjStoreLocationLockDetailsVO.ID = objDetailsVO.ID;
                        BizAction.ObjStoreLocationLockDetailsVO.UnitID = objDetailsVO.UnitID;
                        BizAction.ObjStoreLocationLockDetailsVO.BlockTypeID = objDetailsVO.BlockTypeID;
                        BizAction.ObjStoreLocationLockDetailsVO.StoreID = objDetailsVO.StoreID;
                        BizAction.ObjStoreLocationLockDetailsVO.RackID = objDetailsVO.RackID;
                        BizAction.ObjStoreLocationLockDetailsVO.ShelfID = objDetailsVO.ShelfID;
                        BizAction.ObjStoreLocationLockDetailsVO.ContainerID = objDetailsVO.ContainerID;
                        BizAction.ObjStoreLocationLockDetailsVO.ItemID = objDetailsVO.ItemID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                if (((GetItemStoreLocationLockBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    if (dgStoreList.SelectedItem != null)
                                        GetStoreDetailsList((dgStoreList.SelectedItem as StoreLocationLockVO).ID, (dgStoreList.SelectedItem as StoreLocationLockVO).UnitID);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Selected Record Revoked Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW1.Show();
                                }
                            }
                        };
                        client.ProcessAsync(BizAction, User);
                        client.CloseAsync();
                    }
                };
                msgWD.Show();
            }
        }

        private void hlkPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Inventory_New/BlockStockCheck.aspx?StoreLocationID=" + ((StoreLocationLockVO)dgStoreList.SelectedItem).ID + "&StoreLocationUnitID=" + ((StoreLocationLockVO)dgStoreList.SelectedItem).UnitID), "_blank");
            }

        }

        #endregion

        #region Validations Methods
        private void CheckDuplicatesWithExistingRecords()
        {
            try
            {
                if (waitIndi == null) waitIndi = new WaitIndicator();
                waitIndi.Show();
                GetStoreLocationLockBizActionVO BizAction = new GetStoreLocationLockBizActionVO();
                BizAction.IsViewClick = false;
                BizAction.IsForValidation = true;
                BizAction.ObjStoreLocationLock = new StoreLocationLockVO();
                BizAction.ObjStoreLocationLock.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (cmbStoreNameBack.SelectedItem != null)
                    BizAction.ObjStoreLocationLock.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
                if (cmbBlockType.SelectedItem != null)
                    BizAction.ObjStoreLocationLock.BlockTypeID = ((MasterListItem)cmbBlockType.SelectedItem).ID;
                if (cmbRackNameback.SelectedItem != null)
                    BizAction.ObjStoreLocationLock.RackID = ((MasterListItem)cmbRackNameback.SelectedItem).ID;
                if (cmbShelfNameback.SelectedItem != null)
                    BizAction.ObjStoreLocationLock.ShelfID = ((MasterListItem)cmbShelfNameback.SelectedItem).ID;
                if (cmbBinNameback.SelectedItem != null)
                    BizAction.ObjStoreLocationLock.ContainerID = ((MasterListItem)cmbBinNameback.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    waitIndi.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        if ((args.Result as GetStoreLocationLockBizActionVO).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "This Record is already found in Existing Request.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if ((args.Result as GetStoreLocationLockBizActionVO).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "This Record Item's is in Transit for Receive Issue", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if ((args.Result as GetStoreLocationLockBizActionVO).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "This Record Item's is in Transit for Receive Return Issue", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            if (AddStoreValidation())
                            {
                                if (BlockTypeValidation())
                                {
                                    if (duplicates())
                                    {
                                        ItemStoreLocationDetailsVO objVO = new ItemStoreLocationDetailsVO();
                                        objVO.StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
                                        objVO.StoreName = ((clsStoreVO)cmbStoreNameBack.SelectedItem).ClinicName + " " + ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreName;

                                        objVO.RackID = ((MasterListItem)cmbRackNameback.SelectedItem).ID;
                                        objVO.Rackname = ((MasterListItem)cmbRackNameback.SelectedItem).Description;

                                        if (cmbBinNameback.SelectedItem != null)
                                        {
                                            if (((MasterListItem)cmbShelfNameback.SelectedItem).ID > 0)
                                            {
                                                objVO.ShelfID = ((MasterListItem)cmbShelfNameback.SelectedItem).ID;
                                                objVO.Shelfname = ((MasterListItem)cmbShelfNameback.SelectedItem).Description;
                                            }
                                            else
                                                objVO.Shelfname = "N/A";
                                        }
                                        else
                                            objVO.Shelfname = "N/A";

                                        if (cmbBinNameback.SelectedItem != null)
                                        {
                                            if (((MasterListItem)cmbBinNameback.SelectedItem).ID > 0)
                                            {
                                                objVO.ContainerID = ((MasterListItem)cmbBinNameback.SelectedItem).ID;
                                                objVO.Containername = ((MasterListItem)cmbBinNameback.SelectedItem).Description;
                                            }
                                            else
                                                objVO.Containername = "N/A";
                                        }
                                        else
                                            objVO.Containername = "N/A";

                                        objVO.ItemName = "N/A";

                                        objVO.BlockType = ((MasterListItem)cmbBlockType.SelectedItem).Description;
                                        objVO.BlockTypeID = ((MasterListItem)cmbBlockType.SelectedItem).ID;
                                        tempMasterList.Add(objVO);
                                        grdStoreListback.ItemsSource = null;
                                        grdStoreListback.ItemsSource = tempMasterList;
                                        grdStoreListback.UpdateLayout();
                                    }
                                }
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                waitIndi.Close();
                throw;
            }
        }

        private Boolean duplicates()
        {
            string strMsg = string.Empty;
            bool result = true;
            long store, rack, shelf, bin, BlockType;
            store = rack = shelf = bin = BlockType = 0;
            int flag = 2;
            if (cmbStoreNameBack.SelectedItem != null)
                store = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
            if (cmbRackNameback.SelectedItem != null)
                rack = ((MasterListItem)cmbRackNameback.SelectedItem).ID;
            if (cmbShelfNameback.SelectedItem != null)
                shelf = ((MasterListItem)cmbShelfNameback.SelectedItem).ID;
            if (cmbBinNameback.SelectedItem != null)
                bin = ((MasterListItem)cmbBinNameback.SelectedItem).ID;
            if (cmbBlockType.SelectedItem != null)
                BlockType = ((MasterListItem)cmbBlockType.SelectedItem).ID;
            foreach (var item in tempMasterList)
            {
                //string strMsg = string.Empty;
                if (item.StoreID != StoreID)
                {
                    strMsg = "You have added '" + item.StoreName + "' Store, You can not add another one";
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //new MessageBoxControl.MessageBoxChildWindow("", "You have added '" + item.StoreName + "' Store, You can not add another one", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();
                    result = false;
                }
                else if (BlockType == Convert.ToInt64(BlockTypes.Rack))
                {
                    if (item.RackID == rack && item.ShelfID == 0 && item.ContainerID == 0)
                    {
                        strMsg = "Entire" + " '" + item.Rackname + "' " + "Rack already added";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Rackname + "' " + "Rack already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                    else if (item.RackID == rack && item.ShelfID != 0 && item.ContainerID != 0)
                    {
                        strMsg = "You have already added Shelf/Bin from" + " '" + item.Rackname + "' " + "Rack, Please delete it before add";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "You have already added Shelf/Bin from" + " '" + item.Rackname + "' " + "Rack, Please delete it before add", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                }
                else if (BlockType == Convert.ToInt64(BlockTypes.Shelf))
                {
                    if (item.RackID == rack && item.ShelfID == shelf)
                    {
                        strMsg = "Entire" + " '" + item.Shelfname + "' " + "Shelf already added";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Shelfname + "' " + "Shelf already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                    else if (item.RackID == rack && item.ShelfID == 0)
                    {
                        strMsg = "Entire" + " '" + item.Rackname + "' " + "Rack already added";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Rackname + "' " + "Rack already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                }
                else if (BlockType == Convert.ToInt64(BlockTypes.Bin))
                {
                    if (item.RackID == rack && item.ShelfID == shelf && item.ContainerID == bin)
                    {
                        strMsg = "Entire" + " '" + item.Containername + "' " + "Bin already added";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                        new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Containername + "' " + "Bin already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                    else if (item.RackID == rack && item.ShelfID == shelf)
                    {
                        strMsg = "Entire" + " '" + item.Shelfname + "' " + "Shelf already added";
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Shelfname + "' " + "Shelf already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //msgW1.Show();
                        result = false;
                    }
                    else if (item.RackID == rack)
                    {
                        strMsg = "Entire" + " '" + item.Rackname + "' " + "Rack already added";
                        // MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Rackname + "' " + "Rack already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        // msgW1.Show();
                        result = false;
                    }
                }
                //if (item.RackID == rack || (item.BlockTypeID != Convert.ToInt64(BlockTypes.Shelf) && item.BlockTypeID != Convert.ToInt64(BlockTypes.Bin)))
                //{
                //    if (item.RackID == rack && item.BlockTypeID != Convert.ToInt64(BlockTypes.Shelf) && item.BlockTypeID != Convert.ToInt64(BlockTypes.Bin))
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                //        new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Rackname + "' " + "Rack already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        msgW1.Show();
                //        result = false;
                //    }
                //    else if (item.RackID == rack && BlockType == Convert.ToInt64(BlockTypes.Rack)) //Rack
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                //        new MessageBoxControl.MessageBoxChildWindow("", "You have already added Shelf/Bin from" + " '" + item.Rackname + "' " + "Rack, Please delete it before add", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        msgW1.Show();
                //        result = false;
                //    }
                //}
                //else if (item.RackID == rack && item.ShelfID == shelf && item.BlockTypeID != Convert.ToInt64(BlockTypes.Bin))
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //    new MessageBoxControl.MessageBoxChildWindow("", "Entire" + " '" + item.Shelfname + "' " + "Shelf already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgW1.Show();
                //    result = false;
                //}
            }
            if (!string.IsNullOrEmpty(strMsg))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
               new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            return result;
        }

        private bool CheckFrontPannelSearch()
        {
            bool result = true;

            DateTime? _FromDate = dtpFromDate.SelectedDate;
            DateTime? _ToDate = dtpToDate.SelectedDate;

            if (_FromDate == null)
            {
                //dtpFromDate.SetValidation("Please Enter FromEffective Date");
                //dtpFromDate.RaiseValidationError();
                //dtpFromDate.Focus();
                //result = false;
            }
            else if (_ToDate == null)
            {
                //dtpToDate.SetValidation("Please Enter ToEffective Date");
                //dtpToDate.RaiseValidationError();
                //dtpToDate.Focus();
                //result = false;
            }
            else if (_FromDate > _ToDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter FromDate less than ToDate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
                dtpToDate.ClearValidationError();
                result = true;
            }
            return result;
        }

        private Boolean AddStoreValidation()
        {
            bool result = true;
            if (cmbStoreNameBack.SelectedItem == null || ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId == 0)
            {
                cmbStoreNameBack.TextBox.SetValidation("Please select Store");
                cmbStoreNameBack.TextBox.RaiseValidationError();
                cmbStoreNameBack.Focus();
                result = false;
            }
            else
                cmbStoreNameBack.TextBox.ClearValidationError();

            if (cmbBlockType.SelectedItem == null || ((MasterListItem)cmbBlockType.SelectedItem).ID == 0)
            {
                cmbBlockType.TextBox.SetValidation("Please select Block Type");
                cmbBlockType.TextBox.RaiseValidationError();
                cmbBlockType.Focus();
                result = false;
            }
            else
                cmbBlockType.TextBox.ClearValidationError();

            return result;
        }

        private Boolean BlockTypeValidation()
        {
            bool result = true;
            if (cmbBlockType.SelectedItem != null)
            {
                switch (((MasterListItem)cmbBlockType.SelectedItem).Description)
                {
                    case "Rack":
                        if (((MasterListItem)cmbRackNameback.SelectedItem).ID == 0)
                        {
                            cmbRackNameback.TextBox.SetValidation("Please select Rack");
                            cmbRackNameback.TextBox.RaiseValidationError();
                            cmbRackNameback.Focus();
                            result = false;
                        }
                        else
                            cmbRackNameback.TextBox.ClearValidationError();

                        break;
                    case "Shelf":
                        if (((MasterListItem)cmbRackNameback.SelectedItem).ID == 0 || ((MasterListItem)cmbShelfNameback.SelectedItem).ID == 0)
                        {
                            if (((MasterListItem)cmbRackNameback.SelectedItem).ID == 0)
                            {
                                cmbRackNameback.TextBox.SetValidation("Please select Rack");
                                cmbRackNameback.TextBox.RaiseValidationError();
                                cmbRackNameback.Focus();
                                result = false;
                            }
                            else
                                cmbRackNameback.TextBox.ClearValidationError();

                            if (((MasterListItem)cmbShelfNameback.SelectedItem).ID == 0)
                            {
                                cmbShelfNameback.TextBox.SetValidation("Please select Rack");
                                cmbShelfNameback.TextBox.RaiseValidationError();
                                cmbShelfNameback.Focus();
                                result = false;
                            }
                            else
                                cmbShelfNameback.TextBox.ClearValidationError();

                        }
                        break;
                    case "Bin":
                        if (((MasterListItem)cmbRackNameback.SelectedItem).ID == 0 || ((MasterListItem)cmbShelfNameback.SelectedItem).ID == 0 || ((MasterListItem)cmbBinNameback.SelectedItem).ID == 0)
                        {
                            if (((MasterListItem)cmbRackNameback.SelectedItem).ID == 0)
                            {
                                cmbRackNameback.TextBox.SetValidation("Please select Rack");
                                cmbRackNameback.TextBox.RaiseValidationError();
                                cmbRackNameback.Focus();
                                result = false;
                            }
                            else
                                cmbRackNameback.TextBox.ClearValidationError();
                            if (((MasterListItem)cmbShelfNameback.SelectedItem).ID == 0)
                            {
                                cmbShelfNameback.TextBox.SetValidation("Please select Shelf");
                                cmbShelfNameback.TextBox.RaiseValidationError();
                                cmbShelfNameback.Focus();
                                result = false;
                            }
                            else
                                cmbShelfNameback.TextBox.ClearValidationError();
                            if (((MasterListItem)cmbBinNameback.SelectedItem).ID == 0)
                            {
                                cmbBinNameback.TextBox.SetValidation("Please select Bin");
                                cmbBinNameback.TextBox.RaiseValidationError();
                                cmbBinNameback.Focus();
                                result = false;
                            }
                            else
                                cmbBinNameback.TextBox.ClearValidationError();
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
        #endregion

        #region Other Events(SelectionChanged, KeyDown)
        private void dgStoreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
                GetStoreDetailsList(((StoreLocationLockVO)dgStoreList.SelectedItem).ID, ((StoreLocationLockVO)dgStoreList.SelectedItem).UnitID);
        }

        private void GetStoreDetailsList(long ID, long UnitID)
        {
            GetStoreLocationLockBizActionVO BizAction = new GetStoreLocationLockBizActionVO();
            BizAction.IsViewClick = false;
            BizAction.Flag = 2;
            BizAction.ObjStoreLocationLock = new StoreLocationLockVO();
            BizAction.ObjStoreLocationLock.ID = ID; //Convert.ToInt64(((StoreLocationLockVO)dgStoreList.SelectedItem).ID);
            BizAction.ObjStoreLocationLock.UnitID = UnitID; //Convert.ToInt64(((StoreLocationLockVO)dgStoreList.SelectedItem).UnitID);
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.StoreLocationLock = (((GetStoreLocationLockBizActionVO)args.Result).StoreLocationLock);
                        MasterListDetails.Clear();
                        foreach (StoreLocationLockVO item in BizAction.StoreLocationLock)
                        {
                            MasterListDetails.Add(item);
                        }
                        dgStoreDetailList.ItemsSource = null;
                        dgStoreDetailList.ItemsSource = MasterListDetails;
                        dgStoreDetailList.SelectedIndex = -1;
                        dgStoreDetailList.UpdateLayout();
                    }

                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtRemarkFront_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setup();
                DataPager.PageIndex = 0;
            }
        }

        private void cmbBlockType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBlockType.SelectedItem != null)
            {
                switch (((MasterListItem)cmbBlockType.SelectedItem).Description)
                {
                    case "Rack":
                        grpRackShelfBinSelection.Visibility = Visibility.Visible;
                        cmbRackNameback.IsEnabled = true;
                        cmbShelfNameback.IsEnabled = false; cmbShelfNameback.SelectedValue = (long)0;
                        cmbBinNameback.IsEnabled = false; cmbBinNameback.SelectedValue = (long)0;
                        grpItemSelection.Visibility = Visibility.Collapsed;
                        break;
                    case "Shelf":
                        grpRackShelfBinSelection.Visibility = Visibility.Visible;
                        cmbRackNameback.IsEnabled = true;
                        cmbShelfNameback.IsEnabled = true;
                        cmbBinNameback.IsEnabled = false; cmbBinNameback.SelectedValue = (long)0;
                        grpItemSelection.Visibility = Visibility.Collapsed;
                        break;
                    case "Bin":
                        grpRackShelfBinSelection.Visibility = Visibility.Visible;
                        cmbRackNameback.IsEnabled = true;
                        cmbShelfNameback.IsEnabled = true;
                        cmbBinNameback.IsEnabled = true;
                        grpItemSelection.Visibility = Visibility.Collapsed;
                        break;
                    case "Item":
                        cmdAddItems.IsEnabled = true;
                        grpRackShelfBinSelection.Visibility = Visibility.Collapsed;
                        grpItemSelection.Visibility = Visibility.Visible;
                        break;
                    default:
                        grpRackShelfBinSelection.Visibility = Visibility.Collapsed;
                        cmbRackNameback.IsEnabled = false;
                        cmbShelfNameback.IsEnabled = false;
                        cmbBinNameback.IsEnabled = false;
                        grpItemSelection.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void cmbStoreNameBack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStoreNameBack.SelectedItem != null && ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId == 0)
            {
                cmbBlockType.IsEnabled = false;
            }
            else if (cmbStoreNameBack.SelectedItem != null && ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId > 0)
            {
                //if (tempMasterList.Count > 0)
                //{
                //    //foreach (var item in tempMasterList.ToList())
                //    //{
                //    //    if (item.StoreID == ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId)
                //    //    {
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //    new MessageBoxControl.MessageBoxChildWindow("", "You can add only one store in the List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();
                //    lnkAddStor.IsEnabled = false;
                //    cmdAddItems.IsEnabled = false;
                //    //    }
                //    //}
                //}
                //else
                //{
                //    lnkAddStor.IsEnabled = true;
                //    cmdAddItems.IsEnabled = true;
                //}
                StoreID = ((clsStoreVO)cmbStoreNameBack.SelectedItem).StoreId;
                cmbBlockType.IsEnabled = true;
                FillRack(StoreID);
            }
        }

        private void cmbRackNameback_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRackNameback.SelectedItem != null && ((MasterListItem)cmbRackNameback.SelectedItem).ID > 0)
            {
                RackID = ((MasterListItem)cmbRackNameback.SelectedItem).ID;
                FillShelf(StoreID, RackID);
            }
        }

        private void cmbShelfNameback_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbShelfNameback.SelectedItem != null && ((MasterListItem)cmbShelfNameback.SelectedItem).ID > 0)
            {
                ShelfID = ((MasterListItem)cmbShelfNameback.SelectedItem).ID;
                FillContainer(StoreID);
            }
        }
        #endregion
    }
}
