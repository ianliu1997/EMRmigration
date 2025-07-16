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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Collections.Generic;
using PalashDynamics;
using PalashDynamics.UserControls;
using System.ComponentModel;
using System.Linq;
using System.Windows.Browser;
using System.Windows.Controls.Data;
using PalashDynamics.ValueObjects;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Patient;
using System.Reflection;
using PalashDynamics;

namespace PalashDynamics.Forms.Home
{
    public partial class HomeOPUET : UserControl
    {
   
        public PagedSortableCollectionView<clsTherapyDashBoardVO> DataList { get; private set; }
        TextBlock mElement;
        UserControl rootPage = Application.Current.RootVisual as UserControl;
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
        public HomeOPUET()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Therapy List";
            DataList = new PagedSortableCollectionView<clsTherapyDashBoardVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(TherapyList_OnRefresh);   
            DataListPageSize = 19;

        }
        void TherapyList_OnRefresh(object sender, RefreshEventArgs e)
        {
            if (CheckValidation() == true)
            {
                GetTherapyList();
            }
        }
        private void frmHomeOPUET_Loaded(object sender, RoutedEventArgs e)
        {
            dtpETFrom.Focus();
            if (CheckValidation() == true)
            {
                GetTherapyList();
            }
          
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
           if( CheckValidation()==true)
            {
               GetTherapyList();
            }
        }
        private bool CheckValidation()
        {
            bool result = true;

           // if (dtpETFrom.SelectedDate == null)
           // {
           //     dtpETFrom.SetValidation("Please select From Date");
           //     dtpETFrom.RaiseValidationError();
           //     dtpETFrom.Focus();
           //     result = false;
           // }
           //else 
                if(dtpETFrom.SelectedDate!=null &&  dtpETTo.SelectedDate==null)      
            {
                dtpETTo.SetValidation("Please select To Date");
                dtpETTo.RaiseValidationError();
                dtpETTo.Focus();
                result = false;
            }
            else
            {
                if (dtpETFrom.SelectedDate > dtpETTo.SelectedDate)
                {
                    dtpETFrom.SetValidation("Please select From Date less than To Date");
                    dtpETFrom.RaiseValidationError();
                    dtpETFrom.Focus();
                    result = false;
                }
                else
                    dtpETFrom.ClearValidationError();
            }
            return result;
            
        }
        private void grdAlerts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void GetTherapyList()
        {
            DataList.Clear();
            clsGetTherapyDetailsForDashBoardBizActionVO BizAction = new clsGetTherapyDetailsForDashBoardBizActionVO();
            BizAction.TherapyDetailsList = new List<clsTherapyDashBoardVO>();
           // BizAction.FromDate = DateTime.Now.Date;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

                        //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (dtpETFrom.SelectedDate != null)
            {
                BizAction.FromDate2 = dtpETFrom.SelectedDate.Value.Date.Date;
            }
            if (dtpETTo.SelectedDate != null)
            {
                BizAction.ToDate2 = dtpETTo.SelectedDate.Value.Date.Date; ;
            }
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetTherapyDetailsForDashBoardBizActionVO)arg.Result).TherapyDetailsList != null)
                    {
                        clsGetTherapyDetailsForDashBoardBizActionVO result = arg.Result as clsGetTherapyDetailsForDashBoardBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        List<clsTherapyDashBoardVO> DataList1 = new List<clsTherapyDashBoardVO>();
                        if (result.TherapyDetailsList != null)
                        {
                            foreach (var item in result.TherapyDetailsList)
                            {
                                DataList.Add(item);
                            }
                        }
                        DataList1 = DataList.ToList();
                        grdAlerts.ItemsSource = DataList1;
                       // dgDataPager.PageIndex = 1;
                        dgDataPager.Source = DataList; 
                    }
                   
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetTherapyList();
            }
        }
    }
}
