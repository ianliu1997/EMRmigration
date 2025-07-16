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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Text;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Administration
{
    public partial class DPR : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; 
        public DPR()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //dtpFromDate.SelectedDate = DateTime.Now.Date;          
            FillClinic();
            FillMonth();
            FillYear();

         
        }

        

        private void FillClinic()
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
                        //if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        //{
                        //    //for selecting unitid according to user login unit id
                        //    //for selecting unitid according to user login unit id
                        //    var res = from r in objList
                        //              where r.ID == User.UserLoginInfo.UnitId
                        //              select r;
                        //    ((MasterListItem)res.First()).Status = true;
                        //    cmbClinic.SelectedItem = ((MasterListItem)res.First());
                        //    cmbClinic.IsEnabled = false;
                        //}
                        //else
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

        private void FillMonth()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- Select --"));
            objList.Add(new MasterListItem(1, "January"));
            objList.Add(new MasterListItem(2, "February"));
            objList.Add(new MasterListItem(3, "March"));
            objList.Add(new MasterListItem(4, "April"));
            objList.Add(new MasterListItem(5, "May"));
            objList.Add(new MasterListItem(6, "Jun"));
            objList.Add(new MasterListItem(7, "Jully"));
            objList.Add(new MasterListItem(8, "Augest"));
            objList.Add(new MasterListItem(9, "September"));
            objList.Add(new MasterListItem(10, "Octomber"));
            objList.Add(new MasterListItem(11, "November"));
            objList.Add(new MasterListItem(12, "December"));
            cmbMonth.ItemsSource = null;
            cmbMonth.ItemsSource = objList;
            cmbMonth.SelectedItem = objList[0];
        }

        private void FillYear()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- Select --"));
            for (int i = 2016; i <= 2099; i++)
            {
                objList.Add(new MasterListItem(i, i.ToString()));
            }
            cmbYear.ItemsSource = null;
            cmbYear.ItemsSource = objList;
            cmbYear.SelectedItem = objList[0];
        }


        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtpF = null;
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            DateTime? date = null;
            long month = 0;
            if(((MasterListItem)cmbMonth.SelectedItem != null))
            {
                month = ((MasterListItem)cmbMonth.SelectedItem).ID;
            }
            long year = 0;
            if (((MasterListItem)cmbYear.SelectedItem != null))
            {
                year = ((MasterListItem)cmbYear.SelectedItem).ID;
            }
            date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1, 00, 00, 01);
 
            long clinic = 0;
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

            if (cmbClinic.SelectedItem != null && cmbClinic.SelectedItem != "")
                clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;

            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            
            string URL;
            if (date != null)
            {
                if (optDprOpr.IsChecked == true)
                {
                    URL = "../Reports/Administrator/DPROperationalMetrics.aspx?Month=" + date.Value.ToString("dd/MMM/yyyy") + "&Type=1&Uid=" + clinic +"&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/Administrator/DPROperationalMetrics.aspx?Month=" + date.Value.ToString("dd/MMM/yyyy") + "&Type=2&Uid=" + clinic +"&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void radioButtons_CheckedChanged(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            //if (radioButton.Name == "optDprOpr")
            //{
               
            //}

            if (radioButton.Name == "optDprCash")            
                if (radioButton.IsChecked == true)   
                    cmbClinic.IsEnabled = true;  
            else
                cmbClinic.IsEnabled = false; 
            
            
            
        }
    }
}
