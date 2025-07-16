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
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
namespace PalashDynamics.IVF
{
    public partial class MaleHistory : UserControl,IInitiateCIMS
    {
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
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
        public bool isModify = false;
        long ID { get; set; }
        public long SelectedRecord;
        
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
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

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        #region Paging

        public PagedSortableCollectionView<clsMaleHistoryVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
      
        #endregion

        public MaleHistory()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<clsMaleHistoryVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = new clsMaleHistoryVO();
            this.Loaded += new RoutedEventHandler(MaleHistory_Loaded);
        }

        private void MaleHistory_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
               // ((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");

            }
            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                this.DataContext = new clsMaleHistoryVO();
                fillCoupleDetails();
                FetchData();
                SetCommandButtonState("New");

                //GetMaleHistory();
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save the Male History?";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            msgW.Show();
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                if (ID == 0)
                {
                    Save();
                }
                else
                {
                    Update();
                }
        }
               
        private void Save()
        {
            clsAddMaleHistoryBizActionVO BizAction = new clsAddMaleHistoryBizActionVO();
            BizAction.HistoryDetails = (clsMaleHistoryVO)this.DataContext;
            BizAction.HistoryDetails.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            
            if (rdbSmokingHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.SmokingHabitual = true;
            else
                BizAction.HistoryDetails.SmokingHabitual = false;
            
            if (rdbAlcoholHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.AlcoholHabitual = true;
            else
                BizAction.HistoryDetails.AlcoholHabitual = false;
            
            if (rdbOtherHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.OtherHabitual = true;
            else
                BizAction.HistoryDetails.OtherHabitual = false;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    ClearFormData();
                    FetchData();
                    SetCommandButtonState("New");
                    objAnimation.Invoke(RotationType.Backward);

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Male History added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    this.DataContext = new clsMaleHistoryVO();
                    rdbSmokingHabitualYes.IsChecked = false;
                    rdbSmokingHabitualNo.IsChecked = false;
                    rdbAlcoholHabitualYes.IsChecked = false;
                    rdbAlcoholHabitualNo.IsChecked = false;
                    rdbOtherHabitualYes.IsChecked = false;
                    rdbOtherHabitualNo.IsChecked = false;

                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ClearFormData();
            FetchData();
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }

        #region FillCouple Details

        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
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
                    CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (CoupleDetails.CoupleId == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create History, History is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    { 
                        GetHeightAndWeight(BizAction.CoupleDetails);

                        
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



        #endregion

        //private void GetMaleHistory()
        //{
        //    clsGetMaleHistoryBizActionVO BizAction = new clsGetMaleHistoryBizActionVO();
        //    BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //     Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (((clsGetMaleHistoryBizActionVO)arg.Result).Details != null)
        //            {
        //                BizAction.Details = ((clsGetMaleHistoryBizActionVO)arg.Result).Details;
        //                if (BizAction.Details != null)
        //                {
        //                    this.DataContext = BizAction.Details;

        //                    ID = BizAction.Details.ID;

        //                    if (BizAction.Details.SmokingHabitual == true)
        //                        rdbSmokingHabitualYes.IsChecked = true;
        //                    else
        //                        rdbSmokingHabitualNo.IsChecked = true;

        //                    if (BizAction.Details.AlcoholHabitual == true)
        //                        rdbAlcoholHabitualYes.IsChecked = true;
        //                    else
        //                        rdbAlcoholHabitualNo.IsChecked = true;

        //                    if (BizAction.Details.OtherHabitual == true)
        //                        rdbOtherHabitualYes.IsChecked = true;
        //                    else
        //                        rdbOtherHabitualNo.IsChecked = true;
                            
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                      new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW1.Show();

        //        }
           
        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        private void FetchData()
        {
            clsGetMaleHistoryBizActionVO BizAction = new clsGetMaleHistoryBizActionVO();
            BizAction.Details = new List<clsMaleHistoryVO>();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetMaleHistoryBizActionVO)args.Result).Details != null)
                    {
                        clsGetMaleHistoryBizActionVO result = args.Result as clsGetMaleHistoryBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.Details != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.Details)
                            {
                                DataList.Add(item);
                            }
                            dgHistory.ItemsSource = null;
                            dgHistory.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        private void Update()
        {
            clsAddMaleHistoryBizActionVO BizAction = new clsAddMaleHistoryBizActionVO();
            BizAction.HistoryDetails = (clsMaleHistoryVO)this.DataContext;
            BizAction.HistoryDetails.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;

            if (rdbSmokingHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.SmokingHabitual = true;
            else
                BizAction.HistoryDetails.SmokingHabitual = false;

            if (rdbAlcoholHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.AlcoholHabitual = true;
            else
                BizAction.HistoryDetails.AlcoholHabitual = false;

            if (rdbOtherHabitualYes.IsChecked == true)
                BizAction.HistoryDetails.OtherHabitual = true;
            else
                BizAction.HistoryDetails.OtherHabitual = false;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Male History Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    this.DataContext = new clsMaleHistoryVO();
                    rdbSmokingHabitualYes.IsChecked = false;
                    rdbSmokingHabitualNo.IsChecked = false;
                    rdbAlcoholHabitualYes.IsChecked = false;
                    rdbAlcoholHabitualNo.IsChecked = false;
                    rdbOtherHabitualYes.IsChecked = false;
                    rdbOtherHabitualNo.IsChecked = false;
                    ID = 0;

                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void chkExposertoheat_Click(object sender, RoutedEventArgs e)
        {
            if (chkExposertoheat.IsChecked == true)
            {
                txtExposerToHeatDetails.Visibility = Visibility.Visible;
                lblExposerDetails.Visibility = Visibility.Visible;
            }
            else
            {
                txtExposerToHeatDetails.Visibility = Visibility.Collapsed;
                lblExposerDetails.Visibility = Visibility.Collapsed;
            }
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void chkSmoking_Click(object sender, RoutedEventArgs e)
        {
            if (chkSmoking.IsChecked == true)
            {
                rdbSmokingHabitualYes.Visibility = Visibility.Visible;
                rdbSmokingHabitualNo.Visibility = Visibility.Visible;
            }
            else
            {
                rdbSmokingHabitualYes.Visibility = Visibility.Collapsed;
                rdbSmokingHabitualNo.Visibility = Visibility.Collapsed;
            }
        }

        private void chkAlcohol_Click(object sender, RoutedEventArgs e)
        {
            if (chkAlcohol.IsChecked == true)
            {
                rdbAlcoholHabitualYes.Visibility = Visibility.Visible;
                rdbAlcoholHabitualNo.Visibility = Visibility.Visible;
            }
            else
            {
                rdbAlcoholHabitualYes.Visibility = Visibility.Collapsed;
                rdbAlcoholHabitualNo.Visibility = Visibility.Collapsed;
            }
        }

        private void chkAnyOther_Click(object sender, RoutedEventArgs e)
        {
            if (chkAnyOther.IsChecked == true)
            {
                rdbOtherHabitualYes.Visibility = Visibility.Visible;
                rdbOtherHabitualNo.Visibility = Visibility.Visible;
                txtIfAnyOther.Visibility = Visibility.Visible;
                lblIfYesDetails.Visibility = Visibility.Visible;

            }
            else
            {
                rdbOtherHabitualYes.Visibility = Visibility.Collapsed;
                rdbOtherHabitualNo.Visibility = Visibility.Collapsed;
                txtIfAnyOther.Visibility = Visibility.Collapsed;
                lblIfYesDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgHistory.SelectedItem != null)
            {
                SetCommandButtonState("Save");
                ClearFormData();
                this.DataContext = (clsMaleHistoryVO)dgHistory.SelectedItem;
                SelectedRecord = ((clsMaleHistoryVO)dgHistory.SelectedItem).ID;
                clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                Male.DataContext = null;
                MalePatientDetails = CoupleDetails.MalePatient;
                //MalePatientDetails.Height = ((clsGeneralExaminationVO)dgHistory.SelectedItem).Height;
                //MalePatientDetails.Weight = ((clsGeneralExaminationVO)dgHistory.SelectedItem).Weight;
                //MalePatientDetails.BMI = ((clsGeneralExaminationVO)dgHistory.SelectedItem).BMI;
                Male.DataContext = MalePatientDetails;
                CmdSave.IsEnabled = false;
            }
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearFormData();
            isModify = false;

            SetCommandButtonState("Save");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ClearFormData()
        {
            this.DataContext = new clsMaleHistoryVO();
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
