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
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory;
using System.Text;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class PharmacyCollectionReport : UserControl
    {
        public string SendClinicID = string.Empty;              //Added by Prashant Channe on 08 March 2019

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PharmacyCollectionReport()
        {
            InitializeComponent();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;

            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            string msgTitle = "";
            bool chkToDate = true;

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
                    // if (dtpF.Equals(dtpT))
                    //dtpT = dtpT.Value.Date.AddDays(1);
                }
            }


            #region Added by Prashant Channe on 08 March 2019 For Sending Multiple Clinic Selection to report querystring
            
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

            #endregion

            ////commented by prashant channe on 08 march 2019
            //long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            long store = ((clsStoreVO)cmbStore.SelectedItem).StoreId;

            //Begin::Added by aniketK on 25-OCt-2018
            long RegistrationTypeID = 0;
            if (cmbRegType.SelectedItem != null)
            {
                RegistrationTypeID = ((MasterListItem)cmbRegType.SelectedItem).ID;
            }
            //End:: Added by aniketK on 25-OCt-2018

            if (chkToDate == true)
            {
                #region added by Prashant Channe to read reports config on 3rdDec2019
                string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
                string URL = null;
                #endregion
                //string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    //Commented and changed by prashant channe on 08 march 2019 for variable clinic to clinicID
                    //URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + clinic + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked;
                    //Commented and modified by Prashant Channe for FertilityPoint to call from configured reports folder on 3rdDec2019
                    //URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
                    if (!string.IsNullOrEmpty(StrConfigReportsDir))
                    {
                        URL = "../" + StrConfigReportsDir + "/InventoryPharmacy/PharmacyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
                    }
                    else
                    {
                        URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
                    }
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    //Commented and changed by prashant channe on 08 march 2019 for variable clinic to clinicID
                    //URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?clinic=" + clinic + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked;
                    //Commented and modified by Prashant Channe on 03rdDec2019 for path to repots configuration folder
                    //URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
                    if (!string.IsNullOrEmpty(StrConfigReportsDir))
                    {
                        URL = "../" + StrConfigReportsDir + "/InventoryPharmacy/PharmacyCollectionReport.aspx?clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
                    }
                    else
                    {
                        URL = "../Reports/InventoryPharmacy/PharmacyCollectionReport.aspx?clinic=" + ClinicID + "&store=" + store + "&RegistrationTypeID=" + RegistrationTypeID + "&IsExcel=" + chkExcel.IsChecked + "&SendClinicID=" + SendClinicID;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;

            #region //Added by Prashant Channe on 08 March 2019
            Store.Visibility = Visibility.Collapsed;
            cmbStore.Visibility = Visibility.Collapsed;
            #endregion

            FillClinic();
            FillRegisterPatient();
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
                            ((MasterListItem)res.First()).Status = true;  //Added by Prashant Channe on 08 March 2019
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                            Store.Visibility = Visibility.Visible;
                            cmbStore.Visibility = Visibility.Visible;    //Added by Prashant Channe on 08 March 2019
                        }
                        else
                        {
                            cmbClinic.SelectedItem = objList[0];
                        }
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

        private void FillRegisterPatient()
        {
            List<MasterListItem> RegTypeList = new List<MasterListItem>();
            RegTypeList.Add(new MasterListItem(10, "-Select-"));
            RegTypeList.Add(new MasterListItem(0, "OPD"));
            RegTypeList.Add(new MasterListItem(1, "IPD"));
            RegTypeList.Add(new MasterListItem(2, "Pharmacy"));
            RegTypeList.Add(new MasterListItem(5, "Pathology"));
            cmbRegType.ItemsSource = RegTypeList;
            cmbRegType.SelectedItem = RegTypeList[0];
        }


        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        #region Added By Pallavi
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

                    FillStores(clinicId);
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        private void FillStores(long clinicId)
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails.ToList()
                                 where item.ClinicId == clinicId && item.Status == true
                                 select item;

                    List<clsStoreVO> tmpList = new List<clsStoreVO>();
                    tmpList = result.ToList();
                    tmpList.Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbStore.ItemsSource = tmpList.ToList();
                        cmbStore.SelectedItem = tmpList.ToList()[0];
                    }
                    else
                    {
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            cmbStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
