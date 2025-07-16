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
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.MachineParameter;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Administration
{
    public partial class FrmMachineParameterMaster : UserControl
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

        #region Variables

        public PagedSortableCollectionView<clsMachineParameterMasterVO> MasterList { get; private set; }
        private SwivelAnimation objAnimation;
        public bool IsCancel = true;
        public bool IsSearch = false;
        public string ParaDescription = "";
        //public long MachineId = 0;
        public bool IsModify = false;
        public long SelectedItemID = 0;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public bool IsView = false;
        public long SearchMachineId = 0;
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

        public FrmMachineParameterMaster()
        {
            InitializeComponent();
            this.DataContext = new clsMachineParameterMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Machine_ParameterLoaded);
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsMachineParameterMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dataGrid2Pager.DataContext = MasterList;
            grdMachineParameters.DataContext = MasterList;
            FillMachineMaster();
            //added by rohini dated 9.3.16
            PageSize = 15;
        }

        private void Machine_ParameterLoaded(object sender, RoutedEventArgs e)
        {
            SetupPage();
        }

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
                clsGetPathoMachineParameterBizActionVO bizActionVO = new clsGetPathoMachineParameterBizActionVO();

                bizActionVO.IsPagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartIndex = MasterList.PageIndex * MasterList.PageSize;
                if (IsSearch == true)
                {
                    if (txtDescription.Text != null)
                    {
                        bizActionVO.Description = txtDescription.Text;
                    }
                    bizActionVO.MachineID = ((MasterListItem)cmbMachineNameSearch.SelectedItem).ID;
                    SearchMachineId = bizActionVO.MachineID;

                }
                bizActionVO.DetailsList = new List<clsMachineParameterMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.DetailsList = (((clsGetPathoMachineParameterBizActionVO)args.Result).DetailsList);
                        MasterList.Clear();
                        if (bizActionVO.DetailsList.Count > 0)
                        {
                            MasterList.TotalItemCount = (int)(((clsGetPathoMachineParameterBizActionVO)args.Result).TotalRows);
                            foreach (clsMachineParameterMasterVO item in bizActionVO.DetailsList)
                            {
                                MasterList.Add(item);
                            }
                            FillMachineMasterForSearch();
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                //  throw;
            }
        }

        #region fill for search
        /// <summary>
        /// Fills Machine Master combo
        /// </summary>
        void FillMachineMasterForSearch()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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

                        cmbMachineNameSearch.ItemsSource = null;
                        cmbMachineNameSearch.ItemsSource = objList;
                        cmbMachineNameSearch.SelectedItem = objList[0];
                    }
                    if (IsSearch == true)
                    {
                        cmbMachineNameSearch.SelectedValue = SearchMachineId;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Fills Machine Master combo
        /// </summary>
        void FillMachineMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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
                        cmbMachineName.ItemsSource = null;
                        cmbMachineName.ItemsSource = objList;
                        cmbMachineName.SelectedItem = objList[0];

                        cmbMachineNameSearch.ItemsSource = null;
                        cmbMachineNameSearch.ItemsSource = objList;
                        cmbMachineNameSearch.SelectedItem = objList[0];
                    }
                    if ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem != null && IsView == true)
                    {
                        cmbMachineName.SelectedValue = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).MachineId;
                      //  cmbMachineName.IsEnabled = false;
                        IsView = false;
                    }
                    else
                    {
                      //  cmbMachineName.IsEnabled = true;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtParameterCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtParameterName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsMachineParameterMasterVO();
            ClearUI();
            Validation();
            IsView = false;
            FillMachineMaster();
            IsModify = false;
            IsSearch = false;
            SetCommandButtonState("New");
            try
            {
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
                IsModify = false;
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select required field to Save details!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    IsModify = true;
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update ?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    SaveDetails();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SaveDetails()
        {
            clsAddUpdatePathoMachineParameterBizActionVO objDetails = new clsAddUpdatePathoMachineParameterBizActionVO();
            try
            {
                clsMachineParameterMasterVO objParameter = new clsMachineParameterMasterVO();
                objParameter = (clsMachineParameterMasterVO)this.DataContext;
                objParameter = CreateFormData();

                objParameter.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objParameter.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objParameter.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objParameter.AddedDateTime = System.DateTime.Now;
                objParameter.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                objDetails.Details = objParameter;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddUpdatePathoMachineParameterBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Machine Parameter Details Added Successfully!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                            SelectedItemID = 0;
                            SetupPage();

                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                        }

                        else if (((clsAddUpdatePathoMachineParameterBizActionVO)args.Result).SuccessStatus == 4)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Machine Parameter Details Updated Successfully!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                            SelectedItemID = 0;
                            SetupPage();

                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                        }
                        else if (((clsAddUpdatePathoMachineParameterBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record Cannot Be Save Because CODE Already Exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdatePathoMachineParameterBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record Cannot Be Save Because DESCRIPTION Already Exist With Same Machine Name!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(objDetails, new clsUserVO());
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
            this.grdBackPanel.DataContext = new clsMachineParameterMasterVO();
            // cmbMachineName.SelectedValue = (int)0; //((clsMachineParameterMasterVO)this.DataContext).MachineId;            
            txtParameterCode.Text = "";
            txtParameterName.Text = "";
            chkIsFreezed.IsChecked = false;

            txtParameterCode.IsEnabled = true;
            txtParameterName.IsEnabled = true;
            chkIsFreezed.IsEnabled = true;
            cmbMachineName.IsEnabled = true;

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");

                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmPathologyConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Pathology Configuration";

                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = "  ";
                    IsCancel = true;
                    SetupPage();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// When cancel button click on front panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            IsSearch = true;
            this.dataGrid2Pager.PageIndex = 0;
            SetupPage();
            
           // MasterList.PageIndex = -1;
            //if(!string.IsNullOrEmpty(txtSearchCriteria.Text)
            //if( !string.IsNullOrEmpty(txtDescript
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
                Validation();
                if (grdMachineParameters.SelectedItem != null)
                {
                    SelectedItemID = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem);
                    txtParameterCode.Text = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).Code;
                    txtParameterName.Text = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).ParameterDesc;
                    //cmbMachineName.SelectedValue = ((clsMachineParameterMasterVO)this.DataContext).MachineId;  
                    chkIsFreezed.IsChecked = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).Freezed;
                    cmdModify.IsEnabled = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).Status;
                    IsView = true;
                    if (chkIsFreezed.IsChecked == true)
                    {
                        // cmbMachineName.SelectedValue = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).MachineId;
                        txtParameterCode.IsEnabled = false;
                        txtParameterName.IsEnabled = false;
                        chkIsFreezed.IsEnabled = false;
                        cmdModify.IsEnabled = false;
                    }
                    else
                    {
                        txtParameterCode.IsEnabled = true;
                        txtParameterName.IsEnabled = true;
                        chkIsFreezed.IsEnabled = true;
                        cmbMachineName.IsEnabled = true;
                        // cmdModify.IsEnabled = false;
                    }
                    FillMachineMaster();
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateStatusMachineParameterBizActionVO objStatus = new clsUpdateStatusMachineParameterBizActionVO();
            objStatus.ID = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).ID;
            objStatus.UnitID = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).UnitId;
            objStatus.MachineID = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).MachineId;
            objStatus.Status = ((clsMachineParameterMasterVO)grdMachineParameters.SelectedItem).Status;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (objStatus.Status == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
                SetCommandButtonState("Cancel");
            };
            client.ProcessAsync(objStatus, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;

                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtParameterCode.Text.Trim()))
            {
                txtParameterCode.SetValidation("Please Enter Code");
                txtParameterCode.RaiseValidationError();
                txtParameterCode.Focus();
                result=false;
            }
            else
            {
                txtParameterCode.ClearValidationError();               
              //  result = true;
            }
            if (string.IsNullOrEmpty(txtParameterName.Text.Trim()))
            {
                txtParameterName.SetValidation("Please Enter Description");
                txtParameterName.RaiseValidationError();
                txtParameterName.Focus();
                result=false;
            }
            else
            {               
                txtParameterName.ClearValidationError();               
               // result = true;
            }
             if (((MasterListItem)cmbMachineName.SelectedItem).ID <= 0)
            {
                cmbMachineName.TextBox.SetValidation("Please Select Machine");
                cmbMachineName.TextBox.RaiseValidationError();
                cmbMachineName.TextBox.Focus();
                result=false;
            }
            else
            {
                cmbMachineName.TextBox.ClearValidationError();
              //  result=true;
            }
            return result;
        }

        #endregion

        private clsMachineParameterMasterVO CreateFormData()
        {
            clsMachineParameterMasterVO objParameter = new clsMachineParameterMasterVO();
            if (IsModify == true)
            {
                objParameter.ID = SelectedItemID;
            }
            else
            {
                objParameter.ID = 0;
            }
            objParameter.Code = txtParameterCode.Text.Trim();
            objParameter.ParameterDesc = txtParameterName.Text.Trim();
            objParameter.Freezed = chkIsFreezed.IsChecked.Value;
            objParameter.Status = true;
            objParameter.MachineId = ((MasterListItem)cmbMachineName.SelectedItem).ID;
            return objParameter;
        }

        private void KeyUP_txtDescription(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }
        }

        private void KeyUP_MachineName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }
        }

    }
}
