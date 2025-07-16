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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.OperationTheatre
{
    public partial class OTScehduleMaster : UserControl
    {
        private SwivelAnimation objAnimation;
        List<MasterListItem> lstDay { get; set; }
        public ObservableCollection<clsOTScheduleDetailsVO> ScheduleList { get; set; }
        bool IsPageLoded = false;

       
        long chkOTID { get; set; }
        long chkOTTableID { get; set; }
        long chkSchedule { get; set; }
        long chkDay { get; set; }

        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }


        long ScheduleDetailID = 0;

        bool IsNew = false;
        #region Paging
        public PagedSortableCollectionView<clsOTScheduleVO> DataList { get; private set; }
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
        public OTScehduleMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));



            DataList = new PagedSortableCollectionView<clsOTScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //FetchData();

        }
        private void FillOT()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
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

                    cmbOT.ItemsSource = null;
                    cmbOT.ItemsSource = objList;


                    if (this.DataContext != null)
                    {
                        cmbOT.SelectedValue = ((clsOTScheduleVO)this.DataContext).OTID;

                    }


                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        private void cmbOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOT.SelectedItem != null)
                {
                    FetchOTTable(((MasterListItem)cmbOT.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Fetch OTTable according to OT
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTTable(long OtTableID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_OTTableMaster;
                BizAction.Parent = new KeyValue { Value = "OTTheatreID", Key = OtTableID.ToString() };
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
                        cmbOTTable.ItemsSource = null;
                        cmbOTTable.ItemsSource = objList;
                        cmbOTTable.SelectedItem = objList[0];


                        if (this.DataContext != null)
                        {
                            cmbOTTable.SelectedValue = ((clsOTScheduleVO)this.DataContext).OTTableID;

                        }


                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private bool CheckValidation()
        {
            bool result = true;


            if ((MasterListItem)cmbOT.SelectedItem == null)
            {
                cmbOT.TextBox.SetValidation("Please Select Operation Theatre");
                cmbOT.TextBox.RaiseValidationError();
                cmbOT.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbOT.SelectedItem).ID == 0)
            {
                cmbOT.TextBox.SetValidation("Please Select Operation Theatre");
                cmbOT.TextBox.RaiseValidationError();
                cmbOT.Focus();
                result = false;

            }
            else
                cmbOT.TextBox.ClearValidationError();


            if ((MasterListItem)cmbOTTable.SelectedItem == null)
            {
                cmbOTTable.TextBox.SetValidation("Please Select OT Table");
                cmbOTTable.TextBox.RaiseValidationError();
                cmbOTTable.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbOTTable.SelectedItem).ID == 0)
            {
                cmbOTTable.TextBox.SetValidation("Please Select OT Table");
                cmbOTTable.TextBox.RaiseValidationError();
                cmbOTTable.Focus();
                result = false;

            }
            else
                cmbOTTable.TextBox.ClearValidationError();


           


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

            if (tpStartTime.Value == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Start Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpStartTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                return result;
            }

            if (tpEndTime.Value == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter End Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpEndTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                return result;
            }



            return result;

        }
        private bool CheckDuplicasy()
        {
            try
            {
                clsOTScheduleVO Obj1 = null;;
                clsOTScheduleVO Obj2 = null;


                if (IsNew)
                {
                    if (dgDoctorList.ItemsSource != null)
                    {
                        Obj1 = ((PagedSortableCollectionView<clsOTScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.OTID.Equals(((MasterListItem)cmbOT.SelectedItem).ID));
                        Obj2 = ((PagedSortableCollectionView<clsOTScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.OTTableID.Equals(((MasterListItem)cmbOTTable.SelectedItem).ID));
                    }

                }
                else
                {
                    if (dgDoctorList.ItemsSource != null)
                    {
                        Obj1 = ((PagedSortableCollectionView<clsOTScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.OTID.Equals(((MasterListItem)cmbOT.SelectedItem).ID) && p.ID != ((clsOTScheduleVO)dgDoctorList.SelectedItem).ID);
                        Obj2 = ((PagedSortableCollectionView<clsOTScheduleVO>)dgDoctorList.ItemsSource).FirstOrDefault(p => p.OTTableID.Equals(((MasterListItem)cmbOTTable.SelectedItem).ID) && p.ID != ((clsOTScheduleVO)dgDoctorList.SelectedItem).ID);
                    }

                }
                if (Obj1 != null && Obj2 != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckValidation() && CheckDuplicasy())
                {

                    try
                    {
                        var item1 = from r in ScheduleList
                                    where ((r.StartTime == tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                    r.StartTime <= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                    r.StartTime <= tpStartTime.Value && r.EndTime == tpEndTime.Value ||
                                    r.StartTime >= tpStartTime.Value && r.EndTime >= tpEndTime.Value ||
                                    r.StartTime >= tpStartTime.Value && r.EndTime <= tpEndTime.Value)
                                    && r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                    )
                                    select new clsOTScheduleDetailsVO
                                    {                                     
                                        UnitID = r.UnitID,
                                        DayID = r.DayID,
                                        ScheduleID = r.ScheduleID,
                                        StartTime = r.StartTime,
                                        EndTime = r.EndTime,
                                        Status = r.Status,

                                    };

                        if (item1.ToList().Count == 0)
                        {
                            CheckTime();

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
        private void CheckTime()
        {
            bool Flag = false;
            clsCheckTimeForOTScheduleExistanceBizActionVO BizAction = new clsCheckTimeForOTScheduleExistanceBizActionVO();

            if (cmbOTTable.SelectedItem != null)
                BizAction.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsCheckTimeForOTScheduleExistanceBizActionVO)arg.Result).Details != null && ((clsCheckTimeForOTScheduleExistanceBizActionVO)arg.Result).Details.Count != 0)
                    {
                        clsCheckTimeForOTScheduleExistanceBizActionVO DetailsVO = new clsCheckTimeForOTScheduleExistanceBizActionVO();
                        DetailsVO = (clsCheckTimeForOTScheduleExistanceBizActionVO)arg.Result;
                        if (DetailsVO.Details != null)
                        {
                            List<clsOTScheduleDetailsVO> ObjItem;
                            ObjItem = DetailsVO.Details;
                            foreach (var item in ObjItem)
                            {
                             
                                chkSchedule = item.ScheduleID;
                                chkDay = item.DayID;
                                chkOTID = item.OTID;
                                chkOTTableID = item.OTTableID;
                                

                                long SelectedSchedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                              
                                long SelectedOTID = ((MasterListItem)cmbOT.SelectedItem).ID;
                                long SelectedOTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
                                long SelectedDayID = ((MasterListItem)cmbDay.SelectedItem).ID;


                                TimeSpan StartTime = tpStartTime.Value.Value.TimeOfDay;
                                TimeSpan EndTime = tpEndTime.Value.Value.TimeOfDay;

                                TimeSpan DbStartTime = item.StartTime.Value.TimeOfDay;
                                TimeSpan DbEndTime = item.EndTime.Value.TimeOfDay;

                                if (chkOTID == SelectedOTID && chkOTTableID == SelectedOTTableID && chkSchedule == SelectedSchedule && StartTime != null && EndTime != null)
                                {
                                    if (chkDay == SelectedDayID)
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
                                }
                                else if (chkOTID == SelectedOTID && chkOTTableID == SelectedOTTableID && chkSchedule == SelectedSchedule && StartTime != null && EndTime != null)
                                {
                                    if (chkDay == SelectedDayID)
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

                                }

                            }
                            if (Flag == true)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "OT Schedule is already defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();

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
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();




        }
        private void GetDetails()
        {
            clsOTScheduleDetailsVO tempDetails = new clsOTScheduleDetailsVO();

            tempDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
            tempDetails.OTName = ((MasterListItem)cmbOT.SelectedItem).Description;

            tempDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
            tempDetails.OTTableName = ((MasterListItem)cmbOTTable.SelectedItem).Description;

           
            tempDetails.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
            tempDetails.Day = ((MasterListItem)cmbDay.SelectedItem).Description;

            tempDetails.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
            tempDetails.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description;

            tempDetails.StartTime = tpStartTime.Value;
            tempDetails.EndTime = tpEndTime.Value;

            ScheduleList.Add(tempDetails);

            dgScheduleList.ItemsSource = null;
            dgScheduleList.ItemsSource = ScheduleList;

            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            tpStartTime.Value = null;
            tpEndTime.Value = null;
        }
        private void ClearData()
        {
            this.DataContext = new clsOTScheduleVO();

            cmbOT.SelectedValue = (long)0;
            cmbOTTable.SelectedValue = (long)0;

            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            tpStartTime.Value = null;
            tpEndTime.Value = null;
            Applytoallday.IsChecked = false;

            ScheduleList = new ObservableCollection<clsOTScheduleDetailsVO>();

            dgScheduleList.ItemsSource = null;


        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleList.Count > 0)
            {
                if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
                {
                    int var = dgScheduleList.SelectedIndex;
                    ScheduleList.RemoveAt(dgScheduleList.SelectedIndex);

                    ScheduleList.Insert(var, new clsOTScheduleDetailsVO
                    {

                        OTID = ((MasterListItem)cmbOT.SelectedItem).ID,
                        OTName = ((MasterListItem)cmbOT.SelectedItem).Description,

                        OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID,
                        OTTableName = ((MasterListItem)cmbOT.SelectedItem).Description,


                        DayID = ((MasterListItem)cmbDay.SelectedItem).ID,
                        Day = ((MasterListItem)cmbDay.SelectedItem).Description,

                        ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID,
                        Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description,

                        StartTime = tpStartTime.Value,
                        EndTime = tpEndTime.Value,
                        ID = ScheduleDetailID



                    }
                    );

                    dgScheduleList.ItemsSource = ScheduleList;
                    dgScheduleList.Focus();
                    dgScheduleList.UpdateLayout();
                    dgScheduleList.SelectedIndex = ScheduleList.Count - 1;


                    cmbDay.SelectedValue = (long)0;
                    cmbSchedule.SelectedValue = (long)0;
                    tpStartTime.Value = null;
                    tpEndTime.Value = null;

                }



            }
        }

        private void cmdDeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgScheduleList.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to delete the selected schedule ?";

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

        private void hlbEditDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgScheduleList.SelectedItem != null)
            {
                cmbDay.SelectedValue = ((clsOTScheduleDetailsVO)dgScheduleList.SelectedItem).DayID;
                cmbSchedule.SelectedValue = ((clsOTScheduleDetailsVO)dgScheduleList.SelectedItem).ScheduleID;
                tpStartTime.Value = ((clsOTScheduleDetailsVO)dgScheduleList.SelectedItem).StartTime;
                tpEndTime.Value = ((clsOTScheduleDetailsVO)dgScheduleList.SelectedItem).EndTime;
                ScheduleDetailID = ((clsOTScheduleDetailsVO)dgScheduleList.SelectedItem).ID;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsOTScheduleVO();
                ScheduleList = new ObservableCollection<clsOTScheduleDetailsVO>();

                SetCommandButtonState("New");
                FillOT();
               
                FillDay();
                FillSchedule();
               
                FetchData();

            }
            IsPageLoded = true;
            
           
        }
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

        private void hlbViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Modify");
            ClearData();
            if (dgDoctorList.SelectedItem != null)
            {
                IsNew = false;
                this.DataContext = (clsOTScheduleVO)dgDoctorList.SelectedItem;

                cmbOT.SelectedValue = ((clsOTScheduleVO)dgDoctorList.SelectedItem).OTID;
                cmbOTTable.SelectedValue = ((clsOTScheduleVO)dgDoctorList.SelectedItem).OTTableID;


                if (((clsOTScheduleVO)dgDoctorList.SelectedItem).ID > 0)
                {
                    FillSchedule(((clsOTScheduleVO)dgDoctorList.SelectedItem).ID);
                }
          
                objAnimation.Invoke(RotationType.Forward);
            }

        }

        private void FillSchedule(long OTScheduleID)
        {
            clsGetOTScheduleListBizActionVO BizAction = new clsGetOTScheduleListBizActionVO();

            BizAction.OTScheduleList = new List<clsOTScheduleDetailsVO>();
            BizAction.OTScheduleID = OTScheduleID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetOTScheduleListBizActionVO DetailsVO = new clsGetOTScheduleListBizActionVO();
                    DetailsVO = (clsGetOTScheduleListBizActionVO)arg.Result;
                    if (DetailsVO.OTScheduleList != null)
                    {

                        List<clsOTScheduleDetailsVO> ObjItem;
                        ObjItem = DetailsVO.OTScheduleList;
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


        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            ClearData();

            
            IsNew = true;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Schedule Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgScheduleList.ItemsSource != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the OT schedule";

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
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can not save OT schedule master without details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
            clsAddOTScheduleMasterBizActionVO BizAction = new clsAddOTScheduleMasterBizActionVO();
            BizAction.OTScheduleDetails = (clsOTScheduleVO)this.DataContext;

            if (cmbOT.SelectedItem != null)
                BizAction.OTScheduleDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;

            if (cmbOTTable.SelectedItem != null)
                BizAction.OTScheduleDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;


            BizAction.OTScheduleDetails.OTScheduleDetailsList = ScheduleList.ToList();

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
                        new MessageBoxControl.MessageBoxChildWindow("", "OT Schedule Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

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
        private void FetchData()
        {
            try
            {
                clsGetOTScheduleMasterListBizActionVO BizAction = new clsGetOTScheduleMasterListBizActionVO();

                //if (cmbOTSearch.SelectedItem != null && ((MasterListItem)cmbOTSearch.SelectedItem).ID != 0)
                //{
                //    BizAction.OTID = ((MasterListItem)cmbOTSearch.SelectedItem).ID;
                //}
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                {
                    BizAction.UnitID = 0;
                }

                if (cmbOTTable.SelectedItem != null)
                    BizAction.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;


                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetOTScheduleMasterListBizActionVO)arg.Result).OTScheduleList != null)
                        {
                            clsGetOTScheduleMasterListBizActionVO result = arg.Result as clsGetOTScheduleMasterListBizActionVO;

                            if (result.OTScheduleList != null)
                            {
                                DataList.Clear();
                                DataList.TotalItemCount = ((clsGetOTScheduleMasterListBizActionVO)arg.Result).TotalRows;
                                foreach (clsOTScheduleVO item in result.OTScheduleList)
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
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to update the OT schedule";

            MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

            msgW1.Show();
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();



        }

        private void Modify()
        {
            clsAddOTScheduleMasterBizActionVO BizAction = new clsAddOTScheduleMasterBizActionVO();
            BizAction.OTScheduleDetails = (clsOTScheduleVO)this.DataContext;

            if (cmbOT.SelectedItem != null)
                BizAction.OTScheduleDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;

            if (cmbOTTable.SelectedItem != null)
                BizAction.OTScheduleDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;


            BizAction.OTScheduleDetails.OTScheduleDetailsList = ScheduleList.ToList();

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
                        new MessageBoxControl.MessageBoxChildWindow("", "OT Schedule Master Modified Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbOTSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
