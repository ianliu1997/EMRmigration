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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;
using System.Text;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using MessageBoxControl;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using System.IO;
using C1.Silverlight.RichTextBox.PdfFilter;
using C1.Silverlight.Pdf;
using PalashDynamics.ValueObjects.User;
using System.Windows.Media.Imaging;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class DoctorApprovalPathoResultEntry : UserControl
    {
        #region List and Variable Declaration
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }
        List<MasterListItem> lstAuthenticationLevel { get; set; }
        private SwivelAnimation objAnimation;
        clsAppConfigVO myAppConfig;
        List<clsUserCategoryLinkVO> UserCategoryLinkedList;
        SolidColorBrush NormalColorCode = new SolidColorBrush();
        SolidColorBrush MinColorCode = new SolidColorBrush();
        SolidColorBrush MaxColorCode = new SolidColorBrush();
        SolidColorBrush FirstLevelcolorCode = new SolidColorBrush();
        SolidColorBrush SecondLevelcolorCode = new SolidColorBrush();
        SolidColorBrush ThirdLevelcolorCode = new SolidColorBrush();
        SolidColorBrush CheckresultColorCode = new SolidColorBrush();
        List<clsGetDeltaCheckValueBizActionVO> PreviousResult;

        public List<clsPathOrderBookingDetailVO> SelectedTestList = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> AgencyList = new List<clsPathOrderBookingDetailVO>();
        long BillId1 = 0;

        #region BackPanel Variables
        List<clsPathOrderBookingDetailVO> lstTest = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemplstTest = new List<clsPathOrderBookingDetailVO>();
        public ObservableCollection<clsPathoTestParameterVO> TestList { get; set; }
        public ObservableCollection<clsPathoTestParameterVO> RemovedTestList { get; set; }
        List<clsPathPatientReportVO> OrderList = new List<clsPathPatientReportVO>();
        int ClickedFlag = 0;
        int ClickedFlag1 = 0;
        bool IsPageLoded = false;
        bool IsUpdate = false;
        List<clsPathoTestParameterVO> MarkBoldList { get; set; }
        float balamt = 0;
        PagedCollectionView collection;
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
        // Added by Anumani
        public List<string> DeltaResultValue = new List<string>();
        List<clsPathOrderBookingDetailVO> List1 = new List<clsPathOrderBookingDetailVO>();
          List<clsPathOrderBookingDetailVO> SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
          long BillId = 0;
        //static  public long ReportBillId = 0;
        //static  public string ReportBillNo = " ";
          public long PathologistId { get; set; }
          Color myRgbColor;
          Color myRgbColor1;
          Color myRgbColor3;
        #endregion
        #endregion
        public DoctorApprovalPathoResultEntry()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(NewPathologyWorkOrderGeneration_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            SelectedTestList = new List<clsPathOrderBookingDetailVO>();
            myAppConfig = new clsAppConfigVO();
            myAppConfig = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;
            UserCategoryLinkedList = new List<clsUserCategoryLinkVO>();
            UserCategoryLinkedList = ((IApplicationConfiguration)App.Current).CurrentUser.UserCategoryLinkList;

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
            lstSampleType = new List<MasterListItem>();
            lstUploadStatus = new List<MasterListItem>();
            lstDeliveryStatus = new List<MasterListItem>();
            //lstSampleType.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            //lstSampleType.Add(new MasterListItem() { ID = 1, Description = "Finalized", Status = true });
            //lstSampleType.Add(new MasterListItem() { ID = 2, Description = "Not Finalized", Status = true });
            //cmbSampleType.ItemsSource = lstSampleType;
            //cmbSampleType.SelectedItem = lstSampleType[0];

            lstAuthenticationLevel = new List<MasterListItem>();
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 1, Description = "Result Entry Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 2, Description = "Result Entry Not Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 3, Description = "Supervisor Approval Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 4, Description = "Doctor Approval Done", Status = true });
            lstAuthenticationLevel.Add(new MasterListItem() { ID = 5, Description = "Forwarded To Check Result", Status = true });
          //  CmbAuthentication.ItemsSource = lstAuthenticationLevel;
           // CmbAuthentication.SelectedItem = lstAuthenticationLevel[0];

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            mElement1.Text = "Doctor Authorization";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
           // fillCatagory();
            FillOrderBookingList();
            //SetColorCode();
            //added by rohini dated 10.2.16
            fillSampleStatus();
            fillAuthorization();
            fillBillType();
            //
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

        #region Private Methods and Event Handlers
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
        private void fillesultEntry()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Done"));
            MList.Add(new MasterListItem(2, "Not Done"));

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
            fillDeliveryType();
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
                    cmbMachine.ItemsSource = null;
                    cmbMachine.ItemsSource = objList;
                    cmbMachine.SelectedItem = objList[0];
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
        }

        private void fillAuthorization()   //changed for CR Points
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));           
            MList.Add(new MasterListItem(1, "Pending"));
            MList.Add(new MasterListItem(2, "Partial Done"));
            //MList.Add(new MasterListItem(3, "Completed"));
            MList.Add(new MasterListItem(3, "Authorized"));

            cmbAuthorization.ItemsSource = null;
            cmbAuthorization.ItemsSource = MList;
            cmbAuthorization.SelectedItem = MList[0];
            
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



        #endregion
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

                        objList1.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                       
                        //CmbCategory.ItemsSource = objList;
                        //CmbCategory.SelectedItem = objList[0];


                        //if (this.DataContext != null)
                        //{
                        //    CmbCategory.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).CategoryID;

                        //}
                        foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                        {
                           foreach(var item in objList)
                            {
                                if (item1.CategoryID == item.ID)
                                {
                                    objList1.Add(item);
                                }

                            }


                        }

                        cmbTestCategory.ItemsSource = null;
                        cmbTestCategory.ItemsSource = objList1;
                        cmbTestCategory.SelectedItem = objList1[0];

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
        private void FillOrderBookingList()
        {
            ClickedFlag1 = 0;
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.PatientID = 0;
                //added by rohini dated 29.2.16
                POBBizAction.IsDispatchedClinic = true;
                POBBizAction.IsFrom = "Authorization";

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
                //by rohini for CR Points
                if (cmbAuthorization.SelectedItem != null && ((MasterListItem)cmbAuthorization.SelectedItem).ID != 0)
                {
                    POBBizAction.StatusID = ((MasterListItem)cmbAuthorization.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.StatusID = 0;
                }
                //BY ROHINI FOR CR POINTS
                if (cmbBillType.SelectedItem != null)
                {
                    POBBizAction.PatientType = ((MasterListItem)cmbBillType.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.PatientType = 0;
                }
                if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
                {
                    POBBizAction.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
                }
                if ((MasterListItem)cmbProcessedBy.SelectedItem != null)
                {
                    if (((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)   // report by hand
                    {
                        POBBizAction.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
                    }
                    else
                    {
                        POBBizAction.AgencyID = 0;
                    }
                }

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
                            //DataList.Count = result.TotalRows1;
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

                                txtTotalCountRecords.Text = "";
                                txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();

                                NoOfWorkOrder.Visibility = Visibility.Visible;
                                txtTotalCountRecords.Visibility = Visibility.Visible;
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
            BizAction.IsFrom = "Authorization";
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

            if ((MasterListItem)cmbResultEntry.SelectedItem != null)
            {
                if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 1)   //result entry done
                {
                    BizAction.OrderDetail.IsResultEntry = true;
                }
                else
                {
                    BizAction.OrderDetail.IsResultEntry = false;
                }
                if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 2)  //result entry Not done
                {
                    BizAction.OrderDetail.IsResultEntry1 = true;
                }
                else
                {
                    BizAction.OrderDetail.IsResultEntry1 = false;
                }
            }
            if ((MasterListItem)cmbDelivery.SelectedItem != null)
            {
                if (((MasterListItem)cmbDelivery.SelectedItem).ID == 1)   //result report done
                {
                    BizAction.OrderDetail.IsDelivered = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDelivered = false;
                }
                if (((MasterListItem)cmbDelivery.SelectedItem).ID == 2)  //result report Not done
                {
                    BizAction.OrderDetail.IsDelivered1 = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDelivered1 = false;
                }
            }
            if ((MasterListItem)cmbDeliveryType.SelectedItem != null)
            {
                if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 1)   //report by email
                {
                    BizAction.OrderDetail.IsDeliverdthroughEmail = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDeliverdthroughEmail = false;
                }
                if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 2)   // report by hand
                {
                    BizAction.OrderDetail.IsDirectDelivered = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDirectDelivered = false;
                }
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
                BizAction.OrderDetail.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
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

                            if (item.IsOutSourced == true )
                            {
                                item.SampleOutSourceImage = "../Icons/tick.png";
                                item.IsOutSourcedD = false;
                            }                                //by rohinee for only dispatched unit should have rights to display reports
                            else if (item.DispatchToID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.UnitId !=((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            {
                                item.SampleOutSourceImage = "../Icons/error.png";
                                item.IsOutSourcedD = false;
                            }
                            else if (item.ReportTemplate == true)
                            {
                                item.SampleOutSourceImage = "../Icons/error.png";
                                item.IsOutSourcedD = false;
                            }
                            else
                            {
                                item.SampleOutSourceImage = "../Icons/error.png";
                                item.IsOutSourcedD = true;
                            } 
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
                        //fillCatagory();  //by rohini commented
                        FillTestList();  //new by rohini  1/6
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
                SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
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

        private void CmdResultEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if((((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.
                //added by rohini dated 29.2.16

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsProcessingUnit == true)
                {
                    //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                    //{
                    //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Dispatched To Another Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
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
                       // CmdMachinlinking.Visibility = Visibility.Collapsed;
                        CmdPrintSelected.Visibility = Visibility.Collapsed;
                        ResultListContent.Content = new PalashDynamics.SearchResultLists.DisplayPatientDetails();

                        BillId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID;

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
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please, Select Lab Order.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        indicator.Close();
                    }
                }
                else
                {

                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName + ", Is Not A Processing Unit.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
            }
            catch (Exception)
            {
                throw;
                indicator.Close();
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
                CmdPrintSelected.Visibility = Visibility.Visible;
                SelectedTestList.Clear();
              //  CatagoryWiseTestList.Clear();
                //CatagoryWiseTestList = new ObservableCollection<clsPathoTestParameterVO>();
                SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                ClearData();
                FillOrderBookingList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region BackPanel

        #region Private Methods

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
                    //    objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                        txtReferenceDoctor.ItemsSource = null;
                        txtReferenceDoctor.ItemsSource = objList;
                        if (dgOrdertList.SelectedItem != null)
                        {
                            txtReferenceDoctor.SelectedItem = objList.ToList().Where(z => z.ID == ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DoctorID).FirstOrDefault();
                            txtReferenceDoctor.Text = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ReferredBy;
                        }
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
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
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
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                cmbCategory.IsEnabled = false;
            }
            catch (Exception)
            {
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
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetPathologistBizActionVO)arg.Result).MasterList);
                        cmbPathologist1.ItemsSource = null;
                        cmbPathologist1.ItemsSource = objList;
                        cmbPathologist1.SelectedItem = objList[0];
                    }


                    if (PathologistId != 0)
                    {
                        cmbPathologist1.SelectedValue = PathologistId;
                        cmbPathologist1.IsEnabled = false;
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

        private void FillTestList()
        {
            try
            {
                clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                BizAction.IsFrom = "Authorization";
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
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbTestCategory.ItemsSource = null;
                        cmbTestCategory.ItemsSource = objList;
                        cmbTestCategory.SelectedItem = objList[0];
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
            ClickedFlag = 0;
            richTextBox.Html = HTMLContent;
        }
        private void GetParameters()
        {
            clsGetPathoTestParameterAndSubTestDetailsBizActionVO BizAction = new clsGetPathoTestParameterAndSubTestDetailsBizActionVO();
            try
            {
                BizAction.TestList = new List<clsPathoTestParameterVO>();
                OrderList = new List<clsPathPatientReportVO>();
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
                            obj.SampleNo = item.SampleNo;  //by rohini
                            OrderList.Add(obj);
                        }
                    }
                    BizAction.SampleNo = Convert.ToString(lstTest[0].SampleNo);
                }
                if (BizAction.TestID.EndsWith(","))
                    BizAction.TestID = BizAction.TestID.Remove(BizAction.TestID.Length - 1, 1);

                if (BizAction.DetailID.EndsWith(","))
                    BizAction.DetailID = BizAction.DetailID.Remove(BizAction.DetailID.Length - 1, 1); //by rohini

                if (cmbCategory.SelectedItem != null)
                    BizAction.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;

                BizAction.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                BizAction.PatientUnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;

                if (dgOrdertList.SelectedItem != null)
                {
                    BizAction.AgeInDays = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).AgeInDays;
                    //  BizAction.Da = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DateOfBirth;
                }


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        dgTestList.ItemsSource = null;
                        if (((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList != null)
                        {
                            List<clsPathoTestParameterVO> TestDetails = new List<clsPathoTestParameterVO>();
                            TestDetails = ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList;
                            foreach (var item in TestDetails)
                            {
                                TestList.Add(item);
                            }
                            GetResultEntry();
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
                                        /* Logic developed considering that none of the ranges, Improbable, Panic , RefValue 
                                         * lies within each other.
                                         */

                                        // Reference Values(Auto Authorization)
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

                                        //Panic Values 
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
                                else if (IsNumeric == true && item5.IsMachine == true)
                                {
                                    bool IsValid = item5.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
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
                                            //Panic Values 
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

                                                    //foreach (var item in HelpValues)
                                                    //{
                                                    //    if (((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).ParameterDetails.ResultValue == item.HelpValue)  //if (item5.ResultValue == item.HelpValue)
                                                    //    {
                                                    //        if (item.IsAbnormal == true)
                                                    //        {

                                                    //            //item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                    //            ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).ParameterDetails.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            item5.ApColor = " ";
                                                    //            //item5.ApColor = "/PalashDynamics;component/Icons/green.png";
                                                    //            ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).ParameterDetails.ApColor = "/PalashDynamics;component/Icons/green.png";
                                                    //        }
                                                    //    }
                                                    //}

                                                    //dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
                                                    //NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList; //Newet added

                                                    for (int i = 0; i < TestList.Count; i++)
                                                    {

                                                        if (TestList[i].ParameterID == ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).ParameterDetails.ParameterID)
                                                        {
                                                            foreach (var item in HelpValues)
                                                            {
                                                                if (TestList[i].ResultValue == item.HelpValue)
                                                                {
                                                                    if (item.IsAbnormal == true)
                                                                    {

                                                                        //item5.ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                                        TestList[i].IsAbnoramal = true;
                                                                        TestList[i].ApColor = "/PalashDynamics;component/Icons/orange.png";
                                                                    }
                                                                    else
                                                                    {
                                                                        //item5.ApColor = " ";
                                                                        //item5.ApColor = "/PalashDynamics;component/Icons/green.png";
                                                                        TestList[i].IsAbnoramal = false;
                                                                        TestList[i].ApColor = "/PalashDynamics;component/Icons/green.png";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    PagedCollectionView Collection2 = new PagedCollectionView(TestList);
                                                    //Collection2.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));
                                                    //Collection2.GroupDescriptions.Add(new PropertyGroupDescription("PathoSubTestName"));
                                                    Collection2.GroupDescriptions.Add(new PropertyGroupDescription("TestAndSampleNO"));  //by rohini for sample no

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

                            if (TestList != null && TestList.Count > 0)
                            {
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
                            }
                            if (SelectedTestParameterValues != null && SelectedTestParameterValues.Count > 0)
                            {
                                foreach (var item in SelectedTestParameterValues)
                                {
                                    foreach (var item1 in TestList)
                                    {
                                        if (item.PathoTestID == item1.PathoTestID && item.ParameterID == item1.ParameterID)
                                        {
                                            item1.ResultValue = item.ResultValue;
                                            item1.FootNote = item.FootNote;
                                            item1.Note = item.Note;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (TestList != null && TestList.Count > 0)
                            {
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
                                                // item.IsReadOnly = true;       // for unit testinh purpose, change afterwards
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
                                                // item.IsReadOnly = true;       // for unit testinh purpose, change afterwards
                                                item.IsReadOnly = false;
                                                item.IsNumeric = false;
                                            }
                                        }
                                    }
                                }
                           
                            PagedCollectionView Collection = new PagedCollectionView(TestList);
                            //Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));
                            //Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoSubTestName"));
                            Collection.GroupDescriptions.Add(new PropertyGroupDescription("TestAndSampleNO"));  //by rohini for sample no
                                                
                            dgTestList.ItemsSource = null;
                            dgTestList.ItemsSource = Collection;
                            }
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
        void GetTemplateResultEntry()
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
                if (BizAction.DetailID != null)
                {
                    if (BizAction.DetailID.EndsWith(","))
                        BizAction.DetailID = BizAction.DetailID.Remove(BizAction.DetailID.Length - 1, 1);
                }

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultList != null)
                        {
                            clsPathPatientReportVO ObjDetails;
                            ObjDetails = ((clsGetPathoFinalizedEntryBizActionVO)arg.Result).ResultEntryDetails;
                            if (ObjDetails.TemplateDetails != null)
                            {
                                cmbPathologist1.SelectedValue = ObjDetails.TemplateDetails.PathologistID;
                                cmbPathologist1.IsEnabled = false;

                                grdTemplate.DataContext = ObjDetails.TemplateDetails;
                                richToolbar.IsEnabled = false;
                                richTextBox.IsReadOnly = true;
                                richTextBox.Html = ObjDetails.TemplateDetails.Template;
                                //if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                                //    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";
                                cmbTemplate.SelectedValue = ObjDetails.TemplateDetails.TemplateID;
                                TemplateID = ObjDetails.TemplateDetails.TemplateID;
                               
                                PrintTemplate = true;
                                FillTemplate();
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
        MessageBoxControl.MessageBoxChildWindow msgW1;
        private bool Validation()
        {
            bool IsValidate = true;
            if (dgTest1.SelectedItem != null)
            {
                clsPathOrderBookingDetailVO objVO = dgTest1.SelectedItem as clsPathOrderBookingDetailVO;
                if (objVO.IsSampleCollected == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Not Collected For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsSampleDispatch == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Not Dispatched For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsSampleReceive == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Not Received For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.SampleAcceptRejectStatus == 0)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Not Accepted For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.SampleAcceptRejectStatus == 2)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Rejected For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.IsOutSourced == true)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is  Outsourced For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.FirstLevel == false)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Result Entry Not Done For " + objVO.TestName + " TEST. \n You Cannot Do Doctor Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
                }
                else if (objVO.DispatchToID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                {
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Sample Is Dispatched To Other Clinic You Can Not Do Authorization For This Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValidate = false;
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

        private bool CheckValidation()
        {
            bool result = true;
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
            if (IsPageLoded)
            {
                if (TabTestDetails.Visibility == Visibility.Visible)
                {

                    if (dgTestList.ItemsSource == null && TemplateID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW13 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "You Cannot Save Doctor Authorization  Without Test Details Or Template Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW13.Show();
                        TabControlMain.SelectedIndex = 0;
                        result = false;
                        ClickedFlag = 0;
                        return result;
                    }
                }

                if (TabTemplateDetails.Visibility == Visibility.Visible)
                {
                    if (richTextBox.Html == null || richTextBox.Html == "")
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW13 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW13.Show();
                        TabControlMain.SelectedIndex = 1;
                        result = false;
                        ClickedFlag = 0;
                        return result;
                    }
                }
            }
            clsPathOrderBookingDetailVO objVO = dgTest1.SelectedItem as clsPathOrderBookingDetailVO;
            if ( objVO != null &&  objVO.IsResultEntry == false )
            {
                MessageBoxControl.MessageBoxChildWindow msgW13 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Result Entry Is Not Done For " + objVO.TestName + " Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW13.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (lstTest != null && lstTest.Count > 0 && res == MessageBoxResult.OK)
                    {
                        lstTest.ToList().Where(z => z.TestID == objVO.TestID).ToList().Where(x => x.IsSelected = false).ToList();
                        collection = new PagedCollectionView(lstTest);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("ReportType"));
                        dgTest1.ItemsSource = null;
                        dgTest1.ItemsSource = collection;
                        dgTest1.SelectedIndex = -1;
                        ClickedFlag = 0;
                        result = false;

                    }
                };
                msgW13.Show();
            }
            
            if (SelectedTestList != null && SelectedTestList.Count > 0)
            {
                if (SelectedTestList.Where(z => z.ReportType != "Template" ).ToList().Count > 0)
                {
                    if (TestList == null)   //change 17/11/16  removed dgTestList.ItemsSource == null &&
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please,Get Parameters Of Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        ClickedFlag = 0;
                        return result;
                    }
                    else if (TestList.Count == 0)   //change 17/11/16  removed dgTestList.ItemsSource == null &&
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please,Get Parameters Of Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        ClickedFlag = 0;
                        return result;
                    }                   
                }
            }

            if (SelectedTestList != null && SelectedTestList.Count > 0)
            {
                foreach (var item in SelectedTestList)
                {
                    if (item.SecondLevel == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Test Already Approved By Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        ClickedFlag = 0;
                        return result;

                    }

                }
            }
            return result;
        }


       
        private void Save()
        {
            string SampleNo = "";
         
            clsAddPathPatientReportBizActionVO BizAction = new clsAddPathPatientReportBizActionVO();
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            strPatInfo = new StringBuilder();
            strDoctorPathInfo = new StringBuilder();
            
            try
            {
                BizAction.OrderList = new List<clsPathPatientReportVO>();
                if (OrderList != null && OrderList.Count > 0)
                    BizAction.OrderList = OrderList;
                foreach (var item in BizAction.OrderList)
                {
                    item.RefDoctorID = ((MasterListItem)cmbPathologist1.SelectedItem).ID;
                    item.ReferredBy = ((MasterListItem)cmbPathologist1.SelectedItem).Description;
                    item.IsFirstLevel = true;
                    item.IsSecondLevel = true;
                    item.IsFinalized = true;
                    item.IsDoctorAuthorization = true;
                    item.ApproveBy = ((IApplicationConfiguration)App.Current).CurrentUser.UserNameNew;
                    item.IsAutoApproved =true;                    
                }
                if (cmbPathologist1.SelectedItem != null)
                {
                    BizAction.OrderList.ForEach(z => z.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID);
                }
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromPathology;
                if (TestList != null && TestList.Count > 0)
                {
                    foreach (var item in TestList)
                    {
                        item.ApColor = null;
                        
                    }
                    BizAction.TestList = TestList.ToList();
                }

              
                BizAction.OrderPathPatientReportList.OrderID = TemplateOrderID;
                BizAction.OrderPathPatientReportList.PathOrderBookingDetailID = TemplatePathOrderBookingDetailID;
                BizAction.OrderPathPatientReportList.TestID = TemplateTestID;
                if (TemplateSampleNo != null)
                    BizAction.OrderPathPatientReportList.SampleNo = TemplateSampleNo.ToString();//Convert.ToInt64(TemplateSampleNo);
                if (TemplateSampleCollectionDatetime != null)
                    BizAction.OrderPathPatientReportList.SampleCollectionTime = Convert.ToDateTime(TemplateSampleCollectionDatetime);
                if (cmbPathologist1.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID;
                if (txtReferenceDoctor.SelectedItem != null)
                {
                    BizAction.OrderPathPatientReportList.RefDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
                    BizAction.OrderPathPatientReportList.ReferredBy = ((MasterListItem)txtReferenceDoctor.SelectedItem).Description;
                }
                BizAction.OrderPathPatientReportList.Status = true;
                if (!string.IsNullOrEmpty(richTextBox.Html))
                    BizAction.OrderPathPatientReportList.TemplateDetails.Template = richTextBox.Html;
                if (TemplateID != null)
                {
                    BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID = TemplateID;
                    BizAction.OrderPathPatientReportList.ISTEMplate = IsTempate;
                    BizAction.OrderPathPatientReportList.ApproveByTemplate = ((IApplicationConfiguration)App.Current).CurrentUser.UserNameNew; ;


                }

                BizAction.OrderPathPatientReportList.IsFirstLevel = true;
                BizAction.OrderPathPatientReportList.IsSecondLevel = true;
                BizAction.OrderPathPatientReportList.IsThirdLevel = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        GetResultEntry();
                        if (arg.Error == null)
                        {
                            long OrderUnitID = 0;
                            if (dgOrdertList.SelectedItem != null)
                            {
                                OrderUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                            }
                            if (((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.TemplateDetails.ID != 0)
                                GetPatientDetailsInHtml(((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.TemplateDetails.ID, BizAction.OrderPathPatientReportList.IsFinalized, OrderUnitID);
                        }

                        List<clsPathOrderBookingDetailVO> objList = new List<clsPathOrderBookingDetailVO>();
                        objList.AddRange(((clsAddPathPatientReportBizActionVO)arg.Result).MasterList);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Result Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                ////ClearData();
                                ////FillOrderBookingDetailsList();
                                //foreach (var item in ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList)
                                //{
                                //    PrintReport(item.OrderID, true, null, null);
                                //    break;
                                //}

                                OrderList = ((clsAddPathPatientReportBizActionVO)arg.Result).OrderList;                            


                                    ISFinalize = ((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.IsFinalized;

                                    if (TabTemplateDetails.Visibility == Visibility.Visible && PrintTemplate == true)
                                    {
                                        PrintReport(((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.TemplateDetails.ID, ISFinalize, strPatInfo.ToString(), strDoctorPathInfo.ToString());
                                    }
                                    else
                                    {
                                        BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;


                                        //  SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                                        //   PrintReport(0, ISFinalize, strPatInfo.ToString(), strDoctorPathInfo.ToString());


                                        // if(((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.


                                        //foreach (var item in objList)
                                        //{
                                        //    if (item.Status == true)
                                        //    {
                                        //        SampleNo = SampleNo + item.Description;
                                        //    }
                                        //    SampleNo = SampleNo + ",";
                                        //}

                                        foreach (var item in objList)
                                        {
                                            if (item.Status == true)
                                            {
                                                SampleNo = SampleNo + item.SampleNo;
                                            }
                                            SampleNo = SampleNo + ",";
                                        }

                                        if (SampleNo.EndsWith(","))
                                            SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                                    

                                        PrintPathologyReport(SampleNo);
                                        //


                                        // For the purpose of Displaying the buttons properly

                                        objAnimation.Invoke(RotationType.Backward);
                                        lblresult.Visibility = Visibility.Collapsed;
                                        CmdResultEntry.Visibility = Visibility.Visible;
                                        GrpColor.Visibility = Visibility.Collapsed;
                                        lblnormal.Visibility = Visibility.Collapsed;
                                        txtNormalValue.Visibility = Visibility.Collapsed;
                                        //lblmin.Visibility = Visibility.Collapsed;
                                        //txtMinValue.Visibility = Visibility.Collapsed;
                                        lblmax.Visibility = Visibility.Collapsed;
                                        txtMaxValue.Visibility = Visibility.Collapsed;
                                        CmdPrintSelected.Visibility = Visibility.Visible;
                                        cmdSave.Visibility = Visibility.Collapsed;
                                        CmdApprove.Visibility = Visibility.Collapsed;
                                        btnCheckResults.Visibility = Visibility.Collapsed;
                                        SelectedTestList.Clear();
                                        SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                                        ClearData();
                                        FillOrderBookingList();

                                    }
                                  //}
                                    //else
                                    //{
                                    //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please first pay the bill then Take the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    //    mgbx.Show();

                                    //}
                                }                              
                            
                        };
                        msgW1.Show();
                        indicator.Close();
                        objAnimation.Invoke(RotationType.Backward);

                        CmdResultEntry.Visibility = Visibility.Visible;
                        cmdSave.Visibility = Visibility.Collapsed;
                        //GrpColor.Visibility = Visibility.Visible;
                       // lblresult.Visibility = Visibility.Visible;
                       // lblnormal.Visibility = Visibility.Visible;
                        ///txtNormalValue.Visibility = Visibility.Visible;
                       // lblmax.Visibility = Visibility.Visible;
                       // txtMaxValue.Visibility = Visibility.Visible;
                        // CmdMachinlinking.Visibility = Visibility.Collapsed;
                        CmdPrintSelected.Visibility = Visibility.Visible;
                        SelectedTestList.Clear();
                        SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                        ClearData();
                        FillOrderBookingList();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Adding Doctor Authorization .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        indicator.Close();
                        objAnimation.Invoke(RotationType.Backward);
                        CmdResultEntry.Visibility = Visibility.Visible;
                        cmdSave.Visibility = Visibility.Collapsed;
                        //GrpColor.Visibility = Visibility.Visible;
                        // lblresult.Visibility = Visibility.Visible;
                        // lblnormal.Visibility = Visibility.Visible;
                        ///txtNormalValue.Visibility = Visibility.Visible;
                        // lblmax.Visibility = Visibility.Visible;
                        // txtMaxValue.Visibility = Visibility.Visible;
                        // CmdMachinlinking.Visibility = Visibility.Collapsed;
                        CmdPrintSelected.Visibility = Visibility.Visible;
                        FillOrderBookingList();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }

            catch (Exception ex)
            {
                throw;
                indicator.Close();
            }

        }

      

        private void PrintPathologyReport(string SampleNo)
     
        {
            bool IsPrinted = false;
            //string SampleNo = "";
            if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
            {

                double BalanceAmount = Convert.ToDouble(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance);
                string ResultID = "";
                //if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || (BalanceAmount == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))
                //{
                if ((BalanceAmount == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                {
                    if (dgOrdertList.SelectedItem != null)
                    {
                        if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                        {
                          //  BillNumber = ReportBillNo; 
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsPrinted;

                            BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;

                           
                            ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID.ToString();


                     //       ResultID = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID);
                           // ResultID = ReportBillId.ToString();

                            //
                            // Commented in order to get the BillId of Selected row
                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            //{
                            //    foreach (var item in lstTest)
                            //    {
                            //        ResultID = ResultID + item.PathPatientReportID;
                            //        ResultID = ResultID + ",";
                            //    }s
                            //    if (ResultID.EndsWith(","))
                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                            //}
                        }
                        else
                        {
                           // BillNumber = ReportBillNo;
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsPrinted;
                           // ResultID = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID);
                           // BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;

                            //  IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                            //  ResultID = ReportBillId.ToString();
                            ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID.ToString();

                            BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;

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
                        }


                        //if (dgTest.SelectedItem != null)
                        //{
                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            //{
                            //    foreach (var item in SelectedTestDetails)
                            //    {
                            //       // ResultID = item.BillID.ToString();
                                    
                            //        SampleNo = SampleNo + item.SampleNo;
                            //        SampleNo = SampleNo + ",";
                            //    }
                            //    if (SampleNo.EndsWith(","))
                            //        SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                            //}
                        //}

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


                        if (IsPrinted == false)
                        {
                            IsDuplicate = false;
                        }
                        else
                        {
                            IsDuplicate = true;
                        }

                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    }
                }
                    
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Collect The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                       mgbx.Show();

                    mgbx.Show();
                }
            }
            else
            {
            //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Please Select Test For Print.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //    mgbx.Show();
            }
        }

        private void GetPatientDetailsInHtml(long ResultId, bool IsFinalize, long OrderUnitID)
        {

            clsPathoResultEntryPrintDetailsBizActionVO BizAction = new clsPathoResultEntryPrintDetailsBizActionVO();

            BizAction.ID = ResultId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsDelivered = 0;
            BizAction.ResultIds = "";

            //((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsPathoResultEntryPrintDetailsVO PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
                        PatientResultEntry = ((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;
                        strPatInfo = new StringBuilder();
                        strPatInfo.Append(PatientResultEntry.PatientInfoHTML);
                        strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
                        strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy"));
                        strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.Salutation.ToString() + " " + PatientResultEntry.PatientName.ToString());
                        strPatInfo = strPatInfo.Replace("[AgeYear]", "    :" + PatientResultEntry.AgeYear.ToString() + "yrs");
                        strPatInfo = strPatInfo.Replace("[AgeMonth]", "    :" + PatientResultEntry.AgeMonth.ToString() + "Mnt");
                        strPatInfo = strPatInfo.Replace("[AgeDate]", "    :" + PatientResultEntry.AgeDate.ToString() + "Dys");
                        strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());
                        //if (PatientResultEntry.ShowinPathoReport == false)
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + "Dr. " + PatientResultEntry.ReferredBy.ToString());
                        //}
                        //else
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + " ");
                        //}
                        strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.SampleNo.ToString());
                        strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                        strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                        strPatInfo = strPatInfo.Replace("[TemplateTestName]", PatientResultEntry.Test.ToString());
                        strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
                        strDoctorPathInfo = new StringBuilder();
                        strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);

                        byte[] imageBytes = null;
                        string imageBase64 = string.Empty;
                        string imageSrc = string.Empty;

                        if (PatientResultEntry.Pathologist1 != null)
                        {
                            if (PatientResultEntry.Signature1 != null)
                            {
                                imageBytes = PatientResultEntry.Signature1;

                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
                            }
                            else
                            {
                                strDoctorPathInfo.Replace("[%Image1%]", "hidden");
                            }

                            strDoctorPathInfo.Replace("[%Pathalogist4%]", PatientResultEntry.Pathologist1);
                            strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education1);
                        }
                        else
                        {
                            strDoctorPathInfo.Replace("[%Pathalogist4%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Image1%]", "hidden");
                        }
                        richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                        richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());

                        PrintReport(ResultId, IsFinalize, strPatInfo.ToString(), strDoctorPathInfo.ToString());
                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();

        }
        private void PrintReport(long ResultID, bool IsFinalize, string PatientInfoString, string DoctorInfoString)
        {
            int ISRferalDoctorSignature = 0;
            //if (ReferDoctorSignature.IsChecked == true)//This is the Check Box Tht Me Takle
            //{
            //    ISRferalDoctorSignature = 1;
            //}

            if (TabTemplateDetails.Visibility == Visibility.Visible && PrintTemplate == true)
            {
                //int IsDelivered = 0;
                ////bool IsResultEntry = false;
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //string URL = "../Reports/Pathology/PathoTemplateResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&IsDelivered=" + IsDelivered;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

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

               // print.Print("A Christmas Carol");


            }
            else
            {
                if (ResultID > 0)
                {
                    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature;
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
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoViewTemplateBizActionVO)arg.Result).Template != null)
                        {
                            richTextBox.Html = ((clsGetPathoViewTemplateBizActionVO)arg.Result).Template.Template;
                            TemplateID = ((clsGetPathoViewTemplateBizActionVO)arg.Result).Template.ID;
                            string rtbstring = string.Empty;
                            string styleString = string.Empty;

                            rtbstring = richTextBox.Html;

                            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                            {
                                rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                                richTextBox.Html = rtbstring;
                                TemplateContent = rtbstring;
                            }
                            if (!(richTextBox.Html.Contains("[%DOCTORINFO%]")))
                            {
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
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
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
                                GetParameters();
                                cmbCategory.ClearValidationError();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "Select Test To Get Parameter .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information); msgW1.Show();
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
                        ClickedFlag = 0;
                    }
                }


                catch (Exception)
                {
                    ClickedFlag = 0;
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
                    if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue != "")
                    {
                        if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue.IsItCharacter())
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Result Value in Correct Format.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
                            Color C = new Color();
                            C.R = 198;
                            C.B = 24;
                            C.G = 15;
                            C.A = 255;

                            string[] NoramlRange1 = null;
                            char[] Splitchar = { '-' };
                            NoramlRange1 = ((clsPathoTestParameterVO)dgTestList.SelectedItem).NormalRange.Split(Splitchar);

                            if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) > (Convert.ToDouble(NoramlRange1[1]))) // && Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) <= (Convert.ToDouble(NoramlRange1[1])))
                            {
                               // ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathomaxColorCode;

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

                               // ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathominColorCode;

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
                               // ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = myAppConfig.pathonormalColorCode;

                                DataGridColumn column = dgTestList.Columns[2];
                                FrameworkElement fe = column.GetCellContent(dgTestList.SelectedItem);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.FontWeight = FontWeights.Normal;
                                }
                            }
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
            WinHelpValue.IsNotFirstLevel = true;
            WinHelpValue.ParameterID = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
            WinHelpValue.IsFromAuthorization = true;//by rohinee
     //       WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
            WinHelpValue.Show();
        }

        void WinHelpValue_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            HelpValueDetails ObjHelpValueDetails = (HelpValueDetails)sender;
            if (ObjHelpValueDetails.SelectedItems != null)
            {
                foreach (var item in ObjHelpValueDetails.SelectedItems)
                {
                    if (TestList.Where(Items => Items.ParameterID == item.ParameterID).Any() == true)
                    {
                        ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue = item.HelpValue;
                    }
                }
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
          
            if ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem != null)
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    bool SaveTemplate = true;
                    int divisor = 1;
                    SaveTemplate = CheckValidation();
                    //added by rohinee dated 9/11/16 to 
                    var test1 = from r in lstTest
                                where r.IsSelected == true  //&& r.ReportTemplate == false
                                select r;

                    if (test1.ToList().Count > 0)
                    {
                        var test123 = from r in test1
                                      where r.FirstLevel == true && r.SecondLevel == false
                                      select r;
                        // divisor = test1.TakeWhile(p => p.FirstLevel == true && p.SecondLevel == false).Count();
                        int abc = test123.ToList().Count;

                      
                        if (SaveTemplate == true)
                        {
                            //if (abc != 0)
                            //{
                                string msgTitle = "Palash";
                                string msgText = "Are You Sure You Want To Approve The Doctor Authorization?";

                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                                msgW.Show();
                            //}

                            //else   //By Rohinee dated 9/11/16 to 
                            //{
                            //    ClickedFlag = 0;
                            //    string msgTitle = "Palash";
                            //    string msgText = "Selected Test Are Already Approved";

                            //    MessageBoxControl.MessageBoxChildWindow msgW =
                            //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            //    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                            //    msgW.Show();
                            //}
                        }

                    }
                }
                else
                {
                    ClickedFlag = 0;
                    string msgTitle = "Palash";
                    string msgText = "Please Select The Test to Approve";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select The Test to Approve";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
                ClickedFlag = 0;

            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
            else
            {
                ClickedFlag = 0;
            }
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
        bool BackPanelTemplate = false;
        public String TemplateContent = String.Empty;
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SelectedTestDetails.Clear();
            if (Validation())
            {
                try
                {
                    if (dgTest1.SelectedItem != null)
                    {
                        clsPathOrderBookingDetailVO objVO = dgTest1.SelectedItem as clsPathOrderBookingDetailVO;
                        if (((CheckBox)sender).IsChecked == false)
                        {
                            #region Remove From Selected Test List
                            ClickedFlag1 = 0; 
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
                                // richTextBox.Html = null;
                                // richTextBox = new C1RichTextBox();
                                //richTextBox.ViewManager.PresenterInfo = null;
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
                        }
                            #endregion
                        else
                        {


                            richTextBox.Html = HTMLContent; // changed on 17122016
                            ClickedFlag1 = 0;  //by rohini 

                                #region Check box Checked
                                #region Add To Selected Test List
                                if (SelectedTestList != null)
                                {
                                    SelectedTestList.Add(objVO);

                                }
                                #endregion

                                #region validation For Single Template Test
                                var TestDtls = from r in SelectedTestList
                                               where r.ReportTemplate == true
                                               select r;
                                IsTempate = true;
                                if (TestDtls.ToList().Count > 1)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("PALASH", "Select Only One Test Template At a time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

                                    #region For Test Having Report Template and Result Entry Done
                                    if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).IsResultEntry == true)
                                    {
                                        //if (richTextBox.ViewManager == null)
                                        if (richTextBox.ViewManager == null)
                                        {
                                            TabTemplateDetails.Visibility = Visibility.Visible;
                                            TabControlMain.SelectedIndex = 1;
                                            PrintTemplate = true;
                                            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                           
                                            GetTemplateResultEntry();
                                        }
                                        else if (richTextBox.ViewManager != null)  //by rohinee
                                        {
                                            TabTemplateDetails.Visibility = Visibility.Visible;
                                            TabControlMain.SelectedIndex = 1;
                                            PrintTemplate = true;
                                            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;

                                            GetTemplateResultEntry();
                                        }
                                        else
                                        {
                                            GetTemplateResultEntry();
                                            TabTemplateDetails.Visibility = Visibility.Visible;
                                            TabControlMain.SelectedIndex = 1;
                                        }
                                        //else
                                        //{
                                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        //        new MessageBoxControl.MessageBoxChildWindow("", "Already One Template Result Value Is Added.It Will Remove The Previous Result Value.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                        //    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                        //    {
                                        //        if (res == MessageBoxResult.No)
                                        //        {
                                        //            TabTemplateDetails.Focus();
                                        //            TabTemplateDetails.IsSelected = true;
                                        //            TabTemplateDetails.Visibility = Visibility.Visible;
                                        //            TabControlMain.SelectedIndex = 1;
                                        //            PrintTemplate = true;
                                        //            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                        //            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                        //            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                        //            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                        //            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        //            TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                        //            GetTemplateResultEntry();
                                        //            foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                                        //            {
                                        //                if (item.ReportTemplate == true)
                                        //                {
                                        //                    if (item.TestID == TestTemplateID)
                                        //                        item.IsSelected = true;
                                        //                    else
                                        //                        item.IsSelected = false;
                                        //                }
                                        //            }
                                        //            dgTest1.UpdateLayout();
                                        //            dgTest1.Focus();
                                        //            ((CheckBox)sender).IsChecked = true;
                                        //        }
                                        //        else
                                        //        {
                                        //            ((CheckBox)sender).IsChecked = false;
                                        //        }
                                        //    };
                                        //    msgW1.Show();
                                        //}
                                    }
                                    #endregion
                                    else
                                    {
                                        if (((MasterListItem)cmbPathologist1.SelectedItem).ID == 0)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Pathologist For Test Template", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW1.Show();
                                            ((CheckBox)sender).IsChecked = false;
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
                                            else if (richTextBox.ViewManager != null)   //added by rohinee
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
                                                        new MessageBoxControl.MessageBoxChildWindow("", "Already One Template Result Value Is Added.It Will Remove The Previous Result Value.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                                    {
                                                        if (res == MessageBoxResult.No)
                                                        {
                                                            richTextBox.Html = HTMLContent;
                                                            TemplateID = 0;
                                                            FillTemplate();
                                                            TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                                            TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                                            TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                                            TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                                            TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
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
                                                            TestTemplateID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                                                        }
                                                        else
                                                        {
                                                            ((CheckBox)sender).IsChecked = false;
                                                        }
                                                    };
                                                    msgW1.Show();
                                                }
                                            }
                                            FillTemplate();
                                        }
                                    }
                                }
                                else
                                {
                                    SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                    BillId1 = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).BillID;
                                    TemplateSampleNo = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleNo;
                                    TemplateSampleCollectionDatetime = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SampleCollectedDateTime;
                                    TemplateOrderID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).OrderBookingID;
                                    TemplatePathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).ID;
                                    TemplateTestID = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
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
                                //    ClickedFlag1 = 0;
                                //    ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                                //}
                            //
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
            e.Row.KeyDown += new KeyEventHandler(Row_KeyDown);
            clsPathoTestParameterVO item = (clsPathoTestParameterVO)e.Row.DataContext;
            string[] NoramlRange2 = null;
            char[] Splitchar = { '-' };
            NoramlRange2 = item.NormalRange.Split(Splitchar);
            List<clsPathoTestParameterVO> HelpValues = new List<clsPathoTestParameterVO>();
            this.dgTestList.Columns[3].IsReadOnly = true;

            if (item.IsNumeric == false)
            {
                this.dgTestList.Columns[3].IsReadOnly = false;
                if (item.ResultValue == string.Empty && item.IsNumeric == false)
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
                item.MachineMannual = "Machine";
                item.IsReadOnly = true;
                this.dgTestList.Columns[2].IsReadOnly = true;

            }
            else
            {
                item.MachineMannual = "Manual";
                item.IsReadOnly = false;
                this.dgTestList.Columns[2].IsReadOnly = true;
            }
            //



            if (item.ResultValue != "")
            {
                if (item.IsNumeric == true && item.IsMachine == false)
                {
                    /* Added by Anumani on 23.02.2016
                     * Displays Deltacheck Results as Pass or Fail
                     * On Comparison with the Default DeltaCheck value
                     */
                    //if (PreviousResult.Count == 0)
                    if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                    {
                        if (item.DeltaCheckValue == 0)
                        {
                            DataGridColumn column = this.dgTestList.Columns[9];
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
                            DataGridColumn column = this.dgTestList.Columns[9];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Content = "Fail";
                            item.DeltaCheck = true;
                        }
                        else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue)
                        {
                            DataGridColumn column = this.dgTestList.Columns[9];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Content = "Pass";
                            item.DeltacheckString = "Pass";
                            item.DeltaCheck = true;
                            //txt.Foreground = 
                        }
                        else
                        {
                        }
                        DeltaResultValue.Clear();
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

                    }

                    if (item.IsReflexTesting == true)
                    {
                        if ((Convert.ToDouble(item.ResultValue) < item.LowReflex) || (Convert.ToDouble(item.ResultValue) > item.HighReflex))
                        {
                            //item.ReflexTesting = "../Icons/Isreflex.png";
                            item.ReflexTestingFlag = true;

                            DataGridColumn column = this.dgTestList.Columns[8];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            // HyperlinkButton txt = thisCell.Content as HyperlinkButton;
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
                            DataGridColumn column = this.dgTestList.Columns[8];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;
                            // HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Visibility = System.Windows.Visibility.Collapsed;
                            txt.Content = " ";
                        }
                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DataGridColumn column = this.dgTestList.Columns[8];
                        FrameworkElement fe = column.GetCellContent(e.Row);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        var thisCell = (DataGridCell)result;
                        // HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                        txt.Visibility = System.Windows.Visibility.Collapsed;
                        txt.Content = " ";
                    }
                    
                    if (item.IsNumeric == false)
                    {

                    }
                    else
                    {
                        item.ReflexTestingFlag = false;
                        DeltaResultValue.Clear();
                    }
                }
                else if (item.IsNumeric == true && item.IsMachine == true)
                {
                    bool IsValid = item.ResultValue.IsValidPositiveNumberWithDecimal(); //by rohini as per mangesh mail for machine ith valid no flag updation
                    if (IsValid)
                    {
                        if (item.DeltaCheckDefaultValue != 0.0 || item.DeltaCheckDefaultValue != 0)
                        {
                            if (item.DeltaCheckValue == 0)
                            {
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;                             
                                item.DeltacheckString = "N.A";
                                item.DeltaCheck = false;
                            }
                            else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue)
                            {
                                DataGridColumn column = this.dgTestList.Columns[9];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Content = "Fail";
                                item.DeltaCheck = true;
                            }
                            else if (item.DeltaCheckValue > item.DeltaCheckDefaultValue)
                            {
                                DataGridColumn column = this.dgTestList.Columns[9];
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
                            DeltaResultValue.Clear();
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

                        if (item.IsReflexTesting == true)
                        {
                            if ((Convert.ToDouble(item.ResultValue) < item.LowReflex) || (Convert.ToDouble(item.ResultValue) > item.HighReflex))
                            {                              
                                item.ReflexTestingFlag = true;
                                DataGridColumn column = this.dgTestList.Columns[8];
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
                                DataGridColumn column = this.dgTestList.Columns[8];
                                FrameworkElement fe = column.GetCellContent(e.Row);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                                var thisCell = (DataGridCell)result;                              
                                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                                txt.Visibility = System.Windows.Visibility.Collapsed;
                                txt.Content = " ";
                            }
                        }
                        else
                        {
                            item.ReflexTestingFlag = false;
                            DataGridColumn column = this.dgTestList.Columns[8];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            var thisCell = (DataGridCell)result;                          
                            HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                            txt.Visibility = System.Windows.Visibility.Collapsed;
                            txt.Content = " ";
                        }
                        if (item.IsNumeric == false)
                        {

                        }
                        else
                        {
                            item.ReflexTestingFlag = false;
                            DeltaResultValue.Clear();
                        }
                    }
                }
                else
                {
                    DataGridColumn column = this.dgTestList.Columns[7];
                    FrameworkElement fe = column.GetCellContent(e.Row);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                    var thisCell = (DataGridCell)result;
                    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                    //txt.Content = "N.A";
                    item.DeltacheckString = "N.A";
                    item.DeltaCheck = false;
                    this.dgTestList.Columns[7].IsReadOnly = true;
                    this.dgTestList.Columns[3].IsReadOnly = false;



                }

            }
        }
        void Row_KeyDown(object sender, KeyEventArgs e)
        {
            dgTestList.CurrentColumn = dgTestList.Columns[2];
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            //ResultListContent.Visibility = Visibility.Collapsed;
            //RefferedBy.Visibility = Visibility.Collapsed;
            //txtReferenceDoctor.Visibility = Visibility.Collapsed;
            //Pathologist.Visibility = Visibility.Collapsed;
            //cmbPathologist1.Visibility = Visibility.Collapsed;
            docHeader2.Visibility = Visibility.Collapsed;
            docHeader225.Visibility = Visibility.Collapsed;
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            //ResultListContent.Visibility = Visibility.Visible;
            //RefferedBy.Visibility = Visibility.Visible;
            //txtReferenceDoctor.Visibility = Visibility.Visible;
            //Pathologist.Visibility = Visibility.Visible;
            //cmbPathologist1.Visibility = Visibility.Visible;
            docHeader2.Visibility = Visibility.Visible;
            docHeader225.Visibility = Visibility.Visible;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgTest1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            foreach (var Agency in AgencyList)
            {
                if (item.TestID == Agency.TestID)
                {
                    if (Agency.AgencyID != 0 && (Agency.AgencyName != null && Agency.AgencyName != ""))
                    {
                        //e.Row.Background = new SolidColorBrush(Colors.Orange);

                        //e.Row.IsEnabled = false;
                    }
                }
            }
            if (item.IsOutSourced)
            {
                e.Row.Background = new SolidColorBrush(Colors.Orange);

                e.Row.IsEnabled = false;

            }
            else
            {
                //if (item.IsResultEntry || item.IsCheckedResults)
                //{
                //    e.Row.Background = new SolidColorBrush(Colors.Green);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel && item.IsCheckedResults == false)
                //{
                //    e.Row.Background = new SolidColorBrush(Colors.Green);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel && item.SecondLevel)
                //{
                //    e.Row.Background = new SolidColorBrush(Colors.Yellow);
                //}
                //if (item.IsResultEntry == true && item.FirstLevel && item.SecondLevel && item.ThirdLevel)
                //{
                //    e.Row.Background = ThirdLevelcolorCode;
                //}
            }
        }

        #endregion

        # region SeTColorCode
        //private void SetColorCode()
        //{

        //    #region Normal Color
        //    string NormalColor = myAppConfig.pathonormalColorCode;
        //    switch (NormalColor)
        //    {
        //        case "Black":
        //            NormalColorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            NormalColorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            NormalColorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            NormalColorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            NormalColorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            NormalColorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            NormalColorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            NormalColorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            NormalColorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            NormalColorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            NormalColorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            NormalColorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            NormalColorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            NormalColorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            NormalColorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtNormalValue.Background = NormalColorCode;
        //    #endregion

        //    #region Min ValueColor
        //    string MinColor = myAppConfig.pathominColorCode;
        //    switch (MinColor)
        //    {
        //        case "Black":
        //            MinColorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            MinColorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            MinColorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            MinColorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            MinColorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            MinColorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            MinColorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            MinColorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            MinColorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            MinColorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            MinColorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            MinColorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            MinColorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            MinColorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            MinColorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }

        //    txtMinValue.Background = MinColorCode;
        //    #endregion

        //    #region MAXColor
        //    string MaxColor = myAppConfig.pathomaxColorCode;
        //    switch (MaxColor)
        //    {
        //        case "Black":
        //            MaxColorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            MaxColorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            MaxColorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            MaxColorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            MaxColorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            MaxColorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            MaxColorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            MaxColorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            MaxColorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            MaxColorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            MaxColorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            MaxColorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            MaxColorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            MaxColorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            MaxColorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtMaxValue.Background = MaxColorCode;
        //    #endregion

        //    #region First Level Color
        //    string FirstLevelColor = myAppConfig.FirstLevelResultColor;
        //    switch (FirstLevelColor)
        //    {
        //        case "Black":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            FirstLevelcolorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtFirstLevel.Background = FirstLevelcolorCode;
        //    #endregion

        //    #region Second Level Color
        //    string SeocndLevelColor = myAppConfig.SecondLevelResultColor;
        //    switch (SeocndLevelColor)
        //    {
        //        case "Black":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            SecondLevelcolorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtSecondLevel.Background = SecondLevelcolorCode;
        //    #endregion

        //    #region Third Level Color
        //    string ThirdLevelColor = myAppConfig.ThirdLevelResultColor;
        //    switch (ThirdLevelColor)
        //    {
        //        case "Black":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            ThirdLevelcolorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtThirdLevel.Background = ThirdLevelcolorCode;
        //    #endregion

        //    #region check Result Color
        //    string CheckResultcolor = myAppConfig.CheckResultColor;
        //    switch (CheckResultcolor)
        //    {
        //        case "Black":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Black);
        //            break;
        //        case "Blue":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Blue);
        //            break;
        //        case "Brown":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Brown);
        //            break;
        //        case "Cyan":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Cyan);
        //            break;
        //        case "DarkGray":
        //            CheckresultColorCode = new SolidColorBrush(Colors.DarkGray);
        //            break;
        //        case "Gray":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Gray);
        //            break;
        //        case "Green":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Green);
        //            break;
        //        case "LightGray":
        //            CheckresultColorCode = new SolidColorBrush(Colors.LightGray);
        //            break;
        //        case "Magenta":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Magenta);
        //            break;
        //        case "Orange":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Orange);
        //            break;
        //        case "Purple":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Purple);
        //            break;
        //        case "Red":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Red);
        //            break;
        //        case "Transparent":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Transparent);
        //            break;
        //        case "White":
        //            CheckresultColorCode = new SolidColorBrush(Colors.White);
        //            break;
        //        case "Yellow":
        //            CheckresultColorCode = new SolidColorBrush(Colors.Yellow);
        //            break;
        //    }
        //    txtCheckedResults.Background = CheckresultColorCode;
        //    #endregion

        //}
        #endregion

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if ((SelectedTestDetails != null && SelectedTestDetails.ToList().Count > 0) || SelectedDetails != null)
            {
                var ChkAlreadyApproved = from r in SelectedTestDetails
                                         where r.SecondLevel == false
                                         select r;

                if (ChkAlreadyApproved.ToList().Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Please First Approve The Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    bool IsPrinted = false;
                    string SampleNo = "";
                    string TestID = "";
                    bool ResultStatus = false;
                    long ResultId;
                    bool IsFinalized = false;

                    try
                    {
                        clsPathoCheckBillingStatusVO BizAction1 = new clsPathoCheckBillingStatusVO();
                        BizAction1.OrderId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        BizAction1.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                        //    BizAction1.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;

                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client1.ProcessCompleted += (s1, arg1) =>
                        {

                            if (arg1.Error == null)
                            {
                                ResultStatus = ((clsPathoCheckBillingStatusVO)arg1.Result).ResultStatus;


                                //if (ResultStatus == true)
                                //{
                                //if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)))
                                //{
                                if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                                {
                                   
                                            if (SelectedDetails.ReportTemplate != false)
                                            {
                                                if (SelectedDetails.SecondLevel == true)
                                                {
                                                    if (SelectedDetails.UnitId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId || SelectedDetails.DispatchToID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                    {
                                                try
                                                {
                                                    clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                                                    BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                                                    BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                                                    BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
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
                                                                    //win_PrintReport.ResultId = ObjDetails.ID;

                                                                    // Commented Temporary
                                                                    win_PrintReport.ObjDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                                                                    win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                                                                    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                                                    win_PrintReport.IsOpdIpd = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External;
                                                                    win_PrintReport.OrderUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                                                                    win_PrintReport.Show();
                                                                    // 

                                                                    //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                                                    //IsFinalized = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;

                                                                    //ResultId = ObjDetails.TemplateDetails.ID;


                                                                    //string URL = "../Reports/Pathology/ResultEntry.aspx?ID=" + ResultId + "&UnitID=" + UnitID+"&IsFinalized="+IsFinalized;
                                                                    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


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
                                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Test Is Not Collected Or Not Dispatched To This Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                        msgW1.Show();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Authorization Not Done For Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                    msgW1.Show();
                                                }
                                            }
                                            else
                                            {
                                                //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry)
                                                //    //GetPatientDetailsInHtml(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID, ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized);
                                                //    PrintReport(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID);
                                                ////if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                                                ////{
                                                ////    ViewUploadedReport();
                                                ////}

                                                /*
                                                 * edited on 26.05.2016
                                                 */


                                                if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
                                                {
                                                    string ResultID = "";
                                                    //if (BalanceAmount == 0.0)
                                                    //{
                                                    if (dgOrdertList.SelectedItem != null)
                                                    {
                                                        if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                                                        {
                                                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                                                            // Added by Anumani on 21/03/2016
                                                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                                                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);

                                                            //
                                                            // Commented in order to get the BillId of Selected row
                                                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                                                            //{
                                                            //    foreach (var item in lstTest)
                                                            //    {
                                                            //        ResultID = ResultID + item.PathPatientReportID;
                                                            //        ResultID = ResultID + ",";
                                                            //    }s
                                                            //    if (ResultID.EndsWith(","))
                                                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                                                            // Added by Anumani on 21/03/2016
                                                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                                                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);


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
                                                        }


                                                        if (dgTest.SelectedItem != null)
                                                        {
                                                            if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                                                            {
                                                                foreach (var item in SelectedTestDetails)
                                                                {
                                                                    if (item.FirstLevel == true && item.SecondLevel == true)  //by rohinee 
                                                                    {
                                                                        SampleNo = SampleNo + item.SampleNo;
                                                                        SampleNo = SampleNo + ",";

                                                                        TestID = TestID + item.TestID;
                                                                        TestID = TestID + ",";

                                                                    }
                                                                }
                                                                if (SampleNo.EndsWith(","))
                                                                    SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                                                                if (TestID.EndsWith(","))
                                                                    TestID = TestID.Remove(TestID.Length - 1, 1);
                                                            }
                                                        }

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


                                                        if (IsPrinted == false)
                                                        {
                                                            IsDuplicate = false;
                                                        }
                                                        else
                                                        {
                                                            IsDuplicate = true;
                                                        }

                                                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                                                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo + "&TestID=" + TestID;
                                                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


                                                    }
                                                }

                                            }
                                       

                                    //   }
                                    // }
                                }
                                else
                                {
                                    //comented by rohinee
                                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Still You Want To Continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    //mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
                                    //mgbx.Show();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Print The Report.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW1.Show();
                                }

                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                        };

                        client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client1.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }


                    //*******************************************************************************************************************************************************************************
                    //if (ResultStatus == true)
                    //{

                    //    if (BalanceAmount == 0.0)
                    //    {
                    //        if (SelectedDetails.ReportTemplate != false)
                    //        {
                    //            try
                    //            {
                    //                clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                    //                BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                    //                BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                    //                BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                    //                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    //                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    //                client.ProcessCompleted += (s, arg) =>
                    //                {
                    //                    if (arg.Error == null)
                    //                    {
                    //                        if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                    //                        {
                    //                            ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;

                    //                            if (ObjDetails.TemplateDetails != null)
                    //                            {

                    //                                TemplateID = ObjDetails.TemplateDetails.TemplateID;

                    //                                PrintPathoReportDelivery win_PrintReport = new PrintPathoReportDelivery();

                    //                                win_PrintReport.ResultId = ObjDetails.TemplateDetails.ID;
                    //                                //win_PrintReport.ResultId = ObjDetails.ID;
                    //                                win_PrintReport.ObjDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                    //                                win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                    //                                win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //                                win_PrintReport.Show();
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //                        msgW1.Show();
                    //                    }
                    //                };

                    //                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    //                client.CloseAsync();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                throw;
                    //            }
                    //        }                                                            // ReportTemplate == true ends
                    //        else
                    //        {
                    //            //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry)
                    //            //    //GetPatientDetailsInHtml(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID, ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized);
                    //            //    PrintReport(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID);
                    //            ////if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                    //            ////{
                    //            ////    ViewUploadedReport();
                    //            ////}

                    //            /*
                    //             * edited on 26.05.2016
                    //             */


                    //            if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
                    //            {
                    //                string ResultID = "";
                    //                if (BalanceAmount == 0.0)
                    //                {
                    //                    if (dgOrdertList.SelectedItem != null)
                    //                    {
                    //                        if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                    //                        {
                    //                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                    //                            // Added by Anumani on 21/03/2016
                    //                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                    //                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);

                    //                            //
                    //                            // Commented in order to get the BillId of Selected row
                    //                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                    //                            //{
                    //                            //    foreach (var item in lstTest)
                    //                            //    {
                    //                            //        ResultID = ResultID + item.PathPatientReportID;
                    //                            //        ResultID = ResultID + ",";
                    //                            //    }s
                    //                            //    if (ResultID.EndsWith(","))
                    //                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                    //                            //}
                    //                        }
                    //                        else
                    //                        {
                    //                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                    //                            // Added by Anumani on 21/03/2016
                    //                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                    //                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);


                    //                            //
                    //                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                    //                            //{
                    //                            //    foreach (var item in SelectedTestDetails)
                    //                            //    {
                    //                            //        ResultID = ResultID + item.BillNo;
                    //                            //        ResultID = ResultID + ",";
                    //                            //    }

                    //                            //    if (ResultID.EndsWith(","))
                    //                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);

                    //                            //}
                    //                        }


                    //                        if (dgTest.SelectedItem != null)
                    //                        {
                    //                            if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                    //                            {
                    //                                foreach (var item in SelectedTestDetails)
                    //                                {
                    //                                    SampleNo = SampleNo + item.SampleNo;
                    //                                    SampleNo = SampleNo + ",";
                    //                                }
                    //                                if (SampleNo.EndsWith(","))
                    //                                    SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                    //                            }
                    //                        }

                    //                        List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                    //                        list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                    //                        var ReferralDoctorSignature = from r in list
                    //                                                      where r.IsChecked = true
                    //                                                      select r;
                    //                        if (ReferralDoctorSignature != null)
                    //                        {
                    //                            ISRferalDoctorSignature = 1;
                    //                        }
                    //                        else
                    //                            ISRferalDoctorSignature = 0;


                    //                        if (IsPrinted == false)
                    //                        {
                    //                            IsDuplicate = false;
                    //                        }
                    //                        else
                    //                        {
                    //                            IsDuplicate = true;
                    //                        }

                    //                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                    //                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo;
                    //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Still You Want To Continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //    mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
                    //    mgbx.Show();
                    //}
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select The Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
            
        private void PrintReport(long ResultId1)
        {
            long empid = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
            bool IsPrinted = false;
            string SampleNo = "";
            string ResultID;
          
            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
            {
                #region commented_on_5.04.2016
                //BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;
                //long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                //List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                //list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                ////var ReferralDoctorSignature = from r in list
                ////                              where r.IsChecked = true
                ////                              select r;
                ////if (ReferralDoctorSignature != null)
                ////{
                ////    ISRferalDoctorSignature = 1;
                ////}
                ////else
                //ISRferalDoctorSignature = 0;
                //string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&empid=" + empid;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                # endregion

                                 BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);
                           
                            //
                            // Commented in order to get the BillId of Selected row
                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            //{
                            //    foreach (var item in lstTest)
                            //    {
                            //        ResultID = ResultID + item.PathPatientReportID;
                            //        ResultID = ResultID + ",";
                            //    }s
                            //    if (ResultID.EndsWith(","))
                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                            //}
                        }
                        else
                        {
                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);
                        

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
                        }


                        if (dgTest.SelectedItem != null)
                        {
                            if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            {
                                foreach (var item in SelectedTestDetails)
                                {
                                    //foreach (var item1 in TestList)
                                    //{
                                    //    if (item1.PathoTestID == item.TestID)
                                    //    {
                                    //        if (item1.IsAbnormal == false)
                                    //        { 
                                                SampleNo = SampleNo + item.SampleNo;
                                                SampleNo = SampleNo + ",";
                                    //        }
                                    //    }
                                    //}
                                }
                                if (SampleNo.EndsWith(","))
                                    SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                            }
                        }

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


                        if (IsPrinted == false)
                        {
                            IsDuplicate = false;
                        }
                        else
                        {
                            IsDuplicate = true;
                        }

                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" +SampleNo;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
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
                        //listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                    }
                };
                client.UploadReportFileAsync(SelectedDetails.SourceURL, SelectedDetails.Report);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }

        }
        double BalanceAmount = 0;
        clsPathOrderBookingDetailVO SelectedDetails { get; set; }
        clsPathPatientReportVO ObjDetails;
        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //commented by rohini for refresh issue
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
            }
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded ==true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;
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
        PasswordWindow Win = new PasswordWindow();
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
                //ClickedFlag = 0;
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Take The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
        void OnOkButton_Click1(object sender, RoutedEventArgs e)
        {
            Win.Close();
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
                                        win_PrintReport.IsOpdIpd = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External;
                                        win_PrintReport.OrderUnitID = win_PrintReport.OrderUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

                                        win_PrintReport.Show();

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

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTest1.ItemsSource != null)
                {
                    var item5 = from r in SelectedTestList.ToList()
                                where r.ReportType.ToUpper() == "Template".ToUpper()
                                select r;
                    if (SelectedTestList.Count == 1 && item5 != null && item5.ToList().Count > 0)
                    {
                        ApproveTest();
                    }
                    else if (SelectedTestList.Count > 1 && TestList != null && TestList.Count > 0)
                    {
                        ApproveTest();
                    }
                    else if (SelectedTestList.Count == 1 && item5 != null && item5.ToList().Count == 0 && TestList != null && TestList.Count > 0)
                    {
                        ApproveTest();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Test To Approve Result.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);                        msgW1.Show();
                        ClickedFlag1 = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        clsPathOrderBookingDetailVO objForPDF;
        private void ApproveTest()
        {
            try
            {
                List<clsPathOrderBookingDetailVO> ApprovalTestList = new List<clsPathOrderBookingDetailVO>();
                List<clsPathOrderBookingDetailVO> ApproveTestList = new List<clsPathOrderBookingDetailVO>();
                clsApprovePathPatientReortBizActionVO BizAction = new clsApprovePathPatientReortBizActionVO();
                
                foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                {
                    if (item.IsSelected == true && item.IsResultEntry == true )
                    {
                        if(item.SecondLevel == false)
                        {
                        ApprovalTestList.Add(item);
                        }
                        else{
                                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Selected Report Has Already Been Approved.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                msgbox.Show();
                        }
                    }
                }
                foreach (clsPathOrderBookingDetailVO item in ApprovalTestList)
                {
                    foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                    {
                        if (item.CategoryID == item1.CategoryID)
                        {
                            ApproveTestList.Add(item);
                        }
                    }
                }


                string OrderDetailsID = "";
               
                if (ApproveTestList != null && ApproveTestList.Count > 0)
                {
                    foreach (var item in ApproveTestList)
                    {
                       
                        
                        
                            OrderDetailsID = OrderDetailsID + item.ID;
                            OrderDetailsID = OrderDetailsID + ",";
                        
                    }

                    if (OrderDetailsID.EndsWith(","))
                        OrderDetailsID = OrderDetailsID.Remove(OrderDetailsID.Length - 1, 1);
                }
                BizAction.OrderDetailsID = OrderDetailsID;
                BizAction.IsThirdLevelApproval = true;
                if (ReferDoctorSignature.IsChecked == true)
                {
                    BizAction.IsDigitalSignatureRequired = true;
                }
                else
                {
                    BizAction.IsDigitalSignatureRequired = false;
                }
                if (TestList != null && TestList.Count > 0)
                    BizAction.TestList = TestList.ToList();
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if((clsApprovePathPatientReortBizActionVO)arg.Result !=null)
                        {
                            if (((clsApprovePathPatientReortBizActionVO)arg.Result).SuccessStatus != 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Test Result Value Approve Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                    {
                                        if (SelectedTestList != null && SelectedTestList.Count > 0)
                                        {

                                            objForPDF = SelectedTestList.Where(z => z.ReportType == "Template").FirstOrDefault();
                                            if (objForPDF != null)
                                            {
                                                GetPatientPathoDetailsInHtml(objForPDF.TemplateResultID, objForPDF.IsFinalized, objForPDF.PatientName);
                                            }
                                        }

                                        SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                                        FillTestList();
                                    }
                                };
                                msgW1.Show();
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
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        int ISRferalDoctorSignature = 0;
        private ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        private void CmdPrintSelected_Click(object sender, RoutedEventArgs e)
        {
            bool IsPrinted = false;
            string SampleNo = "";
            if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
            {
                  double BalanceAmount = Convert.ToDouble(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance);
                string ResultID = "";

                if ((BalanceAmount == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                {
                    if (dgOrdertList.SelectedItem != null)
                    {

                        if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                        {
                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);
                           
                            //
                            // Commented in order to get the BillId of Selected row
                            //if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            //{
                            //    foreach (var item in lstTest)
                            //    {
                            //        ResultID = ResultID + item.PathPatientReportID;
                            //        ResultID = ResultID + ",";
                            //    }s
                            //    if (ResultID.EndsWith(","))
                            //        ResultID = ResultID.Remove(ResultID.Length - 1, 1);
                            //}
                        }
                        else
                        {
                            BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                            // Added by Anumani on 21/03/2016
                            IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                            ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);
                        

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
                        }


                        if (dgTest.SelectedItem != null)
                        {
                            if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                            {
                                foreach (var item in SelectedTestDetails)
                                {
                                    SampleNo = SampleNo + item.SampleNo;
                                    SampleNo = SampleNo + ",";
                                }
                                if (SampleNo.EndsWith(","))
                                    SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                            }
                        }

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


                        if (IsPrinted == false)
                        {
                            IsDuplicate = false;
                        }
                        else
                        {
                            IsDuplicate = true;
                        }

                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" +SampleNo;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


                    }
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Still You Want To Continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
                    //mgbx.Show();
                    //Rohinee
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount You Can Not Print The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Please Select Test For Print.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
                ClickedFlag1 = 0;
            }
        }
        clsPathOrderBookingDetailVO SelectedTest1;
        private void chk_Click(object sender, RoutedEventArgs e)
        {
           // SelectedTestDetails.Clear();

            BalanceAmount = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance;
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);    
            }
            else
            {
               SelectedTestDetails.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                //SelectedTest1=((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                //SelectedTestDetails.Remove(SelectedTest1);
            }
        }


        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest1.SelectedItem != null)
            {
                if (((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).IsCompleted == true)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadReportFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SourceURL });
                            listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SourceURL);
                        }
                    };
                    client.UploadReportFileAsync(((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).SourceURL, ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).Report);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This Test's Report Is Not Uploaded. \n please upload report to view.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Please, Select Test To View Uploaded Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
                ClickedFlag1 = 0;
            }
        }

        private void btnCheckResults_Click(object sender, RoutedEventArgs e)
        {
            bool IsSelected = false;

            foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
            {
                if (item.IsSelected == true)
                {
                    IsSelected = true;
                    break;
                }
            }
            if (IsSelected == true)
            {
                CheckResultMsgWin CheckResMsgWin = null;
                CheckResMsgWin = new CheckResultMsgWin();
                CheckResMsgWin.OnAddButton_Click += new RoutedEventHandler(CheckResMsgWin_OnAddButton_Click);
                CheckResMsgWin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please select Test To Check Result.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                ClickedFlag1 = 0;
            }
        }
        void CheckResMsgWin_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckResultMsgWin)sender).DialogResult == true)
            {
                try
                {
                    List<clsPathOrderBookingDetailVO> CheckTestList = new List<clsPathOrderBookingDetailVO>();
                    List<clsPathOrderBookingDetailVO> ChkTestList = new List<clsPathOrderBookingDetailVO>();
                    clsApprovePathPatientReortBizActionVO BizAction = new clsApprovePathPatientReortBizActionVO();
                    foreach (clsPathOrderBookingDetailVO item in dgTest1.ItemsSource)
                    {
                        if (item.IsSelected == true && item.IsResultEntry == true)
                        {
                            CheckTestList.Add(item);
                        }
                    }
                    foreach (clsPathOrderBookingDetailVO item in CheckTestList)
                    {
                        //foreach (clsUserCategoryLinkVO item1 in UserCategoryLinkedList)
                        //{
                        //    if (item.CategoryID == item1.CategoryID)
                        //    {
                        //        ChkTestList.Add(item);
                        //    }
                        //}
                    }
                    string OrderDetailsID = "";
                    if (ChkTestList != null && ChkTestList.Count > 0)
                    {
                        foreach (var item in ChkTestList)
                        {
                            OrderDetailsID = OrderDetailsID + item.ID;
                            OrderDetailsID = OrderDetailsID + ",";
                        }
                        if (OrderDetailsID.EndsWith(","))
                            OrderDetailsID = OrderDetailsID.Remove(OrderDetailsID.Length - 1, 1);
                    }
                    if (!String.IsNullOrEmpty(((CheckResultMsgWin)sender).checkResultMessage))
                        BizAction.checkResultValueMessage = ((CheckResultMsgWin)sender).checkResultMessage;
                    BizAction.OrderDetailsID = OrderDetailsID;
                    BizAction.IsThirdLevelCheckResult = true;
                    if (ReferDoctorSignature.IsChecked == true)
                    {
                        BizAction.IsDigitalSignatureRequired = true;
                    }
                    else
                    {
                        BizAction.IsDigitalSignatureRequired = false;
                    }
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (((clsApprovePathPatientReortBizActionVO)arg.Result).SuccessStatus != 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Test Results Are Forwarded For First Level.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
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
                catch
                {
                    throw;
                }
            }
        }

        public long TestID = 0;
        private void HyperlinkNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgTestList.ItemsSource != null)
            {
                Notes Nt = null;
                Nt = new Notes();
                HyperlinkButton HY = sender as HyperlinkButton;
                Nt.TestDetails = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0]));
                TestID = ((clsPathoTestParameterVO)(((CollectionViewGroup)HY.DataContext).Items[0])).PathoTestID;
                Nt.IsSecondLevel = false;
                Nt.IsThirdLevel = true;
                Nt.OnAddButton_Click += new RoutedEventHandler(Notes_OnAddButton_Click);
                Nt.Show();
            }
        }
        void Notes_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Notes)sender).DialogResult == true)
            {
                if (TestList != null && TestList.Count > 0)
                {
                    TestList.ToList().Where(x => x.PathoTestID == TestID).ToList().ForEach(z => z.FootNote = ((Notes)sender).FootNote);
                    TestList.ToList().Where(x => x.PathoTestID == TestID).ToList().ForEach(x => x.Note = ((Notes)sender).SuggestionNote);
                    TestList.ToList().ForEach(z => z.OrderID = OrderID);
                }
                if (SelectedTestParameterValues != null && SelectedTestParameterValues.Count > 0)
                {
                    SelectedTestParameterValues.ToList().Where(x => x.PathoTestID == TestID).ToList().ForEach(z => z.FootNote = ((Notes)sender).FootNote);
                    SelectedTestParameterValues.ToList().Where(x => x.PathoTestID == TestID).ToList().ForEach(x => x.Note = ((Notes)sender).SuggestionNote);
                    SelectedTestParameterValues.ToList().ForEach(z => z.OrderID = OrderID);
                }
            }
        }

        private void txtsample_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FillOrderBookingList();
        }

        #region Generate PDF OF Template Based Test

        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }
        clsPathoResultEntryPrintDetailsVO PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
        private void GetPatientPathoDetailsInHtml(long ResultId, bool IsFinalize, string PatientName)
        {
            WaitIndicator Wait = new WaitIndicator();
            Wait.Show();
            try
            {
                clsPathoResultEntryPrintDetailsBizActionVO BizAction = new clsPathoResultEntryPrintDetailsBizActionVO();
                BizAction.ID = Convert.ToInt64(ResultId);
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.IsDelivered = 1;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails != null)
                            {
                                PatientResultEntry = new clsPathoResultEntryPrintDetailsVO();
                                PatientResultEntry = ((clsPathoResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;
                                strPatInfo = new StringBuilder();
                                strPatInfo.Append(PatientResultEntry.PatientInfoHTML);
                                strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
                                strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy"));

                                strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.PatientName.ToString());
                                strPatInfo = strPatInfo.Replace("[Age]", "   :" + PatientResultEntry.AgeYear.ToString() + " Yrs " + PatientResultEntry.AgeMonth.ToString() + " Mth " + PatientResultEntry.AgeDate.ToString() + " Dys");
                                strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());
                                strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + "Dr. " + PatientResultEntry.ReferredBy.ToString());
                                strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.SampleNo.ToString());
                                strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.SampleCollectionTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                                strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy hh:mm tt"));
                                strPatInfo = strPatInfo.Replace("[TemplateTestName]", PatientResultEntry.Test.ToString());
                                if (IsDuplicate == true)
                                {
                                    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "DUPLICATE REPORT");
                                }
                                else
                                {
                                    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
                                }
                                strDoctorPathInfo = new StringBuilder();
                                strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);
                                byte[] imageBytes = null;
                                string imageBase64 = string.Empty;
                                string imageSrc = string.Empty;

                                if (PatientResultEntry.Pathologist1 != null)
                                {
                                    if (PatientResultEntry.Signature1 != null)
                                    {
                                        imageBytes = PatientResultEntry.Signature1;
                                        imageBase64 = Convert.ToBase64String(imageBytes);
                                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
                                        if (imageSrc != null && imageSrc.Length > 0)
                                        {
                                            strDoctorPathInfo.Replace("[%Signature4%]", imageSrc);
                                        }
                                    }

                                    strDoctorPathInfo.Replace("[%Pathalogist4%]", PatientResultEntry.Pathologist1 + " " + '(' + PatientResultEntry.Education1 + ')');
                                    strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Roles);
                                }
                                else
                                {
                                    strDoctorPathInfo.Replace("[%Pathalogist4%]", string.Empty);
                                    strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
                                }
                                //-------------------------------------------------------------------------------------------

                                if (PatientResultEntry.Pathologist2 != null)
                                {
                                    if (PatientResultEntry.Signature2 != null)
                                    {
                                        imageBytes = PatientResultEntry.Signature2;
                                        imageBase64 = Convert.ToBase64String(imageBytes);
                                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid2)  // && ReferDoctorSignature.IsChecked == true)
                                        {
                                            strDoctorPathInfo.Replace("[%Signature3%]", imageSrc);
                                        }
                                    }
                                    strDoctorPathInfo.Replace("[%Pathalogist3%]", PatientResultEntry.Pathologist3 + " " + '(' + PatientResultEntry.Education3 + ')');
                                    strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Roles);
                                }
                                else
                                {
                                    strDoctorPathInfo.Replace("[%Pathalogist3%]", string.Empty);
                                    strDoctorPathInfo.Replace("[%Education3%]", string.Empty);
                                }
                                //-------------------------------------------------------------------------------------------

                                if (PatientResultEntry.Pathologist3 != null)
                                {
                                    if (PatientResultEntry.Signature3 != null)
                                    {
                                        imageBytes = PatientResultEntry.Signature3;
                                        imageBase64 = Convert.ToBase64String(imageBytes);
                                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid3)  // && ReferDoctorSignature.IsChecked == true)
                                        {
                                            strDoctorPathInfo.Replace("[%Signature2%]", imageSrc);
                                        }
                                    }


                                    strDoctorPathInfo.Replace("[%Pathalogist2%]", PatientResultEntry.Pathologist2 + " " + '(' + PatientResultEntry.Education2 + ')');
                                    strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Roles);
                                }
                                else
                                {
                                    strDoctorPathInfo.Replace("[%Pathalogist2%]", string.Empty);
                                    strDoctorPathInfo.Replace("[%Education2%]", string.Empty);
                                }
                                //-------------------------------------------------------------------------------------------

                                if (PatientResultEntry.Pathologist4 != null)
                                {
                                    if (PatientResultEntry.Signature4 != null)
                                    {
                                        imageBytes = PatientResultEntry.Signature4;
                                        imageBase64 = Convert.ToBase64String(imageBytes);
                                        imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                        if (PatientResultEntry.Pathologist == PatientResultEntry.PathoDoctorid4)  // && ReferDoctorSignature.IsChecked == true)
                                        {
                                            strDoctorPathInfo.Replace("[%Signature1%]", imageSrc);
                                        }
                                    }

                                    strDoctorPathInfo.Replace("[%Pathalogist1%]", PatientResultEntry.Pathologist1);
                                    strDoctorPathInfo.Replace("[%Pathalogist1%]", PatientResultEntry.Pathologist1 + " " + '(' + PatientResultEntry.Education1 + ')');
                                    strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Roles);
                                }
                                else
                                {
                                    strDoctorPathInfo.Replace("[%Pathalogist1%]", string.Empty);
                                    strDoctorPathInfo.Replace("[%Education1%]", string.Empty);
                                }

                                richTextBox1.Html = PatientResultEntry.Template;
                                richTextBox1.Html = richTextBox1.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                                richTextBox1.Html = richTextBox1.Html.Replace("[%DOCTORINFO%]", strDoctorPathInfo.ToString());

                                try
                                {
                                    var print = new PrintDocument();
                                    var pdf = new C1PdfDocument(PaperKind.A4);
                                    PrintMargin = new Thickness(0, 152, 0, 0);
                                    PdfFilter.PrintDocument(richTextBox1.Document, pdf, PrintMargin);
                                    String appPath;
                                    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                    appPath = "Afriglobal_" + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientName + "_" + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo + "_" + objForPDF.PathPatientReportID + ".pdf";

                                    //Uri address2 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                    //DataTemplateServiceClient client1 = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address2.AbsoluteUri);
                                    
                                    Uri address2 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                    DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address2.AbsoluteUri);

                                    client1.DeletePathReportFileCompleted += (s1, args1) =>
                                    {
                                        if (args1.Error == null)
                                        {
                                            Stream FileStream = new MemoryStream();
                                            MemoryStream MemStrem = new MemoryStream();
                                            pdf.Save(MemStrem);
                                            FileStream.CopyTo(MemStrem);
                                            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                            //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                            
                                            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                            client.UploadReportFileForPathologyCompleted += (p, args) =>
                                            {
                                                if (args.Error == null)
                                                {
                                                    Stream FileStream1 = new MemoryStream();
                                                    MemoryStream MemStrem1 = new MemoryStream();
                                                    FileStream = FileStream1;
                                                    MemStrem = MemStrem1;
                                                    Uri address3 = new Uri(Application.Current.Host.Source, "../PatientPathTestReportDocuments");
                                                    string fileName1 = address3.ToString();
                                                    fileName1 = fileName1 + "/" + appPath;
                                                    //HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
                                                    richTextBox1.Html = null;
                                                    richTextBox1.Html = "[%PATIENTINFO%]" + richTextBox1.Html + "[%DOCTORINFO%]";
                                                    richTextBox1.Html = richTextBox1.Html.Replace("[%PATIENTINFO%]", "");
                                                    richTextBox1.Html = richTextBox1.Html.Replace("[%DOCTORINFO%]", "");
                                                    richTextBox1.Html = strDoctorPathInfo.ToString();//PreviewPatientResultEntry.FirstLevelDescription;
                                                }
                                            };
                                            client.UploadReportFileForPathologyAsync(appPath, MemStrem.ToArray());
                                            client.CloseAsync();
                                        }
                                    };
                                    client1.DeletePathReportFileAsync(appPath);
                                    client1.CloseAsync();
                                }
                                catch
                                {

                                }
                                finally
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client1.CloseAsync();

            }
            catch
            {
                Wait.Close();
            }
            finally
            {
                Wait.Close();
            }
        }

        #endregion


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


        private void PreviousRecord_Click(object sender, RoutedEventArgs e)
        {
            //if (((clsPathoTestParameterVO)dgTestList.SelectedItem).DeltaCheckValue != 0)
            //{
                PreviousResults win = new PreviousResults();
                win.ParameterId = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
                win.TestId = ((clsPathOrderBookingDetailVO)dgTest1.SelectedItem).TestID;
                win.PatientId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                win.PatientUnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitID;
                win.mainPathologyid = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                win.OrderDate = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderDate;
                win.Show();

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

        //private void ResultEntry_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox text = (TextBox)sender;
        //    string ResultValue = text.Text;
        //    clsPathoTestParameterVO item = (clsPathoTestParameterVO)dgTestList.SelectedItem;
        //    List<clsPathoTestParameterVO> HelpValues = new List<clsPathoTestParameterVO>();

        //    if (item.IsNumeric == true)
        //    {
        //        if (item.PreviousResultValue != null && (ResultValue != "") && item.PreviousResultValue != "")
        //        {
        //            item.DeltaCheckValue = (Convert.ToDouble(item.PreviousResultValue) - Convert.ToDouble(ResultValue)) / Convert.ToDouble(item.PreviousResultValue) * 100;
        //        }
        //    }

        //    if (ResultValue != "")
        //    {
        //        if (item.IsNumeric == true)
        //        {
        //            // Added by Anumani on 23.02.2016
        //            if (item.PreviousResultValue == null || item.PreviousResultValue == "")
        //            {
        //                DataGridColumn column = this.dgTestList.Columns[7];
        //                FrameworkElement fe = column.GetCellContent(item);
        //                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                var thisCell = (DataGridCell)result;
        //                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                txt.Content = "N.A";
        //                item.DeltaCheck = false;

        //            }
        //            else if (item.DeltaCheckValue < item.DeltaCheckDefaultValue && item.DeltaCheckValue != 0.0)
        //            {
        //                DataGridColumn column = this.dgTestList.Columns[7];
        //                FrameworkElement fe = column.GetCellContent(item);
        //                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                var thisCell = (DataGridCell)result;
        //                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                txt.Content = "Fail";
        //                item.DeltaCheck = true;
        //            }
        //            else
        //            {
        //                DataGridColumn column = this.dgTestList.Columns[7];
        //                FrameworkElement fe = column.GetCellContent(item);
        //                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                var thisCell = (DataGridCell)result;
        //                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                txt.Content = "Pass";
        //                item.DeltaCheck = true;
        //            }
        //            DeltaResultValue.Clear();


        //            if (item.IsReflexTesting == true)
        //            {
        //                if ((Convert.ToDouble(ResultValue) < (Convert.ToDouble(item.MinValue))) || (Convert.ToDouble(ResultValue) >= (Convert.ToDouble(item.MaxValue))))
        //                {
        //                    //item.ReflexTesting = "../Icons/Isreflex.png";
        //                    item.ReflexTestingFlag = true;
        //                    DataGridColumn column = this.dgTestList.Columns[6];
        //                    FrameworkElement fe = column.GetCellContent(item);
        //                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                    var thisCell = (DataGridCell)result;

        //                    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                    txt.Visibility = System.Windows.Visibility.Visible;
        //                    Image img = new Image();
        //                    img.Source = new BitmapImage(new Uri("/PalashDynamics;component/Icons/red_flag.png", UriKind.RelativeOrAbsolute));
        //                    img.Height = 20;
        //                    img.Width = 20;
        //                    txt.Content = img;



        //                    //     this.dgTestList.Columns[6].Visibility = System.Windows.Visibility.Visible;

        //                    //txt.Content = "Pass";

        //                }
        //                else
        //                {
        //                    item.ReflexTestingFlag = false;
        //                    DataGridColumn column = this.dgTestList.Columns[6];
        //                    FrameworkElement fe = column.GetCellContent(item);
        //                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                    var thisCell = (DataGridCell)result;

        //                    HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                    txt.Visibility = System.Windows.Visibility.Collapsed;
        //                    txt.Content = "";
        //                }
        //            }
        //            else
        //            {
        //                item.ReflexTestingFlag = false;
        //                DataGridColumn column = this.dgTestList.Columns[6];
        //                FrameworkElement fe = column.GetCellContent(item);
        //                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
        //                var thisCell = (DataGridCell)result;
        //                // HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
        //                txt.Visibility = System.Windows.Visibility.Collapsed;
        //                txt.Content = "";
        //            }

        //            if (Convert.ToDouble(ResultValue) <= item.MinImprobable && item.MinImprobable != 0.0)
        //            {
        //                item.ApColor = new SolidColorBrush(Colors.Brown);
        //            }
        //            else if (Convert.ToDouble(ResultValue) >= item.MaxImprobable && item.MaxImprobable != 0.0)
        //            {
        //                item.ApColor = new SolidColorBrush(Colors.Brown);
        //            }



        //            if ((Convert.ToDouble(ResultValue) > (Convert.ToDouble(item.MaxValue))) && (Convert.ToDouble(ResultValue) < (item.MaxImprobable))) // && (Convert.ToDouble(item5.ResultValue) <= (Convert.ToDouble(NoramlRange1[1]))))
        //            {
        //                item.ApColor = new SolidColorBrush(Colors.Red);
        //                //item5.ApColor = myAppConfig.pathomaxColorCode;
        //            }
        //            else if ((Convert.ToDouble(ResultValue) < (Convert.ToDouble(item.MinValue))) && (Convert.ToDouble(ResultValue) > (item.MinImprobable)))
        //            {
        //                item.ApColor = new SolidColorBrush(Colors.LightGray);
        //                // item5.ApColor = myAppConfig.pathominColorCode;
        //            }
        //            else
        //            {
        //                item.ApColor = new SolidColorBrush(Colors.Purple);
        //                //      item5.ApColor = myAppConfig.pathonormalColorCode;
        //            }


        //        }

        //        if (item.IsNumeric == false)
        //        {

        //            try
        //            {
        //                clsGetHelpValuesFroResultEntryBizActionVO BizAction1 = new clsGetHelpValuesFroResultEntryBizActionVO();
        //                BizAction1.HelpValueList = new List<clsPathoTestParameterVO>();
        //                BizAction1.ParameterID = item.ParameterID;


        //                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


        //                client.ProcessCompleted += (s1, arg1) =>
        //                {
        //                    if (arg1.Error == null)
        //                    {

        //                        if (((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList != null)
        //                        {
        //                            HelpValues = ((clsGetHelpValuesFroResultEntryBizActionVO)arg1.Result).HelpValueList;
        //                            foreach (var item1 in HelpValues)
        //                            {
        //                                if (ResultValue.Equals(item1.HelpValue))
        //                                {
        //                                    if (item1.IsAbnormal == true)
        //                                    {

        //                                        item.ApColor = new SolidColorBrush(Colors.Red);
        //                                    }
        //                                    else
        //                                    {

        //                                    }
        //                                }
        //                            }
        //                            //dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
        //                            //NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList; //Newet added

        //                        }

        //                    }
        //                    else
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                        msgW1.Show();
        //                    }

        //                };

        //                client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                client.CloseAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }


        //        }

        //    }
        //}

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dgTest_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

     
    }
}

