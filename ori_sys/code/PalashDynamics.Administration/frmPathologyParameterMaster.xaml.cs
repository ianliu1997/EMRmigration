
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
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace PalashDynamics.Administration
{
    public partial class frmPathologyParameterMaster : UserControl
    {

        #region Public Variables
       
        public bool ValidCode = true;
        public bool ValidDescription = true;
        public bool ValidPrintName = true;
        public bool ValidValues = true;
        public bool ValidTechnique = true;
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        WaitIndicator waiting = new WaitIndicator();
        string msgTitle = "";
        string msgText = "";
        bool IsPageLoaded = false;
        bool IsCancel = true;
        public PagedCollectionView PCVData;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public ObservableCollection<clsPathoParameterHelpValueMasterVO> ItemList { get; set; }
        public ObservableCollection<clsPathoParameterDefaultValueMasterVO> DefaultList { get; set; }
        public bool VDefault, VCategory, VAge, VLLAge, VULAge, VMaxValue, VMinValue, VDefaultValue, VMax, VDefaultRange, VAgeRange, IMax, IMin, IDefault, UpperPanic, LowerPanic, HighReff, LowReff, LReflex, HReflex, VLLReflex, VULReflex, VLLMI, VULMXI, ILMax, ILReff, ChkAge, InvAge, IAge;
        public bool FOperand = false;
        public bool FAdd = false;
        public bool FMinus = false;
        public bool FDivide = false;
        public bool FOpen = false;
        public bool FClose = false;
        public bool FMultiply = false;
        public long OpenCount = 0;
       // public string Formula = "";
        public bool FormulaComplt = true;
        public string FinalFormula = "";
        public bool IsModify = false;
        bool IsNew = false;
        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        bool VaryingReferenceCheck = true;
        //by rohinee dated 22/11/16
        public bool IsValidFormula = false;
        string FormulaID = "";
        //
        //string textBefore = null;
        //int selectionStart = 0;
        //int selectionLength = 0;
        //public operator
        #endregion

        #region Pagging


        public PagedSortableCollectionView<clsPathoParameterMasterVO> DataList { get; private set; }

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
            }
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillParameterList();
            startindex = 1;
            //Fill the Grid on Front Panel.
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        #endregion

        public frmPathologyParameterMaster()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            dgDefaultValues.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgDefaultValues_CellEditEnded);
            dgHelpValues.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgHelpValues_CellEditEnded);
            //this.Unloaded += new RoutedEventHandler(UserControl_Unloaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathoParameterMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        void dgDefaultValues_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        { }

        void dgHelpValues_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        { }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                this.DataContext = null;
                this.DataContext = new clsPathoParameterMasterVO();
                ItemList = new ObservableCollection<clsPathoParameterHelpValueMasterVO>();
                DefaultList = new ObservableCollection<clsPathoParameterDefaultValueMasterVO>();
                //fill the List on Front Panel.
                FillParameterList();
                FillParameterUnit();
                FillParameterCategory();
                FillAge();
                //FillParameterFormula();
                SetCommandButtonState("Load");
                cmdModifToListDefault.IsEnabled = false;
                cmdAddService.IsEnabled = false;
                HandleOperator(false);
            }
            IsPageLoaded = true;
        }

        /// <summary>
        /// This Function is Called Whenever We Have To Fill The Grid On The Front Panel.
        /// The Master Data From The Respective Table is Retrieved For The List.        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FillParameterList()
        {
            try
            {
                waiting.Show();
                clsGetPathoParameterMasterBizActionVO objTempList = new clsGetPathoParameterMasterBizActionVO();
                objTempList.IsPagingEnabled = true;
                objTempList.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objTempList.MaximumRows = DataList.PageSize;
                objTempList.AllParameter = false;
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
                        clsGetPathoParameterMasterBizActionVO result = ea.Result as clsGetPathoParameterMasterBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.ParameterList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.ParameterList)
                            {
                                DataList.Add(item);
                            }
                            dgPathoParameterList.ItemsSource = null;
                            dgPathoParameterList.ItemsSource = DataList;

                            //dgDataPager.Source = null;
                            //dgDataPager.PageSize = objTempList.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured while filling the Pathology Parameter List";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                client.ProcessAsync(objTempList, User);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw ex;
                waiting.Close();
            }
            finally
            {
                waiting.Close();
            }

        }

        /// <summary>
        /// This Event is Called When We click on New Button, the BackPanel is displayed
        /// The BackPanel is Cleared and Command Buttons State is set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VLLAge = true;
                VULAge = true;
                UpperPanic = true;
                dgPathoParameterList.SelectedIndex = -1;
                PathoDetailsInfo.SelectedIndex = 0;
                // tabDashBoard.SelectedIndex = 0;
                //IsNew = true;
                ClearFormData();
                IsNew = true;
                FillParameterCategory();
                // FillParameterFormula();
                FillParameterUnit();
                SetFormValidation();
               // ViewServices1.Visibility = Visibility.Collapsed;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New Parameter Master";
                _flip.Invoke(RotationType.Forward);
                this.SetCommandButtonState("New");
                cmdAddToListDefault.IsEnabled = true;
                cmdModifToListDefault.IsEnabled = false;
                //added by rohinee
                //ExistFormula.Visibility = Visibility.Collapsed;
                //txtExistFormula.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Event is Called When We click on Modify Button and Update Master Tables Details
        /// (For Add and Modify Master Tables Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        bool isNotDefault = false;
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                SetFormValidation();

                if (dgHelpValues.ItemsSource != null)
                {
                    //List<clsPathoParameterHelpValueMasterVO> HelpList =new List<clsPathoParameterHelpValueMasterVO>();
                    List<clsPathoParameterHelpValueMasterVO> HelpList = new List<clsPathoParameterHelpValueMasterVO>();
                    //List<clsPathoParameterHelpValueMasterVO> HelpList=new List<clsPathoParameterHelpValueMasterVO>();
                   // HelpList = (List<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource;

                    if (dgHelpValues.ItemsSource.GetType().Name.Equals("ObservableCollection`1"))
                    {
                        HelpList = (((ObservableCollection<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource)).ToList();
                    }
                    else
                        HelpList = ((List<clsPathoParameterHelpValueMasterVO>)(dgHelpValues.ItemsSource));

                    if (HelpList.Count > 0)
                    {
                        foreach (var item in HelpList)
                        {
                            if (item.IsDefault == true)
                            {
                                isNotDefault = true;
                                break;
                            }
                            else
                            {
                                isNotDefault = false;
                            }

                        }

                        if (isNotDefault == false)
                        {
                            msgText = "Please Select At List One Default Value";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        isNotDefault = true;
                    }
                }

                if (ValidCode == true && ValidDescription == true && ValidPrintName == true && FormulaComplt == true && isNotDefault == true && ValidTechnique == true)
                {
                    waiting.Close();
                    msgText = "Are You Sure \n  You Want To Modify The Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_Update_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else if (ValidCode == false)
                    txtCode.Focus();
                else if (ValidDescription == false)
                    txtName.Focus();
                else if (ValidPrintName == false)
                    txtPrintName.Focus();
                else if (ValidTechnique == false)
                    txtTechUsed.Focus();

                else if (FormulaComplt == false)
                {
                    PathoDetailsInfo.TabIndex = 2;
                    txtFormula.Focus();
                    msgText = "Incorrect Formula Syntax.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                //else if (ValidValues == false)
                //{
                //    msgText = "Please Enter Default Values or Help Values";
                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgWindow.Show();
                //}
                waiting.Close();
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw ex;
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
            waiting.Show();
            try
            {
                SetFormValidation();
                //if (dgHelpValues.ItemsSource != null)
                //{
                //    //List<clsPathoParameterHelpValueMasterVO> HelpList =new List<clsPathoParameterHelpValueMasterVO>();
                //    //ObservableCollection<clsPathoParameterHelpValueMasterVO> HelpList = new ObservableCollection<clsPathoParameterHelpValueMasterVO>();
                //    //List<clsPathoParameterHelpValueMasterVO> HelpList=new List<clsPathoParameterHelpValueMasterVO>();
                //    //HelpList = ((ObservableCollection<clsPahoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource).Count;
                //    if (((ObservableCollection<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource).Count > 0)
                //    {
                //        foreach (var item in (ObservableCollection<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource)
                //        {
                //            if (item.IsDefault == true)
                //            {
                //                isNotDefault = true;
                //                break;
                //            }
                //            else
                //            {
                //                isNotDefault = false;
                //            }

                //        }
                //        if (isNotDefault == false)
                //        {
                //            msgText = "Please Select At List One Default Value";
                //            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            msgWindow.Show();
                //        }
                //    }
                //    else
                //    {
                //        isNotDefault = true;
                //    }
                //}
                if (dgHelpValues.ItemsSource != null)
                {
                    //List<clsPathoParameterHelpValueMasterVO> HelpList =new List<clsPathoParameterHelpValueMasterVO>();
                    List<clsPathoParameterHelpValueMasterVO> HelpList = new List<clsPathoParameterHelpValueMasterVO>();
                    //List<clsPathoParameterHelpValueMasterVO> HelpList=new List<clsPathoParameterHelpValueMasterVO>();
                    // HelpList = (List<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource;

                    if (dgHelpValues.ItemsSource.GetType().Name.Equals("ObservableCollection`1"))
                    {
                        HelpList = (((ObservableCollection<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource)).ToList();
                    }
                    else
                        HelpList = ((List<clsPathoParameterHelpValueMasterVO>)(dgHelpValues.ItemsSource));

                    if (HelpList.Count > 0)
                    {
                        foreach (var item in HelpList)
                        {
                            if (item.IsDefault == true)
                            {
                                isNotDefault = true;
                                break;
                            }
                            else
                            {
                                isNotDefault = false;
                            }

                        }

                        if (isNotDefault == false)
                        {
                            msgText = "Please Select At List One Default Value";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        isNotDefault = true;
                    }
                }
                if (ValidCode == true && ValidDescription == true && ValidPrintName == true && FormulaComplt == true && isNotDefault == true && ValidTechnique == true)
                {
                    waiting.Close();
                    msgText = "Are You Sure \n You Want To Save The Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_Update_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else if (ValidCode == false)
                    txtCode.Focus();
                else if (ValidDescription == false)
                    txtName.Focus();

                else if (FormulaComplt == false)
                {
                    PathoDetailsInfo.TabIndex = 2;
                    txtFormula.Focus();
                    msgText = "Incorrect Formula Syntax.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else if (ValidPrintName == false)
                    txtPrintName.Focus();
                else if (ValidTechnique == false)
                    txtTechUsed.Focus();
                //else if (ValidValues == false)
                //{
                //    msgText = "Please Enter Default Values or Help Values";
                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgWindow.Show();
                //}
                waiting.Close();
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw ex;
            }
        }

        /// <summary>
        /// This Function is Called On Click Of Confirmation Message For Modify and Save Event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msgWindowSave_Update_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                    Save();
            }
            else
            {
            }
        }

        /// <summary>f
        /// This Function Is Called To Capture All The Data On The Back Panel. 
        /// In Order To Save Or Modify The Record.
        /// </summary>
        /// <returns></returns>
        private clsPathoParameterMasterVO CreateFormData()
        {
            clsPathoParameterMasterVO objPathoParameter = new clsPathoParameterMasterVO();
            objPathoParameter = (clsPathoParameterMasterVO)this.DataContext;
            objPathoParameter.ParamUnit = ((MasterListItem)cmbUnit.SelectedItem).ID;
            if (optNumeric.IsChecked == true)
                objPathoParameter.IsNumeric = true;
            else
                objPathoParameter.IsNumeric = false;
            objPathoParameter.DefaultValues = DefaultList.ToList();
            objPathoParameter.Items = ItemList.ToList();

            if (FormulaID.Trim() != string.Empty)
            {
                objPathoParameter.FormulaID = FormulaID;
            }

            return objPathoParameter;
        }

        /// <summary>
        /// This Function Is Called To Save The Details Of Instruction Master After Confirmation From User.
        /// </summary>
        void Save()
        {
            try
            {
                clsPathoParameterMasterVO objPathoParameter = CreateFormData();
                clsAddUpdatePathoParameterBizActionVO objParameter = new clsAddUpdatePathoParameterBizActionVO();
                objParameter.PathologyParameter = objPathoParameter;
               // objParameter.PathologyParameter.Formula = txtFormula.Text;
                //objParameter.PathologyParameter = sMultiService;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        if (((clsAddUpdatePathoParameterBizActionVO)ea.Result).SuccessStatus == 1)
                        {
                            // FillFront Panel List FillEmailTemplateList();
                            FillParameterList();
                            FillParameterUnit();
                            FillParameterCategory();
                            if (IsModify == true)
                                IsModify = false;
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = "";
                            SetCommandButtonState("Load");
                            _flip.Invoke(RotationType.Backward);
                            string msgText = "Pathology Parameter Details Added Successfully";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdatePathoParameterBizActionVO)ea.Result).SuccessStatus == 2)
                        {
                            string msgText = "Code is already exist";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdatePathoParameterBizActionVO)ea.Result).SuccessStatus == 3)
                        {
                            string msgText = "Decription is already exist";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }

                    }
                    else
                    {
                        string msgText = "Record Cannot Be Added \n Please Check The Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objParameter, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Event is Called When We click on Cancel Button
        /// If Back Panel Is Open With The Details, Then We Display The Front Panel On Which We Have DataGrid
        /// To Display All Master Tables Records List.
        /// If Front Panel Is Open Then We Traverse To The Pathology Configuration Form Where We Have The List Of All
        /// The Masters Under Radilogy.    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                txtCode.ClearValidationError();
                txtName.ClearValidationError();
                ClearFormData();

                FillParameterUnit();
                FillParameterList();
                FillParameterCategory();
                HandleOperator(false); 

                dgPathoParameterList.SelectedIndex = -1;
                _flip.Invoke(RotationType.Backward);
               // txtSearch.Text = string.Empty;
                cmdSearch_Click(sender, e);
                txtSearchCriteria.Focus();
                SetCommandButtonState("Cancel");

                if (IsCancel == true)
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Pathology Configuration";                   
                    
                    UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPathologyConfiguration") as UIElement;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                }
                else
                {
                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                    mElement1.Text = " ";
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Event is Called When We click on Search Icon
        /// This Filters The List Displayed on The Front Panel As Per The Criteria Entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        //added by rohini dated 12/1/2016
        int startindex = 1;
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //by rohini
            //startindex = 0;
            FillParameterList();
            dgDataPager.PageIndex = 0;

        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdatePathoParameterStatusBizActionVO objUpdateStatus = new clsUpdatePathoParameterStatusBizActionVO();
            objUpdateStatus.PathoParameterStatus = new clsPathoParameterMasterVO();
            objUpdateStatus.PathoParameterStatus.ID = (((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID);
            objUpdateStatus.PathoParameterStatus.Status = (((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).Status);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if ((ea.Error == null) && (ea.Result != null))
                {
                    if (objUpdateStatus.PathoParameterStatus.Status == false)
                    {
                        msgText = "Status Updated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        msgText = "Status Updated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
                else
                {
                    string msgText = "An Error Occured";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(objUpdateStatus, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void optNumeric_Checked(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                PathoDetailsInfo.SelectedIndex = 0;
                TabFormula.Visibility = Visibility.Visible;
                TabDefaultInfo.Visibility = Visibility.Visible;
                TabHelpValues.Visibility = Visibility.Collapsed;
                //docContet1.Visibility = Visibility.Visible;
                //docContet3.Visibility = Visibility.Visible;
                //DocText.Visibility = Visibility.Collapsed;

                //addeed by rohini dated 1.3.16
                txtDelta.Visibility = Visibility.Visible;
                Delta.Visibility = Visibility.Visible;
            }
        }


        private void optValue_Checked(object sender, RoutedEventArgs e)
        {
            if (optValue.IsChecked == true)
            {
                TextChecked();
                //addeed by rohini dated 1.3.16
                txtDelta.Visibility = Visibility.Collapsed;
                Delta.Visibility = Visibility.Collapsed;
            }
        }

        private void TextChecked()
        {
            PathoDetailsInfo.SelectedIndex = 1;
            //docContet1.Visibility = Visibility.Collapsed;
            //docContet3.Visibility = Visibility.Collapsed;
            TabHelpValues.Visibility = Visibility.Visible;
            TabFormula.Visibility = Visibility.Collapsed;
            TabDefaultInfo.Visibility = Visibility.Collapsed;
            // DocText.Visibility = Visibility.Visible;
        }
        //added by rohini dated 17.2.16
        private void chkIsFlagReflex_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIsFlagReflex.IsChecked == true)
            {
                txtLowerReflexValue.IsEnabled = true;
                txtHighReflexValue.IsEnabled = true;
            }
            else
            {
                txtLowerReflexValue.IsEnabled = false;
                txtHighReflexValue.IsEnabled = false;
                txtLowerReflexValue.Text = "";
                txtHighReflexValue.Text = "";
                txtHighReflexValue.ClearValidationError();
                txtLowerReflexValue.ClearValidationError();

            }

        }
        //


        private void dgPathoParameterList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void dgPathoParameterList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgPathoParameterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// This Function Is Called To Avoid Duplicate Entry Of Records.
        /// </summary>
        private bool CheckDuplicasy()
        {
            clsPathoParameterMasterVO Item;
            clsPathoParameterMasterVO Item1;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsPathoParameterMasterVO>)dgPathoParameterList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper().Trim()));
                Item1 = ((PagedSortableCollectionView<clsPathoParameterMasterVO>)dgPathoParameterList.ItemsSource).FirstOrDefault(p => p.ParameterDesc.ToUpper().Equals(txtName.Text.ToUpper().Trim()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsPathoParameterMasterVO>)dgPathoParameterList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsPathoParameterMasterVO>)dgPathoParameterList.ItemsSource).FirstOrDefault(p => p.ParameterDesc.ToUpper().Equals(txtName.Text.ToUpper()) && p.ID != ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID);
            }
            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Cannot Be Saved Because CODE Already Exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Cannot Be Saved Because Description Already Exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// This Function Is Called To Retrieve The Details Of The Selected Parameter.
        /// </summary>
        /// <param name="ParameterID"></param>
        private void ShowParameterDetails(long ParameterID)
        {
            try
            {
                clsGetPathoParameterByIDBizActionVO BizActionObj = new clsGetPathoParameterByIDBizActionVO();
                clsPathoParameterMasterVO Obj = new clsPathoParameterMasterVO();

                BizActionObj.Details = Obj;
                BizActionObj.Details.ID = ParameterID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        PathoDetailsInfo.SelectedIndex = 0;

                        clsPathoParameterMasterVO ObjParameter = new clsPathoParameterMasterVO();
                        ObjParameter = ((clsGetPathoParameterByIDBizActionVO)ea.Result).Details;
                        this.DataContext = ObjParameter;
                        dgDefaultValues.ItemsSource = ObjParameter.DefaultValues;
                        dgDefaultValues.UpdateLayout();
                        dgHelpValues.ItemsSource = ObjParameter.Items;
                        dgHelpValues.UpdateLayout();
                        cmbUnit.SelectedValue = (long)ObjParameter.ParamUnit;
                        foreach (var item in ObjParameter.DefaultValues)
                        {
                            DefaultList.Add(item);
                        }
                        foreach (var item in ObjParameter.Items)
                        {
                            ItemList.Add(item);
                        }
                        if (ObjParameter.IsNumeric == false)
                        {
                            //PathoDetailsInfo.SelectedIndex = 1;
                            optValue.IsChecked = true;
                            TextChecked();
                        }

                    }
                };
                client.ProcessAsync(BizActionObj, User);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
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
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPathoParameterList.SelectedItem != null)
                {
                    waiting.Show();
                    ClearFormData();
                    IsNew = false;
                    IsModify = true;
                    // PathoDetailsInfo.SelectedIndex = 0;
                    VLLAge = true;
                    VULAge = true;
                    UpperPanic = true;
                    //ExistFormula.Visibility = Visibility.Visible;
                    //txtExistFormula.Visibility = Visibility.Visible;
                    ShowParameterDetails(((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID);
                    waiting.Close();
                    //modify and view service only at the time of modify
                   // ViewServices1.Visibility = Visibility.Visible;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ParameterDesc; //((clsUserVO)dgUserList.SelectedItem).UserGeneralDetailVO.UserName;
                    SetCommandButtonState("View");
                    cmdModify.IsEnabled = ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).Status;
                    _flip.Invoke(RotationType.Forward);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Function Is Called Whenever We Have To Clear Back Panel.        
        /// </summary>        
        private void ClearFormData()
        {
            try
            {
                this.DataContext = new clsPathoParameterMasterVO();
                //txtCode.Text = string.Empty;
                txtCode.Focus();
                //txtName.Text = string.Empty;                
                //txtPrintName.Text = string.Empty;
                //cmbUnit.SelectedIndex = 0;
                // ((MasterListItem)cmbCategory.SelectedItem).ID = 0;
                //((MasterListItem)cmbUnit.SelectedItem).ID = 0;
                cmbCategory.SelectedValue = (long)0;
               
                //by rohini dated 14/1/2016
                cmbMachine.SelectedValue = (long)0;
                cmbAge.SelectedValue = (long)2;
                txtCode.Text = string.Empty;
                txtSearchCriteria.Text = string.Empty;
                txtNumericValue.Text = string.Empty;
                //txtExistFormula.Text = string.Empty;
                txtFormula.Text = string.Empty;
                txtFormulaCode.Text = string.Empty;
                //
                cmbUnit.SelectedValue = (long)0;
                optNumeric.IsChecked = true;
                autoParameter.Text = "";
                DefaultList = new ObservableCollection<clsPathoParameterDefaultValueMasterVO>();
                ItemList = new ObservableCollection<clsPathoParameterHelpValueMasterVO>();
                dgDefaultValues.ItemsSource = DefaultList;
                dgHelpValues.ItemsSource = ItemList;
                IsPara = 0;
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Function Is Called To Set The Validations On The BackPanel Of The Form.
        /// </summary>
        //private void SetFormValidation()
        //{
        //    ValidCode = true;
        //    ValidDescription = true;
        //    ValidPrintName = true;

        //    if (string.IsNullOrEmpty(txtName.Text.Trim()))
        //    {
        //        txtName.SetValidation("Parameter Name Cannot Be Blank.");
        //        txtName.RaiseValidationError();
        //        ValidDescription = false;
        //    }
        //    else
        //        txtName.ClearValidationError();

        //    if (string.IsNullOrEmpty(txtCode.Text.Trim()))
        //    {
        //        txtCode.SetValidation("Code Cannot Be Blank.");
        //        txtCode.RaiseValidationError();
        //        ValidCode = false;
        //    }
        //    else
        //        txtCode.ClearValidationError();

        //    if (string.IsNullOrEmpty(txtPrintName.Text.Trim()))
        //    {
        //        txtPrintName.SetValidation("Print Name Cannot Be Blank.");
        //        txtPrintName.RaiseValidationError();
        //        ValidPrintName = false;
        //    }
        //    else
        //        txtPrintName.ClearValidationError();
          
        //}
        private void SetFormValidation()
        {
            ValidCode = true;
            ValidDescription = true;
            ValidPrintName = true;
            ValidValues = true;
            ValidTechnique = true;
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.SetValidation("Parameter Name Cannot Be Blank.");
                txtName.RaiseValidationError();
                ValidDescription = false;
            }
            else
                txtName.ClearValidationError();

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Code Cannot Be Blank.");
                txtCode.RaiseValidationError();
                ValidCode = false;
            }
            else
                txtCode.ClearValidationError();

            if (string.IsNullOrEmpty(txtPrintName.Text.Trim()))
            {
                txtPrintName.SetValidation("Print Name Cannot Be Blank.");
                txtPrintName.RaiseValidationError();
                ValidPrintName = false;
            }
            else
                txtPrintName.ClearValidationError();


            if (string.IsNullOrEmpty(txtTechUsed.Text.Trim()))
            {
                txtTechUsed.SetValidation("Print Name Cannot Be Blank.");
                txtTechUsed.RaiseValidationError();
                ValidTechnique = false;
            }
            else
                txtTechUsed.ClearValidationError();
            //if (dgDefaultValues.ItemsSource == null && dgHelpValues.ItemsSource == null)
            //{
            //    ValidValues = false;
            //}

            //rohini for code
            if (txtCode.Text.Length > 0 && txtCode.Text.IsOperatorNameInValid() == true)
            {
                txtCode.SetValidation("Parameter Code Can Not Allow Operator And Brackets ()");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                ValidTechnique = false;
            }
            else
                txtCode.ClearValidationError();

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
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAddToListDefault.IsEnabled = true;
                    cmdModifToListDefault.IsEnabled = false;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void PathoDetailsInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdChange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToHelpValueList()
        {
            try
            {
                clsPathoParameterHelpValueMasterVO tempValue = new clsPathoParameterHelpValueMasterVO();
                tempValue.strHelp = txtHelpValue.Text.Trim();
                //if(chkDefault.IsChecked==true)
                tempValue.IsDefault = chkDefault.IsChecked.Value;
                //added by rohini 
                tempValue.IsAbnoramal = chkAbnoramal.IsChecked.Value;
                ItemList.Add(tempValue);
                dgHelpValues.ItemsSource = null;
                dgHelpValues.ItemsSource = ItemList;

                txtHelpValue.Text = "";
                chkDefault.IsChecked = false;
                chkAbnoramal.IsChecked = false;
                msgText = "Help Value Added To The List successfully.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        ///This Event Is Called When User Wants To Add The Details To The Help Value's Grid 
        ///Of The Help Value Tab. First All The Details Are Validated and Then Added To The List.
        ///</summary>
        ///
        bool IsDefault = false;
        bool IsAbonormal = false;
        string ReffValue = "";
        private void cmdAddToList_Click(object sender, RoutedEventArgs e)
        {
            // add to list.
            try
            {
                clsPathoParameterHelpValueMasterVO tempValue = new clsPathoParameterHelpValueMasterVO();
                if (!string.IsNullOrEmpty(txtHelpValue.Text))
                {
                    //if (chkDefault.IsChecked == true )
                    //{
                    //    IsDefault = true;
                    //}

                    //if (chkAbnoramal.IsChecked == true)
                    //{
                    //    IsAbonormal = true;
                    //}
                    //if(txtDefaultValue.Text.Trim()!=string.Empty)
                    //{
                    //    ReffValue=txtDefaultValue.Text.Trim();
                    //}
                    if (chkDefault.IsChecked == true)
                    {
                        //Chk For Default
                        var item = from r in ItemList
                                   where (r.IsDefault == true )
                                   select new clsPathoParameterHelpValueMasterVO
                                   {
                                       IsDefault = r.IsDefault,                                       
                                       IsAbnoramal = r.IsAbnoramal,
                                       strHelp = r.strHelp,
                                   };

                        var itemDupl = from r in ItemList
                                       where (r.strHelp == txtHelpValue.Text.Trim())
                                   select new clsPathoParameterHelpValueMasterVO
                                   {
                                       IsDefault = r.IsDefault,
                                       IsAbnoramal = r.IsAbnoramal,
                                       strHelp = r.strHelp,
                                   };
                        if (itemDupl.ToList().Count() > 0)
                        {
                            msgText = "A Help Value Already Exists.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //chkDefault.IsChecked = false;
                        }
                        else if (item.ToList().Count > 0)
                        {
                            msgText = "A Default Help Value Already Exists.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            chkDefault.IsChecked = false;                          
                        }
                        else
                        {
                            AddToHelpValueList();
                            item.ToList().Clear();
                        }
                    }
                    else
                    {
                        var itemDupl = from r in ItemList
                                       where (r.strHelp == txtHelpValue.Text.Trim())
                                       select new clsPathoParameterHelpValueMasterVO
                                       {
                                           IsDefault = r.IsDefault,
                                           IsAbnoramal = r.IsAbnoramal,
                                           strHelp = r.strHelp,
                                       };
                        if (itemDupl.ToList().Count() > 0)
                        {
                            msgText = "A Help Value Already Exists.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            chkDefault.IsChecked = false;
                        }
                        else
                        {
                            AddToHelpValueList();
                        }
                    }

                    //added by rohini

                    //if (chkAbnoramal.IsChecked == true)
                    //{
                    //    //Chk For Default
                    //    var item = from r in ItemList
                    //               where (r.IsAbnoramal == true)
                    //               select new clsPathoParameterHelpValueMasterVO
                    //               {
                    //                   IsAbnoramal = r.IsAbnoramal,
                    //                   strHelp = r.strHelp
                    //               };
                    //    if (item.ToList().Count == 0)
                    //    {
                    //        AddToHelpValueList();
                    //    }
                    //    else
                    //    {
                    //        msgText = "A Default Help Value Already Exists.";
                    //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //        msgWindow.Show();
                    //        chkAbnoramal.IsChecked = false;
                    //    }
                    //}
                    //else
                    //{
                    //    AddToHelpValueList();
                    //}
                    //
                }
                else
                {
                    msgText = "Help Value not entered";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    txtHelpValue.Focus();
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void cmdOpen_Click(object sender, RoutedEventArgs e)
        {
            if (FOperand == false)// && FClose == false)
            {
                //Concatinate
                if (string.IsNullOrEmpty(txtFormula.Text))
                {
                    txtFormula.Text = "(";
                    txtFormulaCode.Text = "(";  //by rohini
                    FOpen = true;
                    OpenCount = OpenCount + 1;
                }
                else
                {
                    txtFormula.Text = txtFormula.Text + "(";
                    txtFormulaCode.Text = txtFormulaCode.Text + "(";
                    FOpen = true;
                    OpenCount = OpenCount + 1;
                }
                //HandleOperator(false);
            }
            else if (FOperand == true)
            {
                // msgText = "Incorrect Formula Syntax. An Operator is Required.";

                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperatorValidation_Msg");
                }
                else
                {
                    msgText = "An operator is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FOpen = false;
            }
            else if (FClose == true)
            {
                //msgText = "Incorrect Formula Syntax. An Operator is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperatorValidation_Msg");
                }
                else
                {
                    msgText = "An operator is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FOpen = false;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (FOperand == true && OpenCount > 0)
            {
                txtFormula.Text = txtFormula.Text + ")";
                txtFormulaCode.Text = txtFormulaCode.Text + ")";
                OpenCount--;
                FClose = true;
                //HandleOperator(false);
            }
            else if (OpenCount <= 0)
            {
                // msgText = "Incorrect Formula Syntax. An Equivalent Opening Bracket is Required.";

                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("BracketValidation_Msg");
                }
                else
                {
                    msgText = "An equivalent opening bracket is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FClose = false;
            }
            else if (FOperand == false)
            {
                // msgText = "Incorrect Formula Syntax. An Operand is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperandValidation_Msg");
                }
                else
                {
                    msgText = "An operand is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FClose = false;
            }

        }

        private void cmdPlus_Click(object sender, RoutedEventArgs e)
        {
            if (IsModify == true || (FOperand == true && !string.IsNullOrEmpty(txtFormula.Text)))
            {
                txtFormula.Text = txtFormula.Text + "+";
                txtFormulaCode.Text = txtFormulaCode.Text + "+";  //by rohini
                FOperand = false;
                FAdd = true;
                HandleOperator(false);
            }
            else
            {
                // msgText = "Incorrect Formula Syntax. An Operand is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperandValidation_Msg");
                }
                else
                {
                    msgText = "An operand is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FAdd = false;
            }
        }

        private void cmdMinus_Click(object sender, RoutedEventArgs e)
        {
            if (IsModify == true || (FOperand == true && !string.IsNullOrEmpty(txtFormula.Text)))
            {
                txtFormula.Text = txtFormula.Text + "-";
                txtFormulaCode.Text = txtFormulaCode.Text + "-";
                FOperand = false;
                FMinus = true;
                HandleOperator(false);
            }
            else
            {
                //msgText = "Incorrect Formula Syntax. An Operand is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperandValidation_Msg");
                }
                else
                {
                    msgText = "An operand is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FMinus = false;
            }
        }

        private void cmdMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (IsModify == true || (FOperand == true && !string.IsNullOrEmpty(txtFormula.Text)))
            {
                txtFormula.Text = txtFormula.Text + "*";
                txtFormulaCode.Text = txtFormulaCode.Text + "*";
                FOperand = false;
                FMultiply = true;
                HandleOperator(false);
            }
            else
            {
                // msgText = "Incorrect Formula Syntax. An Operand is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperandValidation_Msg");
                }
                else
                {
                    msgText = "An operand is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FMultiply = false;
            }
        }

        private void cmdDivide_Click(object sender, RoutedEventArgs e)
        {
            if (IsModify == true || (FOperand == true && !string.IsNullOrEmpty(txtFormula.Text)))
            {
                txtFormula.Text = txtFormula.Text + "/";
                txtFormulaCode.Text = txtFormulaCode.Text + "/";
                FOperand = false;
                FDivide = true;
                HandleOperator(false);
            }
            else
            {
                // msgText = "Incorrect Formula Syntax. An Operand is Required.";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperandValidation_Msg");
                }
                else
                {
                    msgText = "An operand is required.";
                }
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FDivide = false;
            }
        }
        private void HandleOperator(bool status)
        {
            //cmdOpen.IsEnabled = status;
            //cmdClose.IsEnabled = status;
            cmdPlus.IsEnabled = status;
            cmdMinus.IsEnabled = status;
            cmdMultiply.IsEnabled = status;
            cmdDivide.IsEnabled = status;

            cmdAddParameter.IsEnabled = !status;
            cmdAddNumeric.IsEnabled = !status;

            autoParameter.Text = string.Empty;
            txtNumericValue.Text = string.Empty;

            autoParameter.IsEnabled = !status;
            txtNumericValue.IsEnabled = !status;

        }

        public void FillParameterUnit()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathoParameterUnits;
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
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;
                    cmbUnit.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, User);
            client.CloseAsync();
        }

        public void FillParameterCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathoParameterCategoryMaster;
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

                    cmbCategory.ItemsSource = null;
                    cmbCategory.ItemsSource = objList;
                    cmbCategory.SelectedItem = objList[0];
                }
                //by rohini dated 14/1/2016 as per discussion with nilesh sir
                FillParameterMachine();
                FillAge();
            };
            client.ProcessAsync(BizAction, User);
            client.CloseAsync();
        }
        public void FillParameterMachine()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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

                    cmbMachine.ItemsSource = null;
                    cmbMachine.ItemsSource = objList;
                    cmbMachine.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, User);
            client.CloseAsync();
        }

        //added by rohini dated 23/2/16 as per disscussion with priyanka mam
        public void FillAge()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            //objList.Add(new MasterListItem(0, "-- Select --"));
            objList.Add(new MasterListItem(0, "Days"));
            objList.Add(new MasterListItem(1, "Months"));
            objList.Add(new MasterListItem(2, "Years"));          
            cmbAge.ItemsSource = null;
            cmbAge.ItemsSource = objList;           
            foreach (var item in objList)
            {
                if ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem != null)
                {
                    if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeValue == item.Description)
                    {
                        cmbAge.SelectedItem = item;
                    }
                }
                else if (item.ID == 2)
                {
                    cmbAge.SelectedItem = item;
                }
            }
            //if ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem != null)
            //{
            //    if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeValue != string.Empty)
            //    {
            //        ((MasterListItem)cmbAge.SelectedItem).Description = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeValue;
            //    }
            //}
            
        }

        /// <summary>
        /// This Funct
        /// </summary>
        private void FillParameterFormula()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, ""));                   
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    autoParameter.ItemsSource = null;
                    autoParameter.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillParameterFormula1()  //by rohini
        {
            clsGetSearchMasterListBizActionVO BizAction = new clsGetSearchMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("", ""));
                    objList.AddRange(((clsGetSearchMasterListBizActionVO)e.Result).MasterList);
                    autoParameter.ItemsSource = null;
                    autoParameter.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void txtHelpValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHelpValue.Text))
            {
                cmdAddToList.IsEnabled = true;
            }
        }

        ///<summary>
        ///This Event Is Called When User Wants To Delete the Selected Row in The Default Value's Grid 
        ///Of The Default Tab.
        ///</summary>               
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgHelpValues.SelectedItem != null)
                {
                    msgText = "Are You Sure \n You Want To Delete The Selected Item ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ItemList.RemoveAt(dgHelpValues.SelectedIndex);
                            dgHelpValues.Focus();
                            dgHelpValues.UpdateLayout();
                            if (ItemList.Count == 0)
                                dgHelpValues.ItemsSource = null;
                            else
                                dgHelpValues.SelectedIndex = ItemList.Count - 1;

                            //foreach (var item in ItemList.ToList())
                            //{
                            //    if (item == (clsPathoParameterHelpValueMasterVO)dgHelpValues.SelectedItem)
                            //    {
                            //        ItemList.Remove(item);
                            //    }

                            //}
                            dgHelpValues.ItemsSource = ItemList;
                        }
                    };
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        ///This Event Is Called When User Wants To Add The Details To The Default Value's Grid 
        ///Of The Default Tab. First All The Details Are Validated and Then Added To The List.
        ///</summary>
        private void cmdAddToListDefault_Click(object sender, RoutedEventArgs e)
        {
            clsPathoParameterDefaultValueMasterVO DetailValue = new clsPathoParameterDefaultValueMasterVO();
            try
            {
                ValidateDefault();

                if (VCategory != false && VLLAge != false && VULAge != false && VMaxValue != false && VMinValue != false && VMax != false && VAgeRange != false && HighReff != false && UpperPanic != false && IDefault != false && HReflex != false && VLLReflex != false && VULReflex != false && VLLMI != false && VULMXI != false && ILMax != false && ILReff != false && ChkAge != false && InvAge != false && VaryingReferenceCheck != false)  // && IAge!=false
                {
                    // #region commented because here it is not required to check for duplicate entery
                    var item = from r in DefaultList
                               where (r.CategoryID == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).ID && r.AgeFrom == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).AgeFrom && r.AgeTo == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).AgeTo)
                               select new clsPathoParameterDefaultValueMasterVO
                               {
                                   Category = r.Category,
                                   CategoryID = r.CategoryID,
                                   IsAge = r.IsAge,
                                   AgeFrom = r.AgeFrom,
                                   AgeTo = r.AgeTo,
                                   DefaultValue = r.DefaultValue,
                                   MaxValue = r.MaxValue,
                                   MinValue = r.MinValue,
                                   //by rohini dated 15/1/2016
                                   MachineID = r.MachineID,
                                   Machine = r.Machine,
                                   MaxImprobable = r.MaxImprobable,
                                   MinImprobable = r.MinImprobable,
                                   HighReffValue = r.HighReffValue,
                                   LowReffValue = r.LowReffValue,
                                   UpperPanicValue = r.UpperPanicValue,
                                   LowerPanicValue = r.LowerPanicValue,

                                   LowReflexValue = r.LowReflexValue,
                                   HighReflexValue = r.HighReflexValue,

                                   Note = r.Note,
                                   IsReflexTesting = r.IsReflexTesting,
                                   AgeValue = r.AgeValue,

                                   //Varying Reference Range
                                   
                                  VaryingReference = r.VaryingReference
                                

                               };
                    // #endregion

                    VDefault = true;
                    if ((MasterListItem)cmbCategory.SelectedItem != null)
                    {
                        DetailValue.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
                        DetailValue.Category = ((MasterListItem)cmbCategory.SelectedItem).Description;
                    }
                    if (txtMaxValue.Text != string.Empty)
                        DetailValue.MaxValue = Double.Parse(txtMaxValue.Text.Trim());
                    if (txtMinValue.Text != string.Empty)
                        DetailValue.MinValue = Double.Parse(txtMinValue.Text.Trim());


                    if (txtHighReffValue.Text != string.Empty)
                        DetailValue.HighReffValue = Double.Parse(txtHighReffValue.Text.Trim());
                    if (txtLowReffValue.Text != string.Empty)
                        DetailValue.LowReffValue = Double.Parse(txtLowReffValue.Text.Trim());
                    if (txtUpperPanicValue.Text != string.Empty)
                        DetailValue.UpperPanicValue = Double.Parse(txtUpperPanicValue.Text.Trim());
                    if (txtLowerPanicValue.Text != string.Empty)
                        DetailValue.LowerPanicValue = Double.Parse(txtLowerPanicValue.Text.Trim());


                    if (txtDefaultValue.Text!=string.Empty)
                        DetailValue.DefaultValue = Double.Parse(txtDefaultValue.Text.Trim());
                    //by rohini
                    if (txtMaxImprobable.Text != string.Empty)
                        DetailValue.MaxImprobable = Double.Parse(txtMaxImprobable.Text.Trim());
                    if (txtMinImprobable.Text != string.Empty)
                        DetailValue.MinImprobable = Double.Parse(txtMinImprobable.Text.Trim());

                  

                    if ((MasterListItem)cmbMachine.SelectedItem != null)
                    {
                        DetailValue.MachineID = ((MasterListItem)cmbMachine.SelectedItem).ID;
                        DetailValue.Machine = ((MasterListItem)cmbMachine.SelectedItem).Description;
                    }
                    if (cmbAge.SelectedItem != null)
                        DetailValue.AgeValue =cmbAge.SelectedItem.ToString();
                    DetailValue.Note = txtNote.Text;

                    DetailValue.VaryingReferences = txtVaryingReferences.Text;
                    if (chkIsFlagReflex.IsChecked == true)
                    {
                        DetailValue.IsReflexTesting = true;
                        if (txtLowerReflexValue.Text != string.Empty)
                            DetailValue.LowReflexValue = Double.Parse(txtLowerReflexValue.Text.Trim());
                        if (txtHighReflexValue.Text != string.Empty)
                            DetailValue.HighReflexValue = Double.Parse(txtHighReflexValue.Text.Trim());
                    }
                    else
                    {
                        DetailValue.IsReflexTesting = false;
                    }
                    //
                    if (chkAgeApplicable.IsChecked == true)
                    {
                        DetailValue.IsAge = true;
                        if (txtAgeFrom.Text != string.Empty)
                            DetailValue.AgeFrom = Double.Parse(txtAgeFrom.Text.Trim());
                        if (txtAgeTo.Text != string.Empty)
                            DetailValue.AgeTo = Double.Parse(txtAgeTo.Text.Trim());
                    }
                    DefaultList.Add(DetailValue);
                    dgDefaultValues.ItemsSource = null;
                    dgDefaultValues.ItemsSource = DefaultList;
                    FillParameterCategory();
                    ClearDefaultTab();
                }
                else if (VCategory == false)
                {
                    msgText = "Category Is Required.";
                    VDefault = false;
                    //((MasterListItem)cmbCategory.SelectedItem).ID = 0;
                    cmbCategory.SelectedValue = (long)0;
                }
                else if (VLLAge == false)
                {
                    msgText = "Lower Limit For Age Is Required.";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }

                else if (ChkAge == false)
                {
                    msgText = "Age Should Be Between 1 To 120";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }
                else if (VULAge == false)
                {
                    msgText = "Upper Limit For Age is Required.";
                    VDefault = false;
                    txtAgeTo.Focus();
                }
                //else if (IAge == false)
                //{
                //    msgText = "Invalid Age";
                //    VDefault = false;
                //    txtAgeTo.Focus();
                //}
                else if (InvAge == false)
                {
                    msgText = "The Applicable Age Cannot Be Saved As It Overlaps With Already Defined Applicable Age";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }
                else if (VAgeRange == false)
                {
                    //else if (VAgeRange == true)
                    //{
                    msgText = "Upper Limit For Age Should Be Greater Than Lower Limit.";
                    VDefault = false;
                    txtAgeTo.Focus();
                }
                else if (VMinValue == false)
                {
                    msgText = "Min Reference Value is Required.";
                    VDefault = false;
                    txtMinValue.Focus();
                }
                else if (VMaxValue == false)
                {
                    msgText = "Max Reference Value is Required.";
                    VDefault = false;
                    txtMaxValue.Focus();
                }
                else if (HReflex == false)
                {
                    msgText = "High Reflex Value Should Be Greater Than Low Reflex Value.";
                    VDefault = false;
                    txtHighReflexValue.Focus();
                }
                else if (VLLReflex == false)
                {
                    msgText = "Lower Reflex Value Is Required.";
                    VDefault = false;
                    txtLowerReflexValue.Focus();
                }
                else if (VULReflex == false)
                {
                    msgText = "High Reflex Value is Required.";
                    VDefault = false;
                    txtHighReflexValue.Focus();
                }
                else if (VMax == false)
                {
                    msgText = "Max Reference Value Should Be Greater Than Min Reference Value.";
                    VDefault = false;
                    txtMaxValue.Focus();
                }
                else if (VLLMI == false)
                {
                    msgText = "Min Improbable Value Is Required.";
                    VDefault = false;
                    txtMinImprobable.Focus();
                }
                else if (VULMXI == false)
                {
                    msgText = "Max Improbable Value is Required.";
                    VDefault = false;
                    txtMaxImprobable.Focus();
                }
                //else if (IMax == false)
                //{
                //    msgText = "Maximum Improbable Should Be Greater Than Minimum Improbable.";
                //    VDefault = false;
                //    txtMaxValue.Focus();
                //}
                else if (ILMax == false)
                {
                    msgText = "Maximum Improbable Should Be Greater Than Minimum Improbable.";
                    VDefault = false;
                    txtMaxImprobable.Focus();
                }
                else if (ILReff == false)
                {
                    msgText = "Max Authorization Value Should Be Greater Than Min Authorization Value.";
                    VDefault = false;
                    txtHighReffValue.Focus();
                }
                else if (UpperPanic == false)
                {
                    msgText = "Upper Panic Value Should Be Greater Than Lower Panic Value.";
                    VDefault = false;
                    txtUpperPanicValue.Focus();
                }
                else if (IDefault == false)
                {
                    msgText = "Default Value Should Be Between Min Value and Max Value";
                    VDefault = false;
                }
                else if (VaryingReferenceCheck == false)
                {
                    msgText = "Reference Ranges Are Mandatory";
                    txtVaryingReferences.Focus();
                    VDefault = false;
                }
                else
                {
                    VDefault = true;
                }
                
                if (VDefault == true)
                {
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                    txtMinValue.Text = "";
                    msgText = "Default Value Added To The List Successfully.";                    
                }
              
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Function Is Called on Click Event Of Add To Default Button.
        /// This Function Clears all The Controls On The Default Tab After Adding To The Grid.
        /// </summary>
        private void ClearDefaultTab()
        {
            chkAgeApplicable.IsChecked = false;
            txtAgeFrom.Text = "";
            txtAgeTo.Text = "";
            txtAgeFrom.IsEnabled = false;
            txtAgeTo.IsEnabled = false;
            cmbAge.IsEnabled = false;
            txtMaxValue.Text = "";
            txtMinValue.Text = "";
            txtDefaultValue.Text = "";

            //added by rohini dated 15/1/2016
            txtMaxImprobable.Text = "";
            txtMinImprobable.Text = "";
            txtHighReffValue.Text = "";
            txtLowReffValue.Text = "";
            txtUpperPanicValue.Text = "";
            txtLowerPanicValue.Text = "";
            txtNote.Text = "";
            chkIsFlagReflex.IsChecked = false;
            txtHighReflexValue.Text = "";
            txtLowerReflexValue.Text = "";
            chkIsFlagReflex.IsChecked = false;
            txtVaryingReferences.Text = "";

        }

        /// <summary>
        /// This Function Is Called on Click Event Of Add To Default Button.
        /// This Function Validates all The Controls On The Default Tab Before Adding To The Grid.
        /// </summary>
        private void ValidateDefault()
        {
            double MinValue = 0.0;
            if (txtMinValue.Text != string.Empty)
                MinValue = Convert.ToDouble(txtMinValue.Text.Trim());
            double MaxValue = 0.0;
            if (txtMaxValue.Text != string.Empty)
                MaxValue = Convert.ToDouble(txtMaxValue.Text.Trim());
            double Default = 0.0;
            if (txtDefaultValue.Text != string.Empty)
                Default = Convert.ToDouble(txtDefaultValue.Text.Trim());
            //ADDED BY ROHINI DATED 15/1/2016
            double MinImprobable = 0.0;
            if (txtMinImprobable.Text != string.Empty)
                MinImprobable = Convert.ToDouble(txtMinImprobable.Text.Trim());
            double MaxImprobable = 0.0;
            if (txtMaxImprobable.Text != string.Empty)
                MaxImprobable = Convert.ToDouble(txtMaxImprobable.Text.Trim());

            double LowReflex = 0.0;
            if (txtLowerReflexValue.Text != string.Empty)
                LowReflex = Convert.ToDouble(txtLowerReflexValue.Text.Trim());
            double HighReflex = 0.0;
            if (txtHighReflexValue.Text != string.Empty)
                HighReflex = Convert.ToDouble(txtHighReflexValue.Text.Trim());

            //

            //ADDED BY ROHINI DATED 15/1/2016
            double HighReffValue = 0.0;
            if (txtHighReffValue.Text != string.Empty)
                HighReffValue = Convert.ToDouble(txtHighReffValue.Text.Trim());
            double LowReffValue = 0.0;
            if (txtLowReffValue.Text != string.Empty)
                LowReffValue = Convert.ToDouble(txtLowReffValue.Text.Trim());

            double UpperPanicValue = 0.0;
            if (txtUpperPanicValue.Text != string.Empty)
                UpperPanicValue = Convert.ToDouble(txtUpperPanicValue.Text.Trim());
            double LowerPanicValue = 0.0;
            if (txtLowerPanicValue.Text != string.Empty)
                LowerPanicValue = Convert.ToDouble(txtLowerPanicValue.Text.Trim());
            //

            if (chkAgeApplicable.IsChecked == true)
            {
                
                if (VLLAge == true && VULAge == true)
                {
                    //double MinAge = Convert.ToDouble(txtAgeFrom.Text.Trim());
                    //double MaxAge = Convert.ToDouble(txtAgeTo.Text.Trim());
                    double MinAge = 0.0;
                    if (txtAgeFrom.Text != string.Empty)
                        MinAge = Convert.ToDouble(txtAgeFrom.Text.Trim());
                    double MaxAge = 0.0;
                    if (txtAgeTo.Text != string.Empty)
                        MaxAge = Convert.ToDouble(txtAgeTo.Text.Trim());

                    if (MinAge >= MaxAge)
                    {
                        txtAgeTo.SetValidation("Upper Limit For Age Should Be Greater Than Lower Limit.");
                        txtAgeTo.RaiseValidationError();
                        txtAgeTo.Focus();
                        VAgeRange = false;
                    }
                    else
                    {
                        txtAgeTo.ClearValidationError();
                        VAgeRange = true;
                    }
                }
                //if (!string.IsNullOrEmpty(txtAgeFrom.Text.Trim()) && !string.IsNullOrEmpty(txtAgeFrom.Text.Trim()))
                //{
                //    if (Convert.ToDouble(txtAgeFrom.Text.Trim()) == 0)
                //    {
                //        txtAgeFrom.SetValidation("Invalid Age Limit.");
                //        txtAgeFrom.RaiseValidationError();
                //        txtAgeFrom.Focus();
                //        IAge = false;
                //    }
                //    else
                //    {
                //        txtAgeFrom.ClearValidationError();
                //        IAge = true;
                //    }
                //}
                //else
                //{
                //    txtAgeFrom.ClearValidationError();
                //    IAge = true;
                //}

                if (!string.IsNullOrEmpty(txtAgeFrom.Text.Trim()) && !string.IsNullOrEmpty(txtAgeFrom.Text.Trim()))
                {
                    if (Convert.ToDouble(txtAgeFrom.Text.Trim()) > 120 || Convert.ToDouble(txtAgeTo.Text.Trim()) > 120)
                    {
                        txtAgeFrom.SetValidation("Age Limit Should Be Between 1 to 120");
                        txtAgeFrom.RaiseValidationError();
                        txtAgeFrom.Focus();
                        ChkAge = false;
                    }

                    else
                    {
                        txtAgeFrom.ClearValidationError();
                        ChkAge = true;
                    }

                }
                else
                {
                    txtAgeFrom.ClearValidationError();
                    ChkAge = true;
                }
                //by rohini as per disscussion with dr priyanka 
                if (txtAgeTo.Text.Trim() != string.Empty && txtAgeFrom.Text.Trim() != string.Empty && cmdAddToListDefault.IsEnabled==true)
                {
                    if (DefaultList.Count > 0)
                    {
                        foreach (var item in DefaultList)
                        {
                            if (item.CategoryID == ((MasterListItem)cmbCategory.SelectedItem).ID && ((Convert.ToDouble(txtAgeTo.Text.Trim()) >= Convert.ToDouble(item.AgeFrom) && Convert.ToDouble(txtAgeTo.Text.Trim()) <= Convert.ToDouble(item.AgeTo)) || (Convert.ToDouble(txtAgeFrom.Text.Trim()) >= Convert.ToDouble(item.AgeFrom) && Convert.ToDouble(txtAgeFrom.Text.Trim()) <= Convert.ToDouble(item.AgeTo))) && ((MasterListItem)cmbAge.SelectedItem).Description == item.AgeValue)
                            {
                                txtAgeFrom.SetValidation("Invalid Age Limit");
                                txtAgeFrom.RaiseValidationError();
                                txtAgeFrom.Focus();
                                InvAge = false;
                            }
                            else
                            {
                                txtAgeFrom.ClearValidationError();
                                InvAge = true;
                            }
                        }
                    }
                    else
                    {
                        txtAgeFrom.ClearValidationError();
                        InvAge = true;
                    }

                }                
                else if (txtAgeTo.Text.Trim() != string.Empty && txtAgeFrom.Text.Trim() != string.Empty && cmdAddToListDefault.IsEnabled == false)
                {
                    if (DefaultList.Count > 0)
                    {
                        foreach (var item in DefaultList)
                        {
                            if ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem != item)
                            {
                                if (item.CategoryID == ((MasterListItem)cmbCategory.SelectedItem).ID && ((MasterListItem)cmbAge.SelectedItem).Description == item.AgeValue && ((Convert.ToDouble(txtAgeTo.Text.Trim()) >= Convert.ToDouble(item.AgeFrom) && Convert.ToDouble(txtAgeTo.Text.Trim()) <= Convert.ToDouble(item.AgeTo)) || (Convert.ToDouble(txtAgeFrom.Text.Trim()) >= Convert.ToDouble(item.AgeFrom) && Convert.ToDouble(txtAgeFrom.Text.Trim()) <= Convert.ToDouble(item.AgeTo))))
                                {
                                    txtAgeFrom.SetValidation("Invalid Age Limit");
                                    txtAgeFrom.RaiseValidationError();
                                    txtAgeFrom.Focus();
                                    InvAge = false;
                                    break;
                                }
                                else
                                {
                                    txtAgeFrom.ClearValidationError();
                                    InvAge = true;
                                }
                            }
                            else
                            {
                                txtAgeFrom.ClearValidationError();
                                InvAge = true;
                            }
                        }
                    }
                    else
                    {
                        txtAgeFrom.ClearValidationError();
                        InvAge = true;
                    }

                }
                else
                {
                    txtAgeFrom.ClearValidationError();
                    InvAge = true;
                }
                if (!string.IsNullOrEmpty(txtAgeFrom.Text))
                {
                    txtAgeFrom.ClearValidationError();
                    VLLAge = true;
                }
                else
                {
                    txtAgeFrom.SetValidation("Lower Limit For Age Is Required.");
                    txtAgeFrom.RaiseValidationError();
                    txtAgeFrom.Focus();
                    VLLAge = false;
                }
                if (!string.IsNullOrEmpty(txtAgeTo.Text))
                {
                    txtAgeTo.ClearValidationError();
                    VULAge = true;
                }
                else
                {
                    txtAgeTo.SetValidation("Upper Limit For Age Is Required.");
                    txtAgeTo.RaiseValidationError();
                    txtAgeTo.Focus();
                    VULAge = false;
                }
               
            }
            else
            {
                VLLAge = true;
                VULAge = true;
                VAgeRange = true;
                InvAge = true;
                ChkAge = true;
                //IAge = true;
            }

            if (MinValue >= MaxValue)
            {
                txtMaxValue.SetValidation("Maximum Value Should Be Greater Than Minimum Value.");
                txtMaxValue.RaiseValidationError();
                txtMaxValue.Focus();
                VMax = false;
            }
            else
            {
                txtMaxValue.ClearValidationError();
                VMax = true;
            }
            if (txtHighReffValue.Text != string.Empty && txtLowReffValue.Text != string.Empty)
            {
                if (LowReffValue >= HighReffValue)
                {
                    txtHighReffValue.SetValidation("Max Authorization Value Should Be Greater Than Min Authorization Value.");
                    txtHighReffValue.RaiseValidationError();
                    txtHighReffValue.Focus();
                    ILReff = false;
                }
                else
                {
                    txtHighReffValue.ClearValidationError();
                    ILReff = true;
                }
            }
            else
            {
                txtHighReffValue.ClearValidationError();
                ILReff = true;
            }
            if ((txtUpperPanicValue.Text != string.Empty && txtLowerPanicValue.Text != string.Empty))
            {
                if ((UpperPanicValue != 0.0 && LowerPanicValue != 0.0))
                {
                    if (LowerPanicValue >= UpperPanicValue)
                    {
                        txtUpperPanicValue.SetValidation("Upper Panic Value Should Be Greater Than Lower Panic Value.");
                        txtUpperPanicValue.RaiseValidationError();
                        txtUpperPanicValue.Focus();
                        UpperPanic = false;
                    }
                    else
                    {
                        txtUpperPanicValue.ClearValidationError();
                        UpperPanic = true;
                    }
                }
                else
                {
                    txtUpperPanicValue.ClearValidationError();
                    UpperPanic = true;
                }
            }
            //else if (Convert.ToDouble(txtLowerPanicValue.Text) != 0.0 && Convert.ToDouble(txtUpperPanicValue.Text) != 0.0)
            //{
            //    if (LowerPanicValue >= UpperPanicValue)
            //    {
            //        txtUpperPanicValue.SetValidation("Upper Panic Value Should Be Greater Than Lower Panic Value.");
            //        txtUpperPanicValue.RaiseValidationError();
            //        txtUpperPanicValue.Focus();
            //        UpperPanic = false;
            //    }
            //    else
            //    {
            //        txtUpperPanicValue.ClearValidationError();
            //        UpperPanic = true;
            //    }
            //}
            else
            {
                txtUpperPanicValue.ClearValidationError();
                UpperPanic = true;
            }


            if (Default != 0 && MinValue != 0 && MaxValue != 0)
            {
                if (Default >= MinValue && Default <= MaxValue)
                {
                    txtDefaultValue.ClearValidationError();
                    IDefault = true;
                }
                else
                {
                    txtDefaultValue.SetValidation("Default Value Should Be Between Min Value And Max Value");
                    txtDefaultValue.RaiseValidationError();
                    txtDefaultValue.Focus();
                    IDefault = false;
                }
            }
            else
            {
                IDefault = true;
            }
            //by rohini
            if (!string.IsNullOrEmpty(txtMinImprobable.Text))
            {
                txtMinImprobable.ClearValidationError();
                VLLMI = true;
            }
            else
            {
                txtMinImprobable.SetValidation("Min Improbable Is Required.");
                txtMinImprobable.RaiseValidationError();
                txtMinImprobable.Focus();
                VLLMI = false;
            }
            if (!string.IsNullOrEmpty(txtMaxImprobable.Text))
            {
                txtMaxImprobable.ClearValidationError();
                VULMXI = true;
            }
            else
            {
                txtMaxImprobable.SetValidation("Max Improbable Is Required.");
                txtMaxImprobable.RaiseValidationError();
                txtMaxImprobable.Focus();
                VULMXI = false;
            }
            if (txtMinImprobable.Text != string.Empty && txtMaxImprobable.Text != string.Empty)
            {
                if (MinImprobable >= MaxImprobable)
                {
                    txtMaxImprobable.SetValidation("Maximum Improbable Should Be Greater Than Minimum Improbable.");
                    txtMaxImprobable.RaiseValidationError();
                    txtMaxImprobable.Focus();
                    ILMax = false;
                }
                else
                {
                    txtMaxImprobable.ClearValidationError();
                    ILMax = true;
                }
            }
            else
            {
                ILMax = true;
            }

            if (chkIsFlagReflex.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtLowerReflexValue.Text) && LowReflex>0)
                {
                    txtLowerReflexValue.ClearValidationError();
                    VLLReflex = true;
                }
                else
                {
                    txtLowerReflexValue.SetValidation("Lower Reflex Value Is Required.");
                    txtLowerReflexValue.RaiseValidationError();
                    txtLowerReflexValue.Focus();
                    VLLReflex = false;
                }
                if (!string.IsNullOrEmpty(txtHighReflexValue.Text) && HighReflex>0)
                {
                    txtHighReflexValue.ClearValidationError();
                    VULReflex = true;
                }
                else
                {
                    txtHighReflexValue.SetValidation("Upper Reflex Value Is Required.");
                    txtHighReflexValue.RaiseValidationError();
                    txtHighReflexValue.Focus();
                    VULReflex = false;
                }
                if (txtLowerReflexValue.Text != string.Empty && txtHighReflexValue.Text != string.Empty)
                {
                    if (LowReflex >= HighReflex)
                    {
                        txtHighReflexValue.SetValidation("High Reflex Value Should Be Greater Than Low Reflex Value.");
                        txtHighReflexValue.RaiseValidationError();
                        txtHighReflexValue.Focus();
                        HReflex = false;
                    }
                    else
                    {
                        txtHighReflexValue.ClearValidationError();
                        HReflex = true;
                    }
                }
                else
                {
                    HReflex = true;
                }
            }
            else
            {
                HReflex = true;
                VLLReflex = true;
                VULReflex = true;

            }
            //
            //added by rohini as per dr. priyanka said
            //if (!string.IsNullOrEmpty(txtMaxImprobable.Text))
            //{
            //    txtMaxImprobable.ClearValidationError();
            //    IMax = true;
            //}
            //else
            //{
            //    txtMaxImprobable.SetValidation("Maximum Improbable Value Is Required.");
            //    txtMaxImprobable.RaiseValidationError();
            //    txtMaxImprobable.Focus();
            //    IMax = false;
            //}
          

            if (!string.IsNullOrEmpty(txtMinImprobable.Text))
            {
                txtMinImprobable.ClearValidationError();
                IMin = true;
            }
            else
            {
                txtMinImprobable.SetValidation("Minimum Improbable Value Is Required.");
                txtMinImprobable.RaiseValidationError();
                txtMinImprobable.Focus();
                IMin = false;
            }

            if (((MasterListItem)cmbCategory.SelectedItem).ID > 0)
            {
                cmbCategory.TextBox.ClearValidationError();
                VCategory = true;
            }
            //
            if (!string.IsNullOrEmpty(txtHighReffValue.Text))
            {
                txtHighReffValue.ClearValidationError();
                HighReff = true;
            }
            else
            {
                txtHighReffValue.SetValidation("Max Authorization Value Is Required.");
                txtHighReffValue.RaiseValidationError();
                txtHighReffValue.Focus();
                HighReff = false;
            }
            if (!string.IsNullOrEmpty(txtLowReffValue.Text))
            {
                txtLowReffValue.ClearValidationError();
                LowReff = true;
            }
            else
            {
                txtLowReffValue.SetValidation("Min Authorization Value Is Required.");
                txtLowReffValue.RaiseValidationError();
                txtLowReffValue.Focus();
                LowReff = false;
            }
            if (!string.IsNullOrEmpty(txtMaxValue.Text))
            {
                txtMaxValue.ClearValidationError();
                VMaxValue = true;
            }
            else
            {
                txtMaxValue.SetValidation("Max Reference Value Is Required.");
                txtMaxValue.RaiseValidationError();
                txtMaxValue.Focus();
                VMaxValue = false;
            }

            if (!string.IsNullOrEmpty(txtMinValue.Text))
            {
                txtMinValue.ClearValidationError();
                VMinValue = true;
            }
            else
            {
                txtMinValue.SetValidation("Min Reference Values Is Required.");
                txtMinValue.RaiseValidationError();
                txtMinValue.Focus();
                VMinValue = false;
            }

            if (((MasterListItem)cmbCategory.SelectedItem).ID > 0)
            {
                cmbCategory.TextBox.ClearValidationError();
                VCategory = true;
            }
            else
            {
                cmbCategory.TextBox.SetValidation("Category must be selected");
                cmbCategory.TextBox.RaiseValidationError();
                cmbCategory.TextBox.Focus();
                VCategory = false;
            }


            if (!string.IsNullOrEmpty(txtVaryingReferences.Text))
            {
                txtLowReffValue.ClearValidationError();
                VaryingReferenceCheck = true;
            }
            else
            {
                txtVaryingReferences.SetValidation("Reference Range Is Required.");
                txtVaryingReferences.RaiseValidationError();
                txtVaryingReferences.Focus();
                VaryingReferenceCheck = false;
            }
           
        }

        /// <summary>
        /// This Event Is Called When The Status Of Age Applicable Check Box Changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAgeApplicable_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAgeApplicable.IsChecked == true)
            {
                txtAgeFrom.IsEnabled = true;
                txtAgeTo.IsEnabled = true;
                cmbAge.IsEnabled = true;
                cmbAge.SelectedValue = (long)2;
            }
            else
            {
                txtAgeFrom.IsEnabled = false;
                txtAgeTo.IsEnabled = false;
                cmbAge.IsEnabled = false;
                //by rohini dated 15.1.2016
                txtAgeFrom.Text = "";
                txtAgeTo.Text = "";
            }
        }

        ///<summary>
        ///This Event Is Called When User Wants To Change The Default Help Value in The Help Value Tab. 
        ///This Function Changes The Exsiting Default Help To The Selected Value. 
        ///Only One Default Help Value Can Exists For Each Parameter.
        ///</summary>
        private void chkIsDefault_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgHelpValues.SelectedItem != null)
                {
                    clsPathoParameterHelpValueMasterVO selItem = new clsPathoParameterHelpValueMasterVO();
                    List<clsPathoParameterHelpValueMasterVO> uList = new List<clsPathoParameterHelpValueMasterVO>();

                    uList = (List<clsPathoParameterHelpValueMasterVO>)dgHelpValues.ItemsSource;
                    selItem = (clsPathoParameterHelpValueMasterVO)dgHelpValues.SelectedItem;

                    if (selItem.IsDefault == true)
                    {
                        foreach (var item in uList)
                        {
                            if (selItem.ID == item.ID)
                                item.IsDefault = true;
                            else
                                item.IsDefault = false;
                        }
                        dgHelpValues.ItemsSource = null;
                        dgHelpValues.ItemsSource = uList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Code Is Executed On The Lost Focus Event Of Parameter Selection On The Formula Tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoParameter_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //clsPathoParameterMasterVO Parameter = new clsPathoParameterMasterVO();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Event Is Called WhenUser Wants To Add The Selected Parameter To The Formula Text Box.
        /// Here Validations Are Performed To Confirm That The Formula Is Formed Correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
       public int IsPara = 0;
       public int IsNum = 0;
        private void cmdAddParameter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPathoParameterMasterVO Parameter = new clsPathoParameterMasterVO();
                //if (Parameter != null && !string.IsNullOrEmpty(autoParameter.Text))
                if ((MasterListItem)autoParameter.SelectedItem != null && !string.IsNullOrEmpty(autoParameter.Text))
                {

                    Parameter.ID = ((MasterListItem)autoParameter.SelectedItem).ID;
                    long lngParameterId = Parameter.ID;
                    Parameter.ParameterDesc = ((MasterListItem)autoParameter.SelectedItem).Description;
                    Parameter.Code = ((MasterListItem)autoParameter.SelectedItem).Code;
                    if (string.IsNullOrEmpty(txtFormula.Text))
                    {
                        //FinalFormula = (string)lngParameterId;
                        txtFormula.Text = ((MasterListItem)autoParameter.SelectedItem).Description;
                        txtFormulaCode.Text = ((MasterListItem)autoParameter.SelectedItem).Code;  //by rohini for formula
                        FOperand = true;
                        HandleOperator(true);
                        //Formula = (string)Parameter.ID;
                    }
                    else if (FAdd == true || FMinus == true || FMultiply == true || FDivide == true || FOpen == true)
                    {
                        txtFormula.Text = txtFormula.Text + Parameter.ParameterDesc;
                        txtFormulaCode.Text = txtFormulaCode.Text + Parameter.Code;
                        FOperand = true;
                        HandleOperator(true);
                    }
                    else
                    {
                        // msgText = "Incorrect Formula Syntax. An Operator is Required.";
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperatorValidation_Msg");
                        }
                        else
                        {
                            msgText = "An operator is required.";
                        }
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                        FOperand = false;
                    }
                }
                else
                {
                    //  msgText = "Parameter Not Selected.";
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ParameterSelectValidation_Msg");
                    }
                    else
                    {
                        msgText = "Parameter not selected.";
                    }
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdAddFormula_Click(object sender, RoutedEventArgs e)
        {
            ValidateFomulaSyntax();
        }

        /// <summary>
        /// This Function Id Called to Validat The Controls on Default Tab.
        /// </summary>
        private void ValidateFomulaSyntax()
        {
            if (OpenCount == 0 && FOperand == true && txtFormula.Text !=string.Empty)
            {
                FormulaComplt = true;
                msgText = "Formula Correctly Formed.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                IsValidFormula=true;
            }
            else if (txtFormula.Text == string.Empty)
            {
                msgText = "Please Form A Formula First";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FormulaComplt = false;
                IsValidFormula = false;
            }
            else
            {
                msgText = "Incorrect Formula Syntax.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                FormulaComplt = false;
                IsValidFormula = false;
            }
        }

        private void autoParameter_GotFocus(object sender, RoutedEventArgs e)
        {
            //FillParameterFormula();
            FillParameterFormula1();
        }

        #region All Commented Functions
        private void txtMinValue_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtMinValue.Text.IsItNumber() == false || txtMinValue.Text.IsItDecimal() == false)
            //{
            //    txtMinValue.ClearValidationError();
            //}
            //else
            //{
            //    ValidateTextBoxes(txtMinValue);
            //}
        }

        private void txtMaxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtMaxValue.Text.IsItNumber() == false || txtMaxValue.Text.IsItDecimal() == false)
            //{
            //    txtMaxValue.ClearValidationError();
            //}
            //else
            //{
            //    ValidateTextBoxes(txtMaxValue);
            //}
        }

        private void txtDefaultValue_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtDefaultValue.Text.IsItNumber() == false || txtDefaultValue.Text.IsItDecimal() == false)
            //{
            //    txtDefaultValue.ClearValidationError();
            //}
            //else
            //{
            //    ValidateTextBoxes(txtDefaultValue);
            //}
        }

        /// <summary>
        /// This Function Is Common To Validate The Text Boxes On The Default Value Tab.
        /// </summary>
        /// <param name="txtInValid"></param>
        private void ValidateTextBoxes(TextBox txtInValid)
        {
            txtInValid.SetValidation("Invalid Values Entered. Only Numbers Allowed.");
            txtInValid.RaiseValidationError();
            txtInValid.Focus();
        }

        #endregion

        private void cmdDeleteDefaultItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDefaultValues.SelectedItem != null)
                {
                    msgText = "Are You Sure \n You Want To Delete The Selected Item ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            DefaultList.RemoveAt(dgDefaultValues.SelectedIndex);
                            dgDefaultValues.Focus();
                            dgDefaultValues.UpdateLayout();
                            if (DefaultList.Count == 0)
                                dgDefaultValues.ItemsSource = null;
                            else
                                dgDefaultValues.SelectedIndex = DefaultList.Count - 1;

                            //foreach (var item in DefaultList.ToList())
                            //{
                            //    if (item.ID == ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).ID)
                            //    {
                            //        DefaultList.Remove(item);
                            //    }
                                
                            //}
                            dgDefaultValues.ItemsSource = DefaultList;

                        }
                    };
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdAddNumeric_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumericValue.Text))
            {
                if (string.IsNullOrEmpty(txtFormula.Text))
                {
                    txtFormula.Text = txtNumericValue.Text.Trim();
                    txtFormulaCode.Text = txtNumericValue.Text.Trim();  //by rohini
                    FOperand = true;
                    HandleOperator(true);
                }
                else if (FAdd == true || FMinus == true || FMultiply == true || FDivide == true || FOpen == true)
                {
                    txtFormula.Text = txtFormula.Text + txtNumericValue.Text.Trim();
                    txtFormulaCode.Text = txtFormulaCode.Text + txtNumericValue.Text.Trim();  //by rohini
                    FOperand = true;
                    HandleOperator(true);
                }
                else
                {
                    // msgText = "Incorrect Formula Syntax. An Operator is Required.";
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperatorValidation_Msg");
                    }
                    else
                    {
                        msgText = "An operator is required.";
                    }
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    FOperand = false;
                }
            }
            else
            {
                // msgText = "Value Not Entered.";
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OperatorValidation_Msg");
                //}
                //else
                //{
                //    msgText = "An operator is required.";
                //}
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void txtAgeFrom_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtAgeFrom_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtAgeTo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtAgeTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumericValue_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumericValue_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtSearchCriteria_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                startindex = 0;
                FillParameterList();
            }
        }

        private void hlbEditItems_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                cmdModifToListDefault.IsEnabled = true;
                cmdAddToListDefault.IsEnabled = false;
                cmbCategory.SelectedValue = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).CategoryID;
                cmbMachine.SelectedValue = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).MachineID;
                //txtSampleQuantity.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).Quantity.ToString();
                //float Quantity = (float)Convert.ToDouble(txtSampleQuantity.Text);
                //txtSampleQuantity.Text = String.Format("{0:0.00}", Quantity);               
                txtMinValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).MinValue.ToString();
                txtMaxValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).MaxValue.ToString();
                txtDefaultValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).DefaultValue.ToString();
                txtMinImprobable.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).MinImprobable.ToString();
                txtMaxImprobable.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).MaxImprobable.ToString();
                txtHighReffValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).HighReffValue.ToString();
                txtLowReffValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).LowReffValue.ToString();
                txtUpperPanicValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).UpperPanicValue.ToString();
                txtLowerPanicValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).LowerPanicValue.ToString();

                txtLowerReflexValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).LowReflexValue.ToString();
                txtHighReflexValue.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).HighReflexValue.ToString();
                if ((((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).Note == string.Empty )|| (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).Note == null))
                {
                   
                }
                else
                {
                    txtNote.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).Note.ToString();
                }

                // Varying Reference Ranges
                if ((((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).VaryingReferences == string.Empty) || (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).VaryingReferences == null))
                {

                }
                else
                {
                    txtVaryingReferences.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).VaryingReferences.ToString();
                }
             
                if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).IsAge == true)
                {
                    chkAgeApplicable.IsChecked = true;
                    txtAgeFrom.IsEnabled = true;
                    txtAgeTo.IsEnabled = true;
                    txtAgeFrom.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeFrom.ToString();
                    txtAgeTo.Text = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeTo.ToString();
                    FillAge();
                    //((MasterListItem)cmbAge.SelectedItem).Description = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeValue;
                }            
                else
                {
                    chkAgeApplicable.IsChecked = false;
                    txtAgeFrom.IsEnabled = false;
                    txtAgeTo.IsEnabled = false;
                    FillAge();
                    //((MasterListItem)cmbAge.SelectedItem).Description = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).AgeValue;
                }
                if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).IsReflexTesting == true)
                {
                    txtLowerReflexValue.IsEnabled=true;
                    txtHighReflexValue.IsEnabled = true;
                    chkIsFlagReflex.IsChecked = true;
                }
                else
                {
                    chkIsFlagReflex.IsChecked = false;
                    txtLowerReflexValue.IsEnabled = false;
                    txtHighReflexValue.IsEnabled = false;
                }
           


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModifyToListDefault_Click(object sender, RoutedEventArgs e)
        {

            clsPathoParameterDefaultValueMasterVO DetailValue = new clsPathoParameterDefaultValueMasterVO();
            try
            {
                ValidateDefault();
                int var = dgDefaultValues.SelectedIndex;
                if (dgDefaultValues.SelectedItem != null && VCategory != false && VLLAge != false && VULAge != false && VMaxValue != false && VMinValue != false && VMax != false && VAgeRange != false && UpperPanic != false && HighReff != false && IDefault != false && HReflex != false && VLLReflex != false && VULReflex != false && VLLMI != false && VULMXI != false && ILMax != false && ILReff != false && ChkAge != false && InvAge != false && VaryingReferenceCheck != false)   //&& IAge!=false
                {
                    if (DefaultList.Count() >= 1)
                    {
                       DefaultList.RemoveAt(dgDefaultValues.SelectedIndex);
                    }
                }
                if (VCategory != false && VLLAge != false && VULAge != false && VMaxValue != false && VMinValue != false && VMax != false && VAgeRange != false && UpperPanic != false && HighReff != false && IDefault != false && HReflex != false && VLLReflex != false && VULReflex != false && VLLMI != false && VULMXI != false && ILMax != false && ILReff != false && ChkAge != false && InvAge != false && VaryingReferenceCheck != false)   //&& IAge != false
                {
                    // #region commented because here it is not required to check for duplicate entery
                    var item = from r in DefaultList
                               where (r.CategoryID == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).ID && r.AgeFrom == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).AgeFrom && r.AgeTo == ((clsPathoParameterDefaultValueMasterVO)cmbCategory.SelectedItem).AgeTo)
                               select new clsPathoParameterDefaultValueMasterVO
                               {

                                   Category = r.Category,
                                   CategoryID = r.CategoryID,
                                   IsAge = r.IsAge,
                                   AgeFrom = r.AgeFrom,
                                   AgeTo = r.AgeTo,
                                   DefaultValue = r.DefaultValue,
                                   MaxValue = r.MaxValue,
                                   MinValue = r.MinValue,
                                   //by rohini dated 15/1/2016
                                   MachineID = r.MachineID,
                                   Machine = r.Machine,
                                   MaxImprobable = r.MaxImprobable,
                                   MinImprobable = r.MinImprobable,
                                   HighReffValue = r.HighReffValue,
                                   LowReffValue = r.LowReffValue,
                                   UpperPanicValue = r.UpperPanicValue,
                                   LowReflexValue = r.LowReflexValue,
                                   HighReflexValue = r.HighReflexValue,
                                   LowerPanicValue = r.LowerPanicValue,
                                   Note = r.Note,
                                   AgeValue = r.AgeValue,
                                   VaryingReferences = r.VaryingReferences
                               };
                    // #endregion
                    VDefault = true;
                    if (((MasterListItem)cmbCategory.SelectedItem)!=null)
                    {
                        DetailValue.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
                        DetailValue.Category = ((MasterListItem)cmbCategory.SelectedItem).Description;
                    }
                    if (!string.IsNullOrEmpty(txtMaxValue.Text))
                        DetailValue.MaxValue = Double.Parse(txtMaxValue.Text.Trim());
                    if (!string.IsNullOrEmpty(txtMinValue.Text))
                        DetailValue.MinValue = Double.Parse(txtMinValue.Text.Trim());
                    if (!string.IsNullOrEmpty(txtDefaultValue.Text))
                        DetailValue.DefaultValue = Double.Parse(txtDefaultValue.Text.Trim());
                    //by rohini
                    if (!string.IsNullOrEmpty(txtMaxImprobable.Text))
                        DetailValue.MaxImprobable = Double.Parse(txtMaxImprobable.Text.Trim());
                    if (!string.IsNullOrEmpty(txtMinImprobable.Text))
                        DetailValue.MinImprobable = Double.Parse(txtMinImprobable.Text.Trim());

               


                    if (!string.IsNullOrEmpty(txtHighReffValue.Text))
                        DetailValue.HighReffValue = Double.Parse(txtHighReffValue.Text.Trim());
                    if (!string.IsNullOrEmpty(txtLowReffValue.Text))
                        DetailValue.LowReffValue = Double.Parse(txtLowReffValue.Text.Trim());

                    if (!string.IsNullOrEmpty(txtUpperPanicValue.Text))
                        DetailValue.UpperPanicValue = Double.Parse(txtUpperPanicValue.Text.Trim());
                    if (!string.IsNullOrEmpty(txtLowerPanicValue.Text))
                        DetailValue.LowerPanicValue = Double.Parse(txtLowerPanicValue.Text.Trim());


                    if (((MasterListItem)cmbMachine.SelectedItem)!=null)
                    {
                        DetailValue.MachineID = ((MasterListItem)cmbMachine.SelectedItem).ID;
                        DetailValue.Machine = ((MasterListItem)cmbMachine.SelectedItem).Description;
                    }
                    if (!string.IsNullOrEmpty(txtNote.Text))
                        DetailValue.Note = txtNote.Text;

                    if (!string.IsNullOrEmpty(txtVaryingReferences.Text))
                        DetailValue.VaryingReferences = txtVaryingReferences.Text;         
                    if (chkIsFlagReflex.IsChecked == true)
                    {
                        DetailValue.IsReflexTesting = true;
                        DetailValue.LowReflexValue = Double.Parse(txtLowerReflexValue.Text.Trim());
                        DetailValue.HighReflexValue = Double.Parse(txtHighReflexValue.Text.Trim());
                    }
                    else
                    {
                        DetailValue.IsReflexTesting = false;
                    }
                    //
                    if (chkAgeApplicable.IsChecked == true)
                    {
                        DetailValue.IsAge = true;
                        DetailValue.AgeFrom = Double.Parse(txtAgeFrom.Text.Trim());
                        DetailValue.AgeTo = Double.Parse(txtAgeTo.Text.Trim());
                        DetailValue.AgeValue = cmbAge.SelectedItem.ToString();
                         //((MasterListItem)cmbAge.SelectedItem).Description;
                    }
                    DefaultList.Insert(var, DetailValue);
                    dgDefaultValues.ItemsSource = DefaultList;
                    dgDefaultValues.Focus();
                    dgDefaultValues.UpdateLayout();
                    dgDefaultValues.SelectedIndex = DefaultList.Count - 1;
                    FillParameterCategory();
                    ClearDefaultTab();
                }
                else if (VCategory == false)
                {
                    msgText = "Category Is Required.";
                    VDefault = false;
                    //((MasterListItem)cmbCategory.SelectedItem).ID = 0;
                    cmbCategory.SelectedValue = (long)0;
                }
                else if (VLLAge == false)
                {
                    msgText = "Lower Limit For Age Is Required.";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }
                else if (ChkAge == false)
                {
                    msgText = "Age Should Be Between 1 To 120";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }
                else if (InvAge == false)
                {
                    msgText = "The Applicable Age Cannot Be Saved As It Overlaps With Already Defined Applicable Age";
                    VDefault = false;
                    txtAgeFrom.Focus();
                }
                else if (VULAge == false)
                {
                    msgText = "Upper Limit For Age is Required.";
                    VDefault = false;
                    txtAgeTo.Focus();
                }
                //else if (IAge == false)
                //{
                //    msgText = "Invalid Age";
                //    VDefault = false;
                //    txtAgeTo.Focus();
                //}
                else if (VAgeRange == false)
                {
                    //else if (VAgeRange == true)
                    //{
                    msgText = "Upper Limit For Age Should Be Greater Than Lower Limit.";
                    VDefault = false;
                    txtAgeTo.Focus();
                }
                else if (VMinValue == false)
                {
                    msgText = "Min Reference Value is Required.";
                    VDefault = false;
                    txtMinValue.Focus();
                }
                else if (VMaxValue == false)
                {
                    msgText = "Max Reference Value is Required.";
                    VDefault = false;
                    txtMaxValue.Focus();
                }

                else if (VMax == false)
                {
                    msgText = "Max Reference Value Should Be Greater Than Min Reference Value.";
                    VDefault = false;
                    txtMaxValue.Focus();
                }
                else if (VLLMI == false)
                {
                    msgText = "Max Improbable Value Is Required.";
                    VDefault = false;
                    txtMinImprobable.Focus();
                }
                else if (VULMXI == false)
                {
                    msgText = "Min Improbable Value is Required.";
                    VDefault = false;
                    txtMaxImprobable.Focus();
                }
                //else if (IMax == false)
                //{
                //    msgText = "Maximum Improbable Should Be Greater Than Minimum Improbable.";
                //    VDefault = false;
                //    txtMaxImprobable.Focus();
                //}
                else if (ILReff == false)
                {
                    msgText = "Max Authorization Value Should Be Greater Than Min Authorization Value.";
                    VDefault = false;
                    txtHighReffValue.Focus();
                }
                else if (ILMax == false)
                {
                    msgText = "Maximum Improbable Should Be Greater Than Minimum Improbable.";
                    VDefault = false;
                    txtMaxImprobable.Focus();
                }   
                else if (HReflex == false)
                {
                    msgText = "High Reflex Value Should Be Greater Than Low Reflex Value.";
                    VDefault = false;
                    txtHighReflexValue.Focus();
                }
                else if (VLLReflex == false)
                {
                    msgText = "Lower Reflex Value Is Required.";
                    VDefault = false;
                    txtLowerReflexValue.Focus();
                }
                else if (VULReflex == false)
                {
                    msgText = "High Reflex Value is Required.";
                    VDefault = false;
                    txtHighReflexValue.Focus();
                }
                else if (IDefault == false)
                {
                    msgText = "Default Value Should Be Between Min Value And Max Value";
                    VDefault = false;
                    txtDefaultValue.Focus();

                }
                else if (ILReff == false)
                {
                    msgText = "Max Authorization Value Should Be Greater Than Min Authorization Value.";
                    VDefault = false;
                    txtHighReffValue.Focus();
                }
                else if (UpperPanic == false)
                {
                    msgText = "Upper Panic Value Should Be Greater Than Lower Panic Value.";
                    VDefault = false;
                    txtUpperPanicValue.Focus();
                }
                else if (VaryingReferenceCheck == false)
                {
                    msgText = "Reference Ranges Are Mandatory";
                    txtVaryingReferences.Focus();
                    VDefault = false;
                }
                if (VDefault == true)
                {
                    msgText = "Default Value Modify To The List Successful.";
                    cmdModifToListDefault.IsEnabled = false;
                    cmdAddToListDefault.IsEnabled = true;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else
                {
                    cmdModifToListDefault.IsEnabled = true;
                    cmdAddToListDefault.IsEnabled = false;
                }

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                //cmdModifToListDefault.IsEnabled = false;
                //cmdAddToListDefault.IsEnabled = true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //added by rohini dated 17.2.16
        private void cmdAddService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPathoParameterDefaultValueMasterVO objItemVO = new clsPathoParameterDefaultValueMasterVO();
                objItemVO = (clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem;

                //if (objItemVO != null)
                //{
                if (objItemVO.IsReflexTesting == false)
                {
                    msgText = "Please select reflex testing ";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
                {

                    try
                    {
                        if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).ID != 0)
                        {

                            clsGetPathoServicesByIDBizActionVO BizActionObj = new clsGetPathoServicesByIDBizActionVO();
                            BizActionObj.ID = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).ID;
                            BizActionObj.ParaID = ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID;
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                            client.ProcessCompleted += (s, ea) =>
                            {
                                if (ea.Error == null && ea.Result != null)
                                {

                                    List<MasterListItem> objServiceVO = new List<MasterListItem>();
                                    objServiceVO = ((clsGetPathoServicesByIDBizActionVO)ea.Result).ServiceDetailsList;  //get selected services

                                    DefineServicesForPatho win = new DefineServicesForPatho();
                                    if (objServiceVO != null)
                                    {
                                        win.selectedMasterList = objServiceVO;
                                    }
                                    win.OnAddButton_Click += new RoutedEventHandler(AddServices_OnAddButton_Click);
                                    win.Show();

                                }
                            };
                            client.ProcessAsync(BizActionObj, User);
                            client.CloseAsync();
                            client = null;
                        }
                        else
                        {

                            DefineServicesForPatho win = new DefineServicesForPatho();
                            win.OnAddButton_Click += new RoutedEventHandler(AddServices_OnAddButton_Click);
                            win.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MasterListItem> sMultiService = new List<MasterListItem>();
        private void AddServices_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            DefineServicesForPatho winService = (DefineServicesForPatho)sender;
            if (winService.obj != null)
            {
                ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).List = new List<MasterListItem>();
                ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).List = winService.obj;
                sMultiService = winService.obj;
            }
            //winService.obj
        }

        private void chkIsReflex_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgDefaultValues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem != null)
            {
                if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).IsReflexTesting == true)
                {
                    cmdAddService.IsEnabled = true;
                }
                else
                {
                    cmdAddService.IsEnabled = false;
                }
            }
        }

        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPathoParameterDefaultValueMasterVO objItemVO = new clsPathoParameterDefaultValueMasterVO();
                objItemVO = (clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem;

                //if (objItemVO != null)
                //{
                if (objItemVO.IsReflexTesting == false)
                {
                    msgText = "Please select reflex testing ";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
                else
                {

                    try
                    {
                        if (((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).ID != 0)
                        {
                            if (objItemVO.List != null)
                            {
                                DefineServicesForPatho win = new DefineServicesForPatho();
                                win.SelectedTempList = objItemVO.List;
                                win.OnAddButton_Click += new RoutedEventHandler(AddServices_OnAddButton_Click);
                                win.Show();
                            }
                            else
                            {

                                clsGetPathoServicesByIDBizActionVO BizActionObj = new clsGetPathoServicesByIDBizActionVO();
                                BizActionObj.ID = ((clsPathoParameterDefaultValueMasterVO)dgDefaultValues.SelectedItem).ID;
                                BizActionObj.ParaID = ((clsPathoParameterMasterVO)dgPathoParameterList.SelectedItem).ID;
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                                client.ProcessCompleted += (s, ea) =>
                                {
                                    if (ea.Error == null && ea.Result != null)
                                    {

                                        List<MasterListItem> objServiceVO = new List<MasterListItem>();
                                        objServiceVO = ((clsGetPathoServicesByIDBizActionVO)ea.Result).ServiceDetailsList;  //get selected services

                                        DefineServicesForPatho win = new DefineServicesForPatho();
                                        win.isView = true;
                                        if (objServiceVO != null)
                                        {
                                            win.selectedMasterList = objServiceVO;
                                        }
                                        win.OnAddButton_Click += new RoutedEventHandler(AddServices_OnAddButton_Click);
                                        win.Show();

                                    }
                                };
                                client.ProcessAsync(BizActionObj, User);
                                client.CloseAsync();
                                client = null;
                            }
                        }
                        else
                        {

                            DefineServicesForPatho win = new DefineServicesForPatho();
                            win.SelectedTempList = objItemVO.List;
                            win.OnAddButton_Click += new RoutedEventHandler(AddServices_OnAddButton_Click);
                            win.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            //

            catch (Exception ex)
            {
                throw;
            }

        }

        private void chkIsAbnoramal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtMinValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }
        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;

            }
            //IsValidate = true;

        }
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private void txtCostPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            textBefore = "";
            selectionStart = 0;
            selectionLength = 0;
        }

        private void chkIsFlagReflex_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtAgeFrom_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) < 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbClear_Click(object sender, RoutedEventArgs e)
        {
            txtFormula.Text = "";
            txtFormulaCode.Text = "";   //by rohini
            FormulaID = "";
            IsPara = 0;
            txtFormula.Text = string.Empty;
            txtFormulaCode.Text = string.Empty;  //by rohini
            txtNumericValue.Text = "";
            textBefore = "";
            autoParameter.Text = string.Empty;
            txtNumericValue.Text = string.Empty;
            HandleOperator(false);
              //Expression e = new Expression("Round(Pow([Pi], 2) + Pow([Pi2], 2) + [X], 2)"
       
        }
       

    }
}
