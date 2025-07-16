using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using CIMS;
using CIMS.Forms;
using PalashDynamics.Collections;
using System.Reflection;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Media.Imaging;
using System.IO;

namespace OPDModule.Forms
{
    public partial class frmPenPusher_Anoto_ : UserControl, IInitiateCIMS
    {
         public PagedSortableCollectionView<clsPatientPenPusherInfoVO> DataList { get; private set; }
         bool IsPatientExist = false;
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
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }


        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        IsPatientExist = false;

                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }
                    break;

            }

        }
        public frmPenPusher_Anoto_()
        {
            InitializeComponent();
            if (IsPatientExist == false)
            {
               // ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.QueueManagement");
                // ((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            }
            else
            {

                objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

                //======================================================
                //Paging
                DataList = new PagedSortableCollectionView<clsPatientPenPusherInfoVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

                DataListPageSize = 15;
                this.dgDataPager.DataContext = DataList;
                this.dgPatientPenPusherist.DataContext = DataList;
                FetchData();
            }
          
          
        }

        private void frmPenPusher_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.QueueManagement");
                // ((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            }
            this.DataContext = new clsPatientPenPusherInfoVO();
           
                //FetchData();
            
           
        }

         
        #region Variable Declaration
        private long IUnitId { get; set; }
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public List<bool> check = new List<bool>();
        public bool CheckStatus { get; set; }
        bool IsCancel = true;
        public clsGetPatientBizActionVO OBJPatient { get; set; }
        //public clsGetBankDetailsVO OBJBank { get; set; }
        List<clsPatientPerscriptionInfoVO> PrescriptionList { get;set;}
        #endregion



        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    //cmdModify.IsEnabled = false;
                    //cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //cmdNew.IsEnabled = true;
                    IsCancel = true;
                   
                    break;
                case "New":
                    //cmdModify.IsEnabled = false;
                    //cmdSave.IsEnabled = true;
                    //cmdNew.IsEnabled = false;
                 
                    cmdCancel.IsEnabled = true;
                   

                    IsCancel = false;
                    break;
                case "Save":
                    //cmdNew.IsEnabled = true;
                    //cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                   
                    IsCancel = true;
                    break;
                case "Modify":
                    //cmdNew.IsEnabled = true;
                    //cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;

                    IsCancel = true;
                    break;
                case "Cancel":
                    //cmdNew.IsEnabled = true;
                    //cmdModify.IsEnabled = false;
                    //cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    //cmdModify.IsEnabled = true;
                    //cmdSave.IsEnabled = false;
                    //cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                   
                    IsCancel = false;
                    break;
               
              

                default:
                    break;
            }
        }
        //private void cmdNew_Click(object sender, RoutedEventArgs e)
        //{
        //    this.SetCommandButtonState("New");
        //    tbDoctorInformation.SelectedIndex = 0;
        //    // this.DataContext = new clsDoctorVO();
           
           
           
        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
        //    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
        //    mElement.Text = " : " + "New Doctor Details";
        //    objAnimation.Invoke(RotationType.Forward);
        //}

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void hlbViewDoctorMaster_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            //ClearControl();
            

            this.DataContext = (clsPatientPenPusherInfoVO)dgPatientPenPusherist.SelectedItem;
            FillData(((clsPatientPenPusherInfoVO)dgPatientPenPusherist.SelectedItem).ID, ((clsPatientPenPusherInfoVO)dgPatientPenPusherist.SelectedItem).UnitId);
            //LblDate.Text = Convert.ToString(objPatient[0].Date);
            //if (objPatient.Count > 0)
            //{
            //    dgPrescriptionList.ItemsSource = objPatient;
            //}

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsPatientPenPusherInfoVO)dgPatientPenPusherist.SelectedItem).PatientName;
            // objAnimation.Invoke(RotationType.Forward);
        }
        List<clsPatientPerscriptionInfoVO> objPatient = new List<clsPatientPerscriptionInfoVO>();
        private void FillData(long iPatientID,long UnitID)
        {

            clsGetPatientPenPusherDetailByIDBizActionVO BizAction = new clsGetPatientPenPusherDetailByIDBizActionVO();
            BizAction.PatientID = iPatientID;
            BizAction.UnitID = UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("View");
                    if (dgPatientPenPusherist.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);
                    objPatient = ((clsGetPatientPenPusherDetailByIDBizActionVO)ea.Result).PatientPrescriptionDetailsList;
                    LblDate.Text = null;
                    dgPrescriptionList.ItemsSource = null;
                    if (objPatient != null)
                    {
                        dgPrescriptionList.ItemsSource = objPatient;
                        dgPrescriptionList.Columns[1].Visibility = Visibility.Collapsed;

                        LblDate.Text = Convert.ToString(objPatient[0].Date);
                    }
                }
                
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FetchData()
        {
           
                clsGetPatientPenPusherDetailListBizActionVO BizAction = new clsGetPatientPenPusherDetailListBizActionVO();
                BizAction.PatientPenPusherDetailsList = new List<clsPatientPenPusherInfoVO>();
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //dgDoctorList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPatientPenPusherDetailListBizActionVO)arg.Result).PatientPenPusherDetailsList != null)
                        {
                            //dgDoctorList.ItemsSource = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                            clsGetPatientPenPusherDetailListBizActionVO result = arg.Result as clsGetPatientPenPusherDetailListBizActionVO;

                            if (result.PatientPenPusherDetailsList != null)
                            {
                                DataList.Clear();
                                DataList.TotalItemCount = ((clsGetPatientPenPusherDetailListBizActionVO)arg.Result).TotalRows;

                                //PrescriptionList = result.PatientPenPusherDetailsList;
                                foreach (clsPatientPenPusherInfoVO item in result.PatientPenPusherDetailsList)
                                {
                                    DataList.Add(item);
                                }

                                //dgPatientPenPusherist.ItemsSource = DataList;

                            }
                            

                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            
           
          
        }

    }
}
