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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;
namespace PalashDynamics.Administration
{
    public partial class SubSpecialization : UserControl, INotifyPropertyChanged
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
            bool result = true;
            if (string.IsNullOrEmpty(txtSubSpecializationCode.Text.Trim()))
            {
                txtSubSpecializationCode.SetValidation("Please Enter Code");
                txtSubSpecializationCode.RaiseValidationError();
                txtSubSpecializationCode.Focus();
                result = false;//return false;
            }
             if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;//return false;
            }
            else if (cmbSpecialization.SelectedItem == null)
            {
                cmbSpecialization.TextBox.SetValidation("Please Select Specialization");
                cmbSpecialization.TextBox.RaiseValidationError();
                cmbSpecialization.Focus();

                //msgText = "Please Select Specialization !";
                //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //msgWindow.Show();
                result = false;//return false;
            }
            else if (((MasterListItem)cmbSpecialization.SelectedItem).ID==0)
            {
                //msgText = "Please Select Specialization !";
                //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //msgWindow.Show();
                cmbSpecialization.TextBox.SetValidation("Please Select Specialization");
                cmbSpecialization.TextBox.RaiseValidationError();
                cmbSpecialization.Focus();
                result = false;//return false;
            }
            else
            {
                txtSubSpecializationCode.ClearValidationError();
                txtDescription.ClearValidationError();
                cmbSpecialization.TextBox.ClearValidationError();
                //return true;
            }
            return result;
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
        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long supspecilizationId;
        public PagedSortableCollectionView<clsSubSpecializationVO> MasterList { get; private set; }
        bool IsCancel = true;
        #endregion

        #region Constructor
        public SubSpecialization()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(SubSpecialization_Loaded);
            SetCommandButtonState("Load");
            supspecilizationId = 0;
            FillSpecialization();

            MasterList = new PagedSortableCollectionView<clsSubSpecializationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh); 
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSupplierSubSpecialization.DataContext = MasterList;

            SetupPage();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        #region Loaded Event
        void SubSpecialization_Loaded(object sender, RoutedEventArgs e)
        {
          
            
        }

        #endregion Loaded Event

        #region Public Methods
        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Supplier Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Supplier on Which we Click  
        /// </summary>
        public void SetupPage()
        {

            clsGetSubSpecializationDetailsBizActionVO bizActionVO = new clsGetSubSpecializationDetailsBizActionVO();
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsSubSpecializationVO getsubspecializationinfo = new clsSubSpecializationVO();
            bizActionVO.ItemMatserDetails = new List<clsSubSpecializationVO>();
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((clsGetSubSpecializationDetailsBizActionVO)args.Result).ItemMatserDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetSubSpecializationDetailsBizActionVO)args.Result).TotalRows);
                        ///Setup Page Fill DataGrid
                        if (supspecilizationId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            foreach (clsSubSpecializationVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                            
                        }
                        //grdSupplierSubSpecialization.ItemsSource = null;
                        //grdSupplierSubSpecialization.ItemsSource = MasterList;              
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
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
            txtDescription.Text = "";
            txtSubSpecializationCode.Text = "";
            MasterListItem Defaultc = ((List<MasterListItem>)cmbSpecialization.ItemsSource).FirstOrDefault(s => s.ID == 0);
            cmbSpecialization.SelectedValue = (long)0; //Defaultc;
        }
        #endregion Public Methods

        #region FillCombobox

        public void FillSpecialization()
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

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
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
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have Subspecialization Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("New");
            grdBackPanel.DataContext = new clsSubSpecializationVO();
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
        /// This Event is Call When We click on Modify Button and Update Subspecialization Details
        /// (For Add and Modify Supplier Details, only One VO 
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

                #region Commented

                //try
                //{
                //    clsAddUpadateSubSpecializationBizActionVO bizactionVO = new clsAddUpadateSubSpecializationBizActionVO();
                //    clsSubSpecializationVO addNewSubspecializationVO = new clsSubSpecializationVO();
                //    addNewSubspecializationVO.SubSpecializationId = supspecilizationId;
                //    addNewSubspecializationVO.Code = txtSubSpecializationCode.Text;
                //    addNewSubspecializationVO.SubSpecializationName = txtDescription.Text;

                //    addNewSubspecializationVO.SpecializationId = Convert.ToInt64(cmbSpecialization.SelectedValue);
                //    addNewSubspecializationVO.ClinicId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).ClinicId;
                //    addNewSubspecializationVO.Status = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).Status;
                //    addNewSubspecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSubspecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewSubspecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewSubspecializationVO.UpdatedDateTime = System.DateTime.Now;
                //    addNewSubspecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewSubspecializationVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {

                //            if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                //            {

                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();

                //                //After Updation Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                supspecilizationId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Modify");

                //            }
                //            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 3)
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

            else{Validation();}
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpadateSubSpecializationBizActionVO bizactionVO = new clsAddUpadateSubSpecializationBizActionVO();
                    clsSubSpecializationVO addNewSubspecializationVO = new clsSubSpecializationVO();
                    addNewSubspecializationVO.SubSpecializationId = supspecilizationId;
                    addNewSubspecializationVO.Code = txtSubSpecializationCode.Text;
                    addNewSubspecializationVO.SubSpecializationName = txtDescription.Text;

                    addNewSubspecializationVO.SpecializationId = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                    addNewSubspecializationVO.ClinicId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).ClinicId;
                    addNewSubspecializationVO.Status = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).Status;
                    addNewSubspecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSubspecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSubspecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSubspecializationVO.UpdatedDateTime = System.DateTime.Now;
                    addNewSubspecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSubspecializationVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                            {

                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                supspecilizationId = 0;
                                SetupPage();
                                SetCommandButtonState("Modify");

                            }
                            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 3)
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

                }
            }
        }

        /// <summary>
        /// This Event is Call When We click on Save Button and Save SubSpecialization Details
        /// (For Add and Modify SubSpecialization Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

                #region Commented
                //try
                //{
                //    clsAddUpadateSubSpecializationBizActionVO bizactionVO = new clsAddUpadateSubSpecializationBizActionVO();
                //    clsSubSpecializationVO addNewSubSpecializationVO = new clsSubSpecializationVO();
                //    addNewSubSpecializationVO.SubSpecializationId = 0;
                //    addNewSubSpecializationVO.Code = txtSubSpecializationCode.Text;
                //    addNewSubSpecializationVO.SubSpecializationName = txtDescription.Text;
                //    addNewSubSpecializationVO.SpecializationId = Convert.ToInt64(cmbSpecialization.SelectedValue);
                //    addNewSubSpecializationVO.Status = true;
                //    addNewSubSpecializationVO.ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSubSpecializationVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSubSpecializationVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewSubSpecializationVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewSubSpecializationVO.AddedDateTime = System.DateTime.Now;
                //    addNewSubSpecializationVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewSubSpecializationVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Insertion Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                supspecilizationId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Save");
                //            }
                //            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 3)
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

            else { Validation(); }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpadateSubSpecializationBizActionVO bizactionVO = new clsAddUpadateSubSpecializationBizActionVO();
                    clsSubSpecializationVO addNewSubSpecializationVO = new clsSubSpecializationVO();
                    addNewSubSpecializationVO.SubSpecializationId = 0;
                    addNewSubSpecializationVO.Code = txtSubSpecializationCode.Text;
                    addNewSubSpecializationVO.SubSpecializationName = txtDescription.Text;
                    addNewSubSpecializationVO.SpecializationId = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                    addNewSubSpecializationVO.Status = true;
                    addNewSubSpecializationVO.ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSubSpecializationVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSubSpecializationVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSubSpecializationVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSubSpecializationVO.AddedDateTime = System.DateTime.Now;
                    addNewSubSpecializationVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSubSpecializationVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                supspecilizationId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 3)
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

                }
            }
        }

        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All Subspecialization List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            txtSubSpecializationCode.ClearValidationError();
            txtDescription.ClearValidationError();
            SetCommandButtonState("Cancel");
            supspecilizationId = 0;
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";  
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }          
        }

        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Subspecialization Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("View");
            supspecilizationId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).SubSpecializationId;
            grdBackPanel.DataContext = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).DeepCopy();
            cmbSpecialization.SelectedValue = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).SpecializationId;
          
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
        /// This Event is Call When We checked and Unchecked status from Datagrid
        ///And Status Updated Sucessfully  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            clsAddUpadateSubSpecializationBizActionVO bizactionVO = new clsAddUpadateSubSpecializationBizActionVO();
            clsSubSpecializationVO addNewSubSpecializationVO = new clsSubSpecializationVO();
            if (grdSupplierSubSpecialization.SelectedItem != null)
            {
                try
                {
                    addNewSubSpecializationVO.SubSpecializationId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).SubSpecializationId;
                    addNewSubSpecializationVO.Code = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).Code;
                    addNewSubSpecializationVO.SubSpecializationName = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).SubSpecializationName;
                    addNewSubSpecializationVO.SpecializationId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).SpecializationId;
                    addNewSubSpecializationVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewSubSpecializationVO.ClinicId = ((clsSubSpecializationVO)grdSupplierSubSpecialization.SelectedItem).ClinicId;
                    addNewSubSpecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSubSpecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSubSpecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSubSpecializationVO.UpdatedDateTime = System.DateTime.Now;
                    addNewSubSpecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSubSpecializationVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpadateSubSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                supspecilizationId = 0;
                                SetupPage();
                                msgText = "Status Updated Sucessfully";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                //cmdAdd.IsEnabled = true;
                                //cmdModify.IsEnabled = false;
                                SetCommandButtonState("Modify");

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsSubSpecializationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSupplierSubSpecialization.DataContext = MasterList;

            SetupPage();
        }

        #endregion Button Click Events

        #region Lost Focus
        private void txtSubSpecializationCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }
        #endregion

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
            }
        }

    }
}
