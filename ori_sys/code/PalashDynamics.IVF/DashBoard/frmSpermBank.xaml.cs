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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmSpermBank : ChildWindow
    {
        public string MRNO;
        public PagedSortableCollectionView<clsSpermFreezingVO> SpermCollectionList { get; private set; }

        public int SpermListPageSize
        {
            get
            {
                return SpermCollectionList.PageSize;
            }
            set
            {
                if (value == SpermCollectionList.PageSize) return;
                SpermCollectionList.PageSize = value;
            }
        }
        public frmSpermBank()
        {
            InitializeComponent();
            SpermCollectionList = new PagedSortableCollectionView<clsSpermFreezingVO>();
            SpermCollectionList.OnRefresh += new EventHandler<RefreshEventArgs>(SpermCollection_OnRefresh);
            SpermListPageSize = 7;
            SpermCollectionList.PageSize = SpermListPageSize;
            SpermsDataPager.DataContext = SpermCollectionList;

        }
        void SpermCollection_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSpermCollectionCryoBank();
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

        Color myRgbColor;
        Color myRgbColor1;
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillSpermCollectionCryoBank();

            myRgbColor1 = new Color();
            myRgbColor1.A = Convert.ToByte(120);
            myRgbColor1.R = Convert.ToByte(240);
            myRgbColor1.G = Convert.ToByte(230);
            myRgbColor1.B = Convert.ToByte(140);

            myRgbColor = new Color();
            myRgbColor.A = Convert.ToByte(120);
            myRgbColor.R = Convert.ToByte(255);
            myRgbColor.G = Convert.ToByte(153);
            myRgbColor.B = Convert.ToByte(51);

            SolidColorBrush brush = new SolidColorBrush(myRgbColor1);
            SolidColorBrush brush1 = new SolidColorBrush(myRgbColor);

            lblExpired.Background = brush1;
            lblNearingExpired.Background = brush;
        }

        private void FillSpermCollectionCryoBank()
        {
            try
            {
                clsGetSpremFreezingDetailsBizActionVO BizAction = new clsGetSpremFreezingDetailsBizActionVO();
                // BizAction.SearchExpression = txtSearch.Text;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = SpermCollectionList.PageSize;
                BizAction.StartRowIndex = SpermCollectionList.PageIndex * SpermCollectionList.PageSize;
                BizAction.Vitrification = new List<clsSpermFreezingVO>();
                BizAction.MRNo = MRNO;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        SpermsDataGrid.ItemsSource = null;
                        SpermsDataPager.DataContext = null;
                        BizAction.Vitrification = (((clsGetSpremFreezingDetailsBizActionVO)arg.Result).Vitrification);
                        if (BizAction.Vitrification.Count > 0)
                        {
                            SpermCollectionList.Clear();
                            SpermCollectionList.TotalItemCount = Convert.ToInt16(((clsGetSpremFreezingDetailsBizActionVO)arg.Result).TotalRows);
                            foreach (clsSpermFreezingVO item in BizAction.Vitrification)
                            {
                                if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                {
                                    item.ExpiryDate = item.ExpiryDate;
                                }
                                else
                                    item.ExpiryDate = null;
                                if (item.ShortTerm == true)
                                    item.Type = "Short Term";
                                if (item.LongTerm == true)
                                    item.Type = "Long Term";
                                SpermCollectionList.Add(item);
                            }
                            SpermsDataGrid.ItemsSource = SpermCollectionList;
                            SpermsDataPager.DataContext = SpermCollectionList;
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



        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdPrintSpermBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (SpermsDataGrid.SelectedItem != null)
            {
                //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
                //{
                BarcodeForm win = new BarcodeForm();

                long VitrificationNo = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).SpremNo;

                win.PrintData = "SP" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select sperm.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
        }

        private void SpermsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsSpermFreezingVO item = (clsSpermFreezingVO)e.Row.DataContext;
            DateTime currentDate = DateTime.Now;
            DateTime NextOneMonthDate = currentDate.AddMonths(1);
            if (item.ExpiryDate != null)
            {
                DateTime NextOneMonthDateExpiry = item.ExpiryDate.Value.AddMonths(1);

                if (item.ExpiryDate >= currentDate && item.ExpiryDate <= NextOneMonthDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor1);

                if (item.ExpiryDate < currentDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor);
            }
        }
    }
}

