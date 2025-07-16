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
    public partial class PatientAdvance : UserControl
    {
        long BillTypeID;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PatientAdvance(long TypeID)
        {
            BillTypeID = TypeID;
            InitializeComponent();
        }

        public string SendClinicID = string.Empty;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

            chkDetailed.IsChecked = true; //Added by AniketK on 15Feb2019
            

            FillClinic();
            FillAdvanceType();
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
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
                long AdvanceType = 0;

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

                long ClinicID = 0;

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


                if (cmbClinic.SelectedItem != null)
                {
                    ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }

                if (cmbAdvanceType.SelectedItem != null)
                {
                    AdvanceType = ((MasterListItem)cmbAdvanceType.SelectedItem).ID;
                }

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                if (chkToDate == true)
                {
                    string URL;


                    URL = "../Reports/Sales/IPD/PatientAdvanceReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + ClinicID + "&Excel=" + chkExcel.IsChecked + "&AdvanceType=" + AdvanceType + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID + "&Detailed=" + chkDetailed.IsChecked + "&Consolidated=" + chkConsolidated.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    string msgText = "Incorrect Date Range. From Date Cannot Be Greater Than To Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

                }
            }
            catch (Exception)
            {
                throw;
            }        
        }

        private void cmbAdvanceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FillAdvanceType()
        {
            List<MasterListItem> objAdvanceType = new List<MasterListItem>();
            MasterListItem Default;
            Default = new MasterListItem();
            Default.ID = 0;
            Default.Description = "- Select -";
            objAdvanceType.Add(Default);

            //if (mAdvanceType == clsAdvanceVO.AdvanceType.Company)
            //{
            //    Default = new MasterListItem();
            //    Default.ID = 1;
            //    Default.Description = "Company";
            //    objAdvanceType.Add(Default);
            //}
            Default = new MasterListItem();
            Default.ID = 1;
            Default.Description = "Company";
            objAdvanceType.Add(Default);
            Default = new MasterListItem();
            Default.ID = 2;
            Default.Description = "Patient";
            objAdvanceType.Add(Default);
            //Default = new MasterListItem();
            //Default.ID = 3;
            //Default.Description = "Patient-Company";
            //objAdvanceType.Add(Default);
            cmbAdvanceType.ItemsSource = objAdvanceType;
            cmbAdvanceType.SelectedItem = objAdvanceType[0]; //objAdvanceType[1];  added on Dated 26042017

           


        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        //Begin::Added by AniketK on 15Feb2019
        private void chkDetailedRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkConsolidatedRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }
        //End::Added by AniketK on 15Feb2019


        
    }
}
