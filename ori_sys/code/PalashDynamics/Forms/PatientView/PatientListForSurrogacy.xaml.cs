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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.Forms.PatientView
{
    public partial class PatientListForSurrogacy : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "REG":
                    isReg = true;
                    break;
                case "VISIT":
                    isReg = false;
                    break;
                case "Surrogacy":
                    isReg = true;
                    isSurrogacy = true;
                    break;
                default:

                    break;
            }
        }

        #endregion

        bool isLoaded = false;
        bool isReg = true;
        bool isSurrogacy = false;
        public PatientListForSurrogacy()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientList_Loaded);
        }

        void PatientList_Loaded(object sender, RoutedEventArgs e)
        {
            if (isReg == false)
                this.DataContext = new SurrogateSearchViewModel();
            else if (isReg == true)
                this.DataContext = new SurrogateSearchViewModel(true);

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //IsPatientExist = true;
            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
            mElement1.Text = "Find Patient";
            FillRegistrationType();
            FillCountryList();
            FillGender();
            FillUnitList();
            FillLoyaltyProgram();
            FillSurrogateAgency();


            SearchList();
            isLoaded = true;
            txtFirstName.Focus();

        }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PatientListForSurrogacy(DateTime? FromDate, DateTime? ToDate)
        {
            InitializeComponent();
            //FillCountryList();
            //FillGender();
            //FillLoyaltyProgram();
            //// By BHUSHAN
            //FillRegistrationType();
            this.DataContext = new SurrogateSearchViewModel(FromDate, ToDate);
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];
                }
                // FillUnitList();
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        private void FillLoyaltyProgram()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbLoyaltyProgram.ItemsSource = null;
                    cmbLoyaltyProgram.ItemsSource = objList;
                    cmbLoyaltyProgram.SelectedItem = objList[0];
                }
                // FillSurrogateAgency();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSurrogateAgency()
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SurrogateAgencyMaster;
            // BizAction.Parent = new KeyValue();

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

                    cmbAgency.ItemsSource = null;
                    cmbAgency.ItemsSource = objList;
                    cmbAgency.SelectedItem = objList[0];

                }

                // SearchList();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillCountryList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Country";
            BizAction.IsDecode = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
                // FillGender();
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();

        }

        private void FillStateList(string pCountry)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pCountry))
            {
                pCountry = pCountry.Trim();
                BizAction.Parent = new KeyValue { Key = "Country", Value = pCountry };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();


        }

        private void FillCityList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        private void FillUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        var res = from r in objList
                                  where r.ID == User.UserLoginInfo.UnitId
                                  select r;
                        cmbClinic.SelectedItem = ((MasterListItem)res.First());
                        cmbClinic.IsEnabled = true;
                    }
                    else
                    {
                        cmbClinic.SelectedValue = User.UserLoginInfo.UnitId;
                        cmbClinic.IsEnabled = false;
                    }
                }
                //FillLoyaltyProgram();
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        private void GetPinCodeList(string pArea)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Pincode";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pArea))
            {
                pArea = pArea.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pArea };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPinCode.ItemsSource = null;
                    txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchList();
        }

        public void SearchList()
        {
            bool res = true;

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.ToDate != null)
            {
                if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate > ((SurrogateSearchViewModel)this.DataContext).BizActionObject.ToDate)
                {
                    dtpFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate.Value.Date > DateTime.Now.Date)
            {
                dtpFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;

            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitToDate != null)
            {
                if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate > ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitToDate)
                {
                    dtpVisitFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpVisitFromDate.RaiseValidationError();
                    dtpVisitFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpVisitFromDate.ClearValidationError();
                }
            }
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpVisitFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpVisitFromDate.RaiseValidationError();
                dtpVisitFromDate.Focus();
                res = false;
            }
            else
            {
                dtpVisitFromDate.ClearValidationError();
            }
            //=====================================

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate != null)
            {
                if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate > ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate)
                {
                    dtpAdmissionFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpAdmissionFromDate.RaiseValidationError();
                    dtpAdmissionFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpAdmissionFromDate.ClearValidationError();
                }
            }

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpAdmissionFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpAdmissionFromDate.RaiseValidationError();
                dtpAdmissionFromDate.Focus();
                res = false;
            }
            else
            {
                dtpAdmissionFromDate.ClearValidationError();
            }
            //========================================
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBToDate != null)
            {
                if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate > ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBToDate)
                {
                    dtpDOBFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpDOBFromDate.RaiseValidationError();
                    dtpDOBFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpDOBFromDate.ClearValidationError();
                }
            }
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpDOBFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpDOBFromDate.RaiseValidationError();
                dtpDOBFromDate.Focus();
                res = false;
            }
            else
            {
                dtpDOBFromDate.ClearValidationError();
            }
            //========================================

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.ToDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.ToDate = DateTime.Now;

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.ToDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.FromDate = Convert.ToDateTime("1/1/1900");


            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitToDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitToDate = DateTime.Now;

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitToDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitFromDate = Convert.ToDateTime("1/1/1900");

            //====================================
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate = DateTime.Now;

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate = Convert.ToDateTime("1/1/1900");
            //=====================================
            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBToDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBToDate = DateTime.Now;

            if (((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBToDate != null && ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate == null)
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBFromDate = Convert.ToDateTime("1/1/1900");
            //=====================================            
            if (res)
            {
                if (cmbGender.SelectedItem != null)
                    ((SurrogateSearchViewModel)this.DataContext).BizActionObject.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.SearchInAnotherClinic = false;

                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                //{
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {
                    //By Anjali....................
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((MasterListItem)cmbClinic.SelectedItem).ID && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    {
                        ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                    }
                    else
                    {
                        ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    }
                    //............................
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false && ((MasterListItem)cmbClinic.SelectedItem).ID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    {
                        ((SurrogateSearchViewModel)this.DataContext).BizActionObject.SearchInAnotherClinic = true;
                    }
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    {
                        ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                    }
                    else
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        else
                            ((SurrogateSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                }

                if (chkLoyaltyMember.IsChecked == true)
                {
                    if (cmbLoyaltyProgram.SelectedItem != null)
                        ((SurrogateSearchViewModel)this.DataContext).BizActionObject.LoyaltyProgramID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;
                }
                //By Anjali......................

                ((SurrogateSearchViewModel)this.DataContext).BizActionObject.IsFromSurrogacyModule = isSurrogacy;
                //................................

                if (cmbAgency.SelectedItem != null)
                {
                    ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AgencyID = ((MasterListItem)cmbAgency.SelectedItem).ID;
                }

                // BY BHUSHAN
                if (cmbRegisttype.SelectedItem != null)
                {
                    ((SurrogateSearchViewModel)this.DataContext).BizActionObject.RegistrationTypeID = ((MasterListItem)cmbRegisttype.SelectedItem).ID;
                }
                ((SurrogateSearchViewModel)this.DataContext).GetData();
                peopleDataPager.PageIndex = 0;


            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DataContext = new clsGetPatientGeneralDetailsListForSurrogacyBizActionVO();
            this.DataContext = new SurrogateSearchViewModel();

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO(); // (clsPatientGeneralVO)dataGrid2.SelectedItem;
            peopleDataPager.PageIndex = 0;
            //dataGrid2.ItemsSource = null;       

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                fillCoupleDetails();
            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
            // txtMiddleName.Focus();
        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                switch (acc.SelectedIndex)
                {
                    case 0:
                        // txtCountry.UpdateLayout();
                        txtCountry.Focus();
                        break;
                    default:
                        //SearchButton.UpdateLayout();
                        //SearchButton.Focus();
                        break;
                }
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
            // txtLastName.Focus();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            //   txtFamilyName.Focus();
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

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCountry.Text = txtCountry.Text.ToTitleCase();
            FillStateList(txtCountry.Text);
            //txtState.Focus();
        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();

            FillCityList(txtState.Text);
            //txtCity.Focus();
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            GetPinCodeList(txtCity.Text);
            // txtPinCode.Focus();
        }

        private void txtPinCode_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtContactNo.Focus();
        }

        private void cmbGender_LostFocus(object sender, RoutedEventArgs e)
        {
            // txtCivilID.Focus();
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PrintCardButton_Click(object sender, RoutedEventArgs e)
        {

            if (((IApplicationConfiguration)App.Current).SelectedPatient != null &&
                (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                long PatientId = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                long UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCard.aspx?ID=" + PatientId + "&UnitID=" + UnitID), "_blank");

            }
        }

        #region Fill Couple Details
        public void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = new clsCoupleVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient = new clsPatientGeneralVO();

                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
                    {
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        GetHeightAndWeight(((IApplicationConfiguration)App.Current).SelectedCoupleDetails);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.BMI = BizAction.CoupleDetails.FemalePatient.BMI;

                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Height = BizAction.CoupleDetails.MalePatient.Height;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #endregion

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkRegistration.IsChecked != true)
            {
                dtpFromDate.Text = String.Empty;
                dtpToDate.Text = String.Empty;
            }

            if (chkVisit.IsChecked != true)
            {
                dtpVisitFromDate.Text = String.Empty;
                dtpVisitToDate.Text = String.Empty;
            }

            if (chkRegistration.IsChecked.HasValue) ((SurrogateSearchViewModel)this.DataContext).BizActionObject.RegistrationWise = chkRegistration.IsChecked.Value;
            if (chkAdmission.IsChecked.HasValue) ((SurrogateSearchViewModel)this.DataContext).BizActionObject.AdmissionWise = chkAdmission.IsChecked.Value;
            if (chkVisit.IsChecked.HasValue) ((SurrogateSearchViewModel)this.DataContext).BizActionObject.VisitWise = chkVisit.IsChecked.Value;
            if (chkDOB.IsChecked.HasValue) ((SurrogateSearchViewModel)this.DataContext).BizActionObject.DOBWise = chkDOB.IsChecked.Value;
        }

        private void PatientFile_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null &&
               (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                long PatientId = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                long PatientUnitId = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/Patientfile.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/PatientFile2.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/PatientFile3.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/Patientfile4.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
            }
        }

        // BY BHUSHAN. . . . . 
        public void FillRegistrationType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
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

                    var results = from r in ((clsGetMasterListBizActionVO)e.Result).MasterList
                                  where r.ID == 10
                                  || r.ID == 7
                                  select r;

                    objList.AddRange(results.ToList());

                    cmbRegisttype.ItemsSource = null;
                    cmbRegisttype.ItemsSource = objList;
                    cmbRegisttype.SelectedItem = objList[0];
                    if (isSurrogacy == true)
                    {
                        cmbRegisttype.SelectedValue = (long)10;
                        //cmbRegisttype.IsEnabled = false;
                    }
                }
                // FillCountryList();
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }


        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void dtpVisitFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           // SearchList();
        }

        private void dtpVisitToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void txtContactNo_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
        }

        private void txtCivilID_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
        }

        private void txtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    SearchList();
            //}
        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    SearchList();
            //}
        }

        private void cmbRegisttype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void SearchButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                SearchList();
            }
        }

        private void cmbUnitSelecetion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             SearchList();
        }

        private void cmbAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AttachDoc_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                frmPatientScanDocument win = new frmPatientScanDocument();
                win.Title = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                win.IsSurrogacy = true;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

    }
}
