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
using System.Windows.Data;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.Diagnostics.CodeAnalysis;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.ValueObjects.Administration.Menu;
using MessageBoxControl;
using PalashDynamics.Collections;
using System.Reflection;

namespace CIMS.Forms
{
    public partial class frmAddRole : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            return;

        }

        #endregion

        #region Variable Declaration

        bool IsPageLoded = false;
        string msgTitle = "Palash";
        string msgText = "";
        public bool isDuplicate;
        PalashDynamics.Animations.SwivelAnimation _flip = null;

        WaitIndicator waiting = new WaitIndicator();
        bool ISPageLoaded = false;
        public bool ValidCode = true;
        public bool ValidDescription = true;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public bool FlgModify = false;
        public bool IsNew = false;

        #endregion Variable Declaration


        #region Pagging

        public PagedSortableCollectionView<clsUserRoleVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get { return DataList.PageSize; }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillUserRoles();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 


        #endregion

        public frmAddRole()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            #region Pagging

            DataList = new PagedSortableCollectionView<clsUserRoleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            #endregion
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsUserRoleVO();

                waiting.Show();
                FillMenuList();
                FillDashBoardList();
                FillUserRoles();
                IsPageLoded = true;
                waiting.Close();
                txtSearchCriteria.Focus();
                SetCommandButtonState("New");
            }

            IsPageLoded = true;
        }

        private void FillUserRoles()
        {
            this.DataContext = null;
            clsGetRoleGeneralDetailsBizActionVO obj = new clsGetRoleGeneralDetailsBizActionVO();
            obj.InputPagingEnabled = true;
            obj.InputStartRowIndex = DataList.PageIndex * DataList.PageSize;
            obj.InputMaximumRows = DataList.PageSize;
            obj.InputSortExpression = "Description";
            obj.InputSearchExpression = txtSearchCriteria.Text.Trim();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetRoleGeneralDetailsBizActionVO result = ea.Result as clsGetRoleGeneralDetailsBizActionVO;
                    DataList.TotalItemCount = result.OutputTotalRows;
                    if (result.RoleGeneralDetailsList.Count>0 )//!= null)
                    {
                        DataList.Clear();
                        foreach (var item in result.RoleGeneralDetailsList)
                        {
                            DataList.Add(item);
                        }

                        dgRoleList.ItemsSource = null;
                        dgRoleList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = obj.InputMaximumRows;
                        dgDataPager.Source = DataList;

                    }
                    else
                    {
                        msgText = "Role is not exists.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                }
                else
                {
                    msgText = "An Error Occured while Populating User Roles.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(obj, User);
            client.CloseAsync();
            client = null;
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                dgRoleList.SelectedIndex = -1;
             //   tabDashBoard.SelectedIndex = 0;
                IsNew = true;
                ClearFormData();
                SetFormValidation();
                txtCode.Focus();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New Role";

                _flip.Invoke(RotationType.Forward);

                this.SetCommandButtonState("Save");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                string a = txtCode.Text;
                string b = txtDescription.Text;
                SetFormValidation();

                if (ValidCode == true)
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Save the Record?";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else
                    SetFormValidation();
                
                    txtCode.Focus();
                
                waiting.Close();
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }            
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                string a = txtCode.Text;
                string b = txtDescription.Text;
                //if (CheckDuplicasy())
                //{
                    Save();
                    //SetCommandButtonState("Save");
                    //if (isDuplicate == false)
                    //    SetCommandButtonState("Save");
                    //else
                    //    SetCommandButtonState("New");
               // }
            }
            else
            {
            }
        }

        void Save()
        {
            try
            {
                clsUserRoleVO objRole = CreateRoleObjectFromFormData();
                clsAddUserRoleBizActionVO objAddRoleBizVO = new clsAddUserRoleBizActionVO();
                objAddRoleBizVO.Details = objRole;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        int intResut = ((clsAddUserRoleBizActionVO)ea.Result).SuccessStatus;
                        if (intResut == 0)
                        {
                            FillUserRoles();
                            dgRoleList.SelectedIndex = 0;
                            ClearFormData();

                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = "";
                            _flip.Invoke(RotationType.Backward);

                            isDuplicate = false;
                            msgText = "Record is successfully submitted!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else if (intResut == 2)
                        {
                            isDuplicate = true;

                            msgText = "Duplicate Code Cannot be Saved!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgWindow.Show();
                            txtCode.Focus();
                        }
                        else if (intResut == 3)
                        {
                            isDuplicate = true;

                            msgText = "Duplicate Description Cannot be Saved!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgWindow.Show();
                            txtCode.Focus();
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();

                        txtSearchCriteria.Focus();
                    }
                    SetCommandButtonState("New");
                };
                client.ProcessAsync(objAddRoleBizVO, User); //user);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region MyRegion
        private clsUserRoleVO CreateRoleObjectFromFormData()
        {
            clsUserRoleVO objRoleVO = new clsUserRoleVO();

            if (FlgModify == true)
                objRoleVO.ID = ((clsUserRoleVO)dgRoleList.SelectedItem).ID;

            objRoleVO.Code = txtCode.Text.Trim();
            objRoleVO.Description = txtDescription.Text.Trim();
            objRoleVO.Status = true;
            objRoleVO.DashBoardList = (List<clsDashBoardVO>)lstItems.ItemsSource;

            #region Get Selected Menu

            List<clsMenuVO> olist = new List<clsMenuVO>();
            List<clsMenuVO> list = trvMenu.ItemsSource as List<clsMenuVO>;
            foreach (var item in list)
            {
                LoadFlatList(olist, item);
            }
            objRoleVO.MenuList = olist.ToList();

            #endregion Get Selected Menu

            return objRoleVO;
        }
        #endregion

        //private void LoadFlatList(List<clsMenuVO> olist, clsMenuVO item)
        //{
        //    if (item.Status == true && item.Mode != "ACCESSLEVEL")
        //    {
        //        olist.Add(new clsMenuVO() { ID = item.ID, Header = item.Header, Active = item.Status.Value , Status = item.Status });
        //    }

        //    if (item.ChildMenuList != null && olist != null && olist.Count > 0)
        //    {
        //        foreach (var citem in item.ChildMenuList)
        //        {
        //            if (citem.ChildMenuList != null)
        //                LoadFlatList(olist, citem);
        //            else
        //            {
        //                if (citem.Mode == "ACCESSLEVEL" && citem.Status == true)
        //                {
        //                    switch (citem.MenuOrder)
        //                    {
        //                        //"Read" = 2,
        //                        //"Update" = 3
        //                        //"Delete" = 4
        //                        //"Print" = 5
        //                        case 1:     //"Create" 
        //                           olist[olist.Count-1].IsCreate = citem.Status.Value;
        //                           break;

        //                        case 2:
        //                           olist[olist.Count - 1].IsRead = citem.Status.Value;
        //                            break;

        //                        case 3:
        //                            olist[olist.Count - 1].IsUpdate = citem.Status.Value;
        //                            break;

        //                        case 4:
        //                            olist[olist.Count - 1].IsDelete = citem.Status.Value;
        //                            break;

        //                        case 5:
        //                            olist[olist.Count - 1].IsPrint = citem.Status.Value;
        //                            break;

        //                        default:
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        private void LoadFlatList(List<clsMenuVO> olist, clsMenuVO item)
        {
            if (item.Status == true)
            {
                olist.Add(new clsMenuVO() { ID = item.ID, Header = item.Header, Active = item.Status.Value, Status = item.Status });
            }
            if (item.ChildMenuList.Count > 0)
            {
                foreach (var citem in item.ChildMenuList)
                {
                    LoadFlatList(olist, citem);
                }

            }
            else
            {
                return;
            }
        }
        
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                SetFormValidation();
                if (ValidCode == true)
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Modify the Record?";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else
                {
                    
                    //SetFormValidation();
                    //txtCode.RaiseValidationError();
                    //txtCode.Focus();
                }
                waiting.Close();
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }
            
        }
               
        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //if (CheckDuplicasy())
                //{
                    Modify();
                    //SetCommandButtonState("Modify");
                    FlgModify = false;
               // }                
            }
            else
            {
             //   tabDashBoard.SelectedIndex = 0;
                txtCode.Text = ((clsUserRoleVO)dgRoleList.SelectedItem).Code;
                txtDescription.Text = ((clsUserRoleVO)dgRoleList.SelectedItem).Description;
                ShowRoleDetails();
                //SetCommandButtonState("Modify");
            }
        }

        private void Modify()
        {
            try
            {
                FlgModify = true;
                clsUserRoleVO objRole = CreateRoleObjectFromFormData();

                clsAddUserRoleBizActionVO objModifyRoleBizVO = new clsAddUserRoleBizActionVO();

                objModifyRoleBizVO.Details = objRole;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        int intResut = ((clsAddUserRoleBizActionVO)ea.Result).SuccessStatus;
                        if (intResut == 0)
                        {
                            ClearFormData();

                            FillMenuList();
                            FillUserRoles();
                            FillDashBoardList();

                            txtSearchCriteria.Focus();

                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = "";

                            _flip.Invoke(RotationType.Backward);

                            msgText = "Record is Successfully Modified!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                           // SetCommandButtonState("Modify");
                        }
                        else
                        {
                            msgText = "Duplicate Record Cannot be Saved!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgWindow.Show();
                            txtCode.Text = "";
                            txtDescription.Text = "";
                            txtCode.Focus();
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    SetCommandButtonState("New");
                };
                client.ProcessAsync(objModifyRoleBizVO, User); //user);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            clsUserRoleVO Item;
            clsUserRoleVO Item1;
            string a=txtCode.Text;
            string b = txtDescription.Text;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsUserRoleVO>)dgRoleList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsUserRoleVO>)dgRoleList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsUserRoleVO>)dgRoleList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsUserRoleVO)dgRoleList.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsUserRoleVO>)dgRoleList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.ID != ((clsUserRoleVO)dgRoleList.SelectedItem).ID);
            }
            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be saved because Code already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be saved because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            //waiting.Show();
            SetFormValidation();
            try
            {
                if (dgRoleList.SelectedItem != null)
                {
                    if (((clsUserRoleVO)dgRoleList.SelectedItem).Status == true)
                    {
                        IsNew = false;
                    //    tabDashBoard.SelectedIndex = 0;
                        txtCode.Text = ((clsUserRoleVO)dgRoleList.SelectedItem).Code;
                        txtDescription.Text = ((clsUserRoleVO)dgRoleList.SelectedItem).Description;
                        ShowRoleDetails();

                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsUserRoleVO)dgRoleList.SelectedItem).Description;

                        _flip.Invoke(RotationType.Forward);

                        //SetCommandButtonState("Modify");
                    }
                    else
                    {
                        msgText = "Cannot view the Details, the Role is disabled";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                    }
                    SetCommandButtonState("Modify");
                }
                else
                {
                    msgText = "Cannot view the Details.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgWindow.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgRoleList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region  Reset All Controls
        private void ClearFormData()
        {
            try
            {
                txtCode.Text = string.Empty;
                txtCode.Focus();
                txtDescription.Text = string.Empty;
                // To Clear the Menu List.

                chkSelectAll.IsChecked = false;
                List<clsMenuVO> tempList = (List<clsMenuVO>)trvMenu.ItemsSource;
                for (int i = 0; i < tempList.Count; i++)
                {
                    ClearMenuStatus(tempList[i]);
                }
                trvMenu.ItemsSource = null;
                trvMenu.ItemsSource = tempList;
                chkSelectAllDashBoard.IsChecked = false;
                List<clsDashBoardVO> lList = (List<clsDashBoardVO>)lstItems.ItemsSource;
                if (lList != null)
                {
                    foreach (var item in lList)
                    {
                        item.Status = false;
                    }
                    lstItems.ItemsSource = null;
                    lstItems.ItemsSource = lList;

                }
            }
            catch (Exception ex)
            {
                //  throw;
            }

        }

        #endregion  Reset All Controls

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtCode.ClearValidationError();
                txtDescription.ClearValidationError();
                ClearFormData();

                FillMenuList();
                FillUserRoles();
                FillDashBoardList();

                dgRoleList.SelectedIndex = -1;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = "";

                _flip.Invoke(RotationType.Backward);

                cmdSearch_Click(sender, e);
                txtSearchCriteria.Focus();
                SetCommandButtonState("Cancel");                
            }
            catch (Exception ex)
            {
                //  throw;
            }
        }

        private void ClearMenuStatus(clsMenuVO item)
        {
            if (item != null)
            {
                item.Status = false;
            }

            if (item.ChildMenuList != null && item.ChildMenuList.Count > 0)
            {
                foreach (var citem in item.ChildMenuList)
                {
                    ClearMenuStatus(citem);
                }
            }
        }

        private void dgRoleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgRoleList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void chkStatus_Click_old(object sender, RoutedEventArgs e)
        {
            txtCode.ClearValidationError();
            txtDescription.ClearValidationError();

            clsUserRoleVO objSelRole = (clsUserRoleVO)dgRoleList.SelectedItem;

            clsAddUserRoleBizActionVO objModifyRoleBizVO = new clsAddUserRoleBizActionVO();

            objModifyRoleBizVO.Details = objSelRole;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                int intResut = ((clsAddUserRoleBizActionVO)ea.Result).SuccessStatus;
                ClearFormData();
                FillUserRoles();
                dgRoleList.SelectedIndex = -1;

                _flip.Invoke(RotationType.Backward);

                msgText = "Form was Successfully Modified!";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            };
            client.ProcessAsync(objModifyRoleBizVO, User); //user);
            client.CloseAsync();
            client = null;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //this.DataContext = null;
            //waiting.Show();
            //clsGetRoleGeneralDetailsBizActionVO obj = new clsGetRoleGeneralDetailsBizActionVO();
            //obj.InputPagingEnabled = false;
            //obj.InputStartRowIndex = 0;
            //obj.InputMaximumRows = 20;
            //obj.InputSortExpression = "Description";
            //obj.InputSearchExpression = txtSearchCriteria.Text.Trim(); ;

            //PalashServiceClient client = new PalashServiceClient();
            //client.ProcessCompleted += (s, ea) =>
            //{
            //    //SetCommandButtonState("New");
            //    if (ea.Result != null && ea.Error == null)
            //    {
            //        PagedCollectionView pcv = new PagedCollectionView(((clsGetRoleGeneralDetailsBizActionVO)ea.Result).RoleGeneralDetailsList);
            //        pcv.PageSize = 20;
            //        DataContext = pcv;
            //    }
            //    waiting.Close();
            //};
            //client.ProcessAsync(obj, new clsUserVO());
            //client.CloseAsync();
            //client = null;

            FillUserRoles();
        }

        #region Fill Menu
        void FillMenuList()
        {
            try
            {
                clsGetMenuGeneralDetailsBizActionVO objGetMenu = new clsGetMenuGeneralDetailsBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetMenuGeneralDetailsProcessCompleted);
                client.ProcessAsync(objGetMenu, User); //user);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                //  throw;
            }
        }

        void client_GetMenuGeneralDetailsProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                clsGetMenuGeneralDetailsBizActionVO objGetMenu = e.Result as clsGetMenuGeneralDetailsBizActionVO;

                if (objGetMenu.MenuList.Count > 0)
                {
                    long menuid = 9000;
                    List<clsMenuVO> Obj = new List<clsMenuVO>();
                    Obj = objGetMenu.MenuList;
                    for (int i = 0; i < Obj.Count; i++)
                    {
                        if (Obj[i].ChildMenuList.Count > 0)
                        {
                            for (int j = 0; j < Obj[i].ChildMenuList.Count; j++)
                            {
                                //Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Create", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 1, Mode = "ACCESSLEVEL" });
                                //Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Read", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 2, Mode = "ACCESSLEVEL" });
                                //Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Update", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 3, Mode = "ACCESSLEVEL" });
                                //Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Delete", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 4, Mode = "ACCESSLEVEL" });
                                //Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Print", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 5, Mode = "ACCESSLEVEL" });
                            }
                        }
                    }
                    trvMenu.ItemsSource = Obj;
                }
            }
        }
        #endregion

        #region Code For Menu Selection parent-child

        /// <summary>
        /// Gets the parent TreeViewItem of the passed in dependancy object.
        /// </summary>
        /// <param name="item">Item whose parent to wish to find.</param>
        /// <returns>
        /// If item is a TreeViewItem then returns its parent TreeViewItem,
        /// else returns the TreeViewItem containing the item.
        /// </returns>
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called from an event declared in XAML")]
        //private static TreeViewItem GetParentTreeViewItem(DependencyObject item)
        //{
        //    if (item != null)
        //    {
        //        DependencyObject parent = VisualTreeHelper.GetParent(item);
        //        TreeViewItem parentTreeViewItem = parent as TreeViewItem;
        //        return (parentTreeViewItem != null) ? ((TreeViewItem)parentTreeViewItem).GetParentTreeViewItem() : GetParentTreeViewItem(parent);
        //        return (parentTreeViewItem != null) ? ((TreeViewItem)parentTreeViewItem).Parent : GetParentTreeViewItem(parent);

        //    }
        //    return null;
        //}
        private static TreeViewItem GetMenuParentTreeViewItem(DependencyObject item)
        {
            if (item != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(item);
                TreeViewItem parentTreeViewItem = parent as TreeViewItem;
                return (parentTreeViewItem != null) ? parentTreeViewItem : GetMenuParentTreeViewItem(parent);
            }
            return null;
        }

        /// <summary>
        /// Handle the ItemCheckbox.Click event.
        /// </summary>
        /// <param name="sender">The CheckBox.</param>
        /// <param name="e">Event arguments.</param>
        // List<clsMenuVO> selectedMenu;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called from an event declared in XAML")]
        private void MenuCheckBox_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = GetMenuParentTreeViewItem((DependencyObject)sender);
            if (item != null)
            {
                clsMenuVO objMenu = item.DataContext as clsMenuVO;
                UpdateChildrenCheckedState(objMenu);
                UpdateParentCheckedState(item);
            }
        }

        /// <summary>
        /// Sets the Feature bound to the item's parent to the combined
        /// check state of all the children.
        /// </summary>
        /// <param name="item">Item whose parent should be adjust.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called from an event declared in XAML")]
        private static void UpdateParentCheckedState(TreeViewItem item)
        {
            TreeViewItem parent = GetMenuParentTreeViewItem(item);
            if (parent != null)
            {

                clsMenuVO objMenu = parent.DataContext as clsMenuVO;
                if (objMenu != null)
                {
                    // Get the combined checked state of all the children,
                    // determing if they're all checked, all unchecked or a
                    // combination.
                    bool? childrenCheckedState = objMenu.ChildMenuList.First<clsMenuVO>().Status;
                    for (int i = 1; i < objMenu.ChildMenuList.Count(); i++)
                    {
                        if (childrenCheckedState != objMenu.ChildMenuList[i].Status)
                        {
                            childrenCheckedState = true;
                            break;
                        }
                    }
                    // Set the parent to the combined state of the children.
                    objMenu.Status = childrenCheckedState;
                    // Continue up the tree updating each parent with the
                    // correct combined state.
                    UpdateParentCheckedState(parent);
                }
            }
        }

        /// <summary>
        /// Sets the feature's children checked states, including subcomponents,
        /// to match the state of feature.
        /// </summary>
        /// <param name="feature">Feature whose children should be set.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called from an event declared in XAML")]
        private static void UpdateChildrenCheckedState(clsMenuVO objMenu)
        {
            if (objMenu.Status.HasValue && objMenu.ChildMenuList != null)
            {
                foreach (var childMenu in objMenu.ChildMenuList)
                {
                    childMenu.Status = objMenu.Status;
                    if (childMenu.ChildMenuList != null)
                    {
                        if (childMenu.ChildMenuList.Count() > 0)
                        {
                            UpdateChildrenCheckedState(childMenu);
                        }
                    }
                }
            }
        }

        #endregion

        void ShowRoleDetails()
        {
            txtCode.Focus();
            clsUserRoleVO objRole = dgRoleList.SelectedItem as clsUserRoleVO;
            clsGetSelectedRoleMenuIdBizActionVO objRoleMenuDetails = new clsGetSelectedRoleMenuIdBizActionVO();
            objRoleMenuDetails.RoleId = objRole.ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_ShowRoleDetailsProcessCompleted);
            client.ProcessAsync(objRoleMenuDetails, User); //user);
            client.CloseAsync();
        }

        void client_ShowRoleDetailsProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                clsGetSelectedRoleMenuIdBizActionVO objRoleMenuDetails = e.Result as clsGetSelectedRoleMenuIdBizActionVO;

                if (objRoleMenuDetails.DashBoardList != null)
                {
                    List<clsDashBoardVO> lList = (List<clsDashBoardVO>)lstItems.ItemsSource;

                    foreach (var smitem in objRoleMenuDetails.DashBoardList)
                    {
                        foreach (var item in lList)
                        {
                            if (item.ID == smitem.ID)
                            {
                                item.Status = smitem.Status;
                                break;
                            }
                        }
                    }
                    lstItems.ItemsSource = null;
                    lstItems.ItemsSource = lList;
                }

                if (objRoleMenuDetails.MenuList != null)
                {
                    foreach (var smitem in objRoleMenuDetails.MenuList)
                    {
                        foreach (var item in ((List<clsMenuVO>)trvMenu.ItemsSource))
                        {
                            if (item.ID == smitem.ID)
                            {
                                item.Status = smitem.Status;
                                //item.IsCreate = smitem.IsCreate;
                                //item.IsUpdate = smitem.IsUpdate;
                                //item.IsRead = smitem.IsRead;
                                //item.IsDelete = smitem.IsDelete;
                                //item.IsPrint = smitem.IsPrint;
                                break;
                            }
                            else
                            {
                                if (item.ChildMenuList.Count > 0)
                                {
                                    if (CheckMenuChildItems(item.ChildMenuList, smitem))
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CheckMenuChildItems(List<clsMenuVO> MenuList, clsMenuVO sMenuItem)
        {
            foreach (var citem in MenuList)
            {
                if (citem.ID == sMenuItem.ID)
                {
                    citem.Status = sMenuItem.Status;
                    if (citem.ChildMenuList != null)
                    {
                        foreach (var aitem in citem.ChildMenuList)
                        {
                            if (aitem.Mode == "ACCESSLEVEL")
                            {
                                switch (aitem.MenuOrder)
                                {
                                    //"Read" = 2,
                                    //"Update" = 3
                                    //"Delete" = 4
                                    //"Print" = 5
                                    case 1:     //"Create" 
                                        aitem.IsCreate = sMenuItem.IsCreate;
                                        aitem.Status = sMenuItem.IsCreate;
                                        break;

                                    case 2:
                                        aitem.IsRead = sMenuItem.IsRead;
                                        aitem.Status = sMenuItem.IsRead;
                                        break;

                                    case 3:
                                        aitem.IsUpdate = sMenuItem.IsUpdate;
                                        aitem.Status = sMenuItem.IsUpdate;
                                        break;

                                    case 4:
                                        aitem.IsDelete = sMenuItem.IsDelete;
                                        aitem.Status = sMenuItem.IsDelete;
                                        break;

                                    case 5:
                                        aitem.IsPrint = sMenuItem.IsPrint;
                                        aitem.Status = sMenuItem.IsPrint;
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }

                    }
                    return true;
                }
                else
                {
                    if (citem.ChildMenuList != null)
                    {
                        if (citem.ChildMenuList.Count > 0)
                        {
                            CheckMenuChildItems(citem.ChildMenuList, sMenuItem);
                        }
                    }
                }
            }
            return false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (IsPageLoded)
            {
                //switch (tabDashBoard.SelectedIndex)
                //{
                //    case 0:
                //        break;

                //    case 1:

                //        //  if (lstItems.ItemsSource == null)

                //        break;
                //}
            }
        }

        private void FillDashBoardList()
        {
            clsGetDashBoardListVO obj = new clsGetDashBoardListVO();
            obj.ID = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    lstItems.ItemsSource = ((clsGetDashBoardListVO)ea.Result).List;
                }
            };
            client.ProcessAsync(obj, User); //user);
            client.CloseAsync();
            client = null;
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateRoleStatusBizActionVO obj = new clsUpdateRoleStatusBizActionVO();
            obj.RoleStatus = new clsUserRoleVO();
            obj.RoleStatus = ((clsUserRoleVO)dgRoleList.SelectedItem);
            obj.RoleStatus.Status = ((clsUserRoleVO)dgRoleList.SelectedItem).Status;

            if (User.RoleName == obj.RoleStatus.Description)
            {


                //((clsUserRoleVO)dgRoleList.SelectedItem).Status = true;
                //dgRoleList.ItemsSource = DataList;
                //dgRoleList.UpdateLayout();
                //dgRoleList.Focus();
                FillUserRoles();

                obj.RoleStatus.Status = true;
                string msgText = "Role cannot be Deactivated.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
               
                msgWindow.Show();
            }
            else
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        if (obj.RoleStatus.Status == false)
                        {
                            string msgText = "Status Updated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            string msgText = "Status Updated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        string msgText = "An Error Occured";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                    }
                };
                client.ProcessAsync(obj, User);
                client.CloseAsync();
                client = null;
            }
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {            //trvMenu.

            try
            {
                if (chkSelectAll.IsChecked == true)
                {
                    List<clsMenuVO> mList = (List<clsMenuVO>)trvMenu.ItemsSource;
                    foreach (var item in mList)
                    {
                        item.Status = true;
                        List<clsMenuVO> objSubMenu = (List<clsMenuVO>)item.ChildMenuList;
                        foreach (var subMenu in objSubMenu)
                        {
                            subMenu.Status = true;
                            int CountSubmenu = subMenu.ChildMenuList.Count;
                            for (int SCount = 0; SCount < CountSubmenu; SCount += 1)
                            {
                                subMenu.ChildMenuList[SCount].Status = true;
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

        private bool SetFormValidation()
        {
            ValidCode = true;
          //  ValidDescription = true;
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Code cannot be blank.");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                ValidCode = false;
            }
            else
             txtCode.ClearValidationError(); 
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Description cannot be blank.");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                ValidCode = false;
            }
            else
                txtDescription.ClearValidationError();
            return ValidCode;
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == false)
                {
                    List<clsMenuVO> mList = (List<clsMenuVO>)trvMenu.ItemsSource;
                    foreach (var item in mList)
                    {
                        item.Status = false;
                        List<clsMenuVO> objSubMenu = (List<clsMenuVO>)item.ChildMenuList;
                        foreach (var subMenu in objSubMenu)
                        {
                            subMenu.Status = false;
                            int CountSubmenu = subMenu.ChildMenuList.Count;
                            for (int SCount = 0; SCount < CountSubmenu; SCount += 1)
                            {
                                subMenu.ChildMenuList[SCount].Status = false;
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

        private void chkSelectAllDashBoard_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAllDashBoard.IsChecked == true)
                {
                    List<clsDashBoardVO> DashBoardList = new List<clsDashBoardVO>();
                    DashBoardList = (List<clsDashBoardVO>)lstItems.ItemsSource;

                    if (DashBoardList != null)
                    {
                        foreach (var item in DashBoardList)
                        {
                            item.Status = true;
                        }
                    }
                    lstItems.ItemsSource = null;
                    lstItems.ItemsSource = DashBoardList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkSelectAllDashBoard_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAllDashBoard.IsChecked == false)
                {
                    List<clsDashBoardVO> DashBoardList = new List<clsDashBoardVO>();
                    DashBoardList = (List<clsDashBoardVO>)lstItems.ItemsSource;

                    if (DashBoardList != null)
                    {
                        foreach (var item in DashBoardList)
                        {
                            item.Status = false;
                        }
                    }
                    lstItems.ItemsSource = null;
                    lstItems.ItemsSource = DashBoardList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillUserRoles();
            }
        }
    }
}
