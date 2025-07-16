//Created Date:20/August/2012
//Created By: Nilesh Raut
//Specification: To Add,Update the City Master(Emrates)

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

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
using PalashDynamics.UserControls;
namespace PalashDynamics.Administration
{
    public partial class frmCityMaster : UserControl
    {
        #region  Variables

        private SwivelAnimation objAnimation;
        WaitIndicator objProgress = new WaitIndicator();
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        public PagedSortableCollectionView<clsCityVO> MasterList { get; private set; }
        bool IsCancel = true;
        bool IsViewRec = false;
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
                //OnPropertyChanged("PageSize");
            }
        }
        #endregion

        public frmCityMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //this.Loaded += new RoutedEventHandler(BankBranch_Loaded);
            SetCommandButtonState("Load");
            FillCountry();
            FillState();
            MasterList = new PagedSortableCollectionView<clsCityVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCityDetails.DataContext = MasterList;
            SetupPage();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #region FillCombobox

        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
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

                    cmbCountry.ItemsSource = null;
                    cmbCountry.ItemsSource = objList;

                    cmbSearchCountry.ItemsSource = null;
                    cmbSearchCountry.ItemsSource = objList;
                    cmbSearchCountry.SelectedItem = objList[0];
                    //cmbSearchCountry

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillState()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_StateMaster;
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

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList;

                    cmbSearchState.ItemsSource = null;
                    cmbSearchState.ItemsSource = objList;
                    cmbSearchState.SelectedItem = objList[0];
                    //cmbSearchCountry

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillSearchState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    cmbSearchState.ItemsSource = null;
                    cmbSearchState.ItemsSource = objList;
                    cmbSearchState.SelectedValue = objList[0];
                    cmbSearchState.SelectedItem = objList[0];

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList;
                    cmbState.SelectedValue = objList[0];
                    cmbState.SelectedItem = objList[0];

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillState(long CountryID, long SelValue)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList;
                    cmbState.SelectedValue = SelValue;
                    //cmbState.SelectedItem = objList[0];

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion FillCombobox
        void SetupPage()
        {
            try
            {
                clsGetCityDetailsBizActionVO bizActionVO = new clsGetCityDetailsBizActionVO();

                bizActionVO.Description = txtSearch.Text.Trim();
                //bizActionVO.Code = txtCityCode.Text.Trim();
                if (cmbSearchState.SelectedItem != null)
                    bizActionVO.StateId = ((MasterListItem)cmbSearchState.SelectedItem).ID;
                if (cmbSearchCountry.SelectedItem != null)
                    bizActionVO.CountryId = ((MasterListItem)cmbSearchCountry.SelectedItem).ID;

                bizActionVO.SearchExpression = null;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

                clsCityVO objStateVo = new clsCityVO();
                bizActionVO.ListCityDetails = new List<clsCityVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        bizActionVO.ListCityDetails = (((clsGetCityDetailsBizActionVO)args.Result).ListCityDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetCityDetailsBizActionVO)args.Result).TotalRows);
                        ///Setup Page Fill DataGrid
                        if (bizActionVO.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in bizActionVO.ListCityDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
                        objProgress.Close();
                    }
                    else
                    {
                        objProgress.Close();
                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                objProgress.Close();
            }

        }
        #region Validation
        public bool Validation()
        {
            bool retVal = true;
            if (string.IsNullOrEmpty(txtCityCode.Text))
            {
                txtCityCode.SetValidation("Please Enter Code");
                txtCityCode.RaiseValidationError();
                txtCityCode.Focus();
                retVal = false;
            }
            else
                txtCityCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Please Enter Emirate/City Name");
                txtDescription.RaiseValidationError();
                if (retVal == true)
                    txtDescription.Focus();
                retVal = false;
            }
            else
                txtDescription.ClearValidationError();
           
            return retVal;


        }


        public bool Validation1()
        {
            bool retVal = true;
            if (cmbState.SelectedItem == null)
            {
                cmbState.TextBox.SetValidation("Please Select State");
                cmbState.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbState.Focus();
                retVal = false;

            }
            else if (((MasterListItem)cmbState.SelectedItem).ID == 0)
            {

                cmbState.TextBox.SetValidation("Please Select State");
                cmbState.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbState.Focus();
                retVal = false;

            }
            else
                cmbState.TextBox.ClearValidationError();
            if (cmbCountry.SelectedItem == null)
            {
                cmbCountry.TextBox.SetValidation("Please Select Country");
                cmbCountry.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbCountry.Focus();
                retVal = false;

            }

            else if (((MasterListItem)cmbCountry.SelectedItem).ID == 0)
            {

                cmbCountry.TextBox.SetValidation("Please Select Country");
                cmbCountry.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbCountry.Focus();
                retVal = false;

            }
            else
                cmbCountry.TextBox.ClearValidationError();
            return retVal;
        }
        #endregion
        public void ClearUI()
        {
            txtDescription.Text = "";
            txtCityCode.Text = "";
            MasterListItem Defaultc = ((List<MasterListItem>)cmbCountry.ItemsSource).FirstOrDefault(s => s.ID == 0);
            cmbCountry.SelectedItem = Defaultc;
            MasterListItem DefaultState = ((List<MasterListItem>)cmbState.ItemsSource).FirstOrDefault(s => s.ID == 0);
            cmbState.SelectedItem = DefaultState;
        }
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            grdBackPanel.DataContext = new clsCityVO();
             Validation();
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

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && Validation1())
            {
                string msgTitle = "";
                string msgText = "ARE YOU SURE YOU WANT TO SAVE THE RECORD ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && Validation1())
            {
                string msgTitle = "";
                string msgText = "ARE YOU SURE YOU WANT TO UPDATE THE RECORD ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCityCode.ClearValidationError();
            txtDescription.ClearValidationError();
            SetCommandButtonState("Cancel");
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

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCityBizActionVO BizActionVO = new clsAddUpadateCityBizActionVO();
                    clsCityVO objCityDet = new clsCityVO();

                    objCityDet.Id = 0;
                    objCityDet.Description = txtDescription.Text.Trim();
                    objCityDet.Code = txtCityCode.Text.Trim();
                    objCityDet.StateId = ((MasterListItem)cmbState.SelectedItem).ID;
                    objCityDet.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    objCityDet.Status = true;

                    objCityDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCityDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCityDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCityDet.AddedDateTime = System.DateTime.Now;
                    objCityDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCity = objCityDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD SAVED SUCCESSFULLY";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }

                        }
                        else
                        {
                            objProgress.Close();
                            msgText = "Record cannot be save ,Error occured.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();

                        }

                    };
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    objProgress.Close();
                    msgText = "Record cannot be save ,Error occured.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCityBizActionVO BizActionVO = new clsAddUpadateCityBizActionVO();
                    clsCityVO objCityDet = new clsCityVO();

                    objCityDet.Id = Convert.ToInt64(txtCityCode.Tag);
                    objCityDet.Description = txtDescription.Text.Trim();
                    objCityDet.Code = txtCityCode.Text.Trim();
                    objCityDet.StateId = ((MasterListItem)cmbState.SelectedItem).ID;
                    objCityDet.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    objCityDet.Status = true;


                    objCityDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCityDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCityDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCityDet.AddedDateTime = System.DateTime.Now;
                    objCityDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCity = objCityDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD UPDATED SUCCESSFULLY.";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }

                        }
                        else
                        {
                            msgText = "Record cannot be save ,Error occured.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }

                    };
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    objProgress.Close();
                    msgText = "Record cannot be save , Error Occured!";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            objProgress.Show();
            MasterList = new PagedSortableCollectionView<clsCityVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCityDetails.DataContext = MasterList;
            SetupPage();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (grdCityDetails.SelectedItem != null)
            {
                objProgress.Show();
                ClearUI();
                SetCommandButtonState("View");
                IsViewRec = true;
                txtCityCode.Tag = Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).Id);
                txtCityCode.Text = Convert.ToString(((clsCityVO)grdCityDetails.SelectedItem).Code);
                txtDescription.Text = Convert.ToString(((clsCityVO)grdCityDetails.SelectedItem).Description);
                cmbCountry.SelectedValue = Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).CountryId);
                cmdModify.IsEnabled = ((clsCityVO)grdCityDetails.SelectedItem).Status;
                FillState(Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).CountryId), Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).StateId));
                //cmbState.UpdateLayout();
                //cmbState.SelectedValue = Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).StateId);
                IsViewRec = false;
                try
                {
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    objProgress.Close();
                    IsViewRec = false;
                    throw;
                }
                objProgress.Close();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdCityDetails.SelectedItem != null)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCityBizActionVO BizActionVO = new clsAddUpadateCityBizActionVO();
                    clsCityVO objCityDet = new clsCityVO();
                    objCityDet.Id = Convert.ToInt64(((clsCityVO)grdCityDetails.SelectedItem).Id);
                    objCityDet.Description = ((clsCityVO)grdCityDetails.SelectedItem).Description;
                    objCityDet.Code = ((clsCityVO)grdCityDetails.SelectedItem).Code;
                    objCityDet.CountryId = ((clsCityVO)grdCityDetails.SelectedItem).CountryId;
                    objCityDet.StateId = ((clsCityVO)grdCityDetails.SelectedItem).StateId;
                    objCityDet.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    objCityDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCityDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCityDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCityDet.AddedDateTime = System.DateTime.Now;
                    objCityDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCity = objCityDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCityBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "STATUS UPDATED SUCCESSFULLY !";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                SetupPage();
                            }
                        }
                        else
                        {
                            objProgress.Close();
                            msgText = "Record cannot be save ,Error occured.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    };
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    objProgress.Close();
                    msgText = "Record cannot be save ,Error occured.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }

        private void txtCityCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCityCode.Text.IsItUpperCase() == false)
                txtCityCode.Text = txtCityCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text.IsItUpperCase() != true)
                txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

        private void cmbSearchCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbSearchCountry.SelectedItem).ID > 0)
            {
                cmbSearchState.ItemsSource = null;
                FillSearchState(((MasterListItem)cmbSearchCountry.SelectedItem).ID);
            }
            else
            {
                FillState();
                if (cmbSearchState.ItemsSource != null)
                    cmbSearchState.SelectedValue = 0;
            }
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsViewRec == false)
            {
                if (cmbCountry.SelectedItem != null)
                    if (((MasterListItem)cmbCountry.SelectedItem).ID > 0)
                    {
                        cmbState.ItemsSource = null;
                        FillState(((MasterListItem)cmbCountry.SelectedItem).ID);
                    }
                    else
                    {
                        FillState();
                        if (cmbState.ItemsSource != null)
                            cmbState.SelectedValue = 0;
                    }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
            }
        }
    }
}
