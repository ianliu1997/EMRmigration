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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Reflection;
using System.Windows.Browser;
using System.Text;

namespace PalashDynamics.MIS.Sales
{
    public partial class DoctorWiseRevenue : UserControl
    {

      
        //clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        //public PatientAdvance(long TypeID)
        //{
        //    BillTypeID = TypeID;
        //    InitializeComponent();
        //}
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

      
       
       

        public string SendClinicID = string.Empty;
        public DoctorWiseRevenue()
        {
            //BillTypeID = TypeID;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

            FillClinic();
            FillDoctor();
           
            
        }

        private void FillDoctor()
        {
            clsGetDoctorListBizActionVO BizAction = new clsGetDoctorListBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsComboBoxFill = true;

           
            //if ((MasterListItem)cmbDepartment.SelectedItem != null)
            //{
            //    BizAction.SpecializationID = iDeptId;
            //}

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorListBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];
                    FillDoctorType();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void FillDoctorType()
        {
            List<MasterListItem> objList1 = new List<MasterListItem>();
            objList1.Add(new MasterListItem(0, "-- Select --"));
            objList1.Add(new MasterListItem(1, "Internal"));
            objList1.Add(new MasterListItem(4, "External"));
            objList1.Add(new MasterListItem(0, "Both"));

            cmbDoctorType.ItemsSource = null;
            cmbDoctorType.ItemsSource = objList1;

            cmbDoctorType.SelectedItem = objList1[0];
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
                            //for selecting unitid according to user login unit id
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
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
            catch (Exception ex)
            {

                throw;
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;
                bool chkToDate = true;
                bool IsExporttoExcel = false;
                long DoctorID = 0;
                long ClinicID = 0;
                long DrType = 0;

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

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
                  if (cmbClinic.SelectedItem != null)
                {
                    ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                if(cmbDoctor.SelectedItem!=null)
                {
                DoctorID=((MasterListItem)cmbDoctor.SelectedItem).ID;
                }

                if (cmbDoctorType.SelectedItem != null)
                {
                    DrType = ((MasterListItem)cmbDoctorType.SelectedItem).ID;
                }

                ////////////Multi select clinic/////////////

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

                if (string.IsNullOrEmpty(SendClinicID))
                    SendClinicID = "0";

                if (cmbClinic.SelectedItem != null && cmbClinic.SelectedItem != "")
                    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                IsExporttoExcel = (bool)chkExcel.IsChecked;

               

                ///ENd Multiselect clinic

                  if (chkToDate == true)
                  {

                      string URL;


                      URL = "../Reports/Sales/DoctorWise_Revenue.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + ClinicID + "&Excel=" + chkExcel.IsChecked + "&DoctorID=" + DoctorID + "&LoginUnitID=" + lUnitID + "&DrType=" + DrType + "&SendClinicID=" + SendClinicID; 
                      HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                  }
                  else
                  {
                      string msgText = "Incorrect Date Range. From Date Cannot Be Greater Than To Date.";
                      MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                      msgWindow.Show();
                  
                  }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }

        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDoctor_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}

