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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.MIS.Masters
{
    public partial class ServiceMasterReportX : UserControl
    {
        public ServiceMasterReportX()
        {
            InitializeComponent();
        }
        WaitIndicator wait = new WaitIndicator();
        public bool IsValidate = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillSpecialization();
            FillCodeType();
        }
        private void FillSpecialization()
        {
            wait.Show();
            try
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

                        //cmbSpecialization.ItemsSource = null;
                        //cmbSpecialization.ItemsSource = objList;
                        //cmbSpecialization.SelectedItem = objList[0];

                        cmbSpecialization1.ItemsSource = null;
                        cmbSpecialization1.ItemsSource = objList;
                        cmbSpecialization1.SelectedItem = objList[0];
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait.Close();
            }
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            WaitIndicator wait2 = new WaitIndicator();
            wait2.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                if (fkSpecializationID != null)
                {
                    BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
                }
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
                        //cmbSubSpecialization.ItemsSource = null;
                        //cmbSubSpecialization.ItemsSource = objList;
                        //cmbSubSpecialization.SelectedValue = objList[0].ID;

                        cmbSubSpecialization1.ItemsSource = null;
                        cmbSubSpecialization1.ItemsSource = objList;
                        cmbSubSpecialization1.SelectedValue = objList[0].ID;
                    }
                    if (this.DataContext != null)
                    {
                        cmbSubSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                    }
                    wait2.Close();
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait2.Close();
            }
        }

        private void cmbSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpecialization1.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization1.SelectedItem).ID.ToString());
                IsValidate = true;
            }
        }

        private void FillCodeType()
        {
            WaitIndicator wait1 = new WaitIndicator();
            wait1.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CodeType;
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

                        cboCodeType.ItemsSource = null;
                        cboCodeType.ItemsSource = objList;
                        cboCodeType.SelectedValue = objList[0].ID;
                    }
                    if (this.DataContext != null)
                    {
                        cboCodeType.SelectedValue = ((clsServiceMasterVO)this.DataContext).CodeType;
                    }
                    wait1.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait1.Close();
            }
        }
        long a, b, c;
        string SC = "",CD="", SD="", BSR="",SN="";
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            a = ((MasterListItem)(cboCodeType.SelectedItem)).ID;
            b = ((MasterListItem)(cmbSpecialization1.SelectedItem)).ID;
            c = ((MasterListItem)(cmbSubSpecialization1.SelectedItem)).ID;
            SC=txtServiceCode1.Text;
            CD=txtCode.Text;
            SD=txtServiceShortDescription.Text;
            BSR=txtServiceRate.Text;
            SN = txtServiceName1.Text;


            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/ServiceMasterReportASPX.aspx?a=" + a + "&b=" + b + "&c=" + c + "&SC=" + SC + "&CD=" + CD + "&SD=" + SD + "&BSR=" + BSR + "&SN=" + SN + "&Uid=" + lUnitID + "&Excel=" + chkExcel.IsChecked;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");     
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
    }
}
