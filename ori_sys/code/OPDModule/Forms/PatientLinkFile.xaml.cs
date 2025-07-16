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
using System.IO;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Windows.Media.Imaging;
using PalashDynamics.UserControls;

namespace OPDModule.Forms
{
    public partial class PatientLinkFile : UserControl,IInitiateCIMS
    {
        byte[] data;
        FileInfo fi;
        public string msgTitle;
        public string msgText;
        clsPatientLinkFileBizActionVO SelectedDetails { get; set; }
        ObservableCollection<clsPatientLinkFileBizActionVO> lstFile { get; set; }
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        string DocumentName = null;
        private SwivelAnimation objAnimation;
        public bool IsNew = false;
        public bool IsCancel = true;
        WaitIndicator Indicatior = null;

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
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        public PatientLinkFile()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }
        #region IInitiateCIMS Members
        private bool IsPatientExist = true;
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        //imgP1.Visibility = System.Windows.Visibility.Visible;
                        //imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        //imgP1.Visibility = System.Windows.Visibility.Visible;
                        //imgP2.Visibility = System.Windows.Visibility.Collapsed;
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                        //btnSearchCriteria.IsEnabled = false;
                    }


                    this.DataContext = new clsPatientVO()
                    {

                        GenderID = 0,
                        RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,


                    };


                    break;

                default:
                    break;
            }

        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //Initiate("NEW");
            if (IsPatientExist == true)
            {
                lstFile = new ObservableCollection<clsPatientLinkFileBizActionVO>();
                SetCommandButtonState("Load");
                RptRcdDate.SelectedDate = DateTime.Now;
                RptRcdTime.Value = DateTime.Now;

                this.DataContext = new clsPatientVO();
                fillCoupleDetails();

                //GetPatient();

                //FillGender();
                //FillDoctor();
            }
            else
            {
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }
        #region Couple Details


        private void fillCoupleDetails()
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
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {
                        PatientInfo.Visibility = Visibility.Collapsed;
                        CoupleInfo.Visibility = Visibility.Visible;

                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        //if (CoupleDetails.CoupleId == 0)
                        //{                    
                        //    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //    //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Upload Result. This Functionality is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    //msgW1.Show();

                        //    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //}
                        //else
                        //{                            
                        CoupleId = CoupleDetails.CoupleId; //((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId;
                        CoupleUnitId = CoupleDetails.CoupleUnitId; //((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleUnitId;
                        GetHeightAndWeight(BizAction.CoupleDetails);
                        //  }
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
                    }
                    else
                    {
                        LoadPatientHeader();
                        GetPatientLinkFile();
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Upload Result. This Functionality is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
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
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();

                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();





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
        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
                {

                    return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }

        #endregion
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            DocumentName = null;

            if (openDialog.ShowDialog() == true)
            {
                TxtReportPath.Text = openDialog.File.Name;

                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = openDialog.File;
                        DocumentName = txtDocumentName.Text;

                    }
                }
                catch (Exception ex)
                {

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                }
            }
        }
        private void GetPatientLinkFile()
        {
            clsGetPatientLinkFileBizActionVO BizAction = new clsGetPatientLinkFileBizActionVO();
            BizAction.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {

                        foreach (var item in ((clsGetPatientLinkFileBizActionVO)arg.Result).PatientDetails)
                        {
                            lstFile.Add(item);
                        }
                        dgReport.ItemsSource = null;
                        dgReport.ItemsSource = lstFile;
                    }
                }
            };
            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try 
            {
                if (lstFile != null && lstFile.Count > 0)
                {
                    clsAddPatientLinkFileBizActionVO BizAction = new clsAddPatientLinkFileBizActionVO();
                    BizAction.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
                    BizAction.PatientDetails = lstFile.ToList();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Document is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            }
                        }
                    };                    
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
            }
               
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one file is required.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show(); 
                Indicatior.Close();
            }                
            } 
            catch (Exception ex)
            {
                Indicatior.Close();
            }
            Indicatior.Close();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

            try
                    {
                        if (IsCancel == true)
                        {
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        }
                        else
                        {
                            objAnimation.Invoke(RotationType.Backward);
                            IsCancel = true;
                   
                        }
                        SetCommandButtonState("Cancel");
                    }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
          
            bool valid = CheckValidations();
            try
            {  
                Indicatior.Show();
                if (valid)
                {
                    clsPatientLinkFileBizActionVO ObjFile = new clsPatientLinkFileBizActionVO();
                    ObjFile.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    ObjFile.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    ObjFile.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                    ObjFile.Time = RptRcdTime.Value;
                    ObjFile.Date = RptRcdDate.SelectedDate;
                    ObjFile.SourceURL = fi.Extension;
                    ObjFile.Report = data;
                    ObjFile.Remarks = TxtRemarks.Text;
                    ObjFile.DocumentName = txtDocumentName.Text;
                    ObjFile.Path = TxtReportPath.Text;
                    lstFile.Add(ObjFile);

                    dgReport.ItemsSource = lstFile;
                    dgReport.UpdateLayout();
                    dgReport.Focus();
                    ClearData();                   
                }               
            }
            catch
            {
                Indicatior.Close();
            } 
            
            Indicatior.Close(); 
        }

        private bool checkModVal()
        {
            bool result = true;
            try
            {
                //if (TxtReportPath.Text == "" || TxtReportPath.Text == null)
                //{
                //    TxtReportPath.SetValidation("Document Path is required.");
                //    TxtReportPath.RaiseValidationError();
                //    TxtReportPath.Focus();
                //    result = false;
                //}
                //else
                //    TxtReportPath.ClearValidationError();

                if (txtDocumentName.Text == "" || txtDocumentName.Text == null)
                {
                    txtDocumentName.SetValidation("Document Name is required.");
                    txtDocumentName.RaiseValidationError();
                    txtDocumentName.Focus();
                    result = false;
                }
                else
                    txtDocumentName.ClearValidationError();


                if (RptRcdTime.Value == null)
                {
                    RptRcdTime.SetValidation("Document Received Time is required.");
                    RptRcdTime.RaiseValidationError();
                    RptRcdTime.Focus();
                    result = false;
                }
                else
                    RptRcdTime.ClearValidationError();


                if (RptRcdDate.SelectedDate == null)
                {
                    RptRcdDate.SetValidation("Document Received Date is required.");
                    RptRcdDate.RaiseValidationError();
                    RptRcdDate.Focus();
                    result = false;
                }
                else
                    RptRcdDate.ClearValidationError();


            }
            catch (Exception ex)
            {

            }
            return result;



        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {

                if (TxtReportPath.Text == "" || TxtReportPath.Text == null)
                {
                    TxtReportPath.SetValidation("Document Path is required.");
                    TxtReportPath.RaiseValidationError();
                    TxtReportPath.Focus();
                    result = false;
                }
                else
                    TxtReportPath.ClearValidationError();

                if (txtDocumentName.Text == "" || txtDocumentName.Text == null)
                {
                    txtDocumentName.SetValidation("Document Name is required.");
                    txtDocumentName.RaiseValidationError();
                    txtDocumentName.Focus();
                    result = false;
                }
                else
                    txtDocumentName.ClearValidationError();


                if (RptRcdTime.Value == null)
                {
                    RptRcdTime.SetValidation("Document Received Time is required.");
                    RptRcdTime.RaiseValidationError();
                    RptRcdTime.Focus();
                    result = false;
                }
                else
                    RptRcdTime.ClearValidationError();


                if (RptRcdDate.SelectedDate == null)
                {
                    RptRcdDate.SetValidation("Document Received Date is required.");
                    RptRcdDate.RaiseValidationError();
                    RptRcdDate.Focus();
                    result = false;
                }
                else
                    RptRcdDate.ClearValidationError();


            }
            catch (Exception ex)
            {

            }
            return result;
        }
        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {

            bool valid = checkModVal();

            if (valid)
            {
                int var = dgReport.SelectedIndex;
                if (lstFile.Count > 0)
                    lstFile.RemoveAt(dgReport.SelectedIndex);

                lstFile.Insert(var, new clsPatientLinkFileBizActionVO
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                    PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                    VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID,

                    Time = RptRcdTime.Value,
                    Date = RptRcdDate.SelectedDate,
                    //SourceURL = fi.Extension,
                    //Report = data,
                    Remarks = TxtRemarks.Text,
                    DocumentName = txtDocumentName.Text
                }
                );
                dgReport.ItemsSource = lstFile;
                dgReport.UpdateLayout();
                dgReport.Focus();
                ClearData();
            }
        }

        private void ClearData()
        {
            TxtReportPath.Text = "";
            TxtRemarks.Text = "";
            txtDocumentName.Text = "";

        }
        private void cmdDeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgReport.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Report ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        lstFile.RemoveAt(dgReport.SelectedIndex);
                        dgReport.ItemsSource = lstFile;
                        dgReport.Focus();
                        dgReport.UpdateLayout();
                        dgReport.SelectedIndex = lstFile.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void hlbEditAttachment_Click(object sender, RoutedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
            RptRcdDate.IsEnabled = true;
            RptRcdTime.IsEnabled = true;
            TxtReportPath.IsEnabled = false;
            RptRcdTime.Value = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Time;
            RptRcdDate.SelectedDate = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Date;
            TxtRemarks.Text = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Remarks;
            txtDocumentName.Text = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).DocumentName;
            TxtReportPath.Text = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Path;
        }

        private void ViewDetailsLink_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).DocumentName))
            {
                if (((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL });
                            AttachedFileNameList.Add(((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL, ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report);
                }
            }
        }

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearData();
               
                SetCommandButtonState("New");
                IsNew = true;
                objAnimation.Invoke(RotationType.Forward);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    CmdSave.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;


                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    break;

               
                case "View":
                    CmdSave.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {

        }
    }
}
