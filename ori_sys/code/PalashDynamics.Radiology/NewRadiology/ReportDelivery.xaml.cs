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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Radiology;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.Pharmacy;
using System.Collections.ObjectModel;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.Radiology.ItemSearch;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.Service.EmailServiceReference;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using C1.Silverlight;
using C1.Silverlight.RichTextBox;
using C1.Silverlight.SpellChecker;
using C1.Silverlight.RichTextBox.Documents;
using System.Windows.Printing;
using System.Text;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

using C1.Silverlight.Pdf;
using C1.Silverlight.PdfViewer;
using C1.Silverlight.RichTextBox.PdfFilter;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.Radiology
{
    public partial class ReportDelivery : UserControl
    {
        public long ResultID;
        public ReportDelivery()
        {
            InitializeComponent();

            DataList = new PagedSortableCollectionView<clsRadOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }

        #region Variable Declaration
        public PagedSortableCollectionView<clsRadOrderBookingVO> DataList { get; private set; }
        long OrderID = 0;
        double Balance = 0;
        bool Freezed;
        bool IsResultEntry;      
        bool IsDelivered;
        bool IsUpload;
        #endregion

        #region Paging



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
            FillOrderBooking();

        }
        #endregion

        private void ReportDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            //FillOrderBooking();
            //FillDeliveryStatus();
          
         
              //  SetCommandButtonState("Load");
                this.DataContext = new clsRadResultEntryVO()
                {
                    StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID
                };
               // ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();
               // FillGender();

                FillDeliveryStatus();
                FillResultEntryStatus();
               // FillTemplateResult();
                FillRadiologist();
                FillClinic();

             //   FillFilm();

                FillModality();
                FillOrderBooking();



               
            
        }

        #region FillData

        /// <summary>
        /// Purpose:For getting order generation information.
        /// Creation Date:05/07/2011 
        /// </summary>

        private void FillOrderBooking()
        {
            clsGetOrderBookingListBizActionVO BizAction = new clsGetOrderBookingListBizActionVO();
            try
            {
                BizAction.BookingList = new List<clsRadOrderBookingVO>();

                //if (dtpFromDate.SelectedDate != null)
                //    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;

                //if (dtpToDate.SelectedDate != null)
                //    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date.Date;

                //if (cmbDeliveryStatus.SelectedItem == null)
                //{
                //    BizAction.CheckDeliveryStatus = false;

                //}
                //else if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
                //{
                //    BizAction.CheckDeliveryStatus = false;

                //}
                //else
                //{
                //    BizAction.CheckDeliveryStatus = true;
                //    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                //        BizAction.DeliveryStatus = true;
                //    else
                //        BizAction.DeliveryStatus = false;
                //}

                if (dtpFromDate.SelectedDate != null)
                {
                    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date;
                }
                if (txtFrontFirstName.Text != "")
                {
                    BizAction.FirstName = txtFrontFirstName.Text;
                }
                if (txtFrontLastName.Text != "")
                {
                    BizAction.LastName = txtFrontLastName.Text;
                }
                if (txtFrontOPDNO.Text != "")
                {
                    BizAction.OPDNO = txtFrontOPDNO.Text;
                }
                if (cmbDeliveryStatus.SelectedItem != null)
                {
                    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
                    {
                        BizAction.CheckDeliveryStatus = null;
                        BizAction.DeliveryStatus = null;
                    }
                    else
                    {

                        if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                            BizAction.DeliveryStatus = true;
                        else
                            BizAction.DeliveryStatus = false;
                    }
                    // BizAction.CheckDeliveryStatus = null;
                }
                if (txtFrontMRNO.Text != "")
                {
                    BizAction.MRNO = txtFrontMRNO.Text;//Added By Yogesh
                }
                if (cmbRadiologist.SelectedItem != null && ((MasterListItem)cmbRadiologist.SelectedItem).ID > 0)
                {
                    BizAction.Radiologist = cmbRadiologist.SelectedItem.ToString();//Added By Yogesh K 

                }


                if ((MasterListItem)cmbModality1.SelectedItem != null)
                {

                    BizAction.CategoryID = ((MasterListItem)cmbModality1.SelectedItem).ID;
                }


               

                if (cmbResultEntryStatus.SelectedItem == null)
                {
                    BizAction.CheckResultEntryStatus = false;

                }
                else if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 0)
                {
                    BizAction.CheckResultEntryStatus = false;
                    // BizAction.NotDone = null;
                }
                else
                {
                    BizAction.CheckResultEntryStatus = true;
                    if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 1)
                        BizAction.ResultEntryStatus = true;
                    else
                        BizAction.ResultEntryStatus = false;
                }


                if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 3)
                {
                    BizAction.IsTechnicianEntry = false;
                    BizAction.NotDone = true;
                }

                if (cmbModality.SelectedItem != null)
                {
                    //BizAction.ModalityID = ((MasterListItem)cmbModality.SelectedItem).ID;
                }

                if (cmbclinic.SelectedItem != null)
                {
                    BizAction.UnitID = ((MasterListItem)cmbclinic.SelectedItem).ID;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetOrderBookingListBizActionVO)arg.Result).BookingList != null)
                        {
                            clsGetOrderBookingListBizActionVO result = arg.Result as clsGetOrderBookingListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.BookingList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.BookingList)
                                {
                                    DataList.Add(item);
                                }

                                dgOrdertList.ItemsSource = null;
                                dgOrdertList.ItemsSource = DataList;
                                dgOrdertList.SelectedIndex = 0;
                                dgOrdertList.UpdateLayout();
                                dgOrdertList.Focus();

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        //private void FillOrderBookingDetails(long iID)
        //{
        //    clsGetOrderBookingDetailsListBizActionVO BizAction = new clsGetOrderBookingDetailsListBizActionVO();
        //    try
        //    {
        //        BizAction.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
        //        BizAction.ID = iID;
        //        BizAction.ReportDelivery = true;

        //        if (cmbDeliveryStatus.SelectedItem == null)
        //        {
        //            BizAction.CheckDeliveryStatus = false;

        //        }
        //        else if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
        //        {
        //            BizAction.CheckDeliveryStatus = false;
        //        }
        //        else
        //        {
        //            BizAction.CheckDeliveryStatus = true;
        //            if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
        //                BizAction.DeliveryStatus = true;
        //            else
        //                BizAction.DeliveryStatus = false;
        //        }

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails != null)
        //                {
        //                    //dgTest.ItemsSource = ((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails;

        //                    List<clsRadOrderBookingDetailsVO> ObjList = new List<clsRadOrderBookingDetailsVO>();

        //                    ObjList = ((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails;

        //                    var results = from r in ObjList
        //                                  where (r.IsResultEntry == true && r.IsFinalized == true) || r.IsUpload == true
        //                                  select r;
        //                    if (results.ToList().Count > 0)
        //                    {
        //                        ObjList = results.ToList();
        //                    }
        //                    dgTest.ItemsSource = null;
        //                    dgTest.ItemsSource = ObjList;
        //                }
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                msgW1.Show();
        //            }
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        private void FillOrderBookingDetails(long iID)
        {
            clsGetOrderBookingDetailsListBizActionVO BizAction = new clsGetOrderBookingDetailsListBizActionVO();
            try
            {
                BizAction.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                BizAction.ID = iID;


                if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 1)
                {
                    BizAction.IsTechnicianEntry = true;
                }

                if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 2)
                {
                    BizAction.IsFinalizedByDr = true;
                }
                if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 3)
                {
                    BizAction.IsTechnicianEntry = false;
                    BizAction.NotDone = true;
                }

                if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                {
                    BizAction.DeliveryStatus = true;
                    //BizAction.NotDone = true;
                }
                else
                {
                    BizAction.DeliveryStatus = false;
                }
                //if (cmbTreatingDr.SelectedItem != null)
                //{
                //    BizAction.RadiologistID = ((MasterListItem)cmbTreatingDr.SelectedItem).ID;
                //}
                if (cmbModality.SelectedItem != null)
                {
                    BizAction.ModalityID = ((MasterListItem)cmbModality.SelectedItem).ID;

                }

                if (cmbRadiologist.SelectedItem != null)
                {
                    BizAction.RadiologistID = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                }
                if (cmbModality1.SelectedItem != null)
                {
                    BizAction.ModalityID = ((MasterListItem)cmbModality1.SelectedItem).ID;

                }

                if (cmbclinic.SelectedItem != null)
                    BizAction.UnitID = ((MasterListItem)cmbclinic.SelectedItem).ID;
                else
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;







                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails != null)
                        {
                            List<clsRadOrderBookingDetailsVO> ObjList = new List<clsRadOrderBookingDetailsVO>();
                            ObjList = ((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails;
                            # region Commented By Yogesh K 24 6 16
                            //var results = from r in ObjList
                            //             where r.IsDelivered == false
                            //             select r;
                            //if (results.ToList().Count > 0)
                            //{
                            //   ObjList = results.ToList();
                            //}
                            #endregion
                            foreach (clsRadOrderBookingDetailsVO item in ObjList)
                            {
                                if (item.IsResultEntry == true)
                                    item.ResultEntryImage = "../Icons/tick.png";
                                else
                                    item.ResultEntryImage = "../Icons/error.png";
                                if (item.IsFinalized == true)
                                    item.TechnicianEntryFinalImage = "../Icons/tick.png";
                                else
                                    item.TechnicianEntryFinalImage = "../Icons/error.png";
                                //if (item.IsTechnicianEntry == true)
                                //    item.TechnicianEntryImage = "../Icons/tick.png";
                                //else
                                //    item.TechnicianEntryImage = "../Icons/error.png";

                                if (item.IsResultEntry == true)
                                    item.TechnicianEntryImage = "../Icons/tick.png";
                                else
                                    item.TechnicianEntryImage = "../Icons/error.png";

                                if (item.Sedation == true)
                                    item.SedationGivenImage = "../Icons/tick.png";
                                else
                                    item.SedationGivenImage = "../Icons/error.png";



                                if (item.IsUpload == true)
                                    item.ReportUploadImage = "../Icons/tick.png";
                                else
                                    item.ReportUploadImage = "../Icons/error.png";


                                if (item.IsDelivered == true)
                                    item.ReportDeliveredImage = "../Icons/tick.png";
                                else
                                    item.ReportDeliveredImage = "../Icons/error.png";

                                if (item.IsCancelled == true)
                                    item.TestCancelImage = "../Icons/tick.png";
                                else
                                    item.TestCancelImage = "../Icons/error.png";

                                //if (item.IsRadiographer == true)

                                //    item.RadioGrapherImage = "../Icons/tick.png";
                                //else
                                //    item.RadioGrapherImage = "../Icons/error.png";

                            }
                            dgTest.ItemsSource = null;
                            dgTest.ItemsSource = ObjList;
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
        //private void FillDeliveryStatus()
        //{
        //    List<MasterListItem> lstDeliveryStatus = new List<MasterListItem>();
        //    lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
        //    lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
        //    lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });
        //    cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;
        //    cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];
        //}

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrdertList.SelectedItem != null)
            {
                long ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
                long resultentry;
                Balance = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).Balance;
                Freezed = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).Freezed;
                resultentry = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ResultEntryCount;
               // IsUpload=((clsRadOrderBookingVO)dgOrdertList.SelectedItem)
               
                OrderID = ID;
               // IsResultEntry =
                if (resultentry > 0)
                {
                    IsResultEntry = true;
                }
                FillOrderBookingDetails(ID);
            }

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBooking();

        }
        #endregion

        #region Deliverd data
        private void CmdDelivered_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsDelivered == false )
                {
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsCancelled == false)
                    {

                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized == true || ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload == true)
                    {

                      

                            if (Balance <= 0 && Freezed == true && IsResultEntry == true || IsUpload == true && Balance <= 0 && Freezed == true)
                            {
                                long TestID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;
                                //PrintReport(OrderID, TestID);
                                //PrintReport(((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID);                                                           
                                UpdateReportDelivery();
                                if ((clsRadOrderBookingVO)dgOrdertList.SelectedItem != null)
                                {
                                    long ID;
                                    ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
                                    FillOrderBookingDetails(ID);
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill is not paid.Please first pay the bill then deliver report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }


                       
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                         new MessageBoxControl.MessageBoxChildWindow("", "Need Doctor Finalization.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW2.Show();
                    }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Test is Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW2.Show();
                    }

                    
                }
              
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Test already delivered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW2.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please select Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW2.Show();
            }
        }

        private void UpdateReportDelivery()
        {
            clsUpdateReportDeliveryBizActionVO BizAction = new clsUpdateReportDeliveryBizActionVO();

            BizAction.Details = (clsRadOrderBookingDetailsVO)dgTest.SelectedItem;
            BizAction.Details.ID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
            BizAction.Details.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.Details.IsDelivered = true;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                   

                    FillOrderBooking();
                  
                    if (((clsUpdateReportDeliveryBizActionVO)arg.Result).Details != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Report Delivered Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                          //  PrintReport();
                        };
                        msgW1.Show();
                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        #endregion

        #region print Report


        //private void PrintReport(long OrderID, long TestID)
        //{
        //    if (OrderID > 0 && TestID > 0)
        //    {
        //        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //        string URL = "../Reports/Radiology/ReportDelivery.aspx?OrderID=" + OrderID + "&TestID=" + TestID + "&UnitID=" + UnitID;
        //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //    }
        //}
        private void PrintReport(long ResultID)
        {
            if (ResultID > 0)
            {
                string msgTitle = "Palash";
                string msgText = "Do you want to print report on letter head?";

                MessageBoxControl.MessageBoxChildWindow msgWw = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);

                msgWw.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        string URL = "../Reports/NewFolder1/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintPage=true";
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else if (res == MessageBoxResult.No)
                    {
                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        string URL = "../Reports/NewFolder1/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                };
                msgWw.Show();
            }
        }
        private void PrintReport()
        {
            if (dgTest.SelectedItem != null)
            {
                if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry == true)
                {
                    PrintRportDelivery win_PrintReport = new PrintRportDelivery();
                    win_PrintReport.IsFromReportDelivery = true;
                    win_PrintReport.ObjDetails = (clsRadOrderBookingDetailsVO)dgTest.SelectedItem;
                    win_PrintReport.ResultId = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    win_PrintReport.ISFinalize = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    win_PrintReport.Show();
                }
                else
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.UploadReportFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).SourceURL });
                        }
                    };
                    client.UploadReportFileAsync(((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).SourceURL, ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).Report);
                }
            }
        }
        private ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry)
                {
                    ResultID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    #region Commented By Bhushan
                    //if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry== true)
                    //{
                    //    PrintRportDelivery win_PrintReport = new PrintRportDelivery();
                    //    win_PrintReport.ResultId = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    //    win_PrintReport.ISFinalize = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                    //    win_PrintReport.TestName = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestName;
                    //    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //    // win_PrintReport.Show();

                    //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //    ResultID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    //    string URL = "../Reports/Radiology/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                    //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    //}

                    //if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload == true)
                    //{
                    //    PrintRportDelivery win_PrintReport = new PrintRportDelivery();
                    //    win_PrintReport.ResultId = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    //    win_PrintReport.ISFinalize = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                    //    win_PrintReport.TestName = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestName;
                    //    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //    // win_PrintReport.Show();

                    //    //long ResultID1;
                    //    //ResultID1 = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderDetailID;
                    //    //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    //    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");






                    //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //    ResultID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                    //    string URL = "../Reports/Radiology/RadiologyUploadedFile.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&Report=" + "~/PatientRadTestReportDocuments/" + ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ReportUploadPath;
                    //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    //}
                    #endregion
                    string sPath = "RadioReport" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "_" + ResultID + ".pdf";
                    ViewPDFReport(sPath);
                }
                else if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadReportFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ReportUploadPath });

                            listOfReports.Add(((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ReportUploadPath);
                        }
                    };
                    client.UploadReportFileAsync(((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ReportUploadPath, ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).Report);
                }
            }

           
        }
        #endregion
        //Added By Bhushanp 25052015
        private void ViewPDFReport(string FileName)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PatientRadTestReportDocuments");
            string fileName1 = address.ToString();
            fileName1 = fileName1 + "/" + FileName;
            // HtmlPage.Window.Invoke("Open", fileName1);
            HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            UIElement MyData = new New_RadiologyWorkOrderGeneration();
            ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
        }

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {
            if (ResultID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/RadiologyUploadedFile.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void FillModality() //Added By Yogesh K
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            // BizAction.MasterTable = MasterTableNameList.M_RadModalityMaster;

            BizAction.MasterTable = MasterTableNameList.M_RadTestCategory;


            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy

            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> listModility = new List<MasterListItem>();
                    listModility.Add(new MasterListItem(0, "--Select--"));
                    listModility.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbModality1.ItemsSource = null;
                    cmbModality1.ItemsSource = listModility;
                    cmbModality1.SelectedItem = listModility[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillResultEntryStatus()
        {
            List<MasterListItem> lstResultEntryStatus = new List<MasterListItem>();
            lstResultEntryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 1, Description = "Technitian Entry", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 2, Description = "Finalized By Dr.", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 3, Description = "Not Done", Status = true });

            cmbResultEntryStatus.ItemsSource = lstResultEntryStatus;
            cmbResultEntryStatus.SelectedItem = lstResultEntryStatus[0];
        }

        private void FillDeliveryStatus()
        {
            List<MasterListItem> lstDeliveryStatus = new List<MasterListItem>();
            lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });
            cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;
            cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];
        }

        //private void FillResultEntry()
        //{
        //    List<MasterListItem> listResultEntry = new List<MasterListItem>();
        //    listResultEntry.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
        //    listResultEntry.Add(new MasterListItem() { ID = 1, Description = "Completed", Status = true });
        //    listResultEntry.Add(new MasterListItem() { ID = 2, Description = "Not Completed", Status = true });
        //    cmbResultEntryStatus.ItemsSource = listResultEntry;
        //    cmbResultEntryStatus.SelectedItem = listResultEntry[0];
        //}

        private void FillRadiologist()
        {

            clsGetRadiologistBizActionVO BizAction = new clsGetRadiologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadiologistBizActionVO)arg.Result).MasterList);

                    //cmbTreatingDr.ItemsSource = null;
                    //cmbTreatingDr.ItemsSource = objList;
                    //cmbTreatingDr.SelectedItem = objList[0];

                    cmbRadiologist.ItemsSource = null;
                    cmbRadiologist.ItemsSource = objList;
                    cmbRadiologist.SelectedItem = objList[0];


                    //cmbTempRadiologist.ItemsSource = null;
                    //cmbTempRadiologist.ItemsSource = objList;


                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillClinic() //Added By Yogesh K
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbclinic.ItemsSource = null;
                        cmbclinic.ItemsSource = objList;
                        cmbclinic.IsEnabled = false;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {

                            var res = from r in objList
                                      where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                      select r;
                            cmbclinic.SelectedItem = ((MasterListItem)res.First());


                        }
                        else
                            cmbclinic.SelectedItem = objList[0];
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
        string textBefore = String.Empty;
        int selectionStart = 0;
        int selectionLength = 0;

        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem != null)
            {
                IsUpload = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload;
            }
           //IsUpload= ((clsRadOrderBookingVO)dgTest.SelectedItem).isu;
        }

        private void txtFrontIPDNO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void txtFrontOPDNO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void txtFrontMRNO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillOrderBooking();

            txtFrontMRNO.Focus();

        }

        private void txtFrontMRNO_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontMRNO.Text = txtFrontMRNO.Text.ToTitleCase();
        }

        private void txtFrontFirstName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillOrderBooking();
            txtFrontFirstName.Focus();
        }

        private void txtFrontFirstName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void txtFrontLastName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillOrderBooking();
            txtFrontLastName.Focus();
        }

        private void txtFrontLastName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFrontFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFrontLastName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmbRadiologist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbRadiologist.SelectedItem != null)
            {
               // cmbTempRadiologist.SelectedValue = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
               // FillTemplate();
            }
        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            clsRadOrderBookingVO item = (clsRadOrderBookingVO)e.Row.DataContext;

            //if (item.TotalCount == item.CompletedTest && item.TotalCount != 0 && item.CompletedTest != 0)
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Green);//completed
            //}

            if (item.TotalCount == item.CompletedTest || item.TotalCount == item.CompletedTest + item.UploadedCount)
            {
                e.Row.Background = new SolidColorBrush(Colors.Green);//completed
            }
            else if (item.ResultEntryCount == 0 && item.CompletedTest == 0 && item.UploadedCount == 0)
            {
                e.Row.Background = new SolidColorBrush(Colors.DarkGray);//New
            }
            else if (item.TotalCount != item.CompletedTest && item.CompletedTest < item.TotalCount || item.UploadedCount < item.TotalCount)//&& item.CompletedTest != 0 && item.ResultEntryCount != 0 && item.CompletedTest != 0
            {
                e.Row.Background = new SolidColorBrush(Colors.Cyan);//Inprocess
            }
            


            if (item.ReferredDoctor == "- Select -")
            {
                item.ReferredDoctor = "Not Assigned";

            }         
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsRadOrderBookingDetailsVO item = (clsRadOrderBookingDetailsVO)e.Row.DataContext;

            if (item.IsCancelled == true)
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);//completed
            }
        }
    }
}
