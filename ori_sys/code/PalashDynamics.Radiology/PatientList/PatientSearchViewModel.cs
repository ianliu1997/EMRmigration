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
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.Generic;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;

namespace PalashDynamics.Radiology
{
    public class PatientSearchViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool DelayNotification { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (DelayNotification) return;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion
      
        #region Constructor
        public PatientSearchViewModel()
        {
            BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            BizActionObject.IsPagingEnabled = true;
            BizActionObject.VisitWise = true;

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            // get from database on first call
            PageSize = 10;
            BizActionObject.VisitFromDate = DateTime.Now.Date;
            BizActionObject.VisitToDate = DateTime.Now.Date;
            GetData();

        }

        public PatientSearchViewModel(DateTime? FrmDate, DateTime? ToDate)
        {
            BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            BizActionObject.IsPagingEnabled = true;
            BizActionObject.FromDate = FrmDate;
            BizActionObject.ToDate = ToDate.Value.AddDays(1);
            BizActionObject.VisitWise = true;
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 10;
            GetData();

        }


         WaitIndicator indicator = new WaitIndicator();

        #endregion

        #region Propertiesh
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
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
                RaisePropertyChanged("PageSize");
            }
        }
        #endregion

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

        public clsGetPatientGeneralDetailsListBizActionVO BizActionObject { get; set; }
        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        public void GetData()
        {
            indicator.Show();
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizActionObject.VisitWise = false;
            //BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
            }
            else
            {
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {

                            DataList.Add(person);
                        }
                    }
                    else
                    {
                        DataList.TotalItemCount = 0;
                        DataList.Clear();
                    }
                }
                indicator.Close();
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion
    }
}
