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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Media.Imaging;
using MessageBoxControl;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.IO;
using DataDrivenApplication.Forms;

namespace PalashDynamics.IVF
{
    public partial class frmSpermThawing : UserControl,IInitiateCIMS
    {
        #region Variables
        public bool IsPatientExist;
        WaitIndicator wait = new WaitIndicator();        
        #endregion

        #region Properties
        public bool IsExpendedWindow { get; set; }
        public string Impression { get; set; }
        public Boolean IsEdit { get; set; }
        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }

        private ObservableCollection<clsSpermVitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsSpermVitrificationDetailsVO>();
        public ObservableCollection<clsSpermVitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }

        private ObservableCollection<clsSpermThawingDetailsVO> _ThawDetails = new ObservableCollection<clsSpermThawingDetailsVO>();
        public ObservableCollection<clsSpermThawingDetailsVO> ThawDetails
        {
            get { return _ThawDetails; }
            set { _ThawDetails = value; }
        }
        
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
       
        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }

        private List<MasterListItem> _Plan = new List<MasterListItem>();
        public List<MasterListItem> SelectedPLan
        {
            get
            {
                return _Plan;
            }
            set
            {
                _Plan = value;
            }
        }


        private List<MasterListItem> _LabIncharge = new List<MasterListItem>();
        public List<MasterListItem> SelectedLabIncharge
        {
            get
            {
                return _LabIncharge;
            }
            set
            {
                _LabIncharge = value;
            }
        }
        #endregion

        public frmSpermThawing()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SpermThawing_Loaded);
            this.Unloaded += new RoutedEventHandler(SpermThawing_Unloaded);
        }

        #region Loaded Event
        void SpermThawing_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {
                    if (IsPatientExist == false)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        fillCanID();
                        IsExpendedWindow = true;
                       
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion 

        #region Unloaded Event

        void SpermThawing_Unloaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //    PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient client = new PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
            //    client.GlobalDeleteFileCompleted += (s1, args1) =>
            //    {
            //        if (args1.Error == null)
            //        {

            //        }
            //    };
            //    client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", AttachedFileNameList);
            //}
            //catch (Exception Exception) { }
        }
        #endregion 

        #region Check Patient Is Selected/Not
        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null )
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    IsPatientExist = true;
                   
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;

                default:
                    break;
            }

        }

        #endregion

        #region Fill Master Item             

        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
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
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillLabPerson();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillLabPerson()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    //cmbLabPerson.ItemsSource = null;
                    SelectedLabIncharge = objList;
                    //cmbLabPerson.SelectedItem = objList[0];                    
                    fillPlan();
                    if (this.DataContext != null)
                    {
                        cmbLabPerson.SelectedValue = ((clsSpermThawingDetailsVO)this.DataContext).LabInchargeId;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillPlan()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PostThawingPlan;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        SelectedPLan = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillCoupleDetails();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        #endregion

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
        private void LoadPatientHeader()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
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
                        PatientInfo.Visibility = Visibility.Visible;
                        CoupleInfo.Visibility = Visibility.Collapsed;
                        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        #region Commented by Saily P on 25.09.12
                        //if (BizAction.PatientDetails.GenderID == 1)
                        //{
                        //    Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        //    {
                        //        Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                        //        CoupleInfo.Visibility = Visibility.Visible;
                        //        PatientInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //    else
                        //    {
                        //        PatientInfo.Visibility = Visibility.Visible;
                        //        CoupleInfo.Visibility = Visibility.Collapsed;
                        //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    }
                        //}
                        //else
                        //{
                        //    Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        //    {
                        //        Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                        //        CoupleInfo.Visibility = Visibility.Visible;
                        //        PatientInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //    else
                        //    {
                        //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //        PatientInfo.Visibility = Visibility.Visible;
                        //        CoupleInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //}
                        #endregion
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                wait.Close();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing is Only For Male", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }
        
        #region Fill Couple Details
        private void fillCoupleDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            {
                LoadPatientHeader();
                if (!IsEdit)
                {
                    FillSpermVitrification();
                }
                else
                {
                    //fillThawingDetails();
                }
                //Commented by Saily P on 260912 purpose, the form is applicable to donor patient
                //wait.Close();
                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msgW1.Show();
                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
            else
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
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();

                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null)
                        {
                            CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                            if (CoupleDetails.CoupleId > 0)
                            {
                                PatientInfo.Visibility = Visibility.Collapsed;
                                CoupleInfo.Visibility = Visibility.Visible;
                                //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                                //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                                GetHeightAndWeight(BizAction.CoupleDetails);
                                //added by priti
                                if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                                {
                                    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                    bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                                    imgPhoto13.Source = bmp;
                                    imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                                }
                                else
                                {
                                    imgP1.Visibility = System.Windows.Visibility.Visible;
                                }
                                if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                                {
                                    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                    bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                                    imgPhoto12.Source = bmp;
                                    imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                                }
                                else
                                {
                                    imgP2.Visibility = System.Windows.Visibility.Visible;
                                }
                                //FillSpermVitrification();
                            }
                            else
                            {
                                LoadPatientHeader();
                                #region Commented by Saily P on 260912 purpose, the form is applicable to donor patient
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                // new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.Show();
                                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                #endregion
                            }
                        }
                        else
                            LoadPatientHeader();
                        wait.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");                              
                    }
                    
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                if (!IsEdit)
                {
                    FillSpermVitrification();
                }
                else
                {
                   // fillThawingDetails();
                }
            }           
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
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        //FemalePatientDetails.BMI = BizAction.CoupleDetails.FemalePatient.BMI;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        //MalePatientDetails.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion
        
        private void FillSpermVitrification()
        {
            clsGetSpermVitrificationBizActionVO VitriDetails = new clsGetSpermVitrificationBizActionVO();
           
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId != 0)
                {
                    VitriDetails.CoupleID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                    VitriDetails.CoupleUintID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                    VitriDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    VitriDetails.IsEdit = IsEdit;
                    VitriDetails.IsDonor = false;
                }
                else if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
                {
                    VitriDetails.CoupleID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                    VitriDetails.CoupleUintID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                    VitriDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    VitriDetails.IsEdit = IsEdit;
                    VitriDetails.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    VitriDetails.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    VitriDetails.IsDonor = true;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        dgVitrificationDetilsGrid.ItemsSource = null;
                        dgThawingDetilsGrid.ItemsSource = null;

                        MasterListItem ThawPlan = new MasterListItem();
                        MasterListItem LabIncharge = new MasterListItem();

                        if (((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification != null)
                        {
                            dgVitrificationDetilsGrid.ItemsSource = ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.VitrificationDetails;

                            for (int i = 0; i < ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails.Count ; i++)
                            {
                                ThawPlan = null;
                                LabIncharge = null;

                                if (!string.IsNullOrEmpty(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlan))
                                {
                                    ThawPlan = SelectedPLan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlanId));
                                }
                                if (!string.IsNullOrEmpty(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeName))
                                {
                                    LabIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeId));
                                }
                             //   if(!string.IsNullOrEmpty(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeName))
                                if (ThawPlan != null)
                                {
                                    if (ThawPlan.ID > 0)
                                    {
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlan = ThawPlan.Description;
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlanId = ThawPlan.ID;
                                    }
                                    else
                                    {
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlan = "";
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlanId = ThawPlan.ID;
                                    }
                                }
                                if (LabIncharge != null)
                                {
                                    if (LabIncharge.ID > 0)
                                    {
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeName = LabIncharge.Description;
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeId = LabIncharge.ID;
                                    }
                                    else
                                    {
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeName = "";
                                        ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeId = LabIncharge.ID;
                                    }
                                }
                                ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PlanIdList = SelectedPLan;
                                ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].InchargeIdList = SelectedLabIncharge;

                                if (((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlanId > 0)
                                {
                                    ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedPlanId = SelectedPLan.FirstOrDefault(p => p.ID == ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].PostThawPlanId);
                                }
                                else
                                {
                                    ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedPlanId = SelectedPLan.FirstOrDefault(p => p.ID == 0);
                                }

                                if (((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeId > 0)
                                {
                                    ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].LabInchargeId);
                                }
                                else
                                {
                                    ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                                }
                                ThawDetails.Add(((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails[i]);
                            }
                            dgThawingDetilsGrid.ItemsSource = ThawDetails;
                            //foreach (var item in ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails)
                            //{
                            //    if(!string.IsNullOrEmpty(item.PostThawPlan))

                            //}
                            //if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange))
                            //{
                            //    CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange));
                            //}

                            //if (CS != null)
                            //{
                            //    if (CS.ID > 0)
                            //    {
                            //        ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = CS.Description;
                            //        ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
                            //    }
                            //    else
                            //    {
                            //        ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = "";
                            //        ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
                            //    }
                            //}
                            //dgThawingDetilsGrid.ItemsSource = ((clsGetSpermVitrificationBizActionVO)arg.Result).Vitrification.ThawingDetails;                           
                        }
                    }              
                };
                client.ProcessAsync(VitriDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            //clsGetSpermVitrificationBizActionVO VitriDetails = new clsGetSpermVitrificationBizActionVO();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //if (Validation())
            //{
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save Thawing Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        ImpressionWindow winImp = new ImpressionWindow();
                        winImp.Day = true;
                        winImp.Impression = Impression;
                        winImp.OnSaveClick += new RoutedEventHandler(winImp_OnSaveClick);
                        winImp.Show();
                    }
                };
                msgWin.Show();
            //}
        }

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            Impression = ObjImp.Impression;
            SaveThawing();
        }

        private void SaveThawing()
        {
            try
            {
                wait.Show();
                ObservableCollection<clsSpermVitrificationDetailsVO> newvit = VitriDetails;
                clsAddUpdateSpermThawingBizActionVO BizAction = new clsAddUpdateSpermThawingBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.Thawing = new List<clsSpermThawingDetailsVO>();
                BizAction.Impression = Impression;
                BizAction.ID = ID;
                BizAction.UintID = UnitID;
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
                BizAction.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
               // BizAction.Thawing.LabPerseonID = ((MasterListItem)cmbLabPerson.SelectedItem).ID;
             //   BizAction.Thawing.Date = dtVitrificationDate.SelectedDate.Value.Date;
               // BizAction.Thawing.Date = BizAction.Thawing.Date.Value.Add(txtTime.Value.Value.TimeOfDay);
                BizAction.Thawing = ((List<clsSpermThawingDetailsVO>)ThawDetails.ToList());

             
               
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Thawing Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Thawing Details Saved Successfully";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        LabDaysSummary LabSumm = new LabDaysSummary();
                        LabSumm.IsPatientExist = true;
                        ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
                        wait.Close();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion

            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        #region Validation
        //public Boolean Validation()
        //{
        //    #region Commented
        //    //if (dtVitrificationDate.SelectedDate == null)
        //    //{
        //    //    dtVitrificationDate.SetValidation("Please Select Thawing Date");
        //    //    dtVitrificationDate.RaiseValidationError();
        //    //    dtVitrificationDate.Focus();
        //    //    return false;
        //    //}
        //    //else if (txtTime.Value == null)
        //    //{
        //    //    dtVitrificationDate.ClearValidationError();
        //    //    txtTime.SetValidation("Please Select Thawing Time");
        //    //    txtTime.RaiseValidationError();
        //    //    txtTime.Focus();
        //    //    return false;
        //    //}
        //    //else if (cmbLabPerson.SelectedItem == null)
        //    //{
        //    //    dtVitrificationDate.ClearValidationError();
        //    //    txtTime.ClearValidationError();
        //    //    cmbLabPerson.TextBox.SetValidation("Please Select Lab Person");
        //    //    cmbLabPerson.TextBox.RaiseValidationError();
        //    //    cmbLabPerson.Focus();
        //    //    return false;
        //    //}
        //    //else if (ThawDetails.Count <= 0)
        //    //{
        //    //    dtVitrificationDate.ClearValidationError();
        //    //    txtTime.ClearValidationError();
        //    //    cmbLabPerson.TextBox.ClearValidationError();
        //    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
        //    //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //    //    msgW1.Show();
        //    //    return false;
        //    //}
        //    //else
        //    //{
        //    //    //dtVitrificationDate.ClearValidationError();
        //    //    //txtTime.ClearValidationError();
        //    //    cmbLabPerson.TextBox.ClearValidationError();
        //    //    return true;
        //    //}
        //    #endregion

        //    if (!string.IsNullOrEmpty(cmbLabPerson.Text.Trim()))
        //    {
        //        cmbLabPerson.ClearValidationError();
        //        return true;
        //    }
        //    else
        //    {
        //        cmbLabPerson.SetValidation("Please Select Lab Person.");
        //        cmbLabPerson.RaiseValidationError();
        //        return false;
        //    }
        //}


        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPlanForSperms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].PostThawPlanId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsSpermThawingDetailsVO ThawRow = (clsSpermThawingDetailsVO)e.Row.DataContext;
            if (ThawRow.IsFreezed == true)
                e.Row.IsEnabled = false;
            else
                e.Row.IsEnabled = true;
        }

        private void cmbLabIncharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].LabInchargeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }
    }
}
 