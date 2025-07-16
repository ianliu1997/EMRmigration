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
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using CIMS;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class DeltaCheck : ChildWindow
    {
        public long PatientId;
        public long PatientUnitID; //BY ROHINE
        public long mainPathologyid; //BY ROHINEE
        public long TestId;
        public long ParameterId;
        public double CalDeltaCheck;
        public string ResultValue;
        public double DeltaCheckDefaultValue;
        public DateTime? OrderDate;

        public PagedSortableCollectionView<clsGetDeltaCheckValueBizActionVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
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
        public DeltaCheck()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsGetDeltaCheckValueBizActionVO>();
            this.Title = "Delta Check: " + DeltaCheckDefaultValue.ToString();
            FillDeltaCheck();
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillDeltaCheck()
        {
            clsGetDeltaCheckValueBizActionVO BizAction = new clsGetDeltaCheckValueBizActionVO();
            BizAction.PathoTestParameter = new clsPathoTestParameterVO();
            BizAction.PathTestId = new clsPathOrderBookingDetailVO();
            BizAction.PathPatientDetail = new clsPathOrderBookingVO();
            BizAction.List = new List<clsGetDeltaCheckValueBizActionVO>();

            

            BizAction.PathoTestParameter.ParameterID = ParameterId;
            BizAction.PathTestId.TestID = TestId;
            BizAction.PathPatientDetail.PatientID = PatientId;
            BizAction.PathPatientDetail.PatientUnitID = PatientUnitID;
            BizAction.DetailID =TestId.ToString();
            BizAction.PathPatientDetail.OrderDate = OrderDate;
            BizAction.PathPatientDetail.ID = mainPathologyid;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetDeltaCheckValueBizActionVO result = arg.Result as clsGetDeltaCheckValueBizActionVO;

                    if (result.List.Count != 0 )
                    {
                        foreach (var item in result.List)
                        {
                            //item.Date = item.Date.ToShortDateString();
                            
                            item.CurrentValue = ResultValue;
                            item.CalDeltaValue = CalDeltaCheck;
                        //    item.CalDeltaValue = Math.Abs(item.CalDeltaValue);
                            DataList.Add(item);
                        }

                        foreach (var item in DataList)
                        {
                            if (item.ParameterID == ParameterId)
                            {
                                if (item.ResultValue != null)
                                {
                                  //  CalDeltaCheck = (Convert.ToDouble(item.ResultValue) - Convert.ToDouble(ResultValue)) / Convert.ToDouble(item.ResultValue) * 100;
                                    //item.CalDeltaValue = CalDeltaCheck;
                                    if (CalDeltaCheck <= DeltaCheckDefaultValue)
                                    {
                                        item.CheckDeltaFlag = "Fail";

                                    }
                                    else
                                    {
                                        item.CheckDeltaFlag = "Pass";
                                    }

                                    if (item.CalDeltaValue < 0)
                                    {
                                        item.CalDeltaValue = Math.Abs(item.CalDeltaValue);
                                    }
                                }

                                else
                                {
                                    item.CheckDeltaFlag = "N.A";
                                }
                            }
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
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

