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
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace OPDModule.Forms
{
    public partial class PatientSearch : ChildWindow, IInitiateCIMS
    {

        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        public bool VisitWise = false;
        public bool isfromCouterSale = false;
        public bool IsClinicVisible = false;
        List<MasterListItem> RegTypeList = new List<MasterListItem>();
        public bool isIPD = false;  //Added by AJ date 14/11/2016

        public void Initiate(string Mode)     //Added by AJ date 14/11/2016
        {
            if (Mode == "ISIPD")
            {
                isIPD = true;
                VisitWise = true;
            }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion

        public PatientSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
            this.DataContext = new clsPatientGeneralVO();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            FillUnitList();
            if (IsClinicVisible)
            {
                cmbClinic.IsEnabled = true;
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            }
            else
            {
                cmbClinic.IsEnabled = false;
            }
            isLoaded = true;
                RegTypeList.Add(new MasterListItem(10,"-Select-"));
                RegTypeList.Add(new MasterListItem(0, "OPD"));
                RegTypeList.Add(new MasterListItem(1, "IPD"));
                RegTypeList.Add(new MasterListItem(2, "Pharmacy"));
                RegTypeList.Add(new MasterListItem(5, "Pathology"));            
            txtFirstName.Focus();
            //GetData();
            cmbRegFrom.ItemsSource = RegTypeList;
            cmbRegFrom.SelectedItem = RegTypeList[0];
            FillUnitList();
			
			//Added by AJ date 14/11/2016
            if (isIPD)
            {
                cmbRegFrom.ItemsSource = RegTypeList;
                cmbRegFrom.SelectedItem = RegTypeList[1];
                cmbRegFrom.IsEnabled = false;
            }
            //GetData();  commented on dated 22042017


        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;

        }

        #region Validation
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        string textBefore = null;

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;


                }
            }
        }

        #endregion

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                try
                {
                    GetData();
                    dataGrid2.SelectedIndex = 0;
                   // peopleDataPager.PageIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error While Processing...", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }
        }

        public void GetData()
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            try
            {
                clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
                BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

                if (isfromCouterSale == true)
                {
                    BizActionObject.RegistrationWise = true;
                    BizActionObject.ISDonorSerch = true;
                }
                else
                {
                    BizActionObject.VisitWise = VisitWise;
                    if (VisitWise != true && isIPD != true)
                    {
                        BizActionObject.VisitFromDate = DateTime.Now.Date;
                        BizActionObject.VisitToDate = DateTime.Now.Date;
                    }
                }

                if (txtFirstName.Text != "")
                    BizActionObject.FirstName = txtFirstName.Text;
                if (txtMiddleName.Text != "")
                    BizActionObject.MiddleName = txtMiddleName.Text;
                if (txtLastName.Text != "")
                    BizActionObject.LastName = txtLastName.Text;
                if (txtFamilyName.Text != "")
                    BizActionObject.FamilyName = txtFamilyName.Text;

                if (txtMrno.Text != "")
                    BizActionObject.MRNo = txtMrno.Text;

                if (txtOPDNo.Text != "")
                    BizActionObject.OPDNo = txtOPDNo.Text;

                if (txtContactNo.Text != "")
                    BizActionObject.ContactNo = txtContactNo.Text;

                if (txtCivilID.Text != "")
                    BizActionObject.CivilID = txtCivilID.Text;
                if (((MasterListItem)cmbRegFrom.SelectedItem) != null && ((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
                {
                    // if (((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
                    BizActionObject.RegistrationTypeID = ((MasterListItem)cmbRegFrom.SelectedItem).ID;
                }
                else
                {
                    BizActionObject.RegistrationTypeID = 10;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizActionObject.UnitID = 0;
                }
                else if ((MasterListItem)cmbClinic.SelectedItem != null)
                {
                    BizActionObject.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                else
                {
                    BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizActionObject.IsPagingEnabled = true;
                BizActionObject.MaximumRows = DataList.PageSize; ;
                BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;
                //  BizActionObject.IsPatientSearchWindow = true; //***// Ajit Date 21/10/2016

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    indicator.Close();
                    if (ea.Error == null)
                    {
                        if (ea.Result != null)
                        {
                            
                            clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;
                            DataList.Clear();
                            foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                            {

                                DataList.Add(person);
                            }

                            dataGrid2.ItemsSource = null;
                            dataGrid2.ItemsSource = DataList;

                            peopleDataPager.Source = null;
                            peopleDataPager.PageSize = BizActionObject.MaximumRows;
                            peopleDataPager.Source = DataList;
                        }
                        //  dataGrid2.SelectedIndex = 0;
                    }

                };
                client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
            }
            finally
            {
                
            }
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
                    OnSaveButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter )
            {
               GetData();
            //   dataGrid2.SelectedIndex = 0;
               peopleDataPager.PageIndex = 0;
            }

        }

        private void txtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter )
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }

        private void txtContactNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ) 
            {
                 GetData();
                 peopleDataPager.PageIndex = 0;
            }
        }
        //Added by Ajit Jadhav Date 25/10/2016
       
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtContactNoTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        private void txtContactNoTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {            
          //  if (IsPageLoded)
            //{
            if (!string.IsNullOrEmpty(((TextBox)sender).Text.Trim()))
                {
                    if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((TextBox)sender).Text.Length > 10)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            //}
           
        }
        //***//--------------------------


        private void cmbLoyaltyProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData();
        }

        private void FillUnitList()
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsUserWise = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID != null)
                {
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsUserRightsVO objUser;
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                        if (objUser.IsCrossAppointment)
                        {
                            cmbClinic.IsEnabled = true;
                        }
                        else
                        {
                            cmbClinic.IsEnabled = false;
                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

