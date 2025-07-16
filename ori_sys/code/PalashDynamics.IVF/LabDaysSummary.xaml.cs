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
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.IVF
{
    public partial class LabDaysSummary : UserControl,IInitiateCIMS
    {

        #region Variables
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        public bool IsSaved = false;
        WaitIndicator wi = new WaitIndicator();
        long PatientID = 0;
        string Impression { get; set; }
        #endregion

        #region Properties
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
        public PagedSortableCollectionView<clsLabDaysSummaryVO> DataList { get; private set; }
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
            }
        }
        string msgTitle = "";
        string msgText = "";
        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
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
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;

            }
        }

        #endregion

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
        setupPage();
        }


        public LabDaysSummary()
        {
            InitializeComponent();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsLabDaysSummaryVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                }
               
                  if(PatientID>=0)
                {
                    try
                    {
                        //this.DataContext = new clsFemaleLabDay0VO();
                        wi.Show();                        
                        fillCoupleDetails();
                    }
                    catch (Exception ex) { }
                    finally { wi.Close(); }
                }
                IsPageLoded = true;
            }
            
        }

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
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
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    
                    //if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null)
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId >0)
                    {
                        PatientInfo.Visibility = Visibility.Collapsed;
                        CoupleInfo.Visibility = Visibility.Visible;
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        GetHeightAndWeight(BizAction.CoupleDetails);
                        setupPage();
                    }
                    else
                    {
                        LoadPatientHeader();
                        setupPage();
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        #region Get Patient EMR Details(Height and Weight)

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

        #endregion
          
        private void setupPage()
        {
            clsGetLabDaysSummaryBizActionVO objTempList = new clsGetLabDaysSummaryBizActionVO();
            objTempList.IsPagingEnabled = true;
            objTempList.MaximumRows = DataList.PageSize;
            objTempList.StartRowIndex = DataList.PageIndex * DataList.PageSize;

            objTempList.CoupleID = CoupleDetails.CoupleId;
            objTempList.CoupleUnitID = CoupleDetails.CoupleUnitId;
            objTempList.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetLabDaysSummaryBizActionVO result = ea.Result as clsGetLabDaysSummaryBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.LabDaysSummary != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.LabDaysSummary)
                        {
                            
                            DataList.Add(item);
                        }

                        dgSummaryList.ItemsSource = null;
                        dgSummaryList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = objTempList.MaximumRows;
                        dgDataPager.Source = DataList;
                    }
                }
                else
                {
                    msgText = "An Error Occured while filling the Email Template List";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(objTempList, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        #region Calculate BMI
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

        private void dgSummaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
            
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            switch (((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).FormID)
            {
                case IVFLabWorkForm.FemaleLabDay0 :
                    //FemaleLabDay0 win0 = new FemaleLabDay0(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    //MainForm.Visibility = Visibility.Collapsed;
                    //LinkedForm.Content = win0;
                    //LinkedForm.Visibility = Visibility.Visible;
                    
                    //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.FemaleLabDay1") as UIElement;
                    FemaleLabDay0 win0 = new FemaleLabDay0(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win0);
                    mElement.Text = " : " + "Lab Day 0";

                    break;
                case IVFLabWorkForm.FemaleLabDay1:
                    //FemaleLabDay1 win1 = new FemaleLabDay1(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                   
                    //MainForm.Visibility = Visibility.Collapsed;
                    //LinkedForm.Content = win1;
                    //LinkedForm.Visibility = Visibility.Visible;
                    //win1.Initiate("EDIT");
                                                          
                    FemaleLabDay1 win1 = new FemaleLabDay1(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win1);
                    mElement.Text = " : " + "Lab Day 1";
                                      
                    break;
                case IVFLabWorkForm.FemaleLabDay2:
                     FemaleLabDay2 win2 = new FemaleLabDay2(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                     ((IApplicationConfiguration)App.Current).OpenMainContent(win2);
                     mElement.Text = " : " + "Lab Day 2";
                                      
                    break;

                case IVFLabWorkForm.FemaleLabDay3:
                    FemaleLabDay3 win3 = new FemaleLabDay3(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win3);
                    mElement.Text = " : " + "Lab Day 3";
                    break;

                case IVFLabWorkForm.FemaleLabDay4:
                    FemaleLabDay4 win4 = new FemaleLabDay4(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win4);
               
                     mElement.Text = " : " + "Lab Day 4";

                    break;
                case IVFLabWorkForm.FemaleLabDay5:
                    FemaleLabDay5 win5 = new FemaleLabDay5(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win5);
                    
                   
                     mElement.Text = " : " + "Lab Day 5";

                    break;
                case IVFLabWorkForm.FemaleLabDay6:
                    FemaleLabDay6 win6 = new FemaleLabDay6(CoupleDetails, (clsLabDaysSummaryVO)dgSummaryList.SelectedItem);
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win6);
                    mElement.Text = " : " + "Lab Day 6";

                    break;

                case IVFLabWorkForm.Vitrification:
                    Vitrification VitWin = new Vitrification();
                    VitWin.IsEdit = true;
                    VitWin.IsPatientExist = true;
                    VitWin.Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
                    VitWin.ID=((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).OocyteID;
                    VitWin.UnitID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).UnitID;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(VitWin);
                    mElement.Text = " : " + "Vitrification";

                    break;

                case IVFLabWorkForm.OnlyVitrification:
                    OnlyVitrification VitonlyWin = new OnlyVitrification();
                    VitonlyWin.IsEdit = true;
                    VitonlyWin.IsPatientExist = true;
                    VitonlyWin.Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
                    VitonlyWin.ID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).OocyteID;
                    VitonlyWin.UnitID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).UnitID;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(VitonlyWin);
                    mElement.Text = " : " + "Only Vitrification";

                    break;
                case IVFLabWorkForm.Thawing:
                    Thawing ThawWin = new Thawing();
                    ThawWin.IsEdit = true;
                    ThawWin.IsPatientExist = true;
                    ThawWin.Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
                    ThawWin.ID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).OocyteID;
                    ThawWin.UnitID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).UnitID;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(ThawWin);
                    mElement.Text = " : " + "Thawing";

                    break;

                 case IVFLabWorkForm.ET:
                    EmbryoTransfer ET = new EmbryoTransfer();
                    ET.IsEdit = true;
                    ET.IsPatientExist = true;
                    ET.Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
                    ET.ID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).OocyteID;
                    ET.UnitID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).UnitID;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(ET);
                    mElement.Text = " : " + "ET";

                    break;

                 case IVFLabWorkForm.OnlyET:
                    OnlyET OnlyET = new OnlyET();
                    OnlyET.IsEdit = true;
                    OnlyET.IsPatientExist = true;
                    OnlyET.Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
                    OnlyET.ID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).OocyteID;
                    OnlyET.UnitID = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).UnitID;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(OnlyET);
                    mElement.Text = " : " + "Only ET";

                    break;

            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkFreeze_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hlbEditTemplate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HlbViewImpression_Click(object sender, RoutedEventArgs e)
        {
            ImpressionWindow winImp = new ImpressionWindow();
            winImp.Summary = true;
            string Impression = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).Impressions;
            winImp.Impression = Impression;
            winImp.Show();
        }

        // BY BHUSHAN . . . . 

        long CoupleID = 0;
        long CoupleUnitID = 0;
        long PlanTherapyId = 0;

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgSummaryList.SelectedItem != null)
            {

                CoupleID = CoupleDetails.CoupleId;
                CoupleUnitID = CoupleDetails.CoupleUnitId;

               // PlanTherapyId = ((clsLabDaysSummaryVO)dgSummaryList.SelectedItem).
                
                string msgTitle = "";
                string msgText = "Are You Sure \n You Want To Print ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinPrint_OnMessageBoxClosed);
                msgWin.Show();
            }
        }

        void msgWinPrint_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                string URL = "../Reports/IVF/ARTAndLabDaysReport.aspx?CoupleID=" + CoupleID + "&CoupleUnitID=" + CoupleUnitID;// +"&PlanTherapyId=" + PlanTherapyId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void dgSummaryList_LoadingRow(object sender, DataGridRowEventArgs e)
        {        
          
          
        }

        void Customerdatagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void PrintOnlyVitrification_Click(object sender, RoutedEventArgs e)
        {
        }

        private void dgSummaryList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {            
              
        }

    }
}
