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
using System;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.Master;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Pharmacy;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration
{
    public partial class PathoSubTestMaster : UserControl
    {
        public PathoSubTestMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsPathoTestMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

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

        public PagedSortableCollectionView<clsPathoTestMasterVO> DataList { get; private set; }

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
        WaitIndicator indicator = new WaitIndicator();
        public bool isModify = false;
        bool IsPageLoded = false;
        bool IsCancel = true;
        bool IsNew = false;
        int ClickedFlag = 0;
        bool chkIsPara = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        bool isView = false;
        bool subTestParam = false;
        public ObservableCollection<MasterListItem> ParameterList { get; set; }
        long SampleID = 0;
        public List<clsPathoTestParameterVO> ParamaterViewList = new List<clsPathoTestParameterVO>();
       
        long ModifyTestId = 0;

        /// <summary>
        /// Constructor
        /// </summary>
       

        /// <summary>
        /// Used when refresh event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        /// <summary>
        /// Fetches Patho test & fills front panel grid
        /// </summary>
        private void FetchData()
        {
            try
            {
                indicator.Show();
                clsGetPathoSubTestMasterBizActionVO BizAction = new clsGetPathoSubTestMasterBizActionVO();
                BizAction.TestList = new List<clsPathoTestMasterVO>();

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
                        if (((clsGetPathoSubTestMasterBizActionVO)arg.Result).TestList != null)
                        {
                            clsGetPathoSubTestMasterBizActionVO result = arg.Result as clsGetPathoSubTestMasterBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.TestList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.TestList)
                                {
                                    DataList.Add(item);

                                }

                                grdPathoTest.ItemsSource = null;
                                grdPathoTest.ItemsSource = DataList;

                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                dataGrid2Pager.Source = DataList;

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
            finally
            {
                indicator.Close();
            }
        }

        /// <summary>
        /// New button click
        /// </summary>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            ClearUI();

            IsNew = true;
            TabControlMain.SelectedIndex = 0;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Sub Test Details";
            objAnimation.Invoke(RotationType.Forward);

        }

        /// <summary>
        /// This function performs activities if sub test is checked
        /// </summary>
        /// <param name="sender"> IsSubTestChecked window </param>
        /// <param name="e">IsSubTestChecked window  ok button or cancel button click</param>
        private void chekSubTestWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckIsSubPathoTest subtestWin = (CheckIsSubPathoTest)sender;
                if (subtestWin.Checked == true)
                {

                    txtblServiceName.Visibility = System.Windows.Visibility.Collapsed;
                    CmbServiceName.Visibility = System.Windows.Visibility.Collapsed;

                    txtblCategory.Visibility = System.Windows.Visibility.Collapsed;
                    CmbCategory.Visibility = System.Windows.Visibility.Collapsed;

                    TabTestSampleDetails.Visibility = System.Windows.Visibility.Collapsed;
                    //TabTestItemDetails.Visibility = System.Windows.Visibility.Collapsed;
                    ((clsPathoTestMasterVO)this.DataContext).IsSubTest = true;
                    rdbParameter.IsChecked = false;
                    rdbSubTest.IsChecked = true;
                }
                else
                {
                    //txtblServiceName.Visibility = System.Windows.Visibility.Visible;
                    //CmbServiceName.Visibility = System.Windows.Visibility.Visible;

                    txtblCategory.Visibility = System.Windows.Visibility.Visible;
                    CmbCategory.Visibility = System.Windows.Visibility.Visible;

                    TabTestSampleDetails.Visibility = System.Windows.Visibility.Visible;
                    //TabTestItemDetails.Visibility = System.Windows.Visibility.Visible;
                    ((clsPathoTestMasterVO)this.DataContext).IsSubTest = false;
                    rdbParameter.IsChecked = true;
                    rdbSubTest.IsChecked = false;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Reset Controls on the form
        /// </summary>
        private void ResetControls()
        {
            try
            {
                //if (txtblServiceName.Visibility == System.Windows.Visibility.Collapsed)
                    //txtblServiceName.Visibility = System.Windows.Visibility.Visible;
                //if (CmbServiceName.Visibility == System.Windows.Visibility.Collapsed)
                //    CmbServiceName.Visibility = System.Windows.Visibility.Visible;

                if (txtblCategory.Visibility == System.Windows.Visibility.Collapsed)
                    txtblCategory.Visibility = System.Windows.Visibility.Visible;
                if (CmbCategory.Visibility == System.Windows.Visibility.Collapsed)
                    CmbCategory.Visibility = System.Windows.Visibility.Visible;

                if (TabTestSampleDetails.Visibility == System.Windows.Visibility.Collapsed)
                    TabTestSampleDetails.Visibility = System.Windows.Visibility.Visible;
                //if (TabTestItemDetails.Visibility == System.Windows.Visibility.Collapsed)
                //    TabTestItemDetails.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                throw;
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
                    this.IsCancel = true;
                    cmdMachine.IsEnabled = true;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdMachine.IsEnabled = false;
                    IsCancel = false;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    IsCancel = true;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdMachine.IsEnabled = false;
                    IsCancel = false;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

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
                        string msgText = "Are you sure you want to save the Sub Test Master?";

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
                indicator.Show();
                clsAddPathoTestMasterBizActionVO BizAction = new clsAddPathoTestMasterBizActionVO();
                BizAction.TestDetails = (clsPathoTestMasterVO)grdBackPanel.DataContext;
                BizAction.TestDetails.IsSubTest = true;
                BizAction.TestDetails.Status = true;
                BizAction.TestDetails.IsFromParameter = true;
                if (CmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;

                if (CmbServiceName.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)CmbServiceName.SelectedItem).ID;

                BizAction.TestDetails.IsParameter = true;

                if (chkIsPara == true)
                {
                    BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;


                }
                else
                {
                    BizAction.TestDetails.TestParameterList = ParamaterViewList;
                    BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                }

                BizAction.TestDetails.TestItemList = ItemList1.ToList();
                BizAction.TestDetails.TestSampleList = SampleList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Load");
                            objAnimation.Invoke(RotationType.Backward);
                            //Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Sub Test Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Sub Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
                indicator.Close();
            }
            finally
            {
                indicator.Close();
            }

        }

       

        /// <summary>
        /// Validates Backpanel
        /// </summary>
        /// <returns>bool</returns>
        private bool Validation()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))  
            {

                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();


            if (string.IsNullOrEmpty(txtCode.Text.Trim()))           
            {

                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                result = false;
            }
            else
                txtCode.ClearValidationError();
            return result;

        }
        private bool ValidateBackPanel()
        {
            try
            {
                bool result = true;
                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {

                    txtDescription.SetValidation("Please Enter Description");
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();

                    result = false;
                }
                else
                    txtDescription.ClearValidationError();

                if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                {

                    txtCode.SetValidation("Please Enter Code");
                    txtCode.RaiseValidationError();
                    txtCode.Focus();

                    result = false;
                }
                else
                    txtCode.ClearValidationError();


                if (IsPageLoded)
                {
                    //if (((clsPathoTestMasterVO)this.DataContext).IsSubTest == false)
                    //{
                    
                      if ((MasterListItem)CmbCategory.SelectedItem == null)
                        {
                            CmbCategory.TextBox.SetValidation("Please Select Category");
                            CmbCategory.TextBox.RaiseValidationError();
                            CmbCategory.TextBox.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                        {
                            CmbCategory.TextBox.SetValidation("Please Select Category");
                            CmbCategory.TextBox.RaiseValidationError();
                            CmbCategory.TextBox.Focus();
                            result = false;
                        }
                        else
                            CmbCategory.TextBox.ClearValidationError();


                        if (grdPathoParameter.ItemsSource == null)
                        {
                            string msgTitle = "Palash";
                            string msgText = "Please Select Parameter";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW.Show();
                            TabControlMain.SelectedIndex = 1;
                            result = false;
                            return result;
                        }

                        //if (ItemList1.Count == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can't Save Test Master Without Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //    msgW1.Show();
                        //    result = false;
                        //    TabControlMain.SelectedIndex = 3;
                        //    return result;

                        //}

                        
                    //}




                }
                return result;

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
                        string msgText = "Are you sure you want to Update the Sub Test Master?";

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
                indicator.Show();
                clsAddPathoTestMasterBizActionVO BizAction = new clsAddPathoTestMasterBizActionVO();
                BizAction.TestDetails = (clsPathoTestMasterVO)grdBackPanel.DataContext;
                BizAction.TestDetails.IsSubTest = true;
                BizAction.TestDetails.Status = true;
                BizAction.TestDetails.IsFromParameter = true;
                if (CmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;

                if (CmbServiceName.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)CmbServiceName.SelectedItem).ID;



                BizAction.TestDetails.IsParameter = true;



                if (chkIsPara == true)
                {
                    BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;

                }
                else
                {
                    BizAction.TestDetails.TestParameterList = ParamaterViewList;
                    BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                }

                BizAction.TestDetails.TestItemList = ItemList1.ToList();
                BizAction.TestDetails.TestSampleList = SampleList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        //if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        //{
                        FetchData();
                        ClearUI();
                        SetCommandButtonState("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Sub Test Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        //}
                        //else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        //{

                        //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgWindow.Show();
                        //}
                        //else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        //{

                        //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgWindow.Show();
                        //}
                        indicator.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Sub Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                        indicator.Close();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
                indicator.Close();
            }
            finally
            {
                //indicator.Close();


            }

        }
 
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
                }
                else
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = "  ";
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
            FetchData();
            dataGrid2Pager.PageIndex = 0;
        }

        /// <summary>
        /// New button click
        /// </summary>
        /// <param name="sender">New button</param>
        /// <param name="e">New button click</param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            ClearUI();
            //BY ROHINI
            textBefore = "";
            selectionStart = 0;
            selectionLength = 0;

            Validation();
            ResetControls();
            if (txtCompletionTime.Text == "0")
                txtCompletionTime.Text = "";
          
            txtblCategory.Visibility = System.Windows.Visibility.Visible;
            CmbCategory.Visibility = System.Windows.Visibility.Visible;

            TabTestSampleDetails.Visibility = System.Windows.Visibility.Visible;
            //TabTestItemDetails.Visibility = System.Windows.Visibility.Visible;
       
            rdbParameter.IsChecked = true;
            IsNew = true;

            TabControlMain.SelectedIndex = 0;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Sub Test Details";
            objAnimation.Invoke(RotationType.Forward);

        }

        /// <summary>
        /// Clears UI
        /// </summary>
        private void ClearUI()
        {
            try
            {
                txtSearch.Text = "";
                txtCompletionTime.Text = "";
                this.DataContext = new clsPathoTestMasterVO();
                CmbServiceName.SelectedValue = (long)0;
                CmbCategory.SelectedValue = (long)0;
                cmbSample.SelectedValue = (long)0;
                cmbMachine.SelectedValue = (long)0;
                //txtSampleQuantity.Text = "";
                txtFrequency.Text = "";
              
                ItemList1 = new ObservableCollection<clsPathoTestItemDetailsVO>();
                SampleList = new ObservableCollection<clsPathoTestSampleVO>();
                ParameterList = new ObservableCollection<MasterListItem>();
                ParamaterViewList = new List<clsPathoTestParameterVO>();
               
                grdPathoParameter.ItemsSource = null;
                FetchPathoParameterList();
                dgSampleDetailsList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = null;
                rdbParameter.IsChecked = true;
                this.DataContext = new clsPathoTestMasterVO();

                isView = false;
                subTestParam = false;


            }
            catch (Exception ex)
            {
                throw;
            }
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
                isView = true;
                if (grdPathoTest.SelectedItem != null)
                {
                    IsNew = false;
                    this.DataContext = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).DeepCopy();
                    if (((clsPathoTestMasterVO)this.DataContext).IsSubTest == true)
                    {
                        txtblServiceName.Visibility = System.Windows.Visibility.Collapsed;
                        CmbServiceName.Visibility = System.Windows.Visibility.Collapsed;

                        txtblCategory.Visibility = System.Windows.Visibility.Visible;
                        CmbCategory.Visibility = System.Windows.Visibility.Visible;

                        TabTestSampleDetails.Visibility = System.Windows.Visibility.Visible;
                        //TabTestItemDetails.Visibility = System.Windows.Visibility.Visible;
                        rdbParameter.IsChecked = true;
                        
                    }
                   
                    CmbCategory.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).CategoryID;
                    CmbServiceName.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ServiceID;
                    cmbMachine.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).MachineID;
                    if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID > 0)
                    {
                        ModifyTestId = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID;
                        FillParameterSampleAndITemList(((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID);
                    }
                    objAnimation.Invoke(RotationType.Forward);
                    TabControlMain.SelectedIndex = 0;
                }

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).Description;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills Parameter,Sample & Items related to test id
        /// </summary>
        /// <param name="testID">long testid</param>

       
        /// <summary>
        /// Fills samples,parameter,Items for test is
        /// </summary>
        private void FillParameterSampleAndITemList(long testID)
        {
            try
            {
                clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO BizAction = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                BizAction.ParameterList = new List<clsPathoTestParameterVO>();
                BizAction.ItemList = new List<clsPathoTestItemDetailsVO>();
                BizAction.SampleList = new List<clsPathoTestSampleVO>();

                BizAction.IsFormSubTest = true;
                BizAction.TestID = testID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO DetailsVO = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                        DetailsVO = (clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO)arg.Result;

                        if (DetailsVO.ParameterList != null)
                        {
                            grdPathoParameter.ItemsSource = null;
                            grdPathoParameter.ItemsSource = DetailsVO.ParameterList;
                           
                            ParamaterViewList = DetailsVO.ParameterList;
                        }

                        


                        if (DetailsVO.SampleList != null)
                        {

                            foreach (var item in DetailsVO.SampleList)
                            {
                                SampleList.Add(item);
                            }
                            dgSampleDetailsList.ItemsSource = null;
                            dgSampleDetailsList.ItemsSource = SampleList;
                            dgSampleDetailsList.Focus();
                            dgSampleDetailsList.UpdateLayout();
                        }
                        if (DetailsVO.ItemList != null)
                        {

                            foreach (var item in DetailsVO.ItemList)
                            {
                                ItemList1.Add(item);

                                //added by rohini dated 10.2.16
                                if (item.UID != null || item.UID != 0)
                                {
                                    ((clsPathoTestItemDetailsVO)item).UsageList = Ulist;
                                    (((clsPathoTestItemDetailsVO)item).SelectedUID).ID = item.UID;
                                    (((clsPathoTestItemDetailsVO)item).SelectedUID).Description = item.UName;
                                }
                                if (item.DID != null || item.DID != 0)
                                {
                                    ((clsPathoTestItemDetailsVO)item).DeductionList = Dlist;
                                    (((clsPathoTestItemDetailsVO)item).SelectedDID).ID = item.DID;
                                    (((clsPathoTestItemDetailsVO)item).SelectedDID).Description = item.DName;
                                }
                                if (item.UOMid != null || item.UOMid != 0)
                                {
                                    (((clsPathoTestItemDetailsVO)item).SelectedUOM).ID = item.UOMid;
                                    (((clsPathoTestItemDetailsVO)item).SelectedUOM).Description = item.UOMName;
                                }
                                //
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList1;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {

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
                if (chkIsPara == true)
                {
                    List<clsPathoTestParameterVO> PathoParameterList = null;

                    if (grdPathoParameter.ItemsSource != null)
                    {
                        if (PathoParameterList == null)
                        {
                            PathoParameterList = new List<clsPathoTestParameterVO>();
                        }
                        foreach (var item in grdPathoParameter.ItemsSource)
                        {
                            //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                            //    continue;
                            clsPathoTestParameterVO obj = PathoParameterList.FirstOrDefault(q => q.ParamSTID == ((clsPathoTestParameterVO)item).ParamSTID);
                            if (obj != null)
                                continue;

                            PathoParameterList.Add((clsPathoTestParameterVO)item);
                        }
                    }
                    else
                        if (PathoParameterList == null)
                            PathoParameterList = new List<clsPathoTestParameterVO>();
                    long position = 0;
                    if (grdPathoParameter.ItemsSource != null && ((List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource).Count != 0)
                    {
                        position = ((List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource)[((List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource).Count - 1].PrintPosition;
                        position = position + 1;
                    }
                    else
                        position = 0;

                    if (grdParameterMaster.ItemsSource != null)
                    {
                        List<clsPathoTestParameterVO> list = (List<clsPathoTestParameterVO>)grdParameterMaster.ItemsSource;
                        foreach (var item in list)
                        {

                            if (((clsPathoTestParameterVO)item).Status == true)
                            {
                                clsPathoTestParameterVO obj = PathoParameterList.FirstOrDefault(q => q.ParamSTID == ((clsPathoTestParameterVO)item).ParamSTID);
                                if (obj != null)
                                    continue;

                                //((clsPathoTestParameterVO)item).Status = false;
                                MasterListItem objMasterList = new MasterListItem(0, "--Select--");
                                ((clsPathoTestParameterVO)item).SelectedPrintName = objMasterList;
                                ((clsPathoTestParameterVO)item).Status = false;
                                ((clsPathoTestParameterVO)item).PrintPosition = position;
                                ((clsPathoTestParameterVO)item).IsParameter = chkIsPara;
                                PathoParameterList.Add((clsPathoTestParameterVO)item);
                                position = position + 1;
                            }
                        }
                    }

                    grdPathoParameter.ItemsSource = null;
                    grdPathoParameter.ItemsSource = PathoParameterList;
                }
                else
                {
                    List<clsPathoSubTestVO> PathoParameterList = null;

                    if (grdPathoParameter.ItemsSource != null)
                    {
                        if (PathoParameterList == null)
                        {
                            PathoParameterList = new List<clsPathoSubTestVO>();
                        }
                        foreach (var item in grdPathoParameter.ItemsSource)
                        {
                            //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                            //    continue;
                            clsPathoSubTestVO obj = PathoParameterList.FirstOrDefault(q => q.ParamSTID == ((clsPathoSubTestVO)item).ParamSTID);
                            if (obj != null)
                                continue;

                            PathoParameterList.Add((clsPathoSubTestVO)item);
                        }
                    }
                    else
                        if (PathoParameterList == null)
                            PathoParameterList = new List<clsPathoSubTestVO>();
                    long position = 0;
                    if (grdPathoParameter.ItemsSource != null && ((List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource).Count != 0)
                    {
                        position = ((List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource)[((List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource).Count - 1].PrintPosition;
                        position = position + 1;
                    }
                    else
                        position = 0;

                    if (grdParameterMaster.ItemsSource != null)
                    {
                        List<clsPathoSubTestVO> list = (List<clsPathoSubTestVO>)grdParameterMaster.ItemsSource;
                        foreach (var item in list)
                        {

                            if (((clsPathoSubTestVO)item).Status == true)
                            {
                                clsPathoSubTestVO obj = PathoParameterList.FirstOrDefault(q => q.ParamSTID == ((clsPathoSubTestVO)item).ParamSTID);
                                if (obj != null)
                                    continue;

                                //((clsPathoTestParameterVO)item).Status = false;
                                MasterListItem objMasterList = new MasterListItem(0, "--Select--");
                                ((clsPathoSubTestVO)item).SelectedPrintName = objMasterList;
                                ((clsPathoSubTestVO)item).Status = false;
                                ((clsPathoSubTestVO)item).PrintPosition = position;
                                ((clsPathoSubTestVO)item).IsParameter = chkIsPara;
                                PathoParameterList.Add((clsPathoSubTestVO)item);
                                position = position + 1;
                            }
                        }
                    }

                    grdPathoParameter.ItemsSource = null;
                    grdPathoParameter.ItemsSource = PathoParameterList;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// removes parameter from 2nd parameter grid
        /// </summary>
        /// <param name="sender">remove button</param>
        /// <param name="e">remove button click</param>
        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsPathoTestParameterVO> GrdPathoParameterList = new List<clsPathoTestParameterVO>();
                if (grdPathoParameter.ItemsSource != null)
                {
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        if (((clsPathoTestParameterVO)item).Status == false)
                        {
                            GrdPathoParameterList.Add((clsPathoTestParameterVO)item);
                        }
                    }
                    //GrdPathoParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                    //GrdPathoParameterList.RemoveAt(grdPathoParameter.SelectedIndex);
                    grdPathoParameter.ItemsSource = null;
                    grdPathoParameter.ItemsSource = GrdPathoParameterList;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        private void cmdUP_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {

        }
        
        //private bool ItemValidation()
        //{
        //    bool result = true;


        //    if (txtQuantity.Text == "")
        //    {
        //        txtQuantity.SetValidation("Please Select Quantity");
        //        txtQuantity.RaiseValidationError();
        //        txtQuantity.Focus();
        //        result = false;
        //    }
        //    else
        //        txtQuantity.ClearValidationError();

        //    if ((MasterListItem)cmbItem.SelectedItem == null)
        //    {
        //        cmbItem.TextBox.SetValidation("Please Select Item");
        //        cmbItem.TextBox.RaiseValidationError();
        //        cmbItem.Focus();
        //        result = false;
        //    }
        //    else if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
        //    {
        //        cmbItem.TextBox.SetValidation("Please Select Item");
        //        cmbItem.TextBox.RaiseValidationError();
        //        cmbItem.Focus();
        //        result = false;
        //    }
        //    else
        //        cmbItem.TextBox.ClearValidationError();




        //    return result;
        //}
        public ObservableCollection<clsPathoTestItemDetailsVO> ItemList1 { get; set; }

        /// <summary>
        /// Calls Item search window
        /// </summary>
        /// <param name="sender">Add Item button</param>
        /// <param name="e">add item button click</param>
        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // PalashDynamics.Pharmacy


                PalashDynamics.Pharmacy.ItemList ItemsWin = new Pharmacy.ItemList();

                ItemsWin.StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyStoreID;
                ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                ItemsWin.ShowBatches = false;
                ItemsWin.cmbStore.IsEnabled = false;


                ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                ItemsWin.Show();


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds items to the item grid
        /// </summary>
        /// <param name="sender">Item search ok button</param>
        /// <param name="e">Item search ok button click</param>
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                foreach (var item in Itemswin.SelectedItems)
                {
                    //bool isExist = CheckForItemExistance(item.ID);
                    //if (!isExist)
                    //{
                    if (ItemList1.Where(Items => Items.ItemID == item.ID).Any() == false)
                        ItemList1.Add(
                        new clsPathoTestItemDetailsVO()
                        {
                            ItemID = item.ID,
                            ItemName = item.ItemName,
                            Status = item.Status,
                            //Quantity = 1,
                            DeductionList=Dlist,
                            UsageList=Ulist

                        });
                }

                dgItemDetailsList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = ItemList1;



            }
        }

        /// <summary>
        /// Deletes item from grid
        /// </summary>
        /// <param name="sender">delete hyperlink in item grid</param>
        /// <param name="e">delete hyperlink click</param>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemDetailsList.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Delete the selected Item ?";
                    int index = dgItemDetailsList.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ItemList1.RemoveAt(index);

                        }
                    };

                    msgWD.Show();
                    dgItemDetailsList.ItemsSource = null;
                    dgItemDetailsList.ItemsSource = ItemList1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This function is used to fetch service names of pathology
        /// </summary>
        private void FetchService()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

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
                       
                       
                        CmbServiceName.ItemsSource = null;
                        CmbServiceName.ItemsSource = objList;
                        CmbServiceName.SelectedItem = objList[0];


                        if (this.DataContext != null)
                        {
                            CmbServiceName.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).ServiceID;

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
        /// this function is used to fetch patholgy catagory
        /// </summary>
        private void FetchCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbCategory.ItemsSource = null;
                        CmbCategory.ItemsSource = objList;
                        CmbCategory.SelectedItem = objList[0];


                        if (this.DataContext != null)
                        {
                            CmbCategory.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).CategoryID;

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
        /// this function fills combo with patho parameter master
        /// </summary>
        private void FetchPathoParameterList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                        foreach (var item in ((clsGetMasterListBizActionVO)e.Result).MasterList)
                        {
                            clsPathoTestParameterVO objPathoTestParameter = new clsPathoTestParameterVO();
                            objPathoTestParameter.ParamSTID = item.ID;
                            objPathoTestParameter.Description = item.Description;
                            objPathoTestParameter.IsParameter = true;
                            objList.Add(objPathoTestParameter);
                        }

                        foreach (var item in objList)
                        {
                            item.PrintPosition = objList.IndexOf(item);
                        }

                        grdParameterMaster.ItemsSource = null;
                        grdParameterMaster.ItemsSource = objList;

                        //Added by Saily P 24.11.11


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
        /// This finction fills combo wth patho sample master
        /// </summary>
        private void FetchPathoSample()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PathoSampleMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {

                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbSample.ItemsSource = null;
                        cmbSample.ItemsSource = objList;
                        cmbSample.SelectedItem = objList[0];




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
        /// this function fills Items done in radiology used as it is
        /// </summary>

        //private void FetchItems()
        //{
        //    try
        //    {
        //        clsGetRadItemBizActionVO BizAction = new clsGetRadItemBizActionVO();
        //        BizAction.MasterList = new List<MasterListItem>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetRadItemBizActionVO)arg.Result).MasterList);

        //                cmbItem.ItemsSource = null;
        //                cmbItem.ItemsSource = objList;
        //                cmbItem.SelectedItem = objList[0];




        //            }

        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}
        private void FetchMachine()
        {

            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {

                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbMachine.ItemsSource = null;
                        cmbMachine.ItemsSource = objList;
                        cmbMachine.SelectedItem = objList[0];




                    }
                    if (this.DataContext != null)
                    {
                        cmbMachine.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).MachineID;

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
        /// User control loaded activities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                SetCommandButtonState("Load");
                this.DataContext = new clsPathoTestMasterVO();
                SampleList = new ObservableCollection<clsPathoTestSampleVO>();
                ItemList1 = new ObservableCollection<clsPathoTestItemDetailsVO>();
                FetchData();
                FetchService();
                FetchCategory();
                fillDeduction();
                fillUsageType();
                FetchPathoParameterList();
                FetchPathoSample();
                FetchMachine();
                ParamaterViewList = new List<clsPathoTestParameterVO>();
                
                
            }
            IsPageLoded = true;
        }
        
   
        
        /// <summary>
        /// First parameter grid parameter checked,status = true
        /// </summary>
        /// <param name="sender">chkParamGrid1 </param>
        /// <param name="e">chkParamGrid1 click</param>
        private void chkParamGrid1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdParameterMaster.SelectedItem != null)
                {
                    if(chkIsPara==true)
                        ((clsPathoTestParameterVO)grdParameterMaster.SelectedItem).Status = true;
                    else
                        ((clsPathoSubTestVO)grdParameterMaster.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Second parameter grid parameter checked,status = true
        /// </summary>
        /// <param name="sender">chkParamGrid2 </param>
        /// <param name="e">chkParamGrid2 click</param>
        private void chkParamGrid2_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdPathoParameter.SelectedItem != null)
                {
                    if(chkIsPara==true)
                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).Status = true;
                    else
                        ((clsPathoSubTestVO)grdPathoParameter.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// First parameter grid parameter checked,status = false
        /// /// </summary>
        /// <param name="sender">chkParamGrid1 </param>
        /// <param name="e">chkParamGrid1 click</param>
        private void chkParamGrid1_Unecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdParameterMaster.SelectedItem != null)
                {
                    if(chkIsPara==true)
                        ((clsPathoTestParameterVO)grdParameterMaster.SelectedItem).Status = false;
                    else
                        ((clsPathoSubTestVO)grdParameterMaster.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Second parameter grid parameter checked,status =false
        /// </summary>
        /// <param name="sender">chkParamGrid2 </param>
        /// <param name="e">chkParamGrid1 click</param>
        private void chkParamGrid2_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdPathoParameter.SelectedItem != null)
                {
                    if(chkIsPara==true)
                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).Status = false;
                    else
                        ((clsPathoSubTestVO)grdPathoParameter.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// sample modify
        /// </summary>
        /// <param name="sender">Modify button on sample list </param>
        /// <param name="e">Modify button click</param>
        private void cmdSampleModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SampleList.Count > 0)
                {
                    if (((MasterListItem)cmbSample.SelectedItem).ID != 0 && txtFrequency.Text != "")  //&& txtSampleQuantity.Text != ""
                    {
                        int var = dgSampleDetailsList.SelectedIndex;
                        SampleList.RemoveAt(dgSampleDetailsList.SelectedIndex);
                        SampleList.Insert(var, new clsPathoTestSampleVO
                        {
                            SampleID = ((MasterListItem)cmbSample.SelectedItem).ID,
                            SampleName = ((MasterListItem)cmbSample.SelectedItem).Description,
                            //Quantity = (float)Convert.ToDouble(txtSampleQuantity.Text),
                            Frequency = txtFrequency.Text,
                            Status = true,
                            ID = SampleID

                        }
                        );

                        dgSampleDetailsList.ItemsSource = SampleList;
                        dgSampleDetailsList.Focus();
                        dgSampleDetailsList.UpdateLayout();
                        dgSampleDetailsList.SelectedIndex = SampleList.Count - 1;

                        cmbSample.SelectedValue = (long)0;
                        //txtSampleQuantity.Text = "";
                        txtFrequency.Text = "";
                        textBefore = "";
                    }
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ObservableCollection<clsPathoTestSampleVO> SampleList { get; set; }

        /// <summary>
        /// add sample 
        /// </summary>
        /// <param name="sender">add button  </param>
        /// <param name="e">add button click</param>
        private void cmdAddSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateSample())
                {
                    clsPathoTestSampleVO tempSample = new clsPathoTestSampleVO();
                    var sampleitem = from r in SampleList
                                     where (r.SampleID == ((MasterListItem)cmbSample.SelectedItem).ID)
                                     select new clsPathoTestSampleVO
                                     {
                                         Status = r.Status,
                                         SampleID = r.SampleID,
                                         //Quantity = r.Quantity,
                                         SampleName = ((MasterListItem)cmbSample.SelectedItem).Description,
                                         Frequency = r.Frequency
                                     };
                    if (sampleitem.ToList().Count == 0)
                    {
                        tempSample.SampleID = ((MasterListItem)cmbSample.SelectedItem).ID;
                        tempSample.SampleName = ((MasterListItem)cmbSample.SelectedItem).Description;
                        //tempSample.Quantity = (float)Convert.ToDouble(txtSampleQuantity.Text);
                        tempSample.Frequency = txtFrequency.Text;
                        tempSample.Even_Odd = false;
                        tempSample.Status = true;
                        SampleList.Add(tempSample);

                        dgSampleDetailsList.ItemsSource = null;
                        dgSampleDetailsList.ItemsSource = SampleList;

                        cmbSample.SelectedValue = (long)0;
                        //txtSampleQuantity.Text = string.Empty;
                        txtFrequency.Text = string.Empty ;
                        textBefore = "";

                    }
                    
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// validates sample
        /// </summary>
        /// <returns></returns>
        private bool ValidateSample()
        {
            bool result = true;
            try
            {
                //if (txtSampleQuantity.Text == "")
                //{
                //    txtSampleQuantity.SetValidation("Please Enter Sample Quantity");
                //    txtSampleQuantity.RaiseValidationError();
                //    txtSampleQuantity.Focus();
                //    result = false;
                //}
                //else
                //    txtSampleQuantity.ClearValidationError();

                //if ((MasterListItem)cmbSample.SelectedItem == null || ((MasterListItem)cmbSample.SelectedItem).ID == 0)
                //{
                //    cmbSample.TextBox.SetValidation("Please Select Sample");
                //    cmbSample.TextBox.RaiseValidationError();
                //    cmbSample.Focus();
                //    result = false;
                //}

                //else
                //    cmbSample.TextBox.ClearValidationError();

                if ((MasterListItem)cmbSample.SelectedItem == null || ((MasterListItem)cmbSample.SelectedItem).ID == 0)
                {
                    cmbSample.TextBox.SetValidation("Please Select Sample");
                    cmbSample.TextBox.RaiseValidationError();
                    cmbSample.Focus();
                    result = false;
                }

                else
                    cmbSample.TextBox.ClearValidationError();
                if (txtFrequency.Text == "")
                {
                    txtFrequency.SetValidation("Please Enter Sample Frequency");
                    txtFrequency.RaiseValidationError();
                    txtFrequency.Focus();
                    result = false;
                }
                else
                    txtFrequency.ClearValidationError();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        /// <summary>
        /// edit sample 
        /// </summary>
        /// <param name="sender">edit sample</param>
        /// <param name="e">edit sample clcik</param>
        private void hlbEditSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmdAddSample.IsEnabled = false;
                cmdSampleModify.IsEnabled = true;

                cmbSample.SelectedValue = ((clsPathoTestSampleVO)dgSampleDetailsList.SelectedItem).SampleID;
                //txtSampleQuantity.Text = ((clsPathoTestSampleVO)dgSampleDetailsList.SelectedItem).Quantity.ToString();
                //float Quantity = (float)Convert.ToDouble(txtSampleQuantity.Text);

                //txtSampleQuantity.Text = String.Format("{0:0.00}", Quantity);

                txtFrequency.Text = ((clsPathoTestSampleVO)dgSampleDetailsList.SelectedItem).Frequency.ToString();
                float frequency = (float)Convert.ToDouble(txtFrequency.Text);

                txtFrequency.Text = String.Format("{0:0.00}", frequency);
                SampleID = ((clsPathoTestSampleVO)dgSampleDetailsList.SelectedItem).ID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// edit item
        /// </summary>
        /// <param name="sender">edit item</param>
        /// <param name="e">item sample clcik</param>
        private void hlbEditItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //cmbItem.SelectedValue = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID;
                //txtQuantity.Text = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).Quantity.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// delete sample
        /// </summary>
        /// <param name="sender">delete link</param>
        /// <param name="e">delete link click</param>
        private void cmdDeleteSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSampleDetailsList.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to delete the selected sample ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SampleList.RemoveAt(dgSampleDetailsList.SelectedIndex);

                        }
                    };

                    msgWD.Show();
                }
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// activities done on free format checked
        /// </summary>
        /// <param name="sender">free format checkbox</param>
        /// <param name="e">free format checkbox checked</param>
        private void chkFreeFormat_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (chkFreeFormat.IsChecked == true)
                //{
                //    chkNeedAuthorization.IsChecked = false;
                //}

            }
            catch (Exception ex)
            {
                throw;
            }
        }
  

        /// <summary>
        /// activities done on parameter checked
        /// </summary>
        /// <param name="sender">parameter radiobutton</param>
        /// <param name="e">parameter radiobutton checkbox checked</param>
        private void rdbParameter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                         
                if (rdbParameter.IsChecked == true)
                {
                    
                        grdPathoParameter.ItemsSource = null;
                        if (ParamaterViewList!=null &&  ParamaterViewList.Count > 0)
                            grdPathoParameter.ItemsSource = ParamaterViewList;
                        else
                        {
                            ParamaterViewList = null;
                            grdPathoParameter.ItemsSource = ParamaterViewList;
                        }
              
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = true;
                    FetchPathoParameterList();                   
                    grdParameterMaster.Columns[1].Header = "Parameter";           
                }
                

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// activities done on sub test checked
        /// </summary>
        /// <param name="sender">parameter radiobutton</param>
        /// <param name="e">parameter radiobutton checkbox checked</param>
        private void rdbSubTest_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// updates parameter postion in second parameter grid
        /// </summary>
        /// <param name="sender">UP button</param>
        /// <param name="e">UP button</param>
        private void cmdMoveUp1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsPathoTestParameterVO> objParameterList = new List<clsPathoTestParameterVO>();
                if (grdPathoParameter.ItemsSource != null)
                {
                    
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        objParameterList.Add((clsPathoTestParameterVO)item);
                    }
                    if (grdPathoParameter.SelectedItem != null && ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition != 0)
                    {
                        long position = 0;
                        long ParameterID = 0;

                        position = ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition;
                        ParameterID = ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).ParamSTID;

                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition = position - 1;

                        foreach (var item in objParameterList)
                        {
                            if (item.PrintPosition == position - 1 && item.ParamSTID != ParameterID)
                            {
                                item.PrintPosition = position;
                            }
                        }

                        var paramlist = from element in objParameterList
                                        orderby element.PrintPosition
                                        select element;





                        grdPathoParameter.ItemsSource = null;
                        grdPathoParameter.ItemsSource = paramlist;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// updates parameter postion in second parameter grid
        /// </summary>
        /// <param name="sender">Down button</param>
        /// <param name="e">Down button</param>
        private void cmdMoveDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsPathoTestParameterVO> objParameterList = new List<clsPathoTestParameterVO>();
                if (grdPathoParameter.ItemsSource != null)
                {

                    //objParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        objParameterList.Add((clsPathoTestParameterVO)item);
                    }
                    if (grdPathoParameter.SelectedItem != null && ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition != objParameterList.Count - 1)
                    {

                        long position = 0;
                        long ParameterID = 0;


                        position = ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition;
                        ParameterID = ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).ParamSTID;

                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).PrintPosition = position + 1;

                        foreach (var item in objParameterList)
                        {
                            if (item.PrintPosition == position + 1 && item.ParamSTID != ParameterID)
                            {
                                item.PrintPosition = position;
                            }
                        }

                        var paramlist = from element in objParameterList
                                        orderby element.PrintPosition
                                        select element;





                        grdPathoParameter.ItemsSource = null;
                        grdPathoParameter.ItemsSource = paramlist;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            clsPathoTestMasterVO Item;
            clsPathoTestMasterVO Item1;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsPathoTestMasterVO>)grdPathoTest.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsPathoTestMasterVO>)grdPathoTest.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsPathoTestMasterVO>)grdPathoTest.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsPathoTestMasterVO>)grdPathoTest.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.ID != ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID);
            }

            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtAgeFrom_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void CmbUsageType_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbUsageType1 = (AutoCompleteBox)sender;

            if (cmbUsageType1.ItemsSource == null || (cmbUsageType1.ItemsSource != null && ((List<MasterListItem>)cmbUsageType1.ItemsSource).ToList().Count == 0) || (((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList == null || ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList.Count == 0))
            {
                //  fillUsageType(cmbUsageType1);
            }

        }
        public List<MasterListItem> Dlist = new List<MasterListItem>();
        public List<MasterListItem> Ulist = new List<MasterListItem>();
        private void fillUsageType()
        {
            List<MasterListItem> UOMConvertLIst1 = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
           

            UOMConvertLIst1.Add(new MasterListItem(0, "- Select -"));
            UOMConvertLIst1.Add(new MasterListItem(1, "Sample Collection"));
            UOMConvertLIst1.Add(new MasterListItem(2, "Sample Processing"));
            Ulist = UOMConvertLIst1;         
        }
        private void cmbPUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            //if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<clsConversionsVO>)cmbConversions.ItemsSource).ToList().Count == 0))
            //{

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMList == null || ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }
        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {

                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];
                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgItemDetailsList.SelectedItem != null)
                        {
                            ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //Indicatior.Close();
                throw;
            }
        }
        private void fillDeduction()
        {
            
            List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            //objConversion.ID = 0;
            //objConversion.Description = "- Select -";
            //UOMConvertLIst.Add(objConversion);

            UOMConvertLIst.Add(new MasterListItem(0, "- Select -"));
            UOMConvertLIst.Add(new MasterListItem(1, "FIFO"));
            UOMConvertLIst.Add(new MasterListItem(2, "FEFO"));
            Dlist = UOMConvertLIst;
           
        }
        private void cmbDeduction_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPUOM_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void txtAgeFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmdMachine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPathoTestMasterVO objItemVO = new clsPathoTestMasterVO();
                objItemVO = (clsPathoTestMasterVO)grdPathoTest.SelectedItem;
                if (objItemVO != null)
                {
                    DefineMachine win = new DefineMachine();
                    win.IsFromSubTest = true;
                    win.Show();
                    win.GetItemDetailsSub(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
                dataGrid2Pager.PageIndex = 0;
            }
        }

        private void txtSampleQuantity_GotFocus(object sender, RoutedEventArgs e)
        {

            textBefore = "";
            selectionStart = 0;
            selectionLength = 0;
        }
    }
}
