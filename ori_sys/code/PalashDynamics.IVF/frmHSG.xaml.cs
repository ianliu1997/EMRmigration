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
using CIMS.Forms;
using System.Windows.Media.Imaging;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Media.Imaging;
using PalashDynamics.UserControls;
using System.Xml.Linq;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using MessageBoxControl;
using System.IO;

namespace PalashDynamics.IVF
{
    public partial class frmHSG : UserControl, IInitiateCIMS
    {
        public frmHSG()
        {
            InitializeComponent();
           // this.DataContext = new clsHSGVO();
           
        }
     

        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        WaitIndicator wi = new WaitIndicator();
        long PatientID = 0;
        long HSGID = 0;
        long HSGUnitID = 0;
        bool IsUpdate = false;
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        string FileNamePreview="";
        byte[] AttachedFileContents;
        string AttachedFileName;
         long ID { get; set; }
         long UnitID { get; set; }
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        public List<clsHSGVO> DataList1 { get; set; }
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
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        ListBox lstFUBox;
        public bool IsCancel = true;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        private List<FileUpload> _FileUpLoadList = new List<FileUpload>();
        public List<FileUpload> FileUpLoadList
        {
            get
            {
                return _FileUpLoadList;
            }
            set
            {
                _FileUpLoadList = value;
            }
        }
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;


            }
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Print.IsEnabled = false;
            CmdAddDocument.IsEnabled = false;
            if (!IsPageLoded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    dtHSGDate.SelectedDate = DateTime.Now.Date.Date;
                    txtHSGTime.Value = DateTime.Now;
                }

                if (PatientID >= 0)
                {
                    if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        try
                        {
                            PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                            wi.Show();
                            fillCoupleDetails();
                            FetchData(PatientID);
                           //LoadFURepeaterControl();
                            this.DataContext = new clsHSGVO();

                        }
                        catch (Exception ex)
                        { throw ex; }
                        finally
                        {
                            wi.Close();
                        }
                    }
                    IsPageLoded = true;
                }
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


   

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {

        }
       

        private void Print_Click(object sender, RoutedEventArgs e)
        {
           
            string msgTitle = "";
            string msgText = "Are You Sure \n You Want To Print ?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinPrint_OnMessageBoxClosed);

            msgWin.Show();

        }
        void msgWinPrint_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                string URL = "../Reports/IVF/HSG.aspx?HSGID=" + HSGID + "&HSGUnitID=" + HSGUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        public bool SaveFlag = false;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreeze.IsChecked == false)
            {
                string msgTitle = "";
                string msgText = "Do you want to freeze the Details ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();

            }
            else
            {
                Save();
                SaveFlag = true;
            }
        }

        private void Save()
        {

                string msgTitle = "";
                string msgText = "Are You Sure \n You Want To SAVE  ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinSAVE_OnMessageBoxClosed);

                msgWin.Show();
            }
       
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                chkFreeze.IsChecked = true;
                Save();
                /* Added By Sudhir Patil on  30/12/2013 */
                SaveFlag = true;
                if (SaveFlag == true)
                {
                    Print.IsEnabled = true;
                }
            }            
            else
            {
                Save();                
                SaveFlag = true;
            }
        }

        public void CheckValidations()
        {
            //if (txtHSGTime.Format != txtHSGTime.Format.Equals(short));
            //{
            //    txtHSGTime.SetValidation("Please, Select Proper format of Time");
            //    txtHSGTime.RaiseValidationError();
            //    txtHSGTime.Focus();
            //}
        }
        /* Added By Sudhir Patil on  30/12/2013 */
        public void ClearUI()
        {
            dtHSGDate.SelectedDate = DateTime.Now.Date.Date;
            txtHSGTime.Value = DateTime.Now;
            txtUterusRemark.Text = "";
            txtotherPatho.Text = "";
            txtTitle.Text = "";
            txtFileName.Text = "";
            txtUterus.Text = "";
            if (rdoCavityYes.IsChecked == true)
                rdoCavityYes.IsChecked = false;
            if (rdoCavityNo.IsChecked == true)
                rdoCavityNo.IsChecked = false;
            if (rdbHydrosalplnxYes.IsChecked == true)
                rdbHydrosalplnxYes.IsChecked = false;
            if(rdbHydrosalplnxNo.IsChecked == true)
                rdbHydrosalplnxNo.IsChecked = false;
            if(ChkPatent.IsChecked == true)
                ChkPatent.IsChecked = false;
            if(chkBlocked.IsChecked == true)
                chkBlocked.IsChecked = false;
            if(chkCornual.IsChecked == true)
                chkCornual.IsChecked = false;
            if(chkIsthmic.IsChecked == true)
                chkIsthmic.IsChecked = false;
            if (chkAmpullary.IsChecked == true)
                chkAmpullary.IsChecked = false;
            if (chkfimbrial.IsChecked == true)
                chkfimbrial.IsChecked = false;
            if (chkFreeze.IsChecked == true)
            {
                chkFreeze.IsChecked = false;
            }
        }
        //  List<PalashDynamics.ValueObjects.Patient.clsTESEDetailsVO> TeseListNew = new List<PalashDynamics.ValueObjects.Patient.clsTESEDetailsVO>();
        void msgWinSAVE_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                    try
                    {            
                        clsAddUpdateHSGBizActionVO BizAction = new clsAddUpdateHSGBizActionVO();               
                        BizAction.HSGDetails = (clsHSGVO)this.DataContext;
                        BizAction.HSGDetails.CoupleDetail = CoupleDetails;
                        BizAction.HSGDetails.Uterus = txtUterus.Text.Trim();                   
                        BizAction.HSGDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        if (rdoCavityYes.IsChecked == true)
                            BizAction.HSGDetails.cavity = true;
                        else
                            BizAction.HSGDetails.cavity = false;
               
                        if (ChkPatent.IsChecked == true)
                            BizAction.HSGDetails.Patent_Tube = true;
                        else
                            BizAction.HSGDetails.Patent_Tube = false;
                        if (chkBlocked.IsChecked == true)
                            BizAction.HSGDetails.Blocked_tube = true;
                        else
                            BizAction.HSGDetails.Blocked_tube = false;
                        if (chkIsthmic.IsChecked == true)
                            BizAction.HSGDetails.Isthmic_Blockage = true;
                        else
                            BizAction.HSGDetails.Isthmic_Blockage = false;

                        if (chkCornual.IsChecked == true)
                            BizAction.HSGDetails.Cornul_blockage = true;
                        else
                            BizAction.HSGDetails.Cornul_blockage = false;
                        if (chkAmpullary.IsChecked == true)
                            BizAction.HSGDetails.Ampullary_Blockage = true;
                        else
                            BizAction.HSGDetails.Ampullary_Blockage = false;
                        if (chkfimbrial.IsChecked == true)
                            BizAction.HSGDetails.Fimbrial_Blockage = true;
                        else
                            BizAction.HSGDetails.Fimbrial_Blockage = false;

                        BizAction.HSGDetails.Remark = txtUterusRemark.Text.Trim();
                        if (rdbHydrosalplnxYes.IsChecked == true)
                            BizAction.HSGDetails.Hydrosalplnx = true;
                        else
                            BizAction.HSGDetails.Hydrosalplnx = false;
                        if (chkFreeze.IsChecked == true)
                            BizAction.HSGDetails.IsFreezed = true;
                        else
                            BizAction.HSGDetails.IsFreezed = false;
                        BizAction.HSGDetails.Other_Patho = txtotherPatho.Text.Trim();


                        BizAction.HSGDetails.AttachedFileContent = AttachedFileContents;
                        if (AttachedFileName == null)
                            BizAction.HSGDetails.AttachedFileName = txtFileName.Text.Trim();
                        else
                            BizAction.HSGDetails.AttachedFileName = AttachedFileName;
                        BizAction.HSGDetails.Title = txtTitle.Text.Trim();
                       // BizAction.HSGDetails.Description = txtDescription.Text.Trim();
               

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {                 
                            if (arg.Error == null && arg.Result != null)
                            {
                        
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "HSG Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                               // this.DataContext = new clsHSGVO();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Video Size is More.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }

                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                     client.CloseAsync();

                     ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                     ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                     ((IApplicationConfiguration)App.Current).FillMenu("Clinical");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                /* Added By Sudhir Patil on  30/12/2013 */
                ClearUI();
            }
     
        }

        public void DisableAll()
        {
            dtHSGDate.IsEnabled = false;
            txtHSGTime.IsEnabled = false;
            txtUterusRemark.IsEnabled = false;
            txtotherPatho.IsEnabled = false;
            txtTitle.IsEnabled = false;
            txtFileName.IsEnabled = false;
            txtUterus.IsEnabled = false;            
            rdoCavityYes.IsEnabled = false;
            rdoCavityNo.IsEnabled = false;
            rdbHydrosalplnxYes.IsEnabled = false;
            rdbHydrosalplnxNo.IsEnabled = false;
            ChkPatent.IsEnabled = false;
            chkBlocked.IsEnabled = false;
            chkCornual.IsEnabled = false;
            chkIsthmic.IsEnabled = false;
            chkAmpullary.IsEnabled = false;
            chkfimbrial.IsEnabled = false;
            chkFreeze.IsEnabled = false;
            CmdBrowse.IsEnabled = false;
            cmdCamera.IsEnabled = false;
        }

        private void FetchData(long patientID)
        {
            clsGetHSGBizActionVO BizAction = new clsGetHSGBizActionVO();
            BizAction.HSGDetails = new clsHSGVO();
            BizAction.HSGDetails.PatientID = patientID; 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetHSGBizActionVO result = arg.Result as clsGetHSGBizActionVO;
                    DataList1 = new List<clsHSGVO>();
                   
                    if (((clsGetHSGBizActionVO)arg.Result).HSGDetails != null)
                        {              
                        this.DataContext = ((clsGetHSGBizActionVO)arg.Result).HSGDetails;
                    

                    if (((clsGetHSGBizActionVO)arg.Result).HSGDetails.cavity == true)
                        rdoCavityYes.IsChecked = true;
                    else
                        rdoCavityNo.IsChecked = true;
                    if (((clsGetHSGBizActionVO)arg.Result).HSGDetails.Hydrosalplnx == true)
                        rdbHydrosalplnxYes.IsChecked = true;
                    else
                        rdbHydrosalplnxNo.IsChecked = true;

                    HSGID = ((clsGetHSGBizActionVO)arg.Result).HSGDetails.ID;
                    HSGUnitID = ((clsGetHSGBizActionVO)arg.Result).HSGDetails.UnitID;
                        FileNamePreview=((clsGetHSGBizActionVO)arg.Result).HSGDetails.AttachedFileName;
                        AttachedFileContents = ((clsGetHSGBizActionVO)arg.Result).HSGDetails.AttachedFileContent;
                        if (((clsGetHSGBizActionVO)arg.Result).HSGDetails.IsFreezed == true)
                        {
                            cmdSave.IsEnabled = false;                            
                            //HSG.IsEnabled = false;                            
                            DisableAll();
                            Print.IsEnabled = true;
                        }
                    }
                }          

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
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
                   // SetupPage();
                }
                SetCommandButtonState("Cancel");
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdCamera_Click(object sender, RoutedEventArgs e)
        {
            PhotoWindow phWin = new PhotoWindow();
            if (this.DataContext != null)
                phWin.MyPhoto = ((clsHSGVO)this.DataContext).Image;
            phWin.OnSaveButton_Click += new RoutedEventHandler(phWinS_OnSaveButton_Click);
            phWin.Show();
        }

        void phWinS_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                ((clsHSGVO)this.DataContext).Image = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                  //  cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                   // CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                   // CmdSave.IsEnabled = true;
                   // cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    //CmdNew.IsEnabled = true;
                   // cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;


                case "Cancel":
                    //CmdNew.IsEnabled = true;
                    //cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

               
                case "View":
                   // cmdSave.IsEnabled = false;
                   // CmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

             
                default:
                    break;
            }
        }
        private void PageDisplay() 
        {
            txtHSGTime.IsEnabled = false;
            dtHSGDate.IsEnabled = false;
            txtHSGTime.IsEnabled = false;
            txtUterus.IsEnabled = false;
            rdoCavityYes.IsEnabled = false;
            rdoCavityNo.IsEnabled = false;
            ChkPatent.IsEnabled = false;
            chkBlocked.IsEnabled = false;
            txtUterusRemark.IsEnabled = false;
            chkIsthmic.IsEnabled = false;
            chkCornual.IsEnabled = false;
            chkAmpullary.IsEnabled = false;
            chkfimbrial.IsEnabled = false;
            rdbHydrosalplnxYes.IsEnabled = false;
            rdbHydrosalplnxNo.IsEnabled = false;
            chkFreeze.IsEnabled = false;
            txtotherPatho.IsEnabled = false;
            txtTitle.IsEnabled = false;
            CmdBrowse.IsEnabled = false;
            CmdAddDocument.IsEnabled = false;
           // cmdSave.IsEnabled = false;

        }

        //#region Upload File
        //private void LoadFURepeaterControl()
        //{

        //    lstFUBox = new ListBox();
        //    if (FileUpLoadList == null || FileUpLoadList.Count == 0)
        //    {
        //        FileUpLoadList = new List<FileUpload>();
        //        FileUpLoadList.Add(new FileUpload());
        //    }

        //    lstFUBox.DataContext = FileUpLoadList;


        //    if (FileUpLoadList != null)
        //    {
        //        for (int i = 0; i < FileUpLoadList.Count; i++)
        //        {
        //            FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
        //            FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
        //            FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
        //            FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);

        //            FileUpLoadList[i].Index = i;
        //            FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

        //            FUrci.DataContext = FileUpLoadList[i];
        //            lstFUBox.Items.Add(FUrci);
        //        }
        //    }
        //    Grid.SetRow(lstFUBox, 0);
        //    Grid.SetColumn(lstFUBox, 0);
        //    GridUploadFile.Children.Add(lstFUBox);
        //}

        //void FUrci_OnViewClick(object sender, RoutedEventArgs e)
        //{
        //    if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != null && ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != "")
        //    {
        //        if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data != null)
        //        {
        //            string FullFile = "ET" + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName;

        //            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
        //            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
        //            client.GlobalUploadFileCompleted += (s, args) =>
        //            {
        //                if (args.Error == null)
        //                {
        //                    HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
        //                    AttachedFileNameList.Add(FullFile);
        //                }
        //            };
        //            client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    else
        //    {
        //        MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File Is Not Uploaded. Please Upload The File Then Click On Preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //        mgbx.Show();
        //    }
        //}
       

        //void FUrci_OnBrowseClick(object sender, RoutedEventArgs e)
        //{

        //    OpenFileDialog openDialog = new OpenFileDialog();
        //    if (openDialog.ShowDialog() == true)
        //    {
        //        AttachedFileName = openDialog.File.Name;

        //        ((TextBox)((Grid)((Button)sender).Parent).FindName("FileName")).Text = openDialog.File.Name;
        //        //txtFileName.Text = openDialog.File.Name;
        //        try
        //        {
        //            using (Stream stream = openDialog.File.OpenRead())
        //            {
        //                AttachedFileContents = new byte[stream.Length];
        //                stream.Read(AttachedFileContents, 0, (int)stream.Length);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string msgText = "Error while reading file.";

        //            MessageBoxControl.MessageBoxChildWindow msgWindow =
        //                new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgWindow.Show();
        //        }
        //    }
        //}

        //void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        //{
        //    if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
        //    {
        //        FileUpLoadList.RemoveAt(((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index);
        //    }
        //    if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
        //    {
        //        if (FileUpLoadList.Where(Items => Items.FileName == null).Any() == true)
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW13 =
        //                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW13.Show();


        //        }
        //        else
        //        {
        //            FileUpLoadList.Add(new ValueObjects.IVFPlanTherapy.FileUpload());
        //        }
        //    }
        //    lstFUBox.Items.Clear();
        //    for (int i = 0; i < FileUpLoadList.Count; i++)
        //    {
        //        FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
        //        FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
        //        FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
        //        FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);


        //        FileUpLoadList[i].Index = i;
        //        FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

        //        FUrci.DataContext = FileUpLoadList[i];
        //        lstFUBox.Items.Add(FUrci);
        //    }
        //}
        //#endregion

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            if (txtFileName.Text != "")
            {
                txtFileName.IsReadOnly = true;
                CmdAddDocument.IsEnabled = true;
            }
        }

        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            //else if (txtDescription.Text.Length == 0)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();

            //}
            else if (txtFileName.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Browse File", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Are you Sure you want to save The Document";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Document Uploaded Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();

                        //txtTitle.Text = "";
                        //txtDescription.Text = "";
                        //txtFileName.Text = "";
                    }
                    //else
                    //{
                    //    txtTitle.Text = "";
                    //    txtDescription.Text = "";
                    //    txtFileName.Text = "";
                    //}
                };
                msgWin.Show();
            }

        }

        private void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void hybPreview_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFileName.Text))
            {
                if (AttachedFileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            //HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FileNamePreview });                            
                            AttachedFileNameList.Add(FileNamePreview);
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FileNamePreview });
                            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, FileNamePreview), "_blank");
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FileNamePreview, AttachedFileContents);
                }
            }
        }
    }
}
