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
using System.Text;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.MIS.Sales.IPD
{
    public partial class DailyCollectionReportIPD : UserControl
    {
        long BillTypeID;
        List<MasterListItem> objList = new List<MasterListItem>();
        clsUserRightsVO objUser;
        long UserID;
        public DailyCollectionReportIPD(long TypeID)
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
            GetUserRights();
            FillClinic();
            FillPayMode();
            FillCollectionMode();

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

        void FillCollectionMode()
        {
            List<MasterListItem> listCollectionMode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            listCollectionMode.Insert(0, Default);
            MasterListItem item1 = new MasterListItem(1, "Advance");
            MasterListItem item2 = new MasterListItem(2, "Patient Invoice");
            MasterListItem item3 = new MasterListItem(3, "Company Invoice");
            listCollectionMode.Insert(1, item1);
            listCollectionMode.Insert(2, item2);
            listCollectionMode.Insert(3, item3);
            cmbCollectionType.ItemsSource = null;
            cmbCollectionType.ItemsSource = listCollectionMode;
            cmbCollectionType.SelectedItem = listCollectionMode[0];
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

        public string SendClinicID = string.Empty;
        public string ClinicID = string.Empty;

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null) //added by ajit date 7/10/2016
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


                    //long clinic = 0;
                    //if (((MasterListItem)cmbClinic.SelectedItem).ID > 0 && ((MasterListItem)cmbClinic.SelectedItem).Status == true)
                    //{
                    //    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    //}

                    long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    //if(clinic>0 )
                    //{
                    //    if (SendClinicID.Length == 0)
                    //    {
                    //        SendClinicID = clinic.ToString();
                    //    }
                    //    else if(SendClinicID.Length > 0)
                    //    {
                    //        SendClinicID = SendClinicID;
                    //    }
                    //}



                    // long Payment = ((MasterListItem)cmbPayment.SelectedItem).ID;

                    long counterID = 0;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        counterID = 0;
                    else
                        counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                    long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    long collectionType = ((MasterListItem)cmbCollectionType.SelectedItem).ID;


                    if (chkToDate == true)
                    {
                        //string URL;
                        #region added by Prashant Channe to read reports config on 27thNov2019
                        string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
                        string URL = null;
                        #endregion

                        if (dtpF != null && dtpT != null)
                        {

                            if (!string.IsNullOrEmpty(StrConfigReportsDir))  //Added by Prashant Channe on 27thNov2019 for Fertility Report configuration
                            {
                                URL = "../" + StrConfigReportsDir + "/Sales/IPD/NewDailyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("dd-MMM-yyyy") + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&CollectionType=" + collectionType + "&SendClinicID=" + SendClinicID + "&UserID=" + UserID;
                            }
                            else
                            {
                                //URL = "../Reports/Sales/IPD/NewDailyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("MM/dd/yyyy") + "&ToDate=" + dtpT.Value.ToString("MM/dd/yyyy") + "&ClinicID=" + clinic + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("MM/dd/yyyy") + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&CollectionType=" + collectionType;
                                URL = "../Reports/Sales/IPD/NewDailyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&Excel=" + chkExcel.IsChecked + "&ToDatePrint=" + dtpP.Value.ToString("dd-MMM-yyyy") + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID + "&LoginUnitID=" + lUnitID + "&CollectionType=" + collectionType + "&SendClinicID=" + SendClinicID + "&UserID=" + UserID;
                            }
                            
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(StrConfigReportsDir))  //Added by Prashant Channe on 27thNov2019 for Fertility Report configuration
                            {
                                URL = "../" + StrConfigReportsDir + "/Sales/IPD/DailyCollectionReportIPD.aspx?FromDate=" + null + "&ToDate=" + null + "&ClinicID=" + clinic + "&ToDatePrint=" + null + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID;
                            }
                            else
                            {
                                URL = "../Reports/Sales/IPD/DailyCollectionReportIPD.aspx?FromDate=" + null + "&ToDate=" + null + "&ClinicID=" + clinic + "&ToDatePrint=" + null + "&BillTypeID=" + BillTypeID + "&counterID=" + counterID;
                            }
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
                else  //added by ajit date 7/10/2016
                {
                    string msgText = " Date Is Required.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
          
            catch (Exception)
            {

            }
        }


        public void GetUserRights()
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        if (objUser.IsDailyCollection || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            UserID = 0;
                        }
                        else
                        {
                            UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void cmdPrint_Checked(object sender, RoutedEventArgs e)
        {

        }

        //private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
            //if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID >= 0 && ((MasterListItem)cmbClinic.SelectedItem).Status == true)
            //{
            //    foreach (var item in objList)
            //    {
            //        if (item.Status == true)
            //        {

            //        }
            //    }

            //}


            //foreach (var item in PackageRelationsList)
            //{
            //    if (item.ID == 0)
            //    {
            //        foreach (var item1 in PackageRelationsList)
            //        {
            //            if (item1.Status == true)
            //                item1.Status = true;
            //            else
            //                item1.Status = false;
            //        }
            //    }
            //}


        //}


    }
}
