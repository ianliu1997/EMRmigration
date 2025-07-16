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
using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;
using PalashDynamics.UserControls;
using System.Text.RegularExpressions;



namespace PalashDynamics.Administration
{
    public partial class PathoTestMaster : UserControl, INotifyPropertyChanged
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

        #region Variables
        WaitIndicator indicator = new WaitIndicator();
        private SwivelAnimation objAnimation;
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
        //long SampleID = 0;
        public List<clsPathoTestParameterVO> ParamaterViewList = new List<clsPathoTestParameterVO>();
        public List<clsPathoSubTestVO> SubtestViewList = new List<clsPathoSubTestVO>();
        long ModifyTestId = 0;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PathoTestMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsPathoTestMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

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
                clsGetPathoTestDetailsBizActionVO BizAction = new clsGetPathoTestDetailsBizActionVO();
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
                        if (((clsGetPathoTestDetailsBizActionVO)arg.Result).TestList != null)
                        {
                            clsGetPathoTestDetailsBizActionVO result = arg.Result as clsGetPathoTestDetailsBizActionVO;
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
           // ClearUI();


            TabControlMain.SelectedIndex = 0;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Test Details";
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

                    //by rohini dated 20.1.16
                   
                    txtblTube.Visibility = System.Windows.Visibility.Collapsed;
                    CmbTube.Visibility = System.Windows.Visibility.Collapsed;
                    //
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
                optBoth.IsChecked = true;
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
                    cmdMachine.IsEnabled = true;
                    cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    this.IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdMachine.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                     cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                     cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                     cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdMachine.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                     cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdMachine.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                     cmdAddSample.IsEnabled = true;
                    cmdSampleModify.IsEnabled = false;
                    IsCancel = false;
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
                        //bool SaveTest1 = true;
                        //if (chkReportTemplate.IsChecked == false)
                        //{
                        //    SaveTest1 = ValidateParameterForFormula();
                        //}
                        //else
                        //{
                        //    SaveTest1 = true;
                        //}
                        //if (SaveTest1 == true)
                        //{
                            string msgTitle = "Palash";
                            string msgText = "Are you sure you want to save the Test Master?";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                            msgW.Show();
                        //}
                        //else
                        //{
                        //    ClickedFlag = 0;
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Check Added Parameter List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //    msgW1.Show();
                        //}
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
        //By Rohinee Dated 23/11/16
        private bool ValidateParameterForFormula()
        {
            bool result = true;
            // take all formulas in one string to split all parameter id to check if the are selected 
            string fullString = "";
            List<clsPathoTestParameterVO> ParaList = new List<clsPathoTestParameterVO>();
            ParaList=(List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;

            List<MasterListItem> ListMaster = new List<MasterListItem>();
            foreach (var item in ParaList)
            {
                MasterListItem m = new MasterListItem();
                m.Description = item.Description;
                ListMaster.Add(m);
            }

            //
            foreach (var item in ParaList)
            {
                if (item.Formula != null)
                {
                    if (fullString.Trim() == string.Empty)
                    {
                        fullString = item.Formula;
                    }
                    else
                    {
                        fullString = fullString + item.Formula;
                    }
                }
            }            
            var pattern = @"\{(\d+)\}";  //to remove {}
            List<MasterListItem> List = new List<MasterListItem>();
            var regex = new Regex("{.*?}");

            var matches = regex.Matches(fullString); //input string to split
            foreach (var match in matches)
            {
                string str = "";
                MasterListItem master = new MasterListItem();             
                str = (match).ToString();
                master.ID =Convert.ToInt32(Regex.Replace(str, pattern, "$1"));
                List.Add(master);
            }
            //
            //by rohinee for checking all items from formula list contains in grid list  
            var Final= List.Where(p => !ListMaster.Any(p2 => p2.ID == p.ID));
           
            List<MasterListItem> FinalList = new List<MasterListItem>();
             FinalList = Final.ToList();

            if (FinalList.Count > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }            

            return result;
        }


        /// <summary>
        /// This function get called if user clicks ok on save message
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();

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
                BizAction.TestDetails.Status = true;
                if (CmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;

                if (CmbServiceName.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)CmbServiceName.SelectedItem).ID;
                //by rohini dated 20.1.16
                if (CmbTube.SelectedItem != null)
                    BizAction.TestDetails.TubeID = ((MasterListItem)CmbTube.SelectedItem).ID;



                BizAction.TestDetails.IsParameter = true;

                if (chkIsAbnormal.IsChecked == true)
                    BizAction.TestDetails.IsAbnormal = true;
                else if (chkIsAbnormal.IsChecked == true)
                    BizAction.TestDetails.IsAbnormal = false;
                //if (chkisab.IsChecked == true)
                //{
                //    BizAction.TestDetails.IsFormTemplate = (int)1;
                //}
                //else
                //{
                //    BizAction.TestDetails.IsFormTemplate = (int)0;
                //}
                //BizAction.TestDetails.IsParameter = true;


                //BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                //if (rdbForm.IsChecked == true)
                //{
                //    BizAction.TestDetails.IsFormTemplate = (int)2;

                //}
                //else
                //{
                    BizAction.TestDetails.IsFormTemplate = (int)4;
                //}
                if (chkReportTemplate.IsChecked == false)
                {
                    if (chkIsPara == true)
                    {
                        BizAction.TestDetails.IsFromParameter = true;
                        BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                        BizAction.TestDetails.SubTestList = SubtestViewList;
                        //  BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)
                    }
                    else
                    {
                        BizAction.TestDetails.IsFromParameter = true;
                        BizAction.TestDetails.TestParameterList = ParamaterViewList;
                        BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                    }

                }
                else
                {
                    chkIsPara = false;
                    BizAction.TestDetails.IsFromParameter = false;
                    BizAction.TestDetails.ReportTemplate = true;
                }
                if ((List<clsPathoTemplateVO>)dgTemplateList.ItemsSource != null && chkIsPara == false)
                {
                    BizAction.TestDetails.IsFromParameter = false;
                    BizAction.TestDetails.TestTemplateList = (List<clsPathoTemplateVO>)dgTemplateList.ItemsSource;
                }

                BizAction.TestDetails.TestItemList = ItemList1.ToList();
                if (dgItemDetailsList.ItemsSource != null)
                    foreach (var item in dgItemDetailsList.ItemsSource)
                    {
                        ((clsPathoTestItemDetailsVO)item).DID = (((clsPathoTestItemDetailsVO)item).SelectedDID).ID;
                        ((clsPathoTestItemDetailsVO)item).DName = (((clsPathoTestItemDetailsVO)item).SelectedDID).Description;
                        ((clsPathoTestItemDetailsVO)item).UID = (((clsPathoTestItemDetailsVO)item).SelectedUID).ID;
                        ((clsPathoTestItemDetailsVO)item).UName = (((clsPathoTestItemDetailsVO)item).SelectedUID).Description;
                        ((clsPathoTestItemDetailsVO)item).UOMid = (((clsPathoTestItemDetailsVO)item).SelectedUOM).ID;
                        ((clsPathoTestItemDetailsVO)item).UOMName = (((clsPathoTestItemDetailsVO)item).SelectedUOM).Description;
                    }
                BizAction.TestDetails.TestSampleList = SampleList.ToList();

                if (optBoth.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.None;
                else if (optFemale.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.Female;
                else if (optMale.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.Male;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            //Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Test Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = " : " + "Test Details";
                            objAnimation.Invoke(RotationType.Backward);
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 4)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because SERVICE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 5)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because SERVICE already Defined In Profile!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }


                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
        /// Validates Backpanel
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateBackPanel()
        {
            try
            {
                bool result = true;

                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))              
                {

                    txtDescription.SetValidation("Please Enter Description");
                    txtDescription.RaiseValidationError();
                    //txtDescription.Focus();

                    result = false;
                }
                else
                    txtDescription.ClearValidationError();

                if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                {

                    txtCode.SetValidation("Please Enter Code");
                    txtCode.RaiseValidationError();
                    //txtCode.Focus();

                    result = false;
                }
                else
                    txtCode.ClearValidationError();

                //added by rohini

                // commented as it is added to Parameter Master
               
                //if (string.IsNullOrEmpty(txtTechUsed.Text.Trim()))              
                //{

                //    txtTechUsed.SetValidation("Please Enter Technique Used");
                //    txtTechUsed.RaiseValidationError();
                //    //txtDescription.Focus();
                //    result = false;
                //}
                //else
                //    txtTechUsed.ClearValidationError();
                //
                

                if (IsPageLoded)
                {
                    if (((clsPathoTestMasterVO)this.DataContext).IsSubTest == false)
                    {
                        //if ((MasterListItem)CmbServiceName.SelectedItem == null)
                        //{
                        //    CmbServiceName.TextBox.SetValidation("Please Select Service");
                        //    CmbServiceName.TextBox.RaiseValidationError();
                        //    CmbServiceName.Focus();
                        //    result = false;
                        //}
                        //else if (((MasterListItem)CmbServiceName.SelectedItem).ID == 0)
                        //{
                        //    CmbServiceName.TextBox.SetValidation("Please Select Service");
                        //    CmbServiceName.TextBox.RaiseValidationError();
                        //    CmbServiceName.Focus();
                        //    result = false;
                        //}
                        //else
                        //    CmbServiceName.TextBox.ClearValidationError();


                        if ((MasterListItem)CmbCategory.SelectedItem == null)
                        {
                            CmbCategory.TextBox.SetValidation("Please Select Category");
                            CmbCategory.TextBox.RaiseValidationError();
                            //CmbCategory.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                        {
                            CmbCategory.TextBox.SetValidation("Please Select Category");
                            CmbCategory.TextBox.RaiseValidationError();
                            //CmbCategory.Focus();
                            result = false;
                        }
                        else
                            CmbCategory.TextBox.ClearValidationError();

                        //added by rohini dated 4.1.16
                        if ((MasterListItem)CmbServiceName.SelectedItem == null)
                        {
                            CmbServiceName.TextBox.SetValidation("Please Select Service");
                            CmbServiceName.TextBox.RaiseValidationError();
                            //CmbServiceName.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)CmbServiceName.SelectedItem).ID == 0)
                        {
                            CmbServiceName.TextBox.SetValidation("Please Select Service");
                            CmbServiceName.TextBox.RaiseValidationError();
                            //CmbServiceName.Focus();
                            result = false;
                        }
                        else
                            CmbServiceName.TextBox.ClearValidationError();

                        //aded by rohini for new requirement 
                        if ((MasterListItem)CmbTube.SelectedItem == null)
                        {
                            CmbTube.TextBox.SetValidation("Please Select Tube");
                            CmbTube.TextBox.RaiseValidationError();
                            //CmbCategory.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                        {
                            CmbTube.TextBox.SetValidation("Please Select Tube");
                            CmbTube.TextBox.RaiseValidationError();
                            //CmbCategory.Focus();
                            result = false;
                        }
                        else
                            CmbTube.TextBox.ClearValidationError();
                        if (grdPathoParameter.ItemsSource == null && chkReportTemplate.IsChecked==false)
                        {
                            string msgTitle = "Palash";
                            string msgText = "Please Select Parameter details Or Template details";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW.Show();
                            TabControlMain.SelectedIndex = 1;
                            result = false;
                            return result;
                        }

                     

                        //commented by rohini as per dr priyanka said
                        //if (ItemList1.Count == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can't Save Test Master Without Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //    msgW1.Show();
                        //    result = false;
                        //    TabControlMain.SelectedIndex = 3;
                        //    return result;

                        //}

                        //if (SampleList.Count == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can't Save Test Master Without Samples.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //    msgW1.Show();
                        //    result = false;
                        //    TabControlMain.SelectedIndex = 3;
                        //    return result;

                        //}
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //added by rohini dated 9.2.16
        private bool ValidateNew()
        {
            try
            {
                bool result = true;
                if (txtDescription.Text == "")
                {

                    txtDescription.SetValidation("Please Enter Description");
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();

                    result = false;
                }
                else
                    txtDescription.ClearValidationError();


                if (txtCode.Text == "")
                {

                    txtCode.SetValidation("Please Enter Code");
                    txtCode.RaiseValidationError();
                    txtCode.Focus();

                    result = false;
                }
                else
                    txtCode.ClearValidationError();

                //if ((MasterListItem)CmbCategory.SelectedItem == null)
                //{
                //    CmbCategory.TextBox.SetValidation("Please Select Category");
                //    CmbCategory.TextBox.RaiseValidationError();
                //    CmbCategory.TextBox.Focus();
                //    result = false;
                //}
                //else if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                //{
                //    CmbCategory.TextBox.SetValidation("Please Select Category");
                //    CmbCategory.TextBox.RaiseValidationError();
                //    CmbCategory.TextBox.Focus();
                //    result = false;
                //}
                //else
                //    CmbCategory.TextBox.ClearValidationError();

             // Technique used
                //if (txtTechUsed.Text == "")
                //{

                //    txtTechUsed.SetValidation("Please Enter Technique Used");
                //    txtTechUsed.RaiseValidationError();
                //    txtTechUsed.Focus();

                //    result = false;
                //}
                //else
                //    txtTechUsed.ClearValidationError();

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
                    //ModifyTest = ValidateBackPanel();
                    //if (ModifyTest == true)
                    //{

                    //    bool ModifyTest1 = true;
                    //    if (chkReportTemplate.IsChecked == false)
                    //    {
                    //        ModifyTest1 = ValidateParameterForFormula();
                    //    }
                    //    else
                    //    {
                    //        ModifyTest1 = true;
                    //    }
                    //    if (ModifyTest1 == true)
                    //    {
                            string msgTitle = "Palash";
                            string msgText = "Are you sure you want to Update the Test Master?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                            msgW1.Show();
                        //}
                        //else
                        //{
                        //    ClickedFlag = 0;
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Check Added Parameter List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //    msgW1.Show();
                        //}
                    }
                    else
                        ClickedFlag = 0;
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


                Modify();


                ClickedFlag = 0;

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
                if (CmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;
                BizAction.IsUpdate = true;
                if (CmbServiceName.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)CmbServiceName.SelectedItem).ID;


                if (CmbServiceName.SelectedItem != null)
                    BizAction.TestDetails.TubeID = ((MasterListItem)CmbTube.SelectedItem).ID;

                //List<clsPathoTestParameterVO> paramlist = new List<clsPathoTestParameterVO>();
                //foreach (var item in grdPathoParameter.ItemsSource)
                //{
                //    //if (rdbParameter.IsChecked == true)
                //    //    ((clsPathoTestParameterVO)item).IsParameter = true;
                //    //else
                //    //    ((clsPathoTestParameterVO)item).IsParameter = false;
                //    paramlist.Add((clsPathoTestParameterVO)item);

                //}
                //BizAction.TestDetails.TestParameterList = paramlist;

                BizAction.TestDetails.IsParameter = true;


                //BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                //by rohini dated 20.1.16
                if (CmbTube.SelectedItem != null)
                    BizAction.TestDetails.TubeID = ((MasterListItem)CmbTube.SelectedItem).ID;

                if (rdbForm.IsChecked == true)
                {
                    BizAction.TestDetails.IsFormTemplate = (int)2;

                }
                else
                {
                    BizAction.TestDetails.IsFormTemplate = (int)4;
                }
                if (chkReportTemplate.IsChecked == false)
                {
                    if (chkIsPara == true)
                    {
                        BizAction.TestDetails.IsFromParameter = true;
                        BizAction.TestDetails.TestParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                        BizAction.TestDetails.SubTestList = SubtestViewList;
                        //  BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)
                    }
                    else
                    {
                        BizAction.TestDetails.IsFromParameter = true;
                        BizAction.TestDetails.TestParameterList = ParamaterViewList;
                        BizAction.TestDetails.SubTestList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                    }

                }
                else
                {
                    chkIsPara = false;
                    BizAction.TestDetails.IsFromParameter = false;
                    BizAction.TestDetails.ReportTemplate = true;
                }

                if ((List<clsPathoTemplateVO>)dgTemplateList.ItemsSource != null && chkIsPara == false)
                {
                    BizAction.TestDetails.IsFromParameter = false;
                    BizAction.TestDetails.TestTemplateList = (List<clsPathoTemplateVO>)dgTemplateList.ItemsSource;
                }
                BizAction.TestDetails.TestItemList = ItemList1.ToList();
                if (dgItemDetailsList.ItemsSource != null)
                    foreach (var item in dgItemDetailsList.ItemsSource)
                    {
                        ((clsPathoTestItemDetailsVO)item).DID = (((clsPathoTestItemDetailsVO)item).SelectedDID).ID;
                        ((clsPathoTestItemDetailsVO)item).DName = (((clsPathoTestItemDetailsVO)item).SelectedDID).Description;
                        ((clsPathoTestItemDetailsVO)item).UID = (((clsPathoTestItemDetailsVO)item).SelectedUID).ID;
                        ((clsPathoTestItemDetailsVO)item).UName = (((clsPathoTestItemDetailsVO)item).SelectedUID).Description;
                        ((clsPathoTestItemDetailsVO)item).UOMid = (((clsPathoTestItemDetailsVO)item).SelectedUOM).ID;
                        ((clsPathoTestItemDetailsVO)item).UOMName = (((clsPathoTestItemDetailsVO)item).SelectedUOM).Description;
                    }
                BizAction.TestDetails.TestSampleList = SampleList.ToList();

                if (optBoth.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.None;
                else if (optFemale.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.Female;
                else if (optMale.IsChecked == true)
                    BizAction.TestDetails.Applicableto = (int)Genders.Male;

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
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Test Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 4)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because SERVICE already Defined!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                            //if((clsPathoTestMasterVO)grdPathoTest.SelectedItem!=null)
                            //{
                            //     long testid =((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID;
                            //     FillParameterSampleAndITemList(testid);
                            //}

                            //if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID > 0)
                            //{
                            //    ModifyTestId = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID;
                            //    FillParameterSampleAndITemList(((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID);
                            //} 
                        }
                        else if (((clsAddPathoTestMasterBizActionVO)arg.Result).SuccessStatus == 5)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because SERVICE already Defined In Profile!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            FetchData();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
                    //ADDED BY ROHINI 
                    ClearUI();
                    FetchData();
                    dataGrid2Pager.PageIndex = 0;
                    //chkIsPara = false;
                    //chkReportTemplate.IsChecked = false;
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

            ResetControls();
            if (chkReportTemplate.IsChecked == true)
            {
                parameters.Visibility = System.Windows.Visibility.Collapsed;
                TabTestTemplateDetails.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                parameters.Visibility = System.Windows.Visibility.Visible;
                TabTestTemplateDetails.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (txtCompletionTime.Text == "0")           
                txtCompletionTime.Text = "";
            ValidateNew();

            txtblCategory.Visibility = System.Windows.Visibility.Visible;
            CmbCategory.Visibility = System.Windows.Visibility.Visible;

            TabTestSampleDetails.Visibility = System.Windows.Visibility.Visible;
            //TabTestItemDetails.Visibility = System.Windows.Visibility.Visible;
            ((clsPathoTestMasterVO)this.DataContext).IsSubTest = false;
            rdbParameter.IsChecked = true;
            rdbSubTest.IsChecked = false;
            
            TabControlMain.SelectedIndex = 0;          
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Test Details";
            objAnimation.Invoke(RotationType.Forward);
            


            cmdAddSample.IsEnabled = true;
            cmdSampleModify.IsEnabled = false;

        }

        /// <summary>
        /// Clears UI
        /// </summary>
        private void ClearUI()
        {
            try
            {
                txtSearch.Text = "";
                //grdBackPanel.DataContext = new clsPathoTestMasterVO();
                this.DataContext = new clsPathoTestMasterVO();
                CmbServiceName.SelectedValue = (long)0;
                //by rohini dated 20.1.16
                txtCompletionTime.Text = "";
                CmbTube.SelectedValue = (long)0;
                CmbCategory.SelectedValue = (long)0;
                cmbSample.SelectedValue = (long)0;
                cmbMachine.SelectedValue = (long)0;
                //txtSampleQuantity.Text = "";
                txtFrequency.Text = "";
                // cmbItem.SelectedValue = (long)0; ;
                ItemList1 = new ObservableCollection<clsPathoTestItemDetailsVO>();
                SampleList = new ObservableCollection<clsPathoTestSampleVO>();
                ParameterList = new ObservableCollection<MasterListItem>();
                ParamaterViewList = new List<clsPathoTestParameterVO>();
                SubtestViewList = new List<clsPathoSubTestVO>();
                grdPathoParameter.ItemsSource = null;
                FetchPathoParameterList();
                dgSampleDetailsList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = null;
                rdbParameter.IsChecked = true;
                this.DataContext = new clsPathoTestMasterVO();
                UserTemplateList = null;
                isView = false;
                subTestParam = false;
                if ((List<clsPathoTemplateVO>)dgTemplateList.ItemsSource != null)
                {
                    List<clsPathoTemplateVO> lList = (List<clsPathoTemplateVO>)dgTemplateList.ItemsSource;
                    foreach (var item in lList)
                    {
                        item.Status = false;

                    }
                    dgTemplateList.ItemsSource = null;
                    dgTemplateList.ItemsSource = lList;
                }


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
                ValidateNew();
                isView = true;
                if (grdPathoTest.SelectedItem != null)
                {
                    this.DataContext = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem);
                    if (((clsPathoTestMasterVO)this.DataContext).IsSubTest == true)
                    {
                        txtblServiceName.Visibility = System.Windows.Visibility.Collapsed;
                        CmbServiceName.Visibility = System.Windows.Visibility.Collapsed;

                        txtblCategory.Visibility = System.Windows.Visibility.Collapsed;
                        CmbCategory.Visibility = System.Windows.Visibility.Collapsed;

                        TabTestSampleDetails.Visibility = System.Windows.Visibility.Collapsed;
                        //TabTestItemDetails.Visibility = System.Windows.Visibility.Collapsed;

                        //Saily
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

                        //Saily
                        rdbParameter.IsChecked = true;
                    }
                    CmbCategory.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).CategoryID;
                    CmbServiceName.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ServiceID;
                    CmbTube.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).TubeID;
                    cmbMachine.SelectedValue = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).MachineID;
                    if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).Applicableto == (int)Genders.None)
                        optBoth.IsChecked = true;
                    else if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).Applicableto == (int)Genders.Female)
                        optFemale.IsChecked = true;
                    else if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).Applicableto == (int)Genders.Male)
                        optMale.IsChecked = true;

                    

                    if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ReportTemplate == true)
                    {
                        parameters.Visibility = Visibility.Collapsed;
                        TabTestTemplateDetails.Visibility = Visibility.Visible;

                        if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).IsFormTemplate == 2)
                        {
                            rdbForm.IsChecked = true;
                            rdbForm.Checked += new RoutedEventHandler(rdbFormTemplate_Checked);
                          
                            
                        }
                        else if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).IsFormTemplate == 4)
                        {
                            rdbWord.IsChecked = true;
                            rdbForm.Checked += new RoutedEventHandler(rdbFormTemplate_Checked);
                        }
                        else
                        {
                            rdbForm.IsChecked = false;
                            rdbWord.IsChecked = false;
                        }

                    }
                    else
                    {
                        parameters.Visibility = Visibility.Visible;
                        TabTestTemplateDetails.Visibility = Visibility.Collapsed;
                    }
                   
                 
                    //by rohini dated 21.1.16
                    if (((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID > 0)
                    {
                        ModifyTestId = ((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID;
                        FillParameterSampleAndITemList(((clsPathoTestMasterVO)grdPathoTest.SelectedItem).ID);
                    }
                    //by rohinee to load at time of view click template details get featch
                    FlagChk = 2;                  
                    FeatchFormDetails();
                    //
                    objAnimation.Invoke(RotationType.Forward);
                    TabControlMain.SelectedIndex = 0;
                }

                cmdAddSample.IsEnabled = true;
                cmdSampleModify.IsEnabled = false;

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
        /// 

        List<clsPathoTemplateVO> SeleectedTempList = new List<clsPathoTemplateVO>();
        private void FillParameterSampleAndITemList(long testID)
        {
            try
            {
                indicator.Show();
                clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO BizAction = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                BizAction.ParameterList = new List<clsPathoTestParameterVO>();
                BizAction.ItemList = new List<clsPathoTestItemDetailsVO>();
                BizAction.SampleList = new List<clsPathoTestSampleVO>();
                BizAction.IsFormSubTest = false;
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
                            grdPathoParameter.UpdateLayout();
                            //if (DetailsVO.ParameterList.Count > 0)
                            //{
                            //    if (DetailsVO.ParameterList[0].IsParameter == false)
                            //    {
                            //        subTestParam = false;
                            //       // rdbSubTest.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        subTestParam = true;
                            //       // rdbParameter.IsChecked = true;
                            //    }
                            //}

                            ParamaterViewList = DetailsVO.ParameterList;
                        }

                        if (DetailsVO.SubTestList != null)
                        {
                            SubtestViewList = DetailsVO.SubTestList;
                        }
                        //if (DetailsVO.ParameterList.Count > 0)
                        //{

                        //    if (DetailsVO.ParameterList[0].IsParameter == false)
                        //        rdbSubTest.IsChecked = true;
                        //    else
                        //        rdbParameter.IsChecked = true;
                        //}


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
                        //rohini dated 21

                        if (DetailsVO.TemplateList != null)
                        {
                            SeleectedTempList = DetailsVO.TemplateList;
                            //List<clsPathoTemplateVO> lstTemplate = (List<clsPathoTemplateVO>)dgTemplateList.ItemsSource;

                            clsPathoTemplateVO selected = new clsPathoTemplateVO();
                            if (SeleectedTempList != null)
                            {


                                if (UserTemplateList != null && UserTemplateList.Count > 0)
                                {
                                    foreach (var items in UserTemplateList)
                                    {
                                        foreach (var item in SeleectedTempList)
                                        {

                                            if (item.Status == true && item.TemplateID == items.TemplateID)
                                            {
                                                items.IsDefault = true;
                                            }

                                        }
                                    }
                                    foreach (var itemnn in UserTemplateList)
                                    {
                                        if (itemnn.IsDefault == true)
                                        {
                                            itemnn.Status = true;
                                        }
                                        else
                                        {
                                            itemnn.Status = false;
                                        }
                                    }
                                }
                            }

                            dgTemplateList.ItemsSource = null;
                            dgTemplateList.ItemsSource = UserTemplateList;
                            txtParameterSearch.Text = "";
                            dgTemplateList.UpdateLayout();
                            //dgTemplateList.ItemsSource = null;
                            //dgTemplateList.ItemsSource = lstTemplate;
                            //dgTemplateList.UpdateLayout();
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

                            if (((clsPathoTestParameterVO)item).SelStatus == true)
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
                               // ((clsPathoTestParameterVO)item).FormulaID = item.FormulaID;  // by rohinee formula getting null when add remove parameter multiple times 
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

                            if (((clsPathoSubTestVO)item).SelStatus == true)
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
                if (chkIsPara == true)
                {
                    List<clsPathoTestParameterVO> GrdPathoParameterList = new List<clsPathoTestParameterVO>();
                    if (grdPathoParameter.ItemsSource != null)
                    {
                        foreach (var item in grdPathoParameter.ItemsSource)
                        {
                            if (((clsPathoTestParameterVO)item).SelStatus == false)
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
                else
                {
                    List<clsPathoSubTestVO> GrdPathoParameterList = new List<clsPathoSubTestVO>();
                    if (grdPathoParameter.ItemsSource != null)
                    {
                        foreach (var item in grdPathoParameter.ItemsSource)
                        {
                            if (((clsPathoSubTestVO)item).SelStatus == false)
                            {
                                GrdPathoParameterList.Add((clsPathoSubTestVO)item);
                            }
                        }
                        //GrdPathoParameterList = (List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                        //GrdPathoParameterList.RemoveAt(grdPathoParameter.SelectedIndex);
                        grdPathoParameter.ItemsSource = null;
                        grdPathoParameter.ItemsSource = GrdPathoParameterList;
                    }
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
                    //}
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
                BizAction.GetServicesForPathology = true;
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


        //by rohini dated 20.1.2016
        private void FetchTube()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TubeMaster;
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
                        CmbTube.ItemsSource = null;
                        CmbTube.ItemsSource = objList;
                        CmbTube.SelectedItem = objList[0];


                        if (this.DataContext != null)
                        {
                            CmbTube.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).TubeID;

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
            #region Commented by rohinee
            //try
            //{
            //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //    //BizAction.IsActive = true;
            //    BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
            //    BizAction.MasterList = new List<MasterListItem>();

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, e) =>
            //    {
            //        if (e.Error == null && e.Result != null)
            //        {
            //            List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

            //            foreach (var item in ((clsGetMasterListBizActionVO)e.Result).MasterList)
            //            {
            //                clsPathoTestParameterVO objPathoTestParameter = new clsPathoTestParameterVO();
            //                objPathoTestParameter.ParamSTID = item.ID;
            //                objPathoTestParameter.Description = item.Description;
            //                objPathoTestParameter.IsParameter = true;
            //                objList.Add(objPathoTestParameter);
            //            }

            //            foreach (var item in objList)
            //            {
            //                item.PrintPosition = objList.IndexOf(item);
            //            }

            //            grdParameterMaster.ItemsSource = null;
            //            grdParameterMaster.ItemsSource = objList;

            //            //Added by Saily P 24.11.11


            //        }
            //    };
            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion

            try
            {
            
                clsGetPathoParameterMasterBizActionVO objTempList = new clsGetPathoParameterMasterBizActionVO();
                objTempList.IsPagingEnabled = true;
                objTempList.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objTempList.MaximumRows = DataList.PageSize;
                objTempList.AllParameter = true;
                //if (startindex == 0)
                //{
                //    objTempList.StartRowIndex = startindex * DataList.PageSize;
                //}
                //else
                //{
                //    objTempList.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                //}

                objTempList.SearchExpression = txtSearchCriteria.Text.Trim();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                       
                        List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                        foreach (var item in ((clsGetPathoParameterMasterBizActionVO)ea.Result).ParameterList)
                        {
                            clsPathoTestParameterVO objPathoTestParameter = new clsPathoTestParameterVO();
                            objPathoTestParameter.ParamSTID = item.ID;
                            objPathoTestParameter.Description = item.ParameterDesc;
                           // objPathoTestParameter.FormulaID = item.FormulaID;
                            objPathoTestParameter.IsParameter = true;
                            objList.Add(objPathoTestParameter);
                        }

                        foreach (var item in objList)
                        {
                            item.PrintPosition = objList.IndexOf(item);
                        }

                        grdParameterMaster.ItemsSource = null;
                        grdParameterMaster.ItemsSource = objList;
                    }
                    else
                    {
                       
                    }
                };
                client.ProcessAsync(objTempList, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw ex;
             
            }
            finally
            {
                
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
                //BizAction.IsActive = true;
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
                this.DataContext = new clsPathoTestMasterVO();
                SampleList = new ObservableCollection<clsPathoTestSampleVO>();
                ItemList1 = new ObservableCollection<clsPathoTestItemDetailsVO>();
                FetchData();
                FetchService();
                FetchCategory();
                FetchPathoParameterList();
                FetchPathoSample();
                //by rohini
                FetchTube();
                fillDeduction();
                fillUsageType();
                if (chkReportTemplate.IsChecked == true)
                {
                    parameters.Visibility = System.Windows.Visibility.Collapsed;
                    TabTestTemplateDetails.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    parameters.Visibility = System.Windows.Visibility.Visible;
                    TabTestTemplateDetails.Visibility = System.Windows.Visibility.Collapsed;
                }
                
                //FillTemplate();           
                FetchMachine();
                ParamaterViewList = new List<clsPathoTestParameterVO>();
                SubtestViewList = new List<clsPathoSubTestVO>();
                SetCommandButtonState("Load");
                //txtTemplateSearch.Text = "";
                //FetchItems();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "Test Details";
               //objAnimation.Invoke(RotationType.Forward);
            }
            IsPageLoded = true;
        }
        
        private void FetchPathoSubTestData()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PathoTestMaster;
                BizAction.Parent = new KeyValue { Key = 1, Value = "IsSubTest" };
                BizAction.MasterList = new List<MasterListItem>();

                #region Commented by Saily P on 24.11.11

                //clsGetPathoTestDetailsBizActionVO BizAction = new clsGetPathoTestDetailsBizActionVO();
                //BizAction.TestList = new List<clsPathoTestMasterVO>();

                //if (txtSearch.Text != "")
                //{
                //    BizAction.Description = txtSearch.Text;
                //}

                //BizAction.IsPagingEnabled = true;
                //BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                //BizAction.MaximumRows = DataList.PageSize;
            #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<clsPathoSubTestVO> objList = new List<clsPathoSubTestVO>();

                        foreach (var item in ((clsGetMasterListBizActionVO)arg.Result).MasterList)
                        {
                            clsPathoSubTestVO objPathoTestParameter = new clsPathoSubTestVO();
                            if (item.ID != ModifyTestId)
                            {
                                objPathoTestParameter.ParamSTID = item.ID;
                                objPathoTestParameter.Description = item.Description;
                                objPathoTestParameter.IsParameter = false;
                                objList.Add(objPathoTestParameter);
                            }
                        }

                        foreach (var item in objList)
                        {
                            item.PrintPosition = objList.IndexOf(item);
                        }

                        grdParameterMaster.ItemsSource = null;
                        grdParameterMaster.ItemsSource = objList;

                        #region Commented by Saily P on 24.11.11
                        //if (((clsGetPathoTestDetailsBizActionVO)arg.Result).TestList != null)
                        //{
                        //    clsGetPathoTestDetailsBizActionVO result = arg.Result as clsGetPathoTestDetailsBizActionVO;
                        //    //DataList.TotalItemCount = result.TotalRows;
                        //    List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();
                        //    if (result.TestList != null)
                        //    {

                        //        var subTests = from item in result.TestList
                        //                       where item.IsSubTest == true
                        //                       select item;

                        //        foreach (var item in subTests)
                        //        {
                        //            clsPathoTestParameterVO objPathoTestParameter = new clsPathoTestParameterVO();
                        //            objPathoTestParameter.ParamSTID = item.ID;
                        //            objPathoTestParameter.Description = item.Description;

                        //            objList.Add(objPathoTestParameter);

                        //        }
                        //        foreach (var item in objList)
                        //        {
                        //            item.PrintPosition = objList.IndexOf(item);

                        //        }
                        //        grdParameterMaster.ItemsSource = null;
                        //        grdParameterMaster.ItemsSource = objList;
                        //    }
                        //}
                        #endregion
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
                        ((clsPathoTestParameterVO)grdParameterMaster.SelectedItem).SelStatus = true;
                    else
                        ((clsPathoSubTestVO)grdParameterMaster.SelectedItem).SelStatus = true;
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
                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).SelStatus = true;
                    else
                        ((clsPathoSubTestVO)grdPathoParameter.SelectedItem).SelStatus = true;
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
                        ((clsPathoTestParameterVO)grdParameterMaster.SelectedItem).SelStatus = false;
                    else
                        ((clsPathoSubTestVO)grdParameterMaster.SelectedItem).SelStatus = false;
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
                        ((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).SelStatus = false;
                    else
                        ((clsPathoSubTestVO)grdPathoParameter.SelectedItem).SelStatus = false;
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
                cmdAddSample.IsEnabled = true;
                cmdSampleModify.IsEnabled = false;
                if (SampleList.Count > 0)
                {
                    if (((MasterListItem)cmbSample.SelectedItem).ID != 0 && txtFrequency.Text != "") //&& txtSampleQuantity.Text != ""
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

                        
                        cmdAddSample.IsEnabled = true;
                        cmdSampleModify.IsEnabled = false;

                        cmbSample.SelectedValue = (long)0;
                        //txtSampleQuantity.Text = "";
                        txtFrequency.Text = "";
                        textBefore = "";
                    }



                }
                else
                {
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

                cmdAddSample.IsEnabled = true;
                cmdSampleModify.IsEnabled = false;
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
                        //if (txtSampleQuantity.Text.Trim() != string.Empty)
                        //{
                        //    tempSample.Quantity = (float)Convert.ToDouble(txtSampleQuantity.Text);
                        //}
                        tempSample.Frequency = txtFrequency.Text;
                        tempSample.Even_Odd = false;
                        tempSample.Status = true;
                        SampleList.Add(tempSample);

                        dgSampleDetailsList.ItemsSource = null;
                        dgSampleDetailsList.ItemsSource = SampleList;

                        cmbSample.SelectedValue = (long)0;
                        //txtSampleQuantity.Text = "";
                        txtFrequency.Text = "";
                        textBefore = "";

                    }


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
            //    if (txtSampleQuantity.Text == "")
            //    {
            //        txtSampleQuantity.SetValidation("Please Enter Sample Quantity");
            //        txtSampleQuantity.RaiseValidationError();
            //        txtSampleQuantity.Focus();
            //        result = false;
            //    }
            //    else if (decimal.Parse(txtSampleQuantity.Text)<0)
            //     {
            //         txtSampleQuantity.SetValidation("Sample Quantity Cannot be Negative");
            //         txtSampleQuantity.RaiseValidationError();
            //         txtSampleQuantity.Focus();
            //         result = false;
                    
            //   }
            //    else
            //         txtSampleQuantity.ClearValidationError();

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
                else if ((decimal.Parse(txtFrequency.Text) < 0))
                {
                    txtFrequency.SetValidation("Sample Frequency Cannot be Negative");
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

                cmdAddSample.IsEnabled = false;
                cmdSampleModify.IsEnabled = true;
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
                if (chkFreeFormat.IsChecked == true)
                {
                    chkNeedAuthorization.IsChecked = false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// activities done on free format unchecked
        /// </summary>
        /// <param name="sender">free format checkbox</param>
        /// <param name="e">free format checkbox unchecked</param>
        private void chkFreeFormat_Unchecked(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    chkNeedAuthorization.IsChecked = false;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
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
                //COMMENTED TEMPRORY
                if (isView == true || chkIsPara==false)
                    SubtestViewList = null;

                chkIsPara = true;                

                if (SubtestViewList == null)
                    SubtestViewList = new List<clsPathoSubTestVO>();

                if (grdPathoParameter.ItemsSource != null)
                {
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        SubtestViewList.Add((clsPathoSubTestVO)item);
                    }
                    //SubtestViewList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                }
                //COMMENTED FOR TEM
                rdbSubTest.IsChecked = false;
                txtParameterSearch.Text = "";
                if (rdbParameter.IsChecked == true)
                {
                    //if (isView == true)
                    //{
                        grdPathoParameter.ItemsSource = null;
                    if(ParamaterViewList != null)
                    {
                        if (ParamaterViewList.Count > 0)
                            grdPathoParameter.ItemsSource = ParamaterViewList;
                        else
                        {
                            ParamaterViewList = null;
                            grdPathoParameter.ItemsSource = ParamaterViewList;
                        }
                    }
                    //    if (subTestParam == false)
                    //        grdPathoParameter.ItemsSource = null;
                    //}
                    //else
                    //{
                    //    grdPathoParameter.ItemsSource = null;
                    //}
                   // ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = true;
                    FetchPathoParameterList();                   
                    grdParameterMaster.Columns[1].Header = "Parameter";
                    //grdPathoParameter.Columns[2].Visibility = Visibility.Visible;
                    grdPathoParameter.Columns[2].Visibility = Visibility.Collapsed;
               
                }
                else
                {
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    grdParameterMaster.Columns[1].Header = "Sub Test";

                    //ADDED BY ROHINI FOR SUB test dated 30.3.16
                    grdPathoParameter.Columns[2].Visibility = System.Windows.Visibility.Collapsed;
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
            try
            {
                //COMMENTED TEMPRORY
                if (isView == true || chkIsPara == true)
                    ParamaterViewList = null;

                chkIsPara = false;


                if (ParamaterViewList == null)
                    ParamaterViewList = new List<clsPathoTestParameterVO>();
                if (grdPathoParameter.ItemsSource != null)
                {
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        ParamaterViewList.Add((clsPathoTestParameterVO)item);
                    }
                    //ParamaterViewList =(List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                }
                //COMMENTED TEMPRORY
                rdbParameter.IsChecked = false;
                txtParameterSearch.Text = "";
                if (rdbSubTest.IsChecked == true)
                {
                   //if (isView == true)
                   //{
                       grdPathoParameter.ItemsSource = null;
                       if (SubtestViewList != null)
                       {
                           if (SubtestViewList.Count > 0)
                               grdPathoParameter.ItemsSource = SubtestViewList;
                           else
                           {
                               SubtestViewList = null;
                               grdPathoParameter.ItemsSource = SubtestViewList;
                           }

                       }
                    //    if (subTestParam == true)
                    //        grdPathoParameter.ItemsSource = null;
                   //}
                   //else
                   //{
                   //    grdPathoParameter.ItemsSource = null;
                   //}

                    //((clsPathoTestMasterVO)this.DataContext).IsParameter = true;
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    FetchPathoSubTestData();
                    
                    grdParameterMaster.Columns[1].Header = "Sub Test";
                    //ADDED BY ROHINI FOR SUB test dated 30.3.16
                    grdPathoParameter.Columns[2].Visibility = Visibility.Collapsed;
                    
                }
                else
                {
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    grdParameterMaster.Columns[1].Header = "Parameter";
                    //ADDED BY ROHINI FOR SUB test dated 30.3.16
                    //grdPathoParameter.Columns[2].Visibility = Visibility.Visible;
                  
                   
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
        /// <param name="sender">UP button</param>
        /// <param name="e">UP button</param>
        private void cmdMoveUp1_Click(object sender, RoutedEventArgs e)
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

                        if (paramlist != null && paramlist.ToList().Count > 0)
                        {
                            List<clsPathoTestParameterVO> LstNew = new List<clsPathoTestParameterVO>();
                            LstNew = paramlist.ToList();
                            
                            grdPathoParameter.ItemsSource = null;
                            grdPathoParameter.ItemsSource = LstNew;
                            //((clsPathoTestParameterVO)grdPathoParameter.SelectedItem).ParamSTID = ParameterID;
                        }
                        else
                            grdPathoParameter.ItemsSource = null;
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

        private void optBoth_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optFemale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void optMale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkReportTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (chkReportTemplate.IsChecked == true)
            {
                parameters.Visibility = System.Windows.Visibility.Collapsed;
                TabTestTemplateDetails.Visibility = System.Windows.Visibility.Visible;
                if (TabControlMain.SelectedItem != TabTestDetails)
                {
                    TabControlMain.SelectedItem = TabTestDetails;
                }
                chkIsPara = false;
                //rdbForm.IsChecked = true;
                FlagChk = 2;

                FeatchFormDetails();
            }
            else
            {
                parameters.Visibility = System.Windows.Visibility.Visible;
                TabTestTemplateDetails.Visibility = System.Windows.Visibility.Collapsed;
                if (TabControlMain.SelectedItem != TabTestDetails)
                {
                    TabControlMain.SelectedItem = TabTestDetails;
                }
                chkIsPara = true;
            }

         
        }

        //private void ViewTemplate_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID != 0)
        //    //{
        //    //    ViewPathologyTemplate Win = new ViewPathologyTemplate();
        //    //    Win.TemplateID = ((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID;
        //    //    Win.Show();
        //    //}
        //    //else
        //    //{
        //    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
        //    //               new MessageBoxControl.MessageBoxChildWindow("", "Please select Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //    //    msgW1.Show();
        //    //}
        //}

        #region by rohini dated 19.1.16 
        private void cmdMachine_Click(object sender, RoutedEventArgs e)
        {
             try
            {
                clsPathoTestMasterVO objItemVO = new clsPathoTestMasterVO();
                objItemVO = (clsPathoTestMasterVO)grdPathoTest.SelectedItem;
                if (objItemVO != null)
                {
                    DefineMachine win = new DefineMachine();
                    win.Show();
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtAgeFrom_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
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
        //by rohini dated 20.1.16
        private void cmdParameterSearch_Click(object sender, RoutedEventArgs e)
        {
            FillParameterBySearch1();  
            
            //if (rdbParameter.IsChecked == true && txtParameterSearch.Text == "")
            //    rdbParameter_Checked(sender, e);
            //else if (rdbSubTest.IsChecked == true && txtParameterSearch.Text == "")
            //    rdbSubTest_Checked(sender, e);
            //else if (rdbParameter.IsChecked == true && txtParameterSearch.Text != string.Empty)
            //    FillParameterBySearch1();               
            //else if (rdbSubTest.IsChecked == true && txtParameterSearch.Text != string.Empty)
            //    FillSubTestBySearch1();
            
        }

        private void FillParameterBySearch()
        {
            try
            {
                //if (isView == true || chkIsPara == false)
                //    SubtestViewList = null;

                //chkIsPara = true;

                //if (SubtestViewList == null)
                //    SubtestViewList = new List<clsPathoSubTestVO>();

                //if (grdPathoParameter.ItemsSource != null)
                //{
                //    foreach (var item in grdPathoParameter.ItemsSource)
                //    {
                //        SubtestViewList.Add((clsPathoSubTestVO)item);
                //    }
                //    //SubtestViewList = (List<clsPathoSubTestVO>)grdPathoParameter.ItemsSource;
                //}
                if (rdbParameter.IsChecked == true)
                {
                    //if (isView == true)
                    //{
                    grdPathoParameter.ItemsSource = null;
                    if (ParamaterViewList != null)
                    {
                        if (ParamaterViewList.Count > 0)
                            grdPathoParameter.ItemsSource = ParamaterViewList;
                    }
                    else
                    {
                        ParamaterViewList = null;
                        grdPathoParameter.ItemsSource = ParamaterViewList;
                    }
                    
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = true;
                    FillParameterBySearch1();
                    grdParameterMaster.Columns[1].Header = "Parameter";
                }
                else
                {
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    grdParameterMaster.Columns[1].Header = "Sub Test";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void FillSubTestBySearch()
        {
            try
            {
                if (isView == true || chkIsPara == true)
                    ParamaterViewList = null;

                chkIsPara = false;


                if (ParamaterViewList == null)
                    ParamaterViewList = new List<clsPathoTestParameterVO>();
                if (grdPathoParameter.ItemsSource != null)
                {
                    foreach (var item in grdPathoParameter.ItemsSource)
                    {
                        ParamaterViewList.Add((clsPathoTestParameterVO)item);
                    }
                    //ParamaterViewList =(List<clsPathoTestParameterVO>)grdPathoParameter.ItemsSource;
                }
                if (rdbSubTest.IsChecked == true)
                {
                    //if (isView == true)
                    //{
                    grdPathoParameter.ItemsSource = null;
                    if (SubtestViewList != null)
                    {
                        if (SubtestViewList.Count > 0)
                            grdPathoParameter.ItemsSource = SubtestViewList;
                    }
                    else
                    {
                        SubtestViewList = null;
                        grdPathoParameter.ItemsSource = SubtestViewList;
                    }
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    FillSubTestBySearch1();

                    grdParameterMaster.Columns[1].Header = "Sub Test";

                }
                else
                {
                    ((clsPathoTestMasterVO)this.DataContext).IsParameter = false;
                    grdParameterMaster.Columns[1].Header = "Parameter";

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void FillParameterBySearch1()
        {
            clsGetParameterOrSubTestSearchBizActionVO objBizActionVO = new clsGetParameterOrSubTestSearchBizActionVO();
            objBizActionVO.Parameter = new clsPathoTestMasterVO();
            //if (txtParameterSearch.Text != null && txtParameterSearch.Text != "")
            objBizActionVO.Description = txtParameterSearch.Text;
            objBizActionVO.Flag = (int)1;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                    foreach (var item in ((clsGetParameterOrSubTestSearchBizActionVO)args.Result).ParameterList)
                    {
                        clsPathoTestParameterVO objPathoTestParameter = new clsPathoTestParameterVO();
                        objPathoTestParameter.ParamSTID = item.ID;
                        objPathoTestParameter.Description = item.Description;                      
                        objPathoTestParameter.IsParameter = true;
                        //objPathoTestParameter.FormulaID = item.FormulaID;      
                        objList.Add(objPathoTestParameter);
                    }

                    foreach (var item in objList)
                    {
                        item.PrintPosition = objList.IndexOf(item);
                    }
                
                    grdParameterMaster.ItemsSource = null;
                    grdParameterMaster.ItemsSource = objList;
                    rdbParameter.IsChecked = true;

                }

            };

            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();
        }
        private void FillSubTestBySearch1()
        {
            clsGetParameterOrSubTestSearchBizActionVO objBizActionVO = new clsGetParameterOrSubTestSearchBizActionVO();
            objBizActionVO.Parameter = new clsPathoTestMasterVO();
            if(txtParameterSearch.Text!=null && txtParameterSearch.Text!="")
                objBizActionVO.Description = txtParameterSearch.Text;
            objBizActionVO.Flag = (int)2;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();
                    if (((clsGetParameterOrSubTestSearchBizActionVO)args.Result).ParameterList != null)
                    {
                        foreach (var item in ((clsGetParameterOrSubTestSearchBizActionVO)args.Result).ParameterList)
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
                    }
                    else
                    {
                        grdParameterMaster.ItemsSource = null;
                    }

                }

            };

            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();
        }
        #endregion     
        public ObservableCollection<MasterListItem> FormTempList { get; set; }
        long SampleID = 0;
        public List<clsPathoTemplateVO> FormTempViewList = new List<clsPathoTemplateVO>();
        public List<clsPathoTemplateVO> WordTempViewList = new List<clsPathoTemplateVO>();
        int FlagChk = 1;
        private void rdbFormTemplate_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rdbForm.IsChecked == true)
                {
                    FlagChk=1;
                    if (UserTemplateList != null)
                        UserTemplateList = new List<clsPathoTemplateVO>();
                    FeatchFormDetails();
                    grdParameterMaster.Columns[1].Header = "Form Template";
                    txtTemplateSearch.Text = "";
                   // SeleectedTempList = null;
                }
                else
                {
                    FlagChk = 2;
                    if (UserTemplateList != null)
                        UserTemplateList = new List<clsPathoTemplateVO>();
                    FeatchFormDetails();
                    grdParameterMaster.Columns[1].Header = "Word Template";
                    txtTemplateSearch.Text = "";
                    //SeleectedTempList = null;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        List<clsPathoTemplateVO> UserTemplateList = new List<clsPathoTemplateVO>();
        private void FeatchFormDetails()
        {
            try
            {
               
                clsGetWordOrReportTemplateBizActionVO objBizActionVO = new clsGetWordOrReportTemplateBizActionVO();
                objBizActionVO.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
               // if (txtTemplateSearch.Text != null && txtTemplateSearch.Text != "")
                    objBizActionVO.Description = txtTemplateSearch.Text;
                objBizActionVO.Flag = (int)FlagChk;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //List<clsPathoTemplateVO> objList = new List<clsPathoTemplateVO>();

                        //if (SeleectedTempList != null)
                        //{
                        //    dgTemplateList.ItemsSource = null;
                        //    dgTemplateList.ItemsSource = SeleectedTempList;
                        //    txtParameterSearch.Text = "";
                        //    dgTemplateList.UpdateLayout();
                       

                        //}
                        //else
                        //{
                        UserTemplateList = new List<clsPathoTemplateVO>();
                            objBizActionVO.TemplateDetails = ((clsGetWordOrReportTemplateBizActionVO)args.Result).TemplateDetails;

                            foreach (var item in objBizActionVO.TemplateDetails)
                            {
                                //objList.Add(new clsPathoTemplateVO() { TemplateID = item.ID, Description = item.Description });
                                
                                UserTemplateList.Add(new clsPathoTemplateVO() { TemplateID = item.ID, TemplateName = item.Description, Code = item.Code, Status = false });
                               
                                
                            }
                            //if(((clsPathoTestMasterVO)grdPathoTest.SelectedItem).IsFormTemplate == 4)
                            //{

                            //}
                            clsPathoTemplateVO selected = new clsPathoTemplateVO();
                            if (SeleectedTempList != null)
                            {
                                

                                if (UserTemplateList != null && UserTemplateList.Count > 0)
                                {
                                    foreach (var items in UserTemplateList)
                                    {
                                         foreach (var item in SeleectedTempList)
                                          {

                                              if (item.Status == true && item.TemplateID == items.TemplateID)
                                                {
                                                    items.IsDefault = true;
                                                }

                                            }
                                        }
                                    foreach (var itemnn in UserTemplateList)
                                    {
                                        if (itemnn.IsDefault == true)
                                        {
                                            itemnn.Status = true;
                                        }
                                        else
                                        {
                                            itemnn.Status = false;
                                        }
                                    }                                   
                                }
                            }
                           
                            dgTemplateList.ItemsSource = null;
                            dgTemplateList.ItemsSource = UserTemplateList;
                            txtParameterSearch.Text = "";
                            dgTemplateList.UpdateLayout();
                       
                        //}

                        //if (DetailsVO.TemplateList != null)
                        //{
                        //    List<clsPathoTemplateVO> lstTemplate = (List<clsPathoTemplateVO>)dgTemplateList.ItemsSource;
                        //    foreach (var item1 in DetailsVO.TemplateList)
                        //    {
                        //        if (lstTemplate != null)
                        //        {
                        //            foreach (var item in lstTemplate)
                        //            {
                        //                if (item.TemplateID == item1.TemplateID)
                        //                {
                        //                    item.Status = item1.Status;
                        //                }
                        //            }
                        //        }
                        //        dgTemplateList.ItemsSource = null;
                        //        dgTemplateList.ItemsSource = lstTemplate;
                        //        dgTemplateList.UpdateLayout();
                        //    }
                        //}
                      
                    }

                };

                client.ProcessAsync(objBizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }      

        private void ViewTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID != 0)
            {
              
                //if (rdbForm.IsChecked == true)
                //{

                //    frmViewEMRFormTemplate win1 = new frmViewEMRFormTemplate();
                //    win1.TemplateID1 = ((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID;
                //    //((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID
                //    //Win.Flag =(int) 1;
                //    win1.Show();

                //}

                //else
                //{
                    ViewPathologyTemplate Win = new ViewPathologyTemplate();
                    Win.TemplateID = ((clsPathoTemplateVO)dgTemplateList.SelectedItem).TemplateID;

                    Win.Flag = (int)2;
                    Win.Show();
                //}
               
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please select Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
        }
        private void cmdTemplateSearch_Click(object sender, RoutedEventArgs e)
        {
            FlagChk = 2;
            //if (UserTemplateList != null)
            //    UserTemplateList = new List<clsPathoTemplateVO>();
            //FeatchFormDetails();
            //grdParameterMaster.Columns[1].Header = "Word Template";
            //txtTemplateSearch.Text = "";
           FeatchFormDetails();
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

        private void cmbDeduction_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbDeduction1 = (AutoCompleteBox)sender;

            if (cmbDeduction1.ItemsSource == null || (cmbDeduction1.ItemsSource != null && ((List<MasterListItem>)cmbDeduction1.ItemsSource).ToList().Count == 0) || (((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).DeductionList == null || ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).DeductionList.Count == 0))
            {
               // fillDeduction(cmbDeduction1);
            }
           
        }
        public List<MasterListItem> Dlist = new List<MasterListItem>();
        public List<MasterListItem> Ulist = new List<MasterListItem>();
        private void fillDeduction()
        {
            //MasterListItem Item1 = new MasterListItem();
            //List<MasterListItem> list = new List<MasterListItem>();
            //List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();

            //UOMConvertLIst.Add(new MasterListItem(0, "- Select -"));
            //UOMConvertLIst.Add(new MasterListItem(1, "FIFO"));
            //UOMConvertLIst.Add(new MasterListItem(2, "FEFO"));

            //// cmbUsageType1.ItemsSource = UOMConvertLIst.DeepCopy();            
            //if (dgItemDetailsList.SelectedItem != null)
            //{
            //    ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).DeductionList = UOMConvertLIst.DeepCopy();              
            //    cmbDeduction1.ItemsSource = UOMConvertLIst.DeepCopy();
            //}
            List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            //objConversion.ID = 0;
            //objConversion.Description = "- Select -";
            //UOMConvertLIst.Add(objConversion);

           UOMConvertLIst.Add(new MasterListItem(0, "- Select -"));
            UOMConvertLIst.Add(new MasterListItem(1, "FIFO"));
            UOMConvertLIst.Add(new MasterListItem(2, "FEFO"));
            Dlist = UOMConvertLIst;
            //cmbDeduction1.ItemsSource = UOMConvertLIst;

            //if (UOMConvertLIst != null)
            //    cmbDeduction1.SelectedItem = UOMConvertLIst[0];
            ////List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
            ////UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

            //if (dgItemDetailsList.SelectedItem != null)
            //{
               // ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).DeductionList = UOMConvertLIst;
                //((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
            //}
        }

        private void CmbUsageType_GotFocus(object sender1, RoutedEventArgs e)
        {
            AutoCompleteBox cmbUsageType1 = (AutoCompleteBox)sender1;

            if (cmbUsageType1.ItemsSource == null || (cmbUsageType1.ItemsSource != null && ((List<MasterListItem>)cmbUsageType1.ItemsSource).ToList().Count == 0) || (((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList == null || ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList.Count == 0))
            {
              //  fillUsageType(cmbUsageType1);
            }

        }
       
        private void fillUsageType()
        {
           // MasterListItem Item1 = new MasterListItem();
           // List<MasterListItem> list = new List<MasterListItem>();
           // List<MasterListItem> UOMConvertLIst1 = new List<MasterListItem>();

           // UOMConvertLIst1.Add(new MasterListItem(0, "- Select -"));
           // UOMConvertLIst1.Add(new MasterListItem(1, "Sample Collection"));
           // UOMConvertLIst1.Add(new MasterListItem(2, "Sample Processing"));
           
           //// cmbUsageType1.ItemsSource = UOMConvertLIst.DeepCopy();            
           // if (dgItemDetailsList.SelectedItem != null)
           // {
           //     ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList = UOMConvertLIst1.DeepCopy();
           //     cmbUsageType1.ItemsSource = UOMConvertLIst1.DeepCopy();
           // }
            List<MasterListItem> UOMConvertLIst1 = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            //objConversion.ID = 0;
            //objConversion.Description = "- Select -";
            //UOMConvertLIst1.Add(objConversion);

            UOMConvertLIst1.Add(new MasterListItem(0, "- Select -"));
            UOMConvertLIst1.Add(new MasterListItem(1, "Sample Collection"));
            UOMConvertLIst1.Add(new MasterListItem(2, "Sample Processing"));
           Ulist = UOMConvertLIst1;
            //cmbUsageType1.ItemsSource = UOMConvertLIst1;

            //if (UOMConvertLIst1 != null)
            //    cmbUsageType1.SelectedItem = UOMConvertLIst1[0];
            ////List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
            ////UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

            //if (dgItemDetailsList.SelectedItem != null)
            //{
               // ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UsageList = UOMConvertLIst1;
                //((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
            //}
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
