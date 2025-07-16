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
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Windows.Controls.Data;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using OPDModule;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Service.EmailServiceReference;
using OPDModule.Forms;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Patient;
using System.Reflection;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects.Patient;

namespace CIMS.Forms
{
    public partial class AppointmentList : UserControl
    {
        #region Variable Declaration
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        bool aVisitMark { get; set; }
        bool Status { get; set; }
        long AppmentID = 0;
        public string Email { get; set; }
        public DateTime Fdt { get; set; }
        public DateTime Tdt { get; set; }
        public static bool IsAppointmentList = false;
        clsUserRightsVO objUser;

        // public clsGetAppointmentBizActionVO BizAction { get; set; }

        //Added by Manisha
        public event RoutedEventHandler OnSaveButton_Click;

        public bool ChkUnit { get; set; }


        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;
        

        #endregion

        #region Paging

        public PagedSortableCollectionView<clsAppointmentVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FetchData();

        }



        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentList()
        {
            InitializeComponent();

            
            DataList = new PagedSortableCollectionView<clsAppointmentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;


            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbDoctorAppointmentSummary.IsEnabled = false;


                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID

                };

            }
            else
            {
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,

                };

            }

            dtpAppointmentSummaryF.SelectedDate = DateTime.Now;
            dtpAppointmentSummaryT.SelectedDate = DateTime.Now;
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
        }

        /// <summary>
        /// Purpose:Get patient appointment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
           
            if (dtpAppointmentSummaryT.SelectedDate != null && dtpAppointmentSummaryF.SelectedDate != null)
            {
                if (dtpAppointmentSummaryT.SelectedDate.Value.Date.Date < dtpAppointmentSummaryF.SelectedDate.Value.Date.Date)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "To Date Cannot Be Less Than From date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();

                }
                else
                {
                    FetchData();
                }
            }
            else
            {
                FetchData();
            }

            //Added by Ajit Jadhav Date 30/9/2016
            if (cmbAppointmentStat.SelectedItem != null && ((MasterListItem)cmbAppointmentStat.SelectedItem).ID == 1)
            {
                dgAppointmentList.Columns[20].Visibility = Visibility.Collapsed;
            }
            else
            {
                dgAppointmentList.Columns[20].Visibility = Visibility.Visible;
            }
            //***//---------------------
        }

        private void frmAppointmentList_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {

                Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                IsAppointmentList = true;
               

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do  nothing
                    }
                    else
                        cmdCancelAppointment.IsEnabled = false;

                 
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbUnitAppointmentSummary.IsEnabled = true;
                    cmdRescAppointment.IsEnabled = false;  
                }
                else
                {
                    cmbUnitAppointmentSummary.IsEnabled = false;
                    cmbUnitAppointmentSummary.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                
                tabAppointmentChart.Visibility = Visibility.Collapsed;
                FillUnitList();
                cmbAppointmentStat.ItemsSource = FillAppointmentStatus();
                cmbAppointmentStat.SelectedValue = (long)0;
                cmbAppointmentType.ItemsSource = FillAppointmentType();
                cmbAppointmentType.SelectedValue = (long)2;
                FillSpecialRegistration();
               
                SetComboboxValue();
                dtpAppointmentSummaryF.Focus();
               // FetchData();

                Indicatior.Close();
            }
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            dtpAppointmentSummaryF.Focus();
            dtpAppointmentSummaryF.UpdateLayout();
            IsPageLoded = true;
        }

        public clsAppointmentVO item = new clsAppointmentVO();

        #region Selected Index Changed
        private void cmbUnitAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID);

        }

        private void cmbDoctorAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbDepartmentAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem != null) ;
            FillDoctor(((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID, ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem).ID);


        }

        private void dtpAppointmentSummary_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {


        }



        private void txtMrno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem != null) ;
            FillChartDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);

        }

        #endregion

        #region FillCombobox

        public static List<MasterListItem> FillAppointmentStatus()
        {
            List<MasterListItem> objAppointStatus = new List<MasterListItem>();
            
            objAppointStatus.Add(new MasterListItem(0, "New"));
            objAppointStatus.Add(new MasterListItem(1, "Cancelled"));
            objAppointStatus.Add(new MasterListItem(2, "ReScheduled"));
            return objAppointStatus;
        }
        public static List<MasterListItem> FillAppointmentType()
        {
            List<MasterListItem> objAppointType = new List<MasterListItem>();
            objAppointType.Add(new MasterListItem(2, "All"));
            objAppointType.Add(new MasterListItem(1, "Turned-Up"));
            objAppointType.Add(new MasterListItem(0, "Non Turned-Up"));
            

            //objAppointType.Add(new MasterListItem(2, "Doctor"));
            //objAppointType.Add(new MasterListItem(3, "Department"));
            return objAppointType;
        }

        private void FillUnitList()
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsUserWise = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
                    cmbUnitAppointmentSummary.ItemsSource = null;
                    cmbUnitAppointmentSummary.ItemsSource = objList;
                    cmbUnitAppointmentSummary.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbUnitAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            //BizAction.MasterList = new List<MasterListItem>();
            ////PalashServiceClient Client = null;
            ////Client = new PalashServiceClient();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "-- Select --"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

            //        cmbUnitAppointmentSummary.ItemsSource = null;
            //        //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
            //        cmbUnitAppointmentSummary.ItemsSource = objList;
            //    }
            //    if (this.DataContext != null)
            //    {
            //        cmbUnitAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;

            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();

        }

        private void FillDepartmentList(long iUnitId)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionVo = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
            BizActionVo.IsUnitWise = true;
            BizActionVo.IsClinical = true;  // flag use to Show/not Clinical Departments  02032017
            BizActionVo.UnitID = iUnitId;   // fill Unitwise Departments  02032017

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem != null)
                    {
                        objList.AddRange(((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem);
                    }
                    cmbDepartmentAppointmentSummary.ItemsSource = objList;
                    cmbDepartmentAppointmentSummary.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
            #region Commented By Bhushan Patil
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            //if (iUnitId > 0)
            //    BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "-- Select --"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);


            //        if (((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID == 0)
            //        {

            //            var results = from a in objList
            //                          group a by a.ID into grouped
            //                          select grouped.First();
            //            objList = results.ToList();
            //        }

            //        cmbDepartmentAppointmentSummary.ItemsSource = null;
            //        cmbDepartmentAppointmentSummary.ItemsSource = objList;
                    



            //        if (this.DataContext != null)
            //        {
            //            if (UseApplicationID == true)
            //            {
            //                cmbDepartmentAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
            //                UseApplicationID = false;
            //                cmbDepartmentAppointmentSummary.SelectedItem = objList[0];
            //            }
            //            else
            //                cmbDepartmentAppointmentSummary.SelectedValue = objList[0].ID;

            //        }



            //    }

            //};


            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            #endregion 
        }
        
        private void FillDoctor(long IUnitId1, long iDeptId1)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId1;
            }
            if ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId1;
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

                    cmbDoctorAppointmentSummary.ItemsSource = null;
                    cmbDoctorAppointmentSummary.ItemsSource = objList;
                    cmbDoctorAppointmentSummary.SelectedItem = objList[0];


                    if (this.DataContext != null)
                    {


                        if (UseApplicationDoctorID == true)
                        {
                            cmbDoctorAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }
                        else
                            cmbDoctorAppointmentSummary.SelectedValue = objList[0].ID;



                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctorAppointmentSummary.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }
                    }

                }
                FetchData();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillSpecialRegistration()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSpRegistration.ItemsSource = null;
                    cmbSpRegistration.ItemsSource = objList;
                    //                    cmbSpRegistration.SelectedItem = objList[0];


                }

                if (this.DataContext != null)
                {
                    cmbSpRegistration.SelectedValue = ((clsAppointmentVO)this.DataContext).SpecialRegistrationID;
                }

                //if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                //{
                //    if (OBJPatient != null)
                //    {
                //        if (OBJPatient.PatientDetails != null)
                //        {
                //            if (OBJPatient.PatientDetails.SpecialRegistrationID != 0)
                //            {
                //                cmbSpRegistration.SelectedValue = OBJPatient.PatientDetails.SpecialRegistrationID;
                //            }
                //        }
                //    }
                //}
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillChartUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
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
                    cmbClinic.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillChartDepartmentList(long iUnitId)
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

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;

                    if (this.DataContext != null)
                    {

                        cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;


                    }



                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        #endregion

        private void SetComboboxValue()
        {
            cmbUnitAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            cmbDepartmentAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
            cmbDoctorAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;

            cmbClinic.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;

        }

        /// <summary>
        /// Method Purpose: Search Appointment by diff search criteria. 
        /// </summary>

        public bool UnRegistered;
        public void FetchData()
        {            
            clsGetAppointmentBizActionVO BizAction = new clsGetAppointmentBizActionVO();
            if (dtpAppointmentSummaryF.SelectedDate != null)
            {
                BizAction.FromDate = dtpAppointmentSummaryF.SelectedDate.Value.Date.Date;
            }
            if (dtpAppointmentSummaryT.SelectedDate != null)
            {
                BizAction.ToDate = dtpAppointmentSummaryT.SelectedDate.Value.Date.Date; ;
            }
            if (UseApplicationID != true || UseApplicationDoctorID != true)
            {
                if (cmbUnitAppointmentSummary.SelectedItem != null)
                {
                    BizAction.UnitId = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitId = 0;
                }
                else
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                if (cmbDepartmentAppointmentSummary.SelectedItem != null)
                    BizAction.DepartmentId = ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem).ID;

                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    BizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                }
                else
                {
                    if (cmbDoctorAppointmentSummary.SelectedItem != null)
                        BizAction.DoctorId = ((MasterListItem)cmbDoctorAppointmentSummary.SelectedItem).ID;
                }
            }
            else
            {
                if (this.DataContext != null)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    {
                        BizAction.UnitId = 0;
                    }
                    else
                    {
                        BizAction.UnitId = ((clsAppointmentVO)this.DataContext).UnitId;
                    }

                    BizAction.DepartmentId = ((clsAppointmentVO)this.DataContext).DepartmentId;

                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                    {
                        BizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    }
                    else
                    {
                        BizAction.DoctorId = ((clsAppointmentVO)this.DataContext).DoctorId;
                    }
                }
            }

            if (cmbAppointmentStat.SelectedItem != null)
            {
                BizAction.AppintmentStatusID = Convert.ToInt32(((MasterListItem)cmbAppointmentStat.SelectedItem).ID);
            }

            if (cmbSpRegistration.SelectedItem != null)
            {
                BizAction.SpecialRegistrationId = ((MasterListItem)cmbSpRegistration.SelectedItem).ID; 
            }

            if (cmbAppointmentStat.SelectedItem != null)
            {
                if (((MasterListItem)cmbAppointmentType.SelectedItem).ID != 2)
                {
                    BizAction.VisitMark = Convert.ToBoolean(((MasterListItem)cmbAppointmentType.SelectedItem).ID);
                }
            } 
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;
            BizAction.ContactNo = txtContactNo.Text;
            BizAction.MrNo = txtMiddleName.Text;            
            BizAction.UnRegistered = UnRegistered;
            BizAction.InputPagingEnabled = true;
            BizAction.InputStartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.InputMaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetAppointmentBizActionVO)arg.Result).AppointmentDetailsList != null)
                    {
                        clsGetAppointmentBizActionVO result = arg.Result as clsGetAppointmentBizActionVO;
                        DataList.TotalItemCount = result.OutputTotalRows;

                        if (result.AppointmentDetailsList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.AppointmentDetailsList)
                            {                                                             
                                DataList.Add(item);
                            }
                        }
                        dgAppointmentList.ItemsSource = DataList;
                        dgDataPager.Source = DataList;
                        if (dgAppointmentList != null && dgAppointmentList.Columns.Count >= 0)
                        {
                            if ((MasterListItem)cmbAppointmentStat.SelectedItem != null && ((MasterListItem)cmbAppointmentStat.SelectedItem).Description.Trim() == "Cancelled")
                            {
                                dgAppointmentList.Columns[20].Visibility = Visibility.Collapsed;   //Added by Ajit Jadhav Date 30/9/2016
                            }
                            else if (((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID)
                            {
                                dgAppointmentList.Columns[20].Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                dgAppointmentList.Columns[20].Visibility = Visibility.Visible;   //Added by Ajit Jadhav Date 30/9/2016
                            }
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        //    dgDataPager.PageIndex = 0;
        }

        /// <summary>
        /// Purpose:For Getting patient information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       

        private void dgAppointmentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsAppointmentVO)dgAppointmentList.SelectedItem != null)
            {
                item = ((clsAppointmentVO)dgAppointmentList.SelectedItem);
                PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO PatInfo = new PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO();
                PatInfo.MRNo = item.MrNo;
                PatInfo.PatientID = item.PatientId;
                PatInfo.PatientUnitID = item.PatientUnitId;
                PatInfo.GenderID = item.GenderId;
                if(item.GenderId == 1)
                {
                    PatInfo.Gender = "MALE";
                    
                }
                else if (item.GenderId == 2)
                {
                    PatInfo.Gender = "Female";
                }
                else
                {
                    PatInfo.Gender = "";
                }
                PatInfo.UnitId = item.UnitId;
                PatInfo.IsFromAppointment = true;
                PatInfo.FirstName = item.FirstName;
                PatInfo.MiddleName = item.MiddleName;
                PatInfo.LastName = item.LastName;


                ((IApplicationConfiguration)App.Current).SelectedPatient = PatInfo;
            }
        }

        private void btnSearchDoctor_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Prpose:For Cancel selected appointment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
         
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID > 0)  //Added by Ajit Jadhav Date 17/10/2016
                {
                    if ((clsAppointmentVO)dgAppointmentList.SelectedItem != null)
                    {
                        if ((clsAppointmentVO)dgAppointmentList.SelectedItem != null && ((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentStatusNew != "Cancelled")   //Added by ajit date7/10/2016 
                        {
                            if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentDate >= DateTime.Today)
                            {
                                clsCheckMarkVisitBizActionVO BizAction = new clsCheckMarkVisitBizActionVO();
                                BizAction.Details = item;
                                BizAction.ID = item.AppointmentID;
                                BizAction.UnitID = item.UnitId;
                                BizAction.VisitMark = item.VisitMark;

                                /* Updated on 18/12/2015 */

                                DateTime FromTime, ToTime;
                                string fromTime = " ";
                                string toTime = " ";

                                FromTime = item.FromTime.Value;
                                ToTime = item.ToTime.Value;

                                fromTime = FromTime.ToString("hh: mm tt");
                                toTime = ToTime.ToString("hh: mm tt");
                                DateTime PresentTime = DateTime.Now;
                                String presentTime = PresentTime.ToString("hh: mm tt");

                                FromTime = Convert.ToDateTime(fromTime);
                                ToTime = Convert.ToDateTime(toTime);
                                PresentTime = Convert.ToDateTime(presentTime);


                                // end                     


                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Error == null)
                                    {

                                        Status = ((clsCheckMarkVisitBizActionVO)arg.Result).SuccessStatus;
                                        if (Status == false)
                                        {
                                            if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentDate == DateTime.Today)
                                            {
                                                if (FromTime < PresentTime && ToTime < PresentTime)
                                                {
                                                    string msgTitle = "";
                                                    string msgText = "Time Has Already Elapsed. You Cannot Cancel The Appointment?";
                                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                                                    msgW.Show();
                                                    ClickedFlag = 0;

                                                }
                                                else
                                                {
                                                    string msgTitle = "";
                                                    string msgText = "Are You Sure You Want To Cancel This Appointment?";
                                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                                                    msgW.Show();
                                                    ClickedFlag = 0;
                                                }

                                            }
                                            else if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentDate > DateTime.Today)
                                            {
                                                string msgTitle = "";
                                                string msgText = "Are You Sure You Want To Cancel This Appointment?";
                                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                                                msgW.Show();
                                                ClickedFlag = 0;
                                            }

                                        }

                                        else
                                        {
                                            string msgTitle = "";
                                            string msgText = "You Cannot Cancel This Appointment Because Visit Is Already Marked For This Appointment";
                                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW2.Show();
                                            ClickedFlag = 0;

                                        }
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();
                            }
                            else
                            {
                                string msgTitle = "";
                                string msgText = "You Cannot Cancel Backdated Appointment";
                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW2.Show();
                                ClickedFlag = 0;
                            }

                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Appointment Is Already Cancel";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }

                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Please Select Appointment";

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Please Select Clinic";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    ClickedFlag = 0;

                }
               
            }

        }

        /// <summary>
        /// Purpose:Add appointment cancel reason
        /// </summary>
        /// <param name="result"></param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (result == MessageBoxResult.Yes)
                {

                    AppointmentReasonWindow win1 = new AppointmentReasonWindow();
                    win1.PatientName = item.PatientName;
                    win1.FromDate = item.FromTime.Value;
                    win1.ToDate = item.ToTime;
                    win1.AppointmentDate = item.AppointmentDate;
                    win1.OnSaveButton_Click += new RoutedEventHandler(win1_OnSaveButton_Click);
                    win1.OnCancelButton_Click += new RoutedEventHandler(win1_OnCancelButton_Click);
                    win1.Show();
                    ClickedFlag = 0;
                }
                else
                    ClickedFlag = 0;
            }
        }

        /// <summary>
        /// Purpose: For Cancel Appointment And Add Cancel Appointment  reason
        /// Creation Date:10/11/2010
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void win1_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                try
                {
                    if (item != null)
                    {
                        clsCancelAppointmentBizActionVO BizAction = new clsCancelAppointmentBizActionVO();
                        BizAction.AppointmentDetails = item;
                        BizAction.AppointmentDetails.AppointmentID = item.AppointmentID;
                        BizAction.AppointmentDetails.AppCancelReason = ((AppointmentReasonWindow)sender).txtAppReason.Text;
                        //BizAction.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        if (cmbUnitAppointmentSummary.SelectedItem != null) //Added by Ajit Jadhav Date 17/10/2016
                        {
                              BizAction.AppointmentDetails.UnitId = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                        }
                      
                        BizAction.AppointmentDetails.AppointmentStatus = 1;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {
                                    WaitIndicator Indicatior = new WaitIndicator();
                                    Indicatior.Show();

                                    FetchData();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Appointment Cancelled Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    msgW1.Show();

                                    ClickedFlag = 0;

                                    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);

                                    EmailClient.SendEmailCompleted += (ea, args) =>
                                    {
                                        if (args.Error == null)
                                        {
                                            ClickedFlag = 0;
                                            Indicatior.Close();
                                        }
                                        else
                                        {
                                            Indicatior.Close();
                                            MessageBoxControl.MessageBoxChildWindow msgW5 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW5.Show();
                                            ClickedFlag = 0;
                                        }
                                    };
                                    if (item.Email != null && item.Email != "")
                                    {
                                        EmailClient.SendEmailAsync(Email, item.Email, "Appointment Cancel", ((AppointmentReasonWindow)sender).txtAppReason.Text);
                                        EmailClient.CloseAsync();
                                        ClickedFlag = 0;
                                        //EmailClient.SendEmailAsync("priyankag@seedhealthcare.com", Email, "Appointment Cancel", win1.txtAppReason.Text);
                                    }
                                    else
                                    {
                                        Indicatior.Close();
                                        ClickedFlag = 0;
                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                    msgW1.Show();
                                    Indicatior.Close();
                                    ClickedFlag = 0;
                                }
                            }
                        };

                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();

                    }
                    else
                        ClickedFlag = 0;
                }
                catch (Exception)
                {
                    Indicatior.Close();
                    ClickedFlag = 0;
                    throw;


                }
            }
        }

        void win1_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }



        /// <summary>
        /// Purpose:To close appointment list window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((ChildWindow)(this.Parent)).Close();

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Purpose:Patient registraion using  appointment list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool DailogResult = false;
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {

            if (OnSaveButton_Click != null)
            {
                DailogResult = true;
                OnSaveButton_Click(this, new RoutedEventArgs());
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                try
                {
                    ((ChildWindow)(this.Parent)).Close();
                }
                catch (Exception ex)
                {

                }

            }
        }

        /// <summary>
        /// Purpose:Add visit of selected patient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPatient_Click(object sender, RoutedEventArgs e)
        {
            if (item.AppointmentDate == DateTime.Now.Date.Date)
            {
                if (item.MrNo == "")
                {
                    string msgTitle = "";
                    string msgText = "This Patient Is Not Registered, Do You Want To Continue With Registration?";

                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW5.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW5_OnMessageBoxClosed);

                    msgW5.Show();

                }
                else
                {
                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.PatientDetails.GeneralDetails.PatientID = item.PatientId;
                    //BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.PatientDetails.GeneralDetails.UnitId = item.PatientUnitId;



                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;

                            EncounterWindow formEncounter = new EncounterWindow();
                            formEncounter.VAppointmentID = item.AppointmentID;
                            formEncounter.Initiate("NEW");

                            UIElement MyContent = formEncounter;
                            ((IApplicationConfiguration)App.Current).OpenMainContent(MyContent);

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            else
            {
                string msgTitle = "";
                string msgText = "You Cannot Mark Visit For This Appointment";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "You Cannot Mark Visit For This Appointment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
                item.VisitMark = false;
            }
        }
        /// <summary>
        /// Purpose:Registration of selected patient
        /// </summary>
        /// <param name="result"></param>
        void msgW5_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

                PatientRegistration Win = new PatientRegistration();
                Win.mAppointmentID = item.AppointmentID;

                Win.Initiate("NEWApp");
                UIElement MyData = Win;
                ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);



            }
            else
            {
                item.VisitMark = false;
            }



        }

        /// <summary>
        /// Purpose:Print appointment details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtFT = null;
            //DateTime? dtTT = null;
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
           // if (dgAppointmentList.ItemsSource == null)
            if(DataList.IsEmpty)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "There are no reports to print", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

            }
            else
            {

                if (dtpAppointmentSummaryF.SelectedDate != null)
                {
                    dtFT = dtpAppointmentSummaryF.SelectedDate.Value.Date.Date;
                }
                if (dtpAppointmentSummaryT.SelectedDate != null)
                {
                    dtTT = dtpAppointmentSummaryT.SelectedDate.Value.Date.Date;
                }
                long clinic = 0;
                long dept = 0;
                long doc = 0;
                long stat = 0;
                long type = 0;
                long spRegistrationId = 0;

                if (cmbUnitAppointmentSummary.SelectedItem != null)
                {
                    clinic = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                }
                if (cmbDepartmentAppointmentSummary.SelectedItem != null)
                {
                    dept = ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem).ID;
                }
                if (cmbDoctorAppointmentSummary.SelectedItem != null)
                {
                    doc = ((MasterListItem)cmbDoctorAppointmentSummary.SelectedItem).ID;
                }
                if (cmbAppointmentStat.SelectedItem != null)
                {
                    stat = ((MasterListItem)cmbAppointmentStat.SelectedItem).ID;
                }
                if (cmbAppointmentType.SelectedItem != null)
                {
                    if (((MasterListItem)cmbAppointmentType.SelectedItem).ID != 2)
                    {
                        type = ((MasterListItem)cmbAppointmentType.SelectedItem).ID;

                    }
                    else
                        type = 0;
                }

                if (cmbSpRegistration.SelectedItem != null)
                {
                    spRegistrationId = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;
                }



                string URL;
                if (dtFT != null && dtTT != null)
                {
                    URL = "../Reports/OPD/AppointmentListReport.aspx?FromDate=" + dtFT.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtTT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + clinic + "&DepartmentID=" + dept + "&DoctorID=" + doc + "&AppointStatus=" + stat + "&VisitMark=" + type + "&SpecialRegistrationId=" + spRegistrationId;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/OPD/AppointmentListReport.aspx?ClinicID=" + clinic + "&DepartmentID=" + dept + "&DoctorID=" + doc;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
        }

        /// <summary>
        /// Purpose:Call when visit is added successfully
        /// </summary>
        /// 
        private void AddMarkVisit()
        {
            clsAddMarkVisitInAppointmenBizActionVO BizAction = new clsAddMarkVisitInAppointmenBizActionVO();
            BizAction.AppointmentDetails = item;

            BizAction.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsAddMarkVisitInAppointmenBizActionVO)arg.Result).AppointmentDetails != null)
                    {
                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdRescAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentStatusNew.Trim() != "Cancelled")   //Added by ajit date 17/10/2016
            {
                if ((clsAppointmentVO)dgAppointmentList.SelectedItem != null)
                {
                    if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentDate >= DateTime.Today)
                    {
                        clsCheckMarkVisitBizActionVO BizAction = new clsCheckMarkVisitBizActionVO();
                        BizAction.Details = item;
                        BizAction.ID = item.AppointmentID;
                        BizAction.VisitMark = item.VisitMark;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                Status = ((clsCheckMarkVisitBizActionVO)arg.Result).SuccessStatus;
                                if (Status == false)
                                {
                                    //if (((clsAppointmentVO)dgAppointmentList.SelectedItem).FromTime < DateTime.Now)
                                    //{
                                    //    string msgTitle = " ";
                                    //    string mesText = " Time Has Elapsed, Still You Want to Reschedule Your Appointment?";
                                    //    MessageBoxControl.MessageBoxChildWindow  msgw=
                                    //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, mesText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    //    msgw.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgReschedule_OnMessageBoxClosed);
                                    //    msgw.Show();
                                    //}
                                    if (((clsAppointmentVO)dgAppointmentList.SelectedItem).AppointmentDate >= DateTime.Today)
                                    {
                                        string msgTitle = "";
                                        string msgText = "Are You Sure, You want To Reschedule This Appointment?";
                                        MessageBoxControl.MessageBoxChildWindow msgW =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgReschedule_OnMessageBoxClosed);
                                        msgW.Show();
                                    }
                                }
                                else
                                {
                                    string msgTitle = "";
                                    string msgText = "You Cannot Reschedule This Appointment Because Visit Is already Marked For This Appointment";
                                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW2.Show();
                                }
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "You Cannot Reschedule Backdated Appointment";
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                              new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                    }
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Please Select Appointment";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else  //Added by ajit date 17/10/2016
            {
                string msgTitle = "";
                string msgText = "You Cannot Reschedule Cancel Appointment";;
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void msgReschedule_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                item.IsReschedule = true;
                clsAppointmentVO senvio = item;
                AppmentID = senvio.AppointmentID;
                //senvio.AppointmentID = 0;
               PatientAppointment objResche = new PatientAppointment(senvio);
               // frmReschedulePatientAppointment objResche = new frmReschedulePatientAppointment(senvio);
                objResche.OnSaveButton_Click += new RoutedEventHandler(winReschedule_OnSaveButton_Click);
                objResche.Show();
            }
        }
        void winReschedule_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                try
                {
                    if (item != null)
                    {
                        //clsCancelAppointmentBizActionVO BizAction = new clsCancelAppointmentBizActionVO();
                        //BizAction.AppointmentDetails = item;
                        //BizAction.AppointmentDetails.AppointmentID = AppmentID;
                        //BizAction.AppointmentDetails.AppCancelReason = "Reschedule Appointment";
                        //BizAction.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        //BizAction.AppointmentDetails.AppReschdule = true;

                        //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        //client.ProcessCompleted += (s, arg) =>
                        //{
                        //    if (arg.Error == null)
                        //    {
                        //        if (arg.Result != null)
                        //        {
                        //            WaitIndicator Indicatior = new WaitIndicator();
                        //            Indicatior.Show();

                        //            FetchData();
                        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //            new MessageBoxControl.MessageBoxChildWindow("", "Appointment Reschedule Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //            msgW1.Show();

                        //            ClickedFlag = 0;

                        //            Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                        //            EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);

                        //            EmailClient.SendEmailCompleted += (ea, args) =>
                        //            {
                        //                if (args.Error == null)
                        //                {
                        //                    ClickedFlag = 0;
                        //                    Indicatior.Close();
                        //                }
                        //                else
                        //                {
                        //                    Indicatior.Close();
                        //                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                        //                    new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //                    msgW5.Show();
                        //                    ClickedFlag = 0;
                        //                }
                        //            };
                        //            if (item.Email != null && item.Email != "")
                        //            {
                        //                EmailClient.SendEmailAsync(Email, item.Email, "Appointment Cancel", "Reschedule Appointment", ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, null);
                        //                EmailClient.CloseAsync();
                        //                ClickedFlag = 0;
                        //            }
                        //            else
                        //            {
                        //                Indicatior.Close();
                        //                ClickedFlag = 0;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //            msgW1.Show();
                        //            Indicatior.Close();
                        //            ClickedFlag = 0;
                        //        }
                        //    }
                        //};

                        //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        //client.CloseAsync();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Appointment Reschedule Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                FetchData();
                                ClickedFlag = 0;
                            }
                        };
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else
                        ClickedFlag = 0;
                }
                catch (Exception)
                {
                    Indicatior.Close();
                    ClickedFlag = 0;
                    throw;


                }
            }
        }
        private void cmbAppointStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbAppointmentStat.SelectedItem != null)
            //{
            //    if (Convert.ToInt32(((MasterListItem)cmbAppointmentStat.SelectedItem).ID) == 1)
            //    {
            //        cmdCancelAppointment.IsEnabled = false;
            //      //  cmdVisit.IsEnabled = false;
            //        cmbAppointmentStat.IsEnabled = false;
            //    }
            //    else
            //    {
            //        cmdCancelAppointment.IsEnabled = true;
            //       // cmdVisit.IsEnabled = true;
            //        cmbAppointmentStat.IsEnabled = true;
            //    }

            //}
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }
     
        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
          
            //if (e.KeyChar == (char)13)
            if (e.Key == Key.Enter)
            {
                FetchData();
            }

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
       

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
               ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtCont_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        if (objUser.IsCrossAppointment)
                        {
                            cmbUnitAppointmentSummary.IsEnabled = true;
                        }
                        else
                        {
                            cmbUnitAppointmentSummary.IsEnabled = false;

                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}



