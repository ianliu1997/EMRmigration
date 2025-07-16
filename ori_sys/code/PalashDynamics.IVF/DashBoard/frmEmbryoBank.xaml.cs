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
using PalashDynamics.ValueObjects.DashBoardVO;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmEmbryoBank : ChildWindow
    {
        public string MRNO;
        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> EmbryoList { get; private set; }
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
        public frmEmbryoBank()
        {
            InitializeComponent();
            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            PageSize = 7;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;
        }
        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
            FillEmbryoCryoBank();
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

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (EmbryoDataGrid.SelectedItem != null)
            {
                BarcodeForm win = new BarcodeForm();
                long VitrificationNo = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).EmbSerialNumber;
                win.PrintData = "EM" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select embryo.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

