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
using CIMS;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmCryoEmbryoList : ChildWindow
    {
        public long PatientID, PatientUnitID;
        public event RoutedEventHandler OnSaveButton_Click;
        public string MRNO;
        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> EmbryoList { get; private set; }
        public frmCryoEmbryoList()
        {
            InitializeComponent();
            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            PageSize = 7;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;
        }

        private void FillEmbryoCryoBank()
        {
            try
            {
                cls_IVFDashboard_GetVitrificationDetailsForCryoBank BizAction = new cls_IVFDashboard_GetVitrificationDetailsForCryoBank();

                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = EmbryoList.PageSize;
                BizAction.StartRowIndex = EmbryoList.PageIndex * EmbryoList.PageSize;
                BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.MRNo = MRNO;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        EmbryoDataGrid.ItemsSource = null;
                        EmbryoDataPager.DataContext = null;
                        BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                        BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                        if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                        {
                            EmbryoList.Clear();
                            EmbryoList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                            foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                            {
                                EmbryoList.Add(item);
                            }
                            EmbryoDataGrid.ItemsSource = EmbryoList;
                            EmbryoDataPager.DataContext = EmbryoList;
                        }
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
        public int PageSize
        {
            get
            {
                return EmbryoList.PageSize;
            }
            set
            {
                if (value == EmbryoList.PageSize) return;
                EmbryoList.PageSize = value;
                //  OnPropertyChanged("PageSize");
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillEmbryoCryoBank();

        }
        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }
        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _Selected;
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> SelectedBatches { get { return _Selected; } }
        private void chkSelect_Click(object sender, RoutedEventArgs e)
        {
            if (EmbryoDataGrid.SelectedItem != null)
            {
                if (_Selected == null)
                    _Selected = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _Selected.Add((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);
                else
                    _Selected.Remove((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
