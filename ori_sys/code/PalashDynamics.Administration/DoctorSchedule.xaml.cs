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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.DoctorScheduleMaster;

namespace PalashDynamics.Administration
{
    public partial class DoctorSchedule : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        List<MasterListItem> lstDay { get; set; }
        public ObservableCollection<clsDoctorScheduleDetailsVO> ScheduleList { get; set; }
        bool IsPageLoded = false;

        long chkUnitID { get; set; }
        long chkDepartmentID { get; set; }
        long chkDoctorID { get; set; }
        long chkSchedule { get; set; }
        long chkDay { get; set; }

        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }


        long ScheduleDetailID = 0;

        bool IsNew = false;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DoctorSchedule()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            DataList = new PagedSortableCollectionView<clsDoctorScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

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
                //OnPropertyChanged("DataListPageSize");
            }
        }
        #endregion

        #region FillCombobox
        private void FillDay()
        {
            lstDay = new List<MasterListItem>();
            lstDay.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lstDay.Add(new MasterListItem() { ID = 1, Description = "Sunday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 2, Description = "Monday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 3, Description = "Tuesday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 4, Description = "Wednesday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 5, Description = "Thursday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 6, Description = "Friday", Status = true });
            lstDay.Add(new MasterListItem() { ID = 7, Description = "Saturday", Status = true });

            cmbDay.ItemsSource = lstDay;
            cmbDay.SelectedItem = lstDay[0];
        }

        private void FillSchedule()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ScheduleMaster;
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

                    cmbSchedule.ItemsSource = null;
                    cmbSchedule.ItemsSource = objList;
                    cmbSchedule.SelectedItem = objList[0];
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        //rohini for two combo fill
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
                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    //cmbDoctorClinic.ItemsSource = null;
                    //cmbDoctorClinic.ItemsSource = objList;

                    //cmbClinic.ItemsSource = null;
                    //cmbClinic.ItemsSource = objList.DeepCopy();

                    //if (this.DataContext != null)
                    //{
                    //    cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
                    //    cmbClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;

                    //}                   
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        cmbDoctorClinic.ItemsSource = null;
                        cmbDoctorClinic.ItemsSource = objList;
                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList.DeepCopy();

                        cmbClinic.SelectedItem = objList[0];
                        cmbDoctorClinic.SelectedItem = objList[0];

                    }
                    if (this.DataContext != null)
                    {
                        cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
                        cmbClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;
                    }
                  


               


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

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

                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    //if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    //{

                    //    var results = from a in objList
                    //                  group a by a.ID into grouped
                    //                  select grouped.First();
                    //    objList = results.ToList();
                    //}
                    //cmbDepartment.ItemsSource = null;
                    //cmbDepartment.ItemsSource = objList.DeepCopy();
                    //cmbDepartment.SelectedItem = objList[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;

                    //}

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {
                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                            }



                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList.DeepCopy();


                    //if (this.DataContext != null)
                    //{
                    //    cmbDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;
                    //    //  txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    //}
                   
                    //else
                    //{
                        cmbDepartment.SelectedItem = objM;
                        // txtSpouseState.SelectedItem = objM;
                 //  }
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
                    cmbDoctorDepartment.ItemsSource = objList.DeepCopy();
                    cmbDoctorDepartment.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDoctorDepartment.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DepartmentID;

                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

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
                    cmbDoctor.ItemsSource = objList.DeepCopy();
                    cmbDoctor.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorID;

                    }


                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDoctorList(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.Add(new MasterListItem(0, "- Select -"));
                    //objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    //cmbDoctorSearch.ItemsSource = null;
                    //cmbDoctorSearch.ItemsSource = objList.DeepCopy();
                    //cmbDoctorSearch.SelectedItem = objList[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbDoctorSearch.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorID;

                    //}

                  
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctorSearch.ItemsSource = null;
                    cmbDoctorSearch.ItemsSource = objList.DeepCopy();


                    //if (this.DataContext != null)
                    //{
                    //    cmbDoctorSearch.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorID;

                    //}
                   
                    //else
                    //{
                        cmbDoctorSearch.SelectedItem = objM;

                  //  }



                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //commented by rohini 
        //private void FillClinicList()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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

        //            cmbClinic.ItemsSource = null;
        //            cmbClinic.ItemsSource = objList;



        //        }
        //        if (this.DataContext != null)
        //        {
        //            cmbClinic.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).UnitID;

        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        //}

     

        #endregion

        #region Selected Index Changed
       
        private void cmbDoctorClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((MasterListItem)cmbDoctorClinic.SelectedItem != null && ((MasterListItem)cmbDoctorClinic.SelectedItem).ID != 0)
            cmbDoctorDepartment.SelectedItem = null;
            cmbDoctor.SelectedItem = null;
            cmbDoctorDepartment.ItemsSource = null;
            cmbDoctor.ItemsSource = null;
                if (cmbDoctorClinic.SelectedItem != null && cmbDoctorClinic.SelectedValue != null)
                {
                    if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID > 0)                   
                        FillDepartmentList(((MasterListItem)cmbDoctorClinic.SelectedItem).ID);                }

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            //cmbDepartment.SelectedItem = null;
            //cmbDoctorSearch.SelectedItem = null;
            //cmbDepartment.ItemsSource = null;
            //cmbDoctorSearch.ItemsSource = null;
            //if (cmbClinic.SelectedItem != null && cmbClinic.SelectedValue != null)
            //    {
            //        if (((MasterListItem)cmbClinic.SelectedItem).ID > 0)
            //            FillDepartment(((MasterListItem)cmbClinic.SelectedItem).ID);                }


            if (cmbClinic.SelectedItem != null && cmbClinic.SelectedValue != null)
            {
                if (((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objM;
                    cmbDoctorSearch.ItemsSource = objList;
                    cmbDoctorSearch.SelectedItem = objM;

                    FillDepartment(((MasterListItem)cmbClinic.SelectedItem).ID);  
                }
            }
        }


        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cmbDoctorSearch.SelectedItem = null;
            //cmbDoctorSearch.ItemsSource = null;
            //if ((MasterListItem)cmbClinic.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem) != null)
            //{
            //    if (((MasterListItem)cmbClinic.SelectedItem).ID != 0 && ((MasterListItem)cmbDepartment.SelectedItem).ID != 0)
            //        FillDoctorList(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
            //}


            if ((MasterListItem)cmbClinic.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem) != null)
            {
                if (((MasterListItem)cmbClinic.SelectedItem).ID != 0 && ((MasterListItem)cmbDepartment.SelectedItem).ID != 0)
                {
                    //((clsAgencyMasterVO)this.DataContext).StateId = ((MasterListItem)txtState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbDoctorSearch.ItemsSource = objList;
                    cmbDoctorSearch.SelectedItem = objM;

                    FillDoctorList(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
                }
            }
        }

        private void cmbDoctorDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbDoctor.SelectedItem = null;
            cmbDoctor.ItemsSource = null;  
            if ((MasterListItem)cmbDoctorClinic.SelectedItem != null && (MasterListItem)cmbDoctorDepartment.SelectedItem != null)
            {
                if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID != 0 && ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID != 0)           
                  FillDoctor(((MasterListItem)cmbDoctorClinic.SelectedItem).ID, ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID);
            }
        }

        #endregion

        private void DoctorSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsDoctorScheduleVO();
                ScheduleList = new ObservableCollection<clsDoctorScheduleDetailsVO>();


                SetCommandButtonState("New");
                //rohinee
                CheckValidationLoaded();
                FillUnitList();             
                FillDay();
                FillSchedule();
                cmbClinic.Focus();
                cmbDoctorClinic.Focus();
                FetchData();

            }
            IsPageLoded = true;
            cmbClinic.Focus();
            cmbClinic.UpdateLayout();
            cmbDoctorClinic.Focus();
            cmbDoctorClinic.UpdateLayout();

        }
        /// <summary>
        /// Purpose:For add new doctor schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            ClearData();

            cmbDoctorClinic.IsEnabled = true;
            cmbDoctorDepartment.IsEnabled = true;
            cmbDoctor.IsEnabled = true;

            IsNew = true;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + " New ";

            //CheckValidation();
            objAnimation.Invoke(RotationType.Forward);

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            ClickedFlag = 0;
            objAnimation.Invoke(RotationType.Backward);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "List ";
            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);    
            cmbDepartment.ItemsSource = objList;
            cmbDepartment.SelectedItem = objM;
            cmbDoctorSearch.ItemsSource = objList;
            cmbDoctorSearch.SelectedItem = objM;
        }

        #region View Data
        /// <summary>
        /// Purpose:For view existing doctor schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlbViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Modify");
            ClearData();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).DoctorName; ;

            if (dgDoctorList.SelectedItem != null)
            {
                IsNew = false;
                this.DataContext = (clsDoctorScheduleVO)dgDoctorList.SelectedItem;

                cmbDoctorClinic.SelectedValue = ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).UnitID;
                cmbDoctorDepartment.SelectedValue = ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).DepartmentID;
                cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).DoctorID;

                if (((clsDoctorScheduleVO)dgDoctorList.SelectedItem).ID > 0)
                {
                    FillSchedule(((clsDoctorScheduleVO)dgDoctorList.SelectedItem).ID);
                }
                //cmbClinic.SelectedValue = (long)0;

                //cmbDepartment.SelectedValue = (long)0;
                //cmbDoctorSearch.SelectedValue = (long)0;
                //cmbDepartment.ItemsSource = null;
                //cmbDoctorSearch.ItemsSource = null;
                cmbDoctorClinic.IsEnabled = false;
                cmbDoctorDepartment.IsEnabled = false;
                cmbDoctor.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }
          
        }

        /// <summary>
        /// Purpose:Getting existing doctor schedule.
        /// </summary>
        /// <param name="iDoctorScheduleID"></param>
        private void FillSchedule(long iDoctorScheduleID)
        {
            clsGetDoctorScheduleListBizActionVO BizAction = new clsGetDoctorScheduleListBizActionVO();

            BizAction.DoctorScheduleList = new List<clsDoctorScheduleDetailsVO>();
            BizAction.DoctorScheduleID = iDoctorScheduleID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetDoctorScheduleListBizActionVO DetailsVO = new clsGetDoctorScheduleListBizActionVO();
                    DetailsVO = (clsGetDoctorScheduleListBizActionVO)arg.Result;
                    if (DetailsVO.DoctorScheduleList != null)
                    {

                        List<clsDoctorScheduleDetailsVO> ObjItem;
                        ObjItem = DetailsVO.DoctorScheduleList;
                        foreach (var item4 in ObjItem)
                        {
                            ScheduleList.Add(item4);
                        }
                        dgScheduleList.ItemsSource = ScheduleList;
                        dgScheduleList.Focus();
                        dgScheduleList.UpdateLayout();

                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Get data
        /// <summary>
        /// Purpose:Search schedule by different search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            DataPager.PageIndex = 0;

        }

        private void FetchData()
        {
            clsGetDoctorScheduleMasterListBizActionVO BizAction = new clsGetDoctorScheduleMasterListBizActionVO();

            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0)
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

            if (cmbDepartment.SelectedItem != null)
                BizAction.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            if (cmbDoctorSearch.SelectedItem != null)
                BizAction.DoctorID = ((MasterListItem)cmbDoctorSearch.SelectedItem).ID;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).DoctorScheduleList != null)
                    {
                        clsGetDoctorScheduleMasterListBizActionVO result = arg.Result as clsGetDoctorScheduleMasterListBizActionVO;

                        if (result.DoctorScheduleList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).TotalRows;
                            foreach (clsDoctorScheduleVO item in result.DoctorScheduleList)
                            {
                                DataList.Add(item);
                            }

                            dgDoctorList.ItemsSource = null;
                            dgDoctorList.ItemsSource = DataList;

                            DataPager.Source = null;
                            DataPager.PageSize = BizAction.MaximumRows;
                            DataPager.Source = DataList;
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
        #endregion

        #region Save and Modify data

        /// <summary>
        /// Purpose:Save new schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //bool SaveSchedule = true;
            //SaveSchedule = ValidateInputs();

            if (dgScheduleList.ItemsSource != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor Schedule";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
            else
            {
                if (ScheduleList.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can not save Doctor Schedule Master without details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void Save()
        {
            clsAddDoctorScheduleMasterBizActionVO BizAction = new clsAddDoctorScheduleMasterBizActionVO();
            BizAction.DoctorScheduleDetails = (clsDoctorScheduleVO)this.DataContext;

            if (cmbDoctorClinic.SelectedItem != null)
                BizAction.DoctorScheduleDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;

            if (cmbDoctorDepartment.SelectedItem != null)
                BizAction.DoctorScheduleDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;

            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorScheduleDetails.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            if (ScheduleList != null && ScheduleList.Count != 0)
            {
                BizAction.DoctorScheduleDetails.DoctorScheduleDetailsList = ScheduleList.ToList();
           
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                        SetCommandButtonState("New");
                        if (arg.Error == null)
                        {
                            FetchData();
                            ClearData();
                            objAnimation.Invoke(RotationType.Backward);

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Doctor Schedule Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }

               };
               client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
               client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "At least one record should present in List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                ClickedFlag = 0;
                msgW1.Show();
            }

        }

        /// <summary>
        /// Purpose:Modify existing doctor schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            //bool ModifySchedule = true;
            //ModifySchedule = ValidateInputs();
            //if (ModifySchedule == true && UIValidation())
            //{
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Update the Doctor Schedule?";

            MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

            msgW1.Show();

            //}
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();

        }

        private void Modify()
        {
            clsAddDoctorScheduleMasterBizActionVO BizAction = new clsAddDoctorScheduleMasterBizActionVO();
            BizAction.DoctorScheduleDetails = (clsDoctorScheduleVO)this.DataContext;

            if (cmbDoctorClinic.SelectedItem != null)
                BizAction.DoctorScheduleDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;

            if (cmbDoctorDepartment.SelectedItem != null)
                BizAction.DoctorScheduleDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;

            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorScheduleDetails.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            if (ScheduleList != null && ScheduleList.Count != 0)
            {
                BizAction.DoctorScheduleDetails.DoctorScheduleDetailsList = ScheduleList.ToList();
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                SetCommandButtonState("New");
                if (arg.Error == null)
                {
                    FetchData();
                    ClearData();
                    objAnimation.Invoke(RotationType.Backward);

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Schedule Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "At least one record should present in list", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                ClickedFlag = 0;
                msgW1.Show();
            }
        }
        #endregion

        /// <summary>
        /// Purpose:Add entered doctor details to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// rohinee
        int ClickedFlag = 0;
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
          //  rohinee for problem of double click
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (CheckValidation() )//&& CheckDuplicasy())
                {

                    //rohinee

                    clsGetDoctorScheduleListByIDBizActionVO BizAction = new clsGetDoctorScheduleListByIDBizActionVO();
                 
                    BizAction.DoctorScheduleListForDoctorID = new List<clsDoctorScheduleDetailsVO>();

                    BizAction.DoctorScheduleID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                    BizAction.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
                    //BizAction.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                    BizAction.StartTime = (DateTime)tpStartTime.Value;
                    BizAction.EndTime = (DateTime)tpEndTime.Value;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            clsGetDoctorScheduleListByIDBizActionVO DetailsVO = new clsGetDoctorScheduleListByIDBizActionVO();
                        
                            DetailsVO = (clsGetDoctorScheduleListByIDBizActionVO)arg.Result;
                            if (DetailsVO.DoctorScheduleListForDoctorID != null && DetailsVO.DoctorScheduleListForDoctorID.Count != 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "Doctor schedule Already Assigned.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                ClickedFlag =0;
                                msgW1.Show();
                               

                            }
                            else
                            {
                                cmbDoctorClinic.IsEnabled = false;
                                cmbDoctorDepartment.IsEnabled = false;
                                cmbDoctor.IsEnabled = false;

                                try
                                {
                                    //commented by rohinee
                                    //var item1 = from r in ScheduleList
                                    //            where ((r.StartTime == tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                    //            r.StartTime <= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                    //            r.StartTime <= tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                    //            r.StartTime >= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                    //            r.StartTime >= tpStartTime.Value && r.EndTime <= tpEndTime.Value)
                                    //            && r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                    //            )
                                   // added by rohinee dated 12/11/2015 for same day diffrent time not accepting time 
                                    var item1 = from r in ScheduleList
                                                where ((r.StartTime.Value.TimeOfDay == tpStartTime.Value.Value.TimeOfDay && r.EndTime.Value.TimeOfDay == tpEndTime.Value.Value.TimeOfDay ||
                                                r.StartTime.Value.TimeOfDay <= tpStartTime.Value.Value.TimeOfDay && r.EndTime.Value.TimeOfDay >= tpEndTime.Value.Value.TimeOfDay ||
                                                r.StartTime.Value.TimeOfDay <= tpStartTime.Value.Value.TimeOfDay && r.EndTime.Value.TimeOfDay == tpEndTime.Value.Value.TimeOfDay ||
                                                //r.StartTime.Value.TimeOfDay >= tpStartTime.Value.Value.TimeOfDay && r.EndTime.Value.TimeOfDay >= tpEndTime.Value.Value.TimeOfDay ||
                                                r.StartTime.Value.TimeOfDay >= tpStartTime.Value.Value.TimeOfDay && r.EndTime.Value.TimeOfDay <= tpEndTime.Value.Value.TimeOfDay)
                                                && r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                                )
                                  //--------------
                                                select new clsDoctorScheduleDetailsVO
                                                {
                                                    DoctorID = r.DoctorID,
                                                    DepartmentID = r.DepartmentID,
                                                    UnitID = r.UnitID,
                                                    DayID = r.DayID,
                                                    ScheduleID = r.ScheduleID,
                                                    StartTime = r.StartTime,
                                                    EndTime = r.EndTime,
                                                    Status = r.Status,

                                                };

                                   
                                    if (item1.ToList().Count == 0)
                                    {
                                        //CheckTime();
                                        GetDetails();

                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        ClickedFlag = 0;
                                        msgW1.Show();
                                    }
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            ClickedFlag = 0;
                            msgW1.Show();
                        }

                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                 }
                
            }
        
        }

        /// <summary>
        /// Purpose:Get doctor details
        /// </summary>
        private void GetDetails()
        {
            clsDoctorScheduleDetailsVO tempDetails = new clsDoctorScheduleDetailsVO();
            //rohinee
            if (((MasterListItem)cmbDoctor.SelectedItem).ID != 0)
            {
                tempDetails.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                tempDetails.DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description;
            }
            if (((MasterListItem)cmbDoctorDepartment.SelectedItem).ID != 0)
            {
                tempDetails.DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
                tempDetails.DepartmentName = ((MasterListItem)cmbDoctorDepartment.SelectedItem).Description;
            }
            if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID != 0)
            {
                tempDetails.UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
                tempDetails.UnitName = ((MasterListItem)cmbDoctorClinic.SelectedItem).Description;
            }
            if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
            {

                tempDetails.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
                tempDetails.Day = ((MasterListItem)cmbDay.SelectedItem).Description;
            }
            if (((MasterListItem)cmbSchedule.SelectedItem).ID != 0)
            {
                tempDetails.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                tempDetails.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description;
            }
            tempDetails.StartTime = tpStartTime.Value;
            tempDetails.EndTime = tpEndTime.Value;

            ScheduleList.Add(tempDetails);

            dgScheduleList.ItemsSource = null;
            dgScheduleList.ItemsSource = ScheduleList;

            cmbDay.SelectedValue = (long)0;
            ClickedFlag = 0;
            //cmbSchedule.SelectedValue = (long)0;
            //tpStartTime.Value = null;
            //tpEndTime.Value = null;
        }
        /// <summary>
        /// Purpose:Modify selected doctor details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlbEditDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgScheduleList.SelectedItem != null)
            {
                cmbDay.SelectedValue = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).DayID;
                cmbSchedule.SelectedValue = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).ScheduleID;
                tpStartTime.Value = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).StartTime;
                tpEndTime.Value = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).EndTime;
                ScheduleDetailID = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).ID;
            }


        }

        /// <summary>
        /// Purpose:Delete selected doctor schedule frm list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgScheduleList.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Delete the selected Schedule ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ScheduleList.RemoveAt(dgScheduleList.SelectedIndex);

                        }
                    };

                    msgWD.Show();
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Purpose:Modify selected doctor schedule from list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {

            if (ScheduleList.Count > 0)
            {
                //commented by rohini
                //rohinee for 
                if (CheckValidation())// && CheckDuplicasy())
                {
                    // by rohinee
                    clsGetDoctorScheduleListByIDBizActionVO BizAction = new clsGetDoctorScheduleListByIDBizActionVO();
                    BizAction.DoctorScheduleListForDoctorID = new List<clsDoctorScheduleDetailsVO>();
                    BizAction.DoctorScheduleID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                    BizAction.ID = ((clsDoctorScheduleDetailsVO)dgScheduleList.SelectedItem).ID;
                    BizAction.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
                    //BizAction.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                    BizAction.StartTime = (DateTime)tpStartTime.Value;
                    BizAction.EndTime = (DateTime)tpEndTime.Value;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            clsGetDoctorScheduleListByIDBizActionVO DetailsVO = new clsGetDoctorScheduleListByIDBizActionVO();
                            DetailsVO = (clsGetDoctorScheduleListByIDBizActionVO)arg.Result;
                            if (DetailsVO.DoctorScheduleListForDoctorID != null && DetailsVO.DoctorScheduleListForDoctorID.Count != 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "Doctor schedule Already Assigned.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                ClickedFlag = 0;
                                msgW1.Show();
                            }
                            else
                            {
                                //cmbDoctorClinic.IsEnabled = false;
                                //cmbDoctorDepartment.IsEnabled = false;
                                //cmbDoctor.IsEnabled = false;

                                //try
                                //{
                                //    var item1 = from r in ScheduleList
                                //                where ((r.StartTime == tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                //                r.StartTime <= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                //                r.StartTime <= tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                //                r.StartTime >= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                //                r.StartTime >= tpStartTime.Value && r.EndTime <= tpEndTime.Value)
                                //                && r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                //                )
                                //                select new clsDoctorScheduleDetailsVO
                                //                {
                                //                    DoctorID = r.DoctorID,
                                //                    DepartmentID = r.DepartmentID,
                                //                    UnitID = r.UnitID,
                                //                    DayID = r.DayID,
                                //                    ScheduleID = r.ScheduleID,
                                //                    StartTime = r.StartTime,
                                //                    EndTime = r.EndTime,
                                //                    Status = r.Status,

                                //                };

                                //    if (item1.ToList().Count == 0)
                                //    {
                                //        if (CheckValidation())// && CheckDuplicasy())
                                //        {
                                //            if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
                                //            {
                                //                int var = dgScheduleList.SelectedIndex;
                                //                ScheduleList.RemoveAt(dgScheduleList.SelectedIndex);

                                //                ScheduleList.Insert(var, new clsDoctorScheduleDetailsVO
                                //                {

                                //                    DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID,
                                //                    DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description,

                                //                    DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID,
                                //                    DepartmentName = ((MasterListItem)cmbDoctorDepartment.SelectedItem).Description,

                                //                    UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID,
                                //                    UnitName = ((MasterListItem)cmbDoctorClinic.SelectedItem).Description,


                                //                    DayID = ((MasterListItem)cmbDay.SelectedItem).ID,
                                //                    Day = ((MasterListItem)cmbDay.SelectedItem).Description,

                                //                    ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID,
                                //                    Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description,

                                //                    StartTime = tpStartTime.Value,
                                //                    EndTime = tpEndTime.Value,
                                //                    ID = ScheduleDetailID



                                //                }
                                //                );

                                //                dgScheduleList.ItemsSource = ScheduleList;
                                //                dgScheduleList.Focus();
                                //                dgScheduleList.UpdateLayout();
                                //                dgScheduleList.SelectedIndex = ScheduleList.Count - 1;

                                //                cmbDay.SelectedValue = (long)0;
                                //            }
                                //        }


                                //    }
                                //    else
                                //    {
                                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //        ClickedFlag = 0;
                                //        msgW1.Show();
                                //    }


                                //}
                                //catch (Exception)
                                //{
                                //    throw;
                                //}

                                //added BY ROHINI
                                if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
                                {
                                    var item1 = from r in ScheduleList
                                                //where (r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                                //     && r.ScheduleID == ((MasterListItem)cmbSchedule.SelectedItem).ID
                                                //)
                                                select new clsDoctorScheduleDetailsVO
                                                {
                                                    DoctorID = r.DoctorID,
                                                    DepartmentID = r.DepartmentID,
                                                    UnitID = r.UnitID,
                                                    DayID = r.DayID,
                                                    ScheduleID = r.ScheduleID,
                                                    StartTime = r.StartTime,
                                                    EndTime = r.EndTime,
                                                    Status = r.Status,
                                                };
                                    if (item1.ToList().Count > 0)
                                    {
                                        int var = dgScheduleList.SelectedIndex;
                                        if (ScheduleList.Count > 0)
                                            ScheduleList.RemoveAt(dgScheduleList.SelectedIndex);

                                        ScheduleList.Insert(var, new clsDoctorScheduleDetailsVO
                                        {
                                            DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID,
                                            DoctorName = ((MasterListItem)cmbDoctor.SelectedItem).Description,

                                            DepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID,
                                            DepartmentName = ((MasterListItem)cmbDoctorDepartment.SelectedItem).Description,

                                            UnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID,
                                            UnitName = ((MasterListItem)cmbDoctorClinic.SelectedItem).Description,

                                            DayID = ((MasterListItem)cmbDay.SelectedItem).ID,
                                            Day = ((MasterListItem)cmbDay.SelectedItem).Description,

                                            ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID,
                                            Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description,

                                            StartTime = tpStartTime.Value,
                                            EndTime = tpEndTime.Value,
                                            ID = ScheduleDetailID
                                        }
                                        );
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Modify Successfully added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();

                                        dgScheduleList.ItemsSource = ScheduleList;
                                        dgScheduleList.Focus();
                                        dgScheduleList.UpdateLayout();
                                        dgScheduleList.SelectedIndex = ScheduleList.Count - 1;                                        
                                        cmbDay.SelectedValue = (long)0;
                                        cmbSchedule.SelectedValue = (long)0;
                                        tpStartTime.Value = null;
                                        tpEndTime.Value = null;
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                    }

                                }
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            ClickedFlag = 0;
                            msgW1.Show();
                        }

                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                
               
            }
        }

        /// <summary>
        /// Purpose:Clear UI
        /// </summary>
        private void ClearData()
        {
            this.DataContext = new clsDoctorScheduleVO();

            cmbDoctorClinic.SelectedValue = (long)0;
            cmbClinic.SelectedValue = (long)0;

            cmbDoctorDepartment.ItemsSource = null;
            cmbDoctor.ItemsSource = null;            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            //rohinee
            tpStartTime.Value = null;
            tpEndTime.Value = null;
            Applytoallday.IsChecked = false;
            ClickedFlag = 0;
            ScheduleList = new ObservableCollection<clsDoctorScheduleDetailsVO>();

            dgScheduleList.ItemsSource = null;


        }

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

        private void Applytoallday_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Purpose:Check doctor is already exists or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckDuplicasy()
        {
            clsDoctorScheduleVO Obj1;
            clsDoctorScheduleVO Obj2;
            clsDoctorScheduleVO Obj3;

            if (IsNew)
            {
                Obj1 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.UnitID.Equals(((MasterListItem)cmbDoctorClinic.SelectedItem).ID));
                Obj2 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.DepartmentID.Equals(((MasterListItem)cmbDoctorDepartment.SelectedItem).ID));
                Obj3 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.DoctorID.Equals(((MasterListItem)cmbDoctor.SelectedItem).ID));
            }
            else
            {
                Obj1 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.UnitID.Equals(((MasterListItem)cmbDoctorClinic.SelectedItem).ID) && p.ID != ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).ID);
                Obj2 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.DepartmentID.Equals(((MasterListItem)cmbDoctorDepartment.SelectedItem).ID) && p.ID != ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).ID);
                Obj3 = ((PagedSortableCollectionView<clsDoctorScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.DoctorID.Equals(((MasterListItem)cmbDoctor.SelectedItem).ID) && p.ID != ((clsDoctorScheduleVO)dgDoctorList.SelectedItem).ID);
            }
            if (Obj1 != null && Obj2 != null && Obj3 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Already Exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                ClickedFlag = 0;
                return false;
            }
            else
            {
                return true;
            }


        }

        /// <summary>
        /// Purpose:Check doctor time is available or not.
        /// </summary>
        //private void CheckTime()
        //{
        //    bool Flag = false;
        //    clsCheckTimeForScheduleExistanceBizActionVO BizAction = new clsCheckTimeForScheduleExistanceBizActionVO();

        //    if (cmbDoctor.SelectedItem != null)
        //        BizAction.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details != null && ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details.Count != 0)
        //            {
        //                clsCheckTimeForScheduleExistanceBizActionVO DetailsVO = new clsCheckTimeForScheduleExistanceBizActionVO();
        //                DetailsVO = (clsCheckTimeForScheduleExistanceBizActionVO)arg.Result;
        //                if (DetailsVO.Details != null)
        //                {
        //                    List<clsDoctorScheduleDetailsVO> ObjItem;
        //                    ObjItem = DetailsVO.Details;
        //                    foreach (var item in ObjItem)
        //                    {
        //                        chkUnitID = item.UnitID;
        //                        chkDepartmentID = item.DepartmentID;
        //                        chkDoctorID = item.DoctorID;
        //                        chkSchedule = item.ScheduleID;
        //                        chkDay = item.DayID;

        //                        long SelectedSchedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
        //                        long SelectedUnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
        //                        long SelectedDepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
        //                        long SelectedDoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
        //                        long SelectedDayID = ((MasterListItem)cmbDay.SelectedItem).ID;


        //                        TimeSpan StartTime = tpStartTime.Value.Value.TimeOfDay;
        //                        TimeSpan EndTime = tpEndTime.Value.Value.TimeOfDay;

        //                        TimeSpan DbStartTime = item.StartTime.Value.TimeOfDay;
        //                        TimeSpan DbEndTime = item.EndTime.Value.TimeOfDay;

        //                        if (chkUnitID != SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID && chkSchedule == SelectedSchedule && StartTime != null && EndTime != null)
        //                        {
        //                            if (chkDay == SelectedDayID)
        //                            {
        //                                if (StartTime == DbStartTime && EndTime == DbEndTime)
        //                                {
        //                                    Flag = true;
        //                                    break;
        //                                }
        //                                else if (StartTime <= DbStartTime && EndTime >= DbEndTime || StartTime >= DbStartTime && EndTime >= DbEndTime
        //                                         || StartTime <= DbStartTime && EndTime <= DbEndTime || StartTime >= DbStartTime && EndTime <= DbEndTime)
        //                                {
        //                                    Flag = true;
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                        else if (chkUnitID == SelectedUnitID && chkDepartmentID != SelectedDepartmentID && chkDoctorID == SelectedDoctorID && chkSchedule == SelectedSchedule && StartTime != null && EndTime != null)
        //                        {
        //                            if (chkDay == SelectedDayID)
        //                            {
        //                                if (StartTime == DbStartTime && EndTime == DbEndTime)
        //                                {
        //                                    Flag = true;
        //                                    break;
        //                                }
        //                                else if (StartTime <= DbStartTime && EndTime >= DbEndTime || StartTime >= DbStartTime && EndTime >= DbEndTime
        //                                         || StartTime <= DbStartTime && EndTime <= DbEndTime || StartTime >= DbStartTime && EndTime <= DbEndTime)
        //                                {
        //                                    Flag = true;
        //                                    break;
        //                                }


        //                            }

        //                        }

        //                    }
        //                    if (Flag == true)
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Schedule is already defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                        msgW1.Show();

        //                    }
        //                    else
        //                    {
        //                        GetDetails();
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                GetDetails();
        //            }

        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW1.Show();
        //        }

        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();




        //}

        private void CheckTime()
        {
            bool Flag = false;
            clsCheckTimeForScheduleExistanceBizActionVO BizAction = new clsCheckTimeForScheduleExistanceBizActionVO();

            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details != null && ((clsCheckTimeForScheduleExistanceBizActionVO)arg.Result).Details.Count != 0)
                    {
                        clsCheckTimeForScheduleExistanceBizActionVO DetailsVO = new clsCheckTimeForScheduleExistanceBizActionVO();
                        DetailsVO = (clsCheckTimeForScheduleExistanceBizActionVO)arg.Result;
                        if (DetailsVO.Details != null)
                        {
                            List<clsDoctorScheduleDetailsVO> ObjItem;
                            ObjItem = DetailsVO.Details;
                            foreach (var item in ObjItem)
                            {
                                chkUnitID = item.UnitID;
                                chkDepartmentID = item.DepartmentID;
                                chkDoctorID = item.DoctorID;
                                chkSchedule = item.ScheduleID;
                                chkDay = item.DayID;

                                long SelectedSchedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                                long SelectedUnitID = ((MasterListItem)cmbDoctorClinic.SelectedItem).ID;
                                long SelectedDepartmentID = ((MasterListItem)cmbDoctorDepartment.SelectedItem).ID;
                                long SelectedDoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                                long SelectedDayID = ((MasterListItem)cmbDay.SelectedItem).ID;

                                TimeSpan StartTime = tpStartTime.Value.Value.TimeOfDay;
                                TimeSpan EndTime = tpEndTime.Value.Value.TimeOfDay;

                                TimeSpan DbStartTime = item.StartTime.Value.TimeOfDay;
                                TimeSpan DbEndTime = item.EndTime.Value.TimeOfDay;

                                if (chkUnitID != SelectedUnitID && chkDepartmentID == SelectedDepartmentID && chkDoctorID == SelectedDoctorID && StartTime != null && EndTime != null)
                                {
                                    if (chkDay == SelectedDayID)
                                    {
                                        if (SelectedSchedule != chkSchedule)
                                        {
                                            if (StartTime == DbStartTime && EndTime == DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime <= DbStartTime && EndTime >= DbEndTime || StartTime >= DbStartTime && EndTime >= DbEndTime
                                                || StartTime <= DbStartTime && EndTime <= DbEndTime || StartTime >= DbStartTime && EndTime <= DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (StartTime == DbStartTime && EndTime == DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime <= DbStartTime && EndTime <= DbEndTime && EndTime >= DbStartTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime >= DbStartTime && EndTime >= DbEndTime && StartTime <= DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }

                                        }
                                    }
                                }
                                else if (chkUnitID == SelectedUnitID && chkDepartmentID != SelectedDepartmentID && chkDoctorID == SelectedDoctorID && StartTime != null && EndTime != null)
                                {
                                    if (chkDay == SelectedDayID)
                                    {
                                        if (SelectedSchedule != chkSchedule)
                                        {

                                            if (StartTime == DbStartTime && EndTime == DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime <= DbStartTime && EndTime >= DbEndTime || StartTime >= DbStartTime && EndTime >= DbEndTime
                                                     || StartTime <= DbStartTime && EndTime <= DbEndTime || StartTime >= DbStartTime && EndTime <= DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (StartTime == DbStartTime && EndTime == DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime <= DbStartTime && EndTime <= DbEndTime && EndTime >= DbStartTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                            else if (StartTime >= DbStartTime && EndTime >= DbEndTime && StartTime <= DbEndTime)
                                            {
                                                Flag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (Flag == true)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Practitioner Schedule Is Already Defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                               ClickedFlag = 0;
                            }
                            else
                            {
                                GetDetails();
                            }
                        }
                    }
                    else
                    {
                        GetDetails();
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    //rohinee
                   ClickedFlag = 0;
                }
                //rohinee
              ClickedFlag = 0;
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        /// <summary>
        /// Purpose:Assign validation to controls
        /// </summary>
        /// <returns></returns>
        private bool CheckValidation()
        {
            bool result = true;


            if ((MasterListItem)cmbDoctorClinic.SelectedItem == null)
            {
                cmbDoctorClinic.TextBox.SetValidation("Please Select Clinic");
                cmbDoctorClinic.TextBox.RaiseValidationError();
                cmbDoctorClinic.Focus();
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbDoctorClinic.SelectedItem).ID == 0)
            {
                cmbDoctorClinic.TextBox.SetValidation("Please Select Clinic");
                cmbDoctorClinic.TextBox.RaiseValidationError();
                cmbDoctorClinic.Focus();
                result = false;
                ClickedFlag = 0;

            }
            else
                cmbDoctorClinic.TextBox.ClearValidationError();


            if ((MasterListItem)cmbDoctorDepartment.SelectedItem == null)
            {
                cmbDoctorDepartment.TextBox.SetValidation("Please Select Department");
                cmbDoctorDepartment.TextBox.RaiseValidationError();
                cmbDoctorDepartment.Focus();
                result = false;

                ClickedFlag = 0;
            }
            else if (((MasterListItem)cmbDoctorDepartment.SelectedItem).ID == 0)
            {
                cmbDoctorDepartment.TextBox.SetValidation("Please Select Department");
                cmbDoctorDepartment.TextBox.RaiseValidationError();
                cmbDoctorDepartment.Focus();
                result = false;
                ClickedFlag = 0;

            }
            else
                cmbDoctorDepartment.TextBox.ClearValidationError();


            if ((MasterListItem)cmbDoctor.SelectedItem == null)
            {
                cmbDoctor.TextBox.SetValidation("Please Select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
            {
                cmbDoctor.TextBox.SetValidation("Please Select Doctor");
                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                ClickedFlag = 0;
                result = false;

            }
            else
                cmbDoctor.TextBox.ClearValidationError();


            if ((MasterListItem)cmbDay.SelectedItem == null)
            {
                cmbDay.TextBox.SetValidation("Please Select Day");
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbDay.SelectedItem).ID == 0)
            {
                cmbDay.TextBox.SetValidation("Please Select Day");
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                ClickedFlag = 0;
                result = false;

            }
            else
                cmbDay.TextBox.ClearValidationError();

            if ((MasterListItem)cmbSchedule.SelectedItem == null)
            {
                cmbSchedule.TextBox.SetValidation("Please Select Schedule");
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbSchedule.SelectedItem).ID == 0)
            {
                cmbSchedule.TextBox.SetValidation("Please Select Schedule");
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                ClickedFlag = 0;
                result = false;

            }
            else
                cmbSchedule.TextBox.ClearValidationError();

            if (tpStartTime.Value == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Start Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpStartTime.Focus();
                        ClickedFlag = 0;
                    }
                };
                msgW11.Show();
                result = false;
                return result;
            }

            if (tpEndTime.Value == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter End Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpEndTime.Focus();
                        ClickedFlag = 0;
                    }
                };
                msgW11.Show();
                result = false;
                return result;
            }



            //rohinee
            if (tpEndTime.Value.Value <= tpStartTime.Value.Value)
            {
                //tpEndTime.SetValidation("Please Select start End time grater than start time");
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "End Time Should Be Greater Than Start Time", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        ClickedFlag = 0;
                        tpEndTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                return result;

            }
         

       
            return result;

        }
        private bool CheckValidationLoaded()
        {
            bool result = true;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "List ";

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


            if ((MasterListItem)cmbDay.SelectedItem == null)
            {
                cmbDay.TextBox.SetValidation("Please Select Day");
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDay.SelectedItem).ID == 0)
            {
                cmbDay.TextBox.SetValidation("Please Select Day");
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                result = false;

            }
            else
                cmbDay.TextBox.ClearValidationError();

            if ((MasterListItem)cmbSchedule.SelectedItem == null)
            {
                cmbSchedule.TextBox.SetValidation("Please Select Schedule");
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSchedule.SelectedItem).ID == 0)
            {
                cmbSchedule.TextBox.SetValidation("Please Select Schedule");
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                result = false;

            }
            else
                cmbSchedule.TextBox.ClearValidationError();        



            return result;

        }
        private void tpEndTime_LostFocus(object sender, RoutedEventArgs e)
        {
         

        }

        private void cmbDoctorSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDepartment.SelectedItem != null && cmbDepartment.SelectedValue != null)
                if (((MasterListItem)cmbDepartment.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");

                }
        }
        

 
       

        #region commentted code
        //(r.StartTime == tpStartTime.Value && r.EndTime == tpEndTime.Value||
        //                r.StartTime <= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
        //                r.StartTime <= tpStartTime.Value && r.EndTime <= tpEndTime.Value ||
        //                r.StartTime >= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
        //                r.StartTime >= tpStartTime.Value && r.EndTime <= tpEndTime.Value)                r.StartTime >= tpStartTime.Value && r.EndTime <= tpEndTime.Value
        #endregion

    }
}
