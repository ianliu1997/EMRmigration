using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Media.Imaging;
using PalashDynamics.Collections;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmBedStatus : UserControl
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        public PagedSortableCollectionView<clsIPDBedStatusVO> DataList { get; private set; }
        WrapPanel pnlBedStatus = null;
        private bool IsOccupied = true;
        public long Service { get; set; }
        private bool IsUnderMaintenance = true;
        public event RoutedEventHandler OnSaveButton_Click;
        public long newBedID, newBedCategoryID, newBedUnitID;

        private long PatientID, PatientUnitID, BedID, BedCategoryId;
        private string MRNO, BedStatus;
        clsIPDBedStatusBizActionVO result;
        public frmBedStatus()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmBedStatus_Loaded);
            this.Loaded += new RoutedEventHandler(frmBedStatus_Loaded);
            btnAdmission.Visibility = Visibility.Collapsed;
        }

        public frmBedStatus(bool IsOccupied, bool IsUnderMaintenance)
        {
            this.IsOccupied = IsOccupied;
            this.IsUnderMaintenance = IsUnderMaintenance;

            InitializeComponent();
            btnAdmission.Visibility = Visibility.Visible;
            this.Loaded += new RoutedEventHandler(frmBedStatus_Loaded);
            FillBedStatus();
        }

        void frmBedStatus_Loaded(object sender, RoutedEventArgs e)
        {
            FillClass();
            btnAdmission.IsEnabled = false;
            btnReleaseBed.IsEnabled = false;
            btnTransferBed.IsEnabled = false;
            btnDetails.IsEnabled = false;
            BedID = 0;

            FillBedStatus();
        }


        #region Fill Combo

        private void FillClass()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    if (IsOccupied == false)
                    {
                        objList.Add(new MasterListItem(0, "- All -"));

                    }
                    else
                    {
                        objList.Add(new MasterListItem(0, "- Select -"));
                    }
                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbClassName.ItemsSource = null;
                    cmbClassName.ItemsSource = objList;

                    cmbClassName.SelectedValue = objList[0].ID;

                    FillFloor();

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void FillFloor()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_FloorMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    if (IsOccupied == false)
                    {
                        objList.Add(new MasterListItem(0, "- All -"));

                    }
                    else
                    {
                        objList.Add(new MasterListItem(0, "- Select -"));
                    }

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbFloor.ItemsSource = null;
                    cmbFloor.ItemsSource = objList;

                    cmbFloor.SelectedValue = objList[0].ID;

                    FillWard();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void FillWard()
        {
            try
            {
                clsGetWardByFloorBizActionVO BizAction = new clsGetWardByFloorBizActionVO();
                BizAction.WardList = new List<clsIPDBedStatusVO>();
                BizAction.Floor = new clsIPDBedStatusVO();
                if (cmbFloor.SelectedItem != null)
                    BizAction.Floor.ID = ((MasterListItem)cmbFloor.SelectedItem).ID;
                BizAction.Floor.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (cmbClassName.SelectedItem != null)
                    BizAction.Floor.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetWardByFloorBizActionVO objClass = ((clsGetWardByFloorBizActionVO)e.Result);

                        List<MasterListItem> objList = new List<MasterListItem>();
                        if (IsOccupied == false)
                        {
                            objList.Add(new MasterListItem(0, "- All -"));
                        }
                        else
                        {
                            objList.Add(new MasterListItem(0, "- Select -"));
                        }

                        foreach (var item in objClass.WardList)
                        {
                            objList.Add(new MasterListItem(item.ID, item.Description));
                        }
                        cmbWard.ItemsSource = null;
                        cmbWard.ItemsSource = objList;

                        cmbWard.SelectedValue = objList[0].ID;


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void cmbFloor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillWard();
        }

        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillWard();
        }

        #endregion


        #region Search Bed

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            btnDetails.IsEnabled = false;
            FillBedStatus();
        }

        private void FillBedStatus()
        {
            clsIPDBedStatusBizActionVO BizAction = new clsIPDBedStatusBizActionVO();
            pnlBedStatus = new WrapPanel();
            pnlBedStatus.Name = "wrppnlBedStatus";
            pnlBedStatus.Orientation = Orientation.Horizontal;

            BizAction.BedStatusList = new List<clsIPDBedStatusVO>();
            BizAction.BedStatus = new clsIPDBedStatusVO();
            if (IsOccupied == false)
            {
                BizAction.BedStatus.Occupied = IsOccupied;
            }
            else
            {
                BizAction.BedStatus.IsOccupiedBoth = true;
            }

            if (IsUnderMaintenance == false)
            {
                BizAction.BedStatus.IsUnderMaintanence = IsUnderMaintenance;
            }
            else
            {
                BizAction.BedStatus.IsUnderMaintanence = true;
            }

            BizAction.BedStatus.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (cmbWard.SelectedItem != null)
            {
                BizAction.BedStatus.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
            }
            if (cmbFloor.SelectedItem != null)
            {
                BizAction.BedStatus.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
            }
            BizAction.BedStatus.IsNonCensus = rbCensus.IsChecked == true ? "0" : (rbAll.IsChecked == true ? "" : "1");

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    result = arg.Result as clsIPDBedStatusBizActionVO;
                    DataList = new PagedSortableCollectionView<clsIPDBedStatusVO>();

                    if (result.BedStatusList != null)
                    {
                        BindBedStatusToPanel(result);
                    }

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        private void BindBedStatusToPanel(clsIPDBedStatusBizActionVO result)
        {
            lstBedStatus.Items.Clear();
            Grid grdBedStatus = new Grid();

            grdBedStatus.Height = lstBedStatus.ActualHeight;
            grdBedStatus.Width = lstBedStatus.ActualWidth;

            int rows = result.BedStatusList.Count;
            double column = Math.Sqrt(rows);
            int row = (int)Math.Sqrt(rows);


            if ((double)row < column)
                column = column + 1;
            column = (int)column;

            if ((row * column) < rows)
                row++;


            for (int gridrowDefination = 0; gridrowDefination < row; gridrowDefination++)
            {
                RowDefinition Gridrow = new RowDefinition();
                Gridrow.Height = new GridLength((double)(lstBedStatus.ActualHeight / row));
                grdBedStatus.RowDefinitions.Add(Gridrow);
            }

            for (int gridDefination = 0; gridDefination < column; gridDefination++)
            {
                ColumnDefinition Gridcolumn = new ColumnDefinition();
                Gridcolumn.Width = new GridLength((double)(lstBedStatus.ActualWidth / column));
                grdBedStatus.ColumnDefinitions.Add(Gridcolumn);
            }

            int GriRrows = 0, GriColumns = 0;

            foreach (var item in result.BedStatusList)
            {
                #region Declaration Of Local Variables and initialization of properties

                Border ImageoutLine = new Border();
                StackPanel st = new StackPanel();
                TextBlock tb = new TextBlock();

                if (BedID == item.BedID)
                {
                    ImageoutLine.BorderThickness = new Thickness(1, 1, 1, 1);
                    ImageoutLine.BorderBrush = new SolidColorBrush(Colors.Red);
                    tb.Foreground = new SolidColorBrush(Colors.Red);

                    //By Anjali............
                    newBedID = item.BedID;
                    newBedCategoryID = item.BedCategoryId;
                    newBedUnitID = item.BedUnitID;

                }
                else
                {
                    ImageoutLine.BorderThickness = new Thickness(1, 1, 1, 1);
                    ImageoutLine.BorderBrush = new SolidColorBrush(Colors.Black);
                    tb.Foreground = new SolidColorBrush(Colors.Black);
                }

                st.Orientation = Orientation.Vertical;

                tb.Text = " " + item.BedDescription;
                tb.FontSize = 9;
                tb.Height = 9.8;

                Image img = new Image();
                img.Name = Convert.ToString(item.BedID);
                img.Tag = item.PatientID + "$" + item.PatientUnitID + "$" + item.MRNO + "$";
                img.Height = ((int)(lstBedStatus.ActualHeight / row) - 10.5);
                img.Width = ((int)(lstBedStatus.ActualWidth / column) - 1);
                ImageoutLine.MouseLeftButtonDown += new MouseButtonEventHandler(ImageoutLine_MouseLeftButtonDown);

                #endregion

                #region Assign Image To Bed
                if (item.IsDischarged == true && item.IsReserved == false && item.IsSecondaryBed == true)
                {
                    img.Source = new BitmapImage(new Uri("../Icons/RelativeBed.jpg", UriKind.RelativeOrAbsolute));
                    img.Tag = Convert.ToString(img.Tag) + BedType.Relative;
                }
                else if (item.IsDischarged && !item.IsUnderMaintanence && !item.IsReserved)
                {
                    img.Source = new BitmapImage(new Uri("../Icons/released.jpg", UriKind.RelativeOrAbsolute));
                    img.Tag = Convert.ToString(img.Tag) + BedType.Released;
                }
                else if (item.IsDischarged && item.IsUnderMaintanence)
                {
                    img.Source = new BitmapImage(new Uri("../Icons/bedmaintenance.jpg", UriKind.RelativeOrAbsolute));
                    img.Tag = Convert.ToString(img.Tag) + BedType.Maintenance;
                }
                else if (!item.IsUnderMaintanence && !item.IsDischarged && !item.IsReserved)
                {
                    img.Source = new BitmapImage(new Uri("../Icons/Occupied.jpg", UriKind.RelativeOrAbsolute));
                    img.Tag = Convert.ToString(img.Tag) + BedType.Occupied;
                }

                //else if (item.IsDischarged == true && item.IsReserved == false && item.IsSecondaryBed == true)
                //{
                //    img.Source = new BitmapImage(new Uri("../Icons/RelativeBed.jpg", UriKind.RelativeOrAbsolute));
                //    img.Tag = Convert.ToString(img.Tag) + BedType.Discharge;
                //}
                else if (item.IsReserved)
                {
                    img.Source = new BitmapImage(new Uri("../Icons/Reserved.jpg", UriKind.RelativeOrAbsolute));
                    img.Tag = Convert.ToString(img.Tag) + BedType.Occupied;
                }
                else
                {
                    img.Tag = Convert.ToString(img.Tag) + BedType.Released;
                }

                img.Tag = Convert.ToString(img.Tag) + "$" + item.BedCategoryId;


                #endregion

                #region Add Images to Grid
                st.Children.Add(tb);
                st.Children.Add(img);

                ImageoutLine.Child = st;
                ImageoutLine.Height = ((int)(lstBedStatus.ActualHeight / row) - 1);
                ImageoutLine.Width = ((int)(lstBedStatus.ActualWidth / column) - 1);

                ImageoutLine.SetValue(Grid.RowProperty, GriRrows);
                ImageoutLine.SetValue(Grid.ColumnProperty, GriColumns);

                ImageoutLine.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                ImageoutLine.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Center);

                grdBedStatus.Children.Add(ImageoutLine);

                #endregion

                #region Change Column and row for Grid
                GriColumns++;
                if (GriColumns == column)
                {
                    GriRrows++;
                    GriColumns = 0;
                }
                #endregion
            }
            lstBedStatus.Items.Add(grdBedStatus);

            if (BedID == 0)
                CommandButtonForBedStatus("Search");
        }

        void ImageoutLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border imageOutline = (Border)sender;
            StackPanel st = (StackPanel)imageOutline.Child;
            Image img = (Image)st.Children[1];

            BedID = Convert.ToInt64(img.Name);
            string tag = img.Tag.ToString();
            string[] Paramaters = tag.Split('$');

            PatientID = Convert.ToInt64(Paramaters[0]);
            PatientUnitID = Convert.ToInt64(Paramaters[1]);
            MRNO = Paramaters[2];
            BedCategoryId = Convert.ToInt64(Paramaters[4]);

            BedStatus = Paramaters[3];
            btnDetails.IsEnabled = true;

            BindBedStatusToPanel(result);
            CommandButtonForBedStatus(Paramaters[3]);
        }


        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            BedID = Convert.ToInt64(img.Name);

            string tag = img.Tag.ToString();
            string[] Paramaters = tag.Split('$');

            PatientID = Convert.ToInt64(Paramaters[0]);
            PatientUnitID = Convert.ToInt64(Paramaters[1]);
            MRNO = Paramaters[2];
            BedCategoryId = Convert.ToInt64(Paramaters[4]);
            CommandButtonForBedStatus(Paramaters[3]);
            BedStatus = Paramaters[3];
            btnDetails.IsEnabled = true;
        }


        #endregion



        private void btnTransferBed_Click(object sender, RoutedEventArgs e)
        {
            //frmBedTransfer myPage = new frmBedTransfer(PatientID, PatientUnitID, MRNO, BedID);
            //myPage.FillDataFromBedStatusPage();
            //UIElement myData = myPage;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement.Text = "Bed Transfer";
            //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void btnAdmission_Click(object sender, RoutedEventArgs e)
        {



            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
            //ChildWindow BedStatus = new ChildWindow();
            //AdmissionDetailsWin admn = new AdmissionDetailsWin();
            //BedStatus.Content = admn;
            //BedStatus.Show();
            //frmPatientAdmission objAdm = new frmPatientAdmission(BedCategoryId, BedID);
            //UIElement myData = objAdm;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement.Text = "Admission";
            //((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            //AdmissionDetailsWin admn = new AdmissionDetailsWin(newBedCategoryID, newBedID, newBedUnitID);

        }

        private void btnReleaseBed_Click(object sender, RoutedEventArgs e)
        {
            //frmBedReleased objAdm = new frmBedReleased(BedID);
            //UIElement myData = objAdm;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement.Text = "Bed Release";
            //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void CommandButtonForBedStatus(string Status)
        {
            switch (Status)
            {
                case "Discharge":
                    btnAdmission.IsEnabled = false;
                    btnReleaseBed.IsEnabled = true;
                    btnTransferBed.IsEnabled = false;
                    break;

                case "Occupied":
                    btnAdmission.IsEnabled = false;
                    btnReleaseBed.IsEnabled = false;
                    btnTransferBed.IsEnabled = true;
                    break;

                case "Relative":
                    btnAdmission.IsEnabled = false;
                    btnReleaseBed.IsEnabled = false;
                    btnTransferBed.IsEnabled = true;
                    break;

                case "Maintenance":
                    btnAdmission.IsEnabled = false;
                    btnReleaseBed.IsEnabled = false;
                    btnTransferBed.IsEnabled = false;
                    break;

                case "Released":
                    btnAdmission.IsEnabled = true;
                    btnReleaseBed.IsEnabled = false;
                    btnTransferBed.IsEnabled = false;
                    break;

                case "Search":
                    btnAdmission.IsEnabled = false;
                    btnReleaseBed.IsEnabled = false;
                    btnTransferBed.IsEnabled = false;
                    break;
                default:
                    break;
            }

        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            FrmBedDetails winBedDetails = new FrmBedDetails();//newBedCategoryID, newBedID, newBedUnitID);
            winBedDetails.BedID = newBedID;
            winBedDetails.BedUnitID = newBedUnitID;
            winBedDetails.Show();
        }
    }
    public enum BedType
    {
        None = 0,
        Discharge,
        Occupied,
        Relative,
        Maintenance,
        Released,
    }

}
