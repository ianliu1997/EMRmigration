using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Patient;

using PalashDynamics.OutPatientDepartment.ViewModels;
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.Generic;
using PalashDynamics;
using PalashDynamics.UserControls;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.Billing;
using System.Linq;
using PalashDynamics.ValueObjects.IVFPlanTherapy;


namespace PalashDynamics.OutPatientDepartment.ViewModels
{
    public class HomePatientsViewModel : ViewModelBase
    {
        public clsGetPatientGeneralDetailsListBizActionVO BizActionObject { get; set; }
        public clsGetAppointmentBizActionVO BizActionAppointmentObject { get; set; }
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsAppointmentVO> AppointmentList { get; private set; }
        public PagedSortableCollectionView<clsAppointmentVO> TomAppointmentList { get; private set; }
        public PagedSortableCollectionView<clsBillVO> PendingBillList { get; private set; }
        public PagedSortableCollectionView<clsQueueVO> QueueList { get; private set; }
        public PagedSortableCollectionView<clsDailyCollectionReportVO> DailyCollection { get; private set; }
        public Double TotalCollection { get; private set; }

        //Added by Saily P for ET and OPU Dashboard
        public PagedSortableCollectionView<clsTherapyDashBoardVO> TherapyList { get; private set; }


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleViewModel"/> class.
        /// </summary>
        public HomePatientsViewModel()
        {
            BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            BizActionObject.IsPagingEnabled = true;
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();

            BizActionAppointmentObject = new clsGetAppointmentBizActionVO();
            BizActionAppointmentObject.InputPagingEnabled = true;

            AppointmentList = new PagedSortableCollectionView<clsAppointmentVO>();
            AppointmentList.OnRefresh += new EventHandler<RefreshEventArgs>(AppointmentList_OnRefresh);

            TherapyList = new PagedSortableCollectionView<clsTherapyDashBoardVO>();
            TherapyList.OnRefresh += new EventHandler<RefreshEventArgs>(TherapyList_OnRefresh);
        

            //---------------commented by rohini Honmute oct 26 for dashboard-------------

            //TomAppointmentList = new PagedSortableCollectionView<clsAppointmentVO>();
            //TomAppointmentList.OnRefresh += new EventHandler<RefreshEventArgs>(TomAppointmentList_OnRefresh);

            //QueueList = new PagedSortableCollectionView<clsQueueVO>();
            //QueueList.OnRefresh += new EventHandler<RefreshEventArgs>(QueueList_OnRefresh);

            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            //PendingBillList = new PagedSortableCollectionView<clsBillVO>();
            //PendingBillList.OnRefresh += new EventHandler<RefreshEventArgs>(PendingBillList_OnRefresh);

            //DailyCollection = new PagedSortableCollectionView<clsDailyCollectionReportVO>();
            //DailyCollection.OnRefresh += new EventHandler<RefreshEventArgs>(DailyCollection_OnRefresh);

            //-----------------------------------

            // get from database on first call
            PageSize = 15;
            GetData();
            GetAppointmentList();
            GetTherapyList();

            //---------------commented by rohini Honmute oct 26 for dashboard-------------
                //GetCollectionList();          
                //GetTomAppointmentList();
                //GetQueueList();
            //-----------------------------------


        }

        void DailyCollection_OnRefresh(object sender, RefreshEventArgs e)
        {
            //throw new NotImplementedException();
            GetCollectionList();
        }
        #endregion

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                AppointmentList.PageSize = value;
                TherapyList.PageSize = value;
                //commented by rohini
                //QueueList.PageSize = value;
                //TomAppointmentList.PageSize = value;               
              //
                
                RaisePropertyChanged("PageSize");
            }
        }

        #region Get Data
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PendingBillList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillBillSearchList();
        }

        void AppointmentList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetAppointmentList();
        }

        void TomAppointmentList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetTomAppointmentList();
        }

        void TherapyList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetTherapyList();
        }

        void QueueList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetQueueList();
        }

        //WaitIndicator indicator = new WaitIndicator();
        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        public void GetData()
        {
            //indicator.Show();
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;
            short? days = 0;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations != null)
                days = (((IApplicationConfiguration)App.Current).ApplicationConfigurations).SearchPatientsInterval;
            if (days == null && days == 0)
                days = 2;

            BizActionObject.FromDate = DateTime.Now.Date.AddDays(-(Convert.ToDouble(days)));
            BizActionObject.ToDate = DateTime.Now.Date.AddDays(1);

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
            }
            else
            {
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            //foreach (SortDescription sortDesc in DataList.SortDescriptions)
            //{
            //    // BizActionObject.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
            //    break;
            //}
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                    {
                        DataList.Add(person);

                    }
                }
                //indicator.Close();
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void FillBillSearchList()
        {
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.FromDate = DateTime.Now.Date;
            BizAction.ToDate = DateTime.Now.Date;
            BizAction.OPDNO = null;
            BizAction.BillNO = null;
            BizAction.BillStatus = 2;
            BizAction.MRNO = null;
            BizAction.FirstName = null;
            BizAction.MiddleName = null;
            BizAction.LastName = null;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = PendingBillList.PageIndex * PendingBillList.PageSize;
            BizAction.MaximumRows = PendingBillList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    PendingBillList.TotalItemCount = result.TotalRows;

                    if (result.PendingBillList != null)
                    {
                        PendingBillList.Clear();
                        foreach (clsBillVO BillList in result.PendingBillList)
                        {
                            PendingBillList.Add(BillList);
                        }
                    }
                }
                //dgBillList.ItemsSource = null;
                //dgBillList.ItemsSource = DataList;

                //dgDataPager.Source = null;
                //dgDataPager.PageSize = BizAction.MaximumRows;
                //dgDataPager.Source = DataList;
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public void GetAppointmentList()
        {
            clsGetAppointmentBizActionVO BizAction = new clsGetAppointmentBizActionVO();
            BizAction.AppointmentDetailsList = new List<clsAppointmentVO>();
            BizAction.FromDate = DateTime.Now.Date;
            BizAction.ToDate = DateTime.Now.Date;
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                BizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
            }
            BizAction.InputPagingEnabled = true;
            BizAction.InputStartRowIndex = AppointmentList.PageIndex * AppointmentList.PageSize;
            BizAction.InputMaximumRows = AppointmentList.PageSize;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitId = 0;
            }
            else
            {
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetAppointmentBizActionVO)arg.Result).AppointmentDetailsList != null)
                    {
                        clsGetAppointmentBizActionVO result = arg.Result as clsGetAppointmentBizActionVO;
                        AppointmentList.TotalItemCount = result.OutputTotalRows;
                        if (result.AppointmentDetailsList != null)
                        {
                            AppointmentList.Clear();
                            foreach (var item in result.AppointmentDetailsList)
                            {
                                AppointmentList.Add(item);
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        // BY BHSUHAN . . . . . . . .
        public void GetTherapyList()
        {
            TherapyList.Clear();
            clsGetTherapyDetailsForDashBoardBizActionVO BizAction = new clsGetTherapyDetailsForDashBoardBizActionVO();
            BizAction.TherapyDetailsList = new List<clsTherapyDashBoardVO>();
            BizAction.FromDate = DateTime.Now.Date;       
            BizAction.IsPagingEnabled = true;          
            BizAction.StartIndex = TherapyList.PageIndex * TherapyList.PageSize;
            BizAction.MaximumRows = TherapyList.PageSize;          
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetTherapyDetailsForDashBoardBizActionVO)arg.Result).TherapyDetailsList != null)
                    {
                        clsGetTherapyDetailsForDashBoardBizActionVO result = arg.Result as clsGetTherapyDetailsForDashBoardBizActionVO;
                        TherapyList.TotalItemCount = result.TotalRows;

                        if (result.TherapyDetailsList != null)
                        {
                            foreach (var item in result.TherapyDetailsList)
                            {
                                TherapyList.Add(item);
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void GetTomAppointmentList()
        {
            clsGetAppointmentBizActionVO BizAction = new clsGetAppointmentBizActionVO();
            BizAction.AppointmentDetailsList = new List<clsAppointmentVO>();
            BizAction.FromDate = DateTime.Now.Date.AddDays(1);
            BizAction.ToDate = DateTime.Now.Date.AddDays(1);

            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                BizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
            }
            BizAction.InputPagingEnabled = true;
            BizAction.InputStartRowIndex = TomAppointmentList.PageIndex * TomAppointmentList.PageSize;
            BizAction.InputMaximumRows = TomAppointmentList.PageSize;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitId = 0;
            }
            else
            {
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetAppointmentBizActionVO)arg.Result).AppointmentDetailsList != null)
                    {
                        clsGetAppointmentBizActionVO result = arg.Result as clsGetAppointmentBizActionVO;
                        TomAppointmentList.TotalItemCount = result.OutputTotalRows;

                        if (result.AppointmentDetailsList != null)
                        {
                            TomAppointmentList.Clear();

                            foreach (var item in result.AppointmentDetailsList)
                            {
                                TomAppointmentList.Add(item);
                            }
                        }
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        public void GetQueueList()
        {
            clsGetQueueListBizActionVO BizAction = new clsGetQueueListBizActionVO();

            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                BizAction.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = QueueList.PageIndex * QueueList.PageSize;
            BizAction.MaximumRows = QueueList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    clsGetQueueListBizActionVO result = arg.Result as clsGetQueueListBizActionVO;
                    QueueList.TotalItemCount = result.TotalRows;

                    if (result.QueueList != null)
                    {
                        QueueList.Clear();
                        foreach (clsQueueVO Queue in result.QueueList)
                        {
                            QueueList.Add(Queue);
                        }
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        public void GetCollectionList()
        {
            //indicator.Show();
            clsGetDailyCollectionListBizActionVO BizAction = new clsGetDailyCollectionListBizActionVO();


            //foreach (SortDescription sortDesc in DataList.SortDescriptions)
            //{
            //    // BizActionObject.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
            //    break;
            //}
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetDailyCollectionListBizActionVO result = ea.Result as clsGetDailyCollectionListBizActionVO;

                    DailyCollection.Clear();
                    if (result != null && result.List != null)
                    {
                        foreach (clsDailyCollectionReportVO Queue in result.List)
                        {
                            DailyCollection.Add(Queue);
                        }

                        var res = from c in DailyCollection
                                  select c;

                        // var results = from r in this.ocIssueItemDetailsList
                        // select r;
                        TotalCollection = res.Sum(cnt => cnt.Collection);
                        // decimal? TotalIssueItem = results.Sum(cnt => cnt.IssueQty);

                    }

                    //   DailyCollection = result.List;

                }
                //indicator.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion
    }
}
