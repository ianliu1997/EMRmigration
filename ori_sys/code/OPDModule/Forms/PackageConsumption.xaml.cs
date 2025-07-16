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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Billing;

namespace OPDModule.Forms
{
    public partial class PackageConsumption : UserControl
    {
        clsPatientGeneralVO SelectedPatient { get; set; }
        WaitIndicator Indicatior = null;

        public PackageConsumption()
        {
            InitializeComponent();
            FillPackage(0);

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            if (rdbPackageBill.IsChecked == true)
            {
                GetData();
            }
            else
            {
                GetConsumptionData();
            }
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (rdbPackageBill.IsChecked == true)
            {
                GetData();
                dataGrid2.SelectedIndex = 0;
            }
            else
            {
                GetConsumptionData();
            }
            //peopleDataPager.PageIndex = 0;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion
        
        public void GetData()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetPatientPackageListBizActionVO BizActionObject = new clsGetPatientPackageListBizActionVO();
                BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

                if (txtFirstName.Text != "")
                    BizActionObject.FirstName = txtFirstName.Text;
                //if (txtMiddleName.Text != "")
                //    BizActionObject.MiddleName = txtMiddleName.Text;
                if (txtLastName.Text != "")
                    BizActionObject.LastName = txtLastName.Text;
                if (txtMRNo.Text != "")
                    BizActionObject.MRNo = txtMRNo.Text;                
                
                if (dtpFromDate.SelectedDate != null)
                    BizActionObject.FromDate = dtpFromDate.SelectedDate.Value.Date;
                if (dtpToDate.SelectedDate != null)
                    BizActionObject.ToDate = dtpToDate.SelectedDate.Value.Date;

                //if (cmbPackage.Text != "- Select -")
                //    BizActionObject.PackageID = ((MasterListItem)cmbPackage.SelectedItem).ID; 
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (((MasterListItem)cmbPackage.SelectedItem) != null)
                {
                    // if (((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
                    //BizActionObject.RegistrationTypeID = ((MasterListItem)cmbPackage.SelectedItem).ID;
                    BizActionObject.PackageID = ((MasterListItem)cmbPackage.SelectedItem).ID;
                }
                else
                {
                    //BizActionObject.RegistrationTypeID = 10;
                    BizActionObject.PackageID = 0;
                }

                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                //{
                //    BizActionObject.UnitID = 0;
                //}
                //else if ((MasterListItem)cmbClinic.SelectedItem != null)
                //{
                //    BizActionObject.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                //}
                //else
                //{
                //    BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //}

                BizActionObject.IsPagingEnabled = true;
                BizActionObject.MaximumRows = DataList.PageSize;
                BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (ea.Result != null)
                        {
                            clsGetPatientPackageListBizActionVO result = ea.Result as clsGetPatientPackageListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;
                            DataList.Clear();
                            foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                            {
                                DataList.Add(person);
                            }

                            dataGrid2.ItemsSource = null;
                            dataGrid2.ItemsSource = DataList;

                            peopleDataPager.Source = null;
                            peopleDataPager.PageSize = BizActionObject.MaximumRows;
                            peopleDataPager.Source = DataList;
                            //
                            dataGrid2.Columns[12].Visibility = Visibility.Visible;
                            dataGrid2.Columns[13].Visibility = Visibility.Visible;
                            dataGrid2.Columns[14].Visibility = Visibility.Visible;
                            dataGrid2.Columns[15].Visibility = Visibility.Visible;
                            dataGrid2.Columns[16].Visibility = Visibility.Visible;
                            dataGrid2.Columns[17].Visibility = Visibility.Visible;
                            dataGrid2.Columns[18].Visibility = Visibility.Visible;
                            dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
                            dataGrid2.Columns[20].Visibility = Visibility.Collapsed;
                            dataGrid2.Columns[21].Visibility = Visibility.Collapsed;
                            dataGrid2.Columns[22].Visibility = Visibility.Collapsed;
                        }
                        //  dataGrid2.SelectedIndex = 0;
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            { 
            }
            finally
            {
                Indicatior.Close();
            }
        }

        public void GetConsumptionData()
        {
            bool res = true;

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpFromDate.RaiseValidationError();
                    string strMsg = "From Date Should Be Less Than To Date";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Plase Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                FillBillSearchList();
            }
        }
        public PagedSortableCollectionView<clsBillVO> DataList1 { get; private set; }
        private void FillBillSearchList()
        {
           // indicator.Show();
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            if (txtFirstName.Text != "")
                BizAction.FirstName = txtFirstName.Text;
            if (txtLastName.Text != "")
                BizAction.LastName = txtLastName.Text;
            if (txtMRNo.Text != "")
                BizAction.MRNO = txtMRNo.Text;
            BizAction.IsConsumption = true;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 0;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            clsPatientGeneralVO obj = new clsPatientGeneralVO();
                            obj.BillID = item.ID;
                            obj.BillUnitID = item.UnitID;
                            obj.UnitId = item.UnitID;
                            obj.VisitID = item.Opd_Ipd_External_Id;
                            obj.VisitUnitID = item.Opd_Ipd_External_UnitId;
                            obj.Opd_Ipd_External = item.Opd_Ipd_External;
                            obj.BillDate = item.Date;
                            obj.BillNo = item.BillNo;
                            obj.MRNo = item.MRNO;
                            obj.PatientName = item.FirstName + ' ' + item.LastName;
                            obj.FirstName = item.FirstName;
                            obj.MiddleName = item.MiddleName;
                            obj.LastName = item.LastName;
                            obj.TotalBillAmount = item.TotalBillAmount;
                            obj.TotalConcessionAmount = item.TotalConcessionAmount;
                            obj.NetBillAmount = item.NetBillAmount;
                            DataList.Add(obj);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;
                        peopleDataPager.Source = null;
                        peopleDataPager.PageSize = BizAction.MaximumRows;
                        peopleDataPager.Source = DataList;
                        //
                        dataGrid2.Columns[12].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[13].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[14].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[15].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[16].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[17].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[18].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
                        dataGrid2.Columns[20].Visibility = Visibility.Visible;
                        dataGrid2.Columns[21].Visibility = Visibility.Visible;
                        dataGrid2.Columns[22].Visibility = Visibility.Visible;
                    }
                }
                //indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPackage.SelectedItem != null && ((MasterListItem)cmbPackage.SelectedItem).ID > 0)
            //{
            //    FillPackage(((MasterListItem)cmbPackage.SelectedItem).ID);
            //}
        }

        //.........................................................................
        private void FillPackage(long PackageID)
        {
            clsGetPackageMasterVO BizAction = new clsGetPackageMasterVO();
            BizAction.PatientSourceID = PackageID;          

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetPackageMasterVO)e.Result).List);
                }
                cmbPackage.ItemsSource = null;
                cmbPackage.ItemsSource = objList;
                cmbPackage.SelectedItem = objList[0];
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                SelectedPatient = new clsPatientGeneralVO();
                SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;

                if (rdbPackageBill.IsChecked == true)
                {
                    PrintReport(((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientUnitID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).PackageID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).UnitId, ((clsPatientGeneralVO)dataGrid2.SelectedItem).VisitID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).BillID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).BillUnitID);
                }
                else
                {
                    if (((clsPatientGeneralVO)dataGrid2.SelectedItem).Opd_Ipd_External == 0)
                    {
                        PrintBill(((clsPatientGeneralVO)dataGrid2.SelectedItem).BillID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).BillUnitID, 1);
                    }
                    else
                    {
                        PrintBill(((clsPatientGeneralVO)dataGrid2.SelectedItem).BillID, ((clsPatientGeneralVO)dataGrid2.SelectedItem).BillUnitID, 4);              
                    }
                        
                }                
            }
        }

        private void PrintReport(long PaitentID, long PUnitID, long PackID, long UnitID, long AdmID, long BillID, long BillUnitID)
        {
            if (PackID > 0 && PaitentID > 0 && PUnitID > 0 && UnitID > 0)
            {
                //  UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //long UnitID = iUnitID;
                string URL = "../Reports/OPD/OPDIPDBIllSummery.aspx?PaitentID=" + PaitentID + "&PUnitID=" + PUnitID + "&UnitID=" + UnitID + "&PackID=" + PackID + "&AdmID=" + AdmID + "&BillID=" + BillID + "&BillUnitID=" + BillUnitID;              
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        //Added By Bhushanp For Package Consumption Print
        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {            
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        private void rdbPackageBill_Click(object sender, RoutedEventArgs e)
        {
            //if (rdbPackageBill.IsChecked == true)
            //{
            //    dataGrid2.Columns[12].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[13].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[14].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[15].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[16].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[17].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[18].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[20].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[21].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[22].Visibility = Visibility.Collapsed;
            //    GetData();
            //    dataGrid2.SelectedIndex = 0;
            //}
            //else
            //{
            //    dataGrid2.Columns[12].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[13].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[14].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[15].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[16].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[17].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[18].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[20].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[21].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[22].Visibility = Visibility.Visible;
            //    GetConsumptionData();
            //}
        }

        private void rdbConsumption_Click(object sender, RoutedEventArgs e)
        {
            //if (rdbPackageBill.IsChecked == true)
            //{
            //    dataGrid2.Columns[12].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[13].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[14].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[15].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[16].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[17].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[18].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[20].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[21].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[22].Visibility = Visibility.Collapsed;
            //    GetData();
            //    dataGrid2.SelectedIndex = 0;
            //}
            //else
            //{
            //    dataGrid2.Columns[12].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[13].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[14].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[15].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[16].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[17].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[18].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[19].Visibility = Visibility.Collapsed;
            //    dataGrid2.Columns[20].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[21].Visibility = Visibility.Visible;
            //    dataGrid2.Columns[22].Visibility = Visibility.Visible;
            //    GetConsumptionData();
            //}
        }
    }
}
