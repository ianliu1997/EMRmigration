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
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmGetSemenBatchList : ChildWindow
    {
        public long PatientID, PatientUnitID;
        public event RoutedEventHandler OnSaveButton_Click;
        public frmGetSemenBatchList()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private ObservableCollection<clsBatchAndSpemFreezingVO> _BatchSelected;
        public ObservableCollection<clsBatchAndSpemFreezingVO> SelectedBatches { get { return _BatchSelected; } }
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillDetails();

        }
        private void fillDetails()
        {
            try
            {

                cls_GetSemenBatchAndSpermiogramBizActionVO BizAction = new cls_GetSemenBatchAndSpermiogramBizActionVO();
                BizAction.DetailsList = new List<ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO>();
                BizAction.Details = new ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO();
                BizAction.Details.PatientID = PatientID;
                BizAction.Details.PatientUnitID = PatientUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList.Count > 0)
                        {
                            dataGrid2.ItemsSource = null;
                            dataGrid2.ItemsSource = ((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList;
                            dataGrid2.UpdateLayout();
                        }
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

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsBatchAndSpemFreezingVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _BatchSelected.Add((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem);
                else
                    _BatchSelected.Remove((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem);
            }
        }

    }
}
