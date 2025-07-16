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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Windows.Controls.Primitives;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;

namespace OPDModule.Forms
{
    public partial class frmCoupleFormation : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
                default:
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    //try
                    //{
                    //    TemplateID = Convert.ToInt64(Mode);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    break;
            }
        }

        #endregion

        #region Paging

        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }

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

        bool IsPatientExist = false;
        bool IsPageLoded = false;
        bool IsFirst = true;
        WaitIndicator Indicatior = null;
        long SelectedPatientGender;
        long SelectedPatHeaderID;
        long SelectedPatID;
        string msgTitle = "";
        string msgText = "";
      
        public frmCoupleFormation()
        {
            InitializeComponent();

            // BY     BHUSAHN.  .   .   .   .   .   
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dataGrid2Pager.PageSize = DataListPageSize;
            dataGrid2Pager.Source = DataList;
        }

        /// <summary>
        /// Used when refresh event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPatientList();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                // BY BHUSHAN . . . . .
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
               
            }
            else if (IsPageLoded == false)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");          
            }
            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                LoadPatientHeader();
                SelectedPatID = 0;
                Tooltip();    // BY BHUSHAN . . . . . .
                optName.IsChecked = true;
            }
            IsPageLoded = true;
        }

        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                    SelectedPatientGender = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GenderID; //BizAction.PatientDetails.GenderID;
                    //if (BizAction.PatientDetails.GenderID == 1)
                    //{//Male
                        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        SelectedPatHeaderID = BizAction.PatientDetails.GeneralDetails.PatientID;
                        FillPatientList();
                    //}
                    //else
                    //{//Spouce
                    //    Patient.DataContext = BizAction.PatientDetails.SpouseDetails;                      
                    //}
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillPatientList()
        {
            clsGetPatientListBizActionVO BizAction = new clsGetPatientListBizActionVO();
           // BizAction.PatientDetails = new clsPatientVO();
            BizAction.GenderID = SelectedPatientGender;

            if (optMRNo.IsChecked == true)
            {
                // BY BHUSHAN . . . . . .
                BizAction.SearchName = true;
                BizAction.Description = txtSearch.Text.Trim();      
            }
            else
            {
                // BY BHUSHAN . . . . . .
                BizAction.SearchName = false;
                BizAction.Description = txtSearch.Text.Trim();
            }
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    DataList.Clear();   // BY BHUSHAN . . . . . .
                    if (((clsGetPatientListBizActionVO)args.Result).PatientDetailsList != null)
                    {
                        clsGetPatientListBizActionVO result = args.Result as clsGetPatientListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.PatientDetailsList != null)
                        {                          
                            foreach (var item in result.PatientDetailsList)
                            {
                                DataList.Add(item);
                            }
                            dgPatientList.ItemsSource = null;
                            dgPatientList.ItemsSource = DataList;

                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = BizAction.MaximumRows;
                            dataGrid2Pager.Source = DataList;
                        }
                    }        
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedPatID > 0)
                {
                    msgText = "Are You Sure \n  You Want To Save the Record?";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);
                    msgWindowUpdate.Show();
                }
                else
                {
                    msgText = "Please Select A New Partner.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgWindow.Show();
                }           
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }           
        }

        private void Save()
        {
            try
            {
               // Indicatior.Show();
                int intResult;
                clsAddNewCoupleBizActionVO BizAction = new clsAddNewCoupleBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.PatientDetails.GenderID = SelectedPatientGender;

                BizAction.PatientDetails.GeneralDetails.PatientID = SelectedPatHeaderID;
                BizAction.PatientDetails.SpouseDetails.PatientID = SelectedPatID;
                BizAction.PatientDetails.GeneralDetails.IsFromNewCouple = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {                        
                        intResult = ((clsAddNewCoupleBizActionVO)args.Result).SuccessStatus;
                        if (intResult == 1)
                        {
                            //Saved
                            msgText = "Record Is Successfully Submitted!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();

                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        }
                        else
                        {
                            //Error occured
                            msgText = "Error Occured.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                //GenderId = 1 Male, GenderId=2 Female               
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
        }

        private void optName_Checked(object sender, RoutedEventArgs e)
        {
           txtSearch.Text = string.Empty;
        }
        
        // BY BHUSHAN . . . . . .
        public void Tooltip()
        {
            ToolTip tt = new ToolTip();

            if (optMRNo.IsChecked == true)
            {
                tt.Template = (ControlTemplate)Resources["ToolTipTemplate"];
                tt.Content = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Red),                   
                    Text = "Only Enter MR No. .",
                };
                ToolTipService.SetToolTip(txtSearch, tt);
            }
            else
            {
                tt.Template = (ControlTemplate)Resources["ToolTipTemplate"];
                tt.Content = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Red),               
                    Text = "Only Enter First Name",
                };
                ToolTipService.SetToolTip(txtSearch, tt);
            }
        }
        
        private void optMRNo_Checked(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
        
            Tooltip();
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            // BY BHUSHAN . . . . . .
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToTitleCase();
            }
        }

        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dgPatientList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void dgPatientList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsPatientGeneralVO SelectedPatient = new clsPatientGeneralVO();
            SelectedPatID = 0;
            if (((clsPatientGeneralVO)dgPatientList.SelectedItem).SelectedStatus==true)
            {
                SelectedPatient = ((clsPatientGeneralVO)dgPatientList.SelectedItem);                
            }    
            DataList = (PagedSortableCollectionView<clsPatientGeneralVO>)dgPatientList.ItemsSource;
            foreach (var item in DataList)
            {
                if (item == SelectedPatient)
                {
                    item.SelectedStatus = true;
                    SelectedPatID = item.PatientID;
                }
                else
                {
                    item.SelectedStatus = false; 
                }
            }
            dgPatientList.ItemsSource = null;
            dgPatientList.ItemsSource = DataList;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillPatientList();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
        }

       
    }
}
