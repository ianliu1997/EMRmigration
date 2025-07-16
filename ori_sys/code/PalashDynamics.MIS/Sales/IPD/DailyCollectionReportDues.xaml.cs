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
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Sales.IPD
{
    public partial class DailyCollectionReportDues : UserControl
    {
        long BillTypeID;
        public DailyCollectionReportDues(long TypeID)
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
            FillClinic();
            FillPayMode();

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
                    //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    cmbClinic.ItemsSource = objList;
                    if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                  select r;
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



        void FillPayMode()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);

            var results = from r in mlPaymode
                          where r.ID != 3 && r.ID != 6 && r.ID != 8 && r.ID != 9 && r.ID != 7
                          select r;
            cmbPayment.ItemsSource = results.ToList();
            // cmbPayMode.SelectedItem = Default;
            cmbPayment.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList, PaymentTransactionType sTransactionType)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                //if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.AdvancePayment || (PaymentTransactionType)sTransactionType == PaymentTransactionType.RefundPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance ||
                //         (MaterPayModeList)Value == MaterPayModeList.StaffFree)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.SelfPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.CompanyAdvance || (MaterPayModeList)Value == MaterPayModeList.PatientAdvance)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.CompanyPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.None)
                //{
                //    //Do Nothing
                //}
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

        public enum PaymentTransactionType
        {
            None = 0,
            SelfPayment = 1,
            CompanyPayment = 2,
            AdvancePayment = 3,
            RefundPayment = 4

        }


        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if ((MasterListItem)cmbClinic.SelectedItem == null)
                //{
                //    cmbClinic.TextBox.SetValidation("Please select clinic");
                //    cmbClinic.TextBox.RaiseValidationError();
                //    cmbClinic.Focus();

                //}
                // else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                // {
                //     cmbClinic.TextBox.SetValidation("Please select clinic");
                //     cmbClinic.TextBox.RaiseValidationError();
                //     cmbClinic.Focus();

                // }
                // else
                // {

                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;
                bool chkToDate = true;

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
                
                long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                //  long Payment = ((MasterListItem)cmbPayment.SelectedItem).ID;
                long counterID = 0;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    counterID = 0;
                else
                    counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                if (chkToDate == true)
                {
                    string URL;

                    if (dtpF != null && dtpT != null)
                    {
                        URL = "../Reports/Sales/IPD/DailySettlementReport.aspx?FromDate=" + dtpF.Value.ToString("MM/dd/yyyy") + "&ToDate=" + dtpT.Value.ToString("MM/dd/yyyy") + "&ClinicID=" + clinic + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("MM/dd/yyyy") + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else
                    {
                        URL = "../Reports/Sales/IPD/DailySettlementReport.aspx?FromDate=" + null + "&ToDate=" + null + "&ClinicID=" + clinic + "&ToDatePrint=" + null + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }


                }
                else
                {
                    string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                //}
            }
            catch (Exception)
            {

            }
        }



        private void cmdPrint_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
