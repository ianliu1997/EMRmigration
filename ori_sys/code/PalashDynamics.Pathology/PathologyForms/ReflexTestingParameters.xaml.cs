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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using CIMS;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class ReflexTestingParameters : ChildWindow
    {

        public PagedSortableCollectionView<clsGetReflexTestingServiceParameterBizActionVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public long ServiceId;
        public long ParameterId;


        public ReflexTestingParameters()
        {
            InitializeComponent();

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
            DataList = new PagedSortableCollectionView<clsGetReflexTestingServiceParameterBizActionVO>();
            GetServiceList();


        }

        private void GetServiceList()
        {
            clsGetReflexTestingServiceParameterBizActionVO BizAction = new clsGetReflexTestingServiceParameterBizActionVO();


            BizAction.ServiceID = ServiceId;
            BizAction.ParameterID = ParameterId;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetReflexTestingServiceParameterBizActionVO result = arg.Result as clsGetReflexTestingServiceParameterBizActionVO;

                    if (result.ServiceList != null)
                    {
                        foreach (var item in result.ServiceList)
                        {
                            //item.Date = item.Date.ToShortDateString();
                            DataList.Add(item);

                        }

                    }


                    dgMachineList1.ItemsSource = null;
                    dgMachineList1.ItemsSource = DataList;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }


         protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
}
    }




