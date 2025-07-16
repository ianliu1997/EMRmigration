using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster;
using System.IO;
using System.Reflection;
using System.Windows.Resources;
using System.Xml.Linq;
using PalashDynamic.Localization;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTScehduleMaster : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
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
        List<MasterListItem> lstDay { get; set; }
        public ObservableCollection<clsOTScheduleDetailsVO> ScheduleList { get; set; }
        bool IsPageLoded = false;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        LocalizationManager objLocalizationManager;
        bool IsCancel = true;
        bool ForSearch = false;
        long ScheduleDetailID = 0;
        bool Is_New = false;
        string msgTitle = "PALASH";
        string msgText = "";
        #endregion

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
                OnPropertyChanged("DataListPageSize");
            }
        }
        #endregion

        #region Constructor

        public frmOTScehduleMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsOTScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
        }
        #endregion

        #region Refresh
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsOTScheduleVO();
                ScheduleList = new ObservableCollection<clsOTScheduleDetailsVO>();
                DataListPageSize = 15;
                SetCommandButtonState("New");
                FillOperationTheatre();
                FillDay();
                FillSchedule();
                FetchData();
            }
            IsPageLoded = true;
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Private Methods
        private void FillOperationTheatre()
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
                    cmbOTSearch.ItemsSource = null;
                    cmbOTSearch.ItemsSource = objList;
                    cmbOTSearch.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbOT.SelectedValue = ((clsOTScheduleVO)this.DataContext).OTID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        if (ForSearch == false)
                        {
                            cmbOTTable.ItemsSource = null;
                            cmbOTTable.ItemsSource = objList;
                        }
                        else
                        {
                            cmbOTTableSearch.ItemsSource = null;
                            cmbOTTableSearch.ItemsSource = objList;
                            cmbOTTableSearch.SelectedItem = objList[0];
                        }
                        if (dgDoctorList.SelectedItem != null)
                            cmbOTTable.SelectedValue = ((clsOTScheduleVO)dgDoctorList.SelectedItem).OTTableID;
                        else
                            cmbOTTable.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckValidation()
        {
            bool result = true;
            if (cmbOT.ItemsSource == null)
            {
                msgText = "Please Select Operation Theatre";
                cmbOT.TextBox.SetValidation(msgText);
                cmbOT.TextBox.RaiseValidationError();
                cmbOT.Focus();
                result = false;
                //return false;
            }
            else if ((MasterListItem)cmbOT.SelectedItem == null || ((MasterListItem)cmbOT.SelectedItem).ID == 0)
            {
                msgText = "Please Select Operation Theatre";
                cmbOT.TextBox.SetValidation(msgText);
                cmbOT.TextBox.RaiseValidationError();
                cmbOT.Focus();
                result = false;
                //return false;
            }
            //else
            //    cmbOT.TextBox.ClearValidationError();

            else if (cmbOTTable.ItemsSource == null)
            {
                msgText = "Please Select OT Table";
                cmbOTTable.TextBox.SetValidation(msgText);
                cmbOTTable.TextBox.RaiseValidationError();
                cmbOTTable.Focus();
                result = false;
            }
            else if ((MasterListItem)cmbOTTable.SelectedItem == null || ((MasterListItem)cmbOTTable.SelectedItem).ID == 0)
            {
                msgText = "Please Select OT Table";
                cmbOTTable.TextBox.SetValidation(msgText);
                cmbOTTable.TextBox.RaiseValidationError();
                cmbOTTable.Focus();
                result = false;
            }
            //else
            //    cmbOTTable.TextBox.ClearValidationError();
            else if (cmbDay.ItemsSource == null)
            {
                msgText = "Please Select Day";
                cmbDay.TextBox.SetValidation(msgText);
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                result = false;
                //return false;
            }
            if ((MasterListItem)cmbDay.SelectedItem == null || ((MasterListItem)cmbDay.SelectedItem).ID == 0)
            {
                msgText = "Please Select Day";
                cmbDay.TextBox.SetValidation(msgText);
                cmbDay.TextBox.RaiseValidationError();
                cmbDay.Focus();
                result = false;
                //return false;
            }
            //else
            //    cmbDay.TextBox.ClearValidationError();
            else if (cmbSchedule.ItemsSource == null)
            {
                msgText = "Please Select Schedule";
                cmbSchedule.TextBox.SetValidation(msgText);
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                result = false;
                //return false;
            }
            else if ((MasterListItem)cmbSchedule.SelectedItem == null || ((MasterListItem)cmbSchedule.SelectedItem).ID == 0)
            {
                msgText = "Please Select Schedule";
                cmbSchedule.TextBox.SetValidation(msgText);
                cmbSchedule.TextBox.RaiseValidationError();
                cmbSchedule.Focus();
                result = false;
                //return false;
            }
            //else
            //    cmbSchedule.TextBox.ClearValidationError();

            else if (tpStartTime.Value == null)
            {
                msgText = "Please Enter Start Time";
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpStartTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                //return false;
            }

            else if (tpEndTime.Value == null)
            {
                msgText = "Please enter end Time.";
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpEndTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                //return false;
            }

            else if (tpEndTime.Value.Value < tpStartTime.Value.Value)
            {
                msgText = "Start time cannot be greater than End time";
                MessageBoxControl.MessageBoxChildWindow msgW11 =
                               new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        tpStartTime.Focus();
                    }
                };
                msgW11.Show();
                result = false;
                //return false;
            }
            //flag = 0;
            return result;

        }

        private bool CheckDuplicasy()
        {
            bool result = true;
            clsOTScheduleVO Obj1;
            long OTID = 0, OTTableID = 0;
            if (dgDoctorList.ItemsSource != null)
            {
                if (cmbOT.SelectedItem != null)
                    OTID = ((MasterListItem)cmbOT.SelectedItem).ID;

                if (cmbOTTable.SelectedItem != null)
                    OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;

                if (Is_New && OTID > 0 && OTTableID > 0)
                {
                    var list = ((PagedCollectionView)dgDoctorList.ItemsSource);
                    ObservableCollection<clsOTScheduleVO> Objlist = new ObservableCollection<clsOTScheduleVO>();

                    foreach (var item in list)
                    {
                        clsOTScheduleVO obj = new clsOTScheduleVO();
                        obj = (clsOTScheduleVO)item;
                        Objlist.Add(obj);
                    }

                    Obj1 = (from n in Objlist
                            where n.OTID == OTID && n.OTTableID == OTTableID
                            select n
                                ).SingleOrDefault();

                    if (Obj1 != null)
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ScheduleExistValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Schedule already exist!";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        result = false;
                    }
                }
            }
            return result;
        }

        private void GetDetails()
        {
            clsOTScheduleDetailsVO tempDetails = new clsOTScheduleDetailsVO();
            tempDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
            tempDetails.OTName = ((MasterListItem)cmbOT.SelectedItem).Description;
            tempDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
            tempDetails.OTTableName = ((MasterListItem)cmbOTTable.SelectedItem).Description;
            if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
                tempDetails.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
            if (((MasterListItem)cmbDay.SelectedItem).Description != "--Select--")
                tempDetails.Day = ((MasterListItem)cmbDay.SelectedItem).Description;
            if (((MasterListItem)cmbSchedule.SelectedItem).ID != 0)
                tempDetails.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
            if (((MasterListItem)cmbSchedule.SelectedItem).Description != "-- Select --")
                tempDetails.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description;
            if (tpStartTime.Value != null)
                tempDetails.StartTime = tpStartTime.Value;
            if (tpEndTime.Value != null)
                tempDetails.EndTime = tpEndTime.Value;
            if (tempDetails.DayID != 0)
                ScheduleList.Add(tempDetails);
            dgScheduleList.ItemsSource = null;
            dgScheduleList.ItemsSource = ScheduleList;
            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            tpStartTime.Value = null;
            tpEndTime.Value = null;
            //flag = 0;
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

        private void ClearControl()
        {
            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            tpStartTime.Value = null;
            tpEndTime.Value = null;
            Applytoallday.IsChecked = false;
            if (!Is_New)
                SetCommandButtonState("ModifySchedule");
        }

        private void AddSchedule()
        {
            try
            {
                if (ScheduleList != null && ScheduleList.Count > 0)
                {
                    var item1 = from r in ScheduleList
                                where (
                                    r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                    &&
                                    (
                                    (tpStartTime.Value.Value.TimeOfDay == r.StartTime.Value.TimeOfDay) ||
                                    (tpEndTime.Value.Value.TimeOfDay == r.EndTime.Value.TimeOfDay) ||

                                    (tpStartTime.Value.Value.TimeOfDay == r.EndTime.Value.TimeOfDay) ||
                                    (tpEndTime.Value.Value.TimeOfDay == r.StartTime.Value.TimeOfDay) ||

                                    (tpStartTime.Value.Value.TimeOfDay < r.StartTime.Value.TimeOfDay && tpEndTime.Value.Value.TimeOfDay > r.StartTime.Value.TimeOfDay) ||
                                    (tpStartTime.Value.Value.TimeOfDay > r.StartTime.Value.TimeOfDay && tpStartTime.Value.Value.TimeOfDay < r.EndTime.Value.TimeOfDay)
                                    )
                                    )
                                select r;

                    if (item1.ToList().Count == 0)
                    {
                        CheckTime(true);
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ScheduleAddedValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Schedule already added";
                        //}
                        //flag = 0;
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
                else
                {
                    CheckTime(true);
                }
                //flag = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void checkDepartmentSchedule()
        {
            clsGetDepartmentScheduleMasterListBizActionVO BizAction = new clsGetDepartmentScheduleMasterListBizActionVO();
            BizAction.UnitID = ((MasterListItem)cmbOT.SelectedItem).ID;
            BizAction.DepartmentID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
            BizAction.IsOTSchedule = true;
            BizAction.checkDepartmentSchedule = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDepartmentScheduleMasterListBizActionVO)arg.Result).IsDepartmentSchedule)
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ScheduleAddedValidation_Msg");
                        //}
                        //else
                        //{
                        //flag = 0;
                        msgText = "Schedule already added";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    else
                    {
                        AddSchedule();
                    }
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    //flag = 0;
                    msgText = "Error Occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            //flag = 0;
        }

        private void CheckTime(bool IsNew)
        {
            clsCheckTimeForOTScheduleExistanceBizActionVO BizAction = new clsCheckTimeForOTScheduleExistanceBizActionVO();
            BizAction.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
            BizAction.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
            BizAction.StartTime = tpStartTime.Value;
            BizAction.EndTime = tpEndTime.Value;
            BizAction.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = null;
                if (arg.Error == null)
                {
                    if ((bool)((clsCheckTimeForOTScheduleExistanceBizActionVO)arg.Result).IsSchedulePresent)
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ScheduleAddedValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Schedule already added";
                        //}
                        msgW1 = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        //flag = 0;

                    }
                    else
                    {
                        if (IsNew)
                            GetDetails();
                        else
                            ModifySchedule();
                    }
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Error Occured while processing.";
                    //}
                    //flag = 0;
                    msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            //flag = 0;
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ModifySchedule()
        {
            clsOTScheduleDetailsVO objOTScheduleDetails = (clsOTScheduleDetailsVO)dgScheduleList.SelectedItem;
            objOTScheduleDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
            objOTScheduleDetails.OTName = ((MasterListItem)cmbOT.SelectedItem).Description;

            objOTScheduleDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;
            objOTScheduleDetails.OTTableName = ((MasterListItem)cmbOT.SelectedItem).Description;

            objOTScheduleDetails.DayID = ((MasterListItem)cmbDay.SelectedItem).ID;
            objOTScheduleDetails.Day = ((MasterListItem)cmbDay.SelectedItem).Description;

            objOTScheduleDetails.ScheduleID = ((MasterListItem)cmbSchedule.SelectedItem).ID;
            objOTScheduleDetails.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).Description;

            objOTScheduleDetails.StartTime = tpStartTime.Value;
            objOTScheduleDetails.EndTime = tpEndTime.Value;
            objOTScheduleDetails.ID = ScheduleDetailID;

            dgScheduleList.ItemsSource = ScheduleList;
            dgScheduleList.Focus();
            dgScheduleList.UpdateLayout();
            dgScheduleList.SelectedIndex = ScheduleList.Count - 1;

            cmbDay.SelectedValue = (long)0;
            cmbSchedule.SelectedValue = (long)0;
            tpStartTime.Value = null;
            tpEndTime.Value = null;

            //flag = 0;

        }

        private void ModifyAfterDeleteSchedule()
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
                if (arg.Error == null)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("DeletVerify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Record deleted Successfully.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Error Occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Error Occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

                if (cmbOTSearch.SelectedItem != null && ((MasterListItem)cmbOTSearch.SelectedItem).ID != 0)
                {
                    BizAction.OTID = ((MasterListItem)cmbOTSearch.SelectedItem).ID;
                }
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

                if (cmbOTTableSearch.SelectedItem != null)
                    BizAction.OTTableID = ((MasterListItem)cmbOTTableSearch.SelectedItem).ID;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;

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
                                PagedCollectionView collection = new PagedCollectionView(DataList);
                                dgDoctorList.ItemsSource = null;
                                dgDoctorList.ItemsSource = collection;
                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;
                            }
                        }
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error Occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Save()
        {
            clsAddOTScheduleMasterBizActionVO BizAction = new clsAddOTScheduleMasterBizActionVO();
            BizAction.OTScheduleDetails = (clsOTScheduleVO)this.DataContext;

            if (cmbOT.SelectedItem != null)
                BizAction.OTScheduleDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;

            if (cmbOTTable.SelectedItem != null)
                BizAction.OTScheduleDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;

            BizAction.OTScheduleDetails.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.OTScheduleDetails.OTScheduleDetailsList = ScheduleList.ToList();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("New");
                if (arg.Error == null)
                {
                    ClearData();
                    objAnimation.Invoke(RotationType.Backward);
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("RecordSaved_Msg");
                    //}
                    //else
                    //{
                    msgText = "Record Saved Successfully.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    FetchData();
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Error Occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void Modify()
        {
            clsAddOTScheduleMasterBizActionVO BizAction = new clsAddOTScheduleMasterBizActionVO();
            BizAction.OTScheduleDetails = (clsOTScheduleVO)this.DataContext;

            if (cmbOT.SelectedItem != null)
                BizAction.OTScheduleDetails.OTID = ((MasterListItem)cmbOT.SelectedItem).ID;

            if (cmbOTTable.SelectedItem != null)
                BizAction.OTScheduleDetails.OTTableID = ((MasterListItem)cmbOTTable.SelectedItem).ID;

            BizAction.OTScheduleDetails.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
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
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("RecordModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Record Updated Successfully.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                    msgText = "Error Occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region SelectionChanged
        private void cmbOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOT.SelectedItem != null)
                {
                    ForSearch = false;
                    FetchOTTable(((MasterListItem)cmbOT.SelectedItem).ID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmbOTSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOTSearch.SelectedItem != null)
            {
                ForSearch = true;
                FetchOTTable(((MasterListItem)cmbOTSearch.SelectedItem).ID);
            }
        }

        private void dgScheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControl();
        }

        #endregion

        #region Click Event

        //int flag = 0;
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Is_New = true;
           
            if (Is_New)
            {
                if (CheckValidation())
                {

                    if (cmbOT.IsEnabled == true && cmbOTTable.IsEnabled == true)
                    {
                        if (cmbOT.SelectedItem != null && cmbOTTable.SelectedItem != null)
                        {
                            checkDepartmentSchedule();
                        }
                    }
                    else
                    {
                        AddSchedule();
                    }
                }

            }
            else if (CheckValidation())
            {
                AddSchedule();
            }
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation() && CheckDuplicasy())
            {
                if (ScheduleList.Count > 0)
                {
                    if (((MasterListItem)cmbDay.SelectedItem).ID != 0)
                    {
                        var item1 = from r in ScheduleList
                                    where (
                                                r.DayID == ((MasterListItem)cmbDay.SelectedItem).ID
                                                &&
                                                (
                                                    (tpStartTime.Value.Value.TimeOfDay == r.StartTime.Value.TimeOfDay) ||
                                                    (tpEndTime.Value.Value.TimeOfDay == r.EndTime.Value.TimeOfDay) ||

                                                    (tpStartTime.Value.Value.TimeOfDay == r.EndTime.Value.TimeOfDay) ||
                                                    (tpEndTime.Value.Value.TimeOfDay == r.StartTime.Value.TimeOfDay) ||

                                                    (tpStartTime.Value.Value.TimeOfDay < r.StartTime.Value.TimeOfDay && tpEndTime.Value.Value.TimeOfDay > r.StartTime.Value.TimeOfDay) ||
                                                    (tpStartTime.Value.Value.TimeOfDay > r.StartTime.Value.TimeOfDay && tpStartTime.Value.Value.TimeOfDay < r.EndTime.Value.TimeOfDay)
                                                )
                                           )
                                    select r;

                        if (item1.ToList().Count == 0)
                        {
                            CheckTime(false);
                            SetCommandButtonState("ModifySchedule");
                        }
                        else
                        {
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("RecordModify_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record Updated Successfully.";
                            //}
                            ModifySchedule();
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void cmdDeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgScheduleList.SelectedItem != null)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("DeleteValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are you sure you want to delete ?";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ScheduleList.RemoveAt(dgScheduleList.SelectedIndex);
                            ClearControl();
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("DeletVerify_Msg");
                            //}
                            //else
                            //{
                            msgText = "Record Deleted Successfully.";
                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                SetCommandButtonState("Edit");
            }
        }

        private void hlbViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Modify");
            ClearData();
            if (dgDoctorList.SelectedItem != null)
            {
                Is_New = false;
                this.DataContext = (clsOTScheduleVO)dgDoctorList.SelectedItem;
                cmbOT.SelectedValue = ((clsOTScheduleVO)dgDoctorList.SelectedItem).OTID;

                if (((clsOTScheduleVO)dgDoctorList.SelectedItem).ID > 0)
                {
                    FillSchedule(((clsOTScheduleVO)dgDoctorList.SelectedItem).ID);
                }
                cmbOT.IsEnabled = false;
                cmbOTTable.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            ClearData();
            Is_New = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Schedule Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgScheduleList.ItemsSource != null)
            {
                //if (objLocalizationManager != null)
                //{
                //    msgText = objLocalizationManager.GetValue("SaveVerification_Msg");
                //}
                //else
                //{
                msgText = "Are you sure you want to Save?";
                //}

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {
                if (ScheduleList.Count == 0)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("AddingOTDetailsValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Record cannot be Saved without adding OT details.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgScheduleList.ItemsSource == null || ((ObservableCollection<clsOTScheduleDetailsVO>)dgScheduleList.ItemsSource).Count < 1)
            {
                //if (objLocalizationManager != null)
                //{
                //    msgText = objLocalizationManager.GetValue("AtleastOneScheduleValidation_Msg");
                //}
                //else
                //{
                msgText = "Please add atleast one Schedule for OT.";
                //}
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else
            {
                //if (objLocalizationManager != null)
                //{
                //    msgText = objLocalizationManager.GetValue("UpdateVerification_Msg");
                //}
                //else
                //{
                msgText = "Are you sure you want to Update ?";
                //}
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                msgW1.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearData();
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmOTConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "OT Configuration";
                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    break;
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    cmbOT.IsEnabled = true;
                    cmbOTTable.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    break;
                case "Edit":
                    cmdAdd.IsEnabled = false;
                    cmdEdit.IsEnabled = true;
                    break;
                case "ModifySchedule":
                    cmdAdd.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
