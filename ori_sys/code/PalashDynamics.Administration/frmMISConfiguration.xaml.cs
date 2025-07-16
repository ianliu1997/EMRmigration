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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.MISConfiguration;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class frmMISConfiguration : UserControl, INotifyPropertyChanged
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

        #region Validation

        private bool ValidationForMisconfig()
        {
            if(cmbClinic.SelectedItem==null )
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Clinic";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbClinic.Focus();
                return false;
            }
            else if (((MasterListItem)cmbClinic.SelectedItem).ID<=0)
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Clinic";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbClinic.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtScheduleName.Text))
            {
                tabConfig.SelectedIndex = 0;
                txtScheduleName.SetValidation("Schedule Name Cannot be Blank.");
                txtScheduleName.RaiseValidationError();
                txtScheduleName.Focus();
                return false;
            }
            else if (cmbMISType.SelectedItem == null)
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Report Type";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbMISType.Focus();
                return false;
            }
            else if(((MasterListItem)cmbMISType.SelectedItem).ID<=0)
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Report Type";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbMISType.Focus();
                return false;
            }
           
            else if (cmbStaffType.SelectedItem == null)
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Staff Type";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbStaffType.Focus();
                return false;
            }
            else if (((MasterListItem)cmbStaffType.SelectedItem).ID <= 0)
            {
                tabConfig.SelectedIndex = 0;
                msgText = "Please Select Staff Type";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbStaffType.Focus();
                return false;
            }

            else
            {
                return true;
            }


        }

        private bool ValidationForSchedule()
        {
            if (rdbDaily.IsChecked == true && tpFromTimeDaily.Value == null)
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Time";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                tpFromTimeDaily.Focus();
                tpFromTimeDaily.SetValidation("");
                tpFromTimeDaily.RaiseValidationError();
                tpFromTimeDaily.Focus();
                return false;
            }
            else  if (rdbWeekly.IsChecked == true && tpFromTimeWeekly.Value == null )
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Schedule Time";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                tpFromTimeWeekly.SetValidation("Please Select Schedule Time");
                tpFromTimeWeekly.RaiseValidationError();
                tpFromTimeWeekly.Focus();
                return false;
                
            }
            else if (rdbWeekly.IsChecked==true && (chkMonday.IsChecked == false && chkTuesday.IsChecked == false && chkWednesday.IsChecked == false && chkThrusday.IsChecked == false && chkFriday.IsChecked == false && chkSaturday.IsChecked == false && chkSunday.IsChecked == false))
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Weekday";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                return false;
            }
            else if (rdbMonthly.IsChecked == true && cmbtxtMonthlyDay1.SelectedItem == null)
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Day";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbtxtMonthlyDay1.Focus();
                return false;
            }
            else if (rdbMonthly.IsChecked == true &&((MasterListItem )cmbtxtMonthlyDay1.SelectedItem).ID<=0)
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Day";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                cmbtxtMonthlyDay1.Focus();
                return false;
            }
            else if (rdbMonthly.IsChecked == true && tpFromTimeMonthly.Value == null)
            {
                tabConfig.SelectedIndex = 1;
                msgText = "Please Select Schedule Time";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                tpFromTimeMonthly.Focus();
                tpFromTimeMonthly.SetValidation("");
                tpFromTimeMonthly.RaiseValidationError();
                tpFromTimeMonthly.Focus();
                return false;
            }
            else if (dtpStartDate.SelectedDate == null)
            {
                tabConfig.SelectedIndex = 1;
                dtpStartDate.SetValidation("Please Select Schedule Date");
                dtpStartDate.RaiseValidationError();
                dtpStartDate.Focus();
                return false;
            }
            else
            {
                tpFromTimeDaily.ClearValidationError();
                tpFromTimeWeekly.ClearValidationError();
                tpFromTimeMonthly.ClearValidationError();
                dtpStartDate.ClearValidationError();
                return true;
            }

        }

        private bool ValidationFordgReportList()
        {
            tabConfig.SelectedIndex = 0;
            bool flag = false;
            foreach (clsGetMISReportTypeVO Item in dgReportList.ItemsSource)
            {
                if (Item.Status == true)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                msgText = "Please Select Report";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
            }
            return flag;
            
        }
        private bool ValidationFordgStaffList()
        {
            tabConfig.SelectedIndex = 0;
            bool flag = false;
            foreach (clsGetMISStaffVO Item in dgStaffList.ItemsSource)
            {
                if (Item.Status == true)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                msgText = "Please Select Staff/Doctor";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
            }
            return flag;

        }
        #endregion

        #region Public Variables

        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public bool Validate = true;
        public string msgText = "";
        public string msgTitle = "";
        bool IsView = false;
        public PagedSortableCollectionView<clsMISConfigurationVO> MasterList { get; private set; }
        public clsGetMISConfigBizActionVO MISConfigList { get; set; }
        public long ReportTypeId;
        public long StaffTypeId;
        bool IsCancel = true;
        #endregion

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region Constructor

        public frmMISConfiguration()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillClinic();
            FillMISType();
            FillDays();
            FillStaffTYpe();
            MasterList = new PagedSortableCollectionView<clsMISConfigurationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dgDataPager.DataContext = MasterList;
            this.dgMISConfigList.DataContext = MasterList;

            FillMISConfigGrid();
            rdbDaily.IsChecked = true;
        }

        #region On Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillMISConfigGrid();
        }

        #endregion

        #endregion

        #region Loaded Event

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


        #endregion

        #region Fill Comboboxes/datagrid

        public void FillMISConfigGrid()
        {
            try
            {
                clsgetMISconfigListBizActionVO BizAction = new clsgetMISconfigListBizActionVO();
                if (cmbMISTypeSearch.SelectedItem != null)
                {
                    BizAction.FilterbyMISType = ((MasterListItem)cmbMISTypeSearch.SelectedItem).ID.ToString();
                }
                if (cmbClinicSearch.SelectedItem!=null)
                {
                    BizAction.FilterbyClinic = ((MasterListItem)cmbClinicSearch.SelectedItem).ID.ToString();
                }
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = MasterList.PageSize;
                BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {

                    if (e.Error == null && e.Result != null)
                    {
                      BizAction.GetMISConfig=((clsgetMISconfigListBizActionVO)e.Result).GetMISConfig;
                      
                      ///Setup Page Fill DataGrid
                      if (BizAction.GetMISConfig.Count > 0)
                      {
                          MasterList.Clear();
                          MasterList.TotalItemCount = (int)(((clsgetMISconfigListBizActionVO)e.Result).TotalRows);
                          foreach (clsMISConfigurationVO item in BizAction.GetMISConfig)
                          {
                              MasterList.Add(item);
                          }

                      }

                    }
                };
              

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
        }

        private void FillClinic()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;

                    cmbClinicSearch.ItemsSource = null;
                    cmbClinicSearch.ItemsSource = objList;
                    MasterListItem item = ((List<MasterListItem>)cmbClinicSearch.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    cmbClinicSearch.SelectedItem = item;

                    //if (myAppConfig != null)
                    //{
                    //    cmbTariff.SelectedValue = myAppConfig.TariffID;
                    //}
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
        }

        private void FillMISType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //change the MAster table name.
                BizAction.MasterTable = MasterTableNameList.Sys_MISType;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    }

                    cmbMISType.ItemsSource = null;                    
                    cmbMISType.ItemsSource = objList;
                  
        
                    cmbMISTypeSearch.ItemsSource = null;
                    cmbMISTypeSearch.ItemsSource = objList;
                    MasterListItem item = ((List<MasterListItem>)cmbMISTypeSearch.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    cmbMISTypeSearch.SelectedItem = item;
                   
                };
              

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
               

                throw;
            }
        }

        public void FillDays()
        {
            try
            {
                int DayNumber = 1;
                List<MasterListItem> Daylist = new List<MasterListItem>();


                Daylist.Add(new MasterListItem(0, "--Select--"));
                DateTime dtTo = System.DateTime.Now;
                dtTo = dtTo.AddMonths(1);
                dtTo = dtTo.AddDays(-(dtTo.Day));

                for (int DaysCount = 0; DaysCount < 31; DaysCount++)
                {
                    Daylist.Add(new MasterListItem(DaysCount + 1, DayNumber.ToString()));
                    DayNumber += 1;
                }
                cmbtxtMonthlyDay1.ItemsSource = Daylist;
               
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void FillStaffTYpe()
        {
            try
            {

                List<MasterListItem> StafftypeList = new List<MasterListItem>();

                StafftypeList.Add(new MasterListItem(0, "--Select--"));
                StafftypeList.Add(new MasterListItem((int) StaffType.All, StaffType.All.ToString()));
                StafftypeList.Add(new MasterListItem((int)StaffType.Staff, StaffType.Staff.ToString()));
                StafftypeList.Add(new MasterListItem((int)StaffType.Doctor, StaffType.Doctor.ToString()));

                cmbStaffType.ItemsSource = StafftypeList;
                   
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void FillStaffGrid()
        {
            try
            {
                clsGetMISStaffBizActionVO BizAction = new clsGetMISStaffBizActionVO();

                BizAction.GetStaffInfo = new List<clsGetMISStaffVO>();

                //change the MAster table name.
                BizAction.TypeId = ((MasterListItem)cmbStaffType.SelectedItem).ID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {
                    if (e1.Error == null && e1.Result != null)
                    {
                        if (IsView == true)
                        {
                            if (BizAction.TypeId == StaffTypeId)
                            {
                                dgStaffList.ItemsSource = MISConfigList.GetConfigStaff;
                            }
                            else
                            {
                                dgStaffList.ItemsSource = (((clsGetMISStaffBizActionVO)e1.Result).GetStaffInfo);
                            }
                        }
                        else
                        {
                            dgStaffList.ItemsSource = (((clsGetMISStaffBizActionVO)e1.Result).GetStaffInfo);
                        }

                        
                    }


                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }


        }

        public void FillReportType()
        {
            try
            {
                clsGetMISReportTypeBizActionVO BizAction = new clsGetMISReportTypeBizActionVO();

                BizAction.GetMIsReport = new List<clsGetMISReportTypeVO>();

                //change the MAster table name.
                
                    BizAction.MISTypeID = ((MasterListItem)cmbMISType.SelectedItem).ID;
                


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {
                    if (e1.Error == null && e1.Result != null)
                    {


                        if (IsView == true)
                        {
                            if (BizAction.MISTypeID == ReportTypeId)
                            {
                                dgReportList.ItemsSource = null;
                                dgReportList.ItemsSource = MISConfigList.GetConfigReport;
                            }
                            else
                            {
                                dgReportList.ItemsSource = null;
                                dgReportList.ItemsSource = (((clsGetMISReportTypeBizActionVO)e1.Result).GetMIsReport);
                            }
                        }
                        else
                        {
                            dgReportList.ItemsSource = null;
                            dgReportList.ItemsSource = (((clsGetMISReportTypeBizActionVO)e1.Result).GetMIsReport);
                        }

                    }


                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();

                throw;
            }

        }
        #endregion

        #region Click Event

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dgMISConfigList.SelectedIndex = -1;
                tabConfig.SelectedIndex = 0;

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New Role";
                
                MISConfigList = new clsGetMISConfigBizActionVO();
                ReportTypeId = 0;
                StaffTypeId = 0;
                IsView = false;
                dgReportList.ItemsSource = null;
                dgStaffList.ItemsSource = null;

                cmbClinic.SelectedItem = null;
                MasterListItem item = ((List<MasterListItem>)cmbClinic.ItemsSource).FirstOrDefault(p => p.ID == 0);
                cmbClinic.SelectedItem = item;

                cmbStaffType.SelectedItem = null;
                item = ((List<MasterListItem>)cmbStaffType.ItemsSource).FirstOrDefault(p => p.ID == 0);
                cmbStaffType.SelectedItem = item;

                cmbMISType.SelectedItem = null;
                item = ((List<MasterListItem>)cmbMISType.ItemsSource).FirstOrDefault(p => p.ID == 0);
                cmbMISType.SelectedItem = item;

                cmbtxtMonthlyDay1.SelectedItem = null;
                item = ((List<MasterListItem>)cmbtxtMonthlyDay1.ItemsSource).FirstOrDefault(p => p.ID == 0);
                cmbtxtMonthlyDay1.SelectedItem = item;

                txtScheduleName.Text = "";
                rdbPDF.IsChecked = true;
                rdbDaily.IsChecked = true;
                
                dtpEndDate.SelectedDate = null;
                dtpStartDate.SelectedDate = null;
                
                rdbnoendate.IsChecked = true;
                
                _flip.Invoke(RotationType.Forward);
                
                tabConfig.SelectedIndex = 0;
                SetCommandButtonState("New");
            }
            catch (Exception ex)
            {
                //throw;
            } 
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationForMisconfig() && ValidationFordgReportList() && ValidationFordgStaffList()  && ValidationForSchedule())
            {
                msgText = "Are you sure you want to Save the Record";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                msgWindowUpdate.Show();
            }
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveModify();
            }
           
        }

        public void SaveModify()
        {
            
                clsAddUpdateMISConfigurationBizActionVO bizActionVO = new clsAddUpdateMISConfigurationBizActionVO();
                bizActionVO.AddMISConfig = new clsMISConfigurationVO();
                bizActionVO.IsUpdateStatus = false;
                if(dgMISConfigList.SelectedItem==null)//Save
                {
                    bizActionVO.ID = 0;
                }
                else //Modify
                {
                     bizActionVO.ID=((clsMISConfigurationVO)dgMISConfigList.SelectedItem).ID;
                }
                bizActionVO.AddMISConfig.ClinicId=((MasterListItem)cmbClinic.SelectedItem).ID;
                bizActionVO.AddMISConfig.ConfigDate = System.DateTime.Now;
                if (rdbPDF.IsChecked == true)
                {
                    bizActionVO.AddMISConfig.MISReportFormatId =(int)ReportFormat.Pdf;
                }
                else if (rdbExcel.IsChecked == true)
                {
                    bizActionVO.AddMISConfig.MISReportFormatId = (int)ReportFormat.Excel;
                }


               
                bizActionVO.AddMISConfig.MISTypeId=((MasterListItem)cmbMISType.SelectedItem).ID;
                if(rdbMonthly.IsChecked==true)
                {
                    bizActionVO.AddMISConfig.ScheduleDetails=((MasterListItem)cmbtxtMonthlyDay1.SelectedItem).ID.ToString();
                    bizActionVO.AddMISConfig.ScheduleTime = tpFromTimeMonthly.Value;
                }
                else if (rdbDaily.IsChecked==true)
                {
                    bizActionVO.AddMISConfig.ScheduleTime = tpFromTimeDaily.Value;
                }
                else if (rdbWeekly.IsChecked == true)
                {
                    bizActionVO.AddMISConfig.ScheduleTime = tpFromTimeWeekly.Value;
                    string days = "";
                    if (chkSunday.IsChecked == true)
                    {
                        //days = days + "1,";
                        days = days + "0,";
                    }

                    if (chkMonday.IsChecked == true)
                    {
                        days = days + "1,";
                    }
                    if (chkTuesday.IsChecked == true)
                    {
                        days = days + "2,";
                    }
                    if (chkWednesday.IsChecked == true)
                    {
                        days = days + "3,";
                    }
                    if (chkThrusday.IsChecked == true)
                    {
                        days = days + "4,";

                    }
                    if (chkFriday.IsChecked == true)
                    {
                        days = days + "5,";
                    }
                    if (chkSaturday.IsChecked == true)
                    {
                        days = days + "6,";
                    }
                    bizActionVO.AddMISConfig.ScheduleDetails = days.Substring(0, days.Length - 1);
                }
                if (dtpEndDate.SelectedDate != null)
                {
                    bizActionVO.AddMISConfig.ScheduleEndDate = dtpEndDate.SelectedDate.Value;
                }
                bizActionVO.AddMISConfig.ScheduleName = txtScheduleName.Text;
                if (rdbDaily.IsChecked == true)
                {
                    bizActionVO.AddMISConfig.ScheduleOn=(int)ScheduleOn.Daily;
                }
                else if (rdbWeekly.IsChecked == true)
                {
                    bizActionVO.AddMISConfig.ScheduleOn = (int)ScheduleOn.Weekly;
                }
                else if(rdbMonthly.IsChecked==true)
                {
                    bizActionVO.AddMISConfig.ScheduleOn = (int)ScheduleOn.Monthly;
                }


                bizActionVO.AddMISConfig.ScheduleStartDate = dtpStartDate.SelectedDate.Value;
               // bizActionVO.AddMISConfig.ScheduleTime = System.DateTime.Now;
                if (dgMISConfigList.SelectedItem == null)//Save
                {
                    bizActionVO.AddMISConfig.Status =true;
                }
                else //Modify
                {
                      bizActionVO.AddMISConfig.Status = ((clsMISConfigurationVO)dgMISConfigList.SelectedItem).Status;
                }
              
                
                foreach (clsGetMISStaffVO item in dgStaffList.ItemsSource)
                {
                    bizActionVO.AddMISConfig.StaffDetails = bizActionVO.AddMISConfig.StaffDetails + item.Id.ToString() + ",";
                    bizActionVO.AddMISConfig.StaffDetails = bizActionVO.AddMISConfig.StaffDetails + item.SelectStaffTypeId.ToString() + ",";
                    bizActionVO.AddMISConfig.StaffDetails = bizActionVO.AddMISConfig.StaffDetails + item.StaffTypeId.ToString() + ",";
                    bizActionVO.AddMISConfig.StaffDetails = bizActionVO.AddMISConfig.StaffDetails + item.Status.ToString();
                    bizActionVO.AddMISConfig.StaffDetails =bizActionVO.AddMISConfig.StaffDetails+ "?";
                }
                bizActionVO.AddMISConfig.StaffDetails = bizActionVO.AddMISConfig.StaffDetails.Substring(0, bizActionVO.AddMISConfig.StaffDetails.Length - 1);
                foreach (clsGetMISReportTypeVO item in dgReportList.ItemsSource)
                {
                    bizActionVO.AddMISConfig.ReportDetails = bizActionVO.AddMISConfig.ReportDetails + item.Id.ToString() + ",";
                    bizActionVO.AddMISConfig.ReportDetails = bizActionVO.AddMISConfig.ReportDetails + item.Status.ToString() ;
                    bizActionVO.AddMISConfig.ReportDetails = bizActionVO.AddMISConfig.ReportDetails + "?";
                }
                bizActionVO.AddMISConfig.ReportDetails = bizActionVO.AddMISConfig.ReportDetails.Substring(0, bizActionVO.AddMISConfig.ReportDetails.Length - 1);
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e1) =>
                    {
                        if (e1.Error == null && e1.Result != null)
                        {
                            msgText = "Record save sucessfully!";
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                              new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindowUpdate.Show();
                            _flip.Invoke(RotationType.Backward);
                            MasterList = new PagedSortableCollectionView<clsMISConfigurationVO>();
                            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                            PageSize = 15;
                            this.dgDataPager.DataContext = MasterList;
                            this.dgMISConfigList.DataContext = MasterList;

                            FillMISConfigGrid();
                            SetCommandButtonState("Save");
                            this.DataContext = null;
                        }


                    };
                   

                    Client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }

                catch (Exception)
                {
                   

                    throw;
                }

               
               
            }
       
        


        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {


            if (ValidationForMisconfig() && ValidationFordgReportList() && ValidationFordgStaffList() && ValidationForSchedule())
            {
                msgText = "Are you sure you want to Save the Record";
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                msgWindowUpdate.Show();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dgMISConfigList.SelectedIndex = -1;
                                
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = "";

                _flip.Invoke(RotationType.Backward);
                MISConfigList = new clsGetMISConfigBizActionVO();
                ReportTypeId = 0;
                StaffTypeId = 0;
                IsView = false;
                SetCommandButtonState("Cancel");
                tabConfig.SelectedIndex = 0;
                rdbDaily.IsChecked = false;
                if (IsCancel == true)
                {
                    mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Alerts & Notifications Configuration";

                    UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmAlertsandNotifications") as UIElement;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                //  throw;
            }           
        }

       

        private void rdbendate_Checked(object sender, RoutedEventArgs e)
        {
            dtpEndDate.IsEnabled = true;
        }

        private void rdbendate_Unchecked(object sender, RoutedEventArgs e)
        {
            dtpEndDate.IsEnabled = false;
            dtpEndDate.SelectedDate = null;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsMISConfigurationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dgDataPager.DataContext = MasterList;
            this.dgMISConfigList.DataContext = MasterList;

            FillMISConfigGrid();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            _flip.Invoke(RotationType.Forward);
            tabConfig.SelectedIndex = 0;

            SetCommandButtonState("View");
            try
            {
                clsGetMISConfigBizActionVO BizAction = new clsGetMISConfigBizActionVO();
                BizAction.ID=((clsMISConfigurationVO)dgMISConfigList.SelectedItem).ID;
                cmbClinic.SelectedItem = null;
                cmbMISType.SelectedItem = null;
                cmbStaffType.SelectedItem = null;
                cmbClinic.SelectedValue = null;
                cmbMISType.SelectedValue = null;
                cmbStaffType.SelectedValue = null;

                IsView = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {

                    if (e1.Error == null && e1.Result != null)
                    {
                        BizAction.GetMISConfig = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig;

                        ///Setup Page Fill DataGrid
                       
                        txtScheduleName.Text = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleName;
                        dtpStartDate.SelectedDate = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleStartDate;
                        if (((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleEndDate == null)
                        {
                            rdbnoendate.IsChecked = true;
                        }
                        else
                        {
                            rdbendate.IsChecked=true;
                            dtpEndDate.SelectedDate = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleEndDate;
                        }

                        if (((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.MISReportFormatId == (int)ReportFormat.Pdf)
                        {
                            rdbPDF.IsChecked = true;
                        }
                        else
                        {
                            rdbExcel.IsChecked = true;
                        }
                        
                        cmbClinic.SelectedValue = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ClinicId;
                        cmbMISType.SelectedValue =((clsGetMISConfigBizActionVO)e1.Result).GetConfigReport[0].MISTypeID;
                        
                        cmbStaffType.SelectedValue = ((clsGetMISConfigBizActionVO)e1.Result).GetConfigStaff[0].SelectStaffTypeId;
                        ReportTypeId = ((clsGetMISConfigBizActionVO)e1.Result).GetConfigReport[0].MISTypeID;
                        StaffTypeId = ((clsGetMISConfigBizActionVO)e1.Result).GetConfigStaff[0].SelectStaffTypeId;
                        MISConfigList = new clsGetMISConfigBizActionVO();
                        MISConfigList.GetConfigReport = new List<clsGetMISReportTypeVO>();
                        MISConfigList.GetMISConfig = new clsMISConfigurationVO();
                        MISConfigList.GetConfigReport=((clsGetMISConfigBizActionVO)e1.Result).GetConfigReport;
                        MISConfigList.GetConfigStaff = ((clsGetMISConfigBizActionVO)e1.Result).GetConfigStaff;

                        if(((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleOn==(int)ScheduleOn.Daily)
                        {
                            rdbDaily.IsChecked = true;
                            tpFromTimeDaily.Value = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleTime;
                        }
                        else if (((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleOn == (int)ScheduleOn.Weekly)
                        {
                            rdbWeekly.IsChecked = true;
                            tpFromTimeWeekly.Value = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleTime;
                            string [] tempstr=((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleDetails.Split(',');
                            for (int i = 0; i < tempstr.Length; i++)
                            {
                                if (tempstr[i] == "0")
                                {
                                    chkSunday.IsChecked = true;
                                }

                                if (tempstr[i] == "1")
                                {
                                    chkMonday.IsChecked = true;
                                }
                                if (tempstr[i] == "2")
                                {
                                    chkTuesday.IsChecked = true;
                                }
                                if (tempstr[i] == "3")
                                {
                                    chkWednesday.IsChecked = true;
                                }
                                if (tempstr[i] == "4")
                                {
                                    chkThrusday.IsChecked = true;
                                }
                                if (tempstr[i] == "5")
                                {
                                    chkFriday.IsChecked = true;
                                }
                                if (tempstr[i] == "6")
                                {
                                    chkSaturday.IsChecked = true;
                                }

                            }
                        }
                        else if (((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleOn == (int)ScheduleOn.Monthly)
                        {
                            rdbMonthly.IsChecked = true;
                            tpFromTimeMonthly.Value = ((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleTime;

                            MasterListItem item = ((List<MasterListItem>)cmbtxtMonthlyDay1.ItemsSource).FirstOrDefault(p => p.ID ==Convert.ToInt32(((clsGetMISConfigBizActionVO)e1.Result).GetMISConfig.ScheduleDetails ));
                            cmbtxtMonthlyDay1.SelectedItem =item;
                        }

                    }
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {
                ///Indicatior.Close();
                throw;
            }
        }

        private void rdbDaily_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeWeekly.Value = null;
            tpFromTimeWeekly.IsEnabled = false;
            chkSunday.IsEnabled = false;
            chkMonday.IsEnabled = false;
            chkTuesday.IsEnabled = false;
            chkWednesday.IsEnabled = false;
            chkThrusday.IsEnabled = false;
            chkFriday.IsEnabled = false;
            chkSaturday.IsEnabled = false;


            chkSunday.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThrusday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;

            tpFromTimeDaily.IsEnabled = true;
            tpFromTimeDaily.Value = null;
            tpFromTimeMonthly.Value = null;
            
            MasterListItem item = ((List<MasterListItem>)cmbtxtMonthlyDay1.ItemsSource).FirstOrDefault(p => p.ID == 0);
            cmbtxtMonthlyDay1.SelectedItem = item;
            
            tpFromTimeMonthly.IsEnabled = false;
            cmbtxtMonthlyDay1.IsEnabled = false;
        }

        private void rdbWeekly_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeWeekly.IsEnabled = true;
            chkSunday.IsEnabled = true;
            chkMonday.IsEnabled = true;
            chkTuesday.IsEnabled = true;
            chkWednesday.IsEnabled = true;
            chkThrusday.IsEnabled = true;
            chkFriday.IsEnabled = true;
            chkSaturday.IsEnabled = true;

            tpFromTimeDaily.Value = null;
            tpFromTimeMonthly.Value = null;

            MasterListItem item = ((List<MasterListItem>)cmbtxtMonthlyDay1.ItemsSource).FirstOrDefault(p => p.ID == 0);
            cmbtxtMonthlyDay1.SelectedItem = item;

            tpFromTimeDaily.IsEnabled = false;
            tpFromTimeMonthly.IsEnabled = false;
            cmbtxtMonthlyDay1.IsEnabled = false;
        }

        private void rdbMonthly_Checked(object sender, RoutedEventArgs e)
        {
            tpFromTimeWeekly.IsEnabled = false;
            chkSunday.IsEnabled = false;
            chkMonday.IsEnabled = false;
            chkTuesday.IsEnabled = false;
            chkWednesday.IsEnabled = false;
            chkThrusday.IsEnabled = false;
            chkFriday.IsEnabled = false;
            chkSaturday.IsEnabled = false;

            tpFromTimeWeekly.Value = null;
            chkSunday.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThrusday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;

            tpFromTimeDaily.Value = null;
            tpFromTimeDaily.IsEnabled = false;
            tpFromTimeMonthly.IsEnabled = true;
            cmbtxtMonthlyDay1.IsEnabled = true;
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgMISConfigList.SelectedItem != null)
            {
                clsAddUpdateMISConfigurationBizActionVO bizActionVO = new clsAddUpdateMISConfigurationBizActionVO();
                bizActionVO.AddMISConfig = new clsMISConfigurationVO();
                bizActionVO.IsUpdateStatus = true;
                bizActionVO.ID = ((clsMISConfigurationVO)dgMISConfigList.SelectedItem).ID;
                bizActionVO.AddMISConfig = new clsMISConfigurationVO();
                bizActionVO.AddMISConfig.Status = ((clsMISConfigurationVO)dgMISConfigList.SelectedItem).Status;
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e1) =>
                    {
                        if (e1.Error == null && e1.Result != null)
                        {
                            msgText = "Status Updated sucessfully!";
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                              new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindowUpdate.Show();
                            _flip.Invoke(RotationType.Backward);
                            MasterList = new PagedSortableCollectionView<clsMISConfigurationVO>();
                            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                            PageSize = 15;
                            this.dgDataPager.DataContext = MasterList;
                            this.dgMISConfigList.DataContext = MasterList;

                            FillMISConfigGrid();
                            SetCommandButtonState("New");
                            this.DataContext = null;
                        }


                    };


                    Client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }

                catch (Exception)
                {


                    throw;
                }

            }
        }


        #endregion

        #region Set Command Button State New/Save/Modify/Print

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

        #endregion
           
        #region SelectionChanged Events

        private void cmbMISType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMISType.SelectedItem != null)
            {
                if (((MasterListItem)cmbMISType.SelectedItem).ID > 0)
                {
                    FillReportType();
                }
            }
        }

        private void cmbStaffType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStaffType.SelectedItem != null)
            {
                if (((MasterListItem)cmbStaffType.SelectedItem).ID > 0)
                {
                    FillStaffGrid();
                }
            }
           
        }


        
        
        #endregion

        #region LostFocus Event

        private void txtScheduleName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }

        #endregion 

    }
}
