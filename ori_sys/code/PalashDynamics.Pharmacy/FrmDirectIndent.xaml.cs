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
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using System.ComponentModel;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Pharmacy
{
    public partial class FrmDirectIndent : UserControl
    {
        public PagedSortableCollectionView<clsIssueListVO> IssueDataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return IssueDataList.PageSize;
            }
            set
            {
                if (value == IssueDataList.PageSize) return;
                IssueDataList.PageSize = value;                
            }
        }

        private clsIssueItemDetailsVO _SelectedRowItem = new clsIssueItemDetailsVO();

        public clsIssueItemDetailsVO SelectedRowItem
        {
            get
            {
                return _SelectedRowItem;
            }
            set
            {
                _SelectedRowItem = value;
            }
        }

        public int SelectedRowItemIndex { get; set; }

        WaitIndicator Indicatior;
        List<clsStoreVO> StoreList;
        bool flagResetControls = false;
        bool IsInterClinicIndent = false; 
        public FrmDirectIndent()
        {
            Indicatior = new WaitIndicator();
            InitializeComponent();
            FillStore();   
            IssueDataList = new PagedSortableCollectionView<clsIssueListVO>();
            DataListPageSize = 15;
            dgdpIssueList.PageSize = DataListPageSize;
            dgdpIssueList.Source = IssueDataList;
            this.Loaded += new RoutedEventHandler(DirectIndent_Loaded); 
        }

        void DirectIndent_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsIssueListVO();
            dpIssueFromDate.SelectedDate = DateTime.Now;
            dpIssueToDate.SelectedDate = DateTime.Now;          
            FillIssueList();
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearchIssue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFrontPannelSearch())
            {
                FillIssueList();
                dgdpIssueList.PageIndex = 0;
            }
        }

        private bool CheckFrontPannelSearch()
        {
            bool result = true;

            DateTime? _FromDate = dpIssueFromDate.SelectedDate;
            DateTime? _ToDate = dpIssueToDate.SelectedDate;

            if (_FromDate > _ToDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter From Date less than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
            }
            else
            {
                dpIssueFromDate.ClearValidationError();
                dpIssueToDate.ClearValidationError();
                result = true;
            }
            return result;
        }

        private void FillIssueList()
        {
            Indicatior.Show();
            try
            {
                clsGetIssueListBizActionVO BizAction = new clsGetIssueListBizActionVO();
                BizAction.IssueList = new List<clsIssueListVO>();
                BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                BizAction.flagReceiveIssue = false;
                BizAction.IndentCriteria = 0;
                if (rdbWithIndent.IsChecked == true)
                    BizAction.IndentCriteria = 1;
                if (rdbWithoutIndent.IsChecked == true)
                    BizAction.IndentCriteria = 2;

                if (!string.IsNullOrEmpty(txtIssueNo.Text))
                    BizAction.IssueDetailsVO.IssueNumber = txtIssueNo.Text;

                BizAction.IssueFromDate = dpIssueFromDate.SelectedDate;               
                if (dpIssueToDate.SelectedDate == null)
                    BizAction.IssueToDate = dpIssueToDate.SelectedDate;
                else
                    BizAction.IssueToDate = dpIssueToDate.SelectedDate.Value.AddDays(1);

                if (cmbIssueFromStoreSrch.SelectedItem != null)
                {
                    BizAction.IssueFromStoreId = ((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId;
                    if (((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId == 0)
                        BizAction.IssueFromStoreId = null;
                }

                if (cmbIssueToStoreSrch.SelectedItem != null)
                {
                    BizAction.IssueToStoreId = ((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId;
                    if (((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId == 0)
                        BizAction.IssueToStoreId = null;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.intIsIndent = 6;  //For Getting the Non QS Items                 

                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = IssueDataList.PageIndex * IssueDataList.PageSize + 1;
                BizAction.MaximumRows = IssueDataList.PageSize;



                foreach (SortDescription sortDesc in IssueDataList.SortDescriptions)
                {
                    BizAction.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                }
                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);//SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    Indicatior.Close();
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetIssueListBizActionVO)e.Result;
                        BizAction.IssueList = ((clsGetIssueListBizActionVO)e.Result).IssueList;
                        IssueDataList.TotalItemCount = BizAction.TotalRows;

                        IssueDataList.Clear();
                        foreach (var item in BizAction.IssueList)
                        {
                            IssueDataList.Add(item);
                        }

                        dgIssueList.ItemsSource = null;
                        dgIssueList.ItemsSource = IssueDataList;

                        dgdpIssueList.Source = null;
                        dgdpIssueList.PageSize = BizAction.MaximumRows;
                        dgdpIssueList.Source = IssueDataList;
                        txtTotalCountRecords.Text = IssueDataList.TotalItemCount.ToString();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }


        }

        private void txtIssueNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillIssueList();
                dgdpIssueList.PageIndex = 0;
            }
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIssueList.SelectedItem != null)
                {
                    if (((clsIssueListVO)dgIssueList.SelectedItem).IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Direct is already approved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Approve Direct ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();                              

                                BizAction.objIndent = new clsIndentMasterVO();
                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                                     BizAction.objIndent.UnitID = ((clsIssueListVO)dgIssueList.SelectedItem).UnitID;
                                else
                                    BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                BizAction.objIndent.IssueId = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                                BizAction.objIndent.IssueUnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;
                                BizAction.objIndent.IsApproved = true;
                                BizAction.objIndent.IsAuthorized = true;
                                BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                BizAction.objIndent.AuthorizationDate = DateTime.Now;
                                BizAction.IsForApproveDirect = true;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Error == null)
                                    {
                                        if (arg.Result != null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Direct Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            mgbox.Show();
                                            FillIssueList();
                                        }
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            }
                        };
                        mgBox.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Direct is not Freezed \n Please Freeze Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void dgIssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByIssueId();
        }


        private void FillItemListByIssueId()
        {
            if (dgIssueList.SelectedItem != null)
            {
                clsGetItemListByIssueIdBizActionVO BizAction = new clsGetItemListByIssueIdBizActionVO();

                BizAction.IssueId = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                BizAction.UnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;
                BizAction.flagReceivedIssue = false;

                BizAction.ItemList = new List<clsIssueItemDetailsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);//SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemList = ((clsGetItemListByIssueIdBizActionVO)e.Result).ItemList;
                        dgIssueItemList.ItemsSource = null;
                        dgIssueItemList.ItemsSource = BizAction.ItemList;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                dgIssueItemList.ItemsSource = null;
            }
        }

        private void txtIssueItem_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtIssueItem_KeyDown(object sender, KeyEventArgs e)
        {
            //SelectedRowItem = ((clsIssueItemDetailsVO)dgItemList.SelectedItem);
            //SelectedRowItemIndex = dgItemList.SelectedIndex;
        }

        private void FillStore()
        {
            try
            {
                if (Indicatior == null) Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
                BizActionObj.IsUserwiseStores = true;
                BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;             
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.StoreType = 2;
                BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                        BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = false, Parent = 0 };


                        BizActionObj.ItemMatserDetails.Insert(0, Default);
                        BizActionObj.ToStoreList.Insert(0, Default);

                        StoreList = new List<clsStoreVO>();
                        StoreList.AddRange(BizActionObj.ItemMatserDetails);

                        var result = from item in StoreList
                                     where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                     select item;

                        var result2 = from item in StoreList
                                      where item.Status == true && item.IsQuarantineStore != true
                                      select item;



                        flagResetControls = true;

                        var NonQSAndUserAssignedStores = from item in BizActionObj.ToStoreList.ToList()
                                                         where item.IsQuarantineStore == false
                                                         select item;

                        NonQSAndUserAssignedStores.ToList().Insert(0, Default);                    

                        cmbIssueFromStoreSrch.ItemsSource = NonQSAndUserAssignedStores.ToList(); //(List<clsStoreVO>)result.ToList();
                        cmbIssueFromStoreSrch.SelectedItem = NonQSAndUserAssignedStores.ToList()[0];//result.ToList()[0];

                        if (IsInterClinicIndent == true) 
                        {                            
                            cmbIssueToStoreSrch.ItemsSource = result2.ToList();
                            cmbIssueToStoreSrch.SelectedItem = result2.ToList()[0];
                        }
                        else
                        {                            
                            cmbIssueToStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();//BizActionObj.ToStoreList.ToList();  //result2.ToList();
                            cmbIssueToStoreSrch.SelectedItem = ((List<clsStoreVO>)result.ToList())[0]; //result2.ToList()[0];
                        }                        
                    }
                    Indicatior.Close();
                };

                client.CloseAsync();
            }
            catch (Exception EX)
            {
                Indicatior.Close();
                throw;
            }

        }
       

    }
}
