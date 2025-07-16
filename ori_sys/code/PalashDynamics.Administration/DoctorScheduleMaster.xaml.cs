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
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using System.Text;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.DoctorScheduleMaster;

namespace PalashDynamics.Administration
{
    public partial class DoctorScheduleMaster : UserControl, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Variable Declaration
        private SwivelAnimation objAnimation;

        private long Sunday1 = 1;
        private long Sunday2 = 2;
        private long Sunday3 = 3;
        private long Sunday4 = 4;
        private long Sunday5 = 5;

        bool UseAppDoctorID = true;

        public long DUnitID { get; set; }
        public long DDepartmentID { get; set; }
        public long DDoctorID { get; set; }
        public bool Status { get; set; }

        public clsDoctorScheduleVO item = new clsDoctorScheduleVO();

        public long LDepartment{ get; set; }

        public string DayID { get; set; }
        public long chkUnitID { get; set; }
        public long chkDepartmentID { get; set; }
        public long chkDoctorID { get; set; }
        public string Schedule1_StartTime { get; set; }
        public string Schedule1_EndTime { get; set; }
        public string Schedule2_StartTime { get; set; }
        public string Schedule2_EndTime { get; set; }

        bool ScheduleStatus { get; set; }

        bool IsModify { get; set; }

#endregion

        #region Paging
        public PagedSortableCollectionView<clsDoctorScheduleVO> DataList { get; private set; }
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
                OnPropertyChanged("DataListPageSize");
            }
        }
        #endregion

        public DoctorScheduleMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsDoctorScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.DataPager.DataContext = DataList;
            this.dgDoctorList.DataContext = DataList;
            FetchData();

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        private void DoctorScheduleMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                
                ControlVisibility();
                this.DataContext = new clsDoctorScheduleVO()
                {
                    //UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID,
                    //DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID
                };
                SetCommandButtonState("New");
                FillUnitList();
                FillClinicList();
                //FetchData();
                SetComboboxValue();
                cmbClinic.Focus();
                cmbDoctorClinic.Focus();
                
            }
            IsPageLoded = true;
            cmbClinic.Focus();
            cmbClinic.UpdateLayout();
            cmbDoctorClinic.Focus();
            cmbDoctorClinic.UpdateLayout();

        }

        #region FillCombobox
        private void FillUnitList()
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

                    cmbDoctorClinic.ItemsSource = null;
                    cmbDoctorClinic.ItemsSource = objList;


                    if (this.DataContext != null)
                    {
                        cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;

                    }

                    if ((clsDoctorScheduleVO)dgDoctorList.SelectedItem != null)
                    {
                        cmbDoctorClinic.SelectedValue = item.UnitID;
                        cmbDoctorClinic.UpdateLayout();
                        
                    }
                }
                
             
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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

                    if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }
                    cmbDoctorDepartment.ItemsSource = null;
                    cmbDoctorDepartment.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDoctorDepartment.ItemsSource = objList;
                    cmbDoctorDepartment.SelectedItem = objList[0];

                   


                    if ((clsDoctorScheduleVO)dgDoctorList.SelectedItem != null)
                    {
                        cmbDoctorDepartment.SelectedValue = item.DepartmentID;
                        cmbDoctorDepartment.UpdateLayout();
                    }
                                                   

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        //private void FillDoctorList(long iDeptId)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_DoctorDepartmentView;
        //    if (iDeptId > 0)
        //        BizAction.Parent = new KeyValue { Key = iDeptId, Value = "DepartmentId" };
        //    BizAction.MasterList = new List<MasterListItem>();
        //    //PalashServiceClient Client = null;
        //    //Client = new PalashServiceClient();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

        //            cmbDoctor.ItemsSource = null;
        //            cmbDoctor.ItemsSource = objList;

        //            if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
        //                cmbDoctor.ItemsSource = objList;
        //                cmbDoctor.SelectedItem = objList[0];


        //            if ((clsDoctorScheduleVO)dgDoctorList.SelectedItem != null)
        //            {
        //                cmbDoctor.SelectedValue = item.DoctorScheduleID;
        //                cmbDoctor.UpdateLayout();
        //            }
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}


        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbDoctorClinic.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDoctorDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;

                    if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];

                    if ((clsDoctorScheduleVO)dgDoctorList.SelectedItem != null)
                    {
                       // cmbDoctor.SelectedValue = item.DoctorScheduleID;
                        cmbDoctor.UpdateLayout();
                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillClinicList()
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



                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillDepartment(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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

                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }
                    cmbDepartment.ItemsSource = null;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDepartment.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        cmbDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;

                    }


                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        #endregion

        #region Selected Index Changed
        private void cmbDoctorDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDoctorClinic.SelectedItem!=null && (MasterListItem)cmbDoctorDepartment.SelectedItem != null) 
                FillDoctor(((MasterListItem)cmbDoctorClinic.SelectedItem).ID,((MasterListItem)cmbDoctorDepartment.SelectedItem).ID);
        }

        private void cmbDoctorClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDoctorClinic.SelectedItem != null) 
                FillDepartmentList(((MasterListItem)cmbDoctorClinic.SelectedItem).ID);

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem != null) 
                FillDepartment(((MasterListItem)cmbClinic.SelectedItem).ID);
        }
        #endregion

        private void SetComboboxValue()
        {
            cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
            cmbDoctorDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;
            //cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorScheduleID;
            cmbClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
            cmbDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;

        }

        bool IsPageLoded = false;
     
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            cmbDoctorClinic.IsEnabled = true;
            cmbDoctorDepartment.IsEnabled = true;
            cmbDoctor.IsEnabled = true;
            IsModify = false;
  
            this.DataContext = new clsDoctorScheduleVO();
            ClearControl();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Doctor Schedule";

            cmbDoctorClinic.Focus();
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = new clsDoctorScheduleVO();
            ClearControl();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";


            objAnimation.Invoke(RotationType.Backward);
            
        }

        /// <summary>
        /// Purpose :Add doctor schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
           
            bool SaveSchedule = true;
            SaveSchedule = ValidateInputs();

            if (SaveSchedule == true  && UIValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor Schedule";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }

           
        }

        private void SaveDoctorSchedule()
        {
           
            clsAddDoctorScheduleMasterBizActionVO BizAction = new clsAddDoctorScheduleMasterBizActionVO();
            BizAction.DoctorScheduleDetails = new clsDoctorScheduleVO();

            if (cmbDoctorClinic.SelectedItem != null)
            {
                BizAction.DoctorScheduleDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
            }
            if (cmbDoctorDepartment.SelectedItem != null)
            {
                BizAction.DoctorScheduleDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
            }
            if (cmbDoctor.SelectedItem != null)
            {
                //BizAction.DoctorScheduleDetails.DoctorScheduleID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            }

            StringBuilder objDayId = new StringBuilder();
            StringBuilder objSchedule1_StartTime = new StringBuilder();
            StringBuilder objSchedule1_EndTime = new StringBuilder();
            StringBuilder objSchedule2_StartTime = new StringBuilder();
            StringBuilder objSchedule2_EndTime = new StringBuilder();
            StringBuilder objOncall = new StringBuilder();
            StringBuilder objOnSundayCall = new StringBuilder();




            if (chkSun.IsChecked == true)
            {
                // BizAction.DoctorScheduleDetails.DayId = Sunday;
                objDayId.Append((long)DayOfWeek.Sunday);
                
                if (tpFromTimeSun.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeSun.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeSun.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeSun.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");
                
                if (tpFromTimeSunday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeSunday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");
                
                if (tpToTimeSunday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeSunday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");

            }

            if (chkMon.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");




                //BizAction.DoctorScheduleDetails.DayId = Monday;
                objDayId.Append((long)DayOfWeek.Monday);

                if (tpFromTimeMon.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeMon.Value);
                }
                else
                  objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeMon.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeMon.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeMonday.Value != null)
                {

                    objSchedule2_StartTime.Append(tpFromTimeMonday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");
                
                if (tpToTimeMonday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeMonday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");
            }

            if (chkTue.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

                // BizAction.DoctorScheduleDetails.DayId = Tuesday;
                objDayId.Append((long)DayOfWeek.Tuesday);

                if (tpFromTimeTue.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeTue.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeTue.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeTue.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeTueday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeTueday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeTueday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeTueday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");
            }

            if (chkWed.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                //BizAction.DoctorScheduleDetails.DayId = Wednesday;
                objDayId.Append((long)DayOfWeek.Wednesday);

                if (tpFromTimeWed.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeWed.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeWed.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeWed.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeWedday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeWedday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeWedday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeWedday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");

            }

            if (chkThr.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

                //BizAction.DoctorScheduleDetails.DayId = Thursday;
                objDayId.Append((long)DayOfWeek.Thursday);

                if (tpFromTimeThr.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeThr.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeThr.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeThr.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeThrday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeThrday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeThrday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeThrday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");

            }

            if (chkFri.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                // BizAction.DoctorScheduleDetails.DayId = Friday;
                objDayId.Append((long)DayOfWeek.Friday);

                if (tpFromTimeFri.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeFri.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeFri.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeFri.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeFriday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeFriday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeFriday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeFriday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");
            }

            if (chkSat.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                // BizAction.DoctorScheduleDetails.DayId = Saturday;
                objDayId.Append((long)DayOfWeek.Saturday);

                if (tpFromTimeSat.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeSat.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeSat.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeSat.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeSatday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeSatday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeSatday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeSatday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");
            }

            //BizAction.DoctorScheduleDetails.DayId = objDayId.ToString();
            //BizAction.DoctorScheduleDetails.Schedule1_StartTime = objSchedule1_StartTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule1_EndTime = objSchedule1_EndTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule2_StartTime = objSchedule2_StartTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule2_EndTime = objSchedule2_EndTime.ToString();




            if (chkSunday.IsChecked == true)
            {
                objOncall.Append((long)DayOfWeek.Sunday);
            }
            if (chkMonday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Monday);
            }
            if (chkTuesday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Tuesday);
            }
            if (chkWednesday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Wednesday);
            }
            if (chkThursday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Thursday);

            }
            if (chkFriday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Friday);

            }
            if (chkSaturday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Saturday);
            }

            //BizAction.DoctorScheduleDetails.OnCall = objOncall.ToString();




            if (chk1stSunday.IsChecked == true)
            {
                objOnSundayCall.Append(Sunday1);
            }
            if (chk2ndSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");

                objOnSundayCall.Append(Sunday2);
            }
            if (chk3rdSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday3);
            }
            if (chk4thSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday4);
            }
            if (chk5thSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday5);
            }

           // BizAction.DoctorScheduleDetails.OnSundayCall = objOnSundayCall.ToString();

                      

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (sa, arg1) =>
            {
                if (arg1.Error == null)
                {
                            
                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Schedule Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Doctor Schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                SetCommandButtonState("New");
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                //SaveDoctorSchedule();
                CheckTime();
        }

        /// <summary>
        /// Purpose:Modify doctors existing record 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifySchedule = true;
            ModifySchedule = ValidateInputs();
            if (ModifySchedule == true  && UIValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update the Doctor Schedule";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();

            }

        }

        private void ModifySchedule()
        {
            clsAddDoctorScheduleMasterBizActionVO BizAction = new clsAddDoctorScheduleMasterBizActionVO();
            BizAction.DoctorScheduleDetails = new clsDoctorScheduleVO();
           // BizAction.DoctorScheduleDetails.DoctorScheduleDetailID = item.DoctorScheduleDetailID;

            if (cmbDoctorClinic.SelectedItem != null)
                BizAction.DoctorScheduleDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;

            if (cmbDoctorDepartment.SelectedItem != null)
                BizAction.DoctorScheduleDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;

            //if (cmbDoctor.SelectedItem != null)
               // BizAction.DoctorScheduleDetails.DoctorScheduleID = ((MasterListItem)cmbDoctor.SelectedItem).ID;


            StringBuilder objDayId = new StringBuilder();
            StringBuilder objSchedule1_StartTime = new StringBuilder();
            StringBuilder objSchedule1_EndTime = new StringBuilder();
            StringBuilder objSchedule2_StartTime = new StringBuilder();
            StringBuilder objSchedule2_EndTime = new StringBuilder();
            StringBuilder objOncall = new StringBuilder();
            StringBuilder objOnSundayCall = new StringBuilder();




            if (chkSun.IsChecked == true)
            {
                // BizAction.DoctorScheduleDetails.DayId = Sunday;
                objDayId.Append((long)DayOfWeek.Sunday);
                if (tpFromTimeSun.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeSun.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeSun.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeSun.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeSunday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeSunday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeSunday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeSunday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");


            }

            if (chkMon.IsChecked == true)
            {
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");




                //BizAction.DoctorScheduleDetails.DayId = Monday;
                objDayId.Append((long)DayOfWeek.Monday);
                if (tpFromTimeMon.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeMon.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeMon.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeMon.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeMonday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeMonday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeMonday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeMonday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");

            }

             if (chkTue.IsChecked == true)
             {
          
                if (objDayId.Length > 0) objDayId.Append(",");
                if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

                // BizAction.DoctorScheduleDetails.DayId = Tuesday;
                objDayId.Append((long)DayOfWeek.Tuesday);

                if (tpFromTimeTue.Value != null)
                {
                    objSchedule1_StartTime.Append(tpFromTimeTue.Value);
                }
                else
                    objSchedule1_StartTime.Append("01/01/1900");

                if (tpToTimeTue.Value != null)
                {
                    objSchedule1_EndTime.Append(tpToTimeTue.Value);
                }
                else
                    objSchedule1_EndTime.Append("01/01/1900");

                if (tpFromTimeTueday.Value != null)
                {
                    objSchedule2_StartTime.Append(tpFromTimeTueday.Value);
                }
                else
                    objSchedule2_StartTime.Append("01/01/1900");

                if (tpToTimeTueday.Value != null)
                {
                    objSchedule2_EndTime.Append(tpToTimeTueday.Value);
                }
                else
                    objSchedule2_EndTime.Append("01/01/1900");
            }

             if (chkWed.IsChecked == true)
             {
                 if (objDayId.Length > 0) objDayId.Append(",");
                 if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                 if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                 if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                 if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                 //BizAction.DoctorScheduleDetails.DayId = Wednesday;
                 objDayId.Append((long)DayOfWeek.Wednesday);

                 if (tpFromTimeWed.Value != null)
                 {
                     objSchedule1_StartTime.Append(tpFromTimeWed.Value);
                 }
                 else
                     objSchedule1_StartTime.Append("01/01/1900");

                 if (tpToTimeWed.Value != null)
                 {
                     objSchedule1_EndTime.Append(tpToTimeWed.Value);
                 }
                 else
                     objSchedule1_EndTime.Append("01/01/1900");

                 if (tpFromTimeWedday.Value != null)
                 {
                     objSchedule2_StartTime.Append(tpFromTimeWedday.Value);
                 }
                 else
                     objSchedule2_StartTime.Append("01/01/1900");

                 if (tpToTimeWedday.Value != null)
                 {
                     objSchedule2_EndTime.Append(tpToTimeWedday.Value);
                 }
                 else
                     objSchedule2_EndTime.Append("01/01/1900");

             }

             if (chkThr.IsChecked == true)
             {
                 if (objDayId.Length > 0) objDayId.Append(",");
                 if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                 if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                 if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                 if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

                 //BizAction.DoctorScheduleDetails.DayId = Thursday;
                 objDayId.Append((long)DayOfWeek.Thursday);

                 if (tpFromTimeThr.Value != null)
                 {
                     objSchedule1_StartTime.Append(tpFromTimeThr.Value);
                 }
                 else
                     objSchedule1_StartTime.Append("01/01/1900");

                 if (tpToTimeThr.Value != null)
                 {
                     objSchedule1_EndTime.Append(tpToTimeThr.Value);
                 }
                 else
                     objSchedule1_EndTime.Append("01/01/1900");

                 if (tpFromTimeThrday.Value != null)
                 {
                     objSchedule2_StartTime.Append(tpFromTimeThrday.Value);
                 }
                 else
                     objSchedule2_StartTime.Append("01/01/1900");

                 if (tpToTimeThrday.Value != null)
                 {
                     objSchedule2_EndTime.Append(tpToTimeThrday.Value);
                 }
                 else
                     objSchedule2_EndTime.Append("01/01/1900");

             }

             if (chkFri.IsChecked == true)
             {
                 if (objDayId.Length > 0) objDayId.Append(",");
                 if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                 if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                 if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                 if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                 // BizAction.DoctorScheduleDetails.DayId = Friday;
                 objDayId.Append((long)DayOfWeek.Friday);

                 if (tpFromTimeFri.Value != null)
                 {
                     objSchedule1_StartTime.Append(tpFromTimeFri.Value);
                 }
                 else
                     objSchedule1_StartTime.Append("01/01/1900");

                 if (tpToTimeFri.Value != null)
                 {
                     objSchedule1_EndTime.Append(tpToTimeFri.Value);
                 }
                 else
                     objSchedule1_EndTime.Append("01/01/1900");

                 if (tpFromTimeFriday.Value != null)
                 {
                     objSchedule2_StartTime.Append(tpFromTimeFriday.Value);
                 }
                 else
                     objSchedule2_StartTime.Append("01/01/1900");

                 if (tpToTimeFriday.Value != null)
                 {
                     objSchedule2_EndTime.Append(tpToTimeFriday.Value);
                 }
                 else
                     objSchedule2_EndTime.Append("01/01/1900");
             }

             if (chkSat.IsChecked == true)
             {
                 if (objDayId.Length > 0) objDayId.Append(",");
                 if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
                 if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
                 if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
                 if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


                 // BizAction.DoctorScheduleDetails.DayId = Saturday;
                 objDayId.Append((long)DayOfWeek.Saturday);

                 if (tpFromTimeSat.Value != null)
                 {
                     objSchedule1_StartTime.Append(tpFromTimeSat.Value);
                 }
                 else
                     objSchedule1_StartTime.Append("01/01/1900");

                 if (tpToTimeSat.Value != null)
                 {
                     objSchedule1_EndTime.Append(tpToTimeSat.Value);
                 }
                 else
                     objSchedule1_EndTime.Append("01/01/1900");

                 if (tpFromTimeSatday.Value != null)
                 {
                     objSchedule2_StartTime.Append(tpFromTimeSatday.Value);
                 }
                 else
                     objSchedule2_StartTime.Append("01/01/1900");

                 if (tpToTimeSatday.Value != null)
                 {
                     objSchedule2_EndTime.Append(tpToTimeSatday.Value);
                 }
                 else
                     objSchedule2_EndTime.Append("01/01/1900");
             }

            //BizAction.DoctorScheduleDetails.DayId = objDayId.ToString();
            //BizAction.DoctorScheduleDetails.Schedule1_StartTime = objSchedule1_StartTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule1_EndTime = objSchedule1_EndTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule2_StartTime = objSchedule2_StartTime.ToString();
            //BizAction.DoctorScheduleDetails.Schedule2_EndTime = objSchedule2_EndTime.ToString();




            if (chkSunday.IsChecked == true)
            {
                objOncall.Append((long)DayOfWeek.Sunday);
            }
            if (chkMonday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Monday);
            }
            if (chkTuesday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Tuesday);
            }
            if (chkWednesday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Wednesday);
            }
            if (chkThursday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Thursday);

            }
            if (chkFriday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Friday);

            }
            if (chkSaturday.IsChecked == true)
            {
                if (objOncall.Length > 0) objOncall.Append(",");

                objOncall.Append((long)DayOfWeek.Saturday);
            }

          //  BizAction.DoctorScheduleDetails.OnCall = objOncall.ToString();




            if (chk1stSunday.IsChecked == true)
            {
                objOnSundayCall.Append(Sunday1);
            }
            if (chk2ndSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");

                objOnSundayCall.Append(Sunday2);
            }
            if (chk3rdSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday3);
            }
            if (chk4thSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday4);
            }
            if (chk5thSunday.IsChecked == true)
            {
                if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
                objOnSundayCall.Append(Sunday5);
            }

           // BizAction.DoctorScheduleDetails.OnSundayCall = objOnSundayCall.ToString();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    
                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);
                    

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Schedule Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while Updating Schedule..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                SetCommandButtonState("New");
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                CheckTime();
             
               

        }

        /// <summary>
        /// Purpose:Get doctor schedule using search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }


        private void FetchData()
        {

            clsGetDoctorScheduleMasterListBizActionVO BizAction = new clsGetDoctorScheduleMasterListBizActionVO();


            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID !=0)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 0;
            }

            //if (BizAction.UnitID == null || BizAction.UnitID == 0)
            //{
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}
            if (cmbDepartment.SelectedItem != null)
                BizAction.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
           

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //dgDoctorList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).DoctorScheduleList != null)
                    {
                        //dgDoctorList.ItemsSource = ((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).DoctorScheduleList;

                        clsGetDoctorScheduleMasterListBizActionVO result = arg.Result as clsGetDoctorScheduleMasterListBizActionVO;

                        if (result.DoctorScheduleList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).TotalRows;
                            foreach (clsDoctorScheduleVO item in result.DoctorScheduleList)
                            {
                                DataList.Add(item);
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


        /// <summary>
        /// Purpose:Get data in respective controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void hlbViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearControl();
              

               // if ((clsDoctorScheduleVO)dgDoctorList.SelectedItem != null)
               // {
               //     item = ((clsDoctorScheduleVO)dgDoctorList.SelectedItem);
               // }

               // FillUnitList();
               // //string ObjDayId = item.DayId;
               // //string ObjSchedule1_StartTime = item.Schedule1_StartTime;
               // //string ObjSchedule1_EndTime = item.Schedule1_EndTime;
               // //string ObjSchedule2_StartTime = item.Schedule2_StartTime;
               // //string ObjSchedule2_EndTime = item.Schedule2_EndTime;
               // //string ObjOnCall = item.OnCall;
               // //string ObjSunday = item.OnSundayCall;

               // char[] Splitchar = { ',' };

               // string[] ObjDayIdString = ObjDayId.Split(Splitchar);
               // string[] ObjSchedule1_StartTimeString = null;
               // string[] ObjSchedule1_EndTimeString = null;


               // if (ObjSchedule1_StartTime != null)
               // {
               //      ObjSchedule1_StartTimeString = ObjSchedule1_StartTime.Split(Splitchar);
               // }
               // if (ObjSchedule1_EndTime != null)
               // {
               //      ObjSchedule1_EndTimeString = ObjSchedule1_EndTime.Split(Splitchar);
               // }

               // string[] ObjSchedule2_StartTimeString = null;
               // string[] ObjSchedule2_EndTimeString = null;
                

               // if (ObjSchedule2_StartTime != null)
               // {
               //     ObjSchedule2_StartTimeString = ObjSchedule2_StartTime.Split(Splitchar);
               // }
               // if (ObjSchedule2_EndTime != null)
               // {
               //     ObjSchedule2_EndTimeString = ObjSchedule2_EndTime.Split(Splitchar);
               // }
                

               
               //string[] ObjOnCallString = ObjOnCall.Split(Splitchar);
                
                
               //string[] ObjSundayString = ObjSunday.Split(Splitchar);
                

               // int count = 0;

               // for (count = 0; count <= ObjDayIdString.Length - 1; count++)
               // {

               //     switch (ObjDayIdString[count])
               //     {
               //         case "0":
               //             //if (ObjDayIdString.Length > 0)
               //             //{
               //                 chkSun.IsChecked = true;

               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeSun.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeSun.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);
               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeSun.Value = null;
               //                     }
               //                     else
               //                         tpToTimeSun.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);
               //                 }

               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeSunday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeSunday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeSunday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeSunday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);
               //                 }
               //             //}
               //                 break;

                            
               //         case "1":
               //                 //if (ObjDayIdString.Length > 1)
               //                 //{
               //                     chkMon.IsChecked = true;

               //                     if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                     {
               //                         if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                         {
               //                             tpFromTimeMon.Value = null;
               //                         }
               //                         else
               //                             tpFromTimeMon.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);
               //                     }
               //                     if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                     {
               //                         if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                         {
               //                             tpToTimeMon.Value = null;
               //                         }
               //                         else
               //                             tpToTimeMon.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                     }
               //                     if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                     {
               //                         if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                         {
               //                             tpFromTimeMonday.Value = null;
               //                         }
               //                         else
               //                             tpFromTimeMonday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                     }
               //                     if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                     {
               //                         if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                         {
               //                             tpToTimeMonday.Value = null;

               //                         }
               //                         else
               //                             tpToTimeMonday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);
               //                     }
               //                // }
               //             break;

               //         case "2":
               //             //if (ObjDayIdString.Length > 2)
               //             //{
               //                 chkTue.IsChecked = true;
               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeTue.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeTue.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeTue.Value = null;
               //                     }
               //                     else
               //                         tpToTimeTue.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                 }

               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeTueday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeTueday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeTueday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeTueday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);
               //                 }
               //             //}
               //             break;

               //         case "3":
               //             //if (ObjDayIdString.Length > 3)
               //             //{
               //                 chkWed.IsChecked = true;
               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeWed.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeWed.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);
               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeWed.Value = null;
               //                     }
               //                     else
               //                         tpToTimeWed.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeWedday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeWedday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeWedday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeWedday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);
               //                 }
               //             //}
               //             break;

               //         case "4":
               //             //if (ObjDayIdString.Length > 4)
               //             //{
               //                 chkThr.IsChecked = true;
               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeThr.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeThr.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeThr.Value = null;
               //                     }
               //                     else
               //                         tpToTimeThr.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeThrday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeThrday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeThrday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeThrday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);

               //                 }
               //             //}
               //             break;

               //         case "5":
               //             //if (ObjDayIdString.Length > 5)
               //             //{
               //                 chkFri.IsChecked = true;
               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeFri.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeFri.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeFri.Value = null;
               //                     }
               //                     else
               //                         tpToTimeFri.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                 }

               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeFriday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeFriday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeFriday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeFriday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);

               //                 }
               //            // }
               //             break;

               //         case "6":
               //             //if (ObjDayIdString.Length > 6)
               //             //{
               //                 chkSat.IsChecked = true;

               //                 if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeSat.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeSat.Value = Convert.ToDateTime(ObjSchedule1_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule1_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeSat.Value = null;
               //                     }
               //                     else
               //                         tpToTimeSat.Value = Convert.ToDateTime(ObjSchedule1_EndTimeString[count]);

               //                 }

               //                 if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_StartTimeString[count] == "01/01/1900")
               //                     {
               //                         tpFromTimeSatday.Value = null;
               //                     }
               //                     else
               //                         tpFromTimeSatday.Value = Convert.ToDateTime(ObjSchedule2_StartTimeString[count]);

               //                 }
               //                 if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
               //                 {
               //                     if (ObjSchedule2_EndTimeString[count] == "01/01/1900")
               //                     {
               //                         tpToTimeSatday.Value = null;
               //                     }
               //                     else
               //                         tpToTimeSatday.Value = Convert.ToDateTime(ObjSchedule2_EndTimeString[count]);
               //                 }
               //             //}

               //             break;


               //     }
                //}



                //for (int count1 = 0; count1 <= ObjOnCallString.Length - 1; count1++)
                //{
                //    switch (ObjOnCallString[count1])
                //    {
                //        case "0": chkSunday.IsChecked = true;
                //            break;

                //        case "1": chkMonday.IsChecked = true;
                //            break;

                //        case "2": chkTuesday.IsChecked = true;
                //            break;

                //        case "3": chkWednesday.IsChecked = true;
                //            break;

                //        case "4": chkThursday.IsChecked = true;
                //            break;

                //        case "5": chkFriday.IsChecked = true;
                //            break;

                //        case "6": chkSaturday.IsChecked = true;
                //            break;
                //    }
                //}


                //for (int count2 = 0; count2 <= ObjSundayString.Length - 1; count2++)
                //{
                //    switch (ObjSundayString[count2])
                //    {
                //        case "1": chk1stSunday.IsChecked = true;
                //            break;

                //        case "2": chk2ndSunday.IsChecked = true;
                //            break;

                //        case "3": chk3rdSunday.IsChecked = true;
                //            break;

                //        case "4": chk4thSunday.IsChecked = true;
                //            break;

                //        case "5": chk5thSunday.IsChecked = true;
                //            break;
                //    }
                //}



                SetCommandButtonState("Modify");

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + item.DoctorName;

                
                objAnimation.Invoke(RotationType.Forward);


                cmbDoctorClinic.IsEnabled = false;
                cmbDoctorDepartment.IsEnabled = false;
                cmbDoctor.IsEnabled = false;

                cmbDoctorClinic.IsTabStop = false;
                cmbDoctorDepartment.IsTabStop = false;
                cmbDoctor.IsTabStop = false;

                IsModify = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Purpose:To clear control.
        /// </summary>
        private void ClearControl()
        {
            chkSun.IsChecked = false;
            chkMon.IsChecked = false;
            chkTue.IsChecked = false;
            chkWed.IsChecked = false;
            chkThr.IsChecked = false;
            chkFri.IsChecked = false;
            chkSat.IsChecked = false;

            tpFromTimeSun.Value = null;
            tpFromTimeMon.Value = null;
            tpFromTimeTue.Value = null;
            tpFromTimeWed.Value = null;
            tpFromTimeThr.Value = null;
            tpFromTimeFri.Value = null;
            tpFromTimeSat.Value = null;

            tpToTimeSun.Value = null;
            tpToTimeMon.Value = null;
            tpToTimeTue.Value = null;
            tpToTimeWed.Value = null;
            tpToTimeThr.Value = null;
            tpToTimeFri.Value = null;
            tpToTimeSat.Value = null;

            tpFromTimeSunday.Value = null;
            tpFromTimeMonday.Value = null;
            tpFromTimeTueday.Value = null;
            tpFromTimeWedday.Value = null;
            tpFromTimeThrday.Value = null;
            tpFromTimeFriday.Value = null;
            tpFromTimeSatday.Value = null;

            tpToTimeSunday.Value = null;
            tpToTimeMonday.Value = null;
            tpToTimeTueday.Value = null;
            tpToTimeWedday.Value = null;
            tpToTimeThrday.Value = null;
            tpToTimeFriday.Value = null;
            tpToTimeSatday.Value = null;


            chkSunday.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThursday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;

            chk1stSunday.IsChecked = false;
            chk2ndSunday.IsChecked = false;
            chk3rdSunday.IsChecked = false;
            chk4thSunday.IsChecked = false;
            chk5thSunday.IsChecked = false;

            cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
            cmbDoctorDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;
            //cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorScheduleID;


        }

        /// <summary>
        /// Purpose:To check control visibility.
        /// </summary>
        private void ControlVisibility()
        {
            tpFromTimeSun.IsEnabled = false;
            tpFromTimeMon.IsEnabled = false;
            tpFromTimeTue.IsEnabled = false;
            tpFromTimeWed.IsEnabled = false;
            tpFromTimeThr.IsEnabled = false;
            tpFromTimeFri.IsEnabled = false;
            tpFromTimeSat.IsEnabled = false;

            tpToTimeSun.IsEnabled = false;
            tpToTimeMon.IsEnabled = false;
            tpToTimeTue.IsEnabled = false;
            tpToTimeWed.IsEnabled = false;
            tpToTimeThr.IsEnabled = false;
            tpToTimeFri.IsEnabled = false;
            tpToTimeSat.IsEnabled = false;

            tpFromTimeSunday.IsEnabled = false;
            tpFromTimeMonday.IsEnabled = false;
            tpFromTimeTueday.IsEnabled = false;
            tpFromTimeWedday.IsEnabled = false;
            tpFromTimeThrday.IsEnabled = false;
            tpFromTimeFriday.IsEnabled = false;
            tpFromTimeSatday.IsEnabled = false;

            tpToTimeSunday.IsEnabled = false;
            tpToTimeMonday.IsEnabled = false;
            tpToTimeTueday.IsEnabled = false;
            tpToTimeWedday.IsEnabled = false;
            tpToTimeThrday.IsEnabled = false;
            tpToTimeFriday.IsEnabled = false;
            tpToTimeSatday.IsEnabled = false;

        }

        /// <summary>
        /// Purpose:To check validations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Validations
        private void chkSun_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeSun.IsEnabled = true;
            tpToTimeSun.IsEnabled = true;
            tpFromTimeSunday.IsEnabled = true;
            tpToTimeSunday.IsEnabled = true;
        }


        private void chkSun_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeSun.IsEnabled = false;
            tpToTimeSun.IsEnabled = false;
            tpFromTimeSunday.IsEnabled = false;
            tpToTimeSunday.IsEnabled = false;

        }

        private void chkMon_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeMon.IsEnabled = true;
            tpToTimeMon.IsEnabled = true;
            tpFromTimeMonday.IsEnabled = true;
            tpToTimeMonday.IsEnabled = true;

        }

        private void chkMon_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeMon.IsEnabled = false;
            tpToTimeMon.IsEnabled = false;
            tpFromTimeMonday.IsEnabled = false;
            tpToTimeMonday.IsEnabled = false;

        }

        private void chkTue_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeTue.IsEnabled = true;
            tpToTimeTue.IsEnabled = true;
            tpFromTimeTueday.IsEnabled = true;
            tpToTimeTueday.IsEnabled = true;
        }

        private void chkTue_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeTue.IsEnabled = false;
            tpToTimeTue.IsEnabled = false;
            tpFromTimeTueday.IsEnabled = false;
            tpToTimeTueday.IsEnabled = false;

        }

        private void chkWed_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeWed.IsEnabled = true;
            tpToTimeWed.IsEnabled = true;
            tpFromTimeWedday.IsEnabled = true;
            tpToTimeWedday.IsEnabled = true;

        }

        private void chkWed_Unchecked(object sender, RoutedEventArgs e)
        {

            tpFromTimeWed.IsEnabled = false;
            tpToTimeWed.IsEnabled = false;
            tpFromTimeWedday.IsEnabled = false;
            tpToTimeWedday.IsEnabled = false;

        }

        private void chkThr_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeThr.IsEnabled = true;
            tpToTimeThr.IsEnabled = true;
            tpFromTimeThrday.IsEnabled = true;
            tpToTimeThrday.IsEnabled = true;
        }

        private void chkThr_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeThr.IsEnabled = false;
            tpToTimeThr.IsEnabled = false;
            tpFromTimeThrday.IsEnabled = false;
            tpToTimeThrday.IsEnabled = false;

        }

        private void chkFri_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeFri.IsEnabled = true;
            tpToTimeFri.IsEnabled = true;
            tpFromTimeFriday.IsEnabled = true;
            tpToTimeFriday.IsEnabled = true;
        }

        private void chkFri_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeFri.IsEnabled = false;
            tpToTimeFri.IsEnabled = false;
            tpFromTimeFriday.IsEnabled =false;
            tpToTimeFriday.IsEnabled = false;

        }

        private void chkSat_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeSat.IsEnabled = true;
            tpToTimeSat.IsEnabled = true;
            tpFromTimeSatday.IsEnabled = true;
            tpToTimeSatday.IsEnabled = true;

        }

        private void chkSat_Unchecked(object sender, RoutedEventArgs e)
        {
            tpFromTimeSat.IsEnabled = false;
            tpToTimeSat.IsEnabled = false;
            tpFromTimeSatday.IsEnabled = false;
            tpToTimeSatday.IsEnabled = false;

        }


        private bool ValidateInputs()
        {
            bool result = true;


            if ((MasterListItem)cmbDoctorClinic.SelectedItem == null)
            {
                cmbDoctorClinic.TextBox.SetValidation("Please Select Clinic");
                cmbDoctorClinic.TextBox.RaiseValidationError();
                cmbDoctorClinic.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID == 0)
            {
                cmbDoctorClinic.TextBox.SetValidation("Please Select Clinic");
                cmbDoctorClinic.TextBox.RaiseValidationError();
                cmbDoctorClinic.Focus();
                result = false;
             
            }
            else
                cmbDoctorClinic.TextBox.ClearValidationError();


            if ((MasterListItem)cmbDoctorDepartment.SelectedItem == null)
            {
                cmbDoctorDepartment.TextBox.SetValidation("Please Select Department");
                cmbDoctorDepartment.TextBox.RaiseValidationError();
                cmbDoctorDepartment.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDoctorDepartment.SelectedItem).ID == 0)
            {
                cmbDoctorDepartment.TextBox.SetValidation("Please Select Department");
                cmbDoctorDepartment.TextBox.RaiseValidationError();
                cmbDoctorDepartment.Focus();
                result = false;
            
            }
            else
                cmbDoctorDepartment.TextBox.ClearValidationError();


            if ((MasterListItem)cmbDoctor.SelectedItem == null)
            {
                cmbDoctor.TextBox.SetValidation("Please Select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
            {
                cmbDoctor.TextBox.SetValidation("Please Select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                result = false;
              
            }
            else
                cmbDoctor.TextBox.ClearValidationError();

            

          


            if (chkSun.IsChecked == true)
            {
                if ((tpFromTimeSun.Value == null && tpToTimeSun.Value == null) && (tpFromTimeSunday.Value == null && tpToTimeSunday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Sunday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    result = false;
                    return result;
                }
            }



            if (chkMon.IsChecked == true)
            {
                if ((tpFromTimeMon.Value == null && tpToTimeMon.Value == null) && (tpFromTimeMonday.Value == null && tpToTimeMonday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Monday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;
                }
            }

            if (chkTue.IsChecked == true)
            {
                if ((tpFromTimeTue.Value == null && tpToTimeTue.Value == null) && (tpFromTimeTueday.Value == null && tpToTimeTueday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Tuesday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;

                }
            }

            if (chkWed.IsChecked == true)
            {
                if ((tpFromTimeWed.Value == null && tpToTimeWed.Value == null) && (tpFromTimeWedday.Value == null && tpToTimeWedday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Wednesday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;

                }
            }

            if (chkThr.IsChecked == true)
            {
                if ((tpFromTimeThr.Value == null && tpToTimeThr.Value == null) && (tpFromTimeThrday.Value == null && tpToTimeThrday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Thursday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;

                }
            }

            if (chkFri.IsChecked == true)
            {
                if ((tpFromTimeFri.Value == null && tpToTimeFri.Value == null) && (tpFromTimeFriday.Value == null && tpToTimeFriday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Friday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;

                }
            }


            if (chkSat.IsChecked == true)
            {
                if ((tpFromTimeSat.Value == null && tpToTimeSat.Value == null) && (tpFromTimeSatday.Value == null && tpToTimeSatday.Value == null))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule for Saturday", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();

                    result = false;
                    return result;

                }
            }

            if (chkMon.IsChecked != true && chkTue.IsChecked != true && chkWed.IsChecked != true && chkThr.IsChecked != true && chkFri.IsChecked != true && chkSat.IsChecked != true && chkSun.IsChecked != true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Schedule ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();

                result = false;
                return result;

            }

            



            return result;


        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion


        private void CheckTime()
        {
            bool Flag = false;
            clsCheckTimeForScheduleExistanceBizActionVO BizAction = new clsCheckTimeForScheduleExistanceBizActionVO();
            BizAction.Details = new List<clsDoctorScheduleDetailsVO>();

            if (cmbDoctorClinic.SelectedItem != null)
                BizAction.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;

            if (cmbDoctorDepartment.SelectedItem != null)
                BizAction.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;

            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details.Count == 0)
                        {
                            SaveDoctorSchedule();
                        }
                        if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details != null && ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details.Count != 0)
                        {

                            for (int i = 0; i < ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details.Count; i++)
                            {
                                chkUnitID = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].UnitID;
                                chkDepartmentID = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].DepartmentID;
                                //chkDoctorID = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].DoctorScheduleID;
                                long SelectedUnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
                                long SelectedDepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
                                long SelectedDoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;



                                
                                    //string ObjDayId = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].DayId;
                                    string ObjSchedule1_StartTime = null;
                                    string ObjSchedule1_EndTime = null;
                                    string ObjSchedule2_StartTime = null;
                                    string ObjSchedule2_EndTime = null;



                                    //if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule1_StartTime != null)
                                    //{
                                    //    ObjSchedule1_StartTime = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule1_StartTime;
                                    //}
                                    //if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule1_EndTime != null)
                                    //{
                                    //    ObjSchedule1_EndTime = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule1_EndTime;
                                    //}

                                    //if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule2_StartTime != null)
                                    //{
                                    //    ObjSchedule2_StartTime = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule2_StartTime;
                                    //}
                                    //if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule2_EndTime != null)
                                    //{
                                    //    ObjSchedule2_EndTime = ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details[i].Schedule2_EndTime;
                                    //}
                                    if (IsModify == false)
                                    {

                                        if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && ObjSchedule1_StartTime != null && ObjSchedule1_EndTime != null && ObjSchedule2_StartTime != null && ObjSchedule2_EndTime != null)
                                        {

                                            char[] Splitchar = { ',' };

                                            string[] ObjDayIdString = null;
                                            string[] ObjSchedule1_StartTimeString = null;
                                            string[] ObjSchedule1_EndTimeString = null;
                                            string[] ObjSchedule2_StartTimeString = null;
                                            string[] ObjSchedule2_EndTimeString = null;


                                            //if (ObjDayId != null)
                                            //{
                                            //    ObjDayIdString = ObjDayId.Split(Splitchar);
                                            //}
                                            if (ObjSchedule1_StartTime != null)
                                            {
                                                ObjSchedule1_StartTimeString = ObjSchedule1_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule1_EndTime != null)
                                            {
                                                ObjSchedule1_EndTimeString = ObjSchedule1_EndTime.Split(Splitchar);
                                            }

                                            if (ObjSchedule2_StartTime != null)
                                            {
                                                ObjSchedule2_StartTimeString = ObjSchedule2_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule2_EndTime != null)
                                            {
                                                ObjSchedule2_EndTimeString = ObjSchedule2_EndTime.Split(Splitchar);
                                            }

                                            if (Flag == true)
                                            {
                                                break;
                                            }



                                            for (int count = 0; count <= ObjDayIdString.Length - 1; count++)
                                            {
                                                Schedule1_StartTime = null;
                                                Schedule1_EndTime = null;
                                                Schedule2_StartTime = null;
                                                Schedule2_EndTime = null;
                                                DayID = (ObjDayIdString[count]);


                                                if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
                                                {

                                                    Schedule1_StartTime = (Convert.ToDateTime(ObjSchedule1_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
                                                {

                                                    Schedule1_EndTime = (Convert.ToDateTime(ObjSchedule1_EndTimeString[count])).ToString();
                                                }


                                                if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
                                                {

                                                    Schedule2_StartTime = (Convert.ToDateTime(ObjSchedule2_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
                                                {

                                                    Schedule2_EndTime = (Convert.ToDateTime(ObjSchedule2_EndTimeString[count])).ToString();
                                                }

                                    
                                                TimeSpan? Sc1FT = Convert.ToDateTime(Schedule1_StartTime).TimeOfDay;
                                                TimeSpan? Sc1TT = Convert.ToDateTime(Schedule1_EndTime).TimeOfDay;
                                                TimeSpan? Sc2FT = Convert.ToDateTime(Schedule2_StartTime).TimeOfDay;
                                                TimeSpan? Sc2TT = Convert.ToDateTime(Schedule2_EndTime).TimeOfDay;

                                                if (chkSun.IsChecked == true)
                                                {
                                                   
                                                    if (tpFromTimeSun.Value != null &&tpToTimeSun.Value !=null || tpFromTimeSunday.Value != null && tpToTimeSunday.Value!=null)
                                                    {
                                                        TimeSpan? FSun=null;
                                                        TimeSpan?TSun=null;
                                                        TimeSpan? FSunday=null;
                                                        TimeSpan? TSunday=null;
                                                       if (tpFromTimeSun.Value != null &&tpToTimeSun.Value !=null)
                                                       {
                                                          FSun= tpFromTimeSun.Value.Value.TimeOfDay;
                                                          TSun = tpToTimeSun.Value.Value.TimeOfDay;
                                                       }
                                                       if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                                                       {

                                                          FSunday = tpFromTimeSunday.Value.Value.TimeOfDay;
                                                          TSunday = tpToTimeSunday.Value.Value.TimeOfDay;
                                                       }
                                                    
                                                        if (DayID == "0")
                                                        {
                                                            if (FSun == Sc1FT && TSun == Sc1TT || FSunday == Sc2FT && TSunday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FSun == Sc2FT && TSun == Sc2TT || FSunday == Sc1FT && TSunday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                        }
                                                    }
                                                
                                                }
                                                if (chkMon.IsChecked == true)
                                                {
                                                    if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null || tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                    {
                                                        TimeSpan? FMon=null;
                                                        TimeSpan? TMon=null;
                                                        TimeSpan? FMonday=null;
                                                        TimeSpan? TMonday=null;
                                                        if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
                                                        {
                                                             FMon = tpFromTimeMon.Value.Value.TimeOfDay;
                                                             TMon = tpToTimeMon.Value.Value.TimeOfDay;
                                                        }
                                                        if(tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                        {

                                                            FMonday = tpFromTimeMonday.Value.Value.TimeOfDay;
                                                            TMonday = tpToTimeMonday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "1")
                                                        {
                                                            if (FMon == Sc1FT && TMon == Sc1TT || FMonday == Sc2FT && TMonday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                            else if (FMon == Sc2FT && TMon == Sc2TT || FMonday == Sc1FT && TMonday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (chkTue.IsChecked == true)
                                                {
                                                    if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null || tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                    {
                                                        TimeSpan? FTue = null;
                                                        TimeSpan? TTue = null;
                                                        TimeSpan? FTueday = null;
                                                        TimeSpan? TTueday = null;

                                                        if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
                                                        {
                                                             FTue = tpFromTimeTue.Value.Value.TimeOfDay;
                                                             TTue = tpToTimeTue.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                        {
                                                             FTueday = tpFromTimeTueday.Value.Value.TimeOfDay;
                                                             TTueday = tpToTimeTueday.Value.Value.TimeOfDay;
                                                        }
                                                    

                                                        if (DayID == "2")
                                                        {
                                                            if (FTue == Sc1FT && TTue == Sc1TT || FTueday == Sc2FT && TTueday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FTue == Sc2FT && TTue == Sc2TT || FTueday == Sc1FT && TTueday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkWed.IsChecked == true)
                                                {
                                                    if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null || tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                    {
                                                        TimeSpan? FWed = null;
                                                        TimeSpan? TWed = null;
                                                        TimeSpan? FWedday =  null;
                                                        TimeSpan? TWedday = null;
                                                        
                                                        if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
                                                        {
                                                             FWed = tpFromTimeWed.Value.Value.TimeOfDay;
                                                             TWed = tpToTimeWed.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                        {
                                                             FWedday = tpFromTimeWedday.Value.Value.TimeOfDay;
                                                             TWedday = tpToTimeWedday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "3")
                                                        {
                                                            if (FWed == Sc1FT && TWed == Sc1TT || FWedday == Sc2FT && TWedday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FWed == Sc2FT && TWed == Sc2TT || FWedday == Sc1FT && TWedday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                                if (chkThr.IsChecked == true)
                                                {
                                                    if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null || tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                    {
                                                        TimeSpan? FThr = null;
                                                        TimeSpan? TThr =  null;
                                                        TimeSpan? FThrday = null;
                                                        TimeSpan? TThrday = null;
                                                        if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
                                                        {
                                                             FThr = tpFromTimeThr.Value.Value.TimeOfDay;
                                                             TThr = tpToTimeThr.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                        {
                                                            FThrday = tpFromTimeThrday.Value.Value.TimeOfDay;
                                                            TThrday = tpToTimeThrday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "4")
                                                        {
                                                            if (FThr == Sc1FT && TThr == Sc1TT || FThrday == Sc2FT && TThrday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                            else if (FThr == Sc2FT && TThr == Sc2TT || FThrday == Sc1FT && FThrday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkFri.IsChecked == true)
                                                {
                                                    if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null || tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                    {
                                                        TimeSpan? FFri = null;
                                                        TimeSpan? TFri = null;
                                                        TimeSpan? FFriday =null;
                                                        TimeSpan? TFriday =null;

                                                        if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
                                                        {
                                                             FFri = tpFromTimeFri.Value.Value.TimeOfDay;
                                                             TFri = tpToTimeFri.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                        {
                                                             FFriday = tpFromTimeFriday.Value.Value.TimeOfDay;
                                                             TFriday = tpToTimeFriday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "5")
                                                        {
                                                            if (FFri == Sc1FT && TFri == Sc1TT || FFriday == Sc2FT && TFriday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                            else if (FFri == Sc2FT && TFri == Sc2TT || FFriday == Sc1FT && TFriday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }


                                                        }
                                                    }
                                                }
                                                if (chkSat.IsChecked == true)
                                                {
                                                    if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null || tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                    {

                                                        TimeSpan? FSat = null;
                                                        TimeSpan? TSat = null;
                                                        TimeSpan? FSatday = null;
                                                        TimeSpan? TSatday = null;

                                                        if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
                                                        {
                                                             FSat = tpFromTimeSat.Value.Value.TimeOfDay;
                                                             TSat = tpToTimeSat.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                        {
                                                             FSatday = tpFromTimeSatday.Value.Value.TimeOfDay;
                                                             TSatday = tpToTimeSatday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "6")
                                                        {
                                                            if (FSat == Sc1FT && TSat == Sc1TT || FSatday == Sc2FT && TSatday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                            else if (FSat == Sc2FT && TSat == Sc2TT || FSatday == Sc1FT && TSatday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        else if (chkUnitID != SelectedUnitID)
                                        {

                                            char[] Splitchar = { ',' };

                                            string[] ObjDayIdString = null;
                                            string[] ObjSchedule1_StartTimeString = null;
                                            string[] ObjSchedule1_EndTimeString = null;
                                            string[] ObjSchedule2_StartTimeString = null;
                                            string[] ObjSchedule2_EndTimeString = null;


                                            //if (ObjDayId != null)
                                            //{
                                            //    ObjDayIdString = ObjDayId.Split(Splitchar);
                                            //}
                                            if (ObjSchedule1_StartTime != null)
                                            {
                                                ObjSchedule1_StartTimeString = ObjSchedule1_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule1_EndTime != null)
                                            {
                                                ObjSchedule1_EndTimeString = ObjSchedule1_EndTime.Split(Splitchar);
                                            }

                                            if (ObjSchedule2_StartTime != null)
                                            {
                                                ObjSchedule2_StartTimeString = ObjSchedule2_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule2_EndTime != null)
                                            {
                                                ObjSchedule2_EndTimeString = ObjSchedule2_EndTime.Split(Splitchar);
                                            }

                                            if (Flag == true)
                                            {
                                                break;
                                            }



                                            for (int count = 0; count <= ObjDayIdString.Length - 1; count++)
                                            {
                                                Schedule1_StartTime = null;
                                                Schedule1_EndTime = null;
                                                Schedule2_StartTime = null;
                                                Schedule2_EndTime = null;
                                                DayID = (ObjDayIdString[count]);


                                                if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
                                                {

                                                    Schedule1_StartTime = (Convert.ToDateTime(ObjSchedule1_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
                                                {

                                                    Schedule1_EndTime = (Convert.ToDateTime(ObjSchedule1_EndTimeString[count])).ToString();
                                                }


                                                if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
                                                {

                                                    Schedule2_StartTime = (Convert.ToDateTime(ObjSchedule2_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
                                                {

                                                    Schedule2_EndTime = (Convert.ToDateTime(ObjSchedule2_EndTimeString[count])).ToString();
                                                }

                                                TimeSpan? Sc1FT = Convert.ToDateTime(Schedule1_StartTime).TimeOfDay;
                                                TimeSpan? Sc1TT = Convert.ToDateTime(Schedule1_EndTime).TimeOfDay;
                                                TimeSpan? Sc2FT = Convert.ToDateTime(Schedule2_StartTime).TimeOfDay;
                                                TimeSpan? Sc2TT = Convert.ToDateTime(Schedule2_EndTime).TimeOfDay;

                                                
                                              
                                                if (chkSun.IsChecked == true)
                                                {
                                                    if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null || tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                                                    {
                                                        TimeSpan? FSun = null;
                                                        TimeSpan? TSun = null;
                                                        TimeSpan? FSunday = null;
                                                        TimeSpan? TSunday = null;
                                                        if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
                                                        {
                                                            FSun = tpFromTimeSun.Value.Value.TimeOfDay;
                                                            TSun = tpToTimeSun.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                                                        {

                                                            FSunday = tpFromTimeSunday.Value.Value.TimeOfDay;
                                                            TSunday = tpToTimeSunday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "0")
                                                        {
                                                            //if (Convert.ToDateTime(tpFromTimeSun.Value.ToString()) == DBStartTime && Convert.ToDateTime(tpToTimeSun.Value.ToString()) == DBEndTime || Convert.ToDateTime(tpFromTimeSunday.Value.ToString()) == DBStartTime1 && Convert.ToDateTime(tpToTimeSunday.Value.ToString()) == DBEndTime1)
                                                            if (FSun == Sc1FT && TSun == Sc1TT || FSunday == Sc2FT && TSunday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FSun == Sc2FT && TSun == Sc2TT || FSunday == Sc1FT && TSunday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                                if (chkMon.IsChecked == true)
                                                {
                                                    if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null || tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                    {
                                                        TimeSpan? FMon=null;
                                                        TimeSpan? TMon=null;
                                                        TimeSpan? FMonday=null;
                                                        TimeSpan? TMonday=null;
                                                        if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
                                                        {
                                                             FMon = tpFromTimeMon.Value.Value.TimeOfDay;
                                                             TMon = tpToTimeMon.Value.Value.TimeOfDay;
                                                        }
                                                        if(tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                        {

                                                            FMonday = tpFromTimeMonday.Value.Value.TimeOfDay;
                                                            TMonday = tpToTimeMonday.Value.Value.TimeOfDay;
                                                        }


                                                        if (DayID == "1")
                                                        {
                                                            if (FMon == Sc1FT && TMon == Sc1TT || FMonday == Sc2FT && TMonday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FMon == Sc2FT && TMon == Sc2TT || FMonday == Sc1FT && TMonday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkTue.IsChecked == true)
                                                {
                                                    if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null || tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                    {
                                                        TimeSpan? FTue = null;
                                                        TimeSpan? TTue = null;
                                                        TimeSpan? FTueday = null;
                                                        TimeSpan? TTueday = null;

                                                        if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
                                                        {
                                                            FTue = tpFromTimeTue.Value.Value.TimeOfDay;
                                                            TTue = tpToTimeTue.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                        {
                                                            FTueday = tpFromTimeTueday.Value.Value.TimeOfDay;
                                                            TTueday = tpToTimeTueday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "2")
                                                        {
                                                            if (FTue == Sc1FT && TTue == Sc1TT || FTueday == Sc2FT && TTueday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FTue == Sc2FT && TTue == Sc2TT || FTueday == Sc1FT && TTueday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkWed.IsChecked == true)
                                                {
                                                    if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null || tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                    {
                                                        TimeSpan? FWed = null;
                                                        TimeSpan? TWed = null;
                                                        TimeSpan? FWedday = null;
                                                        TimeSpan? TWedday = null;

                                                        if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
                                                        {
                                                            FWed = tpFromTimeWed.Value.Value.TimeOfDay;
                                                            TWed = tpToTimeWed.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                        {
                                                            FWedday = tpFromTimeWedday.Value.Value.TimeOfDay;
                                                            TWedday = tpToTimeWedday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "3")
                                                        {
                                                            if (FWed == Sc1FT && TWed == Sc1TT || FWedday == Sc2FT && TWedday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FWed == Sc2FT && TWed == Sc2TT || FWedday == Sc1FT && TWedday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                                if (chkThr.IsChecked == true)
                                                {
                                                    if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null || tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                    {
                                                        TimeSpan? FThr = null;
                                                        TimeSpan? TThr = null;
                                                        TimeSpan? FThrday = null;
                                                        TimeSpan? TThrday = null;
                                                        if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
                                                        {
                                                            FThr = tpFromTimeThr.Value.Value.TimeOfDay;
                                                            TThr = tpToTimeThr.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                        {
                                                            FThrday = tpFromTimeThrday.Value.Value.TimeOfDay;
                                                            TThrday = tpToTimeThrday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "4")
                                                        {
                                                            if (FThr == Sc1FT && TThr == Sc1TT || FThrday == Sc2FT && FThrday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FThr == Sc2FT && TThr == Sc2TT || FThrday == Sc1FT && FThrday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkFri.IsChecked == true)
                                                {
                                                    if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null || tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                    {
                                                        TimeSpan? FFri = null;
                                                        TimeSpan? TFri = null;
                                                        TimeSpan? FFriday = null;
                                                        TimeSpan? TFriday = null;

                                                        if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
                                                        {
                                                            FFri = tpFromTimeFri.Value.Value.TimeOfDay;
                                                            TFri = tpToTimeFri.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                        {
                                                            FFriday = tpFromTimeFriday.Value.Value.TimeOfDay;
                                                            TFriday = tpToTimeFriday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "5")
                                                        {
                                                            if (FFri == Sc1FT && TFri == Sc1TT || FFriday == Sc2FT && TFriday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FFri == Sc2FT && TFri == Sc2TT || FFriday == Sc1FT && TFriday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }


                                                        }
                                                    }
                                                }
                                                if (chkSat.IsChecked == true)
                                                {
                                                    if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null || tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                    {
                                                        TimeSpan? FSat = null;
                                                        TimeSpan? TSat = null;
                                                        TimeSpan? FSatday = null;
                                                        TimeSpan? TSatday = null;

                                                        if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
                                                        {
                                                            FSat = tpFromTimeSat.Value.Value.TimeOfDay;
                                                            TSat = tpToTimeSat.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                        {
                                                            FSatday = tpFromTimeSatday.Value.Value.TimeOfDay;
                                                            TSatday = tpToTimeSatday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "6")
                                                        {
                                                            if (FSat == Sc1FT && TSat == Sc1TT || FSatday == Sc2FT && TSatday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                            else if (FSat == Sc2FT && TSat == Sc2TT || FSatday == Sc1FT && TSatday == Sc1TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        else
                                        {
                                            Flag = true;
                                        }
                                    }
                                    else if (IsModify == true)
                                    {
                                        if (chkUnitID == SelectedUnitID && chkDepartmentID == SelectedDepartmentID)
                                        {
                                            char[] Splitchar = { ',' };

                                            string[] ObjDayIdString = null;
                                            string[] ObjSchedule1_StartTimeString = null;
                                            string[] ObjSchedule1_EndTimeString = null;
                                            string[] ObjSchedule2_StartTimeString = null;
                                            string[] ObjSchedule2_EndTimeString = null;


                                            //if (ObjDayId != null)
                                            //{
                                            //    ObjDayIdString = ObjDayId.Split(Splitchar);
                                            //}
                                            if (ObjSchedule1_StartTime != null)
                                            {
                                                ObjSchedule1_StartTimeString = ObjSchedule1_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule1_EndTime != null)
                                            {
                                                ObjSchedule1_EndTimeString = ObjSchedule1_EndTime.Split(Splitchar);
                                            }

                                            if (ObjSchedule2_StartTime != null)
                                            {
                                                ObjSchedule2_StartTimeString = ObjSchedule2_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule2_EndTime != null)
                                            {
                                                ObjSchedule2_EndTimeString = ObjSchedule2_EndTime.Split(Splitchar);
                                            }

                                            if (Flag == true)
                                            {
                                                break;
                                            }



                                            for (int count = 0; count <= ObjDayIdString.Length - 1; count++)
                                            {
                                                Schedule1_StartTime = null;
                                                Schedule1_EndTime = null;
                                                Schedule2_StartTime = null;
                                                Schedule2_EndTime = null;
                                                DayID = (ObjDayIdString[count]);


                                                if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
                                                {

                                                    Schedule1_StartTime = (Convert.ToDateTime(ObjSchedule1_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
                                                {

                                                    Schedule1_EndTime = (Convert.ToDateTime(ObjSchedule1_EndTimeString[count])).ToString();
                                                }


                                                if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
                                                {

                                                    Schedule2_StartTime = (Convert.ToDateTime(ObjSchedule2_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
                                                {

                                                    Schedule2_EndTime = (Convert.ToDateTime(ObjSchedule2_EndTimeString[count])).ToString();
                                                }
                                                

                                            }
                                        }
                                        else if (chkUnitID != SelectedUnitID)
                                        {

                                            char[] Splitchar = { ',' };

                                            string[] ObjDayIdString = null;
                                            string[] ObjSchedule1_StartTimeString = null;
                                            string[] ObjSchedule1_EndTimeString = null;
                                            string[] ObjSchedule2_StartTimeString = null;
                                            string[] ObjSchedule2_EndTimeString = null;


                                            //if (ObjDayId != null)
                                            //{
                                            //    ObjDayIdString = ObjDayId.Split(Splitchar);
                                            //}
                                            if (ObjSchedule1_StartTime != null)
                                            {
                                                ObjSchedule1_StartTimeString = ObjSchedule1_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule1_EndTime != null)
                                            {
                                                ObjSchedule1_EndTimeString = ObjSchedule1_EndTime.Split(Splitchar);
                                            }

                                            if (ObjSchedule2_StartTime != null)
                                            {
                                                ObjSchedule2_StartTimeString = ObjSchedule2_StartTime.Split(Splitchar);
                                            }
                                            if (ObjSchedule2_EndTime != null)
                                            {
                                                ObjSchedule2_EndTimeString = ObjSchedule2_EndTime.Split(Splitchar);
                                            }

                                            if (Flag == true)
                                            {
                                                break;
                                            }



                                            for (int count = 0; count <= ObjDayIdString.Length - 1; count++)
                                            {
                                                Schedule1_StartTime = null;
                                                Schedule1_EndTime = null;
                                                Schedule2_StartTime = null;
                                                Schedule2_EndTime = null;
                                                DayID = (ObjDayIdString[count]);


                                                if (ObjSchedule1_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_StartTimeString[count].Trim()))
                                                {

                                                    Schedule1_StartTime = (Convert.ToDateTime(ObjSchedule1_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule1_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule1_EndTimeString[count].Trim()))
                                                {

                                                    Schedule1_EndTime = (Convert.ToDateTime(ObjSchedule1_EndTimeString[count])).ToString();
                                                }


                                                if (ObjSchedule2_StartTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_StartTimeString[count].Trim()))
                                                {

                                                    Schedule2_StartTime = (Convert.ToDateTime(ObjSchedule2_StartTimeString[count])).ToString();
                                                }
                                                if (ObjSchedule2_EndTimeString != null && !string.IsNullOrEmpty(ObjSchedule2_EndTimeString[count].Trim()))
                                                {

                                                    Schedule2_EndTime = (Convert.ToDateTime(ObjSchedule2_EndTimeString[count])).ToString();
                                                }

                                                TimeSpan? Sc1FT = Convert.ToDateTime(Schedule1_StartTime).TimeOfDay;
                                                TimeSpan? Sc1TT = Convert.ToDateTime(Schedule1_EndTime).TimeOfDay;
                                                TimeSpan? Sc2FT = Convert.ToDateTime(Schedule2_StartTime).TimeOfDay;
                                                TimeSpan? Sc2TT = Convert.ToDateTime(Schedule2_EndTime).TimeOfDay;

                                                if (chkSun.IsChecked == true)
                                                {
                                                    if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null || tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                                                    {
                                                        TimeSpan? FSun = null;
                                                        TimeSpan? TSun = null;
                                                        TimeSpan? FSunday = null;
                                                        TimeSpan? TSunday = null;
                                                        if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
                                                        {
                                                            FSun = tpFromTimeSun.Value.Value.TimeOfDay;
                                                            TSun = tpToTimeSun.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                                                        {

                                                            FSunday = tpFromTimeSunday.Value.Value.TimeOfDay;
                                                            TSunday = tpToTimeSunday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "0")
                                                        {
                                                            //if (tpFromTimeSun.Value.ToString() == Schedule1_StartTime && tpToTimeSun.Value.ToString() == Schedule1_EndTime || tpFromTimeSunday.Value.ToString() == Schedule2_StartTime && tpToTimeSunday.Value.ToString() == Schedule2_EndTime)
                                                            if (FSun == Sc1FT && TSun == Sc1TT || FSunday == Sc2FT && TSunday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkMon.IsChecked == true)
                                                {
                                                    if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null || tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                    {
                                                        TimeSpan? FMon = null;
                                                        TimeSpan? TMon = null;
                                                        TimeSpan? FMonday = null;
                                                        TimeSpan? TMonday = null;
                                                        if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
                                                        {
                                                            FMon = tpFromTimeMon.Value.Value.TimeOfDay;
                                                            TMon = tpToTimeMon.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                                                        {

                                                            FMonday = tpFromTimeMonday.Value.Value.TimeOfDay;
                                                            TMonday = tpToTimeMonday.Value.Value.TimeOfDay;
                                                        }
                                                        if (DayID == "1")
                                                        {
                                                            if (FMon == Sc1FT && TMon == Sc1TT || FMonday == Sc2FT && TMonday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkTue.IsChecked == true)
                                                {
                                                    if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null || tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                    {
                                                        TimeSpan? FTue = null;
                                                        TimeSpan? TTue = null;
                                                        TimeSpan? FTueday = null;
                                                        TimeSpan? TTueday = null;

                                                        if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
                                                        {
                                                            FTue = tpFromTimeTue.Value.Value.TimeOfDay;
                                                            TTue = tpToTimeTue.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                                                        {
                                                            FTueday = tpFromTimeTueday.Value.Value.TimeOfDay;
                                                            TTueday = tpToTimeTueday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "2")
                                                        {
                                                            if (FTue == Sc1FT && TTue == Sc1TT || FTueday == Sc2FT && TTueday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkWed.IsChecked == true)
                                                {
                                                    if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null || tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                    {
                                                        TimeSpan? FWed = null;
                                                        TimeSpan? TWed = null;
                                                        TimeSpan? FWedday = null;
                                                        TimeSpan? TWedday = null;

                                                        if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
                                                        {
                                                            FWed = tpFromTimeWed.Value.Value.TimeOfDay;
                                                            TWed = tpToTimeWed.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                                                        {
                                                            FWedday = tpFromTimeWedday.Value.Value.TimeOfDay;
                                                            TWedday = tpToTimeWedday.Value.Value.TimeOfDay;
                                                        }


                                                        if (DayID == "3")
                                                        {
                                                            if (FWed == Sc1FT && TWed == Sc1TT || FWedday == Sc2FT && TWedday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                                if (chkThr.IsChecked == true)
                                                {
                                                    if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null || tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                    {
                                                        TimeSpan? FThr = null;
                                                        TimeSpan? TThr = null;
                                                        TimeSpan? FThrday = null;
                                                        TimeSpan? TThrday = null;
                                                        if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
                                                        {
                                                            FThr = tpFromTimeThr.Value.Value.TimeOfDay;
                                                            TThr = tpToTimeThr.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                                                        {
                                                            FThrday = tpFromTimeThrday.Value.Value.TimeOfDay;
                                                            TThrday = tpToTimeThrday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "4")
                                                        {
                                                            if (FThr == Sc1FT && TThr == Sc1TT || FThrday == Sc2FT && FThrday == Sc2FT && TThrday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (chkFri.IsChecked == true)
                                                {
                                                    if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null || tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                    {
                                                        TimeSpan? FFri = null;
                                                        TimeSpan? TFri = null;
                                                        TimeSpan? FFriday = null;
                                                        TimeSpan? TFriday = null;

                                                        if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
                                                        {
                                                            FFri = tpFromTimeFri.Value.Value.TimeOfDay;
                                                            TFri = tpToTimeFri.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                                                        {
                                                            FFriday = tpFromTimeFriday.Value.Value.TimeOfDay;
                                                            TFriday = tpToTimeFriday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "5")
                                                        {
                                                            if (FFri == Sc1FT && TFri == Sc1TT || FFriday == Sc2FT && TFriday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }


                                                        }
                                                    }
                                                }
                                                if (chkSat.IsChecked == true)
                                                {
                                                    if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null || tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                    {
                                                        TimeSpan? FSat = null;
                                                        TimeSpan? TSat = null;
                                                        TimeSpan? FSatday = null;
                                                        TimeSpan? TSatday = null;

                                                        if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
                                                        {
                                                            FSat = tpFromTimeSat.Value.Value.TimeOfDay;
                                                            TSat = tpToTimeSat.Value.Value.TimeOfDay;
                                                        }
                                                        if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                                                        {
                                                            FSatday = tpFromTimeSatday.Value.Value.TimeOfDay;
                                                            TSatday = tpToTimeSatday.Value.Value.TimeOfDay;
                                                        }

                                                        if (DayID == "6")
                                                        {
                                                            if (FSat == Sc1FT && TSat == Sc1TT || FSatday == Sc2FT && TSatday == Sc2TT)
                                                            {
                                                                Flag = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        else
                                        {
                                            Flag = true;
                                        }
                                    }

                                

                       
                                
                            }
                            if (Flag == true)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Schedule is already defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }
                            else
                            {

                                if (IsModify == true)
                                {
                                    ModifySchedule();
                                }
                                else
                                {
                                    SaveDoctorSchedule();
                                }
                                
                                

                            }
                                  
                            


                            
                        }



                    }
                }
                

                

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private bool UIValidation()
        {
            bool result = true;
            

            if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
            {
                if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeSunday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSun.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeSunday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSun.Value.Value.ToShortTimeString()))
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        tpFromTimeSunday.Focus();

                    }
                    else if (Convert.ToDateTime(tpToTimeSunday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSun.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSunday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSun.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;

                    }
                   
                }

            }
           
            if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
            {
                if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                {

                    if (Convert.ToDateTime(tpFromTimeMonday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeMon.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeMonday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeMon.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", " Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;

                    }
                    else if (Convert.ToDateTime(tpToTimeMonday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeMon.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeMonday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeMon.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    


                }

            }

            if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
            {
                if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeTueday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeTue.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeTueday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeTue.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;

                    }
                    else if (Convert.ToDateTime(tpToTimeTueday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeTue.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeTueday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeTue.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    
                }
            }


            if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null )
            {
                if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeWedday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeWed.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeWedday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeWed.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;

                    }
                    else if (Convert.ToDateTime(tpToTimeWedday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeWed.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeWedday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeWed.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    
                }
            }

            if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
            {
                if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeThrday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeThr.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeThrday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeThr.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeThrday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeThr.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeThrday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeThr.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    
                }
            }
            if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
            {
                if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeFriday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeFri.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeFriday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeFri.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeFriday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeFri.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeFriday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeFri.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    
                }
            }

            if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
            {
                if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeSatday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSat.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeSatday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSat.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
                    {
                        if (Convert.ToDateTime(tpToTimeSatday.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSat.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSatday.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSat.Value.Value.ToShortTimeString()) && result == true)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with first schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            result = false;
                        }
                    }
                    
                }
            }

            //-----------------------------First Scedule-----------------------
            #region First
            if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
            {
                if (tpFromTimeSun.Value != null && tpToTimeSun.Value!=null)
                {
                    if (Convert.ToDateTime(tpFromTimeSun.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSunday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeSun.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSunday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeSun.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSunday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSun.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSunday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }

            if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
            {
                if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeMon.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeMonday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeMon.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeMonday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeMon.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeMonday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeMon.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeMonday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }

            if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
            {
                if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeTue.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeTueday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeTue.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeTueday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeTue.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeTueday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeTue.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeTueday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }

            if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
            {
                if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeWed.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeWedday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeWed.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeWedday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeWed.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeWedday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeWed.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeWedday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }

            if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
            {
                if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeThr.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeThrday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeThr.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeThrday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeThr.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeThrday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeThr.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeThrday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }
            if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
            {
                if (tpFromTimeFri.Value != null && tpToTimeFri.Value !=null)
                {
                    if (Convert.ToDateTime(tpFromTimeFri.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeFriday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeFri.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeFriday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeFri.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeFriday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeFri.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeFriday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }
            if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
            {
                if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeSat.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSatday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpFromTimeSat.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSatday.Value.Value.ToShortTimeString()) && result == true)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Convert.ToDateTime(tpToTimeSat.Value.Value.ToShortTimeString()) > Convert.ToDateTime(tpFromTimeSatday.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSat.Value.Value.ToShortTimeString()) < Convert.ToDateTime(tpToTimeSatday.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                }
            }
            #endregion
            
            //--------------For same Time---------
            if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
            {
                if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeSunday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeSun.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSunday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeSun.Value.Value.ToShortTimeString()))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }
            }

            if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
            {
                if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeMonday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeMon.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeMonday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeMon.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }

            }

            if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
            {
                if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeTueday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeTue.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeTueday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeTue.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;
                    }
                }
            }
            if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
            {
                if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeWedday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeWed.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeWedday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeWed.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;
                    }
                }
            }

            if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
            {
                if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeThrday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeThr.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeThrday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeThr.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }
            }
            if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
            {
                if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeFriday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeFri.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeFriday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeFri.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }
            }

            if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
            {
                if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
                {
                    if (Convert.ToDateTime(tpFromTimeSatday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpFromTimeSat.Value.Value.ToShortTimeString()) && Convert.ToDateTime(tpToTimeSatday.Value.Value.ToShortTimeString()) == Convert.ToDateTime(tpToTimeSat.Value.Value.ToShortTimeString()) && result == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule time can not be same.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }
            }

            

            return result;
        }


        #region Validation For Second Schedule
        private void tpFromTimeSunday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
            //{

            //    if (tpFromTimeSunday.Value > tpFromTimeSun.Value && tpFromTimeSunday.Value < tpToTimeSun.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
            //    }
                

            //}
        }

        private void tpToTimeSunday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSun.Value != null && tpToTimeSun.Value != null)
            //{
            //    if (tpToTimeSunday.Value > tpFromTimeSun.Value && tpToTimeSunday.Value < tpToTimeSun.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
            //    }
            //}
        }

        private void tpFromTimeMonday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
            //{
            //    if (tpFromTimeMonday.Value > tpFromTimeMon.Value && tpFromTimeMonday.Value < tpToTimeMon.Value)
            //    {
        
                
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
                   
            //    }
            //}

        }

        private void tpToTimeMonday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeMon.Value != null && tpToTimeMon.Value != null)
            //{
            //    if (tpToTimeMonday.Value > tpFromTimeMon.Value && tpToTimeMonday.Value < tpToTimeMon.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
            //    }
            //}
        }

        private void tpFromTimeTueday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
            //{
            //    if (tpFromTimeTueday.Value > tpFromTimeTue.Value && tpFromTimeTueday.Value < tpToTimeTue.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpToTimeTueday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeTue.Value != null && tpToTimeTue.Value != null)
            //{
            //    if (tpToTimeTueday.Value > tpFromTimeTue.Value && tpToTimeTueday.Value < tpToTimeTue.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
            //    }
            //}
        }

        private void tpFromTimeWedday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
            //{
            //    if (tpFromTimeWedday.Value > tpFromTimeWed.Value && tpFromTimeWedday.Value < tpToTimeWed.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpToTimeWedday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeWed.Value != null && tpToTimeWed.Value != null)
            //{
            //    if (tpToTimeWedday.Value > tpFromTimeWed.Value && tpToTimeWedday.Value < tpToTimeWed.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();
            //    }
            //}

        }

        private void tpFromTimeThrday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
            //{
            //    if (tpFromTimeThrday.Value > tpFromTimeThr.Value && tpFromTimeThrday.Value < tpToTimeThr.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeThrday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeThr.Value != null && tpToTimeThr.Value != null)
            //{
            //    if (tpToTimeThrday.Value > tpFromTimeThr.Value && tpToTimeThrday.Value < tpToTimeThr.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpFromTimeFriday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
            //{
            //    if (tpFromTimeFriday.Value > tpFromTimeFri.Value && tpFromTimeFriday.Value < tpToTimeFri.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeFriday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeFri.Value != null && tpToTimeFri.Value != null)
            //{
            //    if (tpToTimeFriday.Value > tpFromTimeFri.Value && tpToTimeFriday.Value < tpToTimeFri.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpFromTimeSatday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
            //{
            //    if (tpFromTimeSatday.Value > tpFromTimeSat.Value && tpFromTimeSatday.Value < tpToTimeSat.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeSatday_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSat.Value != null && tpToTimeSat.Value != null)
            //{
            //    if (tpToTimeSatday.Value > tpFromTimeSat.Value && tpToTimeSatday.Value < tpToTimeSat.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Second schedule is overlapping with First schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }
        #endregion

        # region Validation For First Schedule
        
        private void tpFromTimeSun_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
            //{
            //    if (tpFromTimeSun.Value > tpFromTimeSunday.Value && tpFromTimeSun.Value < tpToTimeSunday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }
       
        private void tpToTimeSun_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSunday.Value != null && tpToTimeSunday.Value != null)
            //{
            //    if (tpToTimeSun.Value > tpFromTimeSunday.Value && tpToTimeSun.Value < tpToTimeSunday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpFromTimeMon_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
            //{
            //    if (tpFromTimeMon.Value > tpFromTimeMonday.Value && tpFromTimeMon.Value < tpToTimeMonday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeMon_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeMonday.Value != null && tpToTimeMonday.Value != null)
            //{
            //    if (tpToTimeMon.Value > tpFromTimeMonday.Value && tpToTimeMon.Value < tpToTimeMonday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpFromTimeTue_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
            //{
            //    if (tpFromTimeTue.Value > tpFromTimeTueday.Value && tpFromTimeTue.Value < tpToTimeTueday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeTue_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeTueday.Value != null && tpToTimeTueday.Value != null)
            //{
            //    if (tpToTimeTue.Value > tpFromTimeTueday.Value && tpToTimeTue.Value < tpToTimeTueday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpFromTimeWed_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
            //{
            //    if (tpFromTimeWed.Value > tpFromTimeWedday.Value && tpFromTimeWed.Value < tpToTimeWedday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeWed_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeWedday.Value != null && tpToTimeWedday.Value != null)
            //{
            //    if (tpToTimeWed.Value > tpFromTimeWedday.Value && tpToTimeWed.Value < tpToTimeWedday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpFromTimeThr_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
            //{
            //    if (tpFromTimeThr.Value > tpFromTimeThrday.Value && tpFromTimeThr.Value < tpToTimeThrday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpToTimeThr_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeThrday.Value != null && tpToTimeThrday.Value != null)
            //{
            //    if (tpToTimeThr.Value > tpFromTimeThrday.Value && tpToTimeThr.Value < tpToTimeThrday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpFromTimeFri_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
            //{
            //    if (tpFromTimeFri.Value > tpFromTimeFriday.Value && tpFromTimeFri.Value < tpToTimeFriday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpToTimeFri_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeFriday.Value != null && tpToTimeFriday.Value != null)
            //{
            //    if (tpToTimeFri.Value > tpFromTimeFriday.Value && tpToTimeFri.Value < tpToTimeFriday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}

        }

        private void tpFromTimeSat_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
            //{
            //    if (tpFromTimeSat.Value > tpFromTimeSatday.Value && tpFromTimeSat.Value < tpToTimeSatday.Value)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }

        private void tpToTimeSat_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tpFromTimeSatday.Value != null && tpToTimeSatday.Value != null)
            //{
            //    if (tpToTimeSat.Value > tpFromTimeSatday.Value && tpToTimeSat.Value < tpToTimeSatday.Value)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", "First schedule is overlapping with Second schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //        msgW1.Show();

            //    }
            //}
        }


#endregion
        
        
        #region Commented code

        //private void SaveDoctorSchedule()
        //{
        //    clsGetDoctorScheduleMasterBizActionVO BizActionObj = new clsGetDoctorScheduleMasterBizActionVO();
        //    BizActionObj.Details = new clsDoctorScheduleVO();
        //    BizActionObj.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
        //    BizActionObj.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
        //    BizActionObj.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {

        //            Status = ((clsGetDoctorScheduleMasterBizActionVO)arg.Result).SuccessStatus;

        //            if (Status == false)
        //            {
        //                clsAddDoctorScheduleMasterBizActionVO BizAction = new clsAddDoctorScheduleMasterBizActionVO();
        //                BizAction.DoctorScheduleDetails = new clsDoctorScheduleVO();

        //                if (cmbDoctorClinic.SelectedItem != null)
        //                {
        //                    BizAction.DoctorScheduleDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
        //                }
        //                if (cmbDoctorDepartment.SelectedItem != null)
        //                {
        //                    BizAction.DoctorScheduleDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
        //                }
        //                if (cmbDoctor.SelectedItem != null)
        //                {
        //                    BizAction.DoctorScheduleDetails.DoctorScheduleID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
        //                }

        //                StringBuilder objDayId = new StringBuilder();
        //                StringBuilder objSchedule1_StartTime = new StringBuilder();
        //                StringBuilder objSchedule1_EndTime = new StringBuilder();
        //                StringBuilder objSchedule2_StartTime = new StringBuilder();
        //                StringBuilder objSchedule2_EndTime = new StringBuilder();
        //                StringBuilder objOncall = new StringBuilder();
        //                StringBuilder objOnSundayCall = new StringBuilder();




        //                if (chkSun.IsChecked == true)
        //                {
        //                    // BizAction.DoctorScheduleDetails.DayId = Sunday;
        //                    objDayId.Append((long)DayOfWeek.Sunday);
        //                    objSchedule1_StartTime.Append(tpFromTimeSun.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeSun.Value);
        //                    if (tpFromTimeSunday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeSunday.Value);
        //                    }
        //                    if (tpToTimeSunday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeSunday.Value);
        //                    }

        //                }

        //                if (chkMon.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");




        //                    //BizAction.DoctorScheduleDetails.DayId = Monday;
        //                    objDayId.Append((long)DayOfWeek.Monday);
        //                    objSchedule1_StartTime.Append(tpFromTimeMon.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeMon.Value);
        //                    if (tpFromTimeMonday.Value != null)
        //                    {

        //                        objSchedule2_StartTime.Append(tpFromTimeMonday.Value);
        //                    }
        //                    if (tpToTimeMonday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeMonday.Value);
        //                    }
        //                }

        //                if (chkTue.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

        //                    // BizAction.DoctorScheduleDetails.DayId = Tuesday;
        //                    objDayId.Append((long)DayOfWeek.Tuesday);
        //                    objSchedule1_StartTime.Append(tpFromTimeTue.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeTue.Value);

        //                    if (tpFromTimeTueday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeTueday.Value);
        //                    }
        //                    if (tpToTimeTueday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeTueday.Value);
        //                    }
        //                }

        //                if (chkWed.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


        //                    //BizAction.DoctorScheduleDetails.DayId = Wednesday;
        //                    objDayId.Append((long)DayOfWeek.Wednesday);
        //                    objSchedule1_StartTime.Append(tpFromTimeWed.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeWed.Value);

        //                    if (tpFromTimeWedday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeWedday.Value);
        //                    }
        //                    if (tpToTimeWedday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeWedday.Value);
        //                    }
        //                }

        //                if (chkThr.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");

        //                    //BizAction.DoctorScheduleDetails.DayId = Thursday;
        //                    objDayId.Append((long)DayOfWeek.Thursday);
        //                    objSchedule1_StartTime.Append(tpFromTimeThr.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeThr.Value);

        //                    if (tpFromTimeThrday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeThrday.Value);
        //                    }
        //                    if (tpToTimeThrday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeThrday.Value);
        //                    }
        //                }

        //                if (chkFri.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


        //                    // BizAction.DoctorScheduleDetails.DayId = Friday;
        //                    objDayId.Append((long)DayOfWeek.Friday);
        //                    objSchedule1_StartTime.Append(tpFromTimeFri.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeFri.Value);
        //                    if (tpFromTimeFriday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeFriday.Value);
        //                    }
        //                    if (tpToTimeFriday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeFriday.Value);
        //                    }
        //                }

        //                if (chkSat.IsChecked == true)
        //                {
        //                    if (objDayId.Length > 0) objDayId.Append(",");
        //                    if (objSchedule1_StartTime.Length > 0) objSchedule1_StartTime.Append(",");
        //                    if (objSchedule1_EndTime.Length > 0) objSchedule1_EndTime.Append(",");
        //                    if (objSchedule2_StartTime.Length > 0) objSchedule2_StartTime.Append(",");
        //                    if (objSchedule2_EndTime.Length > 0) objSchedule2_EndTime.Append(",");


        //                    // BizAction.DoctorScheduleDetails.DayId = Saturday;
        //                    objDayId.Append((long)DayOfWeek.Saturday);
        //                    objSchedule1_StartTime.Append(tpFromTimeSat.Value);
        //                    objSchedule1_EndTime.Append(tpToTimeSat.Value);

        //                    if (tpFromTimeSatday.Value != null)
        //                    {
        //                        objSchedule2_StartTime.Append(tpFromTimeSatday.Value);
        //                    }
        //                    if (tpToTimeSatday.Value != null)
        //                    {
        //                        objSchedule2_EndTime.Append(tpToTimeSatday.Value);
        //                    }
        //                }

        //                BizAction.DoctorScheduleDetails.DayId = objDayId.ToString();
        //                BizAction.DoctorScheduleDetails.Schedule1_StartTime = objSchedule1_StartTime.ToString();
        //                BizAction.DoctorScheduleDetails.Schedule1_EndTime = objSchedule1_EndTime.ToString();
        //                BizAction.DoctorScheduleDetails.Schedule2_StartTime = objSchedule2_StartTime.ToString();
        //                BizAction.DoctorScheduleDetails.Schedule2_EndTime = objSchedule2_EndTime.ToString();




        //                if (chkSunday.IsChecked == true)
        //                {
        //                    objOncall.Append((long)DayOfWeek.Sunday);
        //                }
        //                if (chkMonday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Monday);
        //                }
        //                if (chkTuesday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Tuesday);
        //                }
        //                if (chkWednesday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Wednesday);
        //                }
        //                if (chkThursday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Thursday);

        //                }
        //                if (chkFriday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Friday);

        //                }
        //                if (chkSaturday.IsChecked == true)
        //                {
        //                    if (objOncall.Length > 0) objOncall.Append(",");

        //                    objOncall.Append((long)DayOfWeek.Saturday);
        //                }

        //                BizAction.DoctorScheduleDetails.OnCall = objOncall.ToString();




        //                if (chk1stSunday.IsChecked == true)
        //                {
        //                    objOnSundayCall.Append(Sunday1);
        //                }
        //                if (chk2ndSunday.IsChecked == true)
        //                {
        //                    if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");

        //                    objOnSundayCall.Append(Sunday2);
        //                }
        //                if (chk3rdSunday.IsChecked == true)
        //                {
        //                    if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
        //                    objOnSundayCall.Append(Sunday3);
        //                }
        //                if (chk4thSunday.IsChecked == true)
        //                {
        //                    if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
        //                    objOnSundayCall.Append(Sunday4);
        //                }
        //                if (chk5thSunday.IsChecked == true)
        //                {
        //                    if (objOnSundayCall.Length > 0) objOnSundayCall.Append(",");
        //                    objOnSundayCall.Append(Sunday5);
        //                }

        //                BizAction.DoctorScheduleDetails.OnSundayCall = objOnSundayCall.ToString();

        //                //WaitIndicator Indicatior = new WaitIndicator();
        //                // Indicatior.Show();

        //                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //                client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


        //                client.ProcessCompleted += (sa, arg1) =>
        //                {
        //                    if (arg1.Error == null)
        //                    {

        //                        FetchData();
        //                        ClearControl();
        //                        objAnimation.Invoke(RotationType.Backward);

        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                            new MessageBoxControl.MessageBoxChildWindow("", "Doctor Schedule Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                        msgW1.Show();

        //                    }
        //                    else
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                               new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Doctor Schedule .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                        msgW1.Show();
        //                    }
        //                    SetCommandButtonState("New");
        //                };
        //                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                client.CloseAsync();
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                               new MessageBoxControl.MessageBoxChildWindow("", "Doctor Schedule already defined .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                msgW1.Show();

        //            }
        //        }

        //    };
        //    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();





        //}

        #endregion


    }
}




