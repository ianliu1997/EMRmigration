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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS.Forms;

namespace OPDModule.Forms
{
    public partial class DonorRegistrationChildWindow : ChildWindow,IInitiateCIMS
    {
        public DonorRegistrationChildWindow()
        {
            InitializeComponent();
        }
        public long PatientID;
        public long PatientUnitID;
        bool Flagref = false;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        bool IsPageLoded = false;
        bool IsPatientExist = false;
        private void DonorRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                rbtExisting.IsChecked = true;
                NewBatch.Visibility = Visibility.Collapsed;
                dgPastBatchList.Visibility = Visibility.Visible;
                //FillReferralName();
                //FillPatientType();
                //FillMaritalStatus();
                //FillReligion();
                //FillOccupation();
                //FillBloodGroup();
                //FillGender();
                //FillPatientSponsorDetails();
                //txtFirstName.Focus();
                //FillCountry();
                //FillDonorSource();
                FillHairColor();
                FillEyeColor();
                FillSkinColor();
                FillLab();
                FillReceivedBy();
            }
        }
        public event RoutedEventHandler OnSaveButton_Click;

        #region IInitiateCIMS Members

        /// <summary>
        /// Function is for Initializing the form.
        /// </summary>
        /// <param name="Mode">New or Edit</param>        
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    IsPatientExist = true;
                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                    mElement1.Text = "";

                    this.DataContext = new clsPatientVO()
                    {
                        MaritalStatusID = 0,
                        BloodGroupID = 0,
                        GenderID = 0,
                        OccupationId = 0,
                        Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                        State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                        District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                        RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                        City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                        Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,

                        // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                    };

                   
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                    ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                    ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                    ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                    
                    break;

             

                    
                    break;

              
            }

        }

        #endregion
        #region Fill Combobox
        //private void FillReferralName()
        //{
        //    //cmbReferralName

        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

              
        //                cmbReferralName.ItemsSource = null;
        //                cmbReferralName.ItemsSource = objList;
        //                cmbReferralName.SelectedItem = objList[0];
                    
        //        }

        //        if (this.DataContext != null)
        //        {
        //            cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void FillPatientType()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                   
        //                cmbPatientType.ItemsSource = null;
        //                cmbPatientType.ItemsSource = objList;
        //                cmbPatientType.SelectedItem = objList[0];

                    
        //        }

        //        if (this.DataContext != null)
        //        {
        //           // cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void GetPinCodeList(string pCity)
        //{
        //    clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
        //    //BizAction.IsActive = true;
        //    BizAction.TableName = "T_Registration";
        //    BizAction.ColumnName = "Pincode";
        //    BizAction.IsDecode = true;
        //    if (!string.IsNullOrEmpty(pCity))
        //    {
        //        pCity = pCity.Trim();
        //        BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
        //    }
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            txtPinCode.ItemsSource = null;
        //            txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void FillMaritalStatus()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
                    
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbMaritalStatus.ItemsSource = null;
        //            cmbMaritalStatus.ItemsSource = objList.DeepCopy();
        //            cmbMaritalStatus.SelectedItem = objList[0];

                   
        //        }
        //        if (this.DataContext != null)
        //        {
                    
                  
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void GetFamilyList()
        //{
        //    clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
        //    //BizAction.IsActive = true;
        //    BizAction.TableName = "T_Registration";
        //    BizAction.ColumnName = "FamilyName";
        //    BizAction.IsDecode = true;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy

        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            txtFamilyName.MinimumPopulateDelay = 100;
        //            txtFamilyName.ItemsSource = null;
        //            txtFamilyName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        private void FillBloodGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
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

                    cmbBloodGroup.ItemsSource = null;
                    cmbBloodGroup.ItemsSource = objList.DeepCopy();
                    cmbBloodGroup.SelectedItem = objList[0];

                  
                }

                if (this.DataContext != null)
                {
                   
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        //private void FillGender()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbGender.ItemsSource = null;
        //            cmbGender.ItemsSource = objList.DeepCopy();
        //            cmbGender.SelectedItem = objList[0];
                    
        //        }
        //        if (this.DataContext != null)
        //        {
                    
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void FillReligion()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_ReligionMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbReligion.ItemsSource = null;
        //            cmbReligion.ItemsSource = objList.DeepCopy();
        //            cmbReligion.SelectedItem = objList[0];
                   
        //        }

        //        if (this.DataContext != null)
        //        {
                   
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void FillOccupation()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_OccupationMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbOccupation.ItemsSource = null;
        //            cmbOccupation.ItemsSource = objList.DeepCopy();
        //            cmbOccupation.SelectedItem = objList[0];

                    
        //        }
        //        if (this.DataContext != null)
        //        {
                    
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //public void FillPatientSponsorDetails()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();

        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //                cmbPatientSource.ItemsSource = null;
        //                cmbPatientSource.ItemsSource = objList;
        //                cmbPatientSource.SelectedItem = objList[0];
        //            }
        //            if (this.DataContext != null)
        //            {
        //                cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void FillCountry()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            txtCountry.ItemsSource = null;
        //            txtCountry.ItemsSource = objList.DeepCopy();
        //            txtCountry.SelectedItem = objList[0];
                   
        //        }
        //        if (this.DataContext != null)
        //        {
                    
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //public void FillCountry(long CountryID, long StateID, long CityID, long RegionID)
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //                txtCountry.ItemsSource = null;
        //                txtCountry.ItemsSource = objList.DeepCopy();
        //                txtCountry.SelectedItem = objList[0];

        //            }
                    
        //            FillState(CountryID, StateID, CityID, RegionID);
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //public void FillState(long CountryID, long StateID, long CityID, long RegionID)
        //{
        //    clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
        //    BizAction.CountryId = CountryID;
        //    BizAction.ListStateDetails = new List<clsStateVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
        //            {
        //                if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
        //                {
        //                    foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            txtState.ItemsSource = null;
        //            txtState.ItemsSource = objList.DeepCopy();
        //            txtState.SelectedItem = objList[0];
                   

        //            if (this.DataContext != null)
        //            {
                        
        //            }
        //            FillCity(StateID, CityID, RegionID);
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //public void FillCity(long StateID, long CityID, long RegionID)
        //{
        //    clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
        //    BizAction.StateId = StateID;
        //    BizAction.ListCityDetails = new List<clsCityVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (BizAction.ListCityDetails != null)
        //            {
        //                if (BizAction.ListCityDetails.Count > 0)
        //                {
        //                    foreach (clsCityVO item in BizAction.ListCityDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            txtCity.ItemsSource = null;
        //            txtCity.ItemsSource = objList.DeepCopy();
        //            txtCity.SelectedItem = objList[0];
        //            if (this.DataContext != null)
        //            {
                       
        //            }
        //            FillRegion(CityID);
        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //public void FillState(long CountryID)
        //{
        //    clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
        //    BizAction.CountryId = CountryID;
        //    BizAction.ListStateDetails = new List<clsStateVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
        //            {
        //                if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
        //                {
        //                    foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }

        //            txtState.ItemsSource = null;
        //            txtState.ItemsSource = objList.DeepCopy();
        //                txtState.SelectedItem = objM;
        //        }


        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //public void FillCity(long StateID)
        //{
        //    clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
        //    BizAction.StateId = StateID;
        //    BizAction.ListCityDetails = new List<clsCityVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (BizAction.ListCityDetails != null)
        //            {
        //                if (BizAction.ListCityDetails.Count > 0)
        //                {
        //                    foreach (clsCityVO item in BizAction.ListCityDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            txtCity.ItemsSource = null;
        //            txtCity.ItemsSource = objList.DeepCopy();
        //                txtCity.SelectedItem = objM;
                 

        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //public void FillRegion(long CityID)
        //{
        //    clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
        //    BizAction.CityId = CityID;
        //    BizAction.ListRegionDetails = new List<clsRegionVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (BizAction.ListRegionDetails != null)
        //            {
        //                if (BizAction.ListRegionDetails.Count > 0)
        //                {
        //                    foreach (clsRegionVO item in BizAction.ListRegionDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            txtArea.ItemsSource = null;
        //            txtArea.ItemsSource = objList.DeepCopy();
        //         txtArea.SelectedItem = objM;
                   
        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //private void FillDonorSource()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_DonorSource;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbDonorSource.ItemsSource = null;
        //            cmbDonorSource.ItemsSource = objList.DeepCopy();
        //            cmbDonorSource.SelectedItem = objList[0];

        //        }
        //        if (this.DataContext != null)
        //        {

        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        private void FillHairColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_HairColor;
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
                    cmbHairColor.ItemsSource = null;
                    cmbHairColor.ItemsSource = objList.DeepCopy();
                    cmbHairColor.SelectedItem = objList[0];


                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillEyeColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_EyeColor;
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
                    cmbEyeColor.ItemsSource = null;
                    cmbEyeColor.ItemsSource = objList.DeepCopy();
                    cmbEyeColor.SelectedItem = objList[0];


                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillSkinColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SkinColor;
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
                    cmbSkinColor.ItemsSource = null;
                    cmbSkinColor.ItemsSource = objList.DeepCopy();
                    cmbSkinColor.SelectedItem = objList[0];


                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
           private void FillLab()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
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
                    cmbLab.ItemsSource = null;
                    cmbLab.ItemsSource = objList.DeepCopy();
                    cmbLab.SelectedItem = objList[0];


                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
           private void FillReceivedBy()
           {
               clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
               BizAction.MasterList = new List<MasterListItem>();
               BizAction.UnitId = 0;
               BizAction.DepartmentId = 0;
               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
               client.ProcessCompleted += (s, arg) =>
               {
                   if (arg.Error == null && arg.Result != null)
                   {
                       List<MasterListItem> objList = new List<MasterListItem>();
                       objList.Add(new MasterListItem(0, "-- Select --"));
                       objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                       cmbReceivedBy.ItemsSource = null;
                       cmbReceivedBy.ItemsSource = objList;
                       cmbReceivedBy.SelectedItem = objList[0];
                   }
               };
               client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
               client.CloseAsync();
           }
        #endregion
        byte[] Photo;
        int ClickedFlag = 0;
        private void SavePatientButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Donor Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
            }
            else
                ClickedFlag = 0;
            
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDonor();
            else
                ClickedFlag = 0;

        }

        private void ClosePatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void SaveDonor() 
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddUpdateDonorBizActionVO BizAction = new clsAddUpdateDonorBizActionVO();
            BizAction.DonorDetails = (clsPatientVO)this.DataContext;
            BizAction.BatchDetails = new clsDonorBatchVO();

            
            //if (cmbMaritalStatus.SelectedItem != null)
            //    BizAction.DonorDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            if (cmbBloodGroup.SelectedItem != null)
                BizAction.DonorDetails.BloodGroupID = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;

            //if (cmbGender.SelectedItem != null)
            //    BizAction.DonorDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            BizAction.DonorDetails.GenderID = 1;

            //if (cmbReligion.SelectedItem != null)
            //    BizAction.DonorDetails.ReligionID = ((MasterListItem)cmbReligion.SelectedItem).ID;

            //if (cmbOccupation.SelectedItem != null)
            //    BizAction.DonorDetails.OccupationId = ((MasterListItem)cmbOccupation.SelectedItem).ID;

            //if (cmbPatientType.SelectedItem != null)
                BizAction.DonorDetails.GeneralDetails.PatientTypeID = 9;

            //if (cmbPatientSource.SelectedItem != null) 
            //    BizAction.DonorDetails.GeneralDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

            //BizAction.DonorDetails.ContactNo1 = txtContactNo1.Text.Trim();
            //BizAction.DonorDetails.ContactNo2 = txtContactNo2.Text.Trim();
            //if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
            //    BizAction.DonorDetails.MobileCountryCode = long.Parse(txtMobileCountryCode.Text.Trim());

            //if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
            //    BizAction.DonorDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());

            //if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
            //    BizAction.DonorDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());
            //if (cmbReferralName.SelectedItem != null)
            //    BizAction.DonorDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

            //BizAction.DonorDetails.CompanyName = txtOfficeName.Text.Trim();
            //if (txtCountry.SelectedItem != null)
            //    BizAction.DonorDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            //if (txtState.SelectedItem != null)
            //    BizAction.DonorDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            //if (txtCity.SelectedItem != null)
            //    BizAction.DonorDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            //if (txtArea.SelectedItem != null)
            //    BizAction.DonorDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
            //...........................
            BizAction.DonorDetails.IsDonor = true;
            if (cmbSkinColor.SelectedItem != null)
                BizAction.DonorDetails.SkinColorID = ((MasterListItem)cmbSkinColor.SelectedItem).ID;
            if (cmbHairColor.SelectedItem != null)
                BizAction.DonorDetails.HairColorID = ((MasterListItem)cmbHairColor.SelectedItem).ID;
            if (cmbEyeColor.SelectedItem != null)
                BizAction.DonorDetails.EyeColorID = ((MasterListItem)cmbEyeColor.SelectedItem).ID;
            //if (cmbDonorSource.SelectedItem != null)
                BizAction.DonorDetails.DonorSourceID = 1;
            if (txtHeight.Text !=null)
            BizAction.DonorDetails.Height = Convert.ToDouble(txtHeight.Text);
            BizAction.DonorDetails.DonorCode = txtDonorCode.Text;
            BizAction.DonorDetails.Photo = Photo;
            //..................................

            BizAction.BatchDetails.BatchCode = txtBatch.Text;
            if (dtpReceivedDate.SelectedDate !=null)
            BizAction.BatchDetails.ReceivedDate = dtpReceivedDate.SelectedDate.Value.Date;
            if (cmbReceivedBy.SelectedItem != null)
                BizAction.BatchDetails.ReceivedByID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;

            if (cmbLab.SelectedItem != null)
                BizAction.BatchDetails.LabID = ((MasterListItem)cmbLab.SelectedItem).ID;
            BizAction.BatchDetails.InvoiceNo = txtInvoiceNumber.Text;
            if (txtNoofVials.Text !=null)
            BizAction.BatchDetails.NoOfVails = Convert.ToInt32(txtNoofVials.Text);
            BizAction.BatchDetails.Remark = txtRemark.Text;

      
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    CmdSave.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientID = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.GeneralDetails.PatientID;
                    ((clsPatientVO)this.DataContext).GeneralDetails.MRNo = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.GeneralDetails.MRNo;
                    ((clsPatientVO)this.DataContext).GeneralDetails.UnitId = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.GeneralDetails.UnitId;
                    PatientID = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.GeneralDetails.PatientID;
                    PatientUnitID = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.GeneralDetails.UnitId;
                 
                    Indicatior.Close();
                  
                        SaveSponsor();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    CmdSave.IsEnabled = true;
                    ClickedFlag = 0;
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void SaveSponsor()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
               
                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails =  new clsPatientSponsorVO();
                BizAction.PatientSponsorDetails.PatientId=((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                  BizAction.PatientSponsorDetails.PatientUnitId=((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                

              //if((MasterListItem)cmbPatientSource.SelectedItem != null)
              //      BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                    BizAction.PatientSponsorDetails.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
               
                
                    BizAction.PatientSponsorDetails.CompanyID = 0;

               
                    BizAction.PatientSponsorDetails.AssociatedCompanyID = 0;

                
                    BizAction.PatientSponsorDetails.DesignationID = 0;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }

                         Indicatior.Close();
                     }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                // throw;
                string err = ex.Message;
                Indicatior.Close();
            }
        }

        private void rbtExisting_Click(object sender, RoutedEventArgs e)
        {
            if (rbtExisting.IsChecked == true)
            {
                dgPastBatchList.Visibility = Visibility.Visible;
                NewBatch.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewBatch.Visibility = Visibility.Visible;
                dgPastBatchList.Visibility = Visibility.Collapsed;
            }
              
        }

        private void rbtNew_Click(object sender, RoutedEventArgs e)
        {
            if (rbtNew.IsChecked == true)
            {
                NewBatch.Visibility = Visibility.Visible;
                dgPastBatchList.Visibility = Visibility.Collapsed;
            }
            else 
            {
                NewBatch.Visibility = Visibility.Collapsed;
                dgPastBatchList.Visibility = Visibility.Visible;
            }

        }
    }
}


