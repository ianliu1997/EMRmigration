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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class PreviousResults : ChildWindow
    {
        public long PatientId;
        public long PatientUnitId;
        public long TestId;
        public long ParameterId;
        public long mainPathologyid;
        public DateTime? OrderDate;

        public PagedSortableCollectionView<clsGetPreviousParameterValueBizActionVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public List<clsGetPreviousParameterValueBizActionVO> PreviousParameter; // = new List<clsGetPreviousParameterValueBizActionVO>();
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }

        }
        public PreviousResults()
        {
            InitializeComponent();
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            PageSize = 10;
           // this.dataGrid2Pager.DataContext = MasterList;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsGetPreviousParameterValueBizActionVO>();
            FillPreviousRecord();


        }
      private void FillPreviousRecord()
        {
            clsGetPreviousParameterValueBizActionVO BizAction = new clsGetPreviousParameterValueBizActionVO();
            BizAction.PathoTestParameter = new clsPathoTestParameterVO();
            BizAction.PathTestId = new clsPathOrderBookingDetailVO();
            BizAction.PathPatientDetail = new clsPathOrderBookingVO();
            

            BizAction.PathoTestParameter.ParameterID = ParameterId;
            BizAction.PathTestId.TestID = TestId;
            BizAction.PathPatientDetail.PatientID = PatientId;

            BizAction.PathPatientDetail.PatientUnitID = PatientUnitId;
            BizAction.PathPatientDetail.ID = mainPathologyid;

            BizAction.PathPatientDetail.OrderDate = OrderDate;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
                 {
                      if (arg.Error == null && arg.Result != null)
                     {
                         clsGetPreviousParameterValueBizActionVO  result = arg.Result as clsGetPreviousParameterValueBizActionVO;
                       
                          if(result.ParameterList != null)
                          {
                            
                              foreach (var item in result.ParameterList)
                              {
                                  //item.Date = item.Date.ToShortDateString();
                                  DataList.Add(item);
                                  
                              }
                          }
                          dgMachineList.ItemsSource = null;
                          dgMachineList.ItemsSource = DataList;
                     }
                 };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void chkMachine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OKGraph_Click(object sender, RoutedEventArgs e)
        {
            string address = string.Empty;
         
            //PreviousResultList = DataList;


            //address = "../GrowthChart/frmPreviousResult.aspx?PatientId=";
            //string URL = address  + PatientId + "&TestId=" + TestId + "&ParameterId=" + ParameterId ;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

