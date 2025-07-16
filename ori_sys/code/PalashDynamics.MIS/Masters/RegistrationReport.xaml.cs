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
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.MIS.Masters
{
    public partial class RegistrationReport : UserControl
    {
        bool Flagref = false;
        public RegistrationReport()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Click_1(object sender, RoutedEventArgs e)
        {
            //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            //added by rohini dated 25/12/2015
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
            FillPatientType();
            FillReferralName();
            FillRefDoctor();
            FillBloodGroup();
            FillEducationMaster();
            FillMaritalStatus();
            FillOccupation();
            FillIdentity();
            FillSpecialRegistrationMaster();
            FillReligion();
            FillGender();
            FillPrefLanguageMaster();
            FillTreatRequiredMasterMaster();
            FillNationalityMaster();
            //FillCountry();
            //FillState();
            //FillCity();
            //  added by rohini dated 24/12/2015
            FillPatientCategory();
            FillCountry();
            FillCamp(); //***//
            FillMarketingExecutives();  //added on 16032018

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpTODate.SelectedDate = DateTime.Now.Date;
        }
        DateTime? dtpF = null;
        DateTime? dtpT = null;
        DateTime? dtpDOB1 = null;
        DateTime? dtpAnni = null;
        //Nullable<DateTime> dtpAnni = null;
        long a, b, c, d, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u,v;
        string FromAge = "";
        string ToAge = "";
        //added by rohini dated 24/12/2015
        string Mobile = "";
        string Mrno = "";

        long photoattached = 2, Documentttachd = 2;
        private void FillClinic()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            var res = from r in objList
                                      where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                      select r;
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                        }
                        else
                            cmbClinic.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillPatientCategory()
        {


            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;
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

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;
                    cmbPatientCategory.SelectedItem = objList[0];


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dtpFromDate.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (dtpTODate.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "To Date Can Not Be Greater Than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (dtpFromDate.SelectedDate > dtpTODate.SelectedDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                try
                {
                    dtpF = dtpFromDate.SelectedDate;
                    dtpT = dtpTODate.SelectedDate;
                    dtpDOB1 = dtpDOB.SelectedDate;
                    dtpAnni = dtpMarriageAnniversary.SelectedDate;
                    if (PhotoAttached.IsChecked == true)
                    {
                        photoattached = 1;
                    }
                    else if (NoPhotoAttached.IsChecked == true)
                    {
                        photoattached = 0;
                    }
                    else
                    {
                        photoattached = 2;
                    }
                    if (DocumentAttached.IsChecked == true)
                    {
                        Documentttachd = 1;
                    }
                    else if (NoDocumentAttached.IsChecked == true)
                    {
                        Documentttachd = 0;
                    }
                    else
                    {
                        Documentttachd = 2;
                    }
                    a = ((MasterListItem)(cmbRegistrationType.SelectedItem)).ID;
                    b = ((MasterListItem)(cmbReferenceFrom.SelectedItem)).ID;
                    c = ((MasterListItem)(cmbRefDoctor.SelectedItem)).ID;
                    d = ((MasterListItem)(cmbBloodGroup.SelectedItem)).ID;
                    s = ((MasterListItem)(cmbEducation.SelectedItem)).ID;
                    f = ((MasterListItem)(cmbMaritalStatus.SelectedItem)).ID;
                    g = ((MasterListItem)(cmbOccupation.SelectedItem)).ID;
                    h = ((MasterListItem)(cmbIdProof.SelectedItem)).ID;
                    i = ((MasterListItem)(cmbSpecialRegistration.SelectedItem)).ID;
                    j = ((MasterListItem)(cmbReligion.SelectedItem)).ID;
                    k = ((MasterListItem)(cmbGender.SelectedItem)).ID;
                    l = ((MasterListItem)(cmbPreferredLanguage.SelectedItem)).ID;
                    m = ((MasterListItem)(cmbTreatmentRequired.SelectedItem)).ID;
                    n = ((MasterListItem)(cmbNationality.SelectedItem)).ID;
                    o = ((MasterListItem)(cmbCountry.SelectedItem)).ID;
                    p = ((MasterListItem)(cmbState.SelectedItem)).ID;
                    q = ((MasterListItem)(cmbCity.SelectedItem)).ID;
                    r = ((MasterListItem)(cmbArea.SelectedItem)).ID;

                    //ADDED BY ROHINI DATED 24/12/2015
                    t = ((MasterListItem)(cmbPatientCategory.SelectedItem)).ID;                  

                    if (txtFromAge.Text != string.Empty)
                        FromAge = txtFromAge.Text;
                    else
                        FromAge = "";

                    if (txtToAge.Text != string.Empty)
                        ToAge = txtToAge.Text;
                    else
                        ToAge = "";
                    //added by rohini h dated 24/12/2015
                    if (txtMobile.Text != string.Empty)
                        Mobile = txtMobile.Text;
                    else
                        Mobile = "";
                    if (txtMRNO.Text != string.Empty)
                        Mrno = txtMRNO.Text;
                    else
                        Mrno = "";
                    //

                    if (cmbMarketingExecutives.SelectedItem != null)
                        u = ((MasterListItem)(cmbMarketingExecutives.SelectedItem)).ID;

                    if (cmbCampDetail.SelectedItem != null) //***//
                        v = ((MasterListItem)(cmbCampDetail.SelectedItem)).ID;

                    long ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    string URL = "../Reports/Administrator/PAtientRegistrationMaster.aspx?a=" + a + "&b=" + b + "&c=" + c + "&d=" + d + "&Uid=" + lUnitID + "&s=" + s + "&f=" + f + "&g=" + g + "&h=" + h + "&i=" + i + "&j=" + j + "&k=" + k + "&l=" + l + "&m=" + m + "&n=" + n + "&o=" + o + "&p=" + p + "&q=" + q + "&r=" + r + "&t=" + t + "&u=" + u + "&v=" + v + "&photoattached=" + photoattached + "&Documentttachd=" + Documentttachd + "&FromAge=" + FromAge + "&ToAge=" + ToAge + "&Mobile=" + Mobile + "&Mrno=" + Mrno + "&ClinicID=" + ClinicID + "&Excel=" + chkExcel.IsChecked;
                    if (dtpF != null)
                    {
                        URL += "&dtpF=" + dtpF.Value.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        URL += "&dtpF=";
                    }
                    if (dtpT != null)
                    {
                        dtpT = dtpT.Value.AddDays(1);
                        URL += "&dtpT=" + dtpT.Value.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        URL += "&dtpT=";
                    }
                    if (dtpDOB1 != null)
                    {
                        URL += "&dtpDOB1=" + dtpDOB1.Value.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        URL += "&dtpDOB1=";
                    }
                    if (dtpAnni != null)
                    {
                        URL += "&dtpAnni=" + dtpAnni.Value.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        URL += "&dtpAnni=";
                    }
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void FillPatientType()
        {
            try
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

                        //if (Flagref == true)
                        //{
                        var results = from r in objList
                                      where r.ID != 10
                                      select r;
                        cmbRegistrationType.ItemsSource = null;
                        cmbRegistrationType.ItemsSource = results.ToList();
                        cmbRegistrationType.SelectedItem = results.ToList()[0];
                        //}
                        //else
                        ////{
                        //    cmbPatientType.ItemsSource = null;
                        //    cmbPatientType.ItemsSource = objList;

                        //}
                        //cmbPatientType.ItemsSource = null;
                        //cmbPatientType.ItemsSource = objList;
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

        private void FillReferralName()
        {
            //cmbReferralName
            try
            {
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

                        if (Flagref == true)
                        {
                            var results = from r in objList
                                          where r.ID == 10
                                          select r;
                            cmbReferenceFrom.ItemsSource = null;
                            cmbReferenceFrom.ItemsSource = results.ToList();
                            cmbReferenceFrom.SelectedItem = results.ToList()[0];

                        }
                        else
                        {
                            cmbReferenceFrom.ItemsSource = null;
                            cmbReferenceFrom.ItemsSource = objList;
                            cmbReferenceFrom.SelectedItem = objList[0];
                        }
                        //cmbPatientType.ItemsSource = null;
                        //cmbPatientType.ItemsSource = objList;
                    }


                    FillPatientType();
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

        private void FillRefDoctor()
        {
            try
            {
                //clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
                //BizAction.ComboList = new List<clsComboMasterBizActionVO>();

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //Client.ProcessCompleted += (s, e) =>
                //{

                //    if (e.Error == null && e.Result != null)
                //    {
                //        if (((clsGetRefernceDoctorBizActionVO)e.Result).ComboList != null)
                //        {

                //            clsGetRefernceDoctorBizActionVO result = (clsGetRefernceDoctorBizActionVO)e.Result;
                //            List<MasterListItem> objList = new List<MasterListItem>();

                //            objList.Add(new MasterListItem(0, "- Select -"));
                //            if (result.ComboList != null)
                //            {
                //                foreach (var item in result.ComboList)
                //                {
                //                    MasterListItem Objmaster = new MasterListItem();
                //                    Objmaster.ID = item.ID;
                //                    Objmaster.Description = item.Value;
                //                    objList.Add(Objmaster);

                //                }
                //            }
                //            cmbRefDoctor.ItemsSource = null;
                //            cmbRefDoctor.ItemsSource = objList;
                //            cmbRefDoctor.SelectedItem = objList[0];

                //            if (this.DataContext != null)
                //            {
                //                cmbRefDoctor.SelectedValue = ((clsPatientVO)this.DataContext).ReferralDoctorID;

                //            }
                //        }
                //    }
                //    //FillIdentity();


                //};

                //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //Client.CloseAsync();

                //commented by rohini dated 28/12/2015 for reffreal doctors not comes 
                clsGetDoctorListForComboBizActionVO BizAction = new clsGetDoctorListForComboBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetDoctorListForComboBizActionVO)arg.Result).MasterList);

                        cmbRefDoctor.ItemsSource = null;
                        cmbRefDoctor.ItemsSource = objList;
                        cmbRefDoctor.SelectedItem = objList[0];


                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();



            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillBloodGroup()
        {
            try
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
                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbBloodGroup.ItemsSource = null;
                        cmbBloodGroup.ItemsSource = objList.DeepCopy();
                        cmbBloodGroup.SelectedItem = objList[0];

                    }

                    //if (this.DataContext != null)
                    //{
                    //    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                    //    cmbSpouseBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;
                    //}
                    //FillGender();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillEducationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_EducationDetailsMaster;
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
                        cmbEducation.ItemsSource = null;
                        cmbEducation.ItemsSource = objList.DeepCopy();
                        cmbEducation.SelectedItem = objList[0];


                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbEducation.SelectedValue = ((clsPatientVO)this.DataContext).EducationID;
                    //    cmbSpouseEducation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.EducationID;
                    //}
                    //FillPrefLanguageMaster();

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillMaritalStatus()
        {
            try
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
                        //cmbMaritalStatus.ItemsSource = null;
                        //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        List<MasterListItem> objList = new List<MasterListItem>();

                        // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbMaritalStatus.ItemsSource = null;
                        cmbMaritalStatus.ItemsSource = objList.DeepCopy();
                        cmbMaritalStatus.SelectedItem = objList[0];


                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                    //    cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                    //}
                    //FillReligion();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
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

                        //cmbSpouseOccupation.ItemsSource = null;
                        //cmbSpouseOccupation.ItemsSource = objList.DeepCopy();
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                    //    cmbSpouseOccupation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.OccupationId;
                    //}
                    //FillBloodGroup();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillIdentity()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IdentityMaster;
                BizAction.IsActive = true;
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
                        cmbIdProof.ItemsSource = null;
                        cmbIdProof.ItemsSource = objList.DeepCopy();
                        cmbIdProof.SelectedItem = objList[0];

                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbIdentity.SelectedValue = ((clsPatientVO)this.DataContext).IdentityID;
                    //    cmbSpouseIdentity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.IdentityID;
                    //}
                    //FillReferralName();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillSpecialRegistrationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
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
                        cmbSpecialRegistration.ItemsSource = null;
                        cmbSpecialRegistration.ItemsSource = objList.DeepCopy();
                        cmbSpecialRegistration.SelectedItem = objList[0];


                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpecialRegID;
                    //    cmbSpouseSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.SpecialRegID;
                    //}
                    //wait.Close();
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

        private void FillReligion()
        {
            try
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

                        //cmbSpouseReligion.ItemsSource = null;
                        //cmbSpouseReligion.ItemsSource = objList.DeepCopy();
                    }

                    //if (this.DataContext != null)
                    //{
                    //    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
                    //    cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;
                    //}
                    //FillOccupation();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillGender()
        {
            try
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
                        //cmbSpouseGender.ItemsSource = null;
                        //cmbSpouseGender.ItemsSource = objList.DeepCopy();
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                    //    cmbSpouseGender.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.GenderID;
                    //}
                    //FillEducationMaster();

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

        private void FillPrefLanguageMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PrefLanguageMaster;
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
                        cmbPreferredLanguage.ItemsSource = null;
                        cmbPreferredLanguage.ItemsSource = objList.DeepCopy();
                        cmbPreferredLanguage.SelectedItem = objList[0];

                        //cmbSpousePreferredLanguage.ItemsSource = null;
                        //cmbSpousePreferredLanguage.ItemsSource = objList.DeepCopy();
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbPreferredLanguage.SelectedValue = ((clsPatientVO)this.DataContext).PreferredLangID;
                    //    cmbSpousePreferredLanguage.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.PreferredLangID;
                    //}
                    //FillTreatRequiredMasterMaster();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillTreatRequiredMasterMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TreatRequiredMaster;
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
                        cmbTreatmentRequired.ItemsSource = null;
                        cmbTreatmentRequired.ItemsSource = objList.DeepCopy();
                        cmbTreatmentRequired.SelectedItem = objList[0];

                        //cmbSpouseTreatmentRequired.ItemsSource = null;
                        //cmbSpouseTreatmentRequired.ItemsSource = objList.DeepCopy();
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbTreatmentRequired.SelectedValue = ((clsPatientVO)this.DataContext).TreatRequiredID;
                    //    cmbSpouseTreatmentRequired.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.TreatRequiredID;
                    //}
                    //FillNationalityMaster();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        private void FillNationalityMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_NationalityMaster;
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
                        cmbNationality.ItemsSource = null;
                        cmbNationality.ItemsSource = objList.DeepCopy();
                        cmbNationality.SelectedItem = objList[0];

                        //cmbSpouseNationality.ItemsSource = null;
                        //cmbSpouseNationality.ItemsSource = objList.DeepCopy();
                    }
                    //if (this.DataContext != null)
                    //{
                    //    cmbNationality.SelectedValue = ((clsPatientVO)this.DataContext).NationalityID;
                    //    cmbSpouseNationality.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.NationalityID;
                    //}
                    //FillSpecialRegistrationMaster();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

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
        //            cmbCountry.ItemsSource = null;
        //            cmbCountry.ItemsSource = objList.DeepCopy();
        //            cmbCountry.SelectedItem = objList[0];

        //            //txtSpouseCountry.ItemsSource = null;
        //            //txtSpouseCountry.ItemsSource = objList.DeepCopy();
        //        }
        //        //if (this.DataContext != null)
        //        //{
        //        //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
        //        //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
        //        //}
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //public void FillState()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_StateMaster;
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
        //            cmbState.ItemsSource = null;
        //            cmbState.ItemsSource = objList.DeepCopy();
        //            cmbState.SelectedItem = objList[0];

        //            //txtSpouseCountry.ItemsSource = null;
        //            //txtSpouseCountry.ItemsSource = objList.DeepCopy();
        //        }
        //        //if (this.DataContext != null)
        //        //{
        //        //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
        //        //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
        //        //}
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //public void FillCity()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_CityMaster;
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
        //            cmbCity.ItemsSource = null;
        //            cmbCity.ItemsSource = objList.DeepCopy();
        //            cmbCity.SelectedItem = objList[0];

        //            //txtSpouseCountry.ItemsSource = null;
        //            //txtSpouseCountry.ItemsSource = objList.DeepCopy();
        //        }
        //        //if (this.DataContext != null)
        //        //{
        //        //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
        //        //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
        //        //}
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //public void FillRegion()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_RegionMaster;
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
        //            cmbArea.ItemsSource = null;
        //            cmbArea.ItemsSource = objList.DeepCopy();
        //            cmbArea.SelectedItem = objList[0];

        //            //txtSpouseCountry.ItemsSource = null;
        //            //txtSpouseCountry.ItemsSource = objList.DeepCopy();
        //        }
        //        //if (this.DataContext != null)
        //        //{
        //        //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
        //        //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
        //        //}
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
        //                cmbCountry.ItemsSource = null;
        //                cmbCountry.ItemsSource = objList.DeepCopy();

        //                //txtSpouseCountry.ItemsSource = null;
        //                //txtSpouseCountry.ItemsSource = objList.DeepCopy();
        //            }
        //            //if (this.DataContext != null)
        //            //{
        //            //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
        //            //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
        //            //}
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
        //            cmbState.ItemsSource = null;
        //            cmbState.ItemsSource = objList.DeepCopy();

        //            //txtSpouseState.ItemsSource = null;
        //            //txtSpouseState.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
        //            //    txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
        //            //}
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
        //            cmbCity.ItemsSource = null;
        //            cmbCity.ItemsSource = objList.DeepCopy();

        //            //txtSpouseCity.ItemsSource = null;
        //            //txtSpouseCity.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
        //            //    txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
        //            //}
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

        //            cmbState.ItemsSource = null;
        //            cmbState.ItemsSource = objList.DeepCopy();

        //            //txtSpouseState.ItemsSource = null;
        //            //txtSpouseState.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
        //            //    txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
        //            //}
        //            //else
        //            //{
        //            //    cmbState.SelectedItem = objM;
        //            //    txtSpouseState.SelectedItem = objM;
        //            //}

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
        //            cmbCity.ItemsSource = null;
        //            cmbCity.ItemsSource = objList.DeepCopy();

        //            //txtSpouseCity.ItemsSource = null;
        //            //txtSpouseCity.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
        //            //    txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
        //            //}
        //            //else
        //            //{
        //            //    txtCity.SelectedItem = objM;
        //            //    txtSpouseCity.SelectedItem = objM;
        //            //}

        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
        //List<clsRegionVO> RegionList;
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
        //                    RegionList = new List<clsRegionVO>();
        //                    RegionList = BizAction.ListRegionDetails;
        //                    foreach (clsRegionVO item in BizAction.ListRegionDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            cmbArea.ItemsSource = null;
        //            cmbArea.ItemsSource = objList.DeepCopy();

        //            //txtSpouseArea.ItemsSource = null;
        //            //txtSpouseArea.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
        //            //    txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
        //            //    txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
        //            //    txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
        //            //}
        //            //else
        //            //{
        //            //    txtArea.SelectedItem = objM;
        //            //    txtSpouseArea.SelectedItem = objM;

        //            //}
        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbCountry.SelectedItem != null && cmbCountry.SelectedValue != null)
        //        if (((MasterListItem)cmbCountry.SelectedItem).ID > 0)
        //        {
        //            ((clsPatientVO)this.DataContext).CountryID = ((MasterListItem)cmbCountry.SelectedItem).ID;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            cmbState.ItemsSource = objList;
        //            cmbState.SelectedItem = objM;
        //            cmbCity.ItemsSource = objList;
        //            cmbCity.SelectedItem = objM;
        //            cmbArea.ItemsSource = objList;
        //            cmbArea.SelectedItem = objM;
        //            FillState(((MasterListItem)cmbCountry.SelectedItem).ID);
        //        }
        //        else
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            cmbState.ItemsSource = objList;
        //            cmbState.SelectedItem = objM;
        //            cmbCity.ItemsSource = objList;
        //            cmbCity.SelectedItem = objM;
        //            cmbArea.ItemsSource = objList;
        //            cmbArea.SelectedItem = objM;
        //        }
        //}

        //private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbState.SelectedItem != null && cmbState.SelectedValue != null)
        //        if (((MasterListItem)cmbState.SelectedItem).ID > 0)
        //        {
        //            ((clsPatientVO)this.DataContext).StateID = ((MasterListItem)cmbState.SelectedItem).ID;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            cmbCity.ItemsSource = objList;
        //            cmbCity.SelectedItem = objList[0];
        //            cmbArea.ItemsSource = objList;
        //            cmbArea.SelectedItem = objList[0];
        //            FillCity(((MasterListItem)cmbState.SelectedItem).ID);
        //        }
        //        else
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            cmbCity.ItemsSource = objList;
        //            cmbCity.SelectedItem = objList[0];
        //            cmbArea.ItemsSource = objList;
        //            cmbArea.SelectedItem = objList[0];
        //        }
        //}

        //private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbState.SelectedItem != null && cmbState.SelectedValue != null)
        //        if (((MasterListItem)cmbState.SelectedItem).ID > 0)
        //        {
        //            if (((MasterListItem)cmbCity.SelectedItem).ID > 0)
        //            {
        //                ((clsPatientVO)this.DataContext).CityID = ((MasterListItem)cmbCity.SelectedItem).ID;

        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                MasterListItem objM = new MasterListItem(0, "-- Select --");
        //                objList.Add(objM);
        //                cmbArea.ItemsSource = null;
        //                cmbArea.SelectedItem = objList[0];
        //                FillRegion(((MasterListItem)cmbCity.SelectedItem).ID);
        //            }
        //            else
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                MasterListItem objM = new MasterListItem(0, "-- Select --");
        //                objList.Add(objM);
        //                cmbArea.ItemsSource = null;
        //                cmbArea.SelectedItem = objList[0];
        //            }
        //        }
        //}

        //private void cmbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbState.SelectedItem != null && cmbState.SelectedValue != null)
        //        if (((MasterListItem)cmbState.SelectedItem).ID > 0)
        //        {
        //            if (((MasterListItem)cmbCity.SelectedItem).ID > 0)
        //            {
        //                ((clsPatientVO)this.DataContext).CityID = ((MasterListItem)cmbCity.SelectedItem).ID;

        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                MasterListItem objM = new MasterListItem(0, "-- Select --");
        //                objList.Add(objM);
        //                cmbArea.ItemsSource = null;
        //                cmbArea.SelectedItem = objList[0];
        //                FillRegion(((MasterListItem)cmbCity.SelectedItem).ID);
        //            }
        //            else
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                MasterListItem objM = new MasterListItem(0, "-- Select --");
        //                objList.Add(objM);
        //                cmbArea.ItemsSource = null;
        //                cmbArea.SelectedItem = objList[0];
        //            }
        //        }
        //}


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
                    cmbCountry.ItemsSource = null;
                    cmbCountry.ItemsSource = objList.DeepCopy();
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID > 0)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID
                                  select r;
                        cmbCountry.SelectedItem = ((MasterListItem)res.First());
                    }
                    else
                        cmbCountry.SelectedItem = objList[0];


                    //cmbCountry.SelectedItem = objList[0];
                    //cmbCountry.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID;

                }
                //if (this.DataContext != null)
                //{
                //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                //}
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
                        cmbCountry.ItemsSource = null;
                        cmbCountry.ItemsSource = objList.DeepCopy();


                    }
                    //if (this.DataContext != null)
                    //{
                    //    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                    //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                    //}
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
                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList.DeepCopy();

                    //txtSpouseState.ItemsSource = null;
                    //txtSpouseState.ItemsSource = objList.DeepCopy();

                    //if (this.DataContext != null)
                    //{
                    //    txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                    //    txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    //}
                    FillCity(StateID, CityID, RegionID);
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void PhotoAttached_Click(object sender, RoutedEventArgs e)
        {
            NoPhotoAttached.IsChecked = false;
        }

        private void NoPhotoAttached_Click(object sender, RoutedEventArgs e)
        {
            PhotoAttached.IsChecked = false;
        }

        private void DocumentAttached_Click(object sender, RoutedEventArgs e)
        {
            NoDocumentAttached.IsChecked = false;
        }

        private void NoDocumentAttached_Click(object sender, RoutedEventArgs e)
        {
            DocumentAttached.IsChecked = false;
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
                    cmbCity.ItemsSource = null;
                    cmbCity.ItemsSource = objList.DeepCopy();

                    //  txtSpouseCity.ItemsSource = null;
                    //   txtSpouseCity.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        cmbCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                        //  txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
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

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList.DeepCopy();

                    // txtSpouseState.ItemsSource = null;
                    // txtSpouseState.ItemsSource = objList.DeepCopy();
                    if (this.DataContext != null)
                    {
                        cmbState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                        //  txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                    else
                    {
                        cmbState.SelectedItem = objM;
                        // txtSpouseState.SelectedItem = objM;
                    }

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID > 0)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID
                                  select r;
                        cmbState.SelectedItem = ((MasterListItem)res.First());
                    }
                    else
                        cmbState.SelectedItem = objList[0];

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
                    cmbCity.ItemsSource = null;
                    cmbCity.ItemsSource = objList.DeepCopy();

                    //txtSpouseCity.ItemsSource = null;
                    //txtSpouseCity.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        cmbCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                        //txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                    }
                    else
                    {
                        cmbCity.SelectedItem = objM;
                        //txtSpouseCity.SelectedItem = objM;
                    }

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        List<clsRegionVO> RegionList;
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
                            RegionList = new List<clsRegionVO>();
                            RegionList = BizAction.ListRegionDetails;
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    cmbArea.ItemsSource = null;
                    cmbArea.ItemsSource = objList.DeepCopy();

                    //txtSpouseArea.ItemsSource = null;
                    //txtSpouseArea.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        cmbArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
                        //txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                        //  txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
                        //  txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
                    }
                    else
                    {
                        cmbArea.SelectedItem = objM;
                        //  txtSpouseArea.SelectedItem = objM;

                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCountry.SelectedItem != null)
                if (((MasterListItem)cmbCountry.SelectedItem).ID > 0)
                {
                    // ((clsPatientVO)this.DataContext).CountryID = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbState.ItemsSource = objList;
                    cmbState.SelectedItem = objM;
                    cmbCity.ItemsSource = objList;
                    cmbCity.SelectedItem = objM;
                    cmbArea.ItemsSource = objList;
                    cmbArea.SelectedItem = objM;
                    FillState(((MasterListItem)cmbCountry.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbState.ItemsSource = objList;
                    cmbState.SelectedItem = objM;
                    cmbCity.ItemsSource = objList;
                    cmbCity.SelectedItem = objM;
                    cmbArea.ItemsSource = objList;
                    cmbArea.SelectedItem = objM;
                }
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedItem != null)
                if (((MasterListItem)cmbState.SelectedItem).ID > 0)
                {
                    // ((clsPatientVO)this.DataContext).StateID = ((MasterListItem)cmbState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbCity.ItemsSource = objList;
                    cmbCity.SelectedItem = objList[0];
                    cmbArea.ItemsSource = objList;
                    cmbArea.SelectedItem = objList[0];
                    FillCity(((MasterListItem)cmbState.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbCity.ItemsSource = objList;
                    cmbCity.SelectedItem = objList[0];
                    cmbArea.ItemsSource = objList;
                    cmbArea.SelectedItem = objList[0];
                }
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbState.SelectedItem != null)
                if (((MasterListItem)cmbState.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbCity.SelectedItem).ID > 0)
                    {
                        //   ((clsPatientVO)this.DataContext).CityID = ((MasterListItem)cmbCity.SelectedItem).ID;

                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        cmbArea.ItemsSource = null;
                        cmbArea.SelectedItem = objList[0];
                        FillRegion(((MasterListItem)cmbCity.SelectedItem).ID);
                    }
                    else
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        cmbArea.ItemsSource = null;
                        cmbArea.SelectedItem = objList[0];
                    }
                }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtFromAge_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFromAge_TextChanged(object sender, RoutedEventArgs e)
        {



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

            }
        }



        private void txtFromAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFromAge.Text != string.Empty && txtToAge.Text != string.Empty)
            {

                if (Convert.ToInt32(txtFromAge.Text) > Convert.ToInt32(txtToAge.Text))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "From Age Cannot Be Greater Than To Age", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    txtFromAge.Text = string.Empty;
                    txtToAge.Text = string.Empty;
                }

            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }


        private void FillMarketingExecutives()
        {
            clsGetMarketingExecutivesListVO BizAction = new clsGetMarketingExecutivesListVO();

            BizAction.IsMarketingExecutives = true;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);

                    objList.AddRange(((clsGetMarketingExecutivesListVO)e.Result).MarketingExecutivesList);

                    cmbMarketingExecutives.ItemsSource = null;
                    cmbMarketingExecutives.ItemsSource = objList;

                    cmbMarketingExecutives.SelectedItem = objM;

                }

                //if (this.DataContext != null)
                //{
                //    cmbMarketingExecutives.SelectedValue = ((clsDoctorVO)this.DataContext).MarketingExecutivesID;
                //}

                //if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                //{
                //    cmbMarketingExecutives.SelectedValue = objDoctor.MarketingExecutivesID;
                //    cmbMarketingExecutives.UpdateLayout();

                //}

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCamp()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();             
                BizAction.MasterTable = MasterTableNameList.M_CampMaster;
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
                        cmbCampDetail.ItemsSource = null;
                        cmbCampDetail.ItemsSource = objList.DeepCopy();
                        cmbCampDetail.SelectedItem = objList[0];
                    }
                   

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }


    }
}
