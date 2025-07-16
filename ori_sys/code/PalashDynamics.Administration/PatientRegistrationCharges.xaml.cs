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
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.Administration
{
    public partial class PatientRegistrationCharges : UserControl
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
        public PagedSortableCollectionView<clsRegistrationChargesVO> DataList { get; private set; }
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
        public PatientRegistrationCharges()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsRegistrationChargesVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.dataGrid2Pager.DataContext = DataList;
            this.grdMaster.DataContext = DataList;
            cmbPatientType.IsEnabled = false;
            FetchData();
            FillPatient();
            FillServices();
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        private void FillPatient()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
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
                   // grdMaster.ItemsSource = null;
                    cmbPatientType.ItemsSource = null;
                    cmbPatientType.ItemsSource = objList;
                  //  grdMaster.ItemsSource = objList;
                  //  cmbPatientCatagory.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }
        private void FillServices()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
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

                    cmbServices.ItemsSource = null;
                    cmbServices.ItemsSource = objList;
                    cmbServices.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FetchData()
        {
            clsGetRegistrationChargesListBizActionVO BizAction = new clsGetRegistrationChargesListBizActionVO();
            BizAction.PatientSourceDetails = new List<clsRegistrationChargesVO>();
            // BizAction.SearchExpression = txtSearch.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRegistrationChargesListBizActionVO)arg.Result).PatientSourceDetails.Count > 0)
                    {
                        clsGetRegistrationChargesListBizActionVO result = arg.Result as clsGetRegistrationChargesListBizActionVO;

                        if (result.PatientSourceDetails != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetRegistrationChargesListBizActionVO)arg.Result).TotalRows;
                            foreach (clsRegistrationChargesVO item in result.PatientSourceDetails)
                            {
                                DataList.Add(item);
                            }
                        //    grdMaster.ItemsSource = DataList;
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
        private void PatientRegistrationCharges_Loaded(object sender, RoutedEventArgs e)
        {

            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
              //  this.DataContext = new clsRegistrationChargesVO();


                SetCommandButtonState("Load");

                cmbServices.Focus();
                Indicatior.Close();
            }
            cmbServices.Focus();
           // txtcode.UpdateLayout();
            IsPageLoded = true;
           // CheckValidations();
        }

        //private void cmdNew_Click(object sender, RoutedEventArgs e)
        //{
        //    this.SetCommandButtonState("New");
        //    this.DataContext = new clsPatientSourceVO();
        //    ClearControl();
        //    FillPatient();
        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
        //    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
        //    mElement.Text = " : " + "New Patient Source Details";
        //    objAnimation.Invoke(RotationType.Forward);
        //}
        private void ClearControl()
        {
            txtRate.Text = "";
            cmbServices.SelectedValue = 0;
            cmbPatientType.SelectedValue = 0;

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
                mElement.Text = "Patient Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPatientConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        //private void cmdSave_Click(object sender, RoutedEventArgs e)
        //{
            
        //}

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyPatientSource = true;
            ModifyPatientSource = CheckValidations();
            if (ModifyPatientSource == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Registration Charges?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

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
            clsAddRegistrationChargesBizActionVO BizAction = new clsAddRegistrationChargesBizActionVO();
          //  BizAction.PatientDetails = (clsRegistrationChargesVO)this.DataContext;
            BizAction.PatientDetails = new clsRegistrationChargesVO();
            BizAction.PatientDetails.ID = ((clsRegistrationChargesVO)grdMaster.SelectedItem).ID;


            if (((MasterListItem)cmbPatientType.SelectedItem).ID != null && ((MasterListItem)cmbPatientType.SelectedItem).ID != 0)
            {
                BizAction.PatientDetails.PatientTypeId = ((MasterListItem)cmbPatientType.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientTypeId = 0;
            }
            if (((MasterListItem)cmbServices.SelectedItem).ID != null && ((MasterListItem)cmbServices.SelectedItem).ID != 0)
            {
                BizAction.PatientDetails.PatientServiceId = ((MasterListItem)cmbServices.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientServiceId = 0;
            }
            if (txtRate.Text != null && txtRate.Text != "")
            {

                BizAction.PatientDetails.Rate = Convert.ToDouble(txtRate.Text);
            }
            BizAction.PatientDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddRegistrationChargesBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Registration Charges Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Load");
                        objAnimation.Invoke(RotationType.Backward);
                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddRegistrationChargesBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be Updated Record already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        //private void cmdSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    FetchData();
        //}

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearControl();

            if (((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientCategoryID != null && ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientCategoryID != 0)
            {
                //cmbPatientCatagory.SelectedItem = ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientName;
                cmbPatientType.SelectedItem = new MasterListItem(((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientCategoryID, ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientName);

            }
            else
            {
                cmbPatientType.SelectedValue = 0;
            }
            if (((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceId != null && ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceId != 0)
            {
               // cmbServices.SelectedValue = ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceName;
                cmbServices.SelectedItem = new MasterListItem(((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceId, ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceName);
            }
            else
            {
            //    cmbServices.SelectedValue =0;
                 cmbServices.SelectedItem = new MasterListItem(0, "- Select -");

            }
            if (((clsRegistrationChargesVO)grdMaster.SelectedItem).ID != null && ((clsRegistrationChargesVO)grdMaster.SelectedItem).ID != 0)
            {
               
                FillData(((clsRegistrationChargesVO)grdMaster.SelectedItem).ID);
            }
            else
            {
                SetCommandButtonState("View");           
            }

            //rohini 13/10/2015
            if (((clsRegistrationChargesVO)grdMaster.SelectedItem).Status == true)
            {
                cmdModify.IsEnabled = true;
            }
            else
            {
                cmdModify.IsEnabled = false;
            }
            //
          //  CheckValidations();
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = " : " + ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientName;
            objAnimation.Invoke(RotationType.Forward);

        }
        private void FillData(long iID)
        {
            clsGetRegistrationChargesDetailsByIDBizActionVO BizAction = new clsGetRegistrationChargesDetailsByIDBizActionVO();
            BizAction.Details = new clsRegistrationChargesVO();
            BizAction.ID = iID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (grdMaster.SelectedItem != null)
                    {
                        clsGetRegistrationChargesDetailsByIDBizActionVO Obj = new clsGetRegistrationChargesDetailsByIDBizActionVO();
                        Obj = (clsGetRegistrationChargesDetailsByIDBizActionVO)arg.Result;

                        cmbPatientType.SelectedItem = new MasterListItem(((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientCategoryID, ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientName);
                        cmbServices.SelectedItem = new MasterListItem(((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceId, ((clsRegistrationChargesVO)grdMaster.SelectedItem).PatientServiceName);
                      //  cmbPatientCatagory.SelectedValue = Obj.Details.PatientId;
                      //  cmbServices.SelectedValue = Obj.Details.PatientServiceId;
                        if (txtRate.Text != "" && txtRate.Text!= null)
                        {
                            txtRate.Text = Convert.ToString(Obj.Details.Rate);
                        }
                        //SetCommandButtonState("Modify");      
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
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                clsAddRegistrationChargesBizActionVO BizAction = new clsAddRegistrationChargesBizActionVO();
                BizAction.PatientDetails = (clsRegistrationChargesVO)grdMaster.SelectedItem;
                BizAction.PatientDetails.ID = ((clsRegistrationChargesVO)grdMaster.SelectedItem).ID;
                BizAction.PatientDetails.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
              
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();


                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
        }

        private void cmbPatientCatagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //private bool CheckValidation()
        //{
        //    bool result = true;
          
          

        //    return result;
        //}
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                   cmdSave.IsEnabled = true;
                    //cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    //cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Modify":
                    //cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    //cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                  //  IsCancel = false;
                    break;
                   
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    //cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SavePatient = true;
            SavePatient = CheckValidations();
            if (SavePatient == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save the Registration Charges?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SavePatientType();
        }
        private void SavePatientType()
        {
            clsAddRegistrationChargesBizActionVO BizAction = new clsAddRegistrationChargesBizActionVO();
            BizAction.PatientDetails = new clsRegistrationChargesVO();
            //BizAction.PatientDetails = (clsRegistrationChargesVO)this.DataContext;
            if (((MasterListItem)cmbPatientType.SelectedItem).ID != null && ((MasterListItem)cmbPatientType.SelectedItem).ID != 0)
            {
                BizAction.PatientDetails.PatientTypeId = ((MasterListItem)cmbPatientType.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientTypeId = 0;
            }
            if (((MasterListItem)cmbServices.SelectedItem).ID != null && ((MasterListItem)cmbServices.SelectedItem).ID != 0)
            {
                BizAction.PatientDetails.PatientServiceId = ((MasterListItem)cmbServices.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.PatientServiceId = 0;
            }
            if (txtRate.Text != null && txtRate.Text != "")
            {

                BizAction.PatientDetails.Rate =Convert.ToDouble(txtRate.Text);
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    if (((clsAddRegistrationChargesBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", " Registration Charges Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Save");

                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddRegistrationChargesBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because data already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            bool isNumPadNumeric = (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);
            bool isNumeric = ((e.Key >= Key.D0 && e.Key <= Key.D9));
            bool isDecimal = ((e.Key == Key.Decimal) && (((TextBox)sender).Text.IndexOf('.') < 0));
            e.Handled = !(isNumPadNumeric || isNumeric || isDecimal);
        }
        private bool CheckValidations()
        {
            bool ErrorStatus = true;
            if ((MasterListItem)cmbServices.SelectedItem == null)
            {
                cmbServices.TextBox.SetValidation("Please Select Service");
                cmbServices.TextBox.RaiseValidationError();
                cmbServices.Focus();
                ErrorStatus = false;
            }

            else
            {
                if (cmbServices.SelectedItem != null)
                {
                    if (((MasterListItem)cmbServices.SelectedItem).ID == 0)
                    {
                        cmbServices.TextBox.SetValidation("Please Select Service");
                        cmbServices.TextBox.RaiseValidationError();
                        cmbServices.Focus();
                        ErrorStatus = false;
                    }
                }
            }

            
            //if (string.IsNullOrEmpty(txtRate.Text))
            //{
            //    txtRate.SetValidation("Please Enter Rate");
            //    txtRate.RaiseValidationError();
            //    txtRate.Focus();
            //    ErrorStatus = false;
            //}

            
            //if (ErrorStatus == true)
            //{
            //    cmbServices.TextBox.ClearValidationError();             
              
            //    txtRate.ClearValidationError();
                
            //    ErrorStatus = true;
            //}

            return ErrorStatus;
        }
    }
}
