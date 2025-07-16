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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.Agency;



namespace PalashDynamics.Administration
{
    public partial class AgentMaster : UserControl
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
        public PagedSortableCollectionView<AgentVO> DataList { get; private set; }
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

        public AgentMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<AgentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.dataGrid2Pager.DataContext = DataList;
            this.grdMaster.DataContext = DataList;
            FillOccupation();



        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetAgencyMasterListBizActionVO BizAction = new clsGetAgencyMasterListBizActionVO();
            BizAction.IsAgent = true;
            BizAction.AgentDetails = new AgentVO();
            BizAction.AgentDetailsList = new List<AgentVO>();
            BizAction.AgentYearList = new List<YearClinic>();
            BizAction.AgentYearListSurrogacy = new List<YearClinic>();
            BizAction.PagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetAgencyMasterListBizActionVO)arg.Result).AgentDetailsList.Count > 0)
                    {
                        clsGetAgencyMasterListBizActionVO result = arg.Result as clsGetAgencyMasterListBizActionVO;

                        if (result.AgentDetailsList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetAgencyMasterListBizActionVO)arg.Result).TotalRows;
                            foreach (AgentVO item in result.AgentDetailsList)
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

                txtName.Focus();
                Indicatior.Close();
            }
            txtName.Focus();
            // txtcode.UpdateLayout();
            IsPageLoded = true;
            // CheckValidations();
        }

        private void ClearControl()
        {
            //grdDonationMaster.DataContext = null;
            txtName.Text = "";
            dtpDOB.SelectedDate = null;
            dtpDOB.DisplayDate = DateTime.Now;
            cmbOccupation.SelectedValue = 0;
            txtMarried.Text = "";
            txtSpouseName.Text = "";
            dtpSpouseDOB.SelectedDate = null;
            dtpSpouseDOB.DisplayDate = DateTime.Now;
            txtNODonation.Text = "";
            grdDonationMaster.ItemsSource = null;
            txtNOSurrogacy.Text = "";
            grdSurrogacyMaster.ItemsSource = null;
            txtMobileCountryCode.Text = "";
            txtContactNo1.Text = "";
            txtAltMobileCountryCode.Text = "";
            txtAltContactNo1.Text = "";
            txtLandlineCountryCode.Text = "";
            txtLandlineContactNo1.Text = "";
            txtAddressLine1.Text = "";
            txtStreet.Text = "";
            txtCountry.SelectedValue = 0;
            txtCity.SelectedValue = 0;
            txtPincode.Text = "";
            txtAddressLine2.Text = "";
            txtLandmark.Text = "";
            txtState.SelectedValue = 0;
            txtArea.Text = "";
            txtPanNO.Text = "";
            txtAadharNo.Text = "";
            chkIfYesrdo.IsChecked = true;
            chkPreviousDoneIfYesrdo.IsChecked = true;
            chkPreviousSurrogacyDoneIfYesrdo.IsChecked = true;
            txtMarried.IsEnabled = true;
            txtNODonation.IsEnabled = true;
            txtNOSurrogacy.IsEnabled = true;

            if (cmbOccupation.ItemsSource != null)
            {
                List<MasterListItem> objOccupationlist = new List<MasterListItem>();

                objOccupationlist = ((List<MasterListItem>)cmbOccupation.ItemsSource).ToList();

                MasterListItem objOccupation = objOccupationlist.Where(x => x.ID == 0).FirstOrDefault();

                cmbOccupation.SelectedItem = objOccupation;
            }
           
            if (txtCountry.ItemsSource != null)
            {
                List<MasterListItem> objCountrylist = new List<MasterListItem>();

                objCountrylist = ((List<MasterListItem>)txtCountry.ItemsSource).ToList();

                MasterListItem objCountry = objCountrylist.Where(x => x.ID == 0).FirstOrDefault();

                txtCountry.SelectedItem = objCountry;
            }

            //FillOccupation();
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

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyPatientSource = true;
            ModifyPatientSource = CheckValidations();
            if (ModifyPatientSource == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the agent master?";

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
            clsAddAgencyMasterBizActionVO BizAction = new clsAddAgencyMasterBizActionVO();
            BizAction.IsAgent = true;
            BizAction.AgentDetails = new AgentVO();
            BizAction.AgentYearList = new List<YearClinic>();

            BizAction.AgentDetails.ID = ((AgentVO)grdMaster.SelectedItem).ID;
            if (txtName.Text != null && txtName.Text != "")
                BizAction.AgentDetails.Name = txtName.Text.ToString();
            if (dtpDOB.SelectedDate != null)
                BizAction.AgentDetails.DOB = dtpDOB.SelectedDate.Value.Date;
            if (((MasterListItem)cmbOccupation.SelectedItem) != null && ((MasterListItem)cmbOccupation.SelectedItem).ID != 0)
                BizAction.AgentDetails.OccupationID = Convert.ToInt32(((MasterListItem)cmbOccupation.SelectedItem).ID);
            if (chkIfYesrdo.IsChecked != null && chkIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.IsMarried = true;
            if (chkIfNordo.IsChecked != null && chkIfNordo.IsChecked == true)
                BizAction.AgentDetails.IsMarried = false;
            if (txtMarried.Text != null && txtMarried.Text != "")
                BizAction.AgentDetails.YearsOfMerrage = Convert.ToInt32(txtMarried.Text);
            if (txtSpouseName.Text != null && txtSpouseName.Text != "")
                BizAction.AgentDetails.SpouseName = txtSpouseName.Text.ToString();
            if (dtpSpouseDOB.SelectedDate != null)
                BizAction.AgentDetails.SpouseDOB = dtpSpouseDOB.SelectedDate.Value.Date;
            if (chkPreviousDoneIfYesrdo.IsChecked != null && chkPreviousDoneIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.PrevioulyEggDonation = true;
            if (chkPreviousDoneIfNordo.IsChecked != null && chkPreviousDoneIfNordo.IsChecked == true)
                BizAction.AgentDetails.PrevioulyEggDonation = false;
            if (txtNODonation.Text != null && txtNODonation.Text != "")
                BizAction.AgentDetails.NoofDonationTime = Convert.ToInt32(txtNODonation.Text);
            if (chkPreviousSurrogacyDoneIfYesrdo.IsChecked != null && chkPreviousSurrogacyDoneIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.PreviousSurogacyDone = true;
            if (chkPreviousSurrogacyDoneIfNordo.IsChecked != null && chkPreviousSurrogacyDoneIfNordo.IsChecked == true)
                BizAction.AgentDetails.PreviousSurogacyDone = false;
            if (txtNOSurrogacy.Text != null && txtNOSurrogacy.Text != "")
                BizAction.AgentDetails.NoofSurogacyDone = Convert.ToInt32(txtNOSurrogacy.Text);
            if (txtMobileCountryCode.Text != null && txtMobileCountryCode.Text != "")
                BizAction.AgentDetails.MobCountryCode = txtMobileCountryCode.Text;
            if (txtContactNo1.Text != null && txtContactNo1.Text != "")
                BizAction.AgentDetails.MobileNo = txtContactNo1.Text;
            if (txtAltMobileCountryCode.Text != null && txtAltMobileCountryCode.Text != "")
                BizAction.AgentDetails.AltMobCountryCode = txtAltMobileCountryCode.Text;
            if (txtAltContactNo1.Text != null && txtAltContactNo1.Text != "")
                BizAction.AgentDetails.AltMobileNo = txtAltContactNo1.Text;
            if (txtLandlineCountryCode.Text != null && txtLandlineCountryCode.Text != "")
                BizAction.AgentDetails.LLAreaCode = txtLandlineCountryCode.Text;
            if (txtLandlineContactNo1.Text != null && txtLandlineContactNo1.Text != "")
                BizAction.AgentDetails.LandlineNo = txtLandlineContactNo1.Text;
            if (txtAddressLine1.Text != null && txtAddressLine1.Text != "")
                BizAction.AgentDetails.AddressLine1 = txtAddressLine1.Text;
            if (txtAddressLine2.Text != null && txtAddressLine2.Text != "")
                BizAction.AgentDetails.AddressLine2 = txtAddressLine2.Text;
            if (txtStreet.Text != null && txtStreet.Text != "")
                BizAction.AgentDetails.Street = txtStreet.Text;
            if (txtLandmark.Text != null && txtLandmark.Text != "")
                BizAction.AgentDetails.LandMark = txtLandmark.Text;
            if (((MasterListItem)txtCountry.SelectedItem) != null && ((MasterListItem)txtCountry.SelectedItem).ID != 0)
                BizAction.AgentDetails.CountryID = Convert.ToInt32(((MasterListItem)txtCountry.SelectedItem).ID);
            if (((MasterListItem)txtState.SelectedItem) != null && ((MasterListItem)txtState.SelectedItem).ID != 0)
                BizAction.AgentDetails.StateID = Convert.ToInt32(((MasterListItem)txtState.SelectedItem).ID);
            if (((MasterListItem)txtCity.SelectedItem) != null && ((MasterListItem)txtCity.SelectedItem).ID != 0)
                BizAction.AgentDetails.CityID = Convert.ToInt32(((MasterListItem)txtCity.SelectedItem).ID);
            if (txtArea.Text != null && txtArea.Text != "")
                BizAction.AgentDetails.Area = txtArea.Text;
            if (txtPincode.Text != null && txtPincode.Text != "")
                BizAction.AgentDetails.Pincode = txtPincode.Text;
            if (txtPanNO.Text != null && txtPanNO.Text != "")
                BizAction.AgentDetails.PanNo = txtPanNO.Text;
            if (txtAadharNo.Text != null && txtAadharNo.Text != "")
                BizAction.AgentDetails.AadharNo = txtAadharNo.Text;

            BizAction.AgentYearList = DonationListData;
            BizAction.AgentYearListSurrogacy = SurrogacyListData;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddAgencyMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Data Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Load");
                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);
                        ClearControl();
                    }
                    else if (((clsAddAgencyMasterBizActionVO)arg.Result).SuccessStatus == 1)
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

        bool isView = false;
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            isView = true;
            SetCommandButtonState("View");
            ClearControl();

            if (((AgentVO)grdMaster.SelectedItem).ID != null && ((AgentVO)grdMaster.SelectedItem).ID != 0)
            {

                FillData(((AgentVO)grdMaster.SelectedItem).ID);
            }
            else
            {
                SetCommandButtonState("View");
            }


            if (((AgentVO)grdMaster.SelectedItem).Status == true)
            {
                cmdModify.IsEnabled = true;
            }
            else
            {
                cmdModify.IsEnabled = false;
            }

            objAnimation.Invoke(RotationType.Forward);
        }


        bool blNo = false;

        private void FillData(long iID)
        {
            clsGetAgencyMasterListBizActionVO BizAction = new clsGetAgencyMasterListBizActionVO();
            BizAction.GetAgentByID = true;
            BizAction.AgentDetails = new AgentVO();
            BizAction.AgentDetails.ID = Convert.ToInt32(iID);


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (grdMaster.SelectedItem != null)
                    {
                        clsGetAgencyMasterListBizActionVO Obj = (clsGetAgencyMasterListBizActionVO)arg.Result;
                        grpMasterDetails.DataContext = Obj.AgentDetails;

                        DonationListData = new List<YearClinic>();
                        SurrogacyListData = new List<YearClinic>();
                        if (Obj.AgentDetails.NoofDonationTime > 0)
                        {
                            blNo = true;
                            txtNODonation.Text = Obj.AgentDetails.NoofDonationTime.ToString();
                        }

                        if (Obj.AgentDetails.NoofSurogacyDone > 0)
                        {
                            blNo = true;
                            txtNOSurrogacy.Text = Obj.AgentDetails.NoofSurogacyDone.ToString();
                        }

                        if (Obj.AgentDetails.DOB != null && Obj.AgentDetails.DOB != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtpDOB.SelectedDate = Obj.AgentDetails.DOB;
                        }
                        else
                        {
                            Obj.AgentDetails.DOB = null;
                            dtpDOB.SelectedDate = null;
                        }

                        if (Obj.AgentDetails.SpouseDOB != null && Obj.AgentDetails.SpouseDOB != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtpSpouseDOB.SelectedDate = Obj.AgentDetails.SpouseDOB;
                        }
                        else
                        {
                            Obj.AgentDetails.SpouseDOB = null;
                            dtpSpouseDOB.SelectedDate = null;
                        }

                        if (Obj.AgentDetails.OccupationID > 0)
                            cmbOccupation.SelectedValue = Obj.AgentDetails.OccupationID;


                        if (cmbOccupation.ItemsSource != null)
                        {
                            List<MasterListItem> objOccupationlist = new List<MasterListItem>();

                            objOccupationlist = ((List<MasterListItem>)cmbOccupation.ItemsSource).ToList();

                            MasterListItem objOccupation = objOccupationlist.Where(x => x.ID == Obj.AgentDetails.OccupationID).FirstOrDefault();

                            cmbOccupation.SelectedItem = objOccupation;
                        }

                        if (Obj.AgentDetails.CountryID > 0)
                            txtCountry.SelectedValue = Obj.AgentDetails.CountryID;

                        if (txtCountry.ItemsSource != null)
                        {
                            List<MasterListItem> objCountrylist = new List<MasterListItem>();

                            objCountrylist = ((List<MasterListItem>)txtCountry.ItemsSource).ToList();

                            MasterListItem objCountry = objCountrylist.Where(x => x.ID == Obj.AgentDetails.CountryID).FirstOrDefault();

                            txtCountry.SelectedItem = objCountry;
                        }



                        if (Obj.AgentDetails.StateID > 0)
                            txtState.SelectedValue = Obj.AgentDetails.StateID;

                        if (Obj.AgentDetails.CityID > 0)
                            txtCity.SelectedValue = Obj.AgentDetails.CityID;

                        txtMobileCountryCode.Text = Obj.AgentDetails.MobCountryCode;
                        txtContactNo1.Text = Obj.AgentDetails.MobileNo;
                        txtAltMobileCountryCode.Text = Obj.AgentDetails.AltMobCountryCode;
                        txtAltContactNo1.Text = Obj.AgentDetails.AltMobileNo;
                        txtLandlineCountryCode.Text = Obj.AgentDetails.LLAreaCode;
                        txtLandlineContactNo1.Text = Obj.AgentDetails.LandlineNo;

                        if (Obj.AgentDetails.IsMarried == true)
                            chkIfYesrdo.IsChecked = true;
                        else
                            chkIfNordo.IsChecked = true;

                        if (Obj.AgentDetails.PrevioulyEggDonation == true)
                            chkPreviousDoneIfYesrdo.IsChecked = true;
                        else
                            chkPreviousDoneIfNordo.IsChecked = true;

                        if (Obj.AgentDetails.PreviousSurogacyDone == true)
                            chkPreviousSurrogacyDoneIfYesrdo.IsChecked = true;
                        else
                            chkPreviousSurrogacyDoneIfNordo.IsChecked = true;



                        if (Obj.AgentYearList.Count > 0)
                        {

                            for (int i = 0; i < Obj.AgentYearList.Count; i++)
                            {
                                DonationListData.Add(Obj.AgentYearList[i]);
                            }
                            grdDonationMaster.ItemsSource = DonationListData;
                            grdDonationMaster.UpdateLayout();
                            blNo = false;
                        }

                        if (Obj.AgentYearListSurrogacy.Count > 0)
                        {
                            for (int i = 0; i < Obj.AgentYearListSurrogacy.Count; i++)
                            {
                                SurrogacyListData.Add(Obj.AgentYearListSurrogacy[i]);
                            }
                            grdSurrogacyMaster.ItemsSource = SurrogacyListData;
                            grdSurrogacyMaster.UpdateLayout();
                            blNo = false;
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


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                clsAddAgencyMasterBizActionVO BizAction = new clsAddAgencyMasterBizActionVO();
                BizAction.CheckStatus = true;
                BizAction.AgentDetails = (AgentVO)grdMaster.SelectedItem;
                BizAction.AgentDetails.ID = ((AgentVO)grdMaster.SelectedItem).ID;
                BizAction.AgentDetails.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null)
                    {
                        clsAddAgencyMasterBizActionVO Obj = (clsAddAgencyMasterBizActionVO)arg.Result;
                        if (Obj.SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW2.Show();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
        }

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
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //  IsCancel = false;
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

        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsValidCountryCode() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 4)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void txtContactNo1_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsMobileNumberValid() && textBefore != null)
                    {
                        //if (!((WaterMarkTextbox)sender).Text.IsItNumber())
                        //{
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                        // }
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void txtPanNo_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtPanNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length > 10)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SavePatient = true;
            SavePatient = CheckValidations();
            if (SavePatient == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save the Agent Master Details?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void Save()
        {
            clsAddAgencyMasterBizActionVO BizAction = new clsAddAgencyMasterBizActionVO();
            BizAction.IsAgent = true;
            BizAction.AgentDetails = new AgentVO();
            BizAction.AgentYearList = new List<YearClinic>();

            if (txtName.Text != null && txtName.Text != "")
                BizAction.AgentDetails.Name = txtName.Text.ToString();
            if (dtpDOB.SelectedDate != null && dtpDOB.SelectedDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                BizAction.AgentDetails.DOB = dtpDOB.SelectedDate.Value.Date;
            if (((MasterListItem)cmbOccupation.SelectedItem) != null && ((MasterListItem)cmbOccupation.SelectedItem).ID != 0)
                BizAction.AgentDetails.OccupationID = Convert.ToInt32(((MasterListItem)cmbOccupation.SelectedItem).ID);
            if (chkIfYesrdo.IsChecked != null && chkIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.IsMarried = true;
            if (chkIfNordo.IsChecked != null && chkIfNordo.IsChecked == true)
                BizAction.AgentDetails.IsMarried = false;
            if (txtMarried.Text != null && txtMarried.Text != "")
                BizAction.AgentDetails.YearsOfMerrage = Convert.ToInt32(txtMarried.Text);
            if (txtSpouseName.Text != null && txtSpouseName.Text != "")
                BizAction.AgentDetails.SpouseName = txtSpouseName.Text.ToString();
            if (dtpSpouseDOB.SelectedDate != null && dtpSpouseDOB.SelectedDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                BizAction.AgentDetails.SpouseDOB = dtpSpouseDOB.SelectedDate.Value.Date;
            if (chkPreviousDoneIfYesrdo.IsChecked != null && chkPreviousDoneIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.PrevioulyEggDonation = true;
            if (chkPreviousDoneIfNordo.IsChecked != null && chkPreviousDoneIfNordo.IsChecked == true)
                BizAction.AgentDetails.PrevioulyEggDonation = false;
            if (txtNODonation.Text != null && txtNODonation.Text != "")
                BizAction.AgentDetails.NoofDonationTime = Convert.ToInt32(txtNODonation.Text);
            if (chkPreviousSurrogacyDoneIfYesrdo.IsChecked != null && chkPreviousSurrogacyDoneIfYesrdo.IsChecked == true)
                BizAction.AgentDetails.PreviousSurogacyDone = true;
            if (chkPreviousSurrogacyDoneIfNordo.IsChecked != null && chkPreviousSurrogacyDoneIfNordo.IsChecked == true)
                BizAction.AgentDetails.PreviousSurogacyDone = false;
            if (txtNOSurrogacy.Text != null && txtNOSurrogacy.Text != "")
                BizAction.AgentDetails.NoofSurogacyDone = Convert.ToInt32(txtNOSurrogacy.Text);
            if (txtMobileCountryCode.Text != null && txtMobileCountryCode.Text != "")
                BizAction.AgentDetails.MobCountryCode = txtMobileCountryCode.Text;
            if (txtContactNo1.Text != null && txtContactNo1.Text != "")
                BizAction.AgentDetails.MobileNo = txtContactNo1.Text;
            if (txtAltMobileCountryCode.Text != null && txtAltMobileCountryCode.Text != "")
                BizAction.AgentDetails.AltMobCountryCode = txtAltMobileCountryCode.Text;
            if (txtAltContactNo1.Text != null && txtAltContactNo1.Text != "")
                BizAction.AgentDetails.AltMobileNo = txtAltContactNo1.Text;
            if (txtLandlineCountryCode.Text != null && txtLandlineCountryCode.Text != "")
                BizAction.AgentDetails.LLAreaCode = txtLandlineCountryCode.Text;
            if (txtLandlineContactNo1.Text != null && txtLandlineContactNo1.Text != "")
                BizAction.AgentDetails.LandlineNo = txtLandlineContactNo1.Text;
            if (txtAddressLine1.Text != null && txtAddressLine1.Text != "")
                BizAction.AgentDetails.AddressLine1 = txtAddressLine1.Text;
            if (txtAddressLine2.Text != null && txtAddressLine2.Text != "")
                BizAction.AgentDetails.AddressLine2 = txtAddressLine2.Text;
            if (txtStreet.Text != null && txtStreet.Text != "")
                BizAction.AgentDetails.Street = txtStreet.Text;
            if (txtLandmark.Text != null && txtLandmark.Text != "")
                BizAction.AgentDetails.LandMark = txtLandmark.Text;
            if (((MasterListItem)txtCountry.SelectedItem) != null && ((MasterListItem)txtCountry.SelectedItem).ID != 0)
                BizAction.AgentDetails.CountryID = Convert.ToInt32(((MasterListItem)txtCountry.SelectedItem).ID);
            if (((MasterListItem)txtState.SelectedItem) != null && ((MasterListItem)txtState.SelectedItem).ID != 0)
                BizAction.AgentDetails.StateID = Convert.ToInt32(((MasterListItem)txtState.SelectedItem).ID);
            if (((MasterListItem)txtCity.SelectedItem) != null && ((MasterListItem)txtCity.SelectedItem).ID != 0)
                BizAction.AgentDetails.CityID = Convert.ToInt32(((MasterListItem)txtCity.SelectedItem).ID);
            if (txtArea.Text != null && txtArea.Text != "")
                BizAction.AgentDetails.Area = txtArea.Text;
            if (txtPincode.Text != null && txtPincode.Text != "")
                BizAction.AgentDetails.Pincode = txtPincode.Text;
            if (txtPanNO.Text != null && txtPanNO.Text != "")
                BizAction.AgentDetails.PanNo = txtPanNO.Text;
            if (txtAadharNo.Text != null && txtAadharNo.Text != "")
                BizAction.AgentDetails.AadharNo = txtAadharNo.Text;

            BizAction.AgentYearList = DonationListData;
            BizAction.AgentYearListSurrogacy = SurrogacyListData;




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddAgencyMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Data Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Load");
                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);
                        ClearControl();
                    }
                    else if (((clsAddAgencyMasterBizActionVO)arg.Result).SuccessStatus == 1)
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

        private bool CheckValidations()
        {
            bool ErrorStatus = true;
            //if ((MasterListItem)cmbServices.SelectedItem == null)
            //{
            //    cmbServices.TextBox.SetValidation("Please Select Service");
            //    cmbServices.TextBox.RaiseValidationError();
            //    cmbServices.Focus();
            //    ErrorStatus = false;
            //}

            //else
            //{
            //    if (cmbServices.SelectedItem != null)
            //    {
            //        if (((MasterListItem)cmbServices.SelectedItem).ID == 0)
            //        {
            //            cmbServices.TextBox.SetValidation("Please Select Service");
            //            cmbServices.TextBox.RaiseValidationError();
            //            cmbServices.Focus();
            //            ErrorStatus = false;
            //        }
            //    }
            //}


            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.SetValidation("Please Enter Name");
                txtName.RaiseValidationError();
                txtName.Focus();
                ErrorStatus = false;
            }
            else
                txtName.ClearValidationError();


            //if (ErrorStatus == true)
            //{
            //    cmbServices.TextBox.ClearValidationError();             

            //    txtRate.ClearValidationError();

            //    ErrorStatus = true;
            //}

            return ErrorStatus;
        }



        private void FillOccupation()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_OccupationMaster;
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
                        cmbOccupation.ItemsSource = null;
                        cmbOccupation.ItemsSource = objList.DeepCopy();
                        cmbOccupation.SelectedItem = objList[0];
                        FillCountry();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                // wait.Close();
                throw;
            }
        }

        public void FillCountry()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
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
                        txtCountry.ItemsSource = null;
                        txtCountry.ItemsSource = objList.DeepCopy();
                        txtCountry.SelectedItem = objList[0];
                        FetchData();
                    }

                    if (grpMasterDetails.DataContext != null)
                    {
                        if (((MasterListItem)txtCountry.SelectedItem).ID != ((AgentVO)grpMasterDetails.DataContext).CountryID)
                            txtCountry.SelectedValue = ((AgentVO)grpMasterDetails.DataContext).CountryID;

                    }


                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public void FillState(long CountryID)
        {
            try
            {
                clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
                BizAction.CountryId = CountryID;
                BizAction.ListStateDetails = new List<clsStateVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                        {
                            if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                            {
                                foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                                {
                                    MasterListItem obj = new MasterListItem();
                                    obj.ID = item.Id;
                                    obj.Description = item.Description;
                                    objList.Add(obj);
                                }
                            }
                        }

                        txtState.ItemsSource = null;
                        txtState.ItemsSource = objList.DeepCopy();


                        if ((grpMasterDetails.DataContext) != null)
                        {
                            if (((MasterListItem)txtState.SelectedItem).ID != ((AgentVO)grpMasterDetails.DataContext).StateID)
                                txtState.SelectedValue = ((AgentVO)grpMasterDetails.DataContext).StateID;

                            if (txtState.ItemsSource != null)
                            {
                                List<MasterListItem> objStatelist = new List<MasterListItem>();

                                objStatelist = ((List<MasterListItem>)txtState.ItemsSource).ToList();

                                MasterListItem objState = objStatelist.Where(x => x.ID == ((AgentVO)grpMasterDetails.DataContext).StateID).FirstOrDefault();

                                txtState.SelectedItem = objState;
                            }
                        }
                        else
                        {
                            txtState.SelectedItem = objM;
                        }

                    }


                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public void FillCity(long StateID)
        {
            try
            {
                clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
                BizAction.StateId = StateID;
                BizAction.ListCityDetails = new List<clsCityVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        if (BizAction.ListCityDetails != null)
                        {
                            if (BizAction.ListCityDetails.Count > 0)
                            {
                                foreach (clsCityVO item in BizAction.ListCityDetails)
                                {
                                    MasterListItem obj = new MasterListItem();
                                    obj.ID = item.Id;
                                    obj.Description = item.Description;
                                    objList.Add(obj);
                                }
                            }
                        }
                        txtCity.ItemsSource = null;
                        txtCity.ItemsSource = objList.DeepCopy();


                        if ((grpMasterDetails.DataContext) != null)
                        {
                            if (((MasterListItem)txtCity.SelectedItem).ID != ((AgentVO)grpMasterDetails.DataContext).CityID)
                                txtCity.SelectedValue = ((AgentVO)grpMasterDetails.DataContext).CityID;

                            if (txtCity.ItemsSource != null)
                            {
                                List<MasterListItem> objCitylist = new List<MasterListItem>();

                                objCitylist = ((List<MasterListItem>)txtCity.ItemsSource).ToList();

                                MasterListItem objCity = objCitylist.Where(x => x.ID == ((AgentVO)grpMasterDetails.DataContext).CityID).FirstOrDefault();

                                txtCity.SelectedItem = objCity;
                            }
                        }
                        else
                        {
                            txtCity.SelectedItem = objM;
                        }
                    }

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {

            }
        }


        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
            if (txtCountry.SelectedItem != null && ((MasterListItem)txtCountry.SelectedItem).ID > 0)
            {
                //((clsPatientVO)this.DataContext).CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;
                ////added by neena
                //((clsPatientVO)this.DataContext).CountryN = ((MasterListItem)txtCountry.SelectedItem).Description;
                ////txtCountry1.Text = ((clsPatientVO)this.DataContext).CountryN;
                ////txtCountry1.Visibility = Visibility.Visible;
                ////txtCountry.Visibility = Visibility.Collapsed;
                ////StateObj = new MasterListItem(((clsPatientVO)this.DataContext).StateID, ((clsPatientVO)this.DataContext).StateCode, ((clsPatientVO)this.DataContext).StateN);
                ////

                ////added by neena
                //List<MasterListItem> objList = new List<MasterListItem>();
                //MasterListItem objM = new MasterListItem(0, "-- Select --");
                //objList.Add(objM);
                //txtState.ItemsSource = objList.DeepCopy();
                //txtState.SelectedItem = objM.DeepCopy();
                //txtCity.ItemsSource = objList.DeepCopy();
                //txtCity.SelectedItem = objM.DeepCopy();


                FillState(((MasterListItem)txtCountry.SelectedItem).ID);
            }
            else
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                MasterListItem objM = new MasterListItem(0, "-- Select --");
                objList.Add(objM);
                txtState.ItemsSource = objList.DeepCopy();
                txtState.SelectedItem = objM.DeepCopy();
                txtCity.ItemsSource = objList.DeepCopy();
                txtCity.SelectedItem = objM.DeepCopy();
            }
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (txtState.SelectedItem != null && txtState.SelectedValue != null)
            if (txtState.SelectedItem != null && ((MasterListItem)txtState.SelectedItem).ID > 0)
            {
                //((clsPatientVO)this.DataContext).StateID = ((MasterListItem)txtState.SelectedItem).ID;
                //((clsPatientVO)this.DataContext).StateN = ((MasterListItem)txtState.SelectedItem).Description;

                //List<MasterListItem> objList = new List<MasterListItem>();
                //MasterListItem objM = new MasterListItem(0, "-- Select --");
                //objList.Add(objM);
                //txtCity.ItemsSource = objList.DeepCopy();
                //txtCity.SelectedItem = objList[0].DeepCopy();

                FillCity(((MasterListItem)txtState.SelectedItem).ID);
            }
            else
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                MasterListItem objM = new MasterListItem(0, "-- Select --");
                objList.Add(objM);
                txtCity.ItemsSource = objList.DeepCopy();
                txtCity.SelectedItem = objList[0].DeepCopy();

            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        int cnt = 0;

        List<YearClinic> DonationListData = null;
        List<YearClinic> DonationList = null;

        List<YearClinic> SurrogacyListData = null;
        List<YearClinic> SurrogacyList = null;

        private void txtNODonation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
                grdDonationMaster.ItemsSource = null;
                if (DonationList != null)
                    DonationList.Clear();
                if (DonationListData != null)
                    DonationListData.Clear();
            }
            else
            {
                //if (blNo != true)
                //{
                if (txtNODonation.Text != string.Empty && txtNODonation.Text != "0")
                {
                    //dgSacsDetailsGrid.ItemsSource = null;

                    if (grdDonationMaster.ItemsSource != null)
                    {
                        DonationList = new List<YearClinic>();
                        DonationList = DonationListData;
                        if (DonationList.Count < Convert.ToInt32(txtNODonation.Text))
                        {
                            for (int i = DonationList.Count; i < Convert.ToInt32(txtNODonation.Text); i++)
                            {
                                cnt = i + 1;
                                DonationListData.Add(new YearClinic()
                                {
                                    SrNo = cnt.ToString(),
                                    Year = "",
                                    Clinic = "",
                                    IsDonation = true,
                                });
                            }
                            grdDonationMaster.ItemsSource = null;
                            grdDonationMaster.ItemsSource = DonationList;
                            DonationListData = DonationList;
                        }
                        else if (DonationList.Count > Convert.ToInt32(txtNODonation.Text))
                        {
                            DonationListData = new List<YearClinic>();
                            for (int i = 0; i < Convert.ToInt32(txtNODonation.Text); i++)
                            {
                                cnt = i + 1;
                                DonationListData.Add(new YearClinic()
                                {
                                    SrNo = cnt.ToString(),
                                    Year = "",
                                    Clinic = "",
                                    IsDonation = true,
                                });

                            }
                            grdDonationMaster.ItemsSource = null;
                            grdDonationMaster.ItemsSource = DonationListData;
                        }

                    }
                    else
                    {
                        DonationListData = new List<YearClinic>();
                        for (int i = 0; i < Convert.ToInt32(txtNODonation.Text); i++)
                        {
                            cnt = i + 1;
                            DonationListData.Add(new YearClinic()
                            {
                                SrNo = cnt.ToString(),
                                Year = "",
                                Clinic = "",
                                IsDonation = true,
                            });
                        }
                        grdDonationMaster.ItemsSource = null;
                        grdDonationMaster.ItemsSource = DonationListData;
                        //SacGrid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    grdDonationMaster.ItemsSource = null;
                    if (DonationList != null)
                        DonationList.Clear();
                    if (DonationListData != null)
                        DonationListData.Clear();
                }

                //}


                //if (txtNODonation.Text != string.Empty && txtNODonation.Text != "0")
                //{
                //    DonationList = new List<YearClinic>();

                //    for (int i = 0; i < Convert.ToInt32(txtNODonation.Text); i++)
                //    {
                //        cnt = i + 1;
                //        DonationList.Add(new YearClinic()
                //        {
                //            SrNo = cnt.ToString(),
                //            Year = "",
                //            Clinic = "",
                //            IsDonation = true,
                //        });

                //        grdDonationMaster.ItemsSource = DonationList;

                //    }
                //}
            }
        }



        private void txtNODonation_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNOSurrogacy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
                grdSurrogacyMaster.ItemsSource = null;
                if (SurrogacyList != null)
                    SurrogacyList.Clear();
                if (SurrogacyListData != null)
                    SurrogacyListData.Clear();
            }
            else
            {
                //if (blNo != true)
                //{
                if (txtNOSurrogacy.Text != string.Empty && txtNOSurrogacy.Text != "0")
                {
                    //dgSacsDetailsGrid.ItemsSource = null;

                    if (grdSurrogacyMaster.ItemsSource != null)
                    {
                        SurrogacyList = new List<YearClinic>();
                        SurrogacyList = SurrogacyListData;
                        if (SurrogacyList.Count < Convert.ToInt32(txtNOSurrogacy.Text))
                        {
                            for (int i = SurrogacyList.Count; i < Convert.ToInt32(txtNOSurrogacy.Text); i++)
                            {
                                cnt = i + 1;
                                SurrogacyListData.Add(new YearClinic()
                                {
                                    SrNo = cnt.ToString(),
                                    Year = "",
                                    Clinic = "",
                                    IsDonation = false,
                                });
                            }
                            grdSurrogacyMaster.ItemsSource = null;
                            grdSurrogacyMaster.ItemsSource = SurrogacyList;
                            SurrogacyListData = SurrogacyList;
                        }
                        else if (SurrogacyList.Count > Convert.ToInt32(txtNOSurrogacy.Text))
                        {
                            SurrogacyListData = new List<YearClinic>();
                            for (int i = 0; i < Convert.ToInt32(txtNOSurrogacy.Text); i++)
                            {
                                cnt = i + 1;
                                SurrogacyListData.Add(new YearClinic()
                                {
                                    SrNo = cnt.ToString(),
                                    Year = "",
                                    Clinic = "",
                                    IsDonation = false,
                                });

                            }
                            grdSurrogacyMaster.ItemsSource = null;
                            grdSurrogacyMaster.ItemsSource = SurrogacyListData;
                        }

                    }
                    else
                    {
                        SurrogacyListData = new List<YearClinic>();
                        for (int i = 0; i < Convert.ToInt32(txtNOSurrogacy.Text); i++)
                        {
                            cnt = i + 1;
                            SurrogacyListData.Add(new YearClinic()
                            {
                                SrNo = cnt.ToString(),
                                Year = "",
                                Clinic = "",
                                IsDonation = false,
                            });
                        }
                        grdSurrogacyMaster.ItemsSource = null;
                        grdSurrogacyMaster.ItemsSource = SurrogacyListData;
                        //SacGrid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    grdSurrogacyMaster.ItemsSource = null;
                    if (SurrogacyList != null)
                        SurrogacyList.Clear();
                    if (SurrogacyListData != null)
                        SurrogacyListData.Clear();
                }

                //}


                //if (txtNOSurrogacy.Text != string.Empty && txtNOSurrogacy.Text != "0")
                //{
                //    SurrogacyList = new List<YearClinic>();
                //    cnt = 0;
                //    for (int i = 0; i < Convert.ToInt32(txtNOSurrogacy.Text); i++)
                //    {
                //        cnt = i + 1;
                //        SurrogacyList.Add(new YearClinic()
                //        {
                //            SrNo = cnt.ToString(),
                //            Year = "",
                //            Clinic = "",
                //            IsDonation = false,
                //        });


                //        grdSurrogacyMaster.ItemsSource = SurrogacyList;

                //    }
                //}
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearControl();
            objAnimation.Invoke(RotationType.Forward);

            //DataList = new PagedSortableCollectionView<AgentVO>();
            //grdMaster.SelectedItem = null;
            this.DataContext = null;
            //FillClassGrid();
            SetCommandButtonState("New");
            // cmbSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            // cmbSubSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        private void chkIfYesrdo_Click(object sender, RoutedEventArgs e)
        {
            if (chkIfYesrdo.IsChecked == true)
                txtMarried.IsEnabled = true;

        }

        private void chkIfNordo_Click(object sender, RoutedEventArgs e)
        {
            if (chkIfNordo.IsChecked == true)
                txtMarried.IsEnabled = false;
            txtMarried.Text = string.Empty;

        }

        private void chkPreviousDoneIfYesrdo_Click(object sender, RoutedEventArgs e)
        {
            if (chkPreviousDoneIfYesrdo.IsChecked == true)
                txtNODonation.IsEnabled = true;
        }

        private void chkPreviousDoneIfNordo_Click(object sender, RoutedEventArgs e)
        {
            if (chkPreviousDoneIfNordo.IsChecked == true)
                txtNODonation.IsEnabled = false;
            txtNODonation.Text = string.Empty;
            DonationListData.Clear();
            grdDonationMaster.ItemsSource = null;
        }

        private void chkPreviousSurrogacyDoneIfYesrdo_Click(object sender, RoutedEventArgs e)
        {
            if (chkPreviousSurrogacyDoneIfYesrdo.IsChecked == true)
                txtNOSurrogacy.IsEnabled = true;
        }

        private void chkPreviousSurrogacyDoneIfNordo_Click(object sender, RoutedEventArgs e)
        {
            if (chkPreviousSurrogacyDoneIfNordo.IsChecked == true)
                txtNOSurrogacy.IsEnabled = false;
            txtNOSurrogacy.Text = string.Empty;
            SurrogacyListData.Clear();
            grdSurrogacyMaster.ItemsSource = null;

        }

        private void txtMarried_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMarried_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtLandlineCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text.Trim()))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsItNumber() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 4)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void txtLandlineContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            //if (((WaterMarkTextbox)sender).Text.EndsWith(" "))
            //    {
            //        ((WaterMarkTextbox)sender).Text=((WaterMarkTextbox)sender).Text.Remove(((WaterMarkTextbox)sender).Text.Length - 1, 1);
            //    }
            //else
            //{
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text.Trim()))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsItNumber() && textBefore != null)
                    //  if (!((WaterMarkTextbox)sender).Text.IsMobileNumberValid() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }

            // }
        }
    }
}
