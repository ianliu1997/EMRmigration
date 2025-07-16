using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory.Indent;
using PalashDynamics.Pharmacy.Inventory;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects.Master;
using OPDModule.Forms;

namespace PalashDynamics.Pharmacy
{
    public partial class StoreIndent : UserControl, IInitiateCIMS
    {
        #region Global Variables
        ChildWindow ObjChildItemStock = new ChildWindow();
        bool IsCentralPurchase = false;
        bool IsDirectIndent = false;
        bool IsInterClinicIndent = false; //***//
        clsIndentMasterVO SelectedIndent { get; set; }
        string SelectedCommand = "Cancel";
        private SwivelAnimation objAnimation;
        List<MasterListItem> UOMLIstNew = new List<MasterListItem>();
        public ObservableCollection<clsIndentDetailVO> DeleteIndentAddedItems { get; set; }
        private long _ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
            }
        }
        public ObservableCollection<clsIndentDetailVO> IndentAddedItems { get; set; }
        bool IsPageLoaded = false;
        WaitIndicator Indicatior = null;
        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        bool UserStoreAssigned = false;
        //Added By Pallavi
        MessageBoxControl.MessageBoxChildWindow mgbx = null;

        int ClickedFlag1 = 0;
        clsUserVO userVO = new clsUserVO();
        #endregion

        #region Initiate/Constructor/Loaded Events
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    CmdForward.Visibility = Visibility.Collapsed;
                    CmdApprove.Visibility = Visibility.Collapsed;

                    break;
                case "Approve":
                    CmdForward.Visibility = Visibility.Visible;
                    CmdApprove.Visibility = Visibility.Visible;
                    CmdNew.Visibility = Visibility.Collapsed;
                    //CmdSave.Visibility = Visibility.Collapsed;
                    //CmdModify.Visibility = Visibility.Collapsed;
                    //dgIndentList.Columns[0].Visibility = Visibility.Collapsed;

                    break;
            }
        }

        public StoreIndent()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            FillUOM();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            grdStoreIndent.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdStoreIndent_CellEditEnded);
            dtpAddIndentDate.SelectedDate = DateTime.Now.Date;
            dtpAddIndentDate.IsEnabled = false;
            //======================================================
            DeleteIndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.dtpFromDate.SelectedDate = DateTime.Now;
                this.dtpToDate.SelectedDate = DateTime.Now;
                FillStore();
                //FillStores(ClinicID);
                //FillIndentList();
                Indicatior.Close();
                IsPageLoaded = true;
                IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                grdStoreIndent.ItemsSource = IndentAddedItems;
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            }
            //IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
            //grdStoreIndent.ItemsSource = IndentAddedItems;
            //FillOrderBookingList();
        }
        #endregion

        #region 'Paging'

        public PagedSortableCollectionView<clsIndentMasterVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillIndentList();
        }
        #endregion

        #region Clicked Events
        private void CmdConvertToPR_Click(object sender, RoutedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                if (ValidationForConvertToPR())
                {
                    //FrmStoreIndent Indent = new FrmStoreIndent();
                    //Indent.IsFromConvertToPR = true;
                    //Indent.IndentDetail = dgIndentList.SelectedItem as clsIndentMasterVO;
                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
                    //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    //mElement.Text = "Material Transfer Request";
                    //((IApplicationConfiguration)App.Current).OpenMainContent(Indent);
                    FormHeaderChange("Material Transfer Request");
                    ConvertToPR();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Indent from List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void CmdForward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidationForForwardIndent())
                {
                    this.DataContext = new clsIndentDetailVO();
                    IndentAddedItems.Clear();
                    foreach (var item in SelectedIndent.IndentDetailsList)
                    {
                        item.UOMList = UOMLIstNew;
                        item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                        item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;
                        IndentAddedItems.Add(item);
                    }

                    var results1 = from r in ((List<clsStoreVO>)cmbFromStoreName.ItemsSource)
                                   where r.StoreId == this.SelectedIndent.FromStoreID
                                   select r;

                    foreach (clsStoreVO item in results1)
                    {
                        cmbAddFromStoreName.SelectedItem = item;
                        //cmbAddToStoreName.SelectedItem = item;
                    }

                    var results2 = from r in ((List<clsStoreVO>)cmbFromStoreName.ItemsSource)
                                   select r;

                    cmbAddToStoreName.SelectedItem = results2.ToList()[0];
                    //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                    //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                    this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                    this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                    //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                    //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                    this.IndentNumber.Text = SelectedIndent.IndentNumber;
                    this.txtRemark.Text = SelectedIndent.Remark;
                    this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                    CmdConvertToPR.IsEnabled = false;
                    CmdPrint.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdModify.IsEnabled = true;
                    CmdForward.IsEnabled = false;
                    CmdApprove.IsEnabled = false;
                    //Disable for Forward
                    grdStoreIndent.IsEnabled = false;
                    chkFreezIndent.IsEnabled = false;
                    lnkAddItems.IsEnabled = false;
                    this.cmbAddFromStoreName.IsEnabled = false;
                    this.cmbAddToStoreName.IsEnabled = true;
                    this.dtpAddIndentDate.IsEnabled = false;
                    this.dtpAddDueDate.IsEnabled = true;
                    this.txtRemark.IsEnabled = false;
                    this.chkFreezIndent.IsEnabled = false;
                    CmdShowStock.IsEnabled = false;
                    CmdModify.Visibility = Visibility.Visible;
                    //CmdChangeApprove.Visibility = Visibility.Collapsed;
                    CmdCancel.Visibility = Visibility.Visible;
                    objAnimation.Invoke(RotationType.Forward);
                    SelectedCommand = "Forward";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {

                    if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected || SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled || SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                    {
                        string strMsg = string.Empty;
                        if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected) strMsg = "Indent is already Rejected";
                        else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled) strMsg = "Indent is already Cancelled";
                        else if (SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted) strMsg = "Indent is Forcefully Closed";
                        if (!string.IsNullOrEmpty(strMsg))
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                    else if (SelectedIndent.IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is already approved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        if (SelectedIndent.IsFreezed)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Approve indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                    BizAction.objIndent = SelectedIndent;
                                    if (BizAction.objIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Can't approve indent it is forcefully closed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbox.Show();
                                        return;
                                    }
                                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)


                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                    else
                                        BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    BizAction.objIndent.IsApproved = true;
                                    BizAction.objIndent.IsAuthorized = true;
                                    BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                    BizAction.objIndent.AuthorizationDate = DateTime.Now;

                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                mgbox.Show();
                                                FillIndentList();
                                            }
                                        }
                                    };
                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                }
                            };
                            mgBox.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is not Freezed \n Please Freeze Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Indent is Selected \n Please select a Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdChangeAndApprove_Click(object sender, RoutedEventArgs e)    //By Umesh
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    if (ModifyValidation())
                    {
                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Modify indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                BizAction.objIndent = SelectedIndent;
                                if (BizAction.objIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Can't approve indent it is forcefully closed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    mgbox.Show();
                                    return;
                                }
                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                                    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                else
                                    BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                FrmStoreIndent Indent = new FrmStoreIndent();
                                Indent.IsFromChangAndApprove = true;
                                Indent.IndentDetail = SelectedIndent;
                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                mElement.Text = "Store Indent";
                                ((IApplicationConfiguration)App.Current).OpenMainContent(Indent);
                                //UpdateForChangeAndApprove();
                            }
                        };
                        mgBox.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Indent is Selected \n Please select a Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed == true)
                {
                    MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Freeze Indent Number " + SelectedIndent.IndentNumber + "", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsUpdateIndentOnlyForFreezeBizActionVO BizAction = new clsUpdateIndentOnlyForFreezeBizActionVO();
                            BizAction.IndentID = Convert.ToInt64(SelectedIndent.ID);

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null)
                                {
                                    if (arg.Result != null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Freezed & Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbox.Show();
                                    }
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        else
                        {
                            ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed = false;
                        }
                    };

                    mgBox.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewIndent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                    clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                    BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                    BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
                    BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //acTextBox = new TextBox();
                                dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                                SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                                this.DataContext = new clsIndentDetailVO();
                                IndentAddedItems.Clear();

                                foreach (var item in SelectedIndent.IndentDetailsList)
                                {
                                    if (UOMLIstNew.Count > 0)
                                    {
                                        item.UOMList = UOMLIstNew;
                                        item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                        item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;
                                    }
                                    IndentAddedItems.Add(item);
                                }

                                var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                              where r.ClinicId == this.SelectedIndent.IndentUnitID && r.StoreId == this.SelectedIndent.FromStoreID
                                              //&& r.ClinicId == this.SelectedIndent.UnitID
                                              select r;

                                foreach (clsStoreVO item in results)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                }

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddToStoreName.SelectedItem = item;
                                }
                                this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                                this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                                this.IndentNumber.Text = SelectedIndent.IndentNumber;
                                this.txtRemark.Text = SelectedIndent.Remark;
                                this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                                ViewIndent();
                                if (SelectedIndent.IsAgainstPatient)
                                {
                                    chkAgainstPatient.IsChecked = SelectedIndent.IsAgainstPatient;
                                    txtMRNo.Text = SelectedIndent.MRNo;
                                    txtPatientName.Text = SelectedIndent.PatientName;
                                }
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = Validate();
                int Count = 0;
                //ClickedFlag1 += 1;
                //if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId > 0 && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId > 0 && IndentAddedItems.Count != 0) //grdStoreIndent.ItemsSource != 0)
                //{
                //    foreach (clsStoreVO item in UserStores)
                //    {
                //        if (item.StoreId == ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId)
                //        {
                Count = 0;
                ClickedFlag1 += 1;
                if (ClickedFlag1 == 1)
                {

                    if (flag)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow("palash", "Do you want to Convert to PR?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                        msgWin.Show();

                        //clsAddIndentBizActionVO BizAction = new clsAddIndentBizActionVO();

                        //BizAction.objIndent = new clsIndentMasterVO();
                        //BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                        //BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                        //BizAction.objIndent.IndentNumber = null;
                        //BizAction.objIndent.TransactionMovementID = 0;
                        //BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                        //BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                        //BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                        //BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                        //BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
                        //BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                        //BizAction.objIndent.AuthorizationDate = DateTime.Now; // dtpAuthorizationDate.SelectedDate;
                        //BizAction.objIndent.Remark = txtRemark.Text;
                        //BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                        //BizAction.objIndent.Status = true;


                        //BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                        //BizAction.objIndent.IsForwarded = false;
                        //BizAction.objIndent.IsApproved = false;


                        //BizAction.objIndentDetailList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
                        //BizAction.objIndentDetailList.Select(indentItems => { indentItems.IndentUnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId; return indentItems; }).ToList();

                        //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        //client.ProcessCompleted += (s, arg) =>
                        //{
                        //    ClickedFlag1 = 0;
                        //    if (arg.Error == null && ((clsAddIndentBizActionVO)arg.Result).objIndent != null)
                        //    {
                        //        if (arg.Result != null)
                        //        {
                        //            CmdPrint.IsEnabled = true;
                        //            CmdNew.IsEnabled = true;
                        //            CmdSave.IsEnabled = false;
                        //            CmdCancel.IsEnabled = false;
                        //            CmdForward.IsEnabled = true;
                        //            CmdModify.IsEnabled = false;
                        //            CmdApprove.IsEnabled = true;
                        //            SelectedCommand = "Save";
                        //            cmbAddFromStoreName.SelectedValue = (long)0;
                        //            cmbAddToStoreName.SelectedValue = (long)0;


                        //            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Saved Successfully With IndentNumber " + ((clsAddIndentBizActionVO)arg.Result).objIndent.IndentNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //            mgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        //            {
                        //                objAnimation.Invoke(RotationType.Backward);
                        //            };
                        //            mgbox.Show();

                        //            FillIndentList();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while saving indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //        mgbox.Show();
                        //    }
                        //};
                        //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    }
                    else
                        ClickedFlag1 = 0;
                }
                //break;
                //        }
                //        else
                //        {
                //            Count += 1;
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    bool flag = Validate();



                    if (flag)
                    {
                        if (SelectedCommand == "Forward")
                        {
                            if (SelectedIndent.ToStoreID == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "ToStore is not changed.\n Change ToStore then Modify it.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgBox.Show();
                                ClickedFlag1 = 0;
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to forward indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                        BizAction.objIndent = new clsIndentMasterVO();
                                        BizAction.objIndent.ID = SelectedIndent.ID;
                                        BizAction.objIndent.Date = SelectedIndent.Date;
                                        BizAction.objIndent.Time = SelectedIndent.Time;
                                        BizAction.objIndent.TransactionMovementID = SelectedIndent.TransactionMovementID;
                                        BizAction.objIndent.FromStoreID = SelectedIndent.FromStoreID;
                                        BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                        BizAction.objIndent.DueDate = SelectedIndent.DueDate;
                                        BizAction.objIndent.IndentCreatedByID = SelectedIndent.IndentCreatedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                        BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;
                                        BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID;
                                        BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate;
                                        BizAction.objIndent.Remark = SelectedIndent.Remark;
                                        BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                        BizAction.objIndent.Status = true;
                                        BizAction.objIndent.IsFreezed = SelectedIndent.IsFreezed;
                                        BizAction.objIndent.IsForwarded = true;
                                        BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                                        BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                        //By anjali...................................
                                        BizAction.objIndent.IsIndent = 1;
                                        //BizAction.objIndent.IsIndent = true;
                                        //..............................................

                                        BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                        client.ProcessCompleted += (s, arg) =>
                                        {
                                            ClickedFlag1 = 0;
                                            if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                            {
                                                if (arg.Result != null)
                                                {
                                                    CmdPrint.IsEnabled = true;
                                                    //CmdNew.IsEnabled = true;
                                                    CmdSave.IsEnabled = false;
                                                    CmdCancel.IsEnabled = false;
                                                    CmdModify.IsEnabled = false;
                                                    CmdForward.IsEnabled = true;
                                                    CmdApprove.IsEnabled = true;
                                                    CmdShowStock.IsEnabled = true;
                                                    CmdConvertToPR.IsEnabled = true;

                                                    SelectedCommand = "Modify";

                                                    FillIndentList();
                                                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Forwarded Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                                    {
                                                        objAnimation.Invoke(RotationType.Backward);
                                                    };
                                                    mgbox.Show();
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while forwarding indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                                mgbox.Show();
                                            }
                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                        ClickedFlag1 = 0;
                                };
                                mgBox.Show();
                            }
                        }
                        else
                        {
                            //string msgTitle = "";
                            //string msgDesc = "";

                            //if ((bool)chkFreezIndent.IsChecked)
                            //{
                            //    msgTitle = "Freeze and Update Indent";
                            //    msgDesc = "Do You want to Freeze and Update the Indent";
                            //}
                            //else
                            //{
                            //    msgTitle = "Update Indent";
                            //    msgDesc = "Do You want to Update the Indent";
                            //}

                            if (ModifyValidation())
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Modify indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        UpdateForChangeAndApprove();
                                        //clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                        //BizAction.objIndent = SelectedIndent;
                                        //if (BizAction.objIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                                        //{
                                        //    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Can't approve indent it is forcefully closed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        //    mgbox.Show();
                                        //    return;
                                        //}
                                        //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                                        //    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                        //else
                                        //    BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        //FrmStoreIndent Indent = new FrmStoreIndent();
                                        //Indent.IsFromChangAndApprove = true;
                                        //Indent.IndentDetail = SelectedIndent;
                                        //UserControl rootPage = Application.Current.RootVisual as UserControl;
                                        //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                        //mElement.Text = "Store Indent";
                                        //((IApplicationConfiguration)App.Current).OpenMainContent(Indent);
                                        //------------------------------------------------------------------------------------------------------------------------------//
                                        //clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();

                                        //BizAction.objIndent = new clsIndentMasterVO();
                                        //BizAction.objIndent.ID = SelectedIndent.ID;
                                        //BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                                        //BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                                        //BizAction.objIndent.TransactionMovementID = 0;
                                        //BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                                        //BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                        //BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                                        //BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                        //BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
                                        //BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                        //BizAction.objIndent.AuthorizationDate = DateTime.Now;// dtpAuthorizationDate.SelectedDate;
                                        //BizAction.objIndent.Remark = txtRemark.Text;
                                        //BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                        //BizAction.objIndent.Status = true;
                                        //BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                                        //BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
                                        //BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                                        //BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                        //BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                        //BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
                                        ////By anjali..........................................
                                        //// BizAction.objIndent.IsIndent = true;
                                        //BizAction.objIndent.IsIndent = 1;
                                        ////...................................................
                                        //BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                                        //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                        //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                        //client.ProcessCompleted += (s, arg) =>
                                        //{
                                        //    ClickedFlag1 = 0;
                                        //    if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                        //    {
                                        //        if (arg.Result != null)
                                        //        {
                                        //            CmdPrint.IsEnabled = true;
                                        //            CmdNew.IsEnabled = true;
                                        //            CmdSave.IsEnabled = false;
                                        //            CmdCancel.IsEnabled = false;
                                        //            CmdModify.IsEnabled = false;
                                        //            CmdForward.IsEnabled = true;
                                        //            CmdApprove.IsEnabled = true;
                                        //            SelectedCommand = "Modify";
                                        //            FillIndentList();
                                        //            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        //            mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                        //            {
                                        //                objAnimation.Invoke(RotationType.Backward);
                                        //            };
                                        //            mgbox.Show();
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                        //        mgbox.Show();
                                        //    }

                                        //};
                                        //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                        ClickedFlag1 = 0;
                                };
                                mgBox.Show();
                            }
                            ClickedFlag1 = 0;
                        }
                    }
                    else
                        ClickedFlag1 = 0;
                }
            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteIndentAddedItems != null)
            {
                DeleteIndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                DeleteIndentAddedItems.Clear();
            }
            FormHeaderChange("Approve Indent");
            SelectedCommand = "Cancel";
            CmdPrint.IsEnabled = true;
            CmdNew.IsEnabled = true;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdForward.IsEnabled = true;
            CmdApprove.IsEnabled = true;
            CmdConvertToPR.IsEnabled = true;
            CmdShowStock.IsEnabled = true;

            IndentAddedItems.Clear();
            cmbAddFromStoreName.SelectedItem = null;
            cmbAddToStoreName.SelectedItem = null;
            //dtpAddDueDate.SelectedDate = DateTime.Now;
            dtpAddIndentDate.SelectedDate = DateTime.Now;
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            this.cmbAddFromStoreName.IsEnabled = true;
            this.cmbAddToStoreName.IsEnabled = true;
            this.dtpAddIndentDate.IsEnabled = true;
            this.dtpAddDueDate.IsEnabled = true;
            this.txtRemark.IsEnabled = true;
            FillIndentList();

            objAnimation.Invoke(RotationType.Backward);

        }

        private void CmdShowStock_Click(object sender, RoutedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                ItemStock win_ItemStock = new ItemStock();
                win_ItemStock.IsForCentralPurChaseFromApproveIndent = true;
                win_ItemStock.IndentID = Convert.ToInt64((dgIndentList.SelectedItem as clsIndentMasterVO).ID);
                win_ItemStock.IndentUnitID = Convert.ToInt64((dgIndentList.SelectedItem as clsIndentMasterVO).UnitID);
                win_ItemStock.chkExcel.Visibility = Visibility.Collapsed;
                win_ItemStock.CmdPrint.Visibility = Visibility.Collapsed;
                win_ItemStock.CmdClose.Visibility = Visibility.Visible;
                win_ItemStock.OnCancel_Click += new RoutedEventHandler(Oncancel);
                ObjChildItemStock.Content = win_ItemStock;
                ObjChildItemStock.Width = this.ActualWidth * 0.70;
                ObjChildItemStock.Height = this.ActualHeight * 0.85;
                ObjChildItemStock.Title = "Current Item Stock";
                ObjChildItemStock.HasCloseButton = false;
                ObjChildItemStock.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWD1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Please select Indent from Indent List to view the current Item Stock", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD1.Show();
            }
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = true;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "New";

            dtpAddDueDate.SelectedDate = null;
            if ((DateTime?)dtpAddDueDate.SelectedDate == null)
            {
                dtpAddDueDate.SetValidation("Please Select Expected Delivery Date");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
            }
            else
                dtpAddDueDate.ClearValidationError();
            //cmbAddFromStoreName.SelectedValue = 0;
            //cmbAddToStoreName.SelectedValue = 0;

            //SelectedIndent = new clsIndentMasterVO();
            //this.cmbAddFromStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //this.cmbAddToStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //cmbAddFromStoreName.SelectedItem = null;
            //cmbAddToStoreName.SelectedItem = null;
            this.dtpAddIndentDate.SelectedDate = DateTime.Now;
            //this.chkIsAuthorized.IsChecked = false;
            //this.dtpAuthorizationDate.SelectedDate = DateTime.Now;

            NewStoreFill();

            this.IndentNumber.Text = "";
            this.txtRemark.Text = "";
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            chkFreezIndent.IsChecked = false;
            this.DataContext = new clsIndentDetailVO();
            //if (GRNAddedItems != null)
            IndentAddedItems.Clear();
            objAnimation.Invoke(RotationType.Forward);

        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAddFromStoreName.SelectedItem != null && ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId != 0)
            {
                ItemList Itemswin = new ItemList();
                Itemswin.IndentAddedItems = this.IndentAddedItems.ToList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                //Itemswin.ShowZeroStockBatches = true;
                Itemswin.ShowZeroStockBatches = false;

                Itemswin.ShowBatches = false;
                //Itemswin.cmbStore.SelectedItem = cmbAddToStoreName.SelectedItem;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowQuantity = true;
                Itemswin.StoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                Itemswin.ClinicID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).ClinicId;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select From Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
            }


            //if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
            //{
            //    ItemList Itemswin = new ItemList();
            //    Itemswin.IndentAddedItems = this.IndentAddedItems.ToList();
            //    Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            //    Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //    //Itemswin.ShowZeroStockBatches = true;
            //    Itemswin.ShowZeroStockBatches = false;

            //    Itemswin.ShowBatches = false;
            //    //Itemswin.cmbStore.SelectedItem = cmbAddToStoreName.SelectedItem;
            //    Itemswin.cmbStore.IsEnabled = false;
            //    Itemswin.ShowQuantity = true;
            //    Itemswin.StoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            //    Itemswin.ClinicID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).ClinicId;
            //    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
            //    Itemswin.Show();
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select To Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //}

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Inventory_New/StoreIndentPrint.aspx?IndentId=" + ((clsIndentMasterVO)dgIndentList.SelectedItem).ID + "&UnitID=" + (((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID)), "_blank");
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            //if (chkFreezIndent.IsChecked == true)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //                      new MessageBoxControl.MessageBoxChildWindow("", "Indent is freezed you can't delete the item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWD.Show();
            //    return;
            //}

            if (grdStoreIndent.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsIndentDetailVO objVO = (clsIndentDetailVO)grdStoreIndent.SelectedItem;
                        clsIndentDetailVO obj;
                        if (objVO != null)
                        {
                            obj = IndentAddedItems.Where(z => z.ItemID == objVO.ItemID).FirstOrDefault();
                            IndentAddedItems.Remove(obj);
                            DeleteIndentAddedItems.Add(obj);
                        }
                        grdStoreIndent.Focus();
                        grdStoreIndent.UpdateLayout();
                        grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;
                        //IndentAddedItems.RemoveAt(grdStoreIndent.SelectedIndex);
                        //grdStoreIndent.Focus();
                        //grdStoreIndent.UpdateLayout();
                        //grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;

                    }
                };
                msgWD.Show();
            }
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                //if (GRNAddedItems == null)
                //    GRNAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                foreach (var item in Itemswin.SelectedItems)
                {
                    IEnumerator<clsIndentDetailVO> list = (IEnumerator<clsIndentDetailVO>)IndentAddedItems.GetEnumerator();
                    bool IsExist = false;

                    while (list.MoveNext())
                    {
                        if (item.ID == ((clsIndentDetailVO)list.Current).ItemID)
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist == false)
                    {
                        {
                            clsIndentDetailVO objVO = new clsIndentDetailVO();
                            objVO.ItemID = item.ID;
                            objVO.ItemName = item.ItemName;
                            objVO.RequiredQuantity = item.RequiredQuantity;
                            //objVO.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                            objVO.UOM = item.PUOM;
                            objVO.SUOM = item.SUOM;
                            objVO.SUOMID = item.SUM;
                            objVO.PUOMID = item.PUM;
                            objVO.BaseUOMID = item.BaseUM;
                            objVO.BaseUOM = item.BaseUMString;
                            objVO.SellingUOMID = item.SellingUM;
                            objVO.SellingUOM = item.SellingUMString;


                            //----------------------------//
                            objVO.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };

                            objVO.StockCF = item.PurchaseToBaseCF / item.StockingToBaseCF;
                            objVO.ConversionFactor = item.PurchaseToBaseCF;

                            IndentAddedItems.Add(objVO);
                        }

                        //IndentAddedItems.Add(new clsIndentDetailVO()
                        //{
                        //    ItemID = item.ID,
                        //    ItemName = item.ItemName,
                        //    RequiredQuantity = item.RequiredQuantity,
                        //    ConversionFactor = Convert.ToSingle(item.ConversionFactor),
                        //    //UOMList = UOMLIstNew, 
                        //    //SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.PUM)), 
                        //    UOM = item.PUOM,
                        //    SUOM = item.SUOM,
                        //    SUOMID = item.SUM,
                        //    //SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM },
                        //    SelectedUOM = new MasterListItem { ID = 0, Description = "--Select--" },
                        //    //MRP = (float)item.MRP,
                        //    //MainMRP = (float)item.MRP,
                        //    PUOMID = item.PUM,
                        //    //SUOMID = item.SUM,
                        //    BaseUOMID = item.BaseUM,
                        //    BaseUOM = item.BaseUMString,
                        //    SellingUOMID = item.SellingUM,
                        //    SellingUOM = item.SellingUMString
                        //});
                    }
                }

                grdStoreIndent.Focus();
                grdStoreIndent.UpdateLayout();

                grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;
            }
        }

        private void cmdCancelIndent_Click(object sender, RoutedEventArgs e)
        {

            if (((clsIndentMasterVO)dgIndentList.SelectedItem) != null)
            {
                if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Rejected || ((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Cancelled || ((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                {
                    string strMsg = string.Empty;
                    if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Rejected) strMsg = "Cannot reject the Indent, The Indent is already Rejected";
                    else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Cancelled) strMsg = "Cannot reject the Indent, The Indent is already Cancelled";
                    else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.ForceFullyCompleted) strMsg = "Cannot reject the Indent, The Indent is Forcefully Closed";
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                    new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                    }
                }
                else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Cannot reject the Indent, The Indent is Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to reject the Indent?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            POCancellationWindow Win = new POCancellationWindow();
                            Win.Title = "Indent Rejection";
                            Win.tblkCAncellationReason.Text = "Indent Rejection";
                            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                            Win.Show();
                        }
                    };
                    msgWD.Show();
                }
            }

        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
            POCancellationWindow Win = (POCancellationWindow)sender;

            clsUpdateRemarkForIndentCancellationBizActionVO objBizActionVO = new clsUpdateRemarkForIndentCancellationBizActionVO();
            objBizActionVO.IsRejectIndent = true;
            objBizActionVO.IndentMaster = new clsIndentMasterVO();
            objBizActionVO.IndentMaster.IndentStatus = InventoryIndentStatus.Rejected;
            objBizActionVO.IndentMaster.ID = SelectedIndent.ID;
            objBizActionVO.IndentMaster.UnitID = SelectedIndent.UnitID;
            objBizActionVO.CancellationRemark = Win.txtAppReason.Text;
            try
            {

                objBizActionVO.CancellationRemark = ((POCancellationWindow)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Rejected successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                        DataListPageSize = 5;
                        FillIndentList();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            FillIndentList();
        }
        #endregion

        #region DataGrid CellEditEnded Events
        void grdStoreIndent_CellEditEnded(object Sender, DataGridCellEditEndedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                clsIndentDetailVO obj = new clsIndentDetailVO();
                obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;

                if (e.Column.Header.ToString().Equals("Indent Quantity")) // for Quantity
                {
                    obj.SingleQuantity = Convert.ToSingle(System.Math.Round(obj.SingleQuantity, 1));
                    if (((int)obj.SingleQuantity).ToString().Length > 5)
                    {
                        obj.SingleQuantity = 1;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return;
                    }

                    if (obj.SelectedUOM != null && obj.SelectedUOM.ID == obj.BaseUOMID && (obj.SingleQuantity % 1) != 0)
                    {
                        obj.SingleQuantity = 0;
                        string msgText = "Indent Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    if (obj.SingleQuantity < 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        //obj.SingleQuantity = 1;
                        return;
                    }

                    if (obj.SelectedUOM != null && obj.SelectedUOM.ID > 0)
                    {
                        obj.StockingQuantity = Convert.ToDouble(obj.SingleQuantity) * obj.StockCF;  // StockCF
                        obj.RequiredQuantity = Convert.ToSingle(obj.SingleQuantity) * obj.ConversionFactor;  //BaseCF
                    }
                    else
                    {
                        CalculateConversionFactorCentral(obj.SelectedUOM.ID, obj.SUOMID);
                    }
                }

                #region Coomented By Ashish Z. for UOM HyperLink
                //clsIndentDetailVO obj = new clsIndentDetailVO();
                //obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;
                //if (e.Column.DisplayIndex == 1) // for Quantity
                //{
                //    if (obj.SingleQuantity == 0)   //if (obj.RequiredQuantity == 0)
                //    {
                //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        mgbx.Show();
                //        obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

                //        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
                //        {
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor);
                //        }
                //        else
                //        {
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
                //        }

                //        return;
                //    }

                //    if (obj.SingleQuantity < 0)  //if (obj.RequiredQuantity < 0)
                //    {
                //        //MessageBoxControl.MessageBoxChildWindow mgbx = null;

                //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        mgbx.Show();
                //        obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

                //        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
                //        {
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor);
                //        }
                //        else
                //        {
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
                //        }

                //        return;
                //    }

                //    if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
                //    {
                //        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor); //BaseConversionFactor
                //    }
                //    else
                //    {
                //        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
                //    }


                //    if (SelectedCommand == "View")
                //    {
                //        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                //        {
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF;
                //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor;
                //        }
                //        else
                //        {
                //            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                //            {
                //                CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                //        {
                //            CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                #endregion
            }

            #region Previos Code
            //clsIndentDetailVO obj = new clsIndentDetailVO();
            //obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;

            ////if (Convert.ToDouble(grdStoreIndent.Columns[1].GetCellContent(obj.RequiredQuantity)) == 0)
            ////{
            ////    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            ////    mgbx.Show();
            ////    obj.RequiredQuantity = 1;
            ////    return;
            ////}

            //if (obj.RequiredQuantity == 0)
            //{
            //    //MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    obj.RequiredQuantity = 1;
            //    return;
            //}

            //if (obj.RequiredQuantity < 0)
            //{
            //    //MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    obj.RequiredQuantity = 1;
            //    return;
            //}
            #endregion

        }
        #endregion

        #region Private Methods
        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveIndent();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void SaveIndent()
        {
            clsAddIndentBizActionVO BizAction = new clsAddIndentBizActionVO();

            BizAction.objIndent = new clsIndentMasterVO();
            BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
            BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
            BizAction.objIndent.IndentNumber = null;
            BizAction.objIndent.TransactionMovementID = 0;
            BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
            BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);

            BizAction.objIndent.IndentCreatedByID = SelectedIndent.IndentCreatedByID;//((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;

            if (SelectedCommand == "IndentToPR")  //For Convert To PR
            {
                BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;
                BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate; //DateTime.Now; // dtpAuthorizationDate.SelectedDate;
            }

            BizAction.objIndent.Remark = txtRemark.Text;
            BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
            BizAction.objIndent.Status = true;


            BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
            BizAction.objIndent.IsForwarded = false;
            BizAction.objIndent.IsApproved = false;
            //By anjali..............................................
            //BizAction.objIndent.IsIndent = 1;
            //BizAction.objIndent.IsIndent = true;
            //.............................................................
            //BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;
            BizAction.objIndent.IsChangeAndApprove = false;


            BizAction.objIndent.ConvertToPRID = Convert.ToInt64(SelectedIndent.ID); //Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);

            BizAction.objIndent.InventoryIndentType = InventoryIndentType.PurchaseRequisition;
            BizAction.IsConvertToPR = true;
            BizAction.objIndent.IsApproved = true;
            BizAction.objIndent.IsIndent = 0;

            BizAction.objIndentDetailList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
            BizAction.objIndentDetailList.Select(indentItems => { indentItems.IndentUnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId; return indentItems; }).ToList();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag1 = 0;
                if (arg.Error == null && ((clsAddIndentBizActionVO)arg.Result).objIndent != null)
                {
                    if (arg.Result != null)
                    {
                        CmdPrint.IsEnabled = true;
                        //CmdNew.IsEnabled = true;
                        CmdSave.IsEnabled = false;
                        CmdCancel.IsEnabled = false;
                        CmdForward.IsEnabled = true;
                        CmdModify.IsEnabled = false;
                        CmdApprove.IsEnabled = true;
                        CmdForward.IsEnabled = true;
                        //CmdChangeApprove.IsEnabled = true;
                        CmdShowStock.IsEnabled = true;
                        CmdConvertToPR.IsEnabled = true;

                        SelectedCommand = "Save";
                        cmbAddFromStoreName.SelectedValue = (long)0;
                        cmbAddToStoreName.SelectedValue = (long)0;
                        FillIndentList();
                        objAnimation.Invoke(RotationType.Backward);
                        FormHeaderChange("Approve Indent");

                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Saved Successfully With PR Number " + ((clsAddIndentBizActionVO)arg.Result).objIndent.IndentNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();


                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while saving indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void ViewIndent()
        {
            if (DeleteIndentAddedItems != null)
            {
                DeleteIndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                DeleteIndentAddedItems.Clear();
            }

            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "View";
            cmbAddFromStoreName.IsEnabled = false;
            cmbAddToStoreName.IsEnabled = false;
            lnkAddItems.IsEnabled = true;
            if (SelectedIndent.IsFreezed)
                chkFreezIndent.IsEnabled = false;
            else
                chkFreezIndent.IsEnabled = true;

            if (SelectedIndent.IsIndent == 0)
                CmdModify.IsEnabled = false;
            else if (SelectedIndent.IsIndent == 1)
                CmdModify.IsEnabled = true;
            CmdShowStock.IsEnabled = false;
            CmdConvertToPR.IsEnabled = false;
            dtpAddIndentDate.IsEnabled = false;

            objAnimation.Invoke(RotationType.Forward);

            //if (SelectedIndent.IsFreezed)
            //{
            //    CmdModify.IsEnabled = true;
            //    chkFreezIndent.IsEnabled = false;
            //    lnkAddItems.IsEnabled = false;
            //}
            //else
            //{
            //    chkFreezIndent.IsEnabled = true;
            //    grdStoreIndent.IsEnabled = true;
            //    CmdModify.IsEnabled = true;
            //    lnkAddItems.IsEnabled = true;
            //}
        }

        private void UpdateForChangeAndApprove()
        {
            clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
            BizAction.IsForChangeAndApproveIndent = true;
            BizAction.objIndent = new clsIndentMasterVO();
            BizAction.objIndent.ID = SelectedIndent.ID;
            BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
            BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
            BizAction.objIndent.TransactionMovementID = 0;
            BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
            BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
            BizAction.objIndent.IndentCreatedByID = SelectedIndent.IndentCreatedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;// chkIsAuthorized.IsChecked;
            BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate; //DateTime.Now;// dtpAuthorizationDate.SelectedDate;
            BizAction.objIndent.Remark = SelectedIndent.Remark;//txtRemark.Text;
            BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
            BizAction.objIndent.Status = true;
            if (SelectedIndent.IsFreezed)
                BizAction.objIndent.IsFreezed = SelectedIndent.IsFreezed;//(bool)chkFreezIndent.IsChecked;
            else
                BizAction.objIndent.IsFreezed = Convert.ToBoolean(chkFreezIndent.IsChecked);
            BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
            BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
            BizAction.objIndent.IsChangeAndApprove = true; //}
            BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
            BizAction.objIndent.UnitID = SelectedIndent.UnitID;
            BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
            BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
            BizAction.objIndent.IsIndent = 1;
            BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag1 = 0;
                if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                {
                    if (arg.Result != null)
                    {
                        CmdPrint.IsEnabled = true;
                        //CmdNew.IsEnabled = true;
                        CmdSave.IsEnabled = false;
                        CmdCancel.IsEnabled = false;
                        CmdModify.IsEnabled = false;
                        CmdForward.IsEnabled = true;
                        CmdApprove.IsEnabled = true;
                        CmdShowStock.IsEnabled = true;
                        CmdConvertToPR.IsEnabled = true;
                        //CmdChangeApprove.IsEnabled = true;

                        chkFreezIndent.IsEnabled = true;
                        grdStoreIndent.IsEnabled = true;
                        lnkAddItems.IsEnabled = true;
                        SelectedCommand = "Modify";
                        FillIndentList();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        public void ConvertToPR()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                BizAction.IndentID = Convert.ToInt64(SelectedIndent.ID);
                BizAction.UnitID = Convert.ToInt64(SelectedIndent.UnitID);
                BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //acTextBox = new TextBox();
                            dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                            this.DataContext = new clsIndentDetailVO();
                            IndentAddedItems.Clear();

                            foreach (var item in SelectedIndent.IndentDetailsList)
                            {
                                if (UOMLIstNew.Count > 0)
                                {
                                    item.UOMList = UOMLIstNew;
                                    item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                    item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;

                                    item.SUOM = UOMLIstNew.Single(u => u.ID.Equals(item.SUOMID)).Description;
                                }
                                IndentAddedItems.Add(item);
                            }

                            if (cmbAddFromStoreName.ItemsSource != null && cmbAddToStoreName.ItemsSource != null)
                            {
                                var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                              where r.StoreId == this.SelectedIndent.FromStoreID  //r.ClinicId == this.SelectedIndent.IndentUnitID &&
                                              //&& r.ClinicId == this.SelectedIndent.UnitID
                                              select r;

                                foreach (clsStoreVO item in results)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                }

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddToStoreName.SelectedItem = item;
                                }
                            }
                            this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                            this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                            this.IndentNumber.Text = SelectedIndent.IndentNumber;
                            this.txtRemark.Text = SelectedIndent.Remark;
                            this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                            ViewIndentToPR();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }
            finally
            {

            }
        }

        public void ViewIndentToPR()
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = true;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            //CmdChangeApprove.IsEnabled = false;
            CmdShowStock.IsEnabled = false;
            SelectedCommand = "IndentToPR";
            objAnimation.Invoke(RotationType.Forward);

            chkFreezIndent.IsEnabled = false;
            grdStoreIndent.IsEnabled = false;
            CmdModify.IsEnabled = false;
            lnkAddItems.IsEnabled = false;
            dtpAddIndentDate.IsEnabled = false;
            cmbAddFromStoreName.IsEnabled = false;
            cmbAddToStoreName.IsEnabled = false;

            CmdConvertToPR.IsEnabled = false;
        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsUserRightsVO objUser;
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                        if (objUser.IsCentarlPurchase)
                            this.IsCentralPurchase = true;
                        else
                            this.IsCentralPurchase = false;

                        if (objUser.IsDirectIndent)
                        {
                            this.IsDirectIndent = true;
                        }
                        else
                        {
                            IsDirectIndent = false;
                        }

                        if (objUser.IsInterClinicIndent)
                        {
                            this.IsInterClinicIndent = true;
                        }
                        else
                        {
                            this.IsInterClinicIndent = false;
                        }
                        
                    }
                    FillIndentList();
                    ButtonVisibility();
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillIndentList()
        {
            try
            {
                indicator.Show();
                dgIndentList.ItemsSource = null;
                dgIndentDetailList.ItemsSource = null;

                clsGetIndenListBizActionVO BizAction = new clsGetIndenListBizActionVO();
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                    {
                        dtpFromDate.SetValidation("From Date should be less than To Date");
                        dtpFromDate.RaiseValidationError();
                        dtpFromDate.Focus();
                        indicator.Close();
                        return;
                    }
                    else
                    {
                        dtpFromDate.ClearValidationError();
                    }

                }
                if (this.dtpFromDate.SelectedDate != null && this.dtpToDate.SelectedDate != null)
                {
                    BizAction.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                    BizAction.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                }
                if (this.cmbFromStoreName.SelectedItem != null)
                    BizAction.FromStoreID = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
                if (this.cmbToStoreName.SelectedItem != null)
                    BizAction.ToStoreID = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;

                if (!string.IsNullOrEmpty(txtIndentNumber.Text))
                    BizAction.IndentNO = txtIndentNumber.Text;
                //Set Paging Variables
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                if (this.IsCentralPurchase)
                {
                    if ((bool)rdbAll.IsChecked)
                    {
                        BizAction.CheckStatusType = false;
                        BizAction.isApproved = true;
                    }
                    else if ((bool)rdbPending.IsChecked)
                    {
                        BizAction.CheckStatusType = true;
                        BizAction.IndentStatus = 1;
                    }
                    else if ((bool)rdbCompleted.IsChecked)
                    {
                        BizAction.CheckStatusType = true;
                        BizAction.IndentStatus = 4;
                    }
                    else if ((bool)rdbApproved.IsChecked)
                    {
                        BizAction.isApproved = true;
                    }
                    else if ((bool)rdbForwarded.IsChecked)
                    {
                        BizAction.isFrowrded = true;
                    }
                    else if ((bool)rdbCancelled.IsChecked)
                    {
                        BizAction.isCancelled = true;
                    }
                    else
                    {
                        BizAction.isApproved = true;
                    }
                    BizAction.isApproved = true;
                    BizAction.isIndent = 2;   // for getting the Indents and PR's for Central Purchase.
                    //BizAction.UnitID = 0;   //for getting all units Indent or PR for Central Purchase.
                }
                else
                {
                    if ((bool)rdbAll.IsChecked)
                    {
                        BizAction.CheckStatusType = false;
                    }
                    else if ((bool)rdbPending.IsChecked)
                    {
                        BizAction.CheckStatusType = true;
                        //BizAction.IndentStatus = 3;
                        BizAction.IndentStatus = 1;
                    }
                    else if ((bool)rdbCompleted.IsChecked)
                    {
                        BizAction.CheckStatusType = true;
                        BizAction.IndentStatus = 4;
                    }
                    else if ((bool)rdbApproved.IsChecked)
                    {
                        BizAction.isApproved = true;
                    }
                    else if ((bool)rdbForwarded.IsChecked)
                    {
                        BizAction.isFrowrded = true;
                    }
                    else if ((bool)rdbCancelled.IsChecked)
                    {
                        BizAction.isCancelled = true;
                    }
                    BizAction.isIndent = 1;
                }
                BizAction.IsFreezed = true;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    BizAction.UnitID = 0;
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                //***//-----
                if (txtMrno.Text.Trim() != "")
                    BizAction.MRNo = txtMrno.Text.Trim();
                else
                    BizAction.MRNo = "";


                if (txtName.Text.Trim() != "")
                    BizAction.PatientName = txtName.Text.Trim();
                else
                    BizAction.PatientName = "";
                //-------


                //By Anjali.............................

                //BizAction.isIndent = true;
                //.....................................
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    indicator.Close();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //dgIndentList.ItemsSource = ((clsGetIndenListBizActionVO)arg.Result).IndentList;

                            clsGetIndenListBizActionVO result = arg.Result as clsGetIndenListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.IndentList != null)
                            {
                                DataList.Clear();
                                //if (IsCentralPurchase)
                                //{
                                //    foreach (var item in result.IndentList)
                                //    {
                                //        if (item.IsApproved)
                                //        {
                                //            DataList.Add(item);
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                foreach (var item in result.IndentList)
                                {
                                    DataList.Add(item);
                                }
                                //}

                                dgIndentList.ItemsSource = null;
                                dgIndentList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;
                            }
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                        }
                    }
                    ButtonVisibility();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception ex)
            {
                indicator.Close();
            }
        }

        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
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
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    UserStores.Clear();

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {    //item.StoreDetails       
                            UserStores = item.UserUnitStore;

                            //if (item.IsDefault == true)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }
                    //UserStores.Insert(0, Default);
                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }

                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);

                    //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                    // StoreListForClinicUser.Insert(0, Default);
                    // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                    if (IsInterClinicIndent == true) //***//
                    {
                        cmbAddToStoreName.ItemsSource = result1.ToList(); 
                        cmbAddToStoreName.SelectedItem = result1.ToList()[0];

                        cmbToStoreName.ItemsSource = result1.ToList();
                        cmbToStoreName.SelectedItem = result1.ToList()[0];
                    }
                    else
                    {
                        cmbAddToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                        cmbAddToStoreName.SelectedItem = StoreListForClinic.ToList()[0];
                        cmbToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                        cmbToStoreName.SelectedItem = StoreListForClinic.ToList()[0];
                    }              
              
                    //if (result1.ToList().Count > 0)
                    //{
                    //    cmbToStoreName.SelectedItem = result1.ToList()[0];
                    //    cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    //}
                    //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                    //if (StoreListForClinicUser.Count > 0)
                    //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];

                    #region Commented by Ashish Z. for show all stores in From Combo..
                    //cmbFromStoreName.ItemsSource = result1.ToList();
                    //cmbAddFromStoreName.ItemsSource = result1.ToList();
                    //if (result1.ToList().Count > 0)
                    //{
                    //    cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                    //    cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                    //}
                    #endregion

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }

                    else
                    {
                        cmbAddFromStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList();//StoreListForClinic;
                        cmbFromStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList(); //StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbFromStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }

                }
            };

            client.CloseAsync();


            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.Parent = new KeyValue();
            //BizAction.Parent.Key = "1";
            //BizAction.Parent.Value = "Status";

            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            //client1.ProcessCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        //cmbFromStoreName.ItemsSource = null;
            //        BizAction.MasterList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
            //        MasterListItem Default = new MasterListItem { ID = 0, Description = "--Select--" };
            //        BizAction.MasterList.Insert(0, Default);
            //        //cmbFromStoreName.ItemsSource = BizAction.MasterList;
            //        //cmbFromStoreName.SelectedItem = Default;

            //        cmbToStoreName.ItemsSource = BizAction.MasterList;
            //        cmbToStoreName.SelectedItem = Default;

            //        //cmbAddFromStoreName.ItemsSource = BizAction.MasterList;
            //        //cmbAddFromStoreName.SelectedItem = Default;

            //        cmbAddToStoreName.ItemsSource = BizAction.MasterList;
            //        cmbAddToStoreName.SelectedItem = Default;
            //    }
            //};
            //client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client1.CloseAsync();
        }

        private void FillUOM()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        UOMLIstNew = new List<MasterListItem>();
                        UOMLIstNew.Add(new MasterListItem(0, "- Select -"));
                        UOMLIstNew.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
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

        private void ButtonVisibility()
        {
            if (this.IsCentralPurchase)
            {
                //CmdConvertToPR.IsEnabled = true;
                //CmdForward.IsEnabled = true;
                //CmdApprove.IsEnabled = false;
                CmdConvertToPR.Visibility = Visibility.Visible;
                CmdForward.Visibility = Visibility.Visible;
                CmdApprove.Visibility = Visibility.Collapsed;
                CmdShowStock.Visibility = Visibility.Visible;
                dgIndentList.Columns[15].Visibility = Visibility.Visible;   //PR
                dgIndentList.Columns[16].Visibility = Visibility.Collapsed;  //Reject Indent
            }
            else
            {
                //CmdConvertToPR.IsEnabled = false;
                //CmdForward.IsEnabled = false;
                //CmdApprove.IsEnabled = true;
                CmdConvertToPR.Visibility = Visibility.Collapsed;
                CmdForward.Visibility = Visibility.Collapsed;
                CmdApprove.Visibility = Visibility.Visible;
                CmdShowStock.Visibility = Visibility.Collapsed;
                dgIndentList.Columns[15].Visibility = Visibility.Collapsed;  //PR
                dgIndentList.Columns[16].Visibility = Visibility.Visible; //Reject Indent
            }
        }

        private void NewStoreFill()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
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
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {
                            UserStores = item.UserUnitStore;
                            //if (item.IsDefault == true)
                            //{
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }

                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }
                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);
                    List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 

                    cmbAddToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                    cmbAddToStoreName.SelectedItem = StoreListForClinic.ToList()[0];
                    //cmbToStoreName.ItemsSource = StoreListForClinic.ToList();  //result1.ToList();
                    //if (result1.ToList().Count > 0)
                    //{
                    //    //cmbToStoreName.SelectedItem = result1.ToList()[0];
                    //    cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    //}

                    if (StoreListForClinicUser.Count > 0)
                        cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        //cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            //cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }
                    else
                    {
                        cmbAddFromStoreName.ItemsSource = BizActionObj.ToStoreList.ToList();//StoreListForClinic;
                        //cmbFromStoreName.ItemsSource = StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = BizActionObj.ToStoreList[0];
                            //cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                        }
                    }
                }
            };

            client.CloseAsync();
        }

        private void Oncancel(object sender, RoutedEventArgs e)
        {
            ObjChildItemStock.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            //ObjChildItemStock = null;
        }

        private void ObjChildOTSchedule_Closed(object sender, EventArgs e)
        {
            ((ChildWindow)sender).DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            //if ((bool)((ChildWindow)sender).DialogResult)
            //{
            //}
        }

        private void FormHeaderChange(string strHeader)
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = strHeader;
        }
        #endregion

        #region SelectionChanged/ KeyUp / Other Events
        private void dgIndentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                if (SelectedIndent.IsApproved == true)
                {
                    CmdApprove.IsEnabled = false;
                }
                else if (SelectedIndent.IsApproved == false && !this.IsCentralPurchase)
                    CmdApprove.IsEnabled = true;
                if (SelectedIndent.IsIndent == 0)
                {
                    CmdConvertToPR.IsEnabled = false;
                    CmdForward.IsEnabled = false;
                }
                else if (this.IsCentralPurchase && SelectedIndent.IsIndent == 1)
                {
                    if (SelectedIndent.IsConvertToPR || SelectedIndent.IsForwarded)
                    {
                        CmdConvertToPR.IsEnabled = false;
                        CmdForward.IsEnabled = false;
                    }
                    else
                    {
                        CmdConvertToPR.IsEnabled = true;
                        CmdForward.IsEnabled = true;
                    }
                }

                clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                BizAction.IndentDetailList = new List<clsIndentDetailVO>();
                BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)


                //    BizAction.UnitID = 0;
                //else
                //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            cmbAddFromStoreName.SelectedValue = ((clsIndentMasterVO)dgIndentList.SelectedItem).FromStoreName;
                            cmbAddToStoreName.SelectedValue = ((clsIndentMasterVO)dgIndentList.SelectedItem).ToStoreName;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }

        private void cmbAddToStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IndentAddedItems != null)
                {
                    if (IndentAddedItems.Count > 0)
                    {
                        if (SelectedIndent != null)
                        {
                            if (!SelectedIndent.IsFreezed)
                                if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
                                {

                                    if (SelectedIndent.ToStoreID != ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                                        IndentAddedItems.Clear();
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;
                ////CalculateOpeningBalanceSummary();

                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    if (cmbConversions.SelectedItem != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList.Count > 0)
                        CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                }
                else
                {

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = 0;
                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = 0;

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).MRP = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainMRP;
                    //(((clsIndentDetailVO)grdStoreIndent.SelectedItem).Rate = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainRate;
                }
            }
        }

        private void txtIndentNumber_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillIndentList();
            }
        }

        //private void grdStoreIndent_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    if (grdStoreIndent.ItemsSource != null)
        //    {
        //        if (((clsIndentDetailVO)e.Row.DataContext).IsItemBlock)
        //            e.Row.IsEnabled = false;
        //        else
        //            e.Row.IsEnabled = true;
        //    }
        //}

        //private void dgIndentList_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    if (this.IsCentralPurchase)
        //        dgIndentList.Columns[13].Visibility = Visibility.Collapsed;
        //    else
        //        dgIndentList.Columns[13].Visibility = Visibility.Visible;

        //}
        #endregion

        #region Validation Methods
        private bool ValidationForConvertToPR()
        {
            bool reasult = true;
            string msg = string.Empty;
            if (SelectedIndent.IsAgainstPatient == true || SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected || SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled || SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted || SelectedIndent.IndentStatus == InventoryIndentStatus.BulkClose)
            {
                string strMsg = string.Empty;
                if (SelectedIndent.IsAgainstPatient == true) strMsg = "Indent is against Patient, you cannot convert to PR";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected) strMsg = "Indent is already Rejected, You cannot convert to PR ";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled) strMsg = "Indent is already Cancelled, You cannot convert to PR";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted) strMsg = "Indent is Forcefully Closed, You cannot convert to PR";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.BulkClose) strMsg = "Indent is Bulk Closed, You cannot convert to PR";

                if (!string.IsNullOrEmpty(strMsg))
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
                reasult = false;
            }
            else if (dgIndentList.SelectedItem != null && (dgIndentList.SelectedItem as clsIndentMasterVO).IsForwarded == true)
            {
                msg = "Selected Indent is already Forwarded, You cannot convert to PR";
                reasult = false;
            }
            else if (dgIndentList.SelectedItem != null && (dgIndentList.SelectedItem as clsIndentMasterVO).IsConvertToPR == true)
            {
                msg = "Selected Indent is already converted to Purchase Requisition";
                reasult = false;
            }
            else if (dgIndentList.SelectedItem != null && (dgIndentList.SelectedItem as clsIndentMasterVO).IsApproved == false)
            {
                msg = "Selected Indent is not Approved, you cannot convert it to Purchase Requisition";
                reasult = false;
            }
            else if (dgIndentList.SelectedItem != null && (dgIndentList.SelectedItem as clsIndentMasterVO).IsFreezed == false)
            {
                msg = "Selected Indent is not freezed, you cannot convert it to Purchase Requisition";
                reasult = false;
            }
            else if (!string.IsNullOrEmpty((dgIndentList.SelectedItem as clsIndentMasterVO).IssueNowithDate))
            {
                msg = "Selected Indent is already Issued, you cannot convert it to Purchase Requisition";
                reasult = false;
            }
            else if (dgIndentList.SelectedItem == null)
            {
                msg = "Please Select Indent to convert into Purchase Requisition";
                reasult = false;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            return reasult;
        }

        private bool ValidationForForwardIndent()
        {
            bool reasult = true;
            string msg = string.Empty;
            if (dgIndentList.SelectedItem == null)
            {
                msg = "No Indent is Selected \n Please select an Indent";
                reasult = false;
            }
            else if (!SelectedIndent.IsApproved)
            {
                msg = "Selected Indent is not approved. \n You cannot forward it";
                reasult = false;
            }
            else if (!SelectedIndent.IsFreezed)
            {
                msg = "Indent is not Freezed \n Please Freeze Indent";
                reasult = false;
            }
            else if (SelectedIndent.IsConvertToPR)
            {
                msg = "Selected Indent is already converted to PR, You cannot forward it";
                reasult = false;
            }
            else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled)
            {
                msg = "Selected Indent is cancelled, You cannot forward it";
                reasult = false;
            }
            else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected)
            {
                msg = "Selected Indent is Rejected, You cannot forward it";
                reasult = false;
            }
            else if (SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
            {
                msg = "Selected Indent is Forcefully Closed, You cannot forward it";
                reasult = false;
            }
            else if (SelectedIndent.IsForwarded)
            {
                msg = "Selected Indent is already Forwarded, You cannot forward it";
                reasult = false;
            }
            else if (!string.IsNullOrEmpty(SelectedIndent.IssueNowithDate))
            {
                msg = "Selected Indent is already Issued, You cannot forward it";
                reasult = false;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            return reasult;
        }

        private bool Validate()
        {

            // Changes done by Harish 17 Apr

            if (cmbAddFromStoreName.SelectedItem == null || ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            //else if ((long)cmbAddFromStoreName.SelectedValue == (long)0)
            //{ 
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //}
            // Changes done by Harish 17 Apr
            if (cmbAddToStoreName.SelectedItem == null || ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Same From  & To Store!Please select different stores", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            //else if ((long)cmbAddToStoreName.SelectedValue == (long)0)
            //{
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //} 
            if ((DateTime?)dtpAddDueDate.SelectedDate == null)
            {
                dtpAddDueDate.SetValidation("Please Select Expected Delivery Date");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Due Date can not be Empty. Please Select a Due Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();
            if ((DateTime?)dtpAddDueDate.SelectedDate < DateTime.Now.Date)
            {
                dtpAddDueDate.SetValidation("Expected Delivery Date can not be Less Than Today's Date.");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Delivery Date can not be Less Than Today's Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();

            if ((DateTime?)dtpAddDueDate.SelectedDate > DateTime.Now.Date.AddMonths(6))
            {
                dtpAddDueDate.SetValidation("Expected delivery date should be within 6 months from Today's Date.");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Delivery Date can not be Less Than Today's Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();

            if ((DateTime?)dtpAddIndentDate.SelectedDate == null)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Date can not be Empty. Please Select a Indent Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            if (IndentAddedItems.Count == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Items List can not be Empty. Please Select Indent Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            //Added By Pallavi
            List<clsIndentDetailVO> objList = IndentAddedItems.ToList<clsIndentDetailVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    float fltOne = 1;
                    float fltZero = 0;
                    float Infinity = fltOne / fltZero;
                    float NaN = fltZero / fltZero;

                    if (item.StockCF <= 0 || item.StockCF == Infinity || item.StockCF == NaN)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        return false;
                    }
                    else if (item.ConversionFactor <= 0 || item.ConversionFactor == Infinity || item.ConversionFactor == NaN)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        return false;
                    }

                    if (item.SingleQuantity == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //item.SingleQuantity = 1;
                        return false;
                    }
                    if (item.SingleQuantity < 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //item.SingleQuantity = 0;
                        return false;
                    }
                    if (item.SelectedUOM.ID == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select UOM for '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        return false;
                    }

                    if (item.SelectedUOM != null && item.SelectedUOM.ID == item.BaseUOMID && (item.SingleQuantity % 1) != 0)
                    {
                        item.SingleQuantity = 0;
                        string msgText = "Indent Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return false;
                    }
                }

            }
            return true;

        }

        private bool ModifyValidation()
        {
            bool result = true;

            if (SelectedIndent.IsIndent == 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "You cannot modify PR", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
                result = false;
            }
            else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected || SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled || SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
            {
                string strMsg = string.Empty;
                if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected) strMsg = "Indent is already Rejected, You cannot Modify it";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled) strMsg = "Indent is already Cancelled, You cannot Modify it";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted) strMsg = "Indent is forcefully closed, You cannot Modify it";

                if (!string.IsNullOrEmpty(strMsg))
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
                result = false;
            }
            else if (!this.IsCentralPurchase && SelectedIndent.IsApproved)
            {
                string strMSG = string.Empty;
                strMSG = "Indent is Approved, You cannot Modify Indent";

                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMSG, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
                result = false;
            }
            else if (SelectedIndent.IsForwarded || SelectedIndent.IsConvertToPR)
            {
                string strMSG = string.Empty;
                if (SelectedIndent.IsForwarded) strMSG = "Indent is Forwarded, You cannot Modify Indent";
                else strMSG = "Indent is Converted To PR, You cannot Modify Indent";

                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMSG, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
                result = false;
            }
            else if (!string.IsNullOrEmpty(SelectedIndent.IssueNowithDate))
            {
                string strMSG = string.Empty;
                strMSG = "Indent is already Issued, You cannot Modify Indent";

                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMSG, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
                result = false;
            }
            return result;
        }
        #endregion

        #region Conversion Factor
        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (grdStoreIndent.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainMRP;
                    //objConversionVO.MainRate = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).MRP = objConversionVO.MRP;

                    if (objConversionVO.BaseQuantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = objConversionVO.BaseQuantity; //objConversionVO.Quantity;
                    if (objConversionVO.Quantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = objConversionVO.Quantity;

                    if (objConversionVO.BaseRate > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    if (objConversionVO.BaseMRP > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    if (objConversionVO.ConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF = objConversionVO.ConversionFactor;
                    if (objConversionVO.BaseConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = objConversionVO.BaseConversionFactor; //objConversionVO.ConversionFactor;


                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList == null || ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);
                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());

                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        //UOMConversionLIst.Add(new MasterListItem(0, "-- Select --"));
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (grdStoreIndent.SelectedItem != null)
                        {
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }


        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID), ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity > 0)
            {
                if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID == ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseUOMID && (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity % 1) != 0)
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                    string msgText = "Quantity Cannot Be In Fraction for Base UOM";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void cmdAddPatient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkAgainstPatient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);

            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = txtName.Text.ToTitleCase();
        }

        int selectionStart = 0;
        int selectionLength = 0;
        string textBefore = null;

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}
