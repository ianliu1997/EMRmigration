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
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewPathologyWorkOrderGeneration : UserControl
    {
        #region List and Variable Declaration
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }
        #endregion

        #region Constructor
        public NewPathologyWorkOrderGeneration()
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " "; 
            InitializeComponent();this.Loaded += new RoutedEventHandler(NewPathologyWorkOrderGeneration_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
        }
        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
                    //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
                   // mElement1.Text = "Clinical Work Order";
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
            lstSampleType.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 1, Description = "Collected", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 2, Description = "Not Collected", Status = true });

            lstUploadStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 1, Description = "Uploaded", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 2, Description = "Not Uploaded", Status = true });

            lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });

            //commented by rohini 10.2.16 for temp
            //cmbSampleType.ItemsSource = lstSampleType;
            //cmbUploadStatus.ItemsSource = lstUploadStatus;
           // cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;

            //commented by rohini 10.2.16 for temp
            //cmbSampleType.SelectedItem = lstSampleType[0];
            //cmbUploadStatus.SelectedItem = lstUploadStatus[0];
           // cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];

           // (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
           // mElement1.Text = "Clinical Work Order";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            FillOrderBookingList();

            //added by rohini dated 10.2.16
            fillSampleStatus();
            dtpFromDate.Focus();
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


        #region added by rohini dated 10/2/16 

        private void fillSampleStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
         
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Billed"));
            MList.Add(new MasterListItem(2, "Collected"));
            MList.Add(new MasterListItem(3, "Dispatched"));
            //MList.Add(new MasterListItem(4, "Received"));
            MList.Add(new MasterListItem(5, "Accepted"));
            MList.Add(new MasterListItem(6, "Rejected"));
            MList.Add(new MasterListItem(7, "Sub Optimal"));          
            MList.Add(new MasterListItem(8, "Outsourced"));
            MList.Add(new MasterListItem(9, "Result Entry Done"));
            MList.Add(new MasterListItem(10, "Report Delivered"));
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
                  //  cmbMachine.ItemsSource = null;
                   // cmbMachine.ItemsSource = objList;
                 //   cmbMachine.SelectedItem = objList[0];
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
            MList.Add(new MasterListItem(2, "Dispatched By"));//Dispatched by
            MList.Add(new MasterListItem(3, "Accepted By"));
            MList.Add(new MasterListItem(4, "Rejected By"));
            MList.Add(new MasterListItem(5, "Report Done By"));
            //MList.Add(new MasterListItem(5, "Recived By"));    
            // cmbSample.itemSource = MList;
            cmbSample.ItemsSource = null;
            cmbSample.ItemsSource = MList;
            cmbSample.SelectedItem = MList[0];
        }
        private void cmbSample_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbSample.SelectedItem).ID == 0)
            {
                txtSample.IsEnabled = false;
                txtSample.Text = "";
            }
            else if (((MasterListItem)cmbSample.SelectedItem).ID != 0)
            {
                txtSample.IsEnabled = true;
            }
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
                //
                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;
                #region COMMENTED BY ROHINI
                //if (cmbSampleType.SelectedItem != null && ((MasterListItem)cmbSampleType.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckSampleType = false;
                //}
                //else
                //{
                //    POBBizAction.CheckSampleType = true;
                //    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
                //        POBBizAction.SampleType = true;
                //    else
                //        POBBizAction.SampleType = false;
                //}
                //if (cmbUploadStatus.SelectedItem != null && ((MasterListItem)cmbUploadStatus.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckUploadStatus = false;
                //}
                //else
                //{
                //    POBBizAction.CheckUploadStatus = true;
                //    if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 1)
                //        POBBizAction.IsUploaded = true;
                //    else
                //        POBBizAction.IsUploaded = false;
                //}
                //if (cmbDeliveryStatus.SelectedItem != null && ((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckDeliveryStatus = false;
                //}
                //else
                //{
                //    POBBizAction.CheckDeliveryStatus = true;
                //    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                //        POBBizAction.IsDelivered = true;
                //    else
                //        POBBizAction.IsDelivered = false;
                //}
                #endregion
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
                            }
                        }
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
            //added by rohini
            BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            //
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.CheckExtraCriteria = true;
            BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            BizAction.SampleType = POBBizAction.SampleType;
            BizAction.IsUploaded = POBBizAction.IsUploaded;
            BizAction.IsDelivered = POBBizAction.IsDelivered;
            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
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

            #region added by rohini new
            //added by rohini dated 11.2.16
            if ((MasterListItem)cmbSampleStatus.SelectedItem != null)
            {
               
                //
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 1)
                {
                    BizAction.OrderDetail.IsBilled = true;
                }
                else
                {
                    BizAction.OrderDetail.IsBilled = false;
                }               
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 2)
                {
                    BizAction.OrderDetail.IsSampleCollected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleCollected = false;
                }
                //
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 3)
                {
                    BizAction.OrderDetail.IsSampleDispatch = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleDispatch = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
                {
                    BizAction.OrderDetail.IsSampleReceive = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleReceive = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 5)
                {
                    BizAction.OrderDetail.IsAccepted = true;
                }
                else
                {
                    BizAction.OrderDetail.IsAccepted = false;
                }               
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 6)
                {
                    BizAction.OrderDetail.IsRejected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsRejected = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 7)
                {
                    BizAction.OrderDetail.IsSubOptimal = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSubOptimal = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 8)
                {
                    BizAction.OrderDetail.IsOutSourced = true;
                }
                else
                {
                    BizAction.OrderDetail.IsOutSourced = false;
                }
                //added by rohini new
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 9)   //result entry done
                {
                    BizAction.OrderDetail.IsResultEntry = true;
                }
                else
                {
                    BizAction.OrderDetail.IsResultEntry = false;
                }
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 10)   //result report done
                {
                    BizAction.OrderDetail.IsDelivered = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDelivered = false;
                }
            }
            #endregion
      #region commented by rohini
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
            //if((MasterListItem)cmbDelivery.SelectedItem!=null)
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
      #endregion
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
            //added by rohini 15.2.16
           if(((MasterListItem)cmbSample.SelectedItem != null))
           {
               if((((MasterListItem)cmbSample.SelectedItem).ID==1 && ((txtSample.Text!=null) ||(txtSample.Text!=""))))
               {
                         BizAction.OrderDetail.SampleCollectedBy=txtSample.Text;
               }
               else if((((MasterListItem)cmbSample.SelectedItem).ID==2 && ((txtSample.Text!=null) ||(txtSample.Text!=""))))
               {
                         BizAction.OrderDetail.DispatchBy=txtSample.Text;
               }
                else if((((MasterListItem)cmbSample.SelectedItem).ID==3 && ((txtSample.Text!=null) ||(txtSample.Text!=""))))
               {
                         BizAction.OrderDetail.AcceptedOrRejectedByName=txtSample.Text;
               }
                else if((((MasterListItem)cmbSample.SelectedItem).ID==4 && ((txtSample.Text!=null) ||(txtSample.Text!=""))))
               {
                         BizAction.OrderDetail.AcceptedOrRejectedByName=txtSample.Text;
               }
                else if((((MasterListItem)cmbSample.SelectedItem).ID==5 && ((txtSample.Text!=null) ||(txtSample.Text!=""))))
               {
                         BizAction.OrderDetail.SampleReceiveBy=txtSample.Text;
               }
           }

           if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
           {
               BizAction.OrderDetail.SampleNo =txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
           }
            if((MasterListItem)cmbProcessedBy.SelectedItem!=null)
            {
                 if (((MasterListItem)cmbProcessedBy.SelectedItem).ID !=0)   // report by hand
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

        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Collapsed;
            docOrderList.Visibility = Visibility.Collapsed;
            grsplSplitter.Visibility = Visibility.Collapsed;
            docTest.HorizontalAlignment = HorizontalAlignment.Stretch;
            
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded == true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            //else
            //    e.Row.Background = null;

            //ADDED BY ROHINI DATED 23.3.16
            if (item.IsResendForNewSample == true)
                e.Row.Background = new SolidColorBrush(Colors.LightGray);
            //else
            //    e.Row.Background = null;

            //comented by rohini for temporary pourpose
            //ADDED BY ROHINI DATED 23.3.16
            if (item.IsSampleCollected == false)
            {
                e.Row.Background = new SolidColorBrush(Colors.Magenta);   //billed
            }
            //else
            //    e.Row.Background = null;

            if (item.IsSampleCollected == true && item.IsSampleDispatch == false)   //collected
            {
                e.Row.Background = new SolidColorBrush(Colors.Yellow);
            }
            //else
            //    e.Row.Background = null;

            if ((item.IsSampleAccepted == true || item.IsRejected == true || item.IsResendForNewSample == true) && (item.SecondLevel == false && item.IsFinalized == false))   //In process 
            {
                e.Row.Background = new SolidColorBrush(Colors.Purple);
            }
            //else
            //    e.Row.Background = null;

            if (item.SecondLevel == true || item.IsCompleted == true)   //Completed  //item.IsFinalized == true &&
            {
                e.Row.Background = new SolidColorBrush(Colors.Green);
            }
            //else
            //    e.Row.Background = null;
            if (item.IsSampleDispatch == true && item.IsSampleReceive == false)   //Dispathed
            {
                e.Row.Background = new SolidColorBrush(Colors.Cyan);
            }

        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Visible;
            docOrderList.Visibility = Visibility.Visible;
            grsplSplitter.Visibility = Visibility.Visible;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void hyTestDetails_Click(object sender, RoutedEventArgs e)
        {
            TestDetailsChildWindow WinHelpValue = new TestDetailsChildWindow();
            WinHelpValue.OrderDetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
            WinHelpValue.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            WinHelpValue.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;
            WinHelpValue.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            WinHelpValue.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;
            
            //WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
            WinHelpValue.Show();
        }

        private void txtFrontFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                FillOrderBookingList();
            }
        }

       
    }
}
