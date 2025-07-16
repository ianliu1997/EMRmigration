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

using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Data;
using System.Windows.Browser;
using PalashDynamics.MIS.Administration;


namespace PalashDynamics.MIS.Masters
{
    public partial class frmRptMaster : UserControl
    {
        //commented by akshays
        //#region Public Properties
        //public List<MasterListItem> DataList { get; private set; }
        //public List<MasterListItem> MasterDataList { get; private set; }
        //#endregion
        //#region Constructor
        //public frmRptMaster()
        //{
        //    InitializeComponent();
        //}
        //#endregion

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    DataList = new List<MasterListItem>();
        //    MasterDataList = new List<MasterListItem>();
        //    fillMaster();
        //    chkActive.IsChecked = true;
        //    dgPrintMasterList.ItemsSource = null;
        //}

        //private void DisplayButton_Click(object sender, RoutedEventArgs e)
        //{
        //    BtnPrint.IsEnabled = true;
        // PalashDynamics.ValueObjects.Master.clsGetDatatoPrintMasterBizActionVO BizAction = new PalashDynamics.ValueObjects.Master.clsGetDatatoPrintMasterBizActionVO();
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.id = ((MasterListItem)cmbMaster.SelectedItem).ID;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, ea) =>
        //    {
        //        if (ea.Error == null && ea.Result != null)
        //        {
        //            PalashDynamics.ValueObjects.Master.clsGetDatatoPrintMasterBizActionVO result = ea.Result as PalashDynamics.ValueObjects.Master.clsGetDatatoPrintMasterBizActionVO;
        //            MasterDataList.Clear();
        //            MasterDataList.AddRange(result.MasterList);
        //            dgPrintMasterList.ItemsSource = null;
        //            dgPrintMasterList.ItemsSource = MasterDataList;
        //            if (dgPrintMasterList.Columns.Count > 0)
        //                dgPrintMasterList.Columns.Clear();

        //            if (BizAction.id == 10) // For the User MAster
        //            {
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Login Name",
        //                    Binding = new Binding("Code")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "User Name",
        //                    Binding = new Binding("Description")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "User Type",
        //                    Binding = new Binding("column0")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "User Role",
        //                    Binding = new Binding("column1")
        //                });
        //            }
        //            else if (BizAction.id == 9) // For the Doctor MAster
        //            {
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Education",
        //                    Binding = new Binding("Code")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "DoctorName Name",
        //                    Binding = new Binding("Description")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Age",
        //                    Binding = new Binding("PurchaseRate")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Doctor Type",
        //                    Binding = new Binding("column0")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Date Of Joining",
        //                    Binding = new Binding("column1")
        //                });
        //            }
        //            else
        //            {
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Code",
        //                    Binding = new Binding("Code")
        //                });
        //                dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                {
        //                    Header = "Description",
        //                    Binding = new Binding("Description")
        //                });

        //                if (MasterDataList.Where(z => z.PurchaseRate != 0).Any())
        //                {
        //                    dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                    {
        //                        Header = "Rate",
        //                        Binding = new Binding("PurchaseRate")
        //                    });
        //                }
        //                if (MasterDataList.Where(z => z.column0 != "").Any())
        //                {
        //                    if (BizAction.id == 3 || BizAction.id == 2)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "Specialization",
        //                            Binding = new Binding("column0")
        //                        });
        //                    else if (BizAction.id == 4)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "Brand Name",
        //                            Binding = new Binding("column0")
        //                        });
        //                    else
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "Column0",
        //                            Binding = new Binding("column0")
        //                        });
        //                }
        //                if (MasterDataList.Where(z => z.column1 != "").Any())
        //                {
        //                    if (BizAction.id == 3)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "SubSpecialization",
        //                            Binding = new Binding("column1")
        //                        });
        //                    else if (BizAction.id == 4)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "Item Group",
        //                            Binding = new Binding("column1")
        //                        });
        //                    else
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "column1",
        //                            Binding = new Binding("column1")
        //                        });

        //                }
        //                if (MasterDataList.Where(z => z.column2 != "").Any())
        //                {
        //                    if (BizAction.id == 3)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "TariffName",
        //                            Binding = new Binding("column2")
        //                        });
        //                    else if (BizAction.id == 4)
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "Item Category",
        //                            Binding = new Binding("column2")
        //                        });
        //                    else
        //                        dgPrintMasterList.Columns.Add(new DataGridTextColumn
        //                        {
        //                            Header = "column2",
        //                            Binding = new Binding("column2")
        //                        });
        //                }
        //            }
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        
        //}

        //private void BtnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cmbMaster.SelectedItem != null && ((MasterListItem)cmbMaster.SelectedItem).ID > 0)
        //    {
        //        string URL = "../Reports/Administrator/MasterPrint.aspx?id=" + ((MasterListItem)cmbMaster.SelectedItem).ID;
        //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //    }
        //}
        //#region FillComboBox
        //private void fillMaster()
        //{
        //    PalashDynamics.ValueObjects.Master.clsGetMasterNamesBizActionVO BizAction = new PalashDynamics.ValueObjects.Master.clsGetMasterNamesBizActionVO();
        //    BizAction.objPrintMasterList = new List<PalashDynamics.ValueObjects.Master.clsPrintMasterVO>();
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            PalashDynamics.ValueObjects.Master.clsGetMasterNamesBizActionVO result = e.Result as PalashDynamics.ValueObjects.Master.clsGetMasterNamesBizActionVO;
        //            DataList.Clear();
        //            DataList.Add(new MasterListItem(0, "", "--Select--", true));
        //            DataList.AddRange(result.MasterList);
        //            cmbMaster.ItemsSource = null;
        //            cmbMaster.ItemsSource = DataList;
        //            cmbMaster.SelectedItem = DataList[0];
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //#endregion

        //private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void hlDoctorMasterReport_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void hlStaffMasterReport_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void hlUserMaster_Click(object sender, RoutedEventArgs e)
        //{

        //}
        //closed by akshays

        public frmRptMaster()
        {
            InitializeComponent();
        }

        private void hlDoctorMasterReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DaoctorMasterReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlStaffMasterReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new StaffMasterReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlUserMaster_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new UserMasterReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPatientRegistartion_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RegistrationReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlItemMaster_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemMasterReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlServiceMaster_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ServiceMasterReportX();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void tariffmasterservice_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new TariffServiceClassRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
