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
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmEmbryoBankNew : ChildWindow
    {
        public string MRNO;
        public frmEmbryoBankNew()
        {
            InitializeComponent();
            // Added By neena For Oocyte 
            OocyteList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            OocyteList.OnRefresh += new EventHandler<RefreshEventArgs>(OocyteList_OnRefresh);
            OocytePageSize = 15;
            OocyteList.PageSize = OocytePageSize;
            OocyteDataPager.DataContext = OocyteList;
            //END 

            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            PageSize = 15;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;
        }



        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        void OocyteList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOocyteCryoBank();
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

        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> OocyteList { get; private set; }
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

        public int OocytePageSize
        {
            get
            {
                return OocyteList.PageSize;
            }
            set
            {
                if (value == OocyteList.PageSize) return;
                OocyteList.PageSize = value;
            }
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
                        EmbryoDataGrid.DataContext = null;
                        //EmbryoDataPager.DataContext = null;
                        BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                        BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                        if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                        {
                            EmbryoList.Clear();
                            EmbryoList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                            foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                            {
                                if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                {
                                    item.ExpiryDate = item.ExpiryDate;
                                    //if (item.ExpiryTime != null)
                                    //    item.ExpiryDate = item.ExpiryDate.Value.Add(item.ExpiryTime.Value.TimeOfDay);
                                }
                                else
                                    item.ExpiryDate = null;
                                if (item.ShortTerm == true)
                                    item.Type = "Short Term";
                                if (item.LongTerm == true)
                                    item.Type = "Long Term";

                                if (item.IsFreshEmbryoPGDPGS || item.IsFrozenEmbryoPGDPGS)
                                    item.PGDPGS = "PGDPGS";

                                EmbryoList.Add(item);
                            }
                            EmbryoDataGrid.ItemsSource = EmbryoList;
                            EmbryoDataGrid.DataContext = EmbryoList;
                            //EmbryoDataPager.DataContext = EmbryoList;
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

        List<MasterListItem> mlSourceOfSperm = null;
        private void fillOocyteMaturity()
        {
            mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteMaturity), mlSourceOfSperm);
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {

                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        private void FillOocyteCryoBank()   // For IVF ADM Changes
        {
            try
            {
                fillOocyteMaturity();
                cls_IVFDashboard_GetVitrificationDetailsForCryoBank BizAction = new cls_IVFDashboard_GetVitrificationDetailsForCryoBank();             
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = OocyteList.PageSize;
                BizAction.StartRowIndex = OocyteList.PageIndex * OocyteList.PageSize;
                BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();           
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.MRNo = MRNO;              
                BizAction.IsFreezeOocytes = true;   

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        OocyteDataGrid.ItemsSource = null;
                        OocyteDataGrid.DataContext = null;
                        BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                        BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                        if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                        {
                            OocyteList.Clear();
                            OocyteList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                            foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                            {
                                //oocyte grade  

                                if ((item.GradeID) > 0)
                                {
                                    item.Grade = mlSourceOfSperm.FirstOrDefault(p => p.ID == item.GradeID).Description;
                                }
                                //
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
                                OocyteList.Add(item);
                            }
                            OocyteDataGrid.ItemsSource = OocyteList;
                            OocyteDataGrid.DataContext = OocyteList;
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

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        Color myRgbColor;
        Color myRgbColor1;
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillOocyteCryoBank();
            FillEmbryoCryoBank();

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

        private void CryoBankInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Oocytetab != null && Oocytetab.IsSelected)
            {
                CmdPrintBarcodeOocyte.Visibility = Visibility.Visible;
                CmdPrintBarcode.Visibility = Visibility.Collapsed;
            }
            else if (Embryotab != null && Embryotab.IsSelected)
            {
                CmdPrintBarcodeOocyte.Visibility = Visibility.Collapsed;
                CmdPrintBarcode.Visibility = Visibility.Visible;
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
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


        private void OocyteDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO item = (clsIVFDashBoard_VitrificationDetailsVO)e.Row.DataContext;
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

        private void EmbryoDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO item = (clsIVFDashBoard_VitrificationDetailsVO)e.Row.DataContext;
            DateTime currentDate = DateTime.Now;
            DateTime NextOneMonthDate = currentDate.AddMonths(1);
            if (item.ExpiryDate != null)
            {
                //item.ExpiryDate = item.ExpiryDate.Value.Add(item.ExpiryTime.Value.TimeOfDay);
                DateTime NextOneMonthDateExpiry = item.ExpiryDate.Value.AddMonths(1);

                if (item.ExpiryDate >= currentDate && item.ExpiryDate <= NextOneMonthDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor1);

                if (item.ExpiryDate < currentDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor);
            }

        }

        private void CmdPrintBarcodeOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (OocyteDataGrid.SelectedItem != null)
            {
                BarcodeForm win = new BarcodeForm();
                long VitrificationNo = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).EmbSerialNumber;
                win.PrintData = "OC" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select oocyte.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
        }
    }
}

