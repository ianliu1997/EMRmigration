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
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Text;

namespace PalashDynamics.MIS.Sales.IPD
{
    public partial class BalanceAmoutReport : UserControl
    {


        long BillTypeID;
        public BalanceAmoutReport(long TypeID)
        {
            InitializeComponent();
            BillTypeID = TypeID;
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbClinic.IsEnabled = false;


                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,

                    DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID

                };

            }
            else
            {
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,

                    DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,

                };
            }

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }

        public string SendClinicID = string.Empty;

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;
                bool chkToDate = true;
                bool IsExporttoExcel = false;
                

                string msgTitle = "";
                if (dtpFromDate.SelectedDate != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                    if (dtpF.Value > dtpT.Value)
                    {
                        dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                        dtpT = dtpF;
                        chkToDate = false;
                    }
                    else
                    {
                        dtpP = dtpT;
                        dtpT = dtpT.Value.AddDays(1);
                        dtpToDate.Focus();
                    }
                }

                if (dtpT != null)
                {
                    if (dtpF != null)
                    {
                        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                    }
                }

                long clinic = 0;
                long CreditGivenBy = 0;
                long SourceID = 0;
                long counterID = 0;

                GetClinicID();

                if ((MasterListItem)cmbClinic.SelectedItem != null)
                {
                    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                if ((MasterListItem)cmbCredit.SelectedItem != null)
                {
                    CreditGivenBy = ((MasterListItem)cmbCredit.SelectedItem).ID;
                }
                if ((MasterListItem)cmbTariff.SelectedItem != null)
                {
                    SourceID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    counterID = 0;
                else
                    counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (chkToDate == true)
                {
                    //Begin :: Added by AniketK on 25Feb2019 for normal report                      
                    if (chkCurrentDate.IsChecked == true)
                    {
                        string URL;

                        URL = "../Reports/Sales/IPD/BalanceAmoutReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&CreditGivenBy=" + CreditGivenBy + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("dd-MMM-yyyy") + "&BillTypeID=" + BillTypeID + "&SorceID=" + SourceID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID + "&IsOutstandingReport=" + 0;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    //Added by AniketK on 25Feb2019 for outstanding report
                    else if (chkAsOnDate.IsChecked == true)
                    {
                        string URL;
                        URL = "../Reports/Sales/IPD/BalanceAmoutReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&CreditGivenBy=" + CreditGivenBy + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("dd-MMM-yyyy") + "&BillTypeID=" + BillTypeID + "&SorceID=" + SourceID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID + "&IsOutstandingReport=" + 1;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    //End :: Added by AniketK on 25Feb2019
                }
                else
                {
                    string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cmdPrint_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            if (BillTypeID > 0)
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.IPD.IPDSalesReport") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            chkCurrentDate.IsChecked = true; //Added by AniketK on 19Feb2019
            FillClinic();
            FillPatientCategory();
            //FillTariff();
            // FillConcessionAutBy(((MasterListItem)cmbClinic.SelectedItem).ID);

        }

        private void FillPatientCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objList[0];

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillTariff()
        {
            clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ValidPatientMasterSourceList = true;
            BizAction.ID = 0;
            //BizAction.IsActive = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetPatientSourceListBizActionVO)e.Result).MasterList);
                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }


        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                  select r;
                        ((MasterListItem)res.First()).Status = true;
                        cmbClinic.SelectedItem = ((MasterListItem)res.First());
                        cmbClinic.IsEnabled = false;
                    }
                    else
                        cmbClinic.SelectedItem = objList[0];

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        //private void FillConcessionAutBy(long iUnitID)
        //{
        //    clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
        //    if ((MasterListItem)cmbClinic.SelectedItem != null)
        //    {
        //        BizAction.UnitID = iUnitID;
        //    }
        //    else
        //    {
        //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    }

        //    BizAction.StaffMasterList = new List<clsStaffMasterVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {

        //            if (((clsGetStaffMasterDetailsBizActionVO)e.Result).StaffMasterList != null)
        //            {
        //                clsGetStaffMasterDetailsBizActionVO result = e.Result as clsGetStaffMasterDetailsBizActionVO;

        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "- Select -"));
        //                foreach (var item in result.StaffMasterList)
        //                {
        //                    MasterListItem Obj = new MasterListItem();
        //                    Obj.ID = item.ID;
        //                    Obj.Description = (item.FirstName + " " + item.MiddleName + " " + item.LastName);
        //                    Obj.Status = item.Status;
        //                    objList.Add(Obj);
        //                }

        //                var myNewList = objList.OrderBy(i => i.Description);

        //                cmbCredit.ItemsSource = null;
        //                cmbCredit.ItemsSource = myNewList.ToList();
        //                cmbCredit.SelectedItem = myNewList.ToList()[0];
        //            }
        //        }


        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void FillConcessionAutBy(string SendClinicID)
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            //if ((MasterListItem)cmbClinic.SelectedItem != null)
            //{
            //    BizAction.UnitID = iUnitID;
            //}
            //else
            //{
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            BizAction.StrClinicID = SendClinicID;
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    if (((clsGetStaffMasterDetailsBizActionVO)e.Result).StaffMasterList != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO result = e.Result as clsGetStaffMasterDetailsBizActionVO;

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        foreach (var item in result.StaffMasterList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = item.ID;
                            Obj.Description = (item.FirstName + " " + item.MiddleName + " " + item.LastName);
                            Obj.Status = item.Status;
                            objList.Add(Obj);
                        }

                        var myNewList = objList.OrderBy(i => i.Description);

                        cmbCredit.ItemsSource = null;
                        cmbCredit.ItemsSource = myNewList.ToList();
                        cmbCredit.SelectedItem = myNewList.ToList()[0];
                    }
                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    //long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    //FillConcessionAutBy(clinicId);
                    GetClinicID();
                    FillConcessionAutBy(SendClinicID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {
            //cmbCredit_GotFocus(sender, e);
        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {
            //cmbCredit_GotFocus(sender, e);
        }

        private void cmbCredit_GotFocus(object sender, RoutedEventArgs e)
        {
            GetClinicID();
            FillConcessionAutBy(SendClinicID);
        }

        public void GetClinicID()
        {
            List<MasterListItem> clinicList = new List<MasterListItem>();
            List<MasterListItem> selectedClinicList = new List<MasterListItem>();


            clinicList = (List<MasterListItem>)cmbClinic.ItemsSource;
            if (clinicList.Count > 0)
            {
                foreach (var item in clinicList)
                {
                    if (item.Status == true)
                    {
                        selectedClinicList.Add(item);
                    }
                }
            }
            long clinicID = 0;
            StringBuilder builder = new StringBuilder();
            foreach (var item in selectedClinicList)
            {
                clinicID = item.ID;

                builder.Append(clinicID).Append(",");

            }

            SendClinicID = builder.ToString();

            if (SendClinicID.Length != 0)
            {
                SendClinicID = SendClinicID.TrimEnd(',');
            }
        }

        private void cmbCredit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //GetClinicID();
            //FillConcessionAutBy(SendClinicID);
        }

        private void cmbCredit_MouseEnter(object sender, MouseEventArgs e)
        {
            GetClinicID();
            FillConcessionAutBy(SendClinicID);
        }


        //Begin:: Added by AniketK on 19Feb2019
        private void chkCurrentDateRadioButton_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void chkAsOnDateRadioButton_Click(object sender, RoutedEventArgs e)
        {       
       
        }
        //End:: Added by AniketK on 19Feb2019
    }
}
