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
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.Pharmacy;

namespace PalashDynamics.Pharmacy
{
    public partial class CurrencyMaster : UserControl
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
        public Boolean Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result= false;
            }
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result= false;
            }
            else
            {
                txtcode.ClearValidationError();
                txtDescription.ClearValidationError();
                result= true;
            }
            return result;
        }
        #endregion

        #region Public Variables

        public string Action { get; set; }
        private SwivelAnimation objAnimation;
        string msgTitle = "";
        string msgText = "";
        string MasterTableName = "M_TermAndCondition";
        private long Id;
        string Path = "";
        bool Ispaging = true;
        public PagedSortableCollectionView<clsCurrencyMasterListVO> MasterList { get; private set; }
        public string ModuleName { get; set; }
        bool IsCancel = true;
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
        public CurrencyMaster()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
           
            SetCommandButtonState("Load");
            Id = 0;
            ClearUI();           
            MasterList = new PagedSortableCollectionView<clsCurrencyMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdMaster.DataContext = MasterList;
        }

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        #endregion

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsCurrencyMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.grdMaster.DataContext = MasterList;
            this.dataGrid2Pager.DataContext = MasterList;
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            if (((clsCurrencyMasterListVO)grdMaster.SelectedItem).Status == false)
            {
                cmdModify.IsEnabled = false;
            }
            grdMasterBackPanel.DataContext = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).DeepCopy();
            Id = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Id;

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                try
                {
                    clsAddUpdateCurrencyMasterListBizActionVO bizactionVO = new clsAddUpdateCurrencyMasterListBizActionVO();
                    clsCurrencyMasterListVO addMasterListVO = new clsCurrencyMasterListVO();
                    addMasterListVO.Id = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Id;
                    addMasterListVO.Code = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Code;
                    addMasterListVO.Description = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Description;
                    addMasterListVO.Symbol = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Symbol;
                    addMasterListVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).UnitId;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                Id = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully ";

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

        private void txtcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
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

        public void SetupPage()
        {
            clsGetCurrencyMasterListDetailsBizActionVO bizActionVO = new clsGetCurrencyMasterListDetailsBizActionVO();
            bizActionVO.SearchExperssion = txtSearch.Text.Trim();
            bizActionVO.PagingEnabled = true;
            bizActionVO.MasterTableName = MasterTableName;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsCurrencyMasterListVO getMasterListinfo = new clsCurrencyMasterListVO();
            bizActionVO.ItemMatserDetails = new List<clsCurrencyMasterListVO>();

            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ItemMatserDetails = (((clsGetCurrencyMasterListDetailsBizActionVO)args.Result).ItemMatserDetails);
                        ///Setup Page Fill DataGrid
                        if (Id == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetCurrencyMasterListDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsCurrencyMasterListVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
                    }
                    txtSearch.Focus();
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
            txtcode.Text = "";
            txtDescription.Text = "";
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("New");
            ClearUI();
            grdMasterBackPanel.DataContext = new clsCurrencyMasterListVO();
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
            if (Validation())
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            { Validation(); }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateCurrencyMasterListBizActionVO bizactionVO = new clsAddUpdateCurrencyMasterListBizActionVO();
                clsCurrencyMasterListVO addMasterListVO = new clsCurrencyMasterListVO();

                try
                {
                    addMasterListVO.Id = 0;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.Status = true;
                    if (txtSymbol.Text != null)
                    {
                        addMasterListVO.Symbol = txtSymbol.Text;
                    }
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully saved!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE is already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        /// This Event is Call When We click on Modify Button and Update Master Tables Details
        /// (For Add and Modify Master Tables Details, only One VO 
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
            }
            else
            { Validation(); }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateCurrencyMasterListBizActionVO bizactionVO = new clsAddUpdateCurrencyMasterListBizActionVO();
                clsCurrencyMasterListVO addMasterListVO = new clsCurrencyMasterListVO();
                try
                {
                    addMasterListVO.Id = Id;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.Status = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Status;
                    if (txtSymbol.Text != null)
                    {
                        addMasterListVO.Symbol = txtSymbol.Text;
                    }
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully updated!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Modify");

                            }
                            else if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE is already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateCurrencyMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        /// Have All Master Tables Records List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtDescription.ClearValidationError();
            txtcode.ClearValidationError();
            SetCommandButtonState("Cancel");
            Id = 0;
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Inventory Configuration";

                //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmInventoryConfiguration") as UIElement;
                //((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            }
            else
            {
                IsCancel = true;
            }


           
        }
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
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");

            grdMasterBackPanel.DataContext = ((clsCurrencyMasterListVO)grdMaster.SelectedItem);
            Id = ((clsCurrencyMasterListVO)grdMaster.SelectedItem).Id;

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
            }
        }
    }
}
