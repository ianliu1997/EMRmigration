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
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class PathologyProfileMaster : UserControl
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
        #region Paging


        public PagedSortableCollectionView<clsPathoProfileMasterVO> DataList { get; private set; }
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



        #endregion
        private SwivelAnimation objAnimation;
        public bool IsCancel = true;
        public bool isModify = false;
        int ClickedFlag = 0;
        bool IsNew = false;

        bool IsPageLoded = false;
        /// <summary>
        /// constructor
        /// </summary>
        public PathologyProfileMaster()
        {
            InitializeComponent();
            ResultListContent.Content = new SearchResultLists.SearchResultLists();
            ((SearchResultLists.SearchResultLists)ResultListContent.Content).TableName = MasterTableNameList.M_PathoTestMaster;
            ((SearchResultLists.SearchResultLists)ResultListContent.Content).labelOfDescriptionOnGrid = "Test";



            this.DataContext = new clsPathoProfileMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");

            DataList = new PagedSortableCollectionView<clsPathoProfileMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

        }
        /// <summary>
        /// handles refresh event
        /// </summary>
        /// <param name="sender">MasterList</param>
        /// <param name="e">MasterList changed</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        /// <summary>
        /// fills front panel grid with available patho profiles
        /// </summary>
        private void SetupPage()
        {
            try
            {
                clsGetPathoProfileDetailsBizActionVO BizAction = new clsGetPathoProfileDetailsBizActionVO();
                BizAction.ProfileList = new List<clsPathoProfileMasterVO>();

                if (txtSearch.Text != "")
                {
                    BizAction.Description = txtSearch.Text;
                }



                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if ((clsGetPathoProfileDetailsBizActionVO)arg.Result != null)
                        {
                            if (((clsGetPathoProfileDetailsBizActionVO)arg.Result).ProfileList != null)
                            {
                                clsGetPathoProfileDetailsBizActionVO result = arg.Result as clsGetPathoProfileDetailsBizActionVO;
                                DataList.TotalItemCount = result.TotalRows;

                                if (result.ProfileList != null)
                                {
                                    DataList.Clear();

                                    foreach (var item in result.ProfileList)
                                    {
                                        DataList.Add(item);
                                    }

                                    grdPathorofile.ItemsSource = null;
                                    grdPathorofile.ItemsSource = DataList;

                                    dataGrid2Pager.Source = null;
                                    dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                    dataGrid2Pager.Source = DataList;

                                }

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// sets command button states
        /// </summary>
        /// <param name="strFormMode">string button content</param>
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
                    CmbService.IsEnabled = true;
                    //CmbService.IsEnabled = false;
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
                    CmbService.IsEnabled = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// new button click
        /// </summary>
        /// <param name="sender">new button</param>
        /// <param name="e">new button</param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            //this.grdBackPanel.DataContext = new clsOTTableVO();
            ClearUI();
            GetServicesLinkedToProfileID();
            CmbService.SelectedValue = (long)0;
            isModify = false;
            SetCommandButtonState("New");
            //Validation();
            IsNew = true;

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
          //  Validation();
        }
        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            try
            {
                ResultListContent.Content = null;
                ResultListContent.Content = new SearchResultLists.SearchResultLists();
                ((SearchResultLists.SearchResultLists)ResultListContent.Content).TableName = MasterTableNameList.M_PathoTestMaster;
                ((SearchResultLists.SearchResultLists)ResultListContent.Content).labelOfDescriptionOnGrid = "Test";
                this.DataContext = new clsPathoProfileMasterVO();
                CmbService.SelectedValue = (long)0;
                //grdTest2.ItemsSource = null;
                FetchPathotest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Validates Backpanel
        /// </summary>
        /// <returns>bool</returns>
        ///      
        private bool Validation()
        {
            try
            {
                bool result = true; 
                    //if ((MasterListItem)CmbService.SelectedItem == null)
                    //{
                    //    CmbService.TextBox.SetValidation("Please Select Service");
                    //    CmbService.TextBox.RaiseValidationError();
                    //    CmbService.TextBox.Focus();
                    //    result = false;
                    //}
                    //else if (((MasterListItem)CmbService.SelectedItem).ID == 0)
                    //{
                    //    CmbService.TextBox.SetValidation("Please Select Service");
                    //    CmbService.TextBox.RaiseValidationError();
                    //    CmbService.TextBox.Focus();
                    //    result = false;
                    //}
                    //else
                    //    CmbService.TextBox.ClearValidationError();
                    if ((MasterListItem)CmbService.SelectedItem != null)
                    {
                        CmbService.TextBox.SetValidation("Please Select Service");
                        CmbService.TextBox.RaiseValidationError();
                        CmbService.TextBox.Focus();
                        result = false;
                       
                    }
                    else
                    {
                        CmbService.TextBox.ClearValidationError();
                        result = true;
                       
                    }
                
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool ValidateBackPanel()
        {
            try
            {
                bool result = true;
                //if (txtProfileName.Text == "")
                //{

                //    txtProfileName.SetValidation("Please Enter Description");
                //    txtProfileName.RaiseValidationError();
                //    txtProfileName.Focus();

                //    result = false;
                //}
                //else
                //    txtProfileName.ClearValidationError();


                //if (txtCode.Text == "")
                //{

                //    txtCode.SetValidation("Please Enter Code");
                //    txtCode.RaiseValidationError();
                //    txtCode.Focus();

                //    result = false;
                //}
                //else
                //    txtCode.ClearValidationError();

                if (IsPageLoded)
                {

                    if ((MasterListItem)CmbService.SelectedItem == null)
                    {
                        CmbService.TextBox.SetValidation("Please Select Service");
                        CmbService.TextBox.RaiseValidationError();
                        CmbService.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)CmbService.SelectedItem).ID == 0)
                    {
                        CmbService.TextBox.SetValidation("Please Select Service");
                        CmbService.TextBox.RaiseValidationError();
                        CmbService.Focus();
                        result = false;
                    }
                    else
                        CmbService.TextBox.ClearValidationError();


                    if (((SearchResultLists.SearchResultLists)ResultListContent.Content).lstObjectList == null || ((SearchResultLists.SearchResultLists)ResultListContent.Content).lstObjectList.Count == 0)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Please Select Test";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW.Show();

                        result = false;
                        return result;
                    }

                }
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Gives confirmation message for save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    bool SaveTest = true;
                    SaveTest = ValidateBackPanel();
                    if (SaveTest == true)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to save the pathology profile master?";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                        msgW.Show();
                    }
                    else
                        ClickedFlag = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This function get called if user clicks ok on save message
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                if (CheckDuplicasy())
                {
                    Save();
                }
                else
                {
                    ClickedFlag = 0;
                }



            }

            else
            {
                ClickedFlag = 0;
            }
        }
        /// <summary>
        /// Save pathoTestMaster
        /// </summary>
        private void Save()
        {
            try
            {
                clsAddPathoProfileMasterBizActionVO BizAction = new clsAddPathoProfileMasterBizActionVO();
                BizAction.ProfileDetails = (clsPathoProfileMasterVO)this.DataContext;
                BizAction.ProfileDetails.Status = true;


                if (CmbService.SelectedItem != null)
                    BizAction.ProfileDetails.ServiceID = ((MasterListItem)CmbService.SelectedItem).ID;

                // BizAction.ProfileDetails.PathoTestList = (List<clsPathoProfileTestDetailsVO>)grdTest2.ItemsSource;
                List<clsPathoProfileTestDetailsVO> lstPathoProfileTest = new List<clsPathoProfileTestDetailsVO>();
                foreach (var item in ((SearchResultLists.SearchResultLists)ResultListContent.Content).lstObjectList)
                {
                    clsPathoProfileTestDetailsVO objPathoProfileTest = new clsPathoProfileTestDetailsVO();
                    objPathoProfileTest.TestID = item.ID;
                    lstPathoProfileTest.Add(objPathoProfileTest);

                }
                BizAction.ProfileDetails.PathoTestList = lstPathoProfileTest;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            SetupPage();
                            ClearUI();
                            SetCommandButtonState("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            //Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Profile Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because Profile for the Service already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        //else if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        //{

                        //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgWindow.Show();
                        //}
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding profile master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }


                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Gives modify confirmation message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {

                    bool ModifyTest = true;
                    ModifyTest = ValidateBackPanel();
                    if (ModifyTest == true)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to update pathology profile master?";

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                        msgW1.Show();
                    }
                    else
                        ClickedFlag = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        ///  This function get called if user clicks ok on modify message
        /// </summary>
        /// <param name="result">MessageBoxResult </param>
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {


                    Modify();
                }
                else
                {


                    ClickedFlag = 0;
                }

            }
            else
            {
                ClickedFlag = 0;
            }
        }
        /// <summary>
        /// Modified PathoTestMaster
        /// </summary>
        private void Modify()
        {
            try
            {
                clsAddPathoProfileMasterBizActionVO BizAction = new clsAddPathoProfileMasterBizActionVO();
                BizAction.ProfileDetails = (clsPathoProfileMasterVO)this.DataContext;
                BizAction.ProfileDetails.ServiceID = ((MasterListItem)CmbService.SelectedItem).ID;
                BizAction.ProfileDetails.Status = true;

                List<clsPathoProfileTestDetailsVO> lstPathoProfileTest = new List<clsPathoProfileTestDetailsVO>();
                foreach (var item in ((SearchResultLists.SearchResultLists)ResultListContent.Content).lstObjectList)
                {
                    clsPathoProfileTestDetailsVO objPathoProfileTest = new clsPathoProfileTestDetailsVO();
                    objPathoProfileTest.TestID = item.ID;
                    lstPathoProfileTest.Add(objPathoProfileTest);

                }
                BizAction.ProfileDetails.PathoTestList = lstPathoProfileTest;
                //BizAction.ProfileDetails.PathoTestList = (SearchResultLists.SearchResultLists)ResultListContent.Content)

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            SetupPage();
                            ClearUI();
                            SetCommandButtonState("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            //Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Profile Master updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be updated because Profile for the Service already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        //else if (((clsAddPathoProfileMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        //{

                        //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be updated because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgWindow.Show();
                        //}
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while updating profile master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        /// <summary>
        /// if cancel button click
        /// </summary>
        /// <param name="sender"> Cancel Button</param>
        /// <param name="e">Click</param>
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

                    txtSearch.Text = "";
                    CmbService.SelectedValue = (long)0;
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

        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender">Search Button(object)</param>
        /// <param name="e">Search button click</param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            SetupPage();
            dataGrid2Pager.PageIndex = 0;
        }
        /// <summary>
        /// View button click
        /// </summary>
        /// <param name="sender">View hyperlink</param>
        /// <param name="e">View hyperlink</param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
                ClearUI();
                FetchService1();
               // Validation();
                ResultListContent.Content = new SearchResultLists.SearchResultLists();
                ((SearchResultLists.SearchResultLists)ResultListContent.Content).TableName = MasterTableNameList.M_PathoTestMaster;
                ((SearchResultLists.SearchResultLists)ResultListContent.Content).labelOfDescriptionOnGrid = "Test";
                if (grdPathorofile.SelectedItem != null)
                {
                    IsNew = false;
                    this.DataContext = ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem);
                    //changed by rohini
                     //CmbService.SelectedItem = ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ServiceName;                   
                     cmdModify.IsEnabled = ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).Status;                 
                   
                   
                    if (((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ID > 0)
                    {
                        FillProfileTestListByProfileID(((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ID);
                    }
                    objAnimation.Invoke(RotationType.Forward);
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).Description;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Fills tests related to profile
        /// </summary>
        /// <param name="testID">long profileId</param>
        private void FillProfileTestListByProfileID(long profileID)
        {
            try
            {
                clsGetPathoProfileTestByIDBizActionVO BizAction = new clsGetPathoProfileTestByIDBizActionVO();
                BizAction.ProfileID = profileID;
                BizAction.TestList = new List<clsPathoProfileTestDetailsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        ((SearchResultLists.SearchResultLists)ResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                        clsGetPathoProfileTestByIDBizActionVO DetailsVO = new clsGetPathoProfileTestByIDBizActionVO();
                        DetailsVO = (clsGetPathoProfileTestByIDBizActionVO)arg.Result;

                        List<MasterListItem> testViewList = new List<MasterListItem>();
                        if (DetailsVO.TestList != null)
                        {
                            foreach (var item in DetailsVO.TestList)
                            {
                                MasterListItem TetsObj = new MasterListItem();
                                TetsObj.ID = item.TestID;
                                TetsObj.Description = item.Description;
                                int test = DetailsVO.TestList.IndexOf(item);
                                if (test == 0)
                                {
                                    //TetsObj.IsDefault = false;    //for fist element disable
                                }
                                else
                                {
                                    //TetsObj.IsDefault = true;
                                }
                                //added b y rohini dated 11.4.16
                                if(((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).Status==true )
                                {
                                    if (item.IsEntryFromTestMaster == true)
                                    {
                                        cmdModify.IsEnabled = false;
                                        CmbService.IsEnabled = false;
                                    }
                                    else
                                    {
                                        //cmdModify.IsEnabled = true;
                                        //CmbService.IsEnabled = true;
                                    }
                                }
                                ((SearchResultLists.SearchResultLists)ResultListContent.Content).grid2ViewList.Add(TetsObj);
                            }

                        }


                        ((SearchResultLists.SearchResultLists)ResultListContent.Content).FillGrid2();



                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //added by rohini dated 11.6.16
        List<MasterListItem> ProfileServiceList = new List<MasterListItem>();
        private void GetServicesLinkedToProfileID()
        {
            try
            {
                clsGetPathoProfileServicesBizActionVO BizAction = new clsGetPathoProfileServicesBizActionVO();
                //BizAction.ProfileID = profileID;
                BizAction.ServiceList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        ((SearchResultLists.SearchResultLists)ResultListContent.Content).grid2ViewList = new List<MasterListItem>();
                        clsGetPathoProfileServicesBizActionVO DetailsVO = new clsGetPathoProfileServicesBizActionVO();
                        DetailsVO = (clsGetPathoProfileServicesBizActionVO)arg.Result;

                        ProfileServiceList = new List<MasterListItem>();
                        if (DetailsVO.ServiceList != null)
                        {

                            ProfileServiceList = DetailsVO.ServiceList;
                        }                        
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    FetchService();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// removes test from 2nd test grid
        /// </summary>
        /// <param name="sender">remove button</param>
        /// <param name="e">remove button click</param>
        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsPathoProfileTestDetailsVO> GrdTestList = new List<clsPathoProfileTestDetailsVO>();
                //if (grdTest2.ItemsSource != null)
                //{
                //    foreach (var item in grdTest2.ItemsSource)
                //    {
                //        if (((clsPathoProfileTestDetailsVO)item).Status == false)
                //        {
                //            GrdTestList.Add((clsPathoProfileTestDetailsVO)item);
                //        }
                //    }

                //    grdTest2.ItemsSource = null;
                //    grdTest2.ItemsSource = GrdTestList;
                //}

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Moves parameter list from first parameter grid to second
        /// </summary>
        /// <param name="sender">add button</param>
        /// <param name="e">add button click</param>

        private void cmdAdd1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsPathoProfileTestDetailsVO> TestList = null;

                //if (grdTest2.ItemsSource != null)
                //{
                //    if (TestList == null)
                //    {
                //        TestList = new List<clsPathoProfileTestDetailsVO>();
                //    }
                //    foreach (var item in grdTest2.ItemsSource)
                //    {
                //        //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                //        //    continue;
                //        clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                //        if (obj != null)
                //            continue;

                //        TestList.Add((clsPathoProfileTestDetailsVO)item);
                //    }
                //}
                //else
                //    if (TestList == null)
                //        TestList = new List<clsPathoProfileTestDetailsVO>();


                //if (grdTest1.ItemsSource != null)
                //{
                //    List<clsPathoProfileTestDetailsVO> list = (List<clsPathoProfileTestDetailsVO>)grdTest1.ItemsSource;
                //    foreach (var item in list)
                //    {

                //        if (((clsPathoProfileTestDetailsVO)item).Status == true)
                //        {
                //            clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                //            if (obj != null)
                //                continue;


                //            ((clsPathoProfileTestDetailsVO)item).Status = false;

                //            TestList.Add((clsPathoProfileTestDetailsVO)item);

                //        }
                //    }
                //}

                //grdTest2.ItemsSource = null;

                //    grdTest2.ItemsSource = TestList;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This function fetches patho services
        /// </summary>
        private void FetchService()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                BizAction.GetServicesForPathology = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();
                        List<MasterListItem> objList1 = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        //objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        clsGetServiceMasterListBizActionVO result = e.Result as clsGetServiceMasterListBizActionVO;
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                MasterListItem objServiceItem = new MasterListItem();
                                objServiceItem.ID = item.ID;
                                objServiceItem.Description = item.ServiceName;
                                objServiceItem.Status = item.Status;
                                objList.Add(objServiceItem);
                            }
                        }
                        //added by rohini
                        if (result.ServiceList != null && ProfileServiceList != null)
                        {
                          var NotSentMessages =
                                    from msg in objList
                                    where !ProfileServiceList.Any(x => x.ID == msg.ID)
                                    select msg;
                                      foreach (var item in NotSentMessages)
                                      {
                                          objList1.Add(item);
                                      }
                        }  
                        //
                        //comented by rohini
                        //CmbService.ItemsSource = null;
                        //CmbService.ItemsSource = objList;
                        //CmbService.SelectedItem = objList[0];
                     
                            CmbService.ItemsSource = null;
                            CmbService.ItemsSource = objList1;
                            CmbService.SelectedItem = objList1[0]; 

                        if (this.DataContext != null)
                        {
                            //CmbService.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).ServiceID;

                        }
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void FetchService1()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                BizAction.GetServicesForPathology = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();
                        List<MasterListItem> objList1 = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        //objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        clsGetServiceMasterListBizActionVO result = e.Result as clsGetServiceMasterListBizActionVO;
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                MasterListItem objServiceItem = new MasterListItem();
                                objServiceItem.ID = item.ID;
                                objServiceItem.Description = item.ServiceName;
                                objServiceItem.Status = item.Status;
                                objList.Add(objServiceItem);
                            }
                        }                                            
                            CmbService.ItemsSource = null;
                            CmbService.ItemsSource = objList;
                            //CmbService.SelectedItem = objList[0];
                        if (((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ServiceID != 0)
                        {
                            CmbService.SelectedValue= ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ServiceID;
                        }
                       
                        if (this.DataContext != null)
                        {
                            //CmbService.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).ServiceID;

                        }
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Fetches PathoTests
        /// </summary>
        private void FetchPathotest()
        {

            try
            {
                //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                ////BizAction.IsActive = true;
                //BizAction.MasterTable = MasterTableNameList.M_PathoTestMaster;
                //BizAction.MasterList = new List<MasterListItem>();

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<clsPathoProfileTestDetailsVO> objList = new List<clsPathoProfileTestDetailsVO>();

                        foreach (var item in ((clsGetMasterListBizActionVO)e.Result).MasterList)
                        {
                            clsPathoProfileTestDetailsVO objPathoProfileTest = new clsPathoProfileTestDetailsVO();
                            objPathoProfileTest.TestID = item.ID;
                            objPathoProfileTest.Description = item.Description;

                            objList.Add(objPathoProfileTest);


                        }

                        //grdTest1.ItemsSource = null;
                        //grdTest1.ItemsSource = objList;

                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// First test grid test checked,status = true
        /// </summary>
        /// <param name="sender">ChkGrdTest1 </param>
        /// <param name="e">ChkGrdTest1 click</param>

        private void ChkGrdTest1_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (grdTest1.SelectedItem != null)
                //{
                //    ((clsPathoProfileTestDetailsVO)grdTest1.SelectedItem).Status = false;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// First test grid test unchecked,status = true
        /// </summary>
        /// <param name="sender">ChkGrdTest1 </param>
        /// <param name="e">ChkGrdTest1 click</param>
        private void ChkGrdTest1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (grdTest1.SelectedItem != null)
                //{
                //    ((clsPathoProfileTestDetailsVO)grdTest1.SelectedItem).Status = true;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void grdTestChkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (grdTest2.SelectedItem != null)
                //{
                //    ((clsPathoProfileTestDetailsVO)grdTest2.SelectedItem).Status = true;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void grdTestChkBox_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (grdTest2.SelectedItem != null)
                //{
                //    ((clsPathoProfileTestDetailsVO)grdTest2.SelectedItem).Status =false;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsPageLoded)
                {
                    this.DataContext = new clsPathoProfileMasterVO();

                    //comented by rohini 
                   // FetchService();
                    //GetServicesLinkedToProfileID();
                    FetchPathotest();
                    SetupPage();
                }
                IsPageLoded = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            #region Commented
            //clsPathoProfileMasterVO Item;

            //if (IsNew)
            //{
            //    Item = ((PagedSortableCollectionView<clsPathoProfileMasterVO>)grdPathorofile.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)CmbService.SelectedItem).ID));

            //}
            //else
            //{
            //    Item = ((PagedSortableCollectionView<clsPathoProfileMasterVO>)grdPathorofile.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)CmbService.SelectedItem).ID) && p.ID != ((clsPathoProfileMasterVO)grdPathorofile.SelectedItem).ID);

            //}

            //if (Item != null)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //               new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because SERVICE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    return false;
            //}

            //else
            //{
            //    return true;
            //}
            #endregion


            return true;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
                dataGrid2Pager.PageIndex = 0;
            }
        }


        
    }
}
