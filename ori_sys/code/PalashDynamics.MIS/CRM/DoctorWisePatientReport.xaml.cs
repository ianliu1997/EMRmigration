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
using System.Reflection;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.MIS.CRM
{
    public partial class DoctorWisePatientReport : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public DoctorWisePatientReport()
        {
            InitializeComponent();
        }

        private void FillUnitList()
        {
            try
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

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {

                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
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
            catch (Exception ex)
            {

                throw;
            }
        }

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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



                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }


                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtFT = null;
            //DateTime? dtTT = null;
            //DateTime? dtTP = null;

            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            Nullable<DateTime> dtTP = null;

          
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            long UnitID = 0;
            long DepartmentID = 0;
            long DoctorID = 0;     
       
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";
            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtFT.Value > dtTT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtTP = dtFT;
                    chkToDate = false;
                }
                else
                {
                    dtTP = dtTT;
                    //dtTT = dtTP.Value.Date.AddDays(1);
                    dtTT = dtTP.Value.AddDays(1);
                }

            }

            if (dtTT != null)
            {
                if (dtFT != null)
                {
                    dtFT = dtpFromDate.SelectedDate.Value.Date.Date;

                    //if (dtpF.Equals(dtpT))
                    //    dtpT = dtpF.Value.Date.AddDays(1);
                }
            }

            if (txtFirstName.Text != null)
            {
                FirstName = txtFirstName.Text;
            }
            if (txtMiddleName.Text != null)
            {

                MiddleName = txtMiddleName.Text;
            }
            if (txtLastName.Text != null)
            {
                LastName = txtLastName.Text;
            }

            if (cmbClinic.SelectedItem != null)
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbDepartment.SelectedItem != null)
                DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            if (cmbDoctor.SelectedItem != null)
                DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

            IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;

            if (chkToDate == true)
            {
                string URL;
                if (dtFT != null && dtTT != null && dtTP != null)
                {
                    URL = "../Reports/CRM/SearchPatientReport.aspx?FromDate=" + dtFT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtTT.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + UnitID + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&ToDatePrint=" + dtTP.Value.ToString("dd/MMM/yyyy") + "&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/CRM/SearchPatientReport.aspx?ClinicID=" + UnitID + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&ToDatePrint=" + dtTP + "&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                }
            }
            else
            {
                string msgText = "Incorrect Date Range. /n From Date Cannot Be Greater Than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((MasterListItem)cmbClinic.SelectedItem != null)
                    FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbClinic_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem == null)
                cmbClinic.SelectedValue = (long)0;
        }

        private void cmbDepartment_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem == null)
                cmbDepartment.SelectedValue = (long)0;
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((MasterListItem)cmbDepartment.SelectedItem != null)
                    FillDoctor(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDoctor.SelectedItem == null)
                cmbDoctor.SelectedValue = (long)0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillUnitList();   
            dtpFromDate.Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if ((bool)chkExporttoExcel.IsChecked)
            {
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.DeleteMISReportFileCompleted += (s1, args1) =>
                {
                    if (args1.Error == null)
                    {
                    }
                };
                client.DeleteMISReportFileAsync("/Reports/CRM");
            }
        }
    }
}
