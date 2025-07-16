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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using CIMS.Forms;
using PalashDynamics;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;

namespace OPDModule.Forms
{
    public partial class frmDonorRegistration : UserControl,IInitiateCIMS
    {
        public frmDonorRegistration()
        {
            InitializeComponent();
        }
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
                FillReferralName();
                FillPatientType();
                FillMaritalStatus();
                FillReligion();
                FillOccupation();
                FillBloodGroup();
                FillGender();
                FillPatientSponsorDetails();
                txtFirstName.Focus();
                FillCountry();
                FillDonorSource();
                FillHairColor();
                FillEyeColor();
                FillSkinColor();
            }
        }

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
        private void FillReferralName()
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
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

              
                        cmbReferralName.ItemsSource = null;
                        cmbReferralName.ItemsSource = objList;
                        cmbReferralName.SelectedItem = objList[0];
                    
                }

                if (this.DataContext != null)
                {
                    cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillPatientType()
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
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                   
                        cmbPatientType.ItemsSource = null;
                        cmbPatientType.ItemsSource = objList;
                        cmbPatientType.SelectedItem = objList[0];

                    
                }

                if (this.DataContext != null)
                {
                   // cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void GetPinCodeList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Pincode";
            BizAction.IsDecode = true;
            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPinCode.ItemsSource = null;
                    txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
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
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList.DeepCopy();
                    cmbMaritalStatus.SelectedItem = objList[0];

                   
                }
                if (this.DataContext != null)
                {
                    
                  
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void GetFamilyList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "FamilyName";
            BizAction.IsDecode = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy

            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtFamilyName.MinimumPopulateDelay = 100;
                    txtFamilyName.ItemsSource = null;
                    txtFamilyName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
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
                    cmbGender.ItemsSource = objList.DeepCopy();
                    cmbGender.SelectedItem = objList[0];
                    
                }
                if (this.DataContext != null)
                {
                    
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillReligion()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReligionMaster;
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
                    cmbReligion.ItemsSource = null;
                    cmbReligion.ItemsSource = objList.DeepCopy();
                    cmbReligion.SelectedItem = objList[0];
                   
                }

                if (this.DataContext != null)
                {
                   
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillOccupation()
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

                    
                }
                if (this.DataContext != null)
                {
                    
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
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
                        cmbPatientSource.ItemsSource = null;
                        cmbPatientSource.ItemsSource = objList;
                        cmbPatientSource.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FillCountry()
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
                   
                }
                if (this.DataContext != null)
                {
                    
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCountry(long CountryID, long StateID, long CityID, long RegionID)
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

                    }
                    
                    FillState(CountryID, StateID, CityID, RegionID);
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }
        public void FillState(long CountryID, long StateID, long CityID, long RegionID)
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
                    txtState.SelectedItem = objList[0];
                   

                    if (this.DataContext != null)
                    {
                        
                    }
                    FillCity(StateID, CityID, RegionID);
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID, long CityID, long RegionID)
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
                    txtCity.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                       
                    }
                    FillRegion(CityID);
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillState(long CountryID)
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
                        txtState.SelectedItem = objM;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID)
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
                        txtCity.SelectedItem = objM;
                 

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillRegion(long CityID)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
            BizAction.CityId = CityID;
            BizAction.ListRegionDetails = new List<clsRegionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();
                 txtArea.SelectedItem = objM;
                   
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillDonorSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DonorSource;
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
                    cmbDonorSource.ItemsSource = null;
                    cmbDonorSource.ItemsSource = objList.DeepCopy();
                    cmbDonorSource.SelectedItem = objList[0];

                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
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
        #endregion
        byte[] Photo;
        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoWindow phWin = new PhotoWindow();
            if (this.DataContext != null)
                phWin.MyPhoto = ((clsPatientVO)this.DataContext).Photo;
            phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            phWin.Show();
        }
        void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
        }
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
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
        }

       
        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            txtFamilyName.Text= txtLastName.Text; //((clsPatientVO)this.DataContext).GeneralDetails.LastName;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtDD.SelectAll();
        }
        private DateTime? CalculateDateFromAge(int Year, int Month, int Days)
        {
            try
            {
                DateTime BirthDate;
                //if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                //    BirthDate = DateTobeConvert.Value;
                //else
                BirthDate = DateTime.Now;
                if (Year > 0)
                {
                    BirthDate = BirthDate.AddYears(-Year);
                }

                if (Month > 0)
                {
                    BirthDate = BirthDate.AddMonths(-Month);
                }

                if (Days > 0)
                {
                    BirthDate = BirthDate.AddDays(-Days);
                }
                //result = (age.Day - 1).ToString();
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        void CalculateBirthDate()
        {

            int Yearval = 0;
            int Monthval = 0;
            int DayVal = 0;

            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
                Yearval = int.Parse(txtYY.Text.Trim());

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
                Monthval = int.Parse(txtMM.Text.Trim());


            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
                DayVal = int.Parse(txtDD.Text.Trim());

            if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            {
                dtpDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
                dtpDOB.ClearValidationError();
                txtYY.ClearValidationError();
                txtMM.ClearValidationError();
                txtDD.ClearValidationError();
            }
            else
            {

                txtYY.SetValidation("Age is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Please select the Date of birth");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";


            }
        }

        private void SaveDonor() 
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddUpdateDonorBizActionVO BizAction = new clsAddUpdateDonorBizActionVO();
            BizAction.DonorDetails = (clsPatientVO)this.DataContext;

            BizAction.DonorDetails.FirstName=txtFirstName.Text;
            BizAction.DonorDetails.MiddleName = txtMiddleName.Text;
            BizAction.DonorDetails.LastName = txtLastName.Text;
            BizAction.DonorDetails.FamilyName = txtFamilyName.Text;
            BizAction.DonorDetails.CivilID = txtCivilId.Text;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.DonorDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            if (cmbBloodGroup.SelectedItem != null)
                BizAction.DonorDetails.BloodGroupID = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;

            if (cmbGender.SelectedItem != null)
                BizAction.DonorDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbReligion.SelectedItem != null)
                BizAction.DonorDetails.ReligionID = ((MasterListItem)cmbReligion.SelectedItem).ID;

            if (cmbOccupation.SelectedItem != null)
                BizAction.DonorDetails.OccupationId = ((MasterListItem)cmbOccupation.SelectedItem).ID;

            if (cmbPatientType.SelectedItem != null)
                BizAction.DonorDetails.GeneralDetails.PatientTypeID = ((MasterListItem)cmbPatientType.SelectedItem).ID;

            if (cmbPatientSource.SelectedItem != null) 
                BizAction.DonorDetails.GeneralDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

            BizAction.DonorDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.DonorDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.DonorDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.DonorDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.DonorDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());
            if (cmbReferralName.SelectedItem != null)
                BizAction.DonorDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

            BizAction.DonorDetails.CompanyName = txtOfficeName.Text.Trim();
            if (txtCountry.SelectedItem != null)
                BizAction.DonorDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.DonorDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.DonorDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.DonorDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
            //...........................
            BizAction.DonorDetails.IsDonor = true;
            if (cmbSkinColor.SelectedItem != null)
                BizAction.DonorDetails.SkinColorID = ((MasterListItem)cmbSkinColor.SelectedItem).ID;
            if (cmbHairColor.SelectedItem != null)
                BizAction.DonorDetails.HairColorID = ((MasterListItem)cmbHairColor.SelectedItem).ID;
            if (cmbEyeColor.SelectedItem != null)
                BizAction.DonorDetails.EyeColorID = ((MasterListItem)cmbEyeColor.SelectedItem).ID;
            if (cmbDonorSource.SelectedItem != null)
                BizAction.DonorDetails.DonorSourceID = ((MasterListItem)cmbDonorSource.SelectedItem).ID;
            if (txtHeight.Text !=null)
            BizAction.DonorDetails.Height = Convert.ToDouble(txtHeight.Text);
            BizAction.DonorDetails.DonorCode = txtDonorCode.Text;
            BizAction.DonorDetails.Photo = Photo;
            //..................................
      
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
                    ((clsPatientVO)this.DataContext).SpouseDetails.PatientID = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.SpouseDetails.PatientID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.UnitId = ((clsAddUpdateDonorBizActionVO)arg.Result).DonorDetails.SpouseDetails.UnitId;
                 
                    Indicatior.Close();
                  
                        SaveSponsor();
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                        txtMRNumber.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
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
                

              if((MasterListItem)cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
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
        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
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
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please Enter Valid Email-ID");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtArea.ItemsSource = null;
                    txtArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
        }

        private void txtDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
        }

        private void txtPinCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtTaluka_LostFocus(object sender, RoutedEventArgs e)
        {
            txtTaluka.Text = txtTaluka.Text.ToTitleCase();
        }

        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDOB.SelectedDate > DateTime.Now)
            {
                txtYY.SetValidation("Age Is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age Is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age Is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Date of Birth Can Not Be Greater Than Today");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else if (dtpDOB.SelectedDate == null)
            {
                txtYY.SetValidation("Age Is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age Is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age Is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Please Select The Date of Birth");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else
            {
                dtpDOB.ClearValidationError();
                txtYY.ClearValidationError();
                txtMM.ClearValidationError();
                txtDD.ClearValidationError();

                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            }
            txtYY.SelectAll();
        }
        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            int day = BirthDate.Day;
                            int curday = DateTime.Now.Day;
                            int dif = 0;
                            if (day > curday)
                                dif = (curday + 30) - day;
                            else
                                dif = curday - day;
                            result = dif.ToString();
                            //result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }
       

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
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
