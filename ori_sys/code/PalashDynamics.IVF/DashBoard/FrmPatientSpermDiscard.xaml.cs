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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmPatientSpermDiscard : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public long PatientID;
        public long PatientUnitID;
        public bool IsSperm;
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

        public FrmPatientSpermDiscard()
        {
            InitializeComponent();
            SpermCollectionList = new PagedSortableCollectionView<clsSpermFreezingVO>();
            SpermCollectionList.OnRefresh += new EventHandler<RefreshEventArgs>(SpermCollection_OnRefresh);
            SpermListPageSize = 15;
            SpermCollectionList.PageSize = SpermListPageSize;
            SpermsDataPager.DataContext = SpermCollectionList;
        }

        void SpermCollection_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSpermCollectionCryoBank();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

                //if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                //    BizAction.FName = txtFirstName.Text.Trim();
                //if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                //    BizAction.MName = txtMiddleName.Text.Trim();
                //if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                //    BizAction.LName = txtLastName.Text.Trim();
                //if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                //    BizAction.FamilyName = txtFamilyName.Text.Trim();
                //if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
                //    BizAction.MRNo = txtMrno.Text.Trim();
                //if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
                //    BizAction.DonorCode = txtDonorCode.Text.Trim();
                // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
                BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.PatientID = PatientID;
                BizAction.PatientUnitID = PatientUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        SpermsDataGrid.ItemsSource = null;
                        SpermsDataGrid.DataContext = null;
                        //SpermsDataPager.DataContext = null;
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
                            SpermsDataGrid.DataContext = SpermCollectionList;
                            //SpermsDataPager.DataContext = SpermCollectionList;
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

        private void cmdSaveOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (SpermCollectionList.Count > 0)
            {
                var item1 = SpermCollectionList.All(x => x.IsDiscard == false);
                if (item1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Embryo.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }           
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsGetSpremFreezingDetailsBizActionVO BizAction = new clsGetSpremFreezingDetailsBizActionVO();
                BizAction.Vitrification = new List<clsSpermFreezingVO>();
                BizAction.Vitrification = SpermDiscardList;
                BizAction.IsDiscard = true;
               
                for (int i = 0; i < SpermDiscardList.Count; i++)
                {
                    //if (SpermDiscardList[i].IsDiscard == true)
                        SpermDiscardList[i].IsDiscard = true;
                    
                }               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }


        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public List<clsSpermFreezingVO> SpermDiscardList = new List<clsSpermFreezingVO>();
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            clsSpermFreezingVO obj;
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < SpermCollectionList.Count; i++)
            {
                if (i == SpermsDataGrid.SelectedIndex)
                {
                    obj = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem);
                    if (chk.IsChecked == true)
                    {
                        obj.IsDiscard = true;
                        SpermDiscardList.Add(obj);
                    }
                    else if (chk.IsChecked == false)
                    {
                        obj.IsDiscard = false;
                        SpermDiscardList.Remove(obj);
                    }
                }

            }
        }
    }
}

