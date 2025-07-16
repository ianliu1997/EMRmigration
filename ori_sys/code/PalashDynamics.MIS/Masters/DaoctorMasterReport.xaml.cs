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
using PalashDynamics.UserControls;

namespace PalashDynamics.MIS.Masters
{
    public partial class DaoctorMasterReport : UserControl
    {
        bool Flagref = false;
        WaitIndicator wait = new WaitIndicator();
        public DaoctorMasterReport()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillDoctorType();
            FillDoctorCategory();
            FillSpecialization();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long a=0, b=0, c=0;
            string d="";
            if (((MasterListItem)cmbDoctorType.SelectedItem).ID > 0)
            {
                a = ((MasterListItem)cmbDoctorType.SelectedItem).ID;
            }
            if (((MasterListItem)cmbDoctorCategory.SelectedItem).ID > 0)
            {
                b = ((MasterListItem)cmbDoctorCategory.SelectedItem).ID;
            }
            if (((MasterListItem)cmbDoctorSpcialization.SelectedItem).ID > 0)
            {
                c = ((MasterListItem)cmbDoctorSpcialization.SelectedItem).ID;
            }
            if (txtEducation.Text != null && txtEducation.Text != "")
            {
                d = txtEducation.Text;
            }
            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/rptMisDoctorMaster.aspx?Type=" + a + "&Cate=" + b + "&Spci=" + c + "&Edu=" + d + "&Uid=" + lUnitID + "&Excel=" + chkExcel.IsChecked;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
        private void FillDoctorType()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorTypeMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 4

                                      select r;
                        cmbDoctorType.ItemsSource = null;
                        cmbDoctorType.ItemsSource = results.ToList();
                    }
                    else
                    {
                        cmbDoctorType.ItemsSource = null;
                        cmbDoctorType.ItemsSource = objList;
                        cmbDoctorType.SelectedItem = objList[0];
                    }

                }

                if (this.DataContext != null)
                {
                    cmbDoctorType.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorId;
                }


                //if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                //{
                //    cmbDoctorType.SelectedValue = objDoctor.DoctorType;
                //    cmbDoctorType.UpdateLayout();

                //}
                wait.Close();
            };
            
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDoctorCategory()
        {
            WaitIndicator wait1 = new WaitIndicator();
            wait1.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorCategoryMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDoctorCategory.ItemsSource = null;
                    cmbDoctorCategory.ItemsSource = objList;
                    cmbDoctorCategory.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    cmbDoctorCategory.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorCategoryId;
                }

                //if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                //{
                //    cmbDoctorCategory.SelectedValue = objDoctor.DoctorCategoryId;
                //    cmbDoctorCategory.UpdateLayout();

                //}
                wait1.Close();
            };
            
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSpecialization()
        {
            WaitIndicator wait2 = new WaitIndicator();
            wait2.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDoctorSpcialization.ItemsSource = null;
                    cmbDoctorSpcialization.ItemsSource = objList;
                    cmbDoctorSpcialization.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    cmbDoctorSpcialization.SelectedValue = ((clsDoctorVO)this.DataContext).Specialization;
                }

                //if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                //{
                //    cmbSpecialization.SelectedValue = objDoctor.Specialization;
                //    cmbSpecialization.UpdateLayout();

                //}
                wait2.Close();
            };
            
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

    }
}
