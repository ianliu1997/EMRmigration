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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using System.Windows.Data;
using System.Diagnostics.CodeAnalysis;
using PalashDynamics.ValueObjects.Administration.Menu;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Collections;
using PalashDynamics.Service.EmailServiceReference;
using System.Reflection;
using MessageBoxControl;
using System.Threading;


namespace CIMS.Forms
{
    public partial class frmAddUser : UserControl
    {
        #region Public Variables

        SwivelAnimation _FlipAnimation = null;
        public bool chkNullMinPassLen = true;
        public bool chkNullMaxPassLen = true;
        public bool chkNullMinPassAge = true;
        public bool chkNullMaxPassAge = true;
        public bool chkNullAccThreshold = true;
        public bool chkNullAccDuration = true;
        public bool chkNullPassToRemember = true;
        public bool Updateconfirm = true;
        string msgTitle = "";
        string msgText = "";
        public bool UserNameValid = false;
        public bool UserExists = false;
        public Int16 value;
        public string tempStr;
        public string tempPass;
        public bool flgModify = false;
        WaitIndicator waiting = new WaitIndicator();
        public bool ValidAutoUserName = true;
        public bool ValidLogin = true;
        public bool isFormValid = true;
        long DefaultUnitId = 0;
        bool isEditMode = false;
       // bool isStoreEditMode = false;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; //new clsUserVO();

        long UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
        public long UserTypeID = ((IApplicationConfiguration)App.Current).CurrentUser.UserType;
        public long RetrievedUserId;
        PalashServiceClient client = null;
        clsGetSelectedRoleMenuIdBizActionVO objSelectedRole;
        public long SelectedUnitId;
        public long SelectedUserId;
        bool IsPageLoded = false;
        public string StoreName;
        public long StoreId;
        clsAppConfigVO myAppConfig = new clsAppConfigVO();
        public string UserEmailId;
        public string UserName;
        public bool LoginNameExists;
        bool EditStore = false;
        bool newUser = false;
        public clsUserUnitDetailsVO objVo;
        List<MasterListItem> TypeList = new List<MasterListItem>();
        List<MasterListItem> StatusList = new List<MasterListItem>();
        #endregion

        #region Pagging

        public PagedSortableCollectionView<clsUserVO> DataList { get; private set; }

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
            FillUserList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        #endregion

        public frmAddUser()
        {
            InitializeComponent();
            optDoctor.IsChecked = true;
            _FlipAnimation = new SwivelAnimation(MainPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 400)));
            objVo = new clsUserUnitDetailsVO();

            #region Pagging

            DataList = new PagedSortableCollectionView<clsUserVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

            #endregion

        }
     
        #region Code To Fill Lists
        #region Fill Menu

        void FillMenuList()
        {
            clsGetMenuGeneralDetailsBizActionVO objGetMenu = new clsGetMenuGeneralDetailsBizActionVO();
            //client = new PalashServiceClient();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetMenuGeneralDetailsProcessCompleted);
            client.ProcessAsync(objGetMenu, User); //user);
            client.CloseAsync();
        }

        void client_GetMenuGeneralDetailsProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            if (e.Result != null && e.Error == null)
            {
                clsGetMenuGeneralDetailsBizActionVO objGetMenu = e.Result as clsGetMenuGeneralDetailsBizActionVO;

                if (objGetMenu.MenuList.Count > 0)
                {
                    long menuid = 9000;
                    List<clsMenuVO> Obj = new List<clsMenuVO>();
                    Obj = objGetMenu.MenuList;
            //        for (int i = 0; i < Obj.Count; i++)
            //        {
            ////            if (Obj[i].ChildMenuList.Count > 0)
            //////            {
            ////                for (int j = 0; j < Obj[i].ChildMenuList.Count; j++)
            ////                {
            ////                    Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Create", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 1, Mode = "ACCESSLEVEL" });
            ////                    Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Read", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 2, Mode = "ACCESSLEVEL" });
            ////                    Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Update", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 3, Mode = "ACCESSLEVEL" });
            ////                    Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Delete", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 4, Mode = "ACCESSLEVEL" });
            ////                    Obj[i].ChildMenuList[j].ChildMenuList.Add(new clsMenuVO() { ID = menuid++, Title = "Print", Status = Obj[i].ChildMenuList[j].Status, ParentId = Obj[i].ChildMenuList[j].ID, MenuOrder = 5, Mode = "ACCESSLEVEL" });
            ////                }
            ////            }
            //        }

                    objGetMenu.MenuList = Obj;
                }
                trvMenu.ItemsSource = objGetMenu.MenuList;
                // trvMenu.CollapseAll();
            }

        }
        #endregion

        WaitIndicator indicator = new WaitIndicator();

        #region Fill User List
        void FillUserList()
        {
            
            try
            {
                indicator.Show();

                this.DataContext = null;

                clsGetUserListBizActionVO obj = new clsGetUserListBizActionVO();
                obj.IsPagingEnabled = true;
                obj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                obj.MaximumRows = DataList.PageSize;
                obj.SearchExpression = txtSearchCriteria.Text.Trim();
                if (((clsUserRoleVO)cmbUserRole.SelectedItem).Description!="")
                    obj.UserRoleID = Convert.ToInt64(((clsUserRoleVO)cmbUserRole.SelectedItem).ID);
                else
                {
                    obj.UserRoleID = 0;
                }
                if (((MasterListItem)cmbUserType.SelectedItem) != null)
                {
                    if (((MasterListItem)cmbUserType.SelectedItem).ID == 1)
                    { obj.IsDoctor = true; }
                    else if (((MasterListItem)cmbUserType.SelectedItem).ID == 2)
                    { obj.IsEmployee = true; }
                    else if (((MasterListItem)cmbUserType.SelectedItem).ID == 3)
                    { obj.IsPatient = true; }
                    //  if (((MasterListItem)cmbUserStatus.SelectedItem).ID == 1)
                    // { obj.IsActive = true; }
                }
                else
                {
                    obj.UserRoleID = 0;
                    obj.IsDoctor = false;
                    obj.IsEmployee = false;
                    obj.IsPatient = false;
                }
                //else if (((clsUserVO)cmbUserStatus.SelectedItem).ID == 2)
                //{ obj.IsDeActive = true; }
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetUserListBizActionVO result = ea.Result as clsGetUserListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.Details!= null)
                        {
                          //  DataList.Clear();
                            foreach (var item in result.Details)
                            {
                                DataList.Add(item);
                            }

                            dgUserList.ItemsSource = null;
                            dgUserList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = obj.MaximumRows;
                            dgDataPager.Source = DataList;
                            
                            txtTotalUser.Text = Convert.ToString(result.TotalRows);
                        }
                        if(DataList.Count==0)
                        {
                            msgText = "User does not exists.";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured while Populating User List.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    indicator.Close();
                    txtSearchCriteria.Focus();
                };
                client.ProcessAsync(obj, User); 
                client.CloseAsync();
                client = null;
               // txtSearchCriteria.Focus();
            }
            catch (Exception ex)
            {
                // throw;
                indicator.Close();
            }
        }

        #endregion            

        #region FillUnits

        private void FillUnits()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        List<MasterListItem> uList = new List<MasterListItem>();
                        uList = ((clsGetMasterListBizActionVO)ea.Result).MasterList;

                        dgUnitList.ItemsSource = null;

                        List<clsUserUnitDetailsVO> userUnitList = new List<clsUserUnitDetailsVO>();

                        foreach (var item in uList)
                        {
                            userUnitList.Add(new clsUserUnitDetailsVO() { UnitID = item.ID, UnitName = item.Description, Status = false, IsDefault = false });
                        }
                        dgUnitList.ItemsSource = userUnitList;
                    }

                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
                client = null;

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region FillRoleList

        void FillRole()
        {
            clsGetRoleGeneralDetailsBizActionVO obj = new clsGetRoleGeneralDetailsBizActionVO();
            obj.InputPagingEnabled = false;
            obj.InputStartRowIndex = 0;
            obj.InputMaximumRows = 20;
            obj.InputSortExpression = "Description";
            obj.InputSearchExpression = "";

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    dgUserRoleList.ItemsSource = null;
                    dgUserRoleList.ItemsSource = ((clsGetRoleGeneralDetailsBizActionVO)ea.Result).RoleGeneralDetailsList;

                    clsUserRoleVO Default = new clsUserRoleVO { ID = 0, Description = "Select" };
                    ((clsGetRoleGeneralDetailsBizActionVO)ea.Result).RoleGeneralDetailsList.Insert(0,Default);
                    cmbUserRole.ItemsSource = ((clsGetRoleGeneralDetailsBizActionVO)ea.Result).RoleGeneralDetailsList;
                    cmbUserRole.SelectedItem=((clsGetRoleGeneralDetailsBizActionVO)ea.Result).RoleGeneralDetailsList[0];
                }
                FillUserList();
            };
            client.ProcessAsync(obj, User);
            client.CloseAsync();
            client = null;
        }

        public enum UserType
        {
            Select,
            Doctor,
            Employee,
            Patient
        };

        public enum UserStatus
        {
            Select,
            Active,
            Deactive
        };

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return typeof(T)
                .GetFields()
                .Where(x => x.IsLiteral)
                .Select(field => (T)field.GetValue(null));
        }
        #endregion

        #endregion

        #region Form_Event
        void frmAddUser_Loaded(object sender, RoutedEventArgs e)
        {
          //  grbUnitStore.Visibility = Visibility.Collapsed;
            if (!IsPageLoded)
            {
                FillRole();
                this.DataContext = null;
                optDoctor.IsChecked = true;
                FillMenuList();
             //   FillUserList();
                FillUserTypeWiseUserList();
                FillUnits();
                optPatient.IsEnabled = false;
                txtSearchCriteria.Focus();
                myAppConfig = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;
                
                SetCommandButtonState("New");
                FillPassConfig();
                //Added By Umesh
                int i = 0;
                foreach (var bindingFlag in GetEnumValues<UserType>())
                {
                    TypeList.Add(new MasterListItem(i, bindingFlag.ToString()));
                    i++;
                }
                cmbUserType.ItemsSource = null;
                cmbUserType.ItemsSource = TypeList.DeepCopy();
                cmbUserType.SelectedItem = TypeList[0];

                int j = 0;
                foreach (var bindingFlag in GetEnumValues<UserStatus>())
                {
                    StatusList.Add(new MasterListItem(j, bindingFlag.ToString()));
                    i++;
                }
                cmbUserStatus.ItemsSource = null;
                cmbUserStatus.ItemsSource = StatusList.DeepCopy();
                cmbUserStatus.SelectedItem = StatusList[0];
                
            }

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "User List ";

            IsPageLoded = true;           
        }

        #region Option Button Event

        #region Check Permission
        
        private void optGroupPermissions_Checked(object sender, RoutedEventArgs e)
        {
            //if (IsPageLoded)
            //{
            //    FillPermissionDetails();
            //    //PermissionLoaded = true;
            //}
        }

        private void optRolePermissions_Checked(object sender, RoutedEventArgs e)
        {
            //if (IsPageLoded)
            //{
            //    FillPermissionDetails();
            //    //PermissionLoaded = true;
            //}
        }

        private void optCustomPermissions_Checked(object sender, RoutedEventArgs e)
        {
            //if (IsPageLoded)
            //{
            //    FillPermissionDetails();
            //}
        }
        #endregion

        #endregion

        #region Unit-Cash Counter Event
        // List<clsCashCounterGeneralDetailVO> selectedCashCounter;
        private void chkCashCounter_Checked(object sender, RoutedEventArgs e)
        {
            CheckUnCheckCashCounter(sender, e, true);
        }

        private void chkCashCounter_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckUnCheckCashCounter(sender, e, false);
        }

        public void CheckUnCheckCashCounter(object sender, RoutedEventArgs e, bool Status)
        {
            //if (selectedCashCounter == null)
            //{
            //    selectedCashCounter = new List<clsCashCounterGeneralDetailVO>();
            //}

            //clsCashCounterGeneralDetailVO t = ((CheckBox)sender).DataContext as clsCashCounterGeneralDetailVO;

            //var records = from cashcounters in selectedCashCounter
            //              where cashcounters.UnitId == t.UnitId && cashcounters.CashCounterId == t.CashCounterId
            //              select cashcounters;


            //if (records != null && records.Count() > 0)
            //{
            //    foreach (var item in records)
            //    {
            //        item.Status = Status;
            //    }
            //}
            //else
            //{
            //    selectedCashCounter.Add((clsCashCounterGeneralDetailVO)((CheckBox)sender).DataContext);
            //}
        }
        // List<clsUnitGeneralDetailVO> objUnit;
        private void UnitCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender) != null)
            {
                //  clsUnitGeneralDetailVO temp = ((CheckBox)sender).DataContext as clsUnitGeneralDetailVO;

                //foreach (var item in temp.CashCounterList)
                //{
                //    item.Status = ((CheckBox)sender).IsChecked.Value;
                //}

            }

        }
        private void chkCashCounter_Click(object sender, RoutedEventArgs e)
        {
            //TreeViewItem parent = GetParentTreeViewItem((DependencyObject)sender);

            // if (parent != null)
            // {
            //clsUnitGeneralDetailVO Unit = parent.DataContext as clsUnitGeneralDetailVO;
            //bool IsChecked = false;
            //foreach (var item in Unit.CashCounterList)
            //{
            //    if (item.Status == true)
            //        IsChecked = true;
            //}
            //Unit.Status = IsChecked;
            // }
        }

        #endregion
        
        private void chkExpirationDays_Click(object sender, RoutedEventArgs e)
        {
            if (chkExpirationDays.IsChecked.Value)
                txtExpirationDays.IsReadOnly = false;
            else
                txtExpirationDays.IsReadOnly = true;
        }

        #region Command Button Event
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            txtBLTotalUser.Visibility = Visibility.Collapsed;
            txtTotalUser.Visibility = Visibility.Collapsed;
            try
            {
                newUser = true;
                ClearFormData();
                TabUser.SelectedIndex = 0;
                autoUserName.IsEnabled = true;
                grbUserType.IsEnabled = true;
                optDoctor.IsChecked = true;

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New User";
                AutoPassword.Visibility = Visibility.Visible;
                lblPassword.Visibility = Visibility.Visible;

                SetFormValidation();
                            

                _FlipAnimation.Invoke(RotationType.Forward);
              
                SetCommandButtonState("Save");
                FillPassConfig();
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

  
        #region TextBox Validation

        private void txtPasswordMinLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
            {
                value = Convert.ToInt16(txtPasswordMinLength.Text);
                if (value > 32767)
                {
                    txtPasswordMinLength.SetValidation("Maximum Allowed Value is 32767");
                    txtPasswordMinLength.RaiseValidationError();
                    txtPasswordMinLength.Text = "";
                    txtPasswordMinLength.Focus();
                    chkNullMinPassLen = false;
                }
            }
        }

        private void txtPasswordMaxLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordMaxLength.Text.ToString() != null && txtPasswordMaxLength.Text.ToString() != "" && txtPasswordMaxLength.Text.Length != 0)
            {
                value = Convert.ToInt16(txtPasswordMaxLength.Text);
                if (value > 32767)
                {
                    txtPasswordMaxLength.SetValidation("Maximum Allowed Value is 32767");
                    txtPasswordMaxLength.RaiseValidationError();
                    txtPasswordMaxLength.Text = "";
                    txtPasswordMaxLength.Focus();
                    chkNullMaxPassLen = false;
                }
                if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
                {
                    int MinPasswordLengthValue = Convert.ToInt32(txtPasswordMinLength.Text);
                    int MaxPasswordLengthValue = Convert.ToInt32(txtPasswordMaxLength.Text);

                    if (MinPasswordLengthValue > MaxPasswordLengthValue)
                    {
                        txtPasswordMaxLength.SetValidation("Maximum Password Length should not be less than Minimum Password Length");
                        txtPasswordMaxLength.RaiseValidationError();
                        txtPasswordMaxLength.Text = "";
                        txtPasswordMaxLength.Focus();
                        chkNullMaxPassLen = false;
                    }
                }
            }
        }

        private void txtMinPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMinPasswordAge.Text.ToString() != null && txtMinPasswordAge.Text.ToString() != "" && txtMinPasswordAge.Text.Length != 0)
            {
                value = Convert.ToInt16(txtMinPasswordAge.Text);
                if (value > 32767)
                {
                    txtMinPasswordAge.SetValidation("Maximum Allowed Value is 32767");
                    txtMinPasswordAge.RaiseValidationError();
                    txtMinPasswordAge.Text = "";
                    txtMinPasswordAge.Focus();
                    chkNullMinPassAge = false;
                }
            }
        }

        private void txtMaxPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMaxPasswordAge.Text.ToString() != null && txtMaxPasswordAge.Text.ToString() != "" && txtMaxPasswordAge.Text.Length != 0)
            {
                value = Convert.ToInt16(txtMaxPasswordAge.Text);
                if (value > 32767)
                {
                    txtMaxPasswordAge.SetValidation("Maximum Allowed Value 32767");
                    txtMaxPasswordAge.RaiseValidationError();
                    txtMaxPasswordAge.Text = "";
                    txtMaxPasswordAge.Focus();
                    chkNullMaxPassAge = false;
                }
                if (txtMinPasswordAge.Text.ToString() != null && txtMinPasswordAge.Text.ToString() != "" && txtMinPasswordAge.Text.Length != 0)
                {
                    int MinPasswordAge = Convert.ToInt16(txtMinPasswordAge.Text);
                    int MaxPasswordAge = Convert.ToInt16(txtMaxPasswordAge.Text);

                    if (MinPasswordAge > MaxPasswordAge)
                    {
                        txtMaxPasswordAge.SetValidation("Maximum Password Age Should be Greater Than Minimum Password Age");
                        txtMaxPasswordAge.RaiseValidationError();
                        txtMaxPasswordAge.Text = "";
                        txtMaxPasswordAge.Focus();
                        chkNullMaxPassAge = false;
                    }
                }
            }
        }

        private void txtAccountLockThreshold_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAccountLockThreshold.Text.ToString() != null && txtAccountLockThreshold.Text.ToString() != "" && txtAccountLockThreshold.Text.Length != 0)
            {
                value = Convert.ToInt16(txtAccountLockThreshold.Text);
                if (value > 32767)
                {
                    txtAccountLockThreshold.SetValidation("Maximum Allowed Value 32767");
                    txtAccountLockThreshold.RaiseValidationError();
                    txtAccountLockThreshold.Text = "";
                    txtAccountLockThreshold.Focus();
                    chkNullAccThreshold = false;
                }
            }
        }

        private void txtAccountLockDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAccountLockDuration.Text.ToString() != null && txtAccountLockDuration.Text.ToString() != "" && txtAccountLockDuration.Text.Length != 0)
            {
                value = Convert.ToInt16(txtAccountLockDuration.Text);
                if (value > 32767)
                {
                    txtAccountLockDuration.SetValidation("Maximum Allowed Value 32767");
                    txtAccountLockDuration.RaiseValidationError();
                    txtAccountLockDuration.Text = "";
                    txtAccountLockDuration.Focus();
                    chkNullAccDuration = false;
                }
            }
        }

        private void txtPasswordRemember_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPasswordRemember.Text.ToString() != null && txtPasswordRemember.Text.ToString() != "" && txtPasswordRemember.Text.Length != 0)
            {
                value = Convert.ToInt16(txtPasswordRemember.Text);
                if (value > 32767)
                {
                    txtPasswordRemember.SetValidation("Maximum Allowed Value 32767");
                    txtPasswordRemember.RaiseValidationError();
                    txtPasswordRemember.Text = "";
                    txtPasswordRemember.Focus();
                    chkNullPassToRemember = false;
                }
            }
        }
        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                newUser = false;
                SetFormValidation();
                //CheckUserNameValidation();
               // CheckUserExists();

                if (ValidAutoUserName == true && ValidLogin == true)
                {
                    waiting.Close();
                    if (DefaultUnitId == 0)
                    {
                        msgText = "Please Select Default Unit.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                        return;
                    }
                    else if (dgUserRoleList.SelectedItem == null || (((clsUserRoleVO)dgUserRoleList.SelectedItem).IsActive==false))
                    {
                        msgText = "Please Select User Role.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                        return;
                    }
                    else
                    {
                        msgText = "Are you sure you want to Save the Record";
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                        msgWindowUpdate.Show();
                    }
                }
                else if (ValidAutoUserName == false)
                    autoUserName.Focus();
                else if (ValidLogin == false)
                    txtLogin.Focus();
                else if (isFormValid == false)
                    txtExpirationDays.Focus();
                waiting.Close();
            }
            catch (Exception ex)
            {
                waiting.Close();       
            }            
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //waiting.Show();
                Save();
                //waiting.Close();
                ////if (LoginNameExists == false)
                ////{
                ////    SetCommandButtonState("Save");
                ////}
                ////else
                ////{
                ////    SetCommandButtonState("New");
                ////    FillUserList();
                ////}
                //SetCommandButtonState("New");
                
            }
            else
            {
            }
        }

        void Save()
        {
            try
            {
                clsUserVO objUser = CreateUserObjectFromFormData();
                clsAddUserBizActionVO objAddUserBizVO = new clsAddUserBizActionVO();
                objAddUserBizVO.Details = objUser;
                bool chkValidity=true;

                if (optDoctor.IsChecked.Value)
                {
                    if (objAddUserBizVO.Details.UserGeneralDetailVO.DoctorID == 0)
                    {
                        autoUserName.SetValidation("Doctor Does Not Exist for this Unit");
                        autoUserName.RaiseValidationError();
                        autoUserName.Text = "";
                        txtLogin.Text = "";
                        chkValidity = false;
                    }
                }

                else if (optEmployee.IsChecked.Value)
                {
                    if (objAddUserBizVO.Details.UserGeneralDetailVO.EmployeeID == 0)
                    {
                        autoUserName.SetValidation("Employee Does Not Exist for this Unit");
                        autoUserName.RaiseValidationError();
                        autoUserName.Text = "";
                        txtLogin.Text = "";
                        chkValidity = false;
                    }
                }
                else if (optPatient.IsChecked.Value)
                {
                    if (objAddUserBizVO.Details.UserGeneralDetailVO.PatientID == 0)
                    {
                        autoUserName.SetValidation("Patient Does Not Exist for this Unit");
                        autoUserName.RaiseValidationError();
                        autoUserName.Text = "";
                        txtLogin.Text = "";
                        chkValidity = false;
                        autoUserName.Focus();
                    }
                }

                if (chkValidity == true) 
                {
                    LoginNameExists = false;

                    int intResult = 0;
                    clsGetLoginNameBizActionVO objLogin = new clsGetLoginNameBizActionVO();
                    objLogin.LoginName = txtLogin.Text.ToString();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, ea) =>
                    {
                        if (ea.Error == null && ea.Result != null)
                        {
                            intResult = ((clsGetLoginNameBizActionVO)ea.Result).SuccessStatus;

                           
                                if (intResult != 0)
                                {
                                    LoginNameExists = true;
                                    ClearFormData();
                                    SetCommandButtonState("Save");
                                    FillUserList();
                                    msgText = "Login Name Already Exists. Please change the Login Name";

                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                    txtLogin.Text = "";
                                    txtLogin.Focus();
                                }
                                else
                                {
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

                                    client1.ProcessCompleted += (s1, ea1) =>
                                    {
                                       
                                        if ((ea1.Error == null) && (ea1.Result != null) && autoUserName.Text != "")
                                        {
                                            if (((clsAddUserBizActionVO)ea1.Result).SuccessStatus == -9)
                                            {
                                                msgText = "Can not save user : You can save maximum no of users specified while subscription";

                                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgWindow.Show();
                                              
                                            }
                                            else
                                            {
                                                FillUserList();
                                                ClearFormData();
                                                dgUserList.SelectedIndex = 0;

                                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                                                mElement.Text = "";

                                                SetCommandButtonState("New");
                                                _FlipAnimation.Invoke(RotationType.Backward);

                                                msgText = "Record is successfully submitted!";

                                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                msgWindow.Show();
                                                SendPasswordDetails();
                                            }
                                        }
                                        else
                                        {
                                            msgText = "Record cannot be added Please check the Details";

                                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgWindow.Show();
                                        }
                                    };
                                    client1.ProcessAsync(objAddUserBizVO, User);
                                    client1.CloseAsync();
                                    client1 = null;
                                }
                            }
                      
                    };
                    client.ProcessAsync(objLogin, User);
                    client.CloseAsync();
                    client = null;     
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
               
        private void SendPasswordDetails()
        {
            if (myAppConfig.EmailConfig.AutogeneratedPassword)
            {
                #region Get Email Template
                clsGetEmailTemplateBizActionVO obj = new clsGetEmailTemplateBizActionVO();
                obj.ID = myAppConfig.EmailConfig.AutoUserPassword;
                if (dgUserList.SelectedItem != null)
                {
                    if (UserEmailId != null)
                    {
                        UserEmailId = ((clsUserVO)dgUserList.SelectedItem).EmailId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, ea) =>
                        {
                            if (ea.Error == null && ea.Result != null)
                            {
                                clsEmailTemplateVO objTemp = new clsEmailTemplateVO();
                                objTemp = ((clsGetEmailTemplateBizActionVO)ea.Result).EmailDetails;

                                if (objTemp != null)
                                {
                                    string Subject, EmailText;

                                    Subject = objTemp.Subject;
                                    EmailText = objTemp.Text;

                                    #region Send Email

                                    string ClinicEmailId = myAppConfig.Email;
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
                                    EmailClient.SendEmailCompleted += (e, args) =>
                                    {
                                        if (args.Error == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Sent To the User Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            msgW1.Show();
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW1.Show();
                                        }
                                    };
                                    EmailClient.SendEmailAsync(ClinicEmailId, UserEmailId, Subject, EmailText);
                                    EmailClient.CloseAsync();

                                    #endregion
                                }
                                else
                                {
                                    msgText = "An Error Occured while Retrieving the Email Template.";

                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                }
                            }
                        };
                        client.ProcessAsync(obj, new clsUserVO());
                        client.CloseAsync();
                    }
                    else
                    {
                        msgText = "User Email Id not added. Could not send an Email to the User.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }

                }
            }
            #endregion
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            
            waiting.Show();
            flgModify = true;
            SetFormValidation();

            try
            {
                if (ValidAutoUserName == true && ValidLogin == true && isFormValid == true)
                {
                    waiting.Close();
                    if (DefaultUnitId == 0)
                    {                        
                        msgText = "Please Select Default Unit";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                        return;
                    }
                    else if (dgUserRoleList.SelectedItem == null)
                    {
                        msgText = "Please Select User Role";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                        return;
                    }
                    else
                    {
                        msgText = "Are you sure you want to Modify the Record";
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        msgWindowUpdate.Show();
                    }
                }
                else if (ValidAutoUserName == false)
                    autoUserName.Focus();
                else if (ValidLogin == false)
                    txtLogin.Focus();
                else if (isFormValid == false)
                    txtExpirationDays.Focus();
                waiting.Close();

            }
            catch (Exception ex)
            {
                waiting.Close();
            }
            finally
            {

            }
        }

        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //waiting.Show();
                Modify();
                //waiting.Close();
                //SetCommandButtonState("Modify");
                FillUserList();
            }
            else
            {
                if (dgUserList.SelectedItem != null)
                {
                    if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status == true)
                    {
                        ShowUserDetails(((clsUserVO)dgUserList.SelectedItem).ID, ((clsUserVO)dgUserList.SelectedItem).UserTypeName);
                    }
                    else
                    {
                        msgText = "User is disabled. Cannot view Details.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    
                }
                _FlipAnimation.Invoke(RotationType.Forward);
                //SetCommandButtonState("Modify");
                FillUserList();
            }
        }

        private void Modify()
        {
            try
            {
                clsUserVO objUser = CreateUserObjectFromFormData();

                clsAddUserBizActionVO objAddUserBizVO = new clsAddUserBizActionVO();
                objAddUserBizVO.Details = objUser;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if ((ea.Error == null) && (ea.Result != null))
                    {
                        if (((clsAddUserBizActionVO)ea.Result).SuccessStatus == 1)
                        {
                            msgText = "Login Name Already Exists. Please change the Login Name";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();

                        }

                        else
                        {

                            ClearFormData();
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = "";

                            _FlipAnimation.Invoke(RotationType.Backward);
                            msgText = "Record is Successfully Modified!";

                            //Clear Previous Unit & Srore Details"
                            List<clsUserUnitDetailsVO> lstUnit = new List<clsUserUnitDetailsVO>();
                            lstUnit = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                            foreach (var item in lstUnit)
                            {
                                item.IsDefault = false;
                                item.Status = false;
                            }
                            dgUnitList.ItemsSource = null;
                            dgUnitList.ItemsSource = lstUnit;

                            dgUnitStoreList.ItemsSource = null;
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            FillUserList();
                            SetCommandButtonState("New");
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    txtBLTotalUser.Visibility = Visibility.Collapsed;
                    txtTotalUser.Visibility = Visibility.Collapsed;
                };

                client.ProcessAsync(objAddUserBizVO, User); // new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
           

            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = " : " + ((clsUserVO)dgUserList.SelectedItem).LoginName; ;
            txtBLTotalUser.Visibility = Visibility.Collapsed;
            txtTotalUser.Visibility = Visibility.Collapsed;
            try
            {
                if (dgUserList.SelectedItem != null)
                {
                    if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status == true || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AdminRoleID==User.UserGeneralDetailVO.RoleDetails.ID)
                    {
                        waiting.Show();
                        UserName = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.UserName;
                      //  autoUserName.Text = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.UserName; 
                        ShowUserDetails(((clsUserVO)dgUserList.SelectedItem).ID, ((clsUserVO)dgUserList.SelectedItem).UserTypeName);
                        waiting.Close();
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.UserName;

                        _FlipAnimation.Invoke(RotationType.Forward);

                        //SetCommandButtonState("Modify");
                    }
                    else
                    {
                        msgText = "Can not view Details,User is disabled.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    SetCommandButtonState("Modify");
                }
                else
                {
                    msgText = "Cannot view the Details.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }

            }
            catch (Exception ex)
            {
                   throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //clsGetUserGeneralDetailsBizActionVO objGetUserGeneralDetails = new clsGetUserGeneralDetailsBizActionVO();
            //objGetUserGeneralDetails.InputPagingEnabled = true;
            //objGetUserGeneralDetails.InputStartRowIndex = 0;
            //objGetUserGeneralDetails.InputMaximumRows = 20;
            //objGetUserGeneralDetails.InputSortExpression = "Code Desc";
            //objGetUserGeneralDetails.InputSearchExpression = txtSearchCriteria.Text;

            //objGetUserGeneralDetails.Status = true;
            //client = new PalashServiceClient();
            //client.ProcessCompleted += (s, ea) =>
            //{
            //    PagedCollectionView pcv = new PagedCollectionView(((clsGetUserGeneralDetailsBizActionVO)ea.Result).UserGeneralDetailsList);
            //    pcv.PageSize = 20;
            //    DataContext = pcv;
            //};
            //client.ProcessAsync(objGetUserGeneralDetails, user);
            //client.CloseAsync();
         //   dgUserList.ItemsSource = null;
            dgDataPager.PageIndex = 0;
            FillUserList();
            
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FillUserList();
                ClearFormData();
                dgUserList.SelectedIndex = 0;

                List<clsUserUnitDetailsVO> lstUnit = new List<clsUserUnitDetailsVO>();
                lstUnit = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                foreach (var item in lstUnit)
                {
                        
                            item.IsDefault = false;
                            item.Status = false;
                       
                }               
                dgUnitList.ItemsSource = null;
                dgUnitList.ItemsSource = lstUnit;


                dgUnitStoreList.ItemsSource = null;

                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                //mElement.Text = "";

                _FlipAnimation.Invoke(RotationType.Backward);

                SetCommandButtonState("Cancel");

                txtBLTotalUser.Visibility = Visibility.Visible;
                txtTotalUser.Visibility = Visibility.Visible;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "List ";
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }  

        private void cmdUserRights_Click(object sender, RoutedEventArgs e)
        {
            if (dgUserList.SelectedItem != null)
            {
                frmUserRights user = new frmUserRights();
                user.OnCloseButton_Click += new RoutedEventHandler(user_OnCloseButton_Click);
                user.UserId = ((clsUserVO)dgUserList.SelectedItem).ID;
                user.Status = ((clsUserVO)dgUserList.SelectedItem).Status;
                user.UserName = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.UserName;
                if (user.Status == true)
                { 
                    user.GetUserRights(user.UserId);
                    user.Show();
                }
                else 
                {
                   // OKButton.IsEnabled = false;
                    MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Activate User First.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else 
            {
                msgText = "Please Select User From List.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }
        }

        void user_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            //throw new NotImplementedException();
        }

        #endregion

        private void cmbUserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                #region UserName
                if (UserTypeID == 1)
                {
                    clsComboMasterBizActionVO fam = new clsComboMasterBizActionVO();
                    fam = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                    tempStr = fam.Value;
                    UserEmailId = ((clsComboMasterBizActionVO)autoUserName.SelectedItem).EmailId;
                }
                else if (UserTypeID == 2)
                {
                    clsStaffMasterVO fam = new clsStaffMasterVO();
                    fam = (clsStaffMasterVO)autoUserName.SelectedItem;
                    UserEmailId = ((clsComboMasterBizActionVO)autoUserName.SelectedItem).EmailId;
                    tempStr = fam.Value;
                }

                string[] temparr = tempStr.Split(' ');

                tempStr = "";
                if (temparr.Length > 0)
                {
                    switch (temparr.Length)
                    {
                        case 1:
                            tempStr = temparr[0];
                            break;
                        case 2:
                            tempStr = temparr[0] + "." + temparr[1];
                            break;
                        case 3:
                            tempStr = temparr[0] + "." + temparr[2];
                            break;
                    }
                }
                txtLogin.Text = tempStr.ToLower();
                #endregion

                #region Password
                //clsComboMasterBizActionVO objPassword = new clsComboMasterBizActionVO();
                //objPassword = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                //string tempPass = objPassword.Value;
                //string[] tempPassArr = tempPass.Split(' ');
                //tempPass = "";
                //if (tempPassArr.Length > 0)
                //{
                //    switch (tempPassArr.Length)
                //    {
                //        case 1:
                //            tempPass = tempPassArr[0] + "123";
                //            break;
                //        case 2:
                //            tempPass = tempPassArr[1] + "123";
                //            break;
                //        case 3:
                //            tempPass = tempPassArr[2] + "123";
                //            break;
                //    }
                //}
                //txtPassword.Password = tempPass.ToLower();
                txtPassword.Password = "Palash";
                #endregion

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region MyRegion

        private clsUserVO CreateUserObjectFromFormData()
        {
            clsUserVO objUserVO = new clsUserVO();
            try
            {
                objUserVO.ID = UserId;
                objUserVO.LoginName = txtLogin.Text.Trim();
                objUserVO.Password = txtPassword.Password.Trim();
                objUserVO.UserGeneralDetailVO.EnablePasswordExpiration = chkExpirationDays.IsChecked.Value;
                // For Password Configuration.

                objUserVO.PassConfig.MinPasswordLength = Int16.Parse(txtPasswordMinLength.Text.Trim());
                objUserVO.PassConfig.MaxPasswordLength = Int16.Parse(txtPasswordMaxLength.Text.Trim());
                objUserVO.PassConfig.MinPasswordAge = Int16.Parse(txtMinPasswordAge.Text.Trim());
                objUserVO.PassConfig.MaxPasswordAge = Int16.Parse(txtMaxPasswordAge.Text.Trim());
                objUserVO.PassConfig.NoOfPasswordsToRemember = Int16.Parse(txtPasswordRemember.Text.Trim());
                objUserVO.PassConfig.AtLeastOneDigit = (bool)chkAtleastOneDigit.IsChecked;
                objUserVO.PassConfig.AtLeastOneLowerCaseChar = (bool)chkAtleastOneLower.IsChecked;
                objUserVO.PassConfig.AtLeastOneUpperCaseChar = (bool)chkAtleasrOneUpper.IsChecked;
                objUserVO.PassConfig.AtLeastOneSpecialChar = (bool)chkAtleastOneSpecial.IsChecked;
                objUserVO.PassConfig.AccountLockThreshold = Int16.Parse(txtAccountLockThreshold.Text.Trim());
                objUserVO.PassConfig.AccountLockDuration = float.Parse(txtAccountLockDuration.Text.Trim());

                // Password Configuration.
                if (objUserVO.UserGeneralDetailVO.EnablePasswordExpiration == false)
                    objUserVO.UserGeneralDetailVO.PasswordExpirationInterval = 0;
                else
                    objUserVO.UserGeneralDetailVO.PasswordExpirationInterval = int.Parse(txtExpirationDays.Text.Trim());

                objUserVO.UserType = 0;
                objUserVO.UserGeneralDetailVO.DoctorID = 0;
                objUserVO.UserGeneralDetailVO.IsDoctor = false;

                objUserVO.UserGeneralDetailVO.EmployeeID = 0;
                objUserVO.UserGeneralDetailVO.IsEmployee = false;

                objUserVO.UserGeneralDetailVO.PatientID = 0;
                objUserVO.UserGeneralDetailVO.IsPatient = false;

                if (optDoctor.IsChecked == true)
                {
                    if (flgModify == true)
                    {
                        objUserVO.UserType = 1;

                        objUserVO.UserGeneralDetailVO.DoctorID = RetrievedUserId; //UserTypeID;
                    }
                    else
                    {
                        clsComboMasterBizActionVO fam = new clsComboMasterBizActionVO();
                        fam = (clsComboMasterBizActionVO)autoUserName.SelectedItem;

                        objUserVO.UserType = 1;
                        objUserVO.UserGeneralDetailVO.DoctorID = fam.ID;  //UserTypeID;
                    }

                    objUserVO.UserGeneralDetailVO.IsDoctor = true;
                }
                else if (optEmployee.IsChecked == true)
                {
                    if (flgModify == true)
                    {
                        objUserVO.UserType = 2;
                        objUserVO.UserGeneralDetailVO.EmployeeID = RetrievedUserId; //UserTypeID;
                    }
                    else
                    {
                        clsStaffMasterVO fam = new clsStaffMasterVO();
                        fam = (clsStaffMasterVO)autoUserName.SelectedItem;

                        objUserVO.UserType = 2;
                        objUserVO.UserGeneralDetailVO.EmployeeID = fam.ID; //UserTypeID;
                    }

                    objUserVO.UserGeneralDetailVO.IsEmployee = true;
                }
                else if (optPatient.IsChecked == true)
                {
                    objUserVO.UserType = 3;
                    objUserVO.UserGeneralDetailVO.PatientID = UserTypeID;
                    objUserVO.UserGeneralDetailVO.IsPatient = true;
                    //objUserVO.UserGeneralDetailVO
                }
                
                objUserVO.UserGeneralDetailVO.UnitDetails = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                               
                clsUserRoleVO ObjRole = new clsUserRoleVO();
                if (dgUserRoleList.ItemsSource != null && dgUserRoleList.SelectedItem != null)
                    ObjRole = (clsUserRoleVO)dgUserRoleList.SelectedItem;

                if (objSelectedRole != null)
                    ObjRole.DashBoardList = objSelectedRole.DashBoardList;


                List<clsMenuVO> olist = new List<clsMenuVO>();
                List<clsMenuVO> list = trvMenu.ItemsSource as List<clsMenuVO>;
                foreach (var item in list)
                {
                    LoadFlatList(olist, item);
                }
                ObjRole.MenuList = olist.ToList();

                objUserVO.UserGeneralDetailVO.RoleDetails = ObjRole;
            }
            catch (Exception ex)
            {
                   throw;
            }
            return objUserVO;
        }

        //private void LoadFlatList(List<clsMenuVO> olist, clsMenuVO item)
        //{
        //    if (item.Status == true && item.Mode != "ACCESSLEVEL")
        //    {
        //        olist.Add(new clsMenuVO() { ID = item.ID, Header = item.Header, Active = item.Status.Value, Status = item.Status });
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
        //                            olist[olist.Count - 1].IsCreate = citem.Status.Value;
        //                            break;

        //                        case 2:
        //                            olist[olist.Count - 1].IsRead = citem.Status.Value;
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
            //if (item.Status == true)
            //{
                olist.Add(new clsMenuVO() { ID = item.ID, Header = item.Header, Active = item.Status.Value, Status = item.Status });
            //}
            //else
            //{
            //    olist.Add(new clsMenuVO() { ID = item.ID, Header = item.Header, Active = item.Status.Value, Status = item.Status });
            //}
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

        WaitIndicator wait = new WaitIndicator();
        void ShowUserDetails(long lUserID, string lUserType)
        {
            wait.Show();
            try
            {
                isEditMode = true;
                EditStore = true;
                UserId = lUserID;
                clsGetUserBizActionVO objBizVO = new clsGetUserBizActionVO();
                clsUserVO objDetailsVO = new clsUserVO();
                objBizVO.Details = objDetailsVO;
                objBizVO.ID = lUserID;
                //grbUnitStore.Visibility = Visibility.Collapsed;
                objBizVO.Details.UserTypeName = lUserType;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AdminRoleID == User.UserGeneralDetailVO.RoleDetails.ID)  //By Umesh
                {
                    objBizVO.FlagDisableUser = true;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {

                        if (dgUserList.SelectedItem != null)
                            _FlipAnimation.Invoke(RotationType.Forward);
                        TabUser.SelectedIndex = 0;

                        clsUserVO objUser = new clsUserVO();
                        objUser = ((clsGetUserBizActionVO)ea.Result).Details;

                        txtLogin.Text = objUser.LoginName;

                        txtPassword.IsEnabled = false;
                        txtPassword.Password = objUser.Password;

                        AutoPassword.Text = objUser.Password;

                        autoUserName.Text = UserName;
                        autoUserName.IsEnabled = false;
                       
                        grbUserType.IsEnabled = false;
                        ///Password Configuration.
                        txtPasswordMinLength.Text = objUser.PassConfig.MinPasswordLength.ToString();
                        txtPasswordMaxLength.Text = objUser.PassConfig.MaxPasswordLength.ToString();
                        txtMinPasswordAge.Text = objUser.PassConfig.MinPasswordAge.ToString();
                        txtMaxPasswordAge.Text = objUser.PassConfig.MaxPasswordAge.ToString();
                        txtPasswordRemember.Text = objUser.PassConfig.NoOfPasswordsToRemember.ToString();
                        txtAccountLockDuration.Text = objUser.PassConfig.AccountLockDuration.ToString();
                        txtAccountLockThreshold.Text = objUser.PassConfig.AccountLockThreshold.ToString();
                        chkAtleastOneDigit.IsChecked = objUser.PassConfig.AtLeastOneDigit;
                        chkAtleastOneLower.IsChecked = objUser.PassConfig.AtLeastOneLowerCaseChar;
                        chkAtleasrOneUpper.IsChecked = objUser.PassConfig.AtLeastOneUpperCaseChar;
                        chkAtleastOneSpecial.IsChecked = objUser.PassConfig.AtLeastOneSpecialChar;

                        //FillPassConfig();

                        if (objUser.UserType == 1)
                        {
                            RetrievedUserId = objUser.UserGeneralDetailVO.DoctorID;
                            UserTypeID = objUser.UserType; //objUser.UserGeneralDetailVO.DoctorID;
                            optDoctor.IsChecked = true;
                        }
                        else if (objUser.UserType == 2)
                        {
                            RetrievedUserId = objUser.UserGeneralDetailVO.EmployeeID;
                            UserTypeID = objUser.UserType; //objUser.UserGeneralDetailVO.EmployeeID;
                            optEmployee.IsChecked = true;
                        }
                        else if (objUser.UserType == 3)
                        {
                            UserTypeID = objUser.UserGeneralDetailVO.PatientID;
                            optPatient.IsChecked = true;
                        }

                        chkExpirationDays.IsChecked = objUser.UserGeneralDetailVO.EnablePasswordExpiration;

                        txtExpirationDays.Text = objUser.UserGeneralDetailVO.PasswordExpirationInterval.ToString();

                        if (chkExpirationDays.IsChecked == true)
                            txtExpirationDays.IsReadOnly = false;

                        List<clsUserRoleVO> lstRole = new List<clsUserRoleVO>();
                        lstRole = (List<clsUserRoleVO>)dgUserRoleList.ItemsSource;
                        foreach (var item in lstRole)
                        {
                            if (item.ID == objUser.UserGeneralDetailVO.RoleDetails.ID)
                            {
                                dgUserRoleList.SelectedIndex = 0;
                                dgUserRoleList.SelectedItem = item;
                                break;
                            }
                        }

                        if (objUser.UserGeneralDetailVO.RoleDetails.MenuList != null)
                        {
                            foreach (var smitem in objUser.UserGeneralDetailVO.RoleDetails.MenuList)
                            {
                                if (trvMenu.ItemsSource != null)
                                {
                                    foreach (clsMenuVO item in trvMenu.ItemsSource)
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
                       

                        if (objUser.UserGeneralDetailVO.UnitDetails != null)
                        {
                            List<clsUserUnitDetailsVO> lstUnit = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;

                            foreach (var item1 in objUser.UserGeneralDetailVO.UnitDetails)
                            {
                                foreach (var item in lstUnit)
                                {

                                    if (item1.Status==true)
                                    {
                                        SelectedUnitId = item1.ID;
                                        FillUnitStoreFirst();
                                       
                                    }
                                   
                                }
                            }
                            
                            //if (SelectedUnitId > 0)
                            //{
                            //    foreach (var item in lstUnit)
                            //    {
                            //        if (item.UnitID == SelectedUnitId)
                            //        {
                            //            dgUnitList.SelectedItem = item;
                            //            break;
                            //        }
                                     
                            //    }
                               
                            //}

                            foreach (var item1 in objUser.UserGeneralDetailVO.UnitDetails)
                            {
                                foreach (var item in lstUnit)
                                {
                                    if (item.UnitID == item1.UnitID)
                                    {
                                        item.IsDefault = item1.IsDefault;
                                        item.Status = item1.Status;
                                        if (item.IsDefault == true)
                                            DefaultUnitId = item.UnitID;
                                    }
                                }
                            }
                            dgUnitList.ItemsSource = null;
                            dgUnitList.ItemsSource = lstUnit;
                        }

             
                      
                    }
                    wait.Close();
                };
                
                client.ProcessAsync(objBizVO, User);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                  throw;
                  wait.Close();
            }
        }

        List<MasterListItem> chklist = null;

        void client_GetUserOtherDetailProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            //clsGetUserOtherDetailsByIdBizActionVO objUserOtherDetails = e.Result as clsGetUserOtherDetailsByIdBizActionVO;

            //cmbStore.SelectedValue = (long)objUserOtherDetails.UserOtherDetails.DefaultStore;
            //DefaultCashCounterId = (long)objUserOtherDetails.UserOtherDetails.DefaultCashCounter;
            //DefaultUnitId = (long)objUserOtherDetails.UserOtherDetails.DefaultUnit;
            //txtPassword.Password = objUserOtherDetails.UserOtherDetails.Password;
            //txtConfirmPassword.Password = objUserOtherDetails.UserOtherDetails.Password;
            //chkFirstLevel.IsChecked = objUserOtherDetails.UserOtherDetails.FirstLevel;
            //chkSecondLevel.IsChecked = objUserOtherDetails.UserOtherDetails.SecondLevel;
            //chkThirdLevel.IsChecked = objUserOtherDetails.UserOtherDetails.ThirdLevel;
            //chkCheckRight.IsChecked = objUserOtherDetails.UserOtherDetails.CheckRight;
            //if (objUserOtherDetails.UserOtherDetails.PwdExpirationDays > 0)
            //{
            //    chkExpirationDays.IsChecked = true;
            //    txtExpirationDays.Text = objUserOtherDetails.UserOtherDetails.PwdExpirationDays.ToString();
            //    txtExpirationDays.IsReadOnly = false;
            //}
            //else
            //{
            //    chkExpirationDays.IsChecked = false;
            //    txtExpirationDays.Text = string.Empty;
            //    txtExpirationDays.IsReadOnly = true;
            //}

            //foreach (var sitem in objUserOtherDetails.UserOtherDetails.UserUnitCashCounterList)
            //{
            //    foreach (var item in ((List<clsUnitGeneralDetailVO>)trvUnit.ItemsSource))
            //    {
            //        if (item.UnitId == sitem.UnitId)
            //        {
            //            item.Status = sitem.Status;
            //            CheckChildItems(item.CashCounterList, sitem);
            //        }
            //    }
            //}


            //switch ((PermissionType)objUserOtherDetails.UserOtherDetails.PermissionType)
            //{
            //    case PermissionType.GroupPermission:
            //        optGroupPermissions.IsChecked = true;
            //        chklist = objUserOtherDetails.UserOtherDetails.UserGroupDetailsList;


            //        break;
            //    case PermissionType.RolePermission:
            //        optRolePermissions.IsChecked = true;

            //        chklist = objUserOtherDetails.UserOtherDetails.UserRoleDetailsList;

            //        break;
            //    case PermissionType.CustomPermission:
            //        optCustomPermissions.IsChecked = true;
            //        break;
            //}



            //foreach (var smitem in objUserOtherDetails.UserOtherDetails.UserMenuDetailsList)
            //{
            //    foreach (var item in ((List<clsMenuVO>)trvMenu.ItemsSource))
            //    {
            //        if (item.MenuId == smitem.MenuId)
            //        {
            //            item.Status = smitem.Status;
            //            break;
            //        }
            //        else
            //        {
            //            if (item.ChildMenuList.Count > 0)
            //            {
            //                if (CheckMenuChildItems(item.ChildMenuList, smitem))
            //                    break;
            //            }
            //        }

            //    }

            //}
        }

        private bool CheckMenuChildItems(List<clsMenuVO> MenuList, clsUserMenuDetailsVO sMenuItem)
        {
            foreach (var citem in MenuList)
            {
                if (citem.ID == sMenuItem.MenuId)
                {
                    citem.Status = sMenuItem.Status;
                    return true;
                }
                else
                {
                    if (citem.ChildMenuList.Count > 0)
                    {
                        CheckMenuChildItems(citem.ChildMenuList, sMenuItem);
                    }
                }
            }
            return false;
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    CmdUserCategoryLink.IsEnabled = true;
                    cmdUserRights.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                case "Save":
                    CmdUserCategoryLink.IsEnabled = false;
                    cmdUserRights.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    CmdUserCategoryLink.IsEnabled = false;
                    cmdUserRights.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status == false)
                    {
                        cmdModify.IsEnabled = false;
                    }
                    break;
                case "Cancel":
                    CmdUserCategoryLink.IsEnabled = true;
                    cmdUserRights.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                //case "User Rights":
                //    cmdUserRights.IsEnabled = true;
                //    cmdNew.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdModify.IsEnabled = false;
                //    cmdCancel.IsEnabled = false;
                //    break;
                default:
                    break;

                //case "New":
                //    cmdNew.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdModify.IsEnabled = false;
                //    cmdCancel.IsEnabled = false;
                //    break;
                //case "Save":
                //    cmdNew.IsEnabled = false;
                //    cmdSave.IsEnabled = true;
                //    cmdModify.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    break;
                //case "Modify":
                //    cmdNew.IsEnabled = false;
                //    cmdSave.IsEnabled = false;
                //    cmdModify.IsEnabled = true;
                //    cmdCancel.IsEnabled = true;
                //    break;
                //case "Cancel":
                //    cmdNew.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdModify.IsEnabled = false;
                //    cmdCancel.IsEnabled = false;
                //    break;
                //default:
                //    break;
            }
        }

        #endregion

        #region  Reset All Controls
        private void ClearFormData()
        {
            try
            {
                isEditMode = false;
                EditStore = false;
                txtLogin.Text = string.Empty;
                txtPassword.Password = string.Empty;
              //  autoUserName.Text = string.Empty;
                AutoPassword.Text = string.Empty;
                DefaultUnitId = 0;
                UserId = 0;
                chkExpirationDays.IsChecked = false;
                txtExpirationDays.Text = string.Empty;
                txtExpirationDays.IsReadOnly = true;

                List<clsUserUnitDetailsVO> lList = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
               // clsUserUnitDetailsVO selItem = null;
                foreach (var item in lList)
                {
                    item.Status = false;
                    item.IsDefault = false;
                    if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    {
                        item.Status = true;
                        //selItem = item;
                    }

                }
                //lList[0].Status = true;
                dgUnitList.ItemsSource = null;
                dgUnitList.ItemsSource = lList;
                dgUnitList.SelectedIndex = 0;
                //dgUnitList.SelectedItem = item;

                //List<clsUserUnitDetailsVO> lList = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                List<clsItemStoreVO> lStoreList = (List<clsItemStoreVO>)dgUnitStoreList.ItemsSource;

               lStoreList = lList[0].StoreDetails;
                foreach (var item in lStoreList)
                {
                    item.StoreStatus = false;
                    //item.IsDefault = false;
                    //item.UnitID == 
                }
                dgUnitStoreList.ItemsSource = null;
                dgUnitStoreList.ItemsSource = lStoreList;
                //dgUnitStoreList.SelectedIndex = 0;

              //  FillUnitStoreFirst();

                dgUserRoleList.SelectedIndex = -1;
                dgUserList.SelectedIndex = -1;
                if (dgUnitList.ItemsSource != null)
                    FillMenuList();
                else
                {
                    List<clsMenuVO> tempList = new List<clsMenuVO>();
                    tempList = (List<clsMenuVO>)trvMenu.ItemsSource;
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        ClearMenuStatus(tempList[i]);
                    }
                    trvMenu.ItemsSource = null;
                    trvMenu.ItemsSource = tempList;
                }
                txtLogin.ClearValidationError();
                autoUserName.ClearValidationError();
                txtPassword.ClearValidationError();
                txtExpirationDays.ClearValidationError();
                FillPassConfig();
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        #endregion  Reset All Controls

        private void FillPassConfig()
        {
            try
            {
                clsGetPassConfigBizActionVO obj = new clsGetPassConfigBizActionVO();
                if (obj.PassConfig == null) obj.PassConfig = new clsPassConfigurationVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsPassConfigurationVO objPwd = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig;
                        DataContext = objPwd;
                    }
                    else
                    {
                        //msgText = "Error While Reading Password Configuration";
                        //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgWindow.Show();
                    }
                };
                Client.ProcessAsync(obj, new clsUserVO());
                Client.CloseAsync();
                Client = null;
            }
            catch (Exception)
            {

                //throw;
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
        List<clsMenuVO> selectedMenu;
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

        private void chkLocked_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateUserLockedStatusBizActionVO obj = new clsUpdateUserLockedStatusBizActionVO();
            obj.UserLockedStatus = new clsUserVO();
            obj.UserLockedStatus = ((clsUserVO)dgUserList.SelectedItem);
          
            if (((clsUserVO)dgUserList.SelectedItem).LoginName != "sa")
            {
                obj.UserLockedStatus.UserGeneralDetailVO.Locked = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Locked;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if ((ea.Error == null) && (ea.Result != null))
                    {
                        if (obj.UserLockedStatus.UserGeneralDetailVO.Locked == false)
                        {
                            msgText = "User Unlocked Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "User Locked Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        string msgText = "An Error Occured";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                 //   SetCommandButtonState("Modify");
                    SetCommandButtonState("New");
                };
                client.ProcessAsync(obj, User);
                client.CloseAsync();
                client = null;
            }
            else
            {
                if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Locked == true)
                {
                    msgText = "System Administrator cannot be Locked.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgWindow.Show();
                }
                else
                {
                    obj.UserLockedStatus.UserGeneralDetailVO.Locked = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Locked;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, ea) =>
                    {
                        if ((ea.Error == null) && (ea.Result != null))
                        {
                            if (obj.UserLockedStatus.UserGeneralDetailVO.Locked == false)
                            {
                                msgText = "User Unlocked Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }                        
                        }
                        else
                        {
                            string msgText = "An Error Occured";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    };
                    client.ProcessAsync(obj, User);
                    client.CloseAsync();
                    client = null;
                }
            }

        }

        private void opt_Clicked(object sender, RoutedEventArgs e)
        {
            UserTypeID = 1;
            
            FillUserTypeWiseUserList();         
        }

        #region FillEmployeeDoctorList
        void FillUserTypeWiseUserList()
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            if(UserTypeID==1)
            {
                clsGetDoctorMasterListBizActionVO BizAction = new clsGetDoctorMasterListBizActionVO();
                BizAction.ID = 0;
                BizAction.IsDecode = false;
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetDoctorMasterListBizActionVO objGetList = e.Result as clsGetDoctorMasterListBizActionVO;
                        if (BackPanel.Visibility == Visibility.Visible && newUser==true)
                        {
                            autoUserName.Text = "";
                            txtLogin.Text = string.Empty;
                        }                      
                        
                        autoUserName.ItemsSource = null;
                        autoUserName.ItemsSource = objGetList.ComboList;
                        autoUserName.ItemFilter = (search, item) =>
                        {
                            clsComboMasterBizActionVO fam = item as clsComboMasterBizActionVO;
                            if (fam != null)
                            {
                                search = search.ToUpper();
                                return (fam.Value.ToUpper().StartsWith(search));
                            }
                            else
                            {
                                return false;
                            }
                        };
                    }
                };

                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            #region for Patient

            else if (optPatient.IsChecked.Value)
            {
                clsGetPatientMasterListBizActionVO BizAction = new clsGetPatientMasterListBizActionVO();
                BizAction.ID = 0;
                BizAction.IsDecode = true;
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetPatientMasterListBizActionVO objGetList = e.Result as clsGetPatientMasterListBizActionVO;
                        clsComboMasterBizActionVO Default = new clsComboMasterBizActionVO() { ID = 0, Value = "-- Select --" };
                        objGetList.ComboList.Insert(0, Default);
                        if (BackPanel.Visibility == Visibility.Visible && newUser == true)
                        {
                            autoUserName.Text = "";
                            txtLogin.Text = string.Empty;
                        }                      
                                               
                        autoUserName.ItemsSource = null;
                        autoUserName.ItemsSource = objGetList.ComboList;
                        autoUserName.ItemFilter = (search, item) =>
                        {
                            clsComboMasterBizActionVO fam = item as clsComboMasterBizActionVO;
                            if (fam != null)
                            {
                                search = search.ToUpper();
                                return (fam.Value.ToUpper().StartsWith(search));
                            }
                            else
                            {
                                return false;
                            }
                        };
                    }
                };

                client.ProcessAsync(BizAction, User); 
                client.CloseAsync();
            }
            #endregion
           // else if (optEmployee.IsChecked.Value)
           // else if(optEmployee.IsChecked==true)
            else if(UserTypeID==2) 
            {
                clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
                BizAction.ID = 0;
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO objGetList = e.Result as clsGetStaffMasterDetailsBizActionVO;
                        if (BackPanel.Visibility == Visibility.Visible && newUser == true)
                        {
                            autoUserName.Text = "";
                            txtLogin.Text = string.Empty;
                        }                      
                        
                        autoUserName.ItemsSource = null;
                        autoUserName.ItemsSource = objGetList.StaffMasterList;
                        autoUserName.ItemFilter = (search, item) =>
                        {
                            clsStaffMasterVO fam = item as clsStaffMasterVO;
                            if (fam != null)
                            {
                                search = search.ToUpper();
                                return (fam.Value.ToUpper().StartsWith(search));
                            }
                            else
                            {
                                return false;
                            }
                        };
                    }
                };
                client.ProcessAsync(BizAction, User); 
                client.CloseAsync();               
            }
        }
        #endregion

        private void chkIsDefault_Checked(object sender, RoutedEventArgs e)
        {
                       
        }

        void ShowRoleDetails()
        {
            objSelectedRole = null;
          
            List<clsMenuVO> tempList = (List<clsMenuVO>)trvMenu.ItemsSource;
            for (int i = 0; i < tempList.Count; i++)
            {
                ClearMenuStatus(tempList[i]);
            }
            trvMenu.ItemsSource = null;
            trvMenu.ItemsSource = tempList;

            clsUserRoleVO objRole = (clsUserRoleVO)dgUserRoleList.SelectedItem;
            clsGetSelectedRoleMenuIdBizActionVO objRoleMenuDetails = new clsGetSelectedRoleMenuIdBizActionVO();
            objRoleMenuDetails.RoleId = objRole.ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_ShowRoleDetailsProcessCompleted);
            client.ProcessAsync(objRoleMenuDetails, User); //user);
            client.CloseAsync();
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

        void client_ShowRoleDetailsProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            // clsGetUserOtherDetailsByIdBizActionVO objUserOtherDetails = e.Result as clsGetUserOtherDetailsByIdBizActionVO;
            if (e.Error == null && e.Result != null)
            {
                clsGetSelectedRoleMenuIdBizActionVO objRoleMenuDetails = e.Result as clsGetSelectedRoleMenuIdBizActionVO;

                objSelectedRole = objRoleMenuDetails;
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

        private void checkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void chkIsDefault_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnitList.SelectedItem != null)
            {
                objVo = (clsUserUnitDetailsVO)dgUnitList.SelectedItem;
                DefaultUnitId = 0;

                clsUserUnitDetailsVO selItem = new clsUserUnitDetailsVO();
                List<clsUserUnitDetailsVO> uList = new List<clsUserUnitDetailsVO>();

                uList = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                selItem = (clsUserUnitDetailsVO)dgUnitList.SelectedItem;

                if (selItem.IsDefault == true)
                {
                    if (dgUnitStoreList.ItemsSource != null)
                    {
                        cmdSetStore.IsEnabled = true;
                    }
                    foreach (var item in uList)
                    {
                        if (item == selItem)
                        {
                            item.Status = true;
                            DefaultUnitId = item.UnitID;
                        }
                        else
                        {
                            item.IsDefault = false;
                        }
                    }
                    dgUnitList.ItemsSource = null;
                    dgUnitList.ItemsSource = uList;
                    dgUnitList.SelectedItem = selItem;
                }
                else
                    cmdSetStore.IsEnabled = false;
            }
        }

        #region For chkStatus old.
        List<clsUserUnitDetailsVO> UnitListOnApplyStore = new List<clsUserUnitDetailsVO>();
        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnitList.SelectedItem != null)
            {
                clsUserUnitDetailsVO selItem = new clsUserUnitDetailsVO();
                List<clsUserUnitDetailsVO> uList = new List<clsUserUnitDetailsVO>();

                uList = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                selItem = (clsUserUnitDetailsVO)dgUnitList.SelectedItem;
                if (selItem.Status == false)
                {
                    foreach (var item in uList)
                    {
                        if (item == selItem)
                        {
                            item.IsDefault = false;
                        }
                    }

                    dgUnitList.ItemsSource = null;
                    dgUnitList.ItemsSource = uList;
                }
                else
                {
                    //hlbUnitStore_Click(sender, e); 
                }

                //By Umesh
                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                {
                    UnitListOnApplyStore.Add(selItem);
                }
                else
                {
                    UnitListOnApplyStore.Remove(selItem);
                }


                //
            }
        }
        #endregion

        private void dgUserRoleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUserRoleList.SelectedItem != null)
            {
                if (isEditMode == true)
                    isEditMode = false;
                else
                    if (((clsUserRoleVO)dgUserRoleList.SelectedItem).IsActive == true)
                    {
                        ShowRoleDetails();
                    }
                    else
                    {
                        //(clsUserRoleVO)dgUserRoleList.SelectedItem = ((clsUserVO)dgUserList.SelectedItem).RoleName;
                        msgText = "Selected Role Is Deactivated So It cannot be Assigned.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                    }
            }
        }
        #region Lost Focus and Text Changed.

        private void autoUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (optDoctor.IsChecked.Value)
                {
                    clsComboMasterBizActionVO BizAction = new clsComboMasterBizActionVO();
                    BizAction = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                    if (BizAction != null && autoUserName.Text.Trim() != null)
                    {

                        if (BizAction.ID == 0 && autoUserName.Text.Trim() != null)
                        {
                            
                            autoUserName.SetValidation("Doctor Name Does Not Exists.");
                            autoUserName.RaiseValidationError();
                            autoUserName.Text = "";
                            autoUserName.Focus();
                        }
                        else
                        {
                            UserEmailId = BizAction.EmailId;
                            autoUserName.ClearValidationError();
                        }
                            autoUserName.ClearValidationError();
                        GenerateLoginDetails();
                    }
                    else if(BizAction==null && !string.IsNullOrEmpty(autoUserName.Text))
                    {
                        msgText = "User Name Does not Exist.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                    }

                }
                else if (optEmployee.IsChecked.Value)
                {
                    clsStaffMasterVO BizAction = new clsStaffMasterVO();
                    BizAction = (clsStaffMasterVO)autoUserName.SelectedItem;
                    if (BizAction != null)
                    {
                        if (BizAction.ID == 0)
                        {
                            //UserEmailId = BizAction.EmailId;
                            autoUserName.SetValidation("Employee Does not Exists");
                            autoUserName.RaiseValidationError();
                            autoUserName.Text = "";
                            autoUserName.Focus();
                        }
                        else
                        {
                            autoUserName.ClearValidationError();
                            UserEmailId = BizAction.EmailId;
                        }                            
                        GenerateLoginDetails();
                    }
                    else if(BizAction==null && !string.IsNullOrEmpty(autoUserName.Text))
                    {
                        msgText = "User Name Does not Exist.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgWindow.Show();
                    }
                }
                if (optPatient.IsChecked.Value)
                {
                    clsComboMasterBizActionVO BizAction = new clsComboMasterBizActionVO();
                    BizAction = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                    if (BizAction != null)
                    {
                        if (BizAction.ID == 0)
                        {
                            autoUserName.SetValidation("Patient Does Not Exists");
                            autoUserName.RaiseValidationError();
                            autoUserName.Text = "";
                            autoUserName.Focus();
                        }
                        else
                            autoUserName.ClearValidationError();
                        GenerateLoginDetails();
                    }
                    else if (BizAction == null && !string.IsNullOrEmpty(autoUserName.Text))
                    {
                        msgText = "User Name Does not Exist.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }

                }
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        void GenerateLoginDetails()
        {
            try
            {
                #region UserName

                if (UserTypeID == 1)
                {
                    clsComboMasterBizActionVO fam = new clsComboMasterBizActionVO();
                    fam = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                    tempStr = fam.Value;
                }
                else if (UserTypeID == 2)
                {
                    clsStaffMasterVO fam = new clsStaffMasterVO();
                    fam = (clsStaffMasterVO)autoUserName.SelectedItem;
                    tempStr = fam.Value;
                }
                else if (UserTypeID == 3)
                {
                    //for Patient
                }

                string[] temparr = tempStr.Split(' ');

                tempStr = "";
                if (temparr.Length > 0)
                {
                    switch (temparr.Length)
                    {
                        case 1:
                            tempStr = temparr[0];
                            break;
                        case 2:
                            tempStr = temparr[0] + "." + temparr[1];
                            break;
                        case 3:
                            tempStr = temparr[0] + "." + temparr[2];
                            break;
                    }
                }
                txtLogin.Text = tempStr.ToLower();
                #endregion

                #region Password

                //if (UserTypeID == 1)
                //{
                //    clsComboMasterBizActionVO objPassword = new clsComboMasterBizActionVO();
                //    objPassword = (clsComboMasterBizActionVO)autoUserName.SelectedItem;
                //    tempPass = objPassword.Value;
                //}

                //else if (UserTypeID == 2)
                //{
                //    clsStaffMasterVO objPassword = new clsStaffMasterVO();
                //    objPassword = (clsStaffMasterVO)autoUserName.SelectedItem;
                //    tempPass = objPassword.Value;
                //}
                //else if (UserTypeID == 3)
                //{
                //    //for Patient
                //}

                //string[] tempPassArr = tempPass.Split(' ');
                //tempPass = "";
                //if (tempPassArr.Length > 0)
                //{
                //    switch (tempPassArr.Length)
                //    {
                //        case 1:
                //            tempPass = tempPassArr[0];// + "123";
                //            break;
                //        case 2:
                //            tempPass = tempPassArr[1];// + "123";
                //            break;
                //        case 3:
                //            tempPass = tempPassArr[2];// + "123";
                //            break;
                //    }
                //}
                //txtPassword.Password = tempPass.ToLower();
                txtPassword.Password = "Palash";
                AutoPassword.Text = txtPassword.Password;
                #endregion
              
            }
            catch (Exception ex)
            {
            }
        }

        private void UpdateUserStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateUserStatusBizActionVO obj = new clsUpdateUserStatusBizActionVO();
            obj.UserStatus = new clsUserVO();
            obj.UserStatus = ((clsUserVO)dgUserList.SelectedItem);
            if (((clsUserVO)dgUserList.SelectedItem).LoginName != "sa")
            {
                if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Locked == true)
                {
                    msgText = "User is Locked. Please unlock the user first.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status == true)
                        ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status = false;
                    else
                        ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status = true;
                }
                else
                {
                    obj.UserStatus.Status = ((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, ea) =>
                    {
                        if ((ea.Error == null) && (ea.Result != null))
                        {
                            if (obj.UserStatus.Status == false)
                            {
                                cmdNew.IsEnabled = true;
                                cmdModify.IsEnabled = false;
                                msgText = "Status Updated Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                            else
                            {
                                msgText = "Status Updated Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                        }
                        else
                        {
                            msgText = "An Error Occured";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                      //  SetCommandButtonState("Modify");
                    };
                    client.ProcessAsync(obj, User); 
                    client.CloseAsync();
                    client = null;
                }
               
            }
            else
            {
                msgText = "System Administrator cannot be Deactivated.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
            }
        }

        private void txtPasswordMinLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPasswordMinLength.Text.IsItNumber())
            {
                txtPasswordMinLength.Text = "";
                txtPasswordMinLength.Focus();
                chkNullMinPassLen = false;
            }
            else if (txtPasswordMinLength.Text.Length == 0)
                chkNullMinPassLen = false;

            else
                chkNullMinPassLen = true;
        }

        private void txtPasswordMaxLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPasswordMaxLength.Text.IsItNumber())
            {
                txtPasswordMaxLength.Text = "";
                txtPasswordMaxLength.Focus();
                chkNullMaxPassLen = false;
            }
            else if (txtPasswordMaxLength.Text.Length == 0)
            {
                chkNullMaxPassLen = false;
            }
            else
                chkNullMaxPassLen = true;
        }

        private void txtMinPasswordAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMinPasswordAge.Text.IsItNumber())
            {
                txtMinPasswordAge.Text = "";
                txtMinPasswordAge.Focus();
                chkNullMinPassAge = false;
            }
            else if (txtMinPasswordAge.Text.Length == 0)
            {
                chkNullMinPassAge = false;
            }
            else
                chkNullMinPassAge = true;
        }

        private void txtMaxPasswordAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMaxPasswordAge.Text.IsItNumber())
            {
                txtMaxPasswordAge.Text = "";
                txtMaxPasswordAge.Focus();
                chkNullMaxPassAge = false;
            }
            else if (txtMaxPasswordAge.Text.Length == 0)
            {
                chkNullMaxPassAge = false;
            }
            else
                chkNullMaxPassAge = true;
        }

        private void txtAccountLockThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtAccountLockThreshold.Text.IsItNumber())
            {
                txtAccountLockThreshold.Text = "";
                txtAccountLockThreshold.Focus();
                chkNullAccThreshold = false;
            }
            else if (txtAccountLockThreshold.Text.Length == 0)
            {
                chkNullAccThreshold = false;
            }
            else
                chkNullAccThreshold = true;
        }

        private void txtAccountLockDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtAccountLockDuration.Text.IsItNumber())
            {
                txtAccountLockDuration.Text = "";
                txtAccountLockDuration.Focus();
                chkNullAccDuration = false;
            }
            else if (txtAccountLockDuration.Text.Length == 0)
            {
                chkNullAccDuration = false;
            }
            else
                chkNullAccDuration = true;
        }

        private void txtPasswordRemember_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPasswordRemember.Text.IsItNumber())
            {
                txtPasswordRemember.Text = "";
                txtPasswordRemember.Focus();
                chkNullPassToRemember = false;
            }
            else if (txtPasswordRemember.Text.Length == 0)
            {
                chkNullPassToRemember = false;
            }
            else
                chkNullPassToRemember = true;
        }

        private void txtLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text.Length > 200)
            {
                txtLogin.SetValidation("Login Name Too Long");
                txtLogin.RaiseValidationError();
                txtLogin.Text = "";
                txtLogin.Focus();
            }
            // check if the login name exists.
            foreach (clsUserVO item in DataList)
            {
                //string ExistingLoginName = DataList[item].LoginName;
                if (txtLogin.Text.Trim() == item.LoginName)
                {
                    msgText = "Login Name Already Exist.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgWindow.Show();
                    //txtLogin.SetValidation("Login Name Already Exists");
                    //txtLogin.RaiseValidationError();
                    ////txtLogin.Text = "";
                    txtLogin.Focus();
                    break;
                }
                else
                    txtLogin.ClearValidationError();
            }

        }

        #endregion

        void CheckUserNameValidation()
        {
            int intResult = 0;

            clsUserVO objUser = CreateUserObjectFromFormData();

            clsGetExistingUserNameBizActionVO objLoginName = new clsGetExistingUserNameBizActionVO();
            objLoginName.UserName = objUser;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    intResult = ((clsGetExistingUserNameBizActionVO)ea.Result).SuccessStatus;

                    if (intResult != 0)
                    {
                        UserNameValid = false;
                    }
                    else
                        UserNameValid = true;
                }                
            };
            client.ProcessAsync(objLoginName, User);
            client.CloseAsync();
            client = null;            
        }

        void CheckUserExists()
        {
            int intResult = 0;

            clsGetLoginNameBizActionVO objLogin = new clsGetLoginNameBizActionVO();

            objLogin.LoginName = txtLogin.Text.ToString();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    intResult = ((clsGetLoginNameBizActionVO)ea.Result).SuccessStatus;
                    if (intResult != 0)
                    {
                        UserExists = false;

                        LoginNameExists = true;
                        msgText = "Login Name Already Exists. Please change the Login Name.";

                        //MessageBoxControl.MessageBoxChildWindow msgLogin = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        //msgLogin.Show();

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        txtLogin.Text = "";
                        txtLogin.Focus();
                    }
                    else
                        UserExists = true;
                }
            };
            client.ProcessAsync(objLogin, User);
            client.CloseAsync();
            client = null;             
        }

        enum PermissionType
        {
            GroupPermission = 1,
            RolePermission = 2,
            CustomPermission = 3
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoPassword.Text = txtPassword.Password;
        }

        private void optEmployee_Click(object sender, RoutedEventArgs e)
        {
            UserTypeID = 2;
            
            FillUserTypeWiseUserList();
        }

        private void SetFormValidation()
        {
            ValidAutoUserName = true;
            ValidLogin = true;
            isFormValid = true;

            if (chkExpirationDays.IsChecked == true)
            {
                if (!txtExpirationDays.Text.IsNumberValid())
                {
                    txtExpirationDays.SetValidation("Expiration days Required.");
                    txtExpirationDays.RaiseValidationError();
                    isFormValid = false;
                }
                else if (int.Parse(txtExpirationDays.Text) < 1)
                {
                    txtExpirationDays.SetValidation("Expiration days must be greater than 0");
                    txtExpirationDays.RaiseValidationError();
                    isFormValid = false;
                }
                else
                    txtExpirationDays.ClearValidationError();
            }
            else
                txtExpirationDays.ClearValidationError();

            if (string.IsNullOrEmpty(txtLogin.Text.Trim()))
            {
                txtLogin.SetValidation("Login Name cannot be blank.");
                txtLogin.RaiseValidationError();
                txtLogin.Focus();
                ValidLogin = false;
            }
            else
                txtLogin.ClearValidationError();

            if (string.IsNullOrEmpty(autoUserName.Text.Trim()))
            {
                autoUserName.SetValidation("User Name cannot be blank.");
                autoUserName.RaiseValidationError();
                autoUserName.Focus();
                ValidAutoUserName = false;
            }
            else
                autoUserName.ClearValidationError();
        }

        #region Fill Unit Store Commented
        
        private  void FillUnitStoreFirst()
        {
            List<clsItemStoreVO> StoreUnitList = new List<clsItemStoreVO>();
            try
            {
                clsGetUnitStoreBizActionVO BizAction = new clsGetUnitStoreBizActionVO();

                if ((clsUserUnitDetailsVO)dgUnitList.SelectedItem != null )
                {

                    grbUnitStore.Header = ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).UnitName + " Stores";
                    SelectedUnitId = ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).UnitID;
                    BizAction.ID = SelectedUnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, ea) =>
                    {
                        if (ea.Result != null && ea.Error == null)
                        {
                            List<clsItemStoreVO> StoreList = new List<clsItemStoreVO>();
                            if (((clsGetUnitStoreBizActionVO)ea.Result).Details != null)
                            {
                                StoreList = ((clsGetUnitStoreBizActionVO)ea.Result).Details;

                                dgUnitStoreList.ItemsSource = null;

                                foreach (var item in StoreList)
                                {
                                  //  if(item.status==true)
                                    StoreUnitList.Add(new clsItemStoreVO() { ID = item.ID, StoreName = item.StoreName, status = item.StoreStatus });
                                }
                               
                                FillUnitStore(StoreUnitList);
                            }
                            else
                            {
                                //clear Store Details.
                                //grbUnitStore.Visibility = Visibility.Collapsed;
                                msgText = "No Store Assigned to the Selected Unit.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, User);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {                
             //   throw;
            }
        }
       
        private void FillUnitStore(List<clsItemStoreVO> lstStore)
        {
            try
            {
                //if((clsUserVO)dgUserList.SelectedItem != null)
                if (EditStore == true)
                {
                     clsGetUnitStoreStatusBizActionVO BizActionDetails = new clsGetUnitStoreStatusBizActionVO();

                     if ((clsUserUnitDetailsVO)dgUnitList.SelectedItem != null)
                     {
                         SelectedUnitId = ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).UnitID;
                         //else
                         //    SelectedUnitId = 1;

                         BizActionDetails.ID = SelectedUnitId;

                         SelectedUserId = ((clsUserVO)dgUserList.SelectedItem).ID;
                         BizActionDetails.UserId = SelectedUserId;

                         Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                         PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

                         client1.ProcessCompleted += (s, ea) =>
                         {
                             if (ea.Result != null && ea.Error == null)
                             {
                                 List<clsItemStoreVO> StoreStatusList = new List<clsItemStoreVO>();
                                 if (((clsGetUnitStoreStatusBizActionVO)ea.Result).Details != null)
                                 {
                                     StoreStatusList = ((clsGetUnitStoreStatusBizActionVO)ea.Result).Details;

                                     if (lstStore.Count > 0)
                                     {
                                         for (int i = 0; i < StoreStatusList.Count; i++)
                                         {
                                             for (int lst = 0; lst < lstStore.Count; lst++)
                                             {
                                                 if (lstStore[lst].ID == StoreStatusList[i].ID)
                                                 {
                                                     lstStore[lst].StoreStatus = StoreStatusList[i].StoreStatus;
                                                     //break;
                                                 }
                                             }
                                         }
                                     }
                                 }
                                 dgUnitStoreList.ItemsSource = null;
                                 dgUnitStoreList.ItemsSource = lstStore;
                             }
                             else
                             {
                                 //Error occured.
                             }
                         };
                         client1.ProcessAsync(BizActionDetails, User);
                         client1.CloseAsync();
                     }
                 }
                 else
                 {
                     dgUnitStoreList.ItemsSource = null;
                     dgUnitStoreList.ItemsSource = lstStore; 
                 }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        private void hlbUnitStore_Click(object sender, RoutedEventArgs e)
        {
            if (((clsUserUnitDetailsVO)dgUnitList.SelectedItem).Status == true)
            {
                FillUnitStoreFirst();                
            }
            else
            {
                msgText = "Please select the Unit to View the Stores.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }
        }

        private void chkStoreStatus_Click(object sender, RoutedEventArgs e)
        {
            bool ChkStatus = false;
            if (dgUnitStoreList.SelectedItem != null)
            {
                clsItemStoreVO objItemStore = new clsItemStoreVO();
                List<clsItemStoreVO> objList = new List<clsItemStoreVO>();

                objList = (List<clsItemStoreVO>)dgUnitStoreList.ItemsSource;
                objItemStore = (clsItemStoreVO)dgUnitStoreList.SelectedItem;

                if (objItemStore.StoreStatus == true)
                {
                    foreach (var item in objList)
                    {
                        if (item == objItemStore)
                        {
                            item.StoreStatus = true;
                            //cmdSetStore.IsEnabled = true;
                            // StoreId = objItemStore.ID;
                            //StoreName = objItemStore.StoreName;
                            //hlbSetStore_Click(sender, e);
                        }
                        //else
                        //{
                        //item.StoreStatus = false;
                        //}
                    }
                    // hlbSetStore_Click(sender, e);
                    dgUnitStoreList.ItemsSource = null;
                    dgUnitStoreList.ItemsSource = objList;
                }
                //else
                //{
                //    foreach (var item in objList)
                //    {
                //        if (item.StoreStatus == true)
                //            ChkStatus = true;
                //    }
                //    if (ChkStatus == true)
                //        cmdSetStore.IsEnabled = true;
                //    else
                //        cmdSetStore.IsEnabled = false;
                //}
                //cmdSetStore.IsEnabled = true;
            }
        }
               
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            txtLogin.ClearValidationError();
            grbUnitStore.Visibility = Visibility.Collapsed;
        }

        #endregion

        private void hlbSetStore_Click(object sender, RoutedEventArgs e)
        {
            //ApplyStores store = (ApplyStores)sender;

            clsUserVO objUnit = new clsUserVO();
            bool StoreAssigned = false;
            string UnitName = "";

                objUnit.UserGeneralDetailVO.UnitDetails = (List<clsUserUnitDetailsVO>)dgUnitList.ItemsSource;
                UnitName = ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).UnitName;
                for (int Count = 0; Count < objUnit.UserGeneralDetailVO.UnitDetails.Count; Count++)
                {
                    if (objUnit.UserGeneralDetailVO.UnitDetails[Count].UnitID == SelectedUnitId)
                    {
                        objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails = (List<clsItemStoreVO>)dgUnitStoreList.ItemsSource; //store.SelectedStoreList;
                        for (int StoreCount = 0; StoreCount < objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails.Count; StoreCount++)
                        {
                            if(objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].StoreStatus==true)
                            {
                                objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].UnitId = SelectedUnitId;
                               // objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].StoreName = StoreName;
                                objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].StoreStatus = true;
                               // objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].ID = StoreId; 
                               // break;
                                StoreAssigned = true;
                            }
                            if (objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].StoreStatus == false)
                            {
                                objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].UnitId = SelectedUnitId;
                                objUnit.UserGeneralDetailVO.UnitDetails[Count].StoreDetails[StoreCount].StoreStatus = false;
                            }
                            dgUnitList.ItemsSource = null;
                            dgUnitList.ItemsSource = objUnit.UserGeneralDetailVO.UnitDetails;
                        }
                    }
                }
                if (StoreAssigned)
                {
                    msgText = "Store/s Assined to " + UnitName + " Unit ";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            //grbUnitStore.Visibility = Visibility.Collapsed;
        }

        private void hlbResetPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgUserList.SelectedItem != null)
                {
                    if (((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.Status == true)
                    {
                        UserEmailId = ((clsUserVO)dgUserList.SelectedItem).EmailId;
                        ResetPassword(((clsUserVO)dgUserList.SelectedItem).ID);
                    }
                }
            }
            catch (Exception ex)
            {                
                throw;
            }          
        }

        private void ResetPassword(long UserID)
        {
            clsResetPasswordBizActionVO objBizVO = new clsResetPasswordBizActionVO();
            clsUserVO objDetailsVO = new clsUserVO();
            objDetailsVO.Password = "Palash";
            objDetailsVO.ID = UserID;
            objBizVO.RPassword = objDetailsVO;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    msgText = "Password Reset To Palash";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                    if (UserEmailId != null && UserEmailId != "")
                        SendPasswordDetails();
                    else
                    {
                        msgText = "User Email Id not added. Could not send an Email to the User.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow1 = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow1.Show();
                    }

                }
            };
            client.ProcessAsync(objBizVO, User);
            client.CloseAsync();
            client = null;
        }

        private void dgUnitList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsUserUnitDetailsVO)dgUnitList.SelectedItem != null)
            {
                FillUnitStoreFirst();

                if (((clsUserUnitDetailsVO)dgUnitList.SelectedItem).IsDefault == true)
                {
                    cmdSetStore.IsEnabled = true;
                }
                else
                    cmdSetStore.IsEnabled = false;
            }            
        }

        private void cmdSetStore_Click(object sender, RoutedEventArgs e)
        {
            //if (objVo != null && objVo.Status == true)
            if (dgUnitList.SelectedItem != null && ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).Status == true)
            { 

                hlbSetStore_Click(sender, e);
            }
            else
            {
                msgText = "Please Select the Unit to set the Stores.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }
        }

        private void hlbAssignEMRTemplates_Click(object sender, RoutedEventArgs e)
        {
            if (dgUserList.SelectedItem != null)
            {
              
                    frmUserEMRTemplates myWin = new frmUserEMRTemplates();
                    myWin.UserID = ((clsUserVO)dgUserList.SelectedItem).ID;
                    myWin.Show();
               
            }
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillUserList();
              //  txtSearchCriteria.Focus();
            }
        }

        private void cmdApplyStores_Click(object sender, RoutedEventArgs e)
        {
            ApplyStores store = new ApplyStores();
            store.UnitList = UnitListOnApplyStore;
            store.OnSaveButton_Click += new RoutedEventHandler(hlbSetStore_Click);
            store.Show();
        }

        private void CmdUserCategoryLink_Click(object sender, RoutedEventArgs e)
        {
            if (dgUserList.SelectedItem != null)
            {
                UserCategoryLink objLink = new UserCategoryLink();
                objLink.SelectedUser = (clsUserVO)dgUserList.SelectedItem;
                objLink.Show();
            }
            else
            {
                string strMsg = "PLEASE, SELECT USER TO LINK WITH CATEGORIES.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
    }
}
