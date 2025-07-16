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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Text.RegularExpressions;

namespace PalashDynamics.Administration
{
    public partial class frmApplicatonConfigEmailCCinCCTo : ChildWindow
    {
        ObservableCollection<clsAppEmailCCToVo> myAppConfigEmail = new ObservableCollection<clsAppEmailCCToVo>();
        public event RoutedEventHandler OnCloseButton_Click;
        public ObservableCollection<clsAppEmailCCToVo> EventList { get; set; }
        long UnitID, configAutoEmailID;
        public PagedSortableCollectionView<clsAppEmailCCToVo> DataList { get; private set; }
        public PagedSortableCollectionView<clsAppEmailCCToVo> DataList1 { get; private set; }
        public frmApplicatonConfigEmailCCinCCTo(long EUnitID, long EconfigAutoEmailID)
        {
            InitializeComponent();
            UnitID = EUnitID;
            configAutoEmailID = EconfigAutoEmailID;

            //this.DataContext = new clsItemMasterVO();
            //objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsAppEmailCCToVo>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //DataListPageSize = 15;
            //dgDataPager.PageSize = DataListPageSize;
            //dgDataPager.Source = DataList;
            //======================================================


        }
        clsAppEmailCCToVo objItemVO = new clsAppEmailCCToVo();

        private void BindItemListGrid()
        {
            try
            {
                #region Commented
                //False when we want to fetch all items
                //clsItemMasterVO obj = new clsItemMasterVO();
                //obj.RetrieveDataFlag = false;

                //if (IsSearchButtonClicked == true)
                //{
                //BizActionObj.AppEmailCC.RetrieveDataFlag = true;
                //}
                //else
                //{
                //    BizActionObj.ItemDetails.RetrieveDataFlag = false;
                //}

                //BizActionObj.PagingEnabled = true;
                //BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                //BizActionObj.MaximumRows = DataList.PageSize;
                //BizActionObj.ItemDetails.ItemName = txtSearchItemName.Text;//== "" ? "" : txtSearchItemName.Text
                //BizActionObj.ItemDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                #endregion
                WaitIndicator w = new WaitIndicator();
                w.Show();

                clsAppEmailCCToBizActionVo BizActionObj = new clsAppEmailCCToBizActionVo();
                BizActionObj.AppEmailCC = new clsAppEmailCCToVo();
                BizActionObj.UnitID = UnitID;
                BizActionObj.ConfigAutoEmailID = configAutoEmailID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsAppEmailCCToBizActionVo result = args.Result as clsAppEmailCCToBizActionVo;
                        DataList.Clear();
                        DataList.TotalItemCount = result.TotalRowCount;
                        if (result.ItemList != null)
                        {
                            foreach (var item in result.ItemList)
                            {
                                DataList.Add(item);
                            }
                            dgEmailCCTo.ItemsSource = null;
                            dgEmailCCTo.ItemsSource = DataList;
                            #region Commented for pagingg

                            //dgEmailCCTo.SelectedIndex = (int)((clsAppEmailCCToVo)this.DataContext).ConfigAutoEmailID;
                            //dgEmailCCTo.SelectedItem = result.ItemList[result.ItemList.Count - 1];
                            //if (DataList.Count > 0)
                            //{
                            //    dgEmailCCTo.SelectedItem = DataList[DataList.Count - 1];
                            //}
                            //dgEmailCCTo.ItemsSource = DataList;
                            //dgEmailCCTo.Source = null;  
                            //dgEmailCCTo.PageSize = BizActionObj.MaximumRows;
                            #endregion
                        }
                    }
                    w.Close();
                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseButton_Click(this, new RoutedEventArgs());
            //  this.DialogResult = false;
            this.Close();
        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindItemListGrid();
        }
        private void dgEmailCCTo_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dgEmailCCTo.SelectedItem != null)
            {
                try
                {     // refence of Store master form.... in pharamcy 
                    clsStatusEmailCCToBizActionVo bizactionVO = new clsStatusEmailCCToBizActionVo();
                    bizactionVO.AppEmailCC = new clsAppEmailCCToVo();

                    bizactionVO.AppEmailCC.ID = ((clsAppEmailCCToVo)dgEmailCCTo.SelectedItem).ID;
                    bizactionVO.AppEmailCC.UnitID = ((clsAppEmailCCToVo)dgEmailCCTo.SelectedItem).UnitID;
                    bizactionVO.AppEmailCC.ConfigAutoEmailID = ((clsAppEmailCCToVo)dgEmailCCTo.SelectedItem).ConfigAutoEmailID;
                    bizactionVO.AppEmailCC.CCToEmailID = ((clsAppEmailCCToVo)dgEmailCCTo.SelectedItem).CCToEmailID;
                    bizactionVO.AppEmailCC.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                    bizactionVO.AppEmailCC.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizactionVO.AppEmailCC.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    bizactionVO.AppEmailCC.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    bizactionVO.AppEmailCC.UpdatedDateTime = System.DateTime.Now;
                    bizactionVO.AppEmailCC.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsStatusEmailCCToBizActionVo)args.Result).ResultStatus == 1)
                            {
                                BindItemListGrid();
                                string msgText = "Status Updated Sucessfully";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        clsAddEmailIDCCToBizActionVo bizactionVO;
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgText = "Are you sure..\n  you want to add New Email ID ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                    bizactionVO = new clsAddEmailIDCCToBizActionVo();
                    bizactionVO.AppEmailCC = new clsAppEmailCCToVo();
                    bizactionVO.AppEmailCC.UnitID = UnitID;
                    bizactionVO.AppEmailCC.ConfigAutoEmailID = configAutoEmailID;
                    bizactionVO.AppEmailCC.CCToEmailID = Convert.ToString(TxtbEmailCCTo.Text.Trim());

                    bizactionVO.AppEmailCC.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizactionVO.AppEmailCC.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    bizactionVO.AppEmailCC.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    bizactionVO.AppEmailCC.AddedDateTime = DateTime.Now;
                    bizactionVO.AppEmailCC.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddEmailIDCCToBizActionVo)args.Result).ResultStatus == 1)
                            {
                                BindItemListGrid();
                                string msgText = "Email ID is successfully updated.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                            else if (((clsAddEmailIDCCToBizActionVo)args.Result).ResultStatus == 2)
                            {
                                string msgText = "Email ID is already exists.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                        }
                        ClearUI();
                    };

                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ClearUI()
        {
            TxtbEmailCCTo.Text = "";
        }

        public bool Validation()
        {
            if (string.IsNullOrEmpty(TxtbEmailCCTo.Text))
            {
                TxtbEmailCCTo.SetValidation("Please Enter EmailD");
                TxtbEmailCCTo.RaiseValidationError();
                TxtbEmailCCTo.Focus();
                return false;
            }
            else
            {
                TxtbEmailCCTo.ClearValidationError();
            }
            var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

            if (regex.IsMatch(TxtbEmailCCTo.Text) == false)
            {
                TxtbEmailCCTo.SetValidation("Please Enter Valid EmailID");
                TxtbEmailCCTo.RaiseValidationError();
                TxtbEmailCCTo.Focus();
                return false;
            }
            else
            {
                TxtbEmailCCTo.ClearValidationError();
            }
            return true;
        }
    }
}

