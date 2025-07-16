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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using System.Text;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using MessageBoxControl;
using PalashDynamics.ValueObjects.User;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.Windows.Media.Effects;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewPathoResultEntry : UserControl
    {
        #region List and Variable Declaration
        clsAppConfigVO myAppConfig;
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstAuthenticationLevel { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }       
        private SwivelAnimation objAnimation;
        SolidColorBrush NormalColorCode = new SolidColorBrush();
        SolidColorBrush MinColorCode = new SolidColorBrush();
        SolidColorBrush MaxColorCode = new SolidColorBrush();
        SolidColorBrush FirstLevelcolorCode = new SolidColorBrush();
        SolidColorBrush SecondLevelcolorCode = new SolidColorBrush();
        SolidColorBrush ThirdLevelcolorCode = new SolidColorBrush();
        SolidColorBrush CheckresultColorCode = new SolidColorBrush();
        //List<clsUserCategoryLinkVO> UserCategoryLinkedList;
        List<clsPathOrderBookingDetailVO> SelectedTestList;

        #region BackPanel Variables
        List<clsPathOrderBookingDetailVO> lstTest = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemplstTest = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> AgencyList = new List<clsPathOrderBookingDetailVO>();
        public ObservableCollection<clsPathoTestParameterVO> TestList { get; set; }
        public ObservableCollection<clsPathoTestParameterVO> CatagoryWiseTestList { get; set; }
        public ObservableCollection<clsPathoTestParameterVO> RemovedTestList { get; set; }
        List<clsPathPatientReportVO> OrderList = new List<clsPathPatientReportVO>();
        bool IsPageLoded = false;
        bool IsUpdate = false;
        List<clsPathoTestParameterVO> MarkBoldList { get; set; }
        float balamt = 0;
        PagedCollectionView collection;
        List<clsUserCategoryLinkVO> UserCategoryLinkedList;
      
        public long OrderID { get; set; }
        public string DoctorName { get; set; }
        public string HTMLContent = "";
        ObservableCollection<clsPathoTestParameterVO> SelectedTestParameterValues = new ObservableCollection<clsPathoTestParameterVO>();
        ObservableCollection<clsPathoTestParameterVO> RemoveSelectedTestParameterValues = new ObservableCollection<clsPathoTestParameterVO>();
        public bool IsTempate = false;
        bool PrintTemplate = false;
        long TemplateID { get; set; }
        public long TemplateOrderID = 0;
        public long TemplatePathOrderBookingDetailID = 0;
        public long TemplateTestID = 0;
        public string TemplateSampleNo = null;
        public DateTime? TemplateSampleCollectionDatetime = null;
        public long TestTemplateID = 0;
        bool IsNumeric { get; set; }
        object temp;
        bool ISFinalize = false;
        StringBuilder strPatInfo, strDoctorPathInfo;
        string BillNumber = null;
        bool IsDuplicate = false;
        double BalanceAmount = 0;
        clsPathOrderBookingDetailVO SelectedDetails { get; set; }
        clsPathPatientReportVO ObjDetails;
        PasswordWindow Win = new PasswordWindow();
        bool BackPanelTemplate = false;
        MessageBoxControl.MessageBoxChildWindow msgW1;
        public bool isTemplateTest = false;
        List<clsPathOrderBookingDetailVO> List1 = new List<clsPathOrderBookingDetailVO>();
        List<clsGetDeltaCheckValueBizActionVO> PreviousResult;
        bool IsHelpValSave = false;
        long BillNum = 0;
        int ISRferalDoctorSignature = 0;
        public String TemplateContent = String.Empty;
        public long TestID = 0;
        public String TestAndSampleNO = "";
        public long? TestCategoryID = 0;
        public List<string> DeltaResultValue = new List<string>();
        public double CalDeltaCheck = 0;
        public long DeltaTestId = 0;
        List<clsPathOrderBookingDetailVO> SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
        public long ReportBillId = 0;
        public string ReportBillNo = " ";
        long PathologistId { get; set; }
        public bool IsResultEntryModified = false;

        #endregion
        #endregion

        Color myRgbColor;
        Color myRgbColor1;
        Color myRgbColor2;
        Color myRgbColor3;
        public NewPathoResultEntry()
        {
            InitializeComponent();
            myAppConfig = new clsAppConfigVO();
            myAppConfig = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(NewPathologyWorkOrderGeneration_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            UserCategoryLinkedList = new List<clsUserCategoryLinkVO>();
            UserCategoryLinkedList = ((IApplicationConfiguration)App.Current).CurrentUser.UserCategoryLinkList;
            SelectedTestList = new List<clsPathOrderBookingDetailVO>();
            CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();


             myRgbColor = new Color();
            myRgbColor.A = Convert.ToByte(120);
            myRgbColor.R = Convert.ToByte(255);
            myRgbColor.G = Convert.ToByte(0);
            myRgbColor.B = Convert.ToByte(0);
            txtPending.Background = new SolidColorBrush(myRgbColor);

             myRgbColor1 = new Color();
            myRgbColor1.A = Convert.ToByte(120);
            myRgbColor1.R = Convert.ToByte(240);
            myRgbColor1.G = Convert.ToByte(230);
            myRgbColor1.B = Convert.ToByte(140);
            txtPartial.Background = new SolidColorBrush(myRgbColor1);

             myRgbColor2 = new Color();
            myRgbColor2.A = Convert.ToByte(120);
            myRgbColor2.R = Convert.ToByte(32);
            myRgbColor2.G = Convert.ToByte(178);
            myRgbColor2.B = Convert.ToByte(170);
            txtCompleted.Background = new SolidColorBrush(myRgbColor2);


             myRgbColor3 = new Color();
            myRgbColor3.A = Convert.ToByte(120);
            myRgbColor3.R = Convert.ToByte(65);
            myRgbColor3.G = Convert.ToByte(105);
            myRgbColor3.B = Convert.ToByte(225);
            txtAuthorized.Background = new SolidColorBrush(myRgbColor3);

        }

        #region Front Panel

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement1.Text = "Pathology Work Order";
                    break;
            }
        }

        #endregion

        #region Loaded Event
        void NewPathologyWorkOrderGeneration_Loaded(object sender, RoutedEventArgs e)
        {
            //lstSampleType = new List<MasterListItem>();
            //lstUploadStatus = new List<MasterListItem>();
            //lstDeliveryStatus = new List<MasterListItem>();
            //lstSampleType.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            //lstSampleType.Add(new MasterListItem() { ID = 1, Description = "Done", Status = true });
            //lstSampleType.Add(new MasterListItem() { ID = 2, Description = "Not Done", Status = true });
            //cmbSampleType.ItemsSource = lstSampleType;
            //cmbSampleType.SelectedItem = lstSampleType[0];

            lstAuthenticationLevel = new List<MasterListItem>();
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 1, Description = "Result Entry Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 2, Description = "Result Entry Not Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 3, Description = "Supervisor Approval Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 4, Description = "Doctor Approval Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 5, Description = "Forwarded To Check Result", Status = true });
            //CmbAuthentication.ItemsSource = lstAuthenticationLevel;
            //CmbAuthentication.SelectedItem = lstAuthenticationLevel[0];
            CmdMachinlinking.Visibility = Visibility.Collapsed;
            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            mElement1.Text = "Pathology Result Entry";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            this.IsResultEntryModified = false;

            // fillCatagory();
            FillOrderBookingList();

            fillSampleStatus();
            FillUsers();
            FillTestCategoryFoSerach();
            //SetColorCode();
        }
        #endregion

        #region 'Paging'

        public PagedSortableCollectionView<clsPathOrderBookingVO> DataList { get; private set; }

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
        WaitIndicator indicator = new WaitIndicator();
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }
        #endregion


        #region added by rohini dated 10/2/16

        private void fillSampleStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Collected"));
            // MList.Add(new MasterListItem(2, "Not Collected"));
            MList.Add(new MasterListItem(2, "Dispatched"));
            MList.Add(new MasterListItem(3, "Received"));
            MList.Add(new MasterListItem(4, "Accepted"));
            MList.Add(new MasterListItem(5, "Rejected"));
            MList.Add(new MasterListItem(6, "Sub Optimal"));
            MList.Add(new MasterListItem(7, "Outsource"));
            cmbSampleStatus.ItemsSource = null;
            cmbSampleStatus.ItemsSource = MList;
            cmbSampleStatus.SelectedItem = MList[0];

            fillesultEntry();
        }
        private void fillesultEntry()   //changed for CR Points
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            //MList.Add(new MasterListItem(1, "Done"));
            //MList.Add(new MasterListItem(2, "Not Done"));
            MList.Add(new MasterListItem(1, "Pending"));
            MList.Add(new MasterListItem(2, "Partial Done"));
            MList.Add(new MasterListItem(3, "Completed"));
            MList.Add(new MasterListItem(4, "Authorized"));

            cmbResultEntry.ItemsSource = null;
            cmbResultEntry.ItemsSource = MList;
            cmbResultEntry.SelectedItem = MList[0];
            fillProcessedBy();
        }
        private void fillProcessedBy()
        {
            //agency name from agency master
            // cmbProcessedBy.itemSource = MList;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbProcessedBy.ItemsSource = null;
                    cmbProcessedBy.ItemsSource = objList;
                    cmbProcessedBy.SelectedItem = objList[0];

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            // fillDeliveryType(); //changed by rohini CR Points
            fillMachine();
        }
        private void fillDeliveryType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Email"));
            MList.Add(new MasterListItem(2, "By Hand"));
            // cmbDeliveryType.itemSource = MList;
            cmbDeliveryType.ItemsSource = null;
            cmbDeliveryType.ItemsSource = MList;
            cmbDeliveryType.SelectedItem = MList[0];
            fillDelivery();  
         
        }
        private void fillDelivery()  
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Delivered"));
            MList.Add(new MasterListItem(2, "Not Delivered"));
            // cmbDelivery.itemSource = MList;
            cmbDelivery.ItemsSource = null;
            cmbDelivery.ItemsSource = MList;
            cmbDelivery.SelectedItem = MList[0];
            fillMachine();
        }
        private void fillMachine()
        {
            //machine name from machine master
            // cmbMachine.itemSource = MList;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    //cmbMachine.ItemsSource = null;
                    //cmbMachine.ItemsSource = objList;
                    //cmbMachine.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            fillSample();
        }
        private void fillSample()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Collected By"));
            MList.Add(new MasterListItem(2, "Sent By"));//Dispatched by
            MList.Add(new MasterListItem(3, "Accepted By"));
            MList.Add(new MasterListItem(4, "Rejected By"));
            MList.Add(new MasterListItem(5, "Report Done By"));
            // cmbSample.itemSource = MList;
            cmbSample.ItemsSource = null;
            cmbSample.ItemsSource = MList;
            cmbSample.SelectedItem = MList[0];
            fillBillType();//BY ROHINI FOR CR POINTS
        }
        private void cmbSample_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbSample.SelectedItem).ID == 0)
            {
                txtSample.IsEnabled = false;
            }
            else if (((MasterListItem)cmbSample.SelectedItem).ID != 0)
            {
                txtSample.IsEnabled = true;
            }
        }

        private void fillBillType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "OPD"));
            MList.Add(new MasterListItem(2, "IPD"));
            // cmbDeliveryType.itemSource = MList;
            cmbBillType.ItemsSource = null;
            cmbBillType.ItemsSource = MList;
            cmbBillType.SelectedItem = MList[0];
        

        }


        #endregion
        #region Private Methods and Event Handlers
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.PatientID = 0;
                //added by rohini dated 29.2.16
                POBBizAction.IsDispatchedClinic = true;
                POBBizAction.IsFrom = "ResultEntry";
                
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    POBBizAction.UnitID = 0;
                }
                else
                {
                    POBBizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    POBBizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                    POBBizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
                }
                else
                {
                    POBBizAction.FromDate = null;
                    POBBizAction.ToDate = null;
                }
                if (txtFrontFirstName.Text != "")
                    POBBizAction.FirstName = txtFrontFirstName.Text;
                if (txtFrontLastName.Text != "")
                    POBBizAction.LastName = txtFrontLastName.Text;
                if (txtFrontMRNO.Text != "")
                    POBBizAction.MRNO = txtFrontMRNO.Text;
                //added by rohini dated 11.2.16
                if (txtBillNO.Text != "")
                    POBBizAction.BillNo = txtBillNO.Text;
                //BY ROHINI FOR CR POINTS
                if (cmbBillType.SelectedItem != null)
                {
                    POBBizAction.PatientType = ((MasterListItem)cmbBillType.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.PatientType = 0;
                }
                if (cmbResultEntry.SelectedItem != null && ((MasterListItem)cmbResultEntry.SelectedItem).ID != 0)
                {
                    POBBizAction.StatusID = ((MasterListItem)cmbResultEntry.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.StatusID = 0;
                }
                if (cmbTestCategorySearch.SelectedItem != null && ((MasterListItem)cmbTestCategorySearch.SelectedItem).ID != 0)
                {
                    POBBizAction.CatagoryID = ((MasterListItem)cmbTestCategorySearch.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.CatagoryID = 0;
                }
                if (cmbResultEntryUsers.SelectedItem != null && ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID != 0)
                {
                    POBBizAction.UserID = ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.UserID = 0;
                }
                if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
                {
                    POBBizAction.SampleNo = Convert.ToString(txtSampleNO.Text.Trim());
                }
               
               // POBBizAction.IsFromResultEntry = true;                
               
                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPathOrderBookingListBizActionVO result = arg.Result as clsGetPathOrderBookingListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;
                           // DataList.Count = result.TotalRows1;
                            if (result.OrderBookingList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.OrderBookingList)
                                {
                                    DataList.Add(item);
                                }
                                dgOrdertList.ItemsSource = null;
                                dgOrdertList.ItemsSource = DataList;
                                DataPager.Source = null;
                                DataPager.PageSize = POBBizAction.MaximumRows;
                                DataPager.Source = DataList;
                                dgTest.ItemsSource = null;
                                //added by rohini
                                txtTotalCountRecords.Text = "";
                                txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                            }
                        }
                        //by rohini
                        FillDoctor();
                        FillCategory();
                        FillGender();
                        FillTestCategory();
                        FillPathologist();                   
                        FillTemplate();

                        NoOfWorkOrder.Visibility = Visibility.Visible;
                        txtTotalCountRecords.Visibility = Visibility.Visible;
                    }
                    indicator.Close();
                };
                client.ProcessAsync(POBBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
            }
        }

        private void FillOrderBookingDetailsList()
        {
            clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            BizAction.IsFrom = "ResultEntry";
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            BizAction.CheckExtraCriteria = true;
            BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            BizAction.SampleType = POBBizAction.SampleType;
            BizAction.IsUploaded = POBBizAction.IsUploaded;
            BizAction.IsDelivered = POBBizAction.IsDelivered;
            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            //   BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //by ROHINI FOR CR Points
            if (cmbTestCategorySearch.SelectedItem != null && ((MasterListItem)cmbTestCategorySearch.SelectedItem).ID != 0)
            {
                BizAction.OrderDetail.CategoryID = ((MasterListItem)cmbTestCategorySearch.SelectedItem).ID;
            }
            else
            {
                BizAction.OrderDetail.CategoryID = 0;
            }           
            if (cmbResultEntryUsers.SelectedItem != null && ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID != 0)
            {
                BizAction.OrderDetail.ResultEntryUserID = ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID;
            }
            else
            {
                BizAction.OrderDetail.ResultEntryUserID = 0;
            }
            //
            //added by rohini dated 11.2.16
            if ((MasterListItem)cmbSampleStatus.SelectedItem != null)
            {
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 1)
                {
                    BizAction.OrderDetail.IsSampleCollected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleCollected = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 2)
                {
                    BizAction.OrderDetail.IsSampleDispatch = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleDispatch = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 3)
                {
                    BizAction.OrderDetail.IsSampleReceive = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleReceive = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
                {
                    BizAction.OrderDetail.IsAccepted = true;
                }
                else
                {
                    BizAction.OrderDetail.IsAccepted = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 5)
                {
                    BizAction.OrderDetail.IsRejected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsRejected = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 6)
                {
                    BizAction.OrderDetail.IsSubOptimal = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSubOptimal = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 7)
                {
                    BizAction.OrderDetail.IsOutSourced = true;
                }
                else
                {
                    BizAction.OrderDetail.IsOutSourced = false;
                }
            }


            //CHANGED fOR CR POINTS
         //   BizAction.OrderDetail.IsAccepted = true;
            //if ((MasterListItem)cmbResultEntry.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 1)   //result entry done
            //    {
            //        BizAction.OrderDetail.IsResultEntry = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsResultEntry = false;
            //    }
            //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 2)  //result entry Not done
            //    {
            //        BizAction.OrderDetail.IsResultEntry1 = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsResultEntry1 = false;
            //    }
            //}
            if (cmbResultEntryUsers.SelectedItem != null && ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID != 0)
            {
                POBBizAction.UserID = ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID;
            }
            else
            {
                POBBizAction.UserID = 0;
            }

            if (cmbResultEntryUsers.SelectedItem != null && ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID != 0)
            {
                POBBizAction.UserID = ((MasterListItem)cmbResultEntryUsers.SelectedItem).ID;
            }
            else
            {
                POBBizAction.UserID = 0;
            }
            //
            #region Comeened by rohini For CR POINTS
            //if ((MasterListItem)cmbDelivery.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 1)   //result report done
            //    {
            //        BizAction.OrderDetail.IsDelivered = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDelivered = false;
            //    }
            //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 2)  //result report Not done
            //    {
            //        BizAction.OrderDetail.IsDelivered1 = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDelivered1 = false;
            //    }
            //}
            //if ((MasterListItem)cmbDeliveryType.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 1)   //report by email
            //    {
            //        BizAction.OrderDetail.IsDeliverdthroughEmail = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDeliverdthroughEmail = false;
            //    }
            //    if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 2)   // report by hand
            //    {
            //        BizAction.OrderDetail.IsDirectDelivered = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDirectDelivered = false;
            //    }
            //}
            #endregion
            //Added By Bhushanp 19012017 For Date Filter
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                BizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
            }
            else
            {
                BizAction.FromDate = null;
                BizAction.ToDate = null;
            }
            //added by rohini 15.2.16
            if (((MasterListItem)cmbSample.SelectedItem != null))
            {
                if ((((MasterListItem)cmbSample.SelectedItem).ID == 1 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.SampleCollectedBy = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 2 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.DispatchBy = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 3 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 4 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 5 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.SampleReceiveBy = txtSample.Text;
                }
            }

            if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
            {
                BizAction.OrderDetail.SampleNo = Convert.ToString(txtSampleNO.Text.Trim());
            }
            if ((MasterListItem)cmbProcessedBy.SelectedItem != null)
            {
                if (((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)   // report by hand
                {
                    BizAction.OrderDetail.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
                }
                else
                {
                    BizAction.OrderDetail.AgencyID = 0;
                }
            }

            //
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        OrderTestList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                        foreach (clsPathOrderBookingDetailVO item in OrderTestList)
                        {
                            if (item.IsSampleCollected == true)
                                item.SampleCollectedImage = "../Icons/tick.png";
                            else
                                item.SampleCollectedImage = "../Icons/error.png";

                            if (item.IsSampleDispatch == true)
                                item.SampleDispatchImage = "../Icons/tick.png";
                            else
                                item.SampleDispatchImage = "../Icons/error.png";

                            if (item.IsSampleReceive == true)
                                item.SampleReceiveImage = "../Icons/tick.png";
                            else
                                item.SampleReceiveImage = "../Icons/error.png";

                            if (item.IsOutSourced == true)
                                item.SampleOutSourceImage = "../Icons/tick.png";
                            else
                                item.SampleOutSourceImage = "../Icons/error.png";

                            if (item.IsResultEntry == true)
                                item.ResultEntryImage = "../Icons/tick.png";
                            else
                                item.ResultEntryImage = "../Icons/error.png";
                            if (item.FirstLevel == true && item.SecondLevel == true)  //as per disscussion with anumani
                                item.FinalizedImage = "../Icons/tick.png";
                            else
                                item.FinalizedImage = "../Icons/error.png";
                            if (item.IsCompleted == true)
                                item.UploadImage = "../Icons/tick.png";
                            else
                                item.UploadImage = "../Icons/error.png";
                            if (item.IsDelivered == true)
                                item.DeliveredImage = "../Icons/tick.png";
                            else
                                item.DeliveredImage = "../Icons/error.png";


                            if (item.SampleAcceptRejectStatus == 1)
                            {
                                item.SamplAcceptImage = "../Icons/tick.png";
                                item.IsSampleAcceptEnable = false;
                                item.IsSampleAccepted = true;
                            }
                            else if (item.SampleAcceptRejectStatus == 2)
                            {
                                item.SamplAcceptImage = "../Icons/error.png";
                                item.IsSampleAcceptEnable = false;
                                item.IsSampleAccepted = true;
                            }
                        }
                        dgTest.ItemsSource = OrderTestList; //((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                        //fillCatagory();  //by rohni commented
                        FillTestList();  //commented by rohini coz called from selection changed
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                    FillOrderBookingDetailsList();
            }
        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           clsPathOrderBookingVO item = (clsPathOrderBookingVO)e.Row.DataContext;

           if (item.ResultColor == 1)
           {              
               e.Row.Background = new SolidColorBrush(myRgbColor);
           }           
           else if (item.ResultColor == 2)
           {
                e.Row.Background = new SolidColorBrush(myRgbColor1);
            }
            else if (item.ResultColor == 3)
            {                
                e.Row.Background = new SolidColorBrush(myRgbColor2);            
            }
            else if (item.ResultColor == 4)
            {
                e.Row.Background = new SolidColorBrush(myRgbColor3);
            }
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }
        #endregion


        #region ColorCode uncommented on 16.02.2016
        private void SetColorCode()
        {

            #region Normal Color
            string NormalColor = myAppConfig.pathonormalColorCode;
            switch (NormalColor)
            {
                case "Black":
                    NormalColorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    NormalColorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    NormalColorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    NormalColorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    NormalColorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    NormalColorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    NormalColorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    NormalColorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    NormalColorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    NormalColorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    NormalColorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    NormalColorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    NormalColorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    NormalColorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    NormalColorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            txtNormalValue.Background = NormalColorCode;
            #endregion

            #region Min ValueColor
            string MinColor = myAppConfig.pathominColorCode;
            switch (MinColor)
            {
                case "Black":
                    MinColorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    MinColorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    MinColorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    MinColorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    MinColorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    MinColorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    MinColorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    MinColorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    MinColorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    MinColorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    MinColorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    MinColorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    MinColorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    MinColorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    MinColorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }

            // txtMinValue.Background = MinColorCode;
            #endregion

            #region MAXColor
            string MaxColor = myAppConfig.pathomaxColorCode;
            switch (MaxColor)
            {
                case "Black":
                    MaxColorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    MaxColorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    MaxColorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    MaxColorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    MaxColorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    MaxColorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    MaxColorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    MaxColorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    MaxColorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    MaxColorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    MaxColorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    MaxColorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    MaxColorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    MaxColorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    MaxColorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            txtMaxValue.Background = MaxColorCode;
            #endregion

            #region First Level Color
            string FirstLevelColor = myAppConfig.FirstLevelResultColor;
            switch (FirstLevelColor)
            {
                case "Black":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    FirstLevelcolorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            //txtFirstLevel.Background = FirstLevelcolorCode;
            #endregion

            #region Second Level Color
            string SeocndLevelColor = myAppConfig.SecondLevelResultColor;
            switch (SeocndLevelColor)
            {
                case "Black":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    SecondLevelcolorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            //txtSecondLevel.Background = SecondLevelcolorCode;
            #endregion

            #region Third Level Color
            string ThirdLevelColor = myAppConfig.ThirdLevelResultColor;
            switch (ThirdLevelColor)
            {
                case "Black":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    ThirdLevelcolorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            txtThirdLevel.Background = ThirdLevelcolorCode;
            #endregion

            #region check Result Color
            string CheckResultcolor = myAppConfig.CheckResultColor;
            switch (CheckResultcolor)
            {
                case "Black":
                    CheckresultColorCode = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    CheckresultColorCode = new SolidColorBrush(Colors.Blue);
                    break;
                case "Brown":
                    CheckresultColorCode = new SolidColorBrush(Colors.Brown);
                    break;
                case "Cyan":
                    CheckresultColorCode = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DarkGray":
                    CheckresultColorCode = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "Gray":
                    CheckresultColorCode = new SolidColorBrush(Colors.Gray);
                    break;
                case "Green":
                    CheckresultColorCode = new SolidColorBrush(Colors.Green);
                    break;
                case "LightGray":
                    CheckresultColorCode = new SolidColorBrush(Colors.LightGray);
                    break;
                case "Magenta":
                    CheckresultColorCode = new SolidColorBrush(Colors.Magenta);
                    break;
                case "Orange":
                    CheckresultColorCode = new SolidColorBrush(Colors.Orange);
                    break;
                case "Purple":
                    CheckresultColorCode = new SolidColorBrush(Colors.Purple);
                    break;
                case "Red":
                    CheckresultColorCode = new SolidColorBrush(Colors.Red);
                    break;
                case "Transparent":
                    CheckresultColorCode = new SolidColorBrush(Colors.Transparent);
                    break;
                case "White":
                    CheckresultColorCode = new SolidColorBrush(Colors.White);
                    break;
                case "Yellow":
                    CheckresultColorCode = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            //txtCheckedResults.Background = CheckresultColorCode;
            #endregion

        }

        #endregion




        private void CmdResultEntry_Click(object sender, RoutedEventArgs e)
        {
           
            //indicator.Show();
            try
            {
                //if((((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.
                //added by rohini dated 29.2.16  
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsProcessingUnit == true)
                {
                    //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                    //{
                    //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is dispatched to another Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //    msgbox.Show();
                    //}
                    //

                    if ((clsPathOrderBookingVO)dgOrdertList.SelectedItem != null)
                    {
                        NoOfWorkOrder.Visibility = Visibility.Collapsed;
                        txtTotalCountRecords.Visibility = Visibility.Collapsed;
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new ValueObjects.Patient.clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.Gender = (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID == 1) ? "Male" : "Female";
                        objAnimation.Invoke(RotationType.Forward);
                        CmdResultEntry.Visibility = Visibility.Collapsed;
                        cmdSave.Visibility = Visibility.Visible;
                        GrpColor.Visibility = Visibility.Visible;
                        lblresult.Visibility = Visibility.Visible;
                        lblnormal.Visibility = Visibility.Visible;
                        txtNormalValue.Visibility = Visibility.Visible;
                        //lblmin.Visibility = Visibility.Visible;
                        //txtMinValue.Visibility = Visibility.Visible;
                        lblmax.Visibility = Visibility.Visible;
                        txtMaxValue.Visibility = Visibility.Visible;
                        CmdMachinlinking.Visibility = Visibility.Collapsed;
                        ResultListContent.Content = new PalashDynamics.SearchResultLists.DisplayPatientDetails();

                       

                        if (!string.IsNullOrEmpty(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ReferredBy))
                            txtReferenceDoctor.Text = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ReferredBy;   //referred by fills as per registration/visit
                        else
                            txtReferenceDoctor.Text = "";
                        if (lstTest.All(z => z.ReportType == "Template"))
                        {
                            TabTemplateDetails.Visibility = Visibility.Visible;
                            TabTestDetails.Visibility = Visibility.Collapsed;
                        }

                        if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID == 1)
                        {
                            cmbCategory.SelectedValue = (long)1;
                        }
                        else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID == 2)
                        {
                            cmbCategory.SelectedValue = (long)2;
                        }
                        if (PathologistId != 0)
                        {
                            cmbPathologist1.SelectedValue = PathologistId;                          
                        }
                        if (TemplateID != null)
                        {
                            cmbTemplate.SelectedValue = TemplateID;
                        }
                        if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        {
                            cmbGender.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                        }                        
                        FillDoctor();  
                        //indicator.Close();
                    }

                    else
                    {
                        indicator.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("PALASH", "PLEASE, SELECT LAB ORDER.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                      
                    }
                }
                else
                {
                    indicator.Close();

                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName + ", Is Not A Processing Unit.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                  
                }
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
              
            }
        }

        void Win_Unloaded(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NoOfWorkOrder.Visibility = Visibility.Visible;
                txtTotalCountRecords.Visibility = Visibility.Visible;
                objAnimation.Invoke(RotationType.Backward);
                CmdResultEntry.Visibility = Visibility.Visible;
                lblresult.Visibility = Visibility.Collapsed;
                GrpColor.Visibility = Visibility.Collapsed;
                lblnormal.Visibility = Visibility.Collapsed;
                txtNormalValue.Visibility = Visibility.Collapsed;
                //lblmin.Visibility = Visibility.Collapsed;
                //txtMinValue.Visibility = Visibility.Collapsed;
                lblmax.Visibility = Visibility.Collapsed;
                txtMaxValue.Visibility = Visibility.Collapsed;
                cmdSave.Visibility = Visibility.Collapsed;
                SelectedTestList.Clear();
                CatagoryWiseTestList.Clear();
                CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();
                SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                ClearData();
                FillOrderBookingList();
                CmdMachinlinking.Visibility = Visibility.Collapsed;
                ClickedFlag1 = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region BackPanel

        #region Private Methods

        private void fillCatagory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();
                        List<MasterListItem> objList1 = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList1.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //foreach (clsUserCategoryLinkVO item in objList)
                        //{
                        //    UserCategoryLinkedList.Add(item);

                        //}
                        //CmbCategory.ItemsSource = null;
                        //CmbCategory.ItemsSource = objList;
                        //CmbCategory.SelectedItem = objList[0];

                        foreach (var item in objList1)
                        {
                            foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                            {
                                if (item1.CategoryID == item.ID)
                                {
                                    objList.Add(item);
                                }
                            }
                        }

                        cmbTestCategory.ItemsSource = null;
                        cmbTestCategory.ItemsSource = objList;
                        cmbTestCategory.SelectedItem = objList[0];



                        //if (this.DataContext != null)
                        //{
                        //    CmbCategory.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).CategoryID;

                        //}
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillDoctor()
        {
            try
            {
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                       
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                        txtReferenceDoctor.ItemsSource = null;
                        txtReferenceDoctor.ItemsSource = objList;
                       
                        if (dgOrdertList.SelectedItem != null)
                        {
                            txtReferenceDoctor.SelectedItem = objList.ToList().Where(z => z.ID == ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DoctorID).FirstOrDefault();
                            txtReferenceDoctor.Text = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ReferredBy;
                        }
                      
                    }
                   // FillCategory();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
              
            }
        }

        private void FillGender()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
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
                    }
                    cmbGender.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                   // FillTestCategory();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void FillCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoParameterCategoryMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbCategory.ItemsSource = null;
                        cmbCategory.ItemsSource = objList;

                        /*commented on 26.02.2016 to set the category as per Patient's gender*/
                        //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 3 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 4)
                        //{
                        //    cmbCategory.SelectedItem = objList[1];
                        //}
                        //else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 1)
                        //{
                        //    cmbCategory.SelectedItem = objList[4];
                        //}
                        //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 2 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 5 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 6)
                        //{
                        //    cmbCategory.SelectedItem = objList[2];
                        //}
                        /* end*/
                        //cmbCategory.SelectedValue = (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID);
                        if (dgOrdertList.SelectedItem != null)
                        {
                            if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID == 1)
                            {
                                // cmbCategory.SelectedItem = objList[5];
                                cmbCategory.SelectedValue = (long)1;

                            }
                            else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID == 2)
                            {
                                // cmbCategory.SelectedItem = objList[2];
                                cmbCategory.SelectedValue = (long)2;

                            }
                            else
                            {
                                //   cmbCategory.SelectedItem = objList[3];
                            }
                        }

                       

                    }
                   // FillGender();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                cmbCategory.IsEnabled = false;
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void FillPathologist()
        {
            try
            {
                clsGetPathologistBizActionVO BizAction = new clsGetPathologistBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList1 = new List<MasterListItem>();
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetPathologistBizActionVO)arg.Result).MasterList);
                        foreach (var item in objList)
                        {
                            //
                        }

                        if (PathologistId != 0)
                        {
                            cmbPathologist1.SelectedValue = PathologistId;
                            //cmbPathologist1.IsEnabled = false; //by rohini
                        }

                        cmbPathologist1.ItemsSource = null;
                        cmbPathologist1.ItemsSource = objList;
                        cmbPathologist1.SelectedItem = objList[0];
                    
                    }
                   // FillTestList();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void FillTestList()
        {
            try
            {
                clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                BizAction.IsFrom = "ResultEntry";
                BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
                if (dgOrdertList.SelectedItem != null)
                {
                    BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                    BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                }
                if (cmbTestCategory.SelectedItem != null)
                {
                    BizAction.TestCategoryID = ((MasterListItem)cmbTestCategory.SelectedItem).ID;
                }
                //Added By Bhushanp 19012017 For Date Filter
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    BizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                    BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
                }
                else
                {
                    BizAction.FromDate = null;
                    BizAction.ToDate = null;
                }
             //   BizAction.OrderDetail.IsAccepted = true;
                BizAction.CheckExtraCriteria = true;
                BizAction.SampleType = true;
                BizAction.CheckSampleType = true;
                BizAction.OrderDetail = new clsPathOrderBookingDetailVO();   // as per Rihini's changes
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        lstTest = new List<clsPathOrderBookingDetailVO>();
                        foreach (var item in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList)
                        {
                            if (item.SampleAcceptRejectStatus != 2)
                                lstTest.Add(item);
                        }
                        List1 = new List<clsPathOrderBookingDetailVO>();
                        AgencyList = new List<clsPathOrderBookingDetailVO>();
                        foreach (clsPathOrderBookingDetailVO item in lstTest)
                        {
                            //foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                            //{
                            // if (item.CategoryID == item1.CategoryID && item.CategoryTypeID == item1.CategoryTypeID)
                            //if (((MasterListItem)cmbTestCategory.SelectedItem).ID != null)
                            //{
                            //    if (item.CategoryID == ((MasterListItem)cmbTestCategory.SelectedItem).ID)
                            //    {
                            //        List1.Add(item);
                            //    }
                            //}
                            //else
                            //{
                            foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                            {
                                if (item.CategoryID == item1.CategoryID && item.CategoryTypeID == item1.CategoryTypeID)
                                {
                                    List1.Add(item);
                                }
                            }
                            //}
                            //}
                        }


                        foreach (var item in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail)
                        {
                            AgencyList.Add(item);
                        }
                        //  collection = new PagedCollectionView(lstTest);
                        collection = new PagedCollectionView(List1);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("ReportType"));
                        dgTest1.ItemsSource = null;
                        dgTest1.ItemsSource = collection;
                        dgTest1.SelectedIndex = -1;
                    
                    }
                   // FillTemplate();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void FillTestCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        List<MasterListItem> objList1 = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList1.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        foreach (var item in objList1)
                        {
                            foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                            {

                                if (item1.CategoryID == item.ID)
                                {
                                    objList.Add(item);
                                }
                            }
                        }

                        cmbTestCategory.ItemsSource = null;
                        cmbTestCategory.ItemsSource = objList;
                        cmbTestCategory.SelectedItem = objList[0];


                        //BY rohini for cr point
                        //cmbTestCategorySearch.ItemsSource = null;
                        //cmbTestCategorySearch.ItemsSource = objList;
                        //cmbTestCategorySearch.SelectedItem = objList[0];
                       
                    }
                   // FillPathologist();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        //by rohni for CR POINTS

        private void FillTestCategoryFoSerach()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        List<MasterListItem> objList1 = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList1.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        foreach (var item in objList1)
                        {
                            foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                            {

                                if (item1.CategoryID == item.ID)
                                {
                                    objList.Add(item);
                                }
                            }
                        }                     
                        //BY rohini for cr point
                        cmbTestCategorySearch.ItemsSource = null;
                        cmbTestCategorySearch.ItemsSource = objList;
                        cmbTestCategorySearch.SelectedItem = objList[0];

                    }
                   
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }
        private void FillUsers()
        {
            try
            {
                clsGetPathoUsersBizActionVO BizAction = new clsGetPathoUsersBizActionVO();
                BizAction.MenuID = (long)10568;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList1 = new List<MasterListItem>();
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetPathoUsersBizActionVO)arg.Result).MasterList);
                        
                        cmbResultEntryUsers.ItemsSource = null;
                        cmbResultEntryUsers.ItemsSource = objList;
                        cmbResultEntryUsers.SelectedItem = objList[0];

                    }
                    // FillTestList();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        void ClearData()
        {
            OrderList = new List<clsPathPatientReportVO>();

            TestList = new ObservableCollection<clsPathoTestParameterVO>();
            dgTestList.ItemsSource = null;
            dgTestList.ItemsSource = TestList;

            lstTest = new List<clsPathOrderBookingDetailVO>();
            dgTest.ItemsSource = null;
            dgTest.ItemsSource = lstTest;

            cmbTestCategory.SelectedValue = (long)0;
            cmbTemplate.SelectedValue = (long)0;
            cmbCategory.SelectedValue = (long)0;

            cmbPathologist1.SelectedValue = (long)0;

            richTextBox.Html = HTMLContent;
            txtTotalCountRecords.Text = "";
        }

        private void GetParameters()
        {

            clsGetPathoTestParameterAndSubTestDetailsBizActionVO BizAction = new clsGetPathoTestParameterAndSubTestDetailsBizActionVO();
            try
            {
                BizAction.TestList = new List<clsPathoTestParameterVO>();
                OrderList = new List<clsPathPatientReportVO>();
                if (dgOrdertList.SelectedItem != null)
                {
                    BizAction.AgeInDays = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).AgeInDays;
                    //  BizAction.Da = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DateOfBirth;
                }

                //added by Anumani on 22.02.2016
                BizAction.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                BizAction.PatientUnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
                //

                if (lstTest != null)
                {
                    foreach (var item in lstTest)
                    {
                        if (item.IsSelected == true)
                        {
                            BizAction.DetailID = BizAction.DetailID + item.ID; //byte ROHINI
                            BizAction.DetailID = BizAction.DetailID + ",";
                            BizAction.TestID = BizAction.TestID + item.TestID;
                            BizAction.TestID = BizAction.TestID + ",";
                            BizAction.MultipleSampleNo = BizAction.MultipleSampleNo + Convert.ToString(item.SampleNo);
                            BizAction.MultipleSampleNo = BizAction.MultipleSampleNo + ",";
                            clsPathPatientReportVO obj = new clsPathPatientReportVO();
                            obj.TestID = item.TestID;
                            obj.OrderID = item.OrderBookingID;
                            obj.PathOrderBookingDetailID = item.ID;
                            obj.CategoryID = item.CategoryID;
                            obj.SampleNo = item.SampleNo;  //by rohini
                            DeltaTestId = item.TestID;
                            OrderList.Add(obj);
                        }
                    }
                }
                if (BizAction.TestID.EndsWith(","))
                    BizAction.TestID = BizAction.TestID.Remove(BizAction.TestID.Length - 1, 1);

                if (BizAction.DetailID.EndsWith(","))
                    BizAction.DetailID = BizAction.DetailID.Remove(BizAction.DetailID.Length - 1, 1); //by rohini

                if (BizAction.MultipleSampleNo.EndsWith(","))
                    BizAction.MultipleSampleNo = BizAction.MultipleSampleNo.Remove(BizAction.MultipleSampleNo.Length - 1, 1);

                if (cmbCategory.SelectedItem != null)
                    BizAction.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && (clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result != null)
                    {
                        dgTestList.ItemsSource = null;
                        if (((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList != null)
                        {
                            List<clsPathoTestParameterVO> TestDetails = new List<clsPathoTestParameterVO>();
                            TestDetails = ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList;
                            foreach (var item in TestDetails)
                            {
                                TestList.Add(item);
                                //by rohinee for formula applied parameter sholud be readonly 
                                if (item.FormulaID != string.Empty)
                                {
                                    item.IsReadOnly = true;
                                }
                                else
                                {
                                    item.IsReadOnly = false;
                                }
                                if (item.IsNumeric == false)
                                {
                                    item.IsReadOnly = true;
                                }
                                else
                                {
                                    item.IsReadOnly = false;
                                }

                                if (item.IsNumeric == true && item.IsMachine == true)
                                {
                                    if (item.ResultValue != null)
                                    {
                                        bool IsValid = item.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                                        if (IsValid)
                                        {
                                            Color C = new Color();
                                            C.R = 198;
                                            C.B = 24;
                                            C.G = 15;
                                            C.A = 255;
                                            string[] NoramlRange1 = null;
                                            char[] Splitchar = { '-' };
                                            NoramlRange1 = item.NormalRange.Split(Splitchar);

                                            if (!String.IsNullOrEmpty(item.ResultValue))
                                            {
                                                #region New Logic

                                                if ((Convert.ToDouble(item.ResultValue) > (item.HighReffValue)))
                                                {
                                                    item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                }
                                                else if ((Convert.ToDouble(item.ResultValue) < (item.LowReffValue)))
                                                {
                                                    item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                }
                                                else
                                                {
                                                    item.ApColor = "/PalashDynamics;component/Icons/green.png";
                                                }
                                                if (item.LowerPanicValue >= 0 && item.UpperPanicValue > 0)
                                                {
                                                    if ((Convert.ToDouble(item.ResultValue) < (item.LowerPanicValue)) || ((Convert.ToDouble(item.ResultValue) > (item.UpperPanicValue))))
                                                    {
                                                        item.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                                    }
                                                    else
                                                    {
                                                        item.ApColorPanic = " ";
                                                    }
                                                }
                                                else
                                                {
                                                    item.ApColorPanic = " ";
                                                }
                                                //Improbable Values
                                                if ((Convert.ToDouble(item.ResultValue) < (item.MinImprobable)) || ((Convert.ToDouble(item.ResultValue) > (item.MaxImprobable))))
                                                {
                                                    item.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                                }
                                                else
                                                {
                                                    item.ApColorImp = " ";
                                                }
                                                # endregion
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (var item10 in TestList)
                            {
                                DeltaResultValue.Add(item10.PreviousResultValue);
                                //if (!String.IsNullOrEmpty(item10.ResultValue))
                                //{
                                //    item10.MachineMannual = "Mannual";
                                //    item10.IsReadOnly = false;
                                //}
                                //else
                                //{
                                //    item10.MachineMannual = "Mannual";
                                //   // item10.MachineMannual = "Mannual";
                                //    item10.IsReadOnly = true;
                                //}
                                if (item10.IsNumeric == true)
                                {

                                    //CalDeltaCheck = (Convert.ToDouble(item10.ResultValue) - Convert.ToDouble(item10.PreviousResultValue)) / Convert.ToDouble(item10.PreviousResultValue) * 100;
                                    //if (CalDeltaCheck < 15)
                                    //{
                                    //    FrameworkElement fe = dgTestList.Columns[7].GetCellContent(item10);
                                    //    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                                    //    var thisCell = (DataGridCell)parent;
                                    //    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //    txt.Content = "Fail";
                                    //}
                                    //else
                                    //{
                                    //    FrameworkElement fe = dgTestList.Columns[7].GetCellContent(item10);
                                    //    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                                    //    var thisCell = (DataGridCell)parent;
                                    //    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //    txt.Content = "Pass";
                                    //}
                                }
                                else
                                {
                                    //FrameworkElement fe = dgTestList.Columns[7].GetCellContent(item10);
                                    //FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                                    //var thisCell = (DataGridCell)parent;
                                    //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //txt.Content = "N.A";
                                }
                            }
                            //    GetLatestPreviousValue();


                            GetResultEntry();

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private double GetLatestPreviousValue(clsPathoTestParameterVO obj)
        //{
        //    clsGetDeltaCheckValueBizActionVO BizAction = new clsGetDeltaCheckValueBizActionVO();
        //    BizAction.PathTestId = new clsPathOrderBookingDetailVO();
        //    BizAction.PathoTestParameter = new clsPathoTestParameterVO();
        //    BizAction.PathPatientDetail = new clsPathOrderBookingVO();
        //    BizAction.PathTestId.TestID = DeltaTestId;
        //    BizAction.PathoTestParameter.ParameterID = obj.ParameterID;
        //    BizAction.PathPatientDetail.PatientID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;

        //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
        //       client1.ProcessCompleted += (s, arg) =>
        //        {
        //             if (arg.Error == null && arg.Result != null)
        //        {
        //            clsGetDeltaCheckValueBizActionVO result = arg.Result as clsGetDeltaCheckValueBizActionVO;

        //            if (result.List != null)
        //            {
        //                foreach (var item in result.List)
        //                {
        //                    //DeltaResultValue = item.ResultValue;
        //                    CalDeltaCheck = (Convert.ToDouble(item.ResultValue) - Convert.ToDouble(DeltaResultValue)) / Convert.ToDouble(item.ResultValue) * 100;


        //                    if (CalDeltaCheck <= 15)
        //                    {
        //                        FrameworkElement fe = dgTestList.Columns[7].GetCellContent(item);
        //                        FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
        //                        var thisCell = (DataGridCell)parent;
        //                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                        txt.Content = "Fail";
        //                    }
        //                    else 
        //                    {
        //                        FrameworkElement fe = dgTestList.Columns[7].GetCellContent(item);
        //                        FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
        //                        var thisCell = (DataGridCell)parent;
        //                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                        txt.Content = "Pass";
        //                    }
        //                }
        //            }


        //             }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //                msgW1.Show();
        //            }
        //        };
        //        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client1.CloseAsync();



        //        //if (CalDeltaCheck <= 15)
        //        //{
        //        //    FrameworkElement fe = dgTestList.Columns[7].GetCellContent(dgTestList.SelectedItem);
        //        //    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
        //        //    var thisCell = (DataGridCell)parent;
        //        //    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //        //    txt.Content = "Fail";


        //        //    return "Fail";


        //        //}
        //        //else
        //        //{
        //        //    FrameworkElement fe = dgTestList.Columns[7].GetCellContent(dgTestList.SelectedItem);
        //        //    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
        //        //    var thisCell = (DataGridCell)parent;
        //        //    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //        //    txt.Content = "Pass";
        //        //    //      item.CheckDeltaFlag = "Pass";
        //        //    // obj.DeltaCheckValue = "Pass";
        //        //    return "Pass";
        //        //}

        //        return CalDeltaCheck;
        //}



        private void GetResultEntry()
        {

            try
            {
                int k = 0;
                clsGetPathoFinalizedEntryBizActionVO BizAction = new clsGetPathoFinalizedEntryBizActionVO();
                List<clsPathoTestParameterVO> HelpValues = new List<clsPathoTestParameterVO>();
                BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                if (lstTest != null)
                {
                    foreach (var item in lstTest)
                    {
                        if (item.IsSelected == true)
                        {
                            BizAction.DetailID = BizAction.DetailID + item.ID;
                            BizAction.DetailID = BizAction.DetailID + ",";
                        }
                    }
                }

                //


                //

                if (BizAction.DetailID.EndsWith(","))
                    BizAction.DetailID = BizAction.DetailID.Remove(BizAction.DetailID.Length - 1, 1);
                dgTestList.ItemsSource = null;
                dgItemDetailsList.ItemsSource = null;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        # region DeltaCheck
                        /*To calculate Delta Check of a paramter
                         * To get the latest Previous Result value of respective paramters of Tests of a particular patient.
                         */

                        //clsGetDeltaCheckValueBizActionVO BizAction2 = new clsGetDeltaCheckValueBizActionVO();
                        //BizAction2.PathoTestParameter = new clsPathoTestParameterVO();
                        //BizAction2.PathTestId = new clsPathOrderBookingDetailVO();
                        //BizAction2.PathPatientDetail = new clsPathOrderBookingVO();
                        //PreviousResult = new List<clsGetDeltaCheckValueBizActionVO>();


                        ////  BizAction.PathoTestParameter.ParameterID = ParameterId;
                        ////BizAction2.PathTestId.TestID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID; 

                        //if (lstTest != null)
                        //{
                        //    foreach (var item in lstTest)
                        //    {
                        //        if (item.IsSelected == true)
                        //        {
                        //            BizAction2.DetailID = BizAction2.DetailID + item.TestID;
                        //            BizAction2.DetailID = BizAction2.DetailID + ",";
                        //        }
                        //    }
                        //}

                        //if (BizAction2.DetailID.EndsWith(","))
                        //    BizAction2.DetailID = BizAction2.DetailID.Remove(BizAction2.DetailID.Length - 1, 1);

                        //BizAction2.PathPatientDetail.PatientID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                        //Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        //PalashServiceClient client2 = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);



                        //client2.ProcessCompleted += (s2, arg2) =>
                        //{
                        //    if (arg2.Error == null && arg2.Result != null)
                        //    {
                        //        clsGetDeltaCheckValueBizActionVO resultDC = arg2.Result as clsGetDeltaCheckValueBizActionVO;
                        //        if (resultDC.List != null)
                        //        {
                        //            foreach (var item in resultDC.List)
                        //            {
                        //                PreviousResult.Add(item);

                        //            }
                        //        }
                        //    }
                        //    //  };
                        //};
                        //client2.ProcessAsync(BizAction2, ((IApplicationConfiguration)App.Current).CurrentUser);
                        //client2.CloseAsync();
                        //

                        # endregion

                        if (((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultList != null)
                        {
                            // Added on 27.04.2016
                            List<clsPathPatientReportVO> objTestlist;
                            objTestlist = ((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultList;
                            foreach (var item in objTestlist)
                            {
                                PathologistId = item.PathologistID1;
                                break;
                            }
                            //


                            List<clsPathoTestParameterVO> ObjTest;
                            ObjTest = ((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultEntryDetails.TestList;
                            MarkBoldList = new List<clsPathoTestParameterVO>();



                            foreach (var item5 in ObjTest)
                            {
                                //if (item5.CategoryID != null)
                                //    cmbCategory.SelectedValue = item5.CategoryID;
                                IsNumeric = item5.IsNumeric;

                                if (IsNumeric == true && item5.IsMachine == false)
                                {
                                    // uncommented on 16.02.2016 for color code
                                    Color C = new Color();
                                    C.R = 198;
                                    C.B = 24;
                                    C.G = 15;
                                    C.A = 255;

                                    string[] NoramlRange1 = null;
                                    char[] Splitchar = { '-' };
                                    NoramlRange1 = item5.NormalRange.Split(Splitchar);

                                    if (!String.IsNullOrEmpty(item5.ResultValue))
                                    {
                                        #region New Logic
                                        
                                        if ((Convert.ToDouble(item5.ResultValue) > (item5.HighReffValue)))
                                        {
                                            item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                        }
                                        else if ((Convert.ToDouble(item5.ResultValue) < (item5.LowReffValue)))
                                        {
                                            item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                        }
                                        else
                                        {
                                            item5.ApColor = "/PalashDynamics;component/Icons/green.png";
                                        }
                                        if (item5.LowerPanicValue >= 0 && item5.UpperPanicValue > 0)
                                        {
                                            if ((Convert.ToDouble(item5.ResultValue) < (item5.LowerPanicValue)) || ((Convert.ToDouble(item5.ResultValue) > (item5.UpperPanicValue))))
                                            {
                                                item5.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                            }
                                            else
                                            {
                                                item5.ApColorPanic = " ";
                                            }
                                        }
                                        else
                                        {
                                            item5.ApColorPanic = " ";
                                        }
                                        //Improbable Values
                                        if ((Convert.ToDouble(item5.ResultValue) < (item5.MinImprobable)) || ((Convert.ToDouble(item5.ResultValue) > (item5.MaxImprobable))))
                                        {
                                            item5.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                        }
                                        else
                                        {
                                            item5.ApColorImp = " ";
                                        }

                                        # endregion
                                    }
                                }
                                if (IsNumeric == true && item5.IsMachine == true)
                                {
                                    bool IsValid=item5.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                                    if (IsValid)
                                    {
                                        Color C = new Color();
                                        C.R = 198;
                                        C.B = 24;
                                        C.G = 15;
                                        C.A = 255;
                                        string[] NoramlRange1 = null;
                                        char[] Splitchar = { '-' };
                                        NoramlRange1 = item5.NormalRange.Split(Splitchar);

                                        if (!String.IsNullOrEmpty(item5.ResultValue))
                                        {
                                            #region New Logic

                                            if ((Convert.ToDouble(item5.ResultValue) > (item5.HighReffValue)))
                                            {
                                                item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                            }
                                            else if ((Convert.ToDouble(item5.ResultValue) < (item5.LowReffValue)))
                                            {
                                                item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                            }
                                            else
                                            {
                                                item5.ApColor = "/PalashDynamics;component/Icons/green.png";
                                            }
                                            if (item5.LowerPanicValue >= 0 && item5.UpperPanicValue > 0)
                                            {
                                                if ((Convert.ToDouble(item5.ResultValue) < (item5.LowerPanicValue)) || ((Convert.ToDouble(item5.ResultValue) > (item5.UpperPanicValue))))
                                                {
                                                    item5.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                                }
                                                else
                                                {
                                                    item5.ApColorPanic = " ";
                                                }
                                            }
                                            else
                                            {
                                                item5.ApColorPanic = " ";
                                            }
                                            //Improbable Values
                                            if ((Convert.ToDouble(item5.ResultValue) < (item5.MinImprobable)) || ((Convert.ToDouble(item5.ResultValue) > (item5.MaxImprobable))))
                                            {
                                                item5.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                            }
                                            else
                                            {
                                                item5.ApColorImp = " ";
                                            }
                                            # endregion
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        clsGetHelpValuesFroResultEntryBizActionVO BizAction1 = new clsGetHelpValuesFroResultEntryBizActionVO();
                                        BizAction1.HelpValueList = new List<clsPathoTestParameterVO>();
                                        BizAction1.ParameterID = item5.ParameterID;
                                        BizAction1.ParameterDetails = item5;                                  

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                                        client.ProcessCompleted += (s1, arg1) =>
                                        {
                                            if (arg1.Error == null)
                                            {

                                                if (((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList != null)
                                                {
                                                    HelpValues = ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList;                                                  

                                                    for (int i = 0; i < TestList.Count; i++)
                                                    {

                                                        if (TestList[i].ParameterID == ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).ParameterDetails.ParameterID)
                                                        {
                                                            foreach (var item in HelpValues)
                                                            {
                                                                if (TestList[i].ResultValue == item.HelpValue)
                                                                {
                                                                   // item5.ResultValue = item.HelpValue;
                                                                    if (item.IsAbnormal == true)
                                                                    {                                                                       
                                                                        TestList[i].IsAbnoramal = true;
                                                                        TestList[i].ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                                    }
                                                                    else
                                                                    {                                                                      
                                                                        TestList[i].IsAbnoramal = false;
                                                                        TestList[i].ApColor = "/PalashDynamics;component/Icons/green.png";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    PagedCollectionView Collection2 = new PagedCollectionView(TestList);                                                  
                                                    Collection2.GroupDescriptions.Add(new PropertyGroupDescription("TestAndSampleNO"));                                                  
                                                    dgTestList.ItemsSource = null;
                                                    dgTestList.ItemsSource = Collection2;
                                                    dgTestList.UpdateLayout();
                                                }
                                                FillPathologist();

                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                                msgW1.Show();
                                            }

                                        };

                                        client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client.CloseAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                }
                                MarkBoldList.Add(item5);
                            }
                            for (int i = 0; i < TestList.Count; i++)
                            {
                                for (int j = 0; j < ObjTest.Count; j++)
                                {
                                    if (TestList[i].PathoTestID == ObjTest[j].PathoTestID)
                                        if (TestList[i].ParameterName.Equals(ObjTest[j].ParameterName))
                                            if (TestList[i].ParameterDefaultValueId == ObjTest[j].ParameterDefaultValueId && TestList[i].TestAndSampleNO == ObjTest[j].TestAndSampleNO)   //modified by rohini aded sample no 
                                                TestList[i] = ObjTest[j];
                                }
                            }
                            foreach (var item in SelectedTestParameterValues)
                            {
                                foreach (var item1 in TestList)
                                {
                                    if (item.PathoTestID == item1.PathoTestID && item.ParameterID == item1.ParameterID )
                                    {
                                        item1.ResultValue = item.ResultValue;
                                        item1.FootNote = item.FootNote;
                                        item1.Note = item.Note;
                                        break;
                                    }
                                }
                            }
                            foreach (var item in TestList)
                            {
                                if (List1.ToList().Count == 0)
                                {
                                    item.IsReadOnly = false;
                                    item.IsNumeric = true;
                                }
                                foreach (var item1 in List1)
                                {
                                    if (item.PathoTestID == item1.TestID)
                                    {
                                        if (item.TestCategoryID == item1.CategoryID)
                                        {
                                            item.IsReadOnly = false;
                                        }
                                        else
                                        {
                                            item.IsReadOnly = true;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        if (item.IsNumeric)
                                        {
                                            item.IsReadOnly = true;
                                            item.IsNumeric = true;
                                        }
                                        else
                                        {
                                            item.IsReadOnly = false;
                                            item.IsNumeric = false;
                                        }
                                    }
                                }
                            }
                            PagedCollectionView Collection = new PagedCollectionView(TestList);                        
                            Collection.GroupDescriptions.Add(new PropertyGroupDescription("TestAndSampleNO"));  //by rohini for sample no
                       
                            dgTestList.ItemsSource = null;
                            dgTestList.ItemsSource = Collection;
                        }

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cmdGetTest.IsEnabled = true;
            }
        }

        private void GetTemplateResultEntry()
        {
            try
            {
                clsGetPathoFinalizedEntryBizActionVO BizAction = new clsGetPathoFinalizedEntryBizActionVO();
                BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                if (dgOrdertList.SelectedItem != null)
                    BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                if (lstTest != null)
                {
                    foreach (var item in lstTest)
                    {
                        if (item.IsSelected == true && item.ReportTemplate == true)
                        {
                            BizAction.DetailID = BizAction.DetailID + item.ID;
                            BizAction.DetailID = BizAction.DetailID + ",";
                        }
                    }
                }
                if (BizAction.DetailID.EndsWith(","))
                    BizAction.DetailID = BizAction.DetailID.Remove(BizAction.DetailID.Length - 1, 1);

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && ((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultList != null)
                    {
                        clsPathPatientReportVO ObjDetails = new clsPathPatientReportVO();
                        ObjDetails = ((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultEntryDetails;
                        if (ObjDetails.TemplateDetails != null)
                        {
                            cmbPathologist1.SelectedValue = ObjDetails.TemplateDetails.PathologistID;
                            //   cmbPathologist1.IsEnabled = false;

                            richTextBox.Html = " ";
                            grdTemplate.DataContext = ObjDetails.TemplateDetails;
                            richTextBox.Html = ObjDetails.TemplateDetails.Template;

                            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                                richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";

                            cmbTemplate.SelectedValue = ObjDetails.TemplateDetails.TemplateID;
                            TemplateID = ObjDetails.TemplateDetails.TemplateID;
                            richTextBox.Html = ObjDetails.TemplateDetails.Template;


                            PrintTemplate = true;
                            FillTemplate();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            if (child != null)
            {
                object parent = child.Parent;
                if (parent != null)
                {
                    if (parent.GetType() == targetType)
                    {
                        return (FrameworkElement)parent;
                    }
                    else
                    {
                        return GetParent((FrameworkElement)parent, targetType);
                    }
                }
                return null;
            }
            return null;
        }

        private bool CheckValidation()
        {
            bool result = true;
            if (TestList != null && TestList.Count > 0)
            {
                foreach (var item in TestList)
                {
                    foreach (var item1 in UserCategoryLinkedList)
                    {
                        if (item.TestCategoryID == item1.CategoryID)
                        {
                            CatagoryWiseTestList.Add(item);
                        }
                    }
                }
            }


            if (TestList != null && TestList.Count > 0)
            {
                foreach (var item in TestList)
                {

                    if (String.IsNullOrEmpty(item.ResultValue))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please,Enter Required Result Value.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }

                }
            }

            if (SelectedTestList != null && SelectedTestList.Count > 0 )
            {
                foreach (var item in SelectedTestList)
                {
                    if ((item.SecondLevel == true) && ((IApplicationConfiguration)App.Current).CurrentUser.UserAditionalRights.IsEditAfterFinalized != true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Test Approved By Doctor And User Dont Have Rights To Modify Result After Authorization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        ClickedFlag1 = 0;
                        return result;

                    }

                }
            }
            if (SelectedTestList != null && SelectedTestList.Count > 0)
            {
                if (SelectedTestList.Where(z => z.ReportType == "Template").ToList().Count > 0)
                {
                    //by rohinee 
                    if (((MasterListItem)cmbPathologist1.SelectedItem).ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Pathologist", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        cmbPathologist1.IsEnabled = true;
                        result = false;
                        return result;
                    }
                    //by rohinee 
                    if (((MasterListItem)cmbTemplate.SelectedItem).ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Template", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }

                    // by rohini 7/1/17
                    if (TabTemplateDetails.Visibility == Visibility.Visible)
                    {  
                        if (String.IsNullOrEmpty(richTextBox.Text))
                        {
                        //    var start = richTextBox.Document.ContentStart;
                        //    var end = richTextBox.Document.ContentEnd;
                        //    int difference = start.GetOffsetToPosition(end);
                            MessageBoxControl.MessageBoxChildWindow msgW13 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW13.Show();
                            TabControlMain.SelectedIndex = 1;
                            result = false;
                            return result;
                        }
                    }

                }
            }

            if (SelectedTestList != null && SelectedTestList.Count > 0)
            {
                if (SelectedTestList.Where(z => z.ReportType != "Template").ToList().Count > 0)
                {
                    if (TestList == null)   //change 17/11/16  removed dgTestList.ItemsSource == null &&
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please,Get Parameters Of Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                    else if (TestList.Count == 0)   //change 17/11/16  removed dgTestList.ItemsSource == null &&
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please,Get Parameters Of Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                    //else if (CatagoryWiseTestList != null && CatagoryWiseTestList.Count == 0)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //        new MessageBoxControl.MessageBoxChildWindow("", "Please select test which are linked to assigned catagory.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //    result = false;
                    //    return result;
                    //}
                }
            }

            //if (txtReferenceDoctor.Text == "")
            //{
            //    txtReferenceDoctor.SetValidation("Please Enter Referred By");
            //    txtReferenceDoctor.RaiseValidationError();
            //    TabControlMain.SelectedIndex = 0;
            //    result = false;
            //    txtReferenceDoctor.Focus();
            //}
            //else
            //    txtReferenceDoctor.ClearValidationError();
            if (TabTestDetails.Visibility == Visibility.Visible)
            {

                if (dgTestList.ItemsSource == null && (TemplateID == 0 && (richTextBox.Document == null)))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "You Cannot Save Result Entry  Without Test Details Or Template Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW13.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;
                }
            }

            var item5 = from r in SelectedTestList.ToList()
                        where r.ReportType.ToUpper() == "Template".ToUpper()
                        select r;
            if (SelectedTestList.Count == 1 && item5 != null && item5.ToList().Count > 0)
            {
                result = true;
                return result;
            }
            else if (SelectedTestList.Count > 1 && TestList != null && TestList.Count > 0)
            {
                result = true;
                return result;
            }
            else if (SelectedTestList.Count == 1 && item5 != null && item5.ToList().Count == 0 && TestList != null && TestList.Count > 0)
            {
                result = true;
                return result;
            }
            else if (SelectedTestList.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Test To Save Result.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
                ClickedFlag1 = 0;
            }
            return result;
        }

        private void Save()
        {


            string SampleNo = "";
            //by rohinee
            //bool BalencePending = false;
            bool AbnormalTest = false;

            clsAddPathPatientReportBizActionVO BizAction = new clsAddPathPatientReportBizActionVO();
            BizAction.MasterList = new List<clsPathOrderBookingDetailVO>();
            //if (CatagoryWiseTestList.ToList().Count > 0 || SelectedTestList.Where(z => z.ReportType == "Template").ToList().Count == 1)
            //{
            WaitIndicator indicator = new WaitIndicator();
            List<bool> Validation = new List<bool>();

            indicator.Show();
            try
            {
                List<clsPathPatientReportVO> LstOrder = new List<clsPathPatientReportVO>();
                BizAction.OrderList = new List<clsPathPatientReportVO>();
                BizAction.OrderPathPatientReportList = new clsPathPatientReportVO();

                if (OrderList != null && OrderList.Count > 0)
                {
                    foreach (clsPathPatientReportVO item in OrderList)
                    {
                        foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                        {
                            if (item.CategoryID == item1.CategoryID)
                            {
                                LstOrder.Add(item);


                            }
                        }
                    }
                }
                if (SelectedTestList != null && SelectedTestList.Count > 0)
                {
                    foreach (clsPathOrderBookingDetailVO item in SelectedTestList)
                    {
                        foreach (clsPathPatientReportVO item1 in LstOrder)
                        {
                            if (item.TestID == item1.TestID)
                                item1.ReportType = item.ReportType;
                        }
                    }
                }
                if (LstOrder != null && LstOrder.Count > 0)
                    BizAction.OrderList = LstOrder;//OrderList;
                foreach (var item in BizAction.OrderList)
                {
                    if (IsUpdate == true)
                    {
                        item.IsFinalized = true;

                        BizAction.OrderPathPatientReportList.ID = item.OrderID;
                    }
                    else
                        item.IsFinalized = false;
                    if (txtReferenceDoctor.SelectedItem != null)
                    {
                        item.RefDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
                        item.ReferredBy = ((MasterListItem)txtReferenceDoctor.SelectedItem).Description;
                    }
                    item.IsFirstLevel = true;


                    //ReportBillId = item.BillID;
                    //ReportBillNo = item.BillNo;
                }
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromPathology;
                if (CatagoryWiseTestList != null && CatagoryWiseTestList.Count > 0)
                    BizAction.TestList = CatagoryWiseTestList.ToList();





                foreach (clsPathPatientReportVO item in LstOrder)
                {
                    if (item.ReportType.ToUpper() == "Template".ToUpper())
                    {
                        if (((MasterListItem)cmbPathologist1.SelectedItem).ID > 0)
                            item.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID;

                    }
                    else
                    {
                        item.PathologistID1 = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                    }
                }
                if (((MasterListItem)cmbPathologist1.SelectedItem).ID > 0)
                    BizAction.OrderPathPatientReportList.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID;
                //else
                //    BizAction.OrderPathPatientReportList.PathologistID1 = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                if (txtReferenceDoctor.SelectedItem != null)
                {
                    BizAction.OrderPathPatientReportList.ReferredBy = ((MasterListItem)txtReferenceDoctor.SelectedItem).Description;//((MasterListItem)cmbPathologist1.SelectedItem).Description;
                    BizAction.OrderPathPatientReportList.RefDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;//((MasterListItem)cmbPathologist1.SelectedItem).ID;
                }
                if (SelectedTestList.Count > 0)
                {
                    var ListTemplate = from r in SelectedTestList.ToList()
                                       where r.ReportTemplate == true
                                       select r;
                    if (ListTemplate.ToList().Count > 0)
                    {
                        BizAction.OrderPathPatientReportList.ISTEMplate = true;
                    }
                    else
                    {
                        BizAction.OrderPathPatientReportList.ISTEMplate = false;
                    }
                }
                //Added by Anumani on 20.02.2016
                // In order to provide Autoverification of Result entry where the ResultValues lies in the Normal Range

                bool flag = true;
                string[] NoramlRange2 = null;
                char[] Splitchar = { '-' };

                foreach (var item in BizAction.TestList)
                {

                    foreach (var itemTest in BizAction.TestList)
                    {
                        //if (BizAction.TestList.Where(z => z.PathoTestID.Equals(item.PathoTestID)).Any())
                        if (itemTest.PathoTestID == item.PathoTestID && itemTest.SampleNo == item.SampleNo)
                        {

                            NoramlRange2 = itemTest.NormalRange.Split(Splitchar);

                            if (itemTest.IsNumeric == true && itemTest.IsMachine == false)
                            {
                                if (Convert.ToDouble(itemTest.ResultValue) >= (itemTest.LowReffValue) && Convert.ToDouble(itemTest.ResultValue) <= (itemTest.HighReffValue))
                                {

                                    itemTest.IsFirstLevel = true;
                                    itemTest.IsSecondLevel = true;
                                    //   TestList.IsFinalize = true;

                                    // BizAction.OrderList

                                }
                                else
                                {
                                    itemTest.IsFirstLevel = true;
                                    itemTest.IsSecondLevel = false;
                                    //    TestList.IsFinalize = false;
                                    //flag = false;

                                    break;

                                }
                            }
                            else if (itemTest.IsNumeric == true && itemTest.IsMachine == true)  //by Rohini
                            {
                                bool IsValid = itemTest.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                                if (IsValid)
                                {
                                    if (Convert.ToDouble(itemTest.ResultValue) >= (itemTest.LowReffValue) && Convert.ToDouble(itemTest.ResultValue) <= (itemTest.HighReffValue))
                                    {

                                        itemTest.IsFirstLevel = true;
                                        itemTest.IsSecondLevel = true;                                       
                                    }
                                    else
                                    {
                                        itemTest.IsFirstLevel = true;
                                        itemTest.IsSecondLevel = false;
                                        
                                        break;

                                    }
                                }
                                else
                                {
                                    itemTest.IsFirstLevel = true;
                                    itemTest.IsSecondLevel = true;
                                    
                                }
                            }
                            else
                            {
                                if (item.ApColor == "/PalashDynamics;component/Icons/orange.png")
                                {
                                    itemTest.IsFirstLevel = true;
                                    itemTest.IsSecondLevel = false;
                                    // TestList.IsFinalize = true;
                                    break;


                                }
                                else
                                {
                                    itemTest.IsFirstLevel = true;
                                    itemTest.IsSecondLevel = true;
                                    //    TestList.IsFinalize = false;   
                                }
                            }
                        }
                    }
                }
                
                foreach (var item11 in BizAction.OrderList)
                {
                   // bool flag1 = false;
                    foreach (var item1 in BizAction.TestList)
                    {
                        //if (!flag1)
                        //{
                            if (item1.PathoTestID == item11.TestID && item1.SampleNo == item11.SampleNo)
                            {
                                if (item1.IsSecondLevel == true)
                                {
                                    item11.IsSecondLevel = true;
                                }
                                else
                                {
                                    item11.IsSecondLevel = false;
                                    //flag1 = true;
                                    break;
                                }
                            }
                        //}
                    }
                    

                   
                }

                //ends
                //BizAction.OrderPathPatientReportList.IsFinalized = false;
                BizAction.OrderPathPatientReportList.IsFirstLevel = true;
                BizAction.OrderPathPatientReportList.IsResultModified = IsResultEntryModified;  // Added in Order to note whether the result entry is modified after getting approved.
                BizAction.OrderPathPatientReportList.OrderID = TemplateOrderID;
                BizAction.OrderPathPatientReportList.PathOrderBookingDetailID = TemplatePathOrderBookingDetailID;
                BizAction.OrderPathPatientReportList.TestID = TemplateTestID;
                if (TemplateSampleNo != null)
                    BizAction.OrderPathPatientReportList.SampleNo = Convert.ToString(TemplateSampleNo);
                if (TemplateSampleCollectionDatetime != null)
                    BizAction.OrderPathPatientReportList.SampleCollectionTime = Convert.ToDateTime(TemplateSampleCollectionDatetime);
                BizAction.OrderPathPatientReportList.Status = true;
                if (!String.IsNullOrEmpty(richTextBox.Html))
                    BizAction.OrderPathPatientReportList.TemplateDetails.Template = richTextBox.Html;
                if (TemplateID != 0)
                    BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID = TemplateID;



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        string TestID = "";
                        

                        List<clsPathOrderBookingDetailVO> objList = new List<clsPathOrderBookingDetailVO>();
                        objList.AddRange(((clsAddPathPatientReportBizActionVO)arg.Result).MasterList);
                        if (IsUpdate == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Result Entry Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                            msgW1.Show();
                            ClickedFlag1 = 0;
                            CatagoryWiseTestList.Clear();
                            CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();
                            if (objList != null)
                            {
                                if (objList.Count > 0)
                                {
                                    foreach (var item in objList)
                                    {
                                        if (item.Status == true)
                                        {
                                            //if (item.Status == true)
                                            //{
                                            //SampleNo = SampleNo + item.Description;
                                            //SampleNo = SampleNo + ",";

                                            SampleNo = SampleNo + item.SampleNo;
                                            SampleNo = SampleNo + ",";

                                            TestID = TestID + item.TestID;
                                            TestID = TestID + ",";


                                            //}
                                        }
                                    }
                                    if (SampleNo.EndsWith(","))
                                        SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                                    if (TestID.EndsWith(","))
                                        TestID = TestID.Remove(TestID.Length - 1, 1);

                                }
                            }
                            //by rohinee===============
                            if (((clsAddPathPatientReportBizActionVO)arg.Result).OrderList != null)
                            {
                                var testAbnormal = from r in ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList
                                                   where r.IsAbnormal == 1
                                                   select r;
                                //var testBalence = from r in ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList
                                //                  where r.IsBalence == 1
                                //                  select r;
                                if (testAbnormal.ToList().Count > 0)
                                {
                                    AbnormalTest = true;
                                }
                                else
                                    AbnormalTest = false;
                                //if (testBalence.ToList().Count > 0)
                                //{
                                //    BalencePending = true;
                                //}
                                //else
                                //    BalencePending = false;
                                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 0)   //by rohini dated 17/1/17 auto report only for opd patients
                                {
                                    PrintPathologyReport(SampleNo, AbnormalTest, TestID);
                                }
                            }
                            indicator.Close();
                            //

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Result Entry Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //  msgW1.Show();

                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                            msgW1.Show();
                            ClickedFlag1 = 0;
                            CatagoryWiseTestList.Clear();
                            CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();

                            //bool ResultStatus = false;
                            if (objList != null)
                            {
                                if (objList.Count > 0)
                                {
                                    foreach (var item in objList)
                                    {
                                        //if (item.FirstLevel == true && item.SecondLevel == true)  //by rohinee 
                                        //{
                                        //SampleNo = SampleNo + item.Description;
                                        //SampleNo = SampleNo + ",";

                                        SampleNo = SampleNo + item.SampleNo;
                                        SampleNo = SampleNo + ",";

                                        TestID = TestID + item.TestID;
                                        TestID = TestID + ",";
                                        //}
                                    }
                                    if (SampleNo.EndsWith(","))
                                        SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                                    if (TestID.EndsWith(","))
                                        TestID = TestID.Remove(TestID.Length - 1, 1);
                                }
                            }

                            //by rohinee===============
                            if (((clsAddPathPatientReportBizActionVO)arg.Result).OrderList != null)
                            {
                                var testAbnormal = from r in ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList
                                                   where r.IsAbnormal == 1
                                                   select r;
                                //var testBalence = from r in ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList
                                //                  where r.IsBalence == 1
                                //                  select r;
                                if (testAbnormal.ToList().Count > 0)
                                {
                                    AbnormalTest = true;
                                }
                                else
                                    AbnormalTest = false;
                                //if (testBalence.ToList().Count > 0)
                                //{
                                //    BalencePending = true;
                                //}
                                //else
                                //    BalencePending = false;
                                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 0)   //by rohini dated 17/1/17 auto report only for opd patients
                                {
                                    PrintPathologyReport(SampleNo, AbnormalTest, TestID);
                                }
                            }
                            indicator.Close();
                            //

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Adding Result Entry .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                indicator.Close();
            }
            catch (Exception ex)
            {
                indicator.Close();
                throw;
            }
            //}
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            //if (result == MessageBoxResult.Yes)
            //{
            objAnimation.Invoke(RotationType.Backward);
            CmdResultEntry.Visibility = Visibility.Visible;
            lblresult.Visibility = Visibility.Collapsed;
            GrpColor.Visibility = Visibility.Collapsed;
            lblnormal.Visibility = Visibility.Collapsed;
            txtNormalValue.Visibility = Visibility.Collapsed;
            //lblmin.Visibility = Visibility.Collapsed;
            //txtMinValue.Visibility = Visibility.Collapsed;
            lblmax.Visibility = Visibility.Collapsed;
            txtMaxValue.Visibility = Visibility.Collapsed;
            cmdSave.Visibility = Visibility.Collapsed;
            SelectedTestList.Clear();
            CatagoryWiseTestList.Clear();
            CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();
            SelectedTestList = new List<clsPathOrderBookingDetailVO>();
            ClearData();
            FillOrderBookingList();
            CmdMachinlinking.Visibility = Visibility.Collapsed;
            //}
            //    else
            //    {

            //    }
        }



        private void PrintPathologyReport(string SampleNo, bool Abnormal, string TestID)
        {
            bool IsPrinted = false;

            string ResultID;
            if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ReportTemplate == true)
            {
                #region commented_on_5.04.2016
                //    //BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;
                //    //long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                //    //List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                //    //list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                //    ////var ReferralDoctorSignature = from r in list
                //    ////                              where r.IsChecked = true
                //    ////                              select r;
                //    ////if (ReferralDoctorSignature != null)
                //    ////{
                //    ////    ISRferalDoctorSignature = 1;
                //    ////}
                //    ////else
                //    //ISRferalDoctorSignature = 0;
                //    //string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&empid=" + empid;
                //    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                //    # endregion

                //                     BillNumber =BillNo;
                //                // Added by Anumani on 21/03/2016
                //                IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                //                ResultID = BillId.ToString();

                //                //
                //                // Commented in order to get the BillId of Selected row
                //                //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                //                //{
                //                //    foreach (var item in lstTest)
                //                //    {
                //                //        ResultID = ResultID + item.PathPatientReportID;
                //                //        ResultID = ResultID + ",";
                //                //    }s
                //                //    if (ResultID.EndsWith(","))
                //                //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                //                //}
                #endregion
            }
            else
            {
                BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;

                //  IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                //  ResultID = ReportBillId.ToString();
                ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID.ToString();


                //
                //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                //{
                //    foreach (var item in SelectedTestDetails)
                //    {
                //        ResultID = ResultID + item.BillNo;
                //        ResultID = ResultID + ",";
                //    }

                //    if (ResultID.EndsWith(","))
                //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);

                //}
                //  }


                //if (dgTest.SelectedItem != null)
                //{
                //    if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                //    {
                //        foreach (var item in SelectedTestDetails)
                //        {
                //            SampleNo = SampleNo + item.SampleNo;
                //            SampleNo = SampleNo + ",";
                //        }
                //        if (SampleNo.EndsWith(","))
                //            SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                //    }
                //}

                List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                //var ReferralDoctorSignature = from r in list
                //                              where r.IsChecked = true
                //                              select r;
                //if (ReferralDoctorSignature != null)
                //{
                //    ISRferalDoctorSignature = 1;
                //}
                //else
                ISRferalDoctorSignature = 0;


                //if (IsPrinted == false)
                //{
                IsDuplicate = false;
                //}
                //else
                //{
                //    IsDuplicate = true;
                //}

                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                //if ((SampleNo != "") && (Abnormal == false) && ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))))
                //{
                if ((SampleNo != "") && (Abnormal == false) && ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1)))  //by rohini dont change) (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))
                {
                                   
                    string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo + "&TestID=" + TestID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                //else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0 && (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false && Abnormal == true))
                //{
                else if ((Abnormal == true) && ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0) && ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) && (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External != 1))) //by rohini dont change  (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) ||((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))  && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Cannot Be Printed, Patient Is Having Balance Amount And Test Reports Are Abnormal.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }
                //else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0 && (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))
                //{
                else if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0) && ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) && (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External != 1)) //by rohini dont change  (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) ||((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))  && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)
                {

                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Cannot Be Printed, Patient Is Having Balance Amount.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }
                else if (Abnormal == true)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Cannot Be Printed, Test Reports Are Abnormal.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }

            }


        }

        //private void GetPatientDetailsInHtml(long ResultId, bool IsFinalize)
        //{

        //    clsPathoResultEntryPrintDetailsBizActionVO BizAction = new clsPathoResultEntryPrintDetailsBizActionVO();

        //    BizAction.ID = ResultId;
        //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    BizAction.IsDelivered = 0;
        //    BizAction.ResultIds = "";
        //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

        //    Client1.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (arg.Result != null)
        //            {
        //                clsPathoResultEntryPrintDetailsVO PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
        //                PatientResultEntry = ((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;




        //                strPatInfo = new StringBuilder();

        //                strPatInfo.Append(PatientResultEntry.PatientInfoHTML);

        //                strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
        //                strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy"));
        //                strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.Salutation.ToString() + " " + PatientResultEntry.PatientName.ToString());

        //                strPatInfo = strPatInfo.Replace("[AgeYear]", "    :" + PatientResultEntry.AgeYear.ToString() + "yrs");
        //                strPatInfo = strPatInfo.Replace("[AgeMonth]", "    :" + PatientResultEntry.AgeMonth.ToString() + "Mnt");
        //                strPatInfo = strPatInfo.Replace("[AgeDate]", "    :" + PatientResultEntry.AgeDate.ToString() + "Dys");
        //                strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());
        //                if (PatientResultEntry.ShowinPathoReport == false)
        //                {
        //                    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + "Dr. " + PatientResultEntry.ReferredBy.ToString());
        //                }
        //                else
        //                {
        //                    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + " ");
        //                }
        //                strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.SampleNo.ToString());

        //                strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt"));

        //                strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
        //                strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
        //                strDoctorPathInfo = new StringBuilder();
        //                strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);

        //                byte[] imageBytes = null;
        //                string imageBase64 = string.Empty;
        //                string imageSrc = string.Empty;

        //                if (PatientResultEntry.Pathologist1 != null)
        //                {
        //                    if (PatientResultEntry.Signature1 != null)
        //                    {
        //                        imageBytes = PatientResultEntry.Signature1;
        //                        imageBase64 = Convert.ToBase64String(imageBytes);
        //                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
        //                    }
        //                    else
        //                    {
        //                        strDoctorPathInfo.Replace("[%Image1%]", "hidden");
        //                    }

        //                    strDoctorPathInfo.Replace("[%Pathalogist4%]", PatientResultEntry.Pathologist1);
        //                    strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education1);
        //                }
        //                else
        //                {
        //                    strDoctorPathInfo.Replace("[%Pathalogist4%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Image1%]", "hidden");
        //                }
        //                //-------------------------------------------------------------------------------------------

        //                if (PatientResultEntry.Pathologist2 != null)
        //                {
        //                    if (PatientResultEntry.Signature2 != null)
        //                    {
        //                        imageBytes = PatientResultEntry.Signature2;
        //                        imageBase64 = Convert.ToBase64String(imageBytes);
        //                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
        //                    }
        //                    else
        //                    {
        //                        strDoctorPathInfo.Replace("[%Image2%]", "hidden");
        //                    }

        //                    strDoctorPathInfo.Replace("[%Pathalogist3%]", PatientResultEntry.Pathologist2);
        //                    strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Education2);
        //                }
        //                else
        //                {
        //                    strDoctorPathInfo.Replace("[%Pathalogist3%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Education3%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Image2%]", "hidden");
        //                }
        //                //-------------------------------------------------------------------------------------------

        //                if (PatientResultEntry.Pathologist3 != null)
        //                {
        //                    if (PatientResultEntry.Signature3 != null)
        //                    {
        //                        imageBytes = PatientResultEntry.Signature3;
        //                        imageBase64 = Convert.ToBase64String(imageBytes);
        //                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
        //                    }
        //                    else
        //                    {
        //                        strDoctorPathInfo.Replace("[%Image3%]", "hidden");
        //                    }

        //                    strDoctorPathInfo.Replace("[%Pathalogist2%]", PatientResultEntry.Pathologist3);
        //                    strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Education3);
        //                }
        //                else
        //                {
        //                    strDoctorPathInfo.Replace("[%Pathalogist2%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Education2%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Image3%]", "hidden");
        //                }
        //                //-------------------------------------------------------------------------------------------

        //                if (PatientResultEntry.Pathologist4 != null)
        //                {
        //                    if (PatientResultEntry.Signature4 != null)
        //                    {
        //                        imageBytes = PatientResultEntry.Signature4;
        //                        imageBase64 = Convert.ToBase64String(imageBytes);
        //                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
        //                    }
        //                    else
        //                    {
        //                        strDoctorPathInfo.Replace("[%Image4%]", "hidden");
        //                    }

        //                    strDoctorPathInfo.Replace("[%Pathalogist1%]", PatientResultEntry.Pathologist4);
        //                    strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Education4);
        //                }
        //                else
        //                {
        //                    strDoctorPathInfo.Replace("[%Pathalogist1%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Education1%]", string.Empty);
        //                    strDoctorPathInfo.Replace("[%Image4%]", "hidden");
        //                }
        //                richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
        //                richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());
        //                PrintReport(ResultId, ISFinalize, strPatInfo.ToString(), strDoctorPathInfo.ToString());
        //            }
        //        }
        //        else
        //        {

        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //              new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgW1.Show();
        //        }
        //    };

        //    Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client1.CloseAsync();

        //}

        private void PrintReport(long ResultID, bool IsFinalize, string PatientInfoString, string DoctorInfoString)
        {

            if (TabTemplateDetails.Visibility == Visibility.Visible && PrintTemplate == true)
            {
                richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", PatientInfoString);
                richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", DoctorInfoString);

                var viewManager = new C1RichTextViewManager
                {
                    Document = richTextBox.Document,
                    PresenterInfo = richTextBox.ViewManager.PresenterInfo
                };
                var print = new PrintDocument();

                int presenter = 0;

                print.PrintPage += (s, printArgs) =>
                {
                    var element = (FrameworkElement)HeaderTemplate.LoadContent();
                    int CC = print.PrintedPageCount;
                    element.DataContext = viewManager.Presenters[presenter];
                    printArgs.PageVisual = element;
                    printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;

                };

                print.Print("A Christmas Carol");
            }
            else
            {
                if (ResultID > 0)
                {
                    BillNum = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                    long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                    List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                    list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                    var ReferralDoctorSignature = from r in list
                                                  where r.IsChecked = true
                                                  select r;
                    if (ReferralDoctorSignature != null)
                    {
                        ISRferalDoctorSignature = 1;
                    }
                    else
                        ISRferalDoctorSignature = 0;
                    string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
        }

        private void FetchTemplate()
        {
            int IsFormTemplate;
            try
            {
                clsGetPathoViewTemplateBizActionVO BizAction = new clsGetPathoViewTemplateBizActionVO();
                BizAction.Template = new clsPathoTestTemplateDetailsVO();
                if (cmbTemplate.SelectedItem != null)
                    BizAction.TemplateID = ((MasterListItem)cmbTemplate.SelectedItem).ID;
                IsFormTemplate = (int)((MasterListItem)cmbTemplate.SelectedItem).Value;
                if (IsFormTemplate == 2)
                {
                    BizAction.Flag = 1;
                }
                else if (IsFormTemplate == 4)
                {
                    BizAction.Flag = 2;
                }
                if (cmbGender.SelectedItem != null)
                    BizAction.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
                else
                    BizAction.GenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoViewTemplateBizActionVO)arg.Result).Template != null)
                        {
                            richTextBox.Html = " ";
                            richTextBox.Html = ((clsGetPathoViewTemplateBizActionVO)arg.Result).Template.Template;
                            TemplateID = ((clsGetPathoViewTemplateBizActionVO)arg.Result).Template.ID;
                            string rtbstring = string.Empty;
                            string styleString = string.Empty;

                            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                            {
                                rtbstring = richTextBox.Html;
                                rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                                rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
                                richTextBox.Html = rtbstring;
                                TemplateContent = rtbstring;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillTemplate()
        {
            try
            {
                FillTemplateComboBoxInPathoResultEntryBizActionVO BizAction = new FillTemplateComboBoxInPathoResultEntryBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                if ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem != null)
                    BizAction.TestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                if (cmbPathologist1.SelectedItem != null)
                    BizAction.Pathologist = ((MasterListItem)cmbPathologist1.SelectedItem).ID;
                if (cmbGender.SelectedItem != null)
                    BizAction.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
                else
                    BizAction.GenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((FillTemplateComboBoxInPathoResultEntryBizActionVO)e.Result).MasterList);
                        cmbTemplate.ItemsSource = null;
                        cmbTemplate.ItemsSource = objList;
                        cmbTemplate.SelectedItem = objList[0];
                        if (TemplateID != null)
                        {
                            cmbTemplate.SelectedValue = TemplateID;
                        }
                    }
                   // indicator.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void PrintReport(long ResultID)
        {
            if (SelectedDetails.ReportTemplate == false)
            {
                BillNum = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                var ReferralDoctorSignature = from r in list
                                              where r.IsChecked = true
                                              select r;
                if (ReferralDoctorSignature != null)
                {
                    ISRferalDoctorSignature = 1;
                }
                else
                    ISRferalDoctorSignature = 0;
                string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNum + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void ViewUploadedReport()
        {
            if (SelectedDetails.IsCompleted == true)
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        HtmlPage.Window.Invoke("OpenReport", new string[] { SelectedDetails.SourceURL });
                    }
                };
                client.UploadReportFileAsync(SelectedDetails.SourceURL, SelectedDetails.Report);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This Test's Report Is Not Uploaded. Please Upload The Report Then Click On Preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }

        }

        private bool Validation()
        {
            bool IsValidate = true;
            if (dgTest1.SelectedItem != null)
            {
                clsPathOrderBookingDetailVO objVO = dgTest1.SelectedItem as clsPathOrderBookingDetailVO;
                if (objVO.IsSampleCollected == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Not Collected For " + objVO.TestName + " TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsSampleDispatch == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Not Dispatched For " + objVO.TestName + " TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsSampleReceive == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Not Received For " + objVO.TestName + " TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.SampleAcceptRejectStatus == 0)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Not Accepted For " + objVO.TestName + "TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.SampleAcceptRejectStatus == 2)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Rejected For " + objVO.TestName + "TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsOutSourced == true)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is  Outsourced For " + objVO.TestName + "TEST. \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.DispatchToID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Sample Is Dispatched To Other Clinic You Can Not Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsServiceRefunded)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Service is Refunded!, \n You Cannot Do Result Entry For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.SecondLevel != true)  //by rohini FOR EDIT TEST WITH NORMAL TEST NOT ALLOWDED
                {
                    foreach (var item in SelectedTestList)
                    {
                        if (item.SecondLevel == true && objVO.ID != item.ID)
                        {
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can Not Select Unauthorized Test With Authorized Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            IsValidate = false;
                        }
                    }
                }
                else if (objVO.SecondLevel == true)  //by rohini FOR EDIT TEST WITH NORMAL TEST NOT ALLOWDED
                {
                    foreach (var item in SelectedTestList)
                    {
                        if (item.SecondLevel != true && objVO.ID != item.ID)
                        {
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can Not Select Unauthorized Test With Authorized Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            IsValidate = false;
                        }
                    }
                }
                if (msgW1 != null)
                {
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (lstTest != null && lstTest.Count > 0 && res == MessageBoxResult.OK)
                        {
                            IsValidate = true;
                            lstTest.ToList().Where(z => z.TestID == objVO.TestID).ToList().Where(x => x.IsSelected = false).ToList();
                            collection = new PagedCollectionView(lstTest);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("ReportType"));
                            dgTest1.ItemsSource = null;
                            dgTest1.ItemsSource = collection;
                            dgTest1.SelectedIndex = -1;
                            msgW1 = null;
                        }
                    };
                    msgW1.Show();
                }
            }
            return IsValidate;
        }

        #endregion

        private void cmbTestCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FillTestList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        int ClickedFlag1 = 0;
        private void cmdGetTest_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 += 1;
            if (ClickedFlag1 == 1)
            {
                try
                {
                    if (cmbCategory.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbCategory.SelectedItem).ID > 0)
                        {
                            var List = from r in lstTest
                                       where r.IsSelected == true && r.ReportTemplate == false
                                       select r;
                            if (List.ToList().Count() > 0)
                            {

                                dgTestList.ItemsSource = null;
                                TestList = new ObservableCollection<clsPathoTestParameterVO>();
                                cmdGetTest.IsEnabled = false;
                                SelectedTestParameterValues.Clear();//by rohini for machine values are not getting refreshed
                                GetParameters();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "Select Test To Get Parameter .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ClickedFlag1 = 0;
                            }
                        }
                        else
                        {
                            cmbCategory.TextBox.SetValidation("Please Select Parameter Category");
                            cmbCategory.RaiseValidationError();
                            cmbCategory.Focus();
                            ClickedFlag1 = 0;
                        }
                    }
                    else
                    {
                        ClickedFlag1 = 0;
                    }
                }

                catch (Exception)
                {
                    ClickedFlag1 = 0;
                    throw;
                    throw;
                }
            }
        }

        private void dgTestList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (((clsPathoTestParameterVO)dgTestList.SelectedItem).IsNumeric == true && ((clsPathoTestParameterVO)dgTestList.SelectedItem).IsMachine == false)
            {
                if (e.Column.DisplayIndex == 2)
                {
                    if (!String.IsNullOrEmpty(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue))
                    {
                        if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue.IsItCharacter())
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Result Value in correct Format.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW14.Show();
                            TabControlMain.SelectedIndex = 0;
                        }
                        else
                        {
                            if (SelectedTestParameterValues != null)
                            {
                                if (SelectedTestParameterValues.Count > 0)
                                {
                                    var list = from r in SelectedTestParameterValues
                                               where r.PathoTestID == ((clsPathoTestParameterVO)dgTestList.SelectedItem).PathoTestID && r.ParameterID == ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID
                                               select r;
                                    if (list.ToList().Count > 0)
                                    {
                                        foreach (clsPathoTestParameterVO item in list)
                                        {
                                            foreach (var item1 in SelectedTestParameterValues)
                                            {
                                                if (item1.PathoTestID == item.PathoTestID && item1.ParameterID == item.ParameterID)
                                                {
                                                    item1.ResultValue = item.ResultValue;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue))
                                        {
                                            SelectedTestParameterValues.Add(((clsPathoTestParameterVO)dgTestList.SelectedItem));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue))
                                    {
                                        SelectedTestParameterValues.Add(((clsPathoTestParameterVO)dgTestList.SelectedItem));
                                    }
                                }
                            }
                            Color C = new Color();
                            C.R = 198;
                            C.B = 24;
                            C.G = 15;
                            C.A = 255;

                            string[] NoramlRange1 = null;
                            char[] Splitchar = { '-' };
                            NoramlRange1 = ((clsPathoTestParameterVO)dgTestList.SelectedItem).NormalRange.Split(Splitchar);

                            //uncommented on 16.02.2016 for color coding
                            if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) > (Convert.ToDouble(NoramlRange1[1])))
                            {
                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathomaxColorCode;
                                DataGridColumn column = dgTestList.Columns[2];
                                FrameworkElement fe = column.GetCellContent(dgTestList.SelectedItem);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.FontWeight = FontWeights.Bold;
                                }


                            }
                            else if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) < (Convert.ToDouble(NoramlRange1[0])))
                            {
                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathominColorCode;
                                DataGridColumn column = dgTestList.Columns[2];
                                FrameworkElement fe = column.GetCellContent(dgTestList.SelectedItem);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.FontWeight = FontWeights.Bold;
                                }
                            }

                            else
                            {
                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathonormalColorCode;
                                DataGridColumn column = dgTestList.Columns[2];
                                FrameworkElement fe = column.GetCellContent(dgTestList.SelectedItem);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.FontWeight = FontWeights.Normal;
                                }

                            }

                            //
                        }
                    }
                }
            }
            else
            {
                if (SelectedTestParameterValues != null)
                {
                    if (SelectedTestParameterValues.Count > 0)
                    {
                        var list = from r in SelectedTestParameterValues
                                   where r.PathoTestID == ((clsPathoTestParameterVO)dgTestList.SelectedItem).PathoTestID && r.ParameterID == ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID
                                   select r;
                        if (list.ToList().Count > 0)
                        {
                            foreach (clsPathoTestParameterVO item in list)
                            {
                                foreach (var item1 in SelectedTestParameterValues)
                                {
                                    if (item1.PathoTestID == item.PathoTestID && item1.ParameterID == item.ParameterID)
                                    {
                                        item1.ResultValue = item.ResultValue;

                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue != "")
                            {
                                SelectedTestParameterValues.Add(((clsPathoTestParameterVO)dgTestList.SelectedItem));
                            }
                        }
                    }
                    else
                    {
                        if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue != "")
                        {
                            SelectedTestParameterValues.Add(((clsPathoTestParameterVO)dgTestList.SelectedItem));
                        }
                    }
                }
            }
        }

        private void HelpValueDetails_Click(object sender, RoutedEventArgs e)
        {
            HelpValueDetails WinHelpValue = new HelpValueDetails();
            WinHelpValue.ParameterID = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
            WinHelpValue.IsFromAuthorization = false;//by rohinee
            WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
            WinHelpValue.Show();
        }

        void WinHelpValue_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            String ResultValues = null;
            HelpValueDetails ObjHelpValueDetails = (HelpValueDetails)sender;
            if (ObjHelpValueDetails.SelectedItems != null)
            {
                foreach (var item in ObjHelpValueDetails.SelectedItems)
                {
                    if (TestList.Where(Items => Items.ParameterID == item.ParameterID).Any() == true)
                    {
                        ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue = item.HelpValue;
                        if (item.IsAbnormal == true)
                        {
                            ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = "/PalashDynamics;component/Icons/orange.png";

                        }


                        else
                        {
                            ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = "/PalashDynamics;component/Icons/green.png";

                        }

                    }


                }
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTemplate = true;
            SaveTemplate = CheckValidation();

            if (SaveTemplate == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Result Entry?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
            //    if (dgTest1.SelectedItem != null)
            //    {
                //by rohini
                var  List = from r in SelectedTestList
                            where r.FirstLevel == true && r.SecondLevel == true && r.IsDelivered == false && ((IApplicationConfiguration)App.Current).CurrentUser.UserAditionalRights.IsEditAfterFinalized == true  
                             select r;

                var List2 = from r in SelectedTestList
                           where r.FirstLevel == true && r.SecondLevel == true && ((IApplicationConfiguration)App.Current).CurrentUser.UserAditionalRights.IsEditAfterFinalized != true
                           select r;
                var List3 = from r in SelectedTestList
                           where r.FirstLevel == true && r.SecondLevel == true && r.IsDelivered == true && ((IApplicationConfiguration)App.Current).CurrentUser.UserAditionalRights.IsEditAfterFinalized == true
                           select r;
                if (List.ToList().Count > 0)
                {
                    //if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).FirstLevel == true && ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SecondLevel == true && ((IApplicationConfiguration)App.Current).CurrentUser.UserAditionalRights.IsEditAfterFinalized == true)  //by rohini
                    //{
                        IsUpdate = true;
                        PasswordWindow Win = new PasswordWindow();
                        Win.clsRemarkHistoryVOList = List.ToList();//SelectedTestList;
                        Win.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        Win.OrderUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                        Win.OnOkButton_Click += new RoutedEventHandler(OnOkButton_Click);
                        Win.Show();
                }
                else if (List2.ToList().Count > 0) 
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "User Not Allowed To Edit Results After Authorization.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (List3.ToList().Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "User Not Allowed To Edit Results After Report Delivered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    IsUpdate = false;
                    Save();
                }
               
                //}
            }
        }

        void OnOkButton_Click(object sender, RoutedEventArgs e)
        {               
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to update the Result Entry?";
            MessageBoxControl.MessageBoxChildWindow msgW2 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW2.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
            msgW2.Show();
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                IsResultEntryModified = true;
                Save();
            }
            //throw new NotImplementedException();
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = 0;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = 0;
                ((IApplicationConfiguration)App.Current).SelectedPatient.Gender = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dgTestList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                int index = dgTestList.CurrentColumn.DisplayIndex;
            }
        }

        private void dgTestList_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    e.Handled = true;
                    int i = dgTestList.SelectedIndex;
                    dgTestList.SelectedIndex = i;
                    dgTestList.Focus();
                    dgTestList.BeginEdit();
                    dgTestList.CurrentColumn = dgTestList.Columns[2];
                    break;

                case Key.Up:
                    e.Handled = true;
                    int j = dgTestList.SelectedIndex;
                    dgTestList.SelectedIndex = j;
                    dgTestList.Focus();
                    dgTestList.BeginEdit();
                    dgTestList.CurrentColumn = dgTestList.Columns[2];
                    break;

                default:
                    break;
            }
        }

        private void cmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTemplate.SelectedItem != null && cmbPathologist1.SelectedItem != null)
            {
                if (((MasterListItem)cmbTemplate.SelectedItem).ID != 0 && ((MasterListItem)cmbPathologist1.SelectedItem).ID != 0)
                {
                    FetchTemplate();
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                try
                {
                    if (dgTest1.SelectedItem != null)
                    {
                        clsPathOrderBookingDetailVO objVO = dgTest1.SelectedItem as clsPathOrderBookingDetailVO;
                        if (((CheckBox)sender).IsChecked == false)
                        {
                            ClickedFlag1 = 0;
                            if (!String.IsNullOrEmpty(richTextBox.Html))
                            {
                                TemplateContent = richTextBox.Html;
                            }
                            #region Remove From Selected Test List
                            clsPathOrderBookingDetailVO obj;
                            if (objVO != null)
                            {
                                obj = SelectedTestList.Where(z => z.TestID == objVO.TestID && z.SampleNo == objVO.SampleNo).FirstOrDefault();
                                SelectedTestList.Remove(obj);
                            }

                            #endregion

                            #region Check Box Unchecked
                            if (OrderList != null && OrderList.Count > 0)
                            {
                                for (int i = OrderList.Count - 1; i >= 0; i--)
                                {
                                    if (OrderList[i].TestID == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID && OrderList[i].SampleNo == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo)
                                    {
                                        OrderList.RemoveAt(i);
                                    }
                                }
                            }
                            if (TestList != null && TestList.Count > 0)
                            {
                                for (int i = TestList.Count - 1; i >= 0; i--)
                                {
                                    if (TestList[i].PathoTestID == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID && TestList[i].SampleNo == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo)
                                    {
                                        TestList.RemoveAt(i);
                                    }
                                }
                            }
                            if (SelectedTestParameterValues != null && SelectedTestParameterValues.ToList().Count > 0)
                            {
                                for (int i = SelectedTestParameterValues.Count - 1; i >= 0; i--)
                                {
                                    if (SelectedTestParameterValues[i].PathoTestID == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID && SelectedTestParameterValues[i].SampleNo == ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo)
                                    {
                                        SelectedTestParameterValues.RemoveAt(i);
                                    }
                                }
                            }
                            if (TestList != null && TestList.Count > 0)
                            {
                                PagedCollectionView Collection = new PagedCollectionView(TestList);
                                //Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));
                                Collection.GroupDescriptions.Add(new PropertyGroupDescription("TestAndSampleNO"));  //by rohini for sample no

                                dgTestList.ItemsSource = null;
                                dgTestList.ItemsSource = Collection;
                            }
                            cmbPathologist1.SelectedValue = (long)0;
                            if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ReportTemplate == true)
                            {
                                TabTestDetails.IsSelected = true;
                                TabTestDetails.Focus();
                                TabTemplateDetails.Visibility = Visibility.Collapsed;
                                //richTextBox.Html = null;
                                //richTextBox = new C1RichTextBox();

                            }
                            else
                            {
                                TabTestDetails.Visibility = Visibility.Visible;
                            }
                            if (SelectedTestList != null && SelectedTestList.Count > 0)
                            {
                                var result = from r in SelectedTestList
                                             where r.ReportType.ToUpper() == "Template".ToUpper()
                                             select r;
                                if (result != null && result.ToList().Count > 0)
                                {
                                    if (richTextBox.Html == "<html><head></head><body><p>[%PATIENTINFO%][%DOCTORINFO%]</p></body></html>")
                                    {
                                        richTextBox.Html = TemplateContent;
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {

                            richTextBox.Html = HTMLContent; // changed on 17122016
                            ClickedFlag1 = 0;  //by rohini 
                            #region Check box Checked
                            #region Add To Selected Test List
                            #region AutoVerification and Printing Report
                            // added on 26.04.2016

                            if (((CheckBox)sender).IsChecked == true)
                            {

                                SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                            }
                            else
                            {
                                SelectedTestDetails.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                            }
                            #endregion

                            if (SelectedTestList != null)
                            {

                                SelectedTestList.Add(objVO);
                            }
                            #endregion

                            #region validation For Single Template Test
                            var TestDtls = from r in SelectedTestList
                                           where r.ReportTemplate == true
                                           select r;
                            if (TestDtls.ToList().Count > 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("PALASH", "Select Only One Test Template At A Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ((CheckBox)sender).IsChecked = false;
                                clsPathOrderBookingDetailVO obj;
                                if (objVO != null)
                                {
                                    obj = SelectedTestList.Where(z => z.TestID == objVO.TestID && z.SampleNo == objVO.SampleNo).FirstOrDefault();
                                    SelectedTestList.Remove(obj);
                                }
                            }
                            #endregion

                            if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ReportTemplate == true)
                            {
                                #region For Template
                                #region For Test Having Report Template and Result Entry Done
                                if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).IsResultEntry == true)
                                {

                                    if (richTextBox.ViewManager == null)
                                    {
                                        IsTempate = true;
                                        TabTemplateDetails.Visibility = Visibility.Visible;
                                        TabControlMain.SelectedIndex = 1;
                                        PrintTemplate = true;
                                        TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo.ToString();
                                        TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                        TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                        TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                        TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        GetTemplateResultEntry();
                                    }
                                    if (richTextBox.ViewManager != null)  //by rohini
                                    {
                                        IsTempate = true;
                                        TabTemplateDetails.Visibility = Visibility.Visible;
                                        TabControlMain.SelectedIndex = 1;
                                        PrintTemplate = true;
                                        TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo.ToString();
                                        TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                        TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                        TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                        TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        GetTemplateResultEntry();
                                    }

                                    else
                                    {
                                        GetTemplateResultEntry();
                                        TabTemplateDetails.Visibility = Visibility.Visible;
                                        TabControlMain.SelectedIndex = 1;
                                    }
                                    ////else
                                    ////{
                                    ////    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    ////        new MessageBoxControl.MessageBoxChildWindow("", "Already One template Result value is Added.It will Remove the previous result value.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    ////    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    ////    {
                                    ////        if (res == MessageBoxResult.No)
                                    ////        {
                                    ////            IsTempate = true;
                                    ////            TabTemplateDetails.Focus();
                                    ////            TabTemplateDetails.IsSelected = true;
                                    ////            TabTemplateDetails.Visibility = Visibility.Visible;
                                    ////            TabControlMain.SelectedIndex = 1;
                                    ////            PrintTemplate = true;
                                    ////            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                    ////            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                    ////            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                    ////            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                    ////            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                    ////            TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                    ////            GetTemplateResultEntry();
                                    ////            foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                                    ////            {
                                    ////                if (item.ReportTemplate == true)
                                    ////                {
                                    ////                    if (item.TestID == TestTemplateID)
                                    ////                        item.IsSelected = true;
                                    ////                    else
                                    ////                        item.IsSelected = false;
                                    ////                }
                                    ////            }
                                    ////            dgTest1.UpdateLayout();
                                    ////            dgTest1.Focus();
                                    ////            ((CheckBox)sender).IsChecked = true;
                                    ////        }
                                    ////        else
                                    ////        {
                                    ////            TabTemplateDetails.Visibility = Visibility.Visible;
                                    ////            ((CheckBox)sender).IsChecked = false;
                                    ////        }
                                    ////    };
                                    ////    msgW1.Show();
                                    ////}
                                }
                                #endregion
                                else
                                {
                                    if (((MasterListItem)cmbPathologist1.SelectedItem).ID == 0)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("", "Please Select Pathologist For test template", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                        ((CheckBox)sender).IsChecked = false;
                                        cmbPathologist1.IsEnabled = true;
                                        clsPathOrderBookingDetailVO obj;
                                        if (objVO != null)
                                        {
                                            obj = SelectedTestList.Where(z => z.TestID == objVO.TestID && z.SampleNo == objVO.SampleNo).FirstOrDefault();
                                            SelectedTestList.Remove(obj);
                                        }
                                    }
                                    else
                                    {
                                        TabTemplateDetails.Focus();
                                        TabTemplateDetails.IsSelected = true;
                                        TabTemplateDetails.Visibility = Visibility.Visible;
                                        if (richTextBox.ViewManager == null)
                                        {
                                            IsTempate = true;
                                            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                            FillTemplate();
                                            TabTestDetails.Visibility = Visibility.Visible;
                                            TabControlMain.SelectedIndex = 1;
                                            PrintTemplate = true;
                                            TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        }
                                        else if (richTextBox.ViewManager != null)
                                        {
                                            IsTempate = true;
                                            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                            FillTemplate();
                                            TabTestDetails.Visibility = Visibility.Visible;
                                            TabControlMain.SelectedIndex = 1;
                                            PrintTemplate = true;
                                            TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        }
                                        else
                                        {
                                            if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).IsResultEntry == true)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("", "Already One template Result value is Added.It will Remove the previous result value.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                                {
                                                    if (res == MessageBoxResult.No)
                                                    {
                                                        IsTempate = true;
                                                        richTextBox.Html = HTMLContent;
                                                        TemplateID = 0;
                                                        FillTemplate();
                                                        TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                                        TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                                        TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                                        TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                                        TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                                        TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                                        
                                                        foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                                                        {
                                                            if (item.TestID == TestTemplateID && item.ReportTemplate == true)
                                                            {
                                                                item.IsSelected = false;
                                                                break;
                                                            }
                                                        }
                                                        dgTest1.UpdateLayout();
                                                        dgTest1.Focus();
                                                       
                                                    }
                                                    else
                                                    {
                                                        TabTemplateDetails.Visibility = Visibility.Visible;
                                                        ((CheckBox)sender).IsChecked = false;
                                                    }
                                                };
                                                msgW1.Show();
                                            }
                                        }
                                        FillTemplate();
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                //TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                //TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                //TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                //TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                                {
                                    if (item.IsSelected == true && item.ReportTemplate == true)
                                    {
                                        BackPanelTemplate = true;
                                        break;
                                    }

                                }
                                if (BackPanelTemplate == true)
                                {
                                    TabTemplateDetails.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    TabTemplateDetails.Visibility = Visibility.Collapsed;
                                }
                                TabTestDetails.Visibility = Visibility.Visible;
                                TabControlMain.SelectedIndex = 0;
                                IsTempate = false;
                            }
                            if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).IsResultEntry == false)
                                cmbPathologist1.SelectedValue = ((MasterListItem)cmbPathologist1.SelectedItem).ID;
                            else
                                cmbPathologist1.SelectedValue = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).PathologistID;//RefDoctorID;



                            #endregion
                            //by rohini dated 5/12/16 for result entry from dispatched unit only
                            //var itemDispatch = from r in SelectedTestList
                            //                   where r.DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId
                            //                   select r;

                            //if (itemDispatch != null && itemDispatch.ToList().Count != 0)
                            //{
                            //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is dispatched to another Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            //    msgbox.Show();
                            //    ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                            //}
                        }

                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void dgTestList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            indicator.Show();
            e.Row.KeyDown += new KeyEventHandler(Row_KeyDown);
            clsPathoTestParameterVO item = (clsPathoTestParameterVO)e.Row.DataContext;
            string[] NoramlRange2 = null;
            char[] Splitchar = { '-' };
            NoramlRange2 = item.NormalRange.Split(Splitchar);
            List<clsPathoTestParameterVO> HelpValues = new List<clsPathoTestParameterVO>();

            if (item.IsNumeric == false)
            {
                this.dgTestList.Columns[3].IsReadOnly = false;
                if (item.ResultValue == string.Empty && item.IsNumeric ==false)
                {
                    item.ResultValue = Convert.ToString(item.HelpValue1); // by rohini                  
                }
                else
                {
                    item.ResultValue = item.ResultValue; // by rohini 
                }

                if (item.IsAbnoramal == true)
                {
                    item.ApColor = "/PalashDynamics;component/Icons/orange.png";

                }
                else
                {
                    item.ApColor = "/PalashDynamics;component/Icons/green.png";

                }
                //Read Only When Text Value
                item.IsReadOnly = true;
                this.dgTestList.Columns[2].IsReadOnly = true;

            }
            //added on 26.02.2016 to represent whether parameters are obtained from Machines or not     
            if (item.ResultValue.Trim() != string.Empty)
            {
                if (item.IsNumeric == true && item.IsMachine == false)
                {
                    if ((Convert.ToDouble(item.ResultValue) > 0))
                    {
                        item.DeltaCheck = true;
                    }
                    else
                    {
                        item.DeltaCheck = false;
                    }
                }
                else if (item.IsNumeric == true && item.IsMachine == true)
                {
                    bool IsValid = item.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                    if (IsValid)
                    {
                        if ((Convert.ToDouble(item.ResultValue) > 0))
                        {
                            item.DeltaCheck = true;
                        }
                        else
                        {
                            item.DeltaCheck = false;
                        }
                    }
                }
            }
            else
            {
                item.DeltaCheck = false;
            }
            if (item.IsMachine == true)
            {
                if (item.IsSecondLevel == false)   //by rohini for cr point enable after authorization machine result value
                {
                    item.MachineMannual = "Machine";
                    item.IsReadOnly = true;
                    this.dgTestList.Columns[2].IsReadOnly = true;
                    // this.dgTestList.Columns[10].IsReadOnly = true;
                }
                else
                {
                    item.MachineMannual = "Machine";
                    item.IsReadOnly = false;
                    this.dgTestList.Columns[2].IsReadOnly = false;
                    // this.dgTestList.Columns[10].IsReadOnly = true;
                }
               
            }
            else if (item.FormulaID == string.Empty)
            {
                item.MachineMannual = "Manual";
                item.IsReadOnly = false;
                this.dgTestList.Columns[2].IsReadOnly = true;
            }
            else
            {
                item.MachineMannual = "Manual";
                item.IsReadOnly = true;
                this.dgTestList.Columns[2].IsReadOnly = true;
            }
            //

            if (item.ResultValue != "")
            {
                if (item.IsNumeric == true && item.IsMachine == false)
                {
                    #region code for numric and manual values
                    if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                    {
                        if (item.DeltaCheckValue == 0)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            //txt.Content = "N.A";
                            item.DeltacheckString = "N.A";
                            item.DeltaCheck = false;
                        }
                        else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;                           
                            item.DeltacheckString = "Fail";
                            item.DeltaCheck = true;
                        }
                        else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Content = "Pass";
                            item.DeltacheckString = "Pass";
                            item.DeltaCheck = true;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;                     
                        item.DeltacheckString = "N.A";
                        item.DeltaCheck = false;
                    }
                    DeltaResultValue.Clear();
                    if (item.IsReflexTesting == true)
                    {
                        if (!(string.IsNullOrEmpty(item.ResultValue) || item.ResultValue == "0"))
                        {
                            if ((Convert.ToDouble(item.ResultValue) < item.LowReflex) || (Convert.ToDouble(item.ResultValue) > item.HighReflex))
                            {
                                item.ReflexTestingFlag = true;
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Visibility = System.Windows.Visibility.Visible;
                                Image img = new Image();
                                img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                img.Height = 20;
                                img.Width = 20;
                                txt.Content = img;
                            }
                            else
                            {
                                item.ReflexTestingFlag = false;
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;                             
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Visibility = System.Windows.Visibility.Collapsed;
                                txt.Content = " ";
                            }
                        }
                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;                     
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        txt.Visibility = System.Windows.Visibility.Collapsed;
                        txt.Content = " ";
                    }                   
                    if (item.IsNumeric == false)
                    {                       
                        this.dgTestList.Columns[3].IsReadOnly = false;
                        this.dgTestList.Columns[2].IsReadOnly = false;//as per dr priyanka  
                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DeltaResultValue.Clear();
                    }
                    #endregion
                }
                else if (item.IsNumeric == true && item.IsMachine == true)
                {
                     bool IsValid = item.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                    if(IsValid)
                    {
                    #region code for numric and manual values
                    if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                    {
                        if (item.DeltaCheckValue == 0)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            //txt.Content = "N.A";
                            item.DeltacheckString = "N.A";
                            item.DeltaCheck = false;
                        }
                        else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            item.DeltacheckString = "Fail";
                            item.DeltaCheck = true;
                        }
                        else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue)
                        {
                            DataGridColumn column = this.dgTestList.Columns[10];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Content = "Pass";
                            item.DeltacheckString = "Pass";
                            item.DeltaCheck = true;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        item.DeltacheckString = "N.A";
                        item.DeltaCheck = false;
                    }
                    DeltaResultValue.Clear();
                    if (item.IsReflexTesting == true)
                    {
                        if (!(string.IsNullOrEmpty(item.ResultValue) || item.ResultValue == "0"))
                        {
                            if ((Convert.ToDouble(item.ResultValue) < item.LowReflex) || (Convert.ToDouble(item.ResultValue) > item.HighReflex))
                            {
                                item.ReflexTestingFlag = true;
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Visibility = System.Windows.Visibility.Visible;
                                Image img = new Image();
                                img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                img.Height = 20;
                                img.Width = 20;
                                txt.Content = img;
                            }
                            else
                            {
                                item.ReflexTestingFlag = false;
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Visibility = System.Windows.Visibility.Collapsed;
                                txt.Content = " ";
                            }
                        }
                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        txt.Visibility = System.Windows.Visibility.Collapsed;
                        txt.Content = " ";
                    }
                    if (item.IsNumeric == false)
                    {
                        this.dgTestList.Columns[3].IsReadOnly = false;
                        this.dgTestList.Columns[2].IsReadOnly = false;//as per dr priyanka  
                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DeltaResultValue.Clear();
                    }
                    #endregion
                    }
                }
                else
                {
                    if (item.IsSecondLevel == false)  //by rohini for cr point enable after authorization machine result value
                    {
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        item.DeltacheckString = "N.A";
                        item.DeltaCheck = false;
                        this.dgTestList.Columns[9].IsReadOnly = true;
                        this.dgTestList.Columns[3].IsReadOnly = false;
                        //for cr points
                        item.IsReadOnly = true;
                        this.dgTestList.Columns[2].IsReadOnly = true;
                    }
                    else
                    {
                        DataGridColumn column = this.dgTestList.Columns[9];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        //txt.Content = "N.A";
                        item.DeltacheckString = "N.A";
                        item.DeltaCheck = false;
                        this.dgTestList.Columns[9].IsReadOnly = true;
                        this.dgTestList.Columns[3].IsReadOnly = false;
                        //for cr points
                        item.IsReadOnly = false;
                        this.dgTestList.Columns[2].IsReadOnly = false;
                    }
                }

            }

            indicator.Close();
        }

        void Row_KeyDown(object sender, KeyEventArgs e)
        {
            dgTestList.CurrentColumn = dgTestList.Columns[3];
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ResultListContent.Visibility = Visibility.Collapsed;
            RefferedBy.Visibility = Visibility.Collapsed;
            txtReferenceDoctor.Visibility = Visibility.Collapsed;
            Pathologist.Visibility = Visibility.Collapsed;
            cmbPathologist1.Visibility = Visibility.Collapsed;
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ResultListContent.Visibility = Visibility.Visible;
            RefferedBy.Visibility = Visibility.Visible;
            txtReferenceDoctor.Visibility = Visibility.Visible;
            Pathologist.Visibility = Visibility.Visible;
            cmbPathologist1.Visibility = Visibility.Visible;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgTest1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            //foreach (var Agency in AgencyList)
            //{
            //    if (item.TestID == Agency.TestID)
            //    {
            //        if (Agency.AgencyID != 0 && (Agency.AgencyName != null && Agency.AgencyName != ""))
            //        {
            //            e.Row.Background = new SolidColorBrush(Colors.Orange);

            //            e.Row.IsEnabled = false;
            //        }
            //    }
            //}
            if (item.IsOutSourced)
            {
                e.Row.Background = new SolidColorBrush(Colors.Orange);

                e.Row.IsEnabled = false;

            }
            else
            {
                //if (item.IsResultEntry || item.IsCheckedResults)
                //{
                //    // e.Row.Background = CheckresultColorCode;//new SolidColorBrush(Colors.Cyan);
                //    e.Row.Background = new SolidColorBrush(Colors.Green);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel && item.IsCheckedResults == false)
                //{
                //    //e.Row.Background = FirstLevelcolorCode;//new SolidColorBrush(Colors.Magenta);
                //    e.Row.Background = new SolidColorBrush(Colors.Green);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel == true && item.SecondLevel == true)
                //{
                //    //   e.Row.Background = SecondLevelcolorCode;//new SolidColorBrush(Colors.Yellow);
                //    e.Row.Background = new SolidColorBrush(Colors.Yellow);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel && item.SecondLevel && item.ThirdLevel)
                //{
                //    e.Row.Background = ThirdLevelcolorCode;//new SolidColorBrush(Colors.Orange);
                //}
            }
        }

        void msgW_OnMessageBoxClosed1(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Win = new PasswordWindow();
                Win.IsForReport = true;
                Win.OnOkButton_Click += new RoutedEventHandler(OnOkButton_Click1);
                Win.OnCancelButton_Click += new RoutedEventHandler(OnCancelButton_Click1);
                Win.Show();
            }

            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Take The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }

        void OnOkButton_Click1(object sender, RoutedEventArgs e)
        {
            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true)
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == true)
                {
                    try
                    {
                        clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                        BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                        BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                                {
                                    ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;
                                    if (ObjDetails.TemplateDetails != null)
                                    {
                                        TemplateID = ObjDetails.TemplateDetails.TemplateID;
                                        PrintPathoReportDelivery win_PrintReport = new PrintPathoReportDelivery();
                                        win_PrintReport.ResultId = ObjDetails.TemplateDetails.ID;
                                        win_PrintReport.ObjDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                                        win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                                        win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                        win_PrintReport.Show();
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
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true)
                    {
                        PrintReport(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID);
                    }
                }
            }
            else
            {
                // To Print Upload Report
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                {
                    ViewUploadedReport();
                }
            }

        }

        void OnCancelButton_Click1(object sender, RoutedEventArgs e)
        {
            //objAnimation.Invoke(RotationType.Backward);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (BalanceAmount == 0.0)
            {
                if (SelectedDetails.ReportTemplate != false)  //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry == true)
                {
                    try
                    {
                        clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                        BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                        BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                                {
                                    ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;

                                    if (ObjDetails.TemplateDetails != null)
                                    {

                                        TemplateID = ObjDetails.TemplateDetails.TemplateID;

                                        PrintPathoReportDelivery win_PrintReport = new PrintPathoReportDelivery();

                                        win_PrintReport.ResultId = ObjDetails.TemplateDetails.ID;
                                        //win_PrintReport.ResultId = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                        win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                                        win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                        win_PrintReport.Show();

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
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry)
                        //GetPatientDetailsInHtml(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID, ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized);
                        PrintReport(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID);
                    //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                    //{
                    //    ViewUploadedReport();
                    //}
                }
            }
            else
            {
                //by rohinee
                //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Still You Want To Continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
                //mgbx.Show();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();

            }
        }

        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

            }
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            //if (item.IsServiceRefunded == true)
            //    e.Row.Background = new SolidColorBrush(Colors.Red);
            //else
            //    e.Row.Background = null;
            if (item.IsOutSourced == true)
            {
                e.Row.Background = new SolidColorBrush(Colors.Orange);
                e.Row.IsEnabled = false;

            }
            //if (item.IsResultEntry || item.IsCheckedResults)
            //{
            //    //e.Row.Background = CheckresultColorCode;//new SolidColorBrush(Colors.Cyan);
            //    e.Row.Background = new SolidColorBrush(Colors.Green);

            //}
            //if (item.IsResultEntry == true && item.FirstLevel && item.IsCheckedResults == false)
            //{
            //    // e.Row.Background = FirstLevelcolorCode;//new SolidColorBrush(Colors.Magenta);
            //    e.Row.Background = new SolidColorBrush(Colors.Green);
            //}
            //if (item.IsResultEntry == true && item.FirstLevel && item.SecondLevel)
            //{
            //    //  e.Row.Background = SecondLevelcolorCode;//new SolidColorBrush(Colors.Yellow);
            //    e.Row.Background = new SolidColorBrush(Colors.Yellow);
            //}
            //if (item.IsResultEntry == true && item.FirstLevel && item.SecondLevel && item.ThirdLevel)
            //{
            //    e.Row.Background = ThirdLevelcolorCode;//new SolidColorBrush(Colors.Orange);
            //}



            //added by rohini for cross clinic handle dispatch item to another clinic            

            if (dgTest.ItemsSource != null)
            {
                if (((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId && ((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != 0)
                {
                    e.Row.IsEnabled = false;
                }
                else
                    e.Row.IsEnabled = true;

                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsProcessingUnit == false)
                {
                    e.Row.IsEnabled = false;
                }
                else
                    e.Row.IsEnabled = true;
            }
        }

        private object GetChildControl(DependencyObject parent, string controlName)
        {

            Object tempObj = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < count; counter++)
            {
                //Get The Child Control based on Index
                tempObj = VisualTreeHelper.GetChild(parent, counter);

                //If Control's name Property matches with the argument control
                //name supplied then Return Control
                if ((tempObj as DependencyObject).GetValue(NameProperty).ToString() == controlName)
                    return tempObj;
                else //Else Search Recursively
                {
                    tempObj = GetChildControl(tempObj as DependencyObject, controlName);
                    if (tempObj != null)
                        return tempObj;
                }
            }
            return null;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        #endregion
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTestList.ItemsSource != null)
            {
                Notes Nt = null;
                Nt = new Notes();
                HyperlinkButton HY = sender as HyperlinkButton;
                if (TestList.ToList() != null && TestList.Count > 0)
                {
                    foreach (var item in TestList)
                    {
                        if (item.PathoTestID == ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).PathoTestID && item.SampleNo == ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).SampleNo)  //modified by rohini for ipd
                        {
                            ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).FootNote = item.FootNote;
                            ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).Note = item.Note;
                            break;
                        }
                    }
                }
                Nt.TestDetails = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0]));
                TestID = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).PathoTestID;
                TestCategoryID = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).CategoryID;
                TestAndSampleNO = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).TestAndSampleNO;  //byte rohini for for foot note ipd
                Nt.IsSecondLevel = false;
                Nt.IsThirdLevel = false;
                foreach (clsUserCategoryLinkVO item in UserCategoryLinkedList)
                {
                    if (item.CategoryID == TestCategoryID)
                    {
                        Nt.txtFootNote.IsEnabled = true;
                        Nt.txtSuggestionNote.IsEnabled = true;
                    }
                }

                Nt.OnAddButton_Click += new RoutedEventHandler(Notes_OnAddButton_Click);
                Nt.Show();
            }
        }

        void Notes_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            //modified by rohini
            if (((Notes)sender).DialogResult == true)
            {
                if (TestList != null && TestList.Count > 0)
                {
                    TestList.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(z => z.FootNote = ((Notes)sender).FootNote);
                    TestList.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(x => x.Note = ((Notes)sender).SuggestionNote);

                }
                if (CatagoryWiseTestList != null && CatagoryWiseTestList.Count > 0)
                {
                    CatagoryWiseTestList.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(z => z.FootNote = ((Notes)sender).FootNote);
                    CatagoryWiseTestList.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(x => x.Note = ((Notes)sender).SuggestionNote);
                }

                if (SelectedTestParameterValues != null && SelectedTestParameterValues.Count > 0)
                {
                    SelectedTestParameterValues.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(z => z.FootNote = ((Notes)sender).FootNote);
                    SelectedTestParameterValues.ToList().Where(x => x.PathoTestID == TestID && x.TestAndSampleNO == TestAndSampleNO).ToList().ForEach(x => x.Note = ((Notes)sender).SuggestionNote);
                }
            }
        }

        private void cmbPathologist1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //richTextBox.Html = null;
            //if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
            //    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";

            cmbTemplate.SelectedValue = (long)0;
            //by rohini
            FillTemplate();
            //richTextBox.Html = null;
            //string rtbstring = string.Empty;
            //string styleString = string.Empty;
            //if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
            //{
            //    rtbstring = richTextBox.Html;
            //    rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
            //    rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
            //    richTextBox.Html = rtbstring;
            //}
        }

        private void txtsample_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FillOrderBookingList();
        }

        private void CmdMachinlinking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTest.ItemsSource != null && dgTest.SelectedItem != null)
                {
                    TestMachinLinking frm = new TestMachinLinking();
                    frm.SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                    frm.OnAddButton_Click += new RoutedEventHandler(MachineSearch_OnAddButton_Click);
                    frm.Show();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void MachineSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }

        private void PreviousRecord_Click(object sender, RoutedEventArgs e)
        {
            //commented by rohini as per disscussed by priyanka
            //if (((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckValue != 0)
            //{
            PreviousResults win = new PreviousResults();
            win.ParameterId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
            win.TestId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).PathoTestID;
            win.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
            win.PatientUnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
            win.mainPathologyid = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            win.OrderDate = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderDate;
            win.Show();
            //}
            //else
            //{

            //}


        }

        private void DeltaCheck_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPathoTestParameterVO)dgTestList.SelectedItem).IsNumeric == true && ((clsPathoTestParameterVO)dgTestList.SelectedItem).IsMachine == false)
            {
                DeltaCheck win = new DeltaCheck();
                win.ParameterId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
                win.ResultValue = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue;
                win.TestId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).PathoTestID;
                win.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                win.PatientUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
                win.mainPathologyid = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                win.DeltaCheckDefaultValue = ((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckDefaultValue;
                win.CalDeltaCheck = ((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckValue;
                win.OrderDate = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderDate;
                win.Show();
            }
            else if (((clsPathoTestParameterVO)dgTestList.SelectedItem).IsNumeric == true && ((clsPathoTestParameterVO)dgTestList.SelectedItem).IsMachine == true)
            {
                bool IsValid = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                if (IsValid)
                {
                    DeltaCheck win = new DeltaCheck();
                    win.ParameterId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
                    win.ResultValue = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue;
                    win.TestId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).PathoTestID;
                    win.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                    win.PatientUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
                    win.mainPathologyid = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                    win.DeltaCheckDefaultValue = ((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckDefaultValue;
                    win.CalDeltaCheck = ((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckValue;
                    win.OrderDate = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderDate;
                    win.Show();
                }
            }
        }

        private void ReflexTestingButton_Click(object sender, RoutedEventArgs e)
        {
            ReflexTestingParameters win = new ReflexTestingParameters();
            win.ParameterId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
            win.ServiceId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ServiceID;
            win.Show();
        }
        // string ResultValue = string.Empty;
        private void ResultEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = (TextBox)sender;

          
            // string ResultValue = text.Text;

         
            string ResultValue = "";
            if ((clsPathoTestParameterVO)dgTestList.SelectedItem != null)
            {
                if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue != String.Empty)
                {
                    ResultValue = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue;
                }
                else
                {
                    ResultValue = text.Text;
                }
            }
          
            clsPathoTestParameterVO item = (clsPathoTestParameterVO)dgTestList.SelectedItem;
            /*To calculate Delta Check of a paramter
           * To get the latest Previous Result value of respective paramters of Tests of a particular patient.
           * Added on 5.05.2016
           */
            clsGetDeltaCheckValueBizActionVO BizAction2 = new clsGetDeltaCheckValueBizActionVO();
            BizAction2.PathoTestParameter = new clsPathoTestParameterVO();
            BizAction2.PathTestId = new clsPathOrderBookingDetailVO();
            BizAction2.PathPatientDetail = new clsPathOrderBookingVO();
            PreviousResult = new List<clsGetDeltaCheckValueBizActionVO>();
            if (item != null)
            {
                if (item.PathoTestID > 0)
                    BizAction2.PathTestId.TestID = item.PathoTestID;
                if (item.ParameterID > 0)
                    BizAction2.PathoTestParameter.ParameterID = item.ParameterID;
                
            }
                   
            if (dgOrdertList.SelectedItem != null)
            {
                BizAction2.PathPatientDetail.PatientID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                BizAction2.PathPatientDetail.PatientUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
                BizAction2.PathPatientDetail.OrderDate = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderDate;
                BizAction2.PathPatientDetail.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;   
            }
            BizAction2.PathPatientDetail.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client2 = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);

            client2.ProcessCompleted += (s2, arg2) =>
            {
                if (arg2.Error == null && arg2.Result != null)
                {
                    clsGetDeltaCheckValueBizActionVO resultDC = arg2.Result as clsGetDeltaCheckValueBizActionVO;
                    if (resultDC.List != null)
                    {
                        foreach (var item2 in resultDC.List)
                        {
                            PreviousResult.Add(item2);
                        }
                    }
                    if (item.IsNumeric == true && item.IsMachine == false)
                    {
                        if (PreviousResult.Count != 0)
                        {
                            if (ResultValue != "")
                            {
                                foreach (var item6 in PreviousResult)
                                {
                                    if (item6.ParameterID == item.ParameterID)
                                    {
                                        item.DeltaCheckValue = (Convert.ToDouble(item6.ResultValue) - Convert.ToDouble(ResultValue)) / Convert.ToDouble(item6.ResultValue) * 100;
                                        if (Double.IsInfinity(item.DeltaCheckValue) || Double.IsNaN(item.DeltaCheckValue))
                                        {
                                            item.DeltaCheck = false;
                                            item.DeltaCheckValue = 0;
                                        }
                                        else
                                        {
                                            item.DeltaCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (item.IsNumeric == true && item.IsMachine == true)
                    {
                        bool IsValid = ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                        if (IsValid)
                        {
                            if (PreviousResult.Count != 0)
                            {
                                if (ResultValue != "")
                                {
                                    foreach (var item6 in PreviousResult)
                                    {
                                        if (item6.ParameterID == item.ParameterID)
                                        {
                                            item.DeltaCheckValue = (Convert.ToDouble(item6.ResultValue) - Convert.ToDouble(ResultValue)) / Convert.ToDouble(item6.ResultValue) * 100;
                                            if (Double.IsInfinity(item.DeltaCheckValue) || Double.IsNaN(item.DeltaCheckValue))
                                            {
                                                item.DeltaCheck = false;
                                                item.DeltaCheckValue = 0;
                                            }
                                            else
                                            {
                                                item.DeltaCheck = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }                   
                    if (ResultValue != "" || ResultValue != string.Empty)
                    {
                        if (item.IsNumeric == true && item.IsMachine == false)
                        {
                            #region Numbric value code
                            //by rohinee formula result
                            #region formula
                            try
                            {
                                //if (!string.IsNullOrEmpty(((clsPathoTestParameterVO)dgTestList.SelectedItem).Formula.Trim()))
                                foreach (var itemResult in (PagedCollectionView)dgTestList.ItemsSource)
                                {
                                    clsPathoTestParameterVO obj = new clsPathoTestParameterVO();
                                    obj = (clsPathoTestParameterVO)itemResult;

                                    String Formula = obj.FormulaID;
                                    String TempFormula = obj.FormulaID;

                                    if (!string.IsNullOrEmpty(Formula))
                                    {
                                        bool Newcharacter = false;

                                        string Parameter = string.Empty;

                                        for (int i = 0; i < TempFormula.Length; i++)
                                        {
                                            String s = TempFormula.Substring(i, 1);

                                            if (s.Equals("("))
                                                Newcharacter = true;
                                            else if (s.Equals("+"))
                                                Newcharacter = true;
                                            else if (s.Equals("-"))
                                                Newcharacter = true;
                                            else if (s.Equals("*"))
                                                Newcharacter = true;
                                            else if (s.Equals("/"))
                                                Newcharacter = true;
                                            else if (s.Equals("sqrt"))
                                                Newcharacter = true;
                                            else if (s.Equals(")"))
                                            {
                                                if ((i + 1) == TempFormula.Length)
                                                {
                                                    foreach (var item1 in (PagedCollectionView)dgTestList.ItemsSource)
                                                    {
                                                        if (Parameter == ((clsPathoTestParameterVO)item1).ParameterCode)
                                                        {
                                                            #region added by hrishikesh 19_12_2014
                                                            if (dgTestList.Columns[4].Visibility == Visibility)
                                                            {
                                                                //  if (dgTestList.Columns[4].IsReadOnly == false)
                                                                //Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).ThirdLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).ThirdLevelResulValue.Trim() + ")");
                                                            }
                                                            if (dgTestList.Columns[3].Visibility == Visibility)
                                                            {
                                                                //  if (dgTestList.Columns[3].IsReadOnly == false)
                                                                // Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).SecondLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).SecondLevelResulValue.Trim() + ")");
                                                                Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item1).ResultValue.Trim() + ")");

                                                            }
                                                            if (dgTestList.Columns[2].Visibility == Visibility)
                                                            {
                                                                if (dgTestList.Columns[2].IsReadOnly == false)
                                                                    Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item1).ResultValue.Trim() + ")");
                                                            }
                                                            #endregion

                                                        }
                                                    }
                                                }

                                                Newcharacter = true;
                                            }
                                            else
                                            {
                                                if (Newcharacter)
                                                {
                                                    if (!string.IsNullOrEmpty(Parameter))
                                                    {
                                                        foreach (var item2 in (PagedCollectionView)dgTestList.ItemsSource)
                                                        {
                                                            if (Parameter == ((clsPathoTestParameterVO)item2).ParameterCode)
                                                            {
                                                                #region added by hrishikesh 19_12_2014
                                                                //if (dgTestList.Columns[4].Visibility == Visibility)
                                                                //{
                                                                //    // if (dgTestList.Columns[4].IsReadOnly == false)
                                                                //    // Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).ThirdLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).ThirdLevelResulValue.Trim() + ")");
                                                                //}
                                                                if (dgTestList.Columns[3].Visibility == Visibility)
                                                                {
                                                                    // if (dgTestList.Columns[3].IsReadOnly == false)
                                                                    // Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).SecondLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).SecondLevelResulValue.Trim() + ")");
                                                                    //if (dgTestList.Columns[3].IsReadOnly == true)
                                                                    Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item2).ResultValue.Trim() + ")");

                                                                }
                                                                if (dgTestList.Columns[2].Visibility == Visibility)
                                                                {
                                                                    if (dgTestList.Columns[2].IsReadOnly == false)
                                                                        Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item2).ResultValue.Trim() + ")");
                                                                }
                                                                #endregion

                                                            }
                                                        }
                                                    }
                                                    Parameter = string.Empty;
                                                    Parameter += s;
                                                }
                                                else
                                                {
                                                    Parameter += s;

                                                    if ((i + 1) == TempFormula.Length)
                                                    {
                                                        foreach (var item3 in (PagedCollectionView)dgTestList.ItemsSource)
                                                        {
                                                            //if (Parameter == ((clsPathoTestParameterVO)item1).ParameterName)
                                                            //{
                                                            if (Parameter == ((clsPathoTestParameterVO)item3).ParameterCode)
                                                            {
                                                                #region added by hrishikesh 19_12_2014
                                                                //if (dgTestList.Columns[4].Visibility == Visibility)
                                                                //{
                                                                //    //if (dgTestList.Columns[4].IsReadOnly == false)
                                                                //    //Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).ThirdLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).ThirdLevelResulValue.Trim() + ")");
                                                                //}
                                                                if (dgTestList.Columns[3].Visibility == Visibility)
                                                                {
                                                                    //if (dgTestList.Columns[3].IsReadOnly == false)
                                                                    // Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).SecondLevelResulValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item).SecondLevelResulValue.Trim() + ")");
                                                                    // if (dgTestList.Columns[3].IsReadOnly == true)
                                                                    Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item3).ResultValue.Trim() + ")");

                                                                }
                                                                if (dgTestList.Columns[2].Visibility == Visibility)
                                                                {
                                                                    if (dgTestList.Columns[2].IsReadOnly == false)
                                                                        Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item3).ResultValue.Trim() + ")");
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }

                                                }
                                                Newcharacter = false;
                                            }
                                        }


                                       double Answer = Math.Round(Convert.ToDouble(HtmlPage.Window.Eval(Formula)),4);  //by vikrant
                                     
                                      // double Answer = Math.Round(Eval(Formula), 4);
                                       string calculatedResultValue = String.Format("{0:0.00}", Answer);



                                        foreach (var item4 in (PagedCollectionView)dgTestList.ItemsSource)
                                        {
                                            clsPathoTestParameterVO Objitem = new clsPathoTestParameterVO();
                                            Objitem = (clsPathoTestParameterVO)item4;

                                            if (TempFormula == Objitem.FormulaID)
                                            {
                                                #region added by hrishikesh 19_12_2014
                                                if (dgTestList.Columns[4].Visibility == Visibility)
                                                {
                                                    // if (dgTestList.Columns[4].IsReadOnly == false)
                                                    //  Objitem.ThirdLevelResulValue = calculatedResultValue;
                                                }
                                                if (dgTestList.Columns[3].Visibility == Visibility)
                                                {
                                                    // if (dgTestList.Columns[3].IsReadOnly == false)
                                                    // Objitem.SecondLevelResulValue = calculatedResultValue;
                                                    // if (dgTestList.Columns[3].IsReadOnly == true)
                                                    obj.ResultValue = calculatedResultValue;
                                                   

                                                    // Flag Update by vikrant
                                                    #region New Logic
                                                    if (obj.DeltaCheckDefaultValue != 0.0 || obj.DeltaCheckDefaultValue != 0)
                                                    {
                                                        if (PreviousResult.Count == 0)
                                                        {
                                                            obj.DeltacheckString = "N.A";
                                                            obj.DeltaCheck = false;
                                                        }
                                                        else if (obj.DeltaCheckValue < obj.DeltaCheckDefaultValue && obj.DeltaCheckValue != 0.0)
                                                        {
                                                            obj.DeltacheckString = "Fail";
                                                            obj.DeltaCheck = true;
                                                        }
                                                        else if (obj.DeltaCheckValue > obj.DeltaCheckDefaultValue && obj.DeltaCheckValue != 0.0)
                                                        {
                                                            obj.DeltacheckString = "Pass";
                                                            obj.DeltaCheck = true;
                                                        }
                                                        else
                                                        {
                                                            obj.DeltacheckString = "N.A";
                                                            obj.DeltaCheck = false;
                                                        }
                                                        DeltaResultValue.Clear();
                                                    }
                                                    else
                                                    {
                                                        obj.DeltacheckString = "N.A";
                                                        obj.DeltaCheck = false;
                                                    }

                                                    if (obj.IsReflexTesting == true)
                                                    {
                                                        if ((Convert.ToDouble(obj.ResultValue) < obj.LowReflex) || (Convert.ToDouble(obj.ResultValue) > obj.HighReflex))
                                                        {
                                                            //item.ReflexTesting = "../Icons/Isreflex.png";
                                                            obj.ReflexTestingFlag = true;
                                                            DataGridColumn column = this.dgTestList.Columns[9];
                                                            FrameworkElement fe = column.GetCellContent(obj);
                                                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                                            var thisCell = (DataGridCell)result;

                                                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                                            txt.Visibility = System.Windows.Visibility.Visible;
                                                            Image img = new Image();
                                                            img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                                            img.Height = 20;
                                                            img.Width = 20;
                                                            txt.Content = img;
                                                        }
                                                        else
                                                        {
                                                            obj.ReflexTestingFlag = false;
                                                            DataGridColumn column = this.dgTestList.Columns[9];
                                                            FrameworkElement fe = column.GetCellContent(obj);
                                                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                                            var thisCell = (DataGridCell)result;
                                                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                                            txt.Visibility = System.Windows.Visibility.Collapsed;
                                                            txt.Content = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        obj.ReflexTestingFlag = false;
                                                        DataGridColumn column = this.dgTestList.Columns[9];
                                                        FrameworkElement fe = column.GetCellContent(obj);
                                                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                                        var thisCell = (DataGridCell)result;
                                                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                                        txt.Visibility = System.Windows.Visibility.Collapsed;
                                                        txt.Content = "";
                                                    }
                                                    if (obj.ResultValue != "")
                                                    {
                                                        // Reference Values(Auto Authorization)
                                                        if ((Convert.ToDouble(obj.ResultValue) > (obj.HighReffValue)))
                                                        {
                                                            obj.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                        }
                                                        else if ((Convert.ToDouble(obj.ResultValue) < (obj.LowReffValue)))
                                                        {
                                                            obj.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                        }
                                                        else
                                                        {
                                                            obj.ApColor = "/PalashDynamics;component/Icons/green.png";
                                                        }
                                                    }
                                                    //Panic Values 
                                                    if (obj.LowerPanicValue >= 0 && obj.UpperPanicValue > 0)
                                                    {
                                                        if (obj.ResultValue != "")
                                                        {
                                                            if ((Convert.ToDouble(obj.ResultValue) < (obj.LowerPanicValue)) || ((Convert.ToDouble(obj.ResultValue) > (obj.UpperPanicValue))))
                                                            {
                                                                obj.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                                            }
                                                            else
                                                            {
                                                                obj.ApColorPanic = " ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            obj.ApColorPanic = " ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        obj.ApColorPanic = " ";
                                                    }
                                                    //Improbable Values 
                                                    if (obj.ResultValue != "")
                                                    {
                                                        if ((Convert.ToDouble(obj.ResultValue) < (obj.MinImprobable)) || ((Convert.ToDouble(obj.ResultValue) > (obj.MaxImprobable))))
                                                        {
                                                            obj.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                                        }
                                                        else
                                                        {
                                                            obj.ApColorImp = " ";
                                                        }
                                                    }
                                                    # endregion
                                                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                }
                                                dgTestList.CurrentColumn = dgTestList.Columns[2]; // You have to use this line instead
                                                dgTestList.Focus();
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string str = ex.Message;
                            }
                            #endregion
                            // Added by Anumani on 23.02.2016
                            if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                            {

                                if (PreviousResult.Count == 0)
                                {
                                    //DataGridColumn column = this.dgTestList.Columns[10];
                                    //FrameworkElement fe = column.GetCellContent(item);
                                    //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    //var thisCell = (DataGridCell)result;
                                    //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //txt.Content = "N.A";
                                    item.DeltacheckString = "N.A";
                                    item.DeltaCheck = false;

                                }
                                else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue && item.DeltaCheckValue != 0.0)
                                {
                                    //DataGridColumn column = this.dgTestList.Columns[10];
                                    //FrameworkElement fe = column.GetCellContent(item);
                                    //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    //var thisCell = (DataGridCell)result;
                                    //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //txt.Content = "Fail";
                                    item.DeltacheckString = "Fail";
                                    item.DeltaCheck = true;
                                }
                                else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue && item.DeltaCheckValue != 0.0)
                                {
                                    //DataGridColumn column = this.dgTestList.Columns[10];
                                    //FrameworkElement fe = column.GetCellContent(item);
                                    //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    //var thisCell = (DataGridCell)result;
                                    //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    item.DeltacheckString = "Pass";
                                    item.DeltaCheck = true;
                                }
                                else
                                {
                                    /*
                                     * Parameter where Default value for Deltacheck is not mentioned
                                     */
                                    //DataGridColumn column = this.dgTestList.Columns[10];
                                    //FrameworkElement fe = column.GetCellContent(item);
                                    //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    //var thisCell = (DataGridCell)result;
                                    //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    //txt.Content = "N.A";
                                    item.DeltacheckString = "N.A";
                                    item.DeltaCheck = false;
                                }
                                DeltaResultValue.Clear();
                            }
                            else
                            {
                                //DataGridColumn column = this.dgTestList.Columns[9];
                                //FrameworkElement fe = column.GetCellContent(item);
                                //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                //var thisCell = (DataGridCell)result;
                                //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                item.DeltacheckString = "N.A";
                                item.DeltaCheck = false;
                            }
                            if (item.IsReflexTesting == true)
                            {
                                if ((Convert.ToDouble(ResultValue) < item.LowReflex) || (Convert.ToDouble(ResultValue) > item.HighReflex))
                                {
                                    //item.ReflexTesting = "../Icons/Isreflex.png";
                                    item.ReflexTestingFlag = true;
                                    DataGridColumn column = this.dgTestList.Columns[9];
                                    FrameworkElement fe = column.GetCellContent(item);
                                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    var thisCell = (DataGridCell)result;
                                    if (thisCell != null)
                                    {
                                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                        txt.Visibility = System.Windows.Visibility.Visible;
                                        Image img = new Image();
                                        img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                        img.Height = 20;
                                        img.Width = 20;
                                        txt.Content = img;
                                    }
                                }
                                else
                                {
                                    item.ReflexTestingFlag = false;
                                    DataGridColumn column = this.dgTestList.Columns[9];
                                    FrameworkElement fe = column.GetCellContent(item);
                                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    var thisCell = (DataGridCell)result;
                                    if (thisCell != null)
                                    {
                                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                        txt.Visibility = System.Windows.Visibility.Collapsed;
                                        txt.Content = "";
                                    }
                                }
                            }
                            else
                            {
                                item.ReflexTestingFlag = false;
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(item);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                if (thisCell != null)
                                {
                                    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                    txt.Visibility = System.Windows.Visibility.Collapsed;
                                    txt.Content = "";
                                }
                            }


                            # region Old Logic
                            /* logic has been devdeloped as per the variation of various Ranges in the following manner
                     * MinImprobable < LowerPanic < LowReffValue(Auto Authorization)
                     *  MaxReffValue  < UpperPanic < MaxImprobable
                     */

                            /*
                             * MIN values, Max Values , MINIMPROBABLE Values, MAXIMPROBABLE Values 
                             */

                            //    if ((Convert.ToDouble(ResultValue) > Convert.ToDouble(item.HighReffValue)))  // && (Convert.ToDouble(item5.ResultValue) <= (Convert.ToDouble(NoramlRange1[1]))))
                            //    {
                            //        if (item.UpperPanicValue != 0.0 && item.LowerPanicValue != 0.0)
                            //        {
                            //            if (Convert.ToDouble(ResultValue) < Convert.ToDouble(item.UpperPanicValue))
                            //            {

                            //                item.ApColor = "/PalashDynamics;component/Icons/purple.png";

                            //            }
                            //            else if (Convert.ToDouble(ResultValue) > Convert.ToDouble(item.MaxImprobable))
                            //            {

                            //                item.ApColor = "/PalashDynamics;component/Icons/brown.png";
                            //            }
                            //            else if (Convert.ToDouble(ResultValue) > (item.UpperPanicValue) && Convert.ToDouble(ResultValue) < item.MaxImprobable)
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/yellow.png";
                            //            }
                            //        }


                            //        else
                            //        {
                            //            if (Convert.ToDouble(ResultValue) > Convert.ToDouble(item.HighReffValue) && Convert.ToDouble(ResultValue) < Convert.ToDouble(item.MaxImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/purple.png";
                            //            }
                            //            else if (Convert.ToDouble(ResultValue) > Convert.ToDouble(item.MaxImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/brown.png";
                            //            }

                            //        }
                            //    }

                            //    else if ((Convert.ToDouble(ResultValue) < Convert.ToDouble(item.LowReffValue))) // && (Convert.ToDouble(ResultValue) > (item.MinImprobable)))
                            //    {
                            //        if (item.UpperPanicValue != 0.0 && item.LowerPanicValue != 0.0)
                            //        {
                            //            if ((Convert.ToDouble(ResultValue) > Convert.ToDouble(item.LowerPanicValue)))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                            //            }
                            //            else if (Convert.ToDouble(ResultValue) < Convert.ToDouble(item.MinImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/brown.png";

                            //            }
                            //            else if (Convert.ToDouble(ResultValue) < Convert.ToDouble(item.LowerPanicValue) && Convert.ToDouble(ResultValue) > Convert.ToDouble(item.MinImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/yellow.png";

                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (Convert.ToDouble(ResultValue) < Convert.ToDouble(item.LowReffValue) && Convert.ToDouble(ResultValue) > Convert.ToDouble(item.MinImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                            //            }
                            //            else if (Convert.ToDouble(ResultValue) < Convert.ToDouble(item.MinImprobable))
                            //            {
                            //                item.ApColor = "/PalashDynamics;component/Icons/brown.png";
                            //            }

                            //        }
                            //        // item.ApColor = myAppConfig.pathominColorCode;
                            //    }
                            //    else
                            //    {
                            //        // item5.ApColor = myAppConfig.pathominColorCode;
                            //        item.ApColor = "/PalashDynamics;component/Icons/green.png";

                            //    }

                            //}
                            # endregion

                            #region New Logic
                            // Logic developed considering that none of the ranges, Improbable, Panic , RefValue 
                            // lies within each other.

                            if (item.ResultValue != "")
                            {
                                // Reference Values(Auto Authorization)
                                if ((Convert.ToDouble(ResultValue) > Convert.ToDouble(item.HighReffValue)))//(item.HighReffValue)))
                                {
                                    item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                }
                                else if ((Convert.ToDouble(ResultValue) < Convert.ToDouble(item.LowReffValue)))
                                {
                                    item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                }
                                else
                                {
                                    item.ApColor = "/PalashDynamics;component/Icons/green.png";
                                }
                            }

                            //Panic Values 
                            if (item.LowerPanicValue >= 0 && item.UpperPanicValue > 0)
                            {
                                if (item.ResultValue != "")
                                {
                                    if ((Convert.ToDouble(ResultValue) < (item.LowerPanicValue)) || ((Convert.ToDouble(ResultValue) > (item.UpperPanicValue))))
                                    {
                                        item.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";

                                    }
                                    else
                                    {
                                        item.ApColorPanic = " ";
                                    }
                                }
                                else
                                {
                                    item.ApColorPanic = " ";

                                }
                            }
                            else
                            {
                                item.ApColorPanic = " ";

                            }

                            //Improbable Values 
                            if (item.ResultValue != "")
                            {
                                if ((Convert.ToDouble(ResultValue) < (item.MinImprobable)) || ((Convert.ToDouble(ResultValue) > (item.MaxImprobable))))
                                {
                                    item.ApColorImp = "/PalashDynamics;component/Icons/brown.png";

                                }
                                else
                                {
                                    item.ApColorImp = " ";

                                }
                            }

                            # endregion

                            if (item.IsNumeric == false)
                            {
                                //Delta Check N.A 
                                //DataGridColumn column = this.dgTestList.Columns[9];
                                //FrameworkElement fe = column.GetCellContent(item);
                                //FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                //var thisCell = (DataGridCell)result;
                                //HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                //txt.Content = "N.A";
                                item.DeltacheckString = "N.A";
                                item.DeltaCheck = false;
                                #region commented

                                //try
                                //{
                                //    clsGetHelpValuesFroResultEntryBizActionVO BizAction1 = new clsGetHelpValuesFroResultEntryBizActionVO();
                                //    BizAction1.HelpValueList = new List<clsPathoTestParameterVO>();
                                //    BizAction1.ParameterID = item.ParameterID;


                                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                                //    client.ProcessCompleted += (s1, arg1) =>
                                //    {
                                //        if (arg1.Error == null)
                                //        {

                                //            if (((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList != null)
                                //            {
                                //                HelpValues = ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList;
                                //                foreach (var item1 in HelpValues)
                                //                {
                                //                    if (ResultValue.Equals(item1.HelpValue))
                                //                    {
                                //                        if (item1.IsAbnormal == true)
                                //                        {

                                //                            item.ApColor = "/PalashDynamics;component/Icons/black.png";
                                //                        }
                                //                        else
                                //                        {
                                //                            item.ApColor = " ";
                                //                            item.ApColor = "/PalashDynamics;component/Icons/green.png";
                                //                        }
                                //                    }
                                //                }
                                //                //dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
                                //                //NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList; //Newet added

                                //            }

                                //        }
                                //        else
                                //        {
                                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                //            msgW1.Show();
                                //        }

                                //    };

                                //    client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                                //    client.CloseAsync();
                                //}
                                //catch (Exception ex)
                                //{
                                //    throw;
                                //}
                                #endregion
                                //Read Only When Text Value as per dr priyanka suggession
                                item.IsReadOnly = true;


                            }
                            #endregion

                        }
                        if (item.IsNumeric == true && item.IsMachine == true)  //by rohini as per changes told by mangesh 
                        {
                            bool Isvalid = ResultValue.IsValidPositiveNumberWithDecimal();
                            if (Isvalid)
                            {
                                #region code for flag generation of valid machine values by rohini
                                #region formula commented by rohini for no calculation for formula at time of machine values
                                //try
                                //{
                                //    foreach (var itemResult in (PagedCollectionView)dgTestList.ItemsSource)
                                //    {
                                //        clsPathoTestParameterVO obj = new clsPathoTestParameterVO();
                                //        obj = (clsPathoTestParameterVO)itemResult;

                                //        String Formula = obj.FormulaID;
                                //        String TempFormula = obj.FormulaID;

                                //        if (!string.IsNullOrEmpty(Formula))
                                //        {
                                //            bool Newcharacter = false;

                                //            string Parameter = string.Empty;

                                //            for (int i = 0; i < TempFormula.Length; i++)
                                //            {
                                //                String s = TempFormula.Substring(i, 1);

                                //                if (s.Equals("("))
                                //                    Newcharacter = true;
                                //                else if (s.Equals("+"))
                                //                    Newcharacter = true;
                                //                else if (s.Equals("-"))
                                //                    Newcharacter = true;
                                //                else if (s.Equals("*"))
                                //                    Newcharacter = true;
                                //                else if (s.Equals("/"))
                                //                    Newcharacter = true;
                                //                else if (s.Equals("sqrt"))
                                //                    Newcharacter = true;
                                //                else if (s.Equals(")"))
                                //                {
                                //                    if ((i + 1) == TempFormula.Length)
                                //                    {
                                //                        foreach (var item1 in (PagedCollectionView)dgTestList.ItemsSource)
                                //                        {
                                //                            if (Parameter == ((clsPathoTestParameterVO)item1).ParameterCode)
                                //                            {
                                //                                if (dgTestList.Columns[3].Visibility == Visibility)
                                //                                {
                                //                                    Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item1).ResultValue.Trim() + ")");
                                //                                }
                                //                                if (dgTestList.Columns[2].Visibility == Visibility)
                                //                                {
                                //                                    if (dgTestList.Columns[2].IsReadOnly == false)
                                //                                        Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item1).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item1).ResultValue.Trim() + ")");
                                //                                }
                                //                            }
                                //                        }
                                //                    }
                                //                    Newcharacter = true;
                                //                }
                                //                else
                                //                {
                                //                    if (Newcharacter)
                                //                    {
                                //                        if (!string.IsNullOrEmpty(Parameter))
                                //                        {
                                //                            foreach (var item2 in (PagedCollectionView)dgTestList.ItemsSource)
                                //                            {
                                //                                if (Parameter == ((clsPathoTestParameterVO)item2).ParameterCode)
                                //                                {
                                //                                    if (dgTestList.Columns[3].Visibility == Visibility)
                                //                                    {
                                //                                        Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item2).ResultValue.Trim() + ")");
                                //                                    }
                                //                                    if (dgTestList.Columns[2].Visibility == Visibility)
                                //                                    {
                                //                                        if (dgTestList.Columns[2].IsReadOnly == false)
                                //                                            Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item2).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item2).ResultValue.Trim() + ")");
                                //                                    }
                                //                                }
                                //                            }
                                //                        }
                                //                        Parameter = string.Empty;
                                //                        Parameter += s;
                                //                    }
                                //                    else
                                //                    {
                                //                        Parameter += s;

                                //                        if ((i + 1) == TempFormula.Length)
                                //                        {
                                //                            foreach (var item3 in (PagedCollectionView)dgTestList.ItemsSource)
                                //                            {
                                //                                if (Parameter == ((clsPathoTestParameterVO)item3).ParameterCode)
                                //                                {
                                //                                    if (dgTestList.Columns[3].Visibility == Visibility)
                                //                                    {
                                //                                        Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item3).ResultValue.Trim() + ")");
                                //                                    }
                                //                                    if (dgTestList.Columns[2].Visibility == Visibility)
                                //                                    {
                                //                                        if (dgTestList.Columns[2].IsReadOnly == false)
                                //                                            Formula = Formula.Replace(Parameter, ((clsPathoTestParameterVO)item3).ResultValue.Trim() == "" ? "(0)" : "(" + ((clsPathoTestParameterVO)item3).ResultValue.Trim() + ")");
                                //                                    }
                                //                                }
                                //                            }
                                //                        }

                                //                    }
                                //                    Newcharacter = false;
                                //                }
                                //            }
                                //            double Answer = Math.Round(Convert.ToDouble(HtmlPage.Window.Eval(Formula)), 4);  //by vikrant
                                //            string calculatedResultValue = String.Format("{0:0.00}", Answer);
                                //            foreach (var item4 in (PagedCollectionView)dgTestList.ItemsSource)
                                //            {
                                //                clsPathoTestParameterVO Objitem = new clsPathoTestParameterVO();
                                //                Objitem = (clsPathoTestParameterVO)item4;

                                //                if (TempFormula == Objitem.FormulaID)
                                //                {


                                //                    if (dgTestList.Columns[3].Visibility == Visibility)
                                //                    {
                                //                        obj.ResultValue = calculatedResultValue;
                                //                        // Flag Update by vikrant
                                //                        #region New Logic
                                //                        if (obj.DeltaCheckDefaultValue != 0.0 || obj.DeltaCheckDefaultValue != 0)
                                //                        {
                                //                            if (PreviousResult.Count == 0)
                                //                            {
                                //                                obj.DeltacheckString = "N.A";
                                //                                obj.DeltaCheck = false;
                                //                            }
                                //                            else if (obj.DeltaCheckValue < obj.DeltaCheckDefaultValue && obj.DeltaCheckValue != 0.0)
                                //                            {
                                //                                obj.DeltacheckString = "Fail";
                                //                                obj.DeltaCheck = true;
                                //                            }
                                //                            else if (obj.DeltaCheckValue > obj.DeltaCheckDefaultValue && obj.DeltaCheckValue != 0.0)
                                //                            {
                                //                                obj.DeltacheckString = "Pass";
                                //                                obj.DeltaCheck = true;
                                //                            }
                                //                            else
                                //                            {
                                //                                obj.DeltacheckString = "N.A";
                                //                                obj.DeltaCheck = false;
                                //                            }
                                //                            DeltaResultValue.Clear();
                                //                        }
                                //                        else
                                //                        {
                                //                            obj.DeltacheckString = "N.A";
                                //                            obj.DeltaCheck = false;
                                //                        }

                                //                        if (obj.IsReflexTesting == true)
                                //                        {
                                //                            if ((Convert.ToDouble(obj.ResultValue) < obj.LowReflex) || (Convert.ToDouble(obj.ResultValue) > obj.HighReflex))
                                //                            {
                                //                                obj.ReflexTestingFlag = true;
                                //                                DataGridColumn column = this.dgTestList.Columns[9];
                                //                                FrameworkElement fe = column.GetCellContent(obj);
                                //                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                //                                var thisCell = (DataGridCell)result;

                                //                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                //                                txt.Visibility = System.Windows.Visibility.Visible;
                                //                                Image img = new Image();
                                //                                img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                //                                img.Height = 20;
                                //                                img.Width = 20;
                                //                                txt.Content = img;
                                //                            }
                                //                            else
                                //                            {
                                //                                obj.ReflexTestingFlag = false;
                                //                                DataGridColumn column = this.dgTestList.Columns[9];
                                //                                FrameworkElement fe = column.GetCellContent(obj);
                                //                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                //                                var thisCell = (DataGridCell)result;
                                //                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                //                                txt.Visibility = System.Windows.Visibility.Collapsed;
                                //                                txt.Content = "";
                                //                            }
                                //                        }
                                //                        else
                                //                        {
                                //                            obj.ReflexTestingFlag = false;
                                //                            DataGridColumn column = this.dgTestList.Columns[9];
                                //                            FrameworkElement fe = column.GetCellContent(obj);
                                //                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                //                            var thisCell = (DataGridCell)result;
                                //                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                //                            txt.Visibility = System.Windows.Visibility.Collapsed;
                                //                            txt.Content = "";
                                //                        }
                                //                        if (obj.ResultValue != "")
                                //                        {
                                //                            if ((Convert.ToDouble(obj.ResultValue) > (obj.HighReffValue)))
                                //                            {
                                //                                obj.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                //                            }
                                //                            else if ((Convert.ToDouble(obj.ResultValue) < (obj.LowReffValue)))
                                //                            {
                                //                                obj.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                //                            }
                                //                            else
                                //                            {
                                //                                obj.ApColor = "/PalashDynamics;component/Icons/green.png";
                                //                            }
                                //                        }
                                //                        //Panic Values 
                                //                        if (obj.LowerPanicValue >= 0 && obj.UpperPanicValue > 0)
                                //                        {
                                //                            if (obj.ResultValue != "")
                                //                            {
                                //                                if ((Convert.ToDouble(obj.ResultValue) < (obj.LowerPanicValue)) || ((Convert.ToDouble(obj.ResultValue) > (obj.UpperPanicValue))))
                                //                                {
                                //                                    obj.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                //                                }
                                //                                else
                                //                                {
                                //                                    obj.ApColorPanic = " ";
                                //                                }
                                //                            }
                                //                            else
                                //                            {
                                //                                obj.ApColorPanic = " ";
                                //                            }
                                //                        }
                                //                        else
                                //                        {
                                //                            obj.ApColorPanic = " ";
                                //                        }
                                //                        //Improbable Values 
                                //                        if (obj.ResultValue != "")
                                //                        {
                                //                            if ((Convert.ToDouble(obj.ResultValue) < (obj.MinImprobable)) || ((Convert.ToDouble(obj.ResultValue) > (obj.MaxImprobable))))
                                //                            {
                                //                                obj.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                //                            }
                                //                            else
                                //                            {
                                //                                obj.ApColorImp = " ";
                                //                            }
                                //                        }
                                //                        # endregion
                                //                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                //                    }
                                //                    dgTestList.CurrentColumn = dgTestList.Columns[2]; // You have to use this line instead
                                //                    dgTestList.Focus();
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    string str = ex.Message;
                                //}
                                #endregion

                                if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                                {

                                    if (PreviousResult.Count == 0)
                                    {
                                        item.DeltacheckString = "N.A";
                                        item.DeltaCheck = false;

                                    }
                                    else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue && item.DeltaCheckValue != 0.0)
                                    {
                                        item.DeltacheckString = "Fail";
                                        item.DeltaCheck = true;
                                    }
                                    else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue && item.DeltaCheckValue != 0.0)
                                    {
                                        item.DeltacheckString = "Pass";
                                        item.DeltaCheck = true;
                                    }
                                    else
                                    {
                                        item.DeltacheckString = "N.A";
                                        item.DeltaCheck = false;
                                    }
                                    DeltaResultValue.Clear();
                                }
                                else
                                {
                                    item.DeltacheckString = "N.A";
                                    item.DeltaCheck = false;
                                }
                                if (item.IsReflexTesting == true)
                                {
                                    if ((Convert.ToDouble(ResultValue) < item.LowReflex) || (Convert.ToDouble(ResultValue) > item.HighReflex))
                                    {
                                        item.ReflexTestingFlag = true;
                                        DataGridColumn column = this.dgTestList.Columns[9];
                                        FrameworkElement fe = column.GetCellContent(item);
                                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                        var thisCell = (DataGridCell)result;
                                        if (thisCell != null)
                                        {
                                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                            txt.Visibility = System.Windows.Visibility.Visible;
                                            Image img = new Image();
                                            img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
                                            img.Height = 20;
                                            img.Width = 20;
                                            txt.Content = img;
                                        }
                                    }
                                    else
                                    {
                                        item.ReflexTestingFlag = false;
                                        DataGridColumn column = this.dgTestList.Columns[9];
                                        FrameworkElement fe = column.GetCellContent(item);
                                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                        var thisCell = (DataGridCell)result;
                                        if (thisCell != null)
                                        {
                                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                            txt.Visibility = System.Windows.Visibility.Collapsed;
                                            txt.Content = "";
                                        }
                                    }
                                }
                                else
                                {
                                    item.ReflexTestingFlag = false;
                                    DataGridColumn column = this.dgTestList.Columns[9];
                                    FrameworkElement fe = column.GetCellContent(item);
                                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                    var thisCell = (DataGridCell)result;
                                    if (thisCell != null)
                                    {
                                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                        txt.Visibility = System.Windows.Visibility.Collapsed;
                                        txt.Content = "";
                                    }
                                }

                                #region New Logic
                                if (item.ResultValue != "")
                                {
                                    // Reference Values(Auto Authorization)
                                    if ((Convert.ToDouble(ResultValue) > Convert.ToDouble(item.HighReffValue)))//(item.HighReffValue)))
                                    {
                                        item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                    }
                                    else if ((Convert.ToDouble(ResultValue) < Convert.ToDouble(item.LowReffValue)))
                                    {
                                        item.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                    }
                                    else
                                    {
                                        item.ApColor = "/PalashDynamics;component/Icons/green.png";
                                    }
                                }
                                //Panic Values 
                                if (item.LowerPanicValue >= 0 && item.UpperPanicValue > 0)
                                {
                                    if (item.ResultValue != "")
                                    {
                                        if ((Convert.ToDouble(ResultValue) < (item.LowerPanicValue)) || ((Convert.ToDouble(ResultValue) > (item.UpperPanicValue))))
                                        {
                                            item.ApColorPanic = "/PalashDynamics;component/Icons/yellow.png";
                                        }
                                        else
                                        {
                                            item.ApColorPanic = " ";
                                        }
                                    }
                                    else
                                    {
                                        item.ApColorPanic = " ";

                                    }
                                }
                                else
                                {
                                    item.ApColorPanic = " ";

                                }
                                //Improbable Values 
                                if (item.ResultValue != "")
                                {
                                    if ((Convert.ToDouble(ResultValue) < (item.MinImprobable)) || ((Convert.ToDouble(ResultValue) > (item.MaxImprobable))))
                                    {
                                        item.ApColorImp = "/PalashDynamics;component/Icons/brown.png";
                                    }
                                    else
                                    {
                                        item.ApColorImp = " ";
                                    }
                                }

                                # endregion

                                if (item.IsNumeric == false)
                                {
                                    item.DeltacheckString = "N.A";
                                    item.DeltaCheck = false;
                                    item.IsReadOnly = true;
                                }
                                #endregion
                            }
                        }
                        if (item.IsNumeric == false)
                        { //Read Only When Text Value as per dr priyanka suggession
                            item.IsReadOnly = true;
                        }
                    }
                    else  //Added By Rohinee Dated 9/11/16 to refresh icons
                    {
                        item.ApColorPanic = " ";
                        item.ApColor = " ";
                        item.ApColorImp = "";
                    }                   
                }               
            };
            client2.ProcessAsync(BizAction2, ((IApplicationConfiguration)App.Current).CurrentUser);
            client2.CloseAsync();
            
        }

        private void CalculateParameterFormula(string ResultValue, long ParamSTID)
        {

        }

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
		(a1, a2) => a1 - a2,
		(a1, a2) => a1 + a2,
		(a1, a2) => a1 / a2,
		(a1, a2) => a1 * a2,
		(a1, a2) => Math.Pow(a1, a2)
	};
        public double Eval(string expression)
        {
            try
            {
                List<string> tokens = getTokens(expression);
                Stack<double> operandStack = new Stack<double>();
                Stack<string> operatorStack = new Stack<string>();
                int tokenIndex = 0;

                while (tokenIndex < tokens.Count)
                {
                    string token = tokens[tokenIndex];
                    if (token == "(")
                    {
                        string subExpr = getSubExpression(tokens, ref tokenIndex);
                        operandStack.Push(Eval(subExpr));
                        continue;
                    }
                    if (token == ")")
                    {
                        throw new ArgumentException("Mis-matched parentheses in expression");
                    }
                    //If this is an operator  
                    if (Array.IndexOf(_operators, token) >= 0)
                    {
                        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                        }
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operandStack.Push(double.Parse(token));
                    }
                    tokenIndex += 1;
                }

                while (operatorStack.Count > 0)
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                return operandStack.Pop();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                double dd = 0;
                return dd;
            }
        }
        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }
        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
        private void hyTestDetails_Click(object sender, RoutedEventArgs e)
        {
            TestDetailsChildWindow WinHelpValue = new TestDetailsChildWindow();
            WinHelpValue.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            WinHelpValue.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;
            WinHelpValue.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            WinHelpValue.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;
            WinHelpValue.OrderDetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
            //WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
            WinHelpValue.Show();
        }

        private void cmbTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    if (((MasterListItem)cmbTemplate.SelectedItem).ID == 0)
            //    {
            //        FetchTemplate();
            //    }

        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract || e.Key == Key.Decimal && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }
        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        private void ResultValue_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;

            }
        }

        private void cmbResultEntry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
                        
        }

    }
}


