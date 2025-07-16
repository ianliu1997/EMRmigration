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
using System.Windows.Browser;
using OPDModule;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master;

namespace CIMS.Forms
{
    public partial class KinDetailWindow : ChildWindow,IInitiateCIMS
    {
        #region IInitiateCIMS Members
        private bool EditMode = false;
        private bool IsAddresSameAsPatient = false;
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    this.Title = this.Title + "- " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    // App.IsPatientMode = true;
                    this.DataContext = new clsPatientKinDetailsVO { PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, CountryId=1};
                    //this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    break;

                case "EDIT":
                    EditMode = true;
                    break;
            }
        }
        #endregion

        public KinDetailWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            clsAddPatientKinDetailsBizActionVO BizAction = new clsAddPatientKinDetailsBizActionVO();
            BizAction.KinDetails = (clsPatientKinDetailsVO)this.DataContext;

            BizAction.KinDetails.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.KinDetails.Name = txtName.Text;
            BizAction.KinDetails.ContactNo1 = txtContact1.Text;
            BizAction.KinDetails.ContactNo2 = txtContact2.Text;
            BizAction.KinDetails.AddressLine1 = txtAddLine1.Text;
            BizAction.KinDetails.AddressLine2 = txtAddLine2.Text;
            BizAction.KinDetails.AddressLine3 = txtAddLine3.Text;
            BizAction.KinDetails.AddedDateTime = DateTime.Now;
            BizAction.KinDetails.UpdatedDateTime = DateTime.Now;
            
            if (cmbCountry.SelectedItem != null)
                BizAction.KinDetails.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;

            if (cmbState.SelectedItem != null)
                BizAction.KinDetails.StateId = ((MasterListItem)cmbState.SelectedItem).ID;

            if (cmbCity.SelectedItem != null)
                BizAction.KinDetails.CityId = ((MasterListItem)cmbCity.SelectedItem).ID;

            BizAction.KinDetails.Pincode = txtPincode.Text;

            PalashServiceClient Client = new PalashServiceClient();
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsAddPatientKinDetailsBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        HtmlPage.Window.Alert("Patient Kin Details Saved Successfully.");
                    }
                }
                else
                    HtmlPage.Window.Alert("Error occured while adding patient Kin Details.");
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            checked
            {
                IsAddresSameAsPatient = true;
                
                HtmlPage.Window.Alert("Copy/Display Address details from Patient to Kin Address"); //Remove this message box letter

                Int64 PID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.PatientDetails.GeneralDetails.PatientID = PID;

                PalashServiceClient Client = new PalashServiceClient();
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        this.txtAddLine1.Text = String.IsNullOrEmpty(((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine1)? "":((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine1;
                        this.txtAddLine2.Text = String.IsNullOrEmpty(((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine2) ? "" : ((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine2;
                        this.txtAddLine3.Text = String.IsNullOrEmpty(((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine3) ? "" : ((clsGetPatientBizActionVO)arg.Result).PatientDetails.AddressLine3;
                        
                        //FillCountryList();
                        //((clsPatientKinDetailsVO)this.DataContext).CountryId = (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.CountryId;
                        //((clsPatientKinDetailsVO)this.DataContext).StateId = (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.StateId;
                        //((clsPatientKinDetailsVO)this.DataContext).CityId = (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.CityId;

                        //IEnumerator<MasterListItem> list = (IEnumerator<MasterListItem>)cmbCountry.ItemsSource.GetEnumerator();
                        //while (list.MoveNext())
                        //{
                        //    MasterListItem a = list.Current;
                        //    if (a.ID == (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.CountryId)
                        //    {
                        //        cmbCountry.SelectedItem = a;
                        //        FillStateList(a.ID);
                        //        break;
                        //    }
                        //}

                        //IEnumerator<MasterListItem> list1 = (IEnumerator<MasterListItem>)cmbState.ItemsSource.GetEnumerator();
                        //while (list1.MoveNext())
                        //{
                        //    MasterListItem a = list1.Current;
                        //    if (a.ID == (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.StateId)
                        //    {
                        //        cmbState.SelectedItem = a;                                
                        //        break;
                        //    }
                        //}
                        
                        //IEnumerator<MasterListItem> list2 = (IEnumerator<MasterListItem>)cmbCity.ItemsSource.GetEnumerator();
                        //while (list2.MoveNext())
                        //{
                        //    MasterListItem a = list2.Current;
                        //    if (a.ID == (Int64)((clsGetPatientBizActionVO)arg.Result).PatientDetails.CityId)
                        //    {
                        //        cmbCity.SelectedItem = a;
                        //        break;
                        //    }
                        //}
                        
                        this.txtPincode.Text = String.IsNullOrEmpty(((clsGetPatientBizActionVO)arg.Result).PatientDetails.Pincode) ? "" : ((clsGetPatientBizActionVO)arg.Result).PatientDetails.Pincode;

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }

            
        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {                        
            FillCountryList();
        }

        private void FillCountryList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.CountryMaster;
            BizAction.MasterList = new List<MasterListItem>();

            PalashServiceClient Client = null;
            Client = new PalashServiceClient();
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    cmbCountry.ItemsSource = null;
                    cmbCountry.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                }

                if (this.DataContext != null)
                {
                    //FillStateList(((clsPatientKinDetailsVO)this.DataContext).CountryId);
                    
                    if (IsAddresSameAsPatient == true)
                    {
                        int i = 0;
                        while (true)
                        {
                            if (((clsGetMasterListBizActionVO)e.Result).MasterList[i].ID == ((clsPatientKinDetailsVO)this.DataContext).CountryId)
                            {
                                cmbState.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[i];
                                break;
                            }
                            i++;
                        }
                    }
                    else
                    {
                        cmbCountry.SelectedValue = ((clsPatientKinDetailsVO)this.DataContext).CountryId;
                    }
                }
            };

            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }

        private void FillStateList(long pCountryID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.StateMaster;

            if (pCountryID > 0)
                BizAction.Parent = new KeyValue { Key = pCountryID, Value = "CountryID" };

            BizAction.MasterList = new List<MasterListItem>();

            PalashServiceClient Client = null;
            Client = new PalashServiceClient();
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    if (((clsGetMasterListBizActionVO)e.Result).MasterList.Count > 0)
                    {
                        if (IsAddresSameAsPatient == true)
                        {
                            int i = 0;
                            while (true)
                            {
                                if (((clsGetMasterListBizActionVO)e.Result).MasterList[i].ID == ((clsPatientKinDetailsVO)this.DataContext).StateId)
                                {
                                    cmbState.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[i];
                                    break;
                                }
                                i++;
                            }
                        }
                        else
                        {
                            cmbState.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[0];
                        }
                        //FillCityList(((clsPatientKinDetailsVO)this.DataContext).StateId);

                    }
                }
            };

            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }
        
        private void FillCityList(long pStateID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.CityMaster;

            if (pStateID > 0)
                BizAction.Parent = new KeyValue { Key = pStateID, Value = "StateID" };


            BizAction.MasterList = new List<MasterListItem>();

            PalashServiceClient Client = null;
            Client = new PalashServiceClient();
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    cmbCity.ItemsSource = null;
                    cmbCity.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    if (((clsGetMasterListBizActionVO)e.Result).MasterList.Count > 0)
                    {
                        if (IsAddresSameAsPatient == true)
                        {
                            int i = 0;
                            while (true)
                            {
                                if (((clsGetMasterListBizActionVO)e.Result).MasterList[i].ID == ((clsPatientKinDetailsVO)this.DataContext).CityId)
                                {
                                    cmbState.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[i];
                                    break;
                                }
                                i++;
                            }
                        }
                        else
                        {
                            cmbCity.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[0];
                        }
                    }
                    //cmbCity.UpdateLayout();
                    //cmbCity.Focus();
                }
            };

            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbCountry.SelectedItem != null)
                FillStateList(((MasterListItem)cmbCountry.SelectedItem).ID);

        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbState.SelectedItem != null)
                FillCityList(((MasterListItem)cmbState.SelectedItem).ID);
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            unchecked
            {
                IsAddresSameAsPatient = false;
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}

