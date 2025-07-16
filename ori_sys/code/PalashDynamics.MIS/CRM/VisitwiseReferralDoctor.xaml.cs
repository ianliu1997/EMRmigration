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
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;
using System.Windows.Browser;
using System.Reflection;

namespace PalashDynamics.MIS.CRM
{
    public partial class VisitwiseReferralDoctor : UserControl, IInitiateCIMS
    {

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "PRO":
                    FlagPro = true;
                    break;

            }
        }
        public VisitwiseReferralDoctor()
        {
            InitializeComponent();
        }
        bool FlagPro = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillRefDoctor();

        }
        private void FillRefDoctor()
        {
            clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {

                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetRefernceDoctorBizActionVO)e.Result).ComboList != null)
                    {

                        clsGetRefernceDoctorBizActionVO result = (clsGetRefernceDoctorBizActionVO)e.Result;
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        if (result.ComboList != null)
                        {
                            foreach (var item in result.ComboList)
                            {
                                MasterListItem Objmaster = new MasterListItem();
                                Objmaster.ID = item.ID;
                                Objmaster.Description = item.Value;
                                objList.Add(Objmaster);

                            }
                        }
                        cmbRefDoctor.ItemsSource = null;
                        cmbRefDoctor.ItemsSource = objList;
                        cmbRefDoctor.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            cmbRefDoctor.SelectedValue = ((clsVisitVO)this.DataContext).ReferredDoctorID;

                        }
                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            bool IsExporttoExcel = false;
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
                    //dtpT = dtpT.Value.Date.AddDays(1);
                    // dtpT = dtpT.Value.AddDays(1);
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                }
            }

            //long DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            //string ReferenceDoctor = txtReferenceDoctor.Text;
            long DoctorID = ((MasterListItem)cmbRefDoctor.SelectedItem).ID;
            long UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/CRM/VisitwiseReferralDoctor.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&DoctorID=" + DoctorID + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitId;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/CRM/VisitwiseReferralDoctor.aspx?DoctorID=" + DoctorID + "&UnitID=" + UnitId;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

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
            if (FlagPro == true)
            {
                //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport.VisitwiseReferralDoctor") as UIElement;
                //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
        }



    }


        
    }

