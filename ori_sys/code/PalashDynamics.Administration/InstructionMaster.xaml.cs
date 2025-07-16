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
using System.IO;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class InstructionMaster : UserControl,INotifyPropertyChanged
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
        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                return false;
            }

            else
            {
                txtCode.ClearValidationError();
                txtDescription.ClearValidationError();
                return true;
            }

        }
        
        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        WaitIndicator waiting = new WaitIndicator();
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long InstructionID;

        public PagedSortableCollectionView<clsInstructionMasterVO> MasterList { get; private set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public bool ValidCode = true;
        public bool ValidDescription = true;
        public bool IsNew = false;
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
       
        public InstructionMaster()
        {
            InitializeComponent();
            this.DataContext = new clsInstructionMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(InsturctionMaster_Loaded);
            SetCommandButtonState("Load");
           
            MasterList = new PagedSortableCollectionView<clsInstructionMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dataGrid2Pager.DataContext = MasterList;
            grdInstruction.DataContext = MasterList;
            SetupPage();
        }

        void InsturctionMaster_Loaded(object sender, RoutedEventArgs e)
        {

        }
     
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        
        clsInstructionMasterVO getinstructioninfo;

        /// <summary>
        /// This Function is Called Whenever We Have To Fill The Grid On The Front Panel.
        /// The Master Data From The Respective Table is Retrieved For The List.        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetupPage()
        {
            try
            {
                clsGetInstructionDetailsBizActionVO bizActionVO = new clsGetInstructionDetailsBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.FilterCriteria = Convert.ToInt32(CmbInstructionTypeSearch.SelectedValue);
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getinstructioninfo = new clsInstructionMasterVO();
                bizActionVO.InstructionMasterDetails = new List<clsInstructionMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.InstructionMasterDetails = (((clsGetInstructionDetailsBizActionVO)args.Result).InstructionMasterDetails);
                        if (bizActionVO.InstructionMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetInstructionDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsInstructionMasterVO item in bizActionVO.InstructionMasterDetails)
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
                throw;
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
        /// This Event is Called When We click on New Button, the BackPanel is displayed
        /// The BackPanel is Cleared and Command Buttons State is set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsOTTableVO();
            ClearUI();
            IsNew = true;            
            SetCommandButtonState("New");
            try
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New Instructions Details";
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This Function Is Called Whenever We Have To Clear Back Panel.        
        /// </summary>        
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsInstructionMasterVO();           
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
                if (ValidCode == true && ValidDescription == true )
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Save the Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_Update_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else if (ValidCode == false)
                    txtCode.Focus();
                else if (ValidDescription == false)
                    txtDescription.Focus();
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        /// <summary>
        /// This Function is Called On Click Of Confirmation Message For Save Event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msgWindowSave_Update_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if(CheckDuplicasy())
                    Save();
            }
        }

        /// <summary>
        /// This Function Is Called To Save The Details Of Instruction Master After Confirmation From User.
        /// </summary>
        private void Save()
        {
            try
            {
                
                clsAddUpdateInstructionDetailsBizActionVO objInstructionAdd = new clsAddUpdateInstructionDetailsBizActionVO();
                objInstructionAdd.InstMaster = (clsInstructionMasterVO)grdBackPanel.DataContext;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        
                        SetupPage();
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";
                        SetCommandButtonState("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        string msgText = "Instruction Details Added Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        string msgText = "Record cannot be added Please check the Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objInstructionAdd, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        /// <summary>
        /// This Function Is Called To Set The Validations On The BackPanel Of The Form.
        /// </summary>
        private void SetFormValidation()
        {
            ValidCode = true;
            ValidDescription = true;

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.SetValidation("Code Cannot Be Blank.");
                txtCode.RaiseValidationError();
                ValidCode = false;
            }
            else
                txtCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Description Cannot Be Blank.");
                txtDescription.RaiseValidationError();
                ValidCode = false;
            }
            else
                txtDescription.ClearValidationError();            
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
            waiting.Show();
            try
            {
                SetFormValidation();
                if (ValidCode == true && ValidDescription == true)
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Modify the Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowModify_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else
                {
                    waiting.Close();
                    if (ValidCode == false)
                    txtCode.Focus();
                    else if (ValidDescription == false)
                    txtDescription.Focus();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }
        }

        /// <summary>
        /// This Function is Called On Click Of Confirmation Message For Modify Event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msgWindowModify_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if(CheckDuplicasy())
                    Modify();
            }
            //else
            //{
            //    //if (grdInstruction.SelectedItem != null)
            //    //    ShowEmailTemplateDetails(((clsInstructionMasterVO)grdInstruction.SelectedItem).ID);

            //    SetCommandButtonState("View");
            //    objAnimation.Invoke(RotationType.Forward);
            //}
        }

        /// <summary>
        /// This Function Is Called To Modify The Details Of Instruction Master After Confirmation From User.
        /// </summary>
        private void Modify()
        {
            try
            {
                clsAddUpdateInstructionDetailsBizActionVO objModfyInst = new clsAddUpdateInstructionDetailsBizActionVO();
                clsInstructionMasterVO objInstruction = new clsInstructionMasterVO();
                objInstruction = ((clsInstructionMasterVO)grdBackPanel.DataContext);

                objModfyInst.InstMaster = objInstruction;
                objModfyInst.InstMaster.ID = ((clsInstructionMasterVO)grdInstruction.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        SetupPage();
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";
                        SetCommandButtonState("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        string msgText = "Instruction Details Modify Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        string msgText = "Record cannot be added Please check the Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objModfyInst, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// This Function Is Called To Avoid Duplicate Entry Of Records.
        /// </summary>
        private bool CheckDuplicasy()
        {
            clsInstructionMasterVO Item;
            clsInstructionMasterVO Item1;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsInstructionMasterVO>)grdInstruction.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsInstructionMasterVO>)grdInstruction.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsInstructionMasterVO>)grdInstruction.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsInstructionMasterVO)grdInstruction.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsInstructionMasterVO>)grdInstruction.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.ID != ((clsInstructionMasterVO)grdInstruction.SelectedItem).ID);
            }
            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be saved because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be saved because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
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
            SetupPage();
            objAnimation.Invoke(RotationType.Backward);
            SetCommandButtonState("Cancel");
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
        /// This Event is Called When We Click On A The View Hyperlink On The Front Panel,
        /// To View The Details For That Particular Record On The BackPanel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                if (((clsInstructionMasterVO)grdInstruction.SelectedItem).Status == true)
                {
                    if (grdInstruction.SelectedItem != null)
                    {
                        IsNew = false;
                        ShowEmailTemplateDetails(((clsInstructionMasterVO)grdInstruction.SelectedItem).ID);
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsInstructionMasterVO)grdInstruction.SelectedItem).Code;
                        objAnimation.Invoke(RotationType.Forward);
                    }
                    SetCommandButtonState("View");
                    waiting.Close();
                }
                else
                {
                    waiting.Close();
                    msgText = "Cannot view the Details, The Record is disabled";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }
        }

        /// <summary>
        /// This Function Is Called Retrieve And Display The Details For The Selected Record.
        /// </summary>
        private void ShowEmailTemplateDetails(long InstructionId)
        {
            try
            {
                clsGetInstructionDetailsByIDBizActionVO obj = new clsGetInstructionDetailsByIDBizActionVO();
                obj.ID = InstructionId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsInstructionMasterVO objTemp = new clsInstructionMasterVO();
                        grdBackPanel.DataContext = ((clsGetInstructionDetailsByIDBizActionVO)args.Result).InstructionDetails;
                        if (((clsGetInstructionDetailsByIDBizActionVO)args.Result).InstructionDetails != null)
                        {                            
                        }
                        else
                        {
                            msgText = "An Error Occured while Retrieving the Email Template.";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(obj, new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
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
            SetupPage();
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateInstructionStatusBizActionVO objUpdateStatus = new clsUpdateInstructionStatusBizActionVO();
            objUpdateStatus.InstructionTempStatus = new clsInstructionMasterVO();
            objUpdateStatus.InstructionTempStatus.ID = (((clsInstructionMasterVO)grdInstruction.SelectedItem).ID);
            objUpdateStatus.InstructionTempStatus.Status = (((clsInstructionMasterVO)grdInstruction.SelectedItem).Status);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if ((ea.Error == null) && (ea.Result != null))
                {
                    if (objUpdateStatus.InstructionTempStatus.Status == false)
                    {
                        msgText = "Email Template Deactivated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        msgText = "Email Template Activated Successfully.";
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
            Client.ProcessAsync(objUpdateStatus, User);
            Client.CloseAsync();
        }

       
    }
}
