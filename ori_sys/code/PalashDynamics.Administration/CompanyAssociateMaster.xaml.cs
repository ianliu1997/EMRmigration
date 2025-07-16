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
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;
namespace PalashDynamics.Administration
{
    public partial class CompanyAssociateMaster : UserControl, INotifyPropertyChanged
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

        #region Validation
        public bool Validation()
        {
            
             if (cmbCompany.SelectedItem == null)
            {
                msgText = "Please Select Company !";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
            else if (cmbCompany.SelectedIndex == 0)
            {
                msgText = "Please Select Company !";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
            else if (cmbTariff.SelectedItem == null)
            {
                msgText = "Please Select Tariff !";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
            else if (cmbTariff.SelectedIndex == 0)
            {
                msgText = "Please Select Tariff !";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
           
                return true;
           

        }


        public Boolean ValidationLoaded()
        {
            bool result;
              
            if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result = false;
            }
            else
            {

                txtDescription.ClearValidationError();
                result = true;
            }
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else
            {

                txtDescription.ClearValidationError();
                result = true;
            }
            return result;

        }

        #endregion 
        
        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long CompanyAssociateId;
        public PagedSortableCollectionView<clsCompanyAssociateVO> MasterList { get; private set; }
        int PageIndex=0;
        public clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool IsCancel = true;
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
        #endregion

        #region Constructor
        public CompanyAssociateMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(CompanyAssociateMaster_Loaded);
            SetCommandButtonState("Load");
            CompanyAssociateId = 0;
            FillCompany();
            FillTariff();
            MasterList = new PagedSortableCollectionView<clsCompanyAssociateVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCompanyAssociate.DataContext = MasterList;
            SetupPage();
        }
        #endregion Constructor

        #region On Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            PageIndex = dataGrid2Pager.PageIndex;
            SetupPage();
        }
        #endregion

        #region Loaded Event
        void CompanyAssociateMaster_Loaded(object sender, RoutedEventArgs e)
        {
            
           
        }
        #endregion Loaded Event

        #region Public Methods

        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Company Associate Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Company Associate on Which we Click  
        /// </summary>
        public void SetupPage()
        {
            
            clsGetCompanyAssociateDetailsBizActionVO bizActionVO = new clsGetCompanyAssociateDetailsBizActionVO();
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsCompanyAssociateVO getCompanyAssociateinfo = new clsCompanyAssociateVO();
            bizActionVO.ItemMatserDetails = new List<clsCompanyAssociateVO>();
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((clsGetCompanyAssociateDetailsBizActionVO)args.Result).ItemMatserDetails);
                        ///Setup Page Fill DataGrid
                        if (CompanyAssociateId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetCompanyAssociateDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsCompanyAssociateVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
                        ValidationLoaded();
                    
                    }

                };
                client.ProcessAsync(bizActionVO, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }

        }





        /// <summary>
        /// When We Click On Add Button All UI Must Empty
        /// </summary>
        public void ClearUI()
        {
            txtcode.Text = "";
            txtDescription.Text = "";
            cmbCompany.SelectedIndex = 0;
            cmbTariff.SelectedIndex = 0;

        }
        #endregion

        #region FillCombobox

        public void FillCompany()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
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

                    cmbCompany.ItemsSource = null;
                    cmbCompany.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, User); // new clsUserVO());
            client.CloseAsync();
        }

        public void FillTariff()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
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

                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, User); //new clsUserVO());
            client.CloseAsync();
        }


        #endregion FillCombobox

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

        #region Button Click Events

        /// <summary>
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have  Company Associate Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ValidationLoaded();
            SetCommandButtonState("New");
            grdBackPenel.DataContext = new clsCompanyAssociateVO();
            ClearUI();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Event is Call When We click on Save Button and Save  Company Associate Details
        /// (For Add and Modify  Company Associate Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && ValidationLoaded())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
                #region Commented
                //try
                //{
                //    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                //    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();

                //    addNewCompanyAssociateVO.Id = 0;
                //    addNewCompanyAssociateVO.Code = txtcode.Text;
                //    addNewCompanyAssociateVO.Description = txtDescription.Text;
                //    addNewCompanyAssociateVO.CompanyId = Convert.ToInt64(cmbCompany.SelectedValue);
                //    addNewCompanyAssociateVO.TariffId = Convert.ToInt64(cmbTariff.SelectedValue);

                //    addNewCompanyAssociateVO.Status = true;
                //    addNewCompanyAssociateVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewCompanyAssociateVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewCompanyAssociateVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewCompanyAssociateVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewCompanyAssociateVO.AddedDateTime = System.DateTime.Now;
                //    addNewCompanyAssociateVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                After Insertion Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                CompanyAssociateId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Save");
                //            }
                //            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist !";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //           else if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }

                //        }

                //    };
                //    client.ProcessAsync(bizactionVO, new clsUserVO());
                //    client.CloseAsync();
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();

                    addNewCompanyAssociateVO.Id = 0;
                    addNewCompanyAssociateVO.Code = txtcode.Text;
                    addNewCompanyAssociateVO.Description = txtDescription.Text;
                    addNewCompanyAssociateVO.CompanyId = Convert.ToInt64(cmbCompany.SelectedValue);
                    addNewCompanyAssociateVO.TariffId = Convert.ToInt64(cmbTariff.SelectedValue);

                    addNewCompanyAssociateVO.Status = true;
                    addNewCompanyAssociateVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewCompanyAssociateVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewCompanyAssociateVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewCompanyAssociateVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewCompanyAssociateVO.AddedDateTime = System.DateTime.Now;
                    addNewCompanyAssociateVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully Added";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                CompanyAssociateId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist !";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, User); //new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// This Event is Call When We click on Modify Button and Update  Company Associate Details
        /// (For Add and Modify  Company Associate Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && ValidationLoaded())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

                #region Commented
                //try
                //{
                //    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                //    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();
                //    addNewCompanyAssociateVO.Id = CompanyAssociateId;
                //    addNewCompanyAssociateVO.Code = txtcode.Text;
                //    addNewCompanyAssociateVO.Description = txtDescription.Text;
                //    addNewCompanyAssociateVO.CompanyId = Convert.ToInt64(cmbCompany.SelectedValue);
                //    addNewCompanyAssociateVO.TariffId = Convert.ToInt64(cmbTariff.SelectedValue);
                //    addNewCompanyAssociateVO.UnitId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).UnitId;
                //    addNewCompanyAssociateVO.Status = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Status;
                //    addNewCompanyAssociateVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewCompanyAssociateVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewCompanyAssociateVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewCompanyAssociateVO.UpdatedDateTime = System.DateTime.Now;
                //    addNewCompanyAssociateVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {

                //            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();

                //                //After Updation Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                CompanyAssociateId = 0;
                //                SetupPage();

                //                SetCommandButtonState("Modify");

                //            }
                //            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }


                //        }

                //    };
                //    client.ProcessAsync(bizactionVO, new clsUserVO());
                //    client.CloseAsync();
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Update The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();
                    addNewCompanyAssociateVO.Id = CompanyAssociateId;
                    addNewCompanyAssociateVO.Code = txtcode.Text;
                    addNewCompanyAssociateVO.Description = txtDescription.Text;
                    addNewCompanyAssociateVO.CompanyId = Convert.ToInt64(cmbCompany.SelectedValue);
                    addNewCompanyAssociateVO.TariffId = Convert.ToInt64(cmbTariff.SelectedValue);
                    addNewCompanyAssociateVO.UnitId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).UnitId;
                    addNewCompanyAssociateVO.Status = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Status;
                    addNewCompanyAssociateVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewCompanyAssociateVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewCompanyAssociateVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewCompanyAssociateVO.UpdatedDateTime = System.DateTime.Now;
                    addNewCompanyAssociateVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully Updated";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                CompanyAssociateId = 0;
                                SetupPage();

                                SetCommandButtonState("Modify");

                            }
                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }


                        }

                    };
                    client.ProcessAsync(bizactionVO, User); //new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All  Company Associate List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            CompanyAssociateId = 0;
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";          

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Company Associate Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            CompanyAssociateId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Id;
            grdBackPenel.DataContext = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).DeepCopy();
            cmbCompany.SelectedValue = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).CompanyId;
            cmbTariff.SelectedValue = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).TariffId;
            if (((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else
                cmdModify.IsEnabled = true;
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Event is call When we Check and Uncheck Status 
        /// then staus updated in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
            if (grdCompanyAssociate.SelectedItem != null)
            {
                #region Commented
                //string msgTitle = "";
                //string msgText = "Are you sure you want to Update Status?";

                //MessageBoxControl.MessageBoxChildWindow msgWinStatus =
                //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgWinStatus.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinStatus_OnMessageBoxClosed);

                //msgWinStatus.Show();
                //((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Status= Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                #endregion

                try
                {
                    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();

                    addNewCompanyAssociateVO.Id = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Id;
                    addNewCompanyAssociateVO.Code = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Code;
                    addNewCompanyAssociateVO.Description = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Description;
                    addNewCompanyAssociateVO.CompanyId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).CompanyId;
                    addNewCompanyAssociateVO.TariffId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).TariffId;
                    addNewCompanyAssociateVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewCompanyAssociateVO.UnitId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).UnitId;
                    addNewCompanyAssociateVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewCompanyAssociateVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewCompanyAssociateVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewCompanyAssociateVO.UpdatedDateTime = System.DateTime.Now;
                    addNewCompanyAssociateVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                CompanyAssociateId = 0;
                                SetupPage();

                                msgText = "Status Updated Successfully ";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");

                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, User); // new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
              
            }
        }

        void msgWinStatus_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateCompanyAssociateBizActionVO bizactionVO = new clsAddUpdateCompanyAssociateBizActionVO();
                    clsCompanyAssociateVO addNewCompanyAssociateVO = new clsCompanyAssociateVO();

                    addNewCompanyAssociateVO.Id = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Id;
                    addNewCompanyAssociateVO.Code = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Code;
                    addNewCompanyAssociateVO.Description = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Description;
                    addNewCompanyAssociateVO.CompanyId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).CompanyId;
                    addNewCompanyAssociateVO.TariffId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).TariffId;
                    addNewCompanyAssociateVO.Status = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).Status;
                    addNewCompanyAssociateVO.UnitId = ((clsCompanyAssociateVO)grdCompanyAssociate.SelectedItem).UnitId;
                    addNewCompanyAssociateVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewCompanyAssociateVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewCompanyAssociateVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewCompanyAssociateVO.UpdatedDateTime = System.DateTime.Now;
                    addNewCompanyAssociateVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewCompanyAssociateVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateCompanyAssociateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                CompanyAssociateId = 0;
                                SetupPage();

                                msgText = "Status Updated Successfully ";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");

                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, User); // new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
            MasterList = new PagedSortableCollectionView<clsCompanyAssociateVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCompanyAssociate.DataContext = MasterList;

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsCompanyAssociateVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCompanyAssociate.DataContext = MasterList;
            dataGrid2Pager.PageIndex = PageIndex;
        }
        #endregion Button Click Events

        #region Lost Focus

        private void txtcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }

        #endregion

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsCompanyAssociateVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                SetupPage();
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdCompanyAssociate.DataContext = MasterList;
                dataGrid2Pager.PageIndex = PageIndex;
            }
        }
      
    }
}
