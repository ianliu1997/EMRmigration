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
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.MIS.IVF
{
    public partial class PregnancySucessRatePieChart : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PregnancySucessRatePieChart()
        {
            InitializeComponent();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            DateTime? Month = null;

            if (dtpMonth != null)
            {
                if (dtpMonth.DisplayDate != DateTime.MinValue)
                {
                    Month = dtpMonth.DisplayDate;

                }  
                
            }
            long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            string URL = "../Reports/IVF/PregnancySucessRate.aspx?Month=" + Month.Value.ToString("dd/MMM/yyyy") + "&PieChart=True" + "&UnitID=" + clinic; // ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {

            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.IVF.IVFReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void dtpMonth_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (dtpMonth != null)
            {
                dtpMonth.DisplayMode = CalendarMode.Year;

            }
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
        }
    }
}
