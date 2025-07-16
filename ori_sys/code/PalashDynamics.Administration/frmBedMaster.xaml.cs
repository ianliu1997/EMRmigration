using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.Windows.Resources;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using PalashDynamics.UserControls;
using System.Windows.Input;

namespace PalashDynamics.Administration
{
    public partial class frmBedMaster : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
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

        #region  Variables
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator objIndicator;
        string msgTitle = string.Empty;
        string msgText = string.Empty;
        long OTTableID;
        public bool isModify = false;
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        List<MasterListItem> objAminitiesList = new List<MasterListItem>();
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDBedMasterVO getstoreinfo;
        public bool Ammenities = false;
        List<MasterListItem> objBedMasterList = null;
        long WID = 0;
        Boolean IsView = false;
        #endregion

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
            }
        }

        public frmBedMaster()
        {
            InitializeComponent();
            this.DataContext = new clsIPDBedMasterVO();
            objIndicator = new WaitIndicator();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillWardMaster(0);
            FillRoomMaster(0);
            FillBedCategoryMaster();
            FillBedAmmenities();
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdBed.DataContext = MasterList;
            SetupPage();
            FillUnit();
        }
        
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
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    cmdModify.IsEnabled = false;  //Added by CDS
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
                case "FrontPanel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    isModify = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        public void SetupPage()
        {
            try
            {
                if (objIndicator == null) objIndicator = new WaitIndicator();
                objIndicator.Show();
                clsIPDGetBedMasterBizActionVO bizActionVO = new clsIPDGetBedMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations != null)
                    bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                getstoreinfo = new clsIPDBedMasterVO();
                bizActionVO.objBedMasterDetails = new List<clsIPDBedMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objBedMasterDetails = (((clsIPDGetBedMasterBizActionVO)args.Result).objBedMasterDetails);
                        if (bizActionVO.objBedMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetBedMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            grdBed.ItemsSource = null;
                            grdBed.ItemsSource = MasterList;
                        }
                    }
                    objIndicator.Close();
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objIndicator.Close();
                throw;
            }
        }
        void FillWardMaster(long iUnitId)
        {
            try
            {
                if (iUnitId > 0)
                {
                    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                    BizAction.MasterTable = MasterTableNameList.M_WardMaster;
                    BizAction.Parent = new KeyValue();
                    BizAction.Parent.Key = "1";
                    BizAction.Parent.Value = "Status";
                    if (iUnitId > 0)
                        BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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
                            cmbWard.ItemsSource = null;
                            cmbWard.ItemsSource = objList;
                            if (IsView == true)
                                cmbWard.SelectedValue = this.WID;
                            else
                                cmbWard.SelectedItem = objList[0];
                        }

                        //if (this.DataContext != null)
                        //{
                        //    cmbWard.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).WardID;
                        //}
                    };
                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    cmbWard.ItemsSource = null;
                    cmbWard.ItemsSource = objList;
                    cmbWard.SelectedItem = objList[0];
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void FillBedAmmenities()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BedAmmenitiesMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objBedMasterList = new List<MasterListItem>();

                        objBedMasterList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        dgAmmenityList.ItemsSource = null;
                        dgAmmenityList.ItemsSource = objBedMasterList;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void FillUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                        cmbUnit.ItemsSource = null;
                        cmbUnit.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbUnit.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).UnitID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void FillRoomMaster(long WardID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RoomMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";

                //if (WardID > 0)
                //    BizAction.Parent = new KeyValue { Key = WardID, Value = "WardID" };

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
                        cmbRoom.ItemsSource = null;
                        cmbRoom.ItemsSource = objList;
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbRoom.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).RoomID;
                    //}
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void FillBedCategoryMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                        cmbBedCategory.ItemsSource = null;
                        cmbBedCategory.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbBedCategory.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).BedCategoryID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void ClearUI()
        {
            IsView = false;
            this.grdBackPanel.DataContext = new clsIPDBedMasterVO();
            cmbWard.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).WardID;
            cmbRoom.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).RoomID;
            cmbBedCategory.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).BedCategoryID;
            cmbUnit.SelectedValue = ((clsIPDBedMasterVO)this.DataContext).UnitID;
            objBedMasterList = new List<MasterListItem>();
            objAminitiesList = new List<MasterListItem>();
        }

        #region Validation
        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                txtStoreName.SetValidation("Please Enter Bed Description");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                return false;
            }
            else if (cmbUnit.SelectedItem == null || ((MasterListItem)cmbUnit.SelectedItem).ID == 0)
            {
                cmbUnit.TextBox.SetValidation("Please select the Bed Unit");
                cmbUnit.TextBox.RaiseValidationError();
                cmbUnit.TextBox.Focus();
                return false;
            }
            else if (cmbWard.SelectedItem == null || ((MasterListItem)cmbWard.SelectedItem).ID == 0)
            {
                cmbWard.TextBox.SetValidation("Please select the Ward");
                cmbWard.TextBox.RaiseValidationError();
                cmbWard.TextBox.Focus();
                return false;
            }
            else if (cmbRoom.SelectedItem == null || ((MasterListItem)cmbRoom.SelectedItem).ID == 0)
            {
                cmbRoom.TextBox.SetValidation("Please select the Room");
                cmbRoom.TextBox.RaiseValidationError();
                cmbRoom.TextBox.Focus();
                return false;
            }
            else if (cmbBedCategory.SelectedItem == null || ((MasterListItem)cmbBedCategory.SelectedItem).ID == 0)
            {
                cmbBedCategory.TextBox.SetValidation("Please select the Bed Class");
                cmbBedCategory.TextBox.RaiseValidationError();
                cmbBedCategory.TextBox.Focus();
                return false;
            }
            else
            {
                txtStoreCode.ClearValidationError();
                txtStoreName.ClearValidationError();
                cmbUnit.TextBox.ClearValidationError();
                cmbWard.TextBox.ClearValidationError();
                cmbRoom.TextBox.ClearValidationError();
                cmbBedCategory.TextBox.ClearValidationError();
                return true;
            }
        }

        #endregion

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

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsIPDBedMasterVO();
            ClearUI();
            isModify = false;
            SetCommandButtonState("New");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                FillBedAmmenities();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();
            }
        }
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsIPDAddUpdateBedMasterBizActionVO bizActionVO = new clsIPDAddUpdateBedMasterBizActionVO();
                    clsIPDBedMasterVO addNewBlockVO = new clsIPDBedMasterVO();

                    isModify = false;
                    addNewBlockVO = CreateFormData();
                    addNewBlockVO.Status = true;
                    addNewBlockVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
                        addNewBlockVO.BedUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewBlockVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewBlockVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewBlockVO.AddedDateTime = System.DateTime.Now;
                    addNewBlockVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizActionVO.objBedMatserDetails = addNewBlockVO;

                    if (dgAmmenityList.ItemsSource != null)
                    {
                        bizActionVO.objBedMatserDetails.AmmenityDetails = new List<MasterListItem>();

                        List<MasterListItem> objBedList = ((List<MasterListItem>)dgAmmenityList.ItemsSource);
                        foreach (var item in objBedList)
                        {
                            if (item.Status == true)
                            {
                                bizActionVO.objBedMatserDetails.IsAmmenity = true;
                                bizActionVO.objBedMatserDetails.AmmenityDetails.Add(item);
                            }
                        }
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is Successfully Saved.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();

                    //clsIPDAddUpdateBedMasterBizActionVO bizActionVO = new clsIPDAddUpdateBedMasterBizActionVO();
                    //clsIPDBedMasterVO addNewBlockVO = new clsIPDBedMasterVO();
                    //if (isModify == true)
                    //{
                    //    addNewBlockVO.ID = ((clsIPDBedMasterVO)grdBed.SelectedItem).ID;
                    //}
                    //if (!String.IsNullOrEmpty(txtStoreName.Text))
                    //    addNewBlockVO.Description = txtStoreName.Text;
                    //if (!String.IsNullOrEmpty(txtStoreCode.Text))
                    //    addNewBlockVO.Code = txtStoreCode.Text;
                    //if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
                    //    addNewBlockVO.BedUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    //if (cmbWard.SelectedItem != null && ((MasterListItem)cmbWard.SelectedItem).ID > 0)
                    //    addNewBlockVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                    //if (cmbRoom.SelectedItem != null && ((MasterListItem)cmbRoom.SelectedItem).ID > 0)
                    //    addNewBlockVO.RoomID = ((MasterListItem)cmbRoom.SelectedItem).ID;
                    //if (cmbBedCategory.SelectedItem != null && ((MasterListItem)cmbBedCategory.SelectedItem).ID > 0)
                    //    addNewBlockVO.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;
                    //if (chkIsNonCensus.IsChecked == true)
                    //{
                    //    addNewBlockVO.IsNonCensus = true;
                    //}

                    //if (isModify == true && grdBed.SelectedItem != null)
                    //{
                    //    addNewBlockVO.Status = ((clsIPDBedMasterVO)grdBed.SelectedItem).Status;
                    //}

                    //addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewBlockVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewBlockVO.RoomID = ((MasterListItem)cmbRoom.SelectedItem).ID;
                    //addNewBlockVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                    //addNewBlockVO.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;
                    //addNewBlockVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //addNewBlockVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    //addNewBlockVO.AddedDateTime = System.DateTime.Now;
                    //addNewBlockVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    //bizActionVO.objBedMatserDetails = addNewBlockVO;
                    //var item = from r in objBedMasterList
                    //           where r.Status == true
                    //           select r;
                    //if (item != null && item.ToList().Count > 0)
                    //{
                    //    bizActionVO.objBedMatserDetails.IsAmmenity = true;
                    //}
                    //else
                    //{
                    //    bizActionVO.objBedMatserDetails.IsAmmenity = false;
                    //}
                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    //client.ProcessCompleted += (s, args) =>
                    //{
                    //    if (args.Error == null && args.Result != null)
                    //    {
                    //        if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 1)
                    //        {
                    //            msgText = "Record is successfully submitted!";
                    //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //            msgWindow.Show();
                    //            OTTableID = 0;
                    //            SetupPage();
                    //            objAnimation.Invoke(RotationType.Backward);
                    //            SetCommandButtonState("Save");
                    //        }
                    //        else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 2)
                    //        {
                    //            msgText = "Record cannot be save because CODE already exist!";
                    //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //            msgWindow.Show();
                    //        }
                    //        else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 3)
                    //        {
                    //            msgText = "Record cannot be save because DESCRIPTION already exist!";
                    //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //            msgWindow.Show();
                    //        }
                    //    }
                    //};
                    //client.ProcessAsync(bizActionVO, new clsUserVO());
                    //client.CloseAsync();
                }
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
                if (Validation())
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update ?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msg_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        void msg_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIPDAddUpdateBedMasterBizActionVO bizactionVO = new clsIPDAddUpdateBedMasterBizActionVO();
                clsIPDBedMasterVO addNewOtTableVO = new clsIPDBedMasterVO();
                try
                {
                    isModify = true;
                    addNewOtTableVO = CreateFormData();
                    addNewOtTableVO.Status = true;
                    if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
                        addNewOtTableVO.BedUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    addNewOtTableVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.objBedMatserDetails = addNewOtTableVO;

                    if (dgAmmenityList.ItemsSource != null)
                    {
                        bizactionVO.objBedMatserDetails.AmmenityDetails = new List<MasterListItem>();

                        List<MasterListItem> objBedList = ((List<MasterListItem>)dgAmmenityList.ItemsSource);
                        foreach (var item in objBedList)
                        {
                            if (item.Status == true)
                            {
                                bizactionVO.objBedMatserDetails.IsAmmenity = true;
                                bizactionVO.objBedMatserDetails.AmmenityDetails.Add(item);
                            }
                        }
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is Successfully Saved.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateBedMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdBackPanel.DataContext = new clsIPDBedMasterVO();
                IsView = false;
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmIPDAdmissionConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Admission Configuration";

                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdBed.DataContext = MasterList;
                SetupPage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");

                if (grdBed.SelectedItem != null)
                {
                    cmdModify.IsEnabled = ((clsIPDBedMasterVO)grdBed.SelectedItem).Status;
                    grdBackPanel.DataContext = ((clsIPDBedMasterVO)grdBed.SelectedItem).DeepCopy();
                    clsIPDBedMasterVO objBed = (clsIPDBedMasterVO)grdBed.SelectedItem;
                    objBedMasterList.ForEach(z => z.Status = false);
                    OTTableID = objBed.ID;
                    grdBackPanel.DataContext = objBed;
                    cmbUnit.SelectedValue = objBed.BedUnitID;
                    IsView = true;
                    WID = objBed.WardID;
                    cmbWard.SelectedValue = objBed.WardID;
                    cmbRoom.SelectedValue = objBed.RoomID;
                    cmbBedCategory.SelectedValue = objBed.BedCategoryID;

                    List<MasterListItem> objList = new List<MasterListItem>();

                    foreach (var item in objBed.AmmenityDetails)
                    {
                        foreach (var Beditem in objBedMasterList)
                        {
                            if (Beditem.ID == item.ID)
                            {
                                Beditem.Status = item.Status;
                            }
                        }
                    }
                    dgAmmenityList.ItemsSource = null;
                    dgAmmenityList.ItemsSource = objBedMasterList;

                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            //try
            //{
            //    SetCommandButtonState("View");
            //    cmdModify.IsEnabled = ((clsIPDBedMasterVO)grdBed.SelectedItem).Status;
            //    if (grdBed.SelectedItem != null)
            //    {
            //        clsIPDBedMasterVO objBed = (clsIPDBedMasterVO)grdBed.SelectedItem;
            //        objBedMasterList.ForEach(z => z.Status = false);
            //        OTTableID = objBed.ID;
            //        grdBackPanel.DataContext = objBed;
            //        cmbRoom.SelectedValue = objBed.RoomID;
            //        cmbBedCategory.SelectedValue = objBed.BedCategoryID;
            //        IsView = true;
            //        WID = objBed.WardID;
            //        cmbWard.SelectedValue = objBed.WardID;
            //        cmbUnit.SelectedValue = objBed.BedUnitID;
            //        chkIsNonCensus.IsChecked = objBed.IsNonCensus;
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        foreach (var Beditem in objBed.AmmenityDetails)
            //        {
            //            objBedMasterList.Where(z => z.ID == Beditem.ID).ToList().ForEach(z => z.Status = Beditem.Status);
            //        }
            //        objAminitiesList = objBed.AmmenityDetails;
            //        dgAmmenityList.ItemsSource = null;
            //        dgAmmenityList.ItemsSource = objBedMasterList;
            //        objAnimation.Invoke(RotationType.Forward);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
                {
                    ((TextBox)e.OriginalSource).Text = ((TextBox)e.OriginalSource).Text.ToTitleCase();
                    ((TextBox)e.OriginalSource).ClearValidationError();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDUpdateBedMasterStatusBizActionVO objStatus = new clsIPDUpdateBedMasterStatusBizActionVO();
                objStatus.BedStatus = new clsIPDBedMasterVO();
                objStatus.BedStatus = (clsIPDBedMasterVO)grdBed.SelectedItem;
                objStatus.BedStatus.Status = ((clsIPDBedMasterVO)grdBed.SelectedItem).Status;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (objStatus.BedStatus.Status == false)
                        {
                            msgText = "Bed Status Deactivated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "Bed Status Activated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    //SetCommandButtonState("View");
                };
                client.ProcessAsync(objStatus, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStatusAmmenity_Click(object sender, RoutedEventArgs e)
        {
            Ammenities = false;
            if (dgAmmenityList.SelectedItem != null)
            {
                MasterListItem objRoom = new MasterListItem();
                List<MasterListItem> objList = new List<MasterListItem>();

                objList = (List<MasterListItem>)dgAmmenityList.ItemsSource;
                objRoom = (MasterListItem)dgAmmenityList.SelectedItem;

                foreach (var item in objList)
                {
                    if (item == objRoom)
                    {
                        item.Status = objRoom.Status;
                    }
                }
                dgAmmenityList.ItemsSource = null;
                dgAmmenityList.ItemsSource = objList;

                foreach (var item in objList)
                {
                    if (item.Status == true)
                    {
                        Ammenities = true;
                    }
                }
            }

            //Ammenities = false;
            //if (dgAmmenityList.SelectedItem != null)
            //{
            //    if (((CheckBox)sender).IsChecked == true && !objAminitiesList.Where(z => z.ID == ((MasterListItem)dgAmmenityList.SelectedItem).ID).Any())
            //    {
            //        objAminitiesList.Add((MasterListItem)dgAmmenityList.SelectedItem);
            //    }
            //    else
            //    {
            //        objAminitiesList.Where(z => z.ID == ((MasterListItem)dgAmmenityList.SelectedItem).ID).FirstOrDefault().Status = (Boolean)((CheckBox)sender).IsChecked;
            //        objAminitiesList.Remove((MasterListItem)dgAmmenityList.SelectedItem);
            //    }
            //}
        }

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUnit.SelectedItem != null)
                FillWardMaster(((MasterListItem)cmbUnit.SelectedItem).ID);
        }

        //private void cmbWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbWard.SelectedItem != null)
        //        FillRoomMaster(((MasterListItem)cmbWard.SelectedItem).ID);
        //}

        private clsIPDBedMasterVO CreateFormData()
        {
            clsIPDBedMasterVO addNewBedVO = new clsIPDBedMasterVO();
            if (isModify == true)
            {
                addNewBedVO.ID = OTTableID;
            }
            else
            {
                addNewBedVO.ID = 0;
                addNewBedVO.Status = true;
            }
            addNewBedVO = (clsIPDBedMasterVO)this.grdBackPanel.DataContext;
            addNewBedVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
            addNewBedVO.RoomID = ((MasterListItem)cmbRoom.SelectedItem).ID;
            addNewBedVO.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;
            return addNewBedVO;
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                try
                {
                    MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                    MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                    PageSize = 15;
                    this.dataGrid2Pager.DataContext = MasterList;
                    this.grdBed.DataContext = MasterList;
                    SetupPage();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


    }
}
