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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.IVF
{
    public partial class frmClinicalSummary : UserControl,IInitiateCIMS
    {
     
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;


            }
        }

        #endregion

        #region Properties
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        string msgTitle = "";
        string msgText = "";
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        WaitIndicator wi = new WaitIndicator();
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
        #endregion

        public frmClinicalSummary()
        {
            InitializeComponent();
        }

        private void chkIsFreezed_Click(object sender, RoutedEventArgs e)
        {

            {
                if (((clsClinicalEventsVO)grdSummary.SelectedItem).Status == true && ((clsClinicalEventsVO)grdSummary.SelectedItem).Date == null)
                {
                    string msgTitle = "";
                    string msgText = "Please Select Date";

                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    ((clsClinicalEventsVO)grdSummary.SelectedItem).Status = false;
                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.OK)
                        {
                            SetupPage();
                        }
                    };
                    msgWin.Show();

                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Save ?";

                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed1);

                    msgWin.Show();
                }
            }
        }
        void msgWin_OnMessageBoxClosed1(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (grdSummary.SelectedItem != null)
                {
                    clsAddClinicalSummaryBizActionVO BizActionObj = new clsAddClinicalSummaryBizActionVO();
                    BizActionObj.Details = new clsClinicalSummaryVO();
                    BizActionObj.Details.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizActionObj.Details.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                    BizActionObj.Details.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                    BizActionObj.Details.SummaryID = ((clsClinicalEventsVO)grdSummary.SelectedItem).EventID;
                    BizActionObj.Details.Status = ((clsClinicalEventsVO)grdSummary.SelectedItem).Status;
                    if (((clsClinicalEventsVO)grdSummary.SelectedItem).Status == false)
                        BizActionObj.Details.Date = null;
                    else
                        BizActionObj.Details.Date = ((clsClinicalEventsVO)grdSummary.SelectedItem).Date;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            msgText = "Record Saved successfully !";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            SetupPage();
                        }
                    };
                    client.ProcessAsync(BizActionObj, new clsUserVO());
                    client.CloseAsync();
                }
            }
            else
            {
                ((clsClinicalEventsVO)grdSummary.SelectedItem).Status = false;
                ((clsClinicalEventsVO)grdSummary.SelectedItem).Date = null;
                SetupPage();
            }
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wi.Show();
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }

                if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    fillCoupleDetails();
                    FillDepartments();
                    //SetupPage();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                wi.Close();
            }
        }
        #region Couple Details

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
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
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

        private void FillDepartments()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_IVF_ClinicalSummary;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        List<MasterListItem> uList = new List<MasterListItem>();
                        uList = ((clsGetMasterListBizActionVO)ea.Result).MasterList;
                        uList = uList.OrderBy(q => q.ID).ToList();
                        grdSummary.ItemsSource = null;

                        List<clsClinicalEventsVO> userEventList = new List<clsClinicalEventsVO>();

                        foreach (var item in uList)
                        {
                            userEventList.Add(new clsClinicalEventsVO() { EventID = item.ID, EventName = item.Description, Status = false });
                        }

                        grdSummary.ItemsSource = userEventList;

                        SetupPage();

                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;

            }
            catch (Exception ex)
            {

            }
        }

        private void SetupPage()
        {
            try
            {
                wi.Show();

                clsGetClinicalSummaryBizActionVO bizActionVO = new clsGetClinicalSummaryBizActionVO();
                bizActionVO.Details = new clsClinicalSummaryVO();
                bizActionVO.Details.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                bizActionVO.Details.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                bizActionVO.DetailsList = new List<clsClinicalSummaryVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.DetailsList = (((clsGetClinicalSummaryBizActionVO)args.Result).DetailsList);

                        //foreach (clsClinicalSummaryVO item in bizActionVO.DetailsList)
                        //{
                        //    MasterList.Add(item);

                        //}

                        List<clsClinicalEventsVO> NewDetailsList = (List<clsClinicalEventsVO>)grdSummary.ItemsSource;
                        foreach (var item1 in bizActionVO.DetailsList)
                        {
                            foreach (var item in NewDetailsList)
                            {
                                if (item.EventID == item1.SummaryID)
                                {
                                    item.Status = item1.Status;
                                    item.Date = item1.Date;
                                }
                            }
                        }



                        grdSummary.ItemsSource = null;
                        grdSummary.ItemsSource = NewDetailsList;

                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                wi.Close();
            }
        }

        private void chkIsFreezed_Checked(object sender, RoutedEventArgs e)
        {


        }
    }
}
