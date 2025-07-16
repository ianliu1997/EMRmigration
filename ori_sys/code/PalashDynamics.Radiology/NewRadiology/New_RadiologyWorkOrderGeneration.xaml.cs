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
using System.Windows.Browser;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using CIMS;
//using CIMS.Forms;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Collections;
using System.Text.RegularExpressions;
namespace PalashDynamics.Radiology
{
    public partial class New_RadiologyWorkOrderGeneration : UserControl
    {
        List<clsRadOrderBookingDetailsVO> RadTestList;
        string textBefore = String.Empty;
        int selectionStart = 0;
        int selectionLength = 0;
        public New_RadiologyWorkOrderGeneration()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsRadOrderBookingVO>();
            RadTestList = new List<clsRadOrderBookingDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            DataPager.PageSize = DataListPageSize;
            DataPager.Source = DataList;
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
           // txtTechEntryDone.Background = new SolidColorBrush(Color.FromArgb(255, 137, 224, 196));
          //  txtResultEntryDone.Background = new SolidColorBrush(Color.FromArgb(255, 125, 203, 30));
            //txtReportDelivered.Background = new SolidColorBrush(Color.FromArgb(255, 247, 211, 88));
            txtTestCancelled.Background = new SolidColorBrush(Colors.Red);
            FillDoctor();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrders();
        }
        #region VariableDeclaration And Properties

        private SwivelAnimation objAnimation;
        ObservableCollection<clsChargeVO> ChargeList { get; set; }

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


        long PatientUnitID { get; set; }
        public PagedSortableCollectionView<clsRadOrderBookingVO> DataList { get; private set; }
        clsBillVO SelectedBill { get; set; }
        public long PatientId { get; set; }
        public long StoreID { get; set; }
        int ClickedFlag = 0;
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; }
        public bool IsFreez { get; set; }
        public long VisitId { get; set; }

        WaitIndicator Indicatior = null;
        public long VisitDoctorID { get; set; }
        #endregion

        #region EventHandlers

        private void WorkOrderGeneration_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                FillClinic();
                FillDeliveryStatus();
                FillResultEntryStatus();
               // FillResultEntry();
                FillModality();
                FillOrders();
            }
            //FillOrders();
        }

        bool IsPageLoaded = false;

        #region ComboBoxFillingMethods

        private void FillDeliveryStatus()
        {
            List<MasterListItem> listDeliveryStatus = new List<MasterListItem>();
            listDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "All", Status = true });
            listDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            listDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });
            cmbDeliveryStatus.ItemsSource = listDeliveryStatus;
            cmbDeliveryStatus.SelectedItem = listDeliveryStatus[0];

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
        private void FillClinic()
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

        private void FillModality()
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
                    cmbModality.ItemsSource = null;
                    cmbModality.ItemsSource = listModility;
                    cmbModality.SelectedItem = listModility[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        //private void FillTreatingDr()//Added By Yogesh Dr cmb
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_DoctorMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy

        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> listModility = new List<MasterListItem>();
        //            listModility.Add(new MasterListItem(0, "--Select--"));
        //            listModility.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbModality.ItemsSource = null;
        //            cmbModality.ItemsSource = listModility;
        //            cmbModality.SelectedItem = listModility[0];
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void FillDoctor()
        {
            //clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "-- Select --"));
            //        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

            //        cmbTreatingDr.ItemsSource = null;
            //        cmbTreatingDr.ItemsSource = objList;


            //        if (cmbTreatingDr.Text != "")
            //        {
            //            cmbTreatingDr.SelectedItem = cmbTreatingDr.Text;
            //        }

            //    }

            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();

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

                    cmbTreatingDr.ItemsSource = null;
                    cmbTreatingDr.ItemsSource = objList;
                    cmbTreatingDr.SelectedItem = objList[0];

                    //cmbTempRadiologist.ItemsSource = null;
                    // cmbTempRadiologist.ItemsSource = objList;


                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void FillOrders()
        {
            clsGetOrderBookingListBizActionVO BizAction = new clsGetOrderBookingListBizActionVO();
            try
            {

                BizAction.BookingList = new List<clsRadOrderBookingVO>();
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
                if (cmbDeliveryStatus.SelectedItem == null)
                {
                   // BizAction.CheckDeliveryStatus = null;
                }
                if (txtMRNO.Text != "")
                {
                    BizAction.MRNO = txtMRNO.Text;//Added By Yogesh
                }
                if (cmbTreatingDr.SelectedItem != null && ((MasterListItem)cmbTreatingDr.SelectedItem).ID > 0)
                {
                    BizAction.Radiologist = cmbTreatingDr.SelectedItem.ToString();//Added By Yogesh K 
                  
                }

                if ((MasterListItem)cmbModality.SelectedItem != null && ((MasterListItem)cmbModality.SelectedItem).ID > 0)
                {                  
                   
                    BizAction.CategoryID = ((MasterListItem)cmbModality.SelectedItem).ID;
                }

              
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
                    if (arg.Error == null && (clsGetOrderBookingListBizActionVO)arg.Result != null)
                    {
                        if (((clsGetOrderBookingListBizActionVO)arg.Result).BookingList != null)
                        {
                            clsGetOrderBookingListBizActionVO result = arg.Result as clsGetOrderBookingListBizActionVO;
                            DataList.Clear();
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.BookingList != null)
                            {
                                foreach (var item in result.BookingList)
                                {
                                    DataList.Add(item);
                                }

                                dgOrdertList.ItemsSource = null;
                                dgOrdertList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;
                                if (DataList.Count == 0)
                                {
                                    dgTest.ItemsSource = null;
                                    Footer.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                {
                                    Footer.Visibility = System.Windows.Visibility.Visible;
                                }

                               
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        private void FillOrdersDetails(long iID)
        {
            clsGetOrderBookingDetailsListForWorkOrderBizActionVO BizAction = new clsGetOrderBookingDetailsListForWorkOrderBizActionVO();
            try
            {
                BizAction.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                BizAction.ID = iID;

                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (cmbModality.SelectedItem != null)
                {
                    BizAction.ModalityID = ((MasterListItem)cmbModality.SelectedItem).ID;
                   
                }
              
                if (cmbDeliveryStatus.SelectedItem == null)
                {
                    BizAction.CheckDeliveryStatus = false;

                }
                if (cmbTreatingDr.SelectedItem != null)
                {
                    BizAction.RadiologistID = ((MasterListItem)cmbTreatingDr.SelectedItem).ID;
                }

                if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                {
                    BizAction.CheckDeliveryStatus = true;
                }
                else
                {
                   
                    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 2)
                        BizAction.CheckDeliveryStatus = false;
                    else
                        BizAction.CheckDeliveryStatus = false;
                }
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

                if (cmbTreatingDr.SelectedItem != null)
                    BizAction.Radiologist = cmbTreatingDr.SelectedItem.ToString();

                if (cmbclinic.SelectedItem != null)
                    BizAction.UnitID = ((MasterListItem)cmbclinic.SelectedItem).ID;
                else
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetOrderBookingDetailsListForWorkOrderBizActionVO)arg.Result).BookingDetails != null)
                        {
                            RadTestList = ((clsGetOrderBookingDetailsListForWorkOrderBizActionVO)arg.Result).BookingDetails;
                            #region Set Images as Status
                            foreach (clsRadOrderBookingDetailsVO item in RadTestList)
                            {
                                //if (item.IsResultEntry == true)
                                //    item.ResultEntryImage = "../Icons/tick.png";
                                //else
                                //    item.ResultEntryImage = "../Icons/error.png";
                                //if (item.IsFinalized == true)
                                //    item.ResultEntryFinalImage = "../Icons/tick.png";
                                //else
                                //    item.ResultEntryFinalImage = "../Icons/error.png";
                                //if (item.IsTechnicianEntry == true)
                                //    item.TechnicianEntryImage = "../Icons/tick.png";
                                //else
                                //    item.TechnicianEntryImage = "../Icons/error.png";
                                //if (item.IsTechnicianEntryFinalized == true)
                                //    item.TechnicianEntryFinalImage = "../Icons/tick.png";
                                //else
                                //    item.TechnicianEntryFinalImage = "../Icons/error.png";
                                //if (item.IsDelivered == true)
                                //    item.ReportDeliveredImage = "../Icons/tick.png";
                                //else
                                //    item.ReportDeliveredImage = "../Icons/error.png";
                                //if (item.Contrast == true)
                                //    item.ContrastGivenImage = "../Icons/tick.png";
                                //else
                                //    item.ContrastGivenImage = "../Icons/error.png";
                                //if (item.Sedation == true)
                                //    item.SedationGivenImage = "../Icons/tick.png";
                                //else
                                //    item.SedationGivenImage = "../Icons/error.png";

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

                            }
                            #endregion
                            dgTest.ItemsSource = RadTestList;
                            if ((((clsGetOrderBookingDetailsListForWorkOrderBizActionVO)arg.Result).BookingDetails).Count == 0)
                                Footer.Visibility = System.Windows.Visibility.Collapsed;
                            else
                                Footer.Visibility = System.Windows.Visibility.Visible;
                          
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
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            CheckValidations();
            FillOrders();
        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrdertList.SelectedItem != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder = (clsRadOrderBookingVO)dgOrdertList.SelectedItem;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.BillID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).BillID;

                clsPatientGeneralVO Obj = new clsPatientGeneralVO();

                Obj.UnitId = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).PatientUnitId;

                Obj.PatientID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).PatientID;

                Obj.PName = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).PatientName;

                ((IApplicationConfiguration)App.Current).SelectedPatient = Obj;
                ////long ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;

                long ID = ((clsRadOrderBookingVO)dgOrdertList.SelectedItem).ID;
                FillOrdersDetails(ID);
            }
            else
            {
                dgOrdertList.ItemsSource = null;
            }
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsRadOrderBookingDetailsVO item = (clsRadOrderBookingDetailsVO)e.Row.DataContext;

            if (item.IsCancelled == true)
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);//completed
            }
            
            //Commented By Yogesh K As discussed with Dr Priyanka K

            //clsRadOrderBookingDetailsVO item = (clsRadOrderBookingDetailsVO)e.Row.DataContext;
            //if (item.IsTechnicianEntry == true && item.IsResultEntry == false)
            //    e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 137, 224, 196));
            //else if (item.IsTechnicianEntry == true && item.IsResultEntry == true && item.IsDelivered == true)
            //{
            //    e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 247, 211, 88));
            //}
            //else if (item.IsTechnicianEntry == true && item.IsResultEntry == true)
            //{
            //    e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 125, 203, 30));
            //}
            //else if (item.IsResultEntry == true)
            //{
            //    e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 125, 203, 30));
            //}
            //else if (item.IsCancelled == true)
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Red);
            //}

            //else
            //{
            //    e.Row.Background = null;
            //}




        }
        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {

                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.TestID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).TestID;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).RadOrderID;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.OrderDetailID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ID;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ChargeID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ChargeID;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ServiceID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ServiceID;

                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsFinalized = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsFinalized;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsResultEntry = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsResultEntry;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsDelivered = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsDelivered;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.ModalityID = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).ModalityID;

                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.Sedation = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).Sedation;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.FilmWastage = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).FilmWastage;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.FilmWastageDetails = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).FilmWastageDetails;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.NoOfFilms = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).NoOfFilms;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsTechnicianEntry = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsTechnicianEntry;
                ((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.IsTechnicianEntryFinalized = ((clsRadOrderBookingDetailsVO)dgTest.SelectedItem).IsTechnicianEntryFinalized;
            }


        }

        #endregion

        #region Validations
        bool flag = false;
        public void CheckValidations()
        {
            bool res = true;
            if (dtpToDate.SelectedDate > System.DateTime.Now)
            {
                dtpToDate.SetValidation("To Date should be less than Today's Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }
            if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate > System.DateTime.Now)
            {
                dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;

            }
            else if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    res = false;
                }
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
        }

        #endregion

        private void txtFrontFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            CheckValidations();
            FillOrders();
            txtFrontFirstName.Focus();
        }

        private void txtFrontLastName_KeyUp(object sender, KeyEventArgs e)
        {
            CheckValidations();
            FillOrders();
            txtFrontLastName.Focus();
        }

        private void txtFrontOPDNO_KeyUp(object sender, KeyEventArgs e)
        {
            CheckValidations();
            FillOrders();
            txtFrontOPDNO.Focus();
        }

        private void txtMRNO_KeyUp(object sender, KeyEventArgs e)
        {
            CheckValidations();
            FillOrders();
            txtMRNO.Focus();

        }

        private void txtMRNO_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMRNO.Text = txtMRNO.Text.ToTitleCase();
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void txtFrontIPDNO_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           

            clsRadOrderBookingVO item = (clsRadOrderBookingVO)e.Row.DataContext;

            if (item.TotalCount == item.CompletedTest ||item.TotalCount==item.CompletedTest+item.UploadedCount)               
            {    
                    e.Row.Background = new SolidColorBrush(Colors.Green);//completed
            }
            else if (item.ResultEntryCount == 0 && item.CompletedTest == 0 && item.UploadedCount==0)
            {
                e.Row.Background = new SolidColorBrush(Colors.DarkGray);//New
            }
            else if (item.TotalCount != item.CompletedTest && item.CompletedTest < item.TotalCount || item.UploadedCount<item.TotalCount)//&& item.CompletedTest != 0 && item.ResultEntryCount != 0 && item.CompletedTest != 0
            {
                e.Row.Background = new SolidColorBrush(Colors.Cyan);//Inprocess
            }


            if (item.ReferredDoctor == "- Select -")
            {
                item.ReferredDoctor = "Not Assigned";

            }         
                            

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

        private void txtFrontLastName_TextChanged(object sender, TextChangedEventArgs e)
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
        
    }
}