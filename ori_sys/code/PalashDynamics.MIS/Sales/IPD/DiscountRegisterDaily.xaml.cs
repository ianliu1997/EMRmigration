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
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Billing;
using System.Windows.Browser;
using System.Text;

namespace PalashDynamics.MIS.Sales.IPD
{
    public partial class DiscountRegisterDaily : UserControl
    {
        #region Variable Declaration
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        bool aVisitMark { get; set; }
        bool Status { get; set; }

        public string Email { get; set; }
        public DateTime Fdt { get; set; }
        public DateTime Tdt { get; set; }

        // public clsGetAppointmentBizActionVO BizAction { get; set; }




        public bool ChkUnit { get; set; }


        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;

        #endregion


        long BillTypeID;
        public DiscountRegisterDaily(long TypeID)
        {
            BillTypeID = TypeID;
            InitializeComponent();

            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbClinic.IsEnabled = false;
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                };
            }
            else
            {
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                };
            }

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;


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
            FillClinic();
            FillCompany();
            FillDepartmentList();
            FillPatientSource();
            FillGrossDiscount();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

        }

        private void FillPatientCategory()
        {
            

        }


        private void FillPatientSource()
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

                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList;
                    cmbPatientSource.SelectedItem = objList[0];

                    //if (this.DataContext != null)
                    //{
                    //    //cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                    //    cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientCategoryID;

                    //    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);

                    //    if (((clsPatientSponsorVO)this.DataContext).PatientCategoryID == 0)
                    //    {
                    //        cmbPatientCategory.TextBox.SetValidation("Please select the Patient Category");
                    //        cmbPatientCategory.TextBox.RaiseValidationError();
                    //    }
                    //}
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

            //clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
            ////BizAction.IsActive = true;
            //BizAction.ValidPatientMasterSourceList = true;
            //BizAction.ID = 0;
            ////BizAction.IsActive = true;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetPatientSourceListBizActionVO)e.Result).MasterList);
            //        cmbPatientSource.ItemsSource = null;
            //        cmbPatientSource.ItemsSource = objList;
            //        cmbPatientSource.SelectedItem = objList[0];
            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        public clsAppointmentVO item = new clsAppointmentVO();

        private void FillCompany()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbCompany.ItemsSource = null;
                        cmbCompany.ItemsSource = objList;

                        cmbCompany.SelectedItem = objList[0];

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

        private void FillDepartmentList()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";

            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];

                }


            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void FillGrossDiscount()
        {
            clsFillGrossDiscountReasonBizActionVO BizAction = new clsFillGrossDiscountReasonBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsFillGrossDiscountReasonBizActionVO)arg.Result).MasterList);

                    cmbGrossDiscountReason.ItemsSource = null;
                    cmbGrossDiscountReason.ItemsSource = objList;
                    cmbGrossDiscountReason.SelectedItem = objList[0];

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

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
                    //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
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

        //private void FillDoctorList(long iUnitID)
        //{
        //    clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if ((MasterListItem)cmbClinic.SelectedItem != null)
        //    {
        //        BizAction.UnitId = iUnitID;
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

        //            var myNewList = objList.OrderBy(i => i.Description);
        //            cmbDoctor.ItemsSource = null;
        //            cmbDoctor.ItemsSource = myNewList.ToList();


        //            if (this.DataContext != null)
        //            {


        //                if (UseApplicationDoctorID == true)
        //                {
        //                    cmbDoctor.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
        //                    UseApplicationDoctorID = false;
        //                }

        //                else
        //                    cmbDoctor.SelectedValue = myNewList.ToList()[0].ID;


        //                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
        //                {
        //                    cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
        //                }
        //            }
        //        }
        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        private void FillDoctorList(string SendClinicID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            //if ((MasterListItem)cmbClinic.SelectedItem != null)
            //{
            //    BizAction.UnitId = iUnitID;
            //}

            BizAction.StrClinicID = SendClinicID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

                    var myNewList = objList.OrderBy(i => i.Description);
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = myNewList.ToList();


                    if (this.DataContext != null)
                    {

                        if (UseApplicationDoctorID == true)
                        {
                            cmbDoctor.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }

                        else
                            cmbDoctor.SelectedValue = myNewList.ToList()[0].ID;


                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }
                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        /// <summary>
        /// Method Purpose: Search Appointment by diff search criteria. 
        /// </summary>

        public bool UnRegistered;
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

                GetClinicID();

                long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                long Department = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                long Doctor = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                long Company = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                long GrossDiscountID = ((MasterListItem)cmbGrossDiscountReason.SelectedItem).ID;
                long counterID = 0;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    counterID = 0;
                else
                    counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (chkToDate == true)
                {
                    string URL;

                    URL = "../Reports/Sales/IPD/DiscountRegisterDaily.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&Excel=" + chkExcel.IsChecked + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&DepartmentID=" + Department + "&Doctor=" + Doctor + "&Company=" + Company + "&ToDatePrint=" + dtpP.Value.ToString("dd-MMM-yyyy") + "&GrossDiscountID=" + GrossDiscountID + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");



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


        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null)
            {
                //FillDoctorList(((MasterListItem)cmbClinic.SelectedItem).ID);
                GetClinicID();
                FillDoctorList(SendClinicID);
            }
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cmbDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            //GetClinicID();
            //FillDoctorList(SendClinicID);
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

        private void cmbDoctor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //GetClinicID();
            //FillDoctorList(SendClinicID);
        }

        private void cmbDoctor_MouseEnter(object sender, MouseEventArgs e)
        {
            GetClinicID();
            FillDoctorList(SendClinicID);
        }

    }
}
