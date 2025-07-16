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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using CIMS;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class ItemGroupMaster : UserControl
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

        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public PagedSortableCollectionView<clsPatientSourceVO> DataList { get; private set; }
        bool IsCancel = true;

        #endregion
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
                OnPropertyChanged("DataListPageSize");
            }
        }
        public ItemGroupMaster()
        {
            InitializeComponent();
           
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsPatientSourceVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.dataGrid2Pager.DataContext = DataList;
            this.grdMaster.DataContext = DataList;

            FetchData();
           // FillPatientCatagory();
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
            BizAction.IsFromItemGroupMaster = true;
            BizAction.PatientSourceDetails = new List<clsPatientSourceVO>();
            BizAction.SearchExpression = txtSearch.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPatientSourceListBizActionVO)arg.Result).PatientSourceDetails != null)
                    {
                        clsGetPatientSourceListBizActionVO result = arg.Result as clsGetPatientSourceListBizActionVO;

                        if (result.PatientSourceDetails != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetPatientSourceListBizActionVO)arg.Result).TotalRows;
                            foreach (clsPatientSourceVO item in result.PatientSourceDetails)
                            {
                                DataList.Add(item);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsPatientSourceVO();
                SetCommandButtonState("Load");
              
                txtcode.Focus();  
                Indicatior.Close();
                FillGeneralLedger();
               
            }
            txtcode.Focus();
            txtcode.UpdateLayout();
            IsPageLoded = true;
            //CheckValidation();
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
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            this.DataContext = new clsPatientSourceVO();
            ClearControl();

           // CheckValidation();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Item Group Details";
            objAnimation.Invoke(RotationType.Forward);           

        }
        bool SavePatient = false;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            SavePatient = true;
            SavePatient = CheckValidation();
            if (SavePatient == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }


        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SavePatientSource();
        }
        private void SavePatientSource()
        {
            clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
            BizAction.IsFromItemGroupMaster = true;
            BizAction.PatientDetails = (clsPatientSourceVO)this.DataContext;
            if (txtcode.Text != "" && txtcode.Text != null)
            {
                BizAction.PatientDetails.Code = txtcode.Text;
            }
            if (txtDescription.Text != "" && txtDescription.Text != null)
            {
                BizAction.PatientDetails.Description = txtDescription.Text;
            }
            if (((MasterListItem)cmbGeneralLedger.SelectedItem).ID != null)
            {
                BizAction.PatientDetails.PatientCatagoryID = ((MasterListItem)cmbGeneralLedger.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientCatagoryID = 0;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Item Group Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Save");
                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        bool ModifyPatientSource = false;
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            ModifyPatientSource = true;
            ModifyPatientSource = CheckValidation();
            if (ModifyPatientSource == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Record ?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Update The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }
        private void Modify()
        {
            clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
            BizAction.IsFromItemGroupMaster = true;
            BizAction.PatientDetails = (clsPatientSourceVO)this.DataContext;
            BizAction.PatientDetails=new clsPatientSourceVO();
            BizAction.PatientDetails.ID = ((clsPatientSourceVO)grdMaster.SelectedItem).ID;
            if (txtcode.Text != "" && txtcode.Text != null)
            {
                BizAction.PatientDetails.Code = txtcode.Text;
            }
            if (txtDescription.Text != "" && txtDescription.Text != null)
            {
                BizAction.PatientDetails.Description = txtDescription.Text;
            }
            if (((MasterListItem)cmbGeneralLedger.SelectedItem).ID != null)
            {
                BizAction.PatientDetails.PatientCatagoryID = ((MasterListItem)cmbGeneralLedger.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientCatagoryID = 0;
            }

            BizAction.PatientDetails.Status = true;
            //BizAction.PatientDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Modify");
                        objAnimation.Invoke(RotationType.Backward);
                        ModifyPatientSource = false;
                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be Updated because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be Updated because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                       
                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmInventoryConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearControl();
          //  CheckValidation();
            if (((clsPatientSourceVO)grdMaster.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else
                cmdModify.IsEnabled = true;
            grpMasterDetails.DataContext = ((clsPatientSourceVO)grdMaster.SelectedItem).DeepCopy();

          //  CheckValidation();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsPatientSourceVO)grdMaster.SelectedItem).Description;

            txtcode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            cmbGeneralLedger.SelectedValue = (long)0;

            txtcode.Text = ((clsPatientSourceVO)grdMaster.SelectedItem).Code;
            txtDescription.Text = ((clsPatientSourceVO)grdMaster.SelectedItem).Description;
            cmbGeneralLedger.SelectedValue = ((clsPatientSourceVO)grdMaster.SelectedItem).PatientCatagoryID;

            objAnimation.Invoke(RotationType.Forward);


        }
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (grdMaster.SelectedItem != null)
            //{
            //    clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
            //    BizAction.PatientDetails = (clsPatientSourceVO)grdMaster.SelectedItem;
            //    BizAction.PatientDetails.ID = ((clsPatientSourceVO)grdMaster.SelectedItem).ID;
            //    BizAction.PatientDetails.PatientCatagoryID = ((clsPatientSourceVO)grdMaster.SelectedItem).PatientCatagoryID;
            //    BizAction.PatientDetails.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
            //    //BizAction.PatientDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;


            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {

            //        if (arg.Error == null)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW2 =
            //            new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW2.Show();
                    

            //        }
            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();

            //}
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

       
        private void cmbPatientCatagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((MasterListItem)cmbPatientCatagory.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Corporate_PatientCategoryL1Id)
            //{
            //    pnTariffList.Visibility = Visibility.Collapsed;
            //}
            //else
            //    pnTariffList.Visibility = Visibility.Visible;
        }

        
        #region Reset All Control

        private void ClearControl()
        {
            txtcode.Text = "";
            txtDescription.Text = "";
            cmbGeneralLedger.SelectedValue = (long)0;          
        }

        #endregion

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;        
           //rohinee
            if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result = false;
            }
            else
            {
                txtcode.ClearValidationError();

            }
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else
            {
                txtDescription.ClearValidationError();

            }         //

            if ((MasterListItem)cmbGeneralLedger.SelectedItem == null)
                {
                    cmbGeneralLedger.TextBox.SetValidation("Please Select General ledger");
                    cmbGeneralLedger.TextBox.RaiseValidationError();
                    cmbGeneralLedger.Focus();
                    result = false;
                }

                else
                {
                    if (cmbGeneralLedger.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbGeneralLedger.SelectedItem).ID == 0)
                        {
                            cmbGeneralLedger.TextBox.SetValidation("Please Select General ledger");
                            cmbGeneralLedger.TextBox.RaiseValidationError();
                            cmbGeneralLedger.Focus();
                            result = false;
                        }
                    }
                
            }
           

            return result;
        }

        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        #endregion

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void FillGeneralLedger()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GeneralLedgerMaster;
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
                    cmbGeneralLedger.ItemsSource = null;
                    cmbGeneralLedger.ItemsSource = objList;
                    cmbGeneralLedger.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, null);
            Client.CloseAsync();
        }

        
    }
}
