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
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using CIMS;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class frmDietPlanMaster : UserControl, INotifyPropertyChanged
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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long OTTableID;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public bool isModify = false;
        public PagedSortableCollectionView<clsIPDDietPlanMasterVO> MasterList { get; private set; }

        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDDietPlanMasterVO getstoreinfo;
        public bool Ammenities = false;
        ObservableCollection<clsIPDDietPlanItemMasterVO> FoodItemList { get; set; }
        bool IsNew = false;

        /// <summary>
        /// gets richtext editor
        /// </summary>
        public Liquid.RichTextEditor TextEditor
        {
            get { return richTextEditor; }
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

        #region Paging

        public PagedSortableCollectionView<clsIPDDietPlanItemMasterVO> DataList { get; private set; }

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
       
        #endregion
               
        public frmDietPlanMaster()
        {
            InitializeComponent();            
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            MasterList = new PagedSortableCollectionView<clsIPDDietPlanMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            MasterList.PageSize = 15;
            SetCommandButtonState("Load");
            SetupPage();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.DataContext = new clsIPDDietPlanMasterVO();
            FoodItemList = new ObservableCollection<clsIPDDietPlanItemMasterVO>();            
        }
        
        /// <summary>
        /// set command button set
        /// </summary>
        /// <param name="strFormMode">button content</param>
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdNew.IsEnabled = true;
                   // cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;

                case "Cancel":
                    cmdNew.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdNew.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }
                
        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {                
                clsIPDGetDietPlanBizActionVO bizActionVO = new clsIPDGetDietPlanBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
             //   getstoreinfo = new clsIPDDietPlanMasterVO();
                bizActionVO.objDietPlanMasterDetails = new List<clsIPDDietPlanMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objDietPlanMasterDetails = (((clsIPDGetDietPlanBizActionVO)args.Result).objDietPlanMasterDetails);
                        if (bizActionVO.objDietPlanMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetDietPlanBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDDietPlanMasterVO item in bizActionVO.objDietPlanMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            dgDietPlan.ItemsSource = null;
                            dgDietPlan.ItemsSource = MasterList;
                        }

                        //if (((clsGetPackageServiceListBizActionVO)arg.Result).PackageList != null)
                        //{
                        //    clsGetPackageServiceListBizActionVO result = arg.Result as clsGetPackageServiceListBizActionVO;

                        //    DataList.TotalItemCount = result.TotalRows;

                        //    if (result.PackageList != null)
                        //    {
                        //        DataList.Clear();

                        //        foreach (var item in result.PackageList)
                        //        {
                        //            DataList.Add(item);
                        //        }

                        //        dgPackage.ItemsSource = null;
                        //        dgPackage.ItemsSource = DataList;

                        //        dataGrid2Pager.Source = null;
                        //        dataGrid2Pager.PageSize = BizAction.MaximumRows;
                        //        dataGrid2Pager.Source = DataList;

                        //    }
                        //}
                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsIPDDietPlanMasterVO();
            dgServiceList.ItemsSource = null;
            FoodItemList = new ObservableCollection<clsIPDDietPlanItemMasterVO>();
            TextEditor.Html = string.Empty;
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
                txtStoreName.SetValidation("Please Enter Bed No.");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                return false;
            }
            else
            {
                txtStoreCode.ClearValidationError();
                txtStoreName.ClearValidationError();
                return true;
            }
        }

        #endregion

        /// <summary>
        /// Gets ID of existing OTTable or new OT Table
        /// </summary>
        /// <returns>clsIPDBlockMasterVO object</returns>
        private clsIPDDietPlanMasterVO CreateFormData()
        {
            clsIPDDietPlanMasterVO addNewDietVO = new clsIPDDietPlanMasterVO();
            {
                if (isModify == true)
                    addNewDietVO.ID = ((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem).ID;
                else
                    addNewDietVO.ID = 0;
                addNewDietVO.Code = txtStoreCode.Text;
                addNewDietVO.Description = txtStoreName.Text;
                addNewDietVO.UnitID = User.UserGeneralDetailVO.UnitId;
                addNewDietVO.CreatedUnitID = User.UserGeneralDetailVO.UnitId;
                addNewDietVO.AddedBy = User.UserGeneralDetailVO.AddedBy;
                addNewDietVO.AddedDateTime = User.UserGeneralDetailVO.AddedDateTime;
                addNewDietVO.AddedOn = User.UserGeneralDetailVO.AddedOn;
                addNewDietVO.AddedWindowsLoginName = User.UserLoginInfo.WindowsUserName;
                addNewDietVO.Status = true;
                addNewDietVO.PlanInst = TextEditor.Html.ToString();
                addNewDietVO.ItemList = FoodItemList.ToList();
            }

            return addNewDietVO;
        }

        /// <summary>
        /// if cancel button clicked on front panel it goes to config screen
        /// </summary>
        /// <param name="sender">Click button</param>
        /// <param name="e">Click Button Click</param>
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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsIPDDietPlanMasterVO();
            ClearUI();
            isModify = false;
            SetCommandButtonState("New");
            try
            {
                TabDietPlan.SelectedIndex = 0;
                objAnimation.Invoke(RotationType.Forward);
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
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the details ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();                
        }

        private void Save()
        {
            clsIPDAddUpdateDietPlanBizactionVO BizAction = new clsIPDAddUpdateDietPlanBizactionVO();
            try
            {
                BizAction.objDietMatserDetails = CreateFormData();
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 1)
                        {
                            if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).objDietMatserDetails != null)
                            {
                                ClearUI();
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record is successfully submitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }
                        }
                        else if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be save because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 3)
                        {
                            msgText = "Record cannot be save because DESCRIPTION already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
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

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to update the record ?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Update();             
            }
        }

        private void Update()
        {
            clsIPDAddUpdateDietPlanBizactionVO BizAction = new clsIPDAddUpdateDietPlanBizactionVO();
            try
            {
               // BizAction.objDietMatserDetails = (clsIPDDietPlanMasterVO)this.DataContext;
                //if (cmbService.SelectedItem != null)
                //    BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
                isModify = true;
                BizAction.objDietMatserDetails = CreateFormData();
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result !=null)
                    {
                        if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 1)
                        {
                            if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).objDietMatserDetails != null)
                            {
                                ClearUI();
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record is successfully submitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                        }
                        else if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be save because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateDietPlanBizactionVO)arg.Result).SuccessStatus == 3)
                        {
                            msgText = "Record cannot be save because DESCRIPTION already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetupPage();
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmDietNutritionConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Diet and Nutrition Configuration";

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

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            SetCommandButtonState("View");
            if (dgDietPlan.SelectedItem != null)
            {
                IsNew = false;
                TabDietPlan.SelectedIndex = 0;
                this.DataContext = ((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem);
                //GetPackageDetails(((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem).ID);
                grdBackPanel.DataContext = ((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem);
                TextEditor.Html = ((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem).PlanInst;
                List<clsIPDDietPlanItemMasterVO> obj=((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem).ItemList;
                foreach (var item in obj)
	            {
                    FoodItemList.Add(item);		 
	            }

                
                dgServiceList.ItemsSource = null;
                dgServiceList.ItemsSource = FoodItemList;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDUpdateDietPlanStatusBizActionVO objStatus = new clsIPDUpdateDietPlanStatusBizActionVO();
                objStatus.DietStatus = new clsIPDDietPlanMasterVO();
                // objStatus.RoomStatus = (clsIPDRoomMasterVO)dgAmmenityList.SelectedItem;
                objStatus.DietStatus = (clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem;
                objStatus.DietStatus.Status = ((clsIPDDietPlanMasterVO)dgDietPlan.SelectedItem).Status;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (objStatus.DietStatus.Status == false)
                        {
                            msgText = "Status Deactivated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "Status Activated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }

                    }
                    SetCommandButtonState("View");

                };
                client.ProcessAsync(objStatus, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }  
        }

        private void TabDietPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected record ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        FoodItemList.RemoveAt(dgServiceList.SelectedIndex);
                        dgServiceList.Focus();
                        dgServiceList.UpdateLayout();
                        dgServiceList.SelectedIndex = FoodItemList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void cmdAddFoodItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            frmFoodItemList objItemList = new frmFoodItemList();
            objItemList.OnAddButton_Click += new RoutedEventHandler(objItemList_OnAddButton_Click);
            objItemList.Show();
        }

        void objItemList_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmFoodItemList)sender).DialogResult == true)
            {
                for (int i = 0; i < ((frmFoodItemList)sender).check.Count; i++)
                {
                    if (((frmFoodItemList)sender).check[i] == true)
                    {
                        var item1 = from r in FoodItemList
                                    where (r.ID == (((frmFoodItemList)sender).FoodItemSource[i]).ID
                                    && r.FoodItem.ItemID == (((frmFoodItemList)sender).FoodItemSource[i]).ItemID)
                                    select new clsIPDDietNutritionMasterVO
                                    {
                                        ItemID=r.FoodItem.ItemID,
                                        ItemName = r.FoodItem.ItemName,
                                        Description=r.Description,
                                        ID=r.ID                       
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsIPDDietPlanItemMasterVO objItem = new clsIPDDietPlanItemMasterVO();
                            objItem.FoodItem.ID = (((frmFoodItemList)sender).FoodItemSource[i].ID);
                            objItem.FoodItem.Description = (((frmFoodItemList)sender).FoodItemSource[i].Description);
                            objItem.FoodItem.ItemID =(((frmFoodItemList)sender).FoodItemSource[i].ItemID);
                            objItem.FoodItem.ItemName=(((frmFoodItemList)sender).FoodItemSource[i].ItemName);
                            objItem.Timing = null;
                            objItem.ItemQty = null;
                            objItem.ItemUnit = null;
                            objItem.ItemCalories = null;
                            objItem.ItemCh = null;
                            objItem.ItemFat = null;
                            objItem.ItemInst = null;
                            objItem.ExpectedCal = null;
                            FoodItemList.Add(objItem);
                        }
                    }
                }
                PagedCollectionView collection = new PagedCollectionView(FoodItemList);
                dgServiceList.ItemsSource = collection;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }
        }

    }
}
