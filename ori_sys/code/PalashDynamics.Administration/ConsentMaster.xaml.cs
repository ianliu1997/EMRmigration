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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Administration;

using Liquid;
using Liquid.Components;
using Liquid.Components.Internal;

using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class ConsentMaster : UserControl, IPreInitiateCIMS
    {

        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

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
        private SwivelAnimation objAnimation;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        public bool isModify = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsentMaster()
        {
            try
            {
                InitializeComponent();
                this.DataContext = new clsConsentMasterVO();
                objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                this.Loaded += new RoutedEventHandler(ConsentMaster_Loaded);
                SetCommandButtonState("Load");

                MasterList = new PagedSortableCollectionView<clsConsentMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                this.dataGrid2Pager.DataContext = MasterList;
                grdConsentable.DataContext = MasterList;
                PageSize = 15;
                SetupPage();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }
        void ConsentMaster_Loaded(object sender, RoutedEventArgs e)
        {
            this.richToolbar.RichTextBox = TextEditor;
            FillContentType();
            if (grdConsentable != null)
            {
                if (grdConsentable.Columns.Count > 0)
                {

                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        grdConsentable.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdView");
                        grdConsentable.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdCode");
                        grdConsentable.Columns[2].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDescription");                       
                        grdConsentable.Columns[3].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdStatus");
                    }
                    else
                    {
                        grdConsentable.Columns[0].Header = "View";
                        grdConsentable.Columns[1].Header = "Code";
                        grdConsentable.Columns[2].Header = "Description";                        
                        grdConsentable.Columns[3].Header = "Status";
                    }
                }
            }




        }
        /// <summary>
        /// Front panel consent master grid refreshed
        /// </summary>
        /// <param name="sender">Front panel consent master grid</param>
        /// <param name="e">Front panel consent master grid refresh</param>
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        /// <summary>
        /// Fills patient config combo with patinet related fields
        /// </summary>
        private void GetPatientConfigFields()
        {
            try
            {
                clsGetPatientConfigFieldsBizActionVO bizActionVO = new clsGetPatientConfigFieldsBizActionVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsPatientFieldsConfigVO> objList = new List<clsPatientFieldsConfigVO>();
                        //objList = 
                        //objList.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);
                        clsPatientFieldsConfigVO obj = new clsPatientFieldsConfigVO();
                        obj.ID = 0;
                        obj.FieldName = "--Select--";
                        objList.Add(obj);
                        objList.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);

                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];
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
        public PagedSortableCollectionView<clsConsentMasterVO> MasterList { get; private set; }
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
        clsConsentMasterVO getconsentinfo;
        /// <summary>
        /// Fills front panel grid with consent master
        /// </summary>
        public void GetConsentMasterDetails()
        {
            try
            {
                clsGetConsentMasterBizActionVO bizActionVO = new clsGetConsentMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getconsentinfo = new clsConsentMasterVO();
                bizActionVO.ConsentMatserDetails = new List<clsConsentMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ConsentMatserDetails = (((clsGetConsentMasterBizActionVO)args.Result).ConsentMatserDetails);
                        if (bizActionVO.ConsentMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetConsentMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsConsentMasterVO item in bizActionVO.ConsentMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch
            {
 
            }
        }
        /// <summary>
        /// Fills required fields on consent master form
        /// </summary>
        public void SetupPage()
        {
            GetConsentMasterDetails();
            GetPatientConfigFields();
        }
        /// <summary>
        /// New button click. Rotates form to back panel
        /// </summary>
        /// <param name="sender">new button</param>
        /// <param name="e">New button click</param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsConsentMasterVO();
            ClearUI();

            isModify = false;
            SetCommandButtonState("New");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsCancel = true;
        /// <summary>
        /// Sets command button sets
        /// </summary>
        /// <param name="strFormMode">Command button content</param>
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdPrint.IsEnabled = false;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPrint.IsEnabled = false;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPrint.IsEnabled = false;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdPrint.IsEnabled = false;
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
                    cmdPrint.IsEnabled = false;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdPrint.IsEnabled = false;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    cmdPrint.IsEnabled = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;
                case "ConsentPrinting":
                    cmdAdd.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdPrint.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Gives confirmation message for save
        /// </summary>
        /// <param name="sender">Save button</param>
        /// <param name="e">Save button click</param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                //}
                //else
                //{
                    msgText = "Are you sure you want to Save?";
                //}            

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();
            }
        }

        /// <summary>
        /// Saves Consent Master
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsAddUpdateConsentMasterBizActionVO bizactionVO = new clsAddUpdateConsentMasterBizActionVO();
                    clsConsentMasterVO addNewConsentMasterVO = new clsConsentMasterVO();

                    isModify = false;
                    addNewConsentMasterVO = (clsConsentMasterVO)this.DataContext;
                    addNewConsentMasterVO = CreateFormData();
                    
                    //Added By Ashish Thombre
                    //if (CmbContentType.SelectedItem != null)
                    //{
                    //    if (((MasterListItem)CmbContentType.SelectedItem).ID > 0)
                    //        addNewConsentMasterVO.ConsentType = ((MasterListItem)CmbContentType.SelectedItem).ID;
                    //}

                    //addNewConsentMasterVO.TemplateName = TextEditor.Html;
                    //addNewConsentMasterVO.TemplateName = txtDescription.Text;
                    //addNewConsentMasterVO.Description = txtDescription.Text;

                    //addNewConsentMasterVO.TemplateName = txtDescription.Text;
                    //addNewConsentMasterVO.Description = TextEditor.Html;

                    addNewConsentMasterVO.TemplateName = TextEditor.Html;
                    addNewConsentMasterVO.Description = txtDescription.Text;
                    addNewConsentMasterVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewConsentMasterVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewConsentMasterVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewConsentMasterVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewConsentMasterVO.AddedDateTime = System.DateTime.Now;
                    addNewConsentMasterVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    bizactionVO.OTTableMasterMatserDetails.Add(addNewConsentMasterVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record Saved Successfully.";
                                //}
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                ConsentMasterID = 0;
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record cannot be save because CODE already exist!";

                                //}
                                
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescDuplicate_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record cannot be save because Description already exist!";

                                //}                                
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        long ConsentMasterID = 0;
        /// <summary>
        /// Gets Consent master details
        /// </summary>
        /// <returns>clsConsentMasterVO object</returns>
        private clsConsentMasterVO CreateFormData()
        {
            clsConsentMasterVO addNewConsentMasterVO = new clsConsentMasterVO();
            if (isModify == true)
            {
                addNewConsentMasterVO.ID = ConsentMasterID;
                addNewConsentMasterVO = ((clsConsentMasterVO)grdConsentable.SelectedItem);
            }
            else
            {
                addNewConsentMasterVO.ID = 0;
                addNewConsentMasterVO = (clsConsentMasterVO)this.grdBackPanel.DataContext;
                addNewConsentMasterVO.Status = true;
            }

            



            return addNewConsentMasterVO;
        }
        #region Validation
        /// <summary>
        /// validates back panel
        /// </summary>
        /// <returns>true or false</returns>
        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Please EnterTemplate Name");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                return false;
            }
            else if (TextEditor.Html == "")
            {
                //richTextEditor.SetValidation("Please enter descrption");
                //richTextEditor.RaiseValidationError();
                //richTextEditor.Focus();
                return false;

            }

            else
            {
                txtStoreCode.ClearValidationError();
                txtDescription.ClearValidationError();
                //richTextEditor.ClearValidationError();
                return true;
            }

        }


        #endregion
        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            //txtStoreCode.Text = "";
            //txtStoreName.Text = "";

            txtStoreCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            //if (CmbContentType.ItemsSource != null)
            //{
            //    CmbContentType.SelectedItem = ((List<MasterListItem>)CmbContentType.ItemsSource)[0];
            //}
            //CmbOtTheatre.SelectedValue = ((clsOTTableVO)this.DataContext).OTTheatreID;
            this.grdBackPanel.DataContext = new clsConsentMasterVO();
            CmbOtTheatre.SelectedValue = ((clsConsentMasterVO)this.DataContext).ID;
            TextEditor.Html = string.Empty;
        }
        /// <summary>
        /// gives confirmation message for modify
        /// </summary>
        /// <param name="sender">modify button</param>
        /// <param name="e">modify button click</param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("UpdateVerification_Msg");
                    //}
                    //else
                    //{
                        msgText = "Are you sure you want to Update ?";
                    //}                   

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Modifies Consent details
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateConsentMasterBizActionVO bizactionVO = new clsAddUpdateConsentMasterBizActionVO();
                clsConsentMasterVO addNewConsentVO = new clsConsentMasterVO();
                try
                {
                    isModify = true;
                    addNewConsentVO = CreateFormData();

                    //Added By Ashish Thombre
                    //if (CmbContentType.SelectedItem != null)
                    //{
                    //    if (((MasterListItem)CmbContentType.SelectedItem).ID > 0)
                    //        addNewConsentVO.ConsentType = ((MasterListItem)CmbContentType.SelectedItem).ID;
                    //}

                    //addNewConsentVO.TemplateName = TextEditor.Html;
                    //addNewConsentVO.Description = txtDescription.Text;

                    addNewConsentVO.TemplateName = txtDescription.Text;
                    addNewConsentVO.Description = TextEditor.Html;

                    addNewConsentVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewConsentVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewConsentVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewConsentVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewConsentVO.UpdatedDateTime = System.DateTime.Now;
                    addNewConsentVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.OTTableMasterMatserDetails.Add(addNewConsentVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordModify_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record Updated Successfully.";
                                //}                           

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                               ConsentMasterID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record cannot be save because CODE already exist!";

                                //}                               
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateConsentMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                //{
                                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescDuplicate_Msg");
                                //}
                                //else
                                //{
                                    msgText = "Record cannot be save because Description already exist!";

                                //} 
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
        /// Cancel button click if on bak panel moves to front panel
        /// if on front panel moves to config screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                SetupPage();
                
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmOTConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Patient Configuration";

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
        /// <summary>
        /// Searching Consent
        /// </summary>
        /// <param name="sender">Search button</param>
        /// <param name="e">Search button click</param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsConsentMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdConsentable.DataContext = MasterList;
                SetupPage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Displays details of selected consent on front panel grid
        /// </summary>
        /// <param name="sender">view hyperlink</param>
        /// <param name="e">view hyperlink click</param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");              

                if (grdConsentable.SelectedItem != null)
                {
                    ConsentMasterID = ((clsConsentMasterVO)grdConsentable.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsConsentMasterVO)grdConsentable.SelectedItem);

                    //TextEditor.Html = ((clsConsentMasterVO)grdConsentable.SelectedItem).TemplateName;
                    TextEditor.Html = ((clsConsentMasterVO)grdConsentable.SelectedItem).Description;
                   
                   // added by kiran 4/4/13
                    if (((clsConsentMasterVO)grdConsentable.SelectedItem).Description != null && ((clsConsentMasterVO)grdConsentable.SelectedItem).Description != " ")
                    {
                        //txtDescription.Text = ((clsConsentMasterVO)grdConsentable.SelectedItem).Description;
                        txtDescription.Text = ((clsConsentMasterVO)grdConsentable.SelectedItem).TemplateName;
                    }
                    //CmbOtTheatre.SelectedItem=((clsConsentMasterVO)grdConsentable.SelectedItem).
                    //Added By Ashish Thombre
                    //CmbContentType.SelectedValue = ((clsConsentMasterVO)grdConsentable.SelectedItem).ConsentType;
                    
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public clsConsentMasterVO consentPrintingObj { get; set; }
        public long consentPrintingID { get; set; }
        public string consentPrintingHtml { get; set; }
        public void ConsentPrinting()
        {
            try
            {
                SetCommandButtonState("ConsentPrinting");
                ConsentMasterID = consentPrintingID;
                grdBackPanel.DataContext = consentPrintingObj;
                TextEditor.Html = consentPrintingHtml;


                objAnimation.Invoke(RotationType.Forward);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Special characters are not allowed.";

                    //}                    
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtStoreCode.Text = txtStoreCode.Text.ToTitleCase();
                }
            }
        }
        /// <summary>
        /// gets richtext editor
        /// </summary>
        //public Liquid.RichTextEditor TextEditor
        //{
        //    get { return richTextEditor; }
        //}

        /// <summary>
        /// gets richtext box
        /// </summary>
        //public Liquid.RichTextBox rt
        //{
        //    get { return richTextEditor.TextBox; }
        //}

        //Added By Ashish
        private void FillContentType()
        {
            //try
            //{
            //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //    BizAction.MasterTable = MasterTableNameList.M_ConsentType;
            //    BizAction.MasterList = new List<MasterListItem>();

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, e) =>
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "- Select -"));

            //        if (e.Error == null && e.Result != null)
            //        {
            //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
            //        }


            //        CmbContentType.ItemsSource = null;
            //        CmbContentType.ItemsSource = objList;
            //        CmbContentType.SelectedValue = objList[0].ID;
                    
            //    };

            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();
            //}
            //catch (Exception)
            //{

            //}
        }

       

     /// <summary>
     /// adds patient config fields to the rich text area
     /// </summary>
     /// <param name="sender">add button </param>
        /// <param name="e">add button  click</param>
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CmbOtTheatre.SelectedItem != null && ((clsPatientFieldsConfigVO)CmbOtTheatre.SelectedItem).ID != 0)
                {
                    //Point str = TextEditor.Html;

                    //TextEditor.SelectAll();

                    string str1 = TextEditor.Html;
                    //string str2 = "<span style=" + "color:#999999;" + ">" + " {" + CmbOtTheatre.SelectedItem.ToString() + "}</span>";
                    string str2 = " {" + CmbOtTheatre.SelectedItem.ToString() + "}";

                    //TextEditor.SelectedText = String.Empty;
                    
                     String html = TextEditor.Html;
                     string s = TextEditor.Html;
                   
                    //Liquid.RichTextBox rt = TextEditor.TextBox;
                     
                    
                   
                    if (TextEditor.Html != null )
                    {

                        if (TextEditor.Html == "")
                            TextEditor.Html = "<p style=\"margin:0px;\">" + str2 + "</p>\r";
                            //TextEditor.Html = "<p style=\"margin:0px;\"><span class=\"Normal\">" + str2 + "</span></p>\r";
                        else
                        {
                            string find = html.Insert(html.LastIndexOf("</p>") , str2);
                            //string find = html + str2;

                            TextEditor.Html = find;
                        }

                    }
                   
                  
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //long ConsentID = 0;
            //try
            //{
               
                  
            //            ConsentID = consentPrintingID;

            //            string URL = "../Reports/OperationTheatre/ConsentPrintingMIS.aspx?ConsentID=" + ConsentID + "&Date=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).Date + "&PatientID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).PatientID + "&UnitID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).UnitID + "&TemplateName=" + TextEditor.Html;
            //            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                   
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            try
            {

                clsAddPatientWiseConsentPrintingBizActionVO bizactionVO = new clsAddPatientWiseConsentPrintingBizActionVO();
                bizactionVO.ConsetPrintingObj.UnitID = ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).UnitID;
                bizactionVO.ConsetPrintingObj.PatientID = ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).PatientID;
                bizactionVO.ConsetPrintingObj.ProcDate = (DateTime)((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).Date;
                bizactionVO.ConsetPrintingObj.ConsentID = consentPrintingID;
                bizactionVO.ConsetPrintingObj.TemplateName = TextEditor.Html;

             

                
                  
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            string URL = "../Reports/OperationTheatre/ConsentPrintingMIS.aspx?ConsentID=" + consentPrintingID + "&Date=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).Date + "&PatientID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).PatientID + "&UnitID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).UnitID ;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                            
                        }
                    };
                    client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
              

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        

        private void txtcode_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e,false);
           
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);

                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecCharNotValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Only & ,.,- and space special characters are allowed.";
                    //}                     
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtDescription.Text = txtDescription.Text.ToTitleCase();
                }
            }
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        //private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
        //    {
        //        if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
        //        {
        //            ((TextBox)e.OriginalSource).Text = string.Empty;
        //            ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
        //            msgText = "Only & ,.,- and space special characters are allowed.";
        //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgWindow.Show();
        //        }
        //        else
        //        {
        //            ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
        //        }
        //    }
        //}

        //private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e,true);
        //}

       
    }
}
