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
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Windows.Data;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;

namespace PalashDynamics.Pathology
{
    public partial class PathologyWorkOrderGeneration : UserControl, IInitiateCIMS
    {
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }

        public PathologyWorkOrderGeneration()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SampleCollection_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }


        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement1.Text = "Pathology Work Order";

                    //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    //mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + "" + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;

                    break;
            }
        }

        #endregion


        void SampleCollection_Loaded(object sender, RoutedEventArgs e)
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

            cmbSampleType.ItemsSource = lstSampleType;
            cmbUploadStatus.ItemsSource = lstUploadStatus;
            cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;

            cmbSampleType.SelectedItem = lstSampleType[0];
            cmbUploadStatus.SelectedItem = lstUploadStatus[0];
            cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            mElement1.Text = "Pathology Work Order";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            //this.rdbAll.IsChecked = true;

            FillOrderBookingList();
        }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }
        #endregion

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();

                POBBizAction = new clsGetPathOrderBookingListBizActionVO();

                POBBizAction.PatientID = 0;

                if (txtFrontFirstName.Text != "")
                    POBBizAction.FirstName = txtFrontFirstName.Text;

                if (txtFrontLastName.Text != "")
                    POBBizAction.LastName = txtFrontLastName.Text;

                if (txtFrontMRNO.Text != "")
                    POBBizAction.MRNO = txtFrontMRNO.Text;


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

                //Set Paging Variables
                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;

                if (cmbSampleType.SelectedItem != null && ((MasterListItem)cmbSampleType.SelectedItem).ID == 0)
                {
                    POBBizAction.CheckSampleType = false;
                }
                else
                {
                    POBBizAction.CheckSampleType = true;
                    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
                        POBBizAction.SampleType = true;
                    else
                        POBBizAction.SampleType = false;
                }

                if (cmbUploadStatus.SelectedItem != null && ((MasterListItem)cmbUploadStatus.SelectedItem).ID == 0)
                {
                    POBBizAction.CheckUploadStatus = false;
                }
                else
                {
                    POBBizAction.CheckUploadStatus = true;
                    if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 1)
                        POBBizAction.IsUploaded = true;
                    else
                        POBBizAction.IsUploaded = false;
                }

                if (cmbDeliveryStatus.SelectedItem != null && ((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
                {
                    POBBizAction.CheckDeliveryStatus = false;
                }
                else
                {
                    POBBizAction.CheckDeliveryStatus = true;
                    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                        POBBizAction.IsDelivered = true;
                    else
                        POBBizAction.IsDelivered = false;
                }

                if (rdbAgainstIPD.IsChecked == true)
                {
                    POBBizAction.PatientType = 2; // Set for Patient Type - 1 : OPD 2 : IPD
                }
                else if (rdbAgainstOPD.IsChecked == true)
                {
                    POBBizAction.PatientType = 1; // Set for Patient Type - 1 : OPD 2 : IPD
                }

                //if (this.rdbAll.IsChecked == true)
                //{
                //    BizAction.CheckSampleType = false;
                //}
                //else
                //{
                //    BizAction.CheckSampleType = true;
                //    if (this.rdbCollected.IsChecked == true)
                //        BizAction.SampleType = true;
                //    else
                //        BizAction.SampleType = false;
                //}

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //List<clsPathOrderBookingVO> list = ((clsGetPathOrderBookingListBizActionVO)arg.Result).OrderBookingList;
                            //PagedCollectionView view = new PagedCollectionView(list);
                            //view.GroupDescriptions.Add(new PropertyGroupDescription("SampleType"));

                            //dgOrdertList.ItemsSource = ((clsGetPathOrderBookingListBizActionVO)arg.Result).OrderBookingList;
                            //dgOrdertList.ItemsSource = view;

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
                            }
                        }
                    }
                    indicator.Close();
                };

                client.ProcessAsync(POBBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
            }

        }

        private void FillOrderBookingDetailsList()
        {
            clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.CheckExtraCriteria = true;
            BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            BizAction.SampleType = POBBizAction.SampleType;
            BizAction.IsUploaded = POBBizAction.IsUploaded;
            BizAction.IsDelivered = POBBizAction.IsDelivered;
            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        dgTest.ItemsSource = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
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
        /// <summary>
        /// Purpose:To get selected patient information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //clsPathOrderBookingVO item=e.Row.DataContext as clsPathOrderBookingVO;
            //if (item.SampleType)
            //    e.Row.Background = new SolidColorBrush(new Color() { A=120,R=200,G=200,B=200});
            //else
            //    e.Row.Background = null;

        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void rdbAgainstIPD_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdbAgainstOPD_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
