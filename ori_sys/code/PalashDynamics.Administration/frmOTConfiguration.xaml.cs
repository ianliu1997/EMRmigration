using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Windows.Input;
using PalashDynamic.Localization;

namespace PalashDynamics.Administration
{
    public partial class frmOTConfiguration : UserControl
    {
        #region Public Variables

        public string Action { get; set; }
        private SwivelAnimation objAnimation;
        string msgTitle = "";
        string msgText = "";
        string MasterTableName = "";
        private long Id;
        string Path = "";
        bool Ispaging = true;
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public string ModuleName { get; set; }
        LocalizationManager objLocalizationManeger;
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
                //OnPropertyChanged("PageSize");
            }
        }

        #endregion

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        #endregion

        #region Validation
        /// <summary>
        /// This Function is Called Whenever We Have To Check If The Mandatory Fields Are Entered By The User
        /// Before Saving Or Updating Data. If The Mandatory Feilds are Not Complete Or As Per The Requirements,
        /// A Validation Error Is Raised.        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        public Boolean Validation()
        {
            string strVal = string.Empty;
            bool Result = true;
            //if (string.IsNullOrEmpty(txtcode.Text))
            if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            {
                strVal = "Please enter code !";
                //strVal = objLocalizationManeger.GetValue("CodeReqd_Msg"); 
                txtcode.SetValidation(strVal);
                txtcode.RaiseValidationError();
                txtcode.Focus();
                Result = false;
                //return false;
            }
            else
                txtcode.ClearValidationError();


            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                strVal = "Please enter description !";
                //strVal = objLocalizationManeger.GetValue("DescReqd_Msg");
                txtDescription.SetValidation(strVal);
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                Result = false;
                //return false;
            }
            else
                txtDescription.ClearValidationError();

            return Result;
    
            //else
            //{
            //    txtcode.ClearValidationError();
            //    txtDescription.ClearValidationError();
            //    return true;
            //}
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor
        public frmOTConfiguration()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            TreeView1.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TreeView1_SelectedItemChanged);
            SetCommandButtonState("Load");
            Id = 0;
            ClearUI();
            TreeView1.ExpandSelectedPath();
            TreeView1.CollapseAllButSelectedPath();
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            //objLocalizationManeger = ((IApplicationConfiguration)App.Current).LocalizedManager;
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdMaster.DataContext = MasterList;
        }
        #endregion

        void TreeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //throw new NotImplementedException();
            TreeView1.CollapseAllButSelectedPath();
            TreeView1.ExpandSelectedPath();
        }

        /// <summary>
        /// This Event is Called When We Click On A Hyperlink of The Master To Open, And We are Going To Open The Common Master
        /// Form Already Created. The Front Panel is displayed On Which We Have DataGrid
        /// To Display The Records In The Master Table.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            grdMainform.Visibility = Visibility.Collapsed;
            MainDockPanel.Visibility = Visibility.Visible;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");

            HyperlinkButton Hb = (HyperlinkButton)e.OriginalSource;
            string strDetails = "Details";
            string strListOf = "List Of";
            //string strDetails = objLocalizationManeger.GetValue("lblDetails");
            //string strListOf = objLocalizationManeger.GetValue("lblListOf");
            grpMasterDetails.Header = Hb.Content +" "+ strDetails;
            ContentControl.Content = strListOf+" " + Hb.Content + " "+strDetails;
            MasterTableName = Hb.Name;
            mElement.Text = Hb.Content.ToString();

            if (Hb.Content.Equals("Doctor Notes") || Hb.Content.Equals("Surgery Notes Master") || Hb.Content.Equals("Anesthsia Notes Master"))
            {
                txtDescription.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtDescription.Height = 100;
                txtDescription.MaxLength = 4000;
            }
            SetupPage();
        }

        /// <summary>
        /// This Function is Called Whenever We Have To Fill The Grid On The Front Panel.
        /// The Master Data From The Respective Table is Retrieved For The List.        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetupPage()
        {
            clsGetMasterListDetailsBizActionVO bizActionVO = new clsGetMasterListDetailsBizActionVO();
            bizActionVO.SearchExperssion = txtSearch.Text.Trim();
            bizActionVO.PagingEnabled = true;
            bizActionVO.MasterTableName = MasterTableName;

            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsMasterListVO getMasterListinfo = new clsMasterListVO();
            bizActionVO.ItemMatserDetails = new List<clsMasterListVO>();

            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ItemMatserDetails = (((clsGetMasterListDetailsBizActionVO)args.Result).ItemMatserDetails);
                        ///Setup Page Fill DataGrid
                        if (Id == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetMasterListDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsMasterListVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
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
        /// This Event is Called When We click on New Button, the BackPanel is displayed
        /// The BackPanel is Cleared and Command Buttons State is set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            Validation();
            ClearUI();
            grdMasterBackPanel.DataContext = new clsMasterListVO();
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
        /// This Event is Called When We click on Save Button To Save The Master Tables Details
        /// (For Add and Modify Master Tables Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to save ?"; 
                //string msgText = objLocalizationManeger.GetValue("SaveVerification_Msg"); 
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();
            }
        }

        /// <summary>
        /// This Event is Called When We click on Modify Button and Update Master Tables Details
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
                string msgText="Are you sure you want to modify ?";
                //string msgText = objLocalizationManeger.GetValue("ModifyVerification_Msg");

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        /// <summary>
        /// This Event is Called When We click on Cancel Button
        /// If Back Panel Is Open With The Details, Then We Display The Front Panel On Which We Have DataGrid
        /// To Display All Master Tables Records List.
        /// If Front Panel Is Open Then We Traverse To The Radiology Configuration Form Where We Have The List Of All
        /// The Masters Under Radilogy.    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtDescription.ClearValidationError();
            txtcode.ClearValidationError();
            SetCommandButtonState("Cancel");
            SetupPage();
            Id = 0;
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {                
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "OT Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmOTConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        /// <summary>
        /// This Event is Called When We click on Close Button
        /// This Event is Currently disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>         
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");

            grdMasterBackPanel.DataContext = ((clsMasterListVO)grdMaster.SelectedItem);
            Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;

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
        /// This Event is Called When We Click On A The View Hyperlink On The Front Panel,
        /// To View The Details For That Particular Record On The BackPanel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");

            grdMasterBackPanel.DataContext = ((clsMasterListVO)grdMaster.SelectedItem);
            Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
            if (((clsMasterListVO)grdMaster.SelectedItem).Status == false)
            {
                cmdModify.IsEnabled = false;
            }
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
        /// This Event Is Called When We Click On The Check Box For a Particular Record In The List Displayed On The Front Panel,
        /// The Status Is Changed For That Record.
        /// i.e. If The Record Was Enabled Then It Is Disabled and Vise Versa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                try
                {
                    clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                    clsMasterListVO addMasterListVO = new clsMasterListVO();
                    addMasterListVO.Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
                    addMasterListVO.Code = ((clsMasterListVO)grdMaster.SelectedItem).Code;
                    addMasterListVO.Description = ((clsMasterListVO)grdMaster.SelectedItem).Description;
                    addMasterListVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
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
                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                Id = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully ";
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                objAnimation.Invoke(RotationType.Backward);
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


        /// <summary>
        /// This Event is Called When We click on Search Icon
        /// This Filters The List Displayed on The Front Panel As Per The Criteria Entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.grdMaster.DataContext = MasterList;
            this.dataGrid2Pager.DataContext = MasterList;
        }

        /// <summary>
        /// This Event Is Called On The Lost Focus Of Code Text Box.
        /// Here If The Text Box Contains Any Value, Then The Validation Flag Is Cleared For Code Text Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcode_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text) )
            //{
            //    ((TextBox)e.OriginalSource).ClearValidationError();
            //}
        }

        /// <summary>
        /// This Function Is Called Whenever We Have To Clear Back Panel.        
        /// </summary>
        public void ClearUI()
        {
            txtcode.Text = "";
            txtDescription.Text = "";
        }

        /// <summary>
        /// This Function is Called On Click Of Confirmation Message For Save Event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                clsMasterListVO addMasterListVO = new clsMasterListVO();

                try
                {
                    addMasterListVO.Id = 0;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.Status = true;
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
                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record Saved Successfully.";
                                //msgText = objLocalizationManeger.GetValue("RecordSaved_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE  already exist !";
                                //msgText = objLocalizationManeger.GetValue("CodeDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist !";
                                //msgText = objLocalizationManeger.GetValue("DescriptionDuplicate_Msg");
                               ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        /// This Function is Called On Click Of Confirmation Message For Modify Event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                clsMasterListVO addMasterListVO = new clsMasterListVO();
                try
                {
                    addMasterListVO.Id = Id;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
                    addMasterListVO.Status = ((clsMasterListVO)grdMaster.SelectedItem).Status;
                    addMasterListVO.MasterTableName = MasterTableName;
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

                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record Updated Successfully.";
                                //msgText = objLocalizationManeger.GetValue("RecordSaved_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Modify");

                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE  already exist !";
                                //msgText = objLocalizationManeger.GetValue("CodeDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist !";
                                //msgText = objLocalizationManeger.GetValue("DescriptionDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        /// This Function is Called When After Whenever We Have To Change The Status Of The Command Buttons.
        /// Depending On The Action That Is Being Performed, The Command Buttons(New, Save, Modify, Cancel) Are 
        /// Enabled/Disabled In This Function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void CreatedForm_Click(object sender, RoutedEventArgs e)
        {
            MainDockPanel.Visibility = Visibility.Visible;
            HyperlinkButton HB = (HyperlinkButton)e.OriginalSource;
            if (HB.Tag != null)
            {
                Action = HB.Tag.ToString();

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = HB.Content.ToString();
                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
            }
        }

        private void CreatedformInOtherProject_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton HB = (HyperlinkButton)e.OriginalSource;
            ModuleName = "PalashDynamics." + HB.Name;
            Action = HB.Tag.ToString();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = HB.Content.ToString();
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
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

        private void cmdSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsMasterListVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                dataGrid2Pager.PageIndex = 0;
                SetupPage();
                this.grdMaster.DataContext = MasterList;
                this.dataGrid2Pager.DataContext = MasterList;
            }
        }

        private void M_ConsentMaster_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void DoctorNotes_Click(object sender, RoutedEventArgs e)
        //{
        //    grdMainform.Visibility = Visibility.Collapsed;
        //    MainDockPanel.Visibility = Visibility.Visible;

        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
        //    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");

        //    HyperlinkButton Hb = (HyperlinkButton)e.OriginalSource;
        //    string strDetails = objLocalizationManeger.GetValue("lblDetails");
        //    string strListOf = objLocalizationManeger.GetValue("lblListOf");
        //    grpMasterDetails.Header = Hb.Content + strDetails;
        //    ContentControl.Content = strListOf + " " + Hb.Content + " " + strDetails;
        //    MasterTableName = Hb.Name;
        //    mElement.Text = Hb.Content.ToString();

        //    SetupPage();
        //}

    }
}
