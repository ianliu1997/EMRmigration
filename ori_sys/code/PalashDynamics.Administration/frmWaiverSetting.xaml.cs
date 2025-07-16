using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using CIMS;
using CIMS.Forms;
using PalashDynamics.Collections;
using System.Reflection;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Media.Imaging;
using System.IO;

namespace PalashDynamics.Administration
{
    public partial class frmWaiverSetting : UserControl
    {
        public PagedSortableCollectionView<clsDoctorWaiverDetailVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        public frmWaiverSetting()
        {
            InitializeComponent();
            
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsDoctorWaiverDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 15;
            this.dgDataPager.DataContext = DataList;
            this.dgDoctorWaiverList.DataContext = DataList;

            PageSelection();
            FetchData();

        }

        private void PageSelection()
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            if (mElement.Text.Equals("Doctor Waiver"))
            {
                objDoctor = new clsDoctorWaiverDetailVO();
                objDoctor.PageName = "Doctor Waiver";

            }
            else if (mElement.Text.Equals("Department Waiver"))
            {
                objDoctor = new clsDoctorWaiverDetailVO();
                cmbDoctorName.Visibility = Visibility.Collapsed;
                cmbDoctor.Visibility = Visibility.Collapsed;
                LblDoctor.Visibility = Visibility.Collapsed;
                LblDoctor1.Visibility = Visibility.Collapsed;
                objDoctor.PageName = "Department Waiver";
            }
            else
            {
                objDoctor = new clsDoctorWaiverDetailVO();
                cmbDoctorName.Visibility = Visibility.Collapsed;
                cmbDoctor.Visibility = Visibility.Collapsed;
                cmbDepartmentName.Visibility = Visibility.Collapsed;
                DepartmentName.Visibility = Visibility.Collapsed;
                LblDept.Visibility = Visibility.Collapsed;
                LblDept1.Visibility = Visibility.Collapsed;
                LblDoctor.Visibility = Visibility.Collapsed;
                LblDoctor1.Visibility = Visibility.Collapsed;
                objDoctor.PageName = "Center Waiver";
            }
        }

        #region Variable Declaration
        private long IUnitId { get; set; }
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public List<bool> check = new List<bool>();
        public bool CheckStatus { get; set; }
        bool IsCancel = true;
        PagedCollectionView collection;
        clsGetDoctorTariffServiceListBizActionVO DoctorTariffServiceDetail = new clsGetDoctorTariffServiceListBizActionVO();
        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;
        public clsGetPatientBizActionVO OBJPatient { get; set; }
        WaitIndicator indicator = new WaitIndicator();

        #endregion

        private void WaiverMaster_Loaded(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            this.DataContext = new clsDoctorWaiverDetailVO()
            {
                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                UnitName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName,


            };
            //this.DataContext = new clsDoctorWaiverDetailVO();
            SetCommandButtonState("Load");
            ClearControl();
            //CheckValidation();

           
            
            FetchData();

            FillDepartmentList();

            FillTeriffList();
            Indicatior.Close();
               
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;


                default:
                    break;
            }
        }

        #region  Button Click
        //Button Click Event

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            ClearControl();
            this.DataContext = new clsDoctorWaiverDetailVO()
            {
                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                UnitName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName,

            };
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Doctor Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            ClearControl();
          //  FetchData();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void hlbViewDoctorMaster_View(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("View");
            FillData();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).DoctorName;
        }

        clsDoctorWaiverDetailVO objDoctor = new clsDoctorWaiverDetailVO();

        private void FillData()
        {
            indicator.Show();
            clsGetDoctorWaiverDetailListByIDBizActionVO BizAction = new clsGetDoctorWaiverDetailListByIDBizActionVO();
            BizAction.DoctorWaiverDetails = new clsDoctorWaiverDetailVO();
            //BizAction.DoctorWaiverDetails.DoctorID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).DoctorID;
            //BizAction.DoctorWaiverDetails.DepartmentID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).DepartmentID;
            //BizAction.DoctorWaiverDetails.TariffID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).TariffID;
            //BizAction.DoctorWaiverDetails.ServiceID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).ServiceID;

            BizAction.DoctorWaiverDetails.ID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("View");
                   
                    if (dgDoctorWaiverList.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);

                    objDoctor = ((clsGetDoctorWaiverDetailListByIDBizActionVO)ea.Result).DoctorWaiverDetails;

                    this.DataContext = objDoctor;
                    LblUnitName.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName;
                    UseApplicationID = true;
                    UseApplicationDoctorID = true;
                    FillDepartmentList();
                    FillTeriffList();

                    //cmbTariff.SelectedValue = objDoctor.TariffID;
                    //cmbService.SelectedValue = objDoctor.ServiceID;


                }
                indicator.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
           

        }

        private void cmbSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveDoctor = true;

            SaveDoctor = CheckValidation();

            if (SaveDoctor == true)
            {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save the Doctor Waiver Details?";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDoctorWaiverMaster();
        }

        private void SaveDoctorWaiverMaster()
        {
            Indicatior.Show();

            clsAddDoctorWaiverDetailsBizActionVO BizAction = new clsAddDoctorWaiverDetailsBizActionVO();
            BizAction.DoctorWaiverDetails = (clsDoctorWaiverDetailVO)this.DataContext;

            if (DepartmentName.SelectedItem != null)
                BizAction.DoctorWaiverDetails.DepartmentID = ((MasterListItem)DepartmentName.SelectedItem).ID;

            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorWaiverDetails.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

            if (cmbTariff.SelectedItem != null)
                BizAction.DoctorWaiverDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            if (cmbService.SelectedItem != null)
                BizAction.DoctorWaiverDetails.ServiceID = ((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null)
                {
                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Waiver Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void cmbModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyDoctor = true;
            ModifyDoctor = CheckValidation();
            if (ModifyDoctor == true)
            {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Update the Doctor Waiver Details?";

            MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

            msgW1.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        private void Modify()
        {
            Indicatior.Show();
            clsUpdateDoctorWaiverDetailsBizActionVO BizAction = new clsUpdateDoctorWaiverDetailsBizActionVO();
            BizAction.objDoctorWaiverDetail = new clsDoctorWaiverDetailVO();

            BizAction.objDoctorWaiverDetail.ID = ((clsDoctorWaiverDetailVO)dgDoctorWaiverList.SelectedItem).ID;
            BizAction.objDoctorWaiverDetail.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (DepartmentName.SelectedItem != null)
                BizAction.objDoctorWaiverDetail.DepartmentID = ((MasterListItem)DepartmentName.SelectedItem).ID;

            if (cmbDoctor.SelectedItem != null)
                BizAction.objDoctorWaiverDetail.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

            if (cmbTariff.SelectedItem != null)
                BizAction.objDoctorWaiverDetail.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            if (cmbService.SelectedItem != null)
                BizAction.objDoctorWaiverDetail.ServiceID = ((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID;

            BizAction.objDoctorWaiverDetail.DoctorShareAmount = Convert.ToDecimal(txtDoctorShare.Text);
            BizAction.objDoctorWaiverDetail.DoctorSharePercentage = Convert.ToDecimal(txtDoctorSharePercentage.Text);
            BizAction.objDoctorWaiverDetail.EmergencyDoctorShareAmount = Convert.ToDecimal(txtEmergencyDoctorShare.Text);
            BizAction.objDoctorWaiverDetail.EmergencyDoctorSharePercentage = Convert.ToDecimal(txtEmergencyDoctorSharePercentage.Text);
            BizAction.objDoctorWaiverDetail.WaiverDays = Convert.ToInt64(txtWaiverPeriod.Text);
            BizAction.objDoctorWaiverDetail.Rate = Convert.ToDecimal(txtRate.Text);
            BizAction.objDoctorWaiverDetail.EmergencyRate = Convert.ToDecimal(txtEmergencyRate.Text);




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Modify"); 
                if (arg.Error == null)
                {
                    ClearControl();
                    FetchData();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();
                   
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Waiver Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while updating Doctor Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FetchData()
        {

            clsGetDoctorWaiverDetailListBizActionVO BizAction = new clsGetDoctorWaiverDetailListBizActionVO();
            BizAction.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
            BizAction.PageName = objDoctor.PageName;
            if (cmbDepartmentName.SelectedItem != null)
            {
                BizAction.DepartmentID = ((MasterListItem)cmbDepartmentName.SelectedItem).ID;
            }

            if (cmbDoctorName.SelectedItem != null)
            {
                BizAction.DoctorID = ((MasterListItem)cmbDoctorName.SelectedItem).ID;
            }
            if (cmbTariffName.SelectedItem != null)
            {
                BizAction.TariffID = ((MasterListItem)cmbTariffName.SelectedItem).ID;
            }

            if (cmbServiceName.SelectedItem != null)
            {
                BizAction.ServiceID = ((clsDoctorWaiverDetailVO)cmbServiceName.SelectedItem).ServiceID;
            }
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            //{
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}
            //else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            //{
            //    BizAction.UnitID = 0;
            //}
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorWaiverDetailListBizActionVO)arg.Result).DoctorWaiverDetails != null)
                    {

                        clsGetDoctorWaiverDetailListBizActionVO result = arg.Result as clsGetDoctorWaiverDetailListBizActionVO;

                        if (result.DoctorWaiverDetails != null)
                        {

                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetDoctorWaiverDetailListBizActionVO)arg.Result).TotalRows;
                            
                            foreach (clsDoctorWaiverDetailVO item in result.DoctorWaiverDetails)
                            {
                                DataList.Add(item);
                            }
                            if (objDoctor.PageName == "Doctor Waiver")
                            {
                                collection = new PagedCollectionView(DataList);
                                collection.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                                dgDoctorWaiverList.ItemsSource = collection;
                            }
                            if (objDoctor.PageName == "Department Waiver")
                            {
                                collection = new PagedCollectionView(DataList);
                                collection.GroupDescriptions.Add(new PropertyGroupDescription("DepartmentName"));
                                dgDoctorWaiverList.ItemsSource = collection;
                                dgDoctorWaiverList.Columns[1].Visibility = Visibility.Collapsed;
                            }
                            if (objDoctor.PageName == "Center Waiver")
                            {
                                collection = new PagedCollectionView(DataList);
                                collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                                dgDoctorWaiverList.ItemsSource = collection;
                         
                                dgDoctorWaiverList.Columns[1].Visibility = Visibility.Collapsed;
                            }

                        }


                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        //End

        #endregion

        #region Method Define
        //Function Defined for the Different Use
        private string ConvertShareToPercentage(string DoctorShare, string Rate)
        {
            try
            {
                string result = "";
                double IntDoctorShare = Convert.ToDouble(DoctorShare);
                double IntRate = Convert.ToDouble(Rate);
                double IntResult;
                IntResult = ((Convert.ToDouble(100) * IntDoctorShare) / IntRate);
                result = Convert.ToString(IntResult);
                return result;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        private string ConvertPercentageToShare(string Percentage, string Rate)
        {
            try
            {
                string result = "";
                decimal IntDoctorPercentage = Convert.ToDecimal(Percentage);
                decimal IntRate = Convert.ToDecimal(Rate);
                decimal IntResult;
                IntResult = (IntRate * IntDoctorPercentage) / 100;
                result = Convert.ToString(IntResult);
                return result;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        private void ServiceRate(long ID)
        {
            if (ID > 0)
            {
                
                //List<clsDoctorWaiverDetailVO> ServiceRate = new List<clsDoctorWaiverDetailVO>();
                //ServiceRate = (List<clsDoctorWaiverDetailVO>)cmbService.ItemsSource;
                var rate = from p in (List<clsDoctorWaiverDetailVO>)cmbService.ItemsSource
                           where p.ServiceID == ID
                           select p;
                DoctorTariffServiceDetail.TeriffServiceDetailList = (List<clsDoctorWaiverDetailVO>)rate.ToList();
                if (objDoctor.ServiceID == ID)
                {
                    if (objDoctor.Rate != DoctorTariffServiceDetail.TeriffServiceDetailList[0].Rate)
                    {
                        txtRate.Text = Convert.ToString(objDoctor.Rate);
                    }
                }
                else
                {
                    txtRate.Text = Convert.ToString(DoctorTariffServiceDetail.TeriffServiceDetailList[0].Rate);
                }
            }
        }

        private void ClearControl()
        {
            this.DataContext = new clsDoctorWaiverDetailVO();
            txtWaiverPeriod.Text = string.Empty;
            cmbDoctorName.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).DoctorID;
            cmbDepartmentName.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).DepartmentID;
            cmbTariffName.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).TariffID;
            cmbServiceName.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).ServiceID;
            cmbDoctor.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).DoctorID;
            DepartmentName.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).DepartmentID;
            cmbTariff.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).TariffID;
            cmbService.SelectedValue = (long)((clsDoctorWaiverDetailVO)this.DataContext).ServiceID;
            txtRate.Text = string.Empty;
            txtDoctorShare.Text = string.Empty;
            txtDoctorSharePercentage.Text = string.Empty;
            txtEmergencyRate.Text = string.Empty;
            txtEmergencyDoctorShare.Text = string.Empty;
            txtEmergencyDoctorSharePercentage.Text = string.Empty;

            PageSelection();




        }
        //End
        #endregion

        #region Fill ComboBoxs
        //Fill Combo For Search Criteria

        //Gobal Declaration to fill the Combo box in the First Page
        List<MasterListItem> objDepartmentList = new List<MasterListItem>();
        List<MasterListItem> objDoctorList = new List<MasterListItem>();
        List<MasterListItem> objTariffList = new List<MasterListItem>();
        List<clsDoctorWaiverDetailVO> TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();
        //End of Global Declaration

        private void FillDepartmentForSearchCriteria()
        {
            cmbDepartmentName.ItemsSource = objDepartmentList;
            if (this.DataContext != null)
            {
                cmbDepartmentName.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).DepartmentID;

            }
        }

        private void FillDoctorForSearchCriteria()
        {

            cmbDoctorName.ItemsSource = objDoctorList;
            if (this.DataContext != null)
            {

                cmbDoctorName.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).DoctorID;

            }
        }

        private void FillTariffServiceListForSearchCriteria()
        {
            cmbServiceName.ItemsSource = TeriffServiceDetailList;
            if (this.DataContext != null)
            {
                cmbServiceName.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).ServiceID;
            }
        }

        private void FillTeriffListForSearchCriteria()
        {
            cmbTariffName.ItemsSource = objTariffList;
            if (this.DataContext != null)
            {
                cmbTariffName.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).TariffID;
            }
        }
        //End

        //Fill Combo Box For New Page
        private void FillDepartmentList()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;
            long iUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objDepartmentList = new List<MasterListItem>();
                    objDepartmentList.Add(new MasterListItem(0, "- Select -"));
                    objDepartmentList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);


                    DepartmentName.ItemsSource = null;
                    DepartmentName.ItemsSource = objDepartmentList;
                    FillDepartmentForSearchCriteria();

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                    {
                        DepartmentName.ItemsSource = objDepartmentList;

                    }


                    if (this.DataContext != null)
                    {

                        DepartmentName.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).DepartmentID;

                    }
                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();


            BizAction.UnitId = IUnitId;

            if ((MasterListItem)DepartmentName.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }
            if (IUnitId > 0 && iDeptId > 0)
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        objDoctorList = null;
                        objDoctorList = new List<MasterListItem>();
                        objDoctorList.Add(new MasterListItem(0, "- Select -"));
                        objDoctorList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                        cmbDoctor.ItemsSource = null;
                        cmbDoctor.ItemsSource = objDoctorList;
                        FillDoctorForSearchCriteria();
                        if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                        {
                            cmbDoctor.ItemsSource = objDoctorList;

                        }

                        if (this.DataContext != null)
                        {

                            cmbDoctor.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).DoctorID;

                        }
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                objDoctorList = new List<MasterListItem>();
                objDoctorList.Add(new MasterListItem(0, "- Select -"));

                cmbDoctor.ItemsSource = null;
                cmbDoctor.ItemsSource = objDoctorList;
                FillDoctorForSearchCriteria();

                cmbDoctor.ItemsSource = objDoctorList;
                if (this.DataContext != null)
                {
                    cmbDoctor.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).DoctorID;
                }
            }
        }

        private void FillTeriffList()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;

            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objTariffList = new List<MasterListItem>();
                    objTariffList.Add(new MasterListItem(0, "- Select -"));
                    objTariffList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objTariffList;
                    FillTeriffListForSearchCriteria();
                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbTariff.ItemsSource = objTariffList;


                    if (this.DataContext != null)
                    {
                        cmbTariff.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).TariffID;
                    }
                }
            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillTariffServiceList(long TariffID)
        {


            DoctorTariffServiceDetail.TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();
            DoctorTariffServiceDetail.TeriffServiceDetail.TariffID = TariffID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            cmbService.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorTariffServiceListBizActionVO)arg.Result).TeriffServiceDetailList != null)
                    {

                        clsGetDoctorTariffServiceListBizActionVO result = arg.Result as clsGetDoctorTariffServiceListBizActionVO;

                        if (result.TeriffServiceDetailList != null)
                        {
                            TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();
                            TeriffServiceDetailList.Add(new clsDoctorWaiverDetailVO(0, "- Select -"));
                            TeriffServiceDetailList.AddRange(((clsGetDoctorTariffServiceListBizActionVO)arg.Result).TeriffServiceDetailList);

                            cmbService.ItemsSource = TeriffServiceDetailList;
                            FillTariffServiceListForSearchCriteria();
                            if (this.DataContext != null)
                            {
                                cmbService.SelectedValue = ((clsDoctorWaiverDetailVO)this.DataContext).ServiceID;

                            }

                        }

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(DoctorTariffServiceDetail, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            //}
        }
        //End

        #endregion

        #region Selection Change Event
        //Selection Change Event
        private void DepartmentDoctor_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)DepartmentName.SelectedItem != null)
            {

                FillDoctor(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)DepartmentName.SelectedItem).ID);

            }
        }

        private void DoctorTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbTariff.SelectedItem != null)
            {
                FillTariffServiceList(((MasterListItem)cmbTariff.SelectedItem).ID);
            }

        }

        private void DoctorServiceRate_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            
            if (cmbService.SelectedItem != null)
            {
                if (((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID > 0)
                {
                    
                    //txtDoctorShare.Text = string.Format("0.00");
                    //txtDoctorSharePercentage.Text = string.Format("0.00");
                    //txtEmergencyDoctorShare.Text = string.Format("0.00");
                    //txtEmergencyDoctorSharePercentage.Text = string.Format("0.00");
                    if (objDoctor.ServiceID == ((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID)
                    {
                        txtDoctorShare.Text = objDoctor.DoctorShareAmount.ToString();
                        txtDoctorSharePercentage.Text = objDoctor.DoctorSharePercentage.ToString();
                        txtEmergencyDoctorShare.Text = objDoctor.EmergencyDoctorShareAmount.ToString();
                        txtEmergencyDoctorSharePercentage.Text = objDoctor.EmergencyDoctorSharePercentage.ToString();
                    }
                    else
                    {
                        txtDoctorShare.Text = string.Format("0.00");
                        txtDoctorSharePercentage.Text = string.Format("0.00");
                        txtEmergencyDoctorShare.Text = string.Format("0.00");
                        txtEmergencyDoctorSharePercentage.Text = string.Format("0.00");
                    }

                    
                    ServiceRate(((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID);
                }
            }
        }

        private void DoctorTariffForSearchCriteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbTariffName.SelectedItem != null)
            {

                FillTariffServiceList(((MasterListItem)cmbTariffName.SelectedItem).ID);
            }
        }

        private void DepartmentDoctorForSearch_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartmentName.SelectedItem != null)
            {

                FillDoctor(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDepartmentName.SelectedItem).ID);

            }
        }
        //End
        #endregion

        #region Lost Focus Event
        //Lost Focus Event
        private void EmergencyDoctorShare_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtEmergencyDoctorShare.Text == null)
            //{
            //    txtEmergencyDoctorShare.SetValidation("Please enter Only Number");
            //    txtEmergencyDoctorShare.RaiseValidationError();
            //}
            //else if (((clsDoctorWaiverDetailVO)this.DataContext).DoctorShareAmount == null)
            //{
            //    txtDoctorShare.SetValidation("Please select the Date of birth");
            //    txtDoctorShare.RaiseValidationError();

            //}
            if (txtEmergencyDoctorShare.Text != "" && txtEmergencyRate.Text!= "")
            {
                //txtEmergencyDoctorShare.ClearValidationError();
                txtEmergencyDoctorSharePercentage.Text = ConvertShareToPercentage(txtEmergencyDoctorShare.Text, txtEmergencyRate.Text);
            }
        }

        private void EmergencyDoctorPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtEmergencyDoctorSharePercentage.Text == null)
            //{
            //    txtEmergencyDoctorSharePercentage.SetValidation("Please enter Only Number");
            //    txtEmergencyDoctorSharePercentage.RaiseValidationError();
            //}
            //else if (((clsDoctorWaiverDetailVO)this.DataContext).DoctorShareAmount == null)
            //{
            //    txtDoctorShare.SetValidation("Please select the Date of birth");
            //    txtDoctorShare.RaiseValidationError();

            //}
            if (txtEmergencyDoctorSharePercentage.Text != "" && txtEmergencyRate.Text!= "")
            {
                //txtEmergencyDoctorSharePercentage.ClearValidationError();
                txtEmergencyDoctorShare.Text = ConvertPercentageToShare(txtEmergencyDoctorSharePercentage.Text, txtEmergencyRate.Text);

            }
        }

        private void txtDoctorSharePercentage_lostFocus(object sender, RoutedEventArgs e)
        {
            //if (Extensions.IsItNumber(txtDoctorSharePercentage.Text) == false)
            //{
            //    txtDoctorSharePercentage.SetValidation("Please enter Only Number");
            //    txtDoctorSharePercentage.RaiseValidationError();
            //}
            //else if (((clsDoctorWaiverDetailVO)this.DataContext).DoctorShareAmount == null)
            //{
            //    txtDoctorShare.SetValidation("Please select the Date of birth");
            //    txtDoctorShare.RaiseValidationError();

            //}
            if (txtDoctorSharePercentage.Text != "" && txtRate.Text!= "")
            {
                txtDoctorSharePercentage.ClearValidationError();
                txtDoctorShare.Text = ConvertPercentageToShare(txtDoctorSharePercentage.Text, txtRate.Text);

            }

        }

        private void txtDaoctorShare_LostFocus(object sender, RoutedEventArgs e)
        {
            
            //if (txtDoctorShare.Text.IsItNumber()==false)
            //{
            //    txtDoctorShare.SetValidation("Please Enter Only Number");
            //    txtDoctorShare.RaiseValidationError();
            //}
            //else if (((clsDoctorWaiverDetailVO)this.DataContext).DoctorShareAmount == null)
            //{
            //    txtDoctorShare.SetValidation("Please Enter the DoctorShareAmount");
            //    txtDoctorShare.RaiseValidationError();

            //}
            if (txtDoctorShare.Text != "" && txtRate.Text!="")
            {
                txtDoctorShare.ClearValidationError();
                txtDoctorSharePercentage.Text = ConvertShareToPercentage(txtDoctorShare.Text, txtRate.Text);

            }
        }
        //End
        #endregion

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;

           


                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                if (mElement.Text.Equals("Doctor Waiver"))
                {
                    objDoctor = new clsDoctorWaiverDetailVO();
                    objDoctor.PageName = "Doctor Waiver";

                }
                else if (mElement.Text.Equals("Department Waiver"))
                {
                    objDoctor = new clsDoctorWaiverDetailVO();
                    objDoctor.PageName = "Department Waiver";
                }
                else
                {
                    objDoctor = new clsDoctorWaiverDetailVO();
                    objDoctor.PageName = "Center Waiver";
                }

                if (objDoctor.PageName != "Center Waiver")
                {

                    if ((MasterListItem)DepartmentName.SelectedItem == null)
                    {
                        DepartmentName.TextBox.SetValidation("Please Select Department Name");
                        DepartmentName.TextBox.RaiseValidationError();
                        DepartmentName.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)DepartmentName.SelectedItem).ID == 0)
                    {
                        DepartmentName.TextBox.SetValidation("Please Select Department Name");
                        DepartmentName.TextBox.RaiseValidationError();
                        DepartmentName.Focus();
                        result = false;
                    }
                    else
                        DepartmentName.TextBox.ClearValidationError();
                }

                if (objDoctor.PageName != "Department Waiver" && objDoctor.PageName != "Center Waiver")
                {

                    if ((MasterListItem)cmbDoctor.SelectedItem == null)
                    {
                        cmbDoctor.TextBox.SetValidation("Please Select Doctor Name");
                        cmbDoctor.TextBox.RaiseValidationError();
                        cmbDoctor.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                    {
                        cmbDoctor.TextBox.SetValidation("Please Select Doctor Name");
                        cmbDoctor.TextBox.RaiseValidationError();
                        cmbDoctor.Focus();
                        result = false;
                    }
                    else
                        cmbDoctor.TextBox.ClearValidationError();
                }

                if ((MasterListItem)cmbTariff.SelectedItem == null)
                {
                    cmbTariff.TextBox.SetValidation("Please Select Tariff Name");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbTariff.SelectedItem).ID == 0)
                {
                    cmbTariff.TextBox.SetValidation("Please Select Tariff Name");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    result = false;
                }
                else
                    cmbTariff.TextBox.ClearValidationError();


                if ((clsDoctorWaiverDetailVO)cmbService.SelectedItem == null)
                {
                    cmbService.TextBox.SetValidation("Please Select Serive Name");
                    cmbService.TextBox.RaiseValidationError();
                    cmbService.Focus();
                    result = false;
                } 
                else if (((clsDoctorWaiverDetailVO)cmbService.SelectedItem).ServiceID == 0)
                {
                    cmbService.TextBox.SetValidation("Please Select Serive Name");
                    cmbService.TextBox.RaiseValidationError();
                    cmbService.Focus();
                    result = false;
                }
                else
                    cmbService.TextBox.ClearValidationError();

                if (Extensions.IsValueDouble(txtDoctorSharePercentage.Text) == false)
                {
                    txtDoctorSharePercentage.SetValidation("Please enter Only Number");
                    txtDoctorSharePercentage.RaiseValidationError();
                    txtDoctorSharePercentage.Focus();
                    result = false;
                }
                else
                {
                    txtDoctorSharePercentage.ClearValidationError();
                }


                if (Extensions.IsValueDouble(txtDoctorShare.Text) == false)
                {
                    txtDoctorShare.SetValidation("Please Enter Only Number");
                    txtDoctorShare.RaiseValidationError();
                    txtDoctorShare.Focus();
                    result = false;
                }
                else
                {
                    txtDoctorShare.ClearValidationError();
                }


                if (Extensions.IsValueDouble(txtEmergencyDoctorShare.Text) == false)
                {
                    txtEmergencyDoctorShare.SetValidation("Please enter Only Number");
                    txtEmergencyDoctorShare.RaiseValidationError();
                    txtEmergencyDoctorShare.Focus();
                    result = false;
                } 
                else
                {
                    txtEmergencyDoctorShare.ClearValidationError();
                }

                if (Extensions.IsValueDouble(txtEmergencyDoctorSharePercentage.Text) == false)
                {
                    txtEmergencyDoctorSharePercentage.SetValidation("Please enter Only Number");
                    txtEmergencyDoctorSharePercentage.RaiseValidationError();
                    txtEmergencyDoctorSharePercentage.Focus();
                    result = false;
                }
                else
                {
                    txtEmergencyDoctorSharePercentage.ClearValidationError();
                }

                if (Extensions.IsItNumber(txtWaiverPeriod.Text)==false)
                {
                    txtWaiverPeriod.SetValidation("Please enter Only Number");
                    txtWaiverPeriod.RaiseValidationError();
                    txtWaiverPeriod.Focus();
                    result = false;
                }
                else
                {
                    txtWaiverPeriod.ClearValidationError();
                }

        
            return result;
        }
        #endregion

    }
}