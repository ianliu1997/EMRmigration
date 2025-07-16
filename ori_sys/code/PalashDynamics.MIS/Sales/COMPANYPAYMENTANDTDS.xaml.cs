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
using System.Reflection;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.MIS.Sales
{
    public partial class COMPANYPAYMENTANDTDS : UserControl
    {
        public COMPANYPAYMENTANDTDS()
        {
            InitializeComponent();

        }

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

      

        //private void FillPatientSource()
        //{
        //    clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.ValidPatientMasterSourceList = true;
        //    BizAction.ID = 0;
        //    //BizAction.IsActive = true;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetPatientSourceListBizActionVO)e.Result).MasterList);
        //            cmbPatientSource.ItemsSource = null;
        //            cmbPatientSource.ItemsSource = objList;
        //            cmbPatientSource.SelectedItem = objList[0];
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}

        //private void FillCompany()
        //{
        //    clsGetCompanyListByUnitBizActionVO BizAction = new clsGetCompanyListByUnitBizActionVO();

        //    BizAction.UnitID = (((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID;

        //    if (cmbPatientSource.SelectedItem != null)
        //    {
        //        BizAction.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
        //    }
        //    BizAction.MasterList = new List<MasterListItem>();


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {

        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objNew = new MasterListItem();
        //            objNew.ID = 0;
        //            objNew.Description = "--Select--";
        //            objList.Add(objNew);
        //            objList.AddRange(((clsGetCompanyListByUnitBizActionVO)e.Result).MasterList);
        //            cmbCompany.ItemsSource = null;
        //            cmbCompany.ItemsSource = objList;
        //            cmbCompany.SelectedItem = objList[0];
        //        }

        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            FillUnitList();
            FillCompanyName();
            //  FillCompany(); 

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }



        private void FillUnitList()
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
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];

                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbClinic.IsEnabled = true;
                    cmbClinic.SelectedValue = (long)0;
                }
                else
                {
                    cmbClinic.IsEnabled = false;
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillCompanyName()
        {
            //============================== Show the Company Name====================
            List<MasterListItem> ObjList = new List<MasterListItem>();
            List<clsCompanyVO> objCompList = new List<clsCompanyVO>();
            clsGetCompanyDetailsBizActionVO bizActionVO = new clsGetCompanyDetailsBizActionVO();
            bizActionVO.ItemMatserDetails = new List<clsCompanyVO>();
            bizActionVO.CompanyId = 0;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsCompanyVO selectObject = new clsCompanyVO();
                        selectObject.Id = 0;
                        selectObject.Description = "-- Select --";
                        objCompList.Add(selectObject);
                        objCompList.AddRange(((clsGetCompanyDetailsBizActionVO)args.Result).ItemMatserDetails);

                        foreach (var item in objCompList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = Convert.ToInt64(item.Id);
                            Obj.Description = item.Description;
                            Obj.Status = false;
                            ObjList.Add(Obj);
                        }
                        cmbCompany.ItemsSource = null;
                        cmbCompany.ItemsSource = ObjList;
                        cmbCompany.SelectedItem = ObjList[0];

                  

                        // cmbCompanyList.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception) { }
            //==================================================
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

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
                    //dtpT = dtpT.Value.AddDays(1);
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                }
            }
            long ClinicID = 0;
            long DoctorID = 0;
            long CompanyID = 0;
            long SouceID = 0;

            if (((MasterListItem)cmbClinic.SelectedItem).ID > 0)
            {
                ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            //if (((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            //{
            //    SouceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
            //}

            if (((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            }


            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/Sales/CompanyandTDSreportpage.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + ClinicID + "&CompanyID=" + CompanyID + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {

                    //URL = "../Reports/Sales/CompanyandTDSreportpage.aspx?ClinicID = " + ClinicID + " & DoctorID = " + DoctorID;
                    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FillPatientSource();
        }

        private void cmbPatientSource_Changed(object sender, SelectionChangedEventArgs e)
        {
            //FillCompany();
        }

    }
}