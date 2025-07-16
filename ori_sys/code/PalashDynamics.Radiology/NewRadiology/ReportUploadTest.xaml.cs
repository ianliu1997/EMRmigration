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
using PalashDynamics.Service.DataTemplateServiceRef;


namespace PalashDynamics.Radiology
{
    public partial class ReportUploadTest : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        public PagedSortableCollectionView<clsRadOrderBookingVO> DataList { get; private set; }
        public ObservableCollection<clsRadItemDetailMasterVO> ItemList { get; set; }
        long TemplateID { get; set; }
        long OrderID { get; set; }
        int ClickedFlag = 0;
        public string Email { get; set; }
        bool IsPageLoded = false;
        long ResultEntryID { get; set; }
        bool IsUpdate = false;
        bool ISFinalize = false;
        public long ResultID;
        StringBuilder strPatInfo, strDoctorPathInfo;
        WaitIndicator Indicatior = null;
        clsUserRightsVO objUser;
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



        #endregion

        public ReportUploadTest()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsRadOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            //txtTechEntryDone.Background = new SolidColorBrush(Color(255, 137, 224, 196));
            //txtResultEntryDone.Background = new SolidColorBrush(Color.FromArgb(255, 125, 203, 30));
            //txtReportDelivered.Background = new SolidColorBrush(Color.FromArgb(255, 247, 211, 88));
            //txtTestCancelled.Background = new SolidColorBrush(Colors.Red);
        }

        private void ResultEntry_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                ScriptableClass myScript = new ScriptableClass();
                HtmlPage.RegisterScriptableObject("SL2JS", myScript);
                SetCommandButtonState("Load");
                this.DataContext = new clsRadResultEntryVO()
                {
                    StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID
                };
                ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();
                FillGender();
                FillClinic();
                FillDeliveryStatus();
                FillResultEntryStatus();
                FillTemplateResult();
                FillRadiologist();
                FillOrderBooking();
               
                FillFilm();
              
                FillModality();
               
             
             
                Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
                if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";
            }
            IsPageLoded = true;
            cmbFilm.Focus();
        }
        #region Fill Datagrid
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBooking();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBooking();
        }
        private void FillOrderBooking()
        {
            clsGetOrderBookingListBizActionVO BizAction = new clsGetOrderBookingListBizActionVO();
            try
            {
                BizAction.BookingList = new List<clsRadOrderBookingVO>();
                if (dtpFromDate.SelectedDate != null)
                    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                if (dtpToDate.SelectedDate != null)
                    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date.Date;
                BizAction.FirstName = txtFrontFirstName.Text;
                BizAction.LastName = txtFrontLastName.Text;
                BizAction.MRNO = txtFrontMRNO.Text;
              //  BizAction.OPDNO = txtFrontOPDNO.Text;
                if (txtFrontOPDNO.Text != "" || txtFrontOPDNO.Text != null)
                {
                    BizAction.OPDNO = txtFrontOPDNO.Text;//Added Ny Yogesh K
                }
                if (cmbModality1.SelectedItem != null && ((MasterListItem)cmbModality1.SelectedItem).ID > 0)//Added By Yogesh K 
                {
                    BizAction.Modality = cmbModality1.SelectedItem.ToString();
                    BizAction.CategoryID = ((MasterListItem)cmbModality1.SelectedItem).ID;
                }
                else if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)//Added By Yogesh K 
                {
                   // BizAction.CheckDeliveryStatus = false;
                }
                else
                {
                    BizAction.CheckDeliveryStatus = true;
                    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                        BizAction.DeliveryStatus = true;
                    else
                        BizAction.DeliveryStatus = false;
                }
                if (cmbResultEntryStatus.SelectedItem == null)
                {
                    BizAction.CheckResultEntryStatus = false;

                }
                else if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 0)
                {
                    BizAction.CheckResultEntryStatus = false;
                }
                else
                {
                    BizAction.CheckResultEntryStatus = true;
                    if (((MasterListItem)cmbResultEntryStatus.SelectedItem).ID == 1)
                        BizAction.ResultEntryStatus = true;
                    else
                        BizAction.ResultEntryStatus = false;
                }

              

                if (cmbModality.SelectedItem != null && ((MasterListItem)cmbModality.SelectedItem).ID > 0)
                {
                    BizAction.Modality = cmbModality.SelectedItem.ToString();//Added By Yogesh K 
                    BizAction.CategoryID = ((MasterListItem)cmbModality.SelectedItem).ID;
                    //((MasterListItem)cmbTreatingDr.SelectedItem).ID )
                }




                if (cmbRadiologist.SelectedItem != null && ((MasterListItem)cmbRadiologist.SelectedItem).ID > 0)//Added By Yogesh K 
                {
                    BizAction.Radiologist = cmbRadiologist.SelectedItem.ToString();
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
       
        private void GetReport()
        {
          //clsGetRadUploadReportBizActionVO BizAction = new clsGetRadUploadReportBizActionVO();
        }

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
                if (cmbTreatingDr.SelectedItem != null)
                {
                    BizAction.RadiologistID = ((MasterListItem)cmbTreatingDr.SelectedItem).ID;
                }
                if (cmbModality.SelectedItem != null)
                {
                    BizAction.ModalityID = ((MasterListItem)cmbModality.SelectedItem).ID;

                }
                ////////////////////////////

                if (cmbRadiologist.SelectedItem != null)
                {
                    BizAction.RadiologistID = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                }
                if (cmbModality1.SelectedItem != null)
                {
                    BizAction.ModalityID = ((MasterListItem)cmbModality1.SelectedItem).ID;

                }
                //////////////////

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
                            //var results = from r in ObjList
                            //              where r.IsDelivered == false
                            //              select r;
                            //if (results.ToList().Count > 0)
                            //{
                            //    ObjList = results.ToList();
                            //}
                       
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

                                if (item.ReportUploadPath != null)
                                { 
                                
                                }

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

        private void FillOrderBookingDetailsAfterSave(long iID)
        {
            clsGetOrderBookingDetailsListBizActionVO BizAction = new clsGetOrderBookingDetailsListBizActionVO();
            try
            {
                BizAction.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                BizAction.ID = iID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && ((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails != null)
                    {
                        List<clsRadOrderBookingDetailsVO> ObjList = new List<clsRadOrderBookingDetailsVO>();
                        ObjList = ((clsGetOrderBookingDetailsListBizActionVO)arg.Result).BookingDetails;
                        var results = from r in ObjList
                                      where r.IsFinalized == false
                                      select r;
                        if (results.ToList().Count > 0)
                        {
                            ObjList = results.ToList();

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

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                long ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder = (clsRadOrderBookingVO)dgOrdertList.SelectedItem;
                if (((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                    FillOrderBookingDetails(ID);
            }




            

            //if (dgOrdertList.SelectedItem != null)
            //{
            //    long ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
            //    if (ID > 0)
            //    {
            //        FillOrderBookingDetails(ID);
            //    }
            //}
            //else
            //{
            //    dgTest.ItemsSource = null;
            //}

           

        }

        private void FillItemList()
        {
            clsGetRadTemplateAndItemByTestIDBizActionVO BizAction = new clsGetRadTemplateAndItemByTestIDBizActionVO();
            BizAction.ItemList = new List<clsRadItemDetailMasterVO>();
            BizAction.TestID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRadTemplateAndItemByTestIDBizActionVO)arg.Result).ItemList != null)
                    {
                        List<clsRadItemDetailMasterVO> ObjItem;
                        ObjItem = ((clsGetRadTemplateAndItemByTestIDBizActionVO)arg.Result).ItemList; ;
                        foreach (var item4 in ObjItem)
                        {
                            ItemList.Add(item4);
                        }
                        dgItemDetailsList.ItemsSource = null;
                        dgItemDetailsList.ItemsSource = ItemList;
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

        private void FillResultEntryStatus()
        {
            //List<MasterListItem> lstResultEntryStatus = new List<MasterListItem>();
            //lstResultEntryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            //lstResultEntryStatus.Add(new MasterListItem() { ID = 1, Description = "Completed", Status = true });
            //lstResultEntryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Completed", Status = true });
            //cmbResultEntryStatus.ItemsSource = lstResultEntryStatus;
            //cmbResultEntryStatus.SelectedItem = lstResultEntryStatus[0];
            List<MasterListItem> lstResultEntryStatus = new List<MasterListItem>();
            lstResultEntryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 1, Description = "Technitian Entry", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 2, Description = "Finalized By Dr.", Status = true });
            lstResultEntryStatus.Add(new MasterListItem() { ID = 3, Description = "Not Done", Status = true });

            cmbResultEntryStatus.ItemsSource = lstResultEntryStatus;
            cmbResultEntryStatus.SelectedItem = lstResultEntryStatus[0];
        }

        //private void FillModality()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_RadModalityMaster;
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "--Select--"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbModality.ItemsSource = null;
        //            cmbModality.ItemsSource = objList;
        //            cmbModality.SelectedItem = objList[0];
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        #endregion

        #region Fillcombobox
        private void FillDeliveryStatus()
        {
            List<MasterListItem> lstDeliveryStatus = new List<MasterListItem>();
            lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });
            cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;
            cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];
        }

        private void FillResultEntry()
        {
            List<MasterListItem> listResultEntry = new List<MasterListItem>();
            listResultEntry.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            listResultEntry.Add(new MasterListItem() { ID = 1, Description = "Completed", Status = true });
            listResultEntry.Add(new MasterListItem() { ID = 2, Description = "Not Completed", Status = true });
            cmbResultEntryStatus.ItemsSource = listResultEntry;
            cmbResultEntryStatus.SelectedItem = listResultEntry[0];
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillFilm()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_RadFilmSize;
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
                    cmbFilm.ItemsSource = null;
                    cmbFilm.ItemsSource = objList;
                    cmbFilm.SelectedItem = objList[0];


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillTemplateResult()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_RadTemplateResult;
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

                    cmbResult.ItemsSource = null;
                    cmbResult.ItemsSource = objList;
                    cmbResult.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbResult.SelectedValue = ((clsRadResultEntryVO)this.DataContext).TemplateResultID;
                    }


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

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


        private void FillRadiologistForResultEntry()
        {
                    
            clsGetRadiologistResultEntryBizActionVO BizAction = new clsGetRadiologistResultEntryBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.TestIDNew =((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadiologistResultEntryBizActionVO)arg.Result).MasterList);

                    cmbTreatingDr.ItemsSource = null;
                    cmbTreatingDr.ItemsSource = objList;
                    cmbTreatingDr.SelectedItem = objList[0];

                    //cmbRadiologist.ItemsSource = null;
                    //cmbRadiologist.ItemsSource = objList;
                    //cmbRadiologist.SelectedItem = objList[0];


                    //cmbTempRadiologist.ItemsSource = null;
                    //cmbTempRadiologist.ItemsSource = objList;


                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillTest()
        {
            clsFillTestComboBoxBizActionVO BizAction = new clsFillTestComboBoxBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsFillTestComboBoxBizActionVO)arg.Result).MasterList);

                    cmbTestName.ItemsSource = null;
                    cmbTestName.ItemsSource = objList;
                    cmbTestName.SelectedValue = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDoctor()
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
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    txtReferenceDoctor.ItemsSource = null;
                    txtReferenceDoctor.ItemsSource = objList;


                    if (txtReferenceDoctor.Text != "")
                    {
                        txtReferenceDoctor.SelectedItem = txtReferenceDoctor.Text;
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }        

        private void FillStore()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
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
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        cmbStore.SelectedValue = ((clsRadResultEntryVO)this.DataContext).StoreID;
                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

        private void cmbRadiologist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbRadiologist.SelectedItem != null)
            {
                cmbTempRadiologist.SelectedValue = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                FillTemplate();
            }
        }


        private void txtReferenceDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            FillDoctor();
        }

        #endregion

        #region FillTemplate combo and get template details

        private void cmbResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRadiologist.SelectedItem != null && cmbGender.SelectedItem != null && cmbResult.SelectedItem != null)
            {
                FillTemplate();
            }



        }

        private void FillTemplate()
        {
            clsFillTemplateComboBoxForResultEntryBizActionVO BizAction = new clsFillTemplateComboBoxForResultEntryBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem != null)
                BizAction.TestID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;

            if (cmbTreatingDr.SelectedItem != null)
                BizAction.Radiologist = ((MasterListItem)cmbTreatingDr.SelectedItem).ID;
            if (cmbGender.SelectedItem != null)
                BizAction.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbResult.SelectedItem != null)
                BizAction.TemplateResultID = ((MasterListItem)cmbResult.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsFillTemplateComboBoxForResultEntryBizActionVO)e.Result).MasterList);
                    cmbTemplate.ItemsSource = null;
                    cmbTemplate.ItemsSource = objList;
                    cmbTemplate.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            //Replace cmbRadiologist as cmbTreatingDr
            if (cmbTemplate.SelectedItem != null && cmbTreatingDr.SelectedItem != null && cmbResult.SelectedItem != null)
            {
                if (((MasterListItem)cmbTemplate.SelectedItem).ID != 0 && ((MasterListItem)cmbTreatingDr.SelectedItem).ID != 0 && ((MasterListItem)cmbResult.SelectedItem).ID != 0)
                {
                    FetchTemplate();
                }
            }

        }

        private void FetchTemplate()
        {
            clsGetRadViewTemplateBizActionVO BizAction = new clsGetRadViewTemplateBizActionVO();
            BizAction.Template = new clsRadiologyVO();
            if (cmbTemplate.SelectedItem != null)
                BizAction.TemplateID = ((MasterListItem)cmbTemplate.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRadViewTemplateBizActionVO)arg.Result).Template != null)
                    {
                        richTextBox.Html = ((clsGetRadViewTemplateBizActionVO)arg.Result).Template.TemplateDesign;
                        TemplateID = ((clsGetRadViewTemplateBizActionVO)arg.Result).Template.TemplateID;

                        string rtbstring = string.Empty;

                        if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                        {
                            rtbstring = richTextBox.Html;
                            rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                            rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
                            richTextBox.Html = rtbstring;
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
        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
            SetCommandButtonState("Cancel");
            Footer1.Visibility = Visibility.Visible;
            objAnimation.Invoke(RotationType.Backward);
            ClearData();
        }

        #region View Details

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsCancelled == true)
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is not Canceled.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                msgbox.Show();
            }
            else
            {
                View();
            }
        }

        void View()
        {
            SetCommandButtonState("Save");

         
            ClearData();
            if (dgTest.SelectedItem != null)
            {
                Footer1.Visibility = Visibility.Collapsed;
                if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID > 0)
                {
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized == true)
                    {
                        chkIsFinalized.IsEnabled = false;
                        chkIsFinalized.IsChecked = true;
                        if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsDigitalSignatureRequired == true)
                        {
                            ChkReferDoctorSignature.IsChecked = true;
                            ChkReferDoctorSignature.IsEnabled = false;
                        }
                        else
                        {
                            ChkReferDoctorSignature.IsChecked = false;
                            ChkReferDoctorSignature.IsEnabled = true;
                        }
                    }
                    else
                    {
                        if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsDigitalSignatureRequired == false)
                        {
                            ChkReferDoctorSignature.IsChecked = false;
                            ChkReferDoctorSignature.IsEnabled = true;
                        }
                        else
                        {
                            ChkReferDoctorSignature.IsChecked = true;
                            ChkReferDoctorSignature.IsEnabled = true;
                        }
                        chkIsFinalized.IsEnabled = true;
                        chkIsFinalized.IsChecked = false;
                    }
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry == false)
                    {
                        FillTest();
                        txtReferenceDoctor.Text = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ReferredDoctor;
                        cmbGender.SelectedValue = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).Gender;
                        cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID;
                        ChkReferDoctorSignature.IsChecked = false;
                        IsUpdate = false;
                        chkIsFinalized.IsChecked = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                        //chkIsFinalized.IsChecked = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).IsFinalized;
                        objAnimation.Invoke(RotationType.Forward);
                    }
                    else
                    {
                        FillTest();
                        txtReferenceDoctor.Text = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ReferredDoctor;
                        cmbGender.SelectedValue = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).Gender;
                        GetResultEntry();
                      
                        chkIsFinalized.IsChecked = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                        //chkIsFinalized.IsChecked = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).IsFinalized;
                        IsUpdate = true;
                        objAnimation.Invoke(RotationType.Forward);
                    }
                }
                txtReferenceDoctor.Text = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ReferredDoctor;
                txtPatientName.Text = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).PatientName;//Added By Yogesh K
                txtPatientName.IsReadOnly = true;
            }
            FillRadiologistForResultEntry();
        }

        #endregion

        #region Get Result Entry details for Update
        private void GetResultEntry()
        {
          //  GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            clsGetRadResultEntryBizActionVO BizAction = new clsGetRadResultEntryBizActionVO();
            BizAction.ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.DetailID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
            BizAction.UnitID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails != null)
                {
                    clsRadResultEntryVO ObjDetails;
                    ObjDetails = ((clsGetRadResultEntryBizActionVO)arg.Result).ResultDetails;
                    if (ObjDetails != null)
                    {
                        this.DataContext = ObjDetails;
                        cmbFilm.SelectedValue = ObjDetails.FilmID;
                        cmbRadiologist.SelectedValue = ObjDetails.RadiologistID1;
                        cmbResult.SelectedValue = ObjDetails.TemplateResultID;
                        cmbTemplate.SelectedValue = ObjDetails.TemplateID;
                        richTextBox.Html = ObjDetails.FirstLevelDescription;
                        ChkReferDoctorSignature.IsChecked = ObjDetails.IsDigitalSignatureRequired;
                        if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                            richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html + "[%DOCTORINFO%]";
                        txtReferenceDoctor.Text = ObjDetails.ReferredBy;
                        txtPatientName.Text = ObjDetails.PatientName;
                    
                    }
                    if (ObjDetails.TestItemList != null)
                    {
                        List<clsRadItemDetailMasterVO> ObjItem;
                        ObjItem = ((clsGetRadResultEntryBizActionVO)arg.Result).TestItemList; ;
                        foreach (var item4 in ObjItem)
                        {
                            cmbStore.SelectedValue = item4.StoreID;
                            ItemList.Add(item4);
                        }
                        dgItemDetailsList.ItemsSource = null;
                        dgItemDetailsList.ItemsSource = ItemList;
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

        #endregion

        #region Save and Update data
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
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
                else
                    ClickedFlag = 0;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsUpdate == false)
                {
                    Save();
                }
                else
                {
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized == true)
                    {
                        PasswordWindow Win = new PasswordWindow();
                        Win.OnOkButton_Click += new RoutedEventHandler(OnOkButton_Click);
                        Win.OnCancelButton_Click += new RoutedEventHandler(OnCancelButton_Click);
                        Win.Show();
                    }
                    else
                    {
                        Update();
                    }
                }
            }
            else
            {
                ClickedFlag = 0;
            }
        }

        void OnOkButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }

        private void Save()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddRadResultEntryBizActionVO BizAction = new clsAddRadResultEntryBizActionVO();
            try
            {
                BizAction.ResultDetails = (clsRadResultEntryVO)this.DataContext;
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromRadiology;
                BizAction.ResultDetails.RadOrderID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderID;
                BizAction.ResultDetails.BookingDetailsID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
                if (ChkReferDoctorSignature.IsChecked == true)
                    BizAction.ResultDetails.IsRefDoctorSigniture = true;
                else
                    BizAction.ResultDetails.IsRefDoctorSigniture = false;
                if (cmbTestName.SelectedItem != null)
                    BizAction.ResultDetails.TestID = ((MasterListItem)cmbTestName.SelectedItem).ID;
                if (cmbFilm.SelectedItem != null)
                    BizAction.ResultDetails.FilmID = ((MasterListItem)cmbFilm.SelectedItem).ID;
                if (cmbRadiologist.SelectedItem != null)
                    BizAction.ResultDetails.RadiologistID1 = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                if (cmbResult.SelectedItem != null)
                    BizAction.ResultDetails.TemplateResultID = ((MasterListItem)cmbResult.SelectedItem).ID;
                if (cmbTemplate.SelectedItem != null)
                    BizAction.ResultDetails.TemplateID = ((MasterListItem)cmbTemplate.SelectedItem).ID;
                if (txtReferenceDoctor.Text != "")
                    BizAction.ResultDetails.ReferredBy = txtReferenceDoctor.Text;

                BizAction.ResultDetails.FirstLevelDescription = richTextBox.Html;//* Text.Html;
                if (((MasterListItem)cmbTemplate.SelectedItem).ID == 0)
                {
                    string rtbstring1 = string.Empty;
                    if (!(BizAction.ResultDetails.FirstLevelDescription.Contains("[%PATIENTINFO%]")))
                    {
                        rtbstring1 = BizAction.ResultDetails.FirstLevelDescription;
                        rtbstring1 = rtbstring1.Insert(rtbstring1.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                        rtbstring1 = rtbstring1.Insert(rtbstring1.IndexOf("</body>"), "[%DOCTORINFO%]");
                        BizAction.ResultDetails.FirstLevelDescription = rtbstring1;
                    }
                }

                BizAction.ResultDetails.TestItemList = ItemList.ToList();
                double Balance = 0;
                if (dgOrdertList.SelectedItem != null)
                {
                    Balance = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).Balance;
                }
                BizAction.ResultDetails.IsResultEntry = true;
                if (chkIsFinalized.IsChecked == true)
                {
                    BizAction.ResultDetails.IsFinalized = true;
                }
                else
                {
                    BizAction.ResultDetails.IsFinalized = false;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Load");
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {
                        FillOrderBooking();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Result Entry Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                if (Balance == 0.0)
                                {
                                    Indicatior.Close();
                                    ISFinalize = ((clsAddRadResultEntryBizActionVO)arg.Result).ResultDetails.IsFinalized;
                                    PrintRportDelivery win_PrintReport = new PrintRportDelivery();
                                    win_PrintReport.ResultId = ((clsAddRadResultEntryBizActionVO)arg.Result).ResultDetails.ID;
                                    win_PrintReport.ISFinalize = ISFinalize;
                                    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                    win_PrintReport.Show();

                                    //if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID > 0)
                                    //{
                                    //    PrintRportDelivery win_PrintReport = new PrintRportDelivery();
                                    //    win_PrintReport.ResultId = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ResultEntryID;
                                    //    win_PrintReport.ISFinalize = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                                    //    win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                    //    win_PrintReport.Show();
                                    //}
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please first pay the bill then Take the Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                            }
                        };
                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Result Entry .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        public void GetUserRights(long UserId) //Added By Yogesh k
        {
            try
            {

                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();

                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        //if (objUser.IsCrossAppointment)
                        //{
                        //    cmbAUnit.IsEnabled = true;
                        //}
                        //else
                        //{
                        //    cmbAUnit.IsEnabled = false;
                        //}


                        if (objUser.Isfinalized == true)
                        {
                            //UserGRNCountForMonth
                         //   rdbDirectPur.Visibility = Visibility.Visible;
                           // Int64 MaxCnt = objUser.FrequencyPerMonth;
                           // decimal MaxPuAmt = objUser.MaxPurchaseAmtPerTrans;
                            chkIsFinalized.IsEnabled = true;
                        }
                        else
                        {
                           // rdbDirectPur.Visibility = Visibility.Collapsed;
                           // rdbPo.IsChecked = true;
                            chkIsFinalized.IsEnabled = false;
                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Update()
        {
            clsAddRadResultEntryBizActionVO BizAction = new clsAddRadResultEntryBizActionVO();
            try
            {
                BizAction.ResultDetails = (clsRadResultEntryVO)this.DataContext;
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromRadiology;
                BizAction.ResultDetails.RadOrderID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderID;
                BizAction.ResultDetails.BookingDetailsID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
                BizAction.ResultDetails.RadTechnicianEntryID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadTechnicianEntryID;
                if (ChkReferDoctorSignature.IsChecked == true)
                    BizAction.ResultDetails.IsRefDoctorSigniture = true;
                else
                    BizAction.ResultDetails.IsRefDoctorSigniture = false;
                if (cmbTestName.SelectedItem != null)
                    BizAction.ResultDetails.TestID = ((MasterListItem)cmbTestName.SelectedItem).ID;
                if (cmbFilm.SelectedItem != null)
                    BizAction.ResultDetails.FilmID = ((MasterListItem)cmbFilm.SelectedItem).ID;
                if (cmbRadiologist.SelectedItem != null)
                    BizAction.ResultDetails.RadiologistID1 = ((MasterListItem)cmbRadiologist.SelectedItem).ID;
                if (cmbResult.SelectedItem != null)
                    BizAction.ResultDetails.TemplateResultID = ((MasterListItem)cmbResult.SelectedItem).ID;
                if (cmbTemplate.SelectedItem != null)
                    BizAction.ResultDetails.TemplateID = ((MasterListItem)cmbTemplate.SelectedItem).ID;
                if (txtReferenceDoctor.Text != "")
                    BizAction.ResultDetails.ReferredBy = txtReferenceDoctor.Text;
                BizAction.ResultDetails.FirstLevelDescription = richTextBox.Html; //*Text.Html;
                BizAction.ResultDetails.TestTemplateRTF = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                BizAction.ResultDetails.TestItemList = ItemList.ToList();
                BizAction.ResultDetails.Time = null;
                BizAction.ResultDetails.IsResultEntry = true;
                if (chkIsFinalized.IsChecked == true)
                {
                    BizAction.ResultDetails.IsFinalized = true;
                }
                else
                {
                    BizAction.ResultDetails.IsFinalized = false;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient clientRad = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Load");
                    ClickedFlag = 0;
                    if (arg.Error == null)
                        //GetPatientDetailsInHtml(((clsAddRadResultEntryBizActionVO)arg.Result).ResultDetails.ID, BizAction.ResultDetails.IsFinalized);Commented By Yogesh K
                        GetPatientDetailsInHtml(BizAction.ResultDetails.ID, BizAction.ResultDetails.IsFinalized);
                    if (arg.Error == null)
                    {
                        FillOrderBooking();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Result Entry Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                //ISFinalize = ((clsAddRadResultEntryBizActionVO)arg.Result).ResultDetails.IsFinalized;
                                //GetPatientDetailsInHtml(((clsAddRadResultEntryBizActionVO)arg.Result).ResultDetails.ID, ISFinalize);
                                ISFinalize = BizAction.ResultDetails.IsFinalized;
                                GetPatientDetailsInHtml(BizAction.ResultDetails.ID, BizAction.ResultDetails.IsFinalized);
                            }
                        };
                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Result Entry .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
            finally
            {

            }
        }

        #endregion

        #region Item details
        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                {
                    cmbStore.ClearValidationError();
                    ItemListNew ItemsWin = new ItemListNew();
                    ItemsWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                    ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    ItemsWin.ShowBatches = true;
                    ItemsWin.cmbStore.IsEnabled = false;
                    ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    ItemsWin.Show();
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null)
                {
                    foreach (var item in Itemswin.ItemBatchList)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsRadItemDetailMasterVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsRadItemDetailMasterVO objItem = new clsRadItemDetailMasterVO();
                            objItem.ItemID = item.ItemID;
                            objItem.ItemCode = item.ItemCode;
                            objItem.ItemName = item.ItemName;
                            objItem.BatchID = item.BatchID;
                            objItem.BatchCode = item.BatchCode;
                            objItem.ExpiryDate = item.ExpiryDate;
                            objItem.BalanceQuantity = item.AvailableStock;
                            objItem.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            ItemList.Add(objItem);
                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID > 0 && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
            {
                cmbStore.ClearValidationError();
                AssignBatch BatchWin = new AssignBatch();
                BatchWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                BatchWin.SelectedItemID = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID;
                BatchWin.ItemName = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemName;
                BatchWin.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButton_Click);
                BatchWin.Show();
            }
            else
            {
                if (cmbStore.SelectedItem == null)
                {
                    cmbStore.TextBox.SetValidation("Please select the store");
                    cmbStore.TextBox.RaiseValidationError();
                }
                else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                {
                    cmbStore.TextBox.SetValidation("Please select the store");
                    cmbStore.TextBox.RaiseValidationError();
                }
            }
        }

        void OnAddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            AssignBatch AssignBatchWin = (AssignBatch)sender;

            if (AssignBatchWin.DialogResult == true)
            {
                if (AssignBatchWin.SelectedBatches != null)
                {
                    foreach (var item in AssignBatchWin.SelectedBatches)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsRadItemDetailMasterVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            foreach (var BatchItems in ItemList.Where(x => x.ItemID == item.ItemID))
                            {
                                BatchItems.ItemID = item.ItemID;
                                BatchItems.ItemCode = item.ItemCode;
                                BatchItems.BatchID = item.BatchID;
                                BatchItems.BatchCode = item.BatchCode;
                                BatchItems.ExpiryDate = item.ExpiryDate;
                                BatchItems.BalanceQuantity = item.AvailableStock;
                                BatchItems.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            }
                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemDetailsList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);
                        dgItemDetailsList.Focus();
                        dgItemDetailsList.UpdateLayout();
                        dgItemDetailsList.SelectedIndex = ItemList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;

                    break;
                case "New":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;

                    break;

                case "Save":


                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                case "Cancel":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;

                    break;

                default:
                    break;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    cmdSave.IsEnabled = false;
            }
        }

        #endregion

        #region Clear UI
        private void ClearData()
        {
            this.DataContext = new clsRadResultEntryVO();
            cmbTestName.SelectedValue = (long)0;
            cmbFilm.SelectedValue = (long)0;
            cmbRadiologist.SelectedValue = (long)0;
            cmbTempRadiologist.SelectedValue = (long)0;
            cmbGender.SelectedValue = (long)0;
            cmbResult.SelectedValue = (long)0;
            cmbTemplate.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
           // txtReferenceDoctor.Text = string.Empty;  Commented By Yogesh K
            txtNoOfFilms.Text = string.Empty;
            richTextBox.Html = string.Empty;
            ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();
            IsUpdate = false;
        }
        #endregion

        #region validation
        private bool CheckValidation()
        {
            bool result = true;
            if (cmbTestName.SelectedItem == null)
            {
                cmbTestName.TextBox.SetValidation("Please select the Test Name");
                cmbTestName.TextBox.RaiseValidationError();
                cmbTestName.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbTestName.SelectedItem).ID == 0)
            {
                cmbTestName.TextBox.SetValidation("Please select the Test Name");
                cmbTestName.TextBox.RaiseValidationError();
                cmbTestName.Focus();
                result = false;

            }
            else
                cmbTestName.TextBox.ClearValidationError();

            if (cmbTreatingDr.SelectedItem == null)
            {
                cmbTreatingDr.TextBox.SetValidation("Please select the Radiologist");
                cmbTreatingDr.TextBox.RaiseValidationError();
                cmbTreatingDr.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbTreatingDr.SelectedItem).ID == 0)
            {
                cmbTreatingDr.TextBox.SetValidation("Please select the Radiologist");
                cmbTreatingDr.TextBox.RaiseValidationError();
                cmbTreatingDr.Focus();
                result = false;
            }
            else
                cmbTreatingDr.TextBox.ClearValidationError();
            if (txtReferenceDoctor.Text == "")
            {
                txtReferenceDoctor.SetValidation("Please enter Referred By");
                txtReferenceDoctor.RaiseValidationError();
                txtReferenceDoctor.Focus();
                result = false;
            }
            else
                txtReferenceDoctor.ClearValidationError();
            return result;
        }
        string textBefore = String.Empty;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        #endregion

        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }

        #region Print Report
        private void PrintReport(long ResultID, bool IsFinalize, string PatientInfoString, string DoctorInfoString)
        {

            richTextBox.Document.Margin = new Thickness(0, 92, 0, 0);

            //GetPatientDetailsInHtml(ResultID);

            //----------------------------------------------------------------------------------------------------------
            //----------------------------------------------------------------------------------------------------------

            richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", PatientInfoString);
            richTextBox.Html = richTextBox.Html.Replace("[%DOCTORINFO%]", DoctorInfoString);
            var viewManager = new C1RichTextViewManager
            {
                Document = richTextBox.Document,
                PresenterInfo = richTextBox.ViewManager.PresenterInfo
            };
            PrintDocument print = new PrintDocument();
            int presenter = 0;
            print.PrintPage += (s, printArgs) =>
            {
                //var element = (FrameworkElement)printTemplate.LoadContent();
                ////element.Margin = new Thickness(0, 100, 0, 0);
                //element.DataContext = viewManager.Presenters[presenter];
                //printArgs.PageVisual = element;
                //printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                var element = (FrameworkElement)HeaderTemplate.LoadContent();
                Grid grd = (Grid)element.FindName("PatientDetails");

                if (grd != null)
                {
                    grd.Visibility = Visibility.Visible;
                    grd.DataContext = PatientResultEntry;
                }
                element.DataContext = viewManager.Presenters[presenter];
                printArgs.PageVisual = element;
                printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;


            };

            //print.BeginPrint += new EventHandler<BeginPrintEventArgs>(print_BeginPrint);
            //print.PrintPage += new EventHandler<PrintPageEventArgs>(print_PrintPage);
            print.Print("A Christmas Carol");


            var pdf = new C1PdfDocument(PaperKind);

            PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);

            String appPath;

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            appPath = UnitID + "_" + ResultID + ".pdf";

            Stream FileStream = new MemoryStream();

            MemoryStream MemStrem = new MemoryStream();

            //pdf.Save(appPath);
            pdf.Save(MemStrem);


            FileStream.CopyTo(MemStrem);

            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

            client.UploadReportFileForRadiologyCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    //HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL });
                    //listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                }
            };
            client.UploadReportFileForRadiologyAsync(appPath, MemStrem.ToArray());
            client.CloseAsync();


            // Printing

            //C1.Silverlight.Pdf.C1PdfDocument pdf = new C1.Silverlight.Pdf.C1PdfDocument();
            //C1.Silverlight.PdfViewer.C1PdfViewer pdfv = new C1.Silverlight.PdfViewer.C1PdfViewer();


            //if (ResultID > 0 && IsFinalize == true)
            //{
            //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    string URL = "../Reports/Radiology/ResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
            //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //}
            //else
            //{
            //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    string URL = "../Reports/Radiology/ResultEntryRoughReport.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
            //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //}

            //  print.Print("A Christmas Carol");

        }

        void print_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Thickness margin = new Thickness
            //{
            //    Left = Math.Max(0, 96 - e.PageMargins.Left),
            //    Top = Math.Max(0, 96 - e.PageMargins.Top),
            //    Right = Math.Max(0, 96 - e.PageMargins.Right),
            //    Bottom = Math.Max(0, 96 - e.PageMargins.Bottom)
            //};
            //Ellipse ellipse = new Ellipse
            //{
            //    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 192, 192)),
            //    Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 192, 255)),
            //    StrokeThickness = 24,   // 1/4 inch 
            //    Margin = margin
            //};
            //Border border = new Border();
            //border.Child = ellipse;
            //e.PageVisual = border; 

        }

        void print_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            //PrintDocument pd = ((PrintDocument)sender);
            //pd.
        }

        clsRadResultEntryPrintDetailsVO PatientResultEntry = new clsRadResultEntryPrintDetailsVO();
        private void GetPatientDetailsInHtml(long ResultId, bool IsFinalize)
        {
            clsRadResultEntryPrintDetailsBizActionVO BizAction = new clsRadResultEntryPrintDetailsBizActionVO();

            BizAction.ResultID = ResultId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        // clsRadResultEntryPrintDetailsVO PatientResultEntry = new clsRadResultEntryPrintDetailsVO();
                        PatientResultEntry = new clsRadResultEntryPrintDetailsVO();
                        PatientResultEntry = ((clsRadResultEntryPrintDetailsBizActionVO)arg.Result).ResultDetails;

                        strPatInfo = new StringBuilder();



                        //strPatInfo.Append(PatientResultEntry.PatientInfoHTML);

                        //strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientResultEntry.MRNo.ToString());
                        //strPatInfo = strPatInfo.Replace("[OrderDate]", "    :" + PatientResultEntry.OrderDate.Value.ToString("dd MMM yyyy"));
                        ////strPatInfo = strPatInfo.Replace("[Salutation]", "    :" + PatientResultEntry.Salutation.ToString());
                        //strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientResultEntry.PatientName.ToString());
                        //strPatInfo = strPatInfo.Replace("[AgeYear]", "    :" + PatientResultEntry.AgeYear.ToString());
                        //strPatInfo = strPatInfo.Replace("[AgeMonth]", "    :" + PatientResultEntry.AgeMonth.ToString());
                        //strPatInfo = strPatInfo.Replace("[AgeDate]", "    :" + PatientResultEntry.AgeDate.ToString());
                        //strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientResultEntry.Gender.ToString());
                        //if (PatientResultEntry.ShowinRadReport == false)
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :" + PatientResultEntry.ReferredDoctor.ToString());
                        //}
                        //else
                        //{
                        //    strPatInfo = strPatInfo.Replace("[RefDoc]", "    :");
                        //}
                        //strPatInfo = strPatInfo.Replace("[BillNo]", "    :" + PatientResultEntry.BillNo.ToString());
                        //strPatInfo = strPatInfo.Replace("[RPTDATE]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToString("dd MMM yyyy"));
                        //strPatInfo = strPatInfo.Replace("[RPTTIME]", "    :" + PatientResultEntry.ResultAddedDateTime.Value.ToShortTimeString());
                        //strPatInfo = strPatInfo.Replace("[TemplateTestName]", PatientResultEntry.PrintTestName.ToString());

                        ////ToString("dd MMM yyyy hh:mm")

                        //if (IsFinalize == false)
                        //{
                        //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "NOT FINALIZED / ROUGH REPORT");
                        //}
                        //else
                        //{
                        //    strPatInfo = strPatInfo.Replace("[NOTFINALIZED]", "");
                        //}

                        strDoctorPathInfo = new StringBuilder();
                        strDoctorPathInfo.Append(PatientResultEntry.DoctorInfoHTML);

                        byte[] imageBytes = null;
                        string imageBase64 = string.Empty;
                        string imageSrc = string.Empty;

                        if (PatientResultEntry.Radiologist1 != null)
                        {
                            if (PatientResultEntry.Signature1 != null)
                            {
                                imageBytes = PatientResultEntry.Signature1;

                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid1)
                                {
                                    strDoctorPathInfo.Replace("[%Signature4%]", imageSrc);
                                }
                            }

                            strDoctorPathInfo.Replace("[%Radiologist4%]", PatientResultEntry.Radiologist1);
                            strDoctorPathInfo.Replace("[%Education4%]", PatientResultEntry.Education1);
                        }
                        else
                        {
                            strDoctorPathInfo.Replace("[%Radiologist4%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Education4%]", string.Empty);
                        }
                        //-------------------------------------------------------------------------------------------

                        if (PatientResultEntry.Radiologist2 != null)
                        {
                            if (PatientResultEntry.Signature2 != null)
                            {
                                imageBytes = PatientResultEntry.Signature2;
                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid2)
                                {
                                    strDoctorPathInfo.Replace("[%Signature3%]", imageSrc);
                                }
                            }

                            strDoctorPathInfo.Replace("[%Radiologist3%]", PatientResultEntry.Radiologist2);
                            strDoctorPathInfo.Replace("[%Education3%]", PatientResultEntry.Education2);
                        }
                        else
                        {
                            strDoctorPathInfo.Replace("[%Radiologist3%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Education3%]", string.Empty);
                        }
                        //-------------------------------------------------------------------------------------------

                        if (PatientResultEntry.Radiologist3 != null)
                        {
                            if (PatientResultEntry.Signature3 != null)
                            {
                                imageBytes = PatientResultEntry.Signature3;
                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid3)
                                {
                                    strDoctorPathInfo.Replace("[%Signature2%]", imageSrc);
                                }
                            }

                            strDoctorPathInfo.Replace("[%Radiologist2%]", PatientResultEntry.Radiologist3);
                            strDoctorPathInfo.Replace("[%Education2%]", PatientResultEntry.Education3);
                        }
                        else
                        {
                            strDoctorPathInfo.Replace("[%Radiologist2%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Education2%]", string.Empty);
                        }
                        //-------------------------------------------------------------------------------------------

                        if (PatientResultEntry.Radiologist4 != null)
                        {
                            if (PatientResultEntry.Signature4 != null)
                            {
                                imageBytes = PatientResultEntry.Signature4;
                                imageBase64 = Convert.ToBase64String(imageBytes);
                                imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);

                                if (PatientResultEntry.Radiologist == PatientResultEntry.RadioDoctorid4)
                                {
                                    strDoctorPathInfo.Replace("[%Signature1%]", imageSrc);
                                }
                            }

                            strDoctorPathInfo.Replace("[%Radiologist1%]", PatientResultEntry.Radiologist4);
                            strDoctorPathInfo.Replace("[%Education1%]", PatientResultEntry.Education4);
                        }
                        else
                        {
                            strDoctorPathInfo.Replace("[%Radiologist1%]", string.Empty);
                            strDoctorPathInfo.Replace("[%Education1%]", string.Empty);
                        }

                        ////PrintReport(ResultId, ISFinalize, strPatInfo.ToString());
                        //   PrintReport(ResultId, IsFinalize, Convert.ToString(strPatInfo), Convert.ToString(strDoctorPathInfo));

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

        //private void ReadPatientInfoC1()
        //{
        //    strPatInfo = new StringBuilder();

        //    String returnValue = "";

        //    WebClient _Client = new WebClient();


        //    _Client.DownloadStringAsync(new Uri(@"../PatientInfoForC1.htm", UriKind.RelativeOrAbsolute));
        //    _Client.DownloadStringCompleted += (s, e) =>
        //        {


        //            //const string f = "../PatientInfoForC1.htm";

        //            //// 1
        //            //// Declare new List.
        //            //List<string> lines = new List<string>();

        //            //// 2
        //            //// Use using StreamReader for disposing.
        //            ////using (StreamReader r = new StreamReader(f))
        //            ////{
        //            ////    // 3
        //            ////    // Use while != null pattern for loop
        //            ////    string line;
        //            ////    while ((line = r.ReadLine()) != null)
        //            ////    {
        //            ////        // 4
        //            ////        // Insert logic here.
        //            ////        // ...
        //            ////        // "line" is a line in the file. Add it to our List.
        //            ////        //lines.Add(line);

        //            ////    }
        //            ////}

        //            //lines = System.IO.File.ReadLines(f).ToList();


        //            //// 5
        //            //// Print out all the lines.
        //            //foreach (string s1 in lines)
        //            //{
        //            //    //Console.WriteLine(s);
        //            //    returnValue = returnValue + s1;
        //            //}

        //            //strPatInfo.Append(returnValue);

        //        };






        //}

        #endregion

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }
        private ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ReportUploadPath))
                {
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).Report != null)
                    {
                        if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsCompleted == true)
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
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            mgbx.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null)
                    return;
                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextBox.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextBox.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextBox.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextBox.Html;
                }
            }
            catch { }
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsRadOrderBookingDetailsVO item = (clsRadOrderBookingDetailsVO)e.Row.DataContext;
            if (item.IsCancelled == true)
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);//completed
            }
        }

        #region Compare Study

        public List<clsRadOrderBookingVO> SelectedOrderList = new List<clsRadOrderBookingVO>();
        private void chkOrder_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (dgOrdertList.SelectedItem != null)
            {
                if (chk.IsChecked == true)
                {
                    SelectedOrderList.Add((clsRadOrderBookingVO)dgOrdertList.SelectedItem);
                }
                else
                {
                    SelectedOrderList.Remove((clsRadOrderBookingVO)dgOrdertList.SelectedItem);
                }
            }
        }

        private void CmdComapare_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderList != null && SelectedOrderList.Count < 2)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("PALASH", "PLEASE, SELECT ATLEAST TWO ORDERS TO COMPARE STUDY.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                frmVisitWisePACS obj = new frmVisitWisePACS();
                obj.SelectedOrderList = SelectedOrderList;
                obj.IsForVisitWiseCompare = true;
                //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Radiology.frmVisitWisePACS") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(obj);
            }
        }
        #endregion

        private void txtFrontOPDNO_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtFrontIPDNO_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)
        {


            clsRadOrderBookingVO item = (clsRadOrderBookingVO)e.Row.DataContext;


            //if (item.TotalCount == item.CompletedTest && item.TotalCount != 0 && item.CompletedTest != 0)
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Green);//completed
            //}

            //if (item.TotalCount == item.CompletedTest || item.TotalCount == item.CompletedTest + item.UploadedCount)
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Green);//completed
            //}

            //else if (item.ResultEntryCount == 0 && item.CompletedTest == 0)
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.DarkGray);//New
            //}
            //else if (item.TotalCount != item.CompletedTest && item.CompletedTest < item.TotalCount)//&& item.CompletedTest != 0 && item.ResultEntryCount != 0 && item.CompletedTest != 0
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Cyan);//Inprocess
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

        private void txtFrontFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            //CheckValidations();
            FillOrderBooking();
            txtFrontFirstName.Focus();
        }

        private void txtFrontLastName_KeyUp(object sender, KeyEventArgs e)
        {
            FillOrderBooking();
            txtFrontLastName.Focus();

        }

        private void txtFrontMRNO_KeyUp(object sender, KeyEventArgs e)
        {
            FillOrderBooking();

            txtFrontMRNO.Focus();
        }

        private void txtFrontMRNO_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontMRNO.Text = txtFrontMRNO.Text.ToTitleCase();
        }

        private void txtFrontFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFrontLastName_KeyDown(object sender, KeyEventArgs e)
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
        void w_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
          //  indicator.Show();
            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            mgbx.Show();
        
          // FillOrderBooking();
          
        }

        private void CmdUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTest.SelectedItem != null)//dgTest.SelectedItem != null
                {
                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsDelivered == false)
                    {
                        if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry == false)
                        {
                            if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID > 0)
                            {
                                if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsCancelled == false)
                                {
                                    if (dgTest.SelectedItem != null && ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload == false && ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsCompleted != true)
                                    {


                                        UploadReportChildWindow w = new UploadReportChildWindow();
                                        w.IsResultEntry = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry;
                                        w.DataContext = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem);
                                        w.ResultID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderID;//PathPatientReportID
                                        w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                        w.Show();

                                    }
                                    




                                    if (((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsUpload == true)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Is Already Uploaded. Do You Want To Replace Existing Uploaded Report.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                                        {
                                            if (res == MessageBoxResult.Yes)
                                            {
                                                UploadReportChildWindow w = new UploadReportChildWindow();
                                                w.IsResultEntry = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry;
                                                w.DataContext = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem);
                                                w.ResultID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderID;
                                                w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                                w.Show();
                                            }
                                        };
                                        mgbx.Show();
                                    }
                                    else
                                    {

                                    }


                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Cancelled", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                    mgbx.Show();
                                }

                               
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Services does not linked with test Master ", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Result Entry Already Done! Please Select Other Test ", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                            mgbx.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Delivered. Please Select the Correct Item to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        mgbx.Show();
                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }


            }
            catch (Exception)
            {
                throw;
            }
        

        }

      

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (((clsRadOrderBookingVO)dgOrdertList.SelectedItem).IsResultEntry==true)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Radiology/RadiologyUploadedFile.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            }
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
